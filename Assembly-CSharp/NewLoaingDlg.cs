using Ndoors.Framework.Stage;
using PROTOCOL;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class NewLoaingDlg : Form
{
	private DrawTexture m_BG;

	private DrawTexture m_LoadingStage;

	private DrawTexture m_Up;

	private DrawTexture m_Down;

	private Label m_Tip;

	private DrawTexture m_Left;

	private DrawTexture m_Center;

	private Label m_Percent;

	private float m_fRoateVal = 5f;

	private Label m_lbLoadingStatus;

	private Label m_lbPacketNum;

	private List<NewLoadingText> m_LoadingText = new List<NewLoadingText>();

	private int magicNum = -1;

	private GameObject m_goLoadingEffect;

	private float m_fProgressMax = 1f;

	private float m_fProgressValue;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.TopMost = true;
		base.Scale = false;
		instance.LoadFileAll(ref form, "Stage/DLG_NewLoading", G_ID.DLG_LOADINGPAGE, false);
		base.AlwaysUpdate = true;
		base.DonotDepthChange(NrTSingleton<FormsManager>.Instance.GetTopMostZ());
		this.m_LoadingText.Clear();
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		this.magicNum = 156782;
	}

	public override void SetComponent()
	{
		base.SetLocation((GUICamera.width - base.GetSizeX()) / 2f, (GUICamera.height - base.GetSizeY()) / 2f);
		float x = -(GUICamera.width - base.GetSizeX()) / 2f;
		this.m_LoadingStage = (base.GetControl("DrawTexture_LoadingIcon") as DrawTexture);
		this.m_Up = (base.GetControl("DrawTexture_UpperScreen") as DrawTexture);
		this.m_Up.SetSize(GUICamera.width, this.m_Up.GetSize().y + (GUICamera.height - base.GetSizeY()) / 2f);
		this.m_Up.SetLocation(x, this.m_Up.GetSize().y - (GUICamera.height - base.GetSizeY()) / 2f - 1f);
		this.m_Down = (base.GetControl("DrawTexture_LowerScreen") as DrawTexture);
		this.m_Down.SetSize(GUICamera.width, this.m_Down.GetSize().y + (GUICamera.height - base.GetSizeY()) / 2f);
		this.m_Down.SetLocation(x, GUICamera.height - this.m_Down.GetSize().y - (GUICamera.height - base.GetSizeY()) / 2f + 1f);
		this.m_Tip = (base.GetControl("Label_Tip") as Label);
		this.m_Tip.Text = " ";
		this.m_Left = (base.GetControl("DrawTexture_ImgLeft") as DrawTexture);
		this.m_Left.Visible = false;
		this.m_Center = (base.GetControl("DrawTexture_ImgCenter") as DrawTexture);
		this.m_Center.Visible = false;
		this.m_Percent = (base.GetControl("Label_LoadingRate") as Label);
		this.m_Percent.Text = "0%";
		this.SetLoadingSize();
		this.ParseToolTip();
		this.m_BG = (base.GetControl("DrawTexture_BG") as DrawTexture);
		string str = string.Empty;
		str = NrTSingleton<UIDataManager>.Instance.FilePath + "Texture/Loading/loading_BG";
		this.m_BG.SetLocation(x, -(GUICamera.height - base.GetSizeY()) / 2f);
		this.m_BG.SetSize(GUICamera.width, GUICamera.height);
		this.m_BG.SetTexture((Texture2D)CResources.Load(str + NrTSingleton<UIDataManager>.Instance.AddFilePath));
		this.m_lbLoadingStatus = (base.GetControl("LB_LoadingStatus1") as Label);
		this.m_lbPacketNum = (base.GetControl("LB_LoadingStatus2") as Label);
		this.Hide();
	}

	private void ParseToolTip()
	{
		string path = NrTSingleton<UIDataManager>.Instance.FilePath + "TextPreloading/LoadingToolTip";
		TextAsset textAsset = (TextAsset)CResources.Load(path);
		if (null == textAsset)
		{
			TsLog.Log("Failed TextPreloading/LoadingToolTip", new object[0]);
			return;
		}
		char[] separator = new char[]
		{
			'\t'
		};
		string[] array = textAsset.text.Split(new char[]
		{
			'\n'
		});
		for (int i = 1; i < array.Length; i++)
		{
			if (!(string.Empty == array[i]))
			{
				string[] array2 = array[i].Split(separator);
				NewLoadingText newLoadingText = new NewLoadingText();
				newLoadingText.m_szTextKey = array2[0];
				newLoadingText.m_nMinLV = int.Parse(array2[1]);
				newLoadingText.m_nMaxLV = int.Parse(array2[2]);
				if (UIDataManager.IsUse256Texture())
				{
					newLoadingText.m_szBundlePath = array2[4];
				}
				else
				{
					newLoadingText.m_szBundlePath = array2[3];
				}
				newLoadingText.m_nShowType = int.Parse(array2[5]);
				this.m_LoadingText.Add(newLoadingText);
			}
		}
		Resources.UnloadAsset(textAsset);
		CResources.Delete(path);
	}

	public override void Update()
	{
		if (base.Visible)
		{
			this.m_LoadingStage.Rotate(this.m_fRoateVal);
			this.SetStageStatus();
			this.SetPacketNum();
		}
	}

	public void OnRelogin(object a_cObject)
	{
		Debug.LogWarning("ReLogin");
		NrTSingleton<NrMainSystem>.Instance.m_ReLogin = false;
		NrTSingleton<NrMainSystem>.Instance.ReLogin(true);
	}

	public void SetLoadingSize()
	{
	}

	public void SetShowHide(bool bShow)
	{
		if (bShow)
		{
			if (!base.Visible)
			{
				NrTSingleton<NrGlobalReference>.Instance.GetDownloadInfo().ResetSceneDownloadSize();
				int num = 1;
				if (PlayerPrefs.HasKey(NrPrefsKey.PLAYER_LEVEL))
				{
					num = PlayerPrefs.GetInt(NrPrefsKey.PLAYER_LEVEL);
				}
				else
				{
					NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
					if (charPersonInfo != null)
					{
						num = charPersonInfo.GetLevel(0L);
					}
				}
				List<NewLoadingText> list = new List<NewLoadingText>();
				foreach (NewLoadingText current in this.m_LoadingText)
				{
					if (num >= current.m_nMinLV && num <= current.m_nMaxLV)
					{
						list.Add(current);
					}
				}
				if (0 < list.Count)
				{
					int index = UnityEngine.Random.Range(0, list.Count);
					NewLoadingText newLoadingText = list[index];
					if (newLoadingText != null && this.m_Tip != null)
					{
						this.m_Tip.Visible = true;
						this.m_Tip.Text = NrTSingleton<CTextParser>.Instance.GetTextColor("1002") + NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText(newLoadingText.m_szTextKey);
						if (string.Empty != newLoadingText.m_szBundlePath)
						{
							this.m_Left.color = new Color(0f, 0f, 0f, 0f);
							this.m_Center.color = new Color(0f, 0f, 0f, 0f);
							if (newLoadingText.m_nShowType == 1)
							{
								this.m_Left.Visible = true;
								this.m_Center.Visible = false;
								this.m_Left.SetFadeTextureFromBundle(newLoadingText.m_szBundlePath);
							}
							else if (newLoadingText.m_nShowType == 2)
							{
								this.m_Left.Visible = false;
								this.m_Center.Visible = true;
								this.m_Center.SetFadeTextureFromBundle(newLoadingText.m_szBundlePath);
							}
						}
					}
				}
				else
				{
					this.m_Tip.Visible = false;
				}
				list.Clear();
				this.Show();
			}
		}
		else if (base.Visible)
		{
			this.m_Left.DeleteMat();
			this.m_Center.DeleteMat();
			this.Hide();
			if (Scene.CurScene == Scene.Type.WORLD && Scene.PreScene != Scene.Type.SOLDIER_BATCH)
			{
				NrTSingleton<UIImageBundleManager>.Instance.DeleteTexture();
			}
		}
	}

	public void SetLoadingPageEffect(GameObject goEffect)
	{
		if (goEffect == null)
		{
			return;
		}
		this.m_goLoadingEffect = (UnityEngine.Object.Instantiate(goEffect) as GameObject);
		Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
		this.m_goLoadingEffect.transform.position = base.GetEffectUIPos(screenPos);
		this.m_goLoadingEffect.SetActive(true);
	}

	public void RemoveLoadingPageEffect()
	{
		if (this.m_goLoadingEffect != null)
		{
			UnityEngine.Object.Destroy(this.m_goLoadingEffect);
		}
	}

	public void ResetProgress(float fMax)
	{
		this.InitProgress(fMax);
	}

	private void InitProgress(float fMax)
	{
		this.m_fProgressMax = 100f;
		this.m_fProgressValue = 0f;
	}

	public void AddProgressValue(float fDelta)
	{
		float progressValue = this.m_fProgressValue + fDelta;
		this.SetProgressValue(progressValue);
	}

	public void SetProgressValue(float fValue)
	{
		if (fValue < this.m_fProgressValue)
		{
			return;
		}
		if (fValue > this.m_fProgressMax)
		{
			fValue = this.m_fProgressMax;
		}
		this.m_fProgressValue = fValue;
		this.m_Percent.Text = ((int)(this.m_fProgressValue * 100f)).ToString() + "%";
	}

	private void SetStageStatus()
	{
		if (this.m_lbLoadingStatus == null)
		{
			return;
		}
		Scene.Type currentStageType = StageSystem.GetCurrentStageType();
		this.m_lbLoadingStatus.Text = this.GetSceneTypeParsingValue(currentStageType);
	}

	private void SetPacketNum()
	{
		if (this.m_lbPacketNum == null)
		{
			return;
		}
		this.m_lbPacketNum.Text = string.Empty;
		long num = 0L;
		if (SendPacket.GetInstance() != null)
		{
			num = SendPacket.GetInstance().m_nLastSendPacketNum;
		}
		long num2 = NrReceiveGame.m_LastReceivePacketNum;
		num += (long)this.magicNum;
		num2 += (long)this.magicNum;
		if (num != (long)this.magicNum)
		{
			this.m_lbPacketNum.Text = num.ToString();
		}
		Label expr_7C = this.m_lbPacketNum;
		expr_7C.Text += "\n";
		if (num2 != (long)this.magicNum)
		{
			Label expr_A4 = this.m_lbPacketNum;
			expr_A4.Text += num2.ToString();
		}
	}

	private string GetSceneTypeParsingValue(Scene.Type sceneType)
	{
		string strTextKey = string.Empty;
		switch (sceneType)
		{
		case Scene.Type.EMPTY:
			strTextKey = "2047";
			break;
		case Scene.Type.ERROR:
			strTextKey = "2048";
			break;
		case Scene.Type.SYSCHECK:
			strTextKey = "2057";
			break;
		case Scene.Type.PREDOWNLOAD:
			strTextKey = "2053";
			break;
		case Scene.Type.NPATCH_DOWNLOAD:
			strTextKey = "2052";
			break;
		case Scene.Type.LOGIN:
			strTextKey = "2051";
			break;
		case Scene.Type.INITIALIZE:
			strTextKey = "2049";
			break;
		case Scene.Type.SELECTCHAR:
			strTextKey = "2055";
			break;
		case Scene.Type.PREPAREGAME:
			strTextKey = "2054";
			break;
		case Scene.Type.JUSTWAIT:
			strTextKey = "2050";
			break;
		case Scene.Type.WORLD:
			strTextKey = "2058";
			break;
		case Scene.Type.DUNGEON:
			strTextKey = "2046";
			break;
		case Scene.Type.BATTLE:
			strTextKey = "2044";
			break;
		case Scene.Type.CUTSCENE:
			strTextKey = "2045";
			break;
		case Scene.Type.SOLDIER_BATCH:
			strTextKey = "2056";
			break;
		}
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText(strTextKey);
	}
}
