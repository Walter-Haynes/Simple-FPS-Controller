namespace CommonGames.Utilities
{
	/// <summary> Singleton that persists across multiple scenes </summary>
	public class PersistentSingleton<T> : Singleton<T> where T : Singleton<T>
	{
		protected override void OnEnable()
		{
			base.OnEnable();
			DontDestroyOnLoad(gameObject);
		}
	}
}