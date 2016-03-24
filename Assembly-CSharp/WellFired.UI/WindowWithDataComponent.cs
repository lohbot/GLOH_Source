using System;
using UnityEngine;
using WellFired.Data;

namespace WellFired.UI
{
	public class WindowWithDataComponent : Window
	{
		protected DataBaseEntry Data
		{
			get;
			set;
		}

		public void InitFromData(DataBaseEntry data)
		{
			this.Data = data;
		}

		protected virtual void Start()
		{
			if (this.Data == null)
			{
				Debug.LogError("Window has started without being Initialized, you probably should have called OpenWindowWithData on the windowStack", base.gameObject);
			}
		}

		public override void Ready()
		{
			base.Ready();
		}
	}
}
