using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLoop : MonoBehaviour {
    public GameObject Char;
    public Transform IdlePos;
    public Transform CakePos;
    public Transform PiePos;

    public CharSprites charSprites;

    private CharState lastState = CharState.Idle;
    private CharState curState = CharState.Idle;

    // Use this for initialization
    //Replace Start later with a function called from cutscenes ending
    void Start()
    {
        StartGameplay();
    }

    void Update()
    {
        if (curState != CharState.Moving)
        {
            if (Input.GetKeyUp("a") || Input.GetKeyUp("left"))
            {
                //Move to Cake State
                if (curState == CharState.Idle)
                {
                    //SetState(CharState.Cake);
                    StartCoroutine(MoveTo(CharState.Cake));
                }
                //Move to Idle State
                else if (curState == CharState.Pie)
                {
                    //SetState(CharState.Idle);
                    StartCoroutine(MoveTo(CharState.Idle));
                }
                //Being in Cake State does nothing
            }
            if (Input.GetKeyUp("d") || Input.GetKeyUp("right"))
            {
                //Move to Pie State
                if (curState == CharState.Idle)
                {
                    //SetState(CharState.Pie);
                    StartCoroutine(MoveTo(CharState.Pie));
                }
                //Move to Idle State
                else if (curState == CharState.Cake)
                {
                    //SetState(CharState.Idle);
                    StartCoroutine(MoveTo(CharState.Idle));
                }
                //Being in Pie State does nothing
            }
        }        
    }

    public void StartGameplay()
    {
        Char.transform.position = IdlePos.position;
        charSprites.ChangeState(CharState.Idle);
    }

    IEnumerator MoveTo(CharState state)
    {
        //Store our last state, note that we're moving
        lastState = curState;
        curState = CharState.Moving;

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
        yield return new WaitForSeconds(0.3f);

        //Change our sprite for the Movement state
        charSprites.ChangeState(curState);

        //If the MidPos is less or more than our current position, rotate the facing direction of the mid frame
        if (midPos.x < Char.transform.position.x)
        {
            //Face left
            Char.GetComponent<SpriteRenderer>().flipX = true;
        }
        //Move to mid point
        Char.transform.position = midPos;

        //Wait again
        yield return new WaitForSeconds(1f);

        Char.transform.position = endPos;

        //Fix our state to the actual end state, and flip us back (if we moved left)
        curState = state;
        Char.GetComponent<SpriteRenderer>().flipX = false;
        charSprites.ChangeState(curState);
    }

    public CharState CurrentState()
    {
        return curState;
    }
}
