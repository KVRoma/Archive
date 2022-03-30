using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive.Models
{
    public class DocumentType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Document> Documents { get; set; } = new List<Document>();   
    }
}
