using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JR.Libs.Lucene.Test
{
    class Storage
    {
        public static ArrayList Get()
        {
            var rs = new ArrayList();
            rs.Add(new { ID = 1, Title="This is a Sample 1", Content="Cocojambo A"});
            rs.Add(new { ID = 2, Title = "This is a kample 2", Content = "Cocojambo B" });
            rs.Add(new { ID = 3, Title = "This is a lample 3", Content = "Cocojambo C" });
            rs.Add(new { ID = 4, Title = "This is a zample 4", Content = "Cocojambo D" });
            return rs;
        }
    }
}
