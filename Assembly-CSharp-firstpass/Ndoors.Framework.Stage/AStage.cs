using Ndoors.Framework.CoTask;
using System;
using System.Collections;

namespace Ndoors.Framework.Stage
{
	public abstract class AStage : Yielder
	{
		private WaitSerialTasks _serialTsk;

		internal bool EndOfSerialTask
		{
			get
			{
				return this._serialTsk == null || this._serialTsk.Count == 0;
			}
		}

		protected abstract void OnUpdateAfterStagePrework();

		public abstract Scene.Type SceneType();

		public virtual void OnPrepareSceneChange()
		{
		}

		public virtual void OnReloadReserved()
		{
		}

		public virtual void OnEnter()
		{
		}

		public virtual void OnExit()
		{
		}

		public string GetStageName()
		{
			return base.GetType().Name;
		}

		public void StartTaskSerial(IEnumerator ie)
		{
			this._serialTsk.Add(ie);
		}

		public void StartTaskSerial(BaseCoTask ct)
		{
			this._serialTsk.Add(ct);
		}

		internal void RegistSeialTaskInternal()
		{
			if (this._serialTsk != null)
			{
				this._serialTsk.Initialize();
			}
			else
			{
				this._serialTsk = new WaitSerialTasks();
			}
			base.StartTaskPararell(this._serialTsk);
		}

		public void Update()
		{
			if (this.EndOfSerialTask)
			{
				this.OnUpdateAfterStagePrework();
			}
			base.ProcessCoTasksInternal();
		}
	}
}
