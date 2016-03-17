using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAO;
using Model;
using System.Collections;
using IBatisNet.Common;

namespace IBatisNetDao
{
    public class OrderDao : IOrderDao
    {
        public void Insert(Order order)
        {
            using (IDalSession session = Mapper.Instance().BeginTransaction())
            {
                Mapper.Instance().Insert("Order-Insert", order);
                foreach (OrderProduct orderProduct in order.OrderProducts)
                {
                    orderProduct.OrderID = order.ID;
                    Mapper.Instance().Insert("OrderProduct-Insert", orderProduct);
                }
                session.Complete();
            }
        }

        public IList Select(PagerItem pagerItem)
        {
            return Mapper.Instance().QueryForList("Order-Select-Pager", pagerItem);
        }

        public Order Select(Guid id)
        {
            Order order = new Order();
            order = (Order)Mapper.Instance().QueryForObject("Order-Select", id);
            IList<OrderProduct> orderProducts = new List<OrderProduct>();
            Mapper.Instance().QueryForList<OrderProduct>("OrderProduct-Select", id, orderProducts);
            order.OrderProducts = orderProducts;

            return order;
        }

        public int Delete(Guid orderId)
        {
            return Mapper.Instance().Delete("Order-Delete", orderId);
        }
    }
}
