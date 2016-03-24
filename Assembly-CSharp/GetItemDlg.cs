using GAME;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class GetItemDlg : Form
{
	private DrawTexture m_DrawTextureBG;

	private ItemTexture m_itemTexture;

	private Label m_itemName;

	private Label m_itemNumber;

	private int m_nIndex;

	private float m_fUpdateRemoveTime = 2f;

	private float m_fStartTime;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Item/DLG_GetItem", G_ID.GET_ITEM_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public void SetIndex(int nIndex)
	{
		this.m_nIndex = nIndex;
	}

	public override void SetComponent()
	{
		this.m_DrawTextureBG = (base.GetControl("DrawTexture_DrawTextureBG") as DrawTexture);
		this.m_itemTexture = (base.GetControl("DrawTexture_DrawTexture1") as ItemTexture);
		this.m_itemName = (base.GetControl("Label_Label1") as Label);
		this.m_itemNumber = (base.GetControl("Label_Label2") as Label);
		this.m_DrawTextureBG.Visible = false;
	}

	public void SetItem(int unique, int number, int nRank)
	{
		this.m_fStartTime = Time.time;
		ITEM iTEM = new ITEM();
		iTEM.m_nItemUnique = unique;
		iTEM.m_nItemNum = number;
		iTEM.m_nOption[2] = nRank;
		this.m_itemTexture.SetItemTexture(iTEM);
		this.m_itemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(iTEM);
		this.m_itemNumber.Text = NrTSingleton<UIDataManager>.Instance.GetString(number.ToString(), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("442"));
		if (this.IsTicketItem(unique))
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "BATTLE", "ITEM_CARD", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		else
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "BATTLE", "ITEM_ARTICLE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	public void SetAttendItem(int unique, int number, float fTime)
	{
		this.m_fStartTime = Time.time;
		ITEM iTEM = new ITEM();
		iTEM.m_nItemUnique = unique;
		iTEM.m_nItemNum = number;
		iTEM.m_nOption[2] = 0;
		this.m_itemTexture.SetItemTexture(iTEM);
		this.m_itemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(iTEM);
		this.m_itemNumber.Text = NrTSingleton<UIDataManager>.Instance.GetString(number.ToString(), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("442"));
		this.m_fUpdateRemoveTime = fTime;
		if (this.IsTicketItem(unique))
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "BATTLE", "ITEM_CARD", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		else
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "BATTLE", "ITEM_ARTICLE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EVENT_NORMAL_ATTEND);
		if (form != null)
		{
			this.m_DrawTextureBG.Visible = true;
			base.SetLocation(GUICamera.width / 2f - base.GetSize().x / 2f, 0f);
		}
	}

	public override void Update()
	{
		base.Update();
		this.UpdatePosition();
		if (this.m_fStartTime != 0f && Time.time - this.m_fStartTime > this.m_fUpdateRemoveTime)
		{
			if (Battle.BATTLE != null)
			{
				Battle.BATTLE.RemoveGetItemDlg(this);
			}
			this.Close();
		}
	}

	public void UpdatePosition()
	{
		Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_COUNT_DLG);
		if (form != null)
		{
			if (form.visible)
			{
				float y = form.GetLocationY() + form.GetSizeY() + ((float)this.m_nIndex * base.GetSize().y + 3f);
				base.SetLocation(GUICamera.width / 2f - base.GetSize().x / 2f, y);
			}
			else
			{
				float y2 = (float)this.m_nIndex * base.GetSize().y + 3f;
				base.SetLocation(GUICamera.width / 2f - base.GetSize().x / 2f, y2);
			}
		}
		else
		{
			float y3 = (float)this.m_nIndex * base.GetSize().y + 3f;
			base.SetLocation(GUICamera.width / 2f - base.GetSize().x / 2f, y3);
		}
	}

	public Vector3 WorldToEZ(Vector3 Pos)
	{
		Camera main = Camera.main;
		if (null != main)
		{
			Pos = main.WorldToScreenPoint(Pos);
		}
		Pos.y = (float)Screen.height - Pos.y;
		Pos = GUICamera.ScreenToGUIPoint(Pos);
		return Pos;
	}

	private bool IsTicketItem(int nItemUnique)
	{
		long atb = 298496L;
		return NrTSingleton<ItemManager>.Instance.IsItemATB(nItemUnique, atb);
	}
}
