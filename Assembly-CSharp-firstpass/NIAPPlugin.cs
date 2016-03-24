using System;
using UnityEngine;

public abstract class NIAPPlugin : ScriptableObject
{
	public abstract void invoke(NIAPParam param);

	public abstract void showMessage(string message);
}
