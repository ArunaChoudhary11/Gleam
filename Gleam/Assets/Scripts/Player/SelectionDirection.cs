using System.Collections.Generic;
using UnityEngine;

public class SelectionDirection : MonoBehaviour
{
    private Vector2 currentSelectionInput;
    private Vector2 lastSelectionInput;
    private Vector2 ScanPoint;
    private float size = 15;
    public Transform Point;
    private List<Transform> Points = new List<Transform>();
    [HideInInspector] public bool canSelect;
    [HideInInspector] public Transform CurrentSelectedEnemy;
	[SerializeField] private LayerMask targetMask;
 	[SerializeField] private LayerMask obstacleMask;
    private Collider2D ClosestPoint;
    void Start()
    {
        CurrentSelectedEnemy = transform;
    }
    void Update()
    {
        if(canSelect == true)
        {
            GetPoint();
            if(CurrentSelectedEnemy != null) Point.position = CurrentSelectedEnemy.position;
            else Point.position = transform.position;
        }
    }
    private void GetPoint()
    {
        currentSelectionInput = new (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if(currentSelectionInput != lastSelectionInput)
        {
            lastSelectionInput = currentSelectionInput;

            if(currentSelectionInput == Vector2.zero) return;

            if(CurrentSelectedEnemy == null) CurrentSelectedEnemy = transform;
            ScanPoint = (Vector2) CurrentSelectedEnemy.position + (currentSelectionInput.normalized * size / 2);

            Points = VisibleTargets();
            CurrentSelectedEnemy = GetClosestTransform();
        }
    }
    private Vector2 ScanSize()
    {
        float sizeX = Mathf.Abs(currentSelectionInput.x);
        float sizeY = Mathf.Abs(currentSelectionInput.y);

        if(!(sizeX == 1 && sizeY == 1))
        {
            if(sizeX == 1 && sizeY == 0)
            {
                sizeX = 1;
                sizeY = 2;
            }
            else if(sizeX == 0 && sizeY == 1)
            {   
                sizeX = 2;
                sizeY = 1;            
            }
        }
        
        return new Vector2(sizeX, sizeY);
    }
    private Transform GetClosestTransform()
    {
        if(Points.Count == 0)
        {
            if(ClosestPoint == null) return transform;
            return ClosestPoint.transform;
        }

        ClosestPoint = null;

        for(int i = 0; i < Points.Count; i++)
        {
            if(ClosestPoint == null)
            {
                ClosestPoint = Points[i].GetComponent<Collider2D>();
            }

            float ShortestDistance = Vector2.Distance(CurrentSelectedEnemy.position, Points[i].transform.position);
            float CurrentDistance = Vector2.Distance(CurrentSelectedEnemy.position, ClosestPoint.transform.position);

            if(ShortestDistance <=  CurrentDistance)
            {
                ClosestPoint = Points[i].GetComponent<Collider2D>();
            }
        }

        return ClosestPoint.transform;
    }
 	public List<Transform> VisibleTargets()
	{
        List<Transform> points = new();

		Collider2D[] targetsInViewRadius = Physics2D.OverlapBoxAll(ScanPoint, ScanSize() * size, 0, targetMask);

		for (int i = 0; i < targetsInViewRadius.Length; i++)
		{
			Transform target = targetsInViewRadius[i].transform;

			Vector3 dirToTarget = (target.position - transform.position).normalized;
			float dstToTarget = Vector3.Distance (transform.position, target.position);

			if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
			{
				if(target != CurrentSelectedEnemy) points.Add (target);
			}
		}

        return points;
	}
}