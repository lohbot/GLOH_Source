using System;
using UnityEngine;

namespace WellFired.UI
{
	public class Window : MonoBehaviour, IWindow
	{
		public WindowStack WindowStack
		{
			get;
			set;
		}

		public virtual GameObject FirstSelectedGameObject
		{
			get;
			set;
		}

		public Action OnClose
		{
			get;
			set;
		}

		public void CloseWindow()
		{
			this.WindowStack.CloseWindow(this);
			UnityEngine.Object.Destroy(base.gameObject);
			if (this.OnClose != null)
			{
				this.OnClose();
			}
		}

		public virtual void Ready()
		{
		}
	}
}
