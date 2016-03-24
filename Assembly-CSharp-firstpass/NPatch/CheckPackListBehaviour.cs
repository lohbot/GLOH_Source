using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace NPatch
{
	public class CheckPackListBehaviour : MonoBehaviour
	{
		private List<string> packedList;

		private Queue<int> packSizeQueue;

		private Queue<string> szMD5Queue;

		private Queue<int> zipfileSizeQueue;

		private Queue<int> unzipfileSizeQueue;

		private Queue<int> unpackSizeQueue;

		private Queue<int> DivisonQueue;

		private Action<bool> ChangeIsNeededPatch;

		private Action EndBehaviour;

		private Launcher.CheckPackListDelegate AddPackListTask;

		private Launcher.AddTaskInstall AddTaskInstall;

		private Launcher Owner;

		private int division;

		public void CheckPackList(List<string> _packedList, Launcher _Owner, int _division, Queue<int> _packSizeQueue, Queue<string> _szMD5Queue, Queue<int> _zipfileSizeQueue, Action<bool> _ChangeIsNeededPatch, Queue<int> _unzipfileSizeQueue, Queue<int> _unpackSizeQueue, Queue<int> _DivisonQueue, Launcher.CheckPackListDelegate packListDelegate, Launcher.AddTaskInstall _addTaskInstall, Action _EndBehaviour)
		{
			this.packedList = _packedList;
			this.Owner = _Owner;
			this.division = _division;
			this.packSizeQueue = _packSizeQueue;
			this.szMD5Queue = _szMD5Queue;
			this.zipfileSizeQueue = _zipfileSizeQueue;
			this.ChangeIsNeededPatch = _ChangeIsNeededPatch;
			this.unzipfileSizeQueue = _unzipfileSizeQueue;
			this.unpackSizeQueue = _unpackSizeQueue;
			this.DivisonQueue = _DivisonQueue;
			this.EndBehaviour = _EndBehaviour;
			this.AddPackListTask = packListDelegate;
			this.AddTaskInstall = _addTaskInstall;
			base.StartCoroutine("CheckPackCoroutine");
		}

		[DebuggerHidden]
		private IEnumerator CheckPackCoroutine()
		{
			CheckPackListBehaviour.<CheckPackCoroutine>c__IteratorD <CheckPackCoroutine>c__IteratorD = new CheckPackListBehaviour.<CheckPackCoroutine>c__IteratorD();
			<CheckPackCoroutine>c__IteratorD.<>f__this = this;
			return <CheckPackCoroutine>c__IteratorD;
		}
	}
}
