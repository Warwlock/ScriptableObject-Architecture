using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
    public abstract class BaseVariable<T> : GameEventBase
    {
        public virtual T Value
        {
            get => _value;
            set => _value = SetValue(value);
        }

        public T DefaultValue
        {
            get => _defaultValue;
            set => _defaultValue = value;
        }

        public virtual T MinClampValue => Clampable ? _minClampedValue : default;
        public virtual T MaxClampValue => Clampable ? _maxClampedValue : default;

        public virtual bool Clampable => false;
        public virtual bool ReadOnly => _readOnly;
        public virtual bool IsClamped => _isClamped;
        public virtual System.Type Type => typeof(T);
        public virtual System.Type ReferenceType => typeof(BaseReference<T, BaseVariable<T>>);
        public virtual bool UseDefaultValue => _useDefaultValue;

        public object BaseValue
        {
            get => _value;
            set => SetValue((T)value);
        }

        [SerializeField]
        protected T _value = default;
        [SerializeField]
        private bool _readOnly = false;
        [SerializeField]
        private bool _useDefaultValue = false;
        [SerializeField, Tooltip("If any script tries to change readonly variable, show a warning message in console.")]
        private bool _raiseWarning = true;
        [SerializeField]
        protected bool _isClamped = false;
        [SerializeField]
        protected T _minClampedValue = default;
        [SerializeField]
        protected T _maxClampedValue = default;
        [SerializeField]
        protected T _defaultValue;

        private T _oldValue;

        public virtual T SetValue(BaseVariable<T> value) => SetValue(value.Value);

        public virtual T SetValue(T newValue)
        {
            if (_readOnly)
            {
                RaiseReadonlyWarning();
                return _value;
            }
            else if (Clampable && IsClamped)
            {
                newValue = ClampValue(newValue);
            }

            _value = newValue;

            if (!AreValuesEqual(newValue, _oldValue))
                Raise();

            _oldValue = _value;

            return newValue;
        }

        protected virtual bool AreValuesEqual(T a, T b) => a != null ? a.Equals(b) : b == null;

        protected virtual T ClampValue(T value) => value;

        private void RaiseReadonlyWarning()
        {
            if (!_readOnly || !_raiseWarning)
                return;

            Debug.LogWarning("Tried to set value on " + name + ", but value is readonly!", this);
        }
        public override string ToString() => _value == null ? "null" : _value.ToString();

        public static implicit operator T(BaseVariable<T> reference) => reference.Value;

        public void OnValidate() => SetValue(Value);

        public void OnEnable()
        {
            _oldValue = _value;

            if (UseDefaultValue)
                ResetToDefaultValue();
        }

        private void ResetToDefaultValue() => Value = _defaultValue;
    }
    public abstract class BaseVariable<T, TEvent> : BaseVariable<T> where TEvent : UnityEvent<T>
    {
        [SerializeField]
        private TEvent _event = default;

        public override void Raise()
        {
            base.Raise();

            _event.Invoke(Value);
        }

        public void AddListener(UnityAction<T> callback) => _event.AddListener(callback);

        public void RemoveListener(UnityAction<T> callback) => _event.RemoveListener(callback);

        public override void RemoveAll()
        {
            base.RemoveAll();
            _event.RemoveAllListeners();
        }
    }
}