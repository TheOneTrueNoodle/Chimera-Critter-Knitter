using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeThrow : AbilityVisual
{
    [SerializeField] private GameObject startPosition;
    [SerializeField] private GameObject visuals;

    public float animationMult;

    public override void Call()
    {
        startPosition.transform.position = abilitySource.transform.position;

        StartCoroutine(LerpAttack());
    }
    private IEnumerator LerpAttack()
    {
        float t = 0;
        //Reach Target Destination
        while (t < 1)
        {
            t += Time.deltaTime * animationMult;
            visuals.transform.position = Vector3.Lerp(startPosition.transform.position, targetPosition, t);

            yield return null;
        }

        //Play Frisbee breaking particles


        Destroy(this);
    }
}
