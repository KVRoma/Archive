using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archive.Models;

namespace Archive.ViewModels
{
    public class MainViewModel : ViewModel
    {
        ArchiveContext db;
        private IEnumerable<DataStore> dataStores;
        private DataStore dataStoreSelect;

        public string TitleView { get; } = "Archive - 2022";
        public string TitleDown { get; } = "My project";
        public IEnumerable<DataStore> DataStores 
        {
            get => dataStores; 
            set 
            { 
                dataStores = value;
                OnPropertyChanged(nameof(DataStores));
            } 
        }
        public DataStore DataStoreSelect 
        {
            get => dataStoreSelect;
            set 
            {
                dataStoreSelect = value; 
                OnPropertyChanged(nameof(DataStoreSelect));
            }
        }

        





        public MainViewModel()
        {
            db = new ArchiveContext();
            db.DataStores.Load();
            db.Cities.Load();
            db.Streets.Load();
            db.DocumentTypes.Load();    
            db.Documents.Load();    
            db.BookTypes.Load();    
            db.Books.Load();
        }
    }
}
