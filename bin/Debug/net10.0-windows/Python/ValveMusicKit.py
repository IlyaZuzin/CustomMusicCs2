#!/usr/bin/env python3
# -*- coding: utf-8 -*-

import subprocess
import shutil
import json
from pathlib import Path
from tkinter import Tk, filedialog

FFMPEG_PATH = None  # укажите свой путь при необходимости

# Технические имена треков (индекс 10 – null)
TRACK_NAMES = [
    "Menu",           # 0
    "WinRound",       # 1
    "LoseRound",      # 2
    "Bomb",           # 3
    "TenSecondBomb",  # 4
    "Mvp",            # 5
    "DeathCam",       # 6
    "StartGame",      # 7
    "StartRound",     # 8
    "StartAction",    # 9
    None,             # 10
    "EndGame",        # 11
    "TenSecondRound"  # 12
]

# Соответствие технических имён Valve-оригиналам (поиск по префиксу)
VALVE_MAPPING = {
    "Menu": ["mainmenu"],
    "WinRound": ["wonround"],
    "LoseRound": ["lostround"],
    "Bomb": ["bombplanted"],
    "TenSecondBomb": ["bombtenseccount"],
    "Mvp": ["roundmvpanthem"],
    "DeathCam": ["deathcam"],
    "StartGame": ["startofmatch"],
    "StartRound": ["startround"],
    "StartAction": ["startaction"],
    "EndGame": ["endofmatch"],
    "TenSecondRound": ["roundtenseccount"]
}

def select_folder():
    root = Tk()
    root.withdraw()
    folder = filedialog.askdirectory(title="Выберите папку с файлами .vsnd_c")
    root.destroy()
    return folder

def select_image_file():
    root = Tk()
    root.withdraw()
    file_path = filedialog.askopenfilename(
        title="Выберите изображение для набора",
        filetypes=[("Изображения", "*.png *.jpg *.jpeg *.bmp *.gif"), ("Все файлы", "*.*")]
    )
    root.destroy()
    return file_path

def convert_files(folder, delete_original=False):
    folder_path = Path(folder)
    files = list(folder_path.glob("*.vsnd_c"))
    if not files:
        print("Файлов .vsnd_c не найдено.")
        return

    ffmpeg_exe = FFMPEG_PATH if FFMPEG_PATH else shutil.which("ffmpeg")
    if not ffmpeg_exe:
        print("ОШИБКА: ffmpeg не найден. Укажите полный путь к ffmpeg.exe в переменной FFMPEG_PATH.")
        return

    print(f"Найдено {len(files)} файлов. Начинаем конвертацию в WAV...")
    converted = 0

    for file in files:
        output = file.with_suffix(".wav")
        cmd = [
            ffmpeg_exe,
            "-i", str(file),
            "-acodec", "pcm_s16le",
            "-ar", "44100",
            "-ac", "2",
            str(output)
        ]
        try:
            subprocess.run(cmd, check=True, capture_output=True, text=True)
            print(f"Конвертирован: {file.name} -> {output.name}")
            if delete_original:
                file.unlink()
                print(f"  Оригинал {file.name} удалён.")
            converted += 1
        except subprocess.CalledProcessError as e:
            print(f"Ошибка при конвертации {file.name}:")
            print(e.stderr)
        except Exception as e:
            print(f"Неизвестная ошибка с {file.name}: {e}")

    print(f"\nГотово! Конвертировано файлов: {converted}")

def create_music_kit(folder):
    folder_path = Path(folder)
    wav_files = list(folder_path.glob("*.wav"))
    if not wav_files:
        print("В выбранной папке нет .wav файлов. Сначала сконвертируйте файлы.")
        return

    # Название набора
    kit_name = input("Введите название набора музыки: ").strip()
    if not kit_name:
        print("Название не может быть пустым. Операция отменена.")
        return

    kit_folder = folder_path / kit_name
    kit_folder.mkdir(exist_ok=True)
    print(f"Создана папка: {kit_folder}")

    # Словарь для хранения выбранных имён (индекс -> имя файла или None)
    selected = [None] * len(TRACK_NAMES)

    # Функция для выбора файла из списка с подсказкой
    def pick_from_list(file_list, prompt, allow_zero=True):
        if not file_list:
            return None
        # Показываем список
        for i, f in enumerate(file_list, start=1):
            print(f"  {i}. {f.name}")
        while True:
            choice = input(prompt).strip()
            if choice == "" and allow_zero:
                return None
            if choice.isdigit():
                num = int(choice)
                if num == 0 and allow_zero:
                    return None
                if 1 <= num <= len(file_list):
                    return file_list[num-1]
            print(f"Введите число от 0 до {len(file_list)}")

    # Проходим по каждому треку (индекс 10 пропускаем)
    for idx, track_name in enumerate(TRACK_NAMES):
        if track_name is None:  # индекс 10
            continue

        print(f"\n--- Трек {idx}: {track_name} ---")

        # 1. Ищем совпадения по Valve-маппингу
        candidates = []
        prefixes = VALVE_MAPPING.get(track_name, [])
        for wav in wav_files:
            stem = wav.stem.lower()
            for prefix in prefixes:
                if stem.startswith(prefix):
                    candidates.append(wav)
                    break  # один файл может подходить под несколько префиксов, но нам достаточно одного

        # Убираем дубликаты (если файл попал под несколько префиксов)
        candidates = list(dict.fromkeys(candidates))

        chosen = None
        if len(candidates) == 1:
            # Ровно один – берём автоматически
            chosen = candidates[0]
            print(f"  Автоматически выбран: {chosen.name}")
        elif len(candidates) > 1:
            # Несколько – даём выбор
            print(f"  Найдено несколько подходящих файлов:")
            chosen = pick_from_list(
                candidates,
                f"  Введите номер файла (1-{len(candidates)}) или 0 для пропуска: ",
                allow_zero=True
            )
            if chosen is None:
                print("  Трек пропущен.")
        else:
            # Нет совпадений – предлагаем выбрать из общего списка всех файлов
            print("  Совпадений не найдено. Выберите файл вручную из общего списка:")
            # Исключаем уже использованные файлы (чтобы не выбрать повторно)
            used_files = [f for f in selected if f is not None]
            available = [f for f in wav_files if f not in used_files]
            if not available:
                print("  Нет доступных файлов. Трек пропущен.")
                continue
            chosen = pick_from_list(
                available,
                f"  Введите номер файла (1-{len(available)}) или 0 для пропуска: ",
                allow_zero=True
            )
            if chosen is None:
                print("  Трек пропущен.")

        if chosen is not None:
            # Копируем и переименовываем
            dest_file = kit_folder / f"{track_name}.wav"
            shutil.copy2(chosen, dest_file)
            print(f"  Скопирован {chosen.name} -> {dest_file.name}")
            selected[idx] = f"{track_name}.wav"

    # Изображение
    image_name = None
    add_image = input("\nДобавить изображение для набора? (y/n): ").strip().lower()
    if add_image == 'y':
        image_path = select_image_file()
        if image_path:
            src_img = Path(image_path)
            dest_img = kit_folder / "Image.png"
            shutil.copy2(src_img, dest_img)
            image_name = "Image.png"
            print(f"Изображение скопировано: {dest_img.name}")
        else:
            print("Изображение не выбрано.")

    # Формируем JSON
    kit_data = {
        "name": kit_name,
        "image": image_name,
        "music": selected
    }

    json_path = kit_folder / "profile.json"
    with open(json_path, 'w', encoding='utf-8') as f:
        json.dump(kit_data, f, ensure_ascii=False, indent=2)

    print(f"\nНабор музыки успешно создан в папке: {kit_folder}")
    print(f"Конфигурационный файл: {json_path}")

if __name__ == "__main__":
    folder = select_folder()
    if not folder:
        print("Выбор папки отменён.")
        exit()

    delete = input("Удалять исходные файлы .vsnd_c после конвертации? (y/n): ").strip().lower() == 'y'
    convert_files(folder, delete_original=delete)

    create_kit = input("\nСоздать набор музыки из полученных WAV-файлов? (y/n): ").strip().lower()
    if create_kit == 'y':
        create_music_kit(folder)
    else:
        print("Генерация набора пропущена.")