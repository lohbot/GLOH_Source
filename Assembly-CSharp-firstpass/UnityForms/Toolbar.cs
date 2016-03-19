using GameMessage;
using System;
using UnityEngine;

namespace UnityForms
{
	public class Toolbar : UIPanelManager
	{
		public enum TEXTPOS
		{
			CENTER,
			UP,
			DOWN
		}

		private UIPanelTab[] _Tab;

		private UIPanel[] _Panel;

		private int _Count;

		private Vector3 position = Vector3.zero;

		public UIPanelTab[] Control_Tab
		{
			get
			{
				return this._Tab;
			}
			set
			{
				this._Tab = value;
			}
		}

		public UIPanel[] Control_Panel
		{
			get
			{
				return this._Panel;
			}
			set
			{
				this._Panel = value;
			}
		}

		public int Count
		{
			get
			{
				return this._Count;
			}
			set
			{
				this._Count = value;
			}
		}

		public int SeletedToolIndex
		{
			get
			{
				if (null == base.CurrentPanel)
				{
					return 0;
				}
				return base.CurrentPanel.index;
			}
			set
			{
				base.CurrentPanel.index = value;
			}
		}

		public override bool controlIsEnabled
		{
			get
			{
				return base.controlIsEnabled;
			}
			set
			{
				base.controlIsEnabled = value;
				for (int i = 0; i < this._Tab.Length; i++)
				{
					this._Tab[i].controlIsEnabled = value;
				}
			}
		}

		public new static Toolbar Create(string name, Vector3 pos)
		{
			return (Toolbar)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(Toolbar));
		}

		public void AddPanelGameObejct(int index, GameObject obj)
		{
			if (index < 0)
			{
				return;
			}
			if (index > this._Count)
			{
				return;
			}
			this._Panel[index].MakeChild(obj);
			this._Panel[index].deactivateAllOnDismiss = true;
			this._Panel[index].gameObject.SetActive(false);
		}

		public void FirstSetting()
		{
			this._Panel[0].gameObject.SetActive(true);
			base.BringIn(this._Panel[0]);
		}

		public void SetFirstTab(int index)
		{
			if (index < 0)
			{
				return;
			}
			if (index > this._Count)
			{
				return;
			}
			this._Panel[index].gameObject.SetActive(true);
			base.BringIn(this._Panel[index]);
		}

		public void SetData(int index, string colorkey, string text, EZValueChangedDelegate clickobj)
		{
			if (index < 0 || index > this._Count)
			{
				return;
			}
			if (colorkey.Length == 0)
			{
				colorkey = "1001";
			}
			this.Control_Tab[index].Text = NrTSingleton<UIDataManager>.Instance.GetString(MsgHandler.HandleReturn<string>("GetTextColor", new object[]
			{
				colorkey
			}), text);
			UIPanelTab expr_5A = this.Control_Tab[index];
			expr_5A.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_5A.ButtonClick, new EZValueChangedDelegate(clickobj.Invoke));
		}

		private void ChangeTab(int index, bool bShow)
		{
			this.Control_Panel[index].gameObject.SetActive(bShow);
		}

		public void SetSelectTabIndex(int index)
		{
			if (index < 0)
			{
				return;
			}
			if (index > this._Count)
			{
				return;
			}
			this.Control_Tab[index].Value = true;
			this.ChangeTab(this.SeletedToolIndex, false);
			base.CurrentPanel = this.Control_Panel[index];
			this.ChangeTab(index, true);
		}

		public void SetLocation(float x, float y, float z)
		{
			this.position.x = x;
			this.position.y = -y;
			this.position.z = z;
			base.transform.localPosition = this.position;
		}

		public Vector3 GetLocation()
		{
			return base.transform.localPosition;
		}

		public void AddPaelTapDelegate()
		{
		}

		public void SetEnabled(bool value)
		{
			base.controlIsEnabled = value;
			for (int i = 0; i < this._Tab.Length; i++)
			{
				this._Tab[i].controlIsEnabled = value;
			}
		}

		public void SetEnabled(int index, bool value)
		{
			if (index < 0 && index >= this._Tab.Length)
			{
				return;
			}
			this._Tab[index].controlIsEnabled = value;
		}

		public bool GetEnabled()
		{
			return base.controlIsEnabled;
		}

		public bool GetEnabled(int index)
		{
			return (index >= 0 || index < this._Tab.Length) && this._Tab[index].controlIsEnabled;
		}

		public void SetSizeWidth(int siWidth)
		{
			siWidth /= this._Tab.Length;
			for (int i = 0; i < this._Tab.Length; i++)
			{
				this._Tab[i].SetLocation((float)(i * siWidth), this._Tab[i].GetLocationY());
				this._Tab[i].SetSize((float)siWidth, this._Tab[i].GetSize().y);
				this._Tab[i].Text = this._Tab[i].Text;
			}
		}

		public void RePosition()
		{
			float num = 0f;
			for (int i = 0; i < this._Tab.Length; i++)
			{
				if (!this._Tab[i].controlIsEnabled || !this._Tab[i].Visible)
				{
					this._Tab[i].Visible = false;
				}
				else
				{
					this._Tab[i].Visible = true;
					this._Tab[i].SetLocation(num, this._Tab[i].GetLocationY());
					num += this._Tab[i].width;
				}
			}
		}
	}
}
