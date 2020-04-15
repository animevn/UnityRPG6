using System;
using Script.Combat;
using Script.Core;
using Script.Movement;
using UnityEngine;

namespace Script.Controller
{
    
    public class PlayerController:MonoBehaviour
    {
        private Health health;

        private void Start()
        {
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (health.IsDead()) return;
            if (InteractWithCombat()) return;
            // ReSharper disable once RedundantJumpStatement
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            // ReSharper disable once Unity.PreferNonAllocApi
            var ray = GetMouseRay();
            // ReSharper disable once Unity.PreferNonAllocApi
            var hits = Physics.RaycastAll(ray);
            foreach (var hit in hits)
            {
                var target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;
                
                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue;
                }

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            var ray = GetMouseRay();
            var hasHit = Physics.Raycast(ray, out var hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
//                Debug.DrawRay(ray.origin, ray.direction * 1000);
                return true;
            }
            return false;
        }

        private Ray GetMouseRay()
        {
            // ReSharper disable once PossibleNullReferenceException
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}