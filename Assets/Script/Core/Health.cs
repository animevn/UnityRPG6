using UnityEngine;

namespace Script.Core
{
    public class Health:MonoBehaviour
    {
        [SerializeField] private float health = 100f;

        private bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }


        public void TakeDamage(float damage)
        {
            health = Mathf.Max((health - damage), 0);
            if ((int)health == 0)
            {
                Die();
            }
        }
        
        private void Die()
        {
            if (isDead) return;
            isDead = true;
            // ReSharper disable once Unity.PreferAddressByIdToGraphicsParams
            GetComponent<Animator>().SetTrigger("death");
            GetComponent<ActionScheduler>().CancelAction();
        }

        public float GetHealth()
        {
            return health;
        }
    }
}