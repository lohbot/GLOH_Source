using System;
using System.IO;
using TsBundle;
using UnityEngine;
using WellFired;

public class Behavior_uSequencer : EventTriggerItem_Behavior
{
	public string m_SequenceName = string.Empty;

	private string m_SequencePath = string.Empty;

	private string cdnPath = "d:/ndoors/at2dev/mobile/";

	private GameObject instance;

	private UsequenceController controller;

	public override void Init()
	{
		this.instance = null;
		string text = Option.GetProtocolRootPath(Protocol.FILE);
		text = text.Substring("file:///".Length, text.Length - "file:///".Length);
		this.m_SequencePath = string.Format("{0}{1}/{2}.assetbundle", text, "uSequencer", this.m_SequenceName);
		Debug.Log("uSequencer path : " + this.m_SequencePath);
	}

	public override bool Excute()
	{
		if (this.m_SequencePath == string.Empty)
		{
			return false;
		}
		if (!File.Exists(this.m_SequencePath))
		{
			Debug.Log("not exist file: " + this.m_SequencePath);
			return false;
		}
		this.instance = new GameObject("uSequence Controller");
		this.controller = this.instance.AddComponent<UsequenceController>();
		this.controller.StartCutScene(this.m_SequencePath);
		this.m_Excute = true;
		return false;
	}

	public override bool IsPopNext()
	{
		return true;
	}

	public override string GetComment()
	{
		if (this.m_SequenceName.Equals(string.Empty))
		{
			return "연출을 선택 해주세요.";
		}
		return string.Format("{0} 연출을 출력한다.", this.m_SequenceName);
	}

	public override float ExcuteTiemSecond()
	{
		return 5f;
	}

	public override Behavior._BEHAVIORTYPE GetBehaviorType()
	{
		return Behavior._BEHAVIORTYPE.DRAMA;
	}

	public override bool IsVaildValue()
	{
		return !string.IsNullOrEmpty(this.m_SequenceName);
	}

	public void SkipUSequencer()
	{
		if (this.controller != null)
		{
			this.controller.skip = true;
		}
	}
}
