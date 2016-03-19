using System;
using UnityEngine;
using UnityForms;

public class BackDlg : Form
{
	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "DLG_Back", G_ID.BACK_DLG, false);
	}

	public override void SetComponent()
	{
	}

	public void BackTownImage()
	{
		Texture2D texture2D = (Texture2D)Resources.Load(NrTSingleton<UIDataManager>.Instance.FilePath + "Texture/TownBG" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		if (null != texture2D)
		{
			base.SetSize(GUICamera.width, GUICamera.height);
			base.SetLocation(0f, 0f);
			base.SetBGImage(texture2D);
		}
	}

	public void BlackImage()
	{
		base.SetSize(GUICamera.width, GUICamera.height);
		base.SetLocation(0f, 0f);
		base.SetupBG("Win_T_BK", 0f, 0f, GUICamera.width, GUICamera.height);
	}

	public void TransparentImage()
	{
		base.SetSize(GUICamera.width, GUICamera.height);
		base.SetLocation(0f, 0f);
		base.SetupBG("Com_I_Transparent", 0f, 0f, GUICamera.width, GUICamera.height);
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}
	}
}
