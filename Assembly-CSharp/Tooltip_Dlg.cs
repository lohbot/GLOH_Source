using GAME;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class Tooltip_Dlg : Form
{
	public enum E_TEXTUER_TYPE
	{
		TEXT,
		LINE,
		LINE_WHITEBG,
		STAR,
		WEAPON,
		END
	}

	public enum eEquipItemPosition
	{
		FIRST = 1,
		SECOND
	}

	public struct Tooltip_Text_Info
	{
		public int m_nTextureType;

		public float m_fY;

		public float m_fFontSize;

		public SpriteText.Anchor_Pos m_eAnchor;

		public string m_strText;

		public string m_strTextColorCode;
	}

	public struct Create_Texture
	{
		public Label m_laText;

		public SpriteText.Anchor_Pos m_eAnchor;

		public int m_nTextureType;

		public int m_nRank;

		public float m_fY;
	}

	private const float F_ITEM_NAME_FONT_SIZE = 22f;

	private const float F_DEFAULT_FONT_SIZE = 22f;

	private const float F_SOLDIER_NAME_FONT_SIZE = 15f;

	private const float F_BUILDING_NAME_FONT_SIZE = 15f;

	private static float F_STAR_HEIGHT = 20f;

	private static float F_STAR_MAX_WIDTH = 108f;

	private static float F_LINE_TEXTURE_HEIGHT = 1f;

	private static float F_WEAPON_HEIGHT = 44f;

	private ITEM m_cItem = new ITEM();

	public string m_strText = string.Empty;

	public int m_nItemUnique;

	public static G_ID m_eParentWindowID = G_ID.NONE;

	public static Label s_laText;

	public static Vector3 MOBILE_TOOLTIP_POS = new Vector3(0f, 0f, 0f);

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Tooltip/DLG_tootip", G_ID.TOOLTIP_DLG, true);
		if (null != base.InteractivePanel)
		{
			base.SetLocation(base.GetLocation().x, base.GetLocation().y, 92f);
		}
	}

	public override void Update()
	{
		base.Update();
		Tooltip_Dlg.Tooltip_Rect(this, Vector3.zero);
		if (Tooltip_Dlg.m_eParentWindowID != G_ID.NONE && Tooltip_Dlg.m_eParentWindowID != G_ID.TERRITORY_TOOTIP && !NrTSingleton<FormsManager>.Instance.IsShow(Tooltip_Dlg.m_eParentWindowID))
		{
			this.Close();
		}
	}

	public override void OnClose()
	{
		Tooltip_Dlg.s_laText = null;
		Tooltip_Dlg.MOBILE_TOOLTIP_POS = Vector3.zero;
	}

	public static void Tooltip_Rect(Form cForm, Vector3 showPosition)
	{
		if (!TsPlatform.IsMobile || (showPosition == Vector3.zero && Tooltip_Dlg.MOBILE_TOOLTIP_POS == Vector3.zero))
		{
			float num = 20f;
			Vector3 vector = GUICamera.ScreenToGUIPoint(NkInputManager.mousePosition);
			Vector2 size = cForm.GetSize();
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.TOOLTIP_SECOND_DLG))
			{
				Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TOOLTIP_SECOND_DLG);
				Vector2 size2 = form.GetSize();
				float num2 = vector.x + num;
				if (num2 + size.x + 2f + size2.x > GUICamera.width)
				{
					num2 = vector.x - num - size.x - 2f - size2.x;
				}
				float num3 = GUICamera.height - vector.y;
				if (num3 + size.y > GUICamera.height)
				{
					float num4 = num3 + size.y - GUICamera.height;
					num3 -= num4;
				}
				form.SetLocation(num2 + size.x + 2f, num3);
				cForm.SetLocation(num2, num3);
			}
			else
			{
				float num2 = vector.x + num;
				if (num2 + size.x > GUICamera.width)
				{
					num2 = vector.x - num - size.x;
				}
				float num3 = GUICamera.height - vector.y;
				if (num3 + size.y > GUICamera.height)
				{
					float num5 = num3 + size.y - GUICamera.height;
					num3 -= num5;
				}
				cForm.SetLocation(num2, num3);
			}
		}
		else
		{
			if (showPosition != Vector3.zero && Tooltip_Dlg.MOBILE_TOOLTIP_POS == Vector3.zero)
			{
				showPosition.y = -showPosition.y;
				Tooltip_Dlg.MOBILE_TOOLTIP_POS = showPosition;
			}
			Vector3 vector = GUICamera.ScreenToGUIPoint(Tooltip_Dlg.MOBILE_TOOLTIP_POS);
			Vector2 size3 = cForm.GetSize();
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.TOOLTIP_SECOND_DLG))
			{
				Form form2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TOOLTIP_SECOND_DLG);
				float x = vector.x;
				float y = vector.y;
				form2.SetLocation(x + size3.x + 2f, y);
				cForm.SetLocation(x, y);
			}
			else
			{
				float x = vector.x;
				float y = vector.y;
				cForm.SetLocation(x, y);
			}
		}
	}

	public void Set_Tooltip(G_ID eWidowID, string strText)
	{
		Tooltip_Dlg.m_eParentWindowID = eWidowID;
		this.m_strText = strText;
		if (this.m_strText == null)
		{
			return;
		}
		Tooltip_Dlg.Item_Tooltip(this, this.m_strText);
		Tooltip_Dlg.Tooltip_Rect(this, Vector3.zero);
		this.Show();
	}

	public void Set_Tooltip(G_ID eWidowID, ITEM pkItem)
	{
		base.RemoveChildControl();
		Tooltip_Dlg.m_eParentWindowID = eWidowID;
		this.m_cItem = pkItem;
		Tooltip_Dlg.Item_Tooltip(this, this.m_cItem, eWidowID);
		Tooltip_Dlg.Tooltip_Rect(this, Vector3.zero);
		this.Show();
	}

	public void Set_Tooltip(G_ID eWidowID, ITEM pkItem, bool showItemNum)
	{
		base.RemoveChildControl();
		Tooltip_Dlg.m_eParentWindowID = eWidowID;
		this.m_cItem = pkItem;
		Tooltip_Dlg.Item_Tooltip(this, this.m_cItem, eWidowID);
		Tooltip_Dlg.Tooltip_Rect(this, Vector3.zero);
		this.Show();
	}

	public void Set_TooltipForEquip(G_ID eWidowID, ITEM pkEquipedItem, ITEM pkItem, bool bEquiped)
	{
		base.RemoveChildControl();
		Tooltip_Dlg.m_eParentWindowID = eWidowID;
		this.m_cItem = pkItem;
		Tooltip_Dlg.Item_Tooltip(this, pkEquipedItem, this.m_cItem, eWidowID, bEquiped);
		Tooltip_Dlg.Tooltip_Rect(this, Vector3.zero);
		this.Show();
	}

	public void Set_Tooltip(G_ID eWidowID, ITEM pkItem, ITEM pkEquipedItem, Vector3 showPosition)
	{
		if (!TsPlatform.IsMobile)
		{
			base.RemoveChildControl();
		}
		Tooltip_Dlg.m_eParentWindowID = eWidowID;
		this.m_cItem = pkItem;
		Tooltip_Dlg.Item_Tooltip(this, this.m_cItem, pkEquipedItem, eWidowID);
		Tooltip_Dlg.Tooltip_Rect(this, showPosition);
		this.Show();
	}

	public void Set_Tooltip(G_ID eWidowID, ITEM pkItem, ITEM pkEquipedItem)
	{
		if (!TsPlatform.IsMobile)
		{
			base.RemoveChildControl();
		}
		Tooltip_Dlg.m_eParentWindowID = eWidowID;
		this.m_cItem = pkItem;
		Tooltip_Dlg.Item_Tooltip(this, this.m_cItem, pkEquipedItem, eWidowID);
		Tooltip_Dlg.Tooltip_Rect(this, Vector3.zero);
		this.Show();
	}

	public void Set_Tooltip(ITEM pkItem)
	{
		this.m_cItem = pkItem;
		Tooltip_Dlg.Item_Tooltip(this, this.m_cItem, G_ID.NONE);
		Tooltip_Dlg.Tooltip_Rect(this, Vector3.zero);
		this.Show();
	}

	public void Set_Tooltip(G_ID eWidowID, int itemunique)
	{
		Tooltip_Dlg.m_eParentWindowID = eWidowID;
		this.m_cItem.Init();
		this.m_cItem.m_nItemUnique = itemunique;
		Tooltip_Dlg.Item_Tooltip(this, this.m_cItem, eWidowID);
		Tooltip_Dlg.Tooltip_Rect(this, Vector3.zero);
		this.Show();
	}

	public void Set_Tooltip(G_ID eWidowID, short solKind, byte SolWeapon)
	{
		this.Set_Tooltip(eWidowID, solKind, SolWeapon);
	}

	public void SetBattleSkillTooltip(G_ID eWidowID, int battleSkillUnique, int battleSkillLevel, NkSoldierInfo solInfo)
	{
		Tooltip_Dlg.m_eParentWindowID = eWidowID;
		Tooltip_Dlg.Skill_Tooltip(this, battleSkillUnique, battleSkillLevel, eWidowID, solInfo);
		Tooltip_Dlg.Tooltip_Rect(this, Vector3.zero);
		this.Show();
	}

	public void SetBattleControlSkillTooltip(G_ID eWidowID, int battleSkillUnique, int battleSkillLevel, int KeepTurn, NkSoldierInfo solInfo)
	{
		Tooltip_Dlg.m_eParentWindowID = eWidowID;
		Tooltip_Dlg.BattleControlSkill_Tooltip(this, battleSkillUnique, battleSkillLevel, eWidowID, KeepTurn, solInfo);
		Tooltip_Dlg.Tooltip_Rect(this, Vector3.zero);
		this.Show();
	}

	private static Label Create_Text(Form cForm, string strName, string strText, Rect rcRect, float fFontSize, SpriteText.Anchor_Pos ePso, bool bMultiLine, string strColor)
	{
		Label label = Label.Create(strName, Vector3.zero);
		label.width = rcRect.width;
		label.height = rcRect.height;
		label.CreateSpriteText();
		label.MaxWidth = Mathf.Min(rcRect.width, 300f);
		label.MultiLine = true;
		label.ColorText = strColor;
		label.SetCharacterSize(fFontSize);
		SpriteRoot.ANCHOR_METHOD anchor;
		SpriteText.Alignment_Type alignment;
		switch (ePso)
		{
		case SpriteText.Anchor_Pos.Upper_Left:
		case SpriteText.Anchor_Pos.Middle_Left:
		case SpriteText.Anchor_Pos.Lower_Left:
			anchor = SpriteRoot.ANCHOR_METHOD.MIDDLE_LEFT;
			alignment = SpriteText.Alignment_Type.Left;
			break;
		case SpriteText.Anchor_Pos.Upper_Center:
		case SpriteText.Anchor_Pos.Middle_Center:
		case SpriteText.Anchor_Pos.Lower_Center:
			anchor = SpriteRoot.ANCHOR_METHOD.MIDDLE_CENTER;
			alignment = SpriteText.Alignment_Type.Center;
			break;
		case SpriteText.Anchor_Pos.Upper_Right:
		case SpriteText.Anchor_Pos.Middle_Right:
		case SpriteText.Anchor_Pos.Lower_Right:
			anchor = SpriteRoot.ANCHOR_METHOD.MIDDLE_RIGHT;
			alignment = SpriteText.Alignment_Type.Right;
			break;
		default:
			anchor = SpriteRoot.ANCHOR_METHOD.MIDDLE_LEFT;
			alignment = SpriteText.Alignment_Type.Left;
			break;
		}
		label.SetAnchor(anchor);
		label.SetAnchor(ePso);
		label.SetAlignment(alignment);
		label.gameObject.renderer.enabled = false;
		label.gameObject.layer = GUICamera.UILayer;
		label.Text = strText;
		cForm.AddDictionaryControl(strName, label);
		cForm.InteractivePanel.MakeChild(label.gameObject);
		label.SetLocation(rcRect.x, rcRect.y, -0.001f);
		return label;
	}

	private static DrawTexture Create_DrawTexture(Form cForm, string strName, string l_strTexture, Rect rcRect)
	{
		return Tooltip_Dlg.Create_DrawTexture(cForm, strName, l_strTexture, rcRect, 1f);
	}

	private static DrawTexture Create_DrawTexture(Form cForm, string strName, string l_strTexture, Rect rcRect, float Alpha)
	{
		DrawTexture drawTexture = DrawTexture.Create(strName, Vector3.zero);
		drawTexture.width = rcRect.width;
		drawTexture.height = rcRect.height;
		drawTexture.SetTexture(l_strTexture);
		drawTexture.autoResize = false;
		drawTexture.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
		drawTexture.gameObject.layer = GUICamera.UILayer;
		cForm.AddDictionaryControl(strName, drawTexture);
		cForm.InteractivePanel.MakeChild(drawTexture.gameObject);
		drawTexture.SetLocation(rcRect.x, rcRect.y, -0.001f);
		if (Alpha != 1f)
		{
			Color color = drawTexture.color;
			color.a = Alpha;
			drawTexture.SetColor(color);
		}
		return drawTexture;
	}

	public static void Item_Tooltip(Form cForm, string strText)
	{
		float fFontSize;
		if (TsPlatform.IsWeb)
		{
			fFontSize = 15f;
		}
		else
		{
			fFontSize = 22f;
		}
		float num = 25f;
		float num2 = 0f;
		for (int i = 0; i < strText.Length; i++)
		{
			if (strText[i] > '가' && strText[i] < '힟')
			{
				num2 += 20f;
			}
			else
			{
				num2 += 8f;
			}
		}
		string strName = "TooltipText";
		Rect rcRect = new Rect(0f, 0f, num2, num);
		Tooltip_Dlg.s_laText = Tooltip_Dlg.Create_Text(cForm, strName, strText, rcRect, fFontSize, SpriteText.Anchor_Pos.Upper_Left, false, NrTSingleton<CTextParser>.Instance.GetTextColor("1101"));
		SpriteText spriteText = Tooltip_Dlg.s_laText.spriteText;
		if (spriteText != null)
		{
			num2 = spriteText.TotalWidth + 20f;
			num = spriteText.TotalHeight + 10f;
		}
		cForm.SetSize((int)num2, (int)num);
		Tooltip_Dlg.s_laText.SetLocation(10, 5);
	}

	public static void Item_Tooltip(Form cThis, ITEM pkItem, G_ID eWidowID)
	{
		Tooltip_Dlg.Tooltip_Text_Info[] array = Tooltip_Dlg.Get_Item_Text_Info(pkItem, null, eWidowID, false);
		if (array != null)
		{
			Tooltip_Dlg.Tooltip_Base(cThis, array, pkItem.m_nRank, eWidowID, 0);
		}
	}

	public static void Item_Tooltip(Form cThis, ITEM pkItem, ITEM pkEquipedItem, G_ID eWidowID)
	{
		Tooltip_Dlg.Tooltip_Text_Info[] array = Tooltip_Dlg.Get_Item_Text_Info(pkItem, pkEquipedItem, eWidowID, false);
		if (array != null)
		{
			Tooltip_Dlg.Tooltip_Base(cThis, array, pkItem.m_nRank, eWidowID, 0);
		}
	}

	public static void Item_Tooltip(Form cThis, ITEM pkItem, ITEM pkEquipedItem, G_ID eWidowID, bool bEquiped)
	{
		Tooltip_Dlg.Tooltip_Text_Info[] array = Tooltip_Dlg.Get_Item_Text_Info(pkItem, pkEquipedItem, eWidowID, bEquiped);
		if (array != null)
		{
			Tooltip_Dlg.Tooltip_Base(cThis, array, pkItem.m_nRank, eWidowID, 0);
		}
	}

	public static void Item_Tooltip(Form cThis, ITEM pkItem, G_ID eWidowID, bool bEquiped)
	{
		Tooltip_Dlg.Tooltip_Text_Info[] array = Tooltip_Dlg.Get_Item_Text_Info(pkItem, null, eWidowID, bEquiped);
		if (array != null)
		{
			Tooltip_Dlg.Tooltip_Base(cThis, array, pkItem.m_nRank, eWidowID, 0);
		}
	}

	public static void Skill_Tooltip(Form cThis, int battleSkillUnique, int battleSkillLevel, G_ID eWidowID, NkSoldierInfo solInfo)
	{
		Tooltip_Dlg.Tooltip_Text_Info[] skillTextInfo = Tooltip_Dlg.GetSkillTextInfo(battleSkillUnique, battleSkillLevel, eWidowID, solInfo);
		if (skillTextInfo != null)
		{
			Tooltip_Dlg.Tooltip_Base(cThis, skillTextInfo, 0, eWidowID, 0);
		}
	}

	public static void BattleControlSkill_Tooltip(Form cThis, int battleSkillUnique, int battleSkillLevel, G_ID eWidowID, int KeepTurn, NkSoldierInfo solInfo)
	{
		Tooltip_Dlg.Tooltip_Text_Info[] battleControlSkillTextInfo = Tooltip_Dlg.GetBattleControlSkillTextInfo(battleSkillUnique, battleSkillLevel, eWidowID, KeepTurn, solInfo);
		if (battleControlSkillTextInfo != null)
		{
			Tooltip_Dlg.Tooltip_Base(cThis, battleControlSkillTextInfo, 0, eWidowID, 0);
		}
	}

	private static void Draw_Star(Form cForm, float fY, float fWidth, int nRank)
	{
		Rect rcRect = default(Rect);
		float num = fWidth / 2f - Tooltip_Dlg.F_STAR_MAX_WIDTH / 2f;
		int num2 = nRank % 5;
		if (num2 == 0 && nRank != 0)
		{
			num2 = 5;
		}
		string l_strTexture = (nRank <= 5) ? "Com_I_Star24" : "Com_I_Star21";
		int i;
		for (i = 0; i < num2; i++)
		{
			string strName = "Star_" + i;
			rcRect.x = num + (float)i * 22f;
			rcRect.y = fY;
			rcRect.width = 20f;
			rcRect.height = Tooltip_Dlg.F_STAR_HEIGHT;
			Tooltip_Dlg.Create_DrawTexture(cForm, strName, l_strTexture, rcRect, 1f);
		}
		while (i < 5)
		{
			string strName2 = "StarBack_" + i;
			string l_strTexture2 = (nRank <= 5) ? "Com_I_Star22" : "Com_I_Star24";
			rcRect.x = num + (float)i * 22f;
			rcRect.y = fY;
			rcRect.width = 20f;
			rcRect.height = Tooltip_Dlg.F_STAR_HEIGHT;
			Tooltip_Dlg.Create_DrawTexture(cForm, strName2, l_strTexture2, rcRect, 1f);
			i++;
		}
	}

	private static void Draw_Line(Form cForm, float fY, float fWidth, bool bWhiteBG)
	{
		Rect rcRect = default(Rect);
		rcRect.x = 0f;
		rcRect.y = fY;
		rcRect.width = fWidth;
		rcRect.height = Tooltip_Dlg.F_LINE_TEXTURE_HEIGHT;
		if (bWhiteBG)
		{
			Tooltip_Dlg.Create_DrawTexture(cForm, "Line" + fY, "Win_T_SubBorderH", rcRect);
		}
		else
		{
			Tooltip_Dlg.Create_DrawTexture(cForm, "Line" + fY, "Win_T_DropDwLine", rcRect);
		}
	}

	private static void Draw_Weapon(Form cForm, float fY, float fWidth, int nValid_Weapons)
	{
		Rect rcRect = default(Rect);
		float num = fWidth / 2f - 75f;
		int num2 = 0;
		for (int i = 1; i < 11; i++)
		{
			float alpha = 1f;
			if ((nValid_Weapons & (int)((short)Math.Pow(2.0, (double)(i - 1)))) == 0)
			{
				alpha = 0.3f;
			}
			string text = "Win_I_Weapon" + i;
			rcRect.x = num + (float)num2 * 38f;
			rcRect.y = fY;
			rcRect.width = 36f;
			rcRect.height = Tooltip_Dlg.F_WEAPON_HEIGHT;
			Tooltip_Dlg.Create_DrawTexture(cForm, text, text, rcRect, alpha);
			num2++;
		}
	}

	private static void Tooltip_Base(Form cForm, Tooltip_Dlg.Tooltip_Text_Info[] saTextInfo, int nRank, G_ID eWidowID, int Valid_Weapons)
	{
		Tooltip_Dlg.Create_Texture item = default(Tooltip_Dlg.Create_Texture);
		List<Tooltip_Dlg.Create_Texture> list = new List<Tooltip_Dlg.Create_Texture>();
		float num = 230f;
		float num2 = 0f;
		float num3 = 0f;
		Rect rcRect = default(Rect);
		num2 += 13f;
		int i = 0;
		while (i < saTextInfo.Length)
		{
			string strName = "ItemInfo_" + i;
			float num4;
			if (saTextInfo[i].m_nTextureType == 3)
			{
				item.m_fY = num2 + saTextInfo[i].m_fY;
				item.m_nRank = nRank;
				num4 = Tooltip_Dlg.F_STAR_HEIGHT + saTextInfo[i].m_fY;
				goto IL_2A9;
			}
			if (saTextInfo[i].m_nTextureType == 4)
			{
				item.m_fY = num2 + saTextInfo[i].m_fY;
				num4 = Tooltip_Dlg.F_WEAPON_HEIGHT + saTextInfo[i].m_fY;
				goto IL_2A9;
			}
			if (saTextInfo[i].m_nTextureType == 1)
			{
				item.m_fY = num2 + 8f - Tooltip_Dlg.F_LINE_TEXTURE_HEIGHT;
				num4 = 16f + Tooltip_Dlg.F_LINE_TEXTURE_HEIGHT;
				goto IL_2A9;
			}
			if (saTextInfo[i].m_nTextureType == 2)
			{
				item.m_fY = num2 + 8f - Tooltip_Dlg.F_LINE_TEXTURE_HEIGHT;
				num4 = 16f + Tooltip_Dlg.F_LINE_TEXTURE_HEIGHT;
				goto IL_2A9;
			}
			if (saTextInfo[i].m_strText != null)
			{
				if (i == 0)
				{
					num4 = 22f;
				}
				else if (i == saTextInfo.Length - 1)
				{
					int num5 = 0;
					if (i == saTextInfo.Length - 1)
					{
						num5 = saTextInfo[i].m_strText.Length / 20;
						num5++;
					}
					num4 = 20f * (float)num5;
				}
				else
				{
					num4 = 20f;
				}
				string strText = saTextInfo[i].m_strText;
				rcRect.x = ((i != 0) ? 13f : 0f);
				rcRect.y = num2;
				rcRect.width = ((i != 0) ? (num - 26f) : num);
				rcRect.height = num4;
				SpriteText.Anchor_Pos ePso = SpriteText.Anchor_Pos.Upper_Left;
				Tooltip_Dlg.s_laText = Tooltip_Dlg.Create_Text(cForm, strName, strText, rcRect, saTextInfo[i].m_fFontSize, ePso, false, NrTSingleton<CTextParser>.Instance.GetTextColor(saTextInfo[i].m_strTextColorCode));
				item.m_laText = Tooltip_Dlg.s_laText;
				if (num3 < Tooltip_Dlg.s_laText.spriteText.TotalWidth)
				{
					num3 = Tooltip_Dlg.s_laText.spriteText.TotalWidth;
				}
				num4 = Tooltip_Dlg.s_laText.spriteText.TotalHeight + 5f;
				goto IL_2A9;
			}
			IL_2DD:
			i++;
			continue;
			IL_2A9:
			item.m_nTextureType = saTextInfo[i].m_nTextureType;
			item.m_eAnchor = saTextInfo[i].m_eAnchor;
			list.Add(item);
			num2 += num4;
			goto IL_2DD;
		}
		num2 += 13f;
		if (num3 < Tooltip_Dlg.F_STAR_MAX_WIDTH)
		{
			num3 = Tooltip_Dlg.F_STAR_MAX_WIDTH + 26f;
		}
		float num6 = num3 + 26f;
		cForm.SetSize((int)num6, (int)num2);
		for (int j = 0; j < list.Count; j++)
		{
			switch (list[j].m_nTextureType)
			{
			case 0:
				if (list[j].m_eAnchor == SpriteText.Anchor_Pos.Middle_Center)
				{
					float x = num6 / 2f - list[j].m_laText.spriteText.TotalWidth / 2f;
					list[j].m_laText.SetLocation(x, list[j].m_laText.GetLocationY());
				}
				break;
			case 1:
				Tooltip_Dlg.Draw_Line(cForm, list[j].m_fY, num6, false);
				break;
			case 2:
				Tooltip_Dlg.Draw_Line(cForm, list[j].m_fY, num6, true);
				break;
			case 3:
				Tooltip_Dlg.Draw_Star(cForm, list[j].m_fY, num6, list[j].m_nRank);
				break;
			case 4:
				Tooltip_Dlg.Draw_Weapon(cForm, list[j].m_fY, num6, Valid_Weapons);
				break;
			}
		}
	}

	public static Tooltip_Dlg.Tooltip_Text_Info[] Get_Item_Text_Info(ITEM pkItem, ITEM pkEquipedItem, G_ID eWidowID, bool bEquiped)
	{
		int level = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1).GetLevel(0L);
		int nItemUnique = pkItem.m_nItemUnique;
		int num;
		if (pkItem.m_nItemID > 0L)
		{
			num = pkItem.m_nDurability;
		}
		else
		{
			num = 100;
		}
		string strTextColorCode = "1101";
		string strTextColorCode2 = "1101";
		string text = "1101";
		string strTextColorCode3 = "1101";
		string strTextColorCode4 = "1104";
		string text2 = "1401";
		string strTextColorCode5 = "1401";
		string strTextColorCode6 = "1304";
		string strTextColorCode7 = "1113";
		int num2 = 21;
		int num3 = 0;
		string text3 = "1101";
		string text4 = "1106";
		string text5 = "1401";
		List<Tooltip_Dlg.Tooltip_Text_Info> list = new List<Tooltip_Dlg.Tooltip_Text_Info>();
		Tooltip_Dlg.Tooltip_Text_Info item = default(Tooltip_Dlg.Tooltip_Text_Info);
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(nItemUnique);
		if (itemInfo != null)
		{
			item.m_strText = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(pkItem);
			item.m_eAnchor = SpriteText.Anchor_Pos.Middle_Center;
			item.m_fFontSize = 22f;
			item.m_strTextColorCode = strTextColorCode;
			list.Add(item);
			item.m_fFontSize = 22f;
			string strText = string.Empty;
			if (bEquiped)
			{
				strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1479");
				item.m_strText = strText;
				item.m_strTextColorCode = strTextColorCode7;
				list.Add(item);
			}
			item.m_eAnchor = SpriteText.Anchor_Pos.Middle_Left;
			item.m_nTextureType = 1;
			list.Add(item);
			item.m_nTextureType = 0;
			int count = list.Count;
			ITEMTYPE_INFO itemTypeInfo = NrTSingleton<ItemManager>.Instance.GetItemTypeInfo(nItemUnique);
			int useMinLevel = itemInfo.GetUseMinLevel(pkItem);
			if (itemTypeInfo != null)
			{
				item.m_strText = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1257"), NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(itemTypeInfo.TEXTKEY));
				item.m_strTextColorCode = ((useMinLevel <= level) ? text : text2);
				list.Add(item);
			}
			if (pkItem.m_nPosType == 5 || pkItem.m_nPosType == 6 || pkItem.m_nPosType == 7)
			{
				item.m_strText = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("265"), " ", pkItem.m_nItemNum.ToString());
				list.Add(item);
			}
			item.m_strText = ((useMinLevel > 0) ? (NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1258") + useMinLevel) : null);
			item.m_strTextColorCode = ((useMinLevel <= level) ? text : text2);
			list.Add(item);
			item.m_strText = ((itemInfo.m_nUseMaxLevel > 0) ? (NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1259") + itemInfo.m_nUseMaxLevel) : null);
			item.m_strTextColorCode = ((itemInfo.m_nUseMaxLevel >= level) ? text : text2);
			list.Add(item);
			if (!NrTSingleton<ItemManager>.Instance.IsItemATB(nItemUnique, 2L))
			{
				item.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("159");
				item.m_strTextColorCode = text2;
				list.Add(item);
			}
			else
			{
				item.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("158");
				item.m_strTextColorCode = strTextColorCode4;
				list.Add(item);
			}
			if (NrTSingleton<ItemManager>.Instance.IsItemATB(nItemUnique, 4L))
			{
				item.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("160");
				item.m_strTextColorCode = strTextColorCode4;
				list.Add(item);
			}
			for (int i = count; i < list.Count; i++)
			{
				if (list[i].m_strText != null)
				{
					item.m_nTextureType = 1;
					list.Add(item);
					item.m_nTextureType = 0;
					break;
				}
			}
			count = list.Count;
			int num4 = 0;
			int num5 = 0;
			if (pkEquipedItem != null)
			{
				int nValue = Protocol_Item.Get_Min_Damage(pkEquipedItem);
				num4 = Tooltip_Dlg.GetOptionValue(pkEquipedItem, nValue, 1);
				int nValue2 = Protocol_Item.Get_Max_Damage(pkEquipedItem);
				num5 = Tooltip_Dlg.GetOptionValue(pkEquipedItem, nValue2, 1);
			}
			int num6 = Protocol_Item.Get_Min_Damage(pkItem);
			int optionValue = Tooltip_Dlg.GetOptionValue(pkItem, num6, 1);
			int nValue3 = Protocol_Item.Get_Max_Damage(pkItem);
			int optionValue2 = Tooltip_Dlg.GetOptionValue(pkItem, nValue3, 1);
			bool flag = false;
			int num7 = optionValue - num4;
			int num8 = optionValue2 - num5;
			if (num7 != 0 || num8 != 0)
			{
				flag = true;
			}
			if (!bEquiped && flag && pkEquipedItem != null)
			{
				string text6 = string.Empty;
				if (num8 < 0)
				{
					item.m_strTextColorCode = strTextColorCode5;
					text6 = NrTSingleton<UIDataManager>.Instance.GetString("(", num7.ToString(), " ~ ", num8.ToString(), ")");
				}
				else
				{
					item.m_strTextColorCode = strTextColorCode6;
					text6 = string.Concat(new string[]
					{
						"(+",
						num7.ToString(),
						" ~ +",
						num8.ToString(),
						")"
					});
				}
				item.m_strText = ((itemInfo.m_nMinDamage > 0 && itemInfo.m_nMaxDamage > 0) ? string.Concat(new string[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1263"),
					optionValue.ToString(),
					" ~ ",
					optionValue2.ToString(),
					" ",
					text6
				}) : null);
			}
			else
			{
				item.m_strText = ((itemInfo.m_nMinDamage > 0 && itemInfo.m_nMaxDamage > 0) ? NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1263"), optionValue.ToString(), " ~ ", optionValue2.ToString()) : null);
				item.m_strTextColorCode = strTextColorCode3;
			}
			list.Add(item);
			if (pkEquipedItem != null)
			{
				int nValue = Protocol_Item.Get_Defense(pkEquipedItem);
				num4 = Tooltip_Dlg.GetOptionValue(pkEquipedItem, nValue, 2);
			}
			num6 = Protocol_Item.Get_Defense(pkItem);
			optionValue = Tooltip_Dlg.GetOptionValue(pkItem, num6, 2);
			num7 = optionValue - num4;
			if (!bEquiped && num7 != 0 && pkEquipedItem != null)
			{
				string text7 = string.Empty;
				if (num7 < 0)
				{
					item.m_strTextColorCode = strTextColorCode5;
					text7 = NrTSingleton<UIDataManager>.Instance.GetString("(", num7.ToString(), ")");
				}
				else
				{
					item.m_strTextColorCode = strTextColorCode6;
					text7 = NrTSingleton<UIDataManager>.Instance.GetString("(", "+", num7.ToString(), ")");
				}
				item.m_strText = ((itemInfo.m_nDefense > 0) ? string.Concat(new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1264"),
					optionValue,
					" ",
					text7
				}) : null);
			}
			else
			{
				item.m_strText = ((itemInfo.m_nDefense > 0) ? (NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1264") + optionValue) : null);
				item.m_strTextColorCode = strTextColorCode3;
			}
			list.Add(item);
			if (pkEquipedItem != null)
			{
				int nValue = Protocol_Item.Get_ADDHP(pkEquipedItem);
				num4 = Tooltip_Dlg.GetOptionValue(pkEquipedItem, nValue, 4);
			}
			num6 = Protocol_Item.Get_ADDHP(pkItem);
			optionValue = Tooltip_Dlg.GetOptionValue(pkItem, num6, 4);
			num7 = optionValue - num4;
			if (!bEquiped && num7 != 0 && pkEquipedItem != null)
			{
				string text8 = string.Empty;
				if (num7 < 0)
				{
					item.m_strTextColorCode = strTextColorCode5;
					text8 = NrTSingleton<UIDataManager>.Instance.GetString("(", num7.ToString(), ")");
				}
				else
				{
					item.m_strTextColorCode = strTextColorCode6;
					text8 = NrTSingleton<UIDataManager>.Instance.GetString("(", "+", num7.ToString(), ")");
				}
				item.m_strText = ((itemInfo.m_nAddHP > 0) ? string.Concat(new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1265"),
					optionValue,
					" ",
					text8
				}) : null);
			}
			else
			{
				item.m_strText = ((itemInfo.m_nAddHP > 0) ? (NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1265") + optionValue) : null);
				item.m_strTextColorCode = strTextColorCode3;
			}
			list.Add(item);
			num6 = Protocol_Item.Get_STR(pkItem);
			item.m_strText = ((itemInfo.m_nSTR > 0) ? (NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1271") + num6) : null);
			item.m_strTextColorCode = strTextColorCode3;
			list.Add(item);
			num6 = Protocol_Item.Get_DEX(pkItem);
			item.m_strText = ((itemInfo.m_nDEX > 0) ? (NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1272") + num6) : null);
			item.m_strTextColorCode = strTextColorCode3;
			list.Add(item);
			num6 = Protocol_Item.Get_INT(pkItem);
			item.m_strText = ((itemInfo.m_nINT > 0) ? (NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1274") + num6) : null);
			item.m_strTextColorCode = strTextColorCode3;
			list.Add(item);
			num6 = Protocol_Item.Get_VIT(pkItem);
			item.m_strText = ((itemInfo.m_nVIT > 0) ? (NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1263") + num6) : null);
			item.m_strTextColorCode = strTextColorCode3;
			list.Add(item);
			if (pkEquipedItem != null)
			{
				int nValue = Protocol_Item.Get_Critical_Plus(pkEquipedItem);
				num4 = Tooltip_Dlg.GetOptionValue(pkEquipedItem, nValue, 3);
			}
			num6 = Protocol_Item.Get_Critical_Plus(pkItem);
			optionValue = Tooltip_Dlg.GetOptionValue(pkItem, num6, 3);
			num7 = optionValue - num4;
			if (!bEquiped && num7 != 0 && pkEquipedItem != null)
			{
				string text9 = string.Empty;
				if (num7 < 0)
				{
					item.m_strTextColorCode = strTextColorCode5;
					text9 = NrTSingleton<UIDataManager>.Instance.GetString("(", num7.ToString(), ")");
				}
				else
				{
					item.m_strTextColorCode = strTextColorCode6;
					text9 = NrTSingleton<UIDataManager>.Instance.GetString("(", "+", num7.ToString(), ")");
				}
				item.m_strText = ((itemInfo.m_nCriticalPlus > 0) ? string.Concat(new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1267"),
					optionValue,
					" ",
					text9
				}) : null);
			}
			else
			{
				item.m_strText = ((itemInfo.m_nCriticalPlus > 0) ? (NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1267") + optionValue) : null);
				item.m_strTextColorCode = strTextColorCode3;
			}
			list.Add(item);
			num6 = Protocol_Item.Get_AttackSpeed(pkItem);
			item.m_strText = ((itemInfo.m_nAttackSpeed > 0) ? (NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1276") + num6) : null);
			item.m_strTextColorCode = strTextColorCode3;
			list.Add(item);
			if (pkEquipedItem != null)
			{
				int nValue = Protocol_Item.Get_Hitrate_Plus(pkEquipedItem);
				num4 = Tooltip_Dlg.GetOptionValue(pkEquipedItem, nValue, 6);
			}
			num6 = Protocol_Item.Get_Hitrate_Plus(pkItem);
			optionValue = Tooltip_Dlg.GetOptionValue(pkItem, num6, 6);
			num7 = optionValue - num4;
			if (!bEquiped && num7 != 0 && pkEquipedItem != null)
			{
				string text10 = string.Empty;
				if (num7 < 0)
				{
					item.m_strTextColorCode = strTextColorCode5;
					text10 = NrTSingleton<UIDataManager>.Instance.GetString("(", num7.ToString(), ")");
				}
				else
				{
					item.m_strTextColorCode = strTextColorCode6;
					text10 = NrTSingleton<UIDataManager>.Instance.GetString("(", "+", num7.ToString(), ")");
				}
				item.m_strText = ((itemInfo.m_nHitratePlus > 0) ? string.Concat(new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("173"),
					optionValue,
					" ",
					text10
				}) : null);
			}
			else
			{
				item.m_strText = ((itemInfo.m_nHitratePlus > 0) ? (NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("173") + optionValue) : null);
				item.m_strTextColorCode = strTextColorCode3;
			}
			list.Add(item);
			if (pkEquipedItem != null)
			{
				int nValue = Protocol_Item.Get_Evasion_Plus(pkEquipedItem);
				num4 = Tooltip_Dlg.GetOptionValue(pkEquipedItem, nValue, 7);
			}
			num6 = Protocol_Item.Get_Evasion_Plus(pkItem);
			optionValue = Tooltip_Dlg.GetOptionValue(pkItem, num6, 7);
			num7 = optionValue - num4;
			if (!bEquiped && num7 != 0 && pkEquipedItem != null)
			{
				string text11 = string.Empty;
				if (num7 < 0)
				{
					item.m_strTextColorCode = strTextColorCode5;
					text11 = NrTSingleton<UIDataManager>.Instance.GetString("(", num7.ToString(), ")");
				}
				else
				{
					item.m_strTextColorCode = strTextColorCode6;
					text11 = NrTSingleton<UIDataManager>.Instance.GetString("(", "+", num7.ToString(), ")");
				}
				item.m_strText = ((itemInfo.m_nEvasionPlus > 0) ? string.Concat(new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("174"),
					optionValue,
					" ",
					text11
				}) : null);
			}
			else
			{
				item.m_strText = ((itemInfo.m_nEvasionPlus > 0) ? (NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("174") + optionValue) : null);
				item.m_strTextColorCode = strTextColorCode3;
			}
			list.Add(item);
			num6 = Protocol_Item.Get_Move_Speed(pkItem);
			item.m_strText = ((itemInfo.m_nMoveSpeed > 0) ? (NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("175") + num6) : null);
			item.m_strTextColorCode = strTextColorCode3;
			list.Add(item);
			item.m_strText = ((itemInfo.m_nDurability > 0) ? string.Concat(new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("176"),
				num,
				" / ",
				itemInfo.m_nDurability
			}) : null);
			item.m_strTextColorCode = ((num < num2) ? ((num > num3) ? text4 : text5) : text3);
			list.Add(item);
			for (int j = count; j < list.Count; j++)
			{
				if (list[j].m_strText != null)
				{
					item.m_nTextureType = 1;
					list.Add(item);
					item.m_nTextureType = 0;
					break;
				}
			}
			count = list.Count;
			for (int k = count; k < list.Count; k++)
			{
				if (list[k].m_strText != null)
				{
					item.m_nTextureType = 1;
					list.Add(item);
					item.m_nTextureType = 0;
					break;
				}
			}
			string empty = string.Empty;
			string empty2 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				empty2
			});
			item.m_eAnchor = SpriteText.Anchor_Pos.Upper_Left;
			item.m_strText = empty;
			item.m_strTextColorCode = strTextColorCode2;
			list.Add(item);
			return list.ToArray();
		}
		return null;
	}

	public static int GetOptionValue(ITEM pkItem, int nValue, int OptionType)
	{
		int optionValue = 0;
		if (OptionType == Protocol_Item.GetStaticItemOption(pkItem.m_nItemUnique, 0))
		{
			optionValue = pkItem.m_nOption[0];
		}
		if (OptionType == Protocol_Item.GetStaticItemOption(pkItem.m_nItemUnique, 1))
		{
			optionValue = pkItem.m_nOption[1];
		}
		return Tooltip_Dlg.GetOptionValue(pkItem.m_nItemUnique, optionValue, nValue, OptionType);
	}

	public static int GetOptionValue(int _nItemUnique, int _OptionValue, int nValue, int OptionType)
	{
		if (_OptionValue != 0)
		{
			nValue = (int)((float)nValue * ((float)_OptionValue / 100f));
		}
		return nValue;
	}

	public static int GetOptionValue(int _OptionValue, int nValue)
	{
		if (_OptionValue != 0)
		{
			nValue = (int)((float)nValue * ((float)_OptionValue / 100f));
		}
		return nValue;
	}

	public static Tooltip_Dlg.Tooltip_Text_Info[] GetSkillTextInfo(int battleSkillUnique, int battleSkillLevel, G_ID eWidowID, NkSoldierInfo solInfo)
	{
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(battleSkillUnique);
		BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(battleSkillUnique, battleSkillLevel);
		BATTLESKILL_DETAIL battleSkillDetail2 = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(battleSkillUnique, battleSkillLevel + 1);
		if (battleSkillDetail == null || battleSkillBase == null)
		{
			return null;
		}
		if (solInfo == null)
		{
			return null;
		}
		List<Tooltip_Dlg.Tooltip_Text_Info> list = new List<Tooltip_Dlg.Tooltip_Text_Info>();
		Tooltip_Dlg.Tooltip_Text_Info item = default(Tooltip_Dlg.Tooltip_Text_Info);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1292"),
			"skillname",
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey),
			"skilllevel",
			battleSkillLevel
		});
		item.m_strText = empty;
		item.m_eAnchor = SpriteText.Anchor_Pos.Middle_Center;
		item.m_fFontSize = 22f;
		item.m_strTextColorCode = "1101";
		list.Add(item);
		item.m_fFontSize = 22f;
		item.m_eAnchor = SpriteText.Anchor_Pos.Middle_Left;
		item.m_nTextureType = 1;
		list.Add(item);
		item.m_nTextureType = 0;
		int count = list.Count;
		empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceBattleSkillParam(ref empty, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillDetail.m_nSkillTooltip), battleSkillDetail, solInfo, -1);
		item.m_strText = empty;
		list.Add(item);
		empty = string.Empty;
		if (battleSkillBase.m_nSkillType == 2)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1506")
			});
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1297"),
				"count",
				battleSkillDetail.m_nSkillNeedAngerlyPoint
			});
		}
		item.m_strText = empty;
		list.Add(item);
		if (!battleSkillBase.ChecJobTypeMagicDamage())
		{
			item.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1352");
		}
		else
		{
			item.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1353");
		}
		list.Add(item);
		for (int i = count; i < list.Count; i++)
		{
			if (list[i].m_strText != null)
			{
				item.m_nTextureType = 1;
				list.Add(item);
				item.m_nTextureType = 0;
				break;
			}
		}
		if (battleSkillDetail2 != null)
		{
			item.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1298");
			list.Add(item);
			if (battleSkillDetail2 != null)
			{
				empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceBattleSkillParam(ref empty, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillDetail2.m_nSkillTooltip), battleSkillDetail2, solInfo, -1);
				item.m_strText = empty;
				list.Add(item);
			}
			empty = string.Empty;
			if (battleSkillBase.m_nSkillType == 2)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1506")
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1297"),
					"count",
					battleSkillDetail.m_nSkillNeedAngerlyPoint
				});
			}
			item.m_strText = empty;
			list.Add(item);
		}
		else if (battleSkillBase.m_nSkillMaxLevel == 30)
		{
			item.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1299");
			list.Add(item);
		}
		for (int j = count; j < list.Count; j++)
		{
			if (list[j].m_strText != null)
			{
				item.m_nTextureType = 1;
				list.Add(item);
				item.m_nTextureType = 0;
				break;
			}
		}
		return list.ToArray();
	}

	public static Tooltip_Dlg.Tooltip_Text_Info[] GetBattleControlSkillTextInfo(int battleSkillUnique, int battleSkillLevel, G_ID eWidowID, int KeepTurn, NkSoldierInfo solInfo)
	{
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(battleSkillUnique);
		BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(battleSkillUnique, battleSkillLevel);
		if (battleSkillDetail == null || battleSkillBase == null)
		{
			return null;
		}
		List<Tooltip_Dlg.Tooltip_Text_Info> list = new List<Tooltip_Dlg.Tooltip_Text_Info>();
		Tooltip_Dlg.Tooltip_Text_Info item = default(Tooltip_Dlg.Tooltip_Text_Info);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1292"),
			"skillname",
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey),
			"skilllevel",
			battleSkillLevel
		});
		item.m_strText = empty;
		item.m_eAnchor = SpriteText.Anchor_Pos.Middle_Center;
		item.m_fFontSize = 22f;
		item.m_strTextColorCode = "1101";
		list.Add(item);
		item.m_fFontSize = 22f;
		item.m_eAnchor = SpriteText.Anchor_Pos.Middle_Left;
		item.m_nTextureType = 1;
		list.Add(item);
		item.m_nTextureType = 0;
		empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceBattleSkillParam(ref empty, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillDetail.m_nSkillTooltip), battleSkillDetail, solInfo, -1);
		item.m_strText = empty;
		list.Add(item);
		empty = string.Empty;
		if (battleSkillBase.m_nSkillType == 2)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1506")
			});
		}
		else
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1297"),
				"count",
				battleSkillDetail.m_nSkillNeedAngerlyPoint
			});
		}
		item.m_strText = empty;
		list.Add(item);
		if (!battleSkillBase.ChecJobTypeMagicDamage())
		{
			item.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1352");
		}
		else
		{
			item.m_strText = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1353");
		}
		list.Add(item);
		return list.ToArray();
	}
}
