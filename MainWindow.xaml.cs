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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Threading;

namespace DH_ZhouBao {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        private DataSource dataSource;
        private bool isEditPage = false;
        private int jobId;

        public MainWindow() {
            dataSource = new DataSource();
            dataSource.Reset();
            InitializeComponent();
            RenderListView();
            this.Loaded += MainWindow_Loaded;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public MainWindow( int dataId ) {
            this.jobId = dataId;
            dataSource = DataSource.load(dataId);
            this.isEditPage = true;
            InitializeComponent();
            this.RenderListViewForEdit();
            this.WindowState = WindowState.Normal;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Loaded += MainWindow_Loaded;
            MyJobBtn.Visibility = Visibility.Collapsed;
            AllJobBtn.Visibility = Visibility.Collapsed;
            UpdateNameBtn.Visibility = Visibility.Collapsed;

        }


        private void RenderListViewForEdit() {
            listView.DataContext = dataSource.ToDataTable(ItemType.Current);
            listView2.DataContext = dataSource.ToDataTable(ItemType.Next);
        }


        private void RenderListView() {
            listView.DataContext = dataSource.ToDataTable(ItemType.Current);
            listView2.DataContext = dataSource.ToDataTable(ItemType.Next);
        }

        private void MainWindow_Loaded( object sender, RoutedEventArgs e ) {
            FillData();
            CheckRegister();
        }

        public void FillData() {
            var thread = new Thread(() => {
                var text = "";
                var userInfo = Identity.GetUserInfo();
                text = "你好，" + Common.GetMessageTitle();
                this.Dispatcher.Invoke(new Action(() => {
                    IdentityInfo.Text = text;
                }));
            });
            thread.Start();
        }

        private void CheckRegister() {
            var thread = new Thread(() => {
                if( Identity.IsRegister() ) {
                    return;
                }
                this.Dispatcher.Invoke(new Action(() => {
                    OpenSetNameDialog();
                }));
            });
            thread.Start();

        }

        private void OpenSetNameDialog() {
            SetNameDialog dialog = WindowManager.Create(typeof(SetNameDialog)) as SetNameDialog;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.ResizeMode = ResizeMode.NoResize;
            dialog.Owner = this;
            dialog.Show();
        }

        private void Button_Click( object sender, RoutedEventArgs e ) {
            var dataItem = new DataItem();
            dataSource.Add(dataItem);
            RenderListView();
        }

        private void Button_Click_1( object sender, RoutedEventArgs e ) {
            var button = sender as Button;
            var index = Convert.ToInt32(button.CommandParameter);
            dataSource.Remove(index);
            RenderListView();
        }

        private void ComboBox_DropDownClosed( object sender, EventArgs e ) {
            var target = sender as ComboBox;
            var selectedItem = target.SelectedItem as ComboBoxItem;
            var type = selectedItem.Content.ToString();
            var dataItem = GetDataItem(target);
            if( dataItem != null ) {
                dataItem.Type = type;
            }
        }

        private void ComboBox_DropDownOpened( object sender, EventArgs e ) {
            var target = sender as ComboBox;
            if( !ProjectIsUnSelected(target) ) {
                return;
            }
            if( target.Items.Count > 3 ) {
                while( target.Items.Count > 3 ) {
                    target.Items.RemoveAt(target.Items.Count - 1);
                }
            }
            var myProject = Project.GetMyProject();
            foreach( var project in myProject ) {
                var item = new ComboBoxItem();
                item.Content = project;
                target.Items.Add(item);
            }
        }

        private void TextBox_TextChanged( object sender, TextChangedEventArgs e ) {
            var target = sender as TextBox;
            var comboBoxItem = target.Parent as ComboBoxItem;
            var comboBox = comboBoxItem.Parent as ComboBox;
            while( comboBox.Items.Count > 2 ) {
                comboBox.Items.RemoveAt(comboBox.Items.Count - 1);
            }
            var text = target.Text.Trim();
            var query = Project.Query(text);
            var result = new List<string>();
            if( !string.IsNullOrEmpty(text) ) {
                result.Add(text);
            }
            result.AddRange(query);
            foreach( var item in result ) {
                var newItem = new ComboBoxItem();
                newItem.Content = item;
                comboBox.Items.Add(newItem);
            }
        }

        private bool ProjectIsUnSelected( ComboBox target ) {
            var selectItem = target.SelectedItem as ComboBoxItem;
            if( selectItem == null || selectItem.Content is TextBox || selectItem.Content.ToString() == "请选择项目" || selectItem.Content.ToString() == "......" || string.IsNullOrEmpty(selectItem.Content.ToString()) ) {
                return true;
            } else {
                return false;
            }
        }

        private void ComboBox_DropDownClosed_1( object sender, EventArgs e ) {
            var target = sender as ComboBox;
            var selectItem = target.SelectedItem as ComboBoxItem;
            var dataItem = GetDataItem(target);
            if( ProjectIsUnSelected(target) ) {
                target.SelectedIndex = 0;
                if( dataItem != null ) {
                    dataItem.Project = string.Empty;
                }
            } else {
                if( dataItem != null ) {
                    var project = selectItem.Content.ToString();
                    dataItem.Project = project;
                    Project.AddTempItemToMyProject(project);
                }
            }
        }

        private void ComboBox_DropDownClosed_2( object sender, EventArgs e ) {
            var target = sender as ComboBox;
            var selectedItem = target.SelectedItem as ComboBoxItem;
            var val = selectedItem.Content.ToString();
            var dataItem = GetDataItem(target);
            if( dataItem != null ) {
                dataItem.Process = val;
            }
        }

        private void TextBox_TextChanged_1( object sender, TextChangedEventArgs e ) {
            var target = sender as TextBox;
            var text = target.Text;
            var dataItem = GetDataItem(target);
            dataItem.Content = text;
        }

        private void Button_Click_2( object sender, RoutedEventArgs e ) {
            var dataItem = GetDataItem(sender as Button);
            if( dataItem.Index > 1 ) {
                var prev = dataSource.Get(dataItem.Index - 1);
                dataItem.Index--;
                prev.Index++;
                RenderListView();
            }
        }

        private void Button_Click_3( object sender, RoutedEventArgs e ) {
            var dataItem = GetDataItem(sender as Button);
            if( dataItem.Index < dataSource.Size() ) {
                var next = dataSource.Get(dataItem.Index + 1);
                dataItem.Index++;
                next.Index--;
                RenderListView();
            }
        }

        private DataItem GetDataItem( Control control ) {
            var data = control.DataContext as DataRowView;
            var index = Convert.ToInt32(data ["index"]);
            var dataItem = dataSource.Get(index);
            return dataItem;
        }

        private void Button_Click_4( object sender, RoutedEventArgs e ) {

            var isValid = dataSource.IsValid();
            if( !isValid.Success ) {
                Common.ShowMessageBox(isValid.Message);
                return;
            }

            if( this.isEditPage ) {
                var result = dataSource.Update(this.jobId);
                Common.ShowMessageBox(result.Message);
                if( !result.Success ) {
                    return;
                }
                this.Close();
            } else {
                var result = dataSource.Save();
                Common.ShowMessageBox(result.Message);
                if( !result.Success ) {
                    return;
                }
                dataSource.Reset();
                RenderListView();
            }
        }

        private void Button_Click_5( object sender, RoutedEventArgs e ) {
            OpenSetNameDialog();
        }

        private void Button_Click_6( object sender, RoutedEventArgs e ) {
            var myJobInfoDialog = WindowManager.Create(typeof(MyJobInfo));
            myJobInfoDialog.Owner = this;
            myJobInfoDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            myJobInfoDialog.Show();
        }

        private void Button_Click_7( object sender, RoutedEventArgs e ) {
            var allJob = new AllJob();
            allJob.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            allJob.Owner = this;
            allJob.Show();
        }

        private void Window_Closed( object sender, EventArgs e ) {
            if( this.Owner != null && this.Owner.Owner != null ) {
                this.Owner.Owner.Focus();
            }
        }

        private void Button_Click_8( object sender, RoutedEventArgs e ) {
            var dataItem = new DataItem();
            dataItem.Type = Text.NEXT_STAGE;
            dataSource.Add(dataItem);
            RenderListView();
        }
    }
}
