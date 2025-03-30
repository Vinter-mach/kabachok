from dataclasses import dataclass


# --- Класс пользователя ---
@dataclass
class User:
    user_id: int
    group: str
    full_name: str


CURRENT_USER: User | None = None


def set_current_user(user: User):
    global CURRENT_USER
    CURRENT_USER = user


def get_current_user() -> User | None:
    return CURRENT_USER


def is_registered() -> bool:
    return CURRENT_USER is not None
