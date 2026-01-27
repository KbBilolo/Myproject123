using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Scene currentScene;

    private GameObject pauseMenuUi;

    // Start is called before the first frame update

    //Here lies the Game Win Condition



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void LoadSceneAsynced(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }


    public void LoadPauseMenuScene(string pauseMenueSceneName)
    {
        SceneManager.LoadSceneAsync(pauseMenueSceneName, LoadSceneMode.Additive);
        foreach (GameObject rootObj in SceneManager.GetSceneByName(pauseMenueSceneName).GetRootGameObjects())
        {
            if (rootObj.CompareTag("PauseMenuUI"))
            {
                pauseMenuUi = rootObj;
                pauseMenuUi.SetActive(false); // Hide by default
                break;
            }
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        if (pauseMenuUi != null)
            pauseMenuUi.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (pauseMenuUi != null)
            pauseMenuUi.SetActive(false);
    }
}
