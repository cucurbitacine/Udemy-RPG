using Game.Control.Player;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CinematicControlRemover : ControlRemover
    {
        public PlayableDirector director { get; private set; }
        
        public void DisableControl(PlayableDirector dir)
        {
            if (director == dir)
            {
                DisableControl();
            }
        }

        public void EnableControl(PlayableDirector dir)
        {
            if (director == dir)
            {
                EnableControl();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            
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
    }
}
