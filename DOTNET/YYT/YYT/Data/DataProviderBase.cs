using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YYT.Query;

namespace YYT.Data
{
    /// <summary>
    /// 数据提供者的基类，用于处理异常，等操作
    /// </summary>
    public abstract class DataProviderBase : IDataProvider
    {
        #region Implement Methods

        public DataResult<TEntity> Add<TEntity>(TEntity entity) where TEntity : IDataEntity
        {
            throw new NotImplementedException();
        }

        public DataResult<TEntity> Add<TEntity>(List<TEntity> entityList) where TEntity : IDataEntity
        {
            throw new NotImplementedException();
        }

        public DataResult<TEntity> Update<TEntity>(TEntity entity) where TEntity : IDataEntity
        {
            throw new NotImplementedException();
        }

        public DataResult<TEntity> Update<TEntity>(List<TEntity> entityList) where TEntity : IDataEntity
        {
            throw new NotImplementedException();
        }

        public bool Update(ICriteria condiftion, object value)
        {
            throw new NotImplementedException();
        }

        public DataResult<TEntity> Delete<TEntity>(TEntity entity) where TEntity : IDataEntity
        {
            throw new NotImplementedException();
        }

        public DataResult<TEntity> Delete<TEntity>(List<TEntity> entityList) where TEntity : IDataEntity
        {
            throw new NotImplementedException();
        }

        public bool Delete(ICriteria condiftion)
        {
            throw new NotImplementedException();
        }

        public int GetCount(ICriteria condition)
        {
            throw new NotImplementedException();
        }

        public DataResult<TEntity> GetOne<TEntity>(ICriteria condition) where TEntity : IDataEntity
        {
            throw new NotImplementedException();
        }

        public DataResult<TEntity> GetList<TEntity>(ICriteria condition) where TEntity : IDataEntity
        {
            throw new NotImplementedException();
        }

        public DataResult<TEntity> GetPageData<TEntity>(ICriteria condition, int pageIndex, int pageSize, ref int entityCount) where TEntity : IDataEntity
        {
            throw new NotImplementedException();
        }

        public List<object> GetCustomData(ICriteria condiftion)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
