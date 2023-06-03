using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Common
{
    public class Paging
    {
        public int PageSize { get; set; }

        public int CurrentPageNumber { get; set; }

        public int LastColumnValue { get; set; }
    }
}
