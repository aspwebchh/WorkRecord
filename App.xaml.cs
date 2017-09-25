using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;

namespace DH_ZhouBao {
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application {
        Window launch;
        public App() {
            
        }

        public void ShowMainWindow() {
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }

        public void ShowWaiting() {
            launch = new Launch();
            launch.Show();
        }

        public void HideWaiting() {
            launch.Close();
        }
 
        void App_Startup( object sender, StartupEventArgs e ) {
            try {
                ShowWaiting();
                var result = DbHelper.GetSingle("select 1");
                Project.Init();
                Identity.Init();
                ShowMainWindow();
                HideWaiting();
            } catch( Exception ex ) {
                Common.ShowMessageBox(ex.Message);
                HideWaiting();
            }

        }
    }
}
