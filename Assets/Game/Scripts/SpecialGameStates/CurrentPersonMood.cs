using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentPersonMood", menuName = "SpecialGameStates/CurrentPersonMood")]
public class CurrentPersonMood : SpecialGameState<Person, float>
{
    public override void Set(Person person, float trust = 0f)
    {
        value1 = person;
        value2 = trust;
        base.Set(person, trust);
    }

    public override T Get<T>()
    {
        if (typeof(T) == value1.GetType()) return (T)(object)value1;
        if (typeof(T) == value2.GetType()) return (T)(object)value2;
        throw new Exception($"Invalid Get type: {typeof(T).Name}");
    }
}