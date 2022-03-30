﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive.Models
{
    public class Book
    {
        public int Id { get; set; } 
        public DateTime DateCreated { get; set; }
        public string NumberBook { get; set; }    
        public string CodeNew { get; set; }    
        public string CodeOld { get; set; } 
        public string Name { get; set; }
        public string House { get; set; }
        public string Apartment { get; set; }

        public int CityId { get; set; } 
        public City City { get; set; }  

        public int StreetId { get; set; }   
        public Street Street { get; set; }      
        
        public int DocumentId { get; set; } 
        public Document Document { get; set; }

        public int BookTypeId { get; set; } 
        public BookType BookType { get; set; }  

    }
}