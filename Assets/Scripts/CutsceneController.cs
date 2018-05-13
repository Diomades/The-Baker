using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour {
    private CutsceneManager _cutsceneManager;

    private List<GameObject> _sceneText = new List<GameObject>();
    private List<float> _textDelay = new List<float>();
    private int _nextText = 0;
    public bool isStartScene;
    public bool isEndScene;

    private float _initialTextDelay = 1f;
    public float delaytoText2;
    public float delaytoText3;
    public float delaytoText4;
    public float delaytoText5;
    public float delaytoText6;
    public float delaytoText7;
    public float delaytoText8;
    public float delaytoText9;
    public float delaytoText10;

    //private bool _isActive = false;

    public void StartCutscene(CutsceneManager man)
    {
        this.gameObject.SetActive(true);
        _cutsceneManager = man;

        _textDelay.Add(_initialTextDelay);
        _textDelay.Add(delaytoText2);
        _textDelay.Add(delaytoText3);
        _textDelay.Add(delaytoText4);
        _textDelay.Add(delaytoText5);
        _textDelay.Add(delaytoText6);
        _textDelay.Add(delaytoText7);
        _textDelay.Add(delaytoText8);
        _textDelay.Add(delaytoText9);
        _textDelay.Add(delaytoText10);

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            _sceneText.Add(child.gameObject);
        }

        StartCoroutine(DelayedShowText());
    }

    //This is activated when the player clicks the Skip button
    public void SkipCutscene()
    {
        //Stop any active coroutines
        StopAllCoroutines();
        _nextText = 0;

        //Display all cutscene text
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void UnloadCutscene()
    {
        _nextText = 0;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    //NextTextPresent returns False if there's no more text to read
    public bool NextTextPresent()
    {
        if(_nextText >= _sceneText.Count)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    IEnumerator DelayedShowText()
    {
        yield return new WaitForSeconds(_textDelay[_nextText]);
        //Show the text after a delay
        ShowText();
    }

    private void ShowText()
    {
        _sceneText[_nextText].SetActive(true);

        _nextText++;

        if (NextTextPresent())
        {
            StartCoroutine(DelayedShowText());
        }
        else
        {
            //We're done with this cutscene. If the cutscene is an end scene, show the Menu button instead of the Continue button
            if (isEndScene)
            {
                _cutsceneManager.ShowMenuButton();
            }
            else
            {
                _cutsceneManager.ShowContinueButton();
            }
        }
    }

    //This is run when the player clicks the "Continue" button
    /*public void EndScene()
    {
        _isActive = false;
        if (isStartScene)
        {
            //Tell the Cutscene Manager to run the next scene
            this.gameObject.SetActive(false);
            _cutsceneManager.NextStartScene();
        }
        else if (!isStartScene)
        {
            if (endGame)
            {
                //This cutscene triggers the end of the game
                this.gameObject.SetActive(false);
                _cutsceneManager.EndGameplay();
            }
            else
            {
                //The cutscene isn't a starting scene and it's finished, so go to gameplay
                this.gameObject.SetActive(false);
                _cutsceneManager.ContinueGameplay();
            }
        }
    }*/
}
