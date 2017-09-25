using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace DH_ZhouBao {
    class Identity {
        private static UserInfo userInfoCache = new DefaultUserInfo();

        public static void Init() {
            new Thread(() => {
                UpdateCache();
            }).Start();
        }

        private static void UpdateCache() {
            userInfoCache = GetUserInfo();
        }

        public static UserInfo GetUserInfoFromCache() {
            return userInfoCache;
        }

        private static string GetMac() {
            string mac = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach( ManagementObject mo in moc ) {
                if( ( bool ) mo ["IPEnabled"] == true ) {
                    mac = mo ["MacAddress"].ToString();
                    break;
                }
            }
            moc = null;
            mc = null;
            return mac;
        }

        public static bool IsRegister() {
            var userInfo = GetUserInfo();
            return !(userInfo is DefaultUserInfo);
        }

        public static UserInfo GetUserInfo() {
            var sql = "select * from identity_info where mac = '" + GetMac() + "'";
            var result = DbHelper.Query(sql);
            if(result.Tables[0].Rows.Count == 0) {
                return new DefaultUserInfo();
            } else {
                var row = result.Tables [0].Rows [0];
                return new UserInfo {
                    Name = row["name"].ToString(),
                    Mac = GetMac(),
                    ID = Convert.ToInt32( row ["id"] ),
                };
            }

        }

        public static void UpdateName( string name ) {
            var mac = GetMac();
            var sql = "select count(1) from identity_info where mac = '" + mac + "'";
            var result = Convert.ToInt32(DbHelper.GetSingle(sql));
            if(result > 0 ) {
                sql = "update identity_info set name = @name where mac = '" + mac + "'";
                var paramList = new SqlParameter [] {
                    new SqlParameter("@name",SqlDbType.NVarChar)
                };
                paramList [0].Value = name;
                DbHelper.ExecuteSql(sql, paramList);
            } else {
                sql = "insert into identity_info(name, mac) values (@name, '" + mac + "')";
                var paramList = new SqlParameter [] {
                    new SqlParameter("@name",SqlDbType.NVarChar)
                };
                paramList [0].Value = name;
                DbHelper.ExecuteSql(sql, paramList);
            }
            UpdateCache();
        }
    }
}
