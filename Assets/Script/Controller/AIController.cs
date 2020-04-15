using Script.Combat;
using Script.Core;
 using Script.Movement;
 using UnityEngine;

namespace Script.Controller
{
    // ReSharper disable once InconsistentNaming
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 10f;
        [SerializeField] private float suspisionTime = 2f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointTolerance = 1f;
        [SerializeField] private float waypointDwellTime = 3f;
        [Range(0, 1)] [SerializeField] private float patrolSpeedFraction = 0.2f;
        
        private Fighter fighter;
        private Health health;
        private Mover mover;
        private GameObject player;
        private Vector3 guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private int currentWayPointIndex;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
            guardPosition = transform.position;
        }

        private bool InAttackRange()
        {
            return Vector3.Distance(player.transform.position, transform.position) < chaseDistance;
        }

        private void Update()
        {
            if (health.IsDead()) return;
            if (InAttackRange() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspisionTime)
            {
                SuspisionBehaviour();
            }
            else
            {
                // ReSharper disable once Unity.PreferAddressByIdToGraphicsParams
                PatrolBehaviour();
            }
            UpdateTimer();
        }

        private void SuspisionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            // ReSharper disable once Unity.PreferAddressByIdToGraphicsParams
            fighter.Attack(player);
        }

        private void UpdateTimer()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            var nextPostion = guardPosition;
            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWayPoint();
                }

                nextPostion = GetCurrentWaypoint();
            }
            
            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                // ReSharper disable once Unity.PreferAddressByIdToGraphicsParams
                mover.StartMoveAction(nextPostion, patrolSpeedFraction);
            }
            
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWayPointIndex);
        }

        private void CycleWayPoint()
        {
            currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);
        }

        private bool AtWayPoint()
        {
            var distance = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distance < waypointTolerance;
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
