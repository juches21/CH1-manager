using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class positions : MonoBehaviour
{
    GameObject manager;
    public GameObject prefab;         // Prefab que se usa para cada elemento de la tienda
    public Transform panel;
    laps scriptlap;
    int tiempoEnMilisegundos;
    int tiempo_anterior = 0;
    int tiempo_lider = 0;

    public int tipo = 0;

    public List<Sprite> logos = new List<Sprite>();

    public List<Data_base.Escuderia> listaEscuderia;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager");
        scriptlap = manager.GetComponent<laps>();

        Data_base loader = FindObjectOfType<Data_base>();
        if (loader != null)
        {
            listaEscuderia = loader.EscuderiasCargadas;

        }
        else
        {
            Debug.LogError("No se encontró el script Data_base en la escena.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    int posicion;
    public void AñadirPrefabAlPanel()
    {
        LimpiarPanel();
        posicion = 0;
        int offsetY = 300; // Separación inicial entre elementos
        tiempo_anterior = 0;
        tiempo_lider=0;
        // Validar que los elementos necesarios están asignados
        if (prefab != null && panel != null)
        {
            foreach (var piloto in scriptlap.copiaSegura)
            {
                posicion++;
                // Instanciar el prefab y asignarlo al panel
                GameObject instancia = Instantiate(prefab, panel);

                // Buscar los componentes de texto en el prefab y asignar los valores correspondientes
                TextMeshProUGUI[] textComponents = instancia.GetComponentsInChildren<TextMeshProUGUI>();
                Image[] escuderia = instancia.GetComponentsInChildren<Image>();
               


                for (int D = 0; D < listaEscuderia.Count; D++)
                {
                    if (listaEscuderia[D].id == piloto.escuderia)
                    {
                       
                        string imagePath = "Fotos/Logos/" + listaEscuderia[D].imagen;
                        Sprite sprite = Resources.Load<Sprite>(imagePath);

                        escuderia[3].sprite = sprite;

                        string[] RGB = listaEscuderia[D].color.Split(',');
                        float r = int.Parse(RGB[0]) / 255f;
                        float g = int.Parse(RGB[1]) / 255f;
                        float b = int.Parse(RGB[2]) / 255f;
           
                        escuderia[2].color = new Color(r, g, b); ;
                    }
                }

                textComponents[0].text = posicion + "";
                textComponents[1].text = piloto.nombre + "";
                textComponents[2].text = Convert.ToInt32(piloto.numero) + "";
                //textComponents[3].text = Convert.ToInt32(piloto.tiempo_lap) + "";


                int tiempoactualEnMilisegundos = Convert.ToInt32(piloto.tiempo_lap);

                // Calcular minutos, segundos y milisegundos
                int minutos = tiempoactualEnMilisegundos / 60000;
                int segundos = (tiempoactualEnMilisegundos % 60000) / 1000;
                int milisegundos = tiempoactualEnMilisegundos % 1000;

                // Formatear el texto en mm:ss:fff
                textComponents[3].text = string.Format("{0:00}:{1:00}:{2:000}", minutos, segundos, milisegundos);




                //modos tabla

                //respecto lider
                if (tipo == 0)
                {

                    if (tiempo_lider > 0)
                    {
                        int diferencia = tiempo_lider - Convert.ToInt32(piloto.tiempo_lap);
                        diferencia = Mathf.Abs(diferencia);
                        if (diferencia == 0)
                        {
                            diferencia = 1;
                        }

                        // Calcular minutos, segundos y milisegundos
                        int minutos_d = diferencia / 60000;
                        int segundos_d = (diferencia % 60000) / 1000;
                        int milisegundos_d = diferencia % 1000;

                        // Formatear el texto en mm:ss:fff
                        textComponents[4].text = string.Format("{0:00}:{1:00}:{2:000}", minutos_d, segundos_d, milisegundos_d);
                    }
                    else
                    {
                        textComponents[4].text = "Leader";
                        tiempo_lider = Convert.ToInt32(piloto.tiempo_lap);
                    }

                }

                //respecto ribal
                if (tipo == 1)
                {

                    if (tiempo_anterior > 0)
                    {

                        int diferencia = Convert.ToInt32(piloto.tiempo_total)-tiempo_anterior ;
                        diferencia = Mathf.Abs(diferencia);
                        if (diferencia == 0)
                        {
                            diferencia = 1;
                        }

                        // Calcular minutos, segundos y milisegundos
                        int minutos_d = diferencia / 60000;
                        int segundos_d = (diferencia % 60000) / 1000;
                        int milisegundos_d = diferencia % 1000;

                        // Formatear el texto en mm:ss:fff
                        print(string.Format("{0:00}:{1:00}:{2:000}", minutos_d, segundos_d, milisegundos_d));
                        textComponents[4].text = string.Format("{0:00}:{1:00}:{2:000}", minutos_d, segundos_d, milisegundos_d);
                        tiempo_anterior = Convert.ToInt32(piloto.tiempo_lap);

                    }
                    else
                    {
                        tiempo_anterior = Convert.ToInt32(piloto.tiempo_lap);
                        textComponents[4].text = "Delta";
                    }
                }
                if (tipo == 2)
                {
                    textComponents[4].text = Convert.ToInt32(piloto.desgaste) + "";

                }
                if (tipo == 3)
                {
                    textComponents[4].text = piloto.compuesto + "";

                }
                tiempo_anterior = Convert.ToInt32(piloto.tiempo_total);

                /*

                //asignacion logo escuderia
                /*
                if (Convert.ToInt32(piloto.escuderia) == 1)
                {

                    escuderia[1].sprite = logos[0];
                }
                if (Convert.ToInt32(piloto.escuderia) == 2)
                {

                    escuderia[1].sprite = logos[1];

                }
                if (Convert.ToInt32(piloto.escuderia) == 3)
                {

                    escuderia[1].sprite = logos[2];

                }
                if (Convert.ToInt32(piloto.escuderia) == 4)
                {

                    escuderia[1].sprite = logos[3];

                }
                if (Convert.ToInt32(piloto.escuderia) == 5)
                {

                    escuderia[1].sprite = logos[4];

                }
                if (Convert.ToInt32(piloto.escuderia) == 6)
                {

                    escuderia[1].sprite = logos[5];

                }
              //

                //diferentes paneles de infoirmacion adicional
                if (tipo == 0)
                {

                    if (tiempo_lider > 0)
                    {
                        print(tiempo_anterior);
                        int diferencia = tiempo_lider - Convert.ToInt32(piloto.tiempo_lap);
                        diferencia = Mathf.Abs(diferencia);
                        if (diferencia == 0)
                        {
                            diferencia = 1;
                        }

                        // Calcular minutos, segundos y milisegundos
                        int minutos_d = diferencia / 60000;
                        int segundos_d = (diferencia % 60000) / 1000;
                        int milisegundos_d = diferencia % 1000;

                        // Formatear el texto en mm:ss:fff
                        textComponents[3].text = string.Format("{0:00}:{1:00}:{2:000}", minutos_d, segundos_d, milisegundos_d);
                    }
                    else
                    {
                        textComponents[3].text = "leader";
                        // tiempo_lider = Convert.ToInt32(piloto.tiempo_lap);
                        tiempo_lider = 1;
                    }

                }
                if (tipo == 1)
                {
                       
                    if (tiempo_anterior > 0)
                    {

                        int diferencia = tiempo_lider - Convert.ToInt32(piloto.tiempo_lap);
                        diferencia = Mathf.Abs(diferencia);
                        if (diferencia == 0)
                        {
                            diferencia = 1;
                        }

                        // Calcular minutos, segundos y milisegundos
                        int minutos_d = diferencia / 60000;
                        int segundos_d = (diferencia % 60000) / 1000;
                        int milisegundos_d = diferencia % 1000;

                        // Formatear el texto en mm:ss:fff
                        textComponents[3].text = string.Format("{0:00}:{1:00}:{2:000}", minutos_d, segundos_d, milisegundos_d);
                    }
                    else
                    {
                        tiempo_lider = Convert.ToInt32(piloto.tiempo_lap);
                        textComponents[3].text = "leader";
                    }
                }
                if (tipo == 2)
                {
                    textComponents[3].text = Convert.ToInt32(piloto.desgaste) + "";

                }
                if (tipo == 3)
                {
                    textComponents[3].text = Convert.ToInt32(piloto.modo) + "";

                }



                //datos pilotos

                textComponents[0].text = piloto.nombre.ToString(); // Nombre del piloto

                tiempoEnMilisegundos = Convert.ToInt32(piloto.tiempo_total);

                tiempo_anterior = Convert.ToInt32(piloto.tiempo_total);

                // Calcular minutos, segundos y milisegundos
                int minutos = tiempoEnMilisegundos / 60000;
                int segundos = (tiempoEnMilisegundos % 60000) / 1000;
                int milisegundos = tiempoEnMilisegundos % 1000;
                // Formatear el texto en mm:ss:fff
                textComponents[1].text = string.Format("{0:00}:{1:00}:{2:000}", minutos, segundos, milisegundos);


                int tiempoactualEnMilisegundos = Convert.ToInt32(piloto.tiempo_lap);

                // Calcular minutos, segundos y milisegundos
                minutos = tiempoactualEnMilisegundos / 60000;
                segundos = (tiempoactualEnMilisegundos % 60000) / 1000;
                milisegundos = tiempoactualEnMilisegundos % 1000;

                // Formatear el texto en mm:ss:fff
                textComponents[2].text = string.Format("{0:00}:{1:00}:{2:000}", minutos, segundos, milisegundos);
                //textComponents[2].text = piloto[2].ToString(); // Nombre de la mejora
                */
                // Ajustar la posición del prefab en el panel
                instancia.transform.localPosition = new Vector3(0, offsetY, 0);
                offsetY -= 50; // Reducir la separación para la siguiente mejora
            }
        }
        else
        {
            Debug.LogError("El prefab o el panel no están asignados.");
        }
    }


    public void LimpiarPanel()
    {
        // Obtener todos los hijos del panel
        Transform[] hijos = panel.GetComponentsInChildren<Transform>();

        foreach (Transform hijo in hijos)
        {
            // No eliminar el propio panel ni elementos específicos como "Exit"
            if (hijo.CompareTag("panel"))
            {
                Destroy(hijo.gameObject); // Eliminar el objeto hijo
            }
        }
    }




    //modos panel

    public void diferencia_L()
    {
        tipo = 0;
        AñadirPrefabAlPanel();
    }
    public void diferencia()
    {
        tipo = 1;
        AñadirPrefabAlPanel();

    }

    public void aptitud()
    {
        tipo = 3;
        AñadirPrefabAlPanel();

    }

    public void rueda()
    {
        tipo = 2;
        AñadirPrefabAlPanel();

    }
}
