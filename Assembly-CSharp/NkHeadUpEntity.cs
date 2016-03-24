using GAME;
using Ndoors.Memory;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class NkHeadUpEntity : IDisposable
{
	private const float m_fRideDiffConstValue = 1.5f;

	private const float m_fNameDiffConstValue = 0.3f;

	private const float m_fChatDiffConstValue = 2.2f;

	private const float m_fStatusDiffConstValue = 2.8f;

	private const float m_fGuildDiffConstValue = 2f;

	private Transform m_pkNameDummy;

	private GameObject m_pkHeadUpRoot;

	private GameObject m_pkNameSprite;

	private GameObject m_pkChatSprite;

	private GameObject m_pkCharStatus;

	private GameObject m_pkGuildSprite;

	private bool m_bBattleChar;

	private bool m_bSubChar;

	private bool m_bShowHeadUp;

	private float m_fChatShowTime;

	private bool m_bCheckChatShowStatus;

	private float m_fCurrentScale = 1f;

	private Vector3 m_kCurrentScale = Vector3.zero;

	private float m_fDiffScaleRate = 1f;

	private float m_fBaseScale = 1f;

	private Vector3 m_kTempVector3 = Vector3.zero;

	private string m_szColorText = string.Empty;

	private Queue<string> m_queueNpcTalk = new Queue<string>();

	public NkHeadUpEntity()
	{
		this.Init();
		this.m_queueNpcTalk.Clear();
	}

	public void Init()
	{
		this.m_pkNameDummy = null;
		this.m_pkHeadUpRoot = null;
		this.m_pkNameSprite = null;
		this.m_pkChatSprite = null;
		this.m_pkCharStatus = null;
		this.m_pkGuildSprite = null;
		this.m_bBattleChar = false;
		this.m_bSubChar = false;
		this.m_bShowHeadUp = false;
		this.m_fChatShowTime = 0f;
		this.m_bCheckChatShowStatus = false;
		this.m_fCurrentScale = 0.2f;
		this.m_kCurrentScale.x = this.m_fCurrentScale;
		this.m_kCurrentScale.y = this.m_fCurrentScale;
		this.m_kCurrentScale.z = this.m_fCurrentScale;
		this.m_fDiffScaleRate = 1f;
		this.m_fBaseScale = 1f;
	}

	public void Dispose()
	{
		this.DisposeGameObject(this.m_pkHeadUpRoot);
		this.DisposeGameObject(this.m_pkNameSprite);
		this.DisposeGameObject(this.m_pkChatSprite);
		this.DisposeGameObject(this.m_pkCharStatus);
		this.DisposeGameObject(this.m_pkGuildSprite);
		this.Init();
	}

	public void SetColorText(string color)
	{
		this.m_szColorText = color;
	}

	private void DisposeGameObject(GameObject pkTarget)
	{
		if (pkTarget != null)
		{
			UnityEngine.Object.DestroyImmediate(pkTarget);
			pkTarget = null;
		}
	}

	public Transform GetNameDummy()
	{
		return this.m_pkNameDummy;
	}

	public GameObject GetHeadUpRoot()
	{
		return this.m_pkHeadUpRoot;
	}

	public GameObject GetNameSprite()
	{
		return this.m_pkNameSprite;
	}

	public GameObject GetChatSprite()
	{
		return this.m_pkChatSprite;
	}

	public GameObject GetUserStatus()
	{
		return this.m_pkCharStatus;
	}

	public GameObject GetCharGuilde()
	{
		return this.m_pkGuildSprite;
	}

	public bool FindNameDummy(GameObject pkBaseObject)
	{
		if (pkBaseObject == null)
		{
			return false;
		}
		this.m_pkNameDummy = NkUtil.GetChild(pkBaseObject.transform, "dmname");
		if (this.m_pkNameDummy == null)
		{
			return false;
		}
		this.m_kTempVector3.x = 1f;
		this.m_kTempVector3.y = 1f;
		this.m_kTempVector3.z = 1f;
		this.m_pkNameDummy.localScale = this.m_kTempVector3;
		return true;
	}

	public void SetLinkHeadUpRoot(GameObject pkBaseObject, bool BattleChar, int basescale)
	{
		if (pkBaseObject != null)
		{
			if (!this.FindNameDummy(pkBaseObject))
			{
				return;
			}
			if (this.m_pkHeadUpRoot == null)
			{
				this.m_pkHeadUpRoot = new GameObject("HeadUpEntity");
				this.m_pkHeadUpRoot.layer = TsLayer.PC;
			}
			this.m_pkHeadUpRoot.transform.parent = this.m_pkNameDummy.transform;
			if (!BattleChar)
			{
				this.m_pkHeadUpRoot.transform.localPosition = Vector3.zero;
			}
			else
			{
				this.m_pkHeadUpRoot.transform.localPosition = new Vector3(0f, -0.3f, 0f);
			}
			this.m_pkHeadUpRoot.transform.localScale = this.m_kCurrentScale;
			if (pkBaseObject.transform.parent != null)
			{
				CharacterController component = pkBaseObject.transform.parent.GetComponent<CharacterController>();
				if (component != null)
				{
					this.m_fDiffScaleRate = Mathf.Max(Mathf.Max(component.bounds.size.x, component.bounds.size.y), component.bounds.size.z);
					this.m_fDiffScaleRate = Mathf.Min(1f, Mathf.Max(0.4f, this.m_fDiffScaleRate * 3f));
				}
			}
			else
			{
				Debug.LogWarning("!!! SetLinkHeadUpRoot BaseObject.parent is null (name = " + pkBaseObject.name);
			}
			this.m_fBaseScale = pkBaseObject.transform.localScale.x;
			if (basescale > 0)
			{
				this.m_fBaseScale *= (float)basescale / 10f;
			}
			if (this.m_fBaseScale == 0f)
			{
				this.m_fBaseScale = 1f;
			}
			this.m_bBattleChar = BattleChar;
		}
	}

	public CharNameBillboardSprite CreateNameSprite()
	{
		if (this.m_pkHeadUpRoot == null)
		{
			return null;
		}
		this.DisposeGameObject(this.m_pkNameSprite);
		if (this.m_pkNameSprite == null)
		{
			this.m_pkNameSprite = CResources.ADDPrefabLoad(this.m_pkHeadUpRoot, NrTSingleton<UIDataManager>.Instance.FilePath + "Prefabs/CharName");
		}
		CharNameBillboardSprite component = this.m_pkNameSprite.GetComponent<CharNameBillboardSprite>();
		if (component == null)
		{
			return null;
		}
		component.Init();
		return component;
	}

	public CharChatBillboardSprite CreateChatSprite()
	{
		if (this.m_pkHeadUpRoot == null)
		{
			return null;
		}
		this.DisposeGameObject(this.m_pkChatSprite);
		if (this.m_pkChatSprite == null)
		{
			this.m_pkChatSprite = CResources.ADDPrefabLoad(this.m_pkHeadUpRoot, NrTSingleton<UIDataManager>.Instance.FilePath + "Prefabs/CharEmoticonChat");
		}
		CharChatBillboardSprite component = this.m_pkChatSprite.GetComponent<CharChatBillboardSprite>();
		if (component == null)
		{
			return null;
		}
		component.Init();
		return component;
	}

	public bool CreateCharStatus()
	{
		if (this.m_pkHeadUpRoot == null)
		{
			return false;
		}
		this.DisposeGameObject(this.m_pkCharStatus);
		if (this.m_pkCharStatus == null)
		{
			this.m_pkCharStatus = new GameObject("CharStatus");
			this.m_pkCharStatus.transform.parent = this.m_pkHeadUpRoot.transform;
			this.m_pkCharStatus.transform.localPosition = new Vector3(0f, 2.8f, 0f);
			this.m_pkCharStatus.transform.localScale = new Vector3(1f, 1f, 1f);
			this.m_pkCharStatus.transform.localRotation = Quaternion.identity;
		}
		return true;
	}

	public void MakeName(eCharKindType chartype, string strMark, string charname, bool ridestate)
	{
		bool flag = false;
		if (this.m_pkNameSprite == null)
		{
			flag = true;
		}
		CharNameBillboardSprite charNameBillboardSprite = this.CreateNameSprite();
		if (charNameBillboardSprite == null)
		{
			return;
		}
		float num = 0.3f;
		switch (chartype)
		{
		case eCharKindType.CKT_USER:
		{
			string textColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1002");
			charNameBillboardSprite.SetText(textColor + charname);
			charNameBillboardSprite.SetTextSize(12f);
			if (ridestate)
			{
				num += 1.5f;
			}
			if (strMark.Length != 0)
			{
				this.MakeCharRank(strMark, charNameBillboardSprite, num);
				charNameBillboardSprite.ShowTextAndPlane(true, false, true);
			}
			else
			{
				charNameBillboardSprite.ShowTextAndPlane(true, false, false);
			}
			break;
		}
		case eCharKindType.CKT_SOLDIER:
		{
			string textColor2 = NrTSingleton<CTextParser>.Instance.GetTextColor("1002");
			charNameBillboardSprite.SetText(textColor2 + charname);
			charNameBillboardSprite.SetTextSize(12f);
			charNameBillboardSprite.ShowTextAndPlane(true, false, false);
			if (ridestate)
			{
				num += 1.5f;
			}
			break;
		}
		case eCharKindType.CKT_MONSTER:
		{
			string textColor3 = NrTSingleton<CTextParser>.Instance.GetTextColor("1404");
			charNameBillboardSprite.SetText(textColor3 + charname);
			charNameBillboardSprite.SetTextSize(12f);
			charNameBillboardSprite.ShowTextAndPlane(true, false, false);
			break;
		}
		case eCharKindType.CKT_NPC:
		{
			string textColor4 = NrTSingleton<CTextParser>.Instance.GetTextColor("1404");
			charNameBillboardSprite.SetText(textColor4 + charname);
			charNameBillboardSprite.SetTextSize(12f);
			charNameBillboardSprite.ShowTextAndPlane(true, false, false);
			break;
		}
		case eCharKindType.CKT_OBJECT:
		{
			string textColor5 = NrTSingleton<CTextParser>.Instance.GetTextColor("1205");
			charNameBillboardSprite.SetText(textColor5 + charname);
			charNameBillboardSprite.SetTextSize(12f);
			charNameBillboardSprite.ShowTextAndPlane(true, true, false);
			break;
		}
		}
		if (flag)
		{
			this.m_kTempVector3.x = 0f;
			this.m_kTempVector3.y = num;
			this.m_kTempVector3.z = 0f;
			this.m_pkNameSprite.transform.localPosition = this.m_kTempVector3;
		}
		this.SyncBillboardRotate(true);
		bool showHeadUp = this.IsCheckShowHeadUp(chartype);
		this.SetShowHeadUp(showHeadUp);
	}

	public bool MakeChat(eCharKindType chartype, string chatText, string guildName, bool ridestate, bool checkshowstatus)
	{
		if (chatText.Length <= 0)
		{
			return false;
		}
		if (this.m_bCheckChatShowStatus && chartype == eCharKindType.CKT_NPC)
		{
			this.m_queueNpcTalk.Enqueue(chatText);
			return false;
		}
		CharChatBillboardSprite charChatBillboardSprite = this.CreateChatSprite();
		if (charChatBillboardSprite == null)
		{
			return false;
		}
		float num = 2.2f;
		float num2 = 0f;
		charChatBillboardSprite.SetFontEffect(SpriteText.Font_Effect.HeadUp);
		switch (chartype)
		{
		case eCharKindType.CKT_USER:
		{
			string first = "[#FFFFFFFF]";
			charChatBillboardSprite.SetText(NrTSingleton<UIDataManager>.Instance.GetString(first, chatText));
			charChatBillboardSprite.SetPlaneKey("Win_T_BK", ref num2);
			if (ridestate)
			{
				num += 1.5f;
			}
			if (string.Empty != guildName)
			{
				num += 2f;
			}
			break;
		}
		case eCharKindType.CKT_SOLDIER:
		{
			string textColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1002");
			charChatBillboardSprite.SetText(NrTSingleton<UIDataManager>.Instance.GetString(textColor, chatText));
			break;
		}
		case eCharKindType.CKT_MONSTER:
		{
			string textColor2 = NrTSingleton<CTextParser>.Instance.GetTextColor("1108");
			charChatBillboardSprite.SetText(textColor2 + chatText);
			break;
		}
		case eCharKindType.CKT_NPC:
		{
			char[] separator = new char[]
			{
				'+'
			};
			string[] array = chatText.Split(separator);
			if (this.m_bCheckChatShowStatus)
			{
				if (string.Empty != this.m_szColorText)
				{
					for (int i = 0; i < array.Length; i++)
					{
						string @string = NrTSingleton<UIDataManager>.Instance.GetString(this.m_szColorText, array[i]);
						this.m_queueNpcTalk.Enqueue(@string);
					}
				}
				else
				{
					for (int j = 0; j < array.Length; j++)
					{
						string string2 = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<CTextParser>.Instance.GetTextColor("1404"), array[j]);
						this.m_queueNpcTalk.Enqueue(string2);
					}
				}
			}
			else
			{
				if (2 <= array.Length)
				{
					if (string.Empty != this.m_szColorText)
					{
						string string3 = NrTSingleton<UIDataManager>.Instance.GetString(this.m_szColorText, array[0]);
						charChatBillboardSprite.SetText(string3);
						for (int k = 1; k < array.Length; k++)
						{
							string string4 = NrTSingleton<UIDataManager>.Instance.GetString(this.m_szColorText, array[k]);
							this.m_queueNpcTalk.Enqueue(string4);
						}
					}
					else
					{
						string string5 = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<CTextParser>.Instance.GetTextColor("1404"), array[0]);
						charChatBillboardSprite.SetText(string5);
						for (int l = 1; l < array.Length; l++)
						{
							string string6 = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<CTextParser>.Instance.GetTextColor("1404"), array[l]);
							this.m_queueNpcTalk.Enqueue(string6);
						}
					}
				}
				else if (string.Empty != this.m_szColorText)
				{
					charChatBillboardSprite.SetText(this.m_szColorText + chatText);
				}
				else
				{
					string textColor3 = NrTSingleton<CTextParser>.Instance.GetTextColor("1404");
					charChatBillboardSprite.SetText(NrTSingleton<UIDataManager>.Instance.GetString(textColor3, chatText));
				}
				charChatBillboardSprite.SetPlaneKey("Win_T_BK", ref num2);
			}
			break;
		}
		}
		charChatBillboardSprite.ShowTextAndPlane(true, true);
		this.m_kTempVector3.x = 0f;
		this.m_kTempVector3.y = num + num2;
		this.m_kTempVector3.z = 0f;
		this.m_pkChatSprite.transform.localPosition = this.m_kTempVector3;
		this.m_fChatShowTime = Time.time;
		this.m_bCheckChatShowStatus = checkshowstatus;
		this.SyncBillboardRotate(true);
		bool showHeadUp = this.IsCheckShowHeadUp(chartype);
		this.SetShowHeadUp(showHeadUp);
		return true;
	}

	public bool MakeCharStatus(eCharKindType chartype, GameObject pkCharStatus, float fScale)
	{
		this.SyncBillboardRotate(true);
		if (!this.CreateCharStatus())
		{
			return false;
		}
		pkCharStatus.transform.parent = this.m_pkCharStatus.transform;
		pkCharStatus.transform.localPosition = Vector3.zero;
		pkCharStatus.transform.localRotation = Quaternion.identity;
		pkCharStatus.transform.localScale = new Vector3(fScale, fScale, fScale);
		bool showHeadUp = this.IsCheckShowHeadUp(chartype);
		this.SetShowHeadUp(showHeadUp);
		return true;
	}

	public void MakeCharGuild(eCharKindType chartype, long i64GuildID, string strGuildName, bool bGuildWar, bool ridestate)
	{
		if (this.m_pkHeadUpRoot == null)
		{
			return;
		}
		this.DisposeGameObject(this.m_pkGuildSprite);
		if (this.m_pkGuildSprite == null)
		{
			this.m_pkGuildSprite = CResources.ADDPrefabLoad(this.m_pkHeadUpRoot, NrTSingleton<UIDataManager>.Instance.FilePath + "Prefabs/CharName");
			if (this.m_pkGuildSprite != null)
			{
				CharNameBillboardSprite component = this.m_pkGuildSprite.GetComponent<CharNameBillboardSprite>();
				if (component == null)
				{
					TsLog.LogWarning("!!!!!!!!!!!!!!!!!!!!! m_pkGuildSprite NULL ", new object[0]);
					return;
				}
				component.Init();
				if (strGuildName.Length != 0)
				{
					string str = string.Empty;
					if (bGuildWar)
					{
						str = NrTSingleton<CTextParser>.Instance.GetTextColor("1401");
					}
					else
					{
						str = NrTSingleton<CTextParser>.Instance.GetTextColor("2005");
					}
					component.SetText(str + strGuildName);
					component.SetTextSize(12f);
					this.MakeCharGuildRank(component);
					if (i64GuildID != 0L)
					{
						string guildPortraitURL = NrTSingleton<NkCharManager>.Instance.GetGuildPortraitURL(i64GuildID);
						WebFileCache.RequestImageWebFile(guildPortraitURL, new WebFileCache.ReqTextureCallback(this.ReqWebImageCallback), component);
					}
					else
					{
						Transform transform = component.transform.FindChild("CharRank");
						if (transform != null)
						{
							DrawTexture drawTexture = (DrawTexture)transform.GetComponent(typeof(DrawTexture));
							drawTexture.SetTexture(NrTSingleton<NewGuildManager>.Instance.GetGuildDefualtTexture());
							component.ShowTextAndPlane(true, false, true);
						}
						else
						{
							component.ShowTextAndPlane(true, false, false);
						}
					}
					this.m_kTempVector3.x = 0f;
					this.m_kTempVector3.y = 2f;
					this.m_kTempVector3.z = 0f;
					this.m_pkGuildSprite.transform.localPosition = this.m_kTempVector3;
				}
				this.SyncBillboardRotate(true);
				bool showHeadUp = this.IsCheckShowHeadUp(chartype);
				this.SetShowHeadUp(showHeadUp);
			}
		}
	}

	private void ReqWebImageCallback(Texture2D txtr, object _param)
	{
		CharNameBillboardSprite component = this.m_pkGuildSprite.GetComponent<CharNameBillboardSprite>();
		if (component == null)
		{
			TsLog.LogWarning("!!!!!!!!!!!!!!!!!!!!! _GuildNameSprite GuildID  NOT ", new object[0]);
			return;
		}
		Transform transform = component.transform.FindChild("CharRank");
		if (transform != null)
		{
			DrawTexture drawTexture = (DrawTexture)transform.GetComponent(typeof(DrawTexture));
			if (txtr == null)
			{
				drawTexture.SetTexture(NrTSingleton<NewGuildManager>.Instance.GetGuildDefualtTexture());
			}
			else
			{
				drawTexture.SetTexture(txtr);
			}
			component.ShowTextAndPlane(true, false, true);
		}
		else
		{
			component.ShowTextAndPlane(true, false, false);
		}
	}

	public void MakeCharGuildRank(CharNameBillboardSprite NameSprite)
	{
		Transform transform = NameSprite.transform.FindChild("CharRank");
		DrawTexture drawTexture;
		if (transform == null)
		{
			drawTexture = (DrawTexture)new GameObject("CharRank")
			{
				transform = 
				{
					parent = NameSprite.gameObject.transform,
					localPosition = new Vector3(0f, 0f, 0f),
					localScale = new Vector3(1f, 1f, 1f),
					localRotation = Quaternion.identity
				}
			}.AddComponent(typeof(DrawTexture));
		}
		else
		{
			drawTexture = (DrawTexture)transform.GetComponent(typeof(DrawTexture));
		}
		if (drawTexture != null)
		{
			float plane_Height = NameSprite.Get_Plane_Height();
			drawTexture.SetSize(plane_Height, plane_Height);
			drawTexture.SetColor(new Color(1f, 1f, 1f, 1f));
			drawTexture.transform.rotation = NameSprite.transform.rotation;
			Vector3 text_Scale = NameSprite.Get_Text_Scale();
			drawTexture.transform.localScale = text_Scale;
			drawTexture.transform.localPosition = new Vector3((NameSprite.Get_Plane_Width() + plane_Height) * 0.5f * -text_Scale.x, 0f, 0f);
			drawTexture.Hide(true);
		}
	}

	public void MakeCharRank(string key, CharNameBillboardSprite NameSprite, float fHeight)
	{
		Transform transform = NameSprite.transform.FindChild("CharRank");
		DrawTexture drawTexture;
		if (transform == null)
		{
			drawTexture = (DrawTexture)new GameObject("CharRank")
			{
				transform = 
				{
					parent = NameSprite.gameObject.transform,
					localPosition = new Vector3(0f, 0f, 0f),
					localScale = new Vector3(1f, 1f, 1f),
					localRotation = Quaternion.identity
				}
			}.AddComponent(typeof(DrawTexture));
		}
		else
		{
			drawTexture = (DrawTexture)transform.GetComponent(typeof(DrawTexture));
		}
		if (drawTexture != null)
		{
			drawTexture.SetTexture(key);
			float plane_Height = NameSprite.Get_Plane_Height();
			drawTexture.SetSize(plane_Height, plane_Height);
			drawTexture.SetColor(new Color(1f, 1f, 1f, 1f));
			drawTexture.transform.rotation = NameSprite.transform.rotation;
			Vector3 text_Scale = NameSprite.Get_Text_Scale();
			drawTexture.transform.localScale = text_Scale;
			drawTexture.transform.localPosition = new Vector3((NameSprite.Get_Plane_Width() + plane_Height) * 0.5f * -text_Scale.x, 0f, 0f);
		}
	}

	public bool IsShowHeadUp()
	{
		return this.m_bShowHeadUp;
	}

	public bool IsCheckShowHeadUp(eCharKindType chartype)
	{
		return !NrTSingleton<NkClientLogic>.Instance.IsNPCTalkState() && (this.m_bBattleChar || !NrTSingleton<NkClientLogic>.Instance.IsBattleScene()) && !this.m_bBattleChar && !this.m_bSubChar && chartype != eCharKindType.CKT_MONSTER && chartype != eCharKindType.CKT_OBJECT;
	}

	public void CheckHideChat()
	{
		if (this.m_bCheckChatShowStatus && this.m_fChatShowTime > 0f && Time.time - this.m_fChatShowTime > 5f)
		{
			this.HideChatText();
		}
	}

	public void HideChatText()
	{
		this.DisposeGameObject(this.m_pkChatSprite);
		this.m_fChatShowTime = 0f;
		this.m_bCheckChatShowStatus = false;
		if (0 < this.m_queueNpcTalk.Count)
		{
			string guildName = this.m_queueNpcTalk.Dequeue();
			this.MakeChat(eCharKindType.CKT_NPC, string.Empty, guildName, false, true);
		}
	}

	public void HideCharStatus()
	{
		this.DisposeGameObject(this.m_pkCharStatus);
	}

	public void SetShowHeadUp(bool bShow)
	{
		if (this.m_pkHeadUpRoot == null)
		{
			return;
		}
		if (this.m_pkNameSprite != null)
		{
			CharNameBillboardSprite component = this.m_pkNameSprite.GetComponent<CharNameBillboardSprite>();
			if (component != null)
			{
				component.SetShowHide(bShow);
			}
		}
		if (this.m_pkChatSprite != null)
		{
			CharChatBillboardSprite component2 = this.m_pkChatSprite.GetComponent<CharChatBillboardSprite>();
			if (component2 != null)
			{
				component2.SetShowHide(bShow);
			}
		}
		if (this.m_pkCharStatus != null)
		{
			NkUtil.SetAllChildActive(this.m_pkCharStatus, bShow);
			NkUtil.SetShowHideRenderer(this.m_pkCharStatus, bShow, true);
		}
		if (this.m_pkGuildSprite != null)
		{
			CharNameBillboardSprite component3 = this.m_pkGuildSprite.GetComponent<CharNameBillboardSprite>();
			if (component3 != null)
			{
				component3.SetShowHide(bShow);
			}
		}
		this.m_bShowHeadUp = bShow;
		if (bShow)
		{
			this.SyncBillboardRotate(true);
		}
	}

	public void SyncBillboardRotate(bool bScaleUpdate)
	{
		if (this.m_pkHeadUpRoot == null)
		{
			return;
		}
		if (Camera.main == null)
		{
			return;
		}
		this.CheckHideChat();
		if (!this.m_bShowHeadUp)
		{
			return;
		}
		if (bScaleUpdate)
		{
			this.CalcScale(false);
		}
		this.UpdateScale();
		this.SyncRotate();
	}

	public void CalcScale(bool bForceCalc)
	{
		if (Camera.main == null)
		{
			return;
		}
	}

	public void UpdateScale()
	{
	}

	public void SetTargetScale()
	{
		if (this.m_pkHeadUpRoot == null)
		{
			return;
		}
		if (Camera.main == null)
		{
			return;
		}
		this.m_kCurrentScale.x = this.m_fCurrentScale;
		this.m_kCurrentScale.y = this.m_fCurrentScale;
		this.m_kCurrentScale.z = this.m_fCurrentScale;
		this.m_pkHeadUpRoot.transform.localScale = this.m_kCurrentScale / this.m_fBaseScale;
	}

	public void SyncRotate()
	{
		if (this.m_pkHeadUpRoot == null)
		{
			return;
		}
		if (Camera.main == null)
		{
			return;
		}
		maxCamera component = Camera.main.GetComponent<maxCamera>();
		if (component == null)
		{
			TsLog.LogError("maxCamera Script is Null in maincamera", new object[0]);
			return;
		}
		if (component.target == null)
		{
			return;
		}
		this.m_pkHeadUpRoot.transform.rotation = component.transform.rotation;
	}

	public void SetSubChar(bool value)
	{
		this.m_bSubChar = value;
	}
}
