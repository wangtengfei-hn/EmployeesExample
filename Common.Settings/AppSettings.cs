using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Settings
{
    public class AppSettings
    {
        public string ThisHostUrlBase { get; set; }
        public RedisSetting Redis { get; set; }
        public RabbitMQSetting RabbitMQ { get; set; }
        public MongoSetting MongoDB { get; set; }
        public SMSSetting SMS { get; set; }
    }
}
