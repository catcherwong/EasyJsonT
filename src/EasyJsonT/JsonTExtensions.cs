namespace EasyJsonT
{
    using System.Collections.Generic;

    public static class JsonTExtensions
    {
        /// <summary>
        /// Filters the nodes.
        /// </summary>
        /// <returns>The nodes.</returns>
        /// <param name="json">Json.</param>
        /// <param name="nodes">Nodes.</param>
        /// <param name="isRemove">If set to <c>true</c> is remove.</param>
        public static string FilterNodes(this string json, IEnumerable<string> nodes, bool isRemove = true)
        {
            return JsonTProvider.FilterNodes(json, nodes, isRemove);
        }

        /// <summary>
        /// Filters the nodes.
        /// </summary>
        /// <returns>The nodes.</returns>
        /// <param name="json">Json.</param>
        /// <param name="root">Root.</param>
        /// <param name="nodes">Nodes.</param>
        /// <param name="isRemove">If set to <c>true</c> is remove.</param>
        public static string FilterNodes(this string json, string root ,IEnumerable<string> nodes, bool isRemove = true)
        {
            return JsonTProvider.FilterNodes(json, root ,nodes, isRemove);
        }

        /// <summary>
        /// Adds the nodes.
        /// </summary>
        /// <returns>The nodes.</returns>
        /// <param name="json">Json.</param>
        /// <param name="dict">Dict.</param>
        /// <param name="nodeName">Node name.</param>
        public static string AddNodes(this string json, Dictionary<string, object> dict, string nodeName = "")
        {
            return JsonTProvider.AddNodes(json, dict, nodeName);
        }

        /// <summary>
        /// Renames the nodes.
        /// </summary>
        /// <returns>The nodes.</returns>
        /// <param name="json">Json.</param>
        /// <param name="nameMap">Name map.</param>
        public static string RenameNodes(this string json, Dictionary<string, string> nameMap)
        {
            return JsonTProvider.RenameNodes(json, nameMap);
        }

        /// <summary>
        /// Renames the nodes.
        /// </summary>
        /// <returns>The nodes.</returns>
        /// <param name="json">Json.</param>
        /// <param name="root">Root.</param>
        /// <param name="nameMap">Name map.</param>
        public static string RenameNodes(this string json, string root , Dictionary<string, string> nameMap)
        {
            return JsonTProvider.RenameNodes(json, root ,nameMap);
        }

        /// <summary>
        /// Translates the values.
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="json">Json.</param>
        /// <param name="nodeValMap">Node value map.</param>
        public static string TranslateValues(this string json, Dictionary<string, Dictionary<object, object>> nodeValMap)
        {
            return JsonTProvider.TranslateValues(json, nodeValMap);
        }

        /// <summary>
        /// Translates the values.
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="json">Json.</param>
        /// <param name="root">Root.</param>
        /// <param name="nodeValMap">Node value map.</param>
        public static string TranslateValues(this string json, string root ,Dictionary<string, Dictionary<object, object>> nodeValMap)
        {
            return JsonTProvider.TranslateValues(json, root ,nodeValMap);
        }
    }
}
