using GAME;
using System;
using UnityEngine;

public class NkCharPartControl
{
	private Nr3DCharBase m_pkTarget3DChar;

	private NrCharKindInfo m_pkTargetKind;

	private bool m_bOnlyWeaponControl;

	private NrCharPartInfo m_kCharPartInfo;

	private bool m_bReadyPartInfo;

	public NkCharPartControl()
	{
		this.m_kCharPartInfo = new NrCharPartInfo();
		this.Init();
	}

	public void Init()
	{
		this.m_pkTarget3DChar = null;
		this.m_pkTargetKind = null;
		this.m_bOnlyWeaponControl = false;
		this.m_kCharPartInfo.Init();
		this.m_bReadyPartInfo = false;
	}

	public void SetPartControl(Nr3DCharBase pk3DChar, bool bOnlyWeapon)
	{
		this.m_pkTarget3DChar = pk3DChar;
		this.m_bOnlyWeaponControl = bOnlyWeapon;
	}

	public void SetCharKindInfo(NrCharKindInfo pkCharKindInfo)
	{
		this.m_pkTargetKind = pkCharKindInfo;
	}

	public void SetPartInfo(NrCharPartInfo pkPartInfo)
	{
		this.m_kCharPartInfo.Set(pkPartInfo);
		this.m_bReadyPartInfo = true;
	}

	public void CollectPartInfo(NrCharBasePart pkBasePart, NrEquipItemInfo pkEquipInfo)
	{
		this.m_kCharPartInfo.m_kBasePart.SetData(pkBasePart);
		this.m_kCharPartInfo.m_kEquipPart.SetData(pkEquipInfo);
		this.m_bReadyPartInfo = true;
	}

	public bool IsReadyPartInfo()
	{
		return this.m_bReadyPartInfo;
	}

	public string GetPartFileName(string strPartPath, string strPartCode, bool bBasePart, int itemunique)
	{
		string text = "000";
		if (!bBasePart)
		{
			ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(itemunique);
			if (itemInfo != null)
			{
				text = itemInfo.m_strModelPath;
			}
		}
		else
		{
			text = itemunique.ToString();
		}
		text = NkUtil.MakeCode(text);
		return string.Format("{0}/{1}_{2}_{3}", new object[]
		{
			strPartPath,
			this.m_pkTargetKind.GetBundlePath(),
			strPartCode,
			text
		});
	}

	public string GetItemFileName(NrCharDefine.eAT2ItemAssetBundle eItemIndex)
	{
		if (this.m_pkTargetKind == null)
		{
			return string.Empty;
		}
		string result = string.Empty;
		int weapontype = this.m_pkTargetKind.GetWeaponType();
		int data = this.m_kCharPartInfo.m_kEquipPart.GetData(eAT2CharEquipPart.CHAREQUIPPART_WEAPON1);
		string arg = "001";
		NkWeaponTypeInfo nkWeaponTypeInfo = null;
		if (eItemIndex != NrCharDefine.eAT2ItemAssetBundle.weapon1)
		{
			if (eItemIndex == NrCharDefine.eAT2ItemAssetBundle.weapon2)
			{
				if (data > 0)
				{
					ITEMTYPE_INFO itemTypeInfo = NrTSingleton<ItemManager>.Instance.GetItemTypeInfo(data);
					if (itemTypeInfo != null)
					{
						weapontype = itemTypeInfo.WEAPONTYPE;
					}
					ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(data);
					if (itemInfo != null)
					{
						arg = NkUtil.MakeCode(itemInfo.m_strModelPath);
					}
				}
				nkWeaponTypeInfo = NrTSingleton<NkWeaponTypeInfoManager>.Instance.GetWeaponTypeInfo(weapontype);
				if (nkWeaponTypeInfo != null)
				{
					if (!nkWeaponTypeInfo.IsATB(8L))
					{
						weapontype = 0;
					}
					if (this.m_pkTargetKind.IsATB(1L) && this.m_pkTargetKind.GetCharTribe() == 2 && this.m_pkTargetKind.GetGender() == 1)
					{
						if (!nkWeaponTypeInfo.IsATB(16L))
						{
							weapontype = 0;
						}
						else
						{
							weapontype = 9;
						}
					}
					nkWeaponTypeInfo = NrTSingleton<NkWeaponTypeInfoManager>.Instance.GetWeaponTypeInfo(weapontype);
				}
			}
		}
		else
		{
			if (data > 0)
			{
				ITEMTYPE_INFO itemTypeInfo2 = NrTSingleton<ItemManager>.Instance.GetItemTypeInfo(data);
				if (itemTypeInfo2 != null)
				{
					weapontype = itemTypeInfo2.WEAPONTYPE;
				}
				ITEMINFO itemInfo2 = NrTSingleton<ItemManager>.Instance.GetItemInfo(data);
				if (itemInfo2 != null)
				{
					arg = NkUtil.MakeCode(itemInfo2.m_strModelPath);
				}
			}
			nkWeaponTypeInfo = NrTSingleton<NkWeaponTypeInfoManager>.Instance.GetWeaponTypeInfo(weapontype);
		}
		if (nkWeaponTypeInfo != null)
		{
			string code = nkWeaponTypeInfo.GetCode();
			result = string.Format("{0}/{0}_{1}", code, arg);
		}
		return result;
	}

	public string GetWeaponTargetName(NrCharDefine.eAT2ItemAssetBundle eItemIndex, bool battleani)
	{
		string str = string.Empty;
		int weapontype = this.m_pkTargetKind.GetWeaponType();
		int data = this.m_kCharPartInfo.m_kEquipPart.GetData(eAT2CharEquipPart.CHAREQUIPPART_WEAPON1);
		string str2 = "1";
		NkWeaponTypeInfo weaponTypeInfo;
		if (eItemIndex == NrCharDefine.eAT2ItemAssetBundle.weapon1)
		{
			ITEMTYPE_INFO itemTypeInfo = NrTSingleton<ItemManager>.Instance.GetItemTypeInfo(data);
			if (itemTypeInfo != null)
			{
				weapontype = itemTypeInfo.WEAPONTYPE;
			}
			weaponTypeInfo = NrTSingleton<NkWeaponTypeInfoManager>.Instance.GetWeaponTypeInfo(weapontype);
		}
		else
		{
			str2 = "2";
			if (data > 0)
			{
				ITEMTYPE_INFO itemTypeInfo2 = NrTSingleton<ItemManager>.Instance.GetItemTypeInfo(data);
				if (itemTypeInfo2 != null)
				{
					weapontype = itemTypeInfo2.WEAPONTYPE;
				}
			}
			weaponTypeInfo = NrTSingleton<NkWeaponTypeInfoManager>.Instance.GetWeaponTypeInfo(weapontype);
			if (weaponTypeInfo != null)
			{
				if (!weaponTypeInfo.IsATB(8L))
				{
					weapontype = 0;
				}
				if (this.m_pkTargetKind.IsATB(1L) && this.m_pkTargetKind.GetCharTribe() == 2 && this.m_pkTargetKind.GetGender() == 1)
				{
					if (!weaponTypeInfo.IsATB(16L))
					{
						weapontype = 0;
					}
					else
					{
						weapontype = 9;
						if (!battleani)
						{
							str2 = "1";
						}
					}
				}
				weaponTypeInfo = NrTSingleton<NkWeaponTypeInfoManager>.Instance.GetWeaponTypeInfo(weapontype);
			}
		}
		if (weaponTypeInfo == null)
		{
			str = "dmweapon";
		}
		else if (!battleani)
		{
			str = weaponTypeInfo.GetBackDummy();
		}
		else
		{
			str = weaponTypeInfo.GetBattleDummy();
			if (weaponTypeInfo.IsATB(4L))
			{
				if (eItemIndex == NrCharDefine.eAT2ItemAssetBundle.weapon1)
				{
					str2 = "2";
				}
				else if (eItemIndex == NrCharDefine.eAT2ItemAssetBundle.weapon2)
				{
					str2 = "1";
				}
			}
		}
		return str + str2;
	}

	public string GetAttachTargetName(NrCharDefine.eAT2ItemAssetBundle eItemIndex)
	{
		string result = string.Empty;
		switch (eItemIndex)
		{
		case NrCharDefine.eAT2ItemAssetBundle.hair:
		case NrCharDefine.eAT2ItemAssetBundle.face:
		case NrCharDefine.eAT2ItemAssetBundle.helmet:
			result = "dmhead";
			break;
		case NrCharDefine.eAT2ItemAssetBundle.weapon1:
		case NrCharDefine.eAT2ItemAssetBundle.weapon2:
		{
			bool battleani = true;
			if (!this.m_pkTarget3DChar.IsBattleChar())
			{
				battleani = false;
				if (this.m_pkTargetKind.IsATB(2L))
				{
					battleani = true;
				}
				else if (this.m_pkTargetKind.GetCharTribe() == 4)
				{
					battleani = true;
				}
				else if (this.m_pkTarget3DChar.GetParentCharAnimation() == null)
				{
					Debug.LogWarning("===> Attack Weapon Target Error (" + eItemIndex.ToString() + ") : " + this.m_pkTarget3DChar.GetName());
				}
				else
				{
					eCharAnimationType currentAniType = this.m_pkTarget3DChar.GetParentCharAnimation().GetCurrentAniType();
					battleani = NrCharAnimation.IsBattleAnimation(currentAniType);
				}
			}
			result = this.GetWeaponTargetName(eItemIndex, battleani);
			break;
		}
		case NrCharDefine.eAT2ItemAssetBundle.decoration:
			result = "dmback";
			break;
		case NrCharDefine.eAT2ItemAssetBundle.bodyitem:
			result = "dmbody";
			break;
		case NrCharDefine.eAT2ItemAssetBundle.centeritem:
			result = "dmcenter";
			break;
		}
		return result;
	}

	public void ChangeWeaponTarget()
	{
		if (this.m_pkTarget3DChar == null)
		{
			return;
		}
		string attachTargetName = this.GetAttachTargetName(NrCharDefine.eAT2ItemAssetBundle.weapon1);
		string attachTargetName2 = this.GetAttachTargetName(NrCharDefine.eAT2ItemAssetBundle.weapon2);
		this.m_pkTarget3DChar.ChangeWeaponTarget(attachTargetName, attachTargetName2);
	}

	public Vector3 GetAttachItemScale(eAT2CharEquipPart eEquipPart)
	{
		Vector3 result = new Vector3(1f, 1f, 1f);
		return result;
	}

	public void SetCharPart(NrCharDefine.eAT2CharPartInfo eCharPart)
	{
		if (this.m_pkTarget3DChar == null)
		{
			return;
		}
		if (this.m_bOnlyWeaponControl)
		{
			return;
		}
		string strPartPath = eCharPart.ToString().ToLower().Substring(9);
		string filename = string.Empty;
		string strPartCode = string.Empty;
		bool flag = false;
		bool bBasePart = false;
		int num = 0;
		int num2 = 0;
		switch (eCharPart)
		{
		case NrCharDefine.eAT2CharPartInfo.CHARPART_HEAD:
			if (TsPlatform.IsMobile)
			{
				return;
			}
			num2 = this.m_kCharPartInfo.m_kEquipPart.GetData(eAT2CharEquipPart.CHAREQUIPPART_HELMET);
			if (num2 > 0)
			{
				strPartCode = "helmet";
				ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(num2);
				if (itemInfo != null)
				{
					flag = itemInfo.IsCharTribe(this.m_pkTargetKind.GetCharTribe());
				}
				if (flag)
				{
					num2 = this.m_kCharPartInfo.m_kBasePart.GetData(eAT2CharBasePart.CHARBASEPART_HAIR);
					strPartCode = "hair";
					bBasePart = true;
				}
			}
			else
			{
				num2 = this.m_kCharPartInfo.m_kBasePart.GetData(eAT2CharBasePart.CHARBASEPART_HAIR);
				strPartCode = "hair";
				bBasePart = true;
			}
			this.m_pkTarget3DChar.RemoveItem(2, true);
			flag = true;
			num = 0;
			break;
		case NrCharDefine.eAT2CharPartInfo.CHARPART_FACE:
			if (TsPlatform.IsMobile)
			{
				return;
			}
			num2 = this.m_kCharPartInfo.m_kBasePart.GetData(eAT2CharBasePart.CHARBASEPART_FACE);
			strPartCode = "face";
			flag = true;
			bBasePart = true;
			num = 1;
			break;
		case NrCharDefine.eAT2CharPartInfo.CHARPART_HELMET:
			if (TsPlatform.IsMobile)
			{
				return;
			}
			num2 = this.m_kCharPartInfo.m_kEquipPart.GetData(eAT2CharEquipPart.CHAREQUIPPART_HELMET);
			if (num2 > 0)
			{
				strPartCode = "helmet";
			}
			else
			{
				strPartCode = "helmet";
			}
			strPartPath = "head";
			num = 0;
			break;
		case NrCharDefine.eAT2CharPartInfo.CHARPART_BODY:
			num2 = this.m_kCharPartInfo.m_kEquipPart.GetData(eAT2CharEquipPart.CHAREQUIPPART_ARMOR);
			if (num2 > 0)
			{
				strPartCode = "body";
			}
			else
			{
				strPartCode = "body";
			}
			num = 1;
			break;
		case NrCharDefine.eAT2CharPartInfo.CHARPART_GLOVE:
			if (TsPlatform.IsMobile)
			{
				return;
			}
			num2 = this.m_kCharPartInfo.m_kEquipPart.GetData(eAT2CharEquipPart.CHAREQUIPPART_GLOVE);
			if (num2 > 0)
			{
				strPartCode = "glove";
			}
			else
			{
				strPartCode = "glove";
			}
			num = 2;
			break;
		case NrCharDefine.eAT2CharPartInfo.CHARPART_BOOTS:
			if (TsPlatform.IsMobile)
			{
				return;
			}
			num2 = this.m_kCharPartInfo.m_kEquipPart.GetData(eAT2CharEquipPart.CHAREQUIPPART_BOOTS);
			if (num2 > 0)
			{
				strPartCode = "boots";
			}
			else
			{
				strPartCode = "boots";
			}
			num = 3;
			break;
		}
		filename = this.GetPartFileName(strPartPath, strPartCode, bBasePart, num2);
		if (!flag)
		{
			this.SetSwitchPart((NrCharDefine.eAT2PartAssetBundle)num, filename);
		}
		else
		{
			this.SetAttachPart((NrCharDefine.eAT2ItemAssetBundle)num, this.GetAttachTargetName((NrCharDefine.eAT2ItemAssetBundle)num), filename);
		}
	}

	public void SetSwitchPart(NrCharDefine.eAT2PartAssetBundle partindex, string filename)
	{
		Nr3DCharActor nr3DCharActor = this.m_pkTarget3DChar as Nr3DCharActor;
		if (nr3DCharActor == null)
		{
			return;
		}
		nr3DCharActor.SetSwitchPart(partindex, filename);
	}

	public void SetAttachPart(NrCharDefine.eAT2ItemAssetBundle partindex, string targetname, string filename)
	{
		this.m_pkTarget3DChar.SetAttachItem(partindex, targetname, filename);
	}

	public void SetAttachItem(NrCharDefine.eAT2ItemAssetBundle itemindex)
	{
		string itemFileName = this.GetItemFileName(itemindex);
		this.m_pkTarget3DChar.RemoveItem((int)itemindex, true);
		if (itemFileName.Length > 0)
		{
			this.m_pkTarget3DChar.SetAttachItem(itemindex, this.GetAttachTargetName(itemindex), itemFileName);
		}
	}

	public void SetCharEquipPart(NrCharEquipPart kNewEquipPart)
	{
		for (int i = 0; i < 6; i++)
		{
			if (kNewEquipPart.m_nPartUnit[i] != this.m_kCharPartInfo.m_kEquipPart.m_nPartUnit[i])
			{
				this.m_kCharPartInfo.m_kEquipPart.m_nPartUnit[i] = kNewEquipPart.m_nPartUnit[i];
			}
		}
	}

	public void ChangeBasePart()
	{
		if (this.m_pkTarget3DChar == null)
		{
			return;
		}
		if (this.m_bOnlyWeaponControl)
		{
			return;
		}
		if (TsPlatform.IsMobile)
		{
			return;
		}
		this.SetCharPart(NrCharDefine.eAT2CharPartInfo.CHARPART_HELMET);
	}

	public void SetWeaponEquipItem()
	{
		if (this.m_pkTarget3DChar == null)
		{
			return;
		}
		this.SetAttachItem(NrCharDefine.eAT2ItemAssetBundle.weapon1);
		this.SetAttachItem(NrCharDefine.eAT2ItemAssetBundle.weapon2);
	}

	public void ChangeEquipItem()
	{
		if (this.m_pkTarget3DChar == null)
		{
			return;
		}
		if (this.m_bOnlyWeaponControl)
		{
			return;
		}
		if (!TsPlatform.IsMobile)
		{
			this.SetCharPart(NrCharDefine.eAT2CharPartInfo.CHARPART_HELMET);
			this.SetCharPart(NrCharDefine.eAT2CharPartInfo.CHARPART_BODY);
			this.SetCharPart(NrCharDefine.eAT2CharPartInfo.CHARPART_GLOVE);
			this.SetCharPart(NrCharDefine.eAT2CharPartInfo.CHARPART_BOOTS);
		}
		this.SetWeaponEquipItem();
	}

	public void OnItemChange(NkSoldierInfo kSoldier, int itemindex, GameObject kGameObj)
	{
		if (kSoldier == null || null == kGameObj)
		{
			return;
		}
		eEQUIP_ITEM itempos = eEQUIP_ITEM.EQUIP_ITEM_MAX;
		if (itemindex == 3)
		{
			itempos = eEQUIP_ITEM.EQUIP_WEAPON1;
		}
		ITEM equipItem = kSoldier.GetEquipItem((int)itempos);
		if (equipItem != null)
		{
			int rank = equipItem.m_nOption[2];
			string strName = ItemManager.RankStateString(rank);
			Transform childContains = NkUtil.GetChildContains(kGameObj.transform, strName);
			if (null != childContains)
			{
				Transform transform = kGameObj.transform;
				childContains.parent = null;
				int childCount = transform.childCount;
				for (int i = 0; i < childCount; i++)
				{
					Transform child = transform.GetChild(0);
					UnityEngine.Object.DestroyImmediate(child.gameObject);
				}
				childContains.parent = transform;
			}
		}
	}

	public void ResetBaseBone()
	{
		Nr3DCharActor nr3DCharActor = this.m_pkTarget3DChar as Nr3DCharActor;
		if (this.m_pkTarget3DChar == null)
		{
			return;
		}
		nr3DCharActor.SetBase();
		if (this.m_bOnlyWeaponControl)
		{
			nr3DCharActor.Reset();
			return;
		}
		if (!TsPlatform.IsMobile)
		{
			this.SetCharPart(NrCharDefine.eAT2CharPartInfo.CHARPART_HELMET);
			this.SetCharPart(NrCharDefine.eAT2CharPartInfo.CHARPART_BODY);
			this.SetCharPart(NrCharDefine.eAT2CharPartInfo.CHARPART_GLOVE);
			this.SetCharPart(NrCharDefine.eAT2CharPartInfo.CHARPART_BOOTS);
		}
		this.SetWeaponEquipItem();
		nr3DCharActor.Reset();
	}
}
