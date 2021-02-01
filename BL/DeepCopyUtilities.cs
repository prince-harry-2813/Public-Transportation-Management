using System;
using System.Reflection;

namespace BL
{
    /// <summary>
    /// Copied From Efarat Amar 3 layer example 
    /// </summary>
    public static class DeepCopyUtilities
    {
        /// <summary>
        /// Copy properties of S to T property by property using the s property name to get the one from s
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void CopyPropertiesTo<T, S>(this S from, T to)
        {
            foreach (PropertyInfo propTo in to.GetType().GetProperties())
            {
                PropertyInfo propFrom = typeof(S).GetProperty(propTo.Name);
                if (propFrom == null)
                    continue;
                var value = propFrom.GetValue(from, null);
                if (value is ValueType || value is string)
                    propTo.SetValue(to, value);
               
            }
        }

        /// <summary>
        /// Copy From S and create new instance of T and copy the properties 
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="from"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object CopyPropertiesToNew<S>(this S from, Type type)
        {
            object to = Activator.CreateInstance(type); // new object of Type
            from.CopyPropertiesTo(to);
            return to;
        }
    }
}
