using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[SerializeField]
public enum SceneIndex
{
    HOME_MENU,
    HOME_SETTING,
    SELECT_MAP,
    PLAY_1,
    PLAY_2,
    PLAY_3,
    PLAY_4,
    SHOP,
    SKILL_SETTING
}

public class SceneController : MonoBehaviour
{
    private static string[] Menu = { "HomeMenu", "HomeSetting" , "SelectMap", "Play1", "Play2", "Play3", "Play4", "Shop", "SkillSetting"};
    private static SceneController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void LoadScene(SceneIndex i)
    {
        instance.StartCoroutine(instance.LoadSceneAsync(Menu[(int)i]));
    }
    public static void LoadScene(int i)
    {
        instance.StartCoroutine(instance.LoadSceneAsync(Menu[i]));
    }
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(sceneName);
    }
}