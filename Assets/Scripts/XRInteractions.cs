using System;
using TMPro;
using UnityEngine;

public class XRInteractions : MonoBehaviour
{
    [SerializeField] private TextMeshPro toggleGamePokeLabel;

    [SerializeField] private Transform activeSprite;
    [SerializeField] private Transform inactiveSprite;
    
    public void Log(string x) => Debug.Log(x);

    private void Awake()
    {
        activeSprite.gameObject.SetActive(true);
        inactiveSprite.gameObject.SetActive(false);
    }

    public void ToggleGate()
    {
        bool isGateClosed = GamePortalManager.Instance.ToggleGameArea();
        if (isGateClosed)
        {
            toggleGamePokeLabel.text = "Open Gate";
            activeSprite.gameObject.SetActive(true);
            inactiveSprite.gameObject.SetActive(false);
        }
        else
        {
            toggleGamePokeLabel.text = "Close Gate";
            activeSprite.gameObject.SetActive(false);
            inactiveSprite.gameObject.SetActive(true);
        }
    }
}
