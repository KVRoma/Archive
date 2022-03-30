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
        public string Code { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Apartment { get; set; }
        public string DeviceInfo { get; set; }        

        public string CounterName { get; set; } 
        public string CounterNumber { get; set; }   
        public string CounterDate { get; set; }   
        public string CounterChecking { get; set; }   
        public string CounterSeal { get; set; }

        public string CounterInfo 
        {
            get 
            {
                return (string.IsNullOrWhiteSpace(CounterName) ? "" : ("Лічильник " + CounterName + ", " + Environment.NewLine)) +
                       (string.IsNullOrWhiteSpace(CounterNumber) ? "" : ("N " + CounterNumber + ", " + Environment.NewLine)) +
                       (string.IsNullOrWhiteSpace(CounterDate) ? "" : ("Дата виготовлення " + CounterDate + ", " + Environment.NewLine)) +
                       (string.IsNullOrWhiteSpace(CounterChecking) ? "" : ("Дата повірки " + CounterChecking + ", " + Environment.NewLine)) +
                       (string.IsNullOrWhiteSpace(CounterSeal) ? "" : ("Пломба N " + CounterSeal + Environment.NewLine));
            }
        }
    }
}
