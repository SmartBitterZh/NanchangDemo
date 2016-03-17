using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Model;

namespace IDAO
{
    public interface ICustomDao
    {
        IList SelectAll();
        Custom Select(int id);
        int Insert(Custom custom);
        int Update(Custom custom);
        int Delete(int id);
    }
}
