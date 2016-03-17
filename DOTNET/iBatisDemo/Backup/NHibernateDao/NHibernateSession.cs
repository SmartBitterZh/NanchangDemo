using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using System.Reflection;
using NHibernate;

namespace NHibernateDao
{
    public class NHibernateSession
    {
        private static readonly object _lockObject = new object();
        private static ISessionFactory _factory;

        private NHibernateSession()
        {
        }

        public static ISessionFactory Factory
        {
            get
            {
                if (_factory == null)
                {
                    lock (_lockObject)
                    {
                        if (_factory == null)
                        {
                            Configuration cfg = new Configuration();
                            //cfg.AddAssembly("NHibernate_hbm_xml");
                            _factory = cfg.Configure().BuildSessionFactory();
                        }
                    }
                }
                return _factory;
            }
        }
        public static ISession Session
        {
            get
            {
                return Factory.OpenSession();
            }
        }
    }
}
