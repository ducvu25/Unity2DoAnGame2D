using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] int levelMap = 0;

    [Header("\n------------Pause-------------")]
    [SerializeField] GameObject menuPause;
    // Start is called before the first frame update
    void Start()
    {
        menuPause.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PauseGame()
    {
        menuPause.SetActive(!menuPause.activeSelf);
        if (menuPause.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void SettingGame()
    {
        Time.timeScale = 1;
    }
    public void TuyChon()
    {
        Time.timeScale = 1;
        SceneController.instance.LoadScene(SceneIndex.SHOP);
    }

    public void SelectMap()
    {
        Time.timeScale = 1;
        SceneController.instance.LoadScene(SceneIndex.SELECT_MAP);
    }
}
