using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModelLayer;

namespace BusinessLogicLayer
{

public class sysdiagramsEntityBLL
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

        public sysdiagramsEntityBLL()
        {
        }

        public IQueryable<sysdiagrams> GetList()
        {
            return MwtsEntities.sysdiagramss.AsQueryable();
        }

        public IQueryable<sysdiagrams> GetList(List<string> searchKeys, sysdiagrams model, Dictionary<string, object> searchParams)
        {
            return GetList(searchKeys, model, searchParams, null, null);
        }

        public IQueryable<sysdiagrams> GetList(List<string> searchKeys, sysdiagrams model, Dictionary<string, object> searchParams, int? pageIndex, int? rowCount)
        {
            var modelQuery = MwtsEntities.sysdiagramss.AsQueryable();
			            if (searchKeys.Contains("name"))
            {
				                modelQuery = modelQuery.Where(item => item.name == model.name);
									}
                        if (searchKeys.Contains("principal_id"))
            {
				                modelQuery = modelQuery.Where(item => item.principal_id == model.principal_id);
									}
                        if (searchKeys.Contains("diagram_id"))
            {
				                modelQuery = modelQuery.Where(item => item.diagram_id == model.diagram_id);
									}
                        if (searchKeys.Contains("version"))
            {
				                modelQuery = modelQuery.Where(item => item.version == model.version);
									}
                        if (searchKeys.Contains("definition"))
            {
				                modelQuery = modelQuery.Where(item => item.definition == model.definition);
									}
                        if (pageIndex != null && rowCount != null)
            {
			  int skip = ((int)pageIndex - 1) * (int)rowCount;
			  int take = (int)rowCount;
              modelQuery =  modelQuery.OrderBy(item => item.diagram_id).Skip(skip>0?skip:0).Take(take);
            }
            if (searchParams.Count > 0)
            {
                //扩展查询条件
            }
            return modelQuery;
        }

        public sysdiagrams GetByKey(Int32 key)
        {
            return MwtsEntities.sysdiagramss.Where(item => item.diagram_id == key).FirstOrDefault();
        }

        public bool CheckExist(Int32 key)
        {
            return MwtsEntities.sysdiagramss.Where(item => item.diagram_id == key).Count() > 0;
        }

        public void Add(sysdiagrams model)
        {
            MwtsEntities.sysdiagramss.AddObject(model);
            Save();
        }

        public void DeleteByKey(Int32 key)
        {
            var model = GetByKey(key);
            if (model == null) return;
            MwtsEntities.sysdiagramss.DeleteObject(model);
            Save();
        }

        public void Save()
        {
            MwtsEntities.SaveChanges();
        }
}
}
