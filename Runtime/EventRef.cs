#nullable enable

using System;

namespace DarkSail.Refs
{
	public interface IReadOnlyEventRef
	{
		event Action? Invoked;
	}

	public interface IEventRef : IReadOnlyEventRef
	{
		void Invoke();
	}

	[Serializable]
	public class EventRef : IEventRef
	{
		public event Action? Invoked;

		public void Invoke() => Invoked?.Invoke();
	}
}
