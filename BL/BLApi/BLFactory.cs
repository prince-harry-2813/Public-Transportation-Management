using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLApi
{
    public static class BLFactory
    {
        public static IBL GetIBL()
        {
            return (IBL) new BLImp();
        }
    }
}
