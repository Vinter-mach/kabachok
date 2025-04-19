import boto3
from aiogram import Bot
from io import BytesIO

from tgBot.config import SECRET_KEY, ACCESS_KEY, ENDPOINT_URL, \
    BUCKET_NAME

CLIENT = boto3.client(
    's3',
    endpoint_url=ENDPOINT_URL,
    aws_access_key_id=ACCESS_KEY,
    aws_secret_access_key=SECRET_KEY
)


async def upload_all_or_none(files: list[dict], bot: Bot) -> bool:
    if not files:
        return False

    prefix = files[0]["mask_for_save"]  # все файлы в одной папке
    loaded_files = []

    # 1. Скачиваем все файлы из Telegram
    for file in files:
        try:
            tg_file = await bot.get_file(file["file_id"])
            file_data = await bot.download_file(tg_file.file_path)
            buffer = BytesIO(file_data.read())

            loaded_files.append({
                "path": prefix + file["original_file_name"],
                "buffer": buffer
            })
        except Exception as e:
            print(f"Ошибка скачивания {file['original_file_name']}: {e}")
            return False

    # 2. Получаем старые файлы по префиксу
    try:
        response = CLIENT.list_objects_v2(Bucket=BUCKET_NAME, Prefix=prefix)
        old_keys = [obj['Key'] for obj in response.get('Contents', [])]
    except Exception as e:
        print(f"Ошибка при получении списка старых файлов: {e}")
        return False

    # 3. Загружаем новые файлы
    try:
        for item in loaded_files:
            CLIENT.put_object(
                Bucket=BUCKET_NAME,
                Key=item["path"],
                Body=item["buffer"]
            )
            print(f"Загружен: {item['path']}")
    except Exception as e:
        print(f"Ошибка загрузки: {e}")
        return False

    # 4. Удаляем старые файлы, только если всё загрузилось успешно
    if old_keys:
        try:
            delete_payload = {"Objects": [{"Key": k} for k in old_keys]}
            CLIENT.delete_objects(Bucket=BUCKET_NAME, Delete=delete_payload)
            print(f"Удалены старые файлы: {old_keys}")
        except Exception as e:
            print(f"Ошибка при удалении старых файлов: {e}")
            return False

    return True


async def get_files_by_mask(prefix: str) -> list[dict] | None:
    result = []
    try:
        response = CLIENT.list_objects_v2(Bucket=BUCKET_NAME, Prefix=prefix)
        contents = response.get("Contents", [])

        for obj in contents:
            key = obj["Key"]
            file_data = CLIENT.get_object(Bucket=BUCKET_NAME, Key=key)[
                "Body"].read()

            result.append({
                "filename": key.split("/")[-1],
                "buffer": BytesIO(file_data)
            })

    except Exception as e:
        print(f"Ошибка при получении файлов по маске '{prefix}': {e}")
        return None
    return result
