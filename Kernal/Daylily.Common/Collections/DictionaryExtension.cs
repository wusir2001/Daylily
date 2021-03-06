﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daylily.Common.Collections
{
    public static class DictionaryExtension
    {
        public static IEnumerable<TValue> RandomValues<TKey, TValue>(this IDictionary<TKey, TValue> dict,
            ConcurrentRandom rand = null)
        {
            rand = rand ?? new ConcurrentRandom();
            List<TValue> values = Enumerable.ToList(dict.Values);
            int size = dict.Count;
            while (true)
            {
                yield return values[rand.Next(size)];
            }
        }
    }
}
