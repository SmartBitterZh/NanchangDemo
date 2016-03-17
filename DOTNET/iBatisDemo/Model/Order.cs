using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Model
{
    [Serializable]
    public class Order
    {
        public virtual Guid ID { get; set; }
        public virtual DateTime CreateTime { get; set; }
        public virtual int CustomID { get; set; }
        public virtual int Status { get; set; }
        public virtual IList<OrderProduct> OrderProducts { get; set; }
    }
}
