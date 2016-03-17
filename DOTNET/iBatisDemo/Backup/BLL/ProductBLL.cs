using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAO;
using IBLL;
using DAOFactory;
using System.Collections;
using Model;

namespace BLL
{
    public class ProductBLL : IProductBLL
    {
        private IProductDao _productDao;

        public ProductBLL()
        {
            _productDao = DAOManager.Instance.CreateCustomDaoInstance<IProductDao>("ProductDao");
        }

        public IList SelectAll()
        {
            return _productDao.SelectAll();
        }

        public Product Select(int id)
        {
            return _productDao.Select(id);
        }

        public IList Select(PagerItem pagerItem)
        {
            return _productDao.Select(pagerItem);
        }

        public int Insert(Product product)
        {
            return _productDao.Insert(product);
        }

        public int Update(Product product)
        {
            return _productDao.Update(product);
        }

        public int Delete(int id)
        {
            return _productDao.Delete(id);
        }
    }
}
