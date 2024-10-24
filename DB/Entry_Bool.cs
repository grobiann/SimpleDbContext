using System;
using UnityEngine;

namespace Pioneer.DB
{
    public struct Entry_Bool : IEquatable<bool>
    {
        [Newtonsoft.Json.JsonProperty]
        public bool Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Newtonsoft.Json.JsonIgnore]
        private bool _value;

        public Entry_Bool(bool value) : this()
        {
            SetValue(value);
        }

        public void SetValue(bool value)
        {
            _value = value;
        }

        public bool GetValue()
        {
            return _value;
        }

        public override string ToString()
        {
            return GetValue().ToString();
        }

        public bool Equals(Entry_Bool other)
        {
            return GetValue().Equals(other.GetValue());
        }

        public int CompareTo(Entry_Bool other)
        {
            return GetValue().CompareTo(other.GetValue());
        }

        public bool Equals(bool other)
        {
            return GetValue().Equals(other);
        }

        public static implicit operator bool(Entry_Bool value)
        {
            return value.GetValue();
        }
    }
}
