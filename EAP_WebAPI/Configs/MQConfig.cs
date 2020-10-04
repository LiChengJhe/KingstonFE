using EAP_Library.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAP_WebAPI.Configs
{
    public class MQConfig
    {
        public QueueConfig Repository { get; set; }
        public QueueConfig Dashboard { get; set; }
    }
}
