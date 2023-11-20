using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Round Event", menuName = "Combat/Round Event", order = 4)]
public class CombatRoundEventData : ScriptableObject
{
    public List<Entity> EnemiesJoiningCombat;
}
