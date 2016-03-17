using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Collections;

namespace IDAO
{
    public interface IOrderDao
    {
        void Insert(Order order);
        IList Select(PagerItem pagerItem);
        Order Select(Guid id);
        int Delete(Guid orderId);
    }
}
