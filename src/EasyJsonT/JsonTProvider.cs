namespace EasyJsonT
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// JsonT Provider.
    /// </summary>
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
                CustomizeJOjbect(jToken, nodes, isRemove);
            }
            else if (jToken.Type == JTokenType.Array)
            {
                CustomizeJArray(jToken, nodes, isRemove);
            }

            return jToken.ToString();
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

            var objProp = objProps.GetJSONNodeName(root);

            if (string.IsNullOrWhiteSpace(objProp))
                throw new NotFoundNodeException($"The input json don't has such node named {root}");
                
            //get the value of the property
            var targetObj = @object.Property(objProp).Value;

            if (targetObj.Type == JTokenType.Array)
            {
                CustomizeJArray(targetObj, nodes, isRemove);
            }
            else if (targetObj.Type == JTokenType.Object)
            {
                CustomizeJOjbect(targetObj, nodes, isRemove);
            }

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
        /// Renames the nodes.
        /// </summary>
        /// <returns>The nodes.</returns>
        /// <param name="json">Json.</param>
        /// <param name="nameMap">Name map.</param>
        public static string RenameNodes(string json, Dictionary<string, string> nameMap)
        {
            var jToken = JToken.Parse(json);

            if (jToken.Type == JTokenType.Object)
            {
                return AddAndRemoveNode (jToken, nameMap).ToString();
            }
            else if (jToken.Type == JTokenType.Array)
            {
                var jArray = (JArray)jToken;

                foreach (var item in jArray)
                {
                    if (item.Type != JTokenType.Object) break;

                    AddAndRemoveNode(item, nameMap);
                }

                return jArray.ToString();
            }

            return json;
        }

        /// <summary>
        /// Adds the and remove node.
        /// </summary>
        /// <returns>The and remove node.</returns>
        /// <param name="jToken">J token.</param>
        /// <param name="nameMap">Name map.</param>
        private static JObject AddAndRemoveNode(JToken jToken, Dictionary<string, string> nameMap)
        {
            var jObj = (JObject)jToken;
            var props = jObj.Properties();
            var list = props.GetJSONNodesNameMap(nameMap.Keys);

            foreach (var (o, n) in list)
            {
                var val = jObj.Property(o).Value;
                jObj.Add(nameMap[n], val);
                jObj.Remove(o);
            }
            return jObj;
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

            var list = properties.GetJSONNodesName(nodes);

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

            foreach (var jT in jArray)
            {
                if (jT.Type != JTokenType.Object) break;

                CustomizeJOjbect(jT, nodes, isRemove);
            }

            return jArray;
        }
    }
}
