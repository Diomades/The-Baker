using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapper : MonoBehaviour {
    public GameObject menu;
    public GameObject startScreen;
    public GameObject pauseScreen;
    public GameObject cutscenes;
    public GameObject gameUI;
    public GameObject gameplay;

	// Use this for initialization
	void Start () {
        //Set all scenes but the Menu to be inactive
        menu.SetActive(true);
        startScreen.SetActive(true);
        pauseScreen.SetActive(false);
        cutscenes.SetActive(false);
        gameUI.SetActive(false);
        gameplay.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyUp("escape"))
        {
            ContinueGameMenu();
        }
    }

    public void StartOpening()
    {
        menu.SetActive(false);
        cutscenes.SetActive(true);
        gameUI.SetActive(false);
        gameplay.SetActive(false);
        cutscenes.GetComponent<CutsceneManager>().StartManager();
    }

    public void StartGame()
    {
        menu.SetActive(false);
        cutscenes.SetActive(false);
        gameUI.SetActive(true);
        gameplay.SetActive(true);
        gameplay.GetComponent<MainLoop>().StartGame(this.GetComponent<Camera>());
    }

    public void ContinueGame()
    {
        menu.SetActive(false);
        cutscenes.SetActive(false);
        gameUI.SetActive(true);
        gameplay.SetActive(true);
        gameplay.GetComponent<MainLoop>().ResumeGame();
    }

    public void NewGameMenu()
    {
        menu.SetActive(true);
        startScreen.SetActive(true);
        pauseScreen.SetActive(false);
        cutscenes.SetActive(false);
        gameUI.SetActive(false);
        gameplay.SetActive(false);
    }

    public void ContinueGameMenu()
    {
        menu.SetActive(true);
        startScreen.SetActive(false);
        pauseScreen.SetActive(true);
        cutscenes.SetActive(false);
        gameUI.SetActive(false);
        gameplay.SetActive(false);
    }

    public void CutsceneEvent(SceneID endType)
    {
        menu.SetActive(false);
        cutscenes.SetActive(true);
        gameUI.SetActive(false);
        gameplay.SetActive(false);
        cutscenes.GetComponent<CutsceneManager>().DisplayCutscene(endType);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
