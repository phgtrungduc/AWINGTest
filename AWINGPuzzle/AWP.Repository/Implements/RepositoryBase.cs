using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using AutoMapper;
using AWP.Repository.Interfaces;
using AWP.DBContext.Models;

namespace AWP.Repository.Implements
{
    public class RepositoryBase<TEntity, TDTO> : IRepositoryBase<TEntity, TDTO> where TEntity : class
    {
        protected AwingDbContext _context;
        //String _connectionString =null;
        //SqlConnection _dbConnection =null;
        protected String tableName = "";

        protected DbSet<TEntity> _dbSet;

        protected readonly IMapper _mapper;
        public RepositoryBase(AwingDbContext context, IMapper mapper)
        {
            _context = context;
            context.ChangeTracker.LazyLoadingEnabled = false;
            tableName = typeof(TEntity).Name;
            _mapper = mapper;
        }

        public bool Delete(string id)
        {
            var res = 0;
            _dbSet = _context.Set<TEntity>();
            var record = _dbSet.Find(Guid.Parse(id));
            if (record != null)
            {
                _dbSet.Remove(record);
                res = _context.SaveChanges();
            }
            return (res > 0 ? true : false);
        }

        public virtual IEnumerable<TDTO> GetAll()
        {
            _dbSet = _context.Set<TEntity>();
            var obj = (TDTO)typeof(TDTO).GetConstructor(new Type[0]).Invoke(new object[0]);
            //var configuration = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<TEntity, TDTO>();
            //});
            //var mapper = configuration.CreateMapper();
            var res = _mapper.Map<List<TEntity>, IEnumerable<TDTO>>(_dbSet?.ToList());
            return res;
        }

        public virtual TEntity GetByID(string Id)
        {
            _dbSet = _context.Set<TEntity>();
            var res = _dbSet.Find(Guid.Parse(Id));
            return res;
        }

        public bool Insert(TEntity entity)
        {
            _dbSet = _context.Set<TEntity>();
            _dbSet.Add(entity);
            var res = _context.SaveChanges();
            return (res > 0 ? true : false);
        }

        public bool Update(TEntity entity, string id)
        {
            var res = 0;
            _dbSet = _context.Set<TEntity>();
            var data = _dbSet.Find(Guid.Parse(id));
            _context.Entry(data).CurrentValues.SetValues(entity);
            res = _context.SaveChanges();
            return (res > 0 ? true : false);
        }

        public IEnumerable<TEntity> GetByKey(string key, string value)
        {

            IEnumerable<TEntity> res;
            _dbSet = _context.Set<TEntity>();
            //var type = typeof(TEntity).GetProperty(key).PropertyType;
            //var valueFind = Convert.ChangeType(value, type);
            res = _dbSet.FromSqlRaw($"select * from {tableName} where {key} = '{value}'");

            return res;

        }
        public TEntity GetOneByKey(string key, string value)
        {

            TEntity res = null;
            _dbSet = _context.Set<TEntity>();
            //var type = typeof(TEntity).GetProperty(key).PropertyType;
            //var valueFind = Convert.ChangeType(value, type);
            res = _dbSet.FromSqlRaw($"select * from {tableName} where {key} = '{value}'").FirstOrDefault();
            return res;

        }
        public TableName GetOneByKeyWithType<TableName>(PropertyInfo prop, TEntity entity) where TableName : class
        {
            TableName res = null;
            var key = prop.Name;
            var value = prop.GetValue(entity);
            var table = typeof(TableName).Name;
            var dbSet = _context.Set<TableName>();
            //var type = typeof(TEntity).GetProperty(key).PropertyType;
            //var valueFind = Convert.ChangeType(value, type);
            res = dbSet.FromSqlRaw($"select * from [{table}] where {key} = '{value}'").FirstOrDefault();
            return res;

        }

        //public virtual ServiceResult GetByPaging(int page, int pageSize)
        //{
        //    _dbSet = _context.Set<TEntity>();
        //    var totalRecords = _dbSet.Count();
        //    var skip = (page - 1) * pageSize;
        //    var res = new ServiceResult();
        //    if (skip >= 0 && pageSize > 0)
        //    {
        //        var data = _dbSet.Skip(skip).Take(pageSize);
        //        if (typeof(TEntity) == typeof(House))
        //        {
        //        }
        //        res.Data = new { TotalRecords = totalRecords, Data = data?.ToList() };
        //    }
        //    return res;
        //}

        public bool DeleteById(string id)
        {
            _dbSet = _context.Set<TEntity>();
            var res = 0;
            var entity = _dbSet.Find(Guid.Parse(id));
            if (entity != null)
            {
                _dbSet.Remove(entity);
                res = _context.SaveChanges();
            }
            return (res > 0 ? true : false);
        }
    }
}
