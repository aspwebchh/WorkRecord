using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DH_ZhouBao {
    public class Common {

        public static string GetMessageTitle() {
            var userInfo = Identity.GetUserInfoFromCache();
            return "我最亲爱的" + userInfo.Name + "！";
        }

        public static void ShowMessageBox( string msg ) {
            MessageBox.Show(msg, GetMessageTitle());
        }
    }

    public class ResultInfo {
        public bool Success { get; set; }
        public string Message { get; set; }

        public static ResultInfo Create(bool success, string message) {
            var resultInfo = new ResultInfo();
            resultInfo.Success = success;
            resultInfo.Message = message;
            return resultInfo;
        }
    }
}
