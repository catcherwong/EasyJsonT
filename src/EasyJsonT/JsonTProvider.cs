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

            if(!@object.TryGetValue(root,StringComparison.OrdinalIgnoreCase,out var targetObj))
                throw new NotFoundNodeException($"The input json don't has such node named {root}");
                           
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
        /// <param name="nodeName">The node name of origin JSON.</param>
        public static string AddNodes(string json, Dictionary<string, object> dict, string nodeName = "")
        {
            var jToken = JToken.Parse(json);

            var isOriginPrpo = !string.IsNullOrWhiteSpace(nodeName);

            if (!isOriginPrpo && jToken.Type != JTokenType.Object)
                throw new NotSpecialJsonTypeException($"The type of input JSON isn't an object.");
                
            var @object = new JObject();

            if (!isOriginPrpo) @object.Merge(jToken);

            if (isOriginPrpo && !@object.ContainsKey(nodeName)) @object.Add(nodeName, jToken);

            foreach (var item in dict)
            {
                //filter the same key.
                if (!@object.ContainsKey(item.Key))
                {
                    @object.Add(item.Key, JToken.FromObject(item.Value));
                }
            }

            return @object.ToString();
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
                return AddAndRemoveNode(jToken, nameMap).ToString();
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
        /// Renames the nodes.
        /// </summary>
        /// <returns>The nodes.</returns>
        /// <param name="json">Json.</param>
        /// <param name="root">Root.</param>
        /// <param name="nameMap">Name map.</param>
        public static string RenameNodes(string json, string root ,Dictionary<string, string> nameMap)
        {
            var jToken = JToken.Parse(json);

            if (jToken.Type != JTokenType.Object)
                throw new NotSpecialJsonTypeException($"The type of input JSON isn't an object.");

            var @object = (JObject)jToken;

            if (!@object.TryGetValue(root, StringComparison.OrdinalIgnoreCase, out var targetObj))
                throw new NotFoundNodeException($"The input json don't has such node named {root}");

            if (targetObj.Type == JTokenType.Array)
            {
                var jArray = (JArray)targetObj;

                foreach (var item in jArray)
                {
                    if (item.Type != JTokenType.Object) break;

                    AddAndRemoveNode(item, nameMap);
                }
            }
            else if (targetObj.Type == JTokenType.Object)
            {
                AddAndRemoveNode(targetObj, nameMap).ToString();
            }

            return @object.ToString();
        }

        /// <summary>
        /// Translates the values.
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="json">Json.</param>
        /// <param name="nodeValMap">Node value map.</param>
        public static string TranslateValues(string json, Dictionary<string, Dictionary<object, object>> nodeValMap)
        {
            var jToken = JToken.Parse(json);

            if (jToken.Type == JTokenType.Object)
            {
                return Translate(jToken, nodeValMap).ToString();
            }
            else if (jToken.Type == JTokenType.Array)
            {
                var jArray = (JArray)jToken;

                foreach (var item in jArray)
                {
                    if (item.Type != JTokenType.Object) break;

                    Translate(item, nodeValMap);
                }

                return jArray.ToString();
            }

            return json;
        }

        /// <summary>
        /// Translates the values with special root.
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="json">Json.</param>
        /// <param name="root">Root.</param>
        /// <param name="nodeValMap">Node value map.</param>
        public static string TranslateValues(string json, string root, Dictionary<string, Dictionary<object, object>> nodeValMap)
        {
            var jToken = JToken.Parse(json);

            if (jToken.Type != JTokenType.Object)
                throw new NotSpecialJsonTypeException($"The input json must be JObject");

            var @object = (JObject)jToken;

            if(!@object.TryGetValue(root,StringComparison.OrdinalIgnoreCase,out var targetObj))            
                throw new NotFoundNodeException($"The input json don't has such node named {root}");

            if (targetObj.Type == JTokenType.Array)
            {
                var jArray = (JArray)targetObj;

                foreach (var item in jArray)
                {
                    if (item.Type != JTokenType.Object) break;

                    Translate(item, nodeValMap);
                }
            }
            else if (targetObj.Type == JTokenType.Object)
            {
                Translate(targetObj, nodeValMap).ToString();
            }

            return @object.ToString();
        }

        #region Private Methods          
        /// <summary>
        /// Translate the specified jToken and nodeValMap.
        /// </summary>
        /// <returns>The translate.</returns>
        /// <param name="jToken">J token.</param>
        /// <param name="nodeValMap">Node value map.</param>
        private static JObject Translate(JToken jToken, Dictionary<string, Dictionary<object, object>> nodeValMap)
        {
            var @object = (JObject)jToken;
            var props = @object.Properties();

            foreach (var item in nodeValMap)
            {
                var nodeName = props.GetJSONNodeName(item.Key);

                if (!string.IsNullOrWhiteSpace(nodeName))
                {
                    var nodeValue = @object.Property(nodeName).Value;

                    if (nodeValue.Type != JTokenType.Object && nodeValue.Type != JTokenType.Array)
                    {
                        var valMap = item.Value;

                        foreach (var val in valMap)
                        {
                            if (nodeValue.Equals(JToken.FromObject(val.Key)))
                            {
                                @object.Property(nodeName).Value = JToken.FromObject(val.Value);
                            }
                        }
                    }
                }
            }

            return @object;
        }

        /// <summary>
        /// Adds the and remove node.
        /// </summary>
        /// <returns>The and remove node.</returns>
        /// <param name="jToken">J token.</param>
        /// <param name="nameMap">Name map.</param>
        private static JObject AddAndRemoveNode(JToken jToken, Dictionary<string, string> nameMap)
        {
            var @object = (JObject)jToken;
            var props = @object.Properties();
            var list = props.GetJSONNodesNameMap(nameMap.Keys);

            foreach (var (o, n) in list)
            {
                var val = @object.Property(o).Value;

                if (!@object.ContainsKey(nameMap[n]))
                {
                    @object.Add(nameMap[n], val);
                    @object.Remove(o);
                }
            }
            return @object;
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
        #endregion
    }
}
