using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentMonitorUi", menuName = "SpecialGameStates/CurrentMonitorUi")]
public class CurrentMonitorUi : SpecialGameState<MonitorUiType, Monitor>
{
    public override void Set(MonitorUiType ui, Monitor monitor = null)
    {
        value1 = ui;
        EventService.Invoke(new ConfigEvents.Monitor_UiChangedEvent {Command = ui, SpecificTarget = monitor});
        base.Set(ui, monitor);
    }

    public override T Get<T>()
    {
        if (typeof(T) == value1.GetType()) return (T)(object)value1;
        if (typeof(T) == value2.GetType()) return (T)(object)value2;
        throw new Exception("Unsupported type");
    }
}