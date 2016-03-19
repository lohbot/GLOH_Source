using GAME;
using GameMessage;
using System;
using UnityEngine;

namespace UnityForms
{
	public class ChatLabel : UIScrollList
	{
		private bool m_bScroll;

		private SpriteText.Font_Effect fontEffect = SpriteText.Font_Effect.Black_Shadow_Small;

		private float fontSize = 14f;

		private int maxChatLabelLine
		{
			get
			{
				if (TsPlatform.IsMobile)
				{
					return 40;
				}
				return 100;
			}
		}

		public bool bScrollView
		{
			get
			{
				return this.m_bScroll;
			}
			set
			{
				this.m_bScroll = value;
				this.slider.Visible = this.m_bScroll;
			}
		}

		public SpriteText.Font_Effect FontEffect
		{
			get
			{
				return this.fontEffect;
			}
			set
			{
				this.fontEffect = value;
			}
		}

		public float FontSize
		{
			get
			{
				return this.fontSize;
			}
			set
			{
				this.fontSize = value;
				this.lineHeight = value;
			}
		}

		public new static ChatLabel Create(string name, Vector3 pos)
		{
			return (ChatLabel)new GameObject(name)
			{
				transform = 
				{
					position = pos
				}
			}.AddComponent(typeof(ChatLabel));
		}

		public void PushText(string Name, string text, string color, ITEM linkItem)
		{
			string text2 = MsgHandler.HandleReturn<string>("GetTextColor", new object[]
			{
				color
			});
			string @string = NrTSingleton<UIDataManager>.Instance.GetString(text2, Name, " ", text2, text);
			this.PushText(@string, linkItem);
		}

		public void PushText(string text, ITEM linkItem, string color)
		{
			string @string = NrTSingleton<UIDataManager>.Instance.GetString(MsgHandler.HandleReturn<string>("GetTextColor", new object[]
			{
				color
			}), text);
			this.PushText(@string, linkItem);
		}

		public void SystemChatPushText(string Name, string text, string color, ITEM linkItem)
		{
			string msg = string.Empty;
			if (color == string.Empty)
			{
				msg = NrTSingleton<UIDataManager>.Instance.GetString(Name, " ", text);
			}
			else
			{
				msg = NrTSingleton<UIDataManager>.Instance.GetString(Name, " ", MsgHandler.HandleReturn<string>("GetTextColor", new object[]
				{
					color
				}), text);
			}
			this.PushText(msg, linkItem);
		}

		public void ParseLinkText(ref string msg, ITEM linkItem)
		{
			if (linkItem != null && linkItem.m_nItemUnique != 0)
			{
				int num = msg.IndexOf('<');
				if (0 < num)
				{
					int num2 = msg.IndexOf('>', num);
					if (0 < num2)
					{
						int num3 = msg.IndexOf('\0');
						string text = MsgHandler.HandleReturn<string>("GetItemName", new object[]
						{
							linkItem
						});
						string b = msg.Substring(num + 1, num2 - (num + 1));
						if (text == b)
						{
							string str = msg.Substring(0, num);
							string str2 = "{@I" + text + "}";
							string str3 = string.Empty;
							if (num3 > 0)
							{
								str3 = msg.Substring(num2 + 1, num3 - (num2 + 1));
							}
							else
							{
								str3 = msg.Substring(num2 + 1, msg.Length - (num2 + 1));
							}
							msg = str + str2 + str3;
						}
					}
				}
			}
		}

		public void PushText(UIListItemContainer item, string msg, ITEM linkItem)
		{
			if (null == item)
			{
				return;
			}
			this.ParseLinkText(ref msg, linkItem);
			this.RemoveFirstLine();
			if ("true" == MsgHandler.HandleReturn<string>("ReservedWordManagerIsUse", new object[0]))
			{
				msg = MsgHandler.HandleReturn<string>("ReservedWordManagerReplaceWord", new object[]
				{
					msg
				});
			}
			if (this.changeScrollPos)
			{
				if (!base.IsClickKnob())
				{
					base.InsertItemDonotPosionUpdate(item, base.Count, null, true);
					base.RepositionItemsDonotClipItems(this.scrollPos);
				}
				else
				{
					base.AddItem(item);
					if (this.changeScrollPos)
					{
						base.RepositionItemsDonotClipItems(1f);
					}
				}
			}
			else
			{
				base.AddItem(item);
				if (this.changeScrollPos)
				{
					base.RepositionItemsDonotClipItems(1f);
				}
			}
		}

		public void PushText(string msg, ITEM linkItem)
		{
			this.ParseLinkText(ref msg, linkItem);
			this.RemoveFirstLine();
			if ("true" == MsgHandler.HandleReturn<string>("ReservedWordManagerIsUse", new object[0]))
			{
				msg = MsgHandler.HandleReturn<string>("ReservedWordManagerReplaceWord", new object[]
				{
					msg
				});
			}
			UIListItemContainer item = MsgHandler.HandleReturn<UIListItemContainer>("EmoticonInfoParseEmoticon", new object[]
			{
				msg,
				base.GetSize().x,
				this.fontSize,
				this.FontEffect,
				linkItem,
				true
			});
			if (this.changeScrollPos)
			{
				if (!base.IsClickKnob())
				{
					base.InsertItemDonotPosionUpdate(item, base.Count, null, true);
					base.RepositionItemsDonotClipItems(this.scrollPos);
				}
				else
				{
					base.InsertItemDonotPosionUpdate(item, base.Count, null, true);
					base.RepositionItemsDonotClipItems(this.scrollPos);
				}
			}
			else
			{
				base.InsertItemDonotPosionUpdate(item, base.Count, null, true);
				base.RepositionItemsDonotClipItems(0f);
				if (this.changeScrollPos)
				{
					base.RepositionItemsDonotClipItems(1f);
				}
			}
		}

		public void RepositionAndClip()
		{
			if (this.changeScrollPos)
			{
				if (!base.IsClickKnob())
				{
					base.RepositionItemsDonotClipItems(1f);
				}
			}
			else if (this.changeScrollPos)
			{
				base.RepositionItemsDonotClipItems(1f);
			}
		}

		public void InitBugTemp()
		{
		}

		private void RemoveFirstLine()
		{
			if (this.items.Count >= this.maxChatLabelLine)
			{
				int num = 0;
				if (num < 0 || num >= this.items.Count)
				{
					return;
				}
				this.items[num].DeleteAnim();
				if (this.container != null)
				{
					this.container.RemoveChild(this.items[num].gameObject);
				}
				if (this.selectedItem == this.items[num])
				{
					this.selectedItem = null;
					this.items[num].SetSelected(false);
				}
				this.visibleItems.Remove(this.items[num]);
				this.items[num].Delete();
				UnityEngine.Object.Destroy(this.items[num].gameObject);
				this.items.RemoveAt(num);
			}
		}

		public void Clear()
		{
			base.ClearList(true);
		}
	}
}
