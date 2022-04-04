using Archive.Resources.Validator;
using Archive.ViewModels;
using Archive.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace Archive
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public DisplayRootRegistry displayRootRegistry = new DisplayRootRegistry();        

        public App()
        {
            displayRootRegistry.RegisterWindowType<MainViewModel, MainView>();
            displayRootRegistry.RegisterWindowType<InformationViewModel, InformationView>();
            displayRootRegistry.RegisterWindowType<DocumentViewModel, DocumentView>();
            displayRootRegistry.RegisterWindowType<DictyionaryViewModel, DictyionaryView>();
            displayRootRegistry.RegisterWindowType<MessageViewModel, MessageView>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
                    
            Validation valid = new Validation();
            valid.Key = ConfigurationManager.AppSettings["key"].ToString();
            valid.Path = Environment.CurrentDirectory + @"\Keys\License.key";

            if (valid.IsValid())
            {
                var mainViewModel = new MainViewModel();
                displayRootRegistry.ShowPresentation(mainViewModel);
            }
            else
            { 
                MessageViewModel message = new MessageViewModel(valid.Message);
                displayRootRegistry.ShowPresentation(message);
            }
            
        }

        protected override void OnExit(ExitEventArgs e)
        {
            displayRootRegistry.UnregisterWindowType<MainViewModel>();
            displayRootRegistry.UnregisterWindowType<InformationViewModel>();
            displayRootRegistry.UnregisterWindowType<DocumentViewModel>();
            displayRootRegistry.UnregisterWindowType<DictyionaryViewModel>();
            displayRootRegistry.UnregisterWindowType<MessageViewModel>();

            base.OnExit(e);
        }
    }
}
