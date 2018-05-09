using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour {
    public Slider cakeBar;
    public Slider pieBar;
    public Slider dadBar;
    public Slider craigBar;
    public Slider happinessBar;

    public Text weekText;

    public GameObject WarningTextBox;
    public GameObject CraigWarning;
    public GameObject DadWarning;
    public GameObject HappinessWarning;

    private bool _showingWarning;

    public void UpdateInterface(float pie, float dad, float craig, float happiness, int week)
    {
        pieBar.value = pie;
        cakeBar.value = 1f - pie;
        dadBar.value = dad;
        craigBar.value = craig;
        happinessBar.value = happiness;

        weekText.text = "Week " + week;
    }

    public void HideWarningInterface()
    {
        _showingWarning = false;

        WarningTextBox.SetActive(false);
        CraigWarning.SetActive(false);
        DadWarning.SetActive(false);
        HappinessWarning.SetActive(false);

        StartCoroutine(ShowWarning());
    }

    public void ShowCraigWarning()
    {
        _showingWarning = true;

        WarningTextBox.SetActive(true);
        CraigWarning.SetActive(true);
        DadWarning.SetActive(false);
        HappinessWarning.SetActive(false);

        StartCoroutine(ShowWarning());
    }

    public void ShowDadWarning()
    {
        _showingWarning = true;

        WarningTextBox.SetActive(true);
        CraigWarning.SetActive(false);
        DadWarning.SetActive(true);
        HappinessWarning.SetActive(false);

        StartCoroutine(ShowWarning());
    }

    public void ShowHappinessWarning()
    {
        _showingWarning = true;

        WarningTextBox.SetActive(true);
        CraigWarning.SetActive(false);
        DadWarning.SetActive(false);
        HappinessWarning.SetActive(true);

        StartCoroutine(ShowWarning());
    }

    IEnumerator ShowWarning()
    {
        yield return new WaitForSeconds(5f);
        HideWarningInterface();
    }

    public bool IsShowingWarning
    {
        get { return _showingWarning; }
    }
}
