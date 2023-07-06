using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TelegramBot.Functional {

    public class Currency {
        public Currency() { }
        private string api_key = "b6d3bb6ca7dccc8d1cebf8c9075a6079";

        private HttpWebRequest httpWebRequest;
        private HttpWebResponse httpWebResponse;

        private string url;
        private string response;

        public Currency_class.Root root;

        public string get_currency(string cur) { //получение координат из названия города
            url = $"https://currate.ru/api/?get=rates&pairs=USDRUB,EURRUB&key={api_key}";
            httpWebRequest = (HttpWebRequest)WebRequest.Create(url); //отправляем запрос сервису на получение координат
            httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse(); //сохраняем ответ от сервиса

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream())) {
                response = streamReader.ReadToEnd();
            }

            root = JsonSerializer.Deserialize<Currency_class.Root>(response);

            if (cur.ToLower() == "eur") {
                return "EUR:RUB: " + root.dt.EURRUB;
            }
            return $"USD:RUB:" + root.dt.USDRUB;
        }

    }
}