using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    [Serializable]
    public class PagerItem
    {
        public virtual int PageSize { get; set; }
        public virtual int CurrentPage { get; set; }
        public virtual int FilterRows { get { return (CurrentPage - 1) * PageSize; } }
        public virtual string Keywords { get; set; }
    }
}
