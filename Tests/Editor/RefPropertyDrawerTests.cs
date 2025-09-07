using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkSail.Refs.Editor.Tests
{
	public class RefPropertyDrawerTests : PropertyDrawerTests
	{
		[Test]
		public void InputInvokesChangedEvent()
		{
			var mock = ScriptableObject.CreateInstance<SharedIntRef>();
			var input = GetPropertyField(mock, "valueRef").Q<BaseField<int>>();
			var callbackCount = 0;

			mock.Changed += value =>
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

			input.value = 100;
			Assert.That(callbackCount, Is.EqualTo(1));

			input.value = 200;
			Assert.That(callbackCount, Is.EqualTo(2));

			input.value = 200;
			Assert.That(callbackCount, Is.EqualTo(2));
		}
	}
}
