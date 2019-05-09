using UnityEngine;

public class PortraitCameraController : MonoBehaviour
{
    private Transform Subject;

    void Start()
    {
        Subject = transform.parent;
    }

    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
