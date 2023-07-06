using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace TelegramBot.Functional {
    public class Weather {

        protected const string api_key = "b32390396081a90dab33fbd6a976f973"; //APPID - апи-ключ для доступа к интерфейсу сервиса

        private HttpWebRequest httpWebRequest; //переменная для создания веб-запроса для сервиса
        private HttpWebResponse httpWebResponse; //переменная для получения ответа от сервиса на запрос

        private double lat; //широта (географические координаты)
        private double lon; //долгота

        private string url; //ссылка

        private string response;

        public Weather_data.root DeserializedClass;

        private bool Error_City;

        public Weather() { }

        public void get_coordinates_by_city_name(string city_name) { //получение координат из названия города

            url = $"http://api.openweathermap.org/geo/1.0/direct?q={city_name}&limit=1&appid={api_key}"; //настраиваем ссылку на сервис
            httpWebRequest = (HttpWebRequest)WebRequest.Create(url); //отправляем запрос сервису на получение координат
            httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse(); //сохраняем ответ от сервиса

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream())) {
                response = streamReader.ReadToEnd();
                string[] separators = new string[] { "\"lat\":", ",\"lon\":", ",\"country\"" };
                string[] res = response.Split(separators, StringSplitOptions.None);

                res[1] = res[1].Replace('.', ',');
                res[2] = res[2].Replace('.', ',');

                lat = double.Parse(res[1]);
                lon = double.Parse(res[2]);
            }
        }
        public string get_current_weather_by_city_name(string city_name) {
            Error_City = false;
            get_coordinates_by_city_name(city_name);

            if (Error_City)
                return "Error";

            url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&lang=ru&lang=ru&appid={api_key}";

            httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream())) {
                response = streamReader.ReadToEnd();
                Console.WriteLine(response);
            }
            Console.WriteLine();


            string[] separators = new string[] { "\"description\":\"", "\",\"icon\"", "\"temp\":", ",\"feels_like\":", ",\"temp_min\"",
            "\"pressure\":",",\"humidity\":","\"speed\":",",\"deg\""};
            string[] res = response.Split(separators, StringSplitOptions.None);

            res[1] = res[1].Replace(res[1][0], char.ToUpper(res[1][0]));
            res[3] = res[3].Replace('.', ','); //температура
            res[4] = res[4].Replace('.', ','); //ощущается
            res[6] = res[6].Replace('.', ','); //давление
            res[8] = res[8].Replace('.', ','); //скорость ветра

            string description = res[1];
            double temperature = Math.Round(double.Parse(res[3]) - 273, 1);
            double feels_like = Math.Round(double.Parse(res[4]) - 273, 1);
            double pressure = double.Parse(res[6]) * 0.75;
            double wind_speed = double.Parse(res[8]);

            string res_string = $"{description}. \nТемпература: {temperature} градусов цельсия. Ощущается как {feels_like} градусов цельсия. " +
                $"\nАтмосферное давление: {pressure} мм. рт. ст. \nСкорость ветра: {wind_speed} м/с.";

            return res_string;

        }
        public string get_weather_for_tomorrow_days_by_hours_by_city_name(string city_name) {
            Error_City = false;
            get_coordinates_by_city_name(city_name);

            if (Error_City)
                return "Error";

            url = $"http://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&appid={api_key}&lang=ru";

            httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
                //Console.WriteLine(response);
            }
            Console.WriteLine();

            DeserializedClass = JsonSerializer.Deserialize<Weather_data.root>(response);


            string description = DeserializedClass.list[5].weather[0].description;
            description = description.Replace(description[0], char.ToUpper(description[0]));
            double temperature = Math.Round(DeserializedClass.list[5].main.temp - 273, 1);
            double feels_like = Math.Round(DeserializedClass.list[5].main.feels_like - 273, 1);
            double pressure = DeserializedClass.list[5].main.pressure * 0.75;
            double wind_speed = DeserializedClass.list[5].wind.speed;

            string res = $"Прогноз на {DeserializedClass.list[5].dt_txt}: \n{description} \nТемпература: {temperature} градусов цельсия." +
                $" Ощущается как {feels_like} градусов цельсия.\nАтмосферное давление: {pressure} мм. рт. ст. \nСкорость ветра: {wind_speed} м/с.";

            return res;
        }
        public string get_weather_for_day_after_tomorrow_days_by_hours_by_city_name(string city_name) {
            Error_City = false;
            get_coordinates_by_city_name(city_name);

            if (Error_City)
                return "Error";

            url = $"http://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&appid={api_key}&lang=ru";

            httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream())) {
                response = streamReader.ReadToEnd();
                //Console.WriteLine(response);
            }
            Console.WriteLine();

            DeserializedClass = JsonSerializer.Deserialize<Weather_data.root>(response);

            string description = DeserializedClass.list[13].weather[0].description;
            description = description.Replace(description[0], char.ToUpper(description[0]));
            double temperature = Math.Round(DeserializedClass.list[13].main.temp - 273, 1);
            double feels_like = Math.Round(DeserializedClass.list[13].main.feels_like - 273, 1);
            double pressure = DeserializedClass.list[13].main.pressure * 0.75;
            double wind_speed = DeserializedClass.list[13].wind.speed;

            string res = $"Прогноз на {DeserializedClass.list[13].dt_txt}: \n{description} \nТемпература: {temperature} градусов цельсия." +
                $" Ощущается как {feels_like} градусов цельсия.\nАтмосферное давление: {pressure} мм. рт. ст. \nСкорость ветра: {wind_speed} м/с.";

            return res;
        }
    }
}