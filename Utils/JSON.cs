using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static DataWinLoad.Utils.Types;

namespace DataWinLoad.Utils {
    internal class JSON {
        // Load the json file
        public static Types.BuildJSON? LoadJson(string filePath) {
            Console.WriteLine($"Loading json at {filePath}");

            if (!File.Exists(filePath)) {
                Console.WriteLine("File (json) not found.");
                return null;
            }

            string jsonString = File.ReadAllText(filePath);

            var json = JsonSerializer.Deserialize<Types.BuildJSON>(jsonString);

            if (json.dataPath == null) {
                Console.WriteLine("Missing data.win path.");
                return null;
            }
            if (json.outputDataPath == null) {
                Console.WriteLine("Missing output data.win path.");
                return null;
            }
            if (json.workingDir == null) {
                Console.WriteLine("Missing working directory.");
                return null;
            }

            return json;
        }


        public static string ExtendPath(string basePath, string relativePath) {
            // Normalize the base path
            basePath = Path.GetFullPath(basePath);

            // Combine the base path with the relative path
            string extendedPath = Path.Combine(basePath, relativePath);

            // Normalize the resulting path
            extendedPath = Path.GetFullPath(extendedPath);

            return extendedPath;
        }


    }
}
