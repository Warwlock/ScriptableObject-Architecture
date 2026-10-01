using UnityEngine;

namespace ScriptableObjectArchitecture
{
    [System.Serializable]
    public sealed class Vector4Reference : BaseReference<Vector4Info, Vector4Variable>
    {
        public Vector4Reference() : base() { }
        public Vector4Reference(Vector4Info value) : base(value) { }
    } 
}