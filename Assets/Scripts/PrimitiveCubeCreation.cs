
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
// using System.Numerics;
using NUnit.Framework.Constraints;
using UnityEngine.Serialization;
// using System.Numerics;
using UnityEngine.UI;


public class PrimitiveCubeCreation : MonoBehaviour
{
    [SerializeField] private GameObject reinforcementSource;
    [SerializeField] private Vector3 minScaleOfReinforcement = Vector3.one;
    [SerializeField] private Vector3 maxScaleOfReinforcement = Vector3.one;
    [SerializeField] private int amountOfReinforcements = 100;

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
        Reinforcements = spawnReinforcements(
            reinforcementSource,
            amountOfReinforcements,
            minScaleOfReinforcement,
            maxScaleOfReinforcement
        );
    }

    // Update is called once per frame
    void Update()
    {
        while (Reinforcements.Count != amountOfReinforcements)
        {
            if (Reinforcements.Count < amountOfReinforcements)
            {
                List<GameObject> newReinforcements = spawnReinforcements(
                    reinforcementSource,
                    amountOfReinforcements - Reinforcements.Count,
                    minScaleOfReinforcement,
                    maxScaleOfReinforcement
                );
                foreach (GameObject reinforcement in newReinforcements)
                {
                    Reinforcements.Add(reinforcement);
                }
            }
            else
            if (Reinforcements.Count > amountOfReinforcements)
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
        amountOfReinforcements = (int)sl.value;
    }


    List<GameObject> spawnReinforcements(GameObject Reinforcement, int AmountOfReinforcements,
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
            NewReinforcements.Last().transform.rotation = Quaternion.Euler(GetRandomVector3(Vector3.zero, new Vector3(360, 360, 360)));
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
