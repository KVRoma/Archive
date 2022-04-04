using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive.Models
{
    public class Document
    {
        public int Id { get; set; } 
        public DateTime DateCreated { get; set; } = DateTime.Today;

        public string NumberDocument { get; set; }
        public DateTime DateDocument { get; set; } = DateTime.Today;
        public string OwnerDocument { get; set; }   
        public string DescriptionsDocument { get; set; }
        public bool IsScannedDocument { get; set; } = false; 
        public string PathScannedDocument { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public int DocumentTypeId { get; set; }   
        public DocumentType DocumentType { get; set; }  
           
    }
}
