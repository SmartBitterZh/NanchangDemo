using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Model;
using IDAO;

namespace IBatisNetDao
{
    public class ProductDao : IProductDao
    {
        public IList SelectAll()
        {
            return Mapper.Instance().QueryForList("Product-Select", null);
        }

        public Product Select(int id)
        {
            return (Product)Mapper.Instance().QueryForObject("Product-Select", id);
        }

        public IList Select(PagerItem pagerItem)
        {
            return Mapper.Instance().QueryForList("Product-Select-Pager", pagerItem);
        }

        public int Insert(Product product)
        {
            Mapper.Instance().Insert("Product-Insert", product);
            return 1;
        }

        public int Update(Product product)
        {
            return Mapper.Instance().Update("Product-Update", product);
        }

        public int Delete(int id)
        {
            return Mapper.Instance().Delete("Product-Delete", id);
        }
    }
}
