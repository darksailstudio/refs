#nullable enable

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSail.Refs
{
	public interface IReadOnlyRef<T>
	{
		event Action<T>? Changed;
		T Value { get; }
	}

	public interface IRef<T> : IReadOnlyRef<T>
	{
		new T Value { get; set; }
		T IReadOnlyRef<T>.Value => Value;
	}

	[Serializable]
	public class Ref<T> : IRef<T>
	{
		public event Action<T>? Changed;

		[Serializable]
		class Gate { }

		[SerializeField, HideInInspector]
		Gate gate = new Gate();

		[SerializeField]
		T value;

		public Ref(T initialValue) => value = initialValue;

		public T Value
		{
			get
			{
				lock (gate)
					return value;
			}
			set
			{
				lock (gate)
				{
					if (EqualityComparer<T>.Default.Equals(this.value, value))
						return;

					this.value = value;
				}

				Changed?.Invoke(value);
			}
		}
	}
}
