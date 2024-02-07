using UnityEngine;

public class BPSetChildrenInvisible : MonoBehaviour
{
    void Start()
    {
        SetAllChildrenInvisible(transform);
    }

    void SetAllChildrenInvisible(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // Set the child GameObject to be invisible
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
            }

            // Recursively set children of this child to be invisible
            SetAllChildrenInvisible(child);
        }
    }
}
