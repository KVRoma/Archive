using Archive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive.ViewModels
{
    public class DocumentViewModel : ViewModel
    {
        private string name;
        private bool isSuccess;
        private Document doc;
        private IEnumerable<DocumentType> docTypes;
        private DocumentType docTypeSelect;       
        
       
        public string Title => "Робота з документом...";
        public bool IsSuccess
        {
            get => isSuccess; 
            set
            {
                isSuccess = value;
                OnPropertyChanged(nameof(IsSuccess));
            }
        }
        public Document Doc
        {
            get => doc; 
            set
            {
                doc = value;
                OnPropertyChanged(nameof(Doc));
            }
        }
        public IEnumerable<DocumentType> DocTypes
        {
            get => docTypes; 
            set
            {
                docTypes = value;
                OnPropertyChanged(nameof(DocTypes));
            }
        }
        public DocumentType DocTypeSelect
        {
            get => docTypeSelect; 
            set
            {
                docTypeSelect = value;
                OnPropertyChanged(nameof(DocTypeSelect));
            }
        }
        public string Name
        {
            get => name;
            set 
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }


        private Command _okCommand;
        private Command _cancelCommand;
        public Command OkCommand => _okCommand ?? (_okCommand = new Command(obj => 
        {
            if (obj is System.Windows.Window)
            {
                IsSuccess = true;
                Doc.DocumentType = DocTypeSelect;
                (obj as System.Windows.Window).Close();
            }
        }));
        public Command CancelCommand => _cancelCommand ?? (_cancelCommand = new Command(obj => 
        {
            if (obj is System.Windows.Window)
            {
                IsSuccess = false;
                (obj as System.Windows.Window).Close();
            }
        }));
    }
}
