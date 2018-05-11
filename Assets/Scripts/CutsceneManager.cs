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

    public void OpeningCutscenes()
    {
        //Add the Start Cutscenes to a list
        _startCutscenes.Add(startCutscene1);
        _startCutscenes.Add(startCutscene2);
        _startCutscenes.Add(startCutscene3);
        _startCutscenes.Add(startCutscene4);

        //Start the opening cutscene
        NextStartScene();
    }

    public void SkipButton()
    {
        //Unload the current scene
        _curActiveScene.UnloadCutscene();

        //If it's an end scene, end gameplay
        if (_curActiveScene == gameOverCraig || _curActiveScene == gameOverDad || _curActiveScene == gameOverSadness || _curActiveScene == endCutscene)
        {
            EndGameplay();
        }
        //If it's a mid scene, continue gameplay
        else if (_curActiveScene == midCutscene1 || _curActiveScene == midCutscene2)
        {
            ContinueGameplay();
        }
        //If it's a start cutscene, go to the gameplay
        else
        {
            GoToGameplay();
            _curStartScene = 0;
        }
    }

    public void NextStartScene()
    {
        if (_curStartScene >= _startCutscenes.Count)
        {
            //We're out of scenes and can start the game
            GoToGameplay();
            _curStartScene = 0;
        }
        else
        {
            //Start the next cutscene
            _curActiveScene = _startCutscenes[_curStartScene];
            _curActiveScene.StartCutscene(this, false);
            _curStartScene++;
        }
    }

    public void DisplayCutscene(SceneID scene)
    {
        //Choose which Cutscene is going to play
        switch (scene)
        {
            case SceneID.Mid1:
                _curActiveScene = midCutscene1;
                //midCutscene1.StartCutscene(this, false);
                break;
            case SceneID.Mid2:
                _curActiveScene = midCutscene2;
                //midCutscene2.StartCutscene(this, false);
                break;
            case SceneID.Craig:
                _curActiveScene = midCutscene1;
                //gameOverCraig.StartCutscene(this, true);
                break;
            case SceneID.Dad:
                _curActiveScene = gameOverDad;
                //gameOverDad.StartCutscene(this, true);
                break;
            case SceneID.Sad:
                _curActiveScene = gameOverSadness;
                //gameOverSadness.StartCutscene(this, true);
                break;
            case SceneID.Happy:
                _curActiveScene = endCutscene;
                //endCutscene.StartCutscene(this, true);
                break;
        }

        //Launch the selected scene
        _curActiveScene.StartCutscene(this, true);
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
        sceneSwapper.GoToMenu();
    }
}
