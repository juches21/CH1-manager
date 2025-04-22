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

            if (Convert.ToInt32(piloto[7]) == 1)
            {
                x = 0;
            }
            if (Convert.ToInt32(piloto[7]) == 2)
            {
                x = 5;
            }
            if (Convert.ToInt32(piloto[7]) == 3)
            {
                x = 10;
            }
            if (piloto[5].ToString() == "s")
            {
                piloto[6] = Convert.ToInt32(piloto[6])- (5 + x);
            }
            else
            if (piloto[5].ToString() == "m")
            {
                piloto[6] = Convert.ToInt32(piloto[6]) - (3 + x);
            }
            else

            {
                piloto[6] = Convert.ToInt32(piloto[6]) - (1 + x);
            }
            if (Convert.ToInt32(piloto[6]) <= 0)
            {
                piloto[6] = 0;
            }

        }
    }
}
