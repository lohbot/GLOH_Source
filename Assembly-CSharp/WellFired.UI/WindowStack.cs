using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using WellFired.Data;

namespace WellFired.UI
{
	public class WindowStack
	{
		private Stack<IWindow> windowStack = new Stack<IWindow>();

		private Canvas rootCanvas;

		private EventSystem eventSystem;

		public WindowStack(Canvas canvas, EventSystem eventSystem)
		{
			this.rootCanvas = canvas;
			this.eventSystem = eventSystem;
		}

		public IWindow OpenWindowWithData(Type windowTypeToOpen, DataBaseEntry data)
		{
			IWindow window = this.OpenWindow(windowTypeToOpen);
			WindowWithDataComponent windowWithDataComponent = window as WindowWithDataComponent;
			windowWithDataComponent.InitFromData(data);
			return window;
		}

		public IWindow OpenWindow(Type windowTypeToOpen)
		{
			string text = windowTypeToOpen.Name.ToString();
			string path = string.Format("UI/Window/{0}/{1}", text, text);
			GameObject original = Resources.Load(path) as GameObject;
			GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
			RectTransform component = gameObject.GetComponent<RectTransform>();
			Window component2 = gameObject.GetComponent<Window>();
			gameObject.transform.SetParent(this.rootCanvas.transform, false);
			if (this.windowStack.Count > 0 && this.windowStack.Peek() != null)
			{
				RectTransform component3 = (this.windowStack.Peek() as MonoBehaviour).GetComponent<RectTransform>();
				float z = component3.localPosition.z;
				Vector3 localPosition = component.localPosition;
				localPosition.z = z - 5f;
				component.localPosition = localPosition;
			}
			this.PushWindow(component2);
			component2.WindowStack = this;
			if (this.eventSystem != null && component2.FirstSelectedGameObject != null)
			{
				this.eventSystem.SetSelectedGameObject(component2.FirstSelectedGameObject);
			}
			return this.windowStack.Peek();
		}

		public void CloseWindow(IWindow window)
		{
			if (this.windowStack.Peek() != window)
			{
				throw new Exception(string.Format("Trying to pop a window {0} that is not at the head of the stack", window.GetType()));
			}
			this.PopWindow();
		}

		private void PushWindow(IWindow window)
		{
			this.windowStack.Push(window);
		}

		private void PopWindow()
		{
			this.windowStack.Pop();
		}
	}
}
