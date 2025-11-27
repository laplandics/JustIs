using UnityEngine;

[CreateAssetMenu(fileName = "PersonDespawned", menuName = "Events/SpecialEvent(PersonDespawned)")]
public class PersonDespawned : BaseSpecialEvent
{
    public override void Invoke(ObjectConfig config)
    {
        DataInjector.InjectState<CurrentPersonMood>().Set(null);
    }
}