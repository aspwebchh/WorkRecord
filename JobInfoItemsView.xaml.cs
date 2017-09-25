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
    /// JobInfoItemsView.xaml 的交互逻辑
    /// </summary>
    public partial class JobInfoItemsView : Window {
        public JobInfoItemsView() {
            InitializeComponent();
        }


        private string GetItemProcess( string process ) {
            return process == Text.NO_SELECT ? "" : process;
        }

        public JobInfoItemsView(int jobId ) {
            InitializeComponent();
            var jobInfoItems = DataSource.GetJobInfoItems(jobId);
            var view = jobInfoItems.DefaultView;
            view.RowFilter = "item_type='"+Text.CURRENT_STAGE+"'";
            var currJobInfo = string.Empty;
            foreach(DataRow item in view.ToTable().Rows) {
                currJobInfo += Environment.NewLine;
                currJobInfo += item ["item_project"].ToString() + "：" + item ["item_content"] + " " + this.GetItemProcess(item ["item_process"].ToString());
            }
            if( string.IsNullOrEmpty(currJobInfo)) {
                currJobInfo += Environment.NewLine;
                currJobInfo += "无内容";
            }
            view.RowFilter = "item_type='"+ Text.NEXT_STAGE +"'";
            var nextJobInfo = string.Empty;
            foreach( DataRow item in view.ToTable().Rows ) {
                nextJobInfo += Environment.NewLine;
                nextJobInfo += item ["item_project"].ToString() + "：" + item ["item_content"] + this.GetItemProcess(item ["item_process"].ToString());
            }
            if( string.IsNullOrEmpty(nextJobInfo) ) {
                nextJobInfo += Environment.NewLine;
                nextJobInfo += "无内容";
            }
            var content = Text.CURRENT_STAGE;
            content += currJobInfo + Environment.NewLine;
            content += Environment.NewLine;
            content +=  Text.NEXT_STAGE;
            content += nextJobInfo + Environment.NewLine;
            ContentView.Text = content;
        }

        private void Window_Closed( object sender, EventArgs e ) {
            if(this.Owner != null && this.Owner.Owner != null) {
                this.Owner.Owner.Focus();
            }
        }
    }
}
