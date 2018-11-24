namespace EasyJsonT
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;

    public static class JsonTProvider
    {
        /// <summary>
        /// Customizes the fields.
        /// </summary>
        /// <returns>The fields.</returns>
        /// <param name="json">Json.</param>
        /// <param name="fields">Fields.</param>
        /// <param name="isRemove">If set to <c>true</c> is remove.</param>
        public static string CustomizeFields(string json, IEnumerable<string> fields, bool isRemove = true)
        {
            var jToken = JToken.Parse(json);

            if (jToken.Type == JTokenType.Object)
            {
                var obj = CustomizeJOjbect(json, fields, isRemove);
                return obj.ToString();
            }
            else if (jToken.Type == JTokenType.Array)
            {
                var arr = CustomizeJArray(json, fields, isRemove);
                return arr.ToString();
            }

            return json;
        }

        /// <summary>
        /// Customizes the fields.
        /// </summary>
        /// <returns>The fields.</returns>
        /// <param name="json">Json.</param>
        /// <param name="property">Property.</param>
        /// <param name="fields">Fields.</param>
        /// <param name="isRemove">If set to <c>true</c> is remove.</param>
        public static string CustomizeFields(string json, string property, IEnumerable<string> fields, bool isRemove = true)
        {
            var jToken = JToken.Parse(json);

            if (jToken.Type != JTokenType.Object)
                throw new ArgumentException($"The input json must be JObject");

            var @object = (JObject)jToken;

            var objProps = @object.Properties();

            var objProp = objProps.FirstOrDefault(x => x.Name.Equals(property, StringComparison.OrdinalIgnoreCase))?.Name;

            if (string.IsNullOrWhiteSpace(objProp))
                throw new NotFoundPropertyException($"The input json don't has such property named {property}");

            var targetResult = string.Empty;
            //get the value of the property
            var targetObj = @object.Property(objProp).Value;

            if (targetObj.Type == JTokenType.Array)
            {
                var jArr = CustomizeJArray(targetObj.ToString(), fields, isRemove);
                targetResult = jArr.ToString();
            }
            else if (targetObj.Type == JTokenType.Object)
            {
                var jObj = CustomizeJOjbect(targetObj.ToString(), fields, isRemove);
                targetResult = jObj.ToString();
            }

            @object.Remove(objProp);
            @object.Add(objProp, JToken.Parse(targetResult));

            return @object.ToString();
        }

        /// <summary>
        /// Customizes the JO jbect.
        /// </summary>
        /// <returns>The JO jbect.</returns>
        /// <param name="json">Json.</param>
        /// <param name="fields">Fields.</param>
        /// <param name="isRemove">If set to <c>true</c> is remove.</param>
        private static JObject CustomizeJOjbect(string json, IEnumerable<string> fields, bool isRemove = true)
        {
            var @object = JObject.Parse(json);

            var properties = @object.Properties();

            var list = properties.SelectMany(jProp => fields, (jProp, name) =>
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
        /// Customizes the JA rray.
        /// </summary>
        /// <returns>The JA rray.</returns>
        /// <param name="json">Json.</param>
        /// <param name="fields">Fields.</param>
        /// <param name="isRemove">If set to <c>true</c> is remove.</param>
        private static JArray CustomizeJArray(string json, IEnumerable<string> fields, bool isRemove = true)
        {
            var jArray = JArray.Parse(json);
            var list = new List<JObject>();

            foreach (var jObj in jArray)
            {
                var obj = CustomizeJOjbect(jObj.ToString(), fields, isRemove);
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
