using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Archive.Models;
using Archive.Services;

namespace Archive.ViewModels
{
    public class MainViewModel : ViewModel
    {
        #region private property
        ArchiveContext db;
        
                
        private Dictionary<string, Visibility> isVisibility;
        private Dictionary<string, bool> isChecked;
        private Dictionary<string, double> isOpacity;
        private Dictionary<string, string> textBoxData;
        private IEnumerable<DataStore> dataStores;
        private DataStore dataStoreSelect;
        private IEnumerable<City> cities;
        private City citySelect;
        private IEnumerable<Street> streets;
        private Street streetSelect;
        private IEnumerable<DocumentType> documentTypes;
        private DocumentType documentTypeSelect;
        private IEnumerable<Document> documents;
        private Document documentSelect;
        private IEnumerable<BookType> bookTypes;
        private BookType bookTypeSelect;
        private IEnumerable<Book> books;
        private Book bookSelect;
        #endregion

        #region public property
        public string TitleView { get; } = "Archive - 2022";
        public string TitleDown { get; } = "© <Kuchinik & Co.>, 2022";
       
        public Dictionary<string, Visibility> IsVisibility
        {
            get => isVisibility; 
            set
            {
                isVisibility = value;
                OnPropertyChanged(nameof(IsVisibility));
            }
        }
        public Dictionary<string, bool> IsChecked
        {
            get => isChecked; 
            set
            { 
                isChecked = value;
                OnPropertyChanged(nameof(IsChecked));                
            }
        }
        public Dictionary<string, double> IsOpacity
        {
            get => isOpacity;
            set
            {
                isOpacity = value;
                OnPropertyChanged(nameof(IsOpacity));
            }
        }
        public Dictionary<string, string> TextBoxData 
        { 
            get => textBoxData;
            set 
            { 
                textBoxData = value;
                OnPropertyChanged(nameof(TextBoxData));
            } 
        }

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

                if (DataStoreSelect != null)
                {
                    CitySelect = Cities.FirstOrDefault(x => x.Name == DataStoreSelect.City);
                    StreetSelect = Streets.FirstOrDefault(x => x.Name == DataStoreSelect.Street);
                    TextBoxData[ControlName.House] = DataStoreSelect.House;
                    TextBoxData[ControlName.Apartment] = DataStoreSelect.Apartment;
                    TextBoxData[ControlName.CodeNew] = DataStoreSelect.Code;
                    TextBoxData[ControlName.NameBook] = DataStoreSelect.Name;
                    OnPropertyChanged(nameof(TextBoxData));
                }
            }
        }

        public IEnumerable<City> Cities 
        { 
            get => cities;
            set 
            {
                cities = value;
                OnPropertyChanged(nameof(Cities));
            } 
        }
        public City CitySelect 
        { 
            get => citySelect;
            set 
            {
                citySelect = value; 
                OnPropertyChanged(nameof(CitySelect));
                LoadStreet();
            }
        }
        public IEnumerable<Street> Streets 
        { 
            get => streets;
            set 
            { 
                streets = value; 
                OnPropertyChanged(nameof(Streets));
            }
        }
        public Street StreetSelect 
        { 
            get => streetSelect;
            set 
            { 
                streetSelect = value; 
                OnPropertyChanged(nameof(StreetSelect));
            }
        }
        public IEnumerable<DocumentType> DocumentTypes
        {
            get => documentTypes; 
            set
            {
                documentTypes = value;
                OnPropertyChanged(nameof(DocumentTypes));
            }
        }
        public DocumentType DocumentTypeSelect
        {
            get => documentTypeSelect; 
            set
            {
                documentTypeSelect = value;
                OnPropertyChanged(nameof(DocumentTypeSelect));
            }
        }
        public IEnumerable<Document> Documents
        {
            get => documents; 
            set
            {
                documents = value;
                OnPropertyChanged(nameof(Documents));
            }
        }
        public Document DocumentSelect
        {
            get => documentSelect; 
            set
            {
                documentSelect = value;
                OnPropertyChanged(nameof(DocumentSelect));
            }
        }
        public IEnumerable<BookType> BookTypes
        {
            get => bookTypes; 
            set
            {
                bookTypes = value;
                OnPropertyChanged(nameof(BookTypes));
            }
        }
        public BookType BookTypeSelect
        {
            get => bookTypeSelect; 
            set
            {
                bookTypeSelect = value;
                OnPropertyChanged(nameof(BookTypeSelect));
            }
        }
        public IEnumerable<Book> Books
        {
            get => books; 
            set
            {
                books = value;
                OnPropertyChanged(nameof(Books));
            }
        }
        public Book BookSelect
        {
            get => bookSelect; 
            set
            {
                bookSelect = value;
                OnPropertyChanged(nameof(BookSelect));
            }
        }
        #endregion

        #region command
        private Command _exitApp;
        private Command _checked;
        private Command _search;
        private Command _clearAdded;
        private Command _searchAdded;

        public Command ExitApp => _exitApp ?? (_exitApp = new Command(obj =>
        {
            ExitApplication();
        }));
        public Command Checked => _checked ?? (_checked = new Command(async obj =>
        {            
            OnPropertyChanged(nameof(IsChecked));
            ProgressBarStart();
            await Task.Run(() => 
            {
                LoadBook();                
                CitySelect = null;
                BookTypeSelect = BookTypes.First();
                
            });
            ProgressBarStop();                        
        }));
        public Command Search => _search ?? (_search = new Command(obj =>
        {
            string item = obj.ToString() + "city" + CitySelect?.Id.ToString() + "street" + StreetSelect?.Id.ToString();
            LoadBook();
            if (!string.IsNullOrWhiteSpace(item))
            {
                Books = Books.Where(x => x.Search.ToUpper().Contains(item.ToUpper()));
            }
        }));
        public Command ClearAdded => _clearAdded ?? (_clearAdded = new Command(async obj =>
        {            
            ProgressBarStart();
            await Task.Run(() =>
            {
                LoadTextBoxData();                
                DataStoreSelect = null;                
                LoadDataStore();                
                CitySelect = null;
                BookTypeSelect = null;
            });
            ProgressBarStop();            
        }));
        public Command SearchAdded => _searchAdded ?? (_searchAdded = new Command(obj =>
        {
            string str = CitySelect?.Name + StreetSelect?.Name + TextBoxData[ControlName.House] + TextBoxData[ControlName.Apartment];
            DataStores = DataStores.Where(x=>x.Search.ToUpper().Contains(str.ToUpper()));
        }));
                
        #endregion

        public MainViewModel()
        {
            LoadIsIsVisibility();
            LoadIsChecked();
            LoadIsOpacity();
            LoadTextBoxData();
                        
            LoadDataBase();            
        }

        #region methods
        private void ProgressBarStart()
        {
            IsVisibility[ControlName.ProgressBar] = Visibility.Visible;
            OnPropertyChanged(nameof(IsVisibility));
            IsOpacity[ControlName.ScreenOpacity] = 0d;
            OnPropertyChanged(nameof(IsOpacity));
        }
        private void ProgressBarStop()
        {
            IsVisibility[ControlName.ProgressBar] = Visibility.Collapsed;
            OnPropertyChanged(nameof(IsVisibility));
            IsOpacity[ControlName.ScreenOpacity] = 1d;
            OnPropertyChanged(nameof(IsOpacity));
        }
        private void LoadDataBase()
        {
            db = new ArchiveContext();
            db.DataStores.Load();
            db.Streets.Load();
            db.Cities.Load();
            db.DocumentTypes.Load();
            db.Documents.Load();
            db.BookTypes.Load();
            db.Books.Load();

            LoadDataStore();
            //LoadStreet();
            LoadCity();
            LoadDocumentType();
            LoadDocument();
            LoadBookType();
            LoadBook();
        }
        private void LoadDataStore()
        {
            DataStores = db.DataStores.Local.ToBindingList().OrderBy(x=>x.Id);
        }
        private void LoadStreet()
        {
            Streets = db.Streets.Local.ToBindingList().Where(x=>x.CityId == CitySelect?.Id);
        }
        private void LoadCity()
        {
            Cities = db.Cities.Local.ToBindingList();
        }
        private void LoadDocumentType()
        {
            DocumentTypes = db.DocumentTypes.Local.ToBindingList();
        }
        private void LoadDocument()
        {
            Documents = db.Documents.Local.ToBindingList();
        }
        private void LoadBookType()
        {
            BookTypes = db.BookTypes.Local.ToBindingList();            
        }
        private void LoadBook()
        {
            Books = db.Books.Local.ToBindingList().OrderBy(x=>x.NumberBook);
        }
        
        private void LoadIsIsVisibility()
        {
            IsVisibility = new Dictionary<string, Visibility>
            {                
                { ControlName.ProgressBar, Visibility.Collapsed}
            };
        }
        private void LoadIsChecked()
        {
            IsChecked = new Dictionary<string, bool>
            {
                { ControlName.Report, true },
                { ControlName.Search, false },
                { ControlName.Added, false }
            };
        }
        private void LoadIsOpacity()
        {
            IsOpacity = new Dictionary<string, double>
            {
                { ControlName.ScreenOpacity, 1d}                
            };            
        }
        private void LoadTextBoxData()
        {
            TextBoxData = new Dictionary<string, string>
            {
                { ControlName.House, ""},
                { ControlName.Apartment, ""},
                { ControlName.CodeNew, ""},
                { ControlName.CodeOld, ""},
                { ControlName.NameBook, ""}
            };
        }
        
        private void ExitApplication()
        {
            db.Dispose();
            Application app = Application.Current;
            app.Shutdown();
        }

        #endregion
    }
}
