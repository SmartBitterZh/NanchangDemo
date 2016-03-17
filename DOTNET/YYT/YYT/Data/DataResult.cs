using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YYT.Data
{
    public class DataResult<TEntity> : IDataResult where TEntity : IDataEntity
    {
        public TEntity DataEntity { get; set; }
        public List<TEntity> DataEntityList { get; set; }
        public Exception Exception { get; set; }
        public bool IsSuccess { get; set; }       
        public int Count { get; set; }
    }
}
