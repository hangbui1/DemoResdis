using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWinformRedis.Models
{
    public class Employee
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AddressEmail { get; set; }
        public DateTime DateJoin { get; set; }
    }
}
