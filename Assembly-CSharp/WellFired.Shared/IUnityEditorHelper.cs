using System;

namespace WellFired.Shared
{
	public interface IUnityEditorHelper
	{
		void AddUpdateListener(Action listener);

		void RemoveUpdateListener(Action listener);
	}
}
