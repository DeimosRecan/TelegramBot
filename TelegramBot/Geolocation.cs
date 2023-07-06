using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Telegram.Bot;

namespace TelegramBot
{
    internal class Geolocation
    {
        public async void ReversGeoResponse_sendVenue(Telegram.Bot.Types.Update update, Telegram.Bot.Types.Location location)
        {
            var client = new HttpClient();
            //var Location = update.Message.Location;
            var requestAddres = "https://forward-reverse-geocoding.p.rapidapi.com/v1/reverse?lat=" + location.Latitude.ToString().Replace(",", ".")
                         + "&lon=" + location.Longitude.ToString().Replace(",", ".") + "&accept-language=en&polygon_threshold=0.0";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestAddres),
                Headers =
                        {
                            { "X-RapidAPI-Key", "49267a2620msh5e0f6e893c6147ap1d0d4ajsne7e0a107a680" },
                            { "X-RapidAPI-Host", "forward-reverse-geocoding.p.rapidapi.com" },
                        },
            };
            using (var response = await client.SendAsync(request))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    var reverseGeodata = (await response.Content.ReadAsStringAsync()).Split("\"display_name\":\"")[1].Replace("/", "");
                    Console.WriteLine(reverseGeodata.Remove(reverseGeodata.Length - 2));
                    TelegramBotProject.GetClient().SendVenueAsync(update.Message.Chat.Id,
                                           latitude: location.Latitude,
                                           longitude: location.Longitude,
                                           title: "",
                                           address: reverseGeodata.Remove(reverseGeodata.Length - 2));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
        public async void ReversGeoResponse_sendMes(Telegram.Bot.Types.Update update, Telegram.Bot.Types.Location location)
        {
            var client = new HttpClient();
            //var Location = update.Message.Location;
            var requestAddres = "https://forward-reverse-geocoding.p.rapidapi.com/v1/reverse?lat=" + location.Latitude.ToString().Replace(",", ".")
                         + "&lon=" + location.Longitude.ToString().Replace(",", ".") + "&accept-language=en&polygon_threshold=0.0";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestAddres),
                Headers =
                        {
                            { "X-RapidAPI-Key", "49267a2620msh5e0f6e893c6147ap1d0d4ajsne7e0a107a680" },
                            { "X-RapidAPI-Host", "forward-reverse-geocoding.p.rapidapi.com" },
                        },
            };
            using (var response = await client.SendAsync(request))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    var reverseGeodata = (await response.Content.ReadAsStringAsync()).Split("\"display_name\":\"")[1].Replace("/", "");
                    Console.WriteLine(reverseGeodata.Remove(reverseGeodata.Length - 2));
                    TelegramBotProject.GetClient().SendTextMessageAsync(update.Message.Chat.Id, reverseGeodata.Remove(reverseGeodata.Length - 2));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
