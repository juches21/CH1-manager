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

    // Start is called before the first frame update
    void Start()
    {
        falta = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void test()
    {
        if (!panelchec.activeSelf)
        {

            penalty.Add(new List<object> { falta, 1, "leve" });
            panelchec.SetActive(true);

            miBoton.onClick.AddListener(() => acierto(falta));
            falta++;

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


}
