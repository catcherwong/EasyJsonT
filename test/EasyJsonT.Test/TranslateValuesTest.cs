namespace EasyJsonT.Test
{
    using EasyJsonT;
    using System.Collections.Generic;
    using Xunit;

    public class TranslateTestValues
    {
        [Theory]
        [InlineData(100, "ok")]
        [InlineData(200, "error")]
        public void Translate_Values_For_JObject_Should_Succeed(int code, string msg)
        {
            var json = new
            {
                code,
                msg,
            }.ToJson();

            var valMap = new Dictionary<string, Dictionary<object, object>>
            {
                {"code",new Dictionary<object, object>{{100,0},{200,-1}}},
                {"msg",new Dictionary<object, object>{{"ok","yes"},{"error","no"}}}
            };

            var res = JsonTProvider.TranslateValues(json, valMap);

            var expected = new
            {
                code = code == 100 ? 0 : -1,
                msg = msg == "ok" ? "yes" : "no",
            }.ToJson();

            Assert.Equal(expected, res);
        }


        [Fact]
        public void Translate_Values_For_JArray_Should_Succeed()
        {
            var json = new List<dynamic>
            {
                new
                {
                    prop1 = true,
                    prop2 = 99
                },
                new
                {
                    prop1 = false,
                    prop2 = -99
                }
            }
            .ToJson();

            var valMap = new Dictionary<string, Dictionary<object, object>>
            {
                {"prop1",new Dictionary<object, object>{{false,0},{true,1}}},
                {"prop2",new Dictionary<object, object>{{-99,99}}}
            };

            var res = JsonTProvider.TranslateValues(json, valMap);

            var expected = new List<dynamic>
            {
                new
                {
                    prop1 = 1,
                    prop2 = 99
                },
                new
                {
                    prop1 = 0,
                    prop2 = 99
                }
            }.ToJson();

            Assert.Equal(expected, res);
        }

        [Fact]
        public void Translate_Values_With_JObject_Node_Should_Succeed()
        {
            var json = new
            {
                code = 0,
                msg = "ok",
                data = new
                {
                    prop1 = true,
                    prop2 = 99
                }
            }
            .ToJson();

            var valMap = new Dictionary<string, Dictionary<object, object>>
            {
                {"prop1",new Dictionary<object, object>{{false,0},{true,1}}},
                {"prop2",new Dictionary<object, object>{{-99,99}}}
            };

            var res = JsonTProvider.TranslateValues(json, "data", valMap);

            var expected = new
            {
                code = 0,
                msg = "ok",
                data = new
                {
                    prop1 = 1,
                    prop2 = 99
                }
            }.ToJson();

            Assert.Equal(expected, res);
        }

        [Fact]
        public void Translate_Values_With_JArray_Node_Should_Succeed()
        {
            var json = new
            {
                code = 0,
                msg ="ok",
                data = new List<dynamic>
                {
                    new
                    {
                        prop1 = true,
                        prop2 = 99
                    },
                    new
                    {
                        prop1 = false,
                        prop2 = -99
                    }
                }
            }
            .ToJson();

            var valMap = new Dictionary<string, Dictionary<object, object>>
            {
                {"prop1",new Dictionary<object, object>{{false,0},{true,1}}},
                {"prop2",new Dictionary<object, object>{{-99,99}}}
            };

            var res = JsonTProvider.TranslateValues(json, "data" ,valMap);

            var expected = new
            {
                code = 0,
                msg = "ok",
                data = new List<dynamic>
                {
                    new
                    {
                        prop1 = 1,
                        prop2 = 99
                    },
                    new
                    {
                        prop1 = 0,
                        prop2 = 99
                    }
                }
            }.ToJson();

            Assert.Equal(expected, res);
        }

        [Fact]
        public void Translate_Values_With_Node_Should_Throw_NotSpecialJsonTypeException_When_Input_Is_Not_Object()
        {
            var json = 1.ToJson();

            var valMap = new Dictionary<string, Dictionary<object, object>>
            {
                {"prop1",new Dictionary<object, object>{{false,0},{true,1}}},
                {"prop2",new Dictionary<object, object>{{-99,99}}}
            };

            Assert.Throws<NotSpecialJsonTypeException>(() => JsonTProvider.TranslateValues(json, "obj", valMap));
        }

        [Fact]
        public void Translate_Values_With_Node_Should_Throw_NotFoundPropertyException_When_Input_DoNot_Contain_Prop()
        {
            var json = new
            {
                code = 0,
                msg = "ok",
                data = new List<dynamic>
                {
                    new
                    {
                        prop1 = true,
                        prop2 = 99
                    },
                    new
                    {
                        prop1 = false,
                        prop2 = -99
                    }
                }
            }
            .ToJson();

            var valMap = new Dictionary<string, Dictionary<object, object>>
            {
                {"prop1",new Dictionary<object, object>{{false,0},{true,1}}},
                {"prop2",new Dictionary<object, object>{{-99,99}}}
            };

            Assert.Throws<NotFoundNodeException>(() => JsonTProvider.TranslateValues(json, "obj", valMap));
        }
    }
}
