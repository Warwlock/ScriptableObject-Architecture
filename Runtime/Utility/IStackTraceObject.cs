using System.Collections.Generic;

namespace ScriptableObjectArchitecture
{
    public interface IStackTraceObject
    {
        StackTraceList StackTraces { get; }

        void AddStackTrace();
        void AddStackTrace(object value);
    } 
}