using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;   

public class LevelTimer : MonoBehaviour
{
    public float levelDuration = 20f;           
    public TextMeshProUGUI timerText;        

    private float currentTime;
    private bool isRunning = true;

    void Start()
    {
        currentTime = levelDuration;
        UpdateTimerText();
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            UpdateTimerText();
            isRunning = false;
            RestartLevel();
        }
        else
        {
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        
        int seconds = Mathf.CeilToInt(currentTime);
        timerText.text = seconds.ToString();
    }

    void RestartLevel()
    {
        
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
