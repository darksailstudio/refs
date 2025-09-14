#nullable enable

using System;
using DarkSail.Resettables;
using UnityEngine;

namespace DarkSail.Refs
{
	[ResetOnExitPlayMode]
	public abstract class ReadOnlySharedEventRef : ScriptableObject, IReadOnlyEventRef
	{
		[SerializeField]
		protected EventRef eventRef = new EventRef();

		public event Action? Invoked
		{
			add => eventRef.Invoked += value;
			remove => eventRef.Invoked -= value;
		}
	}
}
