using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModelLayer;

namespace BusinessLogicLayer.Entity
{

	public class UsersEntityBLL
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

        public UsersEntityBLL()
        {
        }

        public IQueryable<Users> GetList()
        {
            return MwtsEntities.Users.AsQueryable();//.Include("子对象").AsQueryable();
        }

        public IQueryable<Users> GetList(List<string> searchKeys, Users model, Dictionary<string, object> searchParams)
        {
            return GetList(searchKeys, model, searchParams, null, null);
        }

        public IQueryable<Users> GetList(List<string> searchKeys, Users model, Dictionary<string, object> searchParams, int? pageIndex, int? rowCount)
        {
            //var modelQuery = MwtsEntities.Users.AsQueryable();
            var modelQuery = new MWTSEntities().Users.AsQueryable();//.Include("子对象").AsQueryable();
			
			#region 默认查询条件
			            if (searchKeys.Contains("UserID"))
            {
				                modelQuery = modelQuery.Where(item => item.UserID == model.UserID);
									}
                        if (searchKeys.Contains("Sid"))
            {
				                modelQuery = modelQuery.Where(item => item.Sid == model.Sid);
									}
                        if (searchKeys.Contains("UserType"))
            {
				                modelQuery = modelQuery.Where(item => item.UserType == model.UserType);
									}
                        if (searchKeys.Contains("AuthType"))
            {
				                modelQuery = modelQuery.Where(item => item.AuthType == model.AuthType);
									}
                        if (searchKeys.Contains("UserName"))
            {
				                modelQuery = modelQuery.Where(item => item.UserName == model.UserName);
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
              modelQuery =  modelQuery.OrderBy(item => item.UserID).Skip(skip>0?skip:0).Take(take);
            }
			#endregion
			
            return modelQuery;
        }

        public Users GetByKey(System.Guid key)
        {
            return MwtsEntities.Users.Where(item => item.UserID == key).FirstOrDefault();
        }

        public bool CheckExist(System.Guid key)
        {
            return MwtsEntities.Users.Where(item => item.UserID == key).Count() > 0;
        }

        public void Add(Users model)
        {
            MwtsEntities.Users.Add(model);
            Save();
        }

        public void DeleteByKey(System.Guid key)
        {
            var model = GetByKey(key);
            if (model == null) return;
            MwtsEntities.Users.Remove(model);
            Save();
        }

        public void Save()
        {
            MwtsEntities.SaveChanges();
        }
}
}
