using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace DH_ZhouBao {
     static class Project {
        private static List<string> myProjects;

        public static void SetDefaultMyProject() {
            myProjects = new List<string>();
            if(!Identity.IsRegister()) {
                return;
            }
            var userInfo = Identity.GetUserInfo();
            var sql = "select project from dbo.f_project(" + userInfo.ID + ")";
            var result = DbHelper.Query(sql);
            foreach(DataRow dataRow in result.Tables[0].Rows) {
                myProjects.Add(dataRow ["project"].ToString());
            }
        }

        public static List<String> GetMyProject() {
            return myProjects;
        }

        public static void AddTempItemToMyProject( string item ) {
            if(!myProjects.Contains(item)) {
                myProjects.Add(item);
            }
        }

        public static List<String> Query( string keyword ) {
            if(string.IsNullOrEmpty(keyword)) {
                return GetMyProject();
            }
            var sql = "select project from v_project where CHARINDEX(@project,project)>0";
            var paramList = new SqlParameter [] {
                new SqlParameter("@project",SqlDbType.NVarChar)
            };
            paramList [0].Value = keyword;
            var ds = DbHelper.Query(sql, paramList);
            var result = new List<string>();
            foreach(DataRow dataRow in ds.Tables[0].Rows) {
                result.Add(dataRow ["project"].ToString());
            }
            return result;
        }

        public static void Init() {
            var thread = new Thread(() => {
                SetDefaultMyProject();
            });
            thread.Start();
        }
    }
}
