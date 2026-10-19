// ---------------------------------------------------------------------------------------------
// AudioEngine.cs — порт legacy-класса WavPlayer из AudioPlayer.cs.
// Хранит по одному готовому плееру на каждое из 13 событий, умеет играть/останавливать,
// менять громкость, перезагружать набор (Kit_Sound) и выбирать случайные треки.
// Каждое действие дублируется в CoreEvents (MusicPlayed/MusicStopped/VolumeChanged),
// чтобы аддоны (например, GUI) могли отображать состояние без опроса ядра.
// ---------------------------------------------------------------------------------------------
using NAudio.Wave;

namespace MusicCore.Audio;

/// <summary>Пара «файл + выходное устройство» для одного события.</summary>
internal sealed class SoundChannel
{
    public AudioFileReader? file;
    public WaveOutEvent? player;
    public string? path;
}

/// <summary>Зацикливание потока (для Menu и StartRound), как в legacy LoopStream.</summary>
internal sealed class LoopStream : WaveStream
{
    private readonly WaveStream sourceStream;
    public bool EnableLooping { get; set; } = true;

    public LoopStream(WaveStream source) => sourceStream = source;

    public override WaveFormat WaveFormat => sourceStream.WaveFormat;
    public override long Length => sourceStream.Length;
    public override long Position { get => sourceStream.Position; set => sourceStream.Position = value; }

    public override int Read(byte[] buffer, int offset, int count)
    {
        int totalBytesRead = 0;
        while (totalBytesRead < count)
        {
            int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
            if (bytesRead == 0)
            {
                if (sourceStream.Position == 0 || !EnableLooping) break;
                sourceStream.Position = 0; // loop
            }
            totalBytesRead += bytesRead;
        }
        return totalBytesRead;
    }
}

public sealed class AudioEngine
{
    private readonly SoundChannel[] channels = new SoundChannel[EventInfo.Count];
    private readonly Random rnd = new();
    private Kit_Sound loaded = new();
    private readonly object gate = new();

    /// <summary>Текущее играющее событие (null — тишина).</summary>
    public MusicIvents? CurrentEvent { get; private set; }

    /// <summary>Инициализация: загрузить набор, выбрать случайные треки, запустить фоновую музыку меню.</summary>
    public void Setup(Kit_Sound? kit)
    {
        lock (gate)
        {
            StopAll();
            if (kit == null) return;
            loaded = kit;
            SelectRandomTracks();
            TryStartMenuMusic();
        }
    }

    /// <summary>Запускает фоновую музыку меню (фикс бага «нет звука при старте»). Идемпотентен.</summary>
    public void TryStartMenuMusic()
    {
        try
        {
            var menu = channels[(int)MusicIvents.Menu];
            if (menu?.player == null) return;
            if (CurrentEvent == MusicIvents.Menu) return;
            menu.file!.Position = 0;
            menu.player.Play();
            CurrentEvent = MusicIvents.Menu;
            CoreEvents.RaiseMusicPlayed(MusicIvents.Menu, menu.path);
            CoreEvents.WriteLog("Menu music started");
        }
        catch (Exception ex) { CoreEvents.WriteLog("TryStartMenuMusic error: " + ex.Message); }
    }

    public void Play(MusicIvents ev)
    {
        try
        {
            var ch = channels[(int)ev];
            if (ch?.player == null || ch.file == null) return;
            ch.file.Position = 0;
            ch.player.Play();
            CoreEvents.RaiseMusicPlayed(ev, ch.path);
        }
        catch (Exception ex) { CoreEvents.WriteLog($"Play({ev}) error: {ex.Message}"); }
    }

    /// <summary>Остановить текущий трек (+ перемешать очередь, если это было не StartAction).</summary>
    public void Stop()
    {
        try
        {
            if (CurrentEvent == null) return;
            var ch = channels[(int)CurrentEvent.Value];
            if (ch != null)
            {
                ch.player?.Stop();
                if (ch.file != null) ch.file.Position = 0;
                CoreEvents.RaiseMusicStopped(CurrentEvent.Value, ch.path);
            }
            if (CurrentEvent != MusicIvents.StartAction) Randomize(CurrentEvent);
            CurrentEvent = null;
        }
        catch (Exception ex) { CoreEvents.WriteLog("Stop error: " + ex.Message); }
    }

    /// <summary>Остановить конкретное событие, если оно является текущим.</summary>
    public void Stop(MusicIvents ev)
    {
        if (CurrentEvent == ev) { Stop(); return; }
        channels[(int)ev]?.player?.Stop();
    }

    /// <summary>Legacy IfEnable: три режима переключения трека.</summary>
    public void IfEnable(MusicIvents song, ModeMusic mode)
    {
        if (CurrentEvent == song && mode != ModeMusic.Play) return;
        var ch = channels[(int)song];
        switch (mode)
        {
            case ModeMusic.IfPlayingStop:
                if (ch?.file != null && ch.file.Volume != 0)
                {
                    Stop();
                    Play(song);
                }
                CurrentEvent = song;
                break;
            case ModeMusic.Play:
                Play(song);
                break;
            case ModeMusic.Stop:
                Stop();
                Play(song);
                CurrentEvent = song;
                break;
        }
    }

    /// <summary>Громкость 0..1 применяется всегда (в т.ч. к остановленному плееру).</summary>
    public void SetVolume(MusicIvents ev, float volume01)
    {
        var ch = channels[(int)ev];
        if (ch?.file == null) return;
        ch.file.Volume = Math.Clamp(volume01, 0f, 1f);
        CoreEvents.RaiseVolumeChanged(ev, ch.file.Volume);
    }

    public float GetVolume(MusicIvents ev) => channels[(int)ev]?.file?.Volume ?? 0f;

    public bool HasTrack(MusicIvents ev) => channels[(int)ev]?.player != null;

    /// <summary>Полная остановка и освобождение всех ресурсов.</summary>
    public void StopAll()
    {
        foreach (var ch in channels)
        {
            if (ch == null) continue;
            try { ch.player?.Stop(); ch.player?.Dispose(); ch.file?.Dispose(); } catch { }
        }
        Array.Clear(channels);
        CurrentEvent = null;
    }

    /// <summary>Перезагрузка набора с сохранением текущего события (или откат на Menu).</summary>
    public void ReloadKit(Kit_Sound? kit)
    {
        if (kit == null) { CoreEvents.WriteLog("ReloadKit: kit is null"); return; }
        var keep = CurrentEvent;
        Setup(kit);
        if (keep == null || !HasTrack(keep.Value))
        {
            TryStartMenuMusic();
            CoreEvents.WriteLog("Reload: previous event missing in kit, fallback to Menu");
            return;
        }
        Play(keep.Value);
        CurrentEvent = keep;
    }

    /// <summary>Выбрать случайный трек из списка дорожек события.</summary>
    public void Randomize(MusicIvents? ev)
    {
        if (ev == null) return;
        int i = (int)ev;
        var list = loaded.music?[i];
        if (list == null || list.Count <= 1) return;
        ref var ch = ref channels[i];
        LoadTrackInto(ref ch, i, rnd.Next(list.Count));
    }

    /// <summary>
    /// Перемешать дорожки события first и подставить next_music в событие second
    /// (legacy Randomizate для пары StartRound/StartAction), плюс DeathCam/KillSound.
    /// </summary>
    public void RandomizatePair(int first, int second)
    {
        var list = loaded.music?[first];
        if (list == null || list.Count == 0) return;
        int rand = rnd.Next(list.Count);
        LoadTrackInto(ref channels[first], first, rand);
        int? next = list[rand].next_music;
        var secondList = loaded.music?[second];
        if (next != null && secondList != null && next.Value >= 0 && next.Value < secondList.Count)
            LoadTrackInto(ref channels[second], second, next.Value);
        Randomize(MusicIvents.DeathCam);
        Randomize(MusicIvents.KillSound);
    }

    // ------------------------------------------------------------------
    // Внутреннее
    // ------------------------------------------------------------------

    private void SelectRandomTracks()
    {
        for (int i = 0; i < EventInfo.Count; i++)
        {
            try
            {
                var list = loaded.music?[i];
                if (list == null || list.Count == 0) { channels[i] = null!; continue; }
                int pick = rnd.Next(list.Count); // фикс legacy bug: Next(count-1)
                if (string.IsNullOrEmpty(list[pick].path)) { channels[i] = null!; continue; }
                LoadTrackInto(ref channels[i], i, pick);
            }
            catch (Exception ex)
            {
                CoreEvents.WriteLog($"[Load error {((MusicIvents)i)}]: {ex.Message}");
                channels[i] = null!;
            }
        }
    }

    private void LoadTrackInto(ref SoundChannel? slot, int eventId, int trackIndex)
    {
        var path = loaded.music?[eventId]?[trackIndex]?.path;
        if (string.IsNullOrEmpty(path)) return;

        // освобождаем старый канал этого события
        if (slot != null)
        {
            try { slot.player?.Stop(); slot.player?.Dispose(); slot.file?.Dispose(); } catch { }
            slot = null;
        }

        var player = new WaveOutEvent();
        var file = new AudioFileReader(path);
        float vol = (loaded.music[eventId][trackIndex].volume > 0 ? loaded.music[eventId][trackIndex].volume : 100) / 100f;
        file.Volume = vol;

        if (eventId == (int)MusicIvents.Menu || eventId == (int)MusicIvents.StartRound)
            player.Init(new LoopStream(file));
        else
            player.Init(file);

        slot = new SoundChannel { file = file, player = player, path = path };
        CoreEvents.WriteLog($"Track loaded: {(MusicIvents)eventId} <- {path} (vol={vol:0.##})");
    }
}
