using UnityEngine;

public class PhotonShield : MonoBehaviour
{
    public float Angle;
    public float range;
    

    void Start()
    {
        
    }

     
    void Update()
    {
        Shield(transform.up);
    }

    public  void Shield(Vector2 hitNormal)
    {
        Vector3 ray1 = Quaternion.AngleAxis(Angle / 2, Vector3.forward) * hitNormal;
        Vector3 ray2 = Quaternion.AngleAxis(-Angle / 2, Vector3.forward) * hitNormal;
        Debug.DrawRay(transform.position, ray1 * range, Color.red);
        Debug.DrawRay(transform.position, ray2 * range, Color.red);
    }
}