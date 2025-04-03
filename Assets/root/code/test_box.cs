using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class test_box : MonoBehaviour
{
    GameObject manager;
    public GameObject B_ask;
    public GameObject B_lap;
    public TextMeshProUGUI T_player;
    [SerializeField]  int id;
    laps scriptlap;


    int minor_fault = 0;
    int medium_fault = 0;
    int major_fault = 0;
    int time_advantage = 0;


    string neumatico;





   
    // Start is called before the first frame update
    void Start()
    {
         manager = GameObject.FindGameObjectWithTag("manager");
        scriptlap=manager.GetComponent<laps>();
        B_ask.SetActive(true);
        B_lap.SetActive(false);
        ask_for_id();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ask_for_id()
    {
        id=scriptlap.give_id();
        if (id == -1)
        {
            print("error");
            B_ask.SetActive(false);

        }
        else
        {
            T_player.text = scriptlap.datos[id][0].ToString();
            B_ask.SetActive(false);
        B_lap.SetActive(true);
        }
    }


    //laps individual
    
       public void timer()
    {


        minor_fault = 0;
        medium_fault = 0;
        major_fault = 0;
        time_advantage = 0;

        int penalizacion = 0;



        if (id == -1)
        {
            print("error");
        }
        else
        {
            if (Convert.ToInt32(scriptlap.datos[id][7]) == 1)
            {
                medium_fault++;

            }
            if (Convert.ToInt32(scriptlap.datos[id][7]) == 2)
            {
                minor_fault++;
            }
            if (Convert.ToInt32(scriptlap.datos[id][7]) == 3)
            {
                time_advantage++;
            }



            for (int j = 0; j < minor_fault; j++) // Penalización leve
            {
                penalizacion += UnityEngine.Random.Range(50, 100); // Reducido de 150 a 100 máx.
            }
            for (int j = 0; j < medium_fault; j++) // Penalización media
            {
                penalizacion += UnityEngine.Random.Range(150, 300); // Reducido el máximo de 400 a 300.
            }
            for (int j = 0; j < major_fault; j++) // Penalización grave
            {
                penalizacion += UnityEngine.Random.Range(500, 1200); // Reducido el máximo de 1500 a 1200.
            }
            for (int j = 0; j < time_advantage; j++)
            {
                penalizacion -= UnityEngine.Random.Range(50, 200);
            }




            int tiempoActual = Convert.ToInt32(scriptlap.datos[id][1]);
        




        float nuevoTiempo = penalizacion + scriptlap.vuelta_promedio+ UnityEngine.Random.Range(0, 30);
            scriptlap.datos[id][2] = nuevoTiempo;

            scriptlap.datos[id][1] = tiempoActual + nuevoTiempo;
            scriptlap.datos[id][4] = Convert.ToInt32(scriptlap.datos[id][4]) + 1;

            print(nuevoTiempo);
        }
    }


    


    //monitores 
    [SerializeField] GameObject[] monitores;

    public void piloto()
    {
        monitores[0].SetActive(true);
        monitores[1].SetActive(false);
        monitores[2].SetActive(false);
    }
    public void mecanico()
    {
        monitores[0].SetActive(false);
        monitores[1].SetActive(true);
        monitores[2].SetActive(false);
    }
    public void radio()
    {
        monitores[0].SetActive(false);
        monitores[1].SetActive(false);
        monitores[2].SetActive(true);
    }


    //botones estado

    public void eco()
    {
        scriptlap.datos[id][7] = 1;

    }
    public void normal()
    {
        scriptlap.datos[id][7] = 2;

    }
    public void fast()
    {
        scriptlap.datos[id][7] = 3;

    }


    public void soft()
    {
        neumatico = "soft";

    }
    public void medium()
    {
        neumatico = "medium";

    }
    public void hard()
    {
        neumatico = "hard";

    }
}
