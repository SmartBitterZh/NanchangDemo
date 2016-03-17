using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context;
using Spring.Context.Support;
using Spring.Aop.Framework;

namespace SpringIoc
{
    public class SpringContext
    {
        private static readonly object _lockObject = new object();
        private static SpringContext _context;

        private SpringContext()
        {
        }

        public Object CreateInstance(string name)
        {
            string[] xmlFiles = new string[] 
            {
                "assembly://SpringIoc/SpringIoc/Spring_bean.xml"
            };
            IApplicationContext _context = new XmlApplicationContext(xmlFiles);

            return _context.GetObject(name);
        }

        public Object CreateSecurityProxyInstance(string name)
        {
            ProxyFactory factory = new ProxyFactory(CreateInstance(name));
            factory.AddAdvice(new SecurityAdvice());

            return factory.GetProxy();
        }

        public static SpringContext Context
        {
            get
            {
                if (_context == null)
                {
                    lock (_lockObject)
                    {
                        if (_context == null)
                        {
                            _context = new SpringContext();
                        }
                    }
                }

                return _context;
            }
        }
    }
}
