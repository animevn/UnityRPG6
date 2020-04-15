using UnityEngine;
using UnityEngine.Playables;

namespace Script.Cinematics
{
    public class CinematicsTrigger : MonoBehaviour
    {
        private bool alreadyTrigger;

        private static bool IsPlayerCollider(Component other)
        {
            return other.gameObject.CompareTag("Player");
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!alreadyTrigger && IsPlayerCollider(other))
            {
                GetComponent<PlayableDirector>().Play();
                alreadyTrigger = true;
            }
        }
    }
}
