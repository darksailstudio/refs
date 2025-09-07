#nullable enable

using System;
using DarkSail.Resettables;
using UnityEngine;

namespace DarkSail.Refs
{
	[ResetOnExitPlayMode]
	public abstract class SharedRef<T> : ScriptableObject, IRef<T>
	{
		[SerializeField]
		Ref<T> valueRef = new Ref<T>(default!);

		public T Value
		{
			get => valueRef.Value;
			set => valueRef.Value = value;
		}

		public event Action<T>? Changed
		{
			add => valueRef.Changed += value;
			remove => valueRef.Changed -= value;
		}
	}
}
