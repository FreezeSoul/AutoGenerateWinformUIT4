using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DataAccessLayer.Common
{
    public class MiniSqlHelper
    {
        #region SqlHelper

        private Database _db;

        public MiniSqlHelper()
        {
            _db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="paramObjs"></param>
        /// <returns></returns>
        public DataTable ExecuteDataSet(string sqlStr, Dictionary<string, object> paramObjs)
        {
            DbCommand dbCommand = _db.GetSqlStringCommand(sqlStr);
            if (paramObjs != null)
            {
                paramObjs.ToList().ForEach(item =>
                {
                    var param = dbCommand.CreateParameter();
                    param.ParameterName = item.Key;
                    param.Value = item.Value;
                    dbCommand.Parameters.Add(param);
                });
            }
            return _db.ExecuteDataSet(dbCommand).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public DataTable ExecuteDataSet(string sqlStr)
        {
            return this.ExecuteDataSet(sqlStr, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="paramObjs"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlStr, Dictionary<string, object> paramObjs)
        {
            DbCommand dbCommand = _db.GetSqlStringCommand(sqlStr);
            if (paramObjs != null)
            {
                paramObjs.ToList().ForEach(item =>
                {
                    var param = dbCommand.CreateParameter();
                    param.ParameterName = item.Key;
                    param.Value = item.Value;
                    dbCommand.Parameters.Add(param);
                });
            }
            return _db.ExecuteScalar(dbCommand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlStr)
        {
            return this.ExecuteScalar(sqlStr, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="paramObjs"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlStr, Dictionary<string, object> paramObjs)
        {
            DbCommand dbCommand = _db.GetSqlStringCommand(sqlStr);
            if (paramObjs != null)
            {
                paramObjs.ToList().ForEach(item =>
                {
                    var param = dbCommand.CreateParameter();
                    param.ParameterName = item.Key;
                    param.Value = item.Value;
                    dbCommand.Parameters.Add(param);
                });
            }
            return _db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlStr)
        {
            return this.ExecuteNonQuery(sqlStr);
        }
        #endregion
    }
}
