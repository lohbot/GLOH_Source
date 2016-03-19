using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class TsLightProbeLoader : MonoBehaviour
{
	private static string OBJ_NAME = "@LightProbeLoader";

	private UnityEngine.Object mRoot;

	public static void Create()
	{
		GameObject gameObject = GameObject.Find(TsLightProbeLoader.OBJ_NAME);
		if (gameObject == null)
		{
			gameObject = new GameObject(TsLightProbeLoader.OBJ_NAME);
		}
		TsLightProbeLoader tsLightProbeLoader = gameObject.AddComponent<TsLightProbeLoader>();
		tsLightProbeLoader.mRoot = gameObject;
	}

	public static TsLightProbeLoader Find()
	{
		return UnityEngine.Object.FindObjectOfType(typeof(TsLightProbeLoader)) as TsLightProbeLoader;
	}

	[DebuggerHidden]
	public IEnumerable Load(string path)
	{
		TsLightProbeLoader.<Load>c__Iterator6A <Load>c__Iterator6A = new TsLightProbeLoader.<Load>c__Iterator6A();
		<Load>c__Iterator6A.path = path;
		<Load>c__Iterator6A.<$>path = path;
		<Load>c__Iterator6A.<>f__this = this;
		TsLightProbeLoader.<Load>c__Iterator6A expr_1C = <Load>c__Iterator6A;
		expr_1C.$PC = -2;
		return expr_1C;
	}
}
