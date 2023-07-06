using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TelegramBot.Functional {

    public class Weather_data {

        public class list {
            public double dt { get; set; }

            public main main { get; set; }

            public List<weather> weather { get; set; }

            public clouds clouds { get; set; }
            public wind wind { get; set; }
            public double visibility { get; set; }
            public double pop { get; set; }
            public rain rain { get; set; }
            public snow snow { get; set; }
            public sys sys { get; set; }
            public string dt_txt { get; set; }

        } //end of list

        public class root {
            public string cod { get; set; }
            public double message { get; set; }
            public int cnt { get; set; }
            public List<list> list { get; set; }
            public city city { get; set; }
        }

        public class main {
            public double temp { get; set; }
            public double feels_like { get; set; }
            public double temp_min { get; set; }
            public double temp_max { get; set; }
            public double pressure { get; set; }
            public double sea_level { get; set; }
            public double grnd_level { get; set; }
            public double humidity { get; set; }
            public double temp_kf { get; set; }
        } //end of main
        public class weather {
            public int id { get; set; }
            public string main { get; set; }
            public string description { get; set; }
            //string icon { get; set; }
        }//end of weather

        public class clouds {
            public double all { get; set; }
        }//end of clouds

        public class wind {
            public double speed { get; set; }
            public double deg { get; set; }
            public double gust { get; set; }
        }//end of wind

        public class rain {
            [JsonPropertyName("3h")]
            public double _3h { get; set; }
        } //end of rain


        public class snow {
            [JsonPropertyName("3h")]
            public double _3h { get; set; }
        } //end of snow

        public class sys {
            public char pod { get; set; }
        }//end of sys

        public class coord {
            public double lat { get; set; }
            public double lon { get; set; }

        }//end of coord

        public class city {
            public double id { get; set; }
            public string name { get; set; }
            coord coord;
            public string country { get; set; }
            public int population { get; set; }
            public double timezone { get; set; }
            public double sunrise { get; set; }
            public double sunset { get; set; }
        }//end of city

    }
}
