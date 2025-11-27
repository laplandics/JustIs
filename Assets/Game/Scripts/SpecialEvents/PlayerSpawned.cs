using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSpawned", menuName = "Events/SpecialEvent(PlayerSpawned)")]
public class PlayerSpawned : BaseSpecialEvent
{
    public override void Invoke(ObjectConfig config)
    {
        if (!config.GetInstances()[0].TryGetComponent<Player>(out var player)) return;
        EventService.Invoke(new ConfigEvents.Player_SpawnedEvent {Player = player});
    }
}