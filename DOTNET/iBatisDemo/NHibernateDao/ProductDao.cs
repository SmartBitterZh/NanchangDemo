using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAO;
using System.Collections;
using Model;
using NHibernate;

namespace NHibernateDao
{
    public class ProductDao : IProductDao
    {
        public IList SelectAll()
        {
            return NHibernateSession.Session.CreateCriteria(typeof(ProductDao)).List();
        }

        public Product Select(int id)
        {
            return NHibernateSession.Session.Get<Product>(id);
        }

        public IList Select(PagerItem pagerItem)
        {
            string sql= string.Format(@"Select top {0} * from Product where ID not in (select top {1} ID from Product where ProductName like '%{2}%') and (ProductName like '%{2}%')", 
                pagerItem.PageSize, pagerItem.FilterRows, pagerItem.Keywords);

            IList<Product> products = NHibernateSession.Session.CreateSQLQuery(sql).AddEntity("product", typeof(Product)).List<Product>();

            return products.ToList();
        }

        public int Insert(Product product)
        {
            using (ISession session = NHibernateSession.Session)
            {
                session.Save(product);
                session.Flush();
                return 1;
            }
        }

        public int Update(Product product)
        {
            using (ISession session = NHibernateSession.Session)
            {
                session.Update(product);
                session.Flush();
                return 1;
            }
        }

        public int Delete(int id)
        {
            Product product = new Product();
            product.ID = id;

            using(ISession session=NHibernateSession.Session)
            using (ITransaction transcation = session.BeginTransaction())
            {
                session.Delete(product);
                transcation.Commit();
                return 1;
            }
        }
    }
}
