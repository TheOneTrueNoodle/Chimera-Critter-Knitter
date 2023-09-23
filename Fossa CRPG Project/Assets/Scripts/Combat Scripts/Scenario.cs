using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario
{
    public float scenarioValue;
    public AbilityData targetAbility;
    public OverlayTile targetTile;
    public OverlayTile positionTile;
    public bool useAutoAttack;

    public Scenario(float scenarioValue, AbilityData targetAbility, OverlayTile targetTile, OverlayTile positionTile, bool useAutoAttack)
    {
        this.scenarioValue = scenarioValue;
        this.targetAbility = targetAbility;
        this.targetTile = targetTile;
        this.positionTile = positionTile;
        this.useAutoAttack = useAutoAttack;
    }

    public Scenario()
    {
        scenarioValue = -10000;
        targetAbility = null;
        targetTile = null;
        positionTile = null;
        useAutoAttack = false;
    }
}
