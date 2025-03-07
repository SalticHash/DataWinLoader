﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UndertaleModLib;
using UndertaleModLib.Models;
using static DataWinLoad.Utils.Types;

namespace DataWinLoad.Utils {
    internal class Data {
        // Load the data.win file
        public static UndertaleData? LoadData(string filePath) {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Loading Data.win at {filePath}");

            if (!File.Exists(filePath)) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("File (data.win) not found");
                return null;
            }
            UndertaleData data;
            using (FileStream fs = new(filePath, FileMode.Open, FileAccess.Read)) {
                data = UndertaleIO.Read(fs);
            }

            stopwatch.Stop();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Loading took {stopwatch.Elapsed}");
            return data;
        }

        // Save the modified data.win file
        public static void SaveData(string filePath, UndertaleData data) {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Saving Data.win at {filePath}");

            if (File.Exists(filePath) && DataWinLoad.config["defaultOverwrite"] != "true") {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("File exists.");
                Console.WriteLine("Overwrite?");
                string? answer = Console.ReadLine();
                if (answer == null || answer != "yes") {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Procces ended.");
                    return;
                } else {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Overwriting.");
                }
            }
            using FileStream fs = new(filePath, FileMode.Create, FileAccess.Write);
            UndertaleIO.Write(fs, data);

            stopwatch.Stop();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Saving took {stopwatch.Elapsed}, Press \"Any\" key to continue");

            //Console.ReadKey();
        }
    }
}
