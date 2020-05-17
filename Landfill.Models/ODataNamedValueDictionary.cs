using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;

namespace Landfill.Models
{
        //enum

        [DataContract]
        public class ODataNamedValueDictionary< TValue> : IDictionary<string, TValue> where TValue : class//TKey, where TKey : struct, Enum
        {
            [DataMember]
            // NOTE: Must be public, otherwise OData Web Api won't serialize anything.
            public IDictionary<string, object> Items
            {
                get
                {
                    return items ?? (items = new Dictionary<string, object>());
                }
                set
                {
                    items = value;
                }
            }
            IDictionary<string, object> items;
            public TValue this[string key]
            {
                get { return (TValue)Items[key]; }
                set { Items[key] = value; }
            }
            public ICollection<string> Keys
            {
                get { return Items.Keys; }
            }

            public ICollection<TValue> Values
            {
                get { return new ReadOnlyCollection<TValue>(Items.Values.Cast<TValue>().ToList()); }
            }
            //https://github.com/OData/WebApi/issues/438
            //https://github.com/OData/WebApi/blob/master/src/Microsoft.AspNet.OData.Shared/Builder/PropertySelectorVisitor.cs
            //https://github.com/OData/WebApi/blob/de7dcd076385664768841856d3a1db1b7ce96b30/src/Microsoft.AspNet.OData.Shared/Builder/StructuralTypeConfiguration.cs

            public int Count
            {
                get
                {
                    return Items.Count;
                }
            }
            public bool IsReadOnly
            {
                get { return Items.IsReadOnly; }
            }


            public void Add(string key, TValue value)
            {
                Items.Add(key, value);
            }

            public void Add(KeyValuePair<string, TValue> item)
            {
                Items.Add(ConvertToObjectDict(item));
            }
            KeyValuePair<string, object> ConvertToObjectDict(KeyValuePair<string, TValue> item)
            {
                return new KeyValuePair<string, object>(item.Key, item.Value);
            }
            private string Convert(KeyValuePair<string, TValue> item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                Items.Clear();
            }

            public bool Contains(KeyValuePair<string, TValue> item)
            {
                return Items.Contains(ConvertToObjectDict(item));
            }

            public bool ContainsKey(string key)
            {
                return Items.ContainsKey(key);
            }

            public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
            {
                if (array == null) throw new ArgumentNullException(nameof(array));
                if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
                if (Items.Count > array.Length - arrayIndex)
                    throw new ArgumentException("The number of elements in the source dictionary " +
                        "is greater than the available space from arrayIndex to the end of the destination array.",
                        nameof(arrayIndex));

                var i = 0;
                foreach (var item in Items)
                {
                    array[i] = Convert(item);
                    i++;
                }
            }

            public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
            {
                foreach (var item in Items)
                    yield return Convert(item);
            }
            public bool Remove(string key)
            {
                return Items.Remove(key);
            }

            public bool Remove(KeyValuePair<string, TValue> item)
            {
                return Items.Remove(Convert(item));
            }

        
            KeyValuePair<string, TValue> Convert(KeyValuePair<string, object> item)
            {
                return new KeyValuePair<string, TValue>(item.Key, (TValue)item.Value);
            }
            public bool TryGetValue(string key, [MaybeNullWhen(false)] out TValue value)
            {
                object obj;
                if (Items.TryGetValue(key, out obj))
                {
                    value = (TValue)obj;
                    return true;
                }

                value = default(TValue);
                return false;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }



    
}
