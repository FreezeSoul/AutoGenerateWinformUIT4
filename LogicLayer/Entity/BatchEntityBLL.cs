using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModelLayer;

namespace BusinessLogicLayer.Entity
{

	public class BatchEntityBLL
    {
        private MWTSEntities _mwtsEntities;
        public MWTSEntities MwtsEntities
        {
            get
            {
                if (_mwtsEntities == null)
                    _mwtsEntities = new MWTSEntities();
                return _mwtsEntities;
            }
        }

        public BatchEntityBLL()
        {
        }

        public IQueryable<Batch> GetList()
        {
            return MwtsEntities.Batch.AsQueryable();//.Include("子对象").AsQueryable();
        }

        public IQueryable<Batch> GetList(List<string> searchKeys, Batch model, Dictionary<string, object> searchParams)
        {
            return GetList(searchKeys, model, searchParams, null, null);
        }

        public IQueryable<Batch> GetList(List<string> searchKeys, Batch model, Dictionary<string, object> searchParams, int? pageIndex, int? rowCount)
        {
            //var modelQuery = MwtsEntities.Batchs.AsQueryable();
            var modelQuery = new MWTSEntities().Batch.AsQueryable();//.Include("子对象").AsQueryable();
			
			#region 默认查询条件
			            if (searchKeys.Contains("BatchID"))
            {
				                modelQuery = modelQuery.Where(item => item.BatchID == model.BatchID);
									}
                        if (searchKeys.Contains("AddedOn"))
            {
														var startTime = DateTime.Parse(model.AddedOn.ToString("yyyy-MM-dd"));
				var endTime = DateTime.Parse(model.AddedOn.AddDays(1).ToString("yyyy-MM-dd"));
                modelQuery = modelQuery.Where(item => item.AddedOn >= startTime && item.AddedOn < endTime);
															}
                        if (searchKeys.Contains("Action"))
            {
				                modelQuery = modelQuery.Where(item => item.Action == model.Action);
									}
                        if (searchKeys.Contains("Item"))
            {
				                modelQuery = modelQuery.Where(item => item.Item == model.Item);
									}
                        if (searchKeys.Contains("Parent"))
            {
				                modelQuery = modelQuery.Where(item => item.Parent == model.Parent);
									}
                        if (searchKeys.Contains("Param"))
            {
				                modelQuery = modelQuery.Where(item => item.Param == model.Param);
									}
                        if (searchKeys.Contains("BoolParam"))
            {
				                modelQuery = modelQuery.Where(item => item.BoolParam == model.BoolParam);
									}
                        if (searchKeys.Contains("Content"))
            {
				                modelQuery = modelQuery.Where(item => item.Content == model.Content);
									}
                        if (searchKeys.Contains("Properties"))
            {
				                modelQuery = modelQuery.Where(item => item.Properties == model.Properties);
									}
            			#endregion

			#region 自定义查询条件
            if (searchParams.Count > 0)
            {
                //扩展查询条件
            }
			#endregion
			
			#region 翻页逻辑
            if (pageIndex != null && rowCount != null)
            {
			  int skip = ((int)pageIndex - 1) * (int)rowCount;
			  int take = (int)rowCount;
              modelQuery =  modelQuery.OrderBy(item => item.BatchID).Skip(skip>0?skip:0).Take(take);
            }
			#endregion
			
            return modelQuery;
        }

        public Batch GetByKey(String key)
        {
            return MwtsEntities.Batch.Where(item => item.BatchID.ToString() == key).FirstOrDefault();
        }

        public bool CheckExist(String key)
        {
            return MwtsEntities.Batch.Where(item => item.BatchID.ToString() == key).Count() > 0;
        }

        public void Add(Batch model)
        {
            MwtsEntities.Batch.Add(model);
            Save();
        }

        public void DeleteByKey(String key)
        {
            var model = GetByKey(key);
            if (model == null) return;
            MwtsEntities.Batch.Remove(model);
            Save();
        }

        public void Save()
        {
            MwtsEntities.SaveChanges();
        }
}
}
