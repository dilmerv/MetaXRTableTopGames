using LearnXR.Core;
using UnityEngine;

public class PalmMenuAxis : Singleton<PalmMenuAxis>
{
    [SerializeField] private Transform palmMenu;
    
    public void TogglePalmMenuVisibility() => palmMenu.gameObject.SetActive(!palmMenu.gameObject.activeSelf);

    public void ToggleAxisHandler()
    {
        var handler = AxisHandler.Instance;
        var axis = handler.gameObject;
        axis.SetActive(!axis.activeSelf);
    }
}
