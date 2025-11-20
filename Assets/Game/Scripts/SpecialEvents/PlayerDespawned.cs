using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDespawned", menuName = "Events/SpecialEvent(PlayerDespawned)")]
public class PlayerDespawned : BaseSpecialEvent
{
    public override void Invoke(ObjectConfig _)
    {
        EventService.Invoke(new OnPlayerDespawnedEvent());
    }
}