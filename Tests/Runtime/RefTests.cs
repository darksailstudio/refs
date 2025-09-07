using System;
using NUnit.Framework;
using UnityEngine;

namespace DarkSail.Refs.Tests
{
	public class RefTests
	{
		static Func<IRef<int>>[] RefFactories =
		{
			() => new Ref<int>(0),
			() => ScriptableObject.CreateInstance<SharedIntRef>(),
		};

		[Test]
		public void Constructor_SetsInitialValue()
		{
			var intRef = new Ref<int>(100);

			Assert.That(intRef.Value, Is.EqualTo(100));
		}

		[Test]
		public void ValueProperty_SetsAndGetsValue(
			[ValueSource(nameof(RefFactories))] Func<IRef<int>> refFactory
		)
		{
			var intRef = refFactory();

			intRef.Value = 100;
			Assert.That(intRef.Value, Is.EqualTo(100));
		}

		[Test]
		public void ChangedEvent_InvokesIfValueIsChanged(
			[ValueSource(nameof(RefFactories))] Func<IRef<int>> refFactory
		)
		{
			var intRef = refFactory();
			var callbackCount = 0;

			intRef.Changed += value =>
			{
				switch (callbackCount)
				{
					case 0:
						Assert.That(value, Is.EqualTo(100));
						break;

					case 1:
						Assert.That(value, Is.EqualTo(200));
						break;

					default:
						Assert.Fail();
						break;
				}

				callbackCount++;
			};

			intRef.Value = 100;
			Assert.That(callbackCount, Is.EqualTo(1));

			intRef.Value = 200;
			Assert.That(callbackCount, Is.EqualTo(2));

			intRef.Value = 200;
			Assert.That(callbackCount, Is.EqualTo(2));
		}
	}
}
