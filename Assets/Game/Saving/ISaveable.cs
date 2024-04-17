namespace Game.Saving
{
    public interface ISaveable
    {
        public object CaptureState();
        
        void RestoreState(object state);
    }
}