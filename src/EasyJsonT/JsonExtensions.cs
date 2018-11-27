namespace EasyJsonT
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;

    public static class JsonExtensions
    {
        /// <summary>
        /// Gets the name of the JSON Node.
        /// </summary>
        /// <returns>The JSON Node name.</returns>
        /// <param name="properties">Properties.</param>
        /// <param name="name">Name.</param>
        /// <param name="comparison">Comparison.</param>
        public static string GetJSONNodeName(this IEnumerable<JProperty> properties, string name, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            var nodeName = string.Empty;
            if (properties != null && properties.Any())
            {
                nodeName = properties.FirstOrDefault(p => p.Name.Equals(name, comparison))?.Name;
            }
            return nodeName;
        }

        /// <summary>
        /// Gets the name of the JSONN odes.
        /// </summary>
        /// <returns>The JSONN odes name.</returns>
        /// <param name="properties">Properties.</param>
        /// <param name="nameList">Name list.</param>
        /// <param name="comparison">Comparison.</param>
        public static List<string> GetJSONNodesName(this IEnumerable<JProperty> properties, IEnumerable<string> nameList, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            if (properties != null && properties.Any())
            {
                return properties.SelectMany(jProp => nameList, (jProp, name) =>
                {
                    return jProp.Name.Equals(name, StringComparison.OrdinalIgnoreCase) ? jProp.Name : string.Empty;
                }).Where(x => !string.IsNullOrEmpty(x)).ToList();
            }
            else
            {
                return new List<string>();
            }
        }

    }
}
