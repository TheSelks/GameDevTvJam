
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void FirstLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void SecondLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    public void ThirdLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
