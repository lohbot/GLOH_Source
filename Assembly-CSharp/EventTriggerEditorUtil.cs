using System;
using UnityEngine;

public class EventTriggerEditorUtil
{
	public delegate UnityEngine.Object OnGetAsset(string path, Type type);

	public delegate string OnGetAssetPath(UnityEngine.Object oj);

	public delegate void OnOpenSceneAsset(string AssetPath);

	public static EventTriggerEditorUtil.OnGetAsset _OnGetAsset;

	public static EventTriggerEditorUtil.OnGetAssetPath _OnGetAssetPath;

	public static EventTriggerEditorUtil.OnOpenSceneAsset _OnOpenSceneAsset;
}
