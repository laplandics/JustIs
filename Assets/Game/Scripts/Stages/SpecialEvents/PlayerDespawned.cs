using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDespawned", menuName = "Events/SpecialEvent(PlayerDespawned)")]
public class PlayerDespawned : ScriptableObject, ISpecialEvent
{
    public void Invoke(StageObject _)
    {
        EventService.Invoke(new OnPlayerDespawnedEvent());
    }
}