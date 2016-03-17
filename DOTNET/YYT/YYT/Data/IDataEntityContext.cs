using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YYT.Query;

namespace YYT.Data
{
    /// <summary>
    /// 所有的数据实体执行者实现这个借口
    /// </summary>
    public interface IDataEntityContext:IDataContext
    {
        TEntity Add<TEntity>(TEntity entity) where TEntity : IDataEntity;
        List<TEntity> Add<TEntity>(List<TEntity> entityList) where TEntity : IDataEntity;

        bool Update<TEntity>(TEntity entity) where TEntity : IDataEntity;
        bool Update<TEntity>(List<TEntity> entityList) where TEntity : IDataEntity;
        bool Update(ICriteria condiftion, object value);

        bool Delete<TEntity>(TEntity entity) where TEntity : IDataEntity;
        bool Delete<TEntity>(List<TEntity> entityList) where TEntity : IDataEntity;
        bool Delete(ICriteria condition);

        int GetCount(ICriteria condition);
        List<TEntity> Query<TEntity>(ICriteria condition);
        List<TEntity> Query<TEntity>(ICriteria condition, int pageIndex, int pageSize, ref int entityCount) where TEntity : IDataEntity;
        List<object> Query(ICriteria condiftion);
    }
}
