using System.Collections.Generic;
using LearnXR.Core;
using UnityEngine;
using Utilities;

public class GameTableManager : Singleton<GameTableManager>
{
    [SerializeField] private GameObject occluderPrefab;

    [SerializeField] private float[] scaleFactors;

    private List<Vector3> scaleOptions = new();
    
    private ScaleObjectOverTime scaleObjectOverTime;

    private int currentScaleIndex = -1;
    
    private Vector3 initialScale;

    public float[] ScaleFactors => scaleFactors;
    
    public void Start()
    {
        initialScale = transform.localScale;
        PopulateScaleOptions();
        Instantiate(occluderPrefab, transform.GetChild(0), false);
        GamePortalManager.Instance.Setup();
    }

    private void PopulateScaleOptions()
    {
        for (int i = 0; i < scaleFactors.Length; i++)
        {
            scaleOptions.Add(
                new Vector3(
                    initialScale.x / scaleFactors[i], 
                    initialScale.y / scaleFactors[i], 
                    initialScale.z / scaleFactors[i]));
        }
    }

    public int NextScaleOption()
    {
        if (scaleObjectOverTime.IsTransitionInProgress) return currentScaleIndex;
        
        Vector3 prevScaleOption;
        
        if (currentScaleIndex == -1)
            prevScaleOption = initialScale;
        else
            prevScaleOption = scaleOptions[currentScaleIndex];
        
        currentScaleIndex++;
        if (currentScaleIndex >= scaleOptions.Count)
        {
            scaleObjectOverTime.Scale(prevScaleOption, initialScale, 0, 1.0f);
            currentScaleIndex = -1;
        }
        else
        {
            var newScaleOption = scaleOptions[currentScaleIndex];
            scaleObjectOverTime.Scale(prevScaleOption, newScaleOption, 0, 1.0f);    
        }

        return currentScaleIndex;
    }
}
