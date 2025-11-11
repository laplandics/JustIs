using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSpawned", menuName = "Events/SpecialEvent(PlayerSpawned)")]
public class PlayerSpawned : ScriptableObject, ISpecialEvent
{
    public void Invoke(StageObject eventSender)
    {
        if (!eventSender.instance.TryGetComponent<Player>(out var player)) return;
        EventService.Invoke(new OnPlayerSpawnedEvent {Player = player});
    }
}