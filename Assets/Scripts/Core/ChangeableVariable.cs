using System;
using System.Collections.Generic;

public class ChangeableVariable<T>
{
    public event Action<T, T> OnValueChanged;

    public T Value { get; private set; }

    public ChangeableVariable()
    {
    }

    public ChangeableVariable(T value)
    {
        Value = value;
    }

    public void Set(T value)
    {
        if (EqualityComparer<T>.Default.Equals(Value, value))
        {
            return;
        }

        T previousValue = Value;
        Value = value;
        OnValueChanged?.Invoke(value, previousValue);
    }

    public void ForceSet(T value)
    {
        T previousValue = Value;
        Value = value;
        OnValueChanged?.Invoke(value, previousValue);
    }
}