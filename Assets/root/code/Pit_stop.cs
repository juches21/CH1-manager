using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pit_stop : MonoBehaviour
{
    [SerializeField] GameObject[] mechanic;
    [SerializeField] GameObject light;

    int wheel_chek;
    // Start is called before the first frame update
    void Start()
    {

        light.GetComponent<Button>().interactable = false;
        for (int i = 0; i <= mechanic.Length; i++)
        {
           // mechanic[i].GetComponent<Button>().interactable = false;


        }
        time();
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
    }
    IEnumerator Change()
    {
        yield return new WaitForSeconds(2f);
        wheel_chek++;

    }

    void time()
    {

        for (int i = 0; i <= mechanic.Length; i++)
        {
            mechanic[i].GetComponent<Button>().interactable = true;
            int j = i;
            mechanic[i].GetComponent<Button>().onClick.AddListener(() => acierto(j));

        }
    }

}
