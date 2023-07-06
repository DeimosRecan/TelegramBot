import requests

API = 'a5604590a320db1ddfe4934158bd550e'
MUSIC_URL = 'https://mp3zen.net/uploads/files/metan-bashnya-klinit.mp3'
BOT_TOKEN = '5665516734:AAHmaBdq0M-vY-8_AXNJj8Lp2Bc5bpAimkg'

def shazam_voice(file):
    """
    По ссылке определяет название и много другой информации
    :param file: Путь к файлу
    :return: возвращает json формат
    """
    data = {
        'api_token': API,
        'url': 'https://api.telegram.org/file/bot{}/{}'.format(BOT_TOKEN,file),
        'return': 'musicbrainz,lyrics,apple_music,spotify,deezer,napster,youtube',
    }
    file = requests.post('https://api.audd.io/', data=data)  # recognizeWithOffset/
    music = file.json()
    print(music)
    return music

""" Берет из файла путь к голосовому сообщению и кладет в FilePath"""
try:
    with open(r"D:\Games2\Desktop\shazam bot\TelegramBot_rardist\TelegramBot\TelegramBot\Functional\FilePath.txt", "a+") as file:
        file.seek(0)
        FilePath = file.readline()
        print(FilePath)
except:
    print("Ошибка при работе с файлом")
print("https://api.telegram.org/file/bot{}/{}".format(BOT_TOKEN,FilePath))
sound = shazam_voice(FilePath)

if sound['result'] is None:
    file = open(r"D:\Games2\Desktop\shazam bot\TelegramBot_rardist\TelegramBot\TelegramBot\Functional\Music.txt", 'w', encoding='utf-8')
    file.write("0")
    file.close()
else:
    file = open(r"D:\Games2\Desktop\shazam bot\TelegramBot_rardist\TelegramBot\TelegramBot\Functional\Music.txt", 'w', encoding='utf-8')
    file.write(
        """ {} by #{}""".format(sound['result']['title'].strip(),sound['result']['artist'].replace(' ', '_').strip().replace(',', '')))
    file.close()