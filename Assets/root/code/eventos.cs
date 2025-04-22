using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Necesario para trabajar con botones

public class eventos : MonoBehaviour
{
    int falta;
    public List<List<object>> penalty = new List<List<object>>();


    public GameObject panelchec;

    public Button miBoton;  // El botón en el Inspector

    [SerializeField] GameObject[] partes;
    [SerializeField] GameObject[] botones;
    int fallo_suspension;
    int fallo_refrigeracion;
    int fallo_power;
    // Start is called before the first frame update
    void Start()
    {
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


    public void test()
    {
        int numeroAleatorio = UnityEngine.Random.Range(0, 101);

        if (numeroAleatorio > 80)
        {
            int accidente = UnityEngine.Random.Range(0, 101);
            print("fallo");
            if (accidente < 33)
            {
            fgallo_suspension();

            }
            else if (accidente > 33 && accidente < 66)
            {
                fgallo_refrigeracion();
            }
            else
            {
            fgallo_power();

            }
        }

      
        /*
        if (!panelchec.activeSelf)
        {

            penalty.Add(new List<object> { falta, 1, "leve" });
            panelchec.SetActive(true);

            miBoton.onClick.AddListener(() => acierto(falta));
            falta++;

        }
        */

        if (fallo_suspension > 0)
        {
            partes[0].GetComponent<RawImage>().color = Color.yellow;

        }
        if (fallo_suspension > 2)
        {
            partes[0].GetComponent<RawImage>().color = Color.red;

        }



        if (fallo_refrigeracion > 0)
        {
            partes[1].GetComponent<RawImage>().color = Color.yellow;

        }
        if (fallo_refrigeracion > 1)
        {
            partes[1].GetComponent<RawImage>().color = Color.red;

        }


        if (fallo_power > 0)
        {
            partes[2].GetComponent<RawImage>().color = Color.yellow;

        }
        if (fallo_power > 1)
        {
            partes[2].GetComponent<RawImage>().color = Color.red;

        }
    }


    public void acierto(int id)
    {
        for (int i = 0; i < penalty.Count; i++)
        {
            if (Convert.ToInt32(penalty[i][0]) == id - 1)
            {
                penalty.RemoveAt(i);
                panelchec.SetActive(false);
                break;
            }
        }
    }


    public void fgallo_suspension()
    {

        fallo_suspension++;
        botones[0].GetComponent<Button>().interactable = true;

    }
    public void fgallo_refrigeracion()
    {

        fallo_refrigeracion++;
        botones[1].GetComponent<Button>().interactable = true;

    }
    public void fgallo_power()
    {

        fallo_power++;
        botones[2].GetComponent<Button>().interactable = true;

    }



    public void reparar_suspension()
    {
        partes[0].GetComponent<RawImage>().color = Color.green;
        fallo_suspension = 0;
        botones[0].GetComponent<Button>().interactable = false;

    }


    public void reparar_refrigeracion()
    {
        partes[1].GetComponent<RawImage>().color = Color.green;
        fallo_refrigeracion = 0;
        botones[1].GetComponent<Button>().interactable = false;

    }


    public void reparar_power()
    {
        partes[2].GetComponent<RawImage>().color = Color.green;
        fallo_power = 0;
        botones[2].GetComponent<Button>().interactable = false;

    }
}
