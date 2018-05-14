using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwapper : MonoBehaviour {
    public GameObject menu;
    public GameObject startScreen;
    public GameObject pauseScreen;
    public GameObject cutscenes;
    public GameObject gameUI;
    public GameObject gameplay;

    public Toggle ending1;
    private static bool _ending1Toggle;
    public Toggle ending2;
    private static bool _ending2Toggle;
    public Toggle ending3;
    private static bool _ending3Toggle;
    public Toggle ending4;
    private static bool _ending4Toggle;

    // Use this for initialization
    void Start () {
        //Set all scenes but the Menu to be inactive
        menu.SetActive(true);
        Toggles();
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
            //Only do this if the game is actually running
            if (gameplay.activeInHierarchy)
            {
                ContinueGameMenu();
            }
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
        Toggles();
        startScreen.SetActive(true);
        pauseScreen.SetActive(false);
        cutscenes.SetActive(false);
        gameUI.SetActive(false);
        gameplay.SetActive(false);
    }

    public void ContinueGameMenu()
    {
        menu.SetActive(true);
        Toggles();
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

    public void ToggleEnding(SceneID endType)
    {
        switch (endType)
        {
            case SceneID.Craig:
                _ending1Toggle = true;
                ending1.isOn = true;
                break;
            case SceneID.Dad:
                _ending2Toggle = true;
                ending2.isOn = true;
                break;
            case SceneID.Sad:
                _ending3Toggle = true;
                ending3.isOn = true;
                break;
            case SceneID.Happy:
                _ending4Toggle = true;
                ending4.isOn = true;
                break;
        }
    }

    private void Toggles()
    {
        ending1.isOn = _ending1Toggle;
        ending2.isOn = _ending2Toggle;
        ending3.isOn = _ending3Toggle;
        ending4.isOn = _ending4Toggle;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
