using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCreation : MonoBehaviour
{
    // public GameObject scalingCube;
    
    public GameObject volumetricCubeSource;
    public GameObject reinforcingCube;
    public GameObject reinforcingSphere;

    private GameObject volumetricCube;
    System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        // volumetricCube.GetComponent<Renderer>().bounds.size;
        // volumetricCube.GetComponent<Transform>().localScale = new Vector3(1000, 1000, 1000);
        
        volumetricCube = Instantiate(volumetricCubeSource, new Vector3(0, 0, 0), Quaternion.identity);
        volumetricCube.transform.position -= volumetricCube.GetComponent<Renderer>().bounds.size / 2;
        
        Debug.Log(volumetricCube.GetComponent<Renderer>().bounds.size);
        Debug.Log(volumetricCube.GetComponent<Renderer>().bounds.size.x);
        
        // reinforcingSphere.GetComponent<Transform>().localScale = new Vector3(10, 10, 10);
        Instantiate(reinforcingSphere,
            reinforcingSphere.GetComponent<Renderer>().bounds.size / 2,
            Quaternion.Euler(0,0, 0)
        );
        
        // reinforcingCube.GetComponent<Transform>().localScale 
    }

    // Update is called once per frame
    void Update()
    {
        volumetricCube.transform.position -= volumetricCube.GetComponent<Renderer>().bounds.size / 2;
        
        Debug.Log(volumetricCube.GetComponent<Renderer>().bounds.size);
        Debug.Log(volumetricCube.GetComponent<Renderer>().bounds.size.x);
        // Debug.Log("IM jopa");
        // Instantiate(myCube,
        //     new Vector3(random.Next(0,300),random.Next(0,300), random.Next(0,300)),
        //     Quaternion.Euler(random.Next(0,90),random.Next(0,90), random.Next(0,90))
        // );
    }


}
