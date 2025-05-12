using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public AudioSource sonido_radio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play_b()
    {
        StartCoroutine(Accion_play());
    }
    public void Exit_b()
    {
        Application.Quit();
    }
    IEnumerator Accion_play() {
        sonido_radio.Play();
        yield return new WaitForSeconds(2f); // Espera 2 segundos
        SceneManager.LoadScene(1);

    }
}
