using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
    [System.Serializable]
    public class StringEvent : UnityEvent<string> { }

    [CreateAssetMenu(
        fileName = "StringVariable.asset",
        menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "string",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_COLLECTIONS + 2)]
    public class StringVariable : BaseVariable<string, StringEvent>
    {
    } 
}