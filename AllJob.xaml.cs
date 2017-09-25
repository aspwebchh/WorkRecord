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
    /// AllJob.xaml 的交互逻辑
    /// </summary>
    public partial class AllJob : Window {
        private List<int> jobIdList = new List<int>();

        public AllJob() {
            InitializeComponent();
            this.RenderListView();
        }

        private void RenderListView() {
            var data = DataSource.GetAllJobInfo();
            listView.DataContext = data;
        }

        private void listView_SelectionChanged( object sender, SelectionChangedEventArgs e ) {
            var data = listView.SelectedItem as DataRowView;
            var id = Convert.ToInt32(data ["id"]);
            var jobInfoItemsView = new JobInfoItemsView(id);
            jobInfoItemsView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            jobInfoItemsView.Owner = this;
            jobInfoItemsView.Show();
        }

        private void Button_Click( object sender, RoutedEventArgs e ) {
            if( jobIdList.Count == 0 ) {
                Common.ShowMessageBox("请选择要导出的记录");
                return;
            }
            var resultWindow = new Result(jobIdList);
            resultWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            resultWindow.Owner = this;
            resultWindow.Show();
        }

        private void CheckBox_Checked( object sender, RoutedEventArgs e ) {
            var checkbox = sender as CheckBox;
            var id = Convert.ToInt32(checkbox.CommandParameter);
            jobIdList.Add(id);
        }

        private void CheckBox_Unchecked( object sender, RoutedEventArgs e ) {
            var checkbox = sender as CheckBox;
            var id = Convert.ToInt32(checkbox.CommandParameter);
            jobIdList.Remove(id);
        }

        private void Button_Click_1( object sender, RoutedEventArgs e ) {
            var name = Name.Text.Trim();
            if( string.IsNullOrEmpty(name) ) {
                Common.ShowMessageBox("请输入姓名");
                return;
            }
            try {
                listView.DataContext = DataSource.GetJobInfoByName(name);
            } catch( Exception ex ) {
                Common.ShowMessageBox(ex.Message);
            }
        }
    }
}
