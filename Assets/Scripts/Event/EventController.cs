using System;

public class EventController
{
    private event Action baseEvent;

    public void AddListener(Action listener) => this.baseEvent += listener;
    public void RemoveListener(Action listener) => this.baseEvent -= listener;
    public void InvokeEvent() => this.baseEvent.Invoke();
}
