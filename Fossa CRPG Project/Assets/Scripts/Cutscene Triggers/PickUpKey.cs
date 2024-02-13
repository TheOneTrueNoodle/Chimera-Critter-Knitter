using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpKey : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            StartCoroutine(OscarTakeKey());
        }
    }

    public void Call()
    {
        StartCoroutine(OscarTakeKey());
    }

    IEnumerator OscarTakeKey()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject.transform.parent.gameObject);
    }
}
