using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using System.Collections;

namespace DAOFactory
{
    public class DAOManager
    {
        private static DAOManager _instance = null;
        private Hashtable _classNameList;
        private string _assemblyName;

        public DAOManager()
        {
            _classNameList = new Hashtable();
            _assemblyName = ConfigurationManager.AppSettings["DaoAssembly"];
        }

        public static DAOManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(DAOManager))
                    {
                        if (_instance == null)
                        {
                            return new DAOManager();
                        }
                    }
                }
                return _instance;
            }
        }
        public T CreateCustomDaoInstance<T>(string className) where T : class
        {
            if (_classNameList.Contains(className))
            {
                return (T)_classNameList[className];
            }
            else
            {
                string objectFullName = _assemblyName + "." + className;
                object obj = Assembly.Load(_assemblyName).CreateInstance(objectFullName);
                _classNameList.Add(className, obj);
                return (T)obj;
            }
        }
    }
}