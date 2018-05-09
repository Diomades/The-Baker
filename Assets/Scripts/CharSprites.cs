using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharState { Idle, Moving, Pie, Cake };

public class CharSprites : MonoBehaviour {
    public Sprite idleSprite1H;
    public Sprite idleSprite1S;
    public Sprite idleSprite2H;
    public Sprite idleSprite2S;
    public Sprite walkSpriteH;
    public Sprite walkSpriteS;
    public Sprite bakingSprite1;
    public Sprite bakingSprite2;

    private CharState _curState;
    private bool _isHappy;
    private string _lastSprite;

    private bool _doAnimate;

    public void ChangeState(CharState state)
    {
        //Set the new state
        _curState = state;
        //Stop any current animation
        StopCoroutine(Animate());

        //Change the sprite
        ChangeSprite();
    }

    //This only gets run when Happiness has changed
    public void UpdateHappiness(bool iH)
    {
        _isHappy = iH;
        
    }

    IEnumerator Animate()
    {
        yield return new WaitForSeconds(0.7f);
        ChangeSprite();
    }

    //This only gets run when Happiness has changed and interjects the Coroutine
    public void ChangeSpriteHappiness(bool iH)
    {
        _isHappy = iH;

        switch (_isHappy)
        {
            case true:
                if (_lastSprite == idleSprite1S.name)
                {
                    this.GetComponent<SpriteRenderer>().sprite = idleSprite1H;
                }
                else if (_lastSprite == idleSprite2S.name)
                {
                    this.GetComponent<SpriteRenderer>().sprite = idleSprite2H;
                }
                else if (_lastSprite == walkSpriteS.name)
                {
                    this.GetComponent<SpriteRenderer>().sprite = walkSpriteH;
                }
                break;
            case false:
                if (_lastSprite == idleSprite1H.name)
                {
                    this.GetComponent<SpriteRenderer>().sprite = idleSprite1S;
                }
                else if (_lastSprite == idleSprite2H.name)
                {
                    this.GetComponent<SpriteRenderer>().sprite = idleSprite2S;
                }
                else if (_lastSprite == walkSpriteH.name)
                {
                    this.GetComponent<SpriteRenderer>().sprite = walkSpriteS;
                }
                break;
        }

        _lastSprite = this.GetComponent<SpriteRenderer>().sprite.name;
    }

    private void ChangeSprite()
    {
        switch (_curState)
        {
            case CharState.Idle:
                if (_lastSprite == idleSprite1H.name || _lastSprite == idleSprite1S.name)
                {
                    if (_isHappy)
                    {
                        this.GetComponent<SpriteRenderer>().sprite = idleSprite2H;
                    }
                    else
                    {
                        this.GetComponent<SpriteRenderer>().sprite = idleSprite2S;
                    }
                    
                }
                else
                {
                    if (_isHappy)
                    {
                        this.GetComponent<SpriteRenderer>().sprite = idleSprite1H;
                    }
                    else
                    {
                        this.GetComponent<SpriteRenderer>().sprite = idleSprite1S;
                    }
                }
                StartCoroutine(Animate());
                break;
            case CharState.Moving:
                if (_isHappy)
                {
                    this.GetComponent<SpriteRenderer>().sprite = walkSpriteH;
                }
                else
                {
                    this.GetComponent<SpriteRenderer>().sprite = walkSpriteS;
                }
                break;
            case CharState.Pie:
            case CharState.Cake:
                if (_lastSprite == bakingSprite1.name)
                {
                    this.GetComponent<SpriteRenderer>().sprite = bakingSprite2;
                }
                else
                {
                    this.GetComponent<SpriteRenderer>().sprite = bakingSprite1;
                }
                StartCoroutine(Animate());
                break;
        }

        //Set the name of this sprite as the last sprite and return it
        _lastSprite = this.GetComponent<SpriteRenderer>().sprite.name;
        //Debug.Log(_lastSprite);
    }
}


