using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YYT.Query;

namespace YYT.Data
{
    /// <summary>
    /// 数据提供者要实现的借口
    /// </summary>
    public interface IDataProvider
    {
        DataResult<TEntity> Add<TEntity>(TEntity entity) where TEntity : IDataEntity;
        DataResult<TEntity> Add<TEntity>(List<TEntity> entityList) where TEntity : IDataEntity;

        DataResult<TEntity> Update<TEntity>(TEntity entity) where TEntity : IDataEntity;
        DataResult<TEntity> Update<TEntity>(List<TEntity> entityList) where TEntity : IDataEntity;
        bool Update(ICriteria condiftion, object value);

        DataResult<TEntity> Delete<TEntity>(TEntity entity) where TEntity : IDataEntity;
        DataResult<TEntity> Delete<TEntity>(List<TEntity> entityList) where TEntity : IDataEntity;
        bool Delete(ICriteria condiftion);

        int GetCount(ICriteria condition);

        DataResult<TEntity> GetOne<TEntity>(ICriteria condition) where TEntity : IDataEntity;
        DataResult<TEntity> GetList<TEntity>(ICriteria condition) where TEntity : IDataEntity;
        DataResult<TEntity> GetPageData<TEntity>(ICriteria condition, int pageIndex, int pageSize, ref int entityCount) where TEntity : IDataEntity;
        List<object> GetCustomData(ICriteria condiftion);
    }
}
