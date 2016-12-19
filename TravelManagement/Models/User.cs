using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMvvmToolkit;

namespace TravelManagement.Models
{
    public class User 
    {
        public string PhoneNum { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Nickname { get; set; }

        public string Sex { get; set; }

        public byte[] Avatar { get; set; }
        public string Description { get; set; }
        public string Birthday { get; set; }

        public string RegisterTime { get; set; }
        public int? IsPointOpen { get; set; }
    }
}
