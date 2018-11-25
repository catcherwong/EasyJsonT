namespace EasyJsonT.Demo
{
    using EasyJsonT;
    using System;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            ////1. JObject
            //var c = new MyClass
            //{
            //    A1 = 1,
            //    A2 = "2",
            //    A3 = 3,
            //    Sub = new MySubClass 
            //    {
            //        B1 = 11,
            //        B2 = "12"
            //    }
            //};

            //var json = JsonConvert.SerializeObject(c);

            //var res1 = JsonTProvider.CustomizeFields(json, new List<string> { "a3" , "sub" });
            //Console.WriteLine(res1);
            //var res2 = JsonTProvider.CustomizeFields(json, new List<string> { "a3","a1","sub" },false);
            //Console.WriteLine(res2);
            //var res3 = JsonTProvider.CustomizeFields(json, "sub" ,new List<string> { "b1" });
            //Console.WriteLine(res3);
            //var res4 = JsonTProvider.CustomizeFields(json, "sub", new List<string> { "b1" },false);
            //Console.WriteLine(res4);

            //2. JArray

            var l = new
            {
                code = 0,
                msg = "",
                data = new List<MyClass>
                {
                    new MyClass
                    {
                        A1 = 1,
                        A2 = "2",
                        A3 = 3,
                        Sub = new MySubClass
                        {
                            B1 = 11,
                            B2 = "12"
                        }
                    }
                }
            };

            var json2 = JsonConvert.SerializeObject(l);

            //var res21 = JsonTProvider.CustomizeFields(json2, new List<string> { "a3", "sub" });
            //Console.WriteLine(res21);
            //var res22 = JsonTProvider.CustomizeFields(json2, new List<string> { "a3", "a1", "sub" }, false);
            //Console.WriteLine(res22);
            var res23 = JsonTProvider.FilterNodes(json2, "data", new List<string> { "a3", "sub" });
            Console.WriteLine(res23);
            var res24 = JsonTProvider.FilterNodes(json2, "data", new List<string> { "a3", "a1", "sub" }, false);
            Console.WriteLine(res24);

            Console.ReadKey();
        }
    }

    class MyClass
    {
        public int A1 { get; set; }
        public string A2 { get; set; }
        public int A3 { get; set; }
        public MySubClass Sub { get; set; }
    }

    class MySubClass
    {
        public int B1 { get; set; }
        public string B2 { get; set; }
    }
}
