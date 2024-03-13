using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscarBloodyTest : MonoBehaviour
{
    public Material basic;
    public Material Injured;
    public Material Mutate1;
    public Material Mutate2;
    public Material Mutate3;
    public GameObject DogsHead;
    public GameObject DogsBody;
    public GameObject DogsFrontLegs;
    public GameObject DogsBackLegs;
    public GameObject DogsTail;
    public GameObject DogsEyes;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            DogsHead.GetComponent<Renderer>().material = basic;
            DogsBody.GetComponent<Renderer>().material = basic;
            DogsFrontLegs.GetComponent<Renderer>().material = basic;
            DogsBackLegs.GetComponent<Renderer>().material = basic;
            DogsTail.GetComponent<Renderer>().material = basic;
            DogsEyes.GetComponent<Renderer>().material = basic;
            Debug.Log("Pressed1");
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            DogsHead.GetComponent<Renderer>().material = Injured;
            DogsBody.GetComponent<Renderer>().material = Injured;
            DogsFrontLegs.GetComponent<Renderer>().material = Injured;
            DogsBackLegs.GetComponent<Renderer>().material = Injured;
            DogsTail.GetComponent<Renderer>().material = Injured;
            DogsEyes.GetComponent<Renderer>().material = Injured;

            Debug.Log("Pressed2");
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            DogsHead.GetComponent<Renderer>().material = Mutate1;
            DogsBody.GetComponent<Renderer>().material = Mutate1;
            DogsFrontLegs.GetComponent<Renderer>().material = Mutate1;
            DogsBackLegs.GetComponent<Renderer>().material = Mutate1;
            DogsTail.GetComponent<Renderer>().material = Mutate1;
            DogsEyes.GetComponent<Renderer>().material = Mutate1;
            Debug.Log("Pressed3");
        }

        else if (Input.GetKey(KeyCode.Alpha4))
        {
            DogsHead.GetComponent<Renderer>().material = Mutate2;
            DogsBody.GetComponent<Renderer>().material = Mutate2;
            DogsFrontLegs.GetComponent<Renderer>().material = Mutate2;
            DogsBackLegs.GetComponent<Renderer>().material = Mutate2;
            DogsTail.GetComponent<Renderer>().material = Mutate2;
            DogsEyes.GetComponent<Renderer>().material = Mutate2;
            Debug.Log("Pressed3");
        }

        else if (Input.GetKey(KeyCode.Alpha5))
        {
            DogsHead.GetComponent<Renderer>().material = Mutate3;
            DogsBody.GetComponent<Renderer>().material = Mutate3;
            DogsFrontLegs.GetComponent<Renderer>().material = Mutate3;
            DogsBackLegs.GetComponent<Renderer>().material = Mutate3;
            DogsTail.GetComponent<Renderer>().material = Mutate3;
            DogsEyes.GetComponent<Renderer>().material = Mutate3;
            Debug.Log("Pressed3");
        }
    }
}