using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    [Serializable]
    public class Product
    {
        public virtual int ID { get; set; }
        public virtual string ProductName { get; set; }
        public virtual decimal NormalPrice { get; set; }
        public virtual decimal MemberPrice { get; set; }
    }
}
