using UnityEngine;

public class PickUpRotator : MonoBehaviour
{
    [SerializeField] private Vector3 rotationVelocity = new Vector3(30f, 45f, 15f);

    void Update()
    {
        transform.Rotate(rotationVelocity * Time.deltaTime);
    }
}
