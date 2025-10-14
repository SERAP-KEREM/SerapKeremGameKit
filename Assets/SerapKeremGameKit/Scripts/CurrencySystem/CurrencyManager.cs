namespace SerapKeremGameKit._CurrencySystem
{
	public static class CurrencyManager
	{
		public static readonly Money Money = new Money();

		static CurrencyManager()
		{
			Money.Init();
		}
	}
}