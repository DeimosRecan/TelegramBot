using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;
using Telegram.Bot.Types;
using TelegramBot.Functional;
using System.Linq;
using IronPython.Runtime.Operations;

namespace TelegramBot {
    public class TelegramBotProject {

        //Константовый текст
        private const string User_Start_Mes = "/start";
        private const string Start_Mes = "Привет!";
        private const string Back_Mes = "Назад";
        private const string Main_Menu_Mes = "Добра пожаловать в главное меню чат бота!";
        private const string Weather_Menu_Mes = "Добро пожаловать в меню погоды чат бота!\n\n" +
                                                "В этом разделе вы можете посмотреть погоду в своем городе!\n" +
                                                "Напиши название вашего города:";
        private const string Weather_Menu_Another_City_Mes = "Напиши название города:";
        private const string Timer_Menu_Mes = "Добро пожаловать в меню таймера чат бота!\n\n" +
                                              "В этом разделе вы можете настроить и запустить таймер, " +
                                              "однако учитывайте, что на время действия таймера другие функции " +
                                              "чат бота будут недоступны.";
        private const string Mathematics_Menu_Mes = "Добро пожаловать в математическое меню чат бота!\n\n" +
                                                    "В данном разделе вы можете узнать:\n" +
                                                    "- Формулировки теорем из курса математического анализа, " +
                                                    "которые есть в базе данных нашего чат бота.\n" +
                                                    "- Быстро посчитать определитель матрицы.\n" +
                                                    "- Найти обратную матрицу.\n" +
                                                    "- Найти корни системы линейных уравнений.";
        private const string Theorems_Menu_Mes = "В данном разделе вы можете посмотреть определие и доказательство " +
                                            "некоторых теорем, которые содержаться в нашей базе данных.";
        private const string Determinant_Menu_Mes = "В данном разделе вы можете найти определитель вашей матрицы.";
        private const string Inverse_Matrix_Menu_Mes = "В данном разделе вы можете найти матрицу обратную вашей.";
        private const string System_of_Equations_Menu_Mes = "В данном разделе вы можете найти корни введенной вами системы уравнений.";
        private const string Music_Menu_Mes = "Добро пожаловать в музыкальное меню чат бота!\n\n" +
                                              "В данном разделе вы можете определить песню с помощью голосового сообщения.\n";
        private const string Unavailable_Mes = "Данная функция пока что не реализована.";
        private const string Unknown_Command = "Я не знаю такой команды...\n" +
                                               "Но на всякий случай верну вас в главное меню.";

        //Сообщения главного меню
        private const string Weather_Butt = "Погода";
        private const string Exchange_Rate_Butt = "Курс валют";
        private const string Timer_Butt = "Таймер";
        private const string Geolocation_Butt = "Геолокация";
        private const string Mathematics_Butt = "Математика";
        private const string Music_Butt = "Музыка";

        //Сообщения меню погоды
        private const string Today_Butt = "Сегодня";
        private const string Tomorrow_Butt = "Завтра";
        private const string Next_Tomorrow_Butt = "Послезавтра";
        private const string Another_City_Butt = "Другой город";
        private const string Error_City_Mes = "Данный город отсутствует в базе данных.\n\n" +
                                              "Попробуйте ввести город снова:";
        private bool _weather_menu = false; //Вкладка погоды
        private bool _new_city = false; //Новый город
        private string city;
        private string weather_in_the_city;
        private string weather_in_the_city_tomorrow;
        private string weather_in_the_city_next_tomorrow;

        //Сообщения меню валют
        const string CurrencyStartMes = "Выберите валюту";
        const string CurrencyEuro_Butt = "EUR";
        const string CurrencyUSD_Butt = "USD";
        const string Currency_Butt = "Валюты";

        private bool _currency_menu = false;
        private Currency currency = new Currency();




        //Сообщения меню таймера
        private const string Start_Timer_Butt = "Запустить";
        private const string Stop_Timer_Butt = "Стоп";
        private const string Timer_Reference_Mes = "Введите время, которое хотите засечь в формате:\n" +
                                                   "xx yy zz\nгде xx - количество часов, yy - количество минут, " +
                                                   "zz - количество секунд. Например:\n5 13 59\nТаймер будет установлен " +
                                                   "на 5 часов 13 минут и 59 секунд.";
        private const string Timer_End_Mes = "Таймер остановлен.";

        private bool _timer_menu = false; //Вкладка таймера
        private bool _new_time = false;
        private int[] time = new int[3];

        //Сообщения меню геолокации
        private const string GeoocationMenuStart_Mes = "Разрешите доступ к местоположению";
        private const string LocationGet_MesBtn = "Разрешить доступ";
        private const string LocationPlaceFindMenuBtn_Mes = "Найти адрес";
        private const string LocationPlaceFind_Mes = "Введите координаты:\n\nКоординаты: Широта:X,XXXX  Долгота:X,XXXX";
        private const string LocationCoordFindMenuBtn_Mes = "Найти координаты";
        private const string LocationCoordFind_Mes = "Введите адрес:";
        //Координаты: Широта: 41.8755616 Долгота: -87.6244212

        private static readonly Geolocation geolocation = new Geolocation();

        private bool _geolocation_menu = true;

        //Сообщения математического меню
        private const string Reference_Butt = "Справка";
        private const string Reference_Matrix_Mes = "Вводить данные о матрице нужно последовательно" +
                                                         " через пробел, например:\n Для матрицы\n| 1 2 3 |\n" +
                                                         "| 4 5 6 |\n| 7 8 9 |\nнужно ввести строку \"1 2 3 " +
                                                         "4 5 6 7 8 9\" и отправить ее боту.";
        private const string Reference_System_Mes = "Вводить данные системы нужно последовательно в два сообщения, например:\n" +
                                                    "Для системы уравнений:\n2x1- 4x2 + 7 = 10\n-9x1 + 3 = -2\n15x2 + 12 = 36\n" +
                                                    "нужно сначала отправить строку \"2 -4 7 10\", а потом \"-9 0 3 -2\", потом \"0 15 12 36\".";
        private const string Matrix_Write_Butt = "Ввести данные";
        private const string Matrix_GetResult_Butt = "Получить результат";

        private const string Matrix_Write_Mes = "Введите данные согласно пункту \"Справка\":";

        private const string Theorems_Butt = "Теоремы";
        private const string Th_Show_Theorems_Butt = "Список теорем";
        private const string Th_Show_Theorems_Mes = "Список теорем, доступных из нашей базы данных:\n";
        private const string Th_Write_Theorems_Butt = "Найти теорему";
        private const string Th_Write_Theorems_Mes = "Для более высокой точности советуется вводить название " +
                                                     "либо по списку доступных теорем, либо через поиск конкретного " +
                                                     "ученого.";
        private const string Th_Write_Autor_Butt = "Поиск ученого";
        private const string Th_Write_Autor_Mes = "Введите фамилию ученого, теоремы которого вы хотите найти, если " +
                                                  "теорема не носит названия, напишите символ '@':";
        private const string Th_Definition_Mes = "Определение";
        private const string Th_Proof_Mes = "Доказательство";
        private const string Th_End_Mes = "Что и следовало доказать";
        private const string Th_Error_Search_Mes = "Теорема не найдена.";

        private const string Determinant_Butt = "Определитель";

        private const string Inverse_Matrix_Butt = "Обратная матрица";

        private const string System_of_Equations_Butt = "Система уравнений";

        private bool _mathematics_menu = false;

        private bool _mathematics_theorems_menu = false;
        private bool _new_theorem = false;
        private bool _new_autor = false;

        private bool _mathematics_determinant_menu = false;
        private bool _new_determinant = false;

        private bool _mathematics_inverse_matrix_menu = false;
        private bool _new_inverse = false;

        private bool _mathematics_system_equations_menu = false;
        private Queue<string[]> SLE_Mesgs = new Queue<string[]>();

        //Сообщения музыкального меню
        private const string Record_Butt = "Записать голосовое сообщение";
        private const string Record_Mes = "Пожалуйста, запишите голосовое сообщение:";
        private const string Error_Sound_Mes = "Запиши голосовое сообщение еще раз и по дольше, не могу опознать.";

        private bool _music_menu = false;
        private bool _new_sound = false;


        //Глобальные 
        private int _offset = 0;
        private string _token;
        private static TelegramBotClient _client;
        private Weather _weather = new Weather();
        private Shazampy music = new Shazampy();
        private Mathematics math = new Mathematics();
        private BotTimer _timer = new BotTimer();

        public TelegramBotProject(string token) {
            this._token = token;
        }

        public static TelegramBotClient GetClient() {
            return _client;
        }
        public void GetUpdates() {
            _client = new TelegramBotClient(_token);
            var Me = _client.GetMeAsync().Result;

            if (Me != null && !string.IsNullOrEmpty(Me.Username)) {
                while (true) {

                    try {
                        var updates = _client.GetUpdatesAsync(_offset).Result;

                        if (updates != null && updates.Length > 0) {
                            foreach (var update in updates) {
                                processUpdate(update);
                                _offset = update.Id + 1;
                            }
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }

                    Thread.Sleep(1000);
                }
            }
        }
        private void Clear_Chat(Update update) {
            for (int i = 0; i < 10; i++) {
                _client.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId - i);
            }
        }
        private void processUpdate(Update update) {
            switch(update.Type) {
                case Telegram.Bot.Types.Enums.UpdateType.Message: {
                        var text = update.Message.Text;
                        
                        if (_weather_menu) {
                            if (_new_city) {
                                city = text;
                                weather_in_the_city = _weather.get_current_weather_by_city_name(city);
                                weather_in_the_city_tomorrow = _weather.get_weather_for_tomorrow_days_by_hours_by_city_name(city);
                                weather_in_the_city_next_tomorrow = _weather.get_weather_for_day_after_tomorrow_days_by_hours_by_city_name(city);

                                if (weather_in_the_city == "Error") {
                                    _client.SendTextMessageAsync(update.Message.Chat.Id, Error_City_Mes);
                                    break;
                                } //Проверка на корректность веденного города

                                _client.SendTextMessageAsync(update.Message.Chat.Id, "Погода в городе " + city + ".", replyMarkup: Weather_Menu());
                                Clear_Chat(update);
                                _new_city = false;
                                break;
                            } //Новый город веденный пользователем
                            switch (text) {
                                case Today_Butt: {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, weather_in_the_city, replyMarkup: Weather_Menu());
                                        Clear_Chat(update);
                                        break;
                                    } //Погода на сегодня
                                case Tomorrow_Butt: {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, weather_in_the_city_tomorrow, replyMarkup: Weather_Menu());
                                        Clear_Chat(update);
                                        break;
                                    } //Погода на завтра
                                case Next_Tomorrow_Butt: {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, weather_in_the_city_next_tomorrow, replyMarkup: Weather_Menu());
                                        Clear_Chat(update);
                                        break;
                                    } //Погода на послезавтра
                                case Another_City_Butt: {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, Weather_Menu_Another_City_Mes);
                                        Clear_Chat(update);
                                        _new_city = true;
                                        break;
                                    } //Ввести другой город
                                case Back_Mes: {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, Main_Menu_Mes, replyMarkup: Main_Menu());
                                        Clear_Chat(update);
                                        _weather_menu = false;
                                        break;
                                    } //Вернуться назад
                                default: {
                                        _client.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                                        break;
                                    } //Удалить непонятное сообщение
                            }
                            break;
                        } //Блок погоды
                        if (_currency_menu) {
                            switch (text) {
                                case CurrencyEuro_Butt: {
                                    _client.SendTextMessageAsync(update.Message.Chat.Id, currency.get_currency("Eur"), replyMarkup: Currency_Menu());
                                    Clear_Chat(update);
                                    break;
                                }
                                case CurrencyUSD_Butt: {
                                    _client.SendTextMessageAsync(update.Message.Chat.Id, currency.get_currency("USD"), replyMarkup: Currency_Menu());
                                    Clear_Chat(update);
                                    break;
                                }
                                case Back_Mes: {
                                    _client.SendTextMessageAsync(update.Message.Chat.Id, Main_Menu_Mes, replyMarkup: Main_Menu());
                                    Clear_Chat(update);
                                    _currency_menu = false;
                                    break;
                                }
                                default: {
                                    _client.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                                    break;
                                }
                            }
                        } //Блок валют
                        if (_timer_menu) {
                            if (_new_time) {
                                bool _stop_timer = false;
                                time = _timer.Set_Time(text);
                                Clear_Chat(update);
                                if (_timer.Err) {
                                    int Iter = 0;
                                    while (_timer.Next_Time(ref time)) {
                                        var updates = _client.GetUpdatesAsync(_offset).Result;

                                        if (updates != null && updates.Length > 0) {
                                            foreach (var up in updates) {
                                                if (up.Message.Text == Stop_Timer_Butt) {
                                                    _stop_timer = true;
                                                }
                                                _offset = update.Id + 1;
                                            }
                                        }
                                        if (_stop_timer)
                                            break;

                                        _client.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId + Iter);
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, _timer.str_time(time), replyMarkup: Timer_Menu_Play());
                                        Iter++;
                                    }
                                }
                                _client.SendTextMessageAsync(update.Message.Chat.Id, Timer_End_Mes, replyMarkup: Timer_Menu_Customization());
                                _new_time = false;
                                break;
                            }

                            switch (text) {
                                case Start_Timer_Butt: {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, Timer_Reference_Mes);
                                        Clear_Chat(update);
                                        _new_time = true;
                                        break;
                                } //Запустить таймер
                                case Back_Mes: {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, Main_Menu_Mes, replyMarkup: Main_Menu());
                                        Clear_Chat(update);
                                        _timer_menu = false;
                                        break;
                                    } //Вернуться назад
                                default: {
                                        _client.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                                        break;
                                    } //Удалить непонятный символ
                            }
                            break;
                        } //Блок таймера
                        if (_mathematics_menu) {

                            if (_mathematics_theorems_menu) {
                                if (_new_theorem) {
                                    if (text == "Назад") {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, Mathematics_Menu_Mes, replyMarkup: Mathematics_Menu());
                                        Clear_Chat(update);
                                        _new_theorem = false;
                                        _mathematics_theorems_menu = false;
                                        break;
                                    }

                                    string path = math.Search_Theorem(text.ToLower());
                                    if (path == Th_Error_Search_Mes) {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, Th_Error_Search_Mes);
                                        break;
                                    }

                                    _client.SendTextMessageAsync(update.Message.Chat.Id, Th_Definition_Mes);
                                    string[] Definition = Directory.GetFiles(path + "\\Условие\\");
                                    foreach (string filename in Definition) {
                                        using (Stream stream = File.OpenRead(filename)) {
                                            var res = _client.SendPhotoAsync(update.Message.Chat.Id, new Telegram.Bot.Types.InputFiles.InputOnlineFile(stream)).Result;
                                        }
                                    }

                                    _client.SendTextMessageAsync(update.Message.Chat.Id, Th_Proof_Mes);
                                    string[] Proof = Directory.GetFiles(path + "\\Доказательство\\");
                                    foreach (string filename in Proof) {
                                        using (Stream stream = File.OpenRead(filename)) {
                                            var res = _client.SendPhotoAsync(update.Message.Chat.Id, new Telegram.Bot.Types.InputFiles.InputOnlineFile(stream)).Result;
                                        }
                                    }

                                    _client.SendTextMessageAsync(update.Message.Chat.Id, Th_End_Mes, replyMarkup: Mathematics_Theorems_Menu());

                                    _new_theorem = false;
                                    break;
                                }
                                if (_new_autor) {
                                    if (text == "Назад") {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, Mathematics_Menu_Mes, replyMarkup: Mathematics_Menu());
                                        Clear_Chat(update);
                                        _new_theorem = false;
                                        _mathematics_theorems_menu = false;
                                        break;
                                    }
                                    _client.SendTextMessageAsync(update.Message.Chat.Id, math.Search_Autor(text.ToLower()), replyMarkup: Mathematics_Theorems_Menu());
                                    _new_autor = false;
                                    break;
                                }
                                switch (text) {
                                    case Th_Show_Theorems_Butt: {
                                            _client.SendTextMessageAsync(update.Message.Chat.Id, Th_Show_Theorems_Mes + math.Get_LT, replyMarkup: Mathematics_Theorems_Menu());
                                            Clear_Chat(update);
                                            break;
                                    } //Вывести список теорем
                                    case Th_Write_Theorems_Butt: {
                                            _client.SendTextMessageAsync(update.Message.Chat.Id, Th_Write_Theorems_Mes);
                                            _new_theorem = true;
                                            break;
                                    } //Написать название теоремы
                                    case Th_Write_Autor_Butt: {
                                            _client.SendTextMessageAsync(update.Message.Chat.Id, Th_Write_Autor_Mes);
                                            Clear_Chat(update);
                                            _new_autor = true;
                                            break;
                                        } //Найти теорему по автору
                                    case Back_Mes: {
                                            _client.SendTextMessageAsync(update.Message.Chat.Id, Mathematics_Menu_Mes, replyMarkup: Mathematics_Menu());
                                            Clear_Chat(update);
                                            _mathematics_theorems_menu = false;
                                            break;
                                    } //Вернуться назад
                                    default: {
                                            _client.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                                            break;
                                    } //Удалить непонятное сообщение
                                }
                                break;
                            } //Блок теорем
                            if (_mathematics_determinant_menu) {
                                if (_new_determinant) {
                                    _client.SendTextMessageAsync(update.Message.Chat.Id, math.Search_Determinant(text), replyMarkup: Mathematics_Determinant_Menu());
                                    Clear_Chat(update);
                                    _new_determinant = false;
                                    break;
                                }
                                switch (text) {
                                    case Matrix_Write_Butt: {
                                            _client.SendTextMessageAsync(update.Message.Chat.Id, Matrix_Write_Mes, replyMarkup: Mathematics_Determinant_Menu());
                                            Clear_Chat(update);
                                            _new_determinant = true;
                                            break;
                                        } //Ввести данные матрицы
                                    case Reference_Butt: {
                                            _client.SendTextMessageAsync(update.Message.Chat.Id, Reference_Matrix_Mes, replyMarkup: Mathematics_Determinant_Menu());
                                            Clear_Chat(update);
                                            break;
                                        } //Получить справку
                                    case Back_Mes: {
                                            _client.SendTextMessageAsync(update.Message.Chat.Id, Mathematics_Menu_Mes, replyMarkup: Mathematics_Menu());
                                            Clear_Chat(update);
                                            _mathematics_determinant_menu = false;
                                            break;
                                        } //Вернуться назад
                                    default: {
                                            _client.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                                            break;
                                        } //Удалить непонятное сообщение
                                }
                                break;
                            } //Блок определителя
                            if (_mathematics_inverse_matrix_menu) {
                                if (_new_inverse) {
                                    _client.SendTextMessageAsync(update.Message.Chat.Id, math.Search_Inverse_Matrix(text), replyMarkup: Mathematics_Inverse_Matrix_Menu());
                                    Clear_Chat(update);
                                    _new_inverse = false;
                                    break;
                                }
                                switch (text) {
                                    case Matrix_Write_Butt: {
                                            _client.SendTextMessageAsync(update.Message.Chat.Id, Matrix_Write_Mes, replyMarkup: Mathematics_Inverse_Matrix_Menu());
                                            Clear_Chat(update);
                                            _new_inverse = true;
                                            break;
                                        } //Ввести данные матрицы
                                    case Reference_Butt: {
                                            _client.SendTextMessageAsync(update.Message.Chat.Id, Reference_Matrix_Mes, replyMarkup: Mathematics_Inverse_Matrix_Menu());
                                            Clear_Chat(update);
                                            break;
                                        } //Получить справку
                                    case Back_Mes: {
                                            _client.SendTextMessageAsync(update.Message.Chat.Id, Mathematics_Menu_Mes, replyMarkup: Mathematics_Menu());
                                            Clear_Chat(update);
                                            _mathematics_theorems_menu = false;
                                            break;
                                        } //Вернуться назад
                                    default: {
                                            _client.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                                            break;
                                        } //Удалить непонятное сообщение
                                }
                                break;
                            } //Блок обратной матрицы
                            if (_mathematics_system_equations_menu) {
                                if (text != null) {
                                    string[] SLE_Mes = text.Split(" ");
                                    string[] tmp = (string[])SLE_Mes.Clone();
                                    if (Array.TrueForAll(tmp, x => x.Replace("-","").All(Char.IsNumber))) {
                                        SLE_Mesgs.Enqueue(SLE_Mes);
                                    }
                                }
                                //Пример:
                                //2 -4 7 10
                                //-9 0 3 -2
                                //0 15 12 36
                                switch (text) {
                                    case Matrix_Write_Butt: {
                                            _client.SendTextMessageAsync(update.Message.Chat.Id, Matrix_Write_Mes, replyMarkup: Mathematics_System_of_Equations_Menu());
                                            //Clear_Chat(update);
                                            //_new_system_first = true;
                                            break;
                                        } //Ввести данные системы
                                    case Reference_Butt: {
                                            _client.SendTextMessageAsync(update.Message.Chat.Id, Reference_System_Mes, replyMarkup: Mathematics_System_of_Equations_Menu());
                                            Clear_Chat(update);
                                            break;
                                        } //Получить справку
                                    case Matrix_GetResult_Butt: {
                                            if (SLE_Mesgs.Count != 0 && SLE_Mesgs.All(x => x.Length - 1 == SLE_Mesgs.Count)) {
                                                string mtrx = "";
                                                string freeTerms = "";
                                                int n = SLE_Mesgs.Count;
                                                for (int i = 0; i < n; i++) {
                                                    //Array.ForEach(SLE_Mesgs.Dequeue(), x => mtrx += x + "");
                                                    var temp = SLE_Mesgs.Dequeue();
                                                    mtrx += String.Join(" ", temp, 0, temp.Length - 1) + " ";
                                                    freeTerms += temp[temp.Length - 1] + " ";
                                                }
                                                _client.SendTextMessageAsync(update.Message.Chat.Id, math.Search_Roots_System_(mtrx.Remove(mtrx.Length - 1), freeTerms.Remove(freeTerms.Length - 1)), replyMarkup: Mathematics_System_of_Equations_Menu());
                                            }

                                            //_client.SendTextMessageAsync(update.Message.Chat.Id, "Ошибка", replyMarkup: Mathematics_System_of_Equations_Menu());
                                            //_new_system_first = true;
                                            break;
                                        } //Получить решение
                                    case Back_Mes: {
                                            _client.SendTextMessageAsync(update.Message.Chat.Id, Mathematics_Menu_Mes, replyMarkup: Mathematics_Menu());
                                            Clear_Chat(update);
                                            _mathematics_theorems_menu = false;
                                            break;
                                        } //Вернуться назад
                                    default: {
                                            _client.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                                            break;
                                        } //Удалить непонятное сообщение
                                }
                                break;
                            } //Блок СЛАУ

                            switch (text) {
                                case Theorems_Butt: {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, Theorems_Menu_Mes, replyMarkup: Mathematics_Theorems_Menu());
                                        Clear_Chat(update);
                                        _mathematics_theorems_menu = true;
                                        break;
                                } //Теоремы
                                case Determinant_Butt: {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, Determinant_Menu_Mes, replyMarkup: Mathematics_Determinant_Menu());
                                        Clear_Chat(update);
                                        _mathematics_determinant_menu = true;
                                        break;
                                } //Определитель
                                case Inverse_Matrix_Butt: {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, Inverse_Matrix_Menu_Mes, replyMarkup: Mathematics_Inverse_Matrix_Menu());
                                        Clear_Chat(update);
                                        _mathematics_inverse_matrix_menu = true;
                                        break;
                                } //Обратная матрица
                                case System_of_Equations_Butt: {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, System_of_Equations_Menu_Mes, replyMarkup: Mathematics_System_of_Equations_Menu());
                                        Clear_Chat(update);
                                        _mathematics_system_equations_menu = true;
                                        break;
                                } //Решение СЛАУ
                                case Back_Mes: {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, Main_Menu_Mes, replyMarkup: Main_Menu());
                                        Clear_Chat(update);
                                        _mathematics_menu = false;
                                        break;
                                } //Вернуться назад
                                default: {
                                        _client.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                                        break;
                                    } //Удалить непонятное сообщение
                            } //Основной блок
                            break;
                        } //Блок математики
                        if (_music_menu) {
                            if (_new_sound) {
                                var voice = update.Message.Voice;
                                if (voice != null) {
                                    File.WriteAllText(Global.Functional_Path + "FilePath.txt", _client.GetFileAsync(voice.FileId).Result.FilePath);
                                    music.Shazam();

                                    //StreamReader sr = new StreamReader(@"D:\Games2\Desktop\shazam bot\TelegramBot\TelegramBot\Functional\text.txt", System.Text.Encoding.GetEncoding(1251));
                                    //string ShazamMusic = sr.ReadToEnd(); 

                                    string ShazamMusic = File.ReadAllText(Global.Functional_Path + "Music.txt");
                                    Console.WriteLine(ShazamMusic);
                                    if (ShazamMusic == "0") {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, Error_Sound_Mes);
                                    }
                                    else {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, ShazamMusic, replyMarkup: Music_Menu());
                                        Clear_Chat(update);
                                        _new_sound = false;
                                    }
                                    break;
                                }
                            }

                            switch (text) {
                                case Record_Butt: {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, Record_Mes);
                                        Clear_Chat(update);
                                        _new_sound = true;
                                        
                                        break;
                                }
                                case Back_Mes: {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, Main_Menu_Mes, replyMarkup: Main_Menu());
                                        Clear_Chat(update);
                                        _music_menu = false;
                                        break;
                                }
                                default: {
                                        _client.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                                        break;
                                }
                            }
                            break;
                        } //Блок музыки
                        if (_geolocation_menu) {
                            switch (text) {
                                case LocationPlaceFindMenuBtn_Mes: {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, LocationPlaceFind_Mes, replyMarkup: Geolocation_Menu());
                                        break;
                                    }
                                case Back_Mes: {
                                        _client.SendTextMessageAsync(update.Message.Chat.Id, Main_Menu_Mes, replyMarkup: Main_Menu());
                                        Clear_Chat(update);
                                        _geolocation_menu = false;
                                        break;
                                    }
                                default: {
                                        _client.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                                        break;
                                    }
                            }
                            if (text != null && text.ToLower().Contains("координаты:"))
                            {
                                var location = new Telegram.Bot.Types.Location();
                                string[] separators = new string[] { "Широта:", "Долгота:" };
                                string[] res = text.Split(separators, StringSplitOptions.None);
                                location.Latitude = Convert.ToDouble(res[1].Replace(" ", "").Replace(".", ","));
                                location.Longitude = Convert.ToDouble(res[2].Replace(" ", "").Replace(".", ","));
                                geolocation.ReversGeoResponse_sendVenue(update, location);
                            }
                            if (update.Message.Location != null)
                            {
                                geolocation.ReversGeoResponse_sendMes(update, update.Message.Location);
                            }
                        } //Блок геолокации

                        switch (text) {
                            case User_Start_Mes: {
                                    _client.SendTextMessageAsync(update.Message.Chat.Id, Start_Mes, replyMarkup: Main_Menu());
                                    break;
                            } //Приветственное сообщение
                            case Weather_Butt: {
                                    _client.SendTextMessageAsync(update.Message.Chat.Id, Weather_Menu_Mes);
                                    Clear_Chat(update);
                                    _weather_menu = true;
                                    _new_city = true;
                                    break;
                            } //Раздел погоды
                            case Exchange_Rate_Butt: {
                                    _client.SendTextMessageAsync(update.Message.Chat.Id, CurrencyStartMes, replyMarkup: Currency_Menu());
                                    Clear_Chat(update);
                                    _currency_menu = true;
                                    break;
                            } //Раздел валют (Разработка)
                            case Timer_Butt: {
                                    _client.SendTextMessageAsync(update.Message.Chat.Id, Timer_Menu_Mes, replyMarkup: Timer_Menu_Customization());
                                    Clear_Chat(update);
                                    for (int i = 0; i < 3; i++)
                                        time[i] = 0;
                                    _timer_menu = true;
                                    break;
                            } //Раздел таймера
                            case Geolocation_Butt:{
                                    _client.SendTextMessageAsync(update.Message.Chat.Id, GeoocationMenuStart_Mes, replyMarkup: Geolocation_Menu());
                                    Clear_Chat(update);
                                    _geolocation_menu = true;
                                    break;
                            } //Раздел геолокации
                            case Mathematics_Butt: {
                                    _client.SendTextMessageAsync(update.Message.Chat.Id, Mathematics_Menu_Mes, replyMarkup: Mathematics_Menu());
                                    Clear_Chat(update);
                                    _mathematics_menu = true;
                                    break;
                            } //Раздел математики
                            case Music_Butt: {
                                    _client.SendTextMessageAsync(update.Message.Chat.Id, Music_Menu_Mes, replyMarkup: Music_Menu());
                                    Clear_Chat(update);
                                    _music_menu = true;
                                    break;
                            } //Раздел музыки
                            default: {
                                    _client.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                                    break;
                            }
                        }
                        
                        break;
                    } //Ответ на текстовые сообщения
                default: {
                        Console.WriteLine(update.Type + " Not implemented!");
                        break;
                }
            }
        }
        private IReplyMarkup Geolocation_Menu() {
            return new ReplyKeyboardMarkup(
                new List<List<KeyboardButton>> {
                    new List<KeyboardButton> {
                        KeyboardButton.WithRequestLocation(LocationGet_MesBtn)
                    },
                    new List<KeyboardButton> {
                        new KeyboardButton(LocationPlaceFindMenuBtn_Mes)
                    },
                    new List<KeyboardButton> {
                        new KeyboardButton(Back_Mes)
                    }
                }
            );
        }//Меню геолокации
        private IReplyMarkup Weather_Menu() {
            return new ReplyKeyboardMarkup(
                 new List<List<KeyboardButton>> {
                     new List<KeyboardButton> {
                        new KeyboardButton(Today_Butt)
                     },
                     new List<KeyboardButton> {
                        new KeyboardButton(Tomorrow_Butt)
                     },
                     new List<KeyboardButton> {
                        new KeyboardButton(Next_Tomorrow_Butt)
                     },
                     new List<KeyboardButton> {
                        new KeyboardButton(Another_City_Butt)
                     },
                     new List<KeyboardButton> {
                        new KeyboardButton(Back_Mes)
                     }
                }
            );
        } //Кнопки меню погоды
        private IReplyMarkup Currency_Menu() {
            return new ReplyKeyboardMarkup(
                 new List<List<KeyboardButton>> {
                    new List<KeyboardButton> {
                        new KeyboardButton(CurrencyEuro_Butt),
                        new KeyboardButton(CurrencyUSD_Butt)
                    },
                    new List<KeyboardButton> {
                        new KeyboardButton(Back_Mes)
                    }
                 }
            );
        } //Кнопки меню валют
        private IReplyMarkup Timer_Menu_Play() {
            return new ReplyKeyboardMarkup(
                 new List<List<KeyboardButton>> {
                     new List<KeyboardButton> {
                        new KeyboardButton(time[0].ToString()),
                        new KeyboardButton(":"),
                        new KeyboardButton(time[1].ToString()),
                        new KeyboardButton(":"),
                        new KeyboardButton(time[2].ToString()),
                     },
                     new List<KeyboardButton> {
                         new KeyboardButton(Stop_Timer_Butt)
                     }
                }
            );
        } //Кнопки меню таймера (ОЖИДАНИЕ)
        private IReplyMarkup Timer_Menu_Customization() {
            return new ReplyKeyboardMarkup(
                 new List<List<KeyboardButton>> {
                     new List<KeyboardButton> {
                        new KeyboardButton(Start_Timer_Butt)
                     },
                     new List<KeyboardButton> {
                        new KeyboardButton(Back_Mes)
                     }
                }
            );
        } //Кнопки меню таймера (НАСТРОЙКА)
        private IReplyMarkup Mathematics_Menu() {
            return new ReplyKeyboardMarkup(
                 new List<List<KeyboardButton>> {
                    new List<KeyboardButton> {
                        new KeyboardButton(Theorems_Butt),
                        new KeyboardButton(Determinant_Butt)
                    },
                    new List<KeyboardButton> {
                        new KeyboardButton(Inverse_Matrix_Butt),
                        new KeyboardButton(System_of_Equations_Butt)
                    },
                    new List<KeyboardButton> {
                        new KeyboardButton(Back_Mes)
                    }
                }
            );
        } //Кнопки математического меню
        private IReplyMarkup Mathematics_Theorems_Menu() {
            return new ReplyKeyboardMarkup(
                 new List<List<KeyboardButton>> {
                    new List<KeyboardButton> {
                        new KeyboardButton(Th_Show_Theorems_Butt)
                    },
                    new List<KeyboardButton> {
                        new KeyboardButton(Th_Write_Theorems_Butt)
                    },
                    new List<KeyboardButton> {
                        new KeyboardButton(Th_Write_Autor_Butt)
                    },
                    new List<KeyboardButton> {
                        new KeyboardButton(Back_Mes)
                    }
                }
            );
        } //Кнопки математического меню (ТЕОРЕМЫ)
        private IReplyMarkup Mathematics_Determinant_Menu() {
            return new ReplyKeyboardMarkup(
                 new List<List<KeyboardButton>> {
                    new List<KeyboardButton> {
                        new KeyboardButton(Matrix_Write_Butt)
                    },
                    new List<KeyboardButton> {
                        new KeyboardButton(Reference_Butt)
                    },
                    new List<KeyboardButton> {
                        new KeyboardButton(Back_Mes)
                    }
                }
            );
        } //Кнопки математического меню (ОПРЕДЕЛИТЕЛЬ)
        private IReplyMarkup Mathematics_Inverse_Matrix_Menu() {
            return new ReplyKeyboardMarkup(
                 new List<List<KeyboardButton>> {
                    new List<KeyboardButton> {
                        new KeyboardButton(Matrix_Write_Butt)
                    },
                    new List<KeyboardButton> {
                        new KeyboardButton(Reference_Butt)
                    },
                    new List<KeyboardButton> {
                        new KeyboardButton(Back_Mes)
                    }
                }
            );
        } //Кнопки математического меню (ОБРАТНАЯ МАТРИЦА)
        private IReplyMarkup Mathematics_System_of_Equations_Menu() {
            return new ReplyKeyboardMarkup(
                 new List<List<KeyboardButton>> {
                    new List<KeyboardButton> {
                        new KeyboardButton(Matrix_Write_Butt)
                    },
                    new List<KeyboardButton> {
                        new KeyboardButton(Reference_Butt)
                    },
                    new List<KeyboardButton> {
                        new KeyboardButton(Matrix_GetResult_Butt)
                    },
                    new List<KeyboardButton> {
                        new KeyboardButton(Back_Mes)
                    }
                }
            );
        } //Кнопки математического меню (СИСТЕМА УРАВНЕНИЙ)
        private IReplyMarkup Music_Menu() {
            return new ReplyKeyboardMarkup(
                 new List<List<KeyboardButton>> {
                    new List<KeyboardButton> {
                        new KeyboardButton(Record_Butt)
                    },
                    new List<KeyboardButton> {
                        new KeyboardButton(Back_Mes)
                    }
                    
                }
            );
        } //Кнопки музыкального меню
        private IReplyMarkup Main_Menu() {
            return new ReplyKeyboardMarkup (
                 new List<List<KeyboardButton>> {
                    new List<KeyboardButton> {
                        new KeyboardButton(Weather_Butt),
                        new KeyboardButton(Exchange_Rate_Butt)
                    },
                    new List<KeyboardButton> {
                        new KeyboardButton(Timer_Butt),
                        new KeyboardButton(Geolocation_Butt)
                    },
                    new List<KeyboardButton> {
                        new KeyboardButton(Mathematics_Butt),
                        new KeyboardButton(Music_Butt)
                    }
                }
            );

        } //Кнопки главного меню
    }
}