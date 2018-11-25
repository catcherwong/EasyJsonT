namespace EasyJsonT.Test
{
    using Xunit;
    using EasyJsonT;
    using System.Collections.Generic;

    public partial class FilterNodesTest
    {
        [Fact]
        public void JObject_Remove_Test_Should_Succeed()
        {
            var json = new 
            {
                prop1 = 1,
                prop2 = "2",
                prop3 = new{ sub1 = 1},
                prop4 = new List<string> { "a", "b" }
            }.ToJson();

            var fields = new List<string> { "prop1", "prop3"  };

            var actual = JsonTProvider.FilterNodes(json, fields, true);

            var expected = new
            {
                prop2 = "2",
                prop4 = new List<string> { "a", "b" }
            }.ToJson();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void JObject_Save_Test_Should_Succeed()
        {
            var json = new
            {
                prop1 = 1,
                prop2 = "2",
                prop3 = new { sub1 = 1 },
                prop4 = new List<string> { "a", "b" }
            }.ToJson();

            var fields = new List<string> { "prop1", "prop3" };

            var actual = JsonTProvider.FilterNodes(json, fields, false);

            var expected = new
            {
                prop1 = 1,
                prop3 = new { sub1 = 1 }
            }.ToJson();

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void JArray_Remove_Test_Should_Succeed()
        {
            var json = new List<dynamic>
            {
                new 
                {
                    prop1 = "a1",
                    prop2 = 12
                },
                new
                {
                    prop1 = "a2",
                    prop2 = 22
                }
            }
           .ToJson();

            var fields = new List<string> { "prop1" };

            var actual = JsonTProvider.FilterNodes(json, fields, true);

            var expected = new List<dynamic>
            {
                new
                {
                    prop2 = 12
                },
                new
                {
                    prop2 = 22
                }
            }
           .ToJson();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void JArray_Save_Test_Should_Succeed()
        {
            var json = new List<dynamic>
            {
                new
                {
                    prop1 = "a1",
                    prop2 = 12
                },
                new
                {
                    prop1 = "a2",
                    prop2 = 22
                }
            }
           .ToJson();

            var fields = new List<string> { "prop1" };

            var actual = JsonTProvider.FilterNodes(json, fields, false);

            var expected = new List<dynamic>
            {
                new
                {
                    prop1 = "a1",
                },
                new
                {
                    prop1 = "a2",
                }
            }
           .ToJson();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void JObject_IgnoreCase_Test_Should_Succeed()
        {
            var json = new
            {
                Prop1 = 1,
                prop2 = "2",
                prop3 = new { sub1 = 1 },
                Prop4 = new List<string> { "a", "b" }
            }.ToJson();

            var fields = new List<string> { "prop1", "prop3" };

            var actual = JsonTProvider.FilterNodes(json, fields, true);

            var expected = new
            {
                prop2 = "2",
                Prop4 = new List<string> { "a", "b" }
            }.ToJson();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void JArray_IgnoreCase_Test_Should_Succeed()
        {
            var json = new List<dynamic>
            {
                new
                {
                    Prop1 = "a1",
                    prop2 = 12
                },
                new
                {
                    Prop1 = "a2",
                    prop2 = 22
                }
            }
           .ToJson();

            var fields = new List<string> { "prop1" };

            var actual = JsonTProvider.FilterNodes(json, fields, false);

            var expected = new List<dynamic>
            {
                new
                {
                    Prop1 = "a1",
                },
                new
                {
                    Prop1 = "a2",
                }
            }
           .ToJson();

            Assert.Equal(expected, actual);
        }

    }
}
