using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    private const string mainLevel = "Main", gameScore = "GameScores", aboutGame = "AboutGame";
    private string currentLevel = null;
    private string nextLevel = null;

    public GameObject resumeGameButton;
    public GameObject restartButton;
    public GameObject nextLevelButton;
    public GameObject mainMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        Utilities.CurrentScene();
        currentLevel = Utilities.GetCurrentSceneName();
        nextLevel = Utilities.GetNextLevelName();
    }

    // Update is called once per frame
    void Update()
    {
        if (Utilities.GetGameProgress() == 100.0f && !Utilities.GetIsPaused()) {
            Menu();
            /*
            if (!nextLevelButton.activeSelf) {
                // Utilities.SetIsPaused(true);
                // nextLevelButton.SetActive(true);
                Menu();
            }
            */
        }
    }

    public void NewGame() {
        Debug.Log("New Game");

        if (nextLevel == null) { return; }

        Utilities.LoadScene(nextLevel);
    }

    public void GameScores() {
        Debug.Log("Game Scores");

        Utilities.LoadScene(gameScore);
    }

    public void AboutGame() {
        Debug.Log("About Game");

        Utilities.LoadScene(aboutGame);
    }

    public void Menu() {
        Debug.Log("Menu");

        if (!Utilities.GetIsPaused()) {
            Utilities.SetIsPaused(true);
            
            resumeGameButton.SetActive(true);
            restartButton.SetActive(true);
            mainMenuButton.SetActive(true);

            if (Utilities.GetGameProgress() == 100.0f) {
                nextLevelButton.SetActive(true);
            } else {
                nextLevelButton.SetActive(false);
            }

        } else {
            Utilities.SetIsPaused(false);

            resumeGameButton.SetActive(false);
            restartButton.SetActive(false);
            mainMenuButton.SetActive(false);
        }

        /*
        if (Utilities.GetGameProgress() == 100.0f) {
            nextLevelButton.SetActive(true);
        } else {
            nextLevelButton.SetActive(false);
        }
        */

        Utilities.PauseGame(Utilities.GetIsPaused());

        
    }

    public void RestartLevel() {
        Debug.Log("Restart Level");

        if (currentLevel == null) { return; }

        
        Utilities.LoadScene(currentLevel);
    }

    public void MainMenu() {
        Debug.Log("Main Menu");

        Utilities.LoadScene(mainLevel);
    }

    public void NextLevel() {
        Debug.Log("Next Level");

        if (nextLevel != null) {
            Utilities.LoadScene(nextLevel);
        } else {
            Debug.LogError("Cannot load Next-Level: (nextLevel is NULL)");
        }
    }
}
