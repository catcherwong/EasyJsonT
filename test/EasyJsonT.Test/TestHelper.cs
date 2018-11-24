using System;
namespace EasyJsonT.Test
{
    using Newtonsoft.Json;

    public static class TestHelper
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj,Formatting.Indented);
        }
    }
}
