using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLoop : MonoBehaviour {
    public SceneSwapper sceneSwapper;
    public InterfaceManager intMan;
    public CharController charCont;

    public int totalWeeks;
    public int event1Week;
    public int event2Week;
    public float weekLengthInSeconds;
    private int _weekCount = 0;

    private float _curDesire; //1 is Pie, 0 is Cake
    public float startHappiness;
    public float happinessThreshold;
    private float _curHappiness;
    private int _curWeek;

    private float _happinessMod = 0f;
    private bool _isHappy;

    public float approvalIncrease;
    public float approvalDecrease;
    public float panicThreshold; //The point at which the player panics
    public float craigApprovalRating;
    public float dadApprovalRating;
    public float approvalHappyThreshold; //Above this number the approval is happy
    public float approvalNeutralThreshold; //Above this number the approval is neutral
    private float _dadUnhappy = 0f;
    private float _craigUnhappy = 0f;

    private bool _isPlaying;

	// Use this for initialization
	public void StartGame (Camera cam)
    {
        //Set our starting values
        _isPlaying = true;
        _curHappiness = startHappiness;
        _curDesire = Random.Range(0f, 1f);
        _curWeek = 1;

        //Start everything
        _isHappy = startHappiness > happinessThreshold; //Set isHappy based on our starting happiness and happiness threshold
        charCont.CharStart(_isHappy, cam);
        intMan.HideWarningInterface();
        intMan.HideBakingNotice();
        intMan.UpdateInterface(_curDesire, ApprovalLevelFromFloat(dadApprovalRating), ApprovalLevelFromFloat(craigApprovalRating), _curHappiness, _curWeek);
        StartCoroutine(TimeTick());
    }

    //This assumes we're returning from a cutscene or the like and do not need to reset anything
    public void ResumeGame()
    {
        _isPlaying = true;
        _weekCount = 0;
        charCont.CharContinue(_isHappy);
        intMan.HideWarningInterface();
        intMan.HideBakingNotice();
        intMan.UpdateInterface(_curDesire, ApprovalLevelFromFloat(dadApprovalRating), ApprovalLevelFromFloat(craigApprovalRating), _curHappiness, _curWeek);
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
                //If we haven't reached a GameOver state
                if (!IsGameOver())
                {
                    _curWeek++;
                    _weekCount = 0;
                    //Update stats
                    intMan.UpdateInterface(_curDesire, ApprovalLevelFromFloat(dadApprovalRating), ApprovalLevelFromFloat(craigApprovalRating), _curHappiness, _curWeek);

                    //Check if there's any events
                    if (_curWeek == totalWeeks)
                    {
                        //We reached the end of the game
                        StopForScene(SceneID.Happy);
                    }
                    else if (_curWeek == event2Week)
                    {
                        StopForScene(SceneID.Mid2);
                    }
                    else if (_curWeek == event1Week)
                    {
                        StopForScene(SceneID.Mid1);
                    }
                }
            }                       
        }
    }

    private bool IsGameOver()
    {
        //Show warnings if necessary and we aren't currently showing a warning
        if (_curWeek < totalWeeks)
        {
            if (_curHappiness <= panicThreshold)
            {
                if (_curHappiness > 0f && !intMan.IsShowingWarning)
                {
                    intMan.ShowWarning(WarningText.Happiness);
                }
                else
                {
                    //Sadness game over
                    StopForScene(SceneID.Sad);
                    return true;
                }
            }
            else if (craigApprovalRating <= panicThreshold)
            {
                if (craigApprovalRating > 0f && !intMan.IsShowingWarning)
                {
                    intMan.ShowWarning(WarningText.Craig);
                }
                else
                {
                    //Craig game over
                    StopForScene(SceneID.Craig);
                    return true;
                }
            }
            else if (dadApprovalRating <= panicThreshold)
            {
                if (dadApprovalRating > 0f && !intMan.IsShowingWarning)
                {
                    intMan.ShowWarning(WarningText.Dad);
                }
                else
                {
                    //Dad game over
                    StopForScene(SceneID.Dad);
                    return true;
                }
            }
        }

        return false;
    }

    private void StopForScene(SceneID sceneType)
    {
        _isPlaying = false;
        StopAllCoroutines();
        charCont.CharStop();

        sceneSwapper.CutsceneEvent(sceneType);
    }

    private void UpdateApproval()
    {
        if (charCont.CurrentState == CharState.Pie)
        {
            //We made a pie, so tell the Interface Manager to show one
            intMan.MakeNewItem(false);            
            //Decrease lingering unhappiness over time
            _dadUnhappy -= approvalDecrease;
            if(_dadUnhappy < 0)
            {
                _dadUnhappy = 0;
            }
            //Increase dad's approval, offset by his unhappiness
            dadApprovalRating += (approvalIncrease + _dadUnhappy);
            //Begin decreasing Craig's happiness and approval
            //This stacks over time!
            _craigUnhappy += approvalDecrease;
            craigApprovalRating -= _craigUnhappy;
        }
        else if (charCont.CurrentState == CharState.Cake)
        {
            //We made a pie, so tell the Interface Manager to show one
            intMan.MakeNewItem(true);
            //Decrease lingering unhappiness over time
            _craigUnhappy -= approvalDecrease;
            if (_craigUnhappy < 0)
            {
                _craigUnhappy = 0;
            }
            //Increase and reset Craig's approval
            craigApprovalRating += (approvalIncrease + _craigUnhappy);
            //Begin decreasing Dad's happiness and approval
            //This stacks over time!
            _dadUnhappy += approvalDecrease;
            dadApprovalRating -= _dadUnhappy;
        }

        //Round out our numbers if necessary
        if (dadApprovalRating > 1f)
        {
            dadApprovalRating = 1f;
        }
        else if (dadApprovalRating < 0f)
        {
            dadApprovalRating = 0f;
        }
        if (craigApprovalRating > 1f)
        {
            craigApprovalRating = 1f;
        }
        else if (craigApprovalRating < 0f)
        {
            craigApprovalRating = 0f;
        }
    }

    //This takes a given float and compares against stored values to declare a character's mood
    private ApprovalLevel ApprovalLevelFromFloat(float lvl)
    {
        if(lvl >= approvalHappyThreshold)
        {
            return ApprovalLevel.Happy;
        }
        else if (lvl >= approvalNeutralThreshold)
        {
            return ApprovalLevel.Neutral;
        }
        else
        {
            return ApprovalLevel.Angry;
        }
    }

    private void ChangeDesires()
    {
        //float _curCakeDesire = 1f - _curDesire;

        if (charCont.CurrentState == CharState.Pie)
        {
            if(_curDesire > 0.5f)
            {
                //Increase Happiness
                _happinessMod = 0.05f + (_curDesire / 20f);
                //Increase Cake desire
                _curDesire = _curDesire - 0.1f;
            }
            else if (_curDesire > 0.3f)
            {
                //I kinda wanted Cake more
                _happinessMod = 0.02f + (_curDesire / 20f);
            }
            else
            {
                //I wanted Cake but I got Pie
                intMan.ShowWarning(WarningText.Pie);
                _happinessMod = -0.05f - ((1f - _curDesire) / 20f);
            }
        }
        else if (charCont.CurrentState == CharState.Cake)
        {
            if(_curDesire <= 0.5f)
            {
                //Increase Happiness
                _happinessMod = 0.05f + ((1f - _curDesire) / 20f);
                //Increase Pie desire
                _curDesire = _curDesire + 0.1f;
            }
            else if (_curDesire < 0.7f)
            {
                //I kinda wanted Pie more
                _happinessMod = 0.02f + ((1f - _curDesire) / 20f);
            }
            else
            {
                //I wanted Pie but I got Cake
                intMan.ShowWarning(WarningText.Cake);
                _happinessMod = -0.05f - (_curDesire / 20f);
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
        if (dadApprovalRating <= 0.2f || craigApprovalRating <= 0.2f)
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

        //Happiness rounding so we can't go over 100%
        if (_curHappiness > 1f)
        {
            _curHappiness = 1f;
        }
    }

    private void RandomDesires()
    {
        //To change desires, we first toss a coin to see if they have changed (weighted to the current larger desire). 0 is Cake, 1 is Pie.
        float coinToss = Random.Range(0f, 1f);
        float desireChange = Mathf.Round(Random.Range(0.2f, 0.5f)* 100f)/100f; //Round to 2 decimal places

        //The closer that _curPieDesire is to 1, the closer it is to being the larger desire
        if (_curDesire > 0.5f)
        {
            coinToss = coinToss - (_curDesire / 10f);
        }
        else
        {
            coinToss = coinToss + (_curDesire / 10f);
        }

        //If closer to 1, Pie desire increases. If closer to 0, Cake desire increases.
        if (coinToss > 0.55f)
        {
            Debug.Log("I want Pie more");
            _curDesire = _curDesire + desireChange;
        }
        else if (coinToss < 0.45f)
        {
            Debug.Log("I want Cake more");
            _curDesire = _curDesire - desireChange;
        }
        else
        {
            Debug.Log("My desires haven't changed");
        }

        //Round out the desire and we're done.
        if (_curDesire > 1f)
        {
            _curDesire = 1f;
        }
        else if (_curDesire < 0f)
        {
            _curDesire = 0f;
        }
    }
}
