using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

    int minor_fault = 0;
    int medium_fault = 0;
    int major_fault = 0;
    int time_advantage = 0;




    void Start()
    {
        // Inicialización de datos
        // name, total time , time last lap, team , lap,compuesto neumatico, desgaste del compuesto,modo de pilotaje 
        datos.Add(new List<object> { "Max", 0, 0, 1, 0, "h", 100, 1 });
        datos.Add(new List<object> { "Russell", 0, 0, 2, 0, "h", 100, 2 });
        datos.Add(new List<object> { "Hamilton", 0, 0, 3, 0, "h", 100, 2 });
        datos.Add(new List<object> { "Carlos", 0, 0, 4, 0, "h", 100, 1 });
        datos.Add(new List<object> { "Alonso", 0, 0, 5, 0, "h", 100, 3 });
        datos.Add(new List<object> { "Rossi", 0, 0, 6, 0, "h", 100, 3 });

        datos = datos.OrderBy(x => UnityEngine.Random.Range(0f, 1f)).ToList();
        // Inicializar posiciones con los mismos datos
        foreach (var piloto in datos)
        {
            posiciones.Add(new List<object> { piloto[0], piloto[1], piloto[2], piloto[3], piloto[4], piloto[5], piloto[6], piloto[7] } );
        }
    

        degradacion =gameObject.GetComponent<Proceso_Degradacion>();

        jugadores = GameObject.FindGameObjectsWithTag("Player");
      
        StartCoroutine(tempo());
    }


    public void boton()
    {
        lap();
    }

    public int give_id()
    {
        if (dorsales <= datos.Count+1)
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
      

        // Actualizar tiempos
        for (int i = 0; i < datos.Count; i++)
        {

            minor_fault = 0;
            medium_fault = 0;
            major_fault = 0;
            time_advantage = 0;

            int penalizacion = 0;

            //tiempo acorde a pilotaje
            if (Convert.ToInt32(datos[i][7]) ==1)
            {
                medium_fault++;
         
            }
            if (Convert.ToInt32(datos[i][7]) == 2)
            {
                minor_fault++;
            }  if (Convert.ToInt32(datos[i][7]) == 3)
            {
                time_advantage++;
            }



            //comprobar estado neumaticos
            if (Convert.ToInt32(datos[i][6]) > 80)
            {
                minor_fault++;
            
           
            }
            else if (Convert.ToInt32(datos[i][6]) > 50 && Convert.ToInt32(datos[i][6]) <= 80)
            {
                //condicion optima
            }
            else if (Convert.ToInt32(datos[i][6]) > 30 && Convert.ToInt32(datos[i][6]) <= 50)
            {
                minor_fault++;
                minor_fault++;
            }
            else if (Convert.ToInt32(datos[i][6]) > 10 && Convert.ToInt32(datos[i][6]) <= 30)
            {
                medium_fault++;
                medium_fault++;
                medium_fault++;
            }
            else if (Convert.ToInt32(datos[i][6]) <= 10) // Aquí incluimos <= 10 explícitamente
            {
                major_fault++;
                major_fault++;
                major_fault++;
                major_fault++;
                major_fault++;
                major_fault++;
                major_fault++;
                print("fallooooooooo" + datos[i][0] + "  " + datos[i][6]);
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




          //  datos[i][6] = wheel_wear(datos[i][5], datos[i][6], datos[i][7]);

            int tiempoActual = Convert.ToInt32(datos[i][1]);

            float nuevoTiempo = UnityEngine.Random.Range(0, 30) + vuelta_promedio;
            datos[i][1] = tiempoActual + nuevoTiempo + penalizacion;
            datos[i][2] = nuevoTiempo + penalizacion; ;
            datos[i][4] = Convert.ToInt32(datos[i][4]) + 1;

            degradacion.wheel_wear();
        }
            evento();

        // Actualizar posiciones con los nuevos tiempos
        posiciones.Clear();
        foreach (var piloto in datos)
        {
            posiciones.Add(new List<object> { piloto[0], piloto[1], piloto[2], piloto[3], piloto[4], piloto[5], piloto[6], piloto[7] });
        }

        // Ordenar posiciones por tiempo (de menor a mayor)
        posiciones.Sort((a, b) => Convert.ToInt32(a[1]).CompareTo(Convert.ToInt32(b[1])));
        posiciones.Sort((b, a) => Convert.ToInt32(a[4]).CompareTo(Convert.ToInt32(b[4])));

        // Mostrar resultados en consola
     
        tiempos.GetComponent<positions>().AñadirPrefabAlPanel();
    }

    IEnumerator tempo()
    {
        yield return new WaitForSeconds(20f); // Espera 2 segundos
        lap();
    }




    //public int wheel_wear(object compuesto, object desgaste, object actitud)
    //{

    //    int x = 0;
    //    int graining = 0;
    //    if (Convert.ToInt32(actitud)==1)
    //    {
    //        x = 0;
    //    }
    //    if (Convert.ToInt32(actitud) == 2)
    //    {
    //        x = 5;
    //    }
    //    if (Convert.ToInt32(actitud) == 3)
    //    {
    //        x = 10;
    //    }
    //    if (compuesto.ToString() == "soft")
    //    {
    //        graining = Convert.ToInt32(desgaste) - (10+x);
    //    }
    //    else
    //    if (compuesto.ToString() == "medium")
    //    {
    //        graining = Convert.ToInt32(desgaste) - (5+x);
    //    }
    //    else

    //    {
    //        graining = Convert.ToInt32(desgaste) - (2+x);
    //    }
    //    if(graining <= 0)
    //    {
    //        graining = 0;
    //    }
    //    return graining;

    //}




    void evento()
    {
        int numeroAleatorio = Random.Range(0, jugadores.Length);
        jugadores[numeroAleatorio].gameObject.GetComponent<eventos>().test();

    }
}
