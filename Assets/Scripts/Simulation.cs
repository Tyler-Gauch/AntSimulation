using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    [SerializeField]
    private GameObject AntHillPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate<GameObject>(AntHillPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
