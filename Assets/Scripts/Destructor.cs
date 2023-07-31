using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructor : MonoBehaviour
{
    // Start is called before the first frame update
    // [SerializeField] private GameObject DirectionalLight;
    // public PrimitiveCubeCreation PrimitiveCubeCreation;

    public delegate void MyDelegate(GameObject gameObject);
    public static event MyDelegate OnTouch;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // void OnCollisionEnter(Collision col) {
    //     // CarIsDeleted?.Invoke(this.gameObject);
    //     Debug.Log("Collision");
    //     if (this.gameObject.CompareTag("Reinforcement") && col.gameObject.CompareTag("Reinforcement"))
    //     {
    //         onTouch.Invoke(this.gameObject);
    //     }
    // }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Collision " + this.gameObject.CompareTag());
        // if (this.gameObject.CompareTag("Reinforcement") && other.gameObject.CompareTag("Reinforcement"))
        // {
            if (OnTouch != null)
            {
                Debug.Log("ЕСТЬ ПОДПЕЩИКИ");
                OnTouch.Invoke(this.gameObject);
            }
            else
            {
                Debug.Log("НЕТ ПОДПЕЩЕКОВ");
            }
            
        // }
    }
}
