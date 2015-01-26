using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace NTomlUnitTests
{
    class AssertHelpers : Assert
    {
        public static T ThrowsDerived<T>(Assert.ThrowsDelegateWithReturn testCode) where T : Exception
        {
            Exception exception = null;
            try
            {
                testCode();
            }
            catch (Exception e)
            {
                exception = e;
            }

            if (!(exception is T))
            {
                throw new AssertException(String.Format("Expected exception of type {0} (or subclass), but got {1}", typeof(T), exception.GetType()));
            }

            return (T)exception;
        }

        public static void ObjectsEqual(Dictionary<string, object> expected, Dictionary<string, object> actual)
        {
            ObjectsEqualInternal(expected, actual);
        }

        private static void ObjectsEqualInternal(object expected, object actual)
        {
            if (expected.GetType() != actual.GetType())
                throw new AssertException(String.Format("Expected node of type {0}, but got {1}", expected.GetType(), actual.GetType()));

            if (expected is Dictionary<string, object>)
            {
                var expectedDict = (Dictionary<string, object>)expected;
                var actualDict = (Dictionary<string, object>)actual;

                // Dicts can either have a type/value pair, or actually be a dictionary (confusing, right?)
                // Assume the 'expected' dictionary is well-formed
                if (expectedDict.ContainsKey("type"))
                {
                    if (!actualDict.ContainsKey("type") || !actualDict.ContainsKey("value"))
                        throw new AssertException("Expected object to contain keys 'type' and 'value', but it didn't");
                    if ((string)expectedDict["type"] != (string)actualDict["type"])
                        throw new AssertException(String.Format("Expected node of type {0}, but got type {1}", expectedDict["type"], actualDict["type"]));
                    ObjectsEqualInternal(expectedDict["value"], actualDict["value"]);
                }
                else
                {
                    foreach (var kvp in expectedDict)
                    {
                        if (!actualDict.ContainsKey(kvp.Key))
                            throw new AssertException(String.Format("Expected object to contain key {0}, but it didn't", kvp.Key));
                        ObjectsEqualInternal(kvp.Value, actualDict[kvp.Key]);
                    }
                }
            }
            else if (expected is List<object>)
            {
                var expectedList = (List<object>)expected;
                var actualList = (List<object>)actual;

                if (expectedList.Count != actualList.Count)
                    throw new AssertException(String.Format("Expected list to contain {0} elements, but it actually contains {1}", expectedList.Count, actualList.Count));

                for (int i = 0; i < expectedList.Count; i++)
                {
                    ObjectsEqualInternal(expectedList[i], actualList[i]);
                }
            }
            else if (expected is string)
            {
                Assert.Equal((string)expected, (string)actual);
            }
            else
            {
                throw new AssertException(String.Format("Unexpected node {0}", expected.GetType()));
            }
        }
    }
}
