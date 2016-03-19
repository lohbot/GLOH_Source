using System;
using UnityForms;

public class ColosseumObserverSummonDlg : Form
{
	private ItemTexture[] m_itSol;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Colosseum/DLG_Colosseum_SummonFace", G_ID.COLOSSEUM_OBSERVER_SUMMON_DLG, false, true);
	}

	public override void SetComponent()
	{
		this.m_itSol = new ItemTexture[4];
		for (int i = 0; i < 4; i++)
		{
			string name = string.Format("ItemTexture_Sol{0}", (i + 1).ToString("00"));
			this.m_itSol[i] = (base.GetControl(name) as ItemTexture);
		}
	}

	public override void Show()
	{
		base.Show();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void InitData()
	{
	}

	public void SetDialgPos(short nAlly)
	{
		this.SetSol();
		Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_COLOSSEUM_CHARINFO_DLG);
		if (form == null)
		{
			this.Close();
		}
		float y = form.GetLocationY() + form.GetSizeY();
		if (nAlly == 0)
		{
			base.SetLocation(0f, y);
		}
		else
		{
			base.SetLocation(GUICamera.width - base.GetSizeX(), y);
		}
	}

	public void SetSol()
	{
		for (int i = 0; i < 4; i++)
		{
			int coloseumSupportSoldier = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetColoseumSupportSoldier(i);
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(coloseumSupportSoldier);
			if (charKindInfo != null)
			{
				string textureFromBundle = string.Empty;
				textureFromBundle = "UI/Soldier/64/" + charKindInfo.GetPortraitFile1(0) + "_64";
				this.m_itSol[i].SetTextureFromBundle(textureFromBundle);
			}
		}
	}
}
