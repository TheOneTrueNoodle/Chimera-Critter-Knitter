using UnityEngine;

[System.Serializable]
public abstract class BaseAI : MonoBehaviour
{
    public abstract Scenario Attack(OverlayTile tile, CombatAIController entity);
}
