using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PitStop : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject panel_botones;

    [SerializeField] GameObject[] mechanic;
    [SerializeField] GameObject light;
    [SerializeField] AudioSource[] Pistolas_audio;
    [SerializeField] AudioSource car_audio;






    public List<Data_base.Escuderia> listaEscuderia;

    public Image car;
    public int pitStopTime = 0;
    int changedWheelsCount;
    bool stop = false;

    // Start is called before the first frame update
    void Start()
    {


        light.GetComponent<Button>().interactable = false;
        for (int i = 0; i <= mechanic.Length - 1; i++)
        {
            mechanic[i].GetComponent<Button>().interactable = true;
            int j = i;
            mechanic[i].GetComponent<Button>().onClick.AddListener(() => OnWheelChanged(j));

        }
        panel.SetActive(false);
        stop = true;
        pitStopTime = 0;

    }

    void Update()
    {
        if (changedWheelsCount == 4)
        {
            changedWheelsCount = 0;
            light.GetComponent<Button>().interactable = true;
            for (int i = 0; i <= mechanic.Length - 1; i++)
            {

            }

        }
        if (!stop)
        {
            pitStopTime++;

        }
    }
    void OnWheelChanged(int id)
    {
       
        Pistolas_audio[id].Play();
        mechanic[id].GetComponent<Button>().interactable = false;
        StartCoroutine(WaitForWheelChange());
    }
    // Update is called once per frame
    IEnumerator WaitForWheelChange()
    {
        yield return new WaitForSeconds(2f);
        changedWheelsCount++;

    }

    public void StartPitStop()
    {

        panel_botones.SetActive(false);
        pitStopTime = 0;
        panel.SetActive(true);
        light.GetComponent<Button>().interactable = false;
        stop = false;
        StartCoroutine(MoveCarIn());
        print(mechanic.Length);
        for (int i = 0; i <= mechanic.Length - 1; i++)
        {
            mechanic[i].GetComponent<Button>().interactable = true;

        }
    }


    public void EndPitStop()
    {

        StartCoroutine(MoveCarOut());
    }


    IEnumerator MoveCarIn()
    {
        for (float D = -541.9f; D <= -34.52f; D += 5)
        {
            car.rectTransform.anchoredPosition = new Vector2(0, D);

            yield return  null;
        }
        car_audio.Play();
    }


    IEnumerator MoveCarOut()
    {
        car_audio.Stop();

        for (float D = -34.52f; D <= 513.61f; D += 5)
        {
            car.rectTransform.anchoredPosition = new Vector2(0, D);

            yield return null;
        }
        stop = true;
        gameObject.GetComponent<Player>().ApplyPitStopTime(pitStopTime);
        car.rectTransform.anchoredPosition = new Vector2(0, -5555);

        panel.SetActive(false);
        panel_botones.SetActive(true);

    }
}
