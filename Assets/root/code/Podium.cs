using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Podium : MonoBehaviour
{
    GameObject manager;

    public List<Data_base.Escuderia> listaEscuderia;
    Manager scriptlap;
    [SerializeField] Image[] imagenes_cascos;
    [SerializeField] Image[] imagenes_escuderia;
    [SerializeField] TextMeshProUGUI[] nombre_pilotos;
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

    // Update is called once per frame
    void Update()
    {

    }

    public void mostrardatos()
    {
        int count = 0;
        foreach (var piloto in scriptlap.backupPilotsList)
        {
            if (count >= 3) break;
            for (int D = 0; D < listaEscuderia.Count; D++)
            {
                nombre_pilotos[count].text = piloto.nombre;


                string imagePath_casco = "Fotos/Cascos/" + piloto.casco;
                Sprite sprite_casco = Resources.Load<Sprite>(imagePath_casco);

                imagenes_cascos[count].sprite = sprite_casco;


                if (listaEscuderia[D].id == piloto.escuderia)
                {
                    // Tu lógica aquí


                    string imagePath_escuderia = "Fotos/Logos/" + listaEscuderia[D].imagen;
                    Sprite sprite_escuderia = Resources.Load<Sprite>(imagePath_escuderia);

                    imagenes_escuderia[count].sprite = sprite_escuderia;
                }
            }
            count++;
        }
        StartCoroutine(volver());

    }

    IEnumerator volver()
    {
        yield return new WaitForSeconds(5f); // Espera 2 segundos

        SceneManager.LoadScene(0);
    }
}
