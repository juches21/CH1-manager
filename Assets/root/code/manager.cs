using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manager : MonoBehaviour
{

    [SerializeField] GameObject[] tarjetas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void esconder()
    {
        foreach (GameObject tarjeta in tarjetas)
        {
            Debug.Log("Tarjeta: " + tarjeta.name);
        }
    }
}
