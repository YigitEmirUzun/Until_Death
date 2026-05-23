using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    public RectTransform selector;
    public float distance = 2;

    void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected != null)
        {
            RectTransform buttonRect =
                selected.GetComponent<RectTransform>();

            selector.position = new Vector3(
                buttonRect.position.x - distance,
                buttonRect.position.y,
                buttonRect.position.z
            );
        }
    }
}
