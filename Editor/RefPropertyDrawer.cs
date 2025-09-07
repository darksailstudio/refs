using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace DarkSail.Refs.Editor
{
	[CustomPropertyDrawer(typeof(Ref<>), useForChildren: true)]
	public class RefPropertyDrawer : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var eventDelegateInfo = fieldInfo
				.FieldType.GetEvent(nameof(Ref<object>.Changed))
				?.DeclaringType.GetField(
					nameof(Ref<object>.Changed),
					BindingFlags.Instance | BindingFlags.NonPublic
				);
			Assert.IsNotNull(eventDelegateInfo);

			var propertyField = new PropertyField(
				property.FindPropertyRelative("value"),
				property.displayName
			);

			propertyField.RegisterValueChangeCallback(changeEvent =>
			{
				foreach (var targetObject in property.serializedObject.targetObjects)
				{
					var target = fieldInfo.GetValue(targetObject);
					var eventDelegate = (Delegate)eventDelegateInfo.GetValue(target);
					eventDelegate?.DynamicInvoke(changeEvent.changedProperty.boxedValue);
				}
			});

			return propertyField;
		}
	}
}
