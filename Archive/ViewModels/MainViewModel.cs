﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Archive.Models;

namespace Archive.ViewModels
{
    public class MainViewModel : ViewModel
    {
        #region private property
        ArchiveContext db;
                
        private Dictionary<string, Visibility> isVisibility;
        private Dictionary<string, bool> isChecked;
        private Dictionary<string, double> isOpacity;        
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
        public string TitleDown { get; } = "My project";
       
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
        #endregion

        public MainViewModel()
        {
            LoadDataBase();
        }

        #region methods
        private void ProgressBarStart()
        { 
        }
        private void ProgressBarStop()
        { 
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
            LoadStreet();
            LoadCity();
            LoadDocumentType();
            LoadDocument();
            LoadBookType();
            LoadBook();
        }
        private void LoadDataStore()
        {
            DataStores = db.DataStores.Local.ToBindingList();
        }
        private void LoadStreet()
        {
            Streets = db.Streets.Local.ToBindingList();
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
            Books = db.Books.Local.ToBindingList();
        }
        #endregion
    }
}
