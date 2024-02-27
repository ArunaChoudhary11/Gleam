using System.Collections.Generic;
using UnityEngine;

public class SpearEnemies : MonoBehaviour
{
    /*
        Attack
        Defence
        Movement
    */
    private List<Vector2> points = new List<Vector2>();
    [SerializeField] private float range = 3f;
    private float pointsTimer;
    [SerializeField] private float speed;
    [SerializeField] private int currentPointIndex;
    void Start()
    {
        Move();
    }
    void Update()
    {
        if(pointsTimer >= 5)
        {
            Move();   
        }

        pointsTimer += Time.deltaTime;

        if(points.Count > 0)
        {
            float distance = Vector2.Distance(transform.position, points[currentPointIndex]);

            if(distance <= 0.2f)
            {
                currentPointIndex = Random.Range(0, points.Count);
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[currentPointIndex], speed * Time.deltaTime);
    }
    private void Move()
    {
        pointsTimer = 0;
        points.Clear();

        for(int i = 0; i < 4; i++)
        {
            Vector2 point = PointsArea(transform.position, Random.Range(0, range));
            Debug.DrawRay(point, Vector2.up, Color.white, 5);
            points.Add(point);
        }

        currentPointIndex = Random.Range(0, points.Count);
    }
    private Vector2 PointsArea(Vector3 center, float radius){
		
        float ang = Random.value * 360;
		Vector2 pos;
		pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
		pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		return pos;
	}
    private void Attack()
    {
        // Move towards the player to hit.
    }
    private void Defence()
    {
        /*
            No damage when holding on defence option.
            after some time will attack.
        */
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}