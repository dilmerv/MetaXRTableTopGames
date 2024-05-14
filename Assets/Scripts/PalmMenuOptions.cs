using System;
using LearnXR.Core;
using TMPro;
using UnityEngine;

public class PalmMenuOptions : Singleton<PalmMenuOptions>
{
    [SerializeField] private Transform palmMenu;

    [SerializeField] private TextMeshPro palmScaleLabel;

    private void Awake() => palmMenu.gameObject.SetActive(false);

    public void TogglePalmMenuVisibility() => palmMenu.gameObject.SetActive(!palmMenu.gameObject.activeSelf);

    public void ToggleAxisHandler()
    {
        var handler = AxisHandler.Instance;
        var axis = handler.gameObject;
        axis.SetActive(!axis.activeSelf);
    }

    public void ScaleCycle()
    {
        var gameTable = GameTableManager.Instance;
        var scaleOption = gameTable.NextScaleOption();
        if (scaleOption == -1)
            palmScaleLabel.text = $"Scale\nCycle(1x)";
        else
            palmScaleLabel.text = $"Scale\nCycle(1/{gameTable.ScaleFactors[scaleOption]})";
    } 
}
