// Made by GitMuslim

using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using UndertaleModLib.Util;
using UndertaleModLib.Models;
using UndertaleModLib;
using DataWinLoad.Utils;
using UndertaleModLib.Scripting;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace DataWinLoad {
    internal class DataWinLoad {

        static public UndertaleData? data;

        static public string? workingDir;
        static public string? jsonPath;

        static public Dictionary<string, UndertaleEmbeddedTexture> textureIDs = [];
        static public Dictionary<string, UndertaleTexturePageItem> pageItemIDs = [];

        static public Types.BuildJSON? json;

        static public IConfigurationRoot? config;

        static void Main(string[] args) {
            // Handling unhandled errors, are they unhandled anymore?
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => {
                Exception ex = (Exception)e.ExceptionObject;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unhandled exception: " + ex.Message);
            };

            // Load or create the config.ini
            string exeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string exeDirectory = Path.GetDirectoryName(exeFilePath);

            string iniPath = Path.Combine(exeDirectory, "config.ini");
            if (!File.Exists(iniPath)) {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Please re-run, config file was created.");
                File.Create(iniPath);
                return;
            }

            config = new ConfigurationBuilder()
                .AddIniFile("config.ini", true)
                .Build();

            // Kill game procces
            if (config["gamePath"] != null) {
                var name = Path.GetFileNameWithoutExtension(config["gamePath"]);
                foreach (var process in Process.GetProcessesByName(name)) {
                    process.Kill();
                }
            }

            // Warn dev mode
            if (config["devMode"] == "true") {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("DevMode Active, the main loop will repeat if you press the enter or space key");
                Console.WriteLine("after the execution of the saving, you can exit by pressing 'X'");
            }


            // Benchmark
            Stopwatch stopwatch = new();
            stopwatch.Start();


            // Get json path
            if (config["jsonPath"] == null) {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Enter the path to the json: ");
                jsonPath = Console.ReadLine();
                if (jsonPath == null) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Input for path failed");
                    return;
                };
            } else {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Used preset json path: \"{config["jsonPath"]}\" (set in config.ini)");
                jsonPath = config["jsonPath"];
            }


            // Load json
            json = JSON.LoadJson(jsonPath);
            if (json == null) return;

            // Set working directory
            workingDir = json?.workingDir;

            // Load data
            data = Data.LoadData(json?.dataPath);
            if (data == null) return;



            // Add and load textures
            if (json?.textures == null) goto skipImageProccesing;
            foreach (var texture in json?.textures) {
                Sprites.AddTexture(data, texture);
            }

            // Add and load sprites
            if (json?.sprites == null) goto skipImageProccesing;
            foreach (var sprite in json?.sprites) {
                Sprites.AddSprite(data, sprite);
            }

            skipImageProccesing:

            // Add and load objects
            foreach (var obj in json?.objects) {
                Objects.AddObject(data, obj);
            }

            while (true) {

                // Add and load scripts
                foreach (var script in json?.scripts) {
                    Scripts.AddScript(data, script);
                }

                // Add and load object events
                foreach (var obj in json?.objects) {
                    Objects.AddObjectEvents(data, obj);
                }


                // Dev mode management
                if (config["devMode"] == "true") {
                    var key = Console.ReadKey();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"{key.KeyChar}");
                    if (key.Key == ConsoleKey.X) {
                        Console.WriteLine("Exited loop.");
                        break;
                    }
                    if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar) {
                        Data.SaveData(json?.outputDataPath, data);
                        continue;
                    }
                } else {
                    break;
                }
            };


            // Save data.win to the output path with all it's changes
            Data.SaveData(json?.outputDataPath, data);


            // Benchmark
            stopwatch.Stop();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n {stopwatch.Elapsed}");

            // Start game if game path
            if (config["gamePath"] != null) {
                var game = new ProcessStartInfo(config["gamePath"]);
                Process.Start(game);
            } else {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nPress any key to close...");
                Console.ReadKey();
            }
        }
    }
}


