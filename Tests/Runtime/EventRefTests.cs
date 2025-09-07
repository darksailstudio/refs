using System;
using NUnit.Framework;
using UnityEngine;

namespace DarkSail.Refs.Tests
{
	public class EventRefTests
	{
		static Func<IEventRef>[] EventRefFactories =
		{
			() => new EventRef(),
			() => ScriptableObject.CreateInstance<SharedEventRef>(),
		};

		[Test]
		public void Invoke_InvokesEvent(
			[ValueSource(nameof(EventRefFactories))] Func<IEventRef> eventRefFactory
		)
		{
			var eventRef = eventRefFactory();
			var callbackCount = 0;

			eventRef.Invoked += () => callbackCount++;

			eventRef.Invoke();
			Assert.That(callbackCount, Is.EqualTo(1));

			eventRef.Invoke();
			Assert.That(callbackCount, Is.EqualTo(2));
		}
	}
}
