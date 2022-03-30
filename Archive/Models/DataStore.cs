using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive.Models
{
    public class DataStore
    {
        public int Id { get; set; }
        public string Rax { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Apartment { get; set; }
        public string DeviceInfo { get; set; }
        public string CounterInfo { get; set; }
    }
}
