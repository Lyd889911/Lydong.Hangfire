using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lydong.Hangfire.Symbols
{
    [AttributeUsage(AttributeTargets.Method)]
    public class JobAttribute: Attribute
    {
        public string Cron { get; set; }
    }
}
