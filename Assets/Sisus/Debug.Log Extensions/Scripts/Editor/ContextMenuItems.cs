using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace Sisus.Debugging
{
	/// <summary>
	/// Utility class that handles the opening of context menus and informing other systems of this taking place.
	/// </summary>
	[InitializeOnLoad]
	public static class ContextMenuItems
	{
		private static int membersAreBeingDisplayed = 0;

		/// <summary>
		/// This is initialized on load due to the usage of the InitializeOnLoad attribute.
		/// </summary>
		static ContextMenuItems()
		{
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

			EditorApplication.contextualPropertyMenu -= OnPropertyContextMenuOpen;
			if(Application.isPlaying || membersAreBeingDisplayed > 0)
			{
				EditorApplication.contextualPropertyMenu += OnPropertyContextMenuOpen;
			}
		}

		private static void OnPlayModeStateChanged(PlayModeStateChange playMode)
		{
			switch(playMode)
			{
				case PlayModeStateChange.EnteredPlayMode:
				case PlayModeStateChange.ExitingPlayMode:
					EditorApplication.contextualPropertyMenu -= OnPropertyContextMenuOpen;
					EditorApplication.contextualPropertyMenu += OnPropertyContextMenuOpen;
					return;
				default:
					if(membersAreBeingDisplayed <= 0)
					{
						EditorApplication.contextualPropertyMenu -= OnPropertyContextMenuOpen;
					}
					return;
			}
		}

		private static void OnPropertyContextMenuOpen(GenericMenu menu, SerializedProperty property)
		{
			// This does work in edit mode too, but we probably want to avoid cluttering
			// the context menu with it outside of play mode, since it's mostly useful in that context.
			if(!Application.isPlaying && membersAreBeingDisplayed <= 0)
			{
				return;
			}

			MemberInfo memberInfo;
			object owner;
			property.GetMemberInfoAndOwner(out memberInfo, out owner);
			if(memberInfo == null)
			{
				return;
			}

			var propertyInfo = memberInfo as PropertyInfo;
			if(propertyInfo != null)
			{
				if(!propertyInfo.CanRead)
				{
					return;
				}

				var indexParameters = propertyInfo.GetIndexParameters();
				if(indexParameters != null && indexParameters.Length > 0)
				{
					return;
				}

				if(owner == null)
				{
					if(!propertyInfo.GetGetMethod().IsStatic)
					{
						return;
					}
				}
				// Currently tracking members of value types is not supported.
				else if(owner.GetType().IsValueType)
				{
					return;
				}
			}
			else
			{
				var fieldInfo = memberInfo as FieldInfo;
				if(fieldInfo != null)
				{
					if(owner == null)
					{
						if(!fieldInfo.IsStatic)
						{
							return;
						}
					}
					// Currently tracking members of value types is not supported.
					else if(owner.GetType().IsValueType)
					{
						return;
					}
				}
			}

			bool isDisplayed = false;
			for(int n = Debug.DisplayedOnScreen.Count - 1; n >= 0; n--)
			{
				var item = Debug.DisplayedOnScreen[n];
				if(item.TargetEquals(memberInfo))
				{
					isDisplayed = true;
					break;
				}
			}

			if(isDisplayed)
			{
				menu.AddItem(new GUIContent("Display On Screen"), true, () =>
				{
					Debug.CancelDisplayOnScreen(memberInfo);
					membersAreBeingDisplayed--;
					if(membersAreBeingDisplayed <= 0 && !Application.isPlaying)
					{
						EditorApplication.contextualPropertyMenu -= OnPropertyContextMenuOpen;
					}
				});
			}
			else
			{
				menu.AddItem(new GUIContent("Display On Screen"), false, () =>
				{
					Debug.DisplayOnScreen(owner, memberInfo);
					membersAreBeingDisplayed++;
				});

				bool isTracked = false;
				for(int n = Debug.DebuggedEveryFrame.Count - 1; n >= 0; n--)
				{
					var item = Debug.DebuggedEveryFrame[n];
					if(item.TargetEquals(memberInfo))
					{
						isTracked = true;
						break;
					}
				}

				if(isTracked)
				{
					menu.AddItem(new GUIContent("Log Changes"), true, () =>
					{
						Debug.CancelLogChanges(memberInfo);
						membersAreBeingDisplayed--;
						if(membersAreBeingDisplayed <= 0 && !Application.isPlaying)
						{
							EditorApplication.contextualPropertyMenu -= OnPropertyContextMenuOpen;
						}
					});
				}
				else
				{
					menu.AddItem(new GUIContent("Log Changes/Log Only"), false, () =>
					{
						Debug.LogChanges(owner, memberInfo, false);
						membersAreBeingDisplayed++;
					});

					menu.AddItem(new GUIContent("Log Changes/Log And Pause"), false, () =>
					{
						Debug.LogChanges(owner, memberInfo, true);
						membersAreBeingDisplayed++;
					});
				}
			}
		}
	}
}