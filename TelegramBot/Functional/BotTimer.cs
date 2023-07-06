using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.Functional {
    public class BotTimer {

        private bool Error = false;

        public BotTimer() {

        }

        public bool Err {
            get { return !Error; }
        }

        public int[] Set_Time(string data) {
            string[] s_time = data.Split(' ');
            int[] i_time = new int[3];

            if (s_time.Length > 3) {
                Error = true;
                return null;
            }

            for(int i = 0; i < 3; i++) {
                if (Int32.TryParse(s_time[i], out i_time[i])) { }
                else { Error = true; return null; }
            }

            while(i_time[2] > 59) {
                i_time[2] -= 60;
                i_time[1] += 1;
            }
            while (i_time[1] > 59) {
                i_time[1] -= 60;
                i_time[0] += 1;
            }

            return i_time;
        }
        public bool Next_Time(ref int[] time) {
            Thread.Sleep(1000);
            if (time[2] > 0) {
                time[2]--;
                return true;
            }
            else if (time[1] > 0) {
                time[1]--;
                time[2] = 59;
                return true;
            }
            else if (time[0] > 0) {
                time[0]--;
                time[1] = 59;
                time[2] = 59;
                return true;
            }

            return false;
        }
        public string str_time(int[] time) {
            return time[0].ToString() + ":" + time[1].ToString() + ":" + time[2].ToString();
        }


    }
}
