using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace Infrastructure
{
    public class PagingHelper
    {
        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int RecordCount { get; set; }

        public int CurrentPageCount { get; set; }

        public int PageCount
        {
            get
            {
                if (RecordCount % PageSize != 0)
                    return (int)(RecordCount / PageSize) + 1;
                return (int)(RecordCount / PageSize);
            }
        }

        private PagingHelper()
        {
        }

        public PagingHelper(int pageSize, int recordCount)
        {
            this.PageIndex = 1;
            this.PageSize = pageSize;
            this.RecordCount = recordCount;
        }
    }
}
