using NToml.Values;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml.Dynamic
{
    internal class DynamicVisitor : IValueVisitor
    {
        private readonly Action<dynamic> callback;

        public DynamicVisitor(Action<dynamic> callback)
        {
            this.callback = callback;
        }

        public void Deserialize(bool value)
        {
            this.callback(value);
        }

        public void Deserialize(DateTime value)
        {
            this.callback(value);
        }

        public void Deserialize(double value)
        {
            this.callback(value);
        }

        public void Deserialize(long value)
        {
            this.callback(value);
        }

        public void Deserialize(string value)
        {
            this.callback(value);
        }

        public void Deserialize(IEnumerable<IValue> values)
        {
            var valueItems = new List<dynamic>();
            var visitor = new DynamicVisitor(value =>
            {
                valueItems.Add(value);
            });

            foreach (var value in values)
            {
                value.Visit(visitor);
            }

            this.callback(valueItems);
        }

        public void Deserialize(IDictionary<string, Values.IValue> values)
        {
            var objects = new ExpandoObject();
            IDictionary<string, object> dict = objects;

            foreach (var kvp in values)
            {
                var visitor = new DynamicVisitor(value =>
                {
                    dict.Add(kvp.Key, value);
                });
                kvp.Value.Visit(visitor);
            }

            this.callback(objects);
        }
    }
}
