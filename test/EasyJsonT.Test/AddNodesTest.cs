namespace EasyJsonT.Test
{
    using Xunit;
    using EasyJsonT;
    using System.Collections.Generic;

    public class AddNodesTest
    {
        [Fact]
        public void Add_Nodes_Should_Succeed()
        {
            var json = new
            {
                prop1 = 1
            }.ToJson();

            var addNodes = new Dictionary<string, object>
            {
                {"prop2",2},
                {"prop3",new{ sub = 1}},
                {"prop4",new List<string>{"a"}}
            };

            var res = JsonTProvider.AddNodes(json, addNodes);

            var expected = new
            {
                prop1 = 1,
                prop2 = 2,
                prop3 = new { sub = 1 },
                prop4 = new List<string> { "a" }
            }.ToJson();

            Assert.Equal(expected, res);
        }

        [Fact]
        public void Add_Exist_Nodes_Should_Not_Effect()
        {
            var json = new
            {
                prop1 = 1
            }.ToJson();

            var addNodes = new Dictionary<string, object>
            {
                {"prop1",2},
                {"prop3",new{ sub = 1}},
                {"prop4",new List<string>{"a"}}
            };

            var res = JsonTProvider.AddNodes(json, addNodes);

            var expected = new
            {
                prop1 = 1, //the value should be 1, not 2
                prop3 = new { sub = 1 },
                prop4 = new List<string> { "a" }
            }.ToJson();

            Assert.Equal(expected, res);
        }

        [Fact]
        public void Add_Nodes_Should_Throw_NotSpecialJsonTypeException()
        {
            var json = new List<string> { "a" }.ToJson();

            var addNodes = new Dictionary<string, object>
            {
                {"prop2",2},
                {"prop3",new{ sub = 1}},
                {"prop4",new List<string>{"a"}}
            };

            Assert.Throws<NotSpecialJsonTypeException>(() => JsonTProvider.AddNodes(json, addNodes));
        }

        [Fact]
        public void Add_Nodes_With_Node_Name_Should_Succeed_When_JSON_Is_Object()
        {
            var json = new
            {
                prop1 = 1
            }.ToJson();

            var addNodes = new Dictionary<string, object>
            {
                {"prop2",2},
                {"prop3",new{ sub = 1}},
                {"prop4",new List<string>{"a"}}
            };

            var res = JsonTProvider.AddNodes(json, addNodes,"nodeName");

            var expected = new
            {
                nodeName = new
                {
                    prop1 = 1
                },
                prop2 = 2,
                prop3 = new { sub = 1 },
                prop4 = new List<string> { "a" }
            }.ToJson();

            Assert.Equal(expected, res);
        }

        [Fact]
        public void Add_Nodes_With_Node_Name_Should_Succeed_When_JSON_Is_Array()
        {
            var json = new List<string>
            {
                "a","b","c"
            }.ToJson();

            var addNodes = new Dictionary<string, object>
            {
                {"prop2",2},
                {"prop3",new{ sub = 1}},
                {"prop4",new List<string>{"a"}}
            };

            var res = JsonTProvider.AddNodes(json, addNodes, "nodeName");

            var expected = new
            {
                nodeName = new List<string>
                {
                    "a","b","c"
                },
                prop2 = 2,
                prop3 = new { sub = 1 },
                prop4 = new List<string> { "a" }
            }.ToJson();

            Assert.Equal(expected, res);
        }
    }
}
