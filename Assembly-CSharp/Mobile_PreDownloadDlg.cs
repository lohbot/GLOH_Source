using GameMessage.Private;
using Ndoors.Framework.Stage;
using NPatch;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Mobile_PreDownloadDlg : Form
{
	private DrawTexture textureBG_Img;

	private DrawTexture textureBG_Img1;

	private DrawTexture ProgressBar_ProgressBar1;

	private DrawTexture ProgressBar_ProgressBar2;

	private DrawTexture m_txTouchArea;

	private DrawTexture DrawTexture_NameLine;

	private Color BgColor = Color.black;

	private Label m_Percent;

	private Label m_FilePercent;

	private Label m_Text;

	private Button btnGameStart;

	private DrawTexture Drawtexture_DTBG03;

	private DrawTexture DT_Portrait01;

	private DrawTexture DT_Portrait02;

	private DrawTexture DT_Portrait03;

	private DrawTexture DT_CharVoice01;

	private DrawTexture DT_CharView;

	private DrawTexture DT_DrawTexture_DrawTexture29;

	private Label LB_CharName01;

	private Label LB_CVName01;

	private Label LB_CVName02;

	private Button BT_PlayVoice;

	private Label LB_Charinfo01;

	private Toggle Toggle_Portrait01;

	private Toggle Toggle_Portrait02;

	private Toggle Toggle_Portrait03;

	private Button BT_PlayMovie;

	private DrawTexture DT_IntroMovie;

	private Label LB_Open;

	private float m_fSize;

	private float m_fMaxSize;

	private int m_PortraitIndex = -1;

	private int m_VoiceIndex = 1;

	private float m_fTextTime;

	private int m_nTextCount;

	private float m_fValue;

	private float m_fSubValue;

	public bool m_bShowText;

	private GameObject pTouchEffectPrefab;

	private GameObject pTouchVoiceObject;

	private PatchLoading_Data m_PatchLoadingData;

	private GameObject pCharPrefab;

	private GameObject pRealChar;

	private Vector2 m_v2TouchStart = new Vector2(0f, 0f);

	private Vector3 m_v3TouchStart = new Vector3(0f, 0f, 0f);

	private float m_fAngle = 180f;

	private float m_fTempAngle;

	private string[] m_strActions = new string[]
	{
		"stay",
		"action1"
	};

	private float oldDelta;

	public bool ShowText
	{
		set
		{
			this.m_bShowText = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFile(ref form, "stage/dlg_preloadingpage", G_ID.PREDOWNLOAD_DLG, false);
		instance.CreateControl(ref this.textureBG_Img, "DrawTexture_DrawTexture0");
		instance.CreateControl(ref this.textureBG_Img1, "DrawTexture_DrawTexture1");
		instance.CreateControl(ref this.ProgressBar_ProgressBar1, "DrawTexture1");
		instance.CreateControl(ref this.ProgressBar_ProgressBar2, "DrawTexture2");
		instance.CreateControl(ref this.m_Percent, "Label_Label3");
		instance.CreateControl(ref this.m_FilePercent, "Label_Label4");
		instance.CreateControl(ref this.m_Text, "Label_text");
		instance.CreateControl(ref this.btnGameStart, "Button_GameStart");
		instance.CreateControl(ref this.Drawtexture_DTBG03, "DT_BG03");
		instance.CreateControl(ref this.DT_Portrait01, "DT_Portrait01");
		instance.CreateControl(ref this.DT_Portrait02, "DT_Portrait02");
		instance.CreateControl(ref this.DT_Portrait03, "DT_Portrait03");
		instance.CreateControl(ref this.DT_CharVoice01, "DT_CharVoice01");
		instance.CreateControl(ref this.DT_CharView, "DT_CharView");
		instance.CreateControl(ref this.DT_DrawTexture_DrawTexture29, "DrawTexture_DrawTexture29");
		instance.CreateControl(ref this.LB_CharName01, "LB_CharName01");
		instance.CreateControl(ref this.LB_CVName01, "LB_CVName01");
		instance.CreateControl(ref this.LB_CVName02, "LB_CVName02");
		instance.CreateControl(ref this.BT_PlayVoice, "BT_PlayVoice");
		instance.CreateControl(ref this.LB_Charinfo01, "LB_Charinfo01");
		instance.CreateControl(ref this.m_txTouchArea, "TouchArea");
		instance.CreateControl(ref this.DrawTexture_NameLine, "DrawTexture_NameLine");
		instance.CreateControl(ref this.Toggle_Portrait01, "Toggle_Portrait01");
		instance.CreateControl(ref this.Toggle_Portrait02, "Toggle_Portrait02");
		instance.CreateControl(ref this.Toggle_Portrait03, "Toggle_Portrait03");
		this.Toggle_Portrait01.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickTabControl));
		this.Toggle_Portrait02.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickTabControl));
		this.Toggle_Portrait03.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickTabControl));
		instance.CreateControl(ref this.BT_PlayMovie, "BT_PlayMovie");
		instance.CreateControl(ref this.DT_IntroMovie, "DT_IntroMovie");
		instance.CreateControl(ref this.LB_Open, "Label_Opening");
		this.BT_PlayMovie.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickPlayMovie));
		this.ProgressBar_ProgressBar1.transform.localScale = new Vector3(1.2f, 1f, 1f);
		this.m_fSize = this.ProgressBar_ProgressBar1.GetSize().x;
		this.m_fMaxSize = this.ProgressBar_ProgressBar1.GetSize().x * 1.2f;
		this.ProgressBar_ProgressBar1.SetTextureUVs(new Vector2(0f, 943f), new Vector2(0f, 30f));
		this.ProgressBar_ProgressBar2.transform.localScale = new Vector3(1.2f, 1f, 1f);
		this.m_fSize = this.ProgressBar_ProgressBar2.GetSize().x;
		this.m_fMaxSize = this.ProgressBar_ProgressBar2.GetSize().x * 1.2f;
		this.ProgressBar_ProgressBar2.SetTextureUVs(new Vector2(0f, 943f), new Vector2(0f, 30f));
		NrMainSystem.CheckAndSetReLoginMainCamera();
		this.BgColor = Camera.main.backgroundColor;
		if (Scene.CurScene == Scene.Type.PREDOWNLOAD || Scene.CurScene == Scene.Type.NPATCH_DOWNLOAD)
		{
			Camera.main.backgroundColor = this.BgColor;
		}
		this.SetTotalProgress(0f, 0f, string.Empty);
		this.m_Percent.Visible = false;
		this.m_FilePercent.Visible = false;
		this.m_Text.Visible = false;
		this.textureBG_Img.Visible = false;
		this.textureBG_Img1.Visible = false;
		this.ProgressBar_ProgressBar1.Visible = false;
		this.ProgressBar_ProgressBar2.Visible = false;
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		this.pTouchEffectPrefab = (CResources.LoadClone(NrTSingleton<UIDataManager>.Instance.FilePath + "Prefabs/fx_fingerpoint_ui") as GameObject);
		CResources.Delete(NrTSingleton<UIDataManager>.Instance.FilePath + "Prefabs/fx_fingerpoint_ui");
		this.pTouchVoiceObject = new GameObject("TouchVoiceObject");
		this.pTouchVoiceObject.transform.position = new Vector3(0f, 0f, -10f);
		this.pTouchVoiceObject.AddComponent<AudioSource>();
		this.m_txTouchArea.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_Click_Ani));
		this.btnGameStart.SetLocation(GUICamera.width / 2f - this.btnGameStart.GetSize().x / 2f, GUICamera.height - 110f, -1f);
		this.btnGameStart.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("2014"));
		this.btnGameStart.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_Click_GameStart));
		this.btnGameStart.Visible = false;
	}

	private void OnClickPlayMovie(IUIObject obj)
	{
		this.PlayMovie();
	}

	public void PlayMovie()
	{
		string str = "intro";
		string str2 = string.Format("{0}GameDrama/", Option.GetProtocolRootPath(Protocol.HTTP));
		NmMainFrameWork.PlayMovieURL(str2 + str + ".mp4", true, false, true);
	}

	private void OnClickTabControl(IUIObject obj)
	{
		if (this.Toggle_Portrait01.GetToggleState())
		{
			if (this.m_PortraitIndex == 1)
			{
				return;
			}
			this.SetPortraitIndex(1);
		}
		else if (this.Toggle_Portrait02.GetToggleState())
		{
			if (this.m_PortraitIndex == 2)
			{
				return;
			}
			this.SetPortraitIndex(2);
		}
		else if (this.Toggle_Portrait03.GetToggleState())
		{
			if (this.m_PortraitIndex == 3)
			{
				return;
			}
			this.SetPortraitIndex(3);
		}
	}

	public void SetGui()
	{
		if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.SELECTSERVER_DLG))
		{
			Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SELECTSERVER_DLG);
			if (form != null)
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.SELECTSERVER_DLG);
			}
		}
		this.m_PatchLoadingData = NrTSingleton<PatchLoading_Data_Manager>.Instance.GetData(1);
		Texture texture = this.LoadTexture(this.m_PatchLoadingData.Menu_Parser);
		this.DT_Portrait01.SetTexture(texture);
		this.m_PatchLoadingData = NrTSingleton<PatchLoading_Data_Manager>.Instance.GetData(2);
		texture = this.LoadTexture(this.m_PatchLoadingData.Menu_Parser);
		this.DT_Portrait02.SetTexture(texture);
		this.m_PatchLoadingData = NrTSingleton<PatchLoading_Data_Manager>.Instance.GetData(3);
		texture = this.LoadTexture(this.m_PatchLoadingData.Menu_Parser);
		this.DT_Portrait03.SetTexture(texture);
		this.BT_PlayVoice.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_Click_Voice));
		this.LB_CVName01.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("2021"));
		this.SetPortraitIndex(1);
		this.Toggle_Portrait01.SetToggleState(true);
	}

	private void SetPortraitIndex(int index)
	{
		if (index <= 0 || index > 3)
		{
			index = 1;
		}
		if (this.m_PortraitIndex == index)
		{
			return;
		}
		this.m_PatchLoadingData = NrTSingleton<PatchLoading_Data_Manager>.Instance.GetData(index);
		if (this.m_PatchLoadingData == null)
		{
			return;
		}
		string text = string.Empty;
		switch (index)
		{
		case 1:
			text = "Prefabs/Loding Char/" + this.m_PatchLoadingData.strPath;
			break;
		case 2:
			text = "Prefabs/Loding Char/" + this.m_PatchLoadingData.strPath;
			break;
		case 3:
			text = "Prefabs/Loding Char/" + this.m_PatchLoadingData.strPath;
			break;
		}
		if (!string.IsNullOrEmpty(text))
		{
			this.LoadChar(text);
		}
		this.m_bShowText = true;
		this.m_PortraitIndex = index;
		this.m_VoiceIndex = 1;
		string text2 = string.Empty;
		text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText(this.m_PatchLoadingData.iText01.ToString());
		this.LB_CharName01.SetText(text2);
		text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText(this.m_PatchLoadingData.iText02.ToString());
		this.LB_CVName02.SetText(text2);
		text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText(this.m_PatchLoadingData.iText03.ToString());
		this.LB_Charinfo01.SetText(text2);
		Texture texture = this.LoadBGTexture(this.m_PatchLoadingData.strBG);
		this.Drawtexture_DTBG03.SetTexture(texture);
	}

	public void SetBG(WWWItem _item, object _param)
	{
		if (this == null)
		{
			return;
		}
		if (_item.isCanceled)
		{
			return;
		}
		if (_item.GetSafeBundle() != null && null != _item.GetSafeBundle().mainAsset)
		{
			Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
			if (null != texture2D)
			{
				this.Drawtexture_DTBG03.SetTexture(texture2D);
			}
		}
	}

	private void LoadChar(string strfilename)
	{
		if (this.pCharPrefab != null)
		{
			UnityEngine.Object.DestroyImmediate(this.pCharPrefab);
			this.pCharPrefab = null;
		}
		this.pCharPrefab = (CResources.LoadClone(NrTSingleton<UIDataManager>.Instance.FilePath + strfilename) as GameObject);
		CResources.Delete(NrTSingleton<UIDataManager>.Instance.FilePath + strfilename);
		GameObject gameObject = GameObject.Find("UI Camera");
		if (gameObject != null)
		{
			Camera componentInChildren = gameObject.GetComponentInChildren<Camera>();
			if (componentInChildren != null)
			{
				Vector3 location = this.m_txTouchArea.GetLocation();
				location.x = this.m_txTouchArea.GetLocation().x + this.m_txTouchArea.GetSize().x / 2f;
				location.y = -(this.m_txTouchArea.GetLocationY() + this.m_txTouchArea.GetSize().y / 2f);
				location.z = 560f;
				this.pCharPrefab.transform.position = location;
				Transform transform = this.pCharPrefab.transform.Find("Loding Char");
				if (transform != null)
				{
					this.pRealChar = transform.gameObject;
				}
				Collider[] componentsInChildren = this.pCharPrefab.GetComponentsInChildren<Collider>();
				Collider[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					Collider collider = array[i];
					collider.enabled = false;
				}
				NkUtil.SetAllChildLayer(this.pCharPrefab, GUICamera.UILayer);
			}
		}
	}

	private Texture LoadBGTexture(string filename)
	{
		return CResources.Load(NrTSingleton<UIDataManager>.Instance.FilePath + "patchloading/BG/" + filename) as Texture;
	}

	private Texture LoadTexture(string filename)
	{
		return CResources.Load(NrTSingleton<UIDataManager>.Instance.FilePath + "patchloading/portrait/" + filename) as Texture;
	}

	public void SetLoadingSize(float fHeight = 0f)
	{
		float num = (GUICamera.width - this.m_fMaxSize) / 2f;
		this.ProgressBar_ProgressBar1.SetLocation(num, GUICamera.height - 62.5f - fHeight, -0.03f);
		this.ProgressBar_ProgressBar2.SetLocation(num, GUICamera.height - 110f - fHeight, -0.03f);
		this.m_Text.SetSize(GUICamera.width - num, this.m_Text.GetSize().y);
		this.m_Percent.SetLocation(this.ProgressBar_ProgressBar1.GetLocationX(), this.ProgressBar_ProgressBar1.GetLocationY());
		this.m_Percent.Text = "0%";
		this.m_FilePercent.SetLocation(this.ProgressBar_ProgressBar2.GetLocationX(), this.ProgressBar_ProgressBar2.GetLocationY());
		this.m_FilePercent.Text = "0%";
		this.textureBG_Img.SetSize(this.m_fMaxSize + 12f, this.textureBG_Img.GetSize().y);
		this.textureBG_Img.SetLocation(num - 6f, GUICamera.height - 68f - fHeight, -0.012f);
		this.textureBG_Img1.SetSize(this.m_fMaxSize + 12f, this.textureBG_Img1.GetSize().y);
		this.textureBG_Img1.SetLocation(num - 6f, GUICamera.height - 115.5f - fHeight, -0.012f);
	}

	public void SetText(string strText)
	{
		this.m_Percent.Visible = true;
		this.m_FilePercent.Visible = true;
		this.ProgressBar_ProgressBar1.Visible = true;
		this.textureBG_Img.Visible = true;
		this.textureBG_Img1.Visible = true;
		this.m_Percent.Text = strText;
		this.m_FilePercent.Text = string.Empty;
		float num = this.m_fSize * this.m_fValue;
		if (num < 0f)
		{
			num = 0f;
		}
		this.ProgressBar_ProgressBar1.SetSize(this.m_fSize * this.m_fValue, this.ProgressBar_ProgressBar1.GetSize().y);
		this.ProgressBar_ProgressBar1.SetTextureUVs(new Vector2(0f, 943f), new Vector2(num, 30f));
		num = this.m_fSize * this.m_fSubValue;
		if (num < 0f)
		{
			num = 0f;
		}
		this.ProgressBar_ProgressBar2.SetSize(this.m_fSize * this.m_fSubValue, this.ProgressBar_ProgressBar2.GetSize().y);
		this.ProgressBar_ProgressBar2.SetTextureUVs(new Vector2(0f, 943f), new Vector2(num, 30f));
		Mobile_PreDownloadDlg mobile_PreDownloadDlg = (Mobile_PreDownloadDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PREDOWNLOAD_DLG);
		if (mobile_PreDownloadDlg != null)
		{
			mobile_PreDownloadDlg.ShowText = true;
		}
		this.m_fTextTime = 0f;
		this.m_nTextCount = 0;
	}

	public void SetTextChange()
	{
		string text = "Check File...";
		if (this.m_fTextTime == 0f)
		{
			this.m_Percent.Text = text;
			this.m_fTextTime = Time.realtimeSinceStartup;
			this.m_nTextCount = 0;
		}
		else if (Time.realtimeSinceStartup - this.m_fTextTime > 0.1f)
		{
			this.m_nTextCount++;
			if (text.Length <= this.m_nTextCount)
			{
				this.m_nTextCount = 0;
				this.m_Percent.Text = text;
			}
			else
			{
				text = text.Remove(this.m_nTextCount);
				this.m_Percent.Text = text;
			}
			this.m_fTextTime = Time.realtimeSinceStartup;
		}
	}

	public void SetTotalProgress(float delta, float fSubDelta, string strText)
	{
		if (this.btnGameStart.Visible)
		{
			return;
		}
		this.m_Percent.Visible = true;
		this.m_FilePercent.Visible = true;
		this.m_Text.Visible = true;
		this.textureBG_Img.Visible = true;
		this.textureBG_Img1.Visible = true;
		this.ProgressBar_ProgressBar1.Visible = true;
		this.ProgressBar_ProgressBar2.Visible = true;
		this.DT_IntroMovie.Visible = true;
		if (delta < 0f)
		{
			delta = 0f;
		}
		if (delta > 1f)
		{
			delta = 1f;
		}
		if (fSubDelta < 0f)
		{
			fSubDelta = 0f;
		}
		if (fSubDelta > 1f)
		{
			fSubDelta = 1f;
		}
		this.m_fValue = delta;
		this.m_fSubValue = fSubDelta;
		if (this.m_fValue <= 0f)
		{
			this.SetTextChange();
		}
		else
		{
			this.m_Percent.Text = ((int)(this.m_fValue * 100f)).ToString() + "%";
		}
		if (this.m_fSubValue < 0f)
		{
			this.m_FilePercent.Text = string.Empty;
		}
		else
		{
			this.m_FilePercent.Text = strText;
		}
		float num = this.m_fSize * this.m_fValue;
		if (num < 0f)
		{
			num = 0f;
		}
		this.ProgressBar_ProgressBar1.SetSize(this.m_fSize * this.m_fValue, this.ProgressBar_ProgressBar1.GetSize().y);
		this.ProgressBar_ProgressBar1.SetTextureUVs(new Vector2(0f, 943f), new Vector2(num, 30f));
		num = this.m_fSize * this.m_fSubValue;
		if (num < 0f)
		{
			num = 0f;
		}
		this.ProgressBar_ProgressBar2.SetSize(this.m_fSize * this.m_fSubValue, this.ProgressBar_ProgressBar2.GetSize().y);
		this.ProgressBar_ProgressBar2.SetTextureUVs(new Vector2(0f, 943f), new Vector2(num, 30f));
		Mobile_PreDownloadDlg mobile_PreDownloadDlg = (Mobile_PreDownloadDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PREDOWNLOAD_DLG);
		if (mobile_PreDownloadDlg != null)
		{
			mobile_PreDownloadDlg.ShowText = true;
		}
	}

	public void UpdateTotalProgress(float delta, float fSubdelta, long totalCount, string strText)
	{
		if (this.btnGameStart.Visible)
		{
			return;
		}
		this.m_Percent.Visible = true;
		this.m_FilePercent.Visible = true;
		this.ProgressBar_ProgressBar1.Visible = true;
		this.ProgressBar_ProgressBar2.Visible = true;
		this.textureBG_Img.Visible = true;
		this.textureBG_Img1.Visible = true;
		this.DT_IntroMovie.Visible = true;
		if (delta < 0f)
		{
			delta = 0f;
		}
		if (delta > 1f)
		{
			delta = 1f;
		}
		if (delta == 0f)
		{
			this.SetTextChange();
			return;
		}
		if (fSubdelta < 0f)
		{
			fSubdelta = 0f;
		}
		if (fSubdelta > 1f)
		{
			fSubdelta = 1f;
		}
		if (delta == 0f)
		{
			this.oldDelta = 0f;
		}
		float num = delta - this.oldDelta;
		if (1f <= num)
		{
			return;
		}
		num /= (float)totalCount;
		float num2 = this.m_fValue + num;
		if (1f <= num2)
		{
			num2 = 1f;
		}
		this.m_fValue = num2;
		this.m_Percent.Text = ((int)(this.m_fValue * 100f)).ToString() + "%";
		this.m_FilePercent.Text = strText;
		float num3 = this.m_fSize * this.m_fValue;
		if (num3 < 0f)
		{
			num3 = 0f;
		}
		this.ProgressBar_ProgressBar1.SetSize(this.m_fSize * this.m_fValue, this.ProgressBar_ProgressBar1.GetSize().y);
		this.ProgressBar_ProgressBar1.SetTextureUVs(new Vector2(0f, 943f), new Vector2(num3, 30f));
		this.m_fSubValue = fSubdelta;
		num3 = this.m_fSize * this.m_fSubValue;
		if (num3 < 0f)
		{
			num3 = 0f;
		}
		this.ProgressBar_ProgressBar2.SetSize(this.m_fSize * this.m_fSubValue, this.ProgressBar_ProgressBar2.GetSize().y);
		this.ProgressBar_ProgressBar2.SetTextureUVs(new Vector2(0f, 943f), new Vector2(num3, 30f));
		this.oldDelta = delta;
		Mobile_PreDownloadDlg mobile_PreDownloadDlg = (Mobile_PreDownloadDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PREDOWNLOAD_DLG);
		if (mobile_PreDownloadDlg != null)
		{
			mobile_PreDownloadDlg.ShowText = true;
		}
	}

	public override void SetComponent()
	{
		base.SetSize(GUICamera.width, GUICamera.height);
		base.SetLocation(0f, 0f);
		this.Drawtexture_DTBG03.SetSize(GUICamera.width, GUICamera.height);
		this.DT_IntroMovie.SetLocation(0, 0);
		float num = GUICamera.width * 0.9f;
		float num2 = GUICamera.height * 0.05f;
		this.DT_DrawTexture_DrawTexture29.SetLocation(num, num2);
		this.BT_PlayMovie.SetLocation(num + 29f, num2 + 15f);
		this.LB_Open.SetLocation(this.BT_PlayMovie.GetLocationX(), this.BT_PlayMovie.GetLocationY() + this.BT_PlayMovie.width - 18f);
		this.Toggle_Portrait01.SetLocation(this.BT_PlayMovie.GetLocationX() - 22f, this.BT_PlayMovie.GetLocationY() + this.BT_PlayMovie.GetSize().y + 12f);
		this.Toggle_Portrait02.SetLocation(this.Toggle_Portrait01.GetLocationX(), this.Toggle_Portrait01.GetLocationY() + this.Toggle_Portrait01.GetSize().y);
		this.Toggle_Portrait03.SetLocation(this.Toggle_Portrait01.GetLocationX(), this.Toggle_Portrait02.GetLocationY() + this.Toggle_Portrait02.GetSize().y);
		this.DT_Portrait01.SetLocation(this.Toggle_Portrait01.GetLocationX() + 22f, this.Toggle_Portrait01.GetLocationY() + 25f);
		this.DT_Portrait02.SetLocation(this.Toggle_Portrait02.GetLocationX() + 22f, this.Toggle_Portrait02.GetLocationY() + 25f);
		this.DT_Portrait03.SetLocation(this.Toggle_Portrait03.GetLocationX() + 22f, this.Toggle_Portrait03.GetLocationY() + 25f);
		num2 = GUICamera.height * 0.65f;
		this.LB_Open.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("2034");
		this.SetLoadingSize(10f);
		Texture texture = (Texture)CResources.Load(NrTSingleton<UIDataManager>.Instance.FilePath + "Texture/loginBG_mobile");
		this.Drawtexture_DTBG03.SetTexture(texture);
		this.SetGui();
	}

	private void ControlVisible(bool flag)
	{
		this.Toggle_Portrait01.Visible = flag;
		this.Toggle_Portrait02.Visible = flag;
		this.Toggle_Portrait03.Visible = flag;
		this.DT_Portrait01.Visible = flag;
		this.DT_Portrait02.Visible = flag;
		this.DT_Portrait03.Visible = flag;
		this.DT_IntroMovie.Visible = flag;
		this.BT_PlayVoice.Visible = flag;
		this.LB_CharName01.Visible = flag;
		this.LB_CVName01.Visible = flag;
		this.LB_CVName02.Visible = flag;
		this.BT_PlayVoice.Visible = flag;
		this.LB_Charinfo01.Visible = flag;
	}

	public override void Update()
	{
		if (TsPlatform.IsMobile)
		{
			if (this.m_FilePercent != null && this.m_Percent != null)
			{
				string text = string.Empty;
				if (this.m_FilePercent.Visible)
				{
					text = this.m_FilePercent.Text;
				}
				if (this.m_Percent.Visible)
				{
					text = text + "\nTotal : " + this.m_Percent.Text;
				}
				NrTSingleton<NrUserDeviceInfo>.Instance.SetMovieText(text);
			}
			Vector3 mousePosition = Input.mousePosition;
			if (NkInputManager.GetMouseButton(0) && this.m_v3TouchStart != Vector3.zero && this.m_v3TouchStart != mousePosition)
			{
				if (mousePosition != Vector3.zero && this.pRealChar != null && Mathf.Abs(this.m_v2TouchStart.x - mousePosition.x) > 5f)
				{
					this.m_fTempAngle = 360f * ((this.m_v2TouchStart.x - mousePosition.x) / (float)Screen.width);
					Quaternion rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.AngleAxis(this.m_fAngle + this.m_fTempAngle, Vector3.up), Time.time * 0.1f);
					this.pRealChar.transform.rotation = rotation;
				}
			}
			else if (NkInputManager.GetMouseButtonUp(0))
			{
				if (mousePosition != Vector3.zero && this.pTouchEffectPrefab != null)
				{
					this.SetTouchEffcet(new Vector2(mousePosition.x, (float)Screen.height - mousePosition.y));
					this.m_v3TouchStart = Vector3.zero;
				}
			}
			else if (NkInputManager.GetMouseButtonDown(0) && mousePosition != Vector3.zero)
			{
				this.m_v2TouchStart = new Vector2(mousePosition.x, (float)Screen.height - mousePosition.y);
				this.m_v3TouchStart = mousePosition;
				this.m_fAngle = this.pRealChar.transform.rotation.eulerAngles.y;
			}
		}
	}

	public void SetTextImg(int nIndex)
	{
		if (this.m_PatchLoadingData == null)
		{
			return;
		}
		this.m_Text.SetLocation(this.ProgressBar_ProgressBar2.GetLocationX(), this.ProgressBar_ProgressBar2.GetLocationY() - 70f);
		if (Launcher.Instance.LocalPatchLevel == 0)
		{
			this.m_Text.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("42");
		}
		else
		{
			this.m_Text.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("43");
		}
	}

	private void On_Click_History(IUIObject a_oObject)
	{
		if (this.m_PatchLoadingData == null)
		{
			return;
		}
		this.m_PatchLoadingData = NrTSingleton<PatchLoading_Data_Manager>.Instance.GetData(2);
		this.m_bShowText = true;
	}

	private void On_Click_Hero(IUIObject a_oObject)
	{
		if (this.m_PatchLoadingData == null)
		{
			return;
		}
		this.m_PatchLoadingData = NrTSingleton<PatchLoading_Data_Manager>.Instance.GetData(3);
		this.m_bShowText = true;
	}

	private void On_Click_Battle(IUIObject a_oObject)
	{
		if (this.m_PatchLoadingData == null)
		{
			return;
		}
		this.m_PatchLoadingData = NrTSingleton<PatchLoading_Data_Manager>.Instance.GetData(4);
		this.m_bShowText = true;
	}

	private void On_Click_GameStart(IUIObject a_oObject)
	{
		FacadeHandler.MoveStage(Scene.Type.INITIALIZE);
	}

	private void SetTouchEffcet(Vector2 ScreenPos)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.pTouchEffectPrefab, Vector3.zero, Quaternion.identity);
		if (null == gameObject)
		{
			return;
		}
		Vector3 effectUIPos = base.GetEffectUIPos(ScreenPos);
		effectUIPos.z = 0f;
		gameObject.transform.position = effectUIPos;
		NkUtil.SetAllChildLayer(gameObject, GUICamera.UILayer);
		UnityEngine.Object.Destroy(gameObject, 1f);
	}

	public void ShowStartButton(bool bShow = true)
	{
		this.btnGameStart.Visible = bShow;
		this.m_Percent.Visible = !bShow;
		this.m_FilePercent.Visible = !bShow;
		this.m_Text.Visible = !bShow;
		this.ProgressBar_ProgressBar1.Visible = !bShow;
		this.ProgressBar_ProgressBar2.Visible = !bShow;
		this.textureBG_Img.Visible = !bShow;
		this.textureBG_Img1.Visible = !bShow;
		this.DT_IntroMovie.Visible = !bShow;
	}

	public override void OnClose()
	{
		base.OnClose();
		if (this.pTouchEffectPrefab != null)
		{
			UnityEngine.Object.DestroyImmediate(this.pTouchEffectPrefab);
		}
		if (this.pTouchVoiceObject != null)
		{
			UnityEngine.Object.DestroyImmediate(this.pTouchVoiceObject);
			this.pTouchVoiceObject = null;
		}
		if (this.pCharPrefab != null)
		{
			UnityEngine.Object.DestroyImmediate(this.pCharPrefab);
		}
		this.pTouchEffectPrefab = null;
		this.pCharPrefab = null;
	}

	public void CharDestroy()
	{
		if (this.pCharPrefab != null)
		{
			UnityEngine.Object.Destroy(this.pCharPrefab);
		}
		this.pCharPrefab = null;
	}

	private void On_Click_Ani(IUIObject a_oObject)
	{
		Animation componentInChildren = this.pCharPrefab.GetComponentInChildren<Animation>();
		if (componentInChildren != null)
		{
			string text = string.Empty;
			if (this.m_strActions.Length > this.m_PortraitIndex)
			{
				this.m_PortraitIndex = 0;
			}
			text = this.m_strActions[1];
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (componentInChildren.GetClip(text) != null)
			{
				componentInChildren.CrossFadeQueued(this.m_strActions[1], 0.5f, QueueMode.PlayNow);
				componentInChildren.CrossFadeQueued(this.m_strActions[0], 0.5f, QueueMode.CompleteOthers);
			}
		}
	}

	private void LoadViocePlay(string filename)
	{
		if (!string.IsNullOrEmpty(filename))
		{
			AudioClip audioClip = CResources.Load(filename) as AudioClip;
			if (audioClip == null)
			{
				TsLog.Log("!!PatchLoadingData Voice File[ {0} ]== NULL!!", new object[]
				{
					filename
				});
				return;
			}
			if (this.pTouchVoiceObject.GetComponent<AudioSource>() == null)
			{
				this.pTouchVoiceObject.AddComponent<AudioSource>();
			}
			AudioSource component = this.pTouchVoiceObject.GetComponent<AudioSource>();
			if (component != null)
			{
				component.PlayOneShot(audioClip);
			}
			else
			{
				TsLog.LogWarning("!!AudioSource Play Error", new object[0]);
			}
		}
	}

	private void StopVioce()
	{
		if (this.pTouchVoiceObject.GetComponent<AudioSource>() != null)
		{
			AudioSource component = this.pTouchVoiceObject.GetComponent<AudioSource>();
			if (component != null)
			{
				component.Stop();
			}
		}
	}

	private void On_Click_Voice(IUIObject a_oObject)
	{
		Button x = a_oObject as Button;
		if (x == null)
		{
			return;
		}
		this.StopVioce();
		if (this.m_VoiceIndex < 0 || this.m_VoiceIndex > 3)
		{
			this.m_VoiceIndex = 1;
		}
		string text = string.Empty;
		switch (this.m_VoiceIndex)
		{
		case 1:
			text = NrTSingleton<UIDataManager>.Instance.FilePath + "patchloading/Voice/" + this.m_PatchLoadingData.strVoice01;
			break;
		case 2:
			text = NrTSingleton<UIDataManager>.Instance.FilePath + "patchloading/Voice/" + this.m_PatchLoadingData.strVoice02;
			break;
		case 3:
			text = NrTSingleton<UIDataManager>.Instance.FilePath + "patchloading/Voice/" + this.m_PatchLoadingData.strVoice03;
			break;
		}
		if (!string.IsNullOrEmpty(text))
		{
			this.LoadViocePlay(text);
			this.m_VoiceIndex++;
		}
	}
}
