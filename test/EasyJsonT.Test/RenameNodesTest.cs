namespace EasyJsonT.Test
{
    using Xunit;
    using EasyJsonT;
    using System.Collections.Generic;

    public class RenameNodesTest
    {
        [Fact]
        public void Rename_Nodes_For_JObject_Should_Succeed()
        {
            var json = new
            {
                code = 100,
                message = "msg",
                data = true
            }.ToJson();

            var nameMap = new Dictionary<string, string>
            {
                {"code","myCode"},
                {"message","msg"}
            };

            var res = JsonTProvider.RenameNodes(json, nameMap);

            var expected = new
            {
                data = true,
                myCode = 100,
                msg = "msg",
            }.ToJson();

            Assert.Equal(expected, res);
        }

        [Fact]
        public void Rename_Nodes_For_JArray_Should_Succeed()
        {
            var json = new List<dynamic>
            {
               new
                {
                    code = 100,
                    message = "msg",
                    data = true,
                },
                new
                {
                    code = 200,
                    message = "msg2",
                    data = false,
                }
            }.ToJson();

            var nameMap = new Dictionary<string, string>
            {
                {"code","myCode"},
                {"message","msg"}
            };

            var res = JsonTProvider.RenameNodes(json, nameMap);

            var expected = new List<dynamic>
            {
               new
                {
                    data = true,
                    myCode = 100,
                    msg = "msg",
                },
                new
                {
                    data =false,
                    myCode = 200,
                    msg = "msg2",
                }
            }.ToJson();

            Assert.Equal(expected, res);
        }

        [Fact]
        public void Rename_Nodes_For_BasicType_Should_Not_Effect()
        {
            var json = 2.ToJson();

            var nameMap = new Dictionary<string, string>
            {
                {"code","myCode"},
                {"message","msg"}
            };

            var res = JsonTProvider.RenameNodes(json, nameMap);

            Assert.Equal(json, res);
        }
    }
}
