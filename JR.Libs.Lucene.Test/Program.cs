using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JR.Libs.Lucene.Test
{
    class Program
    {        
        static void Main(string[] args)
        {
            var def = new IndexDefinition("Storage", @"C:\Storage");
            def.Fields.Add(new IndexField(def)
            {
                FieldName = "Title",
                IsIndexed = true,
                IsStored = true                
            });

            def.Fields.Add(new IndexField(def)
            {
                FieldName = "ID",
                IsIndexed = false,
                IsStored = true
            });

            def.Fields.Add(new IndexField(def)
            {
                FieldName = "Content",
                IsIndexed = true,
                IsStored = true
            });

            Console.WriteLine("Build Index");
            Lucene.IndexFactory.Instance.BuildIndex(Storage.Get().ToArray().ToList(), def);
            Console.WriteLine("Build Index Done");

            Console.WriteLine("Start search for coco");
            IndexFactory.Instance.Retrieve(new SearchQuery("sample", "Storage.Title", def, 100)).Results.ForEach(a =>
            {
                Console.WriteLine(string.Format("{0}:{1}:{2}", a["ID"], a["Title"],a["Content"]));
            });

            Console.ReadKey();
        }
    }
}
