using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive.Models
{
    public class BookType
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public string Key { get; set; } 
        public List<Book> Books { get; set; } = new List<Book>();   
    }
}
