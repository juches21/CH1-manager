using System.Collections.Generic;
using UnityEngine;

public class Data_base : MonoBehaviour
{
    public TextAsset DB_Pilotos;
    public TextAsset DB_Pistas;
    public TextAsset DB_Escuderias;
    public TextAsset DB_Radios;

    [System.Serializable]
    public class Piloto
    {
        public string nombre;
        public int numero;
        public int escuderia;
        public string compuesto;
        public int desgaste;
        public int tiempo_total;
        public int tiempo_lap;
        public int modo;
        public int vuelta;
        public string casco;
    }

    [System.Serializable]
    public class Pista
    {
        public string nombre;
        public int vueltas;
        public int tiempo_promedio;
    }

    [System.Serializable]
    public class Escuderia
    {
        public int id;
        public string nombre;
        public string imagen;
        public string color;
        public string coche;
    }


    [System.Serializable]
    public class Radio
    {
        public string texto;
        public string opcion_1;
        public string opcion_2;
        
    }
    [System.Serializable]
    public class PilotoList
    {
        public List<Piloto> pilotos; // <-- Coincide con el JSON
    }

    [System.Serializable]
    public class PistaList
    {
        public List<Pista> pistas; // <-- Coincide con el JSON
    }
    [System.Serializable]
    public class EscuderiaList
    {
        public List<Escuderia> escuderia; // <-- Coincide con el JSON
    }


    [System.Serializable]
    public class RadioList
    {
        public List<Radio> radios; // <-- Coincide con el JSON
    }


    public List<Piloto> PilotosCargados = new List<Piloto>();
    public List<Pista> PistasCargadas = new List<Pista>();
    public List<Escuderia> EscuderiasCargadas = new List<Escuderia>();
    public List<Radio> RadiosCargada = new List<Radio>();

    void Awake()
    {
        CargarPilotos();
        CargarPistas();
        CargarEscuderias();
        CargarRadios();
    }

    void CargarPilotos()
    {
        if (DB_Pilotos != null)
        {
            PilotoList lista = JsonUtility.FromJson<PilotoList>(DB_Pilotos.text);
            PilotosCargados = lista.pilotos;
        }
        else
        {
            Debug.LogError("DB_Pilotos no asignado.");
        }
    }

    void CargarPistas()
    {
        if (DB_Pistas != null)
        {
            PistaList lista = JsonUtility.FromJson<PistaList>(DB_Pistas.text);
            PistasCargadas = lista.pistas;
        }
        else
        {
            Debug.LogError("DB_Pistas no asignado.");
        }
    }


    void CargarEscuderias()
    {
        if (DB_Escuderias != null)
        {
            EscuderiaList lista = JsonUtility.FromJson<EscuderiaList>(DB_Escuderias.text);
            EscuderiasCargadas = lista.escuderia;
        }
        else
        {
            Debug.LogError("DB_Escuderias no asignado.");
        }
    }  void CargarRadios()
    {
        if (DB_Escuderias != null)
        {
            RadioList lista = JsonUtility.FromJson<RadioList>(DB_Radios.text);
           RadiosCargada = lista.radios;
        }
        else
        {
            Debug.LogError("DB_Escuderias no asignado.");
        }
    }
}
