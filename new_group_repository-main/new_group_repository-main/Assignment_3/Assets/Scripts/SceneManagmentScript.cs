using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagmentScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
}

        /*if (sceneNumber == 1)
        {
            SceneManager.LoadScene(1);
        }
        else if (sceneNumber == 2)
        {
            SceneManager.LoadScene(1);
        }
        else if (sceneNumber == 3)
        {
            SceneManager.LoadScene(3);''
        }*/
        