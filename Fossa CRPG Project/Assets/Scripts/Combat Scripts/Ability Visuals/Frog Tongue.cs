using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogTongue : MonoBehaviour
{
    [SerializeField] private GameObject startPosition;
    [SerializeField] private GameObject endPosition;
    [SerializeField] private GameObject visuals;
    private Vector3 InitialScale;

    public GameObject targetUnit;
    public float animationMult;

    private void Start()
    {
        InitialScale = transform.localScale;
        UpdateTransformForScale();
        StartCoroutine(LerpAttack());
    }

    private IEnumerator LerpAttack()
    {
        float t = 0;
        //Reach Target Destination
        while (t < 1)
        {
            t += Time.deltaTime * animationMult;
            endPosition.transform.position = Vector3.Lerp(startPosition.transform.position, targetUnit.transform.position, t);
            UpdateTransformForScale();
            yield return null;
        }

        //Return to attack caster
        while (t > 0)
        {
            t -= Time.deltaTime * animationMult;
            endPosition.transform.position = Vector3.Lerp(startPosition.transform.position, targetUnit.transform.position, t);
            UpdateTransformForScale();
            yield return null;
        }
    }

    private void UpdateTransformForScale()
    {

        float distance = Vector3.Distance(startPosition.transform.position, endPosition.transform.position);
        visuals.transform.localScale = new Vector3(InitialScale.x, InitialScale.y, distance);

        Vector3 middlePoint = Vector3.Lerp(startPosition.transform.position, endPosition.transform.position, 0.5f);
        visuals.transform.position = middlePoint;

        visuals.transform.LookAt(endPosition.transform.position);
    }
}
