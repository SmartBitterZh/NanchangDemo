using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWithOracle.Helper.PersistenceHelper;

namespace WPFWithOracle.Repository
{
  public abstract class BaseRepositoryImpl<T> : IRepository<T>
    where T : new()
  {
    public virtual void Save(T obj)
    {
      throw new NotImplementedException();
    }

    public virtual void Update(T obj)
    {
      throw new NotImplementedException();
    }

    public virtual void Delete(T obj, int id)
    {
      throw new NotImplementedException();
    }

    public virtual void Delete(T obj, int[] ids)
    {
      throw new NotImplementedException();
    }

    public virtual T Find(T obj, int id)
    {
      throw new NotImplementedException();
    }

    public virtual IList<T> FindAll()
    {
      throw new NotImplementedException();
    }

    public virtual IList<T> FindByProperty(string property, string value)
    {
      throw new NotImplementedException();
    }

    public virtual QueryResult<T> GetPageData(int firstResult, int maxResults, string wherehql, object[] paramss, Dictionary<string, string> orderby)
    {
      throw new NotImplementedException();
    }

    public virtual QueryResult<T> GetPageData(int firstResult, int maxResults, string wherehql, object[] paramss)
    {
      throw new NotImplementedException();
    }

    public virtual QueryResult<T> GetPageData(int firstResult, int maxResults, Dictionary<string, string> orderby)
    {
      throw new NotImplementedException();
    }

    public virtual QueryResult<T> GetPageData(int firstResult, int maxResults)
    {
      throw new NotImplementedException();
    }

    public virtual QueryResult<T> GetPageData()
    {
      throw new NotImplementedException();
    }


    public void Update(IList<T> obj)
    {
      throw new NotImplementedException();
    }
  }
}
