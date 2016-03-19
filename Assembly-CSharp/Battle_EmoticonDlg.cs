using System;
using UnityForms;

public class Battle_EmoticonDlg : Form
{
	private NewListBox m_nlbEmoticon;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Battle/DLG_Battle_Emoticon", G_ID.BATTLE_EMOTICON_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_nlbEmoticon = (base.GetControl("NLB_Emoticon") as NewListBox);
		this.m_nlbEmoticon.Reserve = false;
		this.m_nlbEmoticon.AddRightMouseDelegate(new EZValueChangedDelegate(this.OnClickEmoticonListBox));
		this._SetDialogPos();
	}

	public override void InitData()
	{
		this._SetDialogPos();
		this.SetEmoticonData();
	}

	public void _SetDialogPos()
	{
		float x = 0f;
		base.SetLocation(x, GUICamera.height - base.GetSizeY());
	}

	public override void ChangedResolution()
	{
		this._SetDialogPos();
	}

	private void SetEmoticonData()
	{
		this.m_nlbEmoticon.Clear();
		BATTLE_EMOTICON_Manager instance = BATTLE_EMOTICON_Manager.GetInstance();
		if (instance == null)
		{
			return;
		}
		for (int i = 0; i < 9; i++)
		{
			BATTLE_EMOTICON data = instance.GetData((eBATTLE_EMOTICON)i);
			if (data != null)
			{
				NewListItem newListItem = new NewListItem(this.m_nlbEmoticon.ColumnNum, true);
				newListItem.Data = data;
				newListItem.SetListItemData(0, string.Empty, data, new EZValueChangedDelegate(this.OnClickEmoticonListBox), null);
				newListItem.SetListItemData(1, data.m_szTexture, null, null, null);
				newListItem.SetListItemData(2, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(data.m_szTextKey), null, null, null);
				this.m_nlbEmoticon.Add(newListItem);
			}
		}
		this.m_nlbEmoticon.RepositionItems();
	}

	public void OnClickEmoticon(IUIObject obj)
	{
	}

	public void OnClickEmoticonListBox(IUIObject obj)
	{
		if (this.m_nlbEmoticon.SelectedItem == null)
		{
			return;
		}
		BATTLE_EMOTICON bATTLE_EMOTICON = (BATTLE_EMOTICON)obj.Data;
		if (!Battle.BATTLE.IsEmotionSet)
		{
			Battle.BATTLE.IsEmotionSet = true;
			Battle.BATTLE.SetEmoticon = bATTLE_EMOTICON.m_eConstant;
			return;
		}
		if (Battle.BATTLE.SetEmoticon == bATTLE_EMOTICON.m_eConstant)
		{
			Battle.BATTLE.IsEmotionSet = false;
			Battle.BATTLE.SetEmoticon = eBATTLE_EMOTICON.eBATTLE_EMOTICON_MAX;
			return;
		}
		Battle.BATTLE.SetEmoticon = bATTLE_EMOTICON.m_eConstant;
	}

	public override void Update()
	{
		base.Update();
		for (int i = 0; i < 9; i++)
		{
			UIListItemContainer uIListItemContainer = this.m_nlbEmoticon.GetItem(i) as UIListItemContainer;
			BATTLE_EMOTICON bATTLE_EMOTICON = (BATTLE_EMOTICON)uIListItemContainer.GetElementObject(0);
			if (!Battle.BATTLE.IsEmotionSet)
			{
				if (uIListItemContainer != null)
				{
					UIButton uIButton = uIListItemContainer.GetElement(0) as UIButton;
					if (uIButton != null)
					{
						uIButton.SetControlState(UIButton.CONTROL_STATE.NORMAL);
					}
				}
			}
			else if (Battle.BATTLE.SetEmoticon == bATTLE_EMOTICON.m_eConstant)
			{
				if (uIListItemContainer != null)
				{
					UIButton uIButton2 = uIListItemContainer.GetElement(0) as UIButton;
					if (uIButton2 != null)
					{
						uIButton2.SetControlState(UIButton.CONTROL_STATE.ACTIVE);
					}
				}
			}
			else if (uIListItemContainer != null)
			{
				UIButton uIButton3 = uIListItemContainer.GetElement(0) as UIButton;
				if (uIButton3 != null)
				{
					uIButton3.SetControlState(UIButton.CONTROL_STATE.NORMAL);
				}
			}
		}
	}
}
