using System;
using System.Collections.Generic;
using UnityEngine;

namespace IndunTriggerClient
{
	public class IndunTriggerEvent
	{
		private IndunTriggerEventBase m_stTriggerEvent;

		private List<IndunTriggerParam> m_arTrigger;

		private List<IndunTriggerParam> m_arAction;

		public bool m_bDelete;

		public IndunTriggerEvent()
		{
			this.m_stTriggerEvent = new IndunTriggerEventBase();
			this.m_stTriggerEvent.Init();
			this.m_arTrigger = new List<IndunTriggerParam>();
			this.m_arAction = new List<IndunTriggerParam>();
			this.m_bDelete = false;
		}

		public IndunTriggerEventBase GetEventBase()
		{
			return this.m_stTriggerEvent;
		}

		public void AddTrigger(IndunTriggerParam Param, bool bLoad)
		{
			if (!bLoad)
			{
				Param.m_iD = this.m_stTriggerEvent.m_iTriggerNum;
			}
			else if (Param.m_iD >= this.m_stTriggerEvent.m_iTriggerNum)
			{
				this.m_stTriggerEvent.m_iTriggerNum = Param.m_iD;
			}
			this.m_arTrigger.Add(Param);
			this.m_stTriggerEvent.m_iTriggerNum = this.m_arTrigger.Count;
		}

		public int GetTriggerCount()
		{
			return this.m_arTrigger.Count;
		}

		public void AddAction(IndunTriggerParam Param, bool bLoad)
		{
			if (!bLoad)
			{
				Param.m_iD = this.m_stTriggerEvent.m_iActionNum;
			}
			else if (Param.m_iD >= this.m_stTriggerEvent.m_iActionNum)
			{
				this.m_stTriggerEvent.m_iActionNum = Param.m_iD;
			}
			this.m_arAction.Add(Param);
			this.m_stTriggerEvent.m_iActionNum = this.m_arAction.Count;
		}

		public int GetActionCount()
		{
			return this.m_arAction.Count;
		}

		public IndunTriggerParam GetTrigger(int index)
		{
			if (index < 0 || index >= this.m_arTrigger.Count)
			{
				return null;
			}
			return this.m_arTrigger[index];
		}

		public IndunTriggerParam GetAction(int index)
		{
			if (index < 0 || index >= this.m_arAction.Count)
			{
				return null;
			}
			return this.m_arAction[index];
		}

		public void RemoveTrigger(int index)
		{
			if (index < 0 || index >= this.m_arTrigger.Count)
			{
				return;
			}
			IndunTriggerParam indunTriggerParam = this.m_arTrigger[index];
			indunTriggerParam.m_bDelete = true;
			this.m_arTrigger.RemoveAt(index);
			this.SortTrigger();
		}

		public void RemoveAction(int index)
		{
			if (index < 0 || index >= this.m_arAction.Count)
			{
				return;
			}
			IndunTriggerParam indunTriggerParam = this.m_arAction[index];
			indunTriggerParam.m_bDelete = true;
			UnityEngine.Object.DestroyImmediate(indunTriggerParam.m_GameObjcet);
			this.m_arAction.RemoveAt(index);
			this.SortAction();
		}

		public void SortTrigger()
		{
			for (int i = 0; i < this.m_arTrigger.Count; i++)
			{
				IndunTriggerParam indunTriggerParam = this.m_arTrigger[i];
				indunTriggerParam.m_iD = i;
			}
			this.m_stTriggerEvent.m_iTriggerNum = this.m_arTrigger.Count;
		}

		public void SortAction()
		{
			for (int i = 0; i < this.m_arAction.Count; i++)
			{
				IndunTriggerParam indunTriggerParam = this.m_arAction[i];
				indunTriggerParam.m_iD = i;
			}
			this.m_stTriggerEvent.m_iActionNum = this.m_arAction.Count;
		}

		public void ChangeTriggerPosition(INDUNTRIGGERTYPE type, int index, bool bUp)
		{
			if (type != INDUNTRIGGERTYPE.INDUNTRIGGERTYPE_TRIGGER)
			{
				if (type == INDUNTRIGGERTYPE.INDUNTRIGGERTYPE_ACTION)
				{
					if (index >= 0 && index < this.m_arAction.Count)
					{
						IndunTriggerParam item = this.m_arAction[index];
						if (bUp)
						{
							if (index > 0)
							{
								this.m_arAction.RemoveAt(index);
								this.m_arAction.Insert(index - 1, item);
							}
						}
						else if (index < this.m_arAction.Count - 1)
						{
							this.m_arAction.RemoveAt(index);
							this.m_arAction.Insert(index + 1, item);
						}
						this.SortAction();
					}
				}
			}
			else if (index >= 0 && index < this.m_arTrigger.Count)
			{
				IndunTriggerParam item2 = this.m_arTrigger[index];
				if (bUp)
				{
					if (index > 0)
					{
						this.m_arTrigger.RemoveAt(index);
						this.m_arTrigger.Insert(index - 1, item2);
					}
				}
				else if (index < this.m_arTrigger.Count - 1)
				{
					this.m_arTrigger.RemoveAt(index);
					this.m_arTrigger.Insert(index + 1, item2);
				}
				this.SortTrigger();
			}
		}
	}
}
