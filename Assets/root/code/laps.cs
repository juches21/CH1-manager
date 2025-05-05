using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class laps : MonoBehaviour
{
   // public int vuelta_promedio = 86252;
    //public List<List<object>> datos = new List<List<object>>();
    // public List<List<object>> posiciones = new List<List<object>>();
    GameObject[] jugadores;
    public GameObject tiempos;

    
    int dorsales;
    
    public int minor_fault = 0;
    public int medium_fault = 0;
    public int major_fault = 0;
    public int time_advantage = 0;

    Proceso_Degradacion degradacion;



    //int vueltas_max = 40;
    public int vueltas_act = 0;

    bool linea = true;


    public List<Data_base.Piloto> listaPilotos;
    public List<Data_base.Pista> listaPistas;
        public List<Data_base.Piloto> copiaSegura = new List<Data_base.Piloto>();

    void Start()
    {
        Data_base loader = FindObjectOfType<Data_base>();
        if (loader != null)
        {
            listaPilotos = loader.PilotosCargados;  //datos
        listaPilotos = listaPilotos.OrderBy(x => UnityEngine.Random.Range(0f, 1f)).ToList(); //aleatorizar lisata pilotos
            listaPistas = loader.PistasCargadas;
           // Debug.Log("Primera pista: " + copiaSegura[0].nombre);
        }
        else
        {
            Debug.LogError("No se encontró el script Data_base en la escena.");
        }


        //Debug.Log(copiaSegura[1].nombre); // Por ejemplo, imprime "Russell"
        //Debug.Log(listaPistas[0].nombre); // Por ejemplo, imprime "Russell"


        foreach (var piloto in listaPilotos)
        {
            Data_base.Piloto nuevo = new Data_base.Piloto()
            {
                nombre = piloto.nombre,
                numero = piloto.numero,
                escuderia = piloto.escuderia,
                compuesto = piloto.compuesto,
                desgaste = piloto.desgaste,
                tiempo_total = piloto.tiempo_total,
                tiempo_lap = piloto.tiempo_lap,
                modo = piloto.modo,
                vuelta = piloto.vuelta,
                casco = piloto.casco
            };

            copiaSegura.Add(nuevo);
        }

        // nombre, número, escudería, compuesto, desgaste, tiempo total, tiempo lap, modo, vuelta
        //datos.Add(new List<object> { "Max", 33, 1, "m", 100, 0, 0, 2, 0, "sol_y_luna" });
        //datos.Add(new List<object> { "Russell", 63, 2, "m", 100, 0, 0, 2, 0, "sol_y_luna" });
        //datos.Add(new List<object> { "Hamilton", 44, 3, "m", 100, 0, 0, 2, 0, "sol_y_luna" });
        //datos.Add(new List<object> { "Carlos", 55, 4, "m", 100, 0, 0, 2, 0, "sol_y_luna" });
        //datos.Add(new List<object> { "Alonso", 14, 5, "m", 100, 0, 0, 2, 0, "sol_y_luna" });
        //datos.Add(new List<object> { "Rossi", 46, 6, "m", 100, 0, 0, 2, 0, "sol_y_luna" });


        //foreach (var piloto in datos)
        //{
        //    posiciones.Add(new List<object>(piloto));
        //}

        degradacion = gameObject.GetComponent<Proceso_Degradacion>();
        jugadores = GameObject.FindGameObjectsWithTag("Player");

    }

    private void Update()
    {
        if (linea)
        {
            linea = false;
            StartCoroutine(tempo());
        }
    }

    //Debug
    public void boton()
    {
        if (vueltas_act < listaPistas[0].vueltas)
        {

            vueltas_act++;
            jugadores[0].gameObject.GetComponent<test_box>().timer();
            lap();
        }

    }
    //----------------
    public int give_id()
    {
        if (dorsales <= listaPilotos.Count + 1)
        {
            dorsales++;
            return dorsales - 1;
        }
        else
        {
            return -1; // Retornar un valor por defecto en caso de que la condición no se cumpla
        }
    }

    public void lap()
    {
        for (int i = 1; i < listaPilotos.Count; i++)
        {
            minor_fault = medium_fault = major_fault = time_advantage = 0;
            float penalizacion = 0;

            int modo = Convert.ToInt32(listaPilotos[i].modo);
            int desgaste = Convert.ToInt32(listaPilotos[i].desgaste);

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
                print("fallo: " + listaPilotos[i].nombre + "  " + desgaste);
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

            float tiempoActual = Convert.ToInt32(listaPilotos[i].tiempo_total);
            float nuevoTiempo = UnityEngine.Random.Range(0, 30) + 86252;

            listaPilotos[i].tiempo_total = Convert.ToInt32( tiempoActual + nuevoTiempo + penalizacion); // Tiempo total
            listaPilotos[i].tiempo_lap = Convert.ToInt32(nuevoTiempo + penalizacion);               // Última vuelta
            listaPilotos[i].vuelta = Convert.ToInt32(listaPilotos[i].vuelta) + 1;          // Sumar vuelta

            boxbox(i);
            Ritmo(i);
        }

        //degradacion.wheel_wear();
        evento();

        copiaSegura.Clear();
        foreach (var piloto in listaPilotos)
        {
            Data_base.Piloto nuevo = new Data_base.Piloto()
            {
                nombre = piloto.nombre,
                numero = piloto.numero,
                escuderia = piloto.escuderia,
                compuesto = piloto.compuesto,
                desgaste = piloto.desgaste,
                tiempo_total = piloto.tiempo_total,
                tiempo_lap = piloto.tiempo_lap,
                modo = piloto.modo,
                vuelta = piloto.vuelta,
                casco = piloto.casco
            };

            copiaSegura.Add(nuevo);
        }
        copiaSegura.Sort((a, b) => Convert.ToInt32(a.tiempo_total).CompareTo(Convert.ToInt32(b.tiempo_total)));
        copiaSegura.Sort((b, a) => Convert.ToInt32(a.vuelta).CompareTo(Convert.ToInt32(b.vuelta)));

        tiempos.GetComponent<positions>().AñadirPrefabAlPanel();
    }

    IEnumerator tempo()
    {

        if (vueltas_act < listaPistas[0].vueltas)
        {
            yield return new WaitForSeconds(1f); // Espera 2 segundos

            vueltas_act++;
            jugadores[0].gameObject.GetComponent<test_box>().timer();
            lap();
            linea = true;
        }

    }






    void evento()
    {

        jugadores[0].gameObject.GetComponent<eventos>().test();

    }




    //IA


    public void boxbox(int id)
    {
        int valor = Convert.ToInt32(listaPilotos[id].desgaste);

        int numero = 0;
        if (valor > 50)
        {
            numero = 10000;
        }
        else if (valor <= 50)
        {
            numero = 5000;
        }
        if (valor < 20)
        {
            numero = 1000;
        }
        if (valor < 10)
        {
            numero = 100;
        }

        int probavilidad = UnityEngine.Random.Range(0, 10000);
        if (probavilidad > numero)
        {

            string neumatico;
            int probabilidad = Random.Range(0, 100); // Genera número entre 0 y 99

            if (probabilidad < 60)
            {
                 neumatico = "h";
            }
            else if (probabilidad < 90)
            {
                 neumatico = "m";
            }
            else
            {
                 neumatico = "s";
            }

            
            listaPilotos[id].desgaste = 100; // desgaste
            listaPilotos[id].compuesto = neumatico; // compuesto
            listaPilotos[id].tiempo_total = Convert.ToInt32(listaPilotos[id].tiempo_total) + UnityEngine.Random.Range(9000, 15000); // tiempo total
            print(listaPilotos[id].nombre + " cambio de riueda");
        }


    }

    public void Ritmo(int id)
    {
        int valor = Convert.ToInt32(listaPilotos[id].desgaste);


        int probavilidad = UnityEngine.Random.Range(0, 100);
        if (probavilidad > 50)
        {

            int actitud;
            int probabilidad = Random.Range(0, 100); // Genera número entre 0 y 99

            if (probabilidad < 20)
            {
                actitud = 1;
            }
            else if (probabilidad < 70)
            {
                actitud = 2;
            }
            else
            {
                actitud = 3;
            }


         
            listaPilotos[id].modo = actitud; // compuesto
            print(listaPilotos[id].nombre + " cambio de ritmo");
        }


    }


}
