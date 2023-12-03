using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshMerger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MergeMeshes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    public void MergeMeshes()
    {
        // Получаем все MeshFilter дочерних объектов
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

        // Создаем массив CombineInstance из MeshFilter компонентов
        CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combineInstances[i].mesh = meshFilters[i].sharedMesh;
            // combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        // Создаем новый пустой объект
        // GameObject combinedObject = new GameObject("CombinedMesh");

        // Добавляем MeshFilter и MeshRenderer
        
        // MeshRenderer meshRenderer = combinedObject.AddComponent<MeshRenderer>();
        // Collider collider = combinedObject.AddComponent<Collider>();

        // Создаем новый меш и объединяем в него меши
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combineInstances, true);

        // Назначаем меш и материал
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = combinedMesh;
        // meshRenderer.sharedMaterial = meshFilters[0].GetComponent<Renderer>().sharedMaterial;

        // // Опционально: Удаляем MeshFilter и MeshRenderer компоненты у дочерних объектов
        // for (int i = 0; i < meshFilters.Length; i++)
        // {
        //     Destroy(meshFilters[i]);
        // }
    }
}
