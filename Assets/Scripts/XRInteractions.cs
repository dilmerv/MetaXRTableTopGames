using TMPro;
using UnityEngine;

public class XRInteractions : MonoBehaviour
{
    [SerializeField] private TextMeshPro toggleGamePokeLabel;
    
    public void Log(string x) => Debug.Log(x);

    public void ToggleGate()
    {
        bool isGateClosed = GamePortalManager.Instance.ToggleGameArea();
        if (isGateClosed) toggleGamePokeLabel.text = "Open Gate";
        else toggleGamePokeLabel.text = "Close Gate";
    }
}
