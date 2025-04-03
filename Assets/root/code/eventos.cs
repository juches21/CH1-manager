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
        fallo_suspension++;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void test()
    {
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

    public void reparar_suspension()
    {
        partes[0].GetComponent<RawImage>().color = Color.green;
        fallo_suspension=0;
    }
}
