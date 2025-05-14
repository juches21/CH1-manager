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
    public GameObject tiempos; // Referencia al objeto que contiene la tabla de tiempos
    public VideoPlayer vide_lap; // Referencia al VideoPlayer para gestionar las repeticiones de la vuelta
    public List<Data_base.Piloto> pilotsList; // Lista de pilotos cargados desde la base de datos
    public List<Data_base.Pista> tracksList; // Lista de pistas cargadas desde la base de datos
    public List<Data_base.Piloto> backupPilotsList = new List<Data_base.Piloto>(); // Backup de la lista de pilotos para guardar el estado anterior

    // === PUBLIC VARIABLES - Fault Counters ===
    // Variables para contar fallos según el nivel de desgaste de los neumáticos
    public int minorFaultCount = 0;
    public int mediumFaultCount = 0;
    public int majorFaultCount = 0;
    public int timeAdvantageCount = 0;

    // === PRIVATE VARIABLES ===
    private GameObject[] jugadores; // Array de jugadores en la escena
    private TyreWearManager degradacion; // Componente para gestionar la degradación de los neumáticos

    // === PRIVATE VARIABLES - Gameplay State ===
    private int dorsales; // ID de los pilotos
    public int id_piloto;
    private int currentLap = 0; // Número de vuelta actual
    private bool isLapComplete = true; // Bandera para saber si una vuelta ha finalizado

    [SerializeField] TextMeshProUGUI tex_laps; // UI para mostrar el número de vuelta
    [SerializeField] GameObject Panel_podio; // Panel que muestra el podio al final de la carrera

    void Start()
    {
        isLapComplete = false;

        Data_base loader = FindObjectOfType<Data_base>(); // Encuentra el script de la base de datos
        if (loader != null)
        {
            // Carga y aleatoriza los pilotos
            pilotsList = loader.PilotosCargados;
            //pilotsList = pilotsList.OrderBy(x => UnityEngine.Random.Range(0f, 1f)).ToList();
            tracksList = loader.PistasCargadas;
        }
        else
        {
            Debug.LogError("No se encontró el script Data_base en la escena.");
        }

        // Realiza un backup de la lista de pilotos para preservar el estado
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

        // Inicializa la UI para las vueltas
        tex_laps.text = currentLap + "/" + tracksList[0].vueltas;

        // Obtiene el componente de degradación y los objetos de los jugadores
        degradacion = gameObject.GetComponent<TyreWearManager>();
        jugadores = GameObject.FindGameObjectsWithTag("Player");
       
        
      
    }

    public void iniciarjuego()
    {
        isLapComplete = true;
        
        pilotsList.Insert(0, pilotsList[id_piloto]);
        pilotsList.RemoveAt(id_piloto+1);
        //jugadores[0].GetComponent<Player>().AskPlayerID();
        StartCoroutine(esperarstart());
        
    }
    IEnumerator esperarstart()
    {
        yield return new WaitForSeconds(.1f); // Espera 100ms antes de completar la vuelta
        jugadores[0].GetComponent<Player>().AskPlayerID();

    }


    private void Update()
    {
        if (isLapComplete)
        {
            isLapComplete = false;
            StartCoroutine(LapTimerRoutine()); // Inicia el contador de la vuelta
        }
    }

    //----------------
    public int AssignPlayerID()
    {
        // Asigna un ID a los jugadores incrementando el contador
        if (dorsales <= pilotsList.Count + 1)
        {
            dorsales++;
            return dorsales - 1;
        }
        else
        {
            return -1; // Si no hay más pilotos disponibles, retorna un valor por defecto
        }
    }

    public void UpdateLapData()
    {
        // Limpia la lista de pilotos respaldada y actualiza los datos de la vuelta
        backupPilotsList.Clear();
        tex_laps.text = currentLap + "/" + tracksList[0].vueltas;

        for (int i = 1; i < pilotsList.Count; i++)
        {
            minorFaultCount = mediumFaultCount = majorFaultCount = timeAdvantageCount = 0; // Reinicia los contadores de fallos
            float penalizacion = 0;

            int modo = Convert.ToInt32(pilotsList[i].modo);
            int desgaste = Convert.ToInt32(pilotsList[i].desgaste);

            // Asigna penalizaciones según el modo de conducción
            if (modo == 1) penalizacion += UnityEngine.Random.Range(150, 300);
            if (modo == 2) penalizacion += UnityEngine.Random.Range(50, 100);
            if (modo == 3) penalizacion -= UnityEngine.Random.Range(500, 900);

            // Determina los fallos según el nivel de desgaste de los neumáticos
            if (desgaste > 80) minorFaultCount++;
            else if (desgaste > 50) { }
            else if (desgaste > 30) mediumFaultCount += 2;
            else if (desgaste > 10) majorFaultCount += 7;
            else if (desgaste <= 10) majorFaultCount += 9;

            // Aplica penalizaciones basadas en los fallos
            for (int j = 0; j <= minorFaultCount; j++) penalizacion += UnityEngine.Random.Range(50, 100);
            for (int j = 0; j <= mediumFaultCount; j++) penalizacion += UnityEngine.Random.Range(150, 300);
            for (int j = 0; j <= majorFaultCount; j++) penalizacion += UnityEngine.Random.Range(500, 1000);
            for (int j = 0; j <= timeAdvantageCount; j++) penalizacion -= UnityEngine.Random.Range(50, 200);

            // Calcula el tiempo total de la vuelta
            float tiempoActual = Convert.ToInt32(pilotsList[i].tiempo_total);
            float nuevoTiempo = UnityEngine.Random.Range(0, 90) + tracksList[0].tiempo_promedio;

            pilotsList[i].tiempo_total = Convert.ToInt32(tiempoActual + nuevoTiempo + penalizacion);
            pilotsList[i].tiempo_lap = Convert.ToInt32(nuevoTiempo + penalizacion);
            pilotsList[i].vuelta = Convert.ToInt32(pilotsList[i].vuelta) + 1;
            penalizacion = 0;

            // Maneja el pit stop y ajusta el ritmo del piloto
            HandlePitStop(i);
            AdjustPilotPace(i);
        }

        degradacion.ApplyTyreWear(); // Aplica la degradación de los neumáticos
        TriggerRadioEvent(); // Llama al evento de radio

        // Realiza un backup de la lista de pilotos después de la actualización
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

        // Ordena la lista de pilotos según el tiempo total y la vuelta
        backupPilotsList.Sort((a, b) => Convert.ToInt32(a.tiempo_total).CompareTo(Convert.ToInt32(b.tiempo_total)));
        backupPilotsList.Sort((b, a) => Convert.ToInt32(a.vuelta).CompareTo(Convert.ToInt32(b.vuelta)));

        // Actualiza la tabla de tiempos en la UI
        tiempos.GetComponent<TimeTable>().UpdateTimeTable();
    }

    IEnumerator LapTimerRoutine()
    {
        // Controla el tiempo de la vuelta, y cuando termina, actualiza los datos de la carrera
        if (currentLap < tracksList[0].vueltas)
        {
            Panel_podio.SetActive(false);
            vide_lap.Stop();
            vide_lap.Play();
            yield return new WaitForSeconds(0.2f); // Espera antes de cambiar la vuelta
            currentLap++;
            jugadores[0].gameObject.GetComponent<Player>().UpdateLapTime();
            UpdateLapData();
            yield return new WaitForSeconds(.1f); // Espera 100ms antes de completar la vuelta
            isLapComplete = true;
        }
        else
        {
            // Si la carrera termina, muestra el podio
            Panel_podio.SetActive(true);
            Panel_podio.GetComponent<Podium>().mostrardatos();
        }
    }

    void TriggerRadioEvent()
    {
        // Llama al evento de radio del primer jugador
        jugadores[0].gameObject.GetComponent<RadioManager>().TriggerRadioMessage();
    }

    // IA

    public void HandlePitStop(int id)
    {
        // Maneja las paradas en boxes según el desgaste de los neumáticos
        int valor = Convert.ToInt32(pilotsList[id].desgaste);
        int numero = 0;

        // Determina la probabilidad de cambio de neumáticos en función del desgaste
        if (valor > 50) numero = 10000;
        else if (valor <= 50) numero = 5000;
        if (valor < 20) numero = 1000;
        if (valor < 10) numero = 100;

        int probavilidad = UnityEngine.Random.Range(0, 10000);
        if (probavilidad > numero)
        {
            // Elige el neumático que cambiará
            string neumatico;
            int probabilidad = Random.Range(0, 100);

            if (probabilidad < 60) neumatico = "h"; // Neumático duro
            else if (probabilidad < 90) neumatico = "m"; // Neumático medio
            else neumatico = "s"; // Neumático blando

            // Actualiza los datos del piloto tras la parada
            pilotsList[id].desgaste = 100; // Reestablece el desgaste
            pilotsList[id].compuesto = neumatico; // Asigna el nuevo compuesto
            pilotsList[id].tiempo_total = Convert.ToInt32(pilotsList[id].tiempo_total) + UnityEngine.Random.Range(9000, 15000); // Añade tiempo por la parada
            print(pilotsList[id].nombre + " cambio de riueda");
        }
    }

    public void AdjustPilotPace(int id)
    {
        // Ajusta el ritmo de conducción del piloto en función de la probabilidad
        int probavilidad = UnityEngine.Random.Range(0, 100);
        if (probavilidad > 50)
        {
            // Asigna una nueva actitud de conducción (1: conservador, 2: normal, 3: agresivo)
            int actitud;
            int probabilidad = Random.Range(0, 100);

            if (probabilidad < 20) actitud = 1;
            else if (probabilidad < 70) actitud = 2;
            else actitud = 3;

            pilotsList[id].modo = actitud; // Asigna la actitud
        }
    }
}
