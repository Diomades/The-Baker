using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLoop : MonoBehaviour {
    public SceneSwapper sceneSwapper;
    public InterfaceManager intMan;
    public CharController charCont;

    public float weekLengthInSeconds;
    private int _weekCount = 0;

    private float _curPieDesire; //We calculate Desire by treating this as Pie, with Cake being the leftover beneath 1f
    private float _curCraigApproval;
    private float _curDadApproval;
    public float startHappiness;
    public float happinessThreshold;
    private float _curHappiness;
    private int _curWeek;

    private float _happinessMod = 0f;
    private bool _isHappy;

    public float panicThreshold; //The point at which the player panics
    private float _dadUnhappy = 0f;
    private float _craigUnhappy = 0f;

    private bool _isPlaying;

	// Use this for initialization
	public void StartGame ()
    {
        //Set our starting values
        _isPlaying = true;
        _curHappiness = startHappiness;
        _curPieDesire = Random.Range(0f, 1f);
        _curCraigApproval = 0.4f;
        _curDadApproval = 0.6f;
        _curWeek = 1;

        //Start everything
        _isHappy = startHappiness > happinessThreshold; //Set isHappy based on our starting happiness and happiness threshold
        charCont.CharStart(_isHappy);
        intMan.HideWarningInterface();
        intMan.UpdateInterface(_curPieDesire,_curDadApproval,_curCraigApproval,_curHappiness, _curWeek);
        StartCoroutine(TimeTick());
    }

    IEnumerator TimeTick()
    {
        while (_isPlaying)
        {
            float curNotch = _weekCount * (1f / 6f);
            intMan.UpdateDayBar(curNotch);
            _weekCount++;

            yield return new WaitForSeconds(weekLengthInSeconds / 7);
            if(_weekCount == 7)
            {
                //Check how we did this past week before defining new desires for the next week
                UpdateApproval();
                ChangeDesires();
                UpdateHappiness();
                _curWeek++;
                _weekCount = 0;
                //Update stats
                intMan.UpdateInterface(_curPieDesire, _curDadApproval, _curCraigApproval, _curHappiness, _curWeek);
                //Show warnings if necessary and we aren't currently showing a warning
                if (!intMan.IsShowingWarning)
                {
                    if (_curHappiness <= panicThreshold)
                    {
                        Debug.Log("Showing Happiness warning");
                        intMan.ShowHappinessWarning();
                    }
                    else if (_curCraigApproval <= panicThreshold)
                    {
                        intMan.ShowCraigWarning();
                    }
                    else if (_curDadApproval <= panicThreshold)
                    {
                        intMan.ShowDadWarning();
                    }
                }
            }                       
        }
    }

    private void UpdateApproval()
    {
        if (charCont.CurrentState == CharState.Pie)
        {
            //Increase and reset Dad's approval
            _curDadApproval += 0.05f;
            _dadUnhappy = 0f;
            //Begin decreasing Craig's happiness and approval
            _craigUnhappy += 0.02f;
            _curCraigApproval -= _craigUnhappy;
        }
        else if (charCont.CurrentState == CharState.Cake)
        {
            //Increase and reset Craig's approval
            _curCraigApproval += 0.05f;
            _craigUnhappy = 0f;
            //Begin decreasing Dad's happiness and approval
            _dadUnhappy += 0.02f;
            _curDadApproval -= _dadUnhappy;
        }

        //Round out our numbers if necessary
        if (_curDadApproval > 1f)
        {
            _curDadApproval = 1f;
        }
        else if (_curDadApproval < 0f)
        {
            _curDadApproval = 0f;
        }
        if (_curCraigApproval > 1f)
        {
            _curCraigApproval = 1f;
        }
        else if (_curCraigApproval < 0f)
        {
            _curCraigApproval = 0f;
        }
    }

    private void ChangeDesires()
    {
        float _curCakeDesire = 1f - _curPieDesire;

        if (charCont.CurrentState == CharState.Pie)
        {
            if(_curPieDesire > 0.5f)
            {
                //Increase Happiness
                _happinessMod = 0.05f + (_curPieDesire / 100f);
                //Increase Cake desire
                _curPieDesire = _curPieDesire - 0.1f;
            }
            else if (_curPieDesire > 0.3f)
            {
                //I kinda wanted Cake more
                _happinessMod = 0.02f + (_curPieDesire / 100f);
            }
            else
            {
                //I wanted Cake but I got Pie
                _happinessMod = -0.05f - (_curCakeDesire / 100f);
            }
        }
        else if (charCont.CurrentState == CharState.Cake)
        {
            if(_curCakeDesire > 0.5f)
            {
                //Increase Happiness
                _happinessMod = 0.05f + ((1f - _curPieDesire) / 100f);
                //Increase Pie desire
                _curPieDesire = _curPieDesire + 0.1f;
            }
            else if (_curCakeDesire > 0.3f)
            {
                //I kinda wanted Pie more
                _happinessMod = 0.02f + (_curCakeDesire / 100f);
            }
            else
            {
                //I want Pie but I got Cake
                _happinessMod = -0.05f - (_curPieDesire / 100f);
            }
        }
        else
        {
            _happinessMod = -0.1f;
        }

        //Go on to randomizing our new desires
        RandomDesires();
    }

    private void UpdateHappiness()
    {
        //If approval is too low, we become unhappy
        if (_curDadApproval <= 0.2f || _curCraigApproval <= 0.2f)
        {
            _happinessMod = -0.1f;
        }

        _curHappiness += _happinessMod;
        //If we're happy now but weren't before
        if(_curHappiness >= 0.6f && !_isHappy)
        {
            _isHappy = true;
            charCont.UpdateHappiness(_isHappy);
        }
        //Else if we're unhappy but were happy before
        else if (_curHappiness < 0.6f && _isHappy)
        {
            _isHappy = false;
            charCont.UpdateHappiness(_isHappy);
        }
    }

    private void RandomDesires()
    {
        //To change desires, we first toss a coin to see if they have changed (weighted to the current larger desire). 0 is Cake, 1 is Pie.
        float coinToss = Random.Range(0f, 1f);
        float desireChange = Mathf.Round(Random.Range(0.2f, 0.5f)* 100f)/100f; //Round to 2 decimal places

        //The closer that _curPieDesire is to 1, the closer it is to being the larger desire
        if (_curPieDesire > 0.5f)
        {
            coinToss = coinToss - (_curPieDesire / 10f);
        }
        else
        {
            coinToss = coinToss + (_curPieDesire / 10f);
        }

        //If closer to 1, Pie desire increases. If closer to 0, Cake desire increases.
        if (coinToss > 0.55f)
        {
            Debug.Log("I want Pie more");
            _curPieDesire = _curPieDesire + desireChange;
        }
        else if (coinToss < 0.45f)
        {
            Debug.Log("I want Cake more");
            _curPieDesire = _curPieDesire - desireChange;
        }
        else
        {
            Debug.Log("My desires haven't changed");
        }

        //Round out the desire and we're done.
        if (_curPieDesire > 1f)
        {
            _curPieDesire = 1f;
        }
        else if (_curPieDesire < 0f)
        {
            _curPieDesire = 0f;
        }
    }
}
