using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    [Serializable]
    public class Custom
    {
        public virtual int ID { get; set; }
        public virtual string CustomName { get; set; }
        public virtual string Address { get; set; }
        public virtual bool IsMember { get; set; }
    }
}
