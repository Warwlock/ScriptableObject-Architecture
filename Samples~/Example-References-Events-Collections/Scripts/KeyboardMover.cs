using UnityEngine;
using UnityEngine.InputSystem;

namespace ScriptableObjectArchitecture.Examples
{
    public class KeyboardMover : MonoBehaviour
    {
        [SerializeField]
        private FloatReference _moveSpeed = default;

        private void Update()
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
                transform.position += Vector3.up * _moveSpeed.Value;

            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                transform.position += Vector3.down * _moveSpeed.Value;

            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                transform.position += Vector3.right * _moveSpeed.Value;

            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                transform.position += Vector3.left * _moveSpeed.Value;
        }
    }
}