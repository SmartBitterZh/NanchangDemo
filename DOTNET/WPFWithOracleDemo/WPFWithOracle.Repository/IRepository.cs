using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWithOracle.Helper.PersistenceHelper;

namespace WPFWithOracle.Repository
{
  public interface IRepository<T>
  {
    void Save(T obj);

    void Update(T obj);

    void Update(IList<T> obj);

    void Delete(T obj, int id);

    void Delete(T obj, int[] ids);

    T Find(T obj, int id);

    IList<T> FindAll();

    IList<T> FindByProperty(string property, string value);

    QueryResult<T> GetPageData(int firstResult, int maxResults,
       string wherehql, object[] paramss, Dictionary<string, string> orderby);

    QueryResult<T> GetPageData(int firstResult, int maxResults,
       string wherehql, object[] paramss);

    QueryResult<T> GetPageData(int firstResult, int maxResults,
       Dictionary<string, string> orderby);

    QueryResult<T> GetPageData(int firstResult, int maxResults);

    QueryResult<T> GetPageData();
  }
}
