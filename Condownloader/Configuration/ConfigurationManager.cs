﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condownloader.Configuration
{
    static class ConfigurationManager
    {
        public static string ConfigurationPath;
        static ConfigurationManager()
        {
            ConfigurationPath = Environment.CurrentDirectory;
        }
        public static ConfigurationFile Read()
        {
            if (!File.Exists(Path.Combine(ConfigurationPath, "config.json")))
            {
                Write(new ConfigurationFile());
            }
            using (StreamReader file = File.OpenText(Path.Combine(ConfigurationPath, "config.json")))
            {
                var jsonSerializer = new JsonSerializer();
                return (ConfigurationFile)jsonSerializer.Deserialize(file, typeof(ConfigurationFile));
            }
        }
        public static void Write(ConfigurationFile config)
        {
            if (!Directory.Exists(ConfigurationPath)) Directory.CreateDirectory(ConfigurationPath);
            using (StreamWriter file = File.CreateText(Path.Combine(ConfigurationPath, "config.json")))
            {
                new JsonSerializer().Serialize(file, config);
            }
        }
    }
}
