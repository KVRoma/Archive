using Archive.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foreground = System.Windows.Media.Brush;

namespace Archive.Resources.Validator
{
    public class MessageViewModel : ViewModel
    {
        public string Title { get; } = "License....";
        private bool pressOk;
        private string text;
        

        public bool PressOk
        {
            get { return pressOk; }
            set
            {
                pressOk = value;
                OnPropertyChanged(nameof(PressOk));
            }
        }
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
        

        private Command okCommand;        

        public Command OkCommand => okCommand ?? (okCommand = new Command(obj =>
        {

            if (obj is System.Windows.Window)
            {
                PressOk = true;
                (obj as System.Windows.Window).Close();
            }

        }));
        

        public MessageViewModel(string message)
        {           
            Text = message;
            PressOk = false;            
        }
    }
}
