using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] int levelMap = 0;

    [Header("\n------------Pause-------------")]
    [SerializeField] GameObject menuPause;

    [Header("\n---------Doi tuong khac-------")]
    [SerializeField] GameObject[] listDoiTuong;
    [SerializeField] float maxDistance;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 50;
        player = FindObjectOfType<PlayerController>().gameObject;
        //Debug.Log(listDoiTuong.Length);
        //menuPause.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i< listDoiTuong.Length; i++)
        {
            //Debug.Log(Vector2.Distance(player.transform.position, listDoiTuong[i].transform.position));
            if (Vector2.Distance(player.transform.position, listDoiTuong[i].transform.position) > maxDistance)
                listDoiTuong[i].SetActive(false);
            else
                listDoiTuong[i].SetActive(true);
        }
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
