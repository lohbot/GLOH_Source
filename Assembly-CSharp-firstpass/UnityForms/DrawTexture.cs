using GameMessage;
using System;
using TsBundle;
using UnityEngine;

namespace UnityForms
{
	public class DrawTexture : AutoSpriteControlBase
	{
		public enum CONTROL_STATE
		{
			NORMAL,
			OVER,
			ACTIVE,
			DISABLED
		}

		[HideInInspector]
		public TextureAnim[] states = new TextureAnim[]
		{
			new TextureAnim("Normal"),
			new TextureAnim("Over"),
			new TextureAnim("Active"),
			new TextureAnim("Disabled")
		};

		[HideInInspector]
		public EZTransitionList[] transitions = new EZTransitionList[]
		{
			new EZTransitionList(new EZTransition[]
			{
				new EZTransition("From Over"),
				new EZTransition("From Active"),
				new EZTransition("From Disabled")
			}),
			new EZTransitionList(new EZTransition[]
			{
				new EZTransition("From Normal"),
				new EZTransition("From Active")
			}),
			new EZTransitionList(new EZTransition[]
			{
				new EZTransition("From Normal"),
				new EZTransition("From Over")
			}),
			new EZTransitionList(new EZTransition[]
			{
				new EZTransition("From Normal"),
				new EZTransition("From Over"),
				new EZTransition("From Active")
			})
		};

		protected DrawTexture.CONTROL_STATE m_ctrlState;

		private bool reserve;

		private string imageKey = string.Empty;

		private bool bHaveEffect;

		private eCharImageType m_etype;

		public override TextureAnim[] States
		{
			get
			{
				return this.states;
			}
			set
			{
				this.states = value;
			}
		}

		public override EZTransitionList[] Transitions
		{
			get
			{
				return this.transitions;
			}
			set
			{
				this.transitions = value;
			}
		}

		public DrawTexture.CONTROL_STATE controlState
		{
			get
			{
				return this.m_ctrlState;
			}
		}

		public override bool controlIsEnabled
		{
			get
			{
				return this.m_controlIsEnabled;
			}
			set
			{
				bool controlIsEnabled = this.m_controlIsEnabled;
				this.m_controlIsEnabled = value;
				if (!value)
				{
					this.SetControlState(DrawTexture.CONTROL_STATE.DISABLED);
				}
				else if (!controlIsEnabled)
				{
					this.SetControlState(DrawTexture.CONTROL_STATE.NORMAL);
				}
			}
		}

		public override Rect3D GetClippingRect()
		{
			return base.GetClippingRect();
		}

		public override void SetClippingRect(Rect3D value)
		{
			if (this.ignoreClipping)
			{
				return;
			}
			base.SetClippingRect(value);
			Transform transform = base.transform.FindChild(NrTSingleton<UIDataManager>.Instance.AttachEffectKeyName);
			if (null != transform)
			{
				Vector3[] v3TotalVertices = this.m_v3TotalVertices;
				Vector3 vector = v3TotalVertices[1];
				Vector3 vector2 = v3TotalVertices[3];
				float num = vector2.y - vector.y;
				if (this.height > num)
				{
					transform.gameObject.SetActive(false);
				}
				else
				{
					transform.gameObject.SetActive(true);
				}
			}
		}

		public static DrawTexture Create(string name, Vector3 pos)
		{
			return (DrawTexture)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(DrawTexture));
		}

		public void UseCustomCollider(bool flag)
		{
			this.customCollider = flag;
		}

		public void UsedCollider(bool use)
		{
			if (use)
			{
				base.SetUseBoxCollider(true);
				this.AddCollider();
			}
			else
			{
				base.SetUseBoxCollider(false);
				BoxCollider boxCollider = (BoxCollider)base.gameObject.GetComponent(typeof(BoxCollider));
				if (null != boxCollider)
				{
					UnityEngine.Object.Destroy(boxCollider);
				}
			}
		}

		protected override void AddCollider()
		{
			base.AddCollider();
		}

		public override EZTransitionList GetTransitions(int index)
		{
			if (index >= this.transitions.Length)
			{
				return null;
			}
			return this.transitions[index];
		}

		private void SetControlState(DrawTexture.CONTROL_STATE s)
		{
			this.m_ctrlState = s;
		}

		public override void OnInput(ref POINTER_INFO ptr)
		{
			if (this.deleted)
			{
				return;
			}
			if (!this.m_controlIsEnabled || base.IsHidden())
			{
				return;
			}
			switch (ptr.evt)
			{
			case POINTER_INFO.INPUT_EVENT.PRESS:
				if (this.mouseDownDelegate != null)
				{
					this.mouseDownDelegate(this);
				}
				break;
			case POINTER_INFO.INPUT_EVENT.DOUBLE_PRESS:
				if (this.doubleClickDelegate != null)
				{
					this.doubleClickDelegate(this);
				}
				break;
			case POINTER_INFO.INPUT_EVENT.TAP:
				if (this.changeDelegate != null)
				{
					this.changeDelegate(this);
				}
				break;
			case POINTER_INFO.INPUT_EVENT.MOVE:
				if (this.m_ctrlState != DrawTexture.CONTROL_STATE.OVER)
				{
					if (this.mouseOverDelegate != null)
					{
						this.mouseOverDelegate(this);
					}
					this.SetControlState(DrawTexture.CONTROL_STATE.OVER);
				}
				break;
			case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
				if (this.mouseOutDelegate != null)
				{
					this.mouseOutDelegate(this);
				}
				this.SetControlState(DrawTexture.CONTROL_STATE.NORMAL);
				break;
			}
			base.OnInput(ref ptr);
		}

		public void AddBoxCollider()
		{
			if (null == base.gameObject.GetComponent(typeof(BoxCollider)))
			{
				BoxCollider boxCollider = (BoxCollider)base.gameObject.AddComponent(typeof(BoxCollider));
				boxCollider.size = new Vector3(this.width, this.height, 0f);
				boxCollider.center = new Vector3(this.width / 2f, -this.height / 2f, 0f);
			}
			else
			{
				TsLog.Log("BoxCollider is added already on object.", new object[0]);
			}
		}

		public void InitSlotEffect()
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				if (!(null == child))
				{
					UnityEngine.Object.DestroyImmediate(child.gameObject);
				}
			}
		}

		public void Update()
		{
			if (string.Empty != this.imageKey && this.reserve)
			{
				Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.imageKey);
				if (null == texture)
				{
					this.reserve = false;
					NrTSingleton<UIImageBundleManager>.Instance.RequestCharImage(this.imageKey, this.m_etype, new PostProcPerItem(this.SetImage));
				}
				else
				{
					if (null == this.texMat)
					{
						return;
					}
					if (null == this.texMat.mainTexture)
					{
						base.SetTexture(texture);
					}
					else if (this.imageKey != this.texMat.mainTexture.name)
					{
						base.SetTexture(texture);
					}
				}
			}
		}

		public void SetTextureEffect(eCharImageType type, int kind, int solgrade)
		{
			this.SetTexture(type, kind, solgrade);
			string s = MsgHandler.HandleReturn<string>("GetLegendTypeToString", new object[]
			{
				kind,
				solgrade
			});
			short num = short.Parse(s);
			if (0 < num)
			{
				Transform y = base.transform.FindChild(NrTSingleton<UIDataManager>.Instance.AttachEffectKeyName);
				if (null == y && !this.bHaveEffect)
				{
					if (type == eCharImageType.SMALL)
					{
						NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect("Effect/Instant/fx_legendhero_ui" + NrTSingleton<UIDataManager>.Instance.AddFilePath, this, base.GetSize());
					}
					else
					{
						NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect("Effect/Instant/fx_herolegendblg_ui" + NrTSingleton<UIDataManager>.Instance.AddFilePath, this, base.GetSize());
					}
					this.AddGameObjectDelegate(new EZGameObjectDelegate(NrTSingleton<UIDataManager>.Instance.ResizeEffect));
					this.bHaveEffect = true;
				}
			}
			else
			{
				Transform transform = base.transform.FindChild(NrTSingleton<UIDataManager>.Instance.AttachEffectKeyName);
				if (null != transform)
				{
					UnityEngine.Object.Destroy(transform.gameObject);
					this.bHaveEffect = false;
				}
			}
		}

		public void SetTexture(eCharImageType type, int kind, int solgrade)
		{
			this.imageKey = string.Empty;
			this.reserve = false;
			string text = MsgHandler.HandleReturn<string>("PortraitFileName", new object[]
			{
				kind,
				solgrade
			});
			if (string.Empty == text)
			{
				return;
			}
			this.m_etype = type;
			if (type == eCharImageType.SMALL)
			{
				this.imageKey = text + "_64";
			}
			else if (type == eCharImageType.MIDDLE)
			{
				this.imageKey = text + "_256";
			}
			else if (type == eCharImageType.LARGE)
			{
				if (UIDataManager.IsUse256Texture())
				{
					this.imageKey = text + "_256";
				}
				else
				{
					this.imageKey = text + "_512";
				}
			}
			Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.imageKey);
			if (null != texture)
			{
				base.SetTexture(texture);
				this.reserve = true;
			}
			else
			{
				NrTSingleton<UIImageBundleManager>.Instance.RequestCharImage(this.imageKey, type, new PostProcPerItem(this.SetImage));
			}
		}

		public void SetTextureEvent(eCharImageType type, int kind, int solgrade)
		{
			this.SetTexture(type, kind, solgrade);
			UIListItemContainer uIListItemContainer = base.gameObject.AddComponent<UIListItemContainer>();
			if (uIListItemContainer == null)
			{
				return;
			}
			GameObject gameObject = new GameObject("EventMark");
			if (gameObject == null)
			{
				return;
			}
			DrawTexture drawTexture = gameObject.AddComponent<DrawTexture>();
			if (drawTexture != null)
			{
				drawTexture.SetUseBoxCollider(false);
				drawTexture.gameObject.layer = GUICamera.UILayer;
				drawTexture.SetTexture("Win_I_Notice05");
				drawTexture.SetSize(this.width * 0.45f, this.height * 0.45f);
				drawTexture.SetAnchor(SpriteRoot.ANCHOR_METHOD.UPPER_LEFT);
				uIListItemContainer.MakeChild(drawTexture.gameObject);
				drawTexture.gameObject.transform.localPosition = new Vector3(this.width * 0.8f, this.height * 0.1f);
				drawTexture.Visible = true;
			}
		}

		public void SetTexture2D(Texture2D _Texture)
		{
			if (_Texture != null)
			{
				base.SetTexture(_Texture);
			}
		}

		public void SetTextureFromUISoldierBundle(eCharImageType type, string bunldname)
		{
			string text = "UI/Soldier";
			string text2 = string.Empty;
			string text3 = string.Empty;
			if (type == eCharImageType.SMALL)
			{
				text2 = "64";
			}
			else if (type == eCharImageType.MIDDLE)
			{
				text2 = "256";
			}
			else if (type == eCharImageType.LARGE)
			{
				if (UIDataManager.IsUse256Texture())
				{
					text2 = "256";
				}
				else
				{
					text2 = "512";
				}
			}
			text3 = string.Format("{0}/{1}/{2}_{3}", new object[]
			{
				text,
				text2,
				bunldname,
				text2
			});
			if (text3 != string.Empty)
			{
				this.SetTextureFromBundle(text3);
			}
		}

		public void SetTextureFromBundle(string bundlepath)
		{
			this.imageKey = string.Empty;
			Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(bundlepath);
			if (null != texture)
			{
				base.SetTexture(texture);
			}
			else
			{
				NrTSingleton<UIImageBundleManager>.Instance.RequestBundleImage(bundlepath, new PostProcPerItem(this.SetImage));
			}
		}

		public void SetFadeTextureFromBundle(string bundlepath)
		{
			this.imageKey = string.Empty;
			Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(bundlepath);
			if (null != texture)
			{
				base.SetTexture(texture);
				FadeSprite.Do(this, EZAnimation.ANIM_MODE.FromTo, new Color(0f, 0f, 0f, 0f), new Color(1f, 1f, 1f, 1f), new EZAnimation.Interpolator(EZAnimation.linear), 0.5f, 0f, null, null);
			}
			else
			{
				NrTSingleton<UIImageBundleManager>.Instance.RequestBundleImage(bundlepath, new PostProcPerItem(this.SetFadeImage));
			}
		}

		private void SetImage(WWWItem _item, object _param)
		{
			if (null == this)
			{
				return;
			}
			if (_item.isCanceled)
			{
				this.reserve = true;
				return;
			}
			if (_item.GetSafeBundle() != null)
			{
				Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
				if (null != texture2D)
				{
					base.SetTexture(texture2D);
					string text = string.Empty;
					if (_param is string)
					{
						text = (string)_param;
						NrTSingleton<UIImageBundleManager>.Instance.AddTexture(text, texture2D);
						return;
					}
				}
			}
			this.reserve = true;
		}

		private void SetFadeImage(WWWItem _item, object _param)
		{
			this.SetImage(_item, _param);
			FadeSprite.Do(this, EZAnimation.ANIM_MODE.FromTo, new Color(0f, 0f, 0f, 0f), new Color(1f, 1f, 1f, 1f), new EZAnimation.Interpolator(EZAnimation.linear), 0.5f, 0f, null, null);
		}
	}
}
