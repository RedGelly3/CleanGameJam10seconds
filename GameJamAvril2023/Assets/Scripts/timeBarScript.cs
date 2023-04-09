using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    public GameObject player;
    private double time;
    private Image timebarImage;

    void Start()
    {
        timebarImage = GetComponent<Image>();
        time = 10f;
        timebarImage.fillAmount = 1f;
    }

    void Update()
    {
        time = player.GetComponent<PlayerScript>().time;
        float amount = (float)time / 10f;
        timebarImage.fillAmount = amount;
    }
  
}
