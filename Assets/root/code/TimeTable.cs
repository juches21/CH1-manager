using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TimeTable : MonoBehaviour
{
    GameObject manager;
    public GameObject prefab;         // Prefab que se usa para cada elemento de la tienda
    public Transform panel;
    Manager scriptlap;
    int tiempo_anterior = 0;
    int tiempo_lider = 0;

    public int tipo = 0;

    public List<Sprite> logos = new List<Sprite>();

    public List<Data_base.Escuderia> listaEscuderia;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager");
        scriptlap = manager.GetComponent<Manager>();

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

   
    int posicion;
    public void UpdateTimeTable()
    {
        ClearPanel();
        posicion = 0;
        int offsetY = 250; // Separación inicial entre elementos
        tiempo_anterior = 0;
        tiempo_lider=0;
        // Validar que los elementos necesarios están asignados
        if (prefab != null && panel != null)
        {
            foreach (var piloto in scriptlap.backupPilotsList)
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

              

                // Formatear el texto en mm:ss:fff
                textComponents[3].text = FormatTime(Convert.ToInt32(piloto.tiempo_lap));




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

                        // Formatear el texto en mm:ss:fff
                        textComponents[4].text = FormatTime(diferencia);
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
                        textComponents[4].text = FormatTime(diferencia);
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



                // Ajustar la posición del prefab en el panel


                instancia.transform.localPosition = new Vector3(0, offsetY, 0);
                offsetY -= 50; // Reducir la separación para la siguiente piloto
            }
        }
        else
        {
            Debug.LogError("El prefab o el panel no están asignados.");
        }
    }


    public void ClearPanel()
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


    string FormatTime(int numeros)
    {
        int minutos_d = numeros / 60000;
        int segundos_d = (numeros % 60000) / 1000;
        int milisegundos_d = numeros % 1000;

        // Formatear el texto en mm:ss:fff
        return string.Format("{0:00}:{1:00}:{2:000}", minutos_d, segundos_d, milisegundos_d);
         
    }

    //modos panel


    public void NextMode()
    {
        if (tipo < 3)
        {

        tipo ++;
        }
        else
        {
            tipo = 0;
        }
        UpdateTimeTable();

    }

    public void PreviousMode()
    {
        if (tipo > 0)
        {

            tipo--;
        }
        else
        {
            tipo = 3;
        }
        UpdateTimeTable();

    }

   
}
