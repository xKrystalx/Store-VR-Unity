﻿using UnityEngine;
using UnityEditor;
using JetBrains.Annotations;

namespace Sisus.Debugging
{
	[CustomEditor(typeof(DebugLogExtensionsDemoComponent))]
	public class DebugLogExtensionsDemoComponentEditor : Editor
	{
		[UsedImplicitly]
		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();
			{
				base.OnInspectorGUI();
			}
			if(EditorGUI.EndChangeCheck())
			{
				(target as DebugLogExtensionsDemoComponent).OnValuesChanged();
			}

			GUILayout.Space(5f);

			(target as DebugLogExtensionsDemoComponent).UpdateRotation();
			Repaint();

			if(GUILayout.Button("Open Demo Window"))
			{
				EditorWindow.GetWindow<DebugLogExtensionsDemoWindow>();
			}
		}
	}
}