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

    public void UpdateInterface(float pie, float dad, float craig, float happiness, int week)
    {
        pieBar.value = pie;
        cakeBar.value = 1f - pie;
        dadBar.value = dad;
        craigBar.value = craig;
        happinessBar.value = happiness;

        weekText.text = "Week " + week;
    }
}
