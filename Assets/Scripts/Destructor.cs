using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructor : MonoBehaviour
{
    // Start is called before the first frame update
    // [SerializeField] private GameObject DirectionalLight;
    public PrimitiveCubeCreation PrimitiveCubeCreation;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter(Collision col) {
        // CarIsDeleted?.Invoke(this.gameObject);
        // Debug.Log("Collision");
        if (this.gameObject.CompareTag("Reinforcement") && col.gameObject.CompareTag("Reinforcement"))
        {
            Destroy(this.gameObject);
            PrimitiveCubeCreation.Reinforcements.Remove(this.gameObject);
            // PrimitiveCubeCreation.Reinforcements.Remove(this.gameObject);
        }
        // if (this.gameObject.CompareTag("Car") && col.gameObject.CompareTag("Car")) {
        //     // Cars.Remove(this.gameObject);
        //     Destroy(this.gameObject);
        // }
    }
}
