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


namespace DH_ZhouBao {
    /// <summary>
    /// SetNameDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SetNameDialog : Window {
        public SetNameDialog() {
            InitializeComponent();

            var isRegister = Identity.IsRegister();
            if(isRegister) {
                var userInfo = Identity.GetUserInfo();
                this.NameTextBox.Text = userInfo.Name;
            }

            this.NameTextBox.Focus();
            this.NameTextBox.SelectAll();
        }

        private void Button_Click( object sender, RoutedEventArgs e ) {
            var name = this.NameTextBox.Text;
            if(string.IsNullOrEmpty(name)) {
                Common.ShowMessageBox("请输入您的大名");
                return;
            }
            Identity.UpdateName(name);
            var owner = this.Owner as MainWindow;
            owner.FillData();
            this.Close();
        }
    }
}
