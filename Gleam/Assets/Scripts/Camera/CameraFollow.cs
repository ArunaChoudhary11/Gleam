using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform player;
    [SerializeField] private float sensitivity;
    void Start()
    {
        
    }
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + offset, sensitivity * Time.deltaTime);
    }
}