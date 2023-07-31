
using System.Collections.Generic;
// using System.Diagnostics;
// using System.Diagnostics;
using UnityEngine;
using System.Linq;
// using System.Numerics;
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
    
    
    private GameObject TheCube;
    public List<GameObject> Reinforcements = new List<GameObject>();
    System.Random random = new System.Random();
    
    // Start is called before the first frame update
    void Start()
    {
        // Спавним главный куб (Рабочий Объем, который мы будем армировать).
        TheCube = GameObject.Find("VolumetricCube");
        Destructor.OnTouch += DestroyReinforcement;
        Reinforcements = SpawnReinforcement(SphereSource, AmountOfReinforcements, MinScaleOfReinforcement, MaxScaleOfReinforcement);
        Debug.Log("Jopa");
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

    void DestroyReinforcement(GameObject Reinforcement)
    {
        Debug.Log("Destroying " + Reinforcement.name);
        Reinforcements.Remove(Reinforcement);
        Destroy(Reinforcement);
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
    

    Vector3 GetRandomScale(Vector3 MinScale, Vector3 MaxScale)
    {
        return new Vector3(Random.Range(MinScale.x, MaxScale.x), Random.Range(MinScale.y, MaxScale.y),
            Random.Range(MinScale.z, MaxScale.z));
    }
}
