using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UndertaleModLib;
using UndertaleModLib.Models;

namespace DataWinLoad.Utils {
    internal class Scripts {
        public static UndertaleCode? AddScript(UndertaleData data, Types.Script script) {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Adding Script with name {script.name}");
            var name = data.Strings.MakeString(script.name);
            var path = Path.GetFullPath(script.path, DataWinLoad.workingDir);

            if (!File.Exists(path)) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Code not found at path \"{path}\"");
                return null;
            }

            var gml = File.ReadAllText(path);

            UndertaleCode? code = data.Code.ByName(script.name);
            var exists = code != null;

            if (!exists) {
                code = new() { Name = name };
                data.Code.Add(code);

                var scriptObj = new UndertaleScript { Name = name, Code = code };
                data.Scripts.Add(scriptObj);
                if (script.global == true) {
                    UndertaleGlobalInit init = new() {Code = code};
                    data.GlobalInitScripts.Add(init);
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            try {
                if (script.append == true) {
                    code.AppendGML(gml, data);
                } else {
                    code.ReplaceGML(gml, data);
                }
            } catch (Exception ex) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                return null;
            }


            return code;
        }
    }
}
