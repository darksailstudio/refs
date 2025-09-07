using NUnit.Framework;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkSail.Refs.Editor.Tests
{
	public abstract class PropertyDrawerTests
	{
		EditorWindow window;

		[SetUp]
		public void Setup()
		{
			window = ScriptableObject.CreateInstance<EditorWindow>();
			window.Show();
		}

		[TearDown]
		public void Teardown()
		{
			window.Close();
			window = null;
		}

		protected PropertyField GetPropertyField(Object target, string propertyPath)
		{
			var editor = UnityEditor.Editor.CreateEditor(target);
			var root = editor.CreateInspectorGUI();

			window.rootVisualElement.Add(root);
			root.Bind(editor.serializedObject);

			var serializedProperty = editor.serializedObject.FindProperty(propertyPath);
			var propertyField = root.Query<PropertyField>()
				.Where(propertyField =>
					propertyField.bindingPath == serializedProperty.propertyPath
				)
				.First();

			propertyField.BindProperty(serializedProperty);

			return propertyField;
		}
	}
}
