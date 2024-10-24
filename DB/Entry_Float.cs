using System;
using UnityEngine;

namespace Pioneer.DB
{
    public struct Entry_Float : IEquatable<float>
    {
        [Newtonsoft.Json.JsonProperty]
        public float Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Newtonsoft.Json.JsonIgnore]
        private float _number;

        [Newtonsoft.Json.JsonIgnore]
        private float _rndCrypt;

        public Entry_Float(float value) : this()
        {
            SetValue(value);
        }

        public void SetValue(float value)
        {
            _rndCrypt = UnityEngine.Random.Range(1000f, 10000f);
            _number = value + _rndCrypt;
        }

        public float GetValue()
        {
            return _number - _rndCrypt;
        }

        public override string ToString()
        {
            return GetValue().ToString();
        }

        public bool Equals(Entry_Float other)
        {
            return GetValue().Equals(other.GetValue());
        }

        public int CompareTo(Entry_Float other)
        {
            return GetValue().CompareTo(other.GetValue());
        }

        public bool Equals(float other)
        {
            return GetValue().Equals(other);
        }

        public static implicit operator float(Entry_Float value)
        {
            return value.GetValue();
        }
    }
}
