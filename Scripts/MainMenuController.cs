using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject creditsPanel;

    [Header("Difficulty UI")]
    public TMP_Dropdown difficultyDropdown; // Easy / Normal / Hard

    public void StartGame()
    {
        // Store chosen difficulty so GameManager can read it
        int diff = difficultyDropdown != null ? difficultyDropdown.value : 1;
        PlayerPrefs.SetInt("Difficulty", diff);
        PlayerPrefs.Save();

        SceneManager.LoadScene(1); // Scene 1 = Game scene
    }

    public void ShowCredits()
    {
        if (mainPanel    != null) mainPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void HideCredits()
    {
        if (mainPanel    != null) mainPanel.SetActive(true);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
