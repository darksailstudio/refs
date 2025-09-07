#nullable enable

using System;
using DarkSail.Resettables;
using UnityEngine;

namespace DarkSail.Refs
{
	[ResetOnExitPlayMode]
	[CreateAssetMenu(menuName = "Shared References/Event")]
	public class SharedEventRef : ScriptableObject, IEventRef
	{
		[SerializeField]
		EventRef eventRef = new EventRef();

		public event Action? Invoked
		{
			add => eventRef.Invoked += value;
			remove => eventRef.Invoked -= value;
		}

		public void Invoke() => eventRef.Invoke();
	}
}
