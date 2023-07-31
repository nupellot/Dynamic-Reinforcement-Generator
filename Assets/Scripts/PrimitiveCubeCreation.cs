
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NUnit.Framework.Constraints;
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
    
    public void IsIntersectionAllowed(bool isIntersectionAllowed)
    {
        if (isIntersectionAllowed)
        {
            Debug.Log("Разрешили пересечения");
            Destructor.OnTouch -= DestroyReinforcement;
        } else if (!isIntersectionAllowed)
        {
            Debug.Log("Запретили пересечения");
            Destructor.OnTouch += DestroyReinforcement;
        }
    }

    private GameObject TheCube;
    private List<GameObject> Reinforcements = new List<GameObject>();
    System.Random random = new System.Random();
    
    // Start is called before the first frame update
    void Start()
    {
        // Спавним главный куб (Рабочий Объем, который мы будем армировать).
        TheCube = GameObject.Find("VolumetricCube");
        // Destructor.OnTouch += DestroyReinforcement;
        Reinforcements = SpawnReinforcements(SphereSource, 
                                                        AmountOfReinforcements, 
                                                        MinScaleOfReinforcement, 
                                                        MaxScaleOfReinforcement
        );
    }

    // Update is called once per frame
    void Update()
    {
        while (Reinforcements.Count != AmountOfReinforcements)
        {
            if (Reinforcements.Count < AmountOfReinforcements)
            {
                List<GameObject> NewReinforcements = SpawnReinforcements(
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


    List<GameObject> SpawnReinforcements(GameObject Reinforcement, int AmountOfReinforcements,
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
            NewReinforcements.Last().transform.localScale = GetRandomVector3(MinScaleOfReinforcement, MaxScaleOfReinforcement);
            NewReinforcements.Last().transform.position = NewReinforcements.Last().GetComponent<Renderer>().bounds.size / 2;
            Vector3 Position = GetRandomVector3(Vector3.zero,
                TheCubeSize - NewReinforcements.Last().GetComponent<Renderer>().bounds.size);
            NewReinforcements.Last().transform.position += Position;

        }

        return NewReinforcements;
    }
    

    Vector3 GetRandomVector3(Vector3 MinVector, Vector3 MaxVector)
    {
        return new Vector3(Random.Range(MinVector.x, MaxVector.x), Random.Range(MinVector.y, MaxVector.y),
            Random.Range(MinVector.z, MaxVector.z));
    }
}
