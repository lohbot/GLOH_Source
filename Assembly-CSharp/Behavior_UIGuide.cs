using System;
using System.Reflection;
using UnityEngine;
using UnityForms;

public class Behavior_UIGuide : EventTriggerItem_Behavior
{
	public string m_Explain = string.Empty;

	public string m_DlgID = string.Empty;

	public string m_Param1 = string.Empty;

	public string m_Param2 = string.Empty;

	public float m_ActionTime;

	private float _StartTime;

	private UI_UIGuide _UIGuide;

	private Form _GuideForm;

	public bool bLoop;

	public bool bActive;

	private void InitData()
	{
		this.bActive = false;
		G_ID windowID = (G_ID)((int)Enum.Parse(typeof(G_ID), this.m_DlgID));
		this._GuideForm = NrTSingleton<FormsManager>.Instance.GetForm(windowID);
		if (this._GuideForm == null)
		{
			return;
		}
		this._UIGuide = (NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.UIGUIDE_DLG) as UI_UIGuide);
		this._UIGuide.SetText(this.m_Explain);
		if ("1" == this.m_Param2)
		{
			this._UIGuide.SetShowLayer(0, false);
			this._UIGuide.SetShowLayer(1, true);
		}
		else
		{
			this._UIGuide.SetShowLayer(0, true);
			this._UIGuide.SetShowLayer(1, false);
		}
		if (this.m_ActionTime == -1f)
		{
			this.bLoop = true;
		}
		if (this._GuideForm != null)
		{
			MethodInfo method = this._GuideForm.GetType().GetMethod("ShowUIGuide");
			if (method != null)
			{
				object[] parameters = new object[]
				{
					this.m_Param1,
					this.m_Param2,
					this._UIGuide.WindowID
				};
				method.Invoke(this._GuideForm, parameters);
			}
		}
		this._StartTime = Time.time;
		this.bActive = true;
	}

	public override void Init()
	{
		this.InitData();
	}

	public override bool Excute()
	{
		this.m_Excute = true;
		if (this.IsPopNext())
		{
			if (this._GuideForm != null)
			{
				MethodInfo method = this._GuideForm.GetType().GetMethod("HideUIGuide");
				if (method != null)
				{
					method.Invoke(this._GuideForm, null);
				}
			}
			this._UIGuide.Close();
			return false;
		}
		if (!this.bActive)
		{
			this.InitData();
		}
		else if (this._GuideForm != null)
		{
			MethodInfo method2 = this._GuideForm.GetType().GetMethod("UpdateUIGuide");
			if (method2 != null)
			{
				method2.Invoke(this._GuideForm, null);
			}
		}
		return true;
	}

	public override bool IsPopNext()
	{
		if (!this.bActive)
		{
			return false;
		}
		if (this.bLoop)
		{
			return this._UIGuide.CloseUI;
		}
		return Math.Abs(this._StartTime - Time.time) >= this.m_ActionTime;
	}

	public override string GetComment()
	{
		return string.Concat(new string[]
		{
			"DLG : ",
			this.m_DlgID,
			"�� ",
			this.m_Param1,
			"+",
			this.m_Param2,
			"���� �� ������ ���� �Ѵ�."
		});
	}

	public override float ExcuteTiemSecond()
	{
		return 0f;
	}

	public override void Draw()
	{
	}

	public override Behavior._BEHAVIORTYPE GetBehaviorType()
	{
		return Behavior._BEHAVIORTYPE.ETC;
	}

	public override bool IsVaildValue()
	{
		return !string.IsNullOrEmpty(this.m_DlgID);
	}
}
