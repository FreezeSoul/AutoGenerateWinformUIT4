using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer.Ado;

namespace BusinessLogicLayer.Ado
{

	public class ExecutionLog2AdoBLL
    {
        
		private ExecutionLog2AdoDAL _executionlog2AdoDAL = new ExecutionLog2AdoDAL();
        public ExecutionLog2AdoBLL()
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
								if(defaultParams.ContainsKey("InstanceName"))
					{
            								whereStr.Append(@" and InstanceName=@InstanceName ");
						paramsTemp.Add("@InstanceName",defaultParams["InstanceName"]);
										}
            					if(defaultParams.ContainsKey("ReportPath"))
					{
            								whereStr.Append(@" and ReportPath=@ReportPath ");
						paramsTemp.Add("@ReportPath",defaultParams["ReportPath"]);
										}
            					if(defaultParams.ContainsKey("UserName"))
					{
            								whereStr.Append(@" and UserName=@UserName ");
						paramsTemp.Add("@UserName",defaultParams["UserName"]);
										}
            					if(defaultParams.ContainsKey("ExecutionId"))
					{
            								whereStr.Append(@" and ExecutionId=@ExecutionId ");
						paramsTemp.Add("@ExecutionId",defaultParams["ExecutionId"]);
										}
            					if(defaultParams.ContainsKey("RequestType"))
					{
            								whereStr.Append(@" and RequestType=@RequestType ");
						paramsTemp.Add("@RequestType",defaultParams["RequestType"]);
										}
            					if(defaultParams.ContainsKey("Format"))
					{
            								whereStr.Append(@" and Format=@Format ");
						paramsTemp.Add("@Format",defaultParams["Format"]);
										}
            					if(defaultParams.ContainsKey("Parameters"))
					{
            								whereStr.Append(@" and Parameters=@Parameters ");
						paramsTemp.Add("@Parameters",defaultParams["Parameters"]);
										}
            					if(defaultParams.ContainsKey("ReportAction"))
					{
            								whereStr.Append(@" and ReportAction=@ReportAction ");
						paramsTemp.Add("@ReportAction",defaultParams["ReportAction"]);
										}
            					if(defaultParams.ContainsKey("TimeStart"))
					{
            								var startTime = DateTime.Parse(((DateTime)defaultParams["TimeStart"]).ToString("yyyy-MM-dd"));
						var endTime = DateTime.Parse(((DateTime)defaultParams["TimeStart"]).AddDays(1).ToString("yyyy-MM-dd"));
						whereStr.Append(@" and TimeStart>=@TimeStart_Start and TimeStart<@TimeStart_End ");
						paramsTemp.Add("@TimeStart_Start",startTime);
						paramsTemp.Add("@TimeStart_End",endTime);
										}
            					if(defaultParams.ContainsKey("TimeEnd"))
					{
            								var startTime = DateTime.Parse(((DateTime)defaultParams["TimeEnd"]).ToString("yyyy-MM-dd"));
						var endTime = DateTime.Parse(((DateTime)defaultParams["TimeEnd"]).AddDays(1).ToString("yyyy-MM-dd"));
						whereStr.Append(@" and TimeEnd>=@TimeEnd_Start and TimeEnd<@TimeEnd_End ");
						paramsTemp.Add("@TimeEnd_Start",startTime);
						paramsTemp.Add("@TimeEnd_End",endTime);
										}
            					if(defaultParams.ContainsKey("TimeDataRetrieval"))
					{
            								whereStr.Append(@" and TimeDataRetrieval=@TimeDataRetrieval ");
						paramsTemp.Add("@TimeDataRetrieval",defaultParams["TimeDataRetrieval"]);
										}
            					if(defaultParams.ContainsKey("TimeProcessing"))
					{
            								whereStr.Append(@" and TimeProcessing=@TimeProcessing ");
						paramsTemp.Add("@TimeProcessing",defaultParams["TimeProcessing"]);
										}
            					if(defaultParams.ContainsKey("TimeRendering"))
					{
            								whereStr.Append(@" and TimeRendering=@TimeRendering ");
						paramsTemp.Add("@TimeRendering",defaultParams["TimeRendering"]);
										}
            					if(defaultParams.ContainsKey("Source"))
					{
            								whereStr.Append(@" and Source=@Source ");
						paramsTemp.Add("@Source",defaultParams["Source"]);
										}
            					if(defaultParams.ContainsKey("Status"))
					{
            								whereStr.Append(@" and Status=@Status ");
						paramsTemp.Add("@Status",defaultParams["Status"]);
										}
            					if(defaultParams.ContainsKey("ByteCount"))
					{
            								whereStr.Append(@" and ByteCount=@ByteCount ");
						paramsTemp.Add("@ByteCount",defaultParams["ByteCount"]);
										}
            					if(defaultParams.ContainsKey("RowCount"))
					{
            								whereStr.Append(@" and RowCount=@RowCount ");
						paramsTemp.Add("@RowCount",defaultParams["RowCount"]);
										}
            					if(defaultParams.ContainsKey("AdditionalInfo"))
					{
            								whereStr.Append(@" and AdditionalInfo=@AdditionalInfo ");
						paramsTemp.Add("@AdditionalInfo",defaultParams["AdditionalInfo"]);
										}
            			#endregion
			#region 自定义查询
				//whereStr.Append(@"columnName=@columnName");
				//paramsTemp.Add("columnName",searchParams["columnName"]);
			#endregion
			if(rowStart == null || rowEnd == null)
			{
            	return _executionlog2AdoDAL.GetPageDataTable(whereStr.ToString(),"InstanceName",paramsTemp);
			}
			else
			{
				return _executionlog2AdoDAL.GetPageDataTable(whereStr.ToString(),"InstanceName",paramsTemp,(int)rowStart,(int)rowEnd);
			}
        }
		
		public int GetDataTableCount(Dictionary<string,object> defaultParams, Dictionary<string, object> searchParams)
		{
			var whereStr = new StringBuilder();
			whereStr.Append(@" Where 1=1 ");
			var paramsTemp = new Dictionary<string,object>();
			#region 默认查询条件
								if(defaultParams.ContainsKey("InstanceName"))
					{
            								whereStr.Append(@" and InstanceName=@InstanceName ");
						paramsTemp.Add("@InstanceName",defaultParams["InstanceName"]);
										}
            					if(defaultParams.ContainsKey("ReportPath"))
					{
            								whereStr.Append(@" and ReportPath=@ReportPath ");
						paramsTemp.Add("@ReportPath",defaultParams["ReportPath"]);
										}
            					if(defaultParams.ContainsKey("UserName"))
					{
            								whereStr.Append(@" and UserName=@UserName ");
						paramsTemp.Add("@UserName",defaultParams["UserName"]);
										}
            					if(defaultParams.ContainsKey("ExecutionId"))
					{
            								whereStr.Append(@" and ExecutionId=@ExecutionId ");
						paramsTemp.Add("@ExecutionId",defaultParams["ExecutionId"]);
										}
            					if(defaultParams.ContainsKey("RequestType"))
					{
            								whereStr.Append(@" and RequestType=@RequestType ");
						paramsTemp.Add("@RequestType",defaultParams["RequestType"]);
										}
            					if(defaultParams.ContainsKey("Format"))
					{
            								whereStr.Append(@" and Format=@Format ");
						paramsTemp.Add("@Format",defaultParams["Format"]);
										}
            					if(defaultParams.ContainsKey("Parameters"))
					{
            								whereStr.Append(@" and Parameters=@Parameters ");
						paramsTemp.Add("@Parameters",defaultParams["Parameters"]);
										}
            					if(defaultParams.ContainsKey("ReportAction"))
					{
            								whereStr.Append(@" and ReportAction=@ReportAction ");
						paramsTemp.Add("@ReportAction",defaultParams["ReportAction"]);
										}
            					if(defaultParams.ContainsKey("TimeStart"))
					{
            								var startTime = DateTime.Parse(((DateTime)defaultParams["TimeStart"]).ToString("yyyy-MM-dd"));
						var endTime = DateTime.Parse(((DateTime)defaultParams["TimeStart"]).AddDays(1).ToString("yyyy-MM-dd"));
						whereStr.Append(@" and TimeStart>=@TimeStart_Start and TimeStart<@TimeStart_End ");
						paramsTemp.Add("@TimeStart_Start",startTime);
						paramsTemp.Add("@TimeStart_End",endTime);
										}
            					if(defaultParams.ContainsKey("TimeEnd"))
					{
            								var startTime = DateTime.Parse(((DateTime)defaultParams["TimeEnd"]).ToString("yyyy-MM-dd"));
						var endTime = DateTime.Parse(((DateTime)defaultParams["TimeEnd"]).AddDays(1).ToString("yyyy-MM-dd"));
						whereStr.Append(@" and TimeEnd>=@TimeEnd_Start and TimeEnd<@TimeEnd_End ");
						paramsTemp.Add("@TimeEnd_Start",startTime);
						paramsTemp.Add("@TimeEnd_End",endTime);
										}
            					if(defaultParams.ContainsKey("TimeDataRetrieval"))
					{
            								whereStr.Append(@" and TimeDataRetrieval=@TimeDataRetrieval ");
						paramsTemp.Add("@TimeDataRetrieval",defaultParams["TimeDataRetrieval"]);
										}
            					if(defaultParams.ContainsKey("TimeProcessing"))
					{
            								whereStr.Append(@" and TimeProcessing=@TimeProcessing ");
						paramsTemp.Add("@TimeProcessing",defaultParams["TimeProcessing"]);
										}
            					if(defaultParams.ContainsKey("TimeRendering"))
					{
            								whereStr.Append(@" and TimeRendering=@TimeRendering ");
						paramsTemp.Add("@TimeRendering",defaultParams["TimeRendering"]);
										}
            					if(defaultParams.ContainsKey("Source"))
					{
            								whereStr.Append(@" and Source=@Source ");
						paramsTemp.Add("@Source",defaultParams["Source"]);
										}
            					if(defaultParams.ContainsKey("Status"))
					{
            								whereStr.Append(@" and Status=@Status ");
						paramsTemp.Add("@Status",defaultParams["Status"]);
										}
            					if(defaultParams.ContainsKey("ByteCount"))
					{
            								whereStr.Append(@" and ByteCount=@ByteCount ");
						paramsTemp.Add("@ByteCount",defaultParams["ByteCount"]);
										}
            					if(defaultParams.ContainsKey("RowCount"))
					{
            								whereStr.Append(@" and RowCount=@RowCount ");
						paramsTemp.Add("@RowCount",defaultParams["RowCount"]);
										}
            					if(defaultParams.ContainsKey("AdditionalInfo"))
					{
            								whereStr.Append(@" and AdditionalInfo=@AdditionalInfo ");
						paramsTemp.Add("@AdditionalInfo",defaultParams["AdditionalInfo"]);
										}
            			#endregion
			#region 自定义查询
				//whereStr.Append(@"columnName=@columnName");
				//paramsTemp.Add("columnName",searchParams["columnName"]);
			#endregion
			return _executionlog2AdoDAL.GetDataTableCount(whereStr.ToString(),paramsTemp);
		}
		
}
}
