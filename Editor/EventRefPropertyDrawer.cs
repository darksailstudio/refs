using System;
using UnityEditor;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace DarkSail.Refs.Editor
{
	[CustomPropertyDrawer(typeof(EventRef), useForChildren: true)]
	public class EventRefPropertyDrawer : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var eventInvokerInfo = fieldInfo.FieldType.GetMethod(nameof(EventRef.Invoke));
			Assert.IsNotNull(eventInvokerInfo);

			return new ButtonField(
				label: property.displayName,
				text: nameof(EventRef.Invoke),
				onClicked: () =>
				{
					foreach (var targetObject in property.serializedObject.targetObjects)
					{
						var target = fieldInfo.GetValue(targetObject);
						eventInvokerInfo.Invoke(target, null);
					}
				}
			);
		}

		class ButtonField : BaseField<object>
		{
			public ButtonField(string label, string text, Action onClicked)
				: base(
					label,
					new Button(onClicked)
					{
						text = text,
						style =
						{
							flexGrow = 1,
							marginLeft = 0,
							marginRight = 0,
							marginTop = 0,
							marginBottom = 0,
						},
					}
				)
			{
				contentContainer.AddToClassList("unity-base-field__aligned");
			}
		}
	}
}
