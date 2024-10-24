using System;
using UnityEngine;

namespace Pioneer.DB
{
    public struct Entry_Int : IComparable, IComparable<int>, IEquatable<int>, IFormattable
    {
        [Newtonsoft.Json.JsonProperty]
        public int Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Newtonsoft.Json.JsonIgnore]
        private int _number;

        [Newtonsoft.Json.JsonIgnore]
        private int _rndCrypt;

        public Entry_Int(int value) : this()
        {
            SetValue(value);
        }

        public void SetValue(int value)
        {
            _rndCrypt = UnityEngine.Random.Range(1000, 10000);
            _number = value + _rndCrypt;
        }

        public int GetValue()
        {
            return _number - _rndCrypt;
        }

        public int CompareTo(object obj)
        {
            return GetValue().CompareTo(obj);
        }

        public int CompareTo(int other)
        {
            return GetValue().CompareTo(other);
        }

        public bool Equals(int other)
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

        public bool Equals(Entry_Int other)
        {
            return GetValue().Equals(other.GetValue());
        }

        public int CompareTo(Entry_Int other)
        {
            return GetValue().CompareTo(other.GetValue());
        }

        public static implicit operator int(Entry_Int value)
        {
            return value.GetValue();
        }
    }
}
