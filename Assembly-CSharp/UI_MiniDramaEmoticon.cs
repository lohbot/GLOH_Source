using System;
using System.Collections;
using UnityEngine;
using UnityForms;

public class UI_MiniDramaEmoticon : Form
{
	private DrawTexture[] _Emoticon = new DrawTexture[2];

	private TsWeakReference<NrCharBase> _kChar;

	private float _Showtime;

	public bool m_ShowUI
	{
		get
		{
			return base.Visible;
		}
		set
		{
			if (value)
			{
				this.Show();
			}
			else
			{
				this.Hide();
			}
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "MiniDrama/DLG_MiniDramaEmoticon", G_ID.MiniDRAMAEMOTICON_DLG, true);
	}

	public override void SetComponent()
	{
		Array values = Enum.GetValues(typeof(MiniDramaActorController.EMOTICON));
		if (values.Length > 0)
		{
			IEnumerator enumerator = values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MiniDramaActorController.EMOTICON eMOTICON = (MiniDramaActorController.EMOTICON)((int)enumerator.Current);
					if (eMOTICON != MiniDramaActorController.EMOTICON.MAX_EMOTICON)
					{
						this._Emoticon[(int)eMOTICON] = (base.GetControl(string.Format("DrawTexture_{0}", eMOTICON.ToString())) as DrawTexture);
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	public override void InitData()
	{
		base.InitData();
		base.Draggable = false;
	}

	public void Init(NrCharBase kChar)
	{
		this._kChar = kChar;
	}

	public void ShowEmoticon(MiniDramaActorController.EMOTICON EmoticonType, float ShowTime)
	{
		this.m_ShowUI = true;
		this._Showtime = Time.time + ShowTime;
		Array values = Enum.GetValues(typeof(MiniDramaActorController.EMOTICON));
		if (values.Length > 0)
		{
			IEnumerator enumerator = values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MiniDramaActorController.EMOTICON eMOTICON = (MiniDramaActorController.EMOTICON)((int)enumerator.Current);
					if (eMOTICON != MiniDramaActorController.EMOTICON.MAX_EMOTICON)
					{
						if (!(this._Emoticon[(int)eMOTICON] == null))
						{
							if (eMOTICON == EmoticonType)
							{
								this._Emoticon[(int)eMOTICON].Visible = true;
							}
							else
							{
								this._Emoticon[(int)eMOTICON].Visible = false;
							}
						}
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	public override void Update()
	{
		if (this._Showtime != 0f && Time.time > this._Showtime)
		{
			this._Showtime = 0f;
			this.m_ShowUI = false;
		}
	}

	public void UpatePotition()
	{
		if (this._kChar != null && this._kChar == null)
		{
			return;
		}
	}
}
