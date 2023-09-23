using System;
using UnityEngine;
using Combat;

[CreateAssetMenu(fileName = "Effect", menuName = "Combat/Effect", order = 2)]
public class ScriptableEffect : ScriptableObject
{
    public StatsList statKey;
    public Operation op;
    public float duration;
    public float value;
    public DamageTypes damageType;
    public bool affectUser;

    public string GetStatKey()
    {
        return Enum.GetName(typeof(StatsList), statKey);
    }
}
