using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WarningText { Happiness, Craig, Dad, Pie, Cake };

public class InterfaceManager : MonoBehaviour {
    public Slider desireBar;
    public Slider dadBar;
    public Slider craigBar;
    public Slider happinessBar;
    public Slider dayBar;

    public Text weekText;

    public GameObject WarningTextBox;
    public GameObject CraigWarning;
    public GameObject DadWarning;
    public GameObject HappinessWarning;
    public GameObject CakeWarning;
    public GameObject PieWarning;

    public Sprite cakeSprite;
    public Sprite pieSprite;
    public GameObject bakingNotice;
    public float noticeMoveDistance;
    public float noticeMoveTime;
    public float noticeFadeTime;

    private bool _showingWarning = false;

    public void UpdateInterface(float desire, float dad, float craig, float happiness, int week)
    {
        desireBar.value = desire;
        dadBar.value = dad;
        craigBar.value = craig;
        happinessBar.value = happiness;

        weekText.text = "Week " + week;
    }

    public void UpdateDayBar(float notch)
    {
        dayBar.value = notch;
    }

    public void HideWarningInterface()
    {
        WarningTextBox.SetActive(false);
        CraigWarning.SetActive(false);
        DadWarning.SetActive(false);
        HappinessWarning.SetActive(false);
        CakeWarning.SetActive(false);
        PieWarning.SetActive(false);
    }

    public void ShowWarning(WarningText warning)
    {
        //Hide everything first
        HideWarningInterface();

        //Set stuff to start showing
        _showingWarning = true;
        WarningTextBox.SetActive(true);

        switch (warning)
        {
            case WarningText.Happiness:
                HappinessWarning.SetActive(true);
                break;
            case WarningText.Craig:
                CraigWarning.SetActive(true);
                break;
            case WarningText.Dad:
                DadWarning.SetActive(true);
                break;
            case WarningText.Cake:
                CakeWarning.SetActive(true);
                break;
            case WarningText.Pie:
                PieWarning.SetActive(true);
                break;
        }

        StartCoroutine(ShowWarning());
    }

    IEnumerator ShowWarning()
    {
        yield return new WaitForSeconds(5f);
        HideWarningInterface();
        _showingWarning = false;
    }

    public void MakeNewItem(bool cake)
    {
        //Create a new object
        GameObject thisMade = Instantiate(bakingNotice, bakingNotice.transform.parent);
        thisMade.transform.position = bakingNotice.transform.position;

        //Set the sprite
        if (cake)
        {
            thisMade.GetComponent<SpriteRenderer>().sprite = cakeSprite;
        }
        else
        {
            thisMade.GetComponent<SpriteRenderer>().sprite = pieSprite;
        }

        //Display the object and start moving it
        thisMade.SetActive(true);
        StartCoroutine(MoveJustMade(thisMade));
    }

    IEnumerator MoveJustMade(GameObject thisMade)
    {
        //Track the time
        float time = 0f;
        //Get the starting position from the actual camera and not the idle position
        Vector3 start = thisMade.transform.localPosition;
        Vector3 target = start;
        target.y += noticeMoveDistance;

        bool fadeOut = false;

        //Move the object while we have time remaining
        while (time < noticeMoveTime)
        {
            //Start fading out
            if(time >= noticeMoveTime/3 && !fadeOut)
            {
                fadeOut = true;
                StartCoroutine(FadeOutJustMade(thisMade));
            }

            time += Time.deltaTime;
            thisMade.transform.localPosition = Vector3.Lerp(start, target, Mathf.SmoothStep(0f, 0.8f, time));
            yield return null;
        }
    }

    IEnumerator FadeOutJustMade(GameObject thisMade)
    {
        //Track the time
        float time = 0f;
        while (time < noticeFadeTime)
        {
            time += Time.deltaTime;
            thisMade.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Mathf.Lerp(1f, 0f, time));
            yield return null;
        }

        Destroy(thisMade);
    }

    public bool IsShowingWarning
    {
        get { return _showingWarning; }
    }
}
