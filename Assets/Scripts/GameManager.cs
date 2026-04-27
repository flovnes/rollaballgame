using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject startMenuUI;
    [SerializeField] private GameObject endMenuUI;
    [SerializeField] private TextMeshProUGUI endScoreText;
    [SerializeField] private TextMeshProUGUI statusText;

    private bool isGamePaused = false;

    void Start()
    {
        if (startMenuUI != null)
        {
            startMenuUI.SetActive(true);
            Time.timeScale = 0f;
            isGamePaused = true;
        }
        
        if (endMenuUI != null) endMenuUI.SetActive(false);
    }

    public void StartGame()
    {
        if (startMenuUI != null) startMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isGamePaused = false;
    }

    public void ShowEndMenu(bool won, int score)
    {
        Time.timeScale = 0f;
        isGamePaused = true;
        
        if (endMenuUI != null)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            endMenuUI.SetActive(true);
            statusText.text = won ? "nice" : "do better";
            
            endScoreText.text = "score: " + score;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
