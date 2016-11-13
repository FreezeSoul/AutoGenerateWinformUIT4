using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace DataAccessLayer.Common
{
    public class AdoPagingHelper
    {

        private int _pageSize;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        private int _pageIndex;

        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }

        private int _recordCount = -1;

        public int RecordCount
        {
            get
            {
                if (_recordCount == -1)
                {
                    InitPaging();
                }
                return _recordCount;
            }
            set { _recordCount = value; }
        }


        public int PageCount
        {
            get
            {
                if (RecordCount % PageSize != 0)
                    return (int)(RecordCount / PageSize) + 1;
                return (int)(RecordCount / PageSize);
            }
        }

        private string _sortColumn;

        public string SortColumn
        {
            get { return _sortColumn; }
            set { _sortColumn = value; }
        }

        private string _strSql;

        public string StrSql
        {
            get { return _strSql; }
            set { _strSql = value; }
        }

        private AdoPagingHelper()
        { }

        public AdoPagingHelper(string strSql, string sortColumn, int pageSize)
        {
            this.StrSql = strSql;
            this.SortColumn = sortColumn;
            this.PageSize = pageSize;
        }

        private void InitPaging()
        {
            var str = string.Format(@"Select Count(*) From ({0}) T", this.StrSql);
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetSqlStringCommand(str);
            this.RecordCount = int.Parse(db.ExecuteScalar(dbCommand).ToString());
        }

        private const string PagingTemplate = @"SELECT  RowIndex ,
                                                                                    T.*
                                                                            FROM    ( SELECT    T2.* ,
                                                                                                ROW_NUMBER() OVER ( ORDER BY {0} DESC ) AS RowIndex
                                                                                      FROM   ( {1} )  T2
                                                                                    ) AS T
                                                                            WHERE   T.RowIndex > {2}
                                                                            AND T.RowIndex <= {3}";


        public string GetPagingStrSql(int pageIndex)
        {
            this.PageIndex = pageIndex;
            return string.Format(PagingTemplate, SortColumn, StrSql, PageIndex * PageSize, (PageIndex + 1) * PageSize);
        }

        public DataTable GetPagingData(int pageIndex)
        {
            if (pageIndex < 0)
                pageIndex = 0;
            if (pageIndex >= PageCount)
                pageIndex = PageCount - 1;
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetSqlStringCommand(GetPagingStrSql(pageIndex));
            return db.ExecuteDataSet(dbCommand).Tables[0];
        }
    }
}
