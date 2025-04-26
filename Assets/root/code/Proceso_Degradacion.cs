using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proceso_Degradacion : MonoBehaviour
{

    GameObject manager;
    laps scriptlap;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager");
        scriptlap = manager.GetComponent<laps>();

    }

    // Update is called once per frame
    void Update()
    {

        
    }
    public void wheel_wear()
    {
        foreach (var piloto in scriptlap.datos)
        {
            int x = 0;

            int modo = Convert.ToInt32(piloto[7]);

            if (modo == 1)
            {
                x = 0;
            }
            else if (modo == 2)
            {
                x = 3;
            }
            else if (modo == 3)
            {
                x = 10;
            }

            string compuesto = piloto[3].ToString();
            int desgasteActual = Convert.ToInt32(piloto[4]);

            if (compuesto == "s")
            {
                desgasteActual -= (10 + x);
            }
            else if (compuesto == "m")
            {
                desgasteActual -= (3 + x);
            }
            else // compuesto "h"
            {
                desgasteActual -= (1 + x);
            }

            if (desgasteActual < 0)
            {
                desgasteActual = 0;
            }

            piloto[4] = desgasteActual;
           // print(piloto[4]);
        }
    }

}
