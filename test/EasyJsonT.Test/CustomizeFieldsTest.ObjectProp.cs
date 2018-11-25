namespace EasyJsonT.Test
{
    using Xunit;
    using EasyJsonT;
    using System.Collections.Generic;
    using System;

    public partial class CustomizeFieldsTest
    {
        [Fact]
        public void JObject_With_JObject_Prop_Remove_Test_Should_Succeed()
        {
            var json = new
            {
                code = 1,
                msg = "2",
                data = new
                {
                    prop1 = 1,
                    prop2 = "2",
                },
            }.ToJson();

            var fields = new List<string> { "prop1" };

            var actual = JsonTProvider.CustomizeFields(json, "data", fields, true);

            var expected = new
            {
                code = 1,
                msg = "2",
                data = new
                {
                    prop2 = "2",
                },
            }.ToJson();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void JObject_With_JObject_Prop_Save_Test_Should_Succeed()
        {
            var json = new
            {
                code = 1,
                msg = "2",
                data = new
                {
                    prop1 = 1,
                    prop2 = "2",
                },
            }.ToJson();

            var fields = new List<string> { "prop1" };

            var actual = JsonTProvider.CustomizeFields(json, "data", fields, false);

            var expected = new
            {
                code = 1,
                msg = "2",
                data = new
                {
                    prop1 = 1,
                },
            }.ToJson();

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void JObject_With_JArray_Prop_Remove_Test_Should_Succeed()
        {
            var json = new
            {
                code = 1,
                msg = "2",
                data = new List<dynamic>
                {
                    new { prop1 = 11, prop2 = "12"},
                    new { prop1 = 21, prop2 = "22"},
                },
            }.ToJson();

            var fields = new List<string> { "prop1" };

            var actual = JsonTProvider.CustomizeFields(json, "data", fields, true);

            var expected = new
            {
                code = 1,
                msg = "2",
                data = new List<dynamic>
                {
                    new {  prop2 = "12"},
                    new {  prop2 = "22"},
                },
            }.ToJson();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void JObject_With_JArray_Prop_Save_Test_Should_Succeed()
        {
            var json = new
            {
                code = 1,
                msg = "2",
                data = new List<dynamic>
                {
                    new { prop1 = 11, prop2 = "12"},
                    new { prop1 = 21, prop2 = "22"},
                },
            }.ToJson();

            var fields = new List<string> { "prop1" };

            var actual = JsonTProvider.CustomizeFields(json, "data", fields, false);

            var expected = new
            {
                code = 1,
                msg = "2",
                data = new List<dynamic>
                {
                    new { prop1 = 11},
                    new { prop1 = 21},
                },
            }.ToJson();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void JObject_With_Prop_Should_Throw_ArgumentException_When_Input_Is_Not_Object()
        {
            var json = new List<dynamic>
                {
                    new { prop1 = 11, prop2 = "12"},
                }.ToJson();

            var fields = new List<string> { "prop1" };

            Assert.Throws<ArgumentException>(() => JsonTProvider.CustomizeFields(json, "data", fields, false));
        }

        [Fact]
        public void JObject_With_Prop_Should_Throw_NotFoundPropertyException_When_Input_DoNot_Contain_Prop()
        {
            var json = new
            {
                code = 1,
                msg = "2",
                data = new List<dynamic>
                {
                    new { prop1 = 11, prop2 = "12"},
                    new { prop1 = 21, prop2 = "22"},
                },
            }.ToJson();

            var fields = new List<string> { "prop1" };

            Assert.Throws<NotFoundNodeException>(() => JsonTProvider.CustomizeFields(json, "obj", fields, false));
        }
    }
}