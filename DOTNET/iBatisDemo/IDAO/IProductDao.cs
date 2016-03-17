using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Collections;

namespace IDAO
{
    public interface IProductDao
    {
        IList SelectAll();
        Product Select(int id);
        IList Select(PagerItem pagerItem);
        int Insert(Product product);
        int Update(Product product);
        int Delete(int id);
    }
}
