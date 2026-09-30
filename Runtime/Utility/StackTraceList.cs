using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;

namespace ScriptableObjectArchitecture
{
    public class StackTraceList : System.Collections.ObjectModel.Collection<StackTraceEntry>
    {
        private static readonly List<WeakReference<StackTraceList>> _activeInstances = new();

        public event Action OnListChanged;

        public StackTraceList()
        {
            _activeInstances.Add(new WeakReference<StackTraceList>(this));
        }

        public static void ClearAllInstances()
        {
            for (int i = _activeInstances.Count - 1; i >= 0; i--)
            {
                if (_activeInstances[i].TryGetTarget(out StackTraceList list))
                {
                    list.Clear();
                }
                else
                {
                    _activeInstances.RemoveAt(i);
                }
            }
        }

        protected override void InsertItem(int index, StackTraceEntry item)
        {
            base.InsertItem(index, item);
            OnListChanged?.Invoke();
        }

        protected override void SetItem(int index, StackTraceEntry item)
        {
            base.SetItem(index, item);
            OnListChanged?.Invoke();
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            OnListChanged?.Invoke();
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            OnListChanged?.Invoke();
        }
        
    }
}