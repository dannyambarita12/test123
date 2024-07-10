using System;
using System.Collections.Generic;

public class StateMachine<T> where T : Enum
{
    public class StateHooks
    {
        public Action<T> onEnter;
        public Action<T> onExit;
        public Action<T> onUpdate;
    }

    private Dictionary<T, StateHooks> _states = new Dictionary<T, StateHooks>();
    private (T current, T previous) previousFrame = (default(T), default(T));

    public StateHooks this[T statename]
    {
        get
        {
            if (!_states.TryGetValue(statename, out StateHooks state))
            {
                state = new StateHooks();
                _states[statename] = state;
            }
            return state;
        }
    }

    public void Update(T currentState, T previousState)
    {
        if (_states.TryGetValue(currentState, out StateHooks current))
            current.onUpdate?.Invoke(currentState);

        if (Equals(currentState, previousFrame.current) && Equals(previousState, previousFrame.previous))
            return;

        if (_states.TryGetValue(previousState, out StateHooks prev))
            prev.onExit?.Invoke(previousState);

        if (_states.TryGetValue(currentState, out StateHooks next))
            next.onEnter?.Invoke(currentState);

        previousFrame = (currentState, previousState);
    }

    public void UnsubscribeAll()
    {
        _states.Clear();
    }
}