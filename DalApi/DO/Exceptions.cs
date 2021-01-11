using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    [Serializable]
    public class BadIdExeption : Exception
    {
        public int ID;
        public BadIdExeption(int id) : base() => ID = id;
        public BadIdExeption(int id, string message) : base(message) => ID = id;
        public BadIdExeption(int id, string message, Exception inner) : base(message, inner) => ID = id;
        protected BadIdExeption(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        public override string ToString() => base.ToString() + $", bad {base.Source} id: {ID}";   
    }

    [Serializable]
    public class DuplicateObjExeption : Exception
    {
        public int ID;
        public DuplicateObjExeption(int id) : base() => ID = id;
        public DuplicateObjExeption(int id, string message) : base(message) => ID = id;
        public DuplicateObjExeption(int id, string message, Exception inner) : base(message, inner) => ID = id;
        protected DuplicateObjExeption(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        public override string ToString() => base.ToString() + $", Object {base.Source} id: {ID} already exist";
    }


}
