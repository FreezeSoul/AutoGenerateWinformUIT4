using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace DataAccessLayer.Ado
{

	public class ExecutionLogAdoDAL
    {
        
        private const string TableName = @"ExecutionLog";
		
		private const string PagingTemplate = @"SELECT  RowIndex ,
                                                                                    T.*
                                                                            FROM    ( SELECT    T2.* ,
                                                                                                ROW_NUMBER() OVER ( ORDER BY {0} DESC ) AS RowIndex
                                                                                      FROM   ( {1} )  T2
                                                                                    ) AS T
                                                                            WHERE   T.RowIndex > {2}
                                                                            AND T.RowIndex <= {3}";
		
		private Database _db ;
		
        public ExecutionLogAdoDAL()
        {
			 _db = DatabaseFactory.CreateDatabase();
        }
		
		public ExecutionLogAdoDAL(string connectionStringName)
        {
			 _db = DatabaseFactory.CreateDatabase(connectionStringName);
        }
		
		public int GetDataTableCount(string searchString,Dictionary<string, object> searchParams)
		{
			var sqlStr = string.Format(@"SELECT COUNT(*) FROM {0} {1}",TableName,searchString);
			var dbCommand = _db.GetSqlStringCommand(sqlStr);
            if (searchParams != null)
            {
                searchParams.ToList().ForEach(item =>
                {
                    var param = dbCommand.CreateParameter();
                    param.ParameterName = item.Key;
                    param.Value = item.Value;
                    dbCommand.Parameters.Add(param);
                });
            }
            var obj = _db.ExecuteScalar(dbCommand);
			int i = 0;
			if(obj!=null)
			{
				int.TryParse(obj.ToString(),out i);
			}
			return i;
		}
		
		public DataTable GetPageDataTable(string searchString,string sortColumn,Dictionary<string, object> searchParams)
		{
			var sqlStr = string.Format(@"SELECT * FROM {0} {1} ORDER BY {2} DESC",TableName,searchString,sortColumn);
			
			var dbCommand = _db.GetSqlStringCommand(sqlStr);
			
            if (searchParams != null)
            {
                searchParams.ToList().ForEach(item =>
                {
                    var param = dbCommand.CreateParameter();
                    param.ParameterName = item.Key;
                    param.Value = item.Value;
                    dbCommand.Parameters.Add(param);
                });
            }
            return _db.ExecuteDataSet(dbCommand).Tables[0];
		}
		
		public DataTable GetPageDataTable(string searchString,string sortColumn,Dictionary<string, object> searchParams, int rowStart, int rowEnd)
		{
			var sqlStr = string.Format(@"SELECT * FROM {0} {1}", TableName, searchString);

            sqlStr = string.Format(PagingTemplate, sortColumn, sqlStr, rowStart, rowEnd);
			
			var dbCommand = _db.GetSqlStringCommand(sqlStr);
            if (searchParams != null)
            {
                searchParams.ToList().ForEach(item =>
                {
                    var param = dbCommand.CreateParameter();
                    param.ParameterName = item.Key;
                    param.Value = item.Value;
                    dbCommand.Parameters.Add(param);
                });
            }
            return _db.ExecuteDataSet(dbCommand).Tables[0];
		}
}
}
