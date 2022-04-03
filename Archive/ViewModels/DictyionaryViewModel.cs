using Archive.Models;
using Archive.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Archive.ViewModels
{
    public class DictyionaryViewModel : ViewModel
    {
        ArchiveContext db;
        string dicName;
        private string nameTable;

        private IEnumerable<object> context;
        private object contextSelect;       
        private object itemSelect;

        private Dictionary<string, Visibility> isVisibility;

        //private IEnumerable<City> cities;
        //private City citySelect;
        //private IEnumerable<Street> streets;
        //private Street streetSelect;
        //private IEnumerable<DocumentType> documentTypes;
        //private DocumentType documentTypeSelect;
        //private IEnumerable<BookType> bookTypes;
        //private BookType bookTypeSelect;


        public string Title => "Довідник";
        public string NameTable
        {
            get => nameTable;
            set
            {
                nameTable = value;
                OnPropertyChanged(nameof(NameTable));
            }
        }
        //public IEnumerable<City> Cities
        //{
        //    get => cities;
        //    set
        //    {
        //        cities = value;
        //        OnPropertyChanged(nameof(Cities));
        //    }
        //}
        //public City CitySelect
        //{
        //    get => citySelect;
        //    set
        //    {
        //        citySelect = value;
        //        OnPropertyChanged(nameof(CitySelect));
        //        Streets = db.Streets.Local.ToBindingList().Where(x => x.Id == citySelect.Id);
        //    }
        //}
        //public IEnumerable<Street> Streets
        //{
        //    get => streets;
        //    set
        //    {
        //        streets = value;
        //        OnPropertyChanged(nameof(Streets));                
        //    }
        //}
        //public Street StreetSelect
        //{
        //    get => streetSelect;
        //    set
        //    {
        //        streetSelect = value;
        //        OnPropertyChanged(nameof(StreetSelect));
        //    }
        //}
        //public IEnumerable<DocumentType> DocumentTypes
        //{
        //    get => documentTypes;
        //    set
        //    {
        //        documentTypes = value;
        //        OnPropertyChanged(nameof(DocumentTypes));
        //    }
        //}
        //public DocumentType DocumentTypeSelect
        //{
        //    get => documentTypeSelect;
        //    set
        //    {
        //        documentTypeSelect = value;
        //        OnPropertyChanged(nameof(DocumentTypeSelect));
        //    }
        //}
        //public IEnumerable<BookType> BookTypes
        //{
        //    get => bookTypes;
        //    set
        //    {
        //        bookTypes = value;
        //        OnPropertyChanged(nameof(BookTypes));
        //    }
        //}
        //public BookType BookTypeSelect
        //{
        //    get => bookTypeSelect;
        //    set
        //    {
        //        bookTypeSelect = value;
        //        OnPropertyChanged(nameof(BookTypeSelect));
        //    }
        //}

        public IEnumerable<object> Context
        {
            get => context;
            set 
            {
                context = value;
                OnPropertyChanged(nameof(Context));
            }
        }
        public object ContextSelect
        {
            get => contextSelect;
            set
            {
                contextSelect = value;
                OnPropertyChanged(nameof(ContextSelect));                
            }
        }       
        public object ItemSelect
        {
            get => itemSelect;
            set 
            { 
                itemSelect = value;                
                OnPropertyChanged(nameof(ItemSelect));                 
            }
        }

        public Dictionary<string, Visibility> IsVisibility
        {
            get => isVisibility;
            set
            {
                isVisibility = value;
                OnPropertyChanged(nameof(IsVisibility));
            }
        }


        private Command _save;
        public Command Save => _save ?? (_save = new Command(obj => 
        {            
            if (obj is System.Windows.Window)
            {
                db.SaveChanges();
                (obj as System.Windows.Window).Close();
            }
        }));

        public DictyionaryViewModel(ref ArchiveContext _db, string _dicName)
        {
            db = _db;
            dicName = _dicName;
            LoadIsIsVisibility();
            Load();
        }

        private void LoadIsIsVisibility()
        {
            IsVisibility = new Dictionary<string, Visibility>
            {
                { ControlName.CityDictionary, Visibility.Collapsed},                
                { ControlName.BookDictionary, Visibility.Collapsed},
                { ControlName.DocumentDictionary, Visibility.Collapsed}
            };
        }
        private void Load()
        {
            switch (dicName)
            {
                case ControlName.CityDictionary:
                    {                         
                        Context = db.Cities.Local.ToBindingList();
                        IsVisibility[ControlName.CityDictionary] = Visibility.Visible;
                        NameTable = "Населені пункти";
                    }
                    break;                
                case ControlName.BookDictionary:
                    {                         
                        Context = db.BookTypes.Local.ToBindingList();
                        IsVisibility[ControlName.BookDictionary] = Visibility.Visible;
                        NameTable = "Перелік справ";
                    }
                    break;
                case ControlName.DocumentDictionary:
                    {                         
                        Context = db.DocumentTypes.Local.ToBindingList();
                        IsVisibility[ControlName.DocumentDictionary] = Visibility.Visible;
                        NameTable = "Перелік документів";
                    }
                    break;
                default:
                    break;
            }
        }

    }
}
