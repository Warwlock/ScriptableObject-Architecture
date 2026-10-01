using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
    [Serializable]
    public class SceneInfoEvent : UnityEvent<SceneInfo> { }

    /// <summary>
    /// <see cref="SceneVariable"/> is a scriptable constant variable whose scene values are assigned at
    /// edit-time by assigning a <see cref="UnityEditor.SceneAsset"/> instance to it.
    /// </summary>
    [CreateAssetMenu(
        fileName = "SceneVariable.asset",
        menuName = SOArchitecture_Utility.ADVANCED_VARIABLE_SUBMENU + "Scene",
        order = 120)]
    public class SceneVariable : BaseVariable<SceneInfo, SceneInfoEvent>
    {
        /// <summary>
        /// Returns the <see cref="SceneInfo"/> of this instance.
        /// </summary>
        public override SceneInfo Value => _value;

        // A scene variable is essentially a constant for edit-time modification only; there is not
        // any kind of expectation for a user to be able to set this at runtime.
        public override bool ReadOnly => true;
    }
}
