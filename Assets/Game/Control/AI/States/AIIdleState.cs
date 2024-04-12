namespace Game.Control.AI.States
{
    public class AIIdleState : AIState
    {
        public override bool Process(AIController ai)
        {
            ai.schedule.CancelCurrentActor();
            return true;
        }
    }
}