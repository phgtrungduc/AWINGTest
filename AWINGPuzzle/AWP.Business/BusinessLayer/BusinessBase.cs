using AutoMapper;
using AWP.Business.Interfaces;
using AWP.Business.Models;
using AWP.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AWP.Business.BusinessLayer
{
    public class BusinessBase<TEntity, TDTO> : IBusinessBase<TEntity, TDTO> where TEntity : class
    {
        protected IRepositoryBase<TEntity, TDTO> _repositoryBase;
        protected ServiceResult _serviceResult = new ServiceResult();

        protected readonly IMapper _mapper;

        public BusinessBase(IRepositoryBase<TEntity, TDTO> repositoryBase, IMapper mapper)
        {
            _repositoryBase = repositoryBase;
            _mapper = mapper;
        }

        public virtual bool BeforeInsert(ref TDTO entity)
        {
            // Add any common pre-insert logic here
            return true;
        }

        public bool BeforeUpdate(ref TEntity entity)
        {
            // Add any common pre-update logic here
            return true;
        }

        public bool Delete(string id)
        {
            return _repositoryBase.Delete(id);
        }

        public IEnumerable<TDTO> GetAll()
        {
            var res = _repositoryBase.GetAll();
            return res;
        }

        public ServiceResult GetByPaging(int page, int pageSize)
        {
            // Implement paging logic here if needed
            _serviceResult.Data = new { TotalRecords = 0, Data = new List<TDTO>() };
            return _serviceResult;
        }

        public virtual TDTO GetByID(string Id)
        {
            var data = _repositoryBase.GetByID(Id);
            var res = default(TDTO);
            if (data != null)
            {
                res = _mapper.Map<TDTO>(data);
            }
            return res;
        }

        public IEnumerable<TEntity> GetByKey(string key, string value)
        {
            var res = _repositoryBase.GetByKey(key, value);
            return res;
        }

        public TEntity GetOneByKey(string key, string value)
        {
            var res = _repositoryBase.GetOneByKey(key, value);
            return res;
        }

        public virtual bool Insert(TDTO entity)
        {
            var res = false;
            if (this.Validate(entity))
            {
                if (this.ValidateCustom(entity))
                {
                    if (this.BeforeInsert(ref entity))
                    {
                        var mapEntity = _mapper.Map<TEntity>(entity);
                        res = _repositoryBase.Insert(mapEntity);
                    }
                }
            }
            return res;
        }

        public bool Update(TDTO entity, string id)
        {
            var mapEntity = _mapper.Map<TEntity>(entity);
            return _repositoryBase.Update(mapEntity, id);
        }

        public virtual bool Validate(TDTO entity)
        {
            // Implement validation logic here if needed
            return true;
        }

        public virtual bool ValidateCustom(TDTO entity)
        {
            // Override this method in derived classes for custom validation
            return true;
        }
    }
}
