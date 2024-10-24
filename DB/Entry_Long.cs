using System;
using UnityEngine;

namespace Pioneer.DB
{
    public struct Entry_Long : IComparable, IComparable<long>, IEquatable<long>, IFormattable
    {
        [Newtonsoft.Json.JsonProperty]
        public long Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Newtonsoft.Json.JsonIgnore]
        private long _number;

        [Newtonsoft.Json.JsonIgnore]
        private long _rndCrypt;

        public Entry_Long(long value) : this()
        {
            SetValue(value);
        }

        public void SetValue(long value)
        {
            _rndCrypt = UnityEngine.Random.Range(10000000, 100000000);
            _number = value + _rndCrypt;
        }

        public long GetValue()
        {
            return _number - _rndCrypt;
        }

        public int CompareTo(object obj)
        {
            return GetValue().CompareTo(obj);
        }

        public int CompareTo(long other)
        {
            return GetValue().CompareTo(other);
        }

        public bool Equals(long other)
        {
            return GetValue().Equals(other);
        }

        public override string ToString()
        {
            return GetValue().ToString();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return GetValue().ToString(format, formatProvider);
        }

        public int CompareTo(Entry_Long other)
        {
            return GetValue().CompareTo(other.GetValue());
        }

        public bool Equals(Entry_Long other)
        {
            return GetValue().Equals(other.GetValue());
        }

        public static implicit operator long(Entry_Long value)
        {
            return value.GetValue();
        }
    }
}
