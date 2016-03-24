using CostumeRoomDlg.COSTUME_LIST;
using GAME;
using PROTOCOL;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

namespace CostumeRoomDlg
{
	public class CostumeListSetter
	{
		private CostumeRoom_Dlg _owner;

		private CharCostumeInfo_Data _selectedCostumeData;

		private int _settedCostumeKind;

		private ListItemMover _costumeListMover;

		public CharCostumeInfo_Data _SelectedCostumeData
		{
			get
			{
				return this._selectedCostumeData;
			}
			set
			{
			}
		}

		public CostumeListSetter(CostumeRoom_Dlg owner)
		{
			this._owner = owner;
			this._costumeListMover = new ListItemMover(owner);
		}

		public void InitCostumeListBox(NewListBox costumeListBox, int costumeKind)
		{
			if (costumeListBox == null || costumeKind < 0)
			{
				return;
			}
			this._settedCostumeKind = costumeKind;
			this.SetBaseSelectedCostume();
			List<CharCostumeInfo_Data> costumeDataByKind = this.GetCostumeDataByKind(costumeKind);
			this.SetCostumeListBox(costumeListBox, costumeDataByKind, costumeKind);
			this._costumeListMover.CostumeListScrollToTarget(this._owner._myCharListSetter._SelectedSolInfo);
		}

		public void RefreshCostumeListBox(ref NewListBox costumeListBox)
		{
			for (int i = 0; i < costumeListBox.Count; i++)
			{
				UIListItemContainer item = costumeListBox.GetItem(i);
				if (!(item == null))
				{
					CharCostumeInfo_Data charCostumeInfo_Data = (CharCostumeInfo_Data)item.data;
					if (charCostumeInfo_Data != null)
					{
						NewListItem item2 = new NewListItem(costumeListBox.ColumnNum, true, string.Empty);
						this.SetCostumeListBoxItem(ref item2, charCostumeInfo_Data);
						costumeListBox.UpdateContents(i, item2);
					}
				}
			}
			costumeListBox.RepositionItems();
		}

		public void SetSelectedCostume(CharCostumeInfo_Data costumeData)
		{
			this._selectedCostumeData = costumeData;
		}

		public void OnCostumeListClick(IUIObject obj)
		{
			NewListBox newListBox = obj as NewListBox;
			if (obj == null || newListBox == null || newListBox.SelectedItem == null)
			{
				return;
			}
			if (newListBox.SelectedItem.Data == null)
			{
				return;
			}
			CharCostumeInfo_Data charCostumeInfo_Data = newListBox.SelectedItem.Data as CharCostumeInfo_Data;
			if (charCostumeInfo_Data == null)
			{
				return;
			}
			this.CostumeRoomRefreshBySelected(charCostumeInfo_Data);
		}

		public void OnCostumeBuyBtn(IUIObject obj)
		{
			if (obj == null || obj.Data == null)
			{
				Debug.LogError("ERROR, CostumeListSetter, OnCostumeBuyBtn(), obj Data is Null");
				return;
			}
			CharCostumeInfo_Data charCostumeInfo_Data = obj.Data as CharCostumeInfo_Data;
			if (charCostumeInfo_Data == null)
			{
				return;
			}
			NrTSingleton<CostumeBuyManager>.Instance.BuyCostumeByMsgBox(charCostumeInfo_Data);
			Debug.Log(charCostumeInfo_Data.m_CharCode);
		}

		public void OnCostumePresentBtn(IUIObject obj)
		{
			if (obj == null || obj.Data == null)
			{
				Debug.LogError("ERROR, CostumeListSetter, OnCostumePresentBtn(), obj Data is Null");
				return;
			}
			CharCostumeInfo_Data charCostumeInfo_Data = obj.Data as CharCostumeInfo_Data;
			if (charCostumeInfo_Data == null)
			{
				return;
			}
			ItemGiftTargetDlg itemGiftTargetDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMGIFTTARGET_DLG) as ItemGiftTargetDlg;
			if (itemGiftTargetDlg == null)
			{
				return;
			}
			itemGiftTargetDlg.SetGiftCostumeData(charCostumeInfo_Data);
			this._owner._costumeViewerSetter._costumeCharSetter.VisibleChar(false);
		}

		public void OnCostumeWearBtn(IUIObject obj)
		{
			if (obj == null || obj.Data == null)
			{
				Debug.LogError("ERROR, CostumeListSetter, OnCostumeWearBtn(), obj Data is Null");
				return;
			}
			CharCostumeInfo_Data charCostumeInfo_Data = obj.Data as CharCostumeInfo_Data;
			if (charCostumeInfo_Data == null)
			{
				return;
			}
			NrTSingleton<CostumeWearManager>.Instance.RequestCostumeWear(this._owner._myCharListSetter._SelectedSolInfo, charCostumeInfo_Data.m_costumeUnique);
		}

		private void SetCostumeListBox(NewListBox costumeListBox, List<CharCostumeInfo_Data> costumeDataList, int costumeKind)
		{
			if (costumeDataList == null || costumeDataList.Count == 0)
			{
				return;
			}
			costumeListBox.Clear();
			foreach (CharCostumeInfo_Data current in costumeDataList)
			{
				if (current != null)
				{
					NewListItem item = new NewListItem(costumeListBox.ColumnNum, true, string.Empty);
					this.SetCostumeListBoxItem(ref item, current);
					costumeListBox.Add(item);
				}
			}
			costumeListBox.RepositionItems();
		}

		private void SetCostumeListBoxItem(ref NewListItem item, CharCostumeInfo_Data costumeData)
		{
			item.Data = costumeData;
			this.SetCostumePortrait(item, costumeData);
			this.SetNormalCostumeTitle(ref item, costumeData);
			this.SetCostumeTitle(ref item, costumeData);
			this.SetBuyButton(ref item, costumeData);
			this.SetPresentImg(ref item, costumeData);
			item.SetListItemData(4, NrTSingleton<NrCharCostumeTableManager>.Instance.IsEventCostume(costumeData));
			item.SetListItemData(7, costumeData.m_New);
			this.SetMoneyType(ref item, costumeData);
			this.SetPrice(ref item, costumeData);
			this.SetWearText(ref item);
			this.SetSelectedBG(ref item, costumeData);
			this.SetWearButton(ref item, costumeData);
			this.SetWearBG(ref item, costumeData);
			this.SetCostumeCount(ref item, costumeData);
		}

		private void SetPrice(ref NewListItem item, CharCostumeInfo_Data costumeData)
		{
			bool visibe = !costumeData.IsNormalCostume();
			item.SetListItemData(19, visibe);
			item.SetListItemData(19, costumeData.m_Price1Num.ToString(), null, null, null);
			item.SetListItemData(21, costumeData.m_Price2Num.ToString(), null, null, null);
			item.SetListItemData(21, visibe);
		}

		private void SetBuyButton(ref NewListItem item, CharCostumeInfo_Data costumeData)
		{
			item.SetListItemData(13, -1, costumeData, new EZValueChangedDelegate(this.OnCostumeBuyBtn), null);
			if (costumeData.IsNormalCostume())
			{
				item.SetListItemData(13, false);
			}
		}

		private void SetPresentImg(ref NewListItem item, CharCostumeInfo_Data costumeData)
		{
			item.SetListItemData(8, -1, costumeData, new EZValueChangedDelegate(this.OnCostumePresentBtn), null);
			item.SetListItemData(9, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3288"), null, null, null);
			bool visibe = costumeData.m_PresentItemUnique != 0;
			if (TsPlatform.IsIPhone)
			{
				visibe = false;
			}
			item.SetListItemData(8, visibe);
			item.SetListItemData(9, visibe);
		}

		private void SetMoneyType(ref NewListItem item, CharCostumeInfo_Data costumeData)
		{
			bool visibe = !costumeData.IsNormalCostume();
			item.SetListItemData(14, visibe);
			item.SetListItemData(20, visibe);
		}

		private List<CharCostumeInfo_Data> GetCostumeDataByKind(int costumeKind)
		{
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(costumeKind);
			if (charKindInfo == null)
			{
				return null;
			}
			return NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeDataList(charKindInfo.GetCode());
		}

		private void SetCostumePortrait(NewListItem item, CharCostumeInfo_Data costumeData)
		{
			CostumeDrawTextureInfo costumeDrawTextureInfo = new CostumeDrawTextureInfo();
			costumeDrawTextureInfo.imageType = eCharImageType.MIDDLE;
			NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(costumeData.m_CharCode);
			if (charKindInfoFromCode != null)
			{
				costumeDrawTextureInfo.charKind = charKindInfoFromCode.GetCharKind();
			}
			costumeDrawTextureInfo.costumePortraitPath = costumeData.m_PortraitPath;
			item.SetListItemData(3, costumeDrawTextureInfo, costumeData, null, null);
		}

		private void SetWearBG(ref NewListItem item, CharCostumeInfo_Data costumeData)
		{
			bool visibe = false;
			if (this.IsWearedCostume(this._owner._myCharListSetter._SelectedSolInfo, costumeData))
			{
				visibe = true;
			}
			item.SetListItemData(10, visibe);
			item.SetListItemData(11, visibe);
			item.SetListItemData(12, visibe);
		}

		private void SetWearText(ref NewListItem item)
		{
			item.SetListItemData(12, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3285"), null, null, null);
		}

		private void SetWearButton(ref NewListItem item, CharCostumeInfo_Data costumeData)
		{
			item.SetListItemData(17, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3286"), costumeData, new EZValueChangedDelegate(this.OnCostumeWearBtn), null);
			bool visibe = NrTSingleton<NrCharCostumeTableManager>.Instance.IsBuyCostume(costumeData.m_costumeUnique);
			if (costumeData.IsNormalCostume())
			{
				visibe = true;
			}
			if (this.IsWearedCostume(this._owner._myCharListSetter._SelectedSolInfo, costumeData))
			{
				visibe = false;
			}
			COSTUME_INFO costumeInfo = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeInfo(costumeData.m_costumeUnique);
			if (!costumeData.IsNormalCostume() && (costumeInfo == null || costumeInfo.i32CostumePossibleToUse <= 0))
			{
				visibe = false;
			}
			if (this._selectedCostumeData == null)
			{
				visibe = false;
			}
			if (this._selectedCostumeData != null && this._selectedCostumeData.m_costumeUnique != costumeData.m_costumeUnique)
			{
				visibe = false;
			}
			if (this._owner._myCharListSetter._SelectedSolInfo == null)
			{
				visibe = false;
			}
			item.SetListItemData(17, visibe);
		}

		private bool IsWearedCostume(NkSoldierInfo solInfo, CharCostumeInfo_Data costumeData)
		{
			if (solInfo == null)
			{
				return false;
			}
			int num = (int)solInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COSTUME);
			return (num == 0 && costumeData.IsNormalCostume()) || num == costumeData.m_costumeUnique;
		}

		private void SetSelectedBG(ref NewListItem item, CharCostumeInfo_Data costumeData)
		{
			bool visibe = false;
			if (this._selectedCostumeData == null)
			{
				item.SetListItemData(1, visibe);
				return;
			}
			if (this._selectedCostumeData.m_costumeUnique == costumeData.m_costumeUnique)
			{
				visibe = true;
			}
			item.SetListItemData(1, visibe);
		}

		private void SetBaseSelectedCostume()
		{
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this._settedCostumeKind);
			if (charKindInfo == null)
			{
				return;
			}
			this._selectedCostumeData = NrTSingleton<NrCharCostumeTableManager>.Instance.GetNormalCostumeData(charKindInfo.GetCode());
			if (this._owner._myCharListSetter._SelectedSolInfo == null)
			{
				return;
			}
			int num = (int)this._owner._myCharListSetter._SelectedSolInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COSTUME);
			if (num <= 0)
			{
				return;
			}
			this._selectedCostumeData = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeData(num);
		}

		private void SetNormalCostumeTitle(ref NewListItem item, CharCostumeInfo_Data costumeData)
		{
			if (costumeData == null)
			{
				return;
			}
			if (!costumeData.IsNormalCostume())
			{
				item.SetListItemData(2, false);
				return;
			}
			item.SetListItemData(2, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(costumeData.m_CostumeTextKey), null, null, null);
			item.SetListItemData(2, true);
		}

		private void SetCostumeTitle(ref NewListItem item, CharCostumeInfo_Data costumeData)
		{
			if (costumeData == null)
			{
				return;
			}
			if (costumeData.IsNormalCostume())
			{
				item.SetListItemData(15, false);
				return;
			}
			item.SetListItemData(15, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(costumeData.m_CostumeTextKey), null, null, null);
			item.SetListItemData(15, true);
		}

		private void SetCostumeCount(ref NewListItem item, CharCostumeInfo_Data costumeData)
		{
			bool visibe = true;
			if (costumeData.IsNormalCostume())
			{
				visibe = false;
			}
			item.SetListItemData(18, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3294"), null, null, null);
			item.SetListItemData(18, visibe);
			string text = string.Empty;
			COSTUME_INFO costumeInfo = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeInfo(costumeData.m_costumeUnique);
			if (costumeInfo == null)
			{
				text = "(0/0)";
			}
			else
			{
				text = string.Concat(new object[]
				{
					"(",
					costumeInfo.i32CostumePossibleToUse,
					"/",
					costumeInfo.i32CostumeCount,
					")"
				});
			}
			item.SetListItemData(16, text, null, null, null);
			item.SetListItemData(16, visibe);
		}

		private void CostumeRoomRefreshBySelected(CharCostumeInfo_Data costumeData)
		{
			if (costumeData == null || this._selectedCostumeData == null)
			{
				return;
			}
			if (this._selectedCostumeData.m_costumeUnique == costumeData.m_costumeUnique)
			{
				return;
			}
			this._selectedCostumeData = costumeData;
			this.RefreshCostumeListBox(ref this._owner._variables._costumeListBox);
			this._owner.InitCostumeView(costumeData);
		}
	}
}
