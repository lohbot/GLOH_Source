using System;
using System.Reflection;
using UnityEngine;

namespace WellFired.Shared
{
	public static class TransitionHelper
	{
		private static Type gameViewType;

		private static MethodInfo getMainGameView;

		private static MethodInfo repaintAllMethod;

		private static PropertyInfo gameViewRenderRect;

		private static FieldInfo shownResolution;

		private static object mainGameView;

		private static Type GameViewType
		{
			get
			{
				if (TransitionHelper.gameViewType == null)
				{
					TransitionHelper.gameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
				}
				return TransitionHelper.gameViewType;
			}
		}

		private static MethodInfo GetMainGameView
		{
			get
			{
				if (TransitionHelper.getMainGameView == null)
				{
					TransitionHelper.getMainGameView = PlatformSpecificFactory.ReflectionHelper.GetNonPublicStaticMethod(TransitionHelper.GameViewType, "GetMainGameView");
				}
				return TransitionHelper.getMainGameView;
			}
		}

		private static MethodInfo RepaintAllMethod
		{
			get
			{
				if (TransitionHelper.repaintAllMethod == null)
				{
					TransitionHelper.repaintAllMethod = PlatformSpecificFactory.ReflectionHelper.GetMethod(TransitionHelper.GameViewType, "Repaint");
				}
				return TransitionHelper.repaintAllMethod;
			}
		}

		private static PropertyInfo GameViewRenderRect
		{
			get
			{
				if (TransitionHelper.gameViewRenderRect == null)
				{
					TransitionHelper.gameViewRenderRect = PlatformSpecificFactory.ReflectionHelper.GetNonPublicInstanceProperty(TransitionHelper.GameViewType, "gameViewRenderRect");
				}
				return TransitionHelper.gameViewRenderRect;
			}
		}

		private static FieldInfo ShownResolution
		{
			get
			{
				if (TransitionHelper.shownResolution == null)
				{
					TransitionHelper.shownResolution = PlatformSpecificFactory.ReflectionHelper.GetNonPublicInstanceField(TransitionHelper.GameViewType, "m_ShownResolution");
				}
				return TransitionHelper.shownResolution;
			}
		}

		private static object MainGameView
		{
			get
			{
				if (TransitionHelper.mainGameView == null)
				{
					TransitionHelper.mainGameView = TransitionHelper.GetMainGameView.Invoke(null, null);
				}
				return TransitionHelper.mainGameView;
			}
		}

		public static Vector2 MainGameViewSize
		{
			get
			{
				if (Application.isEditor)
				{
					Vector2 vector = (Vector2)TransitionHelper.ShownResolution.GetValue(TransitionHelper.MainGameView);
					if (vector == Vector2.zero)
					{
						object value = TransitionHelper.GameViewRenderRect.GetValue(TransitionHelper.MainGameView, null);
						vector = ((Rect)value).size;
					}
					return vector;
				}
				return new Vector2((float)Screen.width, (float)Screen.height);
			}
			set
			{
			}
		}

		public static float DefaultTransitionTimeFor(TypeOfTransition transitionType)
		{
			if (transitionType != TypeOfTransition.Cut)
			{
				return 2f;
			}
			return 0f;
		}

		public static void ForceGameViewRepaint()
		{
			if (Application.isEditor)
			{
				TransitionHelper.RepaintAllMethod.Invoke(TransitionHelper.MainGameView, null);
			}
		}
	}
}
