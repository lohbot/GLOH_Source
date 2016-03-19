using System;
using System.Collections.Generic;
using UnityForms;

public class DLG_Audio : Form
{
	public class AudioControl
	{
		public HorizontalSlider m_HSliderScroll;

		public TextField m_TextField_TextField;
	}

	private List<DLG_Audio.AudioControl> m_AudioControlList = new List<DLG_Audio.AudioControl>();

	private Button m_Button_Select;

	private float[] m_fPrevVolume = new float[9];

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Main/DLG_Audio", G_ID.DLG_AUDIO, true);
	}

	public override void SetComponent()
	{
		for (int i = 0; i < 9; i++)
		{
			DLG_Audio.AudioControl audioControl = new DLG_Audio.AudioControl();
			audioControl.m_HSliderScroll = (base.GetControl("HSlider_Scroll" + (i + 1).ToString()) as HorizontalSlider);
			audioControl.m_TextField_TextField = (base.GetControl("TextField_TextField" + (i + 1).ToString()) as TextField);
			audioControl.m_TextField_TextField.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnChangeTextFieldValue));
			audioControl.m_TextField_TextField.Data = i;
			this.m_AudioControlList.Add(audioControl);
		}
		this.m_Button_Select = (base.GetControl("Button_Select") as Button);
		Button expr_B5 = this.m_Button_Select;
		expr_B5.Click = (EZValueChangedDelegate)Delegate.Combine(expr_B5.Click, new EZValueChangedDelegate(this.OnSaveVolumes));
	}

	public override void InitData()
	{
		base.InitData();
		this.SetData();
		float x = (GUICamera.width - base.GetSize().x) * 0.5f;
		float y = (GUICamera.height - base.GetSize().y) * 0.5f;
		base.SetLocation(x, y);
	}

	public override void Update()
	{
		base.Update();
		this.UpdateData();
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	private void SetData()
	{
		float masterVolume = TsAudio.MasterVolume;
		this.m_AudioControlList[0].m_HSliderScroll.defaultValue = masterVolume;
		this.m_AudioControlList[0].m_HSliderScroll.Value = masterVolume;
		this.m_AudioControlList[0].m_TextField_TextField.Text = masterVolume.ToString();
		this.m_fPrevVolume[0] = masterVolume;
		EAudioType[] array = Enum.GetValues(typeof(EAudioType)) as EAudioType[];
		EAudioType[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			EAudioType eAudioType = array2[i];
			if (eAudioType < EAudioType.TOTAL)
			{
				float volumeScaling = TsAudio.GetVolumeScaling(eAudioType);
				this.m_AudioControlList[(int)(eAudioType + 1)].m_HSliderScroll.defaultValue = volumeScaling;
				this.m_AudioControlList[(int)(eAudioType + 1)].m_HSliderScroll.Value = volumeScaling;
				this.m_AudioControlList[(int)(eAudioType + 1)].m_TextField_TextField.Text = volumeScaling.ToString();
				this.m_fPrevVolume[(int)(eAudioType + 1)] = volumeScaling;
			}
		}
	}

	private void UpdateData()
	{
		float value = this.m_AudioControlList[0].m_HSliderScroll.Value;
		if (this.ChangeVolume(0, value))
		{
			this.ChangeTextFieldValue(0, value);
		}
		EAudioType[] array = Enum.GetValues(typeof(EAudioType)) as EAudioType[];
		EAudioType[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			EAudioType eAudioType = array2[i];
			if (eAudioType < EAudioType.TOTAL)
			{
				float value2 = this.m_AudioControlList[(int)(eAudioType + 1)].m_HSliderScroll.Value;
				if (this.ChangeVolume((int)(eAudioType + 1), value2))
				{
					this.ChangeTextFieldValue((int)(eAudioType + 1), value2);
				}
			}
		}
	}

	private void OnChangeTextFieldValue(IUIObject obj)
	{
		TextField textField = obj as TextField;
		int num = (int)textField.Data;
		string text = this.m_AudioControlList[num].m_TextField_TextField.Text;
		float num2 = 0f;
		try
		{
			num2 = float.Parse(text);
			if (num2 > 1f)
			{
				this.m_AudioControlList[num].m_TextField_TextField.Text = "1";
				num2 = 1f;
			}
		}
		catch (FormatException)
		{
			num2 = 0f;
		}
		if (this.ChangeVolume(num, num2))
		{
			this.ChangeSliderValue(num, num2);
		}
	}

	private bool ChangeVolume(int nIndex, float fVolume)
	{
		if (this.m_fPrevVolume[nIndex] == fVolume)
		{
			return false;
		}
		if (nIndex == 0)
		{
			TsAudio.MasterVolume = fVolume;
		}
		else
		{
			TsAudio.SetVolumeScaling((EAudioType)(nIndex - 1), fVolume);
			TsAudio.RefreshAudioVolumes((EAudioType)(nIndex - 1));
		}
		this.m_fPrevVolume[nIndex] = fVolume;
		return true;
	}

	private void ChangeSliderValue(int nIndex, float fVolume)
	{
		this.m_AudioControlList[nIndex].m_HSliderScroll.Value = fVolume;
	}

	private void ChangeTextFieldValue(int nIndex, float fVolume)
	{
		this.m_AudioControlList[nIndex].m_TextField_TextField.Text = fVolume.ToString();
	}

	private void OnSaveVolumes(IUIObject obj)
	{
	}
}
