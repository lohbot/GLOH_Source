using System;
using System.Collections.Generic;
using UnityForms;

public class SortedButtonManager
{
	public class ButtonBundle
	{
		private Button m_button;

		private SortedButtonManager.ARROW_INVERSE m_eArrowState;

		private string m_strNormalStyle = "Win_B_ListSort";

		private string m_strArrowStyle = "Win_T_DropDwBtn";

		public void SetData(Button _btn, string _strNormalStyle, string _strInverStyle)
		{
			this.m_button = _btn;
			this.m_strNormalStyle = _strNormalStyle;
			this.m_strArrowStyle = _strInverStyle;
			this.SetArrowStyle(SortedButtonManager.ARROW_INVERSE.NONE);
		}

		public void SetArrowStyle(SortedButtonManager.ARROW_INVERSE _Style)
		{
			if (this.m_eArrowState != _Style)
			{
				if (this.m_eArrowState == SortedButtonManager.ARROW_INVERSE.INVERSE)
				{
					this.m_button.Inverse(INVERSE_MODE.TOP_TO_BOTTOM);
				}
				switch (_Style)
				{
				case SortedButtonManager.ARROW_INVERSE.NONE:
					this.m_button.SetButtonTextureKey(this.m_strNormalStyle);
					break;
				case SortedButtonManager.ARROW_INVERSE.NORMAL:
					this.m_button.SetButtonTextureKey(this.m_strArrowStyle);
					break;
				case SortedButtonManager.ARROW_INVERSE.INVERSE:
					TsLog.LogWarning("KKIComment2011/9/7/ : SetArrowStyle ", new object[0]);
					this.m_button.SetButtonTextureKey(this.m_strArrowStyle);
					this.m_button.Inverse(INVERSE_MODE.TOP_TO_BOTTOM);
					break;
				}
				this.m_eArrowState = _Style;
			}
		}

		public SortedButtonManager.ARROW_INVERSE GetNextButtonState()
		{
			SortedButtonManager.ARROW_INVERSE aRROW_INVERSE = this.m_eArrowState + 1;
			if (aRROW_INVERSE > SortedButtonManager.ARROW_INVERSE.INVERSE)
			{
				aRROW_INVERSE = SortedButtonManager.ARROW_INVERSE.NORMAL;
			}
			else if (aRROW_INVERSE < SortedButtonManager.ARROW_INVERSE.NORMAL)
			{
				aRROW_INVERSE = SortedButtonManager.ARROW_INVERSE.NORMAL;
			}
			return aRROW_INVERSE;
		}
	}

	public enum ARROW_INVERSE
	{
		INIT,
		NONE,
		NORMAL,
		INVERSE,
		END
	}

	private const string C_strBasicNormal = "Win_B_ListSort";

	private const string C_strBasicDesc = "Win_T_DropDwBtn";

	private const string C_strBasicAsc = "Win_T_DropDwBtn";

	private Dictionary<string, SortedButtonManager.ButtonBundle> m_ButtonList;

	public SortedButtonManager()
	{
		this.m_ButtonList = new Dictionary<string, SortedButtonManager.ButtonBundle>();
	}

	public void AddButtonBundle(string key, Button _button, string _strNormalStyle, string _strArrowStyle)
	{
		if (!this.m_ButtonList.ContainsKey(key))
		{
			SortedButtonManager.ButtonBundle buttonBundle = new SortedButtonManager.ButtonBundle();
			buttonBundle.SetData(_button, _strNormalStyle, _strArrowStyle);
			this.m_ButtonList.Add(key, buttonBundle);
		}
	}

	public SortedButtonManager.ARROW_INVERSE ButtonStateNext(string key)
	{
		SortedButtonManager.ARROW_INVERSE aRROW_INVERSE = SortedButtonManager.ARROW_INVERSE.NONE;
		if (this.m_ButtonList.ContainsKey(key))
		{
			aRROW_INVERSE = this.m_ButtonList[key].GetNextButtonState();
			this.ChangeButtonState(key, aRROW_INVERSE);
		}
		return aRROW_INVERSE;
	}

	public void ChangeButtonState(string key, SortedButtonManager.ARROW_INVERSE _eArrowState)
	{
		if (this.m_ButtonList.ContainsKey(key))
		{
			foreach (string current in this.m_ButtonList.Keys)
			{
				if (current.Equals(key))
				{
					this.m_ButtonList[current].SetArrowStyle(_eArrowState);
				}
				else
				{
					this.m_ButtonList[current].SetArrowStyle(SortedButtonManager.ARROW_INVERSE.NONE);
				}
			}
		}
	}
}
