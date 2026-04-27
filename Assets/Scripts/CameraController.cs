using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float mouseSensitivity = 15f;
    [SerializeField] private float distanceFromPlayer = 5f;
    [SerializeField] private float minPitch = -10f;
    [SerializeField] private float maxPitch = 80f;

    private float pitch = 30f;
    private float yaw = 0f;

    void LateUpdate()
    {
        if (player == null) return;

        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity * Time.deltaTime;
            yaw += mouseDelta.x;
            pitch -= mouseDelta.y;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 position = player.position - (rotation * Vector3.forward * distanceFromPlayer);

        transform.position = position;
        transform.LookAt(player.position);
    }
}