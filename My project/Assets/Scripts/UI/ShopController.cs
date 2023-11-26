using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [SerializeField] GameObject[] goShop;
    [SerializeField] Slider sldNumberItem;
    [SerializeField] TextMeshProUGUI txtNumberItem;


    public void Select(int type)
    {
        for(int i = 0; i < goShop.Length; i++)
        {
            goShop[i].SetActive(false);
        }
        goShop[type].SetActive(true);
    }
    public void SliderNumber()
    {
        txtNumberItem.text = sldNumberItem.value + "";
    }
}
