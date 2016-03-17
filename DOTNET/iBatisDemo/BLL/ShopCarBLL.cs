using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Web;
using System.Collections;
using IBLL;

namespace BLL
{
    public class ShopCarBLL : IShopCarBLL
    {
        private Dictionary<int, OrderProduct> items = new Dictionary<int, OrderProduct>();

        public ShopCarBLL()
        {
            if (HttpContext.Current.Session["ShopCar"] != null)
            {
                items = (Dictionary<int, OrderProduct>)HttpContext.Current.Session["ShopCar"];
            }
            else
            {
                items = new Dictionary<int, OrderProduct>();
            }
        }

        public void Add(OrderProduct item)
        {
            if (items.ContainsKey(item.ProductID))
            {
                items[item.ProductID].Num += item.Num;
            }
            else
            {
                items.Add(item.ProductID, item);
            }

            Save();
        }

        public void Remove(int productId)
        {
            if (items.ContainsKey(productId))
            {
                items.Remove(productId);
                Save();
            }
        }

        public void SetNum(int productId, int num)
        {
            items[productId].Num = num;
            Save();
        }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        public void Clear()
        {
            items.Clear();
            Save();
        }

        public decimal Total
        {
            get
            {
                decimal total = 0;
                foreach (OrderProduct orderProduct in items.Values)
                {
                    total += orderProduct.Price * orderProduct.Num;
                }
                return total;
            }
        }

        public IList<OrderProduct> GetList()
        {
            IList<OrderProduct> list = new List<OrderProduct>();
            foreach (OrderProduct orderProduct in items.Values)
            {
                list.Add(orderProduct);
            }

            return list;
        }

        private void Save()
        {
            HttpContext.Current.Session["ShopCar"] = items;
        }
    }
}
