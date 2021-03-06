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
        private bool isDelBook;
        private Dictionary<string, string> report;
        #endregion

        #region public property
        public string TitleView { get; } = "Архів - 2022";
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
                    Report["Error"] = "";
                    OnPropertyChanged(nameof(Report));
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
                LoadDocument();
            }
        }
        public bool IsDelBook
        {
            get => isDelBook;
            set
            {
                isDelBook = value;
                OnPropertyChanged(nameof(IsDelBook));
            }
        }
        public Dictionary<string, string> Report
        {
            get => report;
            set
            {
                report = value;
                OnPropertyChanged(nameof(Report));
            }
        }
        #endregion

        #region command
        private Command _exitApp;
        private Command _checked;
        private Command _search;
        private Command _clearAdded;
        private Command _searchAdded;
        private Command _infoDataStore;
        private Command _register;
        private Command _addDocument;
        private Command _insDocument;
        private Command _delDocument;
        private Command _delBook;
        private Command _delBookResolution;
        private Command _cityDic;        
        private Command _bookDic;
        private Command _documentDic;
        private Command _filter1;
        private Command _filter2;
        private Command _filter3;
        private Command _filter4;

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
                LoadReport();
            });
            ProgressBarStop();                        
        }));
        public Command Search => _search ?? (_search = new Command(obj =>
        {
            string item = obj.ToString() + CitySelect?.Id.ToString() + StreetSelect?.Id.ToString();
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
            });
            ProgressBarStop();            
        }));
        public Command SearchAdded => _searchAdded ?? (_searchAdded = new Command(obj =>
        {
            string str = CitySelect?.Name + StreetSelect?.Name + TextBoxData[ControlName.House] + TextBoxData[ControlName.Apartment];
            DataStores = DataStores.Where(x=>x.Search.ToUpper().Contains(str.ToUpper()));
        }));
        public Command InfoDataStore => _infoDataStore ?? (_infoDataStore = new Command( async obj =>
        {
            if (DataStoreSelect != null)
            {
                var displayRootRegistry = (Application.Current as App).displayRootRegistry;
                InformationViewModel info = new InformationViewModel();
                info.Data = DataStoreSelect;
                await displayRootRegistry.ShowModalPresentation(info);
            }
        }));
        public Command Register => _register ?? (_register = new Command(async obj =>
        {
            if (!string.IsNullOrWhiteSpace(BookTypeSelect?.Name) &&
                !string.IsNullOrWhiteSpace(CitySelect?.Name) &&
                !string.IsNullOrWhiteSpace(StreetSelect?.Name) &&
                !string.IsNullOrWhiteSpace(TextBoxData[ControlName.CodeNew]) &&
                !string.IsNullOrWhiteSpace(TextBoxData[ControlName.NameBook]))
            {
                ProgressBarStart();
                await Task.Run(() =>
                {
                    //*** Визначаю порядковий номер
                    var temp = db.Books.Where(x => x.BookTypeId == BookTypeSelect.Id);
                    int num = (temp?.Count() != 0) ? (temp.Max(x=>x.Number) + 1) : 1;
                    
                    //*** Перевірка дублікату
                    string code = TextBoxData[ControlName.CodeNew];
                    var filter = temp.Where(x => x.CodeNew == code);

                    if (filter?.Count() == 0)
                    {
                        Book item = new Book()
                        {
                            Number = num,
                            NumberBook = num.ToString() + BookTypeSelect.Key,
                            CodeNew = TextBoxData[ControlName.CodeNew],
                            CodeOld = TextBoxData[ControlName.CodeOld],
                            Name = TextBoxData[ControlName.NameBook],
                            City = CitySelect,
                            Street = StreetSelect,
                            House = TextBoxData[ControlName.House],
                            Apartment = TextBoxData[ControlName.Apartment],
                            BookType = BookTypeSelect
                        };
                        db.Books.Add(item);
                        db.SaveChanges();
                        ClearAdded.Execute("");
                    }
                    else 
                    {
                        Report["Error"] = "Справа вже зареєстрована за номером - " + filter.FirstOrDefault().NumberBook + " !!!";
                        OnPropertyChanged(nameof(Report));
                    }
                });
                ProgressBarStop();
    }            
        }));
        public Command AddDocument => _addDocument ?? (_addDocument = new Command(async obj => 
        {
            if (BookSelect != null)
            {
                var displayRootRegistry = (Application.Current as App).displayRootRegistry;
                DocumentViewModel document = new DocumentViewModel();
                document.DocTypes = DocumentTypes;
                document.Name = "Створення";
                document.Doc = new Document();                                               
                await displayRootRegistry.ShowModalPresentation(document);
                
                if (document.IsSuccess)
                {
                    document.Doc.Book = BookSelect;
                    db.Documents.Add(document.Doc);                    
                    db.SaveChanges();
                    LoadDocument();
                }                
            }
        }));
        public Command InsDocument => _insDocument ?? (_insDocument = new Command(async obj => 
        {
            if (DocumentSelect != null)
            {
                var displayRootRegistry = (Application.Current as App).displayRootRegistry;
                DocumentViewModel document = new DocumentViewModel();
                document.DocTypes = DocumentTypes;
                document.DocTypeSelect = DocumentTypes.FirstOrDefault(x => x.Id == DocumentSelect.DocumentTypeId);
                document.Name = "Редагування";
                document.Doc = DocumentSelect;                
                await displayRootRegistry.ShowModalPresentation(document);
                if (document.IsSuccess)
                {
                    db.Entry(document.Doc).State = EntityState.Modified;                    
                    db.SaveChanges();
                    LoadDocument();
                }
            }
        }));
        public Command DelDocument => _delDocument ?? (_delDocument = new Command(obj => 
        {
            if (DocumentSelect != null)
            {
                db.Documents.Remove(DocumentSelect);
                db.SaveChanges();
                LoadDocument();
                IsDelBook = false;
            }
        }));
        public Command DelBook => _delBook ?? (_delBook = new Command(obj => 
        {
            if (BookSelect != null)
            {

                db.Books.Remove(BookSelect);
                db.SaveChanges();
                LoadBook();
                IsDelBook = false;
            }
        }));
        public Command DelBookResolution => _delBookResolution ?? (_delBookResolution = new Command(obj => 
        {
            if (IsDelBook)
            {
                IsDelBook = false;
            }
            else
            {
                IsDelBook = true;
            }
        }));
        public Command CityDic => _cityDic ?? (_cityDic = new Command(async obj => 
        {
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            DictyionaryViewModel dic = new DictyionaryViewModel(ref db, ControlName.CityDictionary);
            await displayRootRegistry.ShowModalPresentation(dic);
        }));        
        public Command BookDic => _bookDic ?? (_bookDic = new Command(async obj => 
        {
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            DictyionaryViewModel dic = new DictyionaryViewModel(ref db, ControlName.BookDictionary);
            await displayRootRegistry.ShowModalPresentation(dic);
        }));
        public Command DocumentDic => _documentDic ?? (_documentDic = new Command(async obj => 
        {
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            DictyionaryViewModel dic = new DictyionaryViewModel(ref db, ControlName.DocumentDictionary);
            await displayRootRegistry.ShowModalPresentation(dic);
        }));
        public Command Filter1 => _filter1 ?? (_filter1 = new Command(obj => 
        {
            if (!IsChecked[ControlName.Search])
            {
                IsChecked[ControlName.Search] = true;
                OnPropertyChanged(nameof(IsChecked));
            }

            Books = Books.Where(x => x.Documents.Count == 0);
        }));
        public Command Filter2 => _filter2 ?? (_filter2 = new Command(obj => 
        {
            if (!IsChecked[ControlName.Search])
            {
                IsChecked[ControlName.Search] = true;
                OnPropertyChanged(nameof(IsChecked));
            }

            List<Book> doubleDoc = new List<Book>();
            foreach (var book in Books)
            {
                var temp =book.Documents.Where(x=>x.DocumentTypeId >= 1 && x.DocumentTypeId <= 4).Select(x=>x.DocumentTypeId).Distinct();
                if (temp.Count() != 4)
                {
                    doubleDoc.Add(book);
                }
            }
            Books = doubleDoc;
        }));
        public Command Filter3 => _filter3 ?? (_filter3 = new Command(obj => 
        {
            if (!IsChecked[ControlName.Search])
            {
                IsChecked[ControlName.Search] = true;
                OnPropertyChanged(nameof(IsChecked));
            }

            Books = db.Documents.Where(x => x.DateDocument == x.DateCreated).Select(x=>x.Book).ToList();
        }));
        public Command Filter4 => _filter4 ?? (_filter4 = new Command(obj => 
        {
            if (!IsChecked[ControlName.Search])
            {
                IsChecked[ControlName.Search] = true;
                OnPropertyChanged(nameof(IsChecked));
            }

            List<Book> doubleDoc = new List<Book>();
            foreach (Book book in Books)
            {
                var temp = book.Documents.Select(x => x.DocumentTypeId);
                if (temp.Count() != temp.Distinct().Count())
                {
                    doubleDoc.Add(book);
                }
            }
            Books = doubleDoc;              
        }));
        #endregion

        public MainViewModel()
        {
            LoadIsIsVisibility();
            LoadIsChecked();
            LoadIsOpacity();
            LoadTextBoxData();
            IsDelBook = false;
                        
            LoadDataBase();

            LoadReport();
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
            LoadCity();
            LoadDocumentType();            
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
            Documents = db.Documents.Local.ToBindingList().Where(x=>x.BookId == BookSelect?.Id)?.OrderBy(x=>x.DocumentTypeId);
        }
        private void LoadBookType()
        {
            BookTypes = db.BookTypes.Local.ToBindingList();            
        }
        private void LoadBook()
        {
            Books = db.Books.Local.ToBindingList().OrderBy(x=>x.BookTypeId).ThenBy(x=>x.Id);
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
        private void LoadReport()
        {
            Report = new Dictionary<string, string>
            {
                { "ReportLeft", ""},
                { "ReportRight", ""},
                { "ReportFooter", ""},
                { "Error","" }
            };
            //**************************************************************************************************************************************            
            var temp = db.BookTypes.Select(x => x.Name).ToList();
            foreach (var item in temp)
            {
                Report["ReportLeft"] += item + " " + db.Books.Where(x => x.BookType.Name == item).Count().ToString() + " шт." + Environment.NewLine;
            }
            //***************************************************************************************************************************************            
            temp = db.DocumentTypes.Select(x => x.Name).ToList();
            foreach (var item in temp)
            {
                Report["ReportRight"] += item + " " + db.Documents.Where(x => x.DocumentType.Name == item).Count().ToString() + " шт." + Environment.NewLine;
            }
            //***************************************************************************************************************************************
            Report["ReportFooter"] = "Опрацьовано за поточний місяць: Справ: " + 
                                      db.Books.Where(x=>x.DateCreated.Month == DateTime.Today.Month).Count().ToString() + " шт. / Документів: " + 
                                      db.Documents.Where(x => x.DateCreated.Month == DateTime.Today.Month).Count().ToString() + " шт." + Environment.NewLine +
                                      "Опрацьовано за минулу добу: Справ: " +
                                      db.Books.Where(x => x.DateCreated.Day == (DateTime.Today.Day - 1)).Count().ToString() + " шт. / Документів: " +
                                      db.Documents.Where(x => x.DateCreated.Day == (DateTime.Today.Day - 1)).Count().ToString() + " шт." + Environment.NewLine +
                                      "Опрацьовано за поточну добу: Справ: " +
                                      db.Books.Where(x => x.DateCreated == DateTime.Today).Count().ToString() + " шт. / Документів: " +
                                      db.Documents.Where(x => x.DateCreated == DateTime.Today).Count().ToString() + " шт." + Environment.NewLine;

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
