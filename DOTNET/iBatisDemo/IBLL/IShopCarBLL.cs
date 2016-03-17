using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace IBLL
{
    public interface IShopCarBLL
    {
        void Add(OrderProduct item);
        void Remove(int productId);
        void SetNum(int productId, int num);
        int Count { get; }
        void Clear();
        decimal Total { get; }
        IList<OrderProduct> GetList();
    }
}
