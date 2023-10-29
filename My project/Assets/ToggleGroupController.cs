using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGroupController : MonoBehaviour
{
    /* ToggleGroup toggleGroup;
     public Toggle curremtSelection
     {
         get { return toggleGroup.ActiveToggles().FirstOrDefault(); }
     }*/
    List<Toggle> toggles;
    // Start is called before the first frame update
    void Start()
    {
        //toggleGroup = GetComponent<ToggleGroup>();
        toggles = new List<Toggle>();
        for (int i = 0; i < transform.childCount; i++)
            toggles.Add(transform.GetChild(i).GetComponent<Toggle>());
        Click(0);
    }
    public void Click(int id)
    {
        for (int i = 0; i < transform.childCount; i++)
            toggles[i].isOn = false;
        toggles[id].isOn = true;
        /*var toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        toggles[id].isOn = true;*/
    }
}
