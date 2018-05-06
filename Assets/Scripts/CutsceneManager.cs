using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour {
    public SceneSwapper sceneSwapper;

    private List<CutsceneController> _startCutscenes = new List<CutsceneController>();
    public CutsceneController startCutscene1;
    public CutsceneController startCutscene2;
    public CutsceneController startCutscene3;
    public CutsceneController startCutscene4;
    private int _curStartScene = 0;

    public CutsceneController midCutscene1;
    public CutsceneController midCutscene2;

    public CutsceneController endCutscene1;
    public CutsceneController endCutscene2;
    public CutsceneController endCutscene3;

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

    public void NextStartScene()
    {
        if (_curStartScene >= _startCutscenes.Count)
        {
            //We're out of scenes and can start the game
            GoToGameplay();
            Debug.Log("Cutscenes done!");
        }
        else
        {
            //Start the next cutscene
            _startCutscenes[_curStartScene].StartCutscene(this);
            _curStartScene++;
        }
    }

    public void GoToGameplay()
    {
        sceneSwapper.StartGame();
    }
}
