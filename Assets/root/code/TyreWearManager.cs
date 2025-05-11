using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyreWearManager : MonoBehaviour
{

    GameObject manager;
    Manager scriptlap;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager");
        scriptlap = manager.GetComponent<Manager>();

    }

    // Update is called once per frame
    void Update()
    {

        
    }
    public void ApplyTyreWear()
    {
        foreach (var piloto in scriptlap.pilotsList)
        {
            int x = 0;

            int modo = Convert.ToInt32(piloto.modo);

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

            string compuesto = piloto.compuesto;
            int desgasteActual = piloto.desgaste;

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

            piloto.desgaste = desgasteActual;
     
        }
    }

}
