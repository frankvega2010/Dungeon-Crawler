using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StaffBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxCharge(int charge)
    {
        slider.maxValue = charge;
        slider.value = charge;

        fill.color = gradient.Evaluate(1f);
    }


    public void SetCharge(float charge)
    {
        slider.value = charge;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }


}
