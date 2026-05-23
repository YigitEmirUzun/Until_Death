using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Target")]
    public Transform cameraTransform;

    [Header("Parallax Strength")]
    [Range(0f, 1f)]
    public float parallaxEffectX = 0.5f;

    [Range(0f, 1f)]
    public float parallaxEffectY = 0.5f;

    private Vector3 lastCameraPosition;

    private void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        lastCameraPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        transform.position += new Vector3(
            deltaMovement.x * parallaxEffectX,
            deltaMovement.y * parallaxEffectY,
            0f
        );

        lastCameraPosition = cameraTransform.position;
    }
}
