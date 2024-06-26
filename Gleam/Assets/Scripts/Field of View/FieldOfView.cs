﻿using UnityEngine;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
	public float viewRadius;
	[Range(0,360)] public float viewAngle;
	[SerializeField] private LayerMask targetMask;
 	[SerializeField] private LayerMask obstacleMask;
	[HideInInspector] public List<Collider2D> visibleTargets = new List<Collider2D>();

 	public void FindVisibleTargets(Vector3 point)
	{
		visibleTargets.Clear ();
		Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll (point, viewRadius, targetMask);

		for (int i = 0; i < targetsInViewRadius.Length; i++)
		{
			Transform target = targetsInViewRadius [i].transform;
			Vector3 dirToTarget = (target.position - point).normalized;
			if (Vector3.Angle (transform.up, dirToTarget) < viewAngle / 2)
			{
				float dstToTarget = Vector3.Distance (point, target.position);

				if (!Physics2D.Raycast (point, dirToTarget, dstToTarget, obstacleMask))
				{
					visibleTargets.Add (target.GetComponent<Collider2D>());
				}
			}
		}
	}
	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
	}
}