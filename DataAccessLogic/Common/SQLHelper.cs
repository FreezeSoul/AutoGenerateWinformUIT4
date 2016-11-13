using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Threading;

namespace DataAccessLayer.Common
{
    /// <summary>
    /// MS-SQL Helper 扩展类。
    /// 提供放置数据层公用(默认)连接串的空间和常用数据库操作方法
    /// 注意　在使用数据库方法之前必须为 DefaultConnectString 属性赋值
    /// 典型时机： web 之 Application_Start , win 之 Main()
    /// </summary>
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    public static class SQLHelper
    {
        #region 数据库默认连接串(需要在操作前赋初值)

        private static string _connstr = null;
        /// <summary>
        /// 数据库默认连接串(需要在操作前赋初值)
        /// 示例：SQLHelper.DefaultConnectString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}", "SQL001", "testdb", "sa", "123");
        /// </summary>
        public static string DefaultConnectString
        {
            get
            {
                if (_connstr == null) throw new Exception("在使用数据库方法之前必须为 DefaultConnectString 属性赋值！");
                return _connstr;
            }
            set { _connstr = value; }
        }

        #endregion

        #region 线程私有连接，事务支持定义，方法

        /// <summary>
        /// 线程私有连接集合
        /// </summary>
        private static Dictionary<int, SqlConnection> _connDict = new Dictionary<int, SqlConnection>();
        /// <summary>
        /// 连接所创建的事务集合
        /// </summary>
        private static Dictionary<SqlConnection, SqlTransaction> _tranDict = new Dictionary<SqlConnection, SqlTransaction>();
        /// <summary>
        /// 线程私有连接，优先度高于公用连接串( 用 NewConn() 创建 )
        /// </summary>
        public static SqlConnection CurrConn
        {
            get
            {
                int tid = Thread.CurrentThread.ManagedThreadId;
                lock (_connDict)
                {
                    if (!_connDict.ContainsKey(tid)) return null;
                    return _connDict[tid];
                }
            }
        }
        /// <summary>
        /// 线程私有事务，优先度高于私有连接( 用 NewTran() 创建 )
        /// </summary>
        public static SqlTransaction CurrTran
        {
            get
            {
                SqlConnection conn = CurrConn;
                if (conn == null) return null;
                lock (_tranDict)
                {
                    if (!_tranDict.ContainsKey(conn)) return null;
                    SqlTransaction stran = _tranDict[conn];
                    if (stran.Connection != null) return stran;
                    _tranDict.Remove(conn);
                }
                return null;
            }
        }
        /// <summary>
        /// 依据公用数据库连接串的内容创建一个线程私有连接并返回
        /// </summary>
        public static SqlConnection NewConn()
        {
            SqlConnection conn = new SqlConnection(DefaultConnectString);
            conn.Disposed += new EventHandler(sconn_Disposed);
            lock (_connDict)
            {
                _connDict.Add(Thread.CurrentThread.ManagedThreadId, conn);
            }
            return conn;
        }
        /// <summary>
        /// 依据传入的连接串创建一个线程私有连接并返回
        /// </summary>
        public static SqlConnection NewConn(string s)
        {
            SqlConnection conn = new SqlConnection(s);
            conn.Disposed += new EventHandler(sconn_Disposed);
            lock (_connDict)
            {
                _connDict.Add(Thread.CurrentThread.ManagedThreadId, conn);
            }
            return conn;
        }
        /// <summary>
        /// 依据公用数据库连接串的内容创建一个线程私有连接并返回(可打开)
        /// </summary>
        public static SqlConnection NewConn(bool opened)
        {
            SqlConnection conn = new SqlConnection(DefaultConnectString);
            conn.Disposed += new EventHandler(sconn_Disposed);
            lock (_connDict)
            {
                _connDict.Add(Thread.CurrentThread.ManagedThreadId, conn);
            }
            if (opened)
                conn.Open();
            return conn;
        }
        /// <summary>
        /// 依据传入的连接串创建一个线程私有连接并返回(可打开)
        /// </summary>
        public static SqlConnection NewConn(string s, bool opened)
        {
            SqlConnection conn = new SqlConnection(s);
            conn.Disposed += new EventHandler(sconn_Disposed);
            lock (_connDict)
            {
                _connDict.Add(Thread.CurrentThread.ManagedThreadId, conn);
            }
            if (opened)
                conn.Open();
            return conn;
        }




        /// <summary>
        /// 当连接 Dispose() 时从集合中移除
        /// </summary>
        private static void sconn_Disposed(object sender, EventArgs e)
        {
            lock (_tranDict)
            {
                _tranDict.Remove(CurrConn);
            }
            lock (_connDict)
            {
                _connDict.Remove(Thread.CurrentThread.ManagedThreadId);
            }
        }

        /// <summary>
        /// 依据当前上下文中的连接创建一个事务并返回
        /// </summary>
        public static SqlTransaction NewTran()
        {
            SqlConnection conn = CurrConn;
            if (conn == null) throw new Exception("NewConn First !");
            SqlTransaction st = conn.BeginTransaction();
            lock (_tranDict)
            {
                _tranDict.Add(conn, st);
            }
            return st;
        }

        #endregion

        #region 命令对象，参数缓存

        private static Dictionary<string, SqlParameter[]> _cachedParms = new Dictionary<string, SqlParameter[]>();
        private static Dictionary<string, SqlCommand> _cachedCmds = new Dictionary<string, SqlCommand>();

        /// <summary>
        /// 将参数列表添加到字典
        /// </summary>
        public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        {
            lock (_cachedParms)
            {
                _cachedParms[cacheKey] = commandParameters;
            }
        }

        /// <summary>
        /// 将命令对象添加到字典（连接和事务属性将被清除）
        /// </summary>
        public static void CacheCommand(string cacheKey, SqlCommand cmd)
        {
            SqlCommand ncmd = cmd.Clone();
            ncmd.Transaction = null;
            ncmd.Connection = null;
            _cachedCmds[cacheKey] = ncmd;
        }

        /// <summary>
        /// 从字典获取参数列表
        /// </summary>
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = null; ;
            lock (_cachedParms)
            {
                if (!_cachedParms.ContainsKey(cacheKey)) return null;
                cachedParms = _cachedParms[cacheKey];
            }
            int count = cachedParms.Length;
            SqlParameter[] clonedParms = new SqlParameter[count];
            for (int i = 0; i < count; i++)
            {
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();
            }
            return clonedParms;
        }

        /// <summary>
        /// 从字典获取命令对象
        /// </summary>
        public static SqlCommand GetCachedCommand(string cacheKey)
        {
            lock (_cachedCmds)
            {
                if (!_cachedCmds.ContainsKey(cacheKey)) return null;
                return _cachedCmds[cacheKey].Clone();
            }
        }

        /// <summary>
        /// 命令对象准备方法
        /// </summary>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #endregion

        #region 数据转义方法

        /// <summary>
        /// 提供对sql200x查询用于 like 之类的比较字串的转义
        /// </summary>
        public static string EscapeLike(string s)
        {
            return string.IsNullOrEmpty(s) ? s : s.Replace("'", "''").Replace("[", "[[]").Replace("_", "[_]").Replace("%", "[%]").Replace("-", "[-]").Replace("^", "[^]");
        }

        /// <summary>
        /// 提供对sql200x查询用于 equal 之类的比较字串的转义
        /// </summary>
        public static string EscapeEqual(string s)
        {
            return string.IsNullOrEmpty(s) ? s : s.Replace("'", "''");
        }


        public static string[] _hexstrs = new string[] { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "0A", "0B", "0C", "0D", "0E", "0F", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "1A", "1B", "1C", "1D", "1E", "1F", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "2A", "2B", "2C", "2D", "2E", "2F", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "3A", "3B", "3C", "3D", "3E", "3F", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "4A", "4B", "4C", "4D", "4E", "4F", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "5A", "5B", "5C", "5D", "5E", "5F", "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "6A", "6B", "6C", "6D", "6E", "6F", "70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "7A", "7B", "7C", "7D", "7E", "7F"
			, "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "8A", "8B", "8C", "8D", "8E", "8F", "90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "9A", "9B", "9C", "9D", "9E", "9F", "A0", "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "AA", "AB", "AC", "AD", "AE", "AF", "B0", "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9", "BA", "BB", "BC", "BD", "BE", "BF", "C0", "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "CA", "CB", "CC", "CD", "CE", "CF", "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "DA", "DB", "DC", "DD", "DE", "DF", "E0", "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9", "EA", "EB", "EC", "ED", "EE", "EF", "F0", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "FA", "FB", "FC", "FD", "FE", "FF" };
        /// <summary>
        /// byte[] 转为 16 进制字串(用于 insert byte[] 值的参数)
        /// </summary>
        public static string GetHexString(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return "0x";
            StringBuilder sb = new StringBuilder("0x");
            foreach (byte b in bytes)
            {
                sb.Append(_hexstrs[b]);
            }
            return sb.ToString();
        }

        #endregion

        #region SQL 操作方法

        #region DataAdapter

        /// <summary>
        /// 通过一个表查询 TSQL 来创建一个全功能 DataAdapter 并返回
        /// </summary>
        public static SqlDataAdapter NewDataAdapter(string s)
        {
            SqlDataAdapter sda = new SqlDataAdapter(s, DefaultConnectString);
            SqlCommandBuilder scb = new SqlCommandBuilder(sda);
            return sda;
        }

        /// <summary>
        /// 构造一个 DataAdapter 并返回
        /// </summary>
        public static SqlDataAdapter NewDataAdapter(SqlCommand insertCmd, SqlCommand deleteCmd, SqlCommand updateCmd, int? updateBatchSize)
        {
            SqlDataAdapter sda = new SqlDataAdapter();
            if (insertCmd != null)
            {
                sda.InsertCommand = insertCmd;
            }
            if (deleteCmd != null)
            {
                sda.DeleteCommand = deleteCmd;
            }
            if (updateCmd != null)
            {
                sda.UpdateCommand = updateCmd;
                if (updateBatchSize != null) sda.UpdateBatchSize = updateBatchSize.Value;
            }
            return sda;
        }
        /// <summary>
        /// 构造一个 DataAdapter 并返回
        /// </summary>
        public static SqlDataAdapter NewDataAdapter(SqlConnection conn, SqlCommand insertCmd, SqlCommand deleteCmd, SqlCommand updateCmd, int? updateBatchSize)
        {
            SqlDataAdapter sda = new SqlDataAdapter();
            if (insertCmd != null)
            {
                insertCmd.Connection = conn;
                sda.InsertCommand = insertCmd;
            }
            if (deleteCmd != null)
            {
                deleteCmd.Connection = conn;
                sda.DeleteCommand = deleteCmd;
            }
            if (updateCmd != null)
            {
                updateCmd.Connection = conn;
                sda.UpdateCommand = updateCmd;
                if (updateBatchSize != null) sda.UpdateBatchSize = updateBatchSize.Value;
            }
            return sda;
        }

        /// <summary>
        /// 构造一个 DataAdapter 并返回
        /// </summary>
        public static SqlDataAdapter NewDataAdapter(SqlTransaction tran, SqlCommand insertCmd, SqlCommand deleteCmd, SqlCommand updateCmd, int? updateBatchSize)
        {
            SqlConnection conn = tran.Connection;
            SqlDataAdapter sda = new SqlDataAdapter();
            if (insertCmd != null)
            {
                insertCmd.Connection = conn;
                insertCmd.Transaction = tran;
                sda.InsertCommand = insertCmd;
            }
            if (deleteCmd != null)
            {
                deleteCmd.Connection = conn;
                deleteCmd.Transaction = tran;
                sda.DeleteCommand = deleteCmd;
            }
            if (updateCmd != null)
            {
                updateCmd.Connection = conn;
                updateCmd.Transaction = tran;
                sda.UpdateCommand = updateCmd;
                if (updateBatchSize != null) sda.UpdateBatchSize = updateBatchSize.Value;
            }
            return sda;
        }

        /// <summary>
        /// 构造一个 DataAdapter 并返回
        /// </summary>
        public static SqlDataAdapter NewDataAdapter(SqlConnection conn, SqlTransaction tran, SqlCommand insertCmd, SqlCommand deleteCmd, SqlCommand updateCmd, int? updateBatchSize)
        {
            SqlDataAdapter sda = new SqlDataAdapter();
            if (insertCmd != null)
            {
                insertCmd.Connection = conn;
                insertCmd.Transaction = tran;
                sda.InsertCommand = insertCmd;
            }
            if (deleteCmd != null)
            {
                deleteCmd.Connection = conn;
                deleteCmd.Transaction = tran;
                sda.DeleteCommand = deleteCmd;
            }
            if (updateCmd != null)
            {
                updateCmd.Connection = conn;
                updateCmd.Transaction = tran;
                sda.UpdateCommand = updateCmd;
                if (updateBatchSize != null) sda.UpdateBatchSize = updateBatchSize.Value;
            }
            return sda;
        }


        /// <summary>
        /// 为 SqlDataAdapter 中的 Commands 设置连接属性
        /// </summary>
        public static void SetConnection(SqlDataAdapter sda, SqlConnection conn)
        {
            if (sda.InsertCommand != null)
            {
                sda.InsertCommand.Connection = conn;
            }
            if (sda.DeleteCommand != null)
            {
                sda.DeleteCommand.Connection = conn;
            }
            if (sda.UpdateCommand != null)
            {
                sda.UpdateCommand.Connection = conn;
            }
        }


        /// <summary>
        /// 为 SqlDataAdapter 中的 Commands 设置连接属性
        /// </summary>
        public static void SetConnection(SqlDataAdapter sda, SqlTransaction tran)
        {
            SqlConnection conn = tran.Connection;

            if (sda.InsertCommand != null)
            {
                sda.InsertCommand.Connection = conn;
                sda.InsertCommand.Transaction = tran;
            }
            if (sda.DeleteCommand != null)
            {
                sda.DeleteCommand.Connection = conn;
                sda.DeleteCommand.Transaction = tran;
            }
            if (sda.UpdateCommand != null)
            {
                sda.UpdateCommand.Connection = conn;
                sda.UpdateCommand.Transaction = tran;
            }
        }

        /// <summary>
        /// 用相应的 command 对象更新数据集指定表的数据
        /// </summary>
        /// <returns>返回受影响行数</returns>
        public static int UpdateData(DataSet ds, string tn, SqlCommand insertCmd, SqlCommand deleteCmd, SqlCommand updateCmd, int? updateBatchSize)
        {
            SqlConnection conn = CurrConn;
            int ocs = -1;
            if (conn == null) conn = new SqlConnection(DefaultConnectString);
            else ocs = (int)conn.State;
            if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Open();
            try
            {
                SqlDataAdapter sda = NewDataAdapter(conn, CurrTran, insertCmd, deleteCmd, updateCmd, updateBatchSize);
                return sda.Update(ds, tn);
            }
            finally
            {
                if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Close();
                if (ocs == -1) conn.Dispose();
            }
        }

        /// <summary>
        /// 用相应的 command 对象更新表的数据
        /// </summary>
        /// <returns>返回受影响行数</returns>
        public static int UpdateData(DataTable dt, SqlCommand insertCmd, SqlCommand deleteCmd, SqlCommand updateCmd, int? updateBatchSize)
        {
            SqlConnection conn = CurrConn;
            int ocs = -1;
            if (conn == null) conn = new SqlConnection(DefaultConnectString);
            else ocs = (int)conn.State;
            if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Open();
            try
            {
                SqlDataAdapter sda = NewDataAdapter(conn, CurrTran, insertCmd, deleteCmd, updateCmd, updateBatchSize);
                return sda.Update(dt);
            }
            finally
            {
                if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Close();
                if (ocs == -1) conn.Dispose();
            }
        }


        /// <summary>
        /// 用相应的 command 对象更新 数行 数据
        /// </summary>
        /// <returns>返回受影响行数</returns>
        public static int UpdateData(DataRow[] rows, SqlCommand insertCmd, SqlCommand deleteCmd, SqlCommand updateCmd, int? updateBatchSize)
        {
            SqlConnection conn = CurrConn;
            int ocs = -1;
            if (conn == null) conn = new SqlConnection(DefaultConnectString);
            else ocs = (int)conn.State;
            if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Open();
            try
            {
                SqlDataAdapter sda = NewDataAdapter(conn, CurrTran, insertCmd, deleteCmd, updateCmd, updateBatchSize);
                return sda.Update(rows);
            }
            finally
            {
                if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Close();
                if (ocs == -1) conn.Dispose();
            }
        }

        /// <summary>
        /// 用相应的 command 对象更新 单行 数据
        /// </summary>
        /// <returns>返回受影响行数</returns>
        public static int UpdateData(DataRow row, SqlCommand insertCmd, SqlCommand deleteCmd, SqlCommand updateCmd, int? updateBatchSize)
        {
            return UpdateData(new DataRow[] { row }, insertCmd, deleteCmd, updateCmd, updateBatchSize);
        }

        #endregion

        #region Command

        /// <summary>
        /// 执行一个 SQL 命令对象，返加受影响行数
        /// </summary>
        public static int ExecuteNonQuery(SqlCommand cmd)
        {
            if (cmd.Connection == null)
            {
                SqlConnection conn = CurrConn;
                int ocs = -1;
                if (conn == null) conn = new SqlConnection(DefaultConnectString);
                else ocs = (int)conn.State;
                if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Open();
                try
                {
                    cmd.Connection = conn;
                    if (cmd.Transaction == null) cmd.Transaction = CurrTran;
                    return cmd.ExecuteNonQuery();
                }
                finally
                {
                    if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Close();
                    if (ocs == -1) conn.Dispose();
                }
            }
            else
            {
                return cmd.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// 执行一个 SQL 命令对象，返回第一行第一列的内容
        /// </summary>
        public static object ExecuteScalar(SqlCommand cmd)
        {
            if (cmd.Connection == null)
            {
                SqlConnection conn = CurrConn;
                int ocs = -1;
                if (conn == null) conn = new SqlConnection(DefaultConnectString);
                else ocs = (int)conn.State;
                if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Open();
                try
                {
                    cmd.Connection = conn;
                    if (cmd.Transaction == null) cmd.Transaction = CurrTran;
                    return cmd.ExecuteScalar();
                }
                finally
                {
                    if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Close();
                    if (ocs == -1) conn.Dispose();
                }
            }
            else
            {
                return cmd.ExecuteScalar();
            }
        }


        /// <summary>
        /// 创建一个新连接（无事务支持），执行一个 SQL 命令对象，返回一个 DataReader（关闭的同时将关闭连接）
        /// </summary>
        public static SqlDataReader ExecuteDataReader(SqlCommand cmd)
        {
            if (cmd.Connection == null)
            {
                SqlConnection conn = CurrConn;
                int ocs = -1;
                if (conn == null) conn = new SqlConnection(DefaultConnectString);
                else ocs = (int)conn.State;
                if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Open();

                cmd.Connection = conn;
                if (cmd.Transaction == null) cmd.Transaction = CurrTran;

                if (ocs == -1 || ocs == (int)ConnectionState.Closed) return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                else return cmd.ExecuteReader();
            }
            else
            {
                return cmd.ExecuteReader();
            }
        }

        /// <summary>
        /// 执行一个 SQL 命令对象，返回一个数据集
        /// </summary>
        public static DataSet ExecuteDataSet(SqlCommand cmd)
        {
            if (cmd.Connection == null)
            {
                SqlConnection conn = CurrConn;
                int ocs = -1;
                if (conn == null) conn = new SqlConnection(DefaultConnectString);
                else ocs = (int)conn.State;
                if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Open();
                try
                {
                    cmd.Connection = conn;
                    if (cmd.Transaction == null) cmd.Transaction = CurrTran;
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        DataSet ds = new DataSet();
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                        return ds;
                    }
                }
                finally
                {
                    if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Close();
                    if (ocs == -1) conn.Dispose();
                }
            }
            else
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    DataSet ds = new DataSet();
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                    return ds;
                }
            }
        }



        /// <summary>
        /// 执行一个 SQL 命令对象，填充一个数据集，返加受影响行数
        /// </summary>
        public static int ExecuteDataSet(DataSet ds, SqlCommand cmd)
        {
            if (cmd.Connection == null)
            {
                SqlConnection conn = CurrConn;
                int ocs = -1;
                if (conn == null) conn = new SqlConnection(DefaultConnectString);
                else ocs = (int)conn.State;
                if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Open();
                try
                {
                    cmd.Connection = conn;
                    if (cmd.Transaction == null) cmd.Transaction = CurrTran;
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        return da.Fill(ds);
                    }
                }
                finally
                {
                    if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Close();
                    if (ocs == -1) conn.Dispose();
                }
            }
            else
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = cmd;
                    return da.Fill(ds);
                }
            }
        }


        /// <summary>
        /// 执行一个 SQL 命令对象，填充一个数据表，返回受影响行数
        /// </summary>
        public static int ExecuteDataTable(DataTable dt, SqlCommand cmd)
        {
            if (cmd.Connection == null)
            {
                SqlConnection conn = CurrConn;
                int ocs = -1;
                if (conn == null) conn = new SqlConnection(DefaultConnectString);
                else ocs = (int)conn.State;
                if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Open();
                try
                {
                    cmd.Connection = conn;
                    if (cmd.Transaction == null) cmd.Transaction = CurrTran;
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        return da.Fill(dt);
                    }
                }
                finally
                {
                    if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Close();
                    if (ocs == -1) conn.Dispose();
                }
            }
            else
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = cmd;
                    return da.Fill(dt);
                }
            }
        }

        /// <summary>
        /// 执行一个 SQL 命令对象，返回一个数据表
        /// </summary>
        public static DataTable ExecuteDataTable(SqlCommand cmd)
        {
            DataSet ds = ExecuteDataSet(cmd);
            if (ds.Tables.Count > 0) return ds.Tables[0];
            else return null;
        }

        #endregion

        #region TSQL String

        /// <summary>
        /// 执行一个 SQL 语句，返回影响行数
        /// </summary>
        public static int ExecuteNonQuery(string s)
        {
            int effect = 0;
            SqlConnection conn = CurrConn;
            int ocs = -1;
            if (conn == null) conn = new SqlConnection(DefaultConnectString);
            else ocs = (int)conn.State;
            if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Open();
            try
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.Transaction = CurrTran;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = s;
                    effect = cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Close();
                if (ocs == -1) conn.Dispose();
            }
            return effect;
        }





        /// <summary>
        /// 执行一个 SQL 语句，返回第一行第一列的内容
        /// </summary>
        public static object ExecuteScalar(string s)
        {
            object r = null;
            SqlConnection conn = CurrConn;
            int ocs = -1;
            if (conn == null) conn = new SqlConnection(DefaultConnectString);
            else ocs = (int)conn.State;
            if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Open();
            try
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.Transaction = CurrTran;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = s;
                    r = cmd.ExecuteScalar();
                }
            }
            finally
            {
                if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Close();
                if (ocs == -1) conn.Dispose();
            }
            return r;
        }




        /// <summary>
        /// 创建一个新连接（无事务支持），执行一个 SQL 语句，返回一个 DataReader（关闭的同时将关闭连接）
        /// </summary>
        public static SqlDataReader ExecuteDataReader(string s)
        {
            SqlDataReader r = null;
            SqlConnection conn = CurrConn;
            int ocs = -1;
            if (conn == null) conn = new SqlConnection(DefaultConnectString);
            else ocs = (int)conn.State;
            if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.Transaction = CurrTran;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = s;
                if (ocs == -1 || ocs == (int)ConnectionState.Closed) r = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                else r = cmd.ExecuteReader();
            }
            return r;
        }






        /// <summary>
        /// 执行一个 SQL 语句，返回一个数据集
        /// </summary>
        public static DataSet ExecuteDataSet(string s)
        {
            DataSet ds = null;
            SqlConnection conn = CurrConn;
            int ocs = -1;
            if (conn == null) conn = new SqlConnection(DefaultConnectString);
            else ocs = (int)conn.State;
            if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Open();
            try
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.Transaction = CurrTran;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = s;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        ds = new DataSet();
                        sda.Fill(ds);
                    }
                }
            }
            finally
            {
                if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Close();
                if (ocs == -1) conn.Dispose();
            }
            return ds;
        }




        /// <summary>
        /// 执行一个 SQL 语句，填充一个数据集，返加受影响行数
        /// </summary>
        public static int ExecuteDataSet(DataSet ds, string s)
        {
            SqlConnection conn = CurrConn;
            int ocs = -1;
            if (conn == null) conn = new SqlConnection(DefaultConnectString);
            else ocs = (int)conn.State;
            if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Open();
            try
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.Transaction = CurrTran;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = s;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        return sda.Fill(ds);
                    }
                }
            }
            finally
            {
                if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Close();
                if (ocs == -1) conn.Dispose();
            }
        }

        /// <summary>
        /// 执行一个 SQL 语句，返回一个数据表
        /// </summary>
        public static DataTable ExecuteDataTable(string s)
        {
            DataSet ds = ExecuteDataSet(s);
            if (ds.Tables.Count > 0) return ds.Tables[0];
            else return null;
        }



        /// <summary>
        /// 执行一个 SQL 语句，填充一个数据表，返加受影响行数
        /// </summary>
        public static int ExecuteDataTable(DataTable dt, string s)
        {
            SqlConnection conn = CurrConn;
            int ocs = -1;
            if (conn == null) conn = new SqlConnection(DefaultConnectString);
            else ocs = (int)conn.State;
            if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Open();
            try
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.Transaction = CurrTran;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = s;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        return sda.Fill(dt);
                    }
                }
            }
            finally
            {
                if (ocs == -1 || ocs == (int)ConnectionState.Closed) conn.Close();
                if (ocs == -1) conn.Dispose();
            }
        }


        /// <summary>
        /// 执行一段 SQL 脚本( for MS SQL200X )
        /// </summary>
        public static void ExecuteSqlScript(string sScript, ScriptExecuteHandler handler)
        {
            string[] statements = System.Text.RegularExpressions.Regex.Split(sScript, "\\sGO\\s", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            using (SqlConnection conn = new SqlConnection(DefaultConnectString))
            {
                conn.Open();
                try
                {
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        foreach (string sql0 in statements)
                        {
                            string sql = sql0.Trim();

                            try
                            {
                                if (sql.ToLower().IndexOf("setuser") >= 0)
                                    continue;

                                if (sql.Length > 0)
                                {
                                    using (SqlCommand cmd = new SqlCommand())
                                    {
                                        cmd.Transaction = trans;
                                        cmd.Connection = conn;
                                        cmd.CommandType = CommandType.Text;
                                        cmd.CommandText = sql.Trim();
                                        object o = cmd.ExecuteScalar();
                                        if (handler != null) handler(new EventSQLScriptArgs(cmd.CommandText, o));
                                    }
                                }
                            }
                            catch (Exception x)
                            {
                                trans.Rollback();
                                throw new Exception(String.Format("ERROR:\n{1}\n\nSTATEMENT:\n{0}", sql, x.Message));
                            }
                        }
                        trans.Commit();
                    }
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }


        /// <summary>
        /// 执行一个 SQL 脚本文件( for MS SQL200X )
        /// </summary>
        public static void ExecuteSqlScriptFile(string fileName, ScriptExecuteHandler handler)
        {

            string sScript = null;
            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader(fileName))
                {
                    sScript = file.ReadToEnd() + Environment.NewLine;
                    file.Close();
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                return;
            }
            catch (Exception x)
            {
                throw new Exception("Failed to read " + fileName, x);
            }

            ExecuteSqlScript(sScript, handler);
        }

        /// <summary>
        /// 返回正在执行的脚本与返回结果的参数的委托
        /// </summary>
        public delegate void ScriptExecuteHandler(EventSQLScriptArgs ea);

        /// <summary>
        /// 正在执行的脚本与返回结果的参数
        /// </summary>
        public class EventSQLScriptArgs : EventArgs
        {
            public EventSQLScriptArgs(string sql, object returns)
            {
                _sql = sql;
                if (returns != null) _returns = returns.ToString();
            }
            private string _sql = "";

            public string Sql
            {
                get { return _sql; }
                set { _sql = value; }
            }
            private string _returns = "";

            public string Returns
            {
                get { return _returns; }
                set { _returns = value; }
            }
        }


        #endregion

        #region DataRow

        /// <summary>
        /// 根据 r 的结构实时生成 Insert 语句并执行。 
        /// 要求 r 所在 Table 的表名 须和 数据表 相符， 字段名和 数据表字段名 相符，具有相同主键和只读属性。
        /// </summary>
        public static int Insert(DataRow r)
        {
            return ExecuteNonQuery(Gen_InsertCommandText(r));
        }
        /// <summary>
        /// 根据 r 的结构实时生成 Insert 语句并执行。 可设置允许和禁止字段名列表。
        /// 要求 r 所在 Table 的表名 须和 数据表 相符， 字段名和 数据表字段名 相符，具有相同主键和只读属性。
        /// </summary>
        public static int Insert(DataRow r, string allowCols, string denyCols)
        {
            return ExecuteNonQuery(Gen_InsertCommandText(r, allowCols, denyCols));
        }


        /// <summary>
        /// 根据 r 的结构实时生成 Insert 语句并执行。 
        /// 要求 r 所在 Table 的表名 须和 数据表 相符， 字段名和 数据表字段名 相符，具有相同主键和只读属性。
        /// </summary>
        public static int Update(DataRow r)
        {
            return ExecuteNonQuery(Gen_UpdateCommandText(r));
        }

        /// <summary>
        /// 根据 r 的结构实时生成 Insert 语句并执行。 可设置允许和禁止字段名列表。
        /// 要求 r 所在 Table 的表名 须和 数据表 相符， 字段名和 数据表字段名 相符，具有相同主键和只读属性。
        /// </summary>
        public static int Update(DataRow r, string allowCols, string denyCols)
        {
            return ExecuteNonQuery(Gen_UpdateCommandText(r, allowCols, denyCols));
        }

        /// <summary>
        /// 根据 r 的结构实时生成 Insert 语句并执行。 
        /// 要求 r 所在 Table 的表名 须和 数据表 相符， 字段名和 数据表字段名 相符，具有相同主键和只读属性。
        /// </summary>
        public static int Delete(DataRow r)
        {
            return ExecuteNonQuery(Gen_DeleteCommandText(r));
        }

        #endregion

        #endregion

        #region SQL 拼接方法

        /// <summary>
        /// 根据一个 DataRow 对象的内容拼接成一个 Insert TSQL 语句。
        /// 要求 r 所在 Table 的表名 须和 数据表 相符， 字段名和 数据表字段名 相符，具有相同主键和只读属性。
        /// </summary>
        public static string Gen_InsertCommandText(DataRow r)
        {
            return Gen_InsertCommandText(r, null, null);
        }

        /// <summary>
        /// 根据一个 DataRow 对象的内容拼接成一个 Insert TSQL 语句。 可设置允许和禁止字段名列表。
        /// 要求 r 所在 Table 的表名 须和 数据表 相符， 字段名和 数据表字段名 相符，具有相同主键和只读属性。
        /// </summary>
        private static string Gen_InsertCommandText(DataRow r, string allowCols, string denyCols)
        {
            DataTable dt = r.Table;
            string tn = dt.TableName;

            List<string> allows;
            if (string.IsNullOrEmpty(allowCols)) allows = new List<string>();
            else allows = new List<string>(allowCols.Split(','));

            List<string> denys;
            if (string.IsNullOrEmpty(denyCols)) denys = new List<string>();
            else denys = new List<string>(denyCols.Split(','));

            StringBuilder sb = new StringBuilder("INSERT [" + tn + @"] (");
            StringBuilder sb2 = new StringBuilder();
            int counter = 0;
            foreach (DataColumn c in dt.Columns)
            {
                string cn = c.ColumnName;
                if (allows.Count > 0 && !allows.Contains(cn)) continue;
                if (denys.Count > 0 && denys.Contains(cn)) continue;
                if (c.ReadOnly) continue;
                string quote = c.DataType == typeof(string) || c.DataType == typeof(Guid) || c.DataType == typeof(DateTime) ? "'" : "";

                if (counter > 0)
                {
                    sb.Append(@", ");
                    sb2.Append(@", ");
                }

                sb.Append("[" + c.ColumnName + "]");
                if (c.AllowDBNull && r.IsNull(c))
                {
                    sb2.Append(@"NULL");
                }
                else if (c.DataType == typeof(byte[]))
                {
                    sb2.Append(GetHexString((byte[])r[c]));
                }
                else if (c.DataType == typeof(bool))
                {
                    sb2.Append((bool)r[c] ? "1" : "0");
                }
                else
                {
                    if (c.DataType == typeof(string))
                    {
                        sb2.Append(quote + EscapeEqual((string)r[c]) + quote);
                    }
                    else
                        sb2.Append(quote + r[c].ToString() + quote);
                }
                counter++;
            }
            sb.Append(@") VALUES (");
            sb.Append(sb2);
            sb.Append(")");

            return sb.ToString();
        }



        /// <summary>
        /// 根据一个 DataRow 对象的内容拼接成一个 Update TSQL 语句。
        /// 要求 r 所在 Table 的表名 须和 数据表 相符， 字段名和 数据表字段名 相符，具有相同主键和只读属性。
        /// </summary>
        public static string Gen_UpdateCommandText(DataRow r)
        {
            return Gen_UpdateCommandText(r, null, null);
        }

        /// <summary>
        /// 根据一个 DataRow 对象的内容拼接成一个 Update TSQL 语句。 可设置允许和禁止字段名列表。
        /// 要求 r 所在 Table 的表名 须和 数据表 相符， 字段名和 数据表字段名 相符，具有相同主键和只读属性。
        /// </summary>
        public static string Gen_UpdateCommandText(DataRow r, string allowCols, string denyCols)
        {
            DataTable dt = r.Table;
            string tn = dt.TableName;

            List<string> allows;
            if (string.IsNullOrEmpty(allowCols)) allows = new List<string>();
            else allows = new List<string>(allowCols.Split(','));

            List<string> denys;
            if (string.IsNullOrEmpty(denyCols)) denys = new List<string>();
            else denys = new List<string>(denyCols.Split(','));

            StringBuilder sb = new StringBuilder("UPDATE [" + tn + @"] SET ");
            StringBuilder sb2 = new StringBuilder();
            int counter = 0;
            foreach (DataColumn c in dt.Columns)
            {
                string cn = c.ColumnName;
                if (allows.Count > 0 && !allows.Contains(cn)) continue;
                if (denys.Count > 0 && denys.Contains(cn)) continue;
                if (c.ReadOnly) continue;
                string quote = c.DataType == typeof(string) || c.DataType == typeof(Guid) || c.DataType == typeof(DateTime) ? "'" : "";

                if (counter > 0)
                {
                    sb.Append(@", ");
                }

                sb.Append("[" + c.ColumnName + "] = ");
                if (c.AllowDBNull && r.IsNull(c))
                {
                    sb.Append(@"NULL");
                }
                else if (c.DataType == typeof(byte[]))
                {
                    sb.Append(GetHexString((byte[])r[c]));
                }
                else if (c.DataType == typeof(bool))
                {
                    sb.Append((bool)r[c] ? "1" : "0");
                }
                else
                {
                    if (c.DataType == typeof(string))
                    {
                        sb.Append(quote + EscapeEqual((string)r[c]) + quote);
                    }
                    else
                        sb.Append(quote + r[c].ToString() + quote);

                }
                counter++;
            }
            sb.Append(@" WHERE ");
            DataRowVersion version = r.HasVersion(DataRowVersion.Original) ? DataRowVersion.Original : DataRowVersion.Current;
            for (int i = 0; i < dt.PrimaryKey.Length; i++)
            {
                DataColumn c = dt.PrimaryKey[i];
                string quote = c.DataType == typeof(string) || c.DataType == typeof(Guid) || c.DataType == typeof(DateTime) ? "'" : "";

                if (i > 0) sb.Append(@" AND ");
                sb.Append("[" + c.ColumnName + "] = ");

                if (c.DataType == typeof(bool))
                {
                    sb.Append((bool)r[c, version] ? "1" : "0");
                }
                else
                {
                    if (c.DataType == typeof(string))
                    {
                        sb.Append(quote + EscapeEqual((string)r[c, version]) + quote);
                    }
                    else
                        sb.Append(quote + r[c, version].ToString() + quote);
                }
            }

            return sb.ToString();
        }


        /// <summary>
        /// 根据一个 DataRow 对象的内容拼接成一个 Delete TSQL 语句。
        /// 要求 r 所在 Table 的表名 须和 数据表 相符， 字段名和 数据表字段名 相符，具有相同主键。
        /// 注意：该方法现在只支持架构名不重复的表
        /// </summary>
        public static string Gen_DeleteCommandText(DataRow r)
        {
            DataTable dt = r.Table;
            string tn = dt.TableName;
            DataRowVersion version = r.HasVersion(DataRowVersion.Original) ? DataRowVersion.Original : DataRowVersion.Current;
            StringBuilder sb = new StringBuilder("DELETE FROM [" + tn + @"] WHERE ");
            for (int i = 0; i < dt.PrimaryKey.Length; i++)
            {
                DataColumn c = dt.PrimaryKey[i];
                string quote = c.DataType == typeof(string) || c.DataType == typeof(Guid) || c.DataType == typeof(DateTime) ? "'" : "";

                if (i > 0) sb.Append(@" AND ");
                sb.Append("[" + c.ColumnName + "] = ");

                if (c.DataType == typeof(bool))
                {
                    sb.Append((bool)r[c, version] ? "1" : "0");
                }
                else
                {
                    if (c.DataType == typeof(string))
                    {
                        sb.Append(quote + EscapeEqual((string)r[c, version]) + quote);
                    }
                    else
                        sb.Append(quote + r[c, version].ToString() + quote);
                }
            }

            return sb.ToString();
        }


        /// <summary>
        /// 返回多关键字模糊查找的 SQL 拼接语句。SQL 语句运行结果为主键序列
        /// </summary>
        /// <param name="keywords">关键字序列</param>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKey">主键字段名</param>
        /// <param name="searchForCol">要找的字段名</param>
        /// <param name="orderByCol">排序字段名（默认情况下认为会传入时间字段并倒序排列）</param>
        /// <returns>用于该查询的 TSQL 语句</returns>
        public static string Gen_KeywordsSearch(IList<string> keywords, string tableName, string primaryKey, string searchForCol, string orderByCol, string whereExpression)
        {
            return Gen_KeywordsSearch(keywords, tableName, new string[] { primaryKey }, new string[] { searchForCol }, new KeyValuePair<string, bool>[] { new KeyValuePair<string, bool>(orderByCol, true) }, null, whereExpression);
        }

        /// <summary>
        /// 返回多关键字模糊查找的 SQL 拼接语句。
        /// </summary>
        /// <param name="keywords">关键字序列</param>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKey">主键字段名序列</param>
        /// <param name="searchForCol">要找的字段名序列</param>
        /// <param name="orderByCol">排序字段名序列</param>
        /// <param name="returnCols"> TSQL 执行结果包含的字段（无则只返回主键） </param>
        /// <param name="where"> 欲传入的 TSQL 判断表达式字串 （主键字段须带 a. 开头） </param>
        /// <returns>用于该查询的 TSQL 语句</returns>
        public static string Gen_KeywordsSearch(IList<string> keywords, string tableName, IList<string> primaryKeys, IList<string> searchForCols, IList<KeyValuePair<string, bool>> orderByCols, IList<string> returnCols, string whereExpression)
        {
            List<KeyValuePair<string, IList<string>>> colKeywords = new List<KeyValuePair<string, IList<string>>>();
            foreach (string s in searchForCols)
            {
                colKeywords.Add(new KeyValuePair<string, IList<string>>(s, keywords));
            }
            return Gen_KeywordsSearch(tableName, primaryKeys, colKeywords.ToArray(), orderByCols, returnCols, whereExpression);
        }




        /// <summary>
        /// 返回多关键字多字段对应模糊查找的 SQL 拼接语句。
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKey">主键字段名序列</param>
        /// <param name="colKeywords">字段，关键字序列对应字典</param>
        /// <param name="orderByCol">排序字段名序列</param>
        /// <param name="returnCols"> TSQL 执行结果包含的字段（无则只返回主键） </param>
        /// <param name="where"> 欲传入的 TSQL 判断表达式字串 （主键字段须带 a. 开头） </param>
        /// <returns>用于该查询的 TSQL 语句</returns>
        public static string Gen_KeywordsSearch(string tableName, IList<string> primaryKeys, IList<KeyValuePair<string, IList<string>>> colKeywords, IList<KeyValuePair<string, bool>> orderByCols, IList<string> returnCols, string whereExpression)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT ");
            if (returnCols == null || returnCols.Count == 0)
            {
                for (int i = 0; i < primaryKeys.Count; i++)
                {
                    if (i > 0) sb.Append(@", ");
                    sb.Append(@"a.[" + primaryKeys[i] + @"]");
                }
            }
            else
            {
                for (int i = 0; i < returnCols.Count; i++)
                {
                    if (i > 0) sb.Append(@", ");
                    sb.Append(@"a.[" + returnCols[i] + @"]");
                }
            }
            sb.Append(@" FROM [" + tableName + @"] a
JOIN (
	SELECT ");
            for (int i = 0; i < primaryKeys.Count; i++)
            {
                if (i > 0) sb.Append(@", ");
                sb.Append(@"[" + primaryKeys[i] + @"]");
            }
            sb.Append(@", COUNT(*) AS 'numof' FROM (");
            int sn = 0;	//子查询计数器。用来解决 union all 的添加问题
            for (int j = 0; j < colKeywords.Count; j++)
            {
                for (int k = 0; k < colKeywords[j].Value.Count; k++)
                {
                    if (sn++ > 0) sb.Append(@"
		UNION ALL");
                    sb.Append(@"
		SELECT ");
                    for (int i = 0; i < primaryKeys.Count; i++)
                    {
                        if (i > 0) sb.Append(@", ");
                        sb.Append(@"[" + primaryKeys[i] + @"]");
                    }
                    sb.Append(@" FROM [" + tableName + @"] WHERE [" + colKeywords[j].Key + @"] LIKE '%" + EscapeLike(colKeywords[j].Value[k]) + @"%'");
                }
            }
            sb.Append(@"
	) a
	GROUP BY ");
            for (int i = 0; i < primaryKeys.Count; i++)
            {
                if (i > 0) sb.Append(@", ");
                sb.Append(@"[" + primaryKeys[i] + @"]");
            }
            sb.Append(@"
) b ON ");
            for (int i = 0; i < primaryKeys.Count; i++)
            {
                if (i > 0) sb.Append(@", ");
                sb.Append(@"a.[" + primaryKeys[i] + @"] = b.[" + primaryKeys[i] + @"]");
            }
            if (!string.IsNullOrEmpty(whereExpression)) sb.Append(@"
WHERE " + whereExpression);
            sb.Append(@"
ORDER BY b.numof DESC");
            if (orderByCols != null)
                foreach (KeyValuePair<string, bool> kvp in orderByCols)
                {
                    sb.Append(@", a.[" + kvp.Key + @"]" + (kvp.Value ? " DESC" : ""));
                }
            return sb.ToString();
        }


        #endregion

        #region 表达式相关

        /// <summary>
        /// 表达式运算符
        /// </summary>
        public enum Operators : int
        {
            Custom = 0,
            Equal,
            LessThan,
            LessEqual,
            LargerThan,
            LargerEqual,
            NotEqual,
            Like,
            CustomLike,
            NotLike,
            CustomNotLike,
            In,
            NotIn
        }

        /// <summary>
        /// 获取运算符的 SQL 写法
        /// </summary>
        public static string GetOperater(Operators op)
        {
            switch (op)
            {
                case Operators.Custom: return "";
                case Operators.Equal: return "=";
                case Operators.LessThan: return "<";
                case Operators.LessEqual: return "<=";
                case Operators.LargerThan: return ">";
                case Operators.LargerEqual: return ">=";
                case Operators.NotEqual: return "<>";
                case Operators.Like:
                case Operators.CustomLike: return "LIKE";
                case Operators.NotLike:
                case Operators.CustomNotLike: return "NOT LIKE";
                case Operators.In: return "IN";
                case Operators.NotIn: return "NOT IN";
            }
            return "";
        }

        /// <summary>
        /// 将一个集合拼接为一个逗号分隔的字串并返回
        /// </summary>
        public static string Combine<T>(IEnumerable<T> list, string separator, string prefix, string suffix)
        {
            if (list == null) return "";
            if (prefix == null) prefix = "";
            if (suffix == null) suffix = "";
            StringBuilder sb = new StringBuilder();
            foreach (T item in list)
            {
                if (item.ToString().Length == 0) continue;
                if (sb.Length > 0) sb.Append(separator);

                if (typeof(T) == typeof(object))
                {
                    if (item.GetType() == typeof(string) || item.GetType() == typeof(Guid) || item.GetType() == typeof(Guid?) || item.GetType() == typeof(DateTime) || item.GetType() == typeof(DateTime?))
                    {
                        sb.Append("'" + item.ToString().Replace("'", "''") + "'");
                    }
                    else sb.Append(item.ToString().Replace("'", "''"));
                }
                else sb.Append(prefix + item.ToString().Replace("'", "''") + suffix);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将一个集合拼接为一个逗号分隔的字串并返回
        /// </summary>
        public static string Combine<T>(IEnumerable<T> list)
        {
            if (typeof(T) == typeof(string) || typeof(T) == typeof(Guid) || typeof(T) == typeof(Guid?) || typeof(T) == typeof(DateTime) || typeof(T) == typeof(DateTime?)) return Combine<T>(list, ",", "'", "'");
            return Combine<T>(list, ",", "", "");
        }

        #endregion
    }
}
