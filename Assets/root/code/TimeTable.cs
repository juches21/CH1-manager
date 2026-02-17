using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TimeTable : MonoBehaviour
{
    // Referencia al objeto Manager
    GameObject manager;

    // Prefab que se instanciará para cada elemento del panel
    public GameObject prefab;

    // Panel donde se mostrarán los elementos
    public Transform panel;

    // Referencia al script Manager
    Manager scriptlap;

    // Variables para almacenar tiempos previos y del líder
    int tiempo_anterior = 0;
    int tiempo_lider = 0;

    // Variable para controlar el modo de visualización
    public int tipo = 0;

    // Lista de logos de las escuderías
    public List<Sprite> logos = new List<Sprite>();

    // Lista de escuderías cargadas desde la base de datos
    public List<Data_base.Escuderia> listaEscuderia;

    void Start()
    {
        // Asigna el objeto con la etiqueta "manager"
        manager = GameObject.FindGameObjectWithTag("manager");

        // Obtiene el componente Manager del objeto encontrado
        scriptlap = manager.GetComponent<Manager>();

        // Carga las escuderías desde la base de datos
        Data_base loader = FindObjectOfType<Data_base>();
        if (loader != null)
        {
            listaEscuderia = loader.EscuderiasCargadas;
        }
        else
        {
            Debug.LogError("No se encontró Data_base ");
        }
    }

    // Índice de posición en la tabla
    int posicion;

    public void UpdateTimeTable()
    {
        // Limpia el panel antes de actualizarlo
        ClearPanel();

        // Inicializa variables de posición y tiempos
        posicion = 0;
        int offsetY = 250;
        tiempo_anterior = 0;
        tiempo_lider = 0;

        // Verifica que prefab y panel estén asignados
        if (prefab != null && panel != null)
        {
            foreach (var piloto in scriptlap.backupPilotsList)
            {
                posicion++;

                // Instancia un nuevo elemento del prefab
                GameObject instancia = Instantiate(prefab, panel);

                // Obtiene los componentes de texto y las imágenes del prefab
                TextMeshProUGUI[] textComponents = instancia.GetComponentsInChildren<TextMeshProUGUI>();
                Image[] escuderia = instancia.GetComponentsInChildren<Image>();

                // Asigna el logo y color del equipo correspondiente
                for (int D = 0; D < listaEscuderia.Count; D++)
                {
                    if (listaEscuderia[D].id == piloto.escuderia)
                    {
                        string imagePath = "Fotos/Logos/" + listaEscuderia[D].imagen;
                        Sprite sprite = Resources.Load<Sprite>(imagePath);
                        escuderia[3].sprite = sprite;

                        // Convierte el color RGB a un formato usable por Unity
                        string[] RGB = listaEscuderia[D].color.Split(',');
                        float r = int.Parse(RGB[0]) / 255f;
                        float g = int.Parse(RGB[1]) / 255f;
                        float b = int.Parse(RGB[2]) / 255f;
                        escuderia[2].color = new Color(r, g, b);
                    }
                }

                // Asigna los valores de posición, nombre y dorsal
                textComponents[0].text = posicion.ToString();
                textComponents[1].text = piloto.nombre;
                textComponents[2].text = piloto.numero.ToString();

                // Formatea el tiempo de vuelta
                textComponents[3].text = FormatTime(Convert.ToInt32(piloto.tiempo_lap));

                // Modo 0: Diferencia con el líder
                if (tipo == 0)
                {
                    if (tiempo_lider > 0)
                    {
                        int diferencia = Mathf.Abs(Convert.ToInt32(piloto.tiempo_total) - tiempo_lider);
                        textComponents[4].text = FormatTime(diferencia);
                    }
                    else
                    {
                        textComponents[4].text = "Leader";
                        tiempo_lider = Convert.ToInt32(piloto.tiempo_total);
                    }
                }

                // Modo 1: Diferencia con el rival anterior
                if (tipo == 1)
                {
                    if (tiempo_anterior > 0)
                    {
                        int diferencia = Mathf.Abs(Convert.ToInt32(piloto.tiempo_total) - tiempo_anterior);
                        textComponents[4].text = FormatTime(diferencia);
                    }
                    else
                    {
                        textComponents[4].text = "Delta";
                    }
                    tiempo_anterior = Convert.ToInt32(piloto.tiempo_total);
                }

                // Modo 2: Desgaste del neumático
                if (tipo == 2)
                {
                    textComponents[4].text = piloto.desgaste.ToString()+"%";
                }

                // Modo 3: Tipo de compuesto del neumático
                if (tipo == 3)
                {
                    textComponents[4].text = piloto.compuesto;
                }

                // Ajusta la posición vertical del elemento
                instancia.transform.localPosition = new Vector3(0, offsetY, 0);
                offsetY -= 50;
            }
        }
        else
        {
            Debug.LogError("El prefab o el panel no están asignados.");
        }
    }

    // Elimina los elementos hijos del panel
    public void ClearPanel()
    {
        Transform[] hijos = panel.GetComponentsInChildren<Transform>();
        foreach (Transform hijo in hijos)
        {
            if (hijo.CompareTag("panel"))
            {
                Destroy(hijo.gameObject);
            }
        }
    }

    // Formatea los tiempos en mm:ss:fff
    string FormatTime(int numeros)
    {
        int minutos = numeros / 60000;
        int segundos = (numeros % 60000) / 1000;
        int milisegundos = numeros % 1000;
        return string.Format("{0:00}:{1:00}:{2:000}", minutos, segundos, milisegundos);
    }

    // Cambia al siguiente modo de visualización
    public void NextMode()
    {
        tipo = (tipo + 1) % 4;
        UpdateTimeTable();
    }

    // Cambia al modo anterior de visualización
    public void PreviousMode()
    {
        tipo = (tipo - 1 + 4) % 4;
        UpdateTimeTable();
    }
}
