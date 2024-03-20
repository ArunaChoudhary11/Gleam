using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TurretLaser : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 2f;
    private Vector2 direction;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        Destroy(gameObject, 3);
    }
    void FixedUpdate()
    {
        rb.velocity =  speed * 100 * Time.deltaTime * direction;
    }
   public  void PlayerDirection(Transform player)
    {
        direction = player.position - transform.position;
    }
}