namespace CommonGames.Utilities
{
    ///<summary> Multiton that persists across multiple scenes.</summary>
    public class PersistentMultiton<T> : Multiton<T> where T : Multiton<T>
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            DontDestroyOnLoad(gameObject);
        }
    }
}