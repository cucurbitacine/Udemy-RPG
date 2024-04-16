using Game.Core;
using UnityEngine;

namespace Game.Control.Player
{
    public class ControlRemover : MonoBehaviour
    {
        public GameObject playerGameObject { get; private set; }
        public ActionSchedule playerSchedule { get; private set; }
        public PlayerController playerController { get; private set; }
        
        public void DisableControl()
        {
            playerSchedule.CancelCurrentActor();
            playerController.enabled = false;
        }

        public void EnableControl()
        {
            playerController.enabled = true;
        }
        
        protected virtual void Awake()
        {
            playerGameObject = GameObject.FindWithTag("Player");
            playerSchedule = playerGameObject.GetComponentInChildren<ActionSchedule>();
            playerController = playerGameObject.GetComponentInChildren<PlayerController>();
        }
    }
}