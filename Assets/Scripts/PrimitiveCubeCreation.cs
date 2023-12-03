
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
// using System.Numerics;
using NUnit.Framework.Constraints;
using UnityEngine.Serialization;
// using System.Numerics;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class PrimitiveCubeCreation : MonoBehaviour
{
    [SerializeField] private GameObject reinforcementSource;
    [SerializeField] private Vector3 minScaleOfReinforcement = Vector3.one;
    [SerializeField] private Vector3 maxScaleOfReinforcement = Vector3.one;
    [SerializeField] private int amountOfReinforcements = 100;

    private GameObject TheCube;
    private List<GameObject> Reinforcements = new List<GameObject>();
    System.Random random = new System.Random();
    
    // Start is called before the first frame update
    void Start()
    {
        // Спавним главный куб (Рабочий Объем, который мы будем армировать).
        TheCube = GameObject.Find("VolumetricCube");
        // Destructor.OnTouch += DestroyReinforcement;
        SetAmountFromSlider(GameObject.Find("AmountOfReinforcementsSlider").GetComponent<Slider>());
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


    List<GameObject> spawnReinforcements(
        GameObject Reinforcement, 
        int AmountOfReinforcements,
        Vector3 MinScaleOfReinforcement, 
        Vector3 MaxScaleOfReinforcement
        )
    {
        List<GameObject> NewReinforcements = new List<GameObject>();

        // Vector3 SphereSize = SphereSource.GetComponent<Renderer>().bounds.size;
        Vector3 TheCubeSize = TheCube.GetComponent<Renderer>().bounds.size;
        Vector3 spawnPoint = TheCube.GetComponent<Transform>().position - TheCubeSize / 2;
        // Vector3 CubeToSphere = new Vector3(TheCubeSize.x / SphereSize.x, TheCubeSize.y / SphereSize.y,
        //     TheCubeSize.z / SphereSize.z);

        for (int i = 0; i < AmountOfReinforcements; i++)
        {
            NewReinforcements.Add(Instantiate(Reinforcement));
            NewReinforcements.Last().name = "RF " + (int)(Reinforcements.Count + i);
            
            
            NewReinforcements.Last().transform.localScale = GetRandomVector3(MinScaleOfReinforcement, MaxScaleOfReinforcement);
            NewReinforcements.Last().transform.rotation = Quaternion.Euler(GetRandomVector3(Vector3.zero, new Vector3(360, 360, 360)));
            NewReinforcements.Last().transform.position = NewReinforcements.Last().GetComponent<Renderer>().bounds.size / 2;
            
            NewReinforcements.Last().transform.position = spawnPoint;
            
            NewReinforcements.Last().transform.position -= NewReinforcements.Last().GetComponent<Renderer>().bounds.size / 2;
            Vector3 Position = GetRandomVector3(
                Vector3.zero + Vector3.one*2,
                TheCubeSize - Vector3.one*2  
            );
            NewReinforcements.Last().transform.position = spawnPoint + Position;
            // MergeMeshes(NewReinforcements.Last());
            

            //
            // if (IsOutsideTheCube(NewReinforcements.Last()))
            // {
            //     // DestroyReinforcement(NewReinforcements.Last());
            // }

        }

        return NewReinforcements;
    }


    public void MergeMeshes(GameObject reinforcement)
    {
        // Получаем все MeshFilter дочерних объектов
        MeshFilter[] meshFilters = reinforcement.GetComponentsInChildren<MeshFilter>();

        // Создаем массив CombineInstance из MeshFilter компонентов
        CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combineInstances[i].mesh = meshFilters[i].sharedMesh;
            combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        // Создаем новый пустой объект
        // GameObject combinedObject = new GameObject("CombinedMesh");

        // Добавляем MeshFilter и MeshRenderer
        MeshFilter meshFilter = reinforcement.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = reinforcement.AddComponent<MeshRenderer>();

        // Создаем новый меш и объединяем в него меши
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combineInstances, true, true);

        // Назначаем меш и материал
        meshFilter.sharedMesh = combinedMesh;
        // meshRenderer.sharedMaterial = meshFilters[0].GetComponent<Renderer>().sharedMaterial;

        
        
    }
    
    public bool IsOutsideTheCube(GameObject Reinforcement)
    {
        Bounds reinforcementBounds = Reinforcement.GetComponent<MeshFilter>().sharedMesh.bounds;
        Bounds theCubeBounds = TheCube.GetComponent<Renderer>().bounds;

        Debug.Log("Границы reinforcement: " + reinforcementBounds.min + " -> " + reinforcementBounds.max);
        // Debug.Log("Границы reinforcement: " + Reinforcement.GetComponent<Renderer>().bounds + " -> " + reinforcementBounds.max);
        Debug.Log("Границы TheCube: " + theCubeBounds.min + " -> " + theCubeBounds.max);
        
        if (reinforcementBounds.max.x > theCubeBounds.max.x ||
            reinforcementBounds.max.y > theCubeBounds.max.y ||
            reinforcementBounds.max.z > theCubeBounds.max.z ||
            
            reinforcementBounds.min.x < theCubeBounds.min.x ||
            reinforcementBounds.min.y < theCubeBounds.min.y ||
            reinforcementBounds.min.z < theCubeBounds.min.z
            )
        {
            Debug.Log("Вывалился за границы");
            return true;
        }

        return false;

    }
    
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

    Vector3 GetRandomVector3(Vector3 MinVector, Vector3 MaxVector)
    {
        return new Vector3(Random.Range(MinVector.x, MaxVector.x), Random.Range(MinVector.y, MaxVector.y),
            Random.Range(MinVector.z, MaxVector.z));
    }
}
