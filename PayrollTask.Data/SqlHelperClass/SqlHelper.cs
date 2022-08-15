using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PayrollTask.Data.SqlHelper
{
   public class SqlHelper
    {
        public static string ConnectionString = string.Empty;

        public static IEnumerable<T> GetData<T>(string sql, List<SqlParameter> sqlParameters = null) where T : new()
        {
            DataSet ds = new DataSet();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand(sql, con))
                {
                    if (sqlParameters != null)
                        cmd.Parameters.AddRange(sqlParameters.ToArray());
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                    }
                    con.Close();
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    IEnumerable<T> list = JsonConvert.DeserializeObject<IEnumerable<T>>(JSONString);
                    return list;
                }
            }
        }
    }
}
