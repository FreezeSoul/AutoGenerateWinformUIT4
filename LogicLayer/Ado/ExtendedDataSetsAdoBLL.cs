using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer.Ado;

namespace BusinessLogicLayer.Ado
{

	public class ExtendedDataSetsAdoBLL
    {
        
		private ExtendedDataSetsAdoDAL _extendeddatasetsAdoDAL = new ExtendedDataSetsAdoDAL();
        public ExtendedDataSetsAdoBLL()
        {
        }

        public DataTable GetDataTable()
        {
			return GetDataTable(null, null, null, null);
        }

        public DataTable GetDataTable(Dictionary<string,object> defaultParams, Dictionary<string, object> searchParams)
        {
			return GetDataTable(defaultParams,searchParams,null,null);
        }

        public DataTable GetDataTable(Dictionary<string,object> defaultParams, Dictionary<string, object> searchParams, int? rowStart, int? rowEnd)
        {
			var whereStr = new StringBuilder();
			whereStr.Append(@" Where 1=1 ");
			var paramsTemp = new Dictionary<string,object>();
			#region 默认查询条件
								if(defaultParams.ContainsKey("ID"))
					{
            								whereStr.Append(@" and ID=@ID ");
						paramsTemp.Add("@ID",defaultParams["ID"]);
										}
            					if(defaultParams.ContainsKey("LinkID"))
					{
            								whereStr.Append(@" and LinkID=@LinkID ");
						paramsTemp.Add("@LinkID",defaultParams["LinkID"]);
										}
            					if(defaultParams.ContainsKey("Name"))
					{
            								whereStr.Append(@" and Name=@Name ");
						paramsTemp.Add("@Name",defaultParams["Name"]);
										}
            					if(defaultParams.ContainsKey("ItemID"))
					{
            								whereStr.Append(@" and ItemID=@ItemID ");
						paramsTemp.Add("@ItemID",defaultParams["ItemID"]);
										}
            			#endregion
			#region 自定义查询
				//whereStr.Append(@"columnName=@columnName");
				//paramsTemp.Add("columnName",searchParams["columnName"]);
			#endregion
			if(rowStart == null || rowEnd == null)
			{
            	return _extendeddatasetsAdoDAL.GetPageDataTable(whereStr.ToString(),"ID",paramsTemp);
			}
			else
			{
				return _extendeddatasetsAdoDAL.GetPageDataTable(whereStr.ToString(),"ID",paramsTemp,(int)rowStart,(int)rowEnd);
			}
        }
		
		public int GetDataTableCount(Dictionary<string,object> defaultParams, Dictionary<string, object> searchParams)
		{
			var whereStr = new StringBuilder();
			whereStr.Append(@" Where 1=1 ");
			var paramsTemp = new Dictionary<string,object>();
			#region 默认查询条件
								if(defaultParams.ContainsKey("ID"))
					{
            								whereStr.Append(@" and ID=@ID ");
						paramsTemp.Add("@ID",defaultParams["ID"]);
										}
            					if(defaultParams.ContainsKey("LinkID"))
					{
            								whereStr.Append(@" and LinkID=@LinkID ");
						paramsTemp.Add("@LinkID",defaultParams["LinkID"]);
										}
            					if(defaultParams.ContainsKey("Name"))
					{
            								whereStr.Append(@" and Name=@Name ");
						paramsTemp.Add("@Name",defaultParams["Name"]);
										}
            					if(defaultParams.ContainsKey("ItemID"))
					{
            								whereStr.Append(@" and ItemID=@ItemID ");
						paramsTemp.Add("@ItemID",defaultParams["ItemID"]);
										}
            			#endregion
			#region 自定义查询
				//whereStr.Append(@"columnName=@columnName");
				//paramsTemp.Add("columnName",searchParams["columnName"]);
			#endregion
			return _extendeddatasetsAdoDAL.GetDataTableCount(whereStr.ToString(),paramsTemp);
		}
		
}
}
