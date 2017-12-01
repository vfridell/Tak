using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib.Helpers
{
    public static class ExtensionMethods
    {
        public static TValue GetOrCreateValue<TKey, TValue>
    (this IDictionary<TKey, TValue> dictionary,
     TKey key,
     TValue value)
        {
            return dictionary.GetOrCreateValue(key, () => value);
        }

        public static TValue GetOrCreateValue<TKey, TValue>
            (this IDictionary<TKey, TValue> dictionary,
             TKey key,
             Func<TValue> valueProvider)
        {
            TValue ret;
            if (!dictionary.TryGetValue(key, out ret))
            {
                ret = valueProvider();
                dictionary[key] = ret;
            }
            return ret;
        }

        public static void CreateOrAddOne<TKey>(this Dictionary<TKey, int> dictionary, TKey key)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary[key] = 1;
            }
            else
            {
                dictionary[key] += 1;
            }
        }
    }
}
