using System;
using System.Text.Json;

namespace stockQuoteAlert
{
    public class ConfigurationReader
    {
        public Configuration ReadConfiguration()
        {
            string json = File.ReadAllText("config.json");
            Configuration config = JsonSerializer.Deserialize<Configuration>(json);
            return config;
        }
    }
}

