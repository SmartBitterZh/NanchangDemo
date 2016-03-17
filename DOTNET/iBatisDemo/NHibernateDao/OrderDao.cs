using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAO;
using NHibernate;
using Model;
using System.Collections;

namespace NHibernateDao
{
    public class OrderDao : IOrderDao
    {
        public void Insert(Order order)
        {
            using (ISession session=NHibernateSession.Session)
            using (ITransaction transcaption = session.BeginTransaction())
            {
                session.Save(order);
                session.Flush();
                transcaption.Commit();
            }
        }

        public IList Select(PagerItem pagerItem)
        {
            string sql = string.Format(@"Select top {0} * from [Order]
            where ID not in (select top {1} ID from [Order] where ID like '%{2}%')
            and (ID like '%{2}%')", pagerItem.PageSize, pagerItem.FilterRows, pagerItem.Keywords);

            IList<Order> orders = NHibernateSession.Session.CreateSQLQuery(sql).AddEntity("order", typeof(Order)).List<Order>();
            return orders.ToList();
        }

        public Order Select(Guid id)
        {
            using (ISession session = NHibernateSession.Session)
            {
                Order order = session.Get<Order>(id);
                return order;
            }
        }

        public int Delete(Guid orderId)
        {
            using (ISession session = NHibernateSession.Session)
            {
                Order order = session.Get<Order>(orderId);
                using (ITransaction transcation = session.BeginTransaction())
                {
                    session.Delete(order);
                    transcation.Commit();
                    return 1;
                }
            }
        }
    }
}
