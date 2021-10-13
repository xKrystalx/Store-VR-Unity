using System.Diagnostics;
using UnityEngine;

namespace Sisus.Debugging
{
	#if DEV_MODE
	[CreateAssetMenu]
	#endif
	public class DebugLogExtensionsProjectSettingsAsset : ScriptableObject, IDebugLogExtensionsProjectSettingsAsset
	{
		private static DebugLogExtensionsProjectSettingsAsset instance;

		private const string ResourcePath = "DebugLogExtensionsProjectSettings";

		public bool useGlobalNamespace = true;
		public bool stripAllCallsFromBuilds = false;
		public bool unlistedChannelsEnabledByDefault = true;
		public DebugChannelInfo[] channels = new DebugChannelInfo[0];
		public KeyConfig toggleView = new KeyConfig(KeyConfig.ToggleViewKey, KeyCode.Insert);
		public IgnoredStackTraceInfo[] hideStackTraceRows = new IgnoredStackTraceInfo[0];

		#if DEV_MODE
		[System.NonSerialized]
		#else
		[SerializeField, HideInInspector]
		#endif
		public bool useGlobalNamespaceDetermined = false;

		public bool autoAddDevUniqueChannels = true;

		public static DebugLogExtensionsProjectSettingsAsset Get()
		{
			if(instance == null)
			{
				instance = Resources.Load<DebugLogExtensionsProjectSettingsAsset>(ResourcePath);

				#if DEV_MODE
				Debug.Log("DebugLogExtensionsProjectSettingsAsset.Get - loaded asset", instance);
				#endif

				#if DEV_MODE && UNITY_EDITOR
				if(instance == null) { Debug.LogWarning(ResourcePath + " not found. EditorApplication.isUpdating=" + UnityEditor.EditorApplication.isUpdating); }
				#endif
			}
			return instance;
		}

        public void Apply(DebugLogExtensionsProjectSettings settings)
        {
			#if DEV_MODE
			Debug.Log("DebugLogExtensionsProjectSettingsAsset.Apply(settings)", this);
			#endif

			settings.useGlobalNamespace = useGlobalNamespace;
			settings.stripAllCallsFromBuilds = stripAllCallsFromBuilds;
			settings.unlistedChannelsEnabledByDefault = unlistedChannelsEnabledByDefault;
			settings.channels = channels;
			settings.toggleView = toggleView;
			settings.hideStackTraceRows = hideStackTraceRows;
			settings.useGlobalNamespaceDetermined = useGlobalNamespaceDetermined;
			settings.autoAddDevUniqueChannels = autoAddDevUniqueChannels;
			settings.Apply();
        }

        private void OnEnable()
        {
			instance = this;
		}

        [Conditional("UNITY_EDITOR")]
		private void OnValidate()
		{
			for(int n = channels.Length - 1; n >= 0; n--)
			{
				channels[n].OnValidate();
			}
		}
	}
}