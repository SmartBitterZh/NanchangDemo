using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    [Serializable]
    public class OrderProduct
    {
        public virtual Guid ID { get; set; }
        public virtual Guid OrderID { get; set; }
        public virtual int ProductID { get; set; }
        public virtual string ProductName { get; set; }
        public virtual int Num { get; set; }
        public virtual decimal Price { get; set; }
    }
}
