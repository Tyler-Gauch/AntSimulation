using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntHill : MonoBehaviour
{
    [SerializeField]
    private GameObject AntPrefab;

    [SerializeField]
    private int NumberOfAnts;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < NumberOfAnts; i++)
        {
            Instantiate<GameObject>(AntPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
