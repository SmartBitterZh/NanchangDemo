using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAO;
using Model;
using System.Collections;
using NHibernate;

namespace NHibernateDao
{
    public class CustomDao : ICustomDao
    {
        public IList SelectAll()
        {
            return NHibernateSession.Session.CreateCriteria(typeof(Custom)).List();
        }

        public Custom Select(int id)
        {
            return NHibernateSession.Session.Get<Custom>(id);
        }

        public int Insert(Custom custom)
        {
            using (ISession session = NHibernateSession.Session) 
            {
                session.Save(custom);
                session.Flush();
                return 1;
            }
        }

        public int Update(Custom custom)
        {
            using (ISession session = NHibernateSession.Session)
            {
                session.Update(custom);
                session.Flush();

                return 1;
            }
        }

        public int Delete(int id)
        {
            Custom custom = new Custom();
            custom.ID = id;

            using(ISession session=NHibernateSession.Session)
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(custom);
                transaction.Commit();
                return 1;
            }
        }
    }
}
