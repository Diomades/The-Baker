using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneID { Mid1, Mid2, Craig, Dad, Sad, Happy };

public class CutsceneManager : MonoBehaviour {
    public SceneSwapper sceneSwapper;

    private CutsceneController _curActiveScene;

    private List<CutsceneController> _startCutscenes = new List<CutsceneController>();
    public CutsceneController startCutscene1;
    public CutsceneController startCutscene2;
    public CutsceneController startCutscene3;
    public CutsceneController startCutscene4;
    private int _curStartScene = 0;

    public CutsceneController midCutscene1;
    public CutsceneController midCutscene2;

    public CutsceneController gameOverCraig;
    public CutsceneController gameOverDad;
    public CutsceneController gameOverSadness;

    public CutsceneController endCutscene;

    public GameObject skipButton;
    public GameObject continueButton;
    public GameObject menuButton;

    public void StartManager()
    {
        //Add the Start Cutscenes to a list
        _startCutscenes.Add(startCutscene1);
        _startCutscenes.Add(startCutscene2);
        _startCutscenes.Add(startCutscene3);
        _startCutscenes.Add(startCutscene4);

        //Activate the Skip button
        ShowSkipButton();

        //Start the opening cutscene
        NextStartScene();
    }

    //Skip to the end of the cutscene and show the Continue button
    public void SkipButton()
    {
        _curActiveScene.SkipCutscene();

        //If the cutscene is an end scene, show the Menu button instead of the Continue button
        if (_curActiveScene.isEndScene)
        {
            ShowMenuButton();
        }
        else
        {
            ShowContinueButton();
        }
    }

    //The Continue button should go to the next scene or to the gameplay
    public void ContinueButton()
    {
        //Figure out our next cutscene if we're in the start of the game
        if (_curActiveScene.isStartScene)
        {
            //Unload the current scene before figuring out if there's another scene
            _curActiveScene.UnloadCutscene();
            NextStartScene();
        }
        else
        {
            _curActiveScene.UnloadCutscene();
            ContinueGameplay();
        }        
    }

    //The Menu button goes back to the menu and unloads the gameplay
    public void MenuButton()
    {
        _curActiveScene.UnloadCutscene();
        sceneSwapper.NewGameMenu();
    }

    public void NextStartScene()
    {
        if (_curStartScene >= _startCutscenes.Count)
        {
            //We've not got anymore scenes to do and can unload it
            _curStartScene = 0;
            _curActiveScene.UnloadCutscene();
            GoToGameplay();
        }
        else
        {
            //Start the next cutscene
            _curActiveScene = _startCutscenes[_curStartScene];
            _curActiveScene.StartCutscene(this);
            _curStartScene++;
            //Show the Skip button again
            ShowSkipButton();
        }
    }

    public void DisplayCutscene(SceneID scene)
    {
        //We create this variable to modify and send on later
        //bool isEndScene = false;

        //Choose which Cutscene is going to play
        switch (scene)
        {
            case SceneID.Mid1:
                //isEndScene = false;
                ShowSkipButton();
                _curActiveScene = midCutscene1;
                //midCutscene1.StartCutscene(this, false);
                break;
            case SceneID.Mid2:
                //isEndScene = false;
                ShowSkipButton();
                _curActiveScene = midCutscene2;
                //midCutscene2.StartCutscene(this, false);
                break;
            case SceneID.Craig:
                //isEndScene = true;
                ShowSkipButton();
                _curActiveScene = midCutscene1;
                //gameOverCraig.StartCutscene(this, true);
                break;
            case SceneID.Dad:
                //isEndScene = true;
                ShowSkipButton();
                _curActiveScene = gameOverDad;
                //gameOverDad.StartCutscene(this, true);
                break;
            case SceneID.Sad:
                //isEndScene = true;
                ShowSkipButton();
                _curActiveScene = gameOverSadness;
                //gameOverSadness.StartCutscene(this, true);
                break;
            case SceneID.Happy:
                //isEndScene = true;
                ShowSkipButton();
                _curActiveScene = endCutscene;
                //endCutscene.StartCutscene(this, true);
                break;
        }

        //Launch the selected scene
        _curActiveScene.StartCutscene(this);
    }

    public void ShowMenuButton()
    {
        skipButton.SetActive(false);
        menuButton.SetActive(true);
        continueButton.SetActive(false);
    }

    public void ShowContinueButton()
    {
        skipButton.SetActive(false);
        menuButton.SetActive(false);
        continueButton.SetActive(true);
    }

    public void ShowSkipButton()
    {
        skipButton.SetActive(true);
        menuButton.SetActive(false);
        continueButton.SetActive(false);
    }

    public void GoToGameplay()
    {
        sceneSwapper.StartGame();
    }

    public void ContinueGameplay()
    {
        sceneSwapper.ContinueGame();
    }

    public void EndGameplay()
    {
        sceneSwapper.NewGameMenu();
    }
}
