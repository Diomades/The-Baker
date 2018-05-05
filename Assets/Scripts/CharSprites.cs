using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharState { Idle, Moving, Pie, Cake };

public class CharSprites : MonoBehaviour {
    public Sprite idleSprite1;
    public Sprite idleSprite2;
    public Sprite walkSprite;
    public Sprite bakingSprite1;
    public Sprite bakingSprite2;

    private CharState _curState;
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

    IEnumerator Animate()
    {
        yield return new WaitForSeconds(0.7f);
        ChangeSprite();
    }

    private void ChangeSprite()
    {
        switch (_curState)
        {
            case CharState.Idle:
                if (_lastSprite == idleSprite1.name)
                {
                    this.GetComponent<SpriteRenderer>().sprite = idleSprite2;
                }
                else
                {
                    this.GetComponent<SpriteRenderer>().sprite = idleSprite1;
                }
                StartCoroutine(Animate());
                break;
            case CharState.Moving:
                this.GetComponent<SpriteRenderer>().sprite = walkSprite;
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


