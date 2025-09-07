using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkSail.Refs.Editor.Tests
{
	public class EventRefPropertyDrawerTests : PropertyDrawerTests
	{
		static void Click(Button button)
		{
			using var submitEvent = NavigationSubmitEvent.GetPooled();
			submitEvent.target = button;
			button.SendEvent(submitEvent);
		}

		[Test]
		public void ButtonClickInvokesEvent()
		{
			var mock = ScriptableObject.CreateInstance<SharedEventRef>();
			var button = GetPropertyField(mock, "eventRef").Q<Button>();
			var callbackCount = 0;

			mock.Invoked += () => callbackCount++;

			Click(button);
			Assert.That(callbackCount, Is.EqualTo(1));

			Click(button);
			Assert.That(callbackCount, Is.EqualTo(2));
		}
	}
}
