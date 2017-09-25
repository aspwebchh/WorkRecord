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
using System.Windows.Forms;
using System.IO;

namespace DH_ZhouBao {
    /// <summary>
    /// Result.xaml 的交互逻辑
    /// </summary>
    public partial class Result : Window {
        public Result(List<int> jobIDs) {
            InitializeComponent();
            this.ShowResult(jobIDs);
        }


        private void ShowResult( List<int> jobIDs ) {
            var curr = DataSource.GetResult(jobIDs, Text.CURRENT_STAGE);
            var next = DataSource.GetResult(jobIDs, Text.NEXT_STAGE);
            var currStr = TableToString(curr);
            var nextStr = TableToString(next);
            Base.Text = Text.CURRENT_STAGE + Environment.NewLine + currStr + Environment.NewLine + Text.NEXT_STAGE + Environment.NewLine + nextStr;
        }

        private string TableToString(DataTable dt) {
            var result = string.Empty;
            foreach(DataRow row in dt.Rows) {
                result += row ["item_project"].ToString() + "：" + row ["item_content"];
                result += Environment.NewLine;
            }
            return result;
        }

        private string GetResultString() {
            var resultString = string.Empty;
            resultString += "重点工作";
            resultString += Environment.NewLine;
            resultString += Focus.Text;
            resultString += Environment.NewLine;
            resultString += Environment.NewLine;
            resultString += "基础工作";
            resultString += Environment.NewLine;
            resultString += Base.Text;
            resultString += Environment.NewLine;
            resultString += "KPI";
            resultString += Environment.NewLine;
            resultString += KPI.Text;
            return resultString;
        }

        private void Button_Click( object sender, RoutedEventArgs e ) {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "文本文档|*.txt";
           if( sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                using( var fs = ( FileStream ) sfd.OpenFile() ) {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(GetResultString());
                    sw.Flush();
                }
            }
        }
        private void Window_Closed( object sender, EventArgs e ) {
            if( this.Owner != null && this.Owner.Owner != null ) {
                this.Owner.Owner.Focus();
            }
        }

    }
}
