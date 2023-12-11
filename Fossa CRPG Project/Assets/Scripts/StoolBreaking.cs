using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoolBreaking : MonoBehaviour
{

    public GameObject Stool1;
    public GameObject Stool2;
    public GameObject StoolFull;


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Stool1.SetActive(false);
            Stool2.SetActive(true);
            StartCoroutine(ExampleCoroutine());

        }
    }

    IEnumerator ExampleCoroutine()
    {
       
        yield return new WaitForSeconds(4);
        Destroy(StoolFull);

    }

}
