using GAME;
using System;

namespace UnityForms
{
	public class ListItem
	{
		public enum TYPE
		{
			TEXT,
			IMAGE,
			BUTTON,
			ITEM,
			SOLDIER,
			PROGRESSBAR,
			BUILDING
		}

		private enum eListItemATB
		{
			LISTITEM_VISIBLE = 1
		}

		private object key;

		private static int MAX_COLUMN_NUM = 15;

		private ListItem.TYPE[] type = new ListItem.TYPE[ListItem.MAX_COLUMN_NUM];

		private string[] columnStr = new string[ListItem.MAX_COLUMN_NUM];

		private string[] columnImageStr = new string[ListItem.MAX_COLUMN_NUM];

		private string[] columnImageStr2 = new string[ListItem.MAX_COLUMN_NUM];

		private object[] columnKey = new object[ListItem.MAX_COLUMN_NUM];

		private object[] columnKey2 = new object[ListItem.MAX_COLUMN_NUM];

		private UIBaseInfoLoader[] columnImageInfo = new UIBaseInfoLoader[ListItem.MAX_COLUMN_NUM];

		private EZValueChangedDelegate[] columnDelegate = new EZValueChangedDelegate[ListItem.MAX_COLUMN_NUM];

		private EZGameObjectDelegate gameDelegate;

		private int ATB;

		private float[] m_faAlpha = new float[ListItem.MAX_COLUMN_NUM];

		private bool[] m_bIsTooltip = new bool[ListItem.MAX_COLUMN_NUM];

		private bool[] m_bIsSecondTooltip = new bool[ListItem.MAX_COLUMN_NUM];

		public bool enable = true;

		public object Key
		{
			get
			{
				return this.key;
			}
			set
			{
				this.key = value;
			}
		}

		public object[] ColumnKey
		{
			get
			{
				return this.columnKey;
			}
			set
			{
				this.columnKey = value;
			}
		}

		public object[] ColumnKey2
		{
			get
			{
				return this.columnKey2;
			}
			set
			{
				this.columnKey2 = value;
			}
		}

		public bool[] p_bIsTooltip
		{
			get
			{
				return this.m_bIsTooltip;
			}
			set
			{
				this.m_bIsTooltip = value;
			}
		}

		public bool[] p_bIsSecondTooltip
		{
			get
			{
				return this.m_bIsSecondTooltip;
			}
			set
			{
				this.m_bIsSecondTooltip = value;
			}
		}

		public bool Visible
		{
			get
			{
				return this.IsATB(ListItem.eListItemATB.LISTITEM_VISIBLE);
			}
			set
			{
				if (value)
				{
					this.SetATB(ListItem.eListItemATB.LISTITEM_VISIBLE);
				}
				else
				{
					this.DelATB(ListItem.eListItemATB.LISTITEM_VISIBLE);
				}
			}
		}

		public ListItem()
		{
			for (int i = 0; i < ListItem.MAX_COLUMN_NUM; i++)
			{
				this.type[i] = ListItem.TYPE.TEXT;
				this.columnImageStr[i] = string.Empty;
				this.columnImageStr2[i] = string.Empty;
				this.columnImageInfo[i] = new UIBaseInfoLoader();
				this.columnImageInfo[i].Material = string.Empty;
				this.m_bIsTooltip[i] = true;
				this.m_bIsSecondTooltip[i] = true;
			}
		}

		public void SetGameObjectDelegate(EZGameObjectDelegate del)
		{
			this.gameDelegate = del;
		}

		public EZGameObjectDelegate GetGameObjectDelegate()
		{
			return this.gameDelegate;
		}

		public bool SetColumnGUIContent(int index, string str)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return false;
			}
			this.columnStr[index] = str;
			this.type[index] = ListItem.TYPE.TEXT;
			return true;
		}

		public bool SetColumnGUIContent(int index, string str, UIBaseInfoLoader image)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return false;
			}
			this.columnStr[index] = str;
			this.columnImageInfo[index] = image;
			this.type[index] = ListItem.TYPE.IMAGE;
			return true;
		}

		public bool SetColumnGUIContent(int index, string str, string imageStr)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return false;
			}
			this.columnStr[index] = str;
			this.columnImageStr[index] = imageStr;
			this.type[index] = ListItem.TYPE.IMAGE;
			return true;
		}

		public bool SetColumnGUIContent(int index, ITEM _protocolItem, bool a_bIsTooltip)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return false;
			}
			this.columnKey[index] = _protocolItem;
			this.m_bIsTooltip[index] = a_bIsTooltip;
			this.type[index] = ListItem.TYPE.ITEM;
			return true;
		}

		public bool SetColumnGUIContent(int index, ITEM _protocolItem, bool a_bIsTooltip, bool bsecondtooltip, ITEM seconditem)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return false;
			}
			this.columnKey[index] = _protocolItem;
			this.m_bIsTooltip[index] = a_bIsTooltip;
			this.m_bIsSecondTooltip[index] = bsecondtooltip;
			this.columnKey2[index] = seconditem;
			this.type[index] = ListItem.TYPE.ITEM;
			return true;
		}

		public bool SetColumnGUIContent(int index, ITEM _protocolItem)
		{
			return this.SetColumnGUIContent(index, _protocolItem, true);
		}

		public bool SetColumnGUIContent(int index, ITEM _protocolItem, EZValueChangedDelegate del)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return false;
			}
			this.columnKey[index] = _protocolItem;
			this.columnDelegate[index] = del;
			this.m_bIsTooltip[index] = true;
			this.type[index] = ListItem.TYPE.ITEM;
			return true;
		}

		public bool SetColumnGUIContent(int index, string imageStr, string a_strKey, float a_fAlpha)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return false;
			}
			this.m_faAlpha[index] = a_fAlpha;
			this.columnImageStr[index] = imageStr;
			this.columnKey[index] = a_strKey;
			this.type[index] = ListItem.TYPE.IMAGE;
			return true;
		}

		public bool SetColumnGUIContent(int index, string imageStr, string imageStr2, float gage, bool bNull)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return false;
			}
			this.columnImageStr[index] = imageStr;
			this.columnImageStr2[index] = imageStr2;
			this.columnKey[index] = gage;
			this.type[index] = ListItem.TYPE.PROGRESSBAR;
			return true;
		}

		public bool SetColumnGUIContent(int index, string str, UIBaseInfoLoader image, string color)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return false;
			}
			this.columnStr[index] = color + str;
			this.columnImageInfo[index] = image;
			this.type[index] = ListItem.TYPE.IMAGE;
			return true;
		}

		public bool SetColumnGUIContent(int index, string str, UIBaseInfoLoader image, object _key)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return false;
			}
			this.columnStr[index] = str;
			this.columnImageInfo[index] = image;
			this.columnKey[index] = _key;
			this.type[index] = ListItem.TYPE.IMAGE;
			return true;
		}

		public bool SetColumnGUIContent(int index, string str, UIBaseInfoLoader image, object _key, object _key2)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return false;
			}
			this.columnStr[index] = str;
			this.columnImageInfo[index] = image;
			this.columnKey[index] = _key;
			this.columnKey2[index] = _key2;
			this.type[index] = ListItem.TYPE.IMAGE;
			return true;
		}

		public bool SetColumnGUIContent(int index, string str, string imageStr, object a_oKey, EZValueChangedDelegate del)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return false;
			}
			this.columnStr[index] = str;
			this.columnImageStr[index] = imageStr;
			this.columnDelegate[index] = del;
			this.columnKey[index] = a_oKey;
			this.type[index] = ListItem.TYPE.BUTTON;
			return true;
		}

		public bool SetColumnGUIContent(int index, object _key, EZValueChangedDelegate del)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return false;
			}
			this.columnDelegate[index] = del;
			this.columnKey[index] = _key;
			return true;
		}

		public bool SetColumnGUIContent(int index, int charKind, bool a_bIsTooltip)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return false;
			}
			this.columnKey[index] = charKind;
			this.m_bIsTooltip[index] = a_bIsTooltip;
			this.type[index] = ListItem.TYPE.SOLDIER;
			return true;
		}

		public bool SetColumnGUIContent(int index, NkListSolInfo solInfo, bool a_bIsTooltip)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return false;
			}
			this.columnKey[index] = solInfo;
			this.m_bIsTooltip[index] = a_bIsTooltip;
			this.type[index] = ListItem.TYPE.SOLDIER;
			return true;
		}

		public EZValueChangedDelegate GetColumnDelegate(int index)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return null;
			}
			return this.columnDelegate[index];
		}

		public UIBaseInfoLoader GetColumnImageInfo(int index)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return null;
			}
			return this.columnImageInfo[index];
		}

		public string GetColumnStr(int index)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return null;
			}
			return this.columnStr[index];
		}

		public string GetColumnImageStr(int index)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return null;
			}
			return this.columnImageStr[index];
		}

		public string GetColumnImageStr2(int index)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return null;
			}
			return this.columnImageStr2[index];
		}

		public object GetColumnKey(int index)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return null;
			}
			return this.columnKey[index];
		}

		public object GetColumnKey2(int index)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return null;
			}
			return this.columnKey2[index];
		}

		public ListItem.TYPE GetType(int index)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return ListItem.TYPE.TEXT;
			}
			return this.type[index];
		}

		public bool SetColumnStr(int index, string str)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return false;
			}
			this.columnStr[index] = str;
			this.type[index] = ListItem.TYPE.TEXT;
			return true;
		}

		public bool SetColumnStr(int index, string str, string _color)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return false;
			}
			this.columnStr[index] = _color + str;
			this.type[index] = ListItem.TYPE.TEXT;
			return true;
		}

		public bool SetColumnStr(int index, string str, string _color, object _key)
		{
			if (ListItem.MAX_COLUMN_NUM <= index)
			{
				return false;
			}
			this.columnStr[index] = _color + str;
			this.columnKey[index] = _key;
			this.type[index] = ListItem.TYPE.TEXT;
			return true;
		}

		public bool Clear()
		{
			for (int i = 0; i < ListItem.MAX_COLUMN_NUM; i++)
			{
				this.columnStr[i] = string.Empty;
				this.columnKey[i] = null;
				this.columnKey2[i] = null;
			}
			return true;
		}

		private bool IsATB(ListItem.eListItemATB flag)
		{
			return (this.ATB & (int)flag) > 0;
		}

		private void SetATB(ListItem.eListItemATB flag)
		{
			this.ATB |= (int)flag;
		}

		private void DelATB(ListItem.eListItemATB flag)
		{
			this.ATB &= (int)(~(int)flag);
		}

		public float Get_Alpha(int a_nIndex)
		{
			if (ListItem.MAX_COLUMN_NUM <= a_nIndex)
			{
				return 0f;
			}
			return this.m_faAlpha[a_nIndex];
		}
	}
}
