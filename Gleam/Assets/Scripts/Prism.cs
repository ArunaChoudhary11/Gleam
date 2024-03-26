using UnityEngine;

public class Prism : MonoBehaviour
{
    public enum PrismType { HORIZONTAL, VERTICAL}
    public PrismType prismType;
    public Transform IncidentRay;
    public float DeviationAngle;
    public float AngleMultiplier;
    public LayerMask prismMask;
    public float range;
    void Update()
    {
        Debug.DrawRay(IncidentRay.position, IncidentRay.right * range, Color.blue);
        RaycastHit2D incidentHit = Physics2D.Raycast(IncidentRay.position, IncidentRay.right, range, prismMask);

        if(incidentHit.collider != null)
        {
            Debug.DrawRay(incidentHit.point, incidentHit.normal * 2, Color.green);
            range = Vector2.Distance(IncidentRay.position, incidentHit.point);

            Vector2 EmergentAngle = Quaternion.AngleAxis(-DeviationAngle * AngleMultiplier, Vector3.forward) * IncidentRay.right;

            float distance = 0;
            
            if(prismType == PrismType.HORIZONTAL) distance = Vector2.Distance(new Vector2(incidentHit.collider.bounds.center.x, incidentHit.point.y), incidentHit.point);
            else distance = Vector2.Distance(new Vector2(incidentHit.point.x, incidentHit.collider.bounds.center.y), incidentHit.point);

            Vector2 emergentPoint = new Vector2();

            if(prismType == PrismType.HORIZONTAL) emergentPoint = new Vector2(incidentHit.collider.bounds.center.x, incidentHit.point.y) + RayDirection(incidentHit.normal) * distance;
            else emergentPoint = new Vector2(incidentHit.point.x, incidentHit.collider.bounds.center.y) + RayDirection(incidentHit.normal) * distance;

            Debug.DrawRay(emergentPoint, EmergentAngle * 10, Color.black);
            Vector2 emergentNormalPoint = (Vector2) incidentHit.collider.bounds.center + RayDirection(incidentHit.normal) * 2;

            RaycastHit2D emergentHit = Physics2D.Raycast(emergentNormalPoint, (Vector2) incidentHit.collider.bounds.center - emergentNormalPoint, range, prismMask);

            if(emergentHit.collider != null)
            {
                Debug.DrawRay(emergentPoint, emergentHit.normal * 2, Color.green);
            }
        }
        else
        {
            range = 10;
        }
    }
    private Vector2 RayDirection(Vector2 normal)
    {
        float direction = -Mathf.RoundToInt(Vector2.SignedAngle(transform.up, normal));

        Vector2 directionVector = new();

        if (direction < 0)
        {
            direction += 360;
        }

        switch (direction)
        {
            case <= 60:
                direction = 0;
                directionVector = new(0, -1);
            break;

            case <= 150:
                direction = 1;
                directionVector = new(-1, 0);
            break;

            case <= 240:
                direction = 2;
                directionVector = new(0, 1);
            break;

            case <= 330:
                direction = 3;
                directionVector = new(1, 0);
            break;

            default:
                direction = 0;
                directionVector = new(0, -1);
            break;
        }

        return directionVector;
    }
}