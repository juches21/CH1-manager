using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // === PUBLIC GAMEOBJECTS ===
    public GameObject B_ask;
    public GameObject B_lap;
    public GameObject manager;

    // === PUBLIC UI ELEMENTS ===
    public TextMeshProUGUI T_player;
    public TextMeshProUGUI T_desgaste;
    public Image Helmet;
    public Image Car;

    // === PUBLIC FAULT COUNTERS ===
    public int minor_fault = 0;
    public int medium_fault = 0;
    public int major_fault = 0;
    public int time_advantage = 0;

    // === PUBLIC PENALTY / STATUS ===
    public int Penalty = 0;

    // === PRIVATE / SERIALIZED VARIABLES ===
    [SerializeField] private int id;
    [SerializeField] private GameObject panel_pit;
    private laps scriptlap;

    // === PRIVATE STATE VARIABLES ===
    private string neumatico;

    public List<Data_base.Escuderia> listaEscuderias;


    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager");
        scriptlap = manager.GetComponent<laps>();

        Data_base loader = FindObjectOfType<Data_base>();
        if (loader != null)
        {
            listaEscuderias = loader.EscuderiasCargadas;  //datos
        }
        else
        {
            Debug.LogError("No se encontró el script Data_base en la escena.");
        }

        B_ask.SetActive(true);
        B_lap.SetActive(false);
        ask_for_id();
        normal();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ask_for_id()
    {
        id = scriptlap.AssignPlayerID();
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
        Load_Car();
        Load_Helmet();
    }


    //laps individual

    public void timer()
    {

        //print("lapeada indi");
  
        time_advantage = 0;








        int modo = Convert.ToInt32(scriptlap.listaPilotos[id].modo);
        int Wear = Convert.ToInt32(scriptlap.listaPilotos[id].desgaste);


        if (modo == 1)
        {
            Penalty += UnityEngine.Random.Range(150, 300);
        }
        if (modo == 2)
        {
            Penalty += UnityEngine.Random.Range(50, 100);
        }
        if (modo == 3)
        {
            Penalty -= UnityEngine.Random.Range(500, 900);
        }

        if (Wear > 80)
        {
            Penalty += UnityEngine.Random.Range(50, 100);
        }
        else if (Wear > 50)
        { }
        else if (Wear > 30)
        {
            Penalty += UnityEngine.Random.Range(150, 900);
        }
        else if (Wear > 10)
        {
            Penalty += UnityEngine.Random.Range(900, 12000);
        }
        else if (Wear <= 10)
        {
            Penalty += UnityEngine.Random.Range(12000, 15000);
            
        }

        
        for (int j = 0; j <= minor_fault; j++)
        {
            Penalty += UnityEngine.Random.Range(50, 100);
        }
        for (int j = 0; j <= medium_fault; j++)
        {
            Penalty += UnityEngine.Random.Range(150, 300);
        }
        for (int j = 0; j <= major_fault; j++)
        {
            Penalty += UnityEngine.Random.Range(500, 1000);
        }
        for (int j = 0; j <= time_advantage; j++)
        {
         
            Penalty -= UnityEngine.Random.Range(500, 900);
        }
        



        int tiempoActual = Convert.ToInt32(scriptlap.listaPilotos[id].tiempo_total);





        float nuevoTiempo = scriptlap.listaPistas[0].tiempo_promedio + UnityEngine.Random.Range(0, 90);

        scriptlap.listaPilotos[id].tiempo_total = Convert.ToInt32(tiempoActual + nuevoTiempo + Penalty);  // .tiempo_total es total
        scriptlap.listaPilotos[id].tiempo_lap = Convert.ToInt32(nuevoTiempo + Penalty);  // .tiempo_lap es última vuelta
        scriptlap.listaPilotos[id].vuelta = Convert.ToInt32(scriptlap.listaPilotos[id].vuelta) + 1;



         T_desgaste.text = "Neumatico " + scriptlap.listaPilotos[id].compuesto.ToString() +" : "+ scriptlap.listaPilotos[id].desgaste.ToString()+"%";

        Penalty = 0;



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
        medium_fault++;
    }
    public void normal()
    {
        scriptlap.listaPilotos[id].modo = 2;
        minor_fault++;
    }
    public void fast()
    {
        scriptlap.listaPilotos[id].modo = 3;
        time_advantage++;
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
        if(neumatico != null)
        {
        gameObject.GetComponent<Pit_stop>().time();

        }


    }

    public void box_time(int tiempo)
    {
        scriptlap.listaPilotos[id].desgaste = 100; // desgaste
        scriptlap.listaPilotos[id].compuesto = neumatico; // compuesto
        scriptlap.listaPilotos[id].tiempo_total = Convert.ToInt32(scriptlap.listaPilotos[id].tiempo_total) + tiempo+9000; // tiempo total


    }

   
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
            Helmet.sprite = sprite;  // Asignar el sprite al objeto de imagen
        }
        else
        {
            Debug.LogError("No se pudo cargar la imagen. Asegúrate de que la ruta sea correcta.");
            // Imprimir más información de depuración para verificar la ruta
            print("Ruta de la imagen no válida o archivo no encontrado: " + imagePath);
        }
    }

    void Load_Car()
    {
        // Obtener la ruta del casco desde los datos
        int numero = scriptlap.listaPilotos[id].escuderia;
        print(numero + "numero");
        print(listaEscuderias[numero-1].coche);
        string imagePath = "Fotos/Monoplazas_up/" + listaEscuderias[numero-1].coche;

        // Cargar el sprite desde Resources usando la ruta proporcionada
        Sprite sprite = Resources.Load<Sprite>(imagePath);
   

        // Comprobar si la imagen fue cargada correctamente
        if (sprite != null)
        {
            Car.sprite = sprite;  // Asignar el sprite al objeto de imagen
        }
        else
        {
            Debug.LogError("No se pudo cargar la imagen. Asegúrate de que la ruta sea correcta.");
            // Imprimir más información de depuración para verificar la ruta
            print("Ruta de la imagen no válida o archivo no encontrado: " + imagePath);
        }
    }

}
