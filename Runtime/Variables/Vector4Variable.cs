using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
    [System.Serializable]
    public class Vector4Event : UnityEvent<Vector4Info> { }

    [CreateAssetMenu(
        fileName = "Vector4Variable.asset",
        menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "Structs/Vector4",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_COLLECTIONS + 12)]
    public class Vector4Variable : BaseVariable<Vector4Info, Vector4Event>
    {
        public new Vector4 Value { get => base.Value.Value; set => base.Value = value; }
    } 
}