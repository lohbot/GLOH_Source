using System;
using UnityEngine;

public class AndroidDumpTest : MonoBehaviour
{
	private void Awake()
	{
		DumpManager.GetInstance().RegistDumpHandler();
	}

	private void OnGUI()
	{
	}
}
