// ---------------------------------------------------------------------------------------------
// ConsoleLogGui.cs — переписанное окно консоли (legacy ConsoleLog.cs). Теперь это не форма,
// запускаемая из Main напрямую, а «приёмник» потока логов: GuiAddon подписан на CoreEvents.Log
// и вызывает Append(). Окно живёт в UI-потоке аддона.
// ---------------------------------------------------------------------------------------------
namespace MusicGui;

public sealed class ConsoleLogGui : Form
{
    private readonly GuiAddon addon;
    private TextBox box = null!;
    private CheckBox autoScroll = null!;

    public ConsoleLogGui(GuiAddon owner)
    {
        addon = owner;
        Text = "Core console";
        ClientSize = new Size(760, 420);
        StartPosition = FormStartPosition.Manual;
        Location = new Point(120, 120);

        box = new TextBox
        {
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Both,
            WordWrap = false,
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(24, 24, 24),
            ForeColor = Color.Gainsboro,
            Font = new Font("Consolas", 9f)
        };
        autoScroll = new CheckBox { Text = "Auto-scroll", Checked = true, Dock = DockStyle.Bottom, AutoSize = true };

        Controls.Add(box);
        Controls.Add(autoScroll);
    }

    /// <summary>Добавить строку лога (поток ядра -> marshal в UI).</summary>
    public void Append(string message, DateTime time)
    {
        if (IsDisposed) return;
        try
        {
            if (InvokeRequired) { BeginInvoke(() => Append(message, time)); return; }
            if (box.TextLength > 400_000) box.Clear(); // защита от разрастания
            box.AppendText($"[{time:HH:mm:ss}] {message}{Environment.NewLine}");
            if (autoScroll.Checked) { box.SelectionStart = box.TextLength; box.ScrollToCaret(); }
        }
        catch { }
    }
}
