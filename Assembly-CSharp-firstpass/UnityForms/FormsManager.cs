using GameMessage;
using Ndoors.Framework.Stage;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;

namespace UnityForms
{
	public class FormsManager : NrTSingleton<FormsManager>
	{
		public enum eMAIN_UI_VISIBLE_MODE
		{
			COMMON,
			BATTLEMATCH
		}

		public enum E_FORM_POSITION
		{
			UP,
			DOWN
		}

		private const float BATTLEUIDEPTH = 800f;

		public static int SHOW_WORLD_SCENE = 1;

		public static int SHOW_BATTLE_SCENE = 2;

		public static int SHOW_TERRITORY_SCENE = 4;

		public static int SHOW_ALL_SCENE = 8;

		public static int FORM_TYPE_MAIN = 1;

		public static int FORM_TYPE_POPUP = 2;

		private Dictionary<int, Form> formList;

		private List<int> formDepth;

		private UIPanelManager panelManager;

		private G_ID[] m_eaMainUI;

		public Dictionary<int, G_ID> pOnesFomeList;

		public Dictionary<G_ID, float> m_BattleUIDepthList;

		public List<int> reserveShowForm;

		public List<int> reserveDeleteForm;

		private int m_nTopMostFormID;

		private bool m_bShowAll = true;

		public UIPanelManager PanelManager
		{
			get
			{
				if (null == this.panelManager)
				{
					this.panelManager = UIPanelManager.Create("Dialog Manager", Vector3.zero);
					this.panelManager.gameObject.layer = GUICamera.UILayer;
					this.panelManager.advancePastEnd = true;
					UnityEngine.Object.DontDestroyOnLoad(this.panelManager);
				}
				return this.panelManager;
			}
		}

		public int TopMostFormID
		{
			get
			{
				return this.m_nTopMostFormID;
			}
			set
			{
				int nTopMostFormID = this.m_nTopMostFormID;
				if (this.m_nTopMostFormID != value)
				{
					Form form = this.GetForm((G_ID)nTopMostFormID);
					if (form != null)
					{
						form.TopMost = false;
						this.PanelManager.DepthChange(form.InteractivePanel);
					}
					this.m_nTopMostFormID = value;
				}
			}
		}

		public bool isShowAllForm
		{
			get;
			private set;
		}

		private FormsManager()
		{
			NrTSingleton<UIDataManager>.Instance.Init();
			this.formList = new Dictionary<int, Form>();
			this.formDepth = new List<int>();
			this.pOnesFomeList = new Dictionary<int, G_ID>();
			this.m_BattleUIDepthList = new Dictionary<G_ID, float>();
			if (TsPlatform.IsWeb)
			{
				this.m_eaMainUI = new G_ID[]
				{
					G_ID.DLG_CHARINFO,
					G_ID.MAIN_QUEST,
					G_ID.MAIN_UI_AUTO_MOVE,
					G_ID.DLG_SYSTEMMESSAGE,
					G_ID.CHAT_MAIN_DLG,
					G_ID.DLG_LOADINGPAGE
				};
			}
			else if (TsPlatform.IsMobile)
			{
				this.m_eaMainUI = new G_ID[]
				{
					G_ID.DLG_CHARINFO,
					G_ID.MAIN_QUEST,
					G_ID.MAIN_UI_AUTO_MOVE,
					G_ID.DLG_SYSTEMMESSAGE,
					G_ID.CHAT_MAIN_DLG,
					G_ID.DLG_LOADINGPAGE,
					G_ID.JOYSTICK_DLG
				};
			}
			this.reserveShowForm = new List<int>();
			this.reserveDeleteForm = new List<int>();
		}

		public void DeleteAll()
		{
			foreach (Form current in this.formList.Values)
			{
				if (current.InteractivePanel != null)
				{
					this.panelManager.RemoveChild(current.InteractivePanel.gameObject);
				}
				current.OnClose();
				if (current.InteractivePanel != null)
				{
					current.ClearDictionary();
					UnityEngine.Object.Destroy(current.InteractivePanel.gameObject);
				}
			}
			this.formList.Clear();
			this.Clear();
		}

		public void AddReserveDeleteForm(int windowID)
		{
			this.reserveDeleteForm.Add(windowID);
		}

		public void AddReserveShowForm(int windowID)
		{
			this.reserveShowForm.Add(windowID);
		}

		public bool IsMouseOverForm()
		{
			return NrTSingleton<UIManager>.Instance.ClickUI;
		}

		public int MouseOverFormID()
		{
			if (this.panelManager.MouseOverPanel)
			{
				return this.panelManager.MouseOverPanel.index;
			}
			return 0;
		}

		public int FocusFormID()
		{
			if (this.panelManager.FocusPanel)
			{
				return this.panelManager.FocusPanel.index;
			}
			return 0;
		}

		public bool Initialize()
		{
			return true;
		}

		public void Update()
		{
			for (int i = 0; i < this.formDepth.Count; i++)
			{
				int key = this.formDepth[i];
				if (this.formList.ContainsKey(key))
				{
					Form form = this.formList[key];
					if (form != null && (form.Visible || form.AlwaysUpdate))
					{
						form.Update();
					}
				}
			}
		}

		public string GetDlgStatus()
		{
			string text = string.Empty;
			for (int i = 0; i < this.formDepth.Count; i++)
			{
				int key = this.formDepth[i];
				if (this.formList.ContainsKey(key))
				{
					Form form = this.formList[key];
					if (form != null)
					{
						text += string.Format("{0} \t SHOW({1}) {2} {3} \r\n", new object[]
						{
							form.Orignal_ID.ToString(),
							form.visible.ToString(),
							form.GetLocationX(),
							form.GetLocationY()
						});
					}
				}
			}
			return text;
		}

		public void LateUpdate()
		{
			if (0 < this.reserveDeleteForm.Count)
			{
				for (int i = 0; i < this.reserveDeleteForm.Count; i++)
				{
					this.CloseForm((G_ID)this.reserveDeleteForm[i]);
				}
				this.reserveDeleteForm.Clear();
			}
		}

		public void FixedUpdate()
		{
		}

		public float GetTopMostZ()
		{
			return this.PanelManager.TOPMOSTZ;
		}

		public float GetZ()
		{
			return this.PanelManager.HighestZValue - 2f;
		}

		public void ShowHide(G_ID windowID)
		{
			Form form = this.GetForm(windowID);
			if (form == null)
			{
				form = this.LoadForm(windowID);
				if (form != null)
				{
					form.Show();
				}
			}
			else if (form.Visible)
			{
				form.Visible = false;
			}
			else
			{
				form.Visible = true;
			}
		}

		public void ShowForm(int windowID)
		{
			if (this.formList.ContainsKey(windowID))
			{
				if (!this.formList[windowID].Visible)
				{
					this.formList[windowID].Show();
				}
				return;
			}
			Form form = this.LoadForm((G_ID)windowID);
			if (form == null)
			{
				return;
			}
			form.Show();
		}

		public void ShowForm(G_ID windowID)
		{
			this.ShowForm((int)windowID);
		}

		public void ToggleForm(G_ID windowID, bool bCloseForm)
		{
			if (this.formList.ContainsKey((int)windowID))
			{
				if (this.formList[(int)windowID].visible)
				{
					if (bCloseForm)
					{
						this.CloseForm(windowID);
					}
					else
					{
						this.formList[(int)windowID].Hide();
					}
				}
				else
				{
					this.formList[(int)windowID].Show();
				}
			}
			else
			{
				this.ShowForm(windowID);
			}
		}

		public Form LoadGroupForm(G_ID windowID)
		{
			int num = Guid.NewGuid().GetHashCode();
			if (10501 >= num && 0 <= num)
			{
				num += 10501;
			}
			Form form = MsgHandler.HandleReturn<Form>("CreateForm", new object[]
			{
				windowID
			});
			if (form == null)
			{
				return null;
			}
			this.formList.Add(num, form);
			form.InitializeForm();
			if (form.InteractivePanel == null)
			{
				return null;
			}
			form.WindowID = num;
			form.Orignal_ID = windowID;
			form.OnLoad();
			if (MsgHandler.Handle("IsNPCTalkState", new object[0]))
			{
				this.AddReserveShowForm(num);
				form.Visible = false;
			}
			this.formDepth.Add(num);
			return form;
		}

		public Form LoadForm(G_ID windowID)
		{
			if (this.formList.ContainsKey((int)windowID) && windowID != G_ID.TOOLTIP_DLG)
			{
				if (this.formList[(int)windowID].ShowHide)
				{
					this.formList[(int)windowID].Show();
				}
				return this.formList[(int)windowID];
			}
			if (this.formList.ContainsKey((int)windowID) && windowID == G_ID.TOOLTIP_DLG)
			{
				this.CloseForm(windowID);
			}
			Form form = MsgHandler.HandleReturn<Form>("CreateForm", new object[]
			{
				windowID
			});
			if (form == null)
			{
				return null;
			}
			this.formList.Add((int)windowID, form);
			form.WindowID = (int)windowID;
			form.Orignal_ID = windowID;
			form.InitializeForm();
			if (form.InteractivePanel == null)
			{
				return null;
			}
			form.OnLoad();
			this.formDepth.Add((int)windowID);
			return form;
		}

		public bool IsShow(G_ID windowID)
		{
			return this.formList.ContainsKey((int)windowID) && this.formList[(int)windowID].Visible;
		}

		public bool IsForm(Form form)
		{
			if (form == null)
			{
				return false;
			}
			int windowID = form.WindowID;
			return this.formList.ContainsKey(windowID);
		}

		public bool IsForm(G_ID _id)
		{
			return this.formList.ContainsKey((int)_id);
		}

		public Form GetForm(G_ID windowID)
		{
			if (this.formList.ContainsKey((int)windowID))
			{
				return this.formList[(int)windowID];
			}
			return null;
		}

		public void Show(G_ID windowID)
		{
			if (this.formList.ContainsKey((int)windowID))
			{
				this.formList[(int)windowID].Show();
			}
		}

		public void Hide(G_ID windowID)
		{
			if (this.formList.ContainsKey((int)windowID))
			{
				Form form = this.formList[(int)windowID];
				form.Hide();
			}
		}

		public void SetShowExcept(G_ID windowID, bool bShow)
		{
			foreach (KeyValuePair<int, Form> current in this.formList)
			{
				if (current.Value != null && current.Value.WindowID != (int)windowID)
				{
					current.Value.Visible = bShow;
				}
			}
		}

		public void ShowExcept(G_ID windowID)
		{
			foreach (KeyValuePair<int, Form> current in this.formList)
			{
				if (current.Value != null)
				{
					if (current.Value.WindowID == (int)windowID)
					{
						current.Value.Visible = false;
					}
					else
					{
						current.Value.Visible = true;
					}
				}
			}
		}

		public void HideExcept(G_ID windowID)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < this.formDepth.Count; i++)
			{
				int num = this.formDepth[i];
				if (!this.formList.ContainsKey(num))
				{
					G_ID g_ID = (G_ID)num;
					switch (g_ID)
					{
					case G_ID.DLG_SYSTEMMESSAGE:
						goto IL_13D;
					case G_ID.DLG_MONSTER_DETAILINFO:
						IL_49:
						if (g_ID == G_ID.QUEST_REWARD)
						{
							goto IL_13D;
						}
						if (g_ID == G_ID.QUEST_GROUP_REWARD)
						{
							goto IL_13D;
						}
						if (g_ID != G_ID.WHISPER_DLG)
						{
							goto IL_13D;
						}
						goto IL_13D;
					case G_ID.DLG_LOADINGPAGE:
						goto IL_13D;
					}
					goto IL_49;
				}
				Form form = this.formList[num];
				if (form != null)
				{
					if (form.WindowID == (int)windowID)
					{
						form.Visible = true;
					}
					else
					{
						bool flag = false;
						for (int j = 0; j < this.m_eaMainUI.Length; j++)
						{
							if (num == (int)this.m_eaMainUI[j])
							{
								flag = true;
							}
						}
						if (flag)
						{
							if (form.Visible)
							{
								form.Visible = false;
							}
						}
						else if (!form.ChangeSceneDestory)
						{
							if (form.Visible)
							{
								form.Visible = false;
							}
						}
						else
						{
							list.Add(num);
						}
					}
				}
				IL_13D:;
			}
			foreach (int current in list)
			{
				this.CloseForm((G_ID)current);
			}
		}

		public void CloseAllExcept(G_ID windowID)
		{
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, Form> current in this.formList)
			{
				if (current.Key != (int)windowID)
				{
					if (current.Value == null || current.Value.ChangeSceneDestory)
					{
						list.Add(current.Key);
					}
				}
			}
			foreach (int current2 in list)
			{
				this.CloseForm(current2);
			}
		}

		public void CloseAll()
		{
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, Form> current in this.formList)
			{
				list.Add(current.Key);
			}
			foreach (int current2 in list)
			{
				this.CloseForm(current2);
			}
		}

		public void ShowAll()
		{
			for (int i = 0; i < this.formDepth.Count; i++)
			{
				int num = this.formDepth[i];
				if (this.formList.ContainsKey(num))
				{
					Form form = this.GetForm((G_ID)num);
					if (form != null)
					{
						if (form.WindowID != 252)
						{
							form.Visible = true;
						}
					}
				}
			}
			this.isShowAllForm = true;
		}

		public void HideAll()
		{
			for (int i = 0; i < this.formDepth.Count; i++)
			{
				int num = this.formDepth[i];
				if (this.formList.ContainsKey(num))
				{
					Form form = this.GetForm((G_ID)num);
					if (form != null)
					{
						if (form.WindowID != 252)
						{
							form.Visible = false;
						}
					}
				}
			}
			this.isShowAllForm = false;
		}

		public void ShowHideAll()
		{
			this.m_bShowAll = !this.m_bShowAll;
			if (this.m_bShowAll)
			{
				this.ShowAll();
			}
			else
			{
				this.HideAll();
			}
		}

		public void CloseForm(int key)
		{
			if (!this.formList.ContainsKey(key))
			{
				return;
			}
			int windowID = this.formList[key].WindowID;
			if (this.pOnesFomeList.ContainsValue((G_ID)key))
			{
				int key2 = 0;
				foreach (KeyValuePair<int, G_ID> current in this.pOnesFomeList)
				{
					if (current.Value == (G_ID)key)
					{
						key2 = current.Key;
						break;
					}
				}
				this.pOnesFomeList.Remove(key2);
			}
			if (this.formList[key] != null)
			{
				G_ID g_ID = G_ID.NONE;
				G_ID g_ID2 = G_ID.NONE;
				G_ID g_ID3 = G_ID.NONE;
				G_ID g_ID4 = G_ID.NONE;
				if (this.formList[key].InteractivePanel != null)
				{
					g_ID = this.formList[key].InteractivePanel.twinFormID;
					g_ID2 = this.formList[key].InteractivePanel.parentFormID;
					g_ID3 = this.formList[key].InteractivePanel.childFormID_0;
					g_ID4 = this.formList[key].InteractivePanel.childFormID_1;
				}
				if (this.formList[key].ShowHide)
				{
					this.Hide((G_ID)windowID);
					if (g_ID != G_ID.NONE)
					{
						this.Hide(g_ID);
					}
					if (g_ID3 != G_ID.NONE)
					{
						this.Hide(g_ID3);
					}
					if (g_ID4 != G_ID.NONE)
					{
						this.Hide(g_ID4);
					}
				}
				else
				{
					if (this.IsForm(g_ID2))
					{
						this.GetForm(g_ID2).DeleteChildForm((G_ID)key);
					}
					if (this.formList[key].InteractivePanel != null)
					{
						this.panelManager.RemoveChild(this.formList[key].InteractivePanel.gameObject);
					}
					this.formList[key].OnClose();
					if (this.formList[key].InteractivePanel != null)
					{
						this.formList[key].ClearDictionary();
						UnityEngine.Object.Destroy(this.formList[key].InteractivePanel.gameObject);
					}
					this.formDepth.Remove(windowID);
					this.formList.Remove(windowID);
					if (g_ID != G_ID.NONE)
					{
						this.CloseForm(g_ID);
					}
					if (g_ID3 != G_ID.NONE)
					{
						this.CloseForm(g_ID3);
					}
					if (g_ID4 != G_ID.NONE)
					{
						this.CloseForm(g_ID4);
					}
				}
			}
		}

		public bool CloseFormESC()
		{
			bool result = false;
			if (Scene.CurScene == Scene.Type.WORLD || Scene.CurScene == Scene.Type.BATTLE || Scene.CurScene == Scene.Type.SOLDIER_BATCH)
			{
				for (int i = this.formDepth.Count - 1; i >= 0; i--)
				{
					int key = this.formDepth[i];
					if (this.formList[key].Visible)
					{
						if (0 >= (FormsManager.FORM_TYPE_MAIN & this.formList[key].ShowSceneType))
						{
							if (this.panelManager.IsTopMost(this.formList[key].InteractivePanel))
							{
								this.formList[key].CloseForm(null);
								result = true;
								break;
							}
						}
					}
				}
				List<UIPanelBase> listPanel = this.PanelManager.GetListPanel();
				for (int j = listPanel.Count - 1; j >= 0; j--)
				{
					foreach (int current in this.formDepth)
					{
						if (listPanel[j] == this.formList[current].InteractivePanel)
						{
							if (!this.formList[current].Visible)
							{
								break;
							}
							if (0 >= (FormsManager.FORM_TYPE_MAIN & this.formList[current].ShowSceneType))
							{
								this.formList[current].Close();
								return true;
							}
						}
					}
				}
			}
			return result;
		}

		public void CloseForm(G_ID windowID)
		{
			this.CloseForm((int)windowID);
		}

		public void ClearShowHideForms()
		{
			Form form = this.GetForm(G_ID.SOLMILITARYGROUP_DLG);
			if (form != null)
			{
				form.ShowHide = false;
				this.CloseForm(G_ID.SOLMILITARYGROUP_DLG);
			}
		}

		public void SetPos(G_ID Id, int X, int Y)
		{
			if (this.formList.ContainsKey((int)Id))
			{
				this.formList[(int)Id].SetLocation((float)X, (float)Y);
			}
		}

		public void Clear()
		{
			this.formDepth.Clear();
			this.formList.Clear();
			this.pOnesFomeList.Clear();
		}

		public void HideMainUI()
		{
			this.Hide(G_ID.BOOKMARK_DLG);
			this.Hide(G_ID.MENUICON_DLG);
			this.Hide(G_ID.MYCHARINFO_DLG);
			this.Hide(G_ID.MAIN_UI_ICON);
			this.Hide(G_ID.MAIN_QUEST);
			this.Hide(G_ID.CHAT_MAIN_DLG);
			this.Hide(G_ID.JOYSTICK_DLG);
			this.Hide(G_ID.GOOGLEPLAY_DLG);
		}

		public void Main_UI_Show(FormsManager.eMAIN_UI_VISIBLE_MODE mode)
		{
			this.ShowForm(G_ID.BOOKMARK_DLG);
			this.ShowForm(G_ID.MENUICON_DLG);
			this.ShowForm(G_ID.MAIN_UI_ICON);
			this.ShowForm(G_ID.MAIN_QUEST);
			this.ShowForm(G_ID.CHAT_MAIN_DLG);
			if (Scene.CurScene == Scene.Type.WORLD)
			{
				this.ShowForm(G_ID.JOYSTICK_DLG);
			}
			MsgHandler.Handle("Main_UI_Show", new object[0]);
			foreach (int current in this.reserveShowForm)
			{
				if (this.formList.ContainsKey(current))
				{
					this.formList[current].Visible = true;
				}
			}
			this.reserveShowForm.Clear();
		}

		public int FindWindowID(string filename)
		{
			foreach (KeyValuePair<int, Form> current in this.formList)
			{
				if (current.Value.FileName.Equals(filename, StringComparison.CurrentCultureIgnoreCase))
				{
					return current.Key;
				}
			}
			return 0;
		}

		public Vector2 Get_Show_Position(int a_nParentKey, int a_nChildKey)
		{
			return this.Get_Show_Position(a_nParentKey, a_nChildKey, FormsManager.E_FORM_POSITION.UP);
		}

		public Vector2 Get_Show_Position(int a_nParentKey, int a_nChildKey, FormsManager.E_FORM_POSITION a_ePosition)
		{
			Vector2 result = default(Vector2);
			if (this.formList.ContainsKey(a_nParentKey) && this.formList[a_nParentKey].Visible)
			{
				Form form;
				if (this.formList.ContainsKey(a_nChildKey))
				{
					form = this.formList[a_nChildKey];
				}
				else
				{
					form = this.LoadForm((G_ID)a_nChildKey);
				}
				Vector2 vector = this.formList[a_nParentKey].GetLocation();
				Vector2 size = this.formList[a_nParentKey].GetSize();
				Vector2 size2 = form.GetSize();
				if (a_ePosition != FormsManager.E_FORM_POSITION.UP)
				{
					if (a_ePosition == FormsManager.E_FORM_POSITION.DOWN)
					{
						result.y = -vector.y + size.y - size2.y;
					}
				}
				else
				{
					result.y = -vector.y;
				}
				float num = vector.x + size.x;
				if (vector.x + size.x + size2.x > (float)Screen.width)
				{
					float value = num + size2.x - (float)Screen.width;
					float num2 = vector.x - size2.x;
					if (num2 < 0f)
					{
						float num3 = Math.Abs(value);
						float num4 = Math.Abs(num2);
						if (num3 > num4)
						{
						}
					}
					result.x = vector.x - size2.x;
				}
				else
				{
					result.x = vector.x + size.x;
				}
				return result;
			}
			return result;
		}

		public void ShowOrClose(G_ID eWindowID)
		{
			if (this.IsShow(eWindowID))
			{
				this.CloseForm(eWindowID);
			}
			else
			{
				this.ShowForm(eWindowID);
			}
		}

		public void ShowMessageBox(string strMessage)
		{
			this.ShowMessageBox(string.Empty, strMessage, eMsgType.MB_OK, null, null);
		}

		public void ShowMessageBox(string strTitle, string strMessage)
		{
			this.ShowMessageBox(strTitle, strMessage, eMsgType.MB_OK, null, null);
		}

		public void ShowMessageBox(string strTitle, string strMessage, eMsgType eMessageType, YesDelegate a_deYes = null, object obj = null)
		{
			MsgHandler.Handle("SetMsgBox", new object[]
			{
				a_deYes,
				obj,
				strTitle,
				strMessage,
				eMessageType
			});
		}

		public void ChangedResolution()
		{
			List<Form> list = new List<Form>(this.formList.Values);
			foreach (Form current in list)
			{
				if (current.Visible)
				{
					current.ChangedResolution();
				}
			}
		}

		public int GetOnesFormCount()
		{
			return this.pOnesFomeList.Count;
		}

		public float GetBattleUIDepth(G_ID GID)
		{
			float result = 800f;
			if (this.m_BattleUIDepthList.ContainsKey(GID))
			{
				return this.m_BattleUIDepthList[GID];
			}
			return result;
		}

		public void AddBattleUIDepth()
		{
			float num = 800f;
			this.m_BattleUIDepthList.Add(G_ID.BATTLE_COUNT_DLG, num - 1f);
		}

		public bool RequestUIBundleDownLoad(string path, PostProcPerItem callbackDelegate, object callbackParam)
		{
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(path + Option.extAsset, MsgHandler.HandleReturn<string>("UIBundleStackName", new object[0]));
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(callbackDelegate, callbackParam);
			if (wWWItem != null)
			{
				TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
				return true;
			}
			return false;
		}

		public void AttachEffectKey(string effectKey, AutoSpriteControlBase obj, Vector2 size)
		{
			string path = MsgHandler.HandleReturn<string>("EffectFileName", new object[]
			{
				effectKey
			});
			this.RequestUIBundleDownLoad(path, new PostProcPerItem(this._funcUIEffectDownloaded), new KeyValuePair<AutoSpriteControlBase, Vector2>(obj, size));
		}

		public void RequestAttachUIEffect(string path, AutoSpriteControlBase obj, Vector2 size)
		{
			if (null == obj)
			{
				return;
			}
			this.RequestUIBundleDownLoad(path, new PostProcPerItem(this._funcUIEffectDownloaded), new KeyValuePair<AutoSpriteControlBase, Vector2>(obj, size));
		}

		private void _funcUIEffectDownloaded(IDownloadedItem wItem, object obj)
		{
			if (null == wItem.mainAsset)
			{
				TsLog.LogWarning("wItem.mainAsset is null -> Path = {0}", new object[]
				{
					wItem.assetPath
				});
				return;
			}
			GameObject gameObject = wItem.mainAsset as GameObject;
			if (null == gameObject)
			{
				return;
			}
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				MsgHandler.Handle("SetEditorShaderConvert", new object[]
				{
					gameObject
				});
			}
			GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity);
			gameObject2.name = NrTSingleton<UIDataManager>.Instance.AttachEffectKeyName;
			if (null == gameObject2)
			{
				UnityEngine.Object.Destroy(gameObject.gameObject);
				return;
			}
			MsgHandler.Handle("SetAllChildLayer", new object[]
			{
				gameObject2,
				GUICamera.UILayer
			});
			KeyValuePair<AutoSpriteControlBase, Vector2> keyValuePair = (KeyValuePair<AutoSpriteControlBase, Vector2>)obj;
			if (null == keyValuePair.Key)
			{
				UnityEngine.Object.Destroy(gameObject2.gameObject);
				return;
			}
			AutoSpriteControlBase key = keyValuePair.Key;
			if (null == key)
			{
				UnityEngine.Object.Destroy(gameObject2.gameObject);
				return;
			}
			if (null == key.gameObject)
			{
				UnityEngine.Object.Destroy(gameObject2.gameObject);
				return;
			}
			Vector2 value = keyValuePair.Value;
			gameObject2.transform.parent = key.gameObject.transform;
			gameObject2.transform.localPosition = new Vector3(value.x / 2f, -value.y / 2f, key.gameObject.transform.localPosition.z - 0.1f);
			key.ExcuteGameObjectDelegate(key, gameObject2);
		}
	}
}
