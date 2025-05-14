using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Eleccion : MonoBehaviour
{
    public Manager Manager;
    int id;
    public TextMeshProUGUI T_Nombre;
    public Image Helmet;
    
    // Start is called before the first frame update
     void Start()
    {
        id=0;

    }

    // Update is called once per frame
    void Update()
    {
        

    }
    public void up()
    {
        if(id>= Manager.pilotsList.Count-1)
        {
            id = 0;
        }
        else
        {

        id++;
        }
        cargar_piloto();
    }

    public void down()
    {
        if (id <= 0)
        {
            id = 11;
        }
        else
        {

            id--;
        }
        cargar_piloto();
    }
    void cargar_piloto()
    {
        T_Nombre.text = Manager.pilotsList[id].nombre.ToString();


        string imagePath = "Fotos/Cascos/" + Manager.pilotsList[id].casco;

        // Cargar el sprite desde Resources usando la ruta proporcionada
        Sprite sprite = Resources.Load<Sprite>(imagePath);
       

        // Comprobar si la imagen fue cargada correctamente
        if (sprite != null)
        {
            Helmet.sprite = sprite;  // Asignar el sprite al objeto de imagen
        }
        else
        {
            // Imprimir más información de depuración para verificar la ruta
            print("Ruta de la imagen no válida o archivo no encontrado: " + imagePath);
        }
    }

    public void acertar()
    {
        gameObject.SetActive(false);
        Manager.id_piloto = id;
        Manager.iniciarjuego();
    }
}
