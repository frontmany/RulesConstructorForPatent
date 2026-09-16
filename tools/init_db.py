"""Первичное заполнение справочника условий профиля в rules.db.

Порядок: запустите программу один раз — она создаст rules.db и сообщит, что справочник пуст. Затем:

    python tools/init_db.py путь/к/rules.db
"""
import os
import sqlite3
import sys

# Условия пп. 5–6.4 ТЗ: вид условия и варианты значений.
CONDITIONS = {
    "Цель въезда": [
        "Трудовая деятельность",
        "Иная цель",
    ],
    "Гражданство": [
        "Азербайджан",
        "Молдова",
        "Таджикистан",
        "Узбекистан",
        "Украина",
        "Другое государство",
    ],
    "Особый статус": [
        "Высококвалифицированный специалист",
        "Член семьи высококвалифицированного специалиста",
        "Участник госпрограммы переселения соотечественников",
        "Член семьи участника госпрограммы",
        "Нет особого статуса",
    ],
}


def main():
    if len(sys.argv) != 2:
        sys.exit("Использование: python tools/init_db.py путь/к/rules.db")

    path = sys.argv[1]
    if not os.path.exists(path):
        sys.exit(f"Файл {path} не найден: запустите программу один раз, она создаст базу")

    connection = sqlite3.connect(path)
    with connection:
        if connection.execute("SELECT COUNT(*) FROM ProfilePropertyKinds").fetchone()[0] > 0:
            sys.exit("Справочник условий уже заполнен")

        for kind, values in CONDITIONS.items():
            kind_id = connection.execute("INSERT INTO ProfilePropertyKinds (Name) VALUES (?)", (kind,)).lastrowid
            connection.executemany(
                "INSERT INTO ProfilePropertyOptions (Value, KindId) VALUES (?, ?)",
                [(value, kind_id) for value in values])

    option_count = sum(len(values) for values in CONDITIONS.values())
    print(f"Справочник условий заполнен. Условий: {len(CONDITIONS)}, вариантов: {option_count}")


if __name__ == "__main__":
    main()
