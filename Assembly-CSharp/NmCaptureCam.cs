using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class NmCaptureCam : MonoBehaviour
{
	public static Texture2D msShot2D;

	public bool bCapture;

	private void Update()
	{
		if (this.bCapture)
		{
			base.StartCoroutine(this.TakeShotCamera());
		}
	}

	public static void GetTakeShot()
	{
	}

	[DebuggerHidden]
	private IEnumerator TakeShotCamera()
	{
		NmCaptureCam.<TakeShotCamera>c__IteratorC <TakeShotCamera>c__IteratorC = new NmCaptureCam.<TakeShotCamera>c__IteratorC();
		<TakeShotCamera>c__IteratorC.<>f__this = this;
		return <TakeShotCamera>c__IteratorC;
	}

	private static Texture2D GetTexture()
	{
		bool flag = null == NmCaptureCam.msShot2D;
		if (!flag && (NmCaptureCam.msShot2D.width != Screen.width || NmCaptureCam.msShot2D.height != Screen.height))
		{
			flag = true;
		}
		if (flag)
		{
			NmCaptureCam.msShot2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		}
		return NmCaptureCam.msShot2D;
	}
}
