using Ndoors.Framework.CoTask;
using Ndoors.Framework.Stage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace StageHelper
{
	public class TableInspector : BaseCoTask
	{
		private bool _isTableNDT;

		private List<BaseCoTask> _listCoTsk = new List<BaseCoTask>(128);

		private int _downCount;

		private AStage _stageYielder;

		private bool _waitDone;

		public int Count
		{
			get
			{
				return this._downCount;
			}
		}

		public bool IsDone
		{
			get;
			private set;
		}

		public TableInspector(AStage yldr)
		{
			this.IsDone = false;
			this._waitDone = false;
			this._stageYielder = yldr;
			this._routine = this._WaitUntileDone();
		}

		public TableInspector(AStage yldr, bool waitDone)
		{
			this.IsDone = false;
			this._waitDone = waitDone;
			this._stageYielder = yldr;
			this._routine = this._WaitUntileDone();
		}

		public void Regist(NrTableBase tbl)
		{
			this._listCoTsk.Add(new WaitTask(this._LoadTable(tbl)));
		}

		public void RegistWait(NrTableBase preTbl, NrTableBase tbl)
		{
			this._listCoTsk.Add(new WaitTask(this._LoadTable(preTbl, tbl)));
		}

		public void RegistSerial(params object[] objs)
		{
			WaitSerialTasks waitSerialTasks = new WaitSerialTasks();
			for (int i = 0; i < objs.Length; i++)
			{
				NrTableBase nrTableBase = objs[i] as NrTableBase;
				if (nrTableBase != null)
				{
					waitSerialTasks.Add(new WaitTask(this._LoadTable(nrTableBase)));
				}
				else
				{
					TsLog.LogError("Non NrTableBase class. {0}", new object[]
					{
						objs[i].GetType().FullName
					});
				}
			}
			this._listCoTsk.Add(waitSerialTasks);
		}

		[DebuggerHidden]
		private IEnumerator _WaitUntileDone()
		{
			TableInspector.<_WaitUntileDone>c__Iterator27 <_WaitUntileDone>c__Iterator = new TableInspector.<_WaitUntileDone>c__Iterator27();
			<_WaitUntileDone>c__Iterator.<>f__this = this;
			return <_WaitUntileDone>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator _Progress()
		{
			TableInspector.<_Progress>c__Iterator28 <_Progress>c__Iterator = new TableInspector.<_Progress>c__Iterator28();
			<_Progress>c__Iterator.<>f__this = this;
			return <_Progress>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator _LoadTable(NrTableBase kTable)
		{
			TableInspector.<_LoadTable>c__Iterator29 <_LoadTable>c__Iterator = new TableInspector.<_LoadTable>c__Iterator29();
			<_LoadTable>c__Iterator.kTable = kTable;
			<_LoadTable>c__Iterator.<$>kTable = kTable;
			<_LoadTable>c__Iterator.<>f__this = this;
			return <_LoadTable>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator _LoadTable(NrTableBase kPreTable, NrTableBase kTable)
		{
			TableInspector.<_LoadTable>c__Iterator2A <_LoadTable>c__Iterator2A = new TableInspector.<_LoadTable>c__Iterator2A();
			<_LoadTable>c__Iterator2A.kPreTable = kPreTable;
			<_LoadTable>c__Iterator2A.kTable = kTable;
			<_LoadTable>c__Iterator2A.<$>kPreTable = kPreTable;
			<_LoadTable>c__Iterator2A.<$>kTable = kTable;
			<_LoadTable>c__Iterator2A.<>f__this = this;
			return <_LoadTable>c__Iterator2A;
		}

		[DebuggerHidden]
		private IEnumerator _WaitUntilFinishPreTable(NrTableBase kPreTable)
		{
			TableInspector.<_WaitUntilFinishPreTable>c__Iterator2B <_WaitUntilFinishPreTable>c__Iterator2B = new TableInspector.<_WaitUntilFinishPreTable>c__Iterator2B();
			<_WaitUntilFinishPreTable>c__Iterator2B.kPreTable = kPreTable;
			<_WaitUntilFinishPreTable>c__Iterator2B.<$>kPreTable = kPreTable;
			return <_WaitUntilFinishPreTable>c__Iterator2B;
		}

		[DebuggerHidden]
		private IEnumerator _LoadFromLocalNDTFile(NrTableBase kTable)
		{
			TableInspector.<_LoadFromLocalNDTFile>c__Iterator2C <_LoadFromLocalNDTFile>c__Iterator2C = new TableInspector.<_LoadFromLocalNDTFile>c__Iterator2C();
			<_LoadFromLocalNDTFile>c__Iterator2C.kTable = kTable;
			<_LoadFromLocalNDTFile>c__Iterator2C.<$>kTable = kTable;
			<_LoadFromLocalNDTFile>c__Iterator2C.<>f__this = this;
			return <_LoadFromLocalNDTFile>c__Iterator2C;
		}

		[DebuggerHidden]
		private IEnumerator _LoadFromPackedFile(NrTableBase kTable)
		{
			TableInspector.<_LoadFromPackedFile>c__Iterator2D <_LoadFromPackedFile>c__Iterator2D = new TableInspector.<_LoadFromPackedFile>c__Iterator2D();
			<_LoadFromPackedFile>c__Iterator2D.<>f__this = this;
			return <_LoadFromPackedFile>c__Iterator2D;
		}

		[DebuggerHidden]
		private IEnumerator _DownloadAssetbundle(NrTableBase kTable)
		{
			TableInspector.<_DownloadAssetbundle>c__Iterator2E <_DownloadAssetbundle>c__Iterator2E = new TableInspector.<_DownloadAssetbundle>c__Iterator2E();
			<_DownloadAssetbundle>c__Iterator2E.kTable = kTable;
			<_DownloadAssetbundle>c__Iterator2E.<$>kTable = kTable;
			<_DownloadAssetbundle>c__Iterator2E.<>f__this = this;
			return <_DownloadAssetbundle>c__Iterator2E;
		}

		[DebuggerHidden]
		private IEnumerable<string> _ParseTableNDT(string strContext)
		{
			TableInspector.<_ParseTableNDT>c__Iterator2F <_ParseTableNDT>c__Iterator2F = new TableInspector.<_ParseTableNDT>c__Iterator2F();
			<_ParseTableNDT>c__Iterator2F.strContext = strContext;
			<_ParseTableNDT>c__Iterator2F.<$>strContext = strContext;
			<_ParseTableNDT>c__Iterator2F.<>f__this = this;
			TableInspector.<_ParseTableNDT>c__Iterator2F expr_1C = <_ParseTableNDT>c__Iterator2F;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
