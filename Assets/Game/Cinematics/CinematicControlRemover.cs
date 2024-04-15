using System;
using Game.Control.Player;
using Game.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CinematicControlRemover : MonoBehaviour
    {
        public PlayableDirector director { get; private set; }

        private GameObject _playerGameObject;
        private ActionSchedule _playerSchedule;
        private PlayerController _playerController;
        
        public void DisableControl(PlayableDirector dir)
        {
            if (director == dir)
            {
                _playerSchedule.CancelCurrentActor();
                _playerController.enabled = false;
            }
        }

        public void EnableControl(PlayableDirector dir)
        {
            if (director == dir)
            {
                _playerController.enabled = true;
            }
        }

        private void Awake()
        {
            director = GetComponent<PlayableDirector>();
        }

        private void OnEnable()
        {
            if (director)
            {
                director.played += DisableControl;
                director.stopped += EnableControl;
            }
        }
        
        private void OnDisable()
        {
            if (director)
            {
                director.played -= DisableControl;
                director.stopped -= EnableControl;
            }
        }

        private void Start()
        {
            _playerGameObject = GameObject.FindWithTag("Player");
            _playerSchedule = _playerGameObject.GetComponentInChildren<ActionSchedule>();
            _playerController = _playerGameObject.GetComponentInChildren<PlayerController>();
        }
    }
}
