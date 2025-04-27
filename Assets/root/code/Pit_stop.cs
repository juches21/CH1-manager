using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pit_stop : MonoBehaviour
{
    [SerializeField] GameObject[] mechanic;
    [SerializeField] GameObject light;

    public Image car;
    public int chrono = 0;
    int wheel_chek;
    bool stop = false;
    // Start is called before the first frame update
    void Start()
    {

        light.GetComponent<Button>().interactable = false;
        for (int i = 0; i <= mechanic.Length; i++)
        {
            //mechanic[i].GetComponent<Button>().interactable = false;


        }
        //time();
    }

    void acierto(int id)
    {
        print("cambio");
        mechanic[id].GetComponent<Button>().interactable = false;
        StartCoroutine(Change());
    }
    // Update is called once per frame
    void Update()
    {
        if (wheel_chek == 4)
        {
            wheel_chek = 0;
            for (int i = 0; i < mechanic.Length; i++)
            {
                light.GetComponent<Button>().interactable = true;

                mechanic[i].SetActive(false);
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
        StartCoroutine(car_in());
        for (int i = 0; i <= mechanic.Length; i++)
        {
            mechanic[i].GetComponent<Button>().interactable = true;
            int j = i;
            mechanic[i].GetComponent<Button>().onClick.AddListener(() => acierto(j));

        }
    }


    public void exit()
    {
        
        StartCoroutine(car_ex());
    }


    IEnumerator car_in()
    {
        for(float D = -541.9f ; D <= -34.52f; D+=5)
        {
            car.rectTransform.anchoredPosition = new Vector2(0, D);

            yield return new WaitForSeconds(0.000001f);
        }

    }


    IEnumerator car_ex()
    {
        for (float D = -34.52f; D <= 513.61f; D += 5)
        {
            car.rectTransform.anchoredPosition = new Vector2(0, D);

            yield return new WaitForSeconds(0.000001f);
        }
        stop = true;
        gameObject.GetComponent<test_box>().box_time(chrono) ;
        car.rectTransform.anchoredPosition = new Vector2(0, -5555);
    }
}
