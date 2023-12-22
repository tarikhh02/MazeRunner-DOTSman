using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenControll : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
}
