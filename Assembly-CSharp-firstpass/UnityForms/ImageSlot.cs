using System;
using UnityEngine;

namespace UnityForms
{
	public class ImageSlot
	{
		public class ImageSlotInfo
		{
			public string _num = string.Empty;

			public string _rank = string.Empty;

			public bool _visibleNum;

			public bool _visibleRank;

			public bool _visibleAddImage;

			public void Init()
			{
				this._num = string.Empty;
				this._rank = string.Empty;
				this._visibleNum = false;
				this._visibleRank = false;
				this._visibleAddImage = false;
			}

			public void Set(string num, string option)
			{
				this._num = num;
				this._rank = option;
			}
		}

		public long _solID;

		private int _index = -1;

		public string imageStr = string.Empty;

		public int itemunique;

		private bool levelUP;

		private int _WindowID;

		public Vector2 CoolTime = Vector2.zero;

		private ImageSlot.ImageSlotInfo _imageSlotInfo = new ImageSlot.ImageSlotInfo();

		private bool itemImage = true;

		public bool c_bEnable
		{
			get;
			set;
		}

		public bool c_bDisable
		{
			get;
			set;
		}

		public int p_nAddEnableSlot
		{
			get;
			set;
		}

		public object c_oItem
		{
			get;
			set;
		}

		public bool ItemImage
		{
			get
			{
				return this.itemImage;
			}
			set
			{
				this.itemImage = value;
			}
		}

		public int WindowID
		{
			get
			{
				return this._WindowID;
			}
			set
			{
				this._WindowID = value;
			}
		}

		public bool LevelUP
		{
			get
			{
				return this.levelUP;
			}
			set
			{
				this.levelUP = value;
			}
		}

		public int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		public string Num
		{
			get
			{
				return this._imageSlotInfo._num;
			}
			set
			{
				this._imageSlotInfo._num = value;
			}
		}

		public string Rank
		{
			get
			{
				return this._imageSlotInfo._rank;
			}
			set
			{
				this._imageSlotInfo._rank = value;
			}
		}

		public bool VisibleNum
		{
			get
			{
				return this._imageSlotInfo._visibleNum;
			}
			set
			{
				this._imageSlotInfo._visibleNum = value;
			}
		}

		public float CurDelayTime
		{
			get
			{
				return this.CoolTime.x;
			}
			set
			{
				this.CoolTime.x = value;
			}
		}

		public float MaxDelayTime
		{
			get
			{
				return this.CoolTime.y;
			}
			set
			{
				this.CoolTime.y = value;
			}
		}

		public ImageSlot.ImageSlotInfo SlotInfo
		{
			get
			{
				return this._imageSlotInfo;
			}
			set
			{
				this._imageSlotInfo = value;
			}
		}

		public ImageSlot()
		{
			this.Init();
		}

		public void Init()
		{
			this._index = -1;
			this.levelUP = false;
			this._imageSlotInfo.Init();
		}
	}
}
