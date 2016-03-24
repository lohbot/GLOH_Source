using Ndoors.Framework.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Ndoors.Framework.Stage
{
	public static class StageSystem
	{
		private class StgCommon : AStage
		{
			protected override void OnUpdateAfterStagePrework()
			{
			}

			public override Scene.Type SceneType()
			{
				return Scene.Type.EMPTY;
			}
		}

		private enum TransMode
		{
			NONE,
			DONE,
			MOVE,
			PUSH,
			POP,
			RELOAD
		}

		private static StageSystem.TransMode _transMode;

		private static AStage _nxtStg;

		private static bool _1stRunPassed;

		private static Stack<AStage> _stack;

		private static bool bStabled;

		private static AStage _stgCommon;

		public static bool IsReservedMoveStage
		{
			get
			{
				return StageSystem._transMode != StageSystem.TransMode.NONE;
			}
		}

		public static bool IsStable
		{
			get
			{
				return StageSystem.bStabled;
			}
			set
			{
				StageSystem.bStabled = value;
			}
		}

		public static bool EndOfSerialTask
		{
			get
			{
				AStage aStage = StageSystem._stack.Peek();
				return aStage == null || aStage.EndOfSerialTask;
			}
		}

		static StageSystem()
		{
			StageSystem._transMode = StageSystem.TransMode.NONE;
			StageSystem._nxtStg = null;
			StageSystem._1stRunPassed = false;
			StageSystem._stack = new Stack<AStage>(4);
			StageSystem.bStabled = false;
			StageSystem._stgCommon = null;
			StageSystem._stgCommon = new StageSystem.StgCommon();
			StageSystem._stack.Push(StageSystem._stgCommon);
		}

		public static string GetCurrentStageName()
		{
			AStage aStage = StageSystem._stack.Peek();
			if (aStage == null)
			{
				return string.Empty;
			}
			return aStage.GetStageName();
		}

		public static Scene.Type GetCurrentStageType()
		{
			AStage aStage = StageSystem._stack.Peek();
			if (aStage == null)
			{
				return Scene.Type.EMPTY;
			}
			return aStage.SceneType();
		}

		public static int GetCurrentStageCoTaskCount()
		{
			AStage aStage = StageSystem._stack.Peek();
			if (aStage == null)
			{
				return 0;
			}
			return aStage.CountCoTask;
		}

		public static void AddCommonPararellTask(IEnumerator ie)
		{
			if (StageSystem._stgCommon != null)
			{
				StageSystem._stgCommon.StartTaskPararell(ie);
			}
		}

		public static void SetStartStage(AStage iStg)
		{
			if (StageSystem._1stRunPassed)
			{
				TsLog.LogWarning("!!!!!! StageSystem.SetStartStage() called multiple! Watch your code carefully!", new object[0]);
			}
			else if (iStg == null)
			{
				TsLog.LogError("!!!!!! StageSystem.SetStartStage(null) null parameter!", new object[0]);
			}
			else
			{
				StageSystem._1stRunPassed = true;
				StageSystem._stack.Push(iStg);
				iStg.RegistSeialTaskInternal();
				iStg.OnEnter();
			}
		}

		public static void ReserveMoveStage(AStage iStg)
		{
			StageSystem._transMode = StageSystem.TransMode.MOVE;
			StageSystem._nxtStg = iStg;
			iStg.OnPrepareSceneChange();
			TsLog.Log("=== StageSystem.ReserveMoveStage : {0}", new object[]
			{
				StageSystem._nxtStg.GetType().Name
			});
		}

		private static void _MoveReserved()
		{
			AStage aStage = StageSystem._stack.Pop();
			aStage.OnExit();
			aStage = StageSystem._nxtStg;
			StageSystem._nxtStg = null;
			StageSystem._transMode = StageSystem.TransMode.DONE;
			StageSystem._stack.Push(aStage);
			aStage.RegistSeialTaskInternal();
			aStage.OnEnter();
		}

		public static void ReservePushStage(AStage iStg)
		{
			if (iStg == null)
			{
				TsLog.LogError("=== StageSystem.ReservePushStage(null) null parameter!", new object[0]);
			}
			else
			{
				StageSystem._nxtStg = iStg;
				StageSystem._transMode = StageSystem.TransMode.PUSH;
				iStg.OnPrepareSceneChange();
				TsLog.Log("=== StageSystem.ReservePushStage PUSH: {0}", new object[]
				{
					StageSystem._nxtStg.GetType().Name
				});
			}
		}

		private static void _PushReserved()
		{
			StageSystem._transMode = StageSystem.TransMode.DONE;
			StageSystem._stack.Peek().OnExit();
			StageSystem._nxtStg.RegistSeialTaskInternal();
			StageSystem._nxtStg.OnEnter();
			StageSystem._stack.Push(StageSystem._nxtStg);
			StageSystem._nxtStg = null;
		}

		public static Scene.Type ReservePopStage()
		{
			Scene.Type result = Scene.Type.EMPTY;
			if (StageSystem._stack.Count <= 1)
			{
				TsLog.LogError("=== StageSystem.ReservePopStage stack empty", new object[0]);
			}
			else
			{
				AStage aStage = StageSystem._stack.Pop();
				AStage aStage2 = StageSystem._stack.Peek();
				aStage2.OnPrepareSceneChange();
				result = aStage2.SceneType();
				StageSystem._stack.Push(aStage);
				StageSystem._transMode = StageSystem.TransMode.POP;
				TsLog.Log("=== StageSystem.ReservePopStage POP: {0}", new object[]
				{
					aStage.GetType().Name
				});
			}
			return result;
		}

		private static void _PopReserved()
		{
			StageSystem._transMode = StageSystem.TransMode.DONE;
			AStage aStage = StageSystem._stack.Pop();
			aStage.OnExit();
			StageSystem._stack.Peek().RegistSeialTaskInternal();
			StageSystem._stack.Peek().OnReloadReserved();
			StageSystem._stack.Peek().OnEnter();
		}

		public static void ReloadStage()
		{
			StageSystem._transMode = StageSystem.TransMode.RELOAD;
			AStage aStage = StageSystem._stack.Peek();
			aStage.OnPrepareSceneChange();
			TsLog.Log("=== StageSystem.ReloadStage POP: {0}", new object[]
			{
				aStage.GetType().Name
			});
		}

		private static void _ReloadReserved()
		{
			StageSystem._transMode = StageSystem.TransMode.DONE;
			AStage aStage = StageSystem._stack.Peek();
			aStage.OnExit();
			aStage.RegistSeialTaskInternal();
			aStage.OnReloadReserved();
			aStage.OnEnter();
		}

		public static void InsertPush(AStage astg)
		{
			TsLog.Log("=== StageSystem.PushStageSwap TOPSWAP: {0} / {1}", new object[]
			{
				StageSystem._stack.Peek().GetType().Name,
				astg.GetType().Name
			});
			AStage t = StageSystem._stack.Pop();
			StageSystem._stack.Push(astg);
			StageSystem._stack.Push(t);
		}

		public static void ResetStack()
		{
			TsLog.Log("=== StageSystem.ResetStack", new object[0]);
			AStage t = StageSystem._stack.Pop();
			while (StageSystem._stack.Count > 1)
			{
				StageSystem._stack.Pop();
			}
			StageSystem._stack.Push(t);
		}

		public static void Update()
		{
			if (StageSystem._transMode == StageSystem.TransMode.NONE)
			{
				StageSystem._stack.Peek().Update();
			}
			else if (StageSystem._transMode == StageSystem.TransMode.MOVE)
			{
				StageSystem._MoveReserved();
			}
			else if (StageSystem._transMode == StageSystem.TransMode.PUSH)
			{
				StageSystem._PushReserved();
			}
			else if (StageSystem._transMode == StageSystem.TransMode.POP)
			{
				StageSystem._PopReserved();
			}
			else if (StageSystem._transMode == StageSystem.TransMode.RELOAD)
			{
				StageSystem._ReloadReserved();
			}
			else if (StageSystem._transMode == StageSystem.TransMode.DONE)
			{
				StageSystem._transMode = StageSystem.TransMode.NONE;
			}
			if (StageSystem._stgCommon != null)
			{
				StageSystem._stgCommon.Update();
			}
		}

		public static bool CurrentStageHandleMessage(string functionName, params object[] Params)
		{
			AStage aStage = StageSystem._stack.Peek();
			return aStage != null && Dispatcher.DispatchDynamic(aStage, functionName, Params);
		}

		public static string ToStringStatus()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("--- StageSystem Information ------\n", new object[0]);
			foreach (AStage current in StageSystem._stack)
			{
				stringBuilder.AppendFormat("{0} : Cnt=({1})\n", current.GetType().Name, current.CountCoTask);
				stringBuilder.AppendLine(current.ToStringStatus(string.Empty));
			}
			return stringBuilder.ToString();
		}
	}
}
