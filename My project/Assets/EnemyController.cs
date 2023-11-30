using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float maxDistance;
    [SerializeField] float timeSpawnE = 20f;
    List<float> timeSpawn;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        timeSpawn = new List<float>();
        for (int i = 0; i < transform.childCount; i++)
            timeSpawn.Add(timeSpawnE);
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            //Debug.Log(Vector2.Distance(player.transform.position, listDoiTuong[i].transform.position));
            if (Vector2.Distance(player.transform.position, transform.GetChild(i).transform.position) > maxDistance)
                transform.GetChild(i).gameObject.SetActive(false);
            else
                transform.GetChild(i).gameObject.SetActive(true);
            if(transform.GetChild(i).GetChild(0) != null && transform.GetChild(i).GetChild(0).GetComponent<EnemyInformation>().hp <= 0)
            {
                timeSpawn[i] -= Time.deltaTime;
                if (timeSpawn[i] < 0)
                {
                    timeSpawn[i] = timeSpawnE;
                    transform.GetChild(i).GetChild(0).GetComponent<EnemyInformation>().hp = transform.GetChild(i).GetChild(0).GetComponent<EnemyInformation>().maxhp;
                    transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
                }
            }
        }
    }
}
