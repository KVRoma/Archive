using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive.Models
{
    public class AppContext : DbContext
    {
        public AppContext() : base("ConStr") { }

        public DbSet<DataStore> DataStores { get; set; }  
        //public DbSet<Adress> Adresse { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<AppContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }
    }
}
