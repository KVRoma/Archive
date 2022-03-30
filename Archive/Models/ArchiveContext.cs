using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive.Models
{
    public class ArchiveContext : DbContext
    {
        public ArchiveContext() : base("ConStr") { }

        public DbSet<DataStore> DataStores { get; set; }
        public DbSet<City> Cities { get; set; } 
        public DbSet<Street> Streets { get; set; }  
        public DbSet<DocumentType> DocumentTypes { get; set; }  
        public DbSet<Document> Documents { get; set; }  
        public DbSet<BookType> BookTypes { get; set; }  
        public DbSet<Book> Books { get; set; }  
                


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<ArchiveContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }
    }
}
