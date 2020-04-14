using System;
using UnityEngine;

namespace Script.Controller
{
    public class PatrolPath: MonoBehaviour
    {
        [SerializeField] private float waypointRadius = 0.2f;
        
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(GetWaypoint(i), waypointRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextIndex(i)));
            }
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
        
        public int GetNextIndex(int i)
        {
            if (i < transform.childCount - 1) return i + 1;
            return 0;
        }
    }
}