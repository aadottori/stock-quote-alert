using System;
namespace stockQuoteAlert
{
    public class Configuration
    {
        public string emailTo { get; set; }
        public string smtpServer { get; set; }
        public int smtpPort { get; set; }
        public string emailFrom { get; set; }
        public string smtpPassword { get; set; }
    }
}

