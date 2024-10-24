using System;
using UnityEngine;

namespace Pioneer.DB
{
    public struct Entry_String : IEquatable<string>
    {
        [Newtonsoft.Json.JsonProperty]
        public string Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Newtonsoft.Json.JsonIgnore]
        private string _value;

        public Entry_String(string value) : this()
        {
            SetValue(value);
        }

        public void SetValue(string value)
        {
            _value = value;
        }

        public string GetValue()
        {
            return _value;
        }

        public int CompareTo(object obj)
        {
            return GetValue().CompareTo(obj);
        }

        public int CompareTo(string other)
        {
            return GetValue().CompareTo(other);
        }

        public bool Equals(string other)
        {
            return GetValue().Equals(other);
        }

        public override string ToString()
        {
            return GetValue().ToString();
        }

        public static implicit operator string(Entry_String value)
        {
            return value.GetValue();
        }
    }
}
