using Script.Controller;
using Script.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace Script.Cinematics
{
    public class CinematicsControlRemover : MonoBehaviour
    {
        private GameObject player;
        
        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void DisableControl(PlayableDirector pd)
        {
            player.GetComponent<ActionScheduler>().CancelAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnableControl(PlayableDirector pd)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
        
    }
}
