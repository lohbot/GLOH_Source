using System;
using UnityEngine;

public class NrDontDestroy : MonoBehaviour
{
	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Update()
	{
	}
}
