using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


public class PrimitiveCubeCreation : MonoBehaviour
{

    
    [SerializeField] private GameObject TheCubeSource;
    [SerializeField] private GameObject SphereSource;
    [SerializeField] private Vector3 TheCubeScale = new Vector3(50, 50, 50);
    [SerializeField] private Vector3 SphereScale = new Vector3(5, 5, 5);
    [SerializeField] private Vector3 MinScaleOfReinforcement = Vector3.one;
    [SerializeField] private Vector3 MaxScaleOfReinforcement = Vector3.one;
    [SerializeField] private int AmountOfReinforcements = 100;

    // public List<GameObject> GetReinforcements()
    // {
    //     return Reinforcements;
    // }
     
    private GameObject TheCube;
    private List<GameObject> Spheres = new List<GameObject>();
    public List<GameObject> Reinforcements = new List<GameObject>();
    System.Random random = new System.Random();
    
    // Start is called before the first frame update
    void Start()
    {
        // volumetricCube = GameObject.Find("VolumetricCube");
        
        // Debug.Log(volumetricCube.GetComponent<Renderer>().bounds.size);
        // Debug.Log(reinforcingSphere.GetComponent<Collider>().bounds.size);
        
        // Debug.Log("size of SphereSource sphere: \n" + SphereSource.GetComponent<Renderer>().bounds.size);
        
        // Спавним главный куб (Рабочий Объем, который мы будем армировать).
        TheCube = GameObject.Find("VolumetricCube");
        // TheCube = Instantiate(TheCubeSource);
        // TheCube.transform.localScale = TheCubeScale;
        // TheCube.transform.position = TheCube.GetComponent<Renderer>().bounds.size / 2;
        // TheCube = Instantiate(
        //     TheCube,
        //     TheCube.GetComponent<Renderer>().bounds.size / 2,
        //     Quaternion.Euler(0, 0, 0)
        // );
        //

        // SpawnSpheres();
        Reinforcements = SpawnReinforcement(SphereSource, AmountOfReinforcements, MinScaleOfReinforcement, MaxScaleOfReinforcement);

        // Debug.Log(Spheres[1].GetComponent<Renderer>().bounds.size);
        // Debug.Log("size of Cube: \n" + TheCube.GetComponent<Renderer>().bounds.size);
        // Debug.Log("size of first sphere: \n" + Spheres[1].GetComponent<Renderer>().bounds.size);
    }

    // Update is called once per frame
    void Update()
    {
        
        while (Reinforcements.Count != AmountOfReinforcements)
        {
            if (Reinforcements.Count < AmountOfReinforcements)
            {
                List<GameObject> NewReinforcements = SpawnReinforcement(
                    SphereSource,
                    AmountOfReinforcements - Reinforcements.Count,
                    MinScaleOfReinforcement,
                    MaxScaleOfReinforcement
                );
                foreach (GameObject Reinforcement in NewReinforcements)
                {
                    Reinforcements.Add(Reinforcement);
                }
            }
            else
            if (Reinforcements.Count > AmountOfReinforcements)
            {
                GameObject.Destroy(Reinforcements.Last());
                Reinforcements.Remove(Reinforcements.Last());
            }
        }
        
        
    }

    void UpdateReinforcements()
    {
        
    }

    public void SetAmountFromSlider(Slider sl)
    {
        AmountOfReinforcements = (int)sl.value;
    }


    List<GameObject> SpawnReinforcement(GameObject Reinforcement, int AmountOfReinforcements,
        Vector3 MinScaleOfReinforcement, Vector3 MaxScaleOfReinforcement)
    {
        List<GameObject> NewReinforcements = new List<GameObject>();

        // Vector3 SphereSize = SphereSource.GetComponent<Renderer>().bounds.size;
        Vector3 TheCubeSize = TheCube.GetComponent<Renderer>().bounds.size;
        // Vector3 CubeToSphere = new Vector3(TheCubeSize.x / SphereSize.x, TheCubeSize.y / SphereSize.y,
        //     TheCubeSize.z / SphereSize.z);

        for (int i = 0; i < AmountOfReinforcements; i++)
        {
            NewReinforcements.Add(Instantiate(Reinforcement));
            NewReinforcements.Last().name = "RF " + (int)(Reinforcements.Count + i);
            NewReinforcements.Last().transform.localScale = GetRandomScale(MinScaleOfReinforcement, MaxScaleOfReinforcement);
            NewReinforcements.Last().transform.position = NewReinforcements.Last().GetComponent<Renderer>().bounds.size / 2;
            NewReinforcements.Last().transform.position += new Vector3(Random.Range(0, TheCubeSize.x), Random.Range(0, TheCubeSize.y), Random.Range(0, TheCubeSize.z));

        }

        return NewReinforcements;
    }


    // void OnCollisionEnter(Collision col) {
    //     // CarIsDeleted?.Invoke(this.gameObject);
    //      Debug.Log("Collision");
    //     if (this.gameObject.CompareTag("Reinforcement") && col.gameObject.CompareTag("Reinforcement")) {
    //         // Cars.Remove(this.gameObject);
    //         Destroy(this.gameObject);
    //         Debug.Log("Collision1");
    //
    //     }
    //     // if (this.gameObject.CompareTag("Car") && col.gameObject.CompareTag("Car")) {
    //     //     // Cars.Remove(this.gameObject);
    //     //     Destroy(this.gameObject);
    //     // }
    // }

    // List<GameObject> SpawnReinforcement(GameObject Reinforcement, int AmountOfReinforcements, Vector3 MinScaleOfReinforcement, Vector3 MaxScaleOfReinforcement)
    // {
    //     List<GameObject> Reinforcements = new List<GameObject>();
    //     
    //     Vector3 SphereSize = SphereSource.GetComponent<Renderer>().bounds.size;
    //     Vector3 TheCubeSize = TheCube.GetComponent<Renderer>().bounds.size;
    //     Vector3 CubeToSphere = new Vector3(TheCubeSize.x / SphereSize.x, TheCubeSize.y / SphereSize.y, TheCubeSize.z / SphereSize.z);
    //     
    //     for (int i = 1; i < AmountOfReinforcements; i++)
    //     {
    //         Reinforcements.Add(Instantiate(Reinforcement));
    //         Reinforcements.Last().name = "RF " + i;
    //         Reinforcements.Last().transform.Scale(GetRandomScale(MinScaleOfReinforcement, MaxScaleOfReinforcement));
    //         Reinforcements.Last().transform.position += Reinforcements.Last().GetComponent<Renderer>().bounds.size / 2;
    //         
    //
    //         Spheres.Add(
    //             Instantiate(
    //                 SphereSource,
    //                 SphereSize / 2 + new Vector3(random.Next(0, (int)(CubeToSphere.x)), random.Next(0, (int)(CubeToSphere.y)), random.Next(0,
    //                     (int)(CubeToSphere.z))) * SphereSize.x,
    //                 Quaternion.Euler(0, 0, 0)
    //             )
    //         );
    //         Spheres.Last().name = "Sphere" + i;
    //     }
    // }

    Vector3 GetRandomScale(Vector3 MinScale, Vector3 MaxScale)
    {
        return new Vector3(Random.Range(MinScale.x, MaxScale.x), Random.Range(MinScale.y, MaxScale.y),
            Random.Range(MinScale.z, MaxScale.z));
    }
    
    void SpawnSpheres()
    {
        for (int i = 1; i < 200; i++)
        {
            Vector3 SphereSize = SphereSource.GetComponent<Renderer>().bounds.size;
            Vector3 TheCubeSize = TheCube.GetComponent<Renderer>().bounds.size;
            Vector3 CubeToSphere = new Vector3(TheCubeSize.x / SphereSize.x, TheCubeSize.y / SphereSize.y, TheCubeSize.z / SphereSize.z);
            
            Spheres.Add(
                Instantiate(
                    SphereSource,
                    SphereSize / 2 + new Vector3(random.Next(0, (int)(CubeToSphere.x)), random.Next(0, (int)(CubeToSphere.y)), random.Next(0,
                        (int)(CubeToSphere.z))) * SphereSize.x,
                    Quaternion.Euler(0, 0, 0)
                )
            );
            Spheres.Last().name = "Sphere" + i;
        }
    }
}
