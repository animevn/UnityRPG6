using UnityEngine;

namespace Script.Core
{
    public class CamFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        
        private void LateUpdate(){
            transform.position = target.position;
        }
    }
}
