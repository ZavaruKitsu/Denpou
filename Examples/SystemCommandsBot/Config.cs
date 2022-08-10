using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SystemCommandsBot.Commands;

namespace SystemCommandsBot
{
    public class Config
    {
        public Config()
        {
            Commandos = new List<Commando>();
        }

        public string Password { get; set; }

        public string ApiKey { get; set; }

        public List<Commando> Commandos { get; set; }

        public void LoadDefaultValues()
        {
            ApiKey = "";
            Commandos.Add(new Commando
            {
                ID = 0, Enabled = true, Title = "Test Befehl", ShellCmd = "explorer.exe", Action = "start",
                MaxInstances = 2
            });
        }


        public static Config Load()
        {
            try
            {
                return LoadFrom(AppContext.BaseDirectory + "config\\default.cfg");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }


        public static Config LoadFrom(string path)
        {
            try
            {
                var cfg = JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));
                return cfg;
            }
            catch (DirectoryNotFoundException ex)
            {
                var di = new DirectoryInfo(path);

                if (!Directory.Exists(di.Parent.FullName)) Directory.CreateDirectory(di.Parent.FullName);

                var cfg = new Config();
                cfg.LoadDefaultValues();
                cfg.Save(path);
                return cfg;
            }
            catch (FileNotFoundException ex)
            {
                var cfg = new Config();
                cfg.LoadDefaultValues();
                cfg.Save(path);
                return cfg;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public void Save(string path)
        {
            try
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(this));
            }
            catch
            {
            }
        }
    }
}