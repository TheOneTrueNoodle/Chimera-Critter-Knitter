using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Config", menuName = "Combat/Level Config", order = 4)]
public class LevelConfig : ScriptableObject
{
    [Header("Animation Curve")]
    public AnimationCurve animationCurve;
    public int MaxLevel;
    public int MaxRequiredExp;

    public int GetRequiredExp(int level)
    {
        int requiredExperience = Mathf.RoundToInt(animationCurve.Evaluate(Mathf.InverseLerp(0, MaxLevel, level)) * MaxRequiredExp);
        return requiredExperience;
    }
}
