﻿namespace SportHub.Services.Config
{
    public class EmailServiceConfig
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public string DisplayName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
