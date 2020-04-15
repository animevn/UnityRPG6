using UnityEngine;
using UnityEngine.Playables;

namespace Script.Cinematics
{
    public class CinematicsTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            GetComponent<PlayableDirector>().Play();
        }
    }
}
