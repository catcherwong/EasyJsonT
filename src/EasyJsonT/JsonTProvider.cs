namespace EasyJsonT
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;

    public static class JsonTProvider
    {
        /// <summary>
        /// Filters the nodes.
        /// </summary>
        /// <returns>The reconstructed JSON string.</returns>
        /// <param name="json">The input JSON string.</param>
        /// <param name="nodes">The nodes need to be filtered.</param>
        /// <param name="isRemove">If set to <c>true</c> is remove.</param>
        public static string FilterNodes(string json, IEnumerable<string> nodes, bool isRemove = true)
        {
            var jToken = JToken.Parse(json);

            if (jToken.Type == JTokenType.Object)
            {
                var obj = CustomizeJOjbect(jToken, nodes, isRemove);
                return obj.ToString();
            }
            else if (jToken.Type == JTokenType.Array)
            {
                var arr = CustomizeJArray(jToken, nodes, isRemove);
                return arr.ToString();
            }

            return json;
        }

        /// <summary>
        /// Filters the nodes.
        /// </summary>
        /// <returns>The reconstructed JSON string.</returns>
        /// <param name="json">The input JSON string.</param>
        /// <param name="root">The name of root node.</param>
        /// <param name="nodes">The nodes need to be filtered.</param>
        /// <param name="isRemove">If set to <c>true</c> is remove.</param>
        public static string FilterNodes(string json, string root, IEnumerable<string> nodes, bool isRemove = true)
        {
            var jToken = JToken.Parse(json);

            if (jToken.Type != JTokenType.Object)
                throw new ArgumentException($"The input json must be JObject");

            var @object = (JObject)jToken;

            var objProps = @object.Properties();

            var objProp = objProps.FirstOrDefault(x => x.Name.Equals(root, StringComparison.OrdinalIgnoreCase))?.Name;

            if (string.IsNullOrWhiteSpace(objProp))
                throw new NotFoundNodeException($"The input json don't has such node named {root}");

            var targetResult = string.Empty;
            //get the value of the property
            var targetObj = @object.Property(objProp).Value;

            if (targetObj.Type == JTokenType.Array)
            {
                var jArr = CustomizeJArray(targetObj, nodes, isRemove);
                targetResult = jArr.ToString();
            }
            else if (targetObj.Type == JTokenType.Object)
            {
                var jObj = CustomizeJOjbect(targetObj, nodes, isRemove);
                targetResult = jObj.ToString();
            }

            @object.Remove(objProp);
            @object.Add(objProp, JToken.Parse(targetResult));

            return @object.ToString();
        }

        /// <summary>
        /// Adds nodes for input JSON string.
        /// </summary>
        /// <returns>The reconstructed JSON string.</returns>
        /// <param name="json">The input JSON string.</param>
        /// <param name="dict">The nodes need to add.</param>
        public static string AddNodes(string json, Dictionary<string, object> dict)
        {
            var jToken = JToken.Parse(json);

            if (jToken.Type != JTokenType.Object)
                throw new NotSpecialJsonTypeException($"The type of input JSON isn't an object.");
                
            var jObj = (JObject)jToken;

            foreach (var item in dict)
            {
                //filter the same key.
                if(!jObj.ContainsKey(item.Key))
                {
                    jObj.Add(item.Key, JToken.FromObject(item.Value));
                }
            }

            return jObj.ToString();
        }

        /// <summary>
        /// Customizes the JOjbect.
        /// </summary>
        /// <returns>The JO jbect.</returns>
        /// <param name="jToken">jToken.</param>
        /// <param name="nodes">Nodes.</param>
        /// <param name="isRemove">If set to <c>true</c> is remove.</param>
        private static JObject CustomizeJOjbect(JToken jToken, IEnumerable<string> nodes, bool isRemove = true)
        {
            var @object = (JObject)jToken;

            var properties = @object.Properties();

            var list = properties.SelectMany(jProp => nodes, (jProp, name) =>
            {
                return jProp.Name.Equals(name, StringComparison.OrdinalIgnoreCase) ? jProp.Name : string.Empty;
            }).Where(x => !string.IsNullOrEmpty(x)).ToList();

            if (!isRemove)
            {
                list = properties.Select(x => x.Name).Except(list).ToList();
            }

            foreach (var item in list)
            {
                @object.Remove(item);
            }

            return @object;

        }

        /// <summary>
        /// Customizes the JArray.
        /// </summary>
        /// <returns>The JA rray.</returns>
        /// <param name="jToken">jToken.</param>
        /// <param name="nodes">Nodes.</param>
        /// <param name="isRemove">If set to <c>true</c> is remove.</param>
        private static JArray CustomizeJArray(JToken jToken, IEnumerable<string> nodes, bool isRemove = true)
        {
            var jArray = (JArray)jToken;

            var list = new List<JToken>();

            foreach (var jObj in jArray)
            {
                var obj = CustomizeJOjbect(jObj, nodes, isRemove);
                list.Add(obj);
            }

            jArray.Clear();

            foreach (var item in list)
            {
                jArray.Add(item);
            }
            return jArray;
        }
    }
}
