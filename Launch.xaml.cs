using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;

namespace DH_ZhouBao {
    enum LaunchState {
        Launched,
        UnLaunch,
        Jump,
    }

    /// <summary>
    /// Launch.xaml 的交互逻辑
    /// </summary>
    public partial class Launch : Window {
        public Launch() {
            InitializeComponent();
            var pathList = new List<string>();
            pathList.Add("/Resources/zw.jpg");
            pathList.Add("/Resources/ryan.jpg");
            pathList.Add("/Resources/zw.jpg");
            pathList.Add("/Resources/ryan.jpg");
            pathList.Add("/Resources/mj.jpg");
            pathList.Add("/Resources/zw.jpg");
            pathList.Add("/Resources/ryan.jpg");
            pathList.Add("/Resources/zw.jpg");
            pathList.Add("/Resources/ryan.jpg");
            pathList.Add("/Resources/zw.jpg");

            BitmapImage image = new BitmapImage(new Uri( pathList[new Random().Next(0,10)], UriKind.Relative));
            BG.Source = image;

            //var success = false;
            //var lannchState = LaunchState.UnLaunch;

            //new Thread(() => {
            //    try {
            //        var result = DbHelper.GetSingle("select 1");
            //        success = true;
            //        Project.Init();
            //        Identity.Init();
            //        if(lannchState == LaunchState.Jump) {
            //            this.Dispatcher.Invoke(() => {
            //                this.ShowMainWindow();
            //            });
            //        }
            //    } catch(Exception ex) {
            //        Common.ShowMessageBox(ex.Message);
            //        this.Dispatcher.Invoke(() => {
            //            this.Close();
            //        });
            //    }

            //}).Start();

            //new Thread(() => {
            //    Thread.Sleep(1500);
            //    if(success) {
            //        lannchState = LaunchState.Launched;
            //        this.Dispatcher.Invoke(() => {
            //            this.ShowMainWindow();
            //        });
            //    } else {
            //        lannchState = LaunchState.Jump;
            //    }
            //}).Start();
        }

        //public void ShowMainWindow() {
        //    var mainWindow = new MainWindow();
        //    mainWindow.Loaded += ( object sender, RoutedEventArgs e ) => {
        //        this.Close();
        //    };
        //    mainWindow.Show();
        //}
    }
}
