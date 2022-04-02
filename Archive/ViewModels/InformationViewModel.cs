using Archive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive.ViewModels
{
    public class InformationViewModel : ViewModel
    {
        private DataStore data;

        public DataStore Data
        {
            get { return data; }
            set 
            { 
                data = value;
                OnPropertyChanged(nameof(Data));
            }
        }
        public string Title => "Опис: " + Data.Code;

        private Command _esc;
        public Command Esc => _esc ?? (_esc = new Command(obj => 
        {
            if (obj is System.Windows.Window)
            {
                (obj as System.Windows.Window).Close();
            }        
        }));


    
    }
}
