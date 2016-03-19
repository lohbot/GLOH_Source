using System;
using System.Collections.Generic;

namespace BattleTriggerClient
{
	public class BattleTriggerEvent
	{
		private BattleTriggerEventBase m_stTriggerEvent;

		private List<BattleTriggerParam> m_arTrigger;

		private List<BattleTriggerParam> m_arAction;

		public bool m_bDelete;

		public BattleTriggerEvent()
		{
			this.m_stTriggerEvent = new BattleTriggerEventBase();
			this.m_stTriggerEvent.Init();
			this.m_arTrigger = new List<BattleTriggerParam>();
			this.m_arAction = new List<BattleTriggerParam>();
			this.m_bDelete = false;
		}

		public BattleTriggerEventBase GetEventBase()
		{
			return this.m_stTriggerEvent;
		}

		public void AddTrigger(BattleTriggerParam Param, bool bLoad)
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

		public void AddAction(BattleTriggerParam Param, bool bLoad)
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

		public BattleTriggerParam GetTrigger(int index)
		{
			if (index < 0 || index >= this.m_arTrigger.Count)
			{
				return null;
			}
			return this.m_arTrigger[index];
		}

		public BattleTriggerParam GetAction(int index)
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
			BattleTriggerParam battleTriggerParam = this.m_arTrigger[index];
			battleTriggerParam.m_bDelete = true;
			this.m_arTrigger.RemoveAt(index);
			this.SortTrigger();
		}

		public void RemoveAction(int index)
		{
			if (index < 0 || index >= this.m_arAction.Count)
			{
				return;
			}
			BattleTriggerParam battleTriggerParam = this.m_arAction[index];
			battleTriggerParam.m_bDelete = true;
			this.m_arAction.RemoveAt(index);
			this.SortAction();
		}

		public void SortTrigger()
		{
			for (int i = 0; i < this.m_arTrigger.Count; i++)
			{
				BattleTriggerParam battleTriggerParam = this.m_arTrigger[i];
				battleTriggerParam.m_iD = i;
			}
			this.m_stTriggerEvent.m_iTriggerNum = this.m_arTrigger.Count;
		}

		public void SortAction()
		{
			for (int i = 0; i < this.m_arAction.Count; i++)
			{
				BattleTriggerParam battleTriggerParam = this.m_arAction[i];
				battleTriggerParam.m_iD = i;
			}
			this.m_stTriggerEvent.m_iActionNum = this.m_arAction.Count;
		}

		public void ChangeTriggerPosition(BATTLETRIGGERTYPE type, int index, bool bUp)
		{
			if (type != BATTLETRIGGERTYPE.TYPE_TRIGGER)
			{
				if (type == BATTLETRIGGERTYPE.TYPE_ACTION)
				{
					if (index >= 0 && index < this.m_arAction.Count)
					{
						BattleTriggerParam item = this.m_arAction[index];
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
				BattleTriggerParam item2 = this.m_arTrigger[index];
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
