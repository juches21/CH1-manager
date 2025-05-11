using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;
using Random = UnityEngine.Random;
using TMPro;
public class Manager : MonoBehaviour
{
    // === PUBLIC VARIABLES ===
    public GameObject tiempos;
    public VideoPlayer vide_lap;
    public List<Data_base.Piloto> pilotsList;
    public List<Data_base.Pista> tracksList;
    public List<Data_base.Piloto> backupPilotsList = new List<Data_base.Piloto>();

    // === PUBLIC VARIABLES - Fault Counters ===
    public int minorFaultCount = 0;
    public int mediumFaultCount = 0;
    public int majorFaultCount = 0;
    public int timeAdvantageCount = 0;

    // === PRIVATE VARIABLES ===
    private GameObject[] jugadores;
    private TyreWearManager degradacion;

    // === PRIVATE VARIABLES - Gameplay State ===
    private int dorsales;
    private int currentLap = 0;
    private bool isLapComplete = true;


    [SerializeField] TextMeshProUGUI tex_laps;

    void Start()
    {
        Data_base loader = FindObjectOfType<Data_base>();
        if (loader != null)
        {
            pilotsList = loader.PilotosCargados;  //datos
            pilotsList = pilotsList.OrderBy(x => UnityEngine.Random.Range(0f, 1f)).ToList(); //aleatorizar lisata pilotos
            tracksList = loader.PistasCargadas;
        }
        else
        {
            Debug.LogError("No se encontró el script Data_base en la escena.");
        }




        foreach (var piloto in pilotsList)
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

            backupPilotsList.Add(nuevo);
        }
        tex_laps.text = currentLap + "/" + tracksList[0].vueltas;

   


        degradacion = gameObject.GetComponent<TyreWearManager>();
        jugadores = GameObject.FindGameObjectsWithTag("Player");

    }

    private void Update()
    {
        if (isLapComplete)
        {
            isLapComplete = false;
            StartCoroutine(LapTimerRoutine());
        }
    }

    

    //----------------
    public int AssignPlayerID()
    {
        if (dorsales <= pilotsList.Count + 1)
        {
            dorsales++;
            return dorsales - 1;
        }
        else
        {
            return -1; // Retornar un valor por defecto en caso de que la condición no se cumpla
        }
    }

    public void UpdateLapData()
    {
        
        tex_laps.text = currentLap + "/" + tracksList[0].vueltas;
        for (int i = 1; i < pilotsList.Count; i++)
        {
            minorFaultCount = mediumFaultCount = majorFaultCount = timeAdvantageCount = 0;
            float penalizacion = 0;

            int modo = Convert.ToInt32(pilotsList[i].modo);
            int desgaste = Convert.ToInt32(pilotsList[i].desgaste);

            if (modo == 1)
            {
                penalizacion += UnityEngine.Random.Range(150, 300);
            }
            if (modo == 2)
            {
                penalizacion += UnityEngine.Random.Range(50, 100);
            }
            if (modo == 3)
            {
                penalizacion -= UnityEngine.Random.Range(500, 900);
            }

            if (desgaste > 80)
            {
                minorFaultCount++;
            }
            else if (desgaste > 50)
            { }
            else if (desgaste > 30)
            {
                mediumFaultCount += 2;
            }
            else if (desgaste > 10)
            {
                majorFaultCount += 7;
            }
            else if (desgaste <= 10)
            {
                majorFaultCount += 9;
            }

            for (int j = 0; j <= minorFaultCount; j++)
            {
                penalizacion += UnityEngine.Random.Range(50, 100);
            }
            for (int j = 0; j <= mediumFaultCount; j++)
            {
                penalizacion += UnityEngine.Random.Range(150, 300);
            }
            for (int j = 0; j <= majorFaultCount; j++)
            {
                penalizacion += UnityEngine.Random.Range(500, 1000);
            }
            for (int j = 0; j <= timeAdvantageCount; j++)
            {
                penalizacion -= UnityEngine.Random.Range(50, 200);
            }

            float tiempoActual = Convert.ToInt32(pilotsList[i].tiempo_total);
            float nuevoTiempo = UnityEngine.Random.Range(0, 90) + tracksList[0].tiempo_promedio;

            pilotsList[i].tiempo_total = Convert.ToInt32(tiempoActual + nuevoTiempo + penalizacion); // Tiempo total
            pilotsList[i].tiempo_lap = Convert.ToInt32(nuevoTiempo + penalizacion);               // Última vuelta
            pilotsList[i].vuelta = Convert.ToInt32(pilotsList[i].vuelta) + 1;          // Sumar vuelta
            penalizacion = 0;
            HandlePitStop(i);
            AdjustPilotPace(i);
        }

        degradacion.ApplyTyreWear();
        TriggerRadioEvent();

        backupPilotsList.Clear();
        foreach (var piloto in pilotsList)
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

            backupPilotsList.Add(nuevo);
        }
        backupPilotsList.Sort((a, b) => Convert.ToInt32(a.tiempo_total).CompareTo(Convert.ToInt32(b.tiempo_total)));
        backupPilotsList.Sort((b, a) => Convert.ToInt32(a.vuelta).CompareTo(Convert.ToInt32(b.vuelta)));

        tiempos.GetComponent<TimeTable>().UpdateTimeTable();
    }

    IEnumerator LapTimerRoutine()
    {

        if (currentLap < tracksList[0].vueltas)
        {
            vide_lap.Stop();
            vide_lap.Play();
            yield return new WaitForSeconds(0.2f);
            currentLap++;
            jugadores[0].gameObject.GetComponent<Player>().UpdateLapTime();
            UpdateLapData();
            yield return new WaitForSeconds(20f); // Espera 2 segundos
            isLapComplete = true;
        }

    }






    void TriggerRadioEvent()
    {
        jugadores[0].gameObject.GetComponent<RadioManager>().TriggerRadioMessage();

    }




    //IA


    public void HandlePitStop(int id)
    {
        int valor = Convert.ToInt32(pilotsList[id].desgaste);

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


            pilotsList[id].desgaste = 100; // desgaste
            pilotsList[id].compuesto = neumatico; // compuesto
            pilotsList[id].tiempo_total = Convert.ToInt32(pilotsList[id].tiempo_total) + UnityEngine.Random.Range(9000, 15000); // tiempo total
            print(pilotsList[id].nombre + " cambio de riueda");
        }


    }

    public void AdjustPilotPace(int id)
    {


        int probavilidad = UnityEngine.Random.Range(0, 100);
        if (probavilidad > 50)
        {

            int actitud;
            int probabilidad = Random.Range(0, 100);

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



            pilotsList[id].modo = actitud; // compuesto
            //print(listaPilotos[id].nombre + " cambio de ritmo");ad
        }


    }


}
