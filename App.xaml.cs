using PGViewer.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PGViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, StartupEventArgs args) 
        {
            var loginView = new LoginView();
            loginView.Show();
            
            loginView.IsVisibleChanged += (s, ev) =>
            {

                Console.WriteLine($"Visible changed");
                if (loginView.IsVisible == false && loginView.IsLoaded)
                {
                    Console.WriteLine($"Main show");
                    var mainView = new MainView();
                    mainView.Show();
                    loginView.Close();
                }
            };
        } 
    }
}
