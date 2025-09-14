#nullable enable

using UnityEngine;

namespace DarkSail.Refs
{
	[CreateAssetMenu(menuName = "Shared References/Event")]
	public class SharedEventRef : ReadOnlySharedEventRef, IEventRef
	{
		public void Invoke() => eventRef.Invoke();
	}
}
