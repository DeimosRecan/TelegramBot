using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Python.Runtime;

namespace TelegramBot.Functional {
    public class Shazampy {
        public Shazampy() {

        }

        public void Shazam() {
            using (Py.GIL()) { // Global Interpreter Lock
                using (var scope = Py.CreateScope()) {

                    //import libs
                    scope.Exec("import sys");
                    List<string> _paths = Directory.GetDirectories(@"C:\Users\Максим\AppData\Local\Programs\Python\Python38\Lib\site-packages\", "*", SearchOption.AllDirectories).ToList();
                    foreach (string path in _paths) {
                        scope.Exec("sys.path.append(r'* + path + *')");
                    }
                    //
                    scope.Exec(File.ReadAllText(Global.Functional_Path + "testing.py"));
                }
            }
        }
        public void ShazamInit() {
            Runtime.PythonDLL = @"C:\Users\Максим\AppData\Local\Programs\Python\Python38\python38.dll";
            string envPythonHome = @"C:\Users\Максим\AppData\Local\Programs\Python\Python38";

            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", envPythonHome, EnvironmentVariableTarget.Process);

            PythonEngine.Initialize();
        }
        
    }
}
