using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer.Ado;

namespace BusinessLogicLayer.Ado
{

	public class ExtendedDataSourcesAdoBLL
    {
        
		private ExtendedDataSourcesAdoDAL _extendeddatasourcesAdoDAL = new ExtendedDataSourcesAdoDAL();
        public ExtendedDataSourcesAdoBLL()
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
								if(defaultParams.ContainsKey("DSID"))
					{
            								whereStr.Append(@" and DSID=@DSID ");
						paramsTemp.Add("@DSID",defaultParams["DSID"]);
										}
            					if(defaultParams.ContainsKey("ItemID"))
					{
            								whereStr.Append(@" and ItemID=@ItemID ");
						paramsTemp.Add("@ItemID",defaultParams["ItemID"]);
										}
            					if(defaultParams.ContainsKey("SubscriptionID"))
					{
            								whereStr.Append(@" and SubscriptionID=@SubscriptionID ");
						paramsTemp.Add("@SubscriptionID",defaultParams["SubscriptionID"]);
										}
            					if(defaultParams.ContainsKey("Name"))
					{
            								whereStr.Append(@" and Name=@Name ");
						paramsTemp.Add("@Name",defaultParams["Name"]);
										}
            					if(defaultParams.ContainsKey("Extension"))
					{
            								whereStr.Append(@" and Extension=@Extension ");
						paramsTemp.Add("@Extension",defaultParams["Extension"]);
										}
            					if(defaultParams.ContainsKey("Link"))
					{
            								whereStr.Append(@" and Link=@Link ");
						paramsTemp.Add("@Link",defaultParams["Link"]);
										}
            					if(defaultParams.ContainsKey("CredentialRetrieval"))
					{
            								whereStr.Append(@" and CredentialRetrieval=@CredentialRetrieval ");
						paramsTemp.Add("@CredentialRetrieval",defaultParams["CredentialRetrieval"]);
										}
            					if(defaultParams.ContainsKey("Prompt"))
					{
            								whereStr.Append(@" and Prompt=@Prompt ");
						paramsTemp.Add("@Prompt",defaultParams["Prompt"]);
										}
            					if(defaultParams.ContainsKey("ConnectionString"))
					{
            								whereStr.Append(@" and ConnectionString=@ConnectionString ");
						paramsTemp.Add("@ConnectionString",defaultParams["ConnectionString"]);
										}
            					if(defaultParams.ContainsKey("OriginalConnectionString"))
					{
            								whereStr.Append(@" and OriginalConnectionString=@OriginalConnectionString ");
						paramsTemp.Add("@OriginalConnectionString",defaultParams["OriginalConnectionString"]);
										}
            					if(defaultParams.ContainsKey("OriginalConnectStringExpressionBased"))
					{
            								whereStr.Append(@" and OriginalConnectStringExpressionBased=@OriginalConnectStringExpressionBased ");
						paramsTemp.Add("@OriginalConnectStringExpressionBased",defaultParams["OriginalConnectStringExpressionBased"]);
										}
            					if(defaultParams.ContainsKey("UserName"))
					{
            								whereStr.Append(@" and UserName=@UserName ");
						paramsTemp.Add("@UserName",defaultParams["UserName"]);
										}
            					if(defaultParams.ContainsKey("Password"))
					{
            								whereStr.Append(@" and Password=@Password ");
						paramsTemp.Add("@Password",defaultParams["Password"]);
										}
            					if(defaultParams.ContainsKey("Flags"))
					{
            								whereStr.Append(@" and Flags=@Flags ");
						paramsTemp.Add("@Flags",defaultParams["Flags"]);
										}
            					if(defaultParams.ContainsKey("Version"))
					{
            								whereStr.Append(@" and Version=@Version ");
						paramsTemp.Add("@Version",defaultParams["Version"]);
										}
            			#endregion
			#region 自定义查询
				//whereStr.Append(@"columnName=@columnName");
				//paramsTemp.Add("columnName",searchParams["columnName"]);
			#endregion
			if(rowStart == null || rowEnd == null)
			{
            	return _extendeddatasourcesAdoDAL.GetPageDataTable(whereStr.ToString(),"DSID",paramsTemp);
			}
			else
			{
				return _extendeddatasourcesAdoDAL.GetPageDataTable(whereStr.ToString(),"DSID",paramsTemp,(int)rowStart,(int)rowEnd);
			}
        }
		
		public int GetDataTableCount(Dictionary<string,object> defaultParams, Dictionary<string, object> searchParams)
		{
			var whereStr = new StringBuilder();
			whereStr.Append(@" Where 1=1 ");
			var paramsTemp = new Dictionary<string,object>();
			#region 默认查询条件
								if(defaultParams.ContainsKey("DSID"))
					{
            								whereStr.Append(@" and DSID=@DSID ");
						paramsTemp.Add("@DSID",defaultParams["DSID"]);
										}
            					if(defaultParams.ContainsKey("ItemID"))
					{
            								whereStr.Append(@" and ItemID=@ItemID ");
						paramsTemp.Add("@ItemID",defaultParams["ItemID"]);
										}
            					if(defaultParams.ContainsKey("SubscriptionID"))
					{
            								whereStr.Append(@" and SubscriptionID=@SubscriptionID ");
						paramsTemp.Add("@SubscriptionID",defaultParams["SubscriptionID"]);
										}
            					if(defaultParams.ContainsKey("Name"))
					{
            								whereStr.Append(@" and Name=@Name ");
						paramsTemp.Add("@Name",defaultParams["Name"]);
										}
            					if(defaultParams.ContainsKey("Extension"))
					{
            								whereStr.Append(@" and Extension=@Extension ");
						paramsTemp.Add("@Extension",defaultParams["Extension"]);
										}
            					if(defaultParams.ContainsKey("Link"))
					{
            								whereStr.Append(@" and Link=@Link ");
						paramsTemp.Add("@Link",defaultParams["Link"]);
										}
            					if(defaultParams.ContainsKey("CredentialRetrieval"))
					{
            								whereStr.Append(@" and CredentialRetrieval=@CredentialRetrieval ");
						paramsTemp.Add("@CredentialRetrieval",defaultParams["CredentialRetrieval"]);
										}
            					if(defaultParams.ContainsKey("Prompt"))
					{
            								whereStr.Append(@" and Prompt=@Prompt ");
						paramsTemp.Add("@Prompt",defaultParams["Prompt"]);
										}
            					if(defaultParams.ContainsKey("ConnectionString"))
					{
            								whereStr.Append(@" and ConnectionString=@ConnectionString ");
						paramsTemp.Add("@ConnectionString",defaultParams["ConnectionString"]);
										}
            					if(defaultParams.ContainsKey("OriginalConnectionString"))
					{
            								whereStr.Append(@" and OriginalConnectionString=@OriginalConnectionString ");
						paramsTemp.Add("@OriginalConnectionString",defaultParams["OriginalConnectionString"]);
										}
            					if(defaultParams.ContainsKey("OriginalConnectStringExpressionBased"))
					{
            								whereStr.Append(@" and OriginalConnectStringExpressionBased=@OriginalConnectStringExpressionBased ");
						paramsTemp.Add("@OriginalConnectStringExpressionBased",defaultParams["OriginalConnectStringExpressionBased"]);
										}
            					if(defaultParams.ContainsKey("UserName"))
					{
            								whereStr.Append(@" and UserName=@UserName ");
						paramsTemp.Add("@UserName",defaultParams["UserName"]);
										}
            					if(defaultParams.ContainsKey("Password"))
					{
            								whereStr.Append(@" and Password=@Password ");
						paramsTemp.Add("@Password",defaultParams["Password"]);
										}
            					if(defaultParams.ContainsKey("Flags"))
					{
            								whereStr.Append(@" and Flags=@Flags ");
						paramsTemp.Add("@Flags",defaultParams["Flags"]);
										}
            					if(defaultParams.ContainsKey("Version"))
					{
            								whereStr.Append(@" and Version=@Version ");
						paramsTemp.Add("@Version",defaultParams["Version"]);
										}
            			#endregion
			#region 自定义查询
				//whereStr.Append(@"columnName=@columnName");
				//paramsTemp.Add("columnName",searchParams["columnName"]);
			#endregion
			return _extendeddatasourcesAdoDAL.GetDataTableCount(whereStr.ToString(),paramsTemp);
		}
		
}
}
