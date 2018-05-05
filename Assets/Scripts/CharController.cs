using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {
    public GameObject Char;
    public Transform IdlePos;
    public Transform CakePos;
    public Transform PiePos;

    public CharSprites charSprites;

    private CharState _lastState = CharState.Idle;
    private CharState _curState = CharState.Idle;

    void Update()
    {
        if (_curState != CharState.Moving)
        {
            if (Input.GetKeyUp("a") || Input.GetKeyUp("left"))
            {
                //Move to Cake State
                if (_curState == CharState.Idle)
                {
                    //SetState(CharState.Cake);
                    StartCoroutine(MoveTo(CharState.Cake));
                }
                //Move to Idle State
                else if (_curState == CharState.Pie)
                {
                    //SetState(CharState.Idle);
                    StartCoroutine(MoveTo(CharState.Idle));
                }
                //Being in Cake State does nothing
            }
            if (Input.GetKeyUp("d") || Input.GetKeyUp("right"))
            {
                //Move to Pie State
                if (_curState == CharState.Idle)
                {
                    //SetState(CharState.Pie);
                    StartCoroutine(MoveTo(CharState.Pie));
                }
                //Move to Idle State
                else if (_curState == CharState.Cake)
                {
                    //SetState(CharState.Idle);
                    StartCoroutine(MoveTo(CharState.Idle));
                }
                //Being in Pie State does nothing
            }
        }        
    }

    public void CharStart()
    {
        Char.transform.position = IdlePos.position;
        charSprites.ChangeState(CharState.Idle);
    }

    IEnumerator MoveTo(CharState state)
    {
        //Store our last state, note that we're moving
        _lastState = _curState;
        _curState = CharState.Moving;

        Vector3 midPos = new Vector3();
        Vector3 endPos = new Vector3();

        //Figure out our mid position by checking it against our current position and next position
        switch (state)
        {
            case CharState.Idle:
                endPos = IdlePos.position;
                midPos = Vector3.Lerp(Char.transform.position, IdlePos.position, 0.5f);
                break;
            case CharState.Pie:
                endPos = PiePos.position;
                midPos = Vector3.Lerp(Char.transform.position, PiePos.position, 0.5f);
                break;
            case CharState.Cake:
                endPos = CakePos.position;
                midPos = Vector3.Lerp(Char.transform.position, CakePos.position, 0.5f);
                break;
        }

        //Calculations are done, wait a little before our first movement
        yield return new WaitForSeconds(0.1f);

        //Change our sprite for the Movement state
        charSprites.ChangeState(_curState);

        //If the MidPos is less or more than our current position, rotate the facing direction of the mid frame
        if (midPos.x < Char.transform.position.x)
        {
            //Face left
            Char.GetComponent<SpriteRenderer>().flipX = false;
        }
        //Move to mid point
        Char.transform.position = midPos;

        //Wait again
        yield return new WaitForSeconds(.9f);

        Char.transform.position = endPos;

        //Fix our state to the actual end state, and flip us back (if we moved left)
        _curState = state;
        Char.GetComponent<SpriteRenderer>().flipX = true;
        charSprites.ChangeState(_curState);
    }

    public CharState CurrentState
    {
        get { return _curState; }
    }
}
