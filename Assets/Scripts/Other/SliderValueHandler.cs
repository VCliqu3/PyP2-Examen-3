using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderValueText;

    private void Awake()
    {
        InitializeSliderListeners();
    }

    private void Start()
    {
        UpdateValue(slider.value);
    }

    private void InitializeSliderListeners()
    {
        slider.onValueChanged.AddListener(UpdateValue);
    }

    private void UpdateValue(float value)
    {
        sliderValueText.text = Mathf.RoundToInt(value).ToString();
    }


}
