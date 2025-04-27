using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class laps : MonoBehaviour
{
    public int vuelta_promedio = 86252;
    public List<List<object>> datos = new List<List<object>>();
    public List<List<object>> posiciones = new List<List<object>>();
    public int dorsales;

    GameObject[] jugadores;

    Proceso_Degradacion degradacion;

    public GameObject tiempos;

    public int minor_fault = 0;
    public int medium_fault = 0;
    public int major_fault = 0;
    int time_advantage = 0;

    int vueltas_max = 40;
    public int vueltas_act = 0;

    bool linea = true;
    void Start()
    {
        // nombre, número, escudería, compuesto, desgaste, tiempo total, tiempo lap, modo, vuelta
        datos.Add(new List<object> { "Max", 33, 1, "m", 100, 0, 0, 2, 0, "sol_y_luna" });
        datos.Add(new List<object> { "Russell", 63, 2, "m", 100, 0, 0, 2, 0, "sol_y_luna" });
        datos.Add(new List<object> { "Hamilton", 44, 3, "m", 100, 0, 0, 2, 0, "sol_y_luna" });
        datos.Add(new List<object> { "Carlos", 55, 4, "m", 100, 0, 0, 2, 0, "sol_y_luna" });
        datos.Add(new List<object> { "Alonso", 14, 5, "m", 100, 0, 0, 2, 0, "sol_y_luna" });
        datos.Add(new List<object> { "Rossi", 46, 6, "m", 100, 0, 0, 2, 0, "sol_y_luna" });

        datos = datos.OrderBy(x => UnityEngine.Random.Range(0f, 1f)).ToList();

        foreach (var piloto in datos)
        {
            posiciones.Add(new List<object>(piloto));
        }

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
    public void boton()
    {
        if (vueltas_act < vueltas_max)
        {

            vueltas_act++;
            jugadores[0].gameObject.GetComponent<test_box>().timer();
            lap();
        }

    }

    public int give_id()
    {
        if (dorsales <= datos.Count + 1)
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
        for (int i = 1; i < datos.Count; i++)
        {
            minor_fault = medium_fault = major_fault = time_advantage = 0;
            int penalizacion = 0;

            int modo = Convert.ToInt32(datos[i][7]);
            int desgaste = Convert.ToInt32(datos[i][4]);

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
                print("fallo: " + datos[i][0] + "  " + desgaste);
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

            int tiempoActual = Convert.ToInt32(datos[i][5]);
            float nuevoTiempo = UnityEngine.Random.Range(0, 30) + vuelta_promedio;

            datos[i][5] = tiempoActual + nuevoTiempo + penalizacion; // Tiempo total
            datos[i][6] = nuevoTiempo + penalizacion;               // Última vuelta
            datos[i][8] = Convert.ToInt32(datos[i][8]) + 1;          // Sumar vuelta

            boxbox(i);
        }

        degradacion.wheel_wear();
        evento();

        posiciones.Clear();
        foreach (var piloto in datos)
        {
            posiciones.Add(new List<object>(piloto));
        }

        posiciones.Sort((a, b) => Convert.ToInt32(a[5]).CompareTo(Convert.ToInt32(b[5])));
        posiciones.Sort((b, a) => Convert.ToInt32(a[8]).CompareTo(Convert.ToInt32(b[8])));

        tiempos.GetComponent<positions>().AñadirPrefabAlPanel();
    }

    IEnumerator tempo()
    {

        if (vueltas_act < vueltas_max)
        {
            yield return new WaitForSeconds(1f); // Espera 2 segundos

            vueltas_act++;
            //jugadores[0].gameObject.GetComponent<test_box>().timer();
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
        int valor = Convert.ToInt32(datos[id][4]);

        int numero=0;
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

            string neumatico = "s";
            datos[id][4] = 100; // desgaste
            datos[id][3] = neumatico; // compuesto
            datos[id][5] = Convert.ToInt32(datos[id][5]) + UnityEngine.Random.Range(9000, 15000); // tiempo total
            print(datos[id][0] + " cambio de riueda");
        }


    }
}
