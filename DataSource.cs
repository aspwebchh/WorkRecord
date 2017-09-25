using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace DH_ZhouBao {
    public enum ItemType {
        Current,
        Next
    }

    public class DataSource {
        private List<DataItem> source = new List<DataItem>();

        public DataSource() {
        
        }

        private int GetNewIndex() {
            if (source.Count() == 0 ) {
                return 1;
            }
            return source.Max(item => {
                return item.Index;
            }) + 1;
        }


        public void Reset() {
            source.Clear();
            var dataItem = new DataItem();
            Add(dataItem);
            var dataItem1 = new DataItem();
            dataItem1.Type = Text.NEXT_STAGE;
            Add(dataItem1);
        }

        public int Size() {
            return source.Count();
        }

        public void Add( DataItem dataItem ) {
            if( string.IsNullOrEmpty(dataItem.Process)) {
                dataItem.Process = Text.NO_SELECT;
            }
            if(string.IsNullOrEmpty(dataItem.Type)) {
                dataItem.Type = Text.CURRENT_STAGE;
            }
            dataItem.Index = GetNewIndex();
            source.Add(dataItem);
        }

        public DataItem Get( int index ) {
            var found = source.Where(n => n.Index == index);
            if( found.Count() > 0 ) {
                return found.ElementAt(0);
            } else {
                return null;
            }
        }

        public bool Remove( int index ) {
            var found = source.Where(n => n.Index == index);
            if (found.Count() > 0 ) {
                source.Remove(found.ElementAt(0));
                return true;
            } else {
                return false;
            }
        }

        public DataTable ToDataTable( ItemType type ) {
            source.Sort(( a, b ) => a.Index - b.Index);
            var dt = new DataTable();
            dt.Columns.Add("type", typeof(string));
            dt.Columns.Add("type_selected_index", typeof(int));
            dt.Columns.Add("project", typeof(string));
            dt.Columns.Add("project_selected_index", typeof(int));
            dt.Columns.Add("content", typeof(string));
            dt.Columns.Add("process", typeof(string));
            dt.Columns.Add("process_selected_index", typeof(int));
            dt.Columns.Add("index", typeof(int));
            source.ForEach(item => {
                if(type == ItemType.Current && item.Type == Text.NEXT_STAGE ) {
                    return;
                }
                if(type == ItemType.Next && item.Type == Text.CURRENT_STAGE ) {
                    return;
                }
                var row = dt.NewRow();
                row ["content"] = item.Content;
                row ["project"] = string.IsNullOrEmpty(item.Project)  ? "......" : item.Project;
                row ["project_selected_index"] = GetProjectSelectedIndex(item.Project);
                row ["type"] = item.Type;
                row ["type_selected_index"] = GetTypeSelectedIndex(item.Type);
                row ["process"] = item.Process;
                row ["process_selected_index"] = GetProcessSelectedIndex(item.Process);
                row ["index"] = item.Index;
                dt.Rows.Add(row);
            });
            return dt;
        }

        private int GetProcessSelectedIndex( string process ) {
            if(string.IsNullOrEmpty(process)) {
                return 2;
            } else {
                switch( process ) {
                    case Text.NO_SELECT:
                        return 0;
                    case Text.ONLINED:
                        return 1;
                    case Text.TESTING:
                        return 2;
                    case Text.FINISHED:
                        return 3;
                    default:
                        var regex = new Regex(@"[1-9]+");
                        var result = regex.Match(process);
                        if(result.Success) {
                            return 13 - Convert.ToInt32(result.Value);
                        }
                        return 0;

                }
            }
        }

        private int GetProjectSelectedIndex( string project ) {
            if(string.IsNullOrEmpty(project)) {
                return 0;
            } else {
                return 2;
            }
        }

        private int GetTypeSelectedIndex( string type ) {
            if(type == Text.NEXT_STAGE) {
                return 1;
            } else {
                return 0;
            }
        }

        public ResultInfo IsValid() {
            var isValid = true;
            var message = string.Empty;
            var index = 0;
            foreach(DataItem item in source) {
                if(string.IsNullOrEmpty(item.Project)) {
                    index = item.Index;
                    isValid = false;
                    message = "项目未填写";
                    break;
                }
                if(string.IsNullOrEmpty(item.Content)) {
                    index = item.Index;
                    isValid = false;
                    message = "内容未填写";
                    break;
                }
            }
            if(!isValid) {
                message = "第" + index + "行数据不完整，" + message;
            }
            if(Size()== 0) {
                isValid = false;
                message = "工作项为空";
            }
            return ResultInfo.Create(isValid, message);
        }

        public ResultInfo Update(int dataId) {
            if( !Identity.IsRegister() ) {
                return ResultInfo.Create(false, Message.MSG1);
            }
            var sql = "delete from job_info_item where job_id=" + dataId;
            DbHelper.ExecuteSql(sql);
            this.SaveItems(dataId);
            return ResultInfo.Create(true, "你的工作项保存好啦");
        }

        public ResultInfo Save() {
            if(!Identity.IsRegister()) {
                return ResultInfo.Create(false, Message.MSG1);
            }
            var userInfo = Identity.GetUserInfo();
            var sql = "insert into job_info (identity_id) output inserted.id values("+ userInfo.ID +")";
            var jobId = Convert.ToInt32(DbHelper.GetSingle(sql));
            this.SaveItems(jobId);
            return ResultInfo.Create(true, "你的工作项保存好啦");
        }

        private void SaveItems(int jobId) {
            foreach( DataItem dataItem in source ) {
                var sql = "insert into job_info_item(job_id,item_type,item_content,item_project,item_process) values(" + jobId + ",@item_type,@item_content,@item_project,@item_process)";
                var paramList = new SqlParameter [] {
                    new SqlParameter("@item_type",SqlDbType.NVarChar),
                    new SqlParameter("@item_content",SqlDbType.NVarChar),
                    new SqlParameter("@item_project",SqlDbType.NVarChar),
                    new SqlParameter("@item_process",SqlDbType.NVarChar),
                };
                paramList [0].Value = dataItem.Type;
                paramList [1].Value = dataItem.Content;
                paramList [2].Value = dataItem.Project;
                paramList [3].Value = dataItem.Process;
                DbHelper.ExecuteSql(sql, paramList);
            }
        }

        public static DataTable GetJobJnfo( int idenityId ) {
            var sql = @"select id,
                         add_time,
                         month(add_time) as month,
                         DATEPART(wk,add_time) as wk
                         from job_info where identity_id = " + idenityId + " order by id desc";
            return DbHelper.Query(sql).Tables [0];
        }

        public static DataTable GetJobInfoItems( int jobId ) {
            var sql = "select * from job_info_item where job_id=" + jobId;
            var result = DbHelper.Query(sql).Tables [0];
            return result;
        }

        public static void DeleteJobInfo(int id ) {
            var sql = "delete from job_info where id = " + id;
            DbHelper.ExecuteSql(sql);
            sql = "delete from job_info_item where job_id = " + id;
            DbHelper.ExecuteSql(sql);
        }

        public static DataSource load( int jobId ) {
            var sql = "select * from job_info_item where job_id = " + jobId;
            var queryResult = DbHelper.Query(sql).Tables [0];
            var dataSource = new DataSource();
            foreach(DataRow row in queryResult.Rows) {
                var dataItem = new DataItem();
                dataItem.Content = row ["item_content"].ToString();
                dataItem.Process = row ["item_process"].ToString();
                dataItem.Project = row ["item_project"].ToString();
                dataItem.Type = row ["item_type"].ToString();
                dataSource.Add(dataItem);
            }
            return dataSource;
        }


        public static DataTable GetAllJobInfo() {
            var sql = @"select a.*,b.name,
                          DATEPART(wk,a.add_time) as wk
                        from job_info as a left join identity_info as b  on a.identity_id = b.id order by a.add_time desc";
            return DbHelper.Query(sql).Tables [0];
        }

        public static DataTable GetJobInfoByName( string name ) {
            var sql = @"select a.*,b.name,
                          DATEPART(wk,a.add_time) as wk
                        from job_info as a left join identity_info as b  on a.identity_id = b.id where  identity_id in (select id from identity_info where CHARINDEX(@name,name)>0 ) order by a.add_time desc";
            var paramList = new SqlParameter [] {
                new SqlParameter("@name",SqlDbType.NVarChar)
            };
            paramList [0].Value = name;
            return DbHelper.Query(sql, paramList).Tables [0];
        }

        public static DataTable GetResult(List<int> jobIDs, string type ) {
            var idString = string.Join(",", jobIDs.Select(n=>n.ToString()).ToArray());
            var sql = "select * from f_get_result(@type,@idString)";
            var paramList = new SqlParameter [] {
                new SqlParameter("@type", SqlDbType.NVarChar),
                new SqlParameter("@idString",SqlDbType.VarChar)
            };
            paramList [0].Value = type;
            paramList [1].Value = idString;
            var result = DbHelper.Query(sql, paramList).Tables [0];
            return result;
        }
 
    }
}