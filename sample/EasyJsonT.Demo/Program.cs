namespace EasyJsonT.Demo
{
    using EasyJsonT;
    using System;
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            //1. filter nodes

            var filterNode = new List<string> { "UserName", "age" };

            var filterJson1 = @"{'userName':'catcherwong','age':18,'hobbies':['write','running']}";
            var filterJsonResult11 = JsonTProvider.FilterNodes(filterJson1, filterNode, true);
            //{"hobbies": ["write, "running"]}
            var filterJsonResult12 = JsonTProvider.FilterNodes(filterJson1, filterNode, false);
            //{"userName": "catcherwong", "age": 18}

            var filterJson2 = @"[{'userName':'catcherwong','age':18,'hobbies':['write','running']},{'userName':'james','age':20,'hobbies':['music']}]";
            var filterJsonResult21 = JsonTProvider.FilterNodes(filterJson2, filterNode, true);
            //[{"hobbies":["write","running"]},{"hobbies":["music"]}]
            var filterJsonResult22 = JsonTProvider.FilterNodes(filterJson2, filterNode, false);
            //[{"userName":"catcherwong","age":18},{"userName":"james","age":20}]

            var filterJson3 = @"{'code':0,'msg':'ok','data':{'userName':'catcherwong','age':18,'hobbies':['write','running']}}";
            var filterJsonResult31 = JsonTProvider.FilterNodes(filterJson3, "data" ,filterNode, true);
            //{"code":0,"msg":"ok","data":{"hobbies":["write","running"]}}
            var filterJsonResult32 = JsonTProvider.FilterNodes(filterJson3, "data" , filterNode, false);
            //{"code":0,"msg":"ok","data":{"userName":"catcherwong","age":18}}

            var filterJson4 = @"{'code':0,'msg':'ok','data':[{'userName':'catcherwong','age':18,'hobbies':['write','running']},{'userName':'james','age':20,'hobbies':['music']}]}";
            var filterJsonResult41 = JsonTProvider.FilterNodes(filterJson4, "data", filterNode, true);
            //{"code":0,"msg":"ok","data":[{"hobbies":["write","running"]},{"hobbies":["music"]}]}
            var filterJsonResult42 = JsonTProvider.FilterNodes(filterJson4, "data", filterNode, false);
            //{"code":0,"msg":"ok","data":[{"userName":"catcherwong","age":18},{"userName":"james","age":20}]}

            //2. add nodes
            var addNodesDict = new Dictionary<string, object>
            {
                {"age",18},
                {"subObj",new{prop1="123"}},
                {"subArray",new List<string> {"a","b"}}
            };

            var addNodesJson = @"{'userName':'catcherwong'}";
            var addNodesResult1 = JsonTProvider.AddNodes(addNodesJson, addNodesDict);
            //{"userName":"catcherwong","age":18,"subObj":{"prop1":"123"},"subArray":["a","b"]}
            var addNodesResult2 = JsonTProvider.AddNodes(addNodesJson, addNodesDict,"basic");
            //{"basic":{"userName":"catcherwong"},"age":18,"subObj":{"prop1":"123"},"subArray":["a","b"]}

            //3. rename nodes
            var renameDict = new Dictionary<string, string>
            {
                {"name","userName"},{"nl","age"}
            };

            var renameJson1 = @"{'name':'catcherwong','nl':18}";
            var renameJsonResult1 = JsonTProvider.RenameNodes(renameJson1, renameDict);
            //{"userName":"catcherwong","age":18}          

            var renameJson2 = @"{'code':0,'msg':'ok','data':{'name':'catcherwong','nl':18}}";
            var renameJsonResult2 = JsonTProvider.RenameNodes(renameJson2, "data" , renameDict);
            //{"code":0,"msg":"ok","data":{"userName":"catcherwong","age":18}}          


            //4. translate values
            var translateValueDict = new Dictionary<string, Dictionary<object, object>>
            {
                {"Code",new Dictionary<object, object>{{-1,0},{-2,1}}},
                {"messAge",new Dictionary<object, object>{{"yes","Success"},{"no","Error"}}}
            };

            var translateJson1 = @"{'code':-1,'message':'yes'}";
            var translateResult1 = JsonTProvider.TranslateValues(translateJson1, translateValueDict);
            //{"code":0,"message":"Success"}

            var translateJson2 = @"[{'code':-1,'message':'yes'},{'code':-2,'message':'no'}]";
            var translateResult2 = JsonTProvider.TranslateValues(translateJson2, translateValueDict);
            //[{"code":0,"message":"Success"},{"code":1,"message":"Error"}]

            var translateJson3 = @"{'myCode':-1,'myMessage':'yes','data':{'code':-2,'message':'no'}}";
            var translateResult3 = JsonTProvider.TranslateValues(translateJson3, "data",translateValueDict);
            //{"myCode":-1,"myMessage":"yes","data":{"code":1,"message":"Error"}}

            var translateJson4 = @"{'myCode':-1,'myMessage':'yes','data':[{'code':-1,'message':'yes'},{'code':-2,'message':'no'}]}";
            var translateResult4 = JsonTProvider.TranslateValues(translateJson4, "data", translateValueDict);
            //{"myCode":-1,"myMessage":"yes","data":[{"code":0,"message":"Success"},{"code":1,"message":"Error"}]}

            Console.ReadKey();
        }
    }
}
