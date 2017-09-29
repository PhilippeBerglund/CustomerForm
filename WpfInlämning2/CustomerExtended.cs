using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfInlämning2
{
    public class CustomerExtended : Customer
    {
        //public string CustomerType
        //{
        //    get
        //    {
        //        if (base.isPrivate.Value)
        //        {
        //            return "Privat";
        //        }
        //        else
        //            return "Företag";
        //    }
        //    set { }
        //}

        //    if (b) { return "Privat"; } else { return "Företag"; }

        public string CustomerType { get; set; }

    }
}
