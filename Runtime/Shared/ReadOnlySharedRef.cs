#nullable enable

using System;
using DarkSail.Resettables;
using UnityEngine;

namespace DarkSail.Refs
{
	[ResetOnExitPlayMode]
	public abstract class ReadOnlySharedRef<T> : ScriptableObject, IReadOnlyRef<T>
	{
		[SerializeField]
		protected Ref<T> valueRef = new Ref<T>(default!);

		public T Value => valueRef.Value;

		public event Action<T>? Changed
		{
			add => valueRef.Changed += value;
			remove => valueRef.Changed -= value;
		}
	}
}
