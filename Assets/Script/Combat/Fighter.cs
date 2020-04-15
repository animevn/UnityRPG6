using Script.Core;
using Script.Movement;
using UnityEngine;

namespace Script.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 1.5f;
        [SerializeField] private float timeBetweenAttack = 1f;
        [SerializeField] private float healthPerHit = 10f;
        // ReSharper disable once InconsistentNaming
        private Health target;
        private float timeSinceLastAttack = 0;
        
        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (target.IsDead()) return;
        
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }

        }

        private void TriggerAttack()
        {
            // ReSharper disable once Unity.PreferAddressByIdToGraphicsParams
            GetComponent<Animator>().ResetTrigger("stopAttack");
            // ReSharper disable once Unity.PreferAddressByIdToGraphicsParams
            GetComponent<Animator>().SetTrigger("attack");
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (!(timeSinceLastAttack > timeBetweenAttack)) return;
            TriggerAttack();
            timeSinceLastAttack = 0;
        }
        
        private void Hit()
        {
            if (target != null)
            {
                var enemyHealth = target.GetComponent<Health>();
                enemyHealth.TakeDamage(healthPerHit);
                if ((int)enemyHealth.GetHealth() == 0) Cancel();
            }
            
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            // ReSharper disable once Unity.PreferAddressByIdToGraphicsParams
            GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
            GetComponent<Mover>().Cancel();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            var targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

       
    }
}
