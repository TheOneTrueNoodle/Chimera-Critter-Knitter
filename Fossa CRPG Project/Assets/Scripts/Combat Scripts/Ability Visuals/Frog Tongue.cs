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
    public float animationTime;

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
        while (t < animationTime)
        {
            t += Time.deltaTime;
            endPosition.transform.position = Vector3.Lerp(startPosition.transform.position, targetUnit.transform.position, t);
            UpdateTransformForScale();
            yield return null;
        }
        //Return to attack caster
        while (t > 0)
        {
            t -= Time.deltaTime;
            endPosition.transform.position = Vector3.Lerp(startPosition.transform.position, targetUnit.transform.position, t);
            UpdateTransformForScale();
            yield return null;
        }
    }

    private void UpdateTransformForScale()
    {
        float distance = Vector3.Distance(startPosition.transform.position, endPosition.transform.position);
        visuals.transform.localScale = new Vector3(InitialScale.x, distance / 2f, InitialScale.z);

        Vector3 middlePoint = (startPosition.transform.position + endPosition.transform.position) / 2f;
        visuals.transform.position = middlePoint;

        Vector3 rotationDirection = (endPosition.transform.position - startPosition.transform.position);
        visuals.transform.up = rotationDirection;
    }
}
