using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * HealthBar Class
 * 
 * Usage:
 * `SetSliderValue` - Immediately sets a slider value.
 * `SetSliderValueGradually` - Makes the value gradually glide to the desired one.
 * `changeRate` - The rate of the glide.
 */
public class HealthBar : MonoBehaviour
{
    private const string SliderTag = "HealthBar";

    private Slider healthBar;

    private float targetValue = 100f;
    public float changeRate = 0.5f;

    public void Start()
    {
        healthBar =  GetComponent<Slider>();
    }

    public void FixedUpdate()
    {
        if (targetValue == healthBar.value)
            return;
        
        if (targetValue < healthBar.value)
            healthBar.value -= changeRate;
        else
            healthBar.value += changeRate;
    }

    public void SetSliderValue(float value)
    {
        healthBar.value = value;
    }

    public void SetSliderValueGradually(float value)
    {
        targetValue = value;
    }

}
