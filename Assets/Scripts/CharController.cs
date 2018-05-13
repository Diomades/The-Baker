using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {
    public GameObject Char;
    public Transform IdlePos;
    public Transform CakePos;
    public Transform PiePos;

    private Camera _mainCam;
    private Vector3 _camIdlePos;

    public CharSprites charSprites;

    private bool _isPlaying;

    public float animationFrameTime;
    public float startMoveHoldTime;
    public float midMoveHoldTime;

    public float cameraMoveTime;
    public float cameraMoveDistance;

    private CharState _curState = CharState.Idle;
    private bool _isHappy;

    void Update()
    {
        if (_isPlaying)
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
    }

    public void CharStart(bool iH, Camera cam)
    {
        _isPlaying = true;

        //Set the camera's hold position
        _mainCam = cam;
        _camIdlePos = _mainCam.transform.position;

        //Set up the sprites
        Char.transform.position = IdlePos.position;
        _isHappy = iH;
        _curState = CharState.Idle;
        charSprites.StartCharacter(CharState.Idle, animationFrameTime);
        //charSprites.ChangeState(CharState.Idle);
    }

    //This function is for when we are continuing the game from a cutscene
    public void CharContinue(bool iH)
    {
        _isPlaying = true;

        //Return the camera to its idle position
        _mainCam.transform.position = _camIdlePos;

        //Set up the sprites
        Char.transform.position = IdlePos.position;
        _isHappy = iH;
        _curState = CharState.Idle;
        charSprites.StartCharacter(CharState.Idle, animationFrameTime);
    }

    public void CharStop()
    {
        _isPlaying = false;
        charSprites.StopAllCoroutines();
    }

    public void UpdateHappiness(bool iH)
    {
        _isHappy = iH;
        charSprites.UpdateHappiness(_isHappy);
    }

    IEnumerator MoveTo(CharState state)
    {
        //Store our last state, note that we're moving
        _curState = CharState.Moving;

        //Store our final position in the sorting order. 3 is the default as that's the idle position
        int sortPos = 3;

        Vector3 midPos = new Vector3();
        Vector3 endPos = new Vector3();

        //Figure out our mid position by checking it against our current position and next position
        switch (state)
        {
            case CharState.Idle:
                sortPos = 3;
                endPos = IdlePos.position;
                midPos = Vector3.Lerp(Char.transform.position, IdlePos.position, 0.5f);
                break;
            case CharState.Pie:
                sortPos = 1;
                endPos = PiePos.position;
                midPos = Vector3.Lerp(Char.transform.position, PiePos.position, 0.5f);
                break;
            case CharState.Cake:
                sortPos = 1;
                endPos = CakePos.position;
                midPos = Vector3.Lerp(Char.transform.position, CakePos.position, 0.5f);
                break;
        }

        //Calculations are done, wait a little before our first movement
        yield return new WaitForSeconds(startMoveHoldTime);

        //Change our sprite for the Movement state & Move their sorting order to hide the sprite behind the counter
        charSprites.ChangeState(_curState);
        Char.GetComponent<SpriteRenderer>().sortingOrder = 1;

        //If the MidPos is less or more than our current position, rotate the facing direction of the mid frame
        if (midPos.x < Char.transform.position.x)
        {
            //Face the sprite left
            Char.GetComponent<SpriteRenderer>().flipX = false;
            //Move the camera left
            MoveCamera(cameraMoveDistance*-1);
        }
        else if (midPos.x > Char.transform.position.x)
        {
            //Move the camera right
            MoveCamera(cameraMoveDistance);
        }

        //Move to mid point
        Char.transform.position = midPos;

        //Wait again
        yield return new WaitForSeconds(midMoveHoldTime);

        //Move the character into its end position and set its sorted position
        Char.transform.position = endPos;
        Char.GetComponent<SpriteRenderer>().sortingOrder = sortPos;

        //Fix our state to the actual end state, and flip us back (if we moved left)
        _curState = state;
        Char.GetComponent<SpriteRenderer>().flipX = true;
        charSprites.ChangeState(_curState);
    }

    public CharState CurrentState
    {
        get { return _curState; }
    }

    private void MoveCamera(float dist)
    {
        //Copy the camera position & update it with the desired change
        Vector3 target = _mainCam.transform.position;
        target.x += dist;
        //Start a coroutine
        StartCoroutine(CamTrack(target, dist));
    }

    IEnumerator CamTrack(Vector3 target, float dist)
    {
        //Track the time
        float time = 0f;
        //Get the starting position from the actual camera and not the idle position
        Vector3 start = _mainCam.transform.position;

        //Move the camera while we have time remaining
        while (time < cameraMoveTime)
        {
            time += Time.deltaTime;
            _mainCam.transform.position = Vector3.Lerp(start, target, Mathf.SmoothStep(0f, 0.5f, time));
            yield return null;
        }
    }
}
