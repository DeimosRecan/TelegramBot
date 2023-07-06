using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot {
    public static class Global {
        public static readonly string Project_Path = System.IO.Directory.GetCurrentDirectory().Replace(@"\bin\Debug\netcoreapp3.1", "");
        public static readonly string Functional_Path = Project_Path + @"\Functional\";
        public static readonly string Math_Data_Path = Project_Path + @"\Math_Data\";
    }
}
