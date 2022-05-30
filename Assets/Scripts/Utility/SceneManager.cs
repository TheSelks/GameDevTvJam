
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void FirstLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    public void Instructions()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void GameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }

    public void Credits()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }

    public void Story()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(5);
    }

    public void Victory()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(6);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
