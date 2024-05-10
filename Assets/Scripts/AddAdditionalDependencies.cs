using UnityEngine;

public class AddAdditionalDependencies : MonoBehaviour
{
    [SerializeField] private GameObject occluderPrefab;

    public void Start()
    {
        Instantiate(occluderPrefab, transform, false);
        GamePortalManager.Instance.Setup();
    }
}
