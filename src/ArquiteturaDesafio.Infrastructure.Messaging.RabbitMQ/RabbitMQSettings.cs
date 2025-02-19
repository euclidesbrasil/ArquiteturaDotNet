using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMQSettings
    {
        public string Hostname { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }
}
