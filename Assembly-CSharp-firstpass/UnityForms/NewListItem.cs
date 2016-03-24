using GAME;
using System;
using UnityEngine;

namespace UnityForms
{
	public class NewListItem
	{
		public class NewListItemData
		{
			public object realData;

			public object data;

			public object data2;

			public EZValueChangedDelegate eventDelegate;

			public EZValueChangedDelegate downDelegate;

			public bool visible = true;

			public bool enable = true;

			public object EventMark;

			public NewListItemData()
			{
				this.realData = null;
				this.eventDelegate = null;
				this.downDelegate = null;
				this.data = null;
				this.visible = true;
				this.enable = true;
			}
		}

		public string m_szColumnData = string.Empty;

		public bool m_bEnable = true;

		public int m_nMaxCoulmnNum = 1;

		public NewListItem.NewListItemData[] m_itemDataList;

		public object m_kData;

		public bool _EventMark;

		public object Data
		{
			get
			{
				return this.m_kData;
			}
			set
			{
				this.m_kData = value;
			}
		}

		public bool EventMark
		{
			get
			{
				return this._EventMark;
			}
			set
			{
				this._EventMark = value;
			}
		}

		public NewListItem(int maxNum, bool enable = true, string columnData = "")
		{
			this.m_szColumnData = columnData;
			this.m_nMaxCoulmnNum = maxNum;
			this.m_itemDataList = new NewListItem.NewListItemData[maxNum];
			for (int i = 0; i < maxNum; i++)
			{
				this.m_itemDataList[i] = new NewListItem.NewListItemData();
			}
			this.m_bEnable = enable;
			this.m_kData = null;
		}

		public void Set(NewListItem item)
		{
			this.m_szColumnData = item.m_szColumnData;
			this.m_bEnable = item.m_bEnable;
			this.m_nMaxCoulmnNum = item.m_nMaxCoulmnNum;
			for (int i = 0; i < this.m_nMaxCoulmnNum; i++)
			{
				this.m_itemDataList[i] = item.m_itemDataList[i];
			}
			this.m_kData = item.m_kData;
			this._EventMark = item._EventMark;
		}

		public void SetEnable(bool flag)
		{
			this.m_bEnable = flag;
		}

		public bool GetEnable()
		{
			return this.m_bEnable;
		}

		public NewListItem.NewListItemData GetData(int index)
		{
			if (0 <= index && this.m_nMaxCoulmnNum > index)
			{
				return this.m_itemDataList[index];
			}
			return null;
		}

		public void SetListItemData(int index, bool visibe)
		{
			if (0 <= index && this.m_nMaxCoulmnNum > index)
			{
				this.m_itemDataList[index].visible = visibe;
			}
		}

		public void SetListItemEnable(int index, bool enable)
		{
			if (0 <= index && this.m_nMaxCoulmnNum > index)
			{
				this.m_itemDataList[index].enable = enable;
			}
		}

		public void SetListItemData(int index, string text, object data = null, EZValueChangedDelegate eventDelegate = null, EZValueChangedDelegate downDelegate = null)
		{
			if (0 <= index && this.m_nMaxCoulmnNum > index)
			{
				this.m_itemDataList[index].realData = text;
				this.m_itemDataList[index].data = data;
				this.m_itemDataList[index].eventDelegate = eventDelegate;
				this.m_itemDataList[index].downDelegate = downDelegate;
			}
		}

		public void SetListItemData(int index, UIBaseInfoLoader loader, object data = null, EZValueChangedDelegate eventDelegate = null, EZValueChangedDelegate downDelegate = null)
		{
			if (0 <= index && this.m_nMaxCoulmnNum > index)
			{
				this.m_itemDataList[index].realData = loader;
				this.m_itemDataList[index].data = data;
				this.m_itemDataList[index].eventDelegate = eventDelegate;
				this.m_itemDataList[index].downDelegate = downDelegate;
			}
		}

		public void SetListItemData(int index, ITEM item, object data = null, EZValueChangedDelegate eventDelegate = null, EZValueChangedDelegate downDelegate = null)
		{
			if (0 <= index && this.m_nMaxCoulmnNum > index)
			{
				this.m_itemDataList[index].realData = item;
				this.m_itemDataList[index].data = data;
				this.m_itemDataList[index].eventDelegate = eventDelegate;
				this.m_itemDataList[index].downDelegate = downDelegate;
			}
		}

		public void SetListItemData(int index, ITEM item, ITEM SecondItem, object data = null, EZValueChangedDelegate eventDelegate = null, EZValueChangedDelegate downDelegate = null)
		{
			if (0 <= index && this.m_nMaxCoulmnNum > index)
			{
				this.m_itemDataList[index].realData = item;
				this.m_itemDataList[index].data = data;
				this.m_itemDataList[index].data2 = SecondItem;
				this.m_itemDataList[index].eventDelegate = eventDelegate;
				this.m_itemDataList[index].downDelegate = downDelegate;
			}
		}

		public void SetListItemData(int index, int charKind, object data = null, EZValueChangedDelegate eventDelegate = null, EZValueChangedDelegate downDelegate = null)
		{
			if (0 <= index && this.m_nMaxCoulmnNum > index)
			{
				this.m_itemDataList[index].realData = charKind;
				this.m_itemDataList[index].data = data;
				this.m_itemDataList[index].eventDelegate = eventDelegate;
				this.m_itemDataList[index].downDelegate = downDelegate;
			}
		}

		public void SetFloatListItemData(int index, float value, object data = null, EZValueChangedDelegate eventDelegate = null, EZValueChangedDelegate downDelegate = null)
		{
			if (0 <= index && this.m_nMaxCoulmnNum > index)
			{
				this.m_itemDataList[index].realData = value;
				this.m_itemDataList[index].data = data;
				this.m_itemDataList[index].eventDelegate = eventDelegate;
				this.m_itemDataList[index].downDelegate = downDelegate;
			}
		}

		public void SetListItemData(int index, NkListSolInfo solInfo, object data = null, EZValueChangedDelegate eventDelegate = null, EZValueChangedDelegate downDelegate = null)
		{
			if (0 <= index && this.m_nMaxCoulmnNum > index)
			{
				this.m_itemDataList[index].realData = solInfo;
				this.m_itemDataList[index].data = data;
				this.m_itemDataList[index].eventDelegate = eventDelegate;
				this.m_itemDataList[index].downDelegate = downDelegate;
			}
		}

		public void SetListItemData(int index, CostumeDrawTextureInfo costumeInfo, object data = null, EZValueChangedDelegate eventDelegate = null, EZValueChangedDelegate downDelegate = null)
		{
			if (0 <= index && this.m_nMaxCoulmnNum > index)
			{
				this.m_itemDataList[index].realData = costumeInfo;
				this.m_itemDataList[index].data = data;
				this.m_itemDataList[index].eventDelegate = eventDelegate;
				this.m_itemDataList[index].downDelegate = downDelegate;
			}
		}

		public void SetListItemData(int index, Texture2D _Texture, object data = null, object data2 = null, EZValueChangedDelegate eventDelegate = null, EZValueChangedDelegate downDelegate = null)
		{
			if (0 <= index && this.m_nMaxCoulmnNum > index)
			{
				this.m_itemDataList[index].realData = _Texture;
				this.m_itemDataList[index].data = data;
				this.m_itemDataList[index].data2 = data2;
				this.m_itemDataList[index].eventDelegate = eventDelegate;
				this.m_itemDataList[index].downDelegate = downDelegate;
			}
		}

		public void SetListItemData2(int index, object data2)
		{
			if (0 <= index && this.m_nMaxCoulmnNum > index)
			{
				this.m_itemDataList[index].data2 = data2;
			}
		}
	}
}
