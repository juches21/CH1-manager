using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_base : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject manager;
    laps scriptlap;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager");
        scriptlap = manager.GetComponent<laps>();

        PilotoList wrapper = JsonUtility.FromJson<PilotoList>(archivoJSON.text);
        //scriptlap.listaPilotos = wrapper.pilotos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Base de datos
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

    public TextAsset archivoJSON; // arrastra aquí tu .json en el Inspector
    public class PilotoList
    {
        public List<Piloto> pilotos;
    }

}
