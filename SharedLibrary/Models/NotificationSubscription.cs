using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class NotificationSubscription
    {
        public string? EndPoint { get; set; }

        public string? P256dh { get; set; }

        public string? Auth { get; set; }
    }
}
