using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;  // Necesario para trabajar con botones

public class eventos : MonoBehaviour
{
    int falta;
    public List<List<object>> penalty = new List<List<object>>();

    public List<Data_base.Radio> listaRadios;

    public Image panelchec_color;

    public Button miBoton;  // El botón en el Inspector

    [SerializeField] GameObject[] partes;
    [SerializeField] GameObject[] botones;
    int fallo_suspension;
    int fallo_refrigeracion;
    int fallo_power;


    public GameObject prefab;         // Prefab que se usa para cada elemento de la tienda
    public Transform panel;

    int menor;
    int medio;
    int grande;


    public AudioSource audio_s;
    // Start is called before the first frame update
    void Start()
    {
       
        Data_base loader = FindObjectOfType<Data_base>();
        if (loader != null)
        {
            listaRadios = loader.RadiosCargada;  //datos

            // Debug.Log("Primera pista: " + copiaSegura[0].nombre);
        }
        else
        {
            Debug.LogError("No se encontró el script Data_base en la escena.");
        }
        falta = 0;
        partes[0].GetComponent<RawImage>().color = Color.green;
        partes[1].GetComponent<RawImage>().color = Color.green;
        partes[2].GetComponent<RawImage>().color = Color.green;

        botones[0].GetComponent<Button>().interactable = false;
        botones[1].GetComponent<Button>().interactable = false;
        botones[2].GetComponent<Button>().interactable = false;



    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool radio_on = false;
    public void maquina_de_radios()
    {
        if (!radio_on)
        {
            int numeroAleatorio = UnityEngine.Random.Range(0, 101);

            if (numeroAleatorio < 50)
            {
            audio_s.Play();
            radio_on = true;
                menor = gameObject.GetComponent<Player>().minor_fault;
                medio = gameObject.GetComponent<Player>().medium_fault;
                grande = gameObject.GetComponent<Player>().major_fault;


                int accidente = UnityEngine.Random.Range(0, 101);

                StartCoroutine(color());

                if (accidente < 33)
                {
                    Problemas_Radio();

                }
                else if (accidente > 33 && accidente < 80)
                {
                    Poner_Radio();
                }
                else
                {

                    Good_Radio();
                }
            }

        }

    }


    //aplica los daños al piloto
    public void aplicar()
    {
        gameObject.GetComponent<Player>().minor_fault = menor;
        gameObject.GetComponent<Player>().medium_fault = medio;
        gameObject.GetComponent<Player>().major_fault = grande;
    }



   

    //_funciones para dañar los diferentes conponentes

    public void fgallo_suspension()
    {

        fallo_suspension++;
        botones[0].GetComponent<Button>().interactable = true;
        if (fallo_suspension == 1)
        {
            menor++;
            partes[0].GetComponent<RawImage>().color = Color.yellow;

        }

        if (fallo_suspension == 3)
        {
            menor--;
            grande++;
            partes[0].GetComponent<RawImage>().color = Color.red;

        }

    }
    public void fgallo_refrigeracion()
    {

        fallo_refrigeracion++;
        botones[1].GetComponent<Button>().interactable = true;
        if (fallo_refrigeracion == 1)
        {
            menor++;
            partes[1].GetComponent<RawImage>().color = Color.yellow;

        }

        if (fallo_refrigeracion == 3)
        {
            menor--;
            grande++;
            partes[1].GetComponent<RawImage>().color = Color.red;

        }
    }
    public void fgallo_power()
    {

        fallo_power++;
        botones[2].GetComponent<Button>().interactable = true;
        if (fallo_power == 1)
        {
            menor++;
            partes[2].GetComponent<RawImage>().color = Color.yellow;

        }

        if (fallo_power == 3)
        {
            menor--;
            grande++;
            partes[2].GetComponent<RawImage>().color = Color.red;

        }

    }


    //_funciones para reparar los diferentes conponentes
    public void reparar_suspension()
    {
        partes[0].GetComponent<RawImage>().color = Color.green;

        if (fallo_suspension == 1)
        {
            menor--;

        }

        if (fallo_suspension == 3)
        {
            grande--;

        }
        fallo_suspension = 0;
        botones[0].GetComponent<Button>().interactable = false;
    }


    public void reparar_refrigeracion()
    {
        partes[1].GetComponent<RawImage>().color = Color.green;

        if (fallo_refrigeracion == 1)
        {
            menor--;

        }

        if (fallo_refrigeracion == 3)
        {
            grande--;

        }
        fallo_refrigeracion = 0;
        botones[1].GetComponent<Button>().interactable = false;
    }


    public void reparar_power()
    {
        partes[2].GetComponent<RawImage>().color = Color.green;

        if (fallo_power == 1)
        {
            menor--;

        }

        if (fallo_power == 3)
        {
            grande--;

        }
        fallo_power = 0;
        botones[2].GetComponent<Button>().interactable = false;
    }





    GameObject radioactual;


    //_radios que anuncian problemas con conponentes 
    public void Poner_Radio()
    {
        if (prefab != null && panel != null)
        {

            int random = UnityEngine.Random.Range(4, listaRadios.Count);
            // Instanciar el prefab y asignarlo al panel
            GameObject instancia = Instantiate(prefab, panel);
            radioactual = instancia;
            // Buscar los componentes de texto en el prefab y asignar los valores correspondientes
            TextMeshProUGUI[] textComponents = instancia.GetComponentsInChildren<TextMeshProUGUI>();
            Image[] escuderia = instancia.GetComponentsInChildren<Image>();
            Button[] Opciones = instancia.GetComponentsInChildren<Button>();

            textComponents[0].text = listaRadios[random].texto;
            textComponents[2].text = listaRadios[random].opcion_1;
            textComponents[1].text = listaRadios[random].opcion_2;
            Opciones[1].GetComponent<Button>().onClick.AddListener(() => neutra());
            Opciones[0].GetComponent<Button>().onClick.AddListener(() => fallo());

            print("fallo   " + listaRadios[random].texto);
        }

    }

    //_radios que plantean dilemas de respuesta
    public void Problemas_Radio()
    {
        if (prefab != null && panel != null)
        {

            int random = UnityEngine.Random.Range(0, 3);
            // Instanciar el prefab y asignarlo al panel
            GameObject instancia = Instantiate(prefab, panel);
            radioactual = instancia;
            // Buscar los componentes de texto en el prefab y asignar los valores correspondientes
            TextMeshProUGUI[] textComponents = instancia.GetComponentsInChildren<TextMeshProUGUI>();
            Image[] escuderia = instancia.GetComponentsInChildren<Image>();
            Button[] Opciones = instancia.GetComponentsInChildren<Button>();

            textComponents[0].text = listaRadios[random].texto;
            textComponents[2].text = listaRadios[random].opcion_1;
            textComponents[1].text = listaRadios[random].opcion_2;
            if (random == 0)
            {
                fgallo_suspension();
            }
            if (random == 1)
            {
                fgallo_refrigeracion();
            }
            if (random == 2)
            {
                fgallo_power();
            }
            Opciones[1].GetComponent<Button>().onClick.AddListener(() => neutra());
            Opciones[0].GetComponent<Button>().onClick.AddListener(() => fallo());


        }

    }

    //_radios que linpian la lista de fallos
    public void Good_Radio()
    {
        if (prefab != null && panel != null)
        {


            // Instanciar el prefab y asignarlo al panel
            GameObject instancia = Instantiate(prefab, panel);
            radioactual = instancia;
            // Buscar los componentes de texto en el prefab y asignar los valores correspondientes
            TextMeshProUGUI[] textComponents = instancia.GetComponentsInChildren<TextMeshProUGUI>();
            Image[] escuderia = instancia.GetComponentsInChildren<Image>();
            Button[] Opciones = instancia.GetComponentsInChildren<Button>();

            textComponents[0].text = listaRadios[3].texto;
            textComponents[2].text = listaRadios[3].opcion_1;
            textComponents[1].text = listaRadios[3].opcion_2;
            Opciones[1].GetComponent<Button>().onClick.AddListener(() => correcta());
            Opciones[0].GetComponent<Button>().onClick.AddListener(() => neutra());


        }

    }

    //_tipos de respuestas
    public void correcta()
    {
        menor = 0;
        medio = 0;

        aplicar();
        radio_on = false;

        Destroy(radioactual);
    }

    public void neutra()
    {
        radio_on = false;

        Destroy(radioactual);
    }

    public void fallo()
    {
        medio++;
        aplicar();
        radio_on = false;

        Destroy(radioactual);
    }



    //notificacion visual de mensaje por radio
    IEnumerator color()
    {
        Color color_original = panelchec_color.color;
        Vector3 coloringo = new Vector3(1f, 1f, 0f); 

        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.1f); 

            panelchec_color.color = new Color(coloringo.x, coloringo.y, coloringo.z, 1f);
            yield return new WaitForSeconds(0.1f); 
            panelchec_color.color = color_original;

        }

    }
}
