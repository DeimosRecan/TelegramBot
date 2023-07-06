using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TelegramBot.Functional {
    [Serializable]
    public class Currency_class {
        public class data {
            public string EURRUB { get; set; }
            public string USDRUB { get; set; }
        }

        [Serializable]
        public class Root {
            [JsonPropertyName("Status")]
            public string st { get; set; }
            [JsonPropertyName("message")]
            public string msg { get; set; }
            [JsonPropertyName("data")]
            public data dt { get; set; }
        }
    }
}