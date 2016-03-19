using System;
using UnityForms;

public class WhisperColorDlg : Form
{
	private Toggle[] _tgColor = new Toggle[10];

	private string[] m_Colors = new string[]
	{
		"1107",
		"1303",
		"1110",
		"1404",
		"1304",
		"1301",
		"1113",
		"1114"
	};

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Whisper/dlg_whisper_textcolor", G_ID.WHISPER_COLOR_DLG, true);
	}

	public override void SetComponent()
	{
		for (int i = 0; i < this.m_Colors.Length; i++)
		{
			this._tgColor[i] = (base.GetControl("Toggle_Toggle" + (i + 1).ToString()) as Toggle);
			this._tgColor[i].Data = this.m_Colors[i];
			if (this.m_Colors[i] == NrTSingleton<WhisperManager>.Instance.ChatColor)
			{
				this._tgColor[i].Value = true;
			}
		}
		for (int j = 0; j < this.m_Colors.Length; j++)
		{
			Toggle expr_8F = this._tgColor[j];
			expr_8F.CheckedChanged = (EZValueChangedDelegate)Delegate.Combine(expr_8F.CheckedChanged, new EZValueChangedDelegate(this.OnClickColor));
		}
		base.Draggable = false;
	}

	public override void Update()
	{
	}

	private void OnClickColor(IUIObject obj)
	{
		Toggle toggle = obj as Toggle;
		NrTSingleton<WhisperManager>.Instance.ChatColor = (toggle.data as string);
		New_Whisper_Dlg new_Whisper_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.WHISPER_DLG) as New_Whisper_Dlg;
		if (new_Whisper_Dlg != null)
		{
			new_Whisper_Dlg.SetChatColor();
			new_Whisper_Dlg.InteractivePanel.twinFormID = G_ID.NONE;
		}
		BabelTower_ChatDlg babelTower_ChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWER_CHAT) as BabelTower_ChatDlg;
		if (babelTower_ChatDlg != null)
		{
			babelTower_ChatDlg.SetChatColor();
			babelTower_ChatDlg.InteractivePanel.twinFormID = G_ID.NONE;
		}
		this.Close();
	}
}
