using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pit_stop : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject panel_botones;

    [SerializeField] GameObject[] mechanic;
    [SerializeField] GameObject light;
    [SerializeField] AudioSource[] Pistolas_audio;
    [SerializeField] AudioSource car_audio;

    public Image car;
    public int chrono = 0;
    int wheel_chek;
    bool stop = false;
    // Start is called before the first frame update
    void Start()
    {

        light.GetComponent<Button>().interactable = false;
        for (int i = 0; i <= mechanic.Length - 1; i++)
        {
            mechanic[i].GetComponent<Button>().interactable = true;
            int j = i;
            mechanic[i].GetComponent<Button>().onClick.AddListener(() => acierto(j));

        }
        panel.SetActive(false);
        stop = true;
        chrono = 0;
        //time();
    }

    void acierto(int id)
    {
        print("cambio");
        Pistolas_audio[id].Play();
        mechanic[id].GetComponent<Button>().interactable = false;
        StartCoroutine(Change());
    }
    // Update is called once per frame
    void Update()
    {
        if (wheel_chek == 4)
        {
            wheel_chek = 0;
                light.GetComponent<Button>().interactable = true;
            for (int i = 0; i <= mechanic.Length - 1; i++)
            {

                //mechanic[i].SetActive(false);
            }

        }
        if (!stop)
        {
            chrono++;

        }
    }
    IEnumerator Change()
    {
        yield return new WaitForSeconds(2f);
        wheel_chek++;

    }

    public void time()
    {
        //car.rectTransform.anchoredPosition += new Vector2(0, 507.38f);
        panel_botones.SetActive(false);
        chrono = 0;
        panel.SetActive(true);
        light.GetComponent<Button>().interactable = false;
        stop = false;
        StartCoroutine(car_in());
        print(mechanic.Length);
        for (int i = 0; i <= mechanic.Length - 1; i++)
        {
            mechanic[i].GetComponent<Button>().interactable = true;

        }
    }


    public void exit()
    {

        StartCoroutine(car_ex());
    }


    IEnumerator car_in()
    {
        for (float D = -541.9f; D <= -34.52f; D += 5)
        {
            car.rectTransform.anchoredPosition = new Vector2(0, D);

            yield return new WaitForSeconds(0.000001f);
        }
        car_audio.Play();
    }


    IEnumerator car_ex()
    {
        car_audio.Stop();

        for (float D = -34.52f; D <= 513.61f; D += 5)
        {
            car.rectTransform.anchoredPosition = new Vector2(0, D);

            yield return new WaitForSeconds(0.000001f);
        }
        stop = true;
        gameObject.GetComponent<test_box>().box_time(chrono);
        car.rectTransform.anchoredPosition = new Vector2(0, -5555);

        panel.SetActive(false);
        panel_botones.SetActive(true);

    }
}
