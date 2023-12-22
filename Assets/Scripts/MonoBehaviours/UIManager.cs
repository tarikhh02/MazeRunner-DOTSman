using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject loseUI;
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject loadingUI;

    [SerializeField] TextMeshProUGUI pelletsText;
    [SerializeField] TextMeshProUGUI scoreText;

    public bool canPlayerPlay = false;

    float timer = 1f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (timer <= 0)
            ChangeUI(gameUI);
        timer-= Time.deltaTime;
    }

    public void Win()
    {
        AnimationManager.instance.SetBoolAnimationTriggers("Win", true);
        ChangeUI(winUI);
    }

    public void Lose()
    {
        AnimationManager.instance.SetBoolAnimationTriggers("Lose", true);
        ChangeUI(loseUI);
    }

    public void UpdateGameUI(string pelletsTxt, string scoreTxt)
    { 
        pelletsText.text = pelletsTxt;
        scoreText.text = scoreTxt;
    }

    void ChangeUI(GameObject uiToChangeTo)
    {
        loadingUI.SetActive(false);
        winUI.SetActive(false);
        loseUI.SetActive(false);
        gameUI.SetActive(false);

        uiToChangeTo.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(1);
    }
}