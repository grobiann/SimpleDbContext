using System;
using UnityEngine;

namespace Pioneer.DB
{
    public struct Entry_DateTime : IComparable, IComparable<DateTime>, IEquatable<DateTime>, IFormattable
    {
        [Newtonsoft.Json.JsonProperty]
        public DateTime Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Newtonsoft.Json.JsonIgnore]
        private DateTime _value;

        public Entry_DateTime(DateTime value) : this()
        {
            SetValue(value);
        }

        public void SetValue(DateTime value)
        {
            _value = value;
        }

        public DateTime GetValue()
        {
            return _value;
        }

        public override string ToString()
        {
            return GetValue().ToString();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return GetValue().ToString(format, formatProvider);
        }

        public int CompareTo(object obj)
        {
            return GetValue().CompareTo(obj);
        }

        public int CompareTo(DateTime other)
        {
            return GetValue().CompareTo(other);
        }

        public bool Equals(DateTime other)
        {
            return GetValue().Equals(other);
        }

        public static implicit operator DateTime(Entry_DateTime value)
        {
            return value.GetValue();
        }
    }
}
