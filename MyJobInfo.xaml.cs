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
using System.Data;


namespace DH_ZhouBao {
    /// <summary>
    /// MyJobInfo.xaml 的交互逻辑
    /// </summary>
    public partial class MyJobInfo : Window {
        public MyJobInfo() {
            InitializeComponent();
            if(Identity.IsRegister()) {
                RenderListView();
            } else {
                MessageBox.Show(Message.MSG1);
            }
        }

        private void RenderListView() {
            var userInfo = Identity.GetUserInfo();
            listView.DataContext = DataSource.GetJobJnfo(userInfo.ID);
        }

        private void Button_Click( object sender, RoutedEventArgs e ) {
            MessageBoxResult confirmToDel = MessageBox.Show("确认要删除所选记录吗？",Common.GetMessageTitle(), MessageBoxButton.YesNo, MessageBoxImage.Question);
            if( confirmToDel == MessageBoxResult.Yes ) {
                var btn = sender as Button;
                var id = Convert.ToInt32(btn.CommandParameter);
                DataSource.DeleteJobInfo(id);
                this.RenderListView();
            }
        }

        private void Button_Click_1( object sender, RoutedEventArgs e ) {
            var btn = sender as Button;
            var id = Convert.ToInt32(btn.CommandParameter);
            var main = new MainWindow(id);
            main.Owner = this;
            main.Show();
        }

        private void listView_SelectionChanged( object sender, SelectionChangedEventArgs e ) {
            var data = listView.SelectedItem as DataRowView;
            if(data == null) {
                return;
            }
            var id = Convert.ToInt32(data ["id"]);
            var jobInfoItemsView = new JobInfoItemsView(id);
            jobInfoItemsView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            jobInfoItemsView.Owner = this;
            jobInfoItemsView.Show();
        }
    }
}
