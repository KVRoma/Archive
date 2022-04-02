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
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainViewModel = new MainViewModel();
            displayRootRegistry.ShowPresentation(mainViewModel);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            displayRootRegistry.UnregisterWindowType<MainViewModel>();
            displayRootRegistry.UnregisterWindowType<InformationViewModel>();
            displayRootRegistry.UnregisterWindowType<DocumentViewModel>();

            base.OnExit(e);
        }
    }
}
