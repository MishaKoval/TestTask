using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsController : MonoBehaviour
{
    public void OnButtonExitClick()
    {
        Application.Quit();
    }

    public void OnButtonRestartClick()
    {
        SceneManager.LoadScene(0);
    }
}
