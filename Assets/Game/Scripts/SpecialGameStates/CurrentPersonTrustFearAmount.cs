using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentPersonTrustFearAmount", menuName = "SpecialGameStates/CurrentPersonTrustFearAmount")]
public class CurrentPersonTrustFearAmount : SpecialGameState<Person, (float, float)>
{
    public override void Set(Person person, (float, float) trustFear = default)
    {
        value1 = person;
        value2 = trustFear;
        base.Set(person, trustFear);
    }

    public override T Get<T>()
    {
        if (typeof(T) == value1.GetType()) return (T)(object)value1;
        if (typeof(T) == value2.GetType()) return (T)(object)value2;
        throw new Exception("Unsupported type");
    }
}