using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using IDAO;
using Model;

namespace IBatisNetDao
{
    public class CustomDao : ICustomDao
    {
        public IList SelectAll()
        {
            return Mapper.Instance().QueryForList("Custom-Select", null);
        }

        public Custom Select(int id)
        {
            return (Custom)Mapper.Instance().QueryForObject("Custom-Select", id);
        }

        public int Insert(Custom custom)
        {
            Mapper.Instance().Insert("Custom-Insert", custom);
            return 1;
        }

        public int Update(Custom custom)
        {
            return Mapper.Instance().Update("Custom-Update", custom);
        }

        public int Delete(int id)
        {
            return Mapper.Instance().Delete("Delete", id);
        }
    }
}
