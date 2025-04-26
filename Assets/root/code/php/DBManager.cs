using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DBManager : MonoBehaviour
{
    public List<Piloto> pilotos = new List<Piloto>();

    void Start()
    {
        StartCoroutine(GetPilotoData());
    }

    IEnumerator GetPilotoData()
    {
        string url = "http://localhost:50/ch1/get_player_data.php"; // Ajusta a tu ruta

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError ||
            www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            Piloto[] arrayPilotos = JsonHelper.FromJson<Piloto>(json);
            pilotos = new List<Piloto>(arrayPilotos);

            foreach (Piloto p in pilotos)
            {
                //Debug.Log($"Piloto {p.nombre}, Casco: {p.Casco}, Escudería: {p.id_escuderia}");
            }
        }
    }
}

[System.Serializable]
    public class Piloto
    {
        public string id_piloto;
        public string nombre;
        public string id_escuderia;
        public string id_neumatico;
        public string Casco;
        public string Tiempo_total;
        public string Tiempo_lap;
        public string Estilo;
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }

