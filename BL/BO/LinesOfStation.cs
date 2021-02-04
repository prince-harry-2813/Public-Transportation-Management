using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
   public class LinesOfStation
    {
       public int  StationCode {get;set;}
        public IEnumerable<Line> Lines { get; set; }
    }
}
