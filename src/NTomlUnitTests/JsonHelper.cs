using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTomlUnitTests
{
    internal static class JsonHelper
    {
        // http://stackoverflow.com/a/19140420/1086121
        public static object Deserialize(string json)
        {
            return ToObject(JToken.Parse(json));
        }

        private static object ToObject(JToken token)
        {
            if (token.Type == JTokenType.Object)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                foreach (JProperty prop in token)
                {
                    dict.Add(prop.Name, ToObject(prop.Value));
                }
                return dict;
            }
            else if (token.Type == JTokenType.Array)
            {
                List<object> list = new List<object>();
                foreach (JToken value in token)
                {
                    list.Add(ToObject(value));
                }
                return list;
            }
            else if (token.Type == JTokenType.Date)
            {
                // Hacky hack back
                // Can't figure out how to tell JSON.net not to deserialize datetimes
                return ((JValue)token).ToObject<DateTime>().ToString("yyyy-MM-dd'T'HH:mm:ssZ");
            }
            else
            {
                return ((JValue)token).Value;
            }
        }
    }
}
