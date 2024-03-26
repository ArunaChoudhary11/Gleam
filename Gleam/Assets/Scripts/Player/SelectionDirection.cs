using System.Collections.Generic;
using UnityEngine;

public class SelectionDirection : MonoBehaviour
{
    private Vector2 currentSelectionInput;
    private Vector2 lastSelectionInput;
    private Vector2 ScanPoint;
    private float size = 15;
    [SerializeField] private Transform Point;
    private List<Transform> Points = new List<Transform>();
    public bool canSelect;
    [HideInInspector] public Transform CurrentSelectedEnemy; // The Output Needed
	[SerializeField] private LayerMask targetMask;
 	[SerializeField] private LayerMask obstacleMask;

    void Start()
    {
        CurrentSelectedEnemy = transform;
    }
    void Update()
    {
        currentSelectionInput = new (Mathf.RoundToInt(Input.GetAxis("Mouse X")), Mathf.RoundToInt(Input.GetAxis("Mouse Y")));

        if(Input.GetMouseButtonDown(2))
        {
            FindVisibleTargets();
            CurrentSelectedEnemy = GetClosestEnemy();
            if(CurrentSelectedEnemy == null) CurrentSelectedEnemy = transform;
            canSelect = !canSelect;
        }

        if(canSelect == true)
        {
            GetPoint();
            if(CurrentSelectedEnemy != null) Point.position = CurrentSelectedEnemy.position;
            else Point.position = transform.position;
        }
    }
    private Transform GetClosestEnemy()
    {
        Transform ClosestEnemy = null;

        if(Points.Count == 0)
        {
            if(CurrentSelectedEnemy != null) return CurrentSelectedEnemy;

            return null;
        }

        for(int i = 0; i < Points.Count; i++)
        {
            if(ClosestEnemy == null)
            {
                ClosestEnemy = Points[i];
            }

            float ShortestDistance = Vector2.Distance(transform.position, Points[i].transform.position);
            float CurrentDistance = Vector2.Distance(transform.position, ClosestEnemy.transform.position);

            if(ShortestDistance <=  CurrentDistance)
            {
                ClosestEnemy = Points[i];
            }
        }

        return ClosestEnemy.transform;
    }
    private void GetPoint()
    {        
        if(currentSelectionInput != lastSelectionInput)
        {
            lastSelectionInput = currentSelectionInput;

            if(CurrentSelectedEnemy != null)
            {
                if(currentSelectionInput == Vector2.zero) return;
            }

            if(CurrentSelectedEnemy == null) CurrentSelectedEnemy = transform;
            ScanPoint = (Vector2) CurrentSelectedEnemy.position + (currentSelectionInput.normalized * size / 2);

            Points = VisibleTargets();
            CurrentSelectedEnemy = GetClosestEnemy();
        }

        if(CurrentSelectedEnemy == null) canSelect = false;
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
                sizeY = 1;
            }
            else if(sizeX == 0 && sizeY == 1)
            {   
                sizeX = 1;
                sizeY = 1;            
            }
        }
        
        return new Vector2(sizeX, sizeY);
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
    public void FindVisibleTargets()
	{
		Points.Clear();
		Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, size, targetMask);

		for (int i = 0; i < targetsInViewRadius.Length; i++)
		{
			Transform target = targetsInViewRadius[i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;

            float dstToTarget = Vector3.Distance (transform.position, target.position);

            if (!Physics2D.Raycast (transform.position, dirToTarget, dstToTarget, obstacleMask))
            {
                Points.Add (target.GetComponent<Collider2D>().transform);
            }
		}
	}
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(ScanPoint, ScanSize() * size);
    }
}