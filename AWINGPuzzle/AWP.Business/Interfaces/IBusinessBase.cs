using AWP.Business.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AWP.Business.Interfaces
{
    public interface IBusinessBase<TEntity, TDTO>
    {
        IEnumerable<TDTO> GetAll();
        TDTO GetByID(string Id);
        bool Delete(string id);
        bool Update(TDTO entity, string id);
        bool Insert(TDTO entity);
        IEnumerable<TEntity> GetByKey(string key, string value);
        TEntity GetOneByKey(string key, string value);
        
        bool Validate(TDTO entity);
        bool ValidateCustom(TDTO entity);
        bool BeforeInsert(ref TDTO entity);
        bool BeforeUpdate(ref TEntity entity);
        ServiceResult GetByPaging(int page, int pageSize);
    }
}
