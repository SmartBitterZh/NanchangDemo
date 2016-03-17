using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using IBLL;
using IDAO;
using DAOFactory;
using Model;

namespace BLL
{
    public class CustomBLL : ICustomBLL
    {
        private ICustomDao _customDao;

        public CustomBLL()
        {
            _customDao = DAOManager.Instance.CreateCustomDaoInstance<ICustomDao>("CustomDao");
        }

        public IList SelectAll()
        {
            return _customDao.SelectAll();
        }

        public Custom Select(int id)
        {
            return _customDao.Select(id);
        }

        public int Insert(Custom custom)
        {
            return _customDao.Insert(custom);
        }

        public int Update(Custom custom)
        {
            return _customDao.Update(custom);
        }

        public int Delete(int id)
        {
            return _customDao.Delete(id);
        }
    }
}