using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
    [Serializable]
    public class Vector4Info
    {
        [SerializeField] public Vector4 Value;

        public static implicit operator Vector4(Vector4Info info) => info != null ? info.Value : Vector4.zero;

        public static implicit operator Vector4Info(Vector4 v) => new() { Value = v };

        public override string ToString() => Value.ToString();
    }
}