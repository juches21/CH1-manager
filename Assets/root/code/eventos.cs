using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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


    int menor;
    int medio;
    int grande;
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
            menor = gameObject.GetComponent<test_box>().minor_fault;
            medio = gameObject.GetComponent<test_box>().medium_fault;
            grande = gameObject.GetComponent<test_box>().major_fault;


            int accidente = UnityEngine.Random.Range(0, 101);
          
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

     



        gameObject.GetComponent<test_box>().minor_fault=menor;
        gameObject.GetComponent<test_box>().medium_fault=medio;
        gameObject.GetComponent<test_box>().major_fault=grande;
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
}
