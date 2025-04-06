lessons = {
    "пара 12 03": "Пара 12.03. Условные распределения и математически ожидания",
    "пара 19 03": "Пара 15.03. Теорема Байеса и дисперсия"
}


def get_lesson():
    return ["пара 12 03", "пара 19 03"]




# Временное хранилище (заглушка вместо настоящей БД)
_submissions: dict[int, dict] = {}


def save_submission(user_id: int, file_id: str, filename: str):
    _submissions[user_id] = {
        "file_id": file_id,
        "filename": filename,
        "status": "На проверке"
    }


def get_submission(user_id: int):
    return _submissions.get(user_id)
