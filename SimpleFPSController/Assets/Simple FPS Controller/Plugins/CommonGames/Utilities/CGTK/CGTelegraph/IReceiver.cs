namespace CommonGames.Utilities.CGTK
{
    public interface IReceiver<in T> : IReceiver
    {
        void HandleSignal(T arg);
    }
    
    public interface IReceiver { }
}