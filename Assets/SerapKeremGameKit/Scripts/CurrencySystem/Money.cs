using UnityEngine;

namespace SerapKeremGameKit._CurrencySystem
{
	public class Money : Currency
	{
		public override long Amount
		{
			get => PlayerPrefs.GetInt("Money", 0);
			set => PlayerPrefs.SetInt("Money", (int)value);
		}
	}
}