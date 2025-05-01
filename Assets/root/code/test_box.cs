using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class test_box : MonoBehaviour
{
    GameObject manager;
    public GameObject B_ask;
    public GameObject B_lap;
    public TextMeshProUGUI T_player;
    [SerializeField] int id;
    laps scriptlap;


    public int minor_fault = 0;
    public int medium_fault = 0;
    public int major_fault = 0;
    int time_advantage = 0;


    string neumatico;


    public int penalizacion = 0;

    [SerializeField] GameObject panel_pit;


    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager");
        scriptlap = manager.GetComponent<laps>();
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
        id = scriptlap.give_id();
        if (id == -1)
        {
            print("error");
            B_ask.SetActive(false);

        }
        else
        {

            T_player.text = scriptlap.listaPilotos[id].nombre.ToString();
            B_ask.SetActive(false);
            B_lap.SetActive(true);
        }

        Load_Helmet();
    }


    //laps individual

    public void timer()
    {

        //print("lapeada indi");
        minor_fault = 0;
        medium_fault = 0;
        major_fault = 0;
        time_advantage = 0;








        int modo = Convert.ToInt32(scriptlap.listaPilotos[id].modo);
        int desgaste = Convert.ToInt32(scriptlap.listaPilotos[id].desgaste);

        if (modo == 1) medium_fault++;
        if (modo == 2) minor_fault++;
        if (modo == 3) time_advantage++;


        if (desgaste > 80)
        {
            minor_fault++;
        }
        else if (desgaste > 50)
        { }
        else if (desgaste > 30)
        {
            medium_fault += 2;
        }
        else if (desgaste > 10)
        {
            major_fault += 7;
        }
        else if (desgaste <= 10)
        {
            major_fault += 9;
        }

        
        for (int j = 0; j <= minor_fault; j++)
        {
            penalizacion += UnityEngine.Random.Range(50, 100);
        }
        for (int j = 0; j <= medium_fault; j++)
        {
            penalizacion += UnityEngine.Random.Range(150, 300);
        }
        for (int j = 0; j <= major_fault; j++)
        {
            penalizacion += UnityEngine.Random.Range(500, 1000);
        }
        for (int j = 0; j <= time_advantage; j++)
        {
            penalizacion -= UnityEngine.Random.Range(50, 200);
        }
        



        int tiempoActual = Convert.ToInt32(scriptlap.listaPilotos[id].tiempo_total);





        float nuevoTiempo = 86252 + UnityEngine.Random.Range(0, 30);

        scriptlap.listaPilotos[id].tiempo_total = Convert.ToInt32(tiempoActual + nuevoTiempo + penalizacion);  // .tiempo_total es total
        scriptlap.listaPilotos[id].tiempo_lap = Convert.ToInt32(nuevoTiempo + penalizacion);  // .tiempo_lap es última vuelta
        scriptlap.listaPilotos[id].vuelta = Convert.ToInt32(scriptlap.listaPilotos[id].vuelta) + 1;

        //print(nuevoTiempo);




        penalizacion = 0;



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
        scriptlap.listaPilotos[id].modo = 1;

    }
    public void normal()
    {
        scriptlap.listaPilotos[id].modo = 2;

    }
    public void fast()
    {
        scriptlap.listaPilotos[id].modo = 3;

    }

    //botones neumaticos
    public void soft()
    {
        neumatico = "s";


    }
    public void medium()
    {
        neumatico = "m";

    }
    public void hard()
    {
        neumatico = "h";

    }


    public void boxbox()
    {

        gameObject.GetComponent<Pit_stop>().time();


    }

    public void box_time(int tiempo)
    {
        scriptlap.listaPilotos[id].desgaste = 100; // desgaste
        scriptlap.listaPilotos[id].compuesto = neumatico; // compuesto
        scriptlap.listaPilotos[id].tiempo_total = Convert.ToInt32(scriptlap.listaPilotos[id].tiempo_total) + tiempo; // tiempo total


    }


    public Image imageObject;
    void Load_Helmet()
    {
        // Obtener la ruta del casco desde los datos
        string imagePath = "Fotos/Cascos/" + scriptlap.listaPilotos[id].casco;

        // Cargar el sprite desde Resources usando la ruta proporcionada
        Sprite sprite = Resources.Load<Sprite>(imagePath);
        Debug.Log("Ruta de la imagen: " + imagePath);

        // Comprobar si la imagen fue cargada correctamente
        if (sprite != null)
        {
            imageObject.sprite = sprite;  // Asignar el sprite al objeto de imagen
        }
        else
        {
            Debug.LogError("No se pudo cargar la imagen. Asegúrate de que la ruta sea correcta.");
            // Imprimir más información de depuración para verificar la ruta
            print("Ruta de la imagen no válida o archivo no encontrado: " + imagePath);
        }
    }

}
