using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAOFactory;
using System.Collections;
using Model;
using IBLL;
using IDAO;

namespace BLL
{
    public class OrderBLL : IOrderBLL
    {
        private IOrderDao _orderDao;

        public OrderBLL()
        {
            _orderDao = DAOManager.Instance.CreateCustomDaoInstance<IOrderDao>("OrderDao");
        }

        public void Insert(Order order)
        {
            _orderDao.Insert(order);
        }

        public IList Select(PagerItem pagerItem)
        {
            return _orderDao.Select(pagerItem);
        }

        public Order Select(Guid id)
        {
            return _orderDao.Select(id);
        }

        public int Delete(Guid orderId)
        {
            return _orderDao.Delete(orderId);
        }
    }
}
