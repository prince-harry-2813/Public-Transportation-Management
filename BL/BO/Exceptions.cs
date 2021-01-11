using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    [Serializable]
    public class BadBusIdException : Exception
    {
        public int ID;
        public BadBusIdException(string message, Exception innerException) :
            base(message, innerException){} /*=> ID = ((DO.BadPersonIdException)innerException).ID;*/
        public override string ToString() => base.ToString() + $", bad student id: {ID}";
    }

    [Serializable]
    public class BadLineIdException : Exception
    {
        public int ID;
        public BadLineIdException(string message, Exception innerException) :
            base(message, innerException){}/* => ID = ((DO.BadPersonIdException)innerException).ID;*/
        public override string ToString() => base.ToString() + $", bad student id: {ID}";
    }

    [Serializable]
    public class BadBusStopIDException : Exception
    {
        public int personID;
        public int courseID;
        public BadBusStopIDException(string message, Exception innerException) :
            base(message, innerException)
        {
            //personID = ((DO.BadPersonIdCourseIDException)innerException).personID;
            //courseID = ((DO.BadPersonIdCourseIDException)innerException).courseID;
        }
        public override string ToString() => base.ToString() + $", bad student id: {personID} and course ID: {courseID}";
    }

}
