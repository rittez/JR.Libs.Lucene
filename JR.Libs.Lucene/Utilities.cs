using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JR.Libs.Lucene
{
    internal static class Utilities
    {
        public static string GetValue<T>(T obj, string propertyName)
        {
            var u = obj.GetType().GetProperty(propertyName).GetValue(obj);
            if (u != null)
            {
                return u.ToString();
            }

            return null;
        }        

        public static Dictionary<string, string> ToDictionary<T>(this T obj, string[] propertyNames=null)
        {
            var rs = new Dictionary<string, string>();
            if (propertyNames == null)
            {
                propertyNames = obj.GetType().GetProperties().Select(a => a.Name).ToArray();
            }

            foreach (var pr in propertyNames)
            {
                var u = obj.GetType().GetProperty(pr).GetValue(obj);
                if (u != null)
                {
                    rs.Add(pr, u.ToString());
                }    
            }

            return rs;
        }  
    }
}
