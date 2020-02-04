namespace CommonGames.Utilities
{
	/// <summary> <see cref="EnsuredSingleton"/> that persists across multiple scenes </summary>
	public class PersistentEnsuredSingleton<T> : EnsuredSingleton<T> where T : EnsuredSingleton<T>
	{
		protected override void OnEnable()
		{
			base.OnEnable();
			DontDestroyOnLoad(gameObject);
		}
	}
}