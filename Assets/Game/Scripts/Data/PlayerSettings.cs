using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Settings/PlayerSettings")]
public class PlayerSettings : ScriptableObject, IGameSettings
{
    public Player playerPrefab;
    public float maxSpeed;
    public Vector2 lookSensitivity;
    public float pitchLimit;
    public float acceleration;
    public bool useGravity;
}