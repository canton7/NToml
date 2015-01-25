using NToml;
using NToml.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomlTestDecoder
{
    public class ObjectVisitor : IValueVisitor
    {
        private readonly Action<object> callback;

        public ObjectVisitor(Action<object> callback)
        {
            this.callback = callback;
        }

        private static Dictionary<string, object> createValue(string type, string value)
        {
            return new Dictionary<string, object>()
            {
                { "type", type },
                { "value", value },
            };
        }

        public void Deserialize(IEnumerable<IValue> values)
        {
            var valueItems = new List<object>();
            var visitor = new ObjectVisitor(value =>
            {
                valueItems.Add(value);
            });

            foreach (var value in values)
            {
                value.Visit(visitor);
            }

            // Now. Because toml-test is stupid, arrays of tables are different to arrays
            // of anything else...
            var first = values.FirstOrDefault();
            if (first != null && first.Type == TomlValueType.Table)
            {
                this.callback(valueItems);
            }
            else
            {
                this.callback(new Dictionary<string, object>()
                {
                    { "type", "array" },
                    { "value", valueItems },
                });
            }
        }

        public void Deserialize(bool value)
        {
            this.callback(createValue("bool", value ? "true" : "false"));
        }

        public void Deserialize(DateTime value)
        {
            this.callback(createValue("datetime", value.ToString("yyyy-MM-dd'T'HH:mm:ssZ")));
        }

        public void Deserialize(double value)
        {
            this.callback(createValue("float", value.ToString()));
        }

        public void Deserialize(long value)
        {
            this.callback(createValue("integer", value.ToString()));
        }

        public void Deserialize(string value)
        {
            this.callback(createValue("string", value.ToString()));
        }

        public void Deserialize(IDictionary<string, IValue> values)
        {
            var objects = new Dictionary<string, object>();

            foreach (var kvp in values)
            {
                var visitor = new ObjectVisitor(value =>
                {
                    objects.Add(kvp.Key, value);
                });
                kvp.Value.Visit(visitor);
            }

            this.callback(objects);
        }
    }
}
