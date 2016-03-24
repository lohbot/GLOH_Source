using GAME;
using GameMessage;
using GameMessage.Private;
using Global;
using IndunTriggerClient;
using Ndoors.Framework.Stage;
using Ndoors.Memory;
using NPatch;
using omniata;
using PROTOCOL.BASE;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using PROTOCOL.MESSAGE.ID;
using PROTOCOL.WORLD;
using PROTOCOL.WORLD.ID;
using SERVICE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TapjoyUnity;
using Ts;
using TsBundle;
using UnityEngine;
using UnityForms;

namespace PROTOCOL
{
	internal class NrReceiveGame
	{
		public delegate void ReceivePaketFunction(NkDeserializePacket kDeserializePacket);

		public static Dictionary<int, NrReceiveGame.ReceivePaketFunction> m_kDicReceivePacketFunction;

		public static long m_LastReceivePacketNum;

		public static void RegisterReceivePacketFunction()
		{
			if (!TsPlatform.IsEditor)
			{
				TsLog.EnableAllLogType(false, false, true, true, true);
			}
			NrReceiveGame.m_kDicReceivePacketFunction = new Dictionary<int, NrReceiveGame.ReceivePaketFunction>();
			Type typeFromHandle = typeof(eGAME_PACKET_ID);
			Type typeFromHandle2 = typeof(NrReceiveGame);
			Type typeFromHandle3 = typeof(NrReceiveGame.ReceivePaketFunction);
			Type[] types = new Type[]
			{
				typeof(NkDeserializePacket)
			};
			foreach (int num in Enum.GetValues(typeFromHandle))
			{
				string name = Enum.GetName(typeFromHandle, num);
				MethodInfo method = typeFromHandle2.GetMethod(name, types);
				if (method != null)
				{
					Delegate @delegate = Delegate.CreateDelegate(typeFromHandle3, method);
					if (@delegate != null)
					{
						NrReceiveGame.m_kDicReceivePacketFunction.Add(num, (NrReceiveGame.ReceivePaketFunction)@delegate);
					}
				}
			}
			Debug.LogWarning("Total Registered Function Count : " + NrReceiveGame.m_kDicReceivePacketFunction.Count);
		}

		public static int Receive_GamePacket(byte[] btBuffer, int index, PacketHeader header)
		{
			int result = 0;
			if (NrReceiveGame.IsRegistPacket(header.type))
			{
				NrReceiveGame.m_LastReceivePacketNum = (long)header.type;
			}
			if (0 < header.type && 2555 > header.type)
			{
				if (NrReceiveGame.m_kDicReceivePacketFunction.ContainsKey(header.type))
				{
					NrReceiveGame.ReceivePaketFunction receivePaketFunction = NrReceiveGame.m_kDicReceivePacketFunction[header.type];
					NkDeserializePacket nkDeserializePacket = new NkDeserializePacket(btBuffer, index);
					receivePaketFunction(nkDeserializePacket);
					result = nkDeserializePacket.TotalReadByte;
				}
				else
				{
					Debug.LogWarning("DONT FIND PACKET METHOD :(T)" + header.type);
				}
			}
			else if (16777216 < header.type && 16777320 > header.type)
			{
				string name = Enum.GetName(typeof(eWORLD_PACKET_ID), header.type);
				MethodInfo method = typeof(NrReceiveGame).GetMethod(name);
				if (method != null)
				{
					NkDeserializePacket nkDeserializePacket2 = new NkDeserializePacket(btBuffer, index);
					object[] parameters = new object[]
					{
						nkDeserializePacket2
					};
					method.Invoke(null, parameters);
					result = nkDeserializePacket2.TotalReadByte;
				}
				else
				{
					Debug.LogWarning("DONT FIND METHOD :(T)" + name + "(F)" + name);
				}
			}
			return result;
		}

		public static string ExtendedTrim(string source)
		{
			string text = source;
			int num = text.IndexOf('\0');
			if (num > -1)
			{
				text = source.Substring(0, num + 1);
			}
			return text.TrimEnd(new char[1]).Trim();
		}

		public static long GetTick()
		{
			return (long)Environment.TickCount;
		}

		public static long GetDueDate(long _time)
		{
			DateTime dateTime = default(DateTime);
			DateTime dateTime2 = new DateTime(1970, 1, 1, 9, 0, 0);
			dateTime = DateTime.Now;
			dateTime2 = dateTime2.AddSeconds((double)_time);
			Debug.Log("LateTimeDateTime :" + dateTime2);
			return (dateTime2.Ticks - dateTime.Ticks) / 10000000L;
		}

		public DateTime GetDate(long _time)
		{
			DateTime result = new DateTime(1970, 1, 1, 9, 0, 0);
			result = result.AddSeconds((double)_time);
			return result;
		}

		public static bool IsRegistPacket(int packetNum)
		{
			return packetNum != 1105 && packetNum != 39 && packetNum != 84 && packetNum != 86 && packetNum != 903 && packetNum != 991 && packetNum != 50;
		}

		public static void GS_AUCTION_REGISTER_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_AUCTION_REGISTER_ACK packet = kDeserializePacket.GetPacket<GS_AUCTION_REGISTER_ACK>();
			AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
			if (auctionMainDlg != null && packet.i32Result == 0)
			{
				auctionMainDlg.InitControl(AuctionMainDlg.eTAB.eTAB_SELL);
			}
			if (packet.i32Result == 0)
			{
				NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
				NrTSingleton<ExplorationManager>.Instance.RemoveSolInfo(packet.i64SolID);
				readySolList.DelSol(packet.i64SolID);
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64Money;
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("233"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				if (0 < packet.i32ItemUnique)
				{
					NrTSingleton<ItemManager>.Instance.PlayItemUseSound(packet.i32ItemUnique, true);
				}
				else if (0L < packet.i64SolID)
				{
					TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "AUCTION", "HERO_SELL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				}
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.SetCharDetail(packet.i32CharDetailType, packet.i64CharDetailValue);
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("395"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.AUCTION_SELLCHECK_DLG);
		}

		public static void GS_AUCTION_SELLLIST_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_AUCTION_SELLLIST_ACK packet = kDeserializePacket.GetPacket<GS_AUCTION_SELLLIST_ACK>();
			AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
			if (auctionMainDlg != null)
			{
				auctionMainDlg.SetSellList(packet, kDeserializePacket);
			}
		}

		public static void GS_AUCTION_TENDER_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_AUCTION_TENDER_ACK packet = kDeserializePacket.GetPacket<GS_AUCTION_TENDER_ACK>();
			AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
			if (auctionMainDlg != null)
			{
			}
			if (packet.i32Result == 0)
			{
				if (0L < packet.i64CostMoney)
				{
					NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64CurCostMoney;
				}
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("242"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				if (auctionMainDlg != null)
				{
					auctionMainDlg.SetTenderSuccess();
				}
				if (0L < packet.i64CostMoney)
				{
					TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MARKET", "BUY", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				}
				else if (0 < packet.i32CostHearts)
				{
					TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_ITEM", "HEARTS", "SELECT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				}
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("241"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				if (packet.i32ServerResult == -1)
				{
					auctionMainDlg.SetTenderSuccess();
				}
			}
		}

		public static void GS_AUCTION_TENDERLIST_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_AUCTION_TENDERLIST_ACK packet = kDeserializePacket.GetPacket<GS_AUCTION_TENDERLIST_ACK>();
			AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
			if (auctionMainDlg != null)
			{
				auctionMainDlg.SetTenderList(packet, kDeserializePacket);
			}
		}

		public static void GS_AUCTION_PURCHASELIST_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_AUCTION_PURCHASELIST_ACK packet = kDeserializePacket.GetPacket<GS_AUCTION_PURCHASELIST_ACK>();
			AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
			if (auctionMainDlg != null)
			{
				auctionMainDlg.SetPurchaseListAll(packet, kDeserializePacket);
			}
		}

		public static void GS_AUCTION_PURCHASELIST_ITEM_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_AUCTION_PURCHASELIST_ITEM_ACK packet = kDeserializePacket.GetPacket<GS_AUCTION_PURCHASELIST_ITEM_ACK>();
			AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
			if (auctionMainDlg != null)
			{
				auctionMainDlg.SetPurchaseListItem(packet, kDeserializePacket);
			}
		}

		public static void GS_AUCTION_PURCHASELIST_SOL_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_AUCTION_PURCHASELIST_SOL_ACK packet = kDeserializePacket.GetPacket<GS_AUCTION_PURCHASELIST_SOL_ACK>();
			AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
			if (auctionMainDlg != null)
			{
				auctionMainDlg.SetPurchaseListSol(packet, kDeserializePacket);
			}
		}

		public static void GS_AUCTION_DIRECTPURCHASE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_AUCTION_DIRECTPURCHASE_ACK packet = kDeserializePacket.GetPacket<GS_AUCTION_DIRECTPURCHASE_ACK>();
			AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
			if (auctionMainDlg != null)
			{
			}
			if (packet.i32Result == 0)
			{
				if (0L < packet.i64DirectCostMoney)
				{
					NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64CurCostMoney;
				}
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("245"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				if (auctionMainDlg != null)
				{
					auctionMainDlg.SetTenderSuccess();
				}
				NoticeIconDlg.SetIcon(ICON_TYPE.POST, true);
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "AUCTION", "IMMEDIATELY", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.SetCharDetail(packet.i32CharDetailType, packet.i64CharDetailValue);
			}
		}

		public static void GS_AUCTION_REGISTER_CANCEL_ACK(NkDeserializePacket kDeserializePacket)
		{
			if (kDeserializePacket.GetPacket<GS_AUCTION_REGISTER_CANCEL_ACK>() == null)
			{
				return;
			}
			AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
			if (auctionMainDlg != null)
			{
				auctionMainDlg.ClickRefresh(null);
			}
		}

		public static void GS_AUCTION_SUCCESSFUL_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_AUCTION_SUCCESSFUL_ACK packet = kDeserializePacket.GetPacket<GS_AUCTION_SUCCESSFUL_ACK>();
			if (packet.i32Result == 0)
			{
				AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
				if (auctionMainDlg != null)
				{
					auctionMainDlg.ClickRefresh(null);
				}
			}
		}

		public static void GS_AUCTION_REGISTERINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_AUCTION_REGISTERINFO_ACK packet = kDeserializePacket.GetPacket<GS_AUCTION_REGISTERINFO_ACK>();
			if (999 < packet.i32RegisterNum)
			{
				packet.i32RegisterNum = 999;
			}
			MainMenuDlg mainMenuDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAINMENU_DLG) as MainMenuDlg;
			if (mainMenuDlg != null)
			{
				mainMenuDlg.SetAuctionRegisterInfo(packet.i32RegisterNum);
			}
		}

		public static void GS_AUCTION_REGISTERCHECK_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_AUCTION_REGISTERCHECK_ACK packet = kDeserializePacket.GetPacket<GS_AUCTION_REGISTERCHECK_ACK>();
			AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
			if (auctionMainDlg != null)
			{
				auctionMainDlg.SetRegisterCheckACK(packet);
			}
		}

		public static void GS_AUCTION_STATE_CHANGE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_AUCTION_STATE_CHANGE_ACK packet = kDeserializePacket.GetPacket<GS_AUCTION_STATE_CHANGE_ACK>();
			AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
			if (auctionMainDlg != null)
			{
				auctionMainDlg.SetTenderSuccess();
			}
			NoticeIconDlg.SetIcon(ICON_TYPE.POST, true);
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "AUCTION", "IMMEDIATELY", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			if (packet.i8Type == 0)
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64PocketMoney;
			}
			else if (packet.i8Type == 1)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("783"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}

		public static void GS_BABELTOWER_CLEARINFO_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BABELTOWER_CLEARINFO_GET_ACK packet = kDeserializePacket.GetPacket<GS_BABELTOWER_CLEARINFO_GET_ACK>();
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			for (int i = 0; i < packet.count; i++)
			{
				BABEL_CLEARINFO packet2 = kDeserializePacket.GetPacket<BABEL_CLEARINFO>();
				myCharInfo.AddBabelClearInfo(packet2);
			}
			for (int j = 0; j < packet.rankinfo_count; j++)
			{
				BABEL_SUBFLOOR_RANKINFO packet3 = kDeserializePacket.GetPacket<BABEL_SUBFLOOR_RANKINFO>();
				myCharInfo.AddBabelSubFloorRankInfo(packet3);
			}
		}

		public static void GS_BABELTOWER_CLEARINFO_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BABELTOWER_CLEARINFO_SET_ACK packet = kDeserializePacket.GetPacket<GS_BABELTOWER_CLEARINFO_SET_ACK>();
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			myCharInfo.SetBabelClearInfo(packet.column, packet.clearinfo, packet.floortype);
			myCharInfo.SetBabelSubFloorRankInfo(packet.floor, packet.subfloor, packet.rank, packet.bTreasure, packet.floortype);
			BabelTowerMainDlg babelTowerMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWERMAIN_DLG) as BabelTowerMainDlg;
			if (babelTowerMainDlg != null)
			{
				babelTowerMainDlg.Show();
			}
		}

		public static void GS_BABELTOWER_GOLOBBY_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BABELTOWER_GOLOBBY_ACK packet = kDeserializePacket.GetPacket<GS_BABELTOWER_GOLOBBY_ACK>();
			if (packet.result == 0)
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BABELTOWERMAIN_DLG);
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BABELTOWERSUB_DLG);
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WHISPER_DLG);
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_DLG);
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_RESULT_DLG);
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "ATTACK-FORMATION", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				if (!Scene.IsCurScene(Scene.Type.SOLDIER_BATCH))
				{
					SoldierBatch.SOLDIER_BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER;
					FacadeHandler.PushStage(Scene.Type.SOLDIER_BATCH);
				}
				SoldierBatch.BABELTOWER_INFO.SetBabelTowerInfo(packet.nBabelRoomIndex, packet.babel_floor, packet.babel_subfloor, packet.nLeaderPersonID, packet.nMinLevel, packet.nMaxLevel, packet.i16BountyHuntUnique, packet.i16babel_floortype);
				for (int i = 0; i < 4; i++)
				{
					string charName = TKString.NEWString(packet.stBabelPersonInfo[i].szCharName);
					SoldierBatch.BABELTOWER_INFO.stBabelPersonInfo[i].SetInfo(packet.stBabelPersonInfo[i].nPartyPersonID, charName, packet.stBabelPersonInfo[i].nLevel, packet.stBabelPersonInfo[i].bReady, packet.stBabelPersonInfo[i].nSlotType, packet.stBabelPersonInfo[i].nCharKind);
				}
				SoldierBatch.BABELTOWER_INFO.SetPartyCount();
				SoldierBatch.SOLDIERBATCH.SetChangeBabelBatchGrid();
				if (SoldierBatch.SOLDIERBATCH == null)
				{
					return;
				}
				if (SoldierBatch.BABELTOWER_INFO.GetPartyCount() > 1)
				{
					SoldierBatch.SOLDIERBATCH.SolBatchLock = true;
					NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
					if (charPersonInfo.GetPersonID() == packet.nLeaderPersonID && !SoldierBatch.BABELTOWER_INFO.IsPartyBatch())
					{
						SoldierBatch.SOLDIERBATCH.ResetSolPosition();
						SoldierBatch.SOLDIERBATCH.LoadBatchSolInfo_Party(null);
					}
				}
				NrPersonInfoUser charPersonInfo2 = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (charPersonInfo2.GetPersonID() == packet.nLeaderPersonID)
				{
					Battle.isLeader = true;
				}
				else
				{
					Battle.isLeader = false;
				}
				SoldierBatch.BABELTOWER_INFO.InitReadyState();
				if (SoldierBatch.BABELTOWER_INFO.GetPartyCount() > 1)
				{
					BabelLobbyUserListDlg babelLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABELTOWERUSERLIST_DLG) as BabelLobbyUserListDlg;
					if (babelLobbyUserListDlg != null)
					{
						babelLobbyUserListDlg.RefreshSolInfo();
						babelLobbyUserListDlg.SetWaitingLock(true);
					}
					BabelTower_ChatDlg babelTower_ChatDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABELTOWER_CHAT) as BabelTower_ChatDlg;
					if (babelTower_ChatDlg != null)
					{
						babelTower_ChatDlg.RefreshChatInfo();
					}
					for (int j = 0; j < 4; j++)
					{
						if (packet.stBabelPersonInfo[j].nPartyPersonID == packet.nEnterPersonID)
						{
							string text = TKString.NEWString(packet.stBabelPersonInfo[j].szCharName);
							string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("187");
							string empty = string.Empty;
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
							{
								textFromNotify,
								"charname",
								text
							});
							Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
							break;
						}
					}
				}
			}
			else
			{
				string text2 = string.Empty;
				int result = packet.result;
				if (result != 8000)
				{
					if (result != 8001)
					{
						if (result != 1)
						{
							if (result != 8)
							{
								if (result != 701)
								{
									if (result != 4100)
									{
									}
								}
								else
								{
									text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("203");
								}
							}
							else
							{
								text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("417");
							}
						}
						else
						{
							text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("175");
						}
					}
					else
					{
						text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("160");
					}
				}
				else
				{
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("161");
				}
				if (text2 != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(text2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
		}

		public static void GS_BABELTOWER_BATTLEPOS_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BABELTOWER_BATTLEPOS_SET_ACK packet = kDeserializePacket.GetPacket<GS_BABELTOWER_BATTLEPOS_SET_ACK>();
			if (SoldierBatch.SOLDIERBATCH == null)
			{
				return;
			}
			SoldierBatch.SOLDIERBATCH.SetBabelTowerSoldierBatch(packet);
			BabelLobbyUserListDlg babelLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWERUSERLIST_DLG) as BabelLobbyUserListDlg;
			if (babelLobbyUserListDlg != null)
			{
				babelLobbyUserListDlg.RefreshSolCount();
			}
		}

		public static void GS_BABELTOWER_BATTLEPOS_REFLASH_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BABELTOWER_BATTLEPOS_REFLASH_ACK packet = kDeserializePacket.GetPacket<GS_BABELTOWER_BATTLEPOS_REFLASH_ACK>();
			if (SoldierBatch.SOLDIERBATCH == null)
			{
				return;
			}
			long[] array = new long[16];
			for (int i = 0; i < 16; i++)
			{
				array[i] = 0L;
			}
			GS_BABELTOWER_BATTLEPOS_SET_ACK[] array2 = new GS_BABELTOWER_BATTLEPOS_SET_ACK[packet.nSolCount];
			for (int j = 0; j < packet.nSolCount; j++)
			{
				array2[j] = kDeserializePacket.GetPacket<GS_BABELTOWER_BATTLEPOS_SET_ACK>();
			}
			int num = 0;
			bool flag = false;
			for (int k = 0; k < SoldierBatch.SOLDIERBATCH.GetBabelTowerTotalBatchInfoCount(); k++)
			{
				bool flag2 = true;
				long babelTowerSolIDFromIndex = SoldierBatch.SOLDIERBATCH.GetBabelTowerSolIDFromIndex(k);
				if (babelTowerSolIDFromIndex > 0L)
				{
					flag = true;
					for (int l = 0; l < packet.nSolCount; l++)
					{
						if (babelTowerSolIDFromIndex == array2[l].nSolID)
						{
							flag2 = false;
						}
					}
					if (flag2)
					{
						array[num] = babelTowerSolIDFromIndex;
						num++;
					}
				}
			}
			for (int m = 0; m < 16; m++)
			{
				if (array[m] > 0L)
				{
					SoldierBatch.SOLDIERBATCH.DeleteBabelBatchInfoFromSolID(array[m]);
				}
			}
			if (!flag)
			{
				for (int n = 0; n < packet.nSolCount; n++)
				{
					SoldierBatch.SOLDIERBATCH.SetBabelTowerSoldierBatch(array2[n]);
				}
			}
			if (!SoldierBatch.BABELTOWER_INFO.IsPartyBatch())
			{
				SoldierBatch.SOLDIERBATCH.LoadBatchSolInfo_Party(array2);
			}
			BabelLobbyUserListDlg babelLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWERUSERLIST_DLG) as BabelLobbyUserListDlg;
			if (babelLobbyUserListDlg != null)
			{
				babelLobbyUserListDlg.RefreshSolCount();
				babelLobbyUserListDlg.SetWaitingLock(false);
			}
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
			{
				PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
				if (plunderSolListDlg != null)
				{
					plunderSolListDlg.m_bMyBatchMode = true;
					NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
					if (charPersonInfo == null)
					{
						return;
					}
					int babelTowerSolCount = SoldierBatch.SOLDIERBATCH.GetBabelTowerSolCount(charPersonInfo.GetPersonID());
					plunderSolListDlg.SetSolNum(babelTowerSolCount, false);
				}
			}
			SoldierBatch.SOLDIERBATCH.SolBatchLock = false;
		}

		public static void GS_BABELTOWER_LEAVE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BABELTOWER_LEAVE_ACK packet = kDeserializePacket.GetPacket<GS_BABELTOWER_LEAVE_ACK>();
			if (SoldierBatch.SOLDIERBATCH == null)
			{
				return;
			}
			if (!SoldierBatch.BABELTOWER_INFO.IsBabelLeader(packet.nLeavePersonID))
			{
				if (packet.mode == 2)
				{
					NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
					if (charPersonInfo.GetPersonID() == packet.nLeavePersonID)
					{
						SoldierBatch.BABELTOWER_INFO.Init();
						NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
						return;
					}
				}
				SoldierBatch.BABELTOWER_INFO.DeletePartyPerson(packet.nLeavePersonID);
				SoldierBatch.BABELTOWER_INFO.SetPartyCount();
				SoldierBatch.SOLDIERBATCH.SetChangeBabelBatchGrid();
				long[] array = new long[16];
				for (int i = 0; i < 16; i++)
				{
					array[i] = 0L;
				}
				GS_BABELTOWER_BATTLEPOS_SET_ACK[] array2 = new GS_BABELTOWER_BATTLEPOS_SET_ACK[(int)packet.nSolCount];
				for (int j = 0; j < (int)packet.nSolCount; j++)
				{
					array2[j] = kDeserializePacket.GetPacket<GS_BABELTOWER_BATTLEPOS_SET_ACK>();
				}
				int num = 0;
				for (int k = 0; k < SoldierBatch.SOLDIERBATCH.GetBabelTowerTotalBatchInfoCount(); k++)
				{
					bool flag = true;
					long babelTowerSolIDFromIndex = SoldierBatch.SOLDIERBATCH.GetBabelTowerSolIDFromIndex(k);
					if (babelTowerSolIDFromIndex > 0L)
					{
						for (int l = 0; l < (int)packet.nSolCount; l++)
						{
							if (babelTowerSolIDFromIndex == array2[l].nSolID)
							{
								flag = false;
							}
						}
						if (flag)
						{
							array[num] = babelTowerSolIDFromIndex;
							num++;
						}
					}
				}
				for (int m = 0; m < 16; m++)
				{
					if (array[m] > 0L)
					{
						SoldierBatch.SOLDIERBATCH.DeleteBabelBatchInfoFromSolID(array[m]);
					}
				}
				SoldierBatch.BABELTOWER_INFO.InitReadyState();
				BabelLobbyUserListDlg babelLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWERUSERLIST_DLG) as BabelLobbyUserListDlg;
				if (babelLobbyUserListDlg != null)
				{
					babelLobbyUserListDlg.RefreshSolInfo();
					babelLobbyUserListDlg.SetWaitingLock(false);
				}
				BabelTower_ChatDlg babelTower_ChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWER_CHAT) as BabelTower_ChatDlg;
				if (babelTower_ChatDlg != null)
				{
					babelTower_ChatDlg.RefreshChatInfo();
				}
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
				{
					PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
					if (plunderSolListDlg != null)
					{
						plunderSolListDlg.m_bMyBatchMode = true;
						plunderSolListDlg.SetSolNum((int)packet.nSolCount, false);
					}
				}
			}
			else
			{
				SoldierBatch.BABELTOWER_INFO.Init();
				NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
			}
		}

		public static void GS_BABELTOWER_READY_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BABELTOWER_READY_ACK packet = kDeserializePacket.GetPacket<GS_BABELTOWER_READY_ACK>();
			if (SoldierBatch.SOLDIERBATCH == null)
			{
				return;
			}
			SoldierBatch.BABELTOWER_INFO.SetReadyBattle(packet.nPersonID, packet.bReady);
			BabelLobbyUserListDlg babelLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABELTOWERUSERLIST_DLG) as BabelLobbyUserListDlg;
			if (babelLobbyUserListDlg != null)
			{
				babelLobbyUserListDlg.RefreshSolInfo();
				babelLobbyUserListDlg.SetWaitingLock(false);
			}
		}

		public static void GS_BABELTOWER_CHANGE_SLOTTYPE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BABELTOWER_CHANGE_SLOTTYPE_ACK packet = kDeserializePacket.GetPacket<GS_BABELTOWER_CHANGE_SLOTTYPE_ACK>();
			if (SoldierBatch.SOLDIERBATCH == null)
			{
				return;
			}
			if (packet.pos < 0)
			{
				return;
			}
			SoldierBatch.BABELTOWER_INFO.SetSlotType(packet.pos, packet.change_type);
			BabelLobbyUserListDlg babelLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABELTOWERUSERLIST_DLG) as BabelLobbyUserListDlg;
			if (babelLobbyUserListDlg != null)
			{
				babelLobbyUserListDlg.RefreshSolInfo();
				babelLobbyUserListDlg.SetWaitingLock(false);
			}
		}

		public static void GS_BABELTOWER_CHANGE_CHECKLEVEL_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BABELTOWER_CHANGE_CHECKLEVEL_ACK packet = kDeserializePacket.GetPacket<GS_BABELTOWER_CHANGE_CHECKLEVEL_ACK>();
			SoldierBatch.BABELTOWER_INFO.SetPossibleLevel(packet.nMinLevel, packet.nMaxLevel);
			BabelLobbyUserListDlg babelLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABELTOWERUSERLIST_DLG) as BabelLobbyUserListDlg;
			if (babelLobbyUserListDlg != null)
			{
				babelLobbyUserListDlg.RefreshPossibleLevel();
			}
		}

		public static void GS_BABELTOWER_OPENROOMLIST_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BABELTOWER_OPENROOMLIST_GET_ACK packet = kDeserializePacket.GetPacket<GS_BABELTOWER_OPENROOMLIST_GET_ACK>();
			BabelTowerOpenRoomListDlg babelTowerOpenRoomListDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABELTOWER_OPENROOMLIST_DLG) as BabelTowerOpenRoomListDlg;
			BabelTowerMainDlg babelTowerMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWERMAIN_DLG) as BabelTowerMainDlg;
			if (babelTowerMainDlg != null)
			{
				babelTowerOpenRoomListDlg.FloorType = babelTowerMainDlg.FloorType;
			}
			babelTowerOpenRoomListDlg.InitOpemRoomList();
			for (int i = 0; i < packet.count; i++)
			{
				BABELTOWER_OPENROOMLIST packet2 = kDeserializePacket.GetPacket<BABELTOWER_OPENROOMLIST>();
				if (babelTowerOpenRoomListDlg != null)
				{
					babelTowerOpenRoomListDlg.AddOpenRoominfo(packet2);
				}
			}
			babelTowerOpenRoomListDlg.ShowInfo();
		}

		public static void GS_BABELTOWER_INVITE_FRIEND_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BABELTOWER_INVITE_FRIEND_ACK packet = kDeserializePacket.GetPacket<GS_BABELTOWER_INVITE_FRIEND_ACK>();
			if (Launcher.Instance.LocalPatchLevel != Launcher.Instance.PatchLevelMax)
			{
				NrReceiveGame.OnBabelInviteCancelPatchLevel(packet);
				return;
			}
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (packet.Result == 0)
			{
				if (kMyCharInfo.m_PersonID == packet.ReqPersonID)
				{
					USER_FRIEND_INFO friend = kMyCharInfo.m_kFriendInfo.GetFriend(packet.InvitePersonID);
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("302");
					string empty = string.Empty;
					if (friend != null)
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							textFromNotify,
							"charname",
							TKString.NEWString(friend.szName)
						});
					}
					else
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							textFromNotify,
							"charname",
							TKString.NEWString(packet.InvitePersonName)
						});
					}
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
				else if (kMyCharInfo.m_PersonID == packet.InvitePersonID)
				{
					USER_FRIEND_INFO friend2 = kMyCharInfo.m_kFriendInfo.GetFriend(packet.ReqPersonID);
					string text = string.Empty;
					string empty2 = string.Empty;
					string text2 = string.Empty;
					string title = string.Empty;
					if (0 >= packet.i16BountyHuntUnique)
					{
						title = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("96");
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("97");
						if (packet.floortype == 2)
						{
							text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2784");
						}
						if (friend2 != null)
						{
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
							{
								text,
								"charname",
								TKString.NEWString(friend2.szName),
								"type",
								text2,
								"floor",
								packet.floor,
								"subfloor",
								(int)(packet.sub_floor + 1)
							});
						}
						else
						{
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
							{
								text,
								"charname",
								TKString.NEWString(packet.ReqPersonName),
								"type",
								text2,
								"floor",
								packet.floor,
								"subfloor",
								(int)(packet.sub_floor + 1)
							});
						}
					}
					else
					{
						short num = 0;
						BountyInfoData bountyInfoDataFromUnique = NrTSingleton<BountyHuntManager>.Instance.GetBountyInfoDataFromUnique(packet.i16BountyHuntUnique);
						if (bountyInfoDataFromUnique != null)
						{
							num = bountyInfoDataFromUnique.i16Episode;
						}
						title = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("210");
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("211");
						if (friend2 != null)
						{
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
							{
								text,
								"charname",
								TKString.NEWString(friend2.szName),
								"stage",
								num
							});
						}
						else
						{
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
							{
								text,
								"charname",
								TKString.NEWString(packet.ReqPersonName),
								"stage",
								num
							});
						}
					}
					MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
					msgBoxUI.SetMsg(new YesDelegate(NrReceiveGame.OnBabelInviteAccept), packet, new NoDelegate(NrReceiveGame.OnBabelInviteCancel), packet, title, empty2, eMsgType.MB_OK_CANCEL);
					msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("317"));
					msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("318"));
				}
			}
			else
			{
				string text3 = string.Empty;
				string text4 = string.Empty;
				int result = packet.Result;
				if (result != 1)
				{
					if (result != 29)
					{
						if (result == 501)
						{
							text4 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("175");
						}
					}
					else
					{
						text4 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("215");
					}
				}
				else
				{
					USER_FRIEND_INFO friend3 = kMyCharInfo.m_kFriendInfo.GetFriend(packet.InvitePersonID);
					text3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("764");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text4, new object[]
					{
						text3,
						"targetname",
						TKString.NEWString(friend3.szName)
					});
				}
				if (text4 != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(text4, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
		}

		public static void OnBabelInviteAccept(object EventObject)
		{
			GS_BABELTOWER_INVITE_FRIEND_ACK gS_BABELTOWER_INVITE_FRIEND_ACK = (GS_BABELTOWER_INVITE_FRIEND_ACK)EventObject;
			if (gS_BABELTOWER_INVITE_FRIEND_ACK == null)
			{
				return;
			}
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo == null)
			{
				return;
			}
			if (gS_BABELTOWER_INVITE_FRIEND_ACK.i16BountyHuntUnique > 0)
			{
				if (kMyCharInfo.m_nActivityPoint <= 0L && NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
				{
					WillChargeDlg willChargeDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.WILLCHARGE_DLG) as WillChargeDlg;
					if (willChargeDlg != null)
					{
						willChargeDlg.BabelInvitePersonInfo = gS_BABELTOWER_INVITE_FRIEND_ACK;
					}
					return;
				}
			}
			else
			{
				BABELTOWER_DATA babelTowerData = NrTSingleton<BabelTowerManager>.Instance.GetBabelTowerData(gS_BABELTOWER_INVITE_FRIEND_ACK.floor, gS_BABELTOWER_INVITE_FRIEND_ACK.sub_floor, gS_BABELTOWER_INVITE_FRIEND_ACK.floortype);
				if (babelTowerData == null)
				{
					TsLog.LogError("BABELTOWER_DATA == NULL  FloorType ={0} Floor={1} SubFloor={2}", new object[]
					{
						gS_BABELTOWER_INVITE_FRIEND_ACK.floortype,
						gS_BABELTOWER_INVITE_FRIEND_ACK.floor,
						gS_BABELTOWER_INVITE_FRIEND_ACK.sub_floor
					});
					return;
				}
				if (!kMyCharInfo.IsEnableBattleUseActivityPoint(babelTowerData.m_nWillSpend))
				{
					WillChargeDlg willChargeDlg2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.WILLCHARGE_DLG) as WillChargeDlg;
					if (willChargeDlg2 != null)
					{
						willChargeDlg2.BabelInvitePersonInfo = gS_BABELTOWER_INVITE_FRIEND_ACK;
					}
					return;
				}
			}
			bool bMoveWorld = false;
			if (Client.m_MyWS != (long)gS_BABELTOWER_INVITE_FRIEND_ACK.nReqPersonWorldID)
			{
				bMoveWorld = true;
			}
			GS_BABELTOWER_INVITE_FRIEND_AGREE_REQ gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ = new GS_BABELTOWER_INVITE_FRIEND_AGREE_REQ();
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.nInvite = 0;
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.bAccept = true;
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.bMoveWorld = bMoveWorld;
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.WorldID = gS_BABELTOWER_INVITE_FRIEND_ACK.nReqPersonWorldID;
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.ChannelID = gS_BABELTOWER_INVITE_FRIEND_ACK.ReqPersonCHID;
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.PersonID = gS_BABELTOWER_INVITE_FRIEND_ACK.ReqPersonID;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_INVITE_FRIEND_AGREE_REQ, gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ);
		}

		public static void OnBabelInviteCancel(object EventObject)
		{
			GS_BABELTOWER_INVITE_FRIEND_ACK gS_BABELTOWER_INVITE_FRIEND_ACK = (GS_BABELTOWER_INVITE_FRIEND_ACK)EventObject;
			GS_BABELTOWER_INVITE_FRIEND_AGREE_REQ gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ = new GS_BABELTOWER_INVITE_FRIEND_AGREE_REQ();
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.nInvite = 1;
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.bAccept = false;
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.bMoveWorld = false;
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.WorldID = 0;
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.ChannelID = 0;
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.PersonID = gS_BABELTOWER_INVITE_FRIEND_ACK.ReqPersonID;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_INVITE_FRIEND_AGREE_REQ, gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ);
		}

		public static void OnBabelInviteCancelPatchLevel(object EventObject)
		{
			GS_BABELTOWER_INVITE_FRIEND_ACK gS_BABELTOWER_INVITE_FRIEND_ACK = (GS_BABELTOWER_INVITE_FRIEND_ACK)EventObject;
			GS_BABELTOWER_INVITE_FRIEND_AGREE_REQ gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ = new GS_BABELTOWER_INVITE_FRIEND_AGREE_REQ();
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.nInvite = 2;
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.bAccept = false;
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.bMoveWorld = false;
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.WorldID = 0;
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.ChannelID = 0;
			gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ.PersonID = gS_BABELTOWER_INVITE_FRIEND_ACK.ReqPersonID;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BABELTOWER_INVITE_FRIEND_AGREE_REQ, gS_BABELTOWER_INVITE_FRIEND_AGREE_REQ);
		}

		public static void GS_BABELTOWER_INVITE_FRIEND_AGREE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BABELTOWER_INVITE_FRIEND_AGREE_ACK packet = kDeserializePacket.GetPacket<GS_BABELTOWER_INVITE_FRIEND_AGREE_ACK>();
			if (packet.Result != 0)
			{
				string text = string.Empty;
				string text2 = string.Empty;
				int result = packet.Result;
				if (result != 8000)
				{
					if (result != 8001)
					{
						if (result != 8007)
						{
							if (result != 8008)
							{
								if (result != 1)
								{
									if (result != 8)
									{
										if (result == 701)
										{
											text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("203");
										}
									}
									else
									{
										text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("417");
									}
								}
								else
								{
									text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("175");
								}
							}
							else
							{
								text = "���� ���� ä�ο� ������ ���� �ʽ��ϴ�.";
							}
						}
						else
						{
							text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("216");
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
							{
								text2,
								"charname",
								TKString.NEWString(packet.szname)
							});
						}
					}
					else
					{
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("160");
					}
				}
				else
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("161");
				}
				if (text != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
		}

		public static void GS_FRIENDS_BABELTOWER_CLEARINFO_ACk(NkDeserializePacket kDeserializePacket)
		{
			GS_FRIENDS_BABELTOWER_CLEARINFO_ACk packet = kDeserializePacket.GetPacket<GS_FRIENDS_BABELTOWER_CLEARINFO_ACk>();
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			for (int i = 0; i < (int)packet.i16FriendCount; i++)
			{
				FRIEND_BABEL_CLEARINFO packet2 = kDeserializePacket.GetPacket<FRIEND_BABEL_CLEARINFO>();
				myCharInfo.m_kFriendInfo.AddFriendBabel(packet2);
			}
			BabelTowerMainDlg babelTowerMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWERMAIN_DLG) as BabelTowerMainDlg;
			if (babelTowerMainDlg != null)
			{
				babelTowerMainDlg.ShowList();
			}
		}

		public static void GS_BABELTOWER_INVITE_RNDUSERLIST_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BABELTOWER_INVITE_RNDUSERLIST_NFY packet = kDeserializePacket.GetPacket<GS_BABELTOWER_INVITE_RNDUSERLIST_NFY>();
			if (SoldierBatch.BABELTOWER_INFO == null)
			{
				return;
			}
			for (int i = 0; i < packet.invite_count; i++)
			{
				BABEL_RNDINVITE_PERSON packet2 = kDeserializePacket.GetPacket<BABEL_RNDINVITE_PERSON>();
				SoldierBatch.BABELTOWER_INFO.AddRndInvitePerson(packet2);
			}
		}

		public static void GS_BATTLE_CLOSE_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_CLOSE_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_CLOSE_NFY>();
			if (Battle.BATTLE == null)
			{
				Debug.LogError("Batte is NULL");
				return;
			}
			Battle.BATTLE.GS_BATTLE_CLOSE_NFY(packet);
		}

		public static void GS_BATTLE_INFO_NFY(NkDeserializePacket kDeserializePacket)
		{
			if (Battle.BATTLE == null)
			{
				return;
			}
			GS_BATTLE_INFO_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_INFO_NFY>();
			PacketClientOrder order = new PacketClientOrder("GS_BATTLE_INFO_NFY", packet);
			Battle.BATTLE.InPacket(order);
		}

		public static void GS_BATTLE_SOLDIER_LIST_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_SOLDIER_LIST_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_SOLDIER_LIST_NFY>();
			BATTLE_SOLDIER_INFO[] array = new BATTLE_SOLDIER_INFO[(int)packet.NumSoldiers];
			sbyte b = 0;
			while ((int)b < (int)packet.NumSoldiers)
			{
				array[(int)b] = kDeserializePacket.GetPacket<BATTLE_SOLDIER_INFO>();
				b += 1;
			}
			Battle.BATTLE.GS_BATTLE_SOLDIER_LIST_NFY(array);
		}

		public static void GS_BATTLE_SOLDIER_LIST_SUBDATA_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_SOLDIER_LIST_SUBDATA_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_SOLDIER_LIST_SUBDATA_NFY>();
			BATTLE_SOLDIER_SUBDATA_INFO[] array = new BATTLE_SOLDIER_SUBDATA_INFO[(int)packet.NumSoldiers];
			sbyte b = 0;
			while ((int)b < (int)packet.NumSoldiers)
			{
				array[(int)b] = kDeserializePacket.GetPacket<BATTLE_SOLDIER_SUBDATA_INFO>();
				b += 1;
			}
			NrTSingleton<NkBattleCharManager>.Instance.ClearSolSubDataList();
			if ((int)packet.NumSoldiers == 0 || array == null || array.Length == 0)
			{
				return;
			}
			NrTSingleton<NkBattleCharManager>.Instance.SetSolSubDataList(array);
		}

		public static void GS_BATTLE_CHAR_POS_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_CHAR_POS_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_CHAR_POS_NFY>();
			Battle.BATTLE.GS_BATTLE_CHAR_POS_NFY(packet);
		}

		public static void GS_BF_MOVE_POS_LIST_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BF_MOVE_POS_LIST_NFY packet = kDeserializePacket.GetPacket<GS_BF_MOVE_POS_LIST_NFY>();
			NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID((short)packet.iFromBFCharUnique);
			if (charByBUID == null)
			{
				return;
			}
			if ((int)packet.iMovePosCount <= 0)
			{
				return;
			}
			charByBUID.MovePath = null;
			GS_CHAR_PATH[] array = new GS_CHAR_PATH[(int)packet.iMovePosCount];
			for (int i = 0; i < (int)packet.iMovePosCount; i++)
			{
				array[i] = kDeserializePacket.GetPacket<GS_CHAR_PATH>();
			}
			charByBUID.MovePath = array;
			Battle.BATTLE.GS_BF_MOVE_POS_LIST_NFY(packet);
		}

		public static void GS_BF_ORDER_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BF_ORDER_ACK packet = kDeserializePacket.GetPacket<GS_BF_ORDER_ACK>();
			Battle.BATTLE.GS_BF_ORDER_ACK(packet);
		}

		public static void GS_BF_CHARINFO_LIST_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BF_CHARINFO_LIST_NFY packet = kDeserializePacket.GetPacket<GS_BF_CHARINFO_LIST_NFY>();
			int iNumCharInfo = packet.iNumCharInfo;
			for (int i = 0; i < iNumCharInfo; i++)
			{
				Battle.BATTLE.DoGS_BF_CHARINFO_NFY(kDeserializePacket.GetPacket<GS_BF_CHARINFO_NFY>());
			}
		}

		public static void GS_BF_CHARINFO_NFY(NkDeserializePacket kDeserializePacket)
		{
		}

		public static void GS_BF_TURNINFO_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BF_TURNINFO_NFY packet = kDeserializePacket.GetPacket<GS_BF_TURNINFO_NFY>();
			Battle.BATTLE.m_TurnActivePower = null;
			GS_BF_TURN_ACTIVE_POWER[] array = new GS_BF_TURN_ACTIVE_POWER[(int)packet.m_nTurnActivePowerNum];
			for (short num = 0; num < packet.m_nTurnActivePowerNum; num += 1)
			{
				array[(int)num] = kDeserializePacket.GetPacket<GS_BF_TURN_ACTIVE_POWER>();
			}
			Battle.BATTLE.m_TurnActivePower = array;
			Battle.BATTLE.GS_BF_TURNINFO_NFY(packet);
		}

		public static void GS_BF_ACT_TURN_NFY(NkDeserializePacket kDeserializePacket)
		{
		}

		public static void GS_BATTLE_BATTLECHAR_ATB_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_BATTLECHAR_ATB_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_BATTLECHAR_ATB_NFY>();
			NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(packet.i16BUID);
			if (charByBUID == null)
			{
				return;
			}
			charByBUID.SetBattleCharATB(packet.i32BATTLEATB);
			charByBUID.CheckBattleSkillBuffeffect();
		}

		public static void GS_BATTLE_BATTLESKILL_ATB_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_BATTLESKILL_ATB_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_BATTLESKILL_ATB_NFY>();
			NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(packet.i16BUID);
			if (charByBUID == null)
			{
				return;
			}
			int num = 0;
			if (charByBUID.IsBattleCharATB(1024))
			{
				num = 1;
			}
			else if (charByBUID.IsBattleCharATB(32768))
			{
				num = 2;
			}
			charByBUID.SetBattleSkillCharATB(packet.i32BattleSkillCharATB);
			switch (num)
			{
			case 0:
				if ((charByBUID.IsBattleCharATB(1024) || charByBUID.IsBattleCharATB(32768)) && charByBUID.IsCharKindATB(128L))
				{
					Battle_BossAggro_DLG battle_BossAggro_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_BOSSAGGRO_DLG) as Battle_BossAggro_DLG;
					if (battle_BossAggro_DLG != null)
					{
						BATTLE_CONSTANT_Manager instance = BATTLE_CONSTANT_Manager.GetInstance();
						int bossImmuneCount = (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_IMMUNE_SKILL_COUNT);
						battle_BossAggro_DLG.SetBossImmuneCount(bossImmuneCount);
						battle_BossAggro_DLG.UpdateBossInfo();
					}
				}
				break;
			case 1:
				if (!charByBUID.IsBattleCharATB(1024) && charByBUID.IsCharKindATB(128L))
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("579"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				break;
			case 2:
				if (!charByBUID.IsBattleCharATB(32768) && !charByBUID.IsBattleCharATB(1024))
				{
					if (charByBUID.IsCharKindATB(128L))
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("579"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					}
				}
				else if (charByBUID.IsBattleCharATB(1024) && charByBUID.IsCharKindATB(128L))
				{
					Battle_BossAggro_DLG battle_BossAggro_DLG2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_BOSSAGGRO_DLG) as Battle_BossAggro_DLG;
					if (battle_BossAggro_DLG2 != null)
					{
						BATTLE_CONSTANT_Manager instance2 = BATTLE_CONSTANT_Manager.GetInstance();
						int bossImmuneCount2 = (int)instance2.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_IMMUNE_SKILL_COUNT);
						battle_BossAggro_DLG2.SetBossImmuneCount(bossImmuneCount2);
						battle_BossAggro_DLG2.UpdateBossInfo();
					}
				}
				break;
			}
		}

		public static void GS_BATTLE_BOSS_AGGRO_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_BOSS_AGGRO_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_BOSS_AGGRO_NFY>();
			if (NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(packet.i16BUID) == null)
			{
				return;
			}
			if (Battle.BATTLE != null && (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MINE || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_EXPEDITION))
			{
				return;
			}
			Battle_BossAggro_DLG battle_BossAggro_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_BOSSAGGRO_DLG) as Battle_BossAggro_DLG;
			if (battle_BossAggro_DLG != null)
			{
				battle_BossAggro_DLG.UpdateAggroSolInfo(packet);
			}
		}

		public static void GS_BATTLE_MYTHRAID_DAMAGE_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_MYTHRAID_DAMAGE_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_MYTHRAID_DAMAGE_NFY>();
			if (Battle.BATTLE != null && Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID)
			{
				return;
			}
			Battle_MythRaidBattleInfo_DLG battle_MythRaidBattleInfo_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTHRAID_BATTLEINFO_DLG) as Battle_MythRaidBattleInfo_DLG;
			if (battle_MythRaidBattleInfo_DLG != null)
			{
				battle_MythRaidBattleInfo_DLG.UpdateDamageInfo(packet.i64AddTotalDamage);
			}
		}

		public static void GS_BATTLE_MYTHRAID_PARTY_BESTDAMAGE_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_MYTHRAID_PARTY_BESTDAMAGE_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_MYTHRAID_PARTY_BESTDAMAGE_NFY>();
			if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID)
			{
				Battle_MythRaidBattleInfo_DLG battle_MythRaidBattleInfo_DLG = (Battle_MythRaidBattleInfo_DLG)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTHRAID_BATTLEINFO_DLG);
				if (battle_MythRaidBattleInfo_DLG != null)
				{
					battle_MythRaidBattleInfo_DLG.UpdatePartyBestDamge(new string(packet.szBestRecordUserName), packet.nAccumulateDamage);
				}
			}
		}

		public static void GS_BF_ANGELANGERLYPOINT_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BF_ANGELANGERLYPOINT_NFY packet = kDeserializePacket.GetPacket<GS_BF_ANGELANGERLYPOINT_NFY>();
			if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID && !Battle.Replay)
			{
				Battle_Control_Dlg battle_Control_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
				if (battle_Control_Dlg != null)
				{
					battle_Control_Dlg.SetAngelAngerlyPointForce(packet.nNowAngerAngerlyPoint);
				}
			}
		}

		public static void GS_BATTLE_ROOM_USER_PERSONID_INFO_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_ROOM_USER_PERSONID_INFO_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_ROOM_USER_PERSONID_INFO_NFY>();
			for (int i = 0; i < (int)packet.bAttackPersonCount; i++)
			{
				GS_BATTLE_ROOM_USER_PERSONID_INFO_SUB packet2 = kDeserializePacket.GetPacket<GS_BATTLE_ROOM_USER_PERSONID_INFO_SUB>();
				if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo != null && NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID == packet2.i64PersonID)
				{
					Battle.BATTLE.MyAlly = eBATTLE_ALLY.eBATTLE_ALLY_0;
				}
			}
			for (int j = 0; j < (int)packet.bDefencePersonCount; j++)
			{
				GS_BATTLE_ROOM_USER_PERSONID_INFO_SUB packet3 = kDeserializePacket.GetPacket<GS_BATTLE_ROOM_USER_PERSONID_INFO_SUB>();
				if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo != null && NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID == packet3.i64PersonID)
				{
					Battle.BATTLE.MyAlly = eBATTLE_ALLY.eBATTLE_ALLY_1;
				}
			}
		}

		public static void GS_BATTLE_ANGERLY_POINT_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_ANGERLY_POINT_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_ANGERLY_POINT_NFY>();
			if (Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID && packet.i32AngerlyPoint >= 0 && !Battle.Replay)
			{
				Battle_Control_Dlg battle_Control_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
				if (battle_Control_Dlg != null)
				{
					battle_Control_Dlg.SetAngerlyPoint(packet.i32AngerlyPoint);
					battle_Control_Dlg.UpdateBattleSkillData();
				}
			}
		}

		public static void GS_BATTLE_COLOSSEUM_INFO_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_COLOSSEUM_INFO_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_COLOSSEUM_INFO_NFY>();
			string szAlly0Name = TKString.NEWString(packet.szAlly0Name);
			string szAlly1Name = TKString.NEWString(packet.szAlly1Name);
			if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM)
			{
				Battle_Colossum_CharinfoDlg battle_Colossum_CharinfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_COLOSSEUM_CHARINFO_DLG) as Battle_Colossum_CharinfoDlg;
				if (battle_Colossum_CharinfoDlg != null)
				{
					battle_Colossum_CharinfoDlg.Set(szAlly0Name, szAlly1Name);
					if (!packet.bObserver)
					{
						battle_Colossum_CharinfoDlg.SetStartCountEffect();
					}
				}
			}
		}

		public static void GS_BATTLE_SERVER_ERROR_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_SERVER_ERROR_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_SERVER_ERROR_NFY>();
			PacketClientOrder order = new PacketClientOrder("GS_BATTLE_SERVER_ERROR_NFY", packet);
			if (Battle.BATTLE == null)
			{
				return;
			}
			Battle.BATTLE.InPacket(order);
		}

		public static void GS_BF_TURN_STATE_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BF_TURN_STATE_NFY packet = kDeserializePacket.GetPacket<GS_BF_TURN_STATE_NFY>();
			Battle.BATTLE.GS_BF_TURN_STATE_NFY(packet);
		}

		public static void GS_BATTLE_CONTINUE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_CONTINUE_ACK packet = kDeserializePacket.GetPacket<GS_BATTLE_CONTINUE_ACK>();
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo == null)
			{
				return;
			}
			if (kMyCharInfo.GetBattleContinueType() != (E_BATTLE_CONTINUE_TYPE)packet.i8Mode)
			{
				kMyCharInfo.SetBattleContinueType((E_BATTLE_CONTINUE_TYPE)packet.i8Mode);
				if (packet.i8Mode == 0)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("361"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				}
				else if (packet.i8Mode == 1)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("360"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				}
			}
		}

		public static void GS_BATTLE_SREWARD_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_SREWARD_ACK packet = kDeserializePacket.GetPacket<GS_BATTLE_SREWARD_ACK>();
			Battle_ResultDlg_Content battle_ResultDlg_Content = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_CONTENT_DLG) as Battle_ResultDlg_Content;
			if (battle_ResultDlg_Content == null)
			{
				return;
			}
			battle_ResultDlg_Content.ClearSRewardSolData();
			for (int i = 0; i < packet.m_nRewardNumSoldiers; i++)
			{
				GS_BATTLE_RESULT_SOLDIER packet2 = kDeserializePacket.GetPacket<GS_BATTLE_RESULT_SOLDIER>();
				battle_ResultDlg_Content.AddSRewardSolData(packet2);
			}
			battle_ResultDlg_Content.ClearSRewardItemData();
			for (int j = 0; j < packet.m_nRewardNumItems; j++)
			{
				GS_BATTLE_RESULT_ITEM packet3 = kDeserializePacket.GetPacket<GS_BATTLE_RESULT_ITEM>();
				battle_ResultDlg_Content.AddSRewardItemData(packet3);
			}
			battle_ResultDlg_Content.SetSRewardBasicData(packet);
			battle_ResultDlg_Content._LinkSRewardDataSelect();
			if (!packet.bGiveVal)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("633"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			}
		}

		public static void GS_BATTLE_EVENT_TRIGGER_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_EVENT_TRIGGER_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_EVENT_TRIGGER_NFY>();
			PacketClientOrder order = new PacketClientOrder("GS_BATTLE_EVENT_TRIGGER_NFY", packet);
			Battle.BATTLE.InPacket(order);
		}

		public static void GS_BATTLE_EVENT_STATUS_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_EVENT_STATUS_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_EVENT_STATUS_NFY>();
			PacketClientOrder order = new PacketClientOrder("GS_BATTLE_EVENT_STATUS_NFY", packet);
			Battle.BATTLE.InPacket(order);
		}

		public static void GS_BATTLE_EVENT_ACTION_EFFECT_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_EVENT_ACTION_EFFECT_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_EVENT_ACTION_EFFECT_NFY>();
			PacketClientOrder order = new PacketClientOrder("GS_BATTLE_EVENT_ACTION_EFFECT_NFY", packet);
			Battle.BATTLE.InPacket(order);
		}

		public static void GS_BATTLE_EVENT_ACTION_SOUND_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_EVENT_ACTION_SOUND_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_EVENT_ACTION_SOUND_NFY>();
			string domainKey = TKString.NEWString(packet.szDomain);
			string categoryKey = TKString.NEWString(packet.szCategory);
			string audioKey = TKString.NEWString(packet.szAudioKey);
			TsAudioManager.Instance.AudioContainer.RequestAudioClip(domainKey, categoryKey, audioKey, new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}

		public static void GS_BATTLE_EVENT_ACTION_ANIMATION_NFY(NkDeserializePacket kDeseializePacket)
		{
			GS_BATTLE_EVENT_ACTION_ANIMATION_NFY packet = kDeseializePacket.GetPacket<GS_BATTLE_EVENT_ACTION_ANIMATION_NFY>();
			PacketClientOrder order = new PacketClientOrder("GS_BATTLE_EVENT_ACTION_ANIMATION_NFY", packet);
			Battle.BATTLE.InPacket(order);
		}

		public static void GS_BATTLE_EVENT_ACTION_CUTSCENE_CAMERA_NFY(NkDeserializePacket kDeseializePacket)
		{
			GS_BATTLE_EVENT_ACTION_CUTSCENE_CAMERA_NFY packet = kDeseializePacket.GetPacket<GS_BATTLE_EVENT_ACTION_CUTSCENE_CAMERA_NFY>();
			PacketClientOrder order = new PacketClientOrder("GS_BATTLE_EVENT_ACTION_CUTSCENE_CAMERA_NFY", packet);
			Battle.BATTLE.InPacket(order);
		}

		public static void GS_BATTLE_OPEN_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_OPEN_ACK packet = kDeserializePacket.GetPacket<GS_BATTLE_OPEN_ACK>();
			if (packet.Result != 0)
			{
				Debug.Log("GS_BATTLE_OPEN_ACK : " + ((eRESULT)packet.Result).ToString());
				NrLoadPageScreen.ShowHideLoadingImg(false);
			}
			if (packet.Result != 0 && NrTSingleton<FormsManager>.Instance.IsShow(G_ID.DAILYDUNGEON_MAIN))
			{
				DailyDungeon_Main_Dlg dailyDungeon_Main_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DAILYDUNGEON_MAIN) as DailyDungeon_Main_Dlg;
				if (dailyDungeon_Main_Dlg != null)
				{
					dailyDungeon_Main_Dlg.RestoreDailyDungeonDlg();
				}
			}
			int result = packet.Result;
			switch (result)
			{
			case 47:
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("200");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				break;
			}
			case 48:
			{
				string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("324");
				Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				break;
			}
			case 49:
			{
				string textFromNotify3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("279");
				Main_UI_SystemMessage.ADDMessage(textFromNotify3, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				Debug.LogError("Scenario is not loaded");
				break;
			}
			case 50:
			{
				string textFromNotify4 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("279");
				Main_UI_SystemMessage.ADDMessage(textFromNotify4, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				break;
			}
			case 51:
			{
				string textFromNotify5 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("279");
				Main_UI_SystemMessage.ADDMessage(textFromNotify5, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				break;
			}
			default:
				switch (result)
				{
				case 9700:
				{
					string textFromNotify6 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("905");
					Main_UI_SystemMessage.ADDMessage(textFromNotify6, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
					return;
				}
				case 9701:
				case 9702:
				{
					IL_B4:
					if (result == 0)
					{
						NrTSingleton<NkBattleReplayManager>.Instance.m_bHiddenEnemyName = ((packet.BattleStartOption & 2) != 0);
						if (NrTSingleton<NkBattleReplayManager>.Instance.SaveReplay && !NrTSingleton<NkBattleReplayManager>.Instance.IsReplay)
						{
							BaseNet_Game.GetInstance().m_bSavePacket = true;
						}
						if (!Scene.IsCurScene(Scene.Type.BATTLE))
						{
							NpcTalkUI_DLG npcTalkUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NPCTALK_DLG) as NpcTalkUI_DLG;
							if (npcTalkUI_DLG != null)
							{
								npcTalkUI_DLG.AcceptExit();
								NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.NPCTALK_DLG);
							}
							Battle_ResultDlg battle_ResultDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_DLG) as Battle_ResultDlg;
							if (battle_ResultDlg != null)
							{
								NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_RESULT_DLG);
							}
							NmMotionBlurLoading.GoToNormalBattle();
						}
						return;
					}
					if (result == 1)
					{
						string textFromNotify7 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("279");
						Main_UI_SystemMessage.ADDMessage(textFromNotify7, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
						return;
					}
					if (result == 8)
					{
						string textFromNotify8 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("488");
						Main_UI_SystemMessage.ADDMessage(textFromNotify8, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
						if (NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
						{
							NrTSingleton<NkBabelMacroManager>.Instance.SetStatus(eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_NONE, 0f);
						}
						return;
					}
					if (result == 29)
					{
						string textFromNotify9 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("288");
						Main_UI_SystemMessage.ADDMessage(textFromNotify9, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
						return;
					}
					if (result != 71)
					{
						string textFromNotify10 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("279");
						Main_UI_SystemMessage.ADDMessage(textFromNotify10, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
						return;
					}
					string textFromNotify11 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("335");
					Main_UI_SystemMessage.ADDMessage(textFromNotify11, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
					return;
				}
				case 9703:
				{
					string textFromNotify12 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("907");
					Main_UI_SystemMessage.ADDMessage(textFromNotify12, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
					return;
				}
				}
				goto IL_B4;
			}
		}

		public static void GS_BATTLE_INTRUSION_NFY(NkDeserializePacket kDeserializePacket)
		{
		}

		public static void MsgBoxYes(object a_oYesObject)
		{
		}

		public static void MsgBoxNo(object a_oYesObject)
		{
		}

		public static void GS_BATTLE_RESULT_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_RESULT_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_RESULT_NFY>();
			if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo() == null)
			{
				return;
			}
			GameObject gameObject = GameObject.Find("UI Camera");
			if (gameObject != null)
			{
				Camera componentInChildren = gameObject.GetComponentInChildren<Camera>();
				if (componentInChildren != null && !componentInChildren.enabled)
				{
					componentInChildren.enabled = true;
				}
			}
			if (Battle.BATTLE == null)
			{
				return;
			}
			if (NrTSingleton<NkBabelMacroManager>.Instance != null && NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
			{
				bool bWin = (eBATTLE_ALLY)packet.i8WinAlly == Battle.BATTLE.MyAlly;
				NrTSingleton<NkBabelMacroManager>.Instance.SaveBattleResult(bWin);
			}
			if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PREVIEW)
			{
				return;
			}
			Battle_ResultDlg battle_ResultDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_RESULT_DLG) as Battle_ResultDlg;
			if (battle_ResultDlg == null)
			{
				return;
			}
			battle_ResultDlg.ClearSolData();
			for (int i = 0; i < (int)packet.NumSoldiers; i++)
			{
				GS_BATTLE_RESULT_SOLDIER packet2 = kDeserializePacket.GetPacket<GS_BATTLE_RESULT_SOLDIER>();
				battle_ResultDlg.AddSolData(packet2);
			}
			battle_ResultDlg.ClearItemData();
			for (int j = 0; j < packet.NumItems; j++)
			{
				GS_BATTLE_RESULT_ITEM packet3 = kDeserializePacket.GetPacket<GS_BATTLE_RESULT_ITEM>();
				battle_ResultDlg.AddItemData(packet3);
			}
			battle_ResultDlg.SetBasicData(packet);
			if (NrTSingleton<NkBattleReplayManager>.Instance.IsReplay)
			{
				packet.BattleSRewardUnique = 0;
			}
			battle_ResultDlg.LinkData(packet.BattleSRewardUnique);
			NrTSingleton<NkQuestManager>.Instance.UpdateVictoryQuestMessage();
		}

		public static void GS_BATTLE_RESULT_PLUNDER_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_RESULT_PLUNDER_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_RESULT_PLUNDER_NFY>();
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			DateTime dateTime = DateTime.Now.ToLocalTime();
			DateTime arg_3B_0 = dateTime;
			DateTime dateTime2 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			int num = (int)(arg_3B_0 - dateTime2.ToLocalTime()).TotalSeconds;
			string value = "lose";
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("ts", num.ToString());
			if (packet.i8WinAlly == 0)
			{
				value = "win";
			}
			dictionary.Add("hero_war_result", value);
			dictionary.Add("total_rank", myCharInfo.PlunderRank.ToString());
			dictionary.Add("earned_gold", packet.nRewardMoney.ToString());
			dictionary.Add("earned_matchingpt", packet.nAddMatchPoint.ToString());
			dictionary.Add("total_matchingpt", packet.nCurrentMatchPoint.ToString());
			dictionary.Add("level", myCharInfo.GetLevel().ToString());
			dictionary.Add("account_id", myCharInfo.m_SN.ToString());
			GameObject pkGoOminiata = StageSystemCheck.pkGoOminiata;
			if (pkGoOminiata)
			{
				OmniataComponent component = pkGoOminiata.GetComponent<OmniataComponent>();
				if (component)
				{
					component.Track("om_herowar", dictionary);
				}
			}
			if (Battle.BATTLE == null)
			{
				return;
			}
			Battle_ResultPlunderDlg battle_ResultPlunderDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_RESULT_PLUNDER_DLG) as Battle_ResultPlunderDlg;
			if (battle_ResultPlunderDlg == null)
			{
				return;
			}
			battle_ResultPlunderDlg.ShowMode();
			battle_ResultPlunderDlg.ClearSolData();
			for (int i = 0; i < (int)packet.NumSoldiers; i++)
			{
				GS_BATTLE_RESULT_SOLDIER packet2 = kDeserializePacket.GetPacket<GS_BATTLE_RESULT_SOLDIER>();
				battle_ResultPlunderDlg.AddSolData(packet2);
			}
			battle_ResultPlunderDlg.SetBasicData(packet);
			battle_ResultPlunderDlg.LinkData();
		}

		public static void GS_BATTLE_RESULT_MYTHRAID_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_RESULT_MYTHRAID_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_RESULT_MYTHRAID_NFY>();
			if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo() == null)
			{
				return;
			}
			GameObject gameObject = GameObject.Find("UI Camera");
			if (gameObject != null)
			{
				Camera componentInChildren = gameObject.GetComponentInChildren<Camera>();
				if (componentInChildren != null && !componentInChildren.enabled)
				{
					componentInChildren.enabled = true;
				}
			}
			if (Battle.BATTLE == null)
			{
				return;
			}
			MythRaid_Result_DLG mythRaid_Result_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTHRAID_RESULT_DLG) as MythRaid_Result_DLG;
			if (mythRaid_Result_DLG == null)
			{
				Debug.LogError("Error : Cannot Find MYTHRAID_REULST_DLG");
				return;
			}
			if (packet.i8isParty == 0)
			{
				NrTSingleton<MythRaidManager>.Instance.IsParty = false;
			}
			else
			{
				NrTSingleton<MythRaidManager>.Instance.IsParty = true;
			}
			for (int i = 0; i < (int)packet.i8NumSoldiers; i++)
			{
				GS_BATTLE_RESULT_MYTHRAID_SOLDIER packet2 = kDeserializePacket.GetPacket<GS_BATTLE_RESULT_MYTHRAID_SOLDIER>();
				if (!NrTSingleton<MythRaidManager>.Instance.dic_SolInfo.ContainsKey(packet2.PersonID))
				{
					NrTSingleton<MythRaidManager>.Instance.dic_SolInfo[packet2.PersonID] = new List<GS_BATTLE_RESULT_MYTHRAID_SOLDIER>();
				}
				NrTSingleton<MythRaidManager>.Instance.dic_SolInfo[packet2.PersonID].Add(packet2);
			}
			mythRaid_Result_DLG.SetData(packet.nRaidType, packet.fBattleTime, packet.nBattleContinueCount, packet.nTotalMythRaidBossDamage, packet.i8isParty);
			mythRaid_Result_DLG.Show();
		}

		public static void GS_BATTLE_STOP_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_STOP_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_STOP_NFY>();
			Battle.BATTLE.GS_BATTLE_STOP_NFY(packet);
		}

		public static void GS_BATTLE_CHANGE_POS_LIST_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_CHANGE_POS_LIST_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_CHANGE_POS_LIST_NFY>();
			GS_BATTLE_CHANGE_POS[] array = new GS_BATTLE_CHANGE_POS[packet.nCount];
			for (int i = 0; i < packet.nCount; i++)
			{
				array[i] = kDeserializePacket.GetPacket<GS_BATTLE_CHANGE_POS>();
			}
			if (Battle.BATTLE == null)
			{
				return;
			}
			Battle.BATTLE.GS_BATTLE_CHANGE_POS_LIST_NFY(array);
		}

		public static void GS_BATTLE_RESTART_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_RESTART_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_RESTART_NFY>();
			if (Battle.BATTLE == null)
			{
				return;
			}
			Battle.BATTLE.GS_BATTLE_RESTART_NFY(packet);
		}

		public static void GS_BATTLE_ANGEL_ORDER_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_ANGEL_ORDER_ACK packet = kDeserializePacket.GetPacket<GS_BATTLE_ANGEL_ORDER_ACK>();
			Battle_Control_Dlg battle_Control_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
			if (battle_Control_Dlg != null)
			{
				battle_Control_Dlg.SetAngelAngerlyPoint(packet.nGuardAngelUnique, packet.nNowAngerAngerlyPoint);
			}
		}

		public static void GS_BATTLE_AUTO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_AUTO_ACK packet = kDeserializePacket.GetPacket<GS_BATTLE_AUTO_ACK>();
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo == null)
			{
				return;
			}
			if (kMyCharInfo.GetAutoBattle() != (E_BF_AUTO_TYPE)packet.i8Mode)
			{
				kMyCharInfo.SetAutoBattle((E_BF_AUTO_TYPE)packet.i8Mode);
				if (Battle.BATTLE != null && Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM)
				{
					if (packet.i8Mode == 0)
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("524"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
					}
					else if (packet.i8Mode == 1)
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("523"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
					}
				}
				if (NrTSingleton<NkBabelMacroManager>.Instance.IsMacro() && kMyCharInfo.GetAutoBattle() == E_BF_AUTO_TYPE.MANUAL)
				{
					NrTSingleton<NkBabelMacroManager>.Instance.SetStatus(eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_NONE, 0f);
				}
			}
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BABELTOWER_FUNCTION_DLG))
			{
				BabelTower_FunctionDlg babelTower_FunctionDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWER_FUNCTION_DLG) as BabelTower_FunctionDlg;
				if (kMyCharInfo.GetAutoBattle() == E_BF_AUTO_TYPE.AUTO)
				{
					babelTower_FunctionDlg.SetAutoBattle(true);
				}
				else
				{
					babelTower_FunctionDlg.SetAutoBattle(false);
				}
			}
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.DAILYDUNGEON_FUNCTION_DLG))
			{
				DailyDungeon_Function_Dlg dailyDungeon_Function_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DAILYDUNGEON_FUNCTION_DLG) as DailyDungeon_Function_Dlg;
				if (kMyCharInfo.GetAutoBattle() == E_BF_AUTO_TYPE.AUTO)
				{
					dailyDungeon_Function_Dlg.Set_AutoBattle(true);
				}
				else
				{
					dailyDungeon_Function_Dlg.Set_AutoBattle(false);
				}
			}
		}

		public static void GS_BATTLE_SLOW_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_SLOW_ACK packet = kDeserializePacket.GetPacket<GS_BATTLE_SLOW_ACK>();
			Battle.BATTLE.GS_BATTLE_SLOW_ACK(packet);
		}

		public static void GS_BATTLE_FIGHT_ALLOW_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_FIGHT_ALLOW_ACK packet = kDeserializePacket.GetPacket<GS_BATTLE_FIGHT_ALLOW_ACK>();
			if (Launcher.Instance.LocalPatchLevel != Launcher.Instance.PatchLevelMax)
			{
				NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
				kMyCharInfo.OnFightPatchCancle(packet);
				return;
			}
			string text = TKString.NEWString(packet.szCharName);
			if (NrTSingleton<NkCharManager>.Instance.IsMySameCharUnique(packet.nCharUnique))
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("689"),
					"Charname",
					text
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			else
			{
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("22");
				string empty2 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("17"),
					"charname",
					text,
					"level",
					packet.nLevel.ToString()
				});
				GS_BATTLE_OPEN_FIGHT_REQ gS_BATTLE_OPEN_FIGHT_REQ = new GS_BATTLE_OPEN_FIGHT_REQ();
				gS_BATTLE_OPEN_FIGHT_REQ.nCharUnique = packet.nCharUnique;
				gS_BATTLE_OPEN_FIGHT_REQ.nPersonID = packet.nPersonID;
				NrMyCharInfo kMyCharInfo2 = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
				msgBoxUI.SetMsg(new YesDelegate(kMyCharInfo2.OnFightOK), gS_BATTLE_OPEN_FIGHT_REQ, new NoDelegate(kMyCharInfo2.OnFightCancle), gS_BATTLE_OPEN_FIGHT_REQ, textFromMessageBox, empty2, eMsgType.MB_OK_CANCEL);
				msgBoxUI.AutoCloseTime = Time.time + 10f;
			}
		}

		public static void GS_BATTLE_FRIEND_HELP_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_FRIEND_HELP_ACK packet = kDeserializePacket.GetPacket<GS_BATTLE_FRIEND_HELP_ACK>();
			if (packet.nResult == 1)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("58"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			}
			else if (packet.nResult == 2)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("77"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			}
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.COMMUNITY_DLG))
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.COMMUNITY_DLG);
			}
		}

		public static void GS_BATTLE_CHANGE_SOLDIER_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_CHANGE_SOLDIER_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_CHANGE_SOLDIER_NFY>();
			if (packet.nResult == 1)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("192"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_EMERGENCY_CALL_DLG) && Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID)
				{
					NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_EMERGENCY_CALL_DLG);
				}
			}
			else if (packet.nResult == 2)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("194"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_EMERGENCY_CALL_DLG) && Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID)
				{
					NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BATTLE_EMERGENCY_CALL_DLG);
				}
			}
			else if (packet.nResult == 3)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			}
			else if (packet.nResult == 4)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("511"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			else if (packet.nResult == 5)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("194"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
		}

		public static void GS_BATTLE_REVIVE_SOLDIER_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_REVIVE_SOLDIER_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_REVIVE_SOLDIER_NFY>();
			if (packet.nResult == 1)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("192"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			}
			else if (packet.nResult == 2)
			{
				BATTLE_CONSTANT_Manager instance = BATTLE_CONSTANT_Manager.GetInstance();
				if (instance == null)
				{
					return;
				}
				int num = (int)instance.GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_RESURRECTION_COUNT);
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("530"),
					"count",
					num.ToString()
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			}
			else if (packet.nResult == 3)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("273"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			}
			else if (packet.nResult == 4)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("534"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
		}

		public static void GS_BATTLE_RECONNECTINFO_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_RECONNECTINFO_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_RECONNECTINFO_NFY>();
			BATTLE_SOLDIER_INFO[] array = new BATTLE_SOLDIER_INFO[(int)packet.nSolCount];
			sbyte b = 0;
			while ((int)b < (int)packet.nSolCount)
			{
				array[(int)b] = kDeserializePacket.GetPacket<BATTLE_SOLDIER_INFO>();
				b += 1;
			}
			GS_BATTLE_RECONNECT_SOLDIERINFO[] array2 = new GS_BATTLE_RECONNECT_SOLDIERINFO[(int)packet.nTurnStateCount];
			sbyte b2 = 0;
			while ((int)b2 < (int)packet.nTurnStateCount)
			{
				array2[(int)b2] = kDeserializePacket.GetPacket<GS_BATTLE_RECONNECT_SOLDIERINFO>();
				b2 += 1;
			}
			if (Battle.BATTLE == null)
			{
				return;
			}
			Battle.BATTLE.BATTLE_RECONNECT_PROCESS(array);
			Battle.BATTLE.BATTLE_RECONNECT_PROCESS(array2);
			if (packet.m_nChangeCount >= 0)
			{
				Battle.BATTLE.ChangeSolCount = (int)packet.m_nChangeCount;
			}
		}

		public static void GS_BATTLE_BABELTOWER_CHARINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_BABELTOWER_CHARINFO_ACK packet = kDeserializePacket.GetPacket<GS_BATTLE_BABELTOWER_CHARINFO_ACK>();
			Battle_Babel_CharinfoDlg battle_Babel_CharinfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_BABEL_CHARINFO_DLG) as Battle_Babel_CharinfoDlg;
			if (battle_Babel_CharinfoDlg != null)
			{
				battle_Babel_CharinfoDlg.SetData(packet.stBabelCharInfo);
				battle_Babel_CharinfoDlg.Show();
			}
		}

		public static void GS_BATTLE_MINE_CHARINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_MINE_CHARINFO_ACK packet = kDeserializePacket.GetPacket<GS_BATTLE_MINE_CHARINFO_ACK>();
			if (packet.nMineCharinfoCount <= 0)
			{
				return;
			}
			BATTLE_MINE_CHARINFO[] array = new BATTLE_MINE_CHARINFO[(int)packet.nMineCharinfoCount];
			for (int i = 0; i < (int)packet.nMineCharinfoCount; i++)
			{
				array[i] = kDeserializePacket.GetPacket<BATTLE_MINE_CHARINFO>();
			}
			Battle_Mine_CharinfoDlg battle_Mine_CharinfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_MINE_CHARINFO_DLG) as Battle_Mine_CharinfoDlg;
			if (battle_Mine_CharinfoDlg != null)
			{
				for (int i = 0; i < (int)packet.nMineCharinfoCount; i++)
				{
					battle_Mine_CharinfoDlg.Set(array[i]);
				}
				battle_Mine_CharinfoDlg.HiddenName();
			}
		}

		public static void GS_BATTLE_MINE_ERASECHAR_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_MINE_ERASECHAR_ACK packet = kDeserializePacket.GetPacket<GS_BATTLE_MINE_ERASECHAR_ACK>();
			for (int i = 0; i < 5; i++)
			{
				if (packet.nEraseBUID[i] >= 0)
				{
					NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(packet.nEraseBUID[i]);
					if (charByBUID != null)
					{
						charByBUID.SetDeleteChar(1);
					}
				}
			}
		}

		public static void GS_BATTLE_ANGRYPOINT_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_ANGRYPOINT_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_ANGRYPOINT_NFY>();
			if (!Battle.BATTLE.Observer)
			{
				return;
			}
			Battle_Control_Dlg battle_Control_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
			if (battle_Control_Dlg != null)
			{
				battle_Control_Dlg.SetAngerlyPoint(packet.nAngryPoint);
			}
		}

		public static void GS_BATTLE_EMOTICON_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_EMOTICON_ACK packet = kDeserializePacket.GetPacket<GS_BATTLE_EMOTICON_ACK>();
			if (Battle.BATTLE == null)
			{
				return;
			}
			Battle.BATTLE.GS_BATTLE_EMOTICON_ACK(packet);
		}

		public static void GS_BATTLE_GUILDBOSS_RANKCHANGE_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_GUILDBOSS_RANKCHANGE_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_GUILDBOSS_RANKCHANGE_NFY>();
			if (Battle.BATTLE == null)
			{
				return;
			}
			if (packet.nBeforeRank == packet.nNewRank)
			{
				return;
			}
			if (packet.nBeforeRank > packet.nNewRank)
			{
				if (packet.nBeforeRank != 9999)
				{
					Battle_BossRankUp_DLG battle_BossRankUp_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_BOSS_RANKUP_DLG) as Battle_BossRankUp_DLG;
					if (battle_BossRankUp_DLG == null)
					{
						battle_BossRankUp_DLG = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_BOSS_RANKUP_DLG) as Battle_BossRankUp_DLG);
					}
					battle_BossRankUp_DLG.SetData(packet.nBeforeRank, packet.nNewRank);
				}
				Battle_GuildBossBattleInfo_DLG battle_GuildBossBattleInfo_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GUILDBOSS_BATTLEINFO_DLG) as Battle_GuildBossBattleInfo_DLG;
				int num = packet.nNewRank;
				if (num <= 0)
				{
					num = 1;
				}
				if (battle_GuildBossBattleInfo_DLG != null)
				{
					battle_GuildBossBattleInfo_DLG.UpdateUpperRankerInfo(num, (long)packet.nUpperRankerDamage, packet.nUpperRankerPersonID);
				}
			}
		}

		public static void GS_BATTLE_COLOSSEUM_WAIT_NFY(NkDeserializePacket kDeserializePacket)
		{
			if (Battle.BATTLE == null)
			{
				return;
			}
			if (Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM)
			{
				return;
			}
			GS_BATTLE_COLOSSEUM_WAIT_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_COLOSSEUM_WAIT_NFY>();
			if (packet.m_fRemainTime > 0f)
			{
				Battle_Colosseum_WaitDlg battle_Colosseum_WaitDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_COLOSSEUM_WAIT_DLG) as Battle_Colosseum_WaitDlg;
				if (battle_Colosseum_WaitDlg != null)
				{
					battle_Colosseum_WaitDlg.SetRemainTime(packet.m_fRemainTime);
				}
			}
		}

		public static void GS_BATTLE_RESULT_TUTORIAL_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_RESULT_TUTORIAL_NFY packet = kDeserializePacket.GetPacket<GS_BATTLE_RESULT_TUTORIAL_NFY>();
			if (packet.nResult != 0)
			{
				return;
			}
			if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo() == null)
			{
				return;
			}
			GameObject gameObject = GameObject.Find("UI Camera");
			if (gameObject != null)
			{
				Camera componentInChildren = gameObject.GetComponentInChildren<Camera>();
				if (componentInChildren != null && !componentInChildren.enabled)
				{
					componentInChildren.enabled = true;
				}
			}
			if (Battle.BATTLE == null)
			{
				return;
			}
			Battle.BATTLE.PlayTutorialEndBGM();
			Battle_ResultTutorialDlg battle_ResultTutorialDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_RESULT_TUTORIAL_DLG) as Battle_ResultTutorialDlg;
			if (battle_ResultTutorialDlg == null)
			{
				return;
			}
			battle_ResultTutorialDlg.ShowResultFx();
		}

		public static void GS_BATTLE_RADIO_ALARM_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_RADIO_ALARM_ACK packet = kDeserializePacket.GetPacket<GS_BATTLE_RADIO_ALARM_ACK>();
			if (packet == null || packet.szRequestUserName == null)
			{
				return;
			}
			Battle_RadioAlarmDlg battle_RadioAlarmDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_RADIOALARM_DLG) as Battle_RadioAlarmDlg;
			if (battle_RadioAlarmDlg == null)
			{
				return;
			}
			eBATTLE_RADIO_ALARM alarmKind = (eBATTLE_RADIO_ALARM)((int)Enum.Parse(typeof(eBATTLE_RADIO_ALARM), packet.i8RadioAlarmKind.ToString()));
			string alarmTextKey = battle_RadioAlarmDlg.GetAlarmTextKey(alarmKind);
			string text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify(alarmTextKey);
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			text = text.Remove(text.IndexOf('@'));
			text += new string(packet.szRequestUserName);
			Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.BATTLE_RADIO_ALARM);
		}

		public static void GS_BATTLE_STOP_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_STOP_ACK packet = kDeserializePacket.GetPacket<GS_BATTLE_STOP_ACK>();
			if (packet.i8Result != 0)
			{
				Debug.LogError("Fail Battle Stop");
				return;
			}
			NrTSingleton<NrMainSystem>.Instance.GetMainCore().BattleTimeStop(packet.bStop);
		}

		public static void GS_BUY_ITEM_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BUY_ITEM_ACK packet = kDeserializePacket.GetPacket<GS_BUY_ITEM_ACK>();
			if (packet.m_nResult == 0)
			{
			}
		}

		public static void GS_BUY_ITEM_MONEY_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BUY_ITEM_MONEY_NFY packet = kDeserializePacket.GetPacket<GS_BUY_ITEM_MONEY_NFY>();
			TsLog.Log(packet.m_nType.ToString() + "|" + packet.m_lBuyMoney.ToString(), new object[0]);
		}

		public static void GS_ITEMMALL_CHECK_CAN_TRADE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEMMALL_CHECK_CAN_TRADE_ACK packet = kDeserializePacket.GetPacket<GS_ITEMMALL_CHECK_CAN_TRADE_ACK>();
			string text = TKString.NEWString(packet.strGiftUserName);
			if (packet.m_nResult == 0)
			{
				NrTSingleton<ItemMallItemManager>.Instance.TradeItem();
				if (text != string.Empty)
				{
				}
			}
			else
			{
				NrTSingleton<ItemMallItemManager>.Instance.TradeCheckFail(packet.m_nResult, text);
			}
		}

		public static void GS_ITEMMALL_TRADE_ACK(NkDeserializePacket kDeserializePacket)
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMGIFTTARGET_DLG);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMGIFTINPUTNAME_DLG);
			GS_ITEMMALL_TRADE_ACK packet = kDeserializePacket.GetPacket<GS_ITEMMALL_TRADE_ACK>();
			if (packet.m_nResult == 0)
			{
				NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
				myCharInfo.m_Money = packet.i64Money;
				NrTSingleton<ItemMallItemManager>.Instance.AddItemMallBuyCountInfo(packet.m_MallIndex, packet.i32BuyCount);
				ITEM_MALL_ITEM item = NrTSingleton<ItemMallItemManager>.Instance.GetItem(packet.m_MallIndex);
				if (item != null)
				{
					string text = TKString.NEWString(packet.strGiftCharName);
					if (text != string.Empty)
					{
						string empty = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("706"),
							"targetname",
							text,
							"Product",
							NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(item.m_strTextKey)
						});
						Main_UI_SystemMessage.ADDMessage(empty);
					}
					else if (packet.bIsInvenFull)
					{
						string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("874");
						Main_UI_SystemMessage.ADDMessage(textFromNotify);
						NoticeIconDlg.SetIcon(ICON_TYPE.POST, true);
					}
					else
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("195"));
					}
					if (item.m_nMoneyType == 1)
					{
						if (BillingManager.eBillingType == BillingManager.eBillingManager_Type.BillingManager_Google)
						{
							BillingManager_Google component = BillingManager_Google.Instance.GetComponent<BillingManager_Google>();
							if (component != null)
							{
								component.ConsumeProduct(item.GetStoreItem());
							}
						}
						else if (BillingManager.eBillingType == BillingManager.eBillingManager_Type.BillingManager_TStore)
						{
							BillingManager_TStore component2 = BillingManager_TStore.Instance.GetComponent<BillingManager_TStore>();
							if (component2 != null)
							{
								component2.ClearData();
							}
						}
						else if (BillingManager.eBillingType == BillingManager.eBillingManager_Type.BillingManager_NStore)
						{
							BillingManager_NStore component3 = BillingManager_NStore.Instance.GetComponent<BillingManager_NStore>();
							if (component3 != null)
							{
								component3.Consume();
							}
						}
						if (PlayerPrefs.GetString(NrPrefsKey.SHOP_PRODUCT_ID).Equals(packet.m_MallIndex.ToString()))
						{
							PlayerPrefs.SetString(NrPrefsKey.SHOP_RECEIPT, string.Empty);
							PlayerPrefs.SetString(NrPrefsKey.SHOP_PRODUCT_ID, string.Empty);
						}
						eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
						if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORGOOGLE || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_KORQA)
						{
							NrTSingleton<AdWords>.Instance.PurchaseData(item);
						}
					}
					eITEMMALL_TYPE nGroup = (eITEMMALL_TYPE)item.m_nGroup;
					switch (nGroup)
					{
					case eITEMMALL_TYPE.BUY_HERO:
					case eITEMMALL_TYPE.BUY_ITEMBOX:
					case eITEMMALL_TYPE.BUY_ORI:
					case eITEMMALL_TYPE.BUY_MYTHELXIR:
						NrTSingleton<FiveRocksEventManager>.Instance.Placement("get_item");
						TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MARKET", "BUY", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
						if (item.m_nMoneyType == 2 && item.m_nGroup == 5)
						{
							NrTSingleton<FiveRocksEventManager>.Instance.HeartsConsume(eHEARTS_CONSUME.BUY_ITEMBOX, item.m_nPrice);
						}
						goto IL_486;
					case eITEMMALL_TYPE.BUY_HEARTS:
						break;
					case eITEMMALL_TYPE.BUY_GOLD:
						NrTSingleton<FiveRocksEventManager>.Instance.Placement("get_money");
						TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ETC", "COMMON-SUCCESS", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
						NrTSingleton<FiveRocksEventManager>.Instance.HeartsConsume(eHEARTS_CONSUME.BUY_GOLD, item.m_nPrice);
						if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.REFORGEMAIN_DLG))
						{
							ReforgeMainDlg reforgeMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REFORGEMAIN_DLG) as ReforgeMainDlg;
							if (reforgeMainDlg != null)
							{
								reforgeMainDlg.UpdateMoney();
							}
						}
						if (SolComposeMainDlg.Instance != null)
						{
							SolComposeMainDlg.Instance.RefreshMoney();
						}
						goto IL_486;
					case eITEMMALL_TYPE.BUY_PROTECT:
					case eITEMMALL_TYPE.BUY_EXPBOOSTER:
						TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ETC", "COMMON-SUCCESS", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
						if (item.m_nGroup == 8)
						{
							NrTSingleton<FiveRocksEventManager>.Instance.HeartsConsume(eHEARTS_CONSUME.BUY_EXPBOOSTER, item.m_nPrice);
						}
						if (item.m_nGroup == 7)
						{
							NrTSingleton<FiveRocksEventManager>.Instance.HeartsConsume(eHEARTS_CONSUME.BUY_PROTECT, item.m_nPrice);
						}
						goto IL_486;
					default:
						if (nGroup != eITEMMALL_TYPE.BUY_EVENT_ITEM)
						{
							if (nGroup != eITEMMALL_TYPE.BUY_COSTUME)
							{
								goto IL_486;
							}
							int costumeIdx = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeIdx(packet.m_MallIndex);
							NrTSingleton<NrCharCostumeTableManager>.Instance.UpdateCostumeCount(costumeIdx, 1, 1);
							CostumeRoom_Dlg costumeRoom_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COSTUMEROOM_DLG) as CostumeRoom_Dlg;
							if (costumeRoom_Dlg != null)
							{
								costumeRoom_Dlg.Refresh(false, true);
							}
							Debug.LogError("BuyItem : " + costumeIdx);
							goto IL_486;
						}
						break;
					}
					TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ETC", "COMMON-SUCCESS", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
					NrTSingleton<FiveRocksEventManager>.Instance.HeartsInflow(eHEARTS_INFLOW.BUY_CASH, item.m_nItemNum);
				}
				IL_486:
				NrTSingleton<FiveRocksEventManager>.Instance.PurchaseItem(packet.m_MallIndex);
				if (NrTSingleton<ItemMallItemManager>.Instance.IsItemVoucherType((eVOUCHER_TYPE)packet.VoucherData.ui8VoucherType))
				{
					NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.AddVoucherData(packet.VoucherData);
					ItemMallDlg itemMallDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMMALL_DLG) as ItemMallDlg;
					if (itemMallDlg != null)
					{
						itemMallDlg.SetShowDataVoucherItem((eVOUCHER_TYPE)packet.VoucherData.ui8VoucherType);
					}
				}
				ITEMMALL_POPUPSHOP poPupShop_AfterItemBuyLimit = NrTSingleton<ItemMallPoPupShopManager>.Instance.GetPoPupShop_AfterItemBuyLimit(packet.m_MallIndex);
				if (poPupShop_AfterItemBuyLimit != null)
				{
				}
				ITEM_MALL_ITEM item2 = NrTSingleton<ItemMallItemManager>.Instance.GetItem(packet.m_MallIndex);
				string textFromItem = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(item2.m_strTextKey);
				GameObject gameObject = GameObject.Find("OmniataManager");
				if (gameObject == null)
				{
					gameObject = new GameObject("OmniataManager");
				}
				if (gameObject != null)
				{
					OmniataComponent component4 = gameObject.GetComponent<OmniataComponent>();
					if (component4)
					{
						DateTime dateTime = DateTime.Now.ToLocalTime();
						DateTime arg_5A4_0 = dateTime;
						DateTime dateTime2 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
						int num = (int)(arg_5A4_0 - dateTime2.ToLocalTime()).TotalSeconds;
						string value = string.Empty;
						string eventType = string.Empty;
						if (item.m_nMoneyType == 1)
						{
							value = "usd";
							eventType = "om_revenue";
						}
						else if (item.m_nMoneyType == 2)
						{
							value = "heart";
							eventType = "om_itemsale";
						}
						else if (item.m_nMoneyType == 3)
						{
							value = "gold";
							eventType = "om_itemsale";
						}
						component4.Track(eventType, new Dictionary<string, string>
						{
							{
								"ts",
								num.ToString()
							},
							{
								"level",
								myCharInfo.GetLevel().ToString()
							},
							{
								"purchased",
								textFromItem
							},
							{
								"account_id",
								myCharInfo.m_SN.ToString()
							},
							{
								"total",
								item2.m_fPrice.ToString()
							},
							{
								"currency_code",
								value
							}
						});
						Adjust.trackRevenue(1.0, null, null);
					}
				}
				NrTSingleton<MATEventManager>.Instance.Measure_Purchase(packet.m_MallIndex);
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("211"));
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ETC", "COMMON-FAIL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				if (BillingManager.eBillingType == BillingManager.eBillingManager_Type.BillingManager_Google)
				{
					BillingManager_Google component5 = BillingManager_Google.Instance.GetComponent<BillingManager_Google>();
					if (component5 != null && component5.m_RecoveryItem != null)
					{
						component5.ConsumeProduct();
					}
				}
				else if (BillingManager.eBillingType == BillingManager.eBillingManager_Type.BillingManager_TStore)
				{
					BillingManager_TStore component6 = BillingManager_TStore.Instance.GetComponent<BillingManager_TStore>();
					if (component6 != null)
					{
						component6.ClearData();
					}
				}
				else if (BillingManager.eBillingType == BillingManager.eBillingManager_Type.BillingManager_NStore)
				{
					BillingManager_NStore component7 = BillingManager_NStore.Instance.GetComponent<BillingManager_NStore>();
					if (component7 != null)
					{
						component7.Consume();
					}
				}
				int nResult = packet.m_nResult;
				if (nResult != 31)
				{
					if (nResult != 76)
					{
						if (nResult == 9200)
						{
							Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("755"));
						}
					}
					else
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("773"));
					}
				}
				else
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46"));
				}
			}
			NrTSingleton<NrMainSystem>.Instance.m_bIsBilling = false;
			if (BillingManager.eBillingType == BillingManager.eBillingManager_Type.BillingManager_TStore)
			{
				BillingManager_TStore component8 = BillingManager_TStore.Instance.GetComponent<BillingManager_TStore>();
				if (component8 != null)
				{
					component8.EnableCloseItemMallDlg();
				}
			}
			NrTSingleton<ItemMallItemManager>.Instance.Trading = false;
		}

		public static void GS_ITEMMALL_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEMMALL_INFO_ACK packet = kDeserializePacket.GetPacket<GS_ITEMMALL_INFO_ACK>();
			if (packet.bStart)
			{
				NrTSingleton<ItemMallItemManager>.Instance.ClearItemMallData();
				NrTSingleton<ItemMallItemManager>.Instance.ClearItemMallItemBaseData();
				NrTSingleton<ItemMallItemManager>.Instance.VoucherRefillTime = packet.i32VoucherRefillTime;
				NrTSingleton<ItemMallItemManager>.Instance.ClearItemVoucherData();
			}
			for (int i = 0; i < (int)packet.i16BaseCount; i++)
			{
				ITEMMALL_ITEM_BASE_DATA packet2 = kDeserializePacket.GetPacket<ITEMMALL_ITEM_BASE_DATA>();
				NrTSingleton<ItemMallItemManager>.Instance.AddItemMallItemBaseData(packet2);
			}
			if (packet.bEnd)
			{
				NrTSingleton<ItemMallItemManager>.Instance.RefreshItemMallData();
			}
			for (int i = 0; i < (int)packet.i16Count; i++)
			{
				ITEMMALL_DATA packet3 = kDeserializePacket.GetPacket<ITEMMALL_DATA>();
				NrTSingleton<ItemMallItemManager>.Instance.AddItemMallData(packet3);
			}
			if (packet.bStart && packet.i8Login == 1)
			{
				NrTSingleton<ItemMallItemManager>.Instance.ClearItemMallBuyCount();
			}
			for (int i = 0; i < (int)packet.i16BuyCount; i++)
			{
				ITEMMALLBUYCOUNT_INFO packet4 = kDeserializePacket.GetPacket<ITEMMALLBUYCOUNT_INFO>();
				NrTSingleton<ItemMallItemManager>.Instance.AddItemMallBuyCountInfo(packet4.i64ItemMallID, packet4.i32BuyCount);
			}
			for (int i = 0; i < (int)packet.i16ItemVoucherDataCount; i++)
			{
				ITEM_VOUCHER_DATA packet5 = kDeserializePacket.GetPacket<ITEM_VOUCHER_DATA>();
				NrTSingleton<ItemMallItemManager>.Instance.AddItemVoucherData(packet5);
			}
			if (packet.bShowDLG)
			{
				ItemMallDlg itemMallDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMMALL_DLG) as ItemMallDlg;
				if (itemMallDlg != null)
				{
					if (packet.i32ItemMallMode != 0)
					{
						itemMallDlg.SetShowMode((ItemMallDlg.eMODE)packet.i32ItemMallMode);
					}
					else
					{
						itemMallDlg.SetShowType((eITEMMALL_TYPE)packet.i32ItemMallType);
					}
				}
			}
			CostumeRoom_Dlg costumeRoom_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COSTUMEROOM_DLG) as CostumeRoom_Dlg;
			if (costumeRoom_Dlg != null && costumeRoom_Dlg._costumeViewerSetter != null && costumeRoom_Dlg._costumeViewerSetter._costumeCharSetter != null)
			{
				costumeRoom_Dlg._costumeViewerSetter._costumeCharSetter.VisibleChar(false);
			}
		}

		public static void GS_ITEMMALL_FREE_TRADE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEMMALL_FREE_TRADE_ACK packet = kDeserializePacket.GetPacket<GS_ITEMMALL_FREE_TRADE_ACK>();
			SolRecruitDlg solRecruitDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLRECRUIT_DLG) as SolRecruitDlg;
			if (solRecruitDlg != null)
			{
				solRecruitDlg.SetRecruitButtonEnable(true);
			}
			if (TsPlatform.IsEditor)
			{
				TsLog.LogWarning(" GS_ITEMMALL_FREE_TRADE_ACK ==== [ {0} ] [ {1} ] ", new object[]
				{
					packet.m_nResult,
					packet.RecruitType
				});
			}
			NrTSingleton<ItemMallItemManager>.Instance.Trading = false;
			if (packet.m_nResult != 0)
			{
				NrTSingleton<NkClientLogic>.Instance.SetCanOpenTicket(true);
				int nResult = packet.m_nResult;
				if (nResult != -60)
				{
					if (nResult == -50)
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("507"));
					}
				}
				else
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"));
				}
				return;
			}
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo == null)
			{
				return;
			}
			SOLDIER_INFO sOLDIER_INFO = new SOLDIER_INFO();
			SOLDIER_TOTAL_INFO packet2 = kDeserializePacket.GetPacket<SOLDIER_TOTAL_INFO>();
			if (packet2.SOLINFO.SolPosType == 1)
			{
				NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo((int)packet2.SOLINFO.SolPosIndex);
				if (soldierInfo != null)
				{
					if (!soldierInfo.IsValid())
					{
						soldierInfo.Set(packet2.SOLINFO);
						soldierInfo.SetBattleSkillInfo(packet2.BATTLESKILLINFO);
					}
					soldierInfo.SetReceivedEquipItem(true);
					soldierInfo.UpdateSoldierStatInfo();
				}
			}
			else
			{
				NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
				readySolList.AddSolInfo(packet2.SOLINFO, packet2.BATTLESKILLINFO, true);
			}
			sOLDIER_INFO.Set(ref packet2.SOLINFO);
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			kMyCharInfo.SetCharSolGuide(packet2.SOLINFO.CharKind);
			for (int i = 0; i < packet.SolSubDataCount; i++)
			{
				SOLDIER_SUBDATA packet3 = kDeserializePacket.GetPacket<SOLDIER_SUBDATA>();
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet3.nSolID);
				if (soldierInfoFromSolID != null)
				{
					soldierInfoFromSolID.SetSolSubData(packet3.nSubDataType, packet3.nSubDataValue);
				}
			}
			NrReceiveGame.SolRecruitAfter(sOLDIER_INFO, null, 1, packet.RecruitType, null, false, null);
			kMyCharInfo.SetCharSubData(packet.SubDataType, packet.SubDataValue);
		}

		public static void GS_CHAR_CHALLENGE_EVENT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_CHAR_CHALLENGE_EVENT_ACK packet = kDeserializePacket.GetPacket<GS_CHAR_CHALLENGE_EVENT_ACK>();
			if (packet.i32Result == 0)
			{
				if (packet.i32MailType <= 0)
				{
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("157");
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						textFromNotify,
						"itemname",
						NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.i32ItemUnique),
						"count",
						packet.i32ItemNum
					});
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					ChallengeDlg challengeDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHALLENGE_DLG) as ChallengeDlg;
					if (challengeDlg != null)
					{
						NrTSingleton<ChallengeManager>.Instance.SetChallengeEventRewardInfo(packet.i16ChallengeUnique);
						NrTSingleton<ChallengeManager>.Instance.CalcEvnetRewardNoticeCount();
						challengeDlg.SetChallengeInfo(ChallengeManager.TYPE.EVENT);
					}
				}
				else if (packet.i32MailType == 141)
				{
					string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("601");
					string empty2 = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
					{
						textFromNotify2,
						"targetname",
						NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.i32ItemUnique),
						"count",
						packet.i32ItemNum
					});
					Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					ChallengeDlg challengeDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHALLENGE_DLG) as ChallengeDlg;
					if (challengeDlg2 != null)
					{
						NrTSingleton<ChallengeManager>.Instance.SetChallengeEventRewardInfo(packet.i16ChallengeUnique);
						NrTSingleton<ChallengeManager>.Instance.CalcEvnetRewardNoticeCount();
						challengeDlg2.SetChallengeInfo(ChallengeManager.TYPE.EVENT);
					}
				}
			}
		}

		public static void GS_CHAR_CHALLENGE_EVENT_REWARD_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_CHAR_CHALLENGE_EVENT_REWARD_ACK packet = kDeserializePacket.GetPacket<GS_CHAR_CHALLENGE_EVENT_REWARD_ACK>();
			short num = 0;
			for (short num2 = 0; num2 < 13; num2 += 1)
			{
				if (packet.i16ChallengeUnique[(int)num2] != 0)
				{
					num += 1;
				}
			}
			NrTSingleton<ChallengeManager>.Instance.SetEvnetRewardNoticeCount((int)num);
		}

		public static void GS_CHARACTER_SUBDATA_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_CHARACTER_SUBDATA_ACK packet = kDeserializePacket.GetPacket<GS_CHARACTER_SUBDATA_ACK>();
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo != null)
			{
				long i64Befordatavalue = 0L;
				for (int i = 0; i < packet.nDataCount; i++)
				{
					CHARACTER_SUBDATA packet2 = kDeserializePacket.GetPacket<CHARACTER_SUBDATA>();
					if (packet2.nSubDataType == 37)
					{
						i64Befordatavalue = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_VIP_EXP);
					}
					myCharInfo.SetCharSubData(packet2.nSubDataType, packet2.nSubDataValue);
					myCharInfo.ResultCharSubData(packet2.nSubDataType, packet2.nSubDataValue, i64Befordatavalue);
				}
			}
		}

		public static void GS_CHAR_DETAIL_INFO_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_CHAR_DETAIL_INFO_NFY packet = kDeserializePacket.GetPacket<GS_CHAR_DETAIL_INFO_NFY>();
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo != null)
			{
				for (int i = 0; i < packet.nCount; i++)
				{
					CHAR_DETAIL_INFO packet2 = kDeserializePacket.GetPacket<CHAR_DETAIL_INFO>();
					myCharInfo.SetCharDetail(packet2.nType, packet2.nValue);
					myCharInfo.ResultCharDetail(packet2.nType, packet2.nValue);
				}
			}
		}

		public static void GS_CHAR_MONTHDATA_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_CHAR_MONTHDATA_NFY packet = kDeserializePacket.GetPacket<GS_CHAR_MONTHDATA_NFY>();
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo != null)
			{
				for (int i = 0; i < packet.nCount; i++)
				{
					CHAR_MONTHDATA_INFO packet2 = kDeserializePacket.GetPacket<CHAR_MONTHDATA_INFO>();
					myCharInfo.SetCharMonthData(packet2.nType, packet2.nValue);
					int nType = packet2.nType;
					if (nType != 0)
					{
					}
				}
			}
		}

		public static void GS_CHAR_WEEKDATA_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_CHAR_WEEKDATA_NFY packet = kDeserializePacket.GetPacket<GS_CHAR_WEEKDATA_NFY>();
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo != null)
			{
				for (int i = 0; i < packet.nCount; i++)
				{
					CHAR_WEEKDATA_INFO packet2 = kDeserializePacket.GetPacket<CHAR_WEEKDATA_INFO>();
					myCharInfo.SetCharWeekData(packet2.nType, packet2.nValue);
					int nType = packet2.nType;
					if (nType == 0)
					{
						NrTSingleton<NrDailyGiftManager>.Instance.SetServerGroupUnique(packet2.nParamValue);
					}
				}
			}
		}

		public static void GS_CHAR_ACTIVITY_UPDATE_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_CHAR_ACTIVITY_UPDATE_NFY packet = kDeserializePacket.GetPacket<GS_CHAR_ACTIVITY_UPDATE_NFY>();
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.SetActivityPointMax(packet.ActivityPoint, packet.ActivityPointMax);
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.SetVipActivityAddTime(packet.VipActivityAddTime);
		}

		public static void GS_CHAR_FINDPATH_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_CHAR_FINDPATH_ACK packet = kDeserializePacket.GetPacket<GS_CHAR_FINDPATH_ACK>();
			NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(packet.CharUnique);
			if (charByCharUnique == null)
			{
				return;
			}
			charByCharUnique.m_kCharMove.ClearPath();
			if (GMCommand_Dlg.m_bShowNavPath)
			{
				charByCharUnique.m_kCharMove.ShowPath(false);
				charByCharUnique.m_kCharMove.m_NavPath.Add(new Vector3(packet.CurPos.x, packet.CurPos.y, packet.CurPos.z));
			}
			for (int i = 0; i < packet.PathCount; i++)
			{
				GS_CHAR_PATH packet2 = kDeserializePacket.GetPacket<GS_CHAR_PATH>();
				charByCharUnique.m_kCharMove.AddPath(new Vector3(packet2.Pos.x, packet2.Pos.y, packet2.Pos.z));
			}
			if (GMCommand_Dlg.m_bShowNavPath)
			{
				charByCharUnique.m_kCharMove.ShowPath(true);
			}
			if (!NrTSingleton<NkClientLogic>.Instance.IsMovable())
			{
				charByCharUnique.m_kCharMove.MoveStop(false, false);
				return;
			}
			charByCharUnique.m_kCharMove.ProcessCharMove(true);
		}

		public static void GS_CHAR_MOVE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_CHAR_MOVE_ACK packet = kDeserializePacket.GetPacket<GS_CHAR_MOVE_ACK>();
			NrReceiveGame.Update_GS_CHAR_MOVE_ACK(packet);
		}

		public static void GS_CHAR_GROUP_MOVE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_CHAR_GROUP_MOVE_ACK packet = kDeserializePacket.GetPacket<GS_CHAR_GROUP_MOVE_ACK>();
			for (int i = 0; i < (int)packet.MoveCount; i++)
			{
				CHAR_MOVE_INFO packet2 = kDeserializePacket.GetPacket<CHAR_MOVE_INFO>();
				NrReceiveGame.Update_GS_CHAR_MOVE_ACK(new GS_CHAR_MOVE_ACK
				{
					MoveType = packet2.MoveType,
					CharUnique = packet2.CharUnique,
					PosStart = packet2.PosStart,
					PosDest = packet2.PosDest,
					Speed = packet2.Speed
				});
			}
		}

		public static void GS_CHAR_LASTMOVE_CHECK_ACK(NkDeserializePacket kDeserializePacket)
		{
		}

		public static void GS_SOLDIERS_UPGRADE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLDIERS_UPGRADE_ACK packet = kDeserializePacket.GetPacket<GS_SOLDIERS_UPGRADE_ACK>();
			if (0 < packet.i32Result)
			{
				PlayerPrefs.SetString(NrPrefsKey.LATEST_SOLID, packet.i64BaseSolID.ToString());
				NkSoldierInfo nkSoldierInfo = null;
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (charPersonInfo != null)
				{
					nkSoldierInfo = charPersonInfo.GetSoldierInfoFromSolID(packet.i64BaseSolID);
				}
				if (nkSoldierInfo == null)
				{
					return;
				}
				short level = nkSoldierInfo.GetLevel();
				byte grade = nkSoldierInfo.GetGrade();
				int num = packet.i32Result;
				if (level < packet.i16Level && num == 2)
				{
					num = 3;
					short level2 = nkSoldierInfo.GetLevel();
					if (nkSoldierInfo.IsLeader())
					{
						nkSoldierInfo.SetLevel(packet.i16Level);
						if (level2 < 7 && packet.i16Level >= 7)
						{
							MenuIconDlg menuIconDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MENUICON_DLG) as MenuIconDlg;
							if (menuIconDlg != null)
							{
								menuIconDlg.ShowDownButton(true);
							}
						}
					}
					if (level2 < 15 && packet.i16Level >= 15)
					{
						NrTSingleton<MATEventManager>.Instance.MeasureEvent("Level15");
					}
				}
				nkSoldierInfo.SetExp(packet.i64Exp);
				nkSoldierInfo.SetLevel(packet.i16Level);
				nkSoldierInfo.SetGrade(packet.ui8Grade);
				nkSoldierInfo.SetSolSubData(3, packet.nEvolutionExp);
				nkSoldierInfo.SetSolSubData(5, (long)packet.i32TradeCount);
				NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
				if (kMyCharInfo == null)
				{
					return;
				}
				if (level != nkSoldierInfo.GetLevel())
				{
					if (NrTSingleton<NrCharKindInfoManager>.Instance.IsUserCharKind(nkSoldierInfo.GetCharKindInfo()) && NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
					{
						kMyCharInfo.SetActivityMax();
						if (kMyCharInfo.m_nActivityPoint < kMyCharInfo.m_nMaxActivityPoint)
						{
							kMyCharInfo.SetActivityPoint(kMyCharInfo.m_nMaxActivityPoint);
						}
					}
					if (nkSoldierInfo.IsLeader())
					{
						NrTSingleton<GameGuideManager>.Instance.Update(GameGuideCheck.LEVELUP);
						BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
						if (bookmarkDlg != null)
						{
							bookmarkDlg.SetBookmarkInfo();
						}
					}
				}
				SolComposeDirection solComposeDirection = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_DIRECTION_DLG) as SolComposeDirection;
				if (solComposeDirection == null)
				{
					return;
				}
				NkSoldierInfo nkSoldierInfo2 = null;
				if (packet.i8Cnt > 1)
				{
					solComposeDirection.SetImage(nkSoldierInfo, num);
				}
				for (int i = 0; i < (int)packet.i8Cnt; i++)
				{
					if (charPersonInfo != null)
					{
						nkSoldierInfo2 = charPersonInfo.GetSoldierInfoFromSolID(packet.i64BaseSolID);
					}
					if (packet.i8Cnt == 1 && nkSoldierInfo2 != null)
					{
						solComposeDirection.SetImage(nkSoldierInfo, nkSoldierInfo2, num);
					}
					NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
					NrTSingleton<ExplorationManager>.Instance.RemoveSolInfo(packet.i64SubSolID[i]);
					readySolList.DelSol(packet.i64SubSolID[i]);
				}
				SolComposeSuccessDlg solComposeSuccessDlg = (SolComposeSuccessDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_SUCCESS_DLG);
				if (solComposeSuccessDlg != null)
				{
					solComposeSuccessDlg.SetData(grade, (int)level, packet.i64AddExp, nkSoldierInfo, packet.nAddEvolutionExp, packet.nMaxLevelEvolution);
					solComposeSuccessDlg.Hide();
				}
				if (SolComposeMainDlg.Instance != null)
				{
					SolComposeMainDlg.Instance.CalcumData();
				}
				nkSoldierInfo.UpdateSoldierStatInfo();
				SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
				if (solMilitaryGroupDlg != null)
				{
					solMilitaryGroupDlg.RefereshSelectSolInfo(nkSoldierInfo);
					solMilitaryGroupDlg.RefreshSolList();
				}
				NrTSingleton<FiveRocksEventManager>.Instance.SolCompose((int)packet.i8Cnt);
				NrTSingleton<GameGuideManager>.Instance.RemoveEquipGuide();
				if (nkSoldierInfo.GetSolPosType() == 5)
				{
					NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.AddSolWarehouseInfo(nkSoldierInfo);
				}
			}
		}

		public static void GS_SOLDIERS_EXTRACT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLDIERS_EXTRACT_ACK packet = kDeserializePacket.GetPacket<GS_SOLDIERS_EXTRACT_ACK>();
			if (packet.i32Result == 0)
			{
				NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
				for (int i = 0; i < 10; i++)
				{
					if (packet.i64ExtractSolID[i] > 0L)
					{
						NrTSingleton<ExplorationManager>.Instance.RemoveSolInfo(packet.i64ExtractSolID[i]);
						readySolList.DelSol(packet.i64ExtractSolID[i]);
					}
				}
				SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
				if (solMilitaryGroupDlg != null)
				{
					solMilitaryGroupDlg.RefreshSolList();
				}
				NrTSingleton<GameGuideManager>.Instance.RemoveEquipGuide();
				SolComposeDirection solComposeDirection = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_DIRECTION_DLG) as SolComposeDirection;
				if (solComposeDirection == null)
				{
					return;
				}
				solComposeDirection.SetExtractData(packet.bGreat, packet.ExtractItemNum);
				if (SolComposeMainDlg.Instance != null)
				{
					SolComposeMainDlg.Instance.InitExtract();
				}
			}
			else if (packet.i32Result == -5)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			else if (packet.i32Result == -10)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			else if (packet.i32Result == -15)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"), SYSTEM_MESSAGE_TYPE.CAUTION_MESSAGE);
			}
		}

		public static void Update_GS_CHAR_MOVE_ACK(GS_CHAR_MOVE_ACK l_cMove)
		{
			if (l_cMove == null)
			{
				return;
			}
			NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(l_cMove.CharUnique);
			if (charByCharUnique != null)
			{
				if (l_cMove.PosDest.y < 0f || l_cMove.PosStart.y < 0f)
				{
					bool flag = !NrTSingleton<NkCharManager>.Instance.IsCharMoveEnable() || !charByCharUnique.IsReady3DModel();
					Debug.Log(string.Concat(new object[]
					{
						charByCharUnique.GetCharName(),
						":",
						(eAT2CharMoveType)l_cMove.MoveType,
						":Ready:",
						flag,
						",Des:",
						l_cMove.PosDest,
						",Start:",
						l_cMove.PosStart
					}));
				}
				switch (l_cMove.MoveType)
				{
				case 0:
					if (!NrTSingleton<NkCharManager>.Instance.IsCharMoveEnable() || !charByCharUnique.IsReady3DModel())
					{
						charByCharUnique.GetPersonInfo().SetCharPos(l_cMove.PosStart.x, l_cMove.PosStart.y, l_cMove.PosStart.z);
						Vector3 vector = new Vector3(l_cMove.PosDest.x - l_cMove.PosStart.x, 0f, l_cMove.PosDest.z - l_cMove.PosStart.z);
						charByCharUnique.GetPersonInfo().SetDirection(vector.x, vector.y, vector.z);
					}
					break;
				case 1:
					charByCharUnique.SetSpeed(l_cMove.Speed);
					if (charByCharUnique.GetID() != 1)
					{
						if (!NrTSingleton<NkCharManager>.Instance.IsCharMoveEnable() || !charByCharUnique.IsReady3DModel())
						{
							charByCharUnique.GetPersonInfo().SetCharPos(l_cMove.PosStart.x, l_cMove.PosStart.y, l_cMove.PosStart.z);
							Vector3 vector2 = new Vector3(l_cMove.PosDest.x - l_cMove.PosStart.x, 0f, l_cMove.PosDest.z - l_cMove.PosStart.z);
							charByCharUnique.GetPersonInfo().SetDirection(vector2.x, vector2.y, vector2.z);
							charByCharUnique.MoveTo(l_cMove.PosDest);
						}
						else
						{
							charByCharUnique.MoveTo(l_cMove.PosDest.x, l_cMove.PosDest.y, l_cMove.PosDest.z, false);
						}
					}
					break;
				case 2:
					if (!NrTSingleton<NkCharManager>.Instance.IsCharMoveEnable() || !charByCharUnique.IsReady3DModel())
					{
						charByCharUnique.GetPersonInfo().SetCharPos(l_cMove.PosStart.x, l_cMove.PosStart.y, l_cMove.PosStart.z);
						Vector3 vector3 = new Vector3(l_cMove.PosDest.x - l_cMove.PosStart.x, 0f, l_cMove.PosDest.z - l_cMove.PosStart.z);
						charByCharUnique.GetPersonInfo().SetDirection(vector3.x, vector3.y, vector3.z);
						charByCharUnique.MoveTo(l_cMove.PosDest);
					}
					else
					{
						Vector2 vector4 = new Vector2(charByCharUnique.m_k3DChar.GetRootGameObject().transform.position.x, charByCharUnique.m_k3DChar.GetRootGameObject().transform.position.z);
						Vector2 vector5 = new Vector2(l_cMove.PosStart.x, l_cMove.PosStart.z);
						float num = Vector2.Distance(vector4, vector5);
						if (num > 50f)
						{
							charByCharUnique.MoveToFast(l_cMove.PosStart, l_cMove.PosDest);
						}
						else if (num > 10f && NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
						{
							Debug.Log(string.Concat(new object[]
							{
								"MoveToFast =====> ",
								charByCharUnique.GetCharName(),
								" [",
								l_cMove.CharUnique,
								", ",
								num.ToString(),
								"] v2Pos: ",
								vector4,
								", v2NewPos: ",
								vector5,
								" Target : ",
								charByCharUnique.m_kCharMove.GetTargetPos(),
								" distan : ",
								num.ToString()
							}));
						}
					}
					break;
				case 3:
					charByCharUnique.m_kCharMove.MoveStop(true, false);
					charByCharUnique.SetLookAt(l_cMove.PosDest.x, l_cMove.PosDest.y, l_cMove.PosDest.z, false);
					break;
				case 4:
					charByCharUnique.m_kCharMove.MoveStop(true, false);
					break;
				case 5:
					if (!TsPlatform.IsMobile)
					{
						charByCharUnique.SetPosition(new Vector3(l_cMove.PosDest.x, l_cMove.PosDest.y, l_cMove.PosDest.z));
					}
					else
					{
						charByCharUnique.MoveToFast(l_cMove.PosStart, l_cMove.PosDest);
					}
					break;
				case 6:
					charByCharUnique.SetSpeed(l_cMove.Speed);
					charByCharUnique.m_kCharMove.MoveStop(true, false);
					charByCharUnique.SetPosition(new Vector3(l_cMove.PosStart.x, l_cMove.PosStart.y, l_cMove.PosStart.z));
					charByCharUnique.MoveTo(l_cMove.PosDest);
					break;
				}
			}
			else if (charByCharUnique == null)
			{
				switch (l_cMove.MoveType)
				{
				case 0:
				{
					NrReservedCharMakeInfo nrReservedCharMakeInfo = NrTSingleton<NkCharManager>.Instance.FindReservedChar(l_cMove.CharUnique);
					if (nrReservedCharMakeInfo != null)
					{
						nrReservedCharMakeInfo.MakeCharInfo.CharPos.x = l_cMove.PosStart.x;
						nrReservedCharMakeInfo.MakeCharInfo.CharPos.y = l_cMove.PosStart.y;
						nrReservedCharMakeInfo.MakeCharInfo.CharPos.z = l_cMove.PosStart.z;
						nrReservedCharMakeInfo.ReservedMoveTo.x = 0f;
						nrReservedCharMakeInfo.ReservedMoveTo.y = 0f;
						nrReservedCharMakeInfo.ReservedMoveTo.z = 0f;
					}
					break;
				}
				case 1:
				{
					NrReservedCharMakeInfo nrReservedCharMakeInfo2 = NrTSingleton<NkCharManager>.Instance.FindReservedChar(l_cMove.CharUnique);
					if (nrReservedCharMakeInfo2 != null)
					{
						nrReservedCharMakeInfo2.MakeCharInfo.CharPos.x = l_cMove.PosStart.x;
						nrReservedCharMakeInfo2.MakeCharInfo.CharPos.y = l_cMove.PosStart.y;
						nrReservedCharMakeInfo2.MakeCharInfo.CharPos.z = l_cMove.PosStart.z;
						nrReservedCharMakeInfo2.ReservedMoveTo.x = l_cMove.PosDest.x;
						nrReservedCharMakeInfo2.ReservedMoveTo.y = l_cMove.PosDest.y;
						nrReservedCharMakeInfo2.ReservedMoveTo.z = l_cMove.PosDest.z;
					}
					break;
				}
				case 2:
				{
					NrReservedCharMakeInfo nrReservedCharMakeInfo3 = NrTSingleton<NkCharManager>.Instance.FindReservedChar(l_cMove.CharUnique);
					if (nrReservedCharMakeInfo3 != null)
					{
						nrReservedCharMakeInfo3.MakeCharInfo.CharPos.x = l_cMove.PosStart.x;
						nrReservedCharMakeInfo3.MakeCharInfo.CharPos.y = l_cMove.PosStart.y;
						nrReservedCharMakeInfo3.MakeCharInfo.CharPos.z = l_cMove.PosStart.z;
						nrReservedCharMakeInfo3.ReservedMoveTo.x = l_cMove.PosDest.x;
						nrReservedCharMakeInfo3.ReservedMoveTo.y = l_cMove.PosDest.y;
						nrReservedCharMakeInfo3.ReservedMoveTo.z = l_cMove.PosDest.z;
					}
					break;
				}
				case 3:
				{
					NrReservedCharMakeInfo nrReservedCharMakeInfo4 = NrTSingleton<NkCharManager>.Instance.FindReservedChar(l_cMove.CharUnique);
					if (nrReservedCharMakeInfo4 != null)
					{
						nrReservedCharMakeInfo4.MakeCharInfo.Direction.x = l_cMove.PosDest.x;
						nrReservedCharMakeInfo4.MakeCharInfo.Direction.y = l_cMove.PosDest.y;
						nrReservedCharMakeInfo4.MakeCharInfo.Direction.z = l_cMove.PosDest.z;
					}
					break;
				}
				}
			}
		}

		public static void GS_SOLDIER_SUBDATA_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLDIER_SUBDATA_ACK packet = kDeserializePacket.GetPacket<GS_SOLDIER_SUBDATA_ACK>();
			if (packet.i32Result == 0)
			{
				bool flag = false;
				Dictionary<long, NkSoldierInfo> dictionary = new Dictionary<long, NkSoldierInfo>();
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (charPersonInfo != null)
				{
					eGROUPSUBDATATYPE i8Type;
					for (int i = 0; i < packet.nDataCount; i++)
					{
						SOLDIER_SUBDATA packet2 = kDeserializePacket.GetPacket<SOLDIER_SUBDATA>();
						NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet2.nSolID);
						if (soldierInfoFromSolID != null)
						{
							soldierInfoFromSolID.SetSolSubData(packet2.nSubDataType, packet2.nSubDataValue);
							eSOL_SUBDATA nSubDataType = (eSOL_SUBDATA)packet2.nSubDataType;
							if (nSubDataType != eSOL_SUBDATA.SOL_SUBDATA_STATUSVALUE)
							{
								if (nSubDataType != eSOL_SUBDATA.SOL_SUBDATA_FIGHTINGPOWER)
								{
									if (nSubDataType == eSOL_SUBDATA.SOL_SUBDATA_COSTUME)
									{
										NrTSingleton<CostumeWearManager>.Instance.Refresh(soldierInfoFromSolID, true, true);
										NrTSingleton<CostumeWearManager>.Instance.ShowCostumeChangeMsg(soldierInfoFromSolID);
										if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLCOMPOSE_MAIN_DLG))
										{
											if (SolComposeMainDlg.Instance.ContainSubSoldier(soldierInfoFromSolID.GetSolID()))
											{
												SolComposeMainDlg.Instance.MakeSubSolList();
											}
											if (SolComposeMainDlg.Instance.ContainExtractSoldier(soldierInfoFromSolID.GetSolID()))
											{
												SolComposeMainDlg.Instance.RefreshSelectExtract();
											}
										}
									}
								}
								else
								{
									NrTSingleton<CostumeWearManager>.Instance.Refresh(soldierInfoFromSolID, true, false);
								}
							}
							else
							{
								flag = true;
								NrTSingleton<NkBabelMacroManager>.Instance.SetRequestInjury(packet2.nSolID);
								NrTSingleton<NkCharManager>.Instance.ResetInjuryCureSolID();
							}
							i8Type = (eGROUPSUBDATATYPE)packet.i8Type;
							if (i8Type != eGROUPSUBDATATYPE.eGROUPSUBDATATYPE_AWAKENING)
							{
								if (i8Type == eGROUPSUBDATATYPE.eGROUPSUBDATATYPE_SOLDIERLOCK)
								{
									SolComposeListDlg solComposeListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMPOSE_LIST_DLG) as SolComposeListDlg;
									if (soldierInfoFromSolID.IsAtbCommonFlag(1L))
									{
										if (solComposeListDlg != null)
										{
											solComposeListDlg.RefreshSoldierInfo(soldierInfoFromSolID, true);
										}
										Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("722"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
									}
									else
									{
										if (solComposeListDlg != null)
										{
											solComposeListDlg.RefreshSoldierInfo(soldierInfoFromSolID, false);
										}
										Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("723"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
									}
									SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLDETAILINFO_DLG) as SolMilitaryGroupDlg;
									if (solMilitaryGroupDlg != null)
									{
										solMilitaryGroupDlg.RefreshSolInfo(soldierInfoFromSolID);
									}
									SolDetailinfoDlg solDetailinfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLDETAILINFO_DLG) as SolDetailinfoDlg;
									if (solDetailinfoDlg != null)
									{
										solDetailinfoDlg.SetSolder();
									}
								}
							}
							else
							{
								if (!dictionary.ContainsKey(soldierInfoFromSolID.GetSolID()))
								{
									dictionary.Add(soldierInfoFromSolID.GetSolID(), soldierInfoFromSolID);
								}
								SolMilitaryGroupDlg solMilitaryGroupDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
								if (solMilitaryGroupDlg2 != null)
								{
									solMilitaryGroupDlg2.RefreshEquipItem(soldierInfoFromSolID);
								}
							}
						}
					}
					if (flag)
					{
						NrTSingleton<NkLocalPushManager>.Instance.SetPush(eLOCAL_PUSH_TYPE.eLOCAL_PUSH_TYPE_INJURYTIME, 0L);
					}
					i8Type = (eGROUPSUBDATATYPE)packet.i8Type;
					if (i8Type != eGROUPSUBDATATYPE.eGROUPSUBDATATYPE_AWAKENING)
					{
						if (i8Type != eGROUPSUBDATATYPE.eGROUPSUBDATATYPE_SOLDIERLOCK)
						{
						}
					}
					else
					{
						SolMilitaryGroupDlg solMilitaryGroupDlg3 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
						foreach (NkSoldierInfo current in dictionary.Values)
						{
							current.Force_UpdateSoldierStatInfo();
							if (solMilitaryGroupDlg3 != null)
							{
								solMilitaryGroupDlg3.SetSoldierUpdate(current);
							}
						}
						SolAwakeningDlg solAwakeningDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLAWAKENING_DLG) as SolAwakeningDlg;
						if (solAwakeningDlg != null)
						{
							solAwakeningDlg.SetAwakening();
						}
					}
				}
			}
		}

		public static void GS_SOLDIER_INJURYCURE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLDIER_INJURYCURE_ACK packet = kDeserializePacket.GetPacket<GS_SOLDIER_INJURYCURE_ACK>();
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo != null)
			{
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet.nSolID);
				if (soldierInfoFromSolID == null)
				{
					return;
				}
				if (packet.nInjurySubData == 0L)
				{
					soldierInfoFromSolID.SetInjuryStatus(false);
					if (!NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
					{
						string empty = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("113"),
							"sol",
							soldierInfoFromSolID.GetName()
						});
						Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
						NrTSingleton<NkLocalPushManager>.Instance.SetPush(eLOCAL_PUSH_TYPE.eLOCAL_PUSH_TYPE_INJURYTIME, 0L);
						BabelLobbyUserListDlg babelLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWERUSERLIST_DLG) as BabelLobbyUserListDlg;
						if (babelLobbyUserListDlg != null)
						{
							babelLobbyUserListDlg.CheckInjurySoldierList();
						}
						BabelTower_FunctionDlg babelTower_FunctionDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWER_FUNCTION_DLG) as BabelTower_FunctionDlg;
						if (babelTower_FunctionDlg != null)
						{
							babelTower_FunctionDlg.CheckInjurySoldier();
						}
						SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
						if (solMilitaryGroupDlg != null)
						{
							solMilitaryGroupDlg.CheckInjurySoldierList();
						}
					}
				}
				else
				{
					soldierInfoFromSolID.SetInjuryStatus(true);
					soldierInfoFromSolID.SetSolSubData(0, packet.nInjurySubData);
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("127");
					Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
			}
		}

		public static void GS_SOLDIERS_SELL_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLDIERS_SELL_ACK packet = kDeserializePacket.GetPacket<GS_SOLDIERS_SELL_ACK>();
			if (packet.i32Result == 0)
			{
				for (int i = 0; i < (int)packet.i8Cnt; i++)
				{
					TsLog.Log(":SOLDIERS_SELL = {0}", new object[]
					{
						packet.i64SolID[i]
					});
					NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
					NrTSingleton<ExplorationManager>.Instance.RemoveSolInfo(packet.i64SolID[i]);
					readySolList.DelSol(packet.i64SolID[i]);
				}
				if (SolComposeMainDlg.Instance != null)
				{
					SolComposeMainDlg.Instance.CalcSellData();
				}
				SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
				if (solMilitaryGroupDlg != null)
				{
					solMilitaryGroupDlg.RefreshSolList();
				}
				NrTSingleton<FiveRocksEventManager>.Instance.SolSell((int)packet.i8Cnt);
				NrTSingleton<GameGuideManager>.Instance.RemoveEquipGuide();
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64AffterMoney;
				SolComposeMainDlg solComposeMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMPOSE_MAIN_DLG) as SolComposeMainDlg;
				if (solComposeMainDlg != null)
				{
					solComposeMainDlg.CalcSellData();
				}
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("845"),
					"count",
					ANNUALIZED.Convert(packet.i64SellMoney)
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			else
			{
				int i32Result = packet.i32Result;
				if (i32Result != -2)
				{
					TsLog.LogError("GS_SOLDIERS_SELL_ACK CNT : {0} Result : {1}", new object[]
					{
						packet.i8Cnt,
						packet.i32Result
					});
				}
				else
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("398"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
					NrTSingleton<GameGuideManager>.Instance.RemoveEquipGuide();
					NrTSingleton<GameGuideManager>.Instance.InitReserveGuide();
					NrTSingleton<GameGuideManager>.Instance.CheckGameGuide(GameGuideType.EQUIP_SELL);
				}
			}
		}

		public static void GS_CHARACTER_SUBCHARKIND_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_CHARACTER_SUBCHARKIND_SET_ACK packet = kDeserializePacket.GetPacket<GS_CHARACTER_SUBCHARKIND_SET_ACK>();
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(packet.i16CharUnique) as NrCharUser;
			if (nrCharUser == null)
			{
				return;
			}
			if (0 >= packet.i32SubCharKind)
			{
				nrCharUser.DeleteSubChar((int)packet.i8SubCharIndex);
				return;
			}
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(packet.i32SubCharKind);
			if (packet.i8Type == 3)
			{
				if (charKindInfo != null)
				{
					ECO_TALK ecoTalk = NrTSingleton<NrBaseTableManager>.Instance.GetEcoTalk(charKindInfo.GetCode());
					if (ecoTalk != null)
					{
						string textFromEco_Talk = NrTSingleton<NrTextMgr>.Instance.GetTextFromEco_Talk(ecoTalk.GetRandTalk());
						nrCharUser.SetSubCharKind(packet.i32SubCharKind, (int)packet.i8SubCharIndex, textFromEco_Talk);
					}
					else
					{
						nrCharUser.SetSubCharKind(packet.i32SubCharKind, (int)packet.i8SubCharIndex);
					}
				}
			}
			else
			{
				nrCharUser.SetSubCharKind(packet.i32SubCharKind, (int)packet.i8SubCharIndex);
			}
			if (nrCharUser.GetID() == 1 && charKindInfo != null)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("246");
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromNotify,
					"targetname",
					charKindInfo.GetName()
				});
				if (Scene.IsCurScene(Scene.Type.BATTLE))
				{
					NrTSingleton<NkQuestManager>.Instance.PushMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
				else
				{
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
			}
		}

		public static void GS_RECOMMEND_CUR_NFY(NkDeserializePacket kDeserializePacket)
		{
		}

		public static void GS_RECOMMEND_RECVADD_NFY(NkDeserializePacket kDeserializePacket)
		{
		}

		public static void GS_USER_SN_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_USER_SN_GET_ACK packet = kDeserializePacket.GetPacket<GS_USER_SN_GET_ACK>();
			if (packet.SN != 0L)
			{
				NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
				myCharInfo.m_SN = packet.SN;
			}
		}

		public static void GS_SUPPORTER_ADD_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_SUPPORTER_ADD_NFY packet = kDeserializePacket.GetPacket<GS_SUPPORTER_ADD_NFY>();
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo.m_PersonID == packet.i64PersonID)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("473");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromNotify, new object[]
				{
					textFromNotify,
					"targetname1",
					TKString.NEWString(packet.RecvName),
					"targetname2",
					NrTSingleton<NkCharManager>.Instance.GetCharName()
				});
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			}
		}

		public static void GS_TREASUREBOX_REWARD_DLG_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_TREASUREBOX_REWARD_DLG_NFY packet = kDeserializePacket.GetPacket<GS_TREASUREBOX_REWARD_DLG_NFY>();
			if (!NrTSingleton<ContentsLimitManager>.Instance.IsTreasure())
			{
				return;
			}
			TreasureBox_DLG treasureBox_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.TREASUREBOX_DLG) as TreasureBox_DLG;
			if (treasureBox_DLG != null)
			{
				treasureBox_DLG.SetRewardItem(packet);
			}
		}

		public static void GS_TREASUREBOX_GETREWARD_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_TREASUREBOX_GETREWARD_ACK packet = kDeserializePacket.GetPacket<GS_TREASUREBOX_GETREWARD_ACK>();
			if (!NrTSingleton<ContentsLimitManager>.Instance.IsTreasure())
			{
				return;
			}
			TreasureBox_DLG treasureBox_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.TREASUREBOX_DLG) as TreasureBox_DLG;
			if (treasureBox_DLG != null)
			{
				treasureBox_DLG.SendRewardItem(packet);
			}
		}

		public static void GS_TREASUREBOX_ALLDATA_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_TREASUREBOX_ALLDATA_NFY packet = kDeserializePacket.GetPacket<GS_TREASUREBOX_ALLDATA_NFY>();
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo != null)
			{
				myCharInfo.ClearTreasureMapData();
				for (int i = 0; i < packet.i32Count; i++)
				{
					TREASURE_MAPDATA packet2 = kDeserializePacket.GetPacket<TREASURE_MAPDATA>();
					myCharInfo.AddTreasureMapData(packet2.i32MapIndex);
				}
				if (packet.i32Count == 0)
				{
					GameGuideTreasureAlarm gameGuideTreasureAlarm = NrTSingleton<GameGuideManager>.Instance.GetGuide(GameGuideType.TREASURE_ALARM) as GameGuideTreasureAlarm;
					if (gameGuideTreasureAlarm != null)
					{
						gameGuideTreasureAlarm.SetInfo(string.Empty, 0, 0);
					}
				}
			}
		}

		public static void GS_TREASUREBOX_ALARM_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_TREASUREBOX_ALARM_NFY packet = kDeserializePacket.GetPacket<GS_TREASUREBOX_ALARM_NFY>();
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo == null)
			{
				return;
			}
			GameGuideTreasureAlarm gameGuideTreasureAlarm = NrTSingleton<GameGuideManager>.Instance.GetGuide(GameGuideType.TREASURE_ALARM) as GameGuideTreasureAlarm;
			if (gameGuideTreasureAlarm != null)
			{
				if (packet.i8ShowAlarm == 1)
				{
					string text = TKString.NEWString(packet.szCharName);
					TsLog.LogWarning("!!!!!!! {0}, {1}", new object[]
					{
						charPersonInfo.GetCharName(),
						text
					});
					if (charPersonInfo.GetCharName() != text)
					{
						gameGuideTreasureAlarm.SetInfo(text, packet.i16TreasureUnique, packet.i32Day);
						NoticeIconDlg.SetIcon(ICON_TYPE.GAMEGUIDE, true);
						NrTSingleton<GameGuideManager>.Instance.CheckGameGuide(GameGuideType.TREASURE_ALARM);
					}
				}
				else
				{
					gameGuideTreasureAlarm.SetInfo(string.Empty, 0, 0);
				}
			}
		}

		public static void GS_BOUNTYHUNT_INFO_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BOUNTYHUNT_INFO_SET_ACK packet = kDeserializePacket.GetPacket<GS_BOUNTYHUNT_INFO_SET_ACK>();
			if (packet.i32Result == 0 && 0 < packet.i16CurBountyHuntUnique)
			{
				bool flag = false;
				if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.BountyHuntUnique != packet.i16CurBountyHuntUnique)
				{
					flag = true;
				}
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.BountyHuntUnique = packet.i16CurBountyHuntUnique;
				if (flag)
				{
					NrTSingleton<BountyHuntManager>.Instance.AutoMoveClientNPC(packet.i16CurBountyHuntUnique);
				}
			}
		}

		public static void GS_BOUNTYHUNT_REWARD_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_BOUNTYHUNT_REWARD_NFY packet = kDeserializePacket.GetPacket<GS_BOUNTYHUNT_REWARD_NFY>();
			if (packet.i32Result == 0)
			{
				Battle_ResultDlg_Content battle_ResultDlg_Content = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_CONTENT_DLG) as Battle_ResultDlg_Content;
				if (battle_ResultDlg_Content != null)
				{
					battle_ResultDlg_Content.SetBountyReward(packet);
				}
				for (int i = 0; i < 5; i++)
				{
					if (0 < packet.i16ClearBountyHuntUnique[i])
					{
						NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.AddBountyHuntClearInfo(packet.i16ClearBountyHuntUnique[i], packet.i8ClearRank);
						NrTSingleton<BountyHuntManager>.Instance.ClearUpdateNPC();
					}
				}
				BountyHuntingDlg bountyHuntingDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOUNTYHUNTING_DLG) as BountyHuntingDlg;
				if (bountyHuntingDlg != null)
				{
					bountyHuntingDlg.RefreshInfo();
				}
				NoticeIconDlg.SetIcon(ICON_TYPE.POST, true);
			}
			else
			{
				string message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270") + packet.i32Result.ToString();
				Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}

		public static void GS_BOUNTYHUNT_INFO_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BOUNTYHUNT_INFO_GET_ACK packet = kDeserializePacket.GetPacket<GS_BOUNTYHUNT_INFO_GET_ACK>();
			NrTSingleton<BountyHuntManager>.Instance.CheckBountyHuntInfoNPCCharKind();
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.ClearBountyHuntClearInfo();
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.BountyHuntUnique = packet.i16CurBountyHuntUnique;
			for (int i = 0; i < (int)packet.i16Count; i++)
			{
				BOUNTYHUNT_CLEARINFO packet2 = kDeserializePacket.GetPacket<BOUNTYHUNT_CLEARINFO>();
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.AddBountyHuntClearInfo(packet2);
			}
			NrTSingleton<BountyHuntManager>.Instance.UpdateClientNpc(0);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BOUNTYCHECK_DLG);
			BountyHuntingDlg bountyHuntingDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOUNTYHUNTING_DLG) as BountyHuntingDlg;
			if (bountyHuntingDlg != null)
			{
				bountyHuntingDlg.SetData();
			}
		}

		public static void GS_BOUNTYHUNT_ACCEPT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BOUNTYHUNT_ACCEPT_ACK packet = kDeserializePacket.GetPacket<GS_BOUNTYHUNT_ACCEPT_ACK>();
			if (packet.i32Result == 0)
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.BountyHuntUnique = packet.i16CurBountyHuntUnique;
				NrTSingleton<BountyHuntManager>.Instance.UpdateClientNpc(0);
			}
		}

		public static void GS_BOUNTYHUNT_DETAILINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BOUNTYHUNT_DETAILINFO_ACK packet = kDeserializePacket.GetPacket<GS_BOUNTYHUNT_DETAILINFO_ACK>();
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BOUNTYHUNTING_DLG))
			{
				BountyHuntingDlg bountyHuntingDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BOUNTYHUNTING_DLG) as BountyHuntingDlg;
				if (bountyHuntingDlg != null)
				{
					bountyHuntingDlg.SetData();
				}
			}
		}

		public static void GS_CHAT_REPORT_USER_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_CHAT_REPORT_USER_ACK packet = kDeserializePacket.GetPacket<GS_CHAT_REPORT_USER_ACK>();
			string text = string.Empty;
			if (packet.i32Result == 0)
			{
				if (packet.i32TargetReportCount > 0)
				{
					int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CHATLIMIT_WARNNING);
					int value2 = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CHATLIMIT_REACH);
					if (value == packet.i32TargetReportCount)
					{
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("324");
					}
					else if (value2 == packet.i32TargetReportCount)
					{
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("325");
					}
					if (text != string.Empty)
					{
						MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
						if (msgBoxUI != null)
						{
							string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2577");
							string message = text;
							msgBoxUI.SetMsg(null, null, textFromInterface, message, eMsgType.MB_OK, 2);
						}
					}
				}
				else
				{
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("707");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						textFromNotify,
						"target",
						TKString.NEWString(packet.szTargetCharName)
					});
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
			else
			{
				int i32Result = packet.i32Result;
				switch (i32Result)
				{
				case 300:
				case 301:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("702");
					break;
				case 302:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("703");
					break;
				default:
					if (i32Result == -5)
					{
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("704");
					}
					break;
				}
				if (text != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
		}

		public static void GS_SOLDIEROPEN_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLDIEROPEN_INFO_ACK packet = kDeserializePacket.GetPacket<GS_SOLDIEROPEN_INFO_ACK>();
			string name = "[SoldierOpenInfo]";
			string msg = string.Empty;
			string text = string.Empty;
			msg = string.Format("HeartsRate0{0} Index {1} Adimg_INDEX : {2}", packet.i32HeartsRate, packet.i16Index, packet.i16SolRecruitImageIndex);
			NrTSingleton<ChatManager>.Instance.PushMsg(CHAT_TYPE.SYSTEM, name, msg);
			for (int i = 0; i < (int)packet.i16Count; i++)
			{
				SOLDIER_OPEN_INFO packet2 = kDeserializePacket.GetPacket<SOLDIER_OPEN_INFO>();
				switch (packet2.m_i16State)
				{
				case 0:
					text = "None";
					break;
				case 1:
					text = "Open";
					break;
				case 2:
					text = "End";
					break;
				}
				DateTime dueDate = PublicMethod.GetDueDate(packet2.m_SoldierOpenData.i64StartTime);
				DateTime dueDate2 = PublicMethod.GetDueDate(packet2.m_SoldierOpenData.i64EndTime);
				switch (packet2.m_SoldierOpenData.i16Type)
				{
				case 1:
					msg = string.Format("Index : {0} Type : {1}(Sol) : {2} : {3} : {4} : {5} ~ {6}", new object[]
					{
						packet2.m_SoldierOpenData.i16Index,
						packet2.m_SoldierOpenData.i16Type,
						text,
						TKString.NEWString(packet2.m_SoldierOpenData.strCharCode),
						packet2.m_SoldierOpenData.i32CharKind,
						dueDate.ToString(),
						dueDate2.ToString()
					});
					break;
				case 2:
					msg = string.Format("Index {0} : Type : {1}(ItemMall) : {2} : {3} : {4} ~ {5}", new object[]
					{
						packet2.m_SoldierOpenData.i16Index,
						packet2.m_SoldierOpenData.i16Type,
						text,
						packet2.m_SoldierOpenData.i64ItemMallIDX,
						dueDate.ToString(),
						dueDate2.ToString()
					});
					break;
				case 3:
					msg = string.Format("Index : {0} Type : {1}(Sol) : {2} : {3} : {4} : {5} ~ {6}", new object[]
					{
						packet2.m_SoldierOpenData.i16Index,
						packet2.m_SoldierOpenData.i16Type,
						text,
						TKString.NEWString(packet2.m_SoldierOpenData.strCharCode),
						packet2.m_SoldierOpenData.i32CharKind,
						dueDate.ToString(),
						dueDate2.ToString()
					});
					break;
				}
				NrTSingleton<ChatManager>.Instance.PushMsg(CHAT_TYPE.SYSTEM, name, msg);
			}
			msg = string.Format("HeartsRate0{0} Index {1} Adimg_INDEX : {2}", packet.i32HeartsRate, packet.i16Index, packet.i16SolRecruitImageIndex);
			NrTSingleton<ChatManager>.Instance.PushMsg(CHAT_TYPE.SYSTEM, name, msg);
			if (packet.i16Count == 0)
			{
				msg = "No Data";
				NrTSingleton<ChatManager>.Instance.PushMsg(CHAT_TYPE.SYSTEM, name, msg);
			}
		}

		public static void GS_CHARACTER_VOUCHER_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_CHARACTER_VOUCHER_GET_ACK packet = kDeserializePacket.GetPacket<GS_CHARACTER_VOUCHER_GET_ACK>();
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.ClearVoucherData();
			for (int i = 0; i < (int)packet.i16Count; i++)
			{
				VOUCHER_DATA packet2 = kDeserializePacket.GetPacket<VOUCHER_DATA>();
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.AddVoucherData(packet2);
			}
		}

		public static void GS_TRANSCENDENCS_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_TRANSCENDENCS_ACK packet = kDeserializePacket.GetPacket<GS_TRANSCENDENCS_ACK>();
			if (packet.i32Result == 0)
			{
				SolTranscendenceSuccess solTranscendenceSuccess = (SolTranscendenceSuccess)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLCOMPOSE_TRANSCENDENCE_DLG);
				if (solTranscendenceSuccess != null)
				{
					if (packet.i32ItemUnique > 0)
					{
						solTranscendenceSuccess.GetComposeTranscendence(false, packet.i32BaseSolKind, packet.i8BaseGrade, packet.i8UpgradeGrade, packet.i32SubSolKind, packet.i8SubGrade, packet.i32ItemNum, packet.i32CostumeUnique);
					}
					else
					{
						solTranscendenceSuccess.GetComposeTranscendence(true, packet.i32BaseSolKind, packet.i8BaseGrade, packet.i8UpgradeGrade, packet.i32SubSolKind, packet.i8SubGrade, packet.i32ItemNum, packet.i32CostumeUnique);
					}
				}
				NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (readySolList == null || charPersonInfo == null)
				{
					return;
				}
				readySolList.DelSol(packet.i64MaterialSolID);
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64CurrentMoney;
				NkSoldierInfo nkSoldierInfo = null;
				NrPersonInfoUser charPersonInfo2 = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (charPersonInfo2 != null)
				{
					nkSoldierInfo = charPersonInfo2.GetSoldierInfoFromSolID(packet.i64BaseSolID);
				}
				if (nkSoldierInfo != null)
				{
					nkSoldierInfo.SetSolStatus(packet.i8UpgradeSolStatus);
					nkSoldierInfo.SetGrade(packet.i8UpgradeGrade);
					nkSoldierInfo.SetLevel(packet.i16UpgradeLevel);
					nkSoldierInfo.SetExp(packet.i64UpgradeExp);
					nkSoldierInfo.SetSolPosType(packet.i8UpgradeSolPosType);
					nkSoldierInfo.UpdateSoldierStatInfo();
					if (nkSoldierInfo.GetSolPosType() == 5)
					{
						NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.AddSolWarehouseInfo(nkSoldierInfo);
					}
				}
				SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
				if (solMilitaryGroupDlg != null)
				{
					solMilitaryGroupDlg.RefreshSolList();
				}
			}
			else
			{
				if (packet.i32Result == -170)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("787"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				}
				else if (packet.i32Result == -190)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("793"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				}
				else if (packet.i32Result == -150)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("789"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				}
				else if (packet.i32Result == -155)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("790"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				}
				else if (packet.i32Result == -170)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("871"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				}
				else
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				}
				TsLog.LogError(" GS_TRANSCENDENCS_ACK Result 오류 {0}", new object[]
				{
					packet.i32Result
				});
			}
		}

		public static void GS_SOLCOMBINATIONLIMIT_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLCOMBINATIONLIMIT_INFO_ACK packet = kDeserializePacket.GetPacket<GS_SOLCOMBINATIONLIMIT_INFO_ACK>();
			for (int i = 0; i < (int)packet.i16Count; i++)
			{
				SOLDIER_OPEN_INFO packet2 = kDeserializePacket.GetPacket<SOLDIER_OPEN_INFO>();
				switch (packet2.m_i16State)
				{
				case 0:
				case 2:
					NrTSingleton<NrSolCombinationSkillInfoManager>.Instance.SetShowHide(packet2.m_SoldierOpenData.i32ItemUnique, 0);
					break;
				case 1:
					NrTSingleton<NrSolCombinationSkillInfoManager>.Instance.SetShowHide(packet2.m_SoldierOpenData.i32ItemUnique, 1);
					break;
				}
			}
			SolCombination_Dlg solCombination_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLCOMBINATION_DLG) as SolCombination_Dlg;
			if (solCombination_Dlg != null)
			{
				solCombination_Dlg.Show();
			}
		}

		public static void GS_CHARACTER_DAILYDUNGEON_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_CHARACTER_DAILYDUNGEON_SET_ACK packet = kDeserializePacket.GetPacket<GS_CHARACTER_DAILYDUNGEON_SET_ACK>();
			if (packet == null)
			{
				return;
			}
			DAILYDUNGEON_INFO dAILYDUNGEON_INFO = new DAILYDUNGEON_INFO();
			dAILYDUNGEON_INFO.m_i32DayOfWeek = packet.i32DayOfWeek;
			dAILYDUNGEON_INFO.m_i8Diff = packet.i8Diff;
			dAILYDUNGEON_INFO.m_i32ResetCount = packet.i32ResetCount;
			dAILYDUNGEON_INFO.m_i32IsClear = packet.i32IsClear;
			dAILYDUNGEON_INFO.m_i8IsReward = packet.i8IsReward;
			NrTSingleton<DailyDungeonManager>.Instance.AddDailyDungeonInfo(dAILYDUNGEON_INFO);
			if ((int)packet.i8IsReward == 1)
			{
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.DAILYDUNGEON_MAIN))
				{
					NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DAILYDUNGEON_MAIN);
				}
				string empty = string.Empty;
				EVENT_DAILY_DUNGEON_INFO dailyDungeonInfo = EVENT_DAILY_DUNGEON_DATA.GetInstance().GetDailyDungeonInfo(packet.i8Diff, (sbyte)packet.i32DayOfWeek);
				if (dailyDungeonInfo != null)
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("400"),
						"itemname",
						NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(dailyDungeonInfo.i32RewardItemUnique),
						"count",
						dailyDungeonInfo.i32RewardItemNum.ToString()
					});
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
			}
			else
			{
				DailyDungeon_Main_Dlg dailyDungeon_Main_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DAILYDUNGEON_MAIN) as DailyDungeon_Main_Dlg;
				if (dailyDungeon_Main_Dlg != null)
				{
					bool bCheck = false;
					if ((int)packet.i8IsReward == 1)
					{
						bCheck = true;
					}
					dailyDungeon_Main_Dlg.IsRewardCheck(bCheck);
				}
			}
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.DAILYDUNGEON_MSGBOX))
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DAILYDUNGEON_MSGBOX);
			}
			DailyDungeon_Select_Dlg dailyDungeon_Select_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DAILYDUNGEON_SELECT) as DailyDungeon_Select_Dlg;
			if (dailyDungeon_Select_Dlg != null)
			{
				dailyDungeon_Select_Dlg.SetData();
			}
			BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
			if (bookmarkDlg != null)
			{
				bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.ADVENTURE);
			}
			AdventureCollect_DLG adventureCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ADVENTURECOLLECT_DLG) as AdventureCollect_DLG;
			if (adventureCollect_DLG != null)
			{
				adventureCollect_DLG.Update_Notice();
			}
		}

		public static void GS_CHARACTER_DAILYDUNGEON_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_CHARACTER_DAILYDUNGEON_GET_ACK packet = kDeserializePacket.GetPacket<GS_CHARACTER_DAILYDUNGEON_GET_ACK>();
			if (packet == null || packet.i32Result != 0)
			{
				return;
			}
			NrTSingleton<DailyDungeonManager>.Instance.ClearDailyDungeon();
			for (int i = 0; i < 5; i++)
			{
				if (packet.i32DayOfWeek[i] != 0)
				{
					DAILYDUNGEON_INFO dAILYDUNGEON_INFO = new DAILYDUNGEON_INFO();
					dAILYDUNGEON_INFO.m_i32DayOfWeek = packet.i32DayOfWeek[i];
					dAILYDUNGEON_INFO.m_i8Diff = packet.i8Diff[i];
					dAILYDUNGEON_INFO.m_i32ResetCount = packet.i32ResetCount[i];
					dAILYDUNGEON_INFO.m_i32IsClear = packet.i32IsClear[i];
					dAILYDUNGEON_INFO.m_i8IsReward = packet.i8IsReward[i];
					NrTSingleton<DailyDungeonManager>.Instance.AddDailyDungeonInfo(dAILYDUNGEON_INFO);
				}
			}
			DailyDungeon_Select_Dlg dailyDungeon_Select_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DAILYDUNGEON_SELECT) as DailyDungeon_Select_Dlg;
			if (dailyDungeon_Select_Dlg != null)
			{
				dailyDungeon_Select_Dlg.SetData();
			}
		}

		public static void GS_CHAT_ACK(NkDeserializePacket kDeserializePacket)
		{
			if (!StageSystem.IsStable)
			{
				return;
			}
			GS_CHAT_ACK packet = kDeserializePacket.GetPacket<GS_CHAT_ACK>();
			using (new ScopeProfile("Size"))
			{
			}
			NrCharBase charByPersonID = NrTSingleton<NkCharManager>.Instance.GetCharByPersonID(packet.SenderPersonID);
			string text = TKString.NEWString(packet.SenderName);
			string text2 = TKString.NEWString(packet.Msg);
			if (packet.ChatType != 7 && packet.ChatType != 4 && packet.ChatType != 5)
			{
				NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
				if (nrCharUser != null)
				{
					NrPersonInfoBase personInfo = nrCharUser.GetPersonInfo();
					if (personInfo != null)
					{
						string charName = personInfo.GetCharName();
						if (!string.IsNullOrEmpty(charName) && text.Equals(charName))
						{
							return;
						}
					}
				}
			}
			string name = string.Empty;
			CHAT_TYPE chatType = (CHAT_TYPE)packet.ChatType;
			switch (chatType)
			{
			case CHAT_TYPE.BABELPARTY:
			{
				BabelTower_ChatDlg babelTower_ChatDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABELTOWER_CHAT) as BabelTower_ChatDlg;
				name = ChatManager.GetChatNameStr(text, packet.ColoseumGrade, true);
				if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BABELTOWER_CHAT))
				{
					BabelTower_FunctionDlg babelTower_FunctionDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWER_FUNCTION_DLG) as BabelTower_FunctionDlg;
					if (babelTower_FunctionDlg != null)
					{
						babelTower_FunctionDlg.m_bChatNew.Visible = true;
					}
				}
				if (babelTower_ChatDlg != null)
				{
					babelTower_ChatDlg.PushMsg(name, text2, packet.Color.ToString());
				}
				return;
			}
			case CHAT_TYPE.MYTHRAID:
			{
				Batch_Chat_DLG batch_Chat_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATCH_CAHT_DLG) as Batch_Chat_DLG;
				if (batch_Chat_DLG != null)
				{
					name = ChatManager.GetChatNameStr(text, packet.ColoseumGrade, true);
					batch_Chat_DLG.PushMsg(name, text2, (CHAT_TYPE)packet.ChatType);
				}
				return;
			}
			case CHAT_TYPE.WATCH:
			{
				IL_F5:
				if (chatType == CHAT_TYPE.NTGUILD)
				{
					Main_UI_SystemMessage.ADDMessage(text2, SYSTEM_MESSAGE_TYPE.ADMIN_SYSTEM_MESSAGE);
					NrTSingleton<ChatManager>.Instance.PushSystemMsg("GUILD", text2, "303");
					return;
				}
				if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG) == null)
				{
					return;
				}
				string chatNameStr = ChatManager.GetChatNameStr(text, packet.ColoseumGrade, true);
				NrTSingleton<ChatManager>.Instance.PushMsg((CHAT_TYPE)packet.ChatType, chatNameStr, text2);
				Batch_Chat_DLG batch_Chat_DLG2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATCH_CAHT_DLG) as Batch_Chat_DLG;
				if (batch_Chat_DLG2 != null && packet.ChatType == 1)
				{
					batch_Chat_DLG2.PushMsg(chatNameStr, text2, (CHAT_TYPE)packet.ChatType);
				}
				if (packet.SenderPersonID <= 0L || charByPersonID == null)
				{
					return;
				}
				NrTSingleton<ChatManager>.Instance.PushCharChat(charByPersonID, text2, (CHAT_TYPE)packet.ChatType);
				return;
			}
			case CHAT_TYPE.NUM:
				NrTSingleton<WhisperManager>.Instance.PushText(packet.RoomUnique, text, text2, (int)packet.Color, false);
				return;
			case CHAT_TYPE.NPC:
				ChatManager.NPCTellChat(text2);
				return;
			}
			goto IL_F5;
		}

		public static void GS_CHAT_DELETEROOM_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_CHAT_DELETEROOM_NFY packet = kDeserializePacket.GetPacket<GS_CHAT_DELETEROOM_NFY>();
			NrTSingleton<WhisperManager>.Instance.DelRoom(packet.RoomUnique);
		}

		public static void GS_CHAT_ADDUSER_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_CHAT_ADDUSER_NFY packet = kDeserializePacket.GetPacket<GS_CHAT_ADDUSER_NFY>();
			if (packet.Type != 7)
			{
				return;
			}
			for (int i = 0; i < packet.UserCount; i++)
			{
				CHAT_USERINFO packet2 = kDeserializePacket.GetPacket<CHAT_USERINFO>();
				string name = TKString.NEWString(packet2.CharName);
				NrTSingleton<WhisperManager>.Instance.AddUser(packet.RoomUnique, packet2.PersonID, name, packet2.PlayerState, packet2.FaceChakKind, packet.Request);
			}
			NrTSingleton<WhisperManager>.Instance.UpdateDlg(packet.RoomUnique);
			if (!NrTSingleton<WhisperManager>.Instance.MySendRequest)
			{
				WhisperRoom room = NrTSingleton<WhisperManager>.Instance.GetRoom(packet.RoomUnique);
				if (!NrTSingleton<WhisperManager>.Instance.WindowClose && room.GetUsers().Count == 2)
				{
					NrTSingleton<WhisperManager>.Instance.WindowClose = true;
				}
			}
			NrTSingleton<WhisperManager>.Instance.MySendRequest = false;
		}

		public static void GS_CHAT_DELUSER_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_CHAT_DELUSER_NFY packet = kDeserializePacket.GetPacket<GS_CHAT_DELUSER_NFY>();
			NrTSingleton<WhisperManager>.Instance.DelUser(packet.RoomUnique, packet.PersonID);
		}

		public static void GS_CHAT_JOINROOM_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_CHAT_JOINROOM_NFY packet = kDeserializePacket.GetPacket<GS_CHAT_JOINROOM_NFY>();
			NrTSingleton<ChatManager>.Instance.SetRoomUnique(packet.Type, packet.RoomUnique);
		}

		public static void GS_TALK_NUMBER_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_TALK_NUMBER_ACK packet = kDeserializePacket.GetPacket<GS_TALK_NUMBER_ACK>();
			if (!NrTSingleton<NkClientLogic>.Instance.IsWorldScene())
			{
				return;
			}
			NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(packet.i16CharUnique);
			if (charByCharUnique != null && packet.i16TextNumber > 0)
			{
				string text = TKString.NEWString(packet.Name);
				if (text == string.Empty)
				{
					string textFromEco_Talk = NrTSingleton<NrTextMgr>.Instance.GetTextFromEco_Talk(packet.i16TextNumber.ToString());
					charByCharUnique.MakeChatText(textFromEco_Talk, true);
				}
				else
				{
					string textFromEco_Talk2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromEco_Talk(packet.i16TextNumber.ToString());
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						textFromEco_Talk2,
						"targetname",
						text
					});
					charByCharUnique.MakeChatText(empty, true);
				}
			}
		}

		public static void GS_WHISPER_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_WHISPER_ACK packet = kDeserializePacket.GetPacket<GS_WHISPER_ACK>();
			WhisperRoom room = NrTSingleton<WhisperManager>.Instance.GetRoom(packet.RoomUnique);
			if (room != null)
			{
				NrTSingleton<WhisperManager>.Instance.WindowClose = false;
				NrTSingleton<WhisperManager>.Instance.ChangeRoom(packet.RoomUnique);
				NrTSingleton<WhisperManager>.Instance.ShowWhisperDlg();
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.WHISPER_DLG);
			}
		}

		public static void GS_WHISPER_INVITE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_WHISPER_INVITE_ACK packet = kDeserializePacket.GetPacket<GS_WHISPER_INVITE_ACK>();
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			string text = string.Empty;
			string empty = string.Empty;
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("47");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"username",
				TKString.NEWString(packet.sSendInvitePersonName)
			});
			if (msgBoxUI != null)
			{
				msgBoxUI.SetMsg(new YesDelegate(NrTSingleton<WhisperManager>.Instance.WhisperApplyYesDelegate), packet, new NoDelegate(NrTSingleton<WhisperManager>.Instance.WhisperApplyNoDelegate), packet, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("542"), empty, eMsgType.MB_OK_CANCEL);
			}
		}

		public static void GS_WHISPER_RESULTMSG_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_WHISPER_RESULTMSG_ACK packet = kDeserializePacket.GetPacket<GS_WHISPER_RESULTMSG_ACK>();
			string text = TKString.NEWString(packet.Name);
			string empty = string.Empty;
			if (packet.i16Result == 0)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("466"),
					"username",
					text
				});
				Main_UI_SystemMessage.ADDMessage(empty);
			}
			else if (packet.i16Result == 1)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("68"),
					"username",
					text
				});
				NrTSingleton<WhisperManager>.Instance.PushText(packet.i32RoomUnique, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94"), empty, 112, true);
			}
			else if (packet.i16Result == 2)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("69"),
					"username",
					text
				});
				NrTSingleton<WhisperManager>.Instance.PushText(packet.i32RoomUnique, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94"), empty, 112, true);
			}
			else if (packet.i16Result == 3)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("65"),
					"username",
					text
				});
				Main_UI_SystemMessage.ADDMessage(empty);
			}
			else if (packet.i16Result == 4)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("568"),
					"targetname",
					text
				});
				Main_UI_SystemMessage.ADDMessage(empty);
			}
			else if (packet.i16Result == 5)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("567"),
					"targetname",
					text
				});
				Main_UI_SystemMessage.ADDMessage(empty);
			}
			else if (packet.i16Result == 6)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("570"));
			}
		}

		public static void GS_WHISPER_STATE_CHANGE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_WHISPER_STATE_CHANGE_ACK packet = kDeserializePacket.GetPacket<GS_WHISPER_STATE_CHANGE_ACK>();
			NrTSingleton<WhisperManager>.Instance.SetUserStateChange(packet.nPersonID, packet.nRoomUnique, packet.i8UserChatState);
			TsLog.LogWarning("ACK.nPersonID = {0}, ACK.nRoomUnique == {1}, ACK.i8UserChatState = {2}, MyPersonID={3}", new object[]
			{
				packet.nPersonID,
				packet.nRoomUnique,
				packet.i8UserChatState,
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID
			});
		}

		public static void GS_TREASUREBOX_MOVE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_TREASUREBOX_MOVE_ACK packet = kDeserializePacket.GetPacket<GS_TREASUREBOX_MOVE_ACK>();
			if (packet.i32Result != 0)
			{
				if (packet.i32Result == -1)
				{
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("632");
					Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				else
				{
					string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("634");
					Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					TsLog.LogWarning("!!!!!!! Find DB Error{0}", new object[]
					{
						packet.i32Result
					});
				}
			}
			else
			{
				if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo() == null)
				{
					return;
				}
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.STORYCHAT_DLG))
				{
					StoryChatDlg storyChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHAT_DLG) as StoryChatDlg;
					if (storyChatDlg != null)
					{
						storyChatDlg.Close();
					}
				}
				if (packet.i32DestMapIndex == 0 && packet.fDestX == 0f && packet.fDestY == 0f && packet.fDestZ == 0f)
				{
					string textFromNotify3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("634");
					Main_UI_SystemMessage.ADDMessage(textFromNotify3, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				else
				{
					NrTSingleton<NkQuestManager>.Instance.TreadureFindPath(packet.i32DestMapIndex, packet.fDestX, packet.fDestY, packet.fDestZ);
				}
			}
		}

		public static void GS_TREASUREBOX_CLICK_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_TREASUREBOX_CLICK_ACK packet = kDeserializePacket.GetPacket<GS_TREASUREBOX_CLICK_ACK>();
			if (packet.i32Result != 0)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("634");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				TsLog.LogWarning("!!!!!!! Find DB Error{0}", new object[]
				{
					packet.i32Result
				});
			}
		}

		public static void GS_COLLECT_START_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_COLLECT_START_ACK packet = kDeserializePacket.GetPacket<GS_COLLECT_START_ACK>();
			string text = string.Empty;
			if (packet.i8Result == 0 || packet.i8Result == 1)
			{
				bool success = false;
				if (packet.i8Result == 0)
				{
					success = true;
				}
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_COLLECT);
				Dlg_Collect dlg_Collect = (Dlg_Collect)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_COLLECT);
				if (dlg_Collect != null)
				{
					dlg_Collect.CollectStart(packet.i32CharUnique, packet.i32PosX, packet.i32PosY, success);
				}
			}
			else if (packet.i8Result != 70)
			{
				if (packet.i8Result == 68)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("329");
				}
				else if (packet.i8Result == 69)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("328");
				}
			}
			if (text.Length > 0)
			{
				Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
		}

		public static void GS_COLLECT_CANCEL_ACK(NkDeserializePacket kDeserializePacket)
		{
			if (kDeserializePacket.GetPacket<GS_COLLECT_CANCEL_ACK>() == null)
			{
				return;
			}
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_COLLECT);
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			if (@char != null)
			{
				@char.DelCharState(512L);
				@char.SetAnimationFromState();
			}
		}

		public static void GS_COLLECT_FINISH_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_COLLECT_FINISH_ACK packet = kDeserializePacket.GetPacket<GS_COLLECT_FINISH_ACK>();
			NrCharObject nrCharObject = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique((short)packet.i32CharUnique) as NrCharObject;
			if (nrCharObject == null)
			{
				return;
			}
			NrCharKindInfo charKindInfo = nrCharObject.GetCharKindInfo();
			if (charKindInfo == null)
			{
				return;
			}
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_COLLECT);
			if (packet.i32Result == 0)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("320");
				string empty = string.Empty;
				string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.itemunique);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromNotify,
					"collectname",
					charKindInfo.GetName(),
					"collectitem",
					itemNameByItemUnique,
					"itemnum",
					packet.itemnum.ToString()
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "COLLECT", "SUCCESS", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			}
		}

		public static void GS_COLOSSEUM_RECORD_LIST_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_COLOSSEUM_RECORD_LIST_GET_ACK packet = kDeserializePacket.GetPacket<GS_COLOSSEUM_RECORD_LIST_GET_ACK>();
			ColosseumBattleRecordDlg colosseumBattleRecordDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUM_BATTLE_RECORD_DLG) as ColosseumBattleRecordDlg;
			if (colosseumBattleRecordDlg != null)
			{
				for (int i = 0; i < packet.Recordcount; i++)
				{
					COLOSSEUM_RECORDINFO packet2 = kDeserializePacket.GetPacket<COLOSSEUM_RECORDINFO>();
					colosseumBattleRecordDlg.AddRecordInfo(packet2);
				}
				colosseumBattleRecordDlg.Show();
			}
		}

		public static void GS_COLOSSEUM_START_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_COLOSSEUM_START_ACK packet = kDeserializePacket.GetPacket<GS_COLOSSEUM_START_ACK>();
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (packet.i32Result == 0)
			{
				if (packet.byMode == 0)
				{
					myCharInfo.ColosseumMatching = true;
					NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.COLOSSEUMNOTICE_DLG);
				}
				else if (packet.byMode == 1)
				{
					myCharInfo.ColosseumMatching = false;
					myCharInfo.Tournament = false;
					NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.COLOSSEUMNOTICE_DLG);
				}
				else if (packet.byMode == 2)
				{
					if (!myCharInfo.ColosseumMatching)
					{
						myCharInfo.ColosseumMatching = true;
					}
					NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.COLOSSEUM_CHALLENGE_DLG);
				}
				else if (packet.byMode == 3)
				{
					myCharInfo.ColosseumMatching = false;
					myCharInfo.Tournament = false;
					NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.COLOSSEUMNOTICE_DLG);
				}
				else if (packet.byMode == 4)
				{
					myCharInfo.ColosseumMatching = true;
					myCharInfo.Tournament = true;
					NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.COLOSSEUMNOTICE_DLG);
				}
			}
			else if (packet.byMode == 0)
			{
				NrLoadPageScreen.ShowHideLoadingImg(false);
				myCharInfo.ColosseumMatching = true;
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.COLOSSEUMNOTICE_DLG);
			}
			else
			{
				NrLoadPageScreen.ShowHideLoadingImg(false);
			}
		}

		public static void GS_COLOSSEUM_RANKINFO_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_COLOSSEUM_RANKINFO_GET_ACK packet = kDeserializePacket.GetPacket<GS_COLOSSEUM_RANKINFO_GET_ACK>();
			if (packet.i32Result == 0)
			{
				ColosseumRankInfoDlg colosseumRankInfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUMRANKINFO_DLG) as ColosseumRankInfoDlg;
				colosseumRankInfoDlg.SetTopRankGuild(packet.TopGuildRank);
				for (int i = 0; i < packet.count; i++)
				{
					COLOSSEUM_RANKINFO packet2 = kDeserializePacket.GetPacket<COLOSSEUM_RANKINFO>();
					colosseumRankInfoDlg.AddRankInfo(packet2);
				}
				colosseumRankInfoDlg.ShowInfo((eCOLOSSEUMRANK_SHOWTYPE)packet.byRank_GetType, packet.i32Page);
			}
			else if (packet.i32Result == -1)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("144"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
		}

		public static void GS_COLOCSSEUM_MYGRADE_USERINFO_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_COLOCSSEUM_MYGRADE_USERINFO_NFY packet = kDeserializePacket.GetPacket<GS_COLOCSSEUM_MYGRADE_USERINFO_NFY>();
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo == null)
			{
				return;
			}
			myCharInfo.Colosseum_MyGrade_Userinfo_Init();
			for (int i = 0; i < packet.i32InfoCount; i++)
			{
				COLOSSEUM_MYGRADE_USERINFO packet2 = kDeserializePacket.GetPacket<COLOSSEUM_MYGRADE_USERINFO>();
				myCharInfo.AddColosseum_MyGrade_UserInfo(packet2);
			}
			myCharInfo.Colosseum_MyGrade_Sort();
			NrTSingleton<GameGuideManager>.Instance.CheckGameGuide(GameGuideType.COLOSSEUM);
		}

		public static void GS_COLOSSEUM_MY_GRADEPOINT_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_COLOSSEUM_MY_GRADEPOINT_NFY packet = kDeserializePacket.GetPacket<GS_COLOSSEUM_MY_GRADEPOINT_NFY>();
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo == null)
			{
				return;
			}
			myCharInfo.UpdateColosseumGradePoint(packet.i32ColosseumGradePoint);
			myCharInfo.ColosseumWinCount = packet.i32ColosseumWinCount;
		}

		public static void GS_COLOSSEUM_SUPPORTSOL_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_COLOSSEUM_SUPPORTSOL_INFO_ACK packet = kDeserializePacket.GetPacket<GS_COLOSSEUM_SUPPORTSOL_INFO_ACK>();
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.ClearColoseumSupportSoldier();
			for (int i = 0; i < (int)packet.i16SolCount; i++)
			{
				COLOSSEUM_SUPPORTSOLDIER packet2 = kDeserializePacket.GetPacket<COLOSSEUM_SUPPORTSOLDIER>();
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.AddColoseumSupportSoldier(packet2);
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(packet2.i32CharKind);
				if (charKindInfo != null)
				{
				}
			}
			if (!Battle.Replay && !Battle.BATTLE.Observer)
			{
				if (Battle.BATTLE.ChangeSolCount < Battle.BATTLE.BattleInfo.m_MAXCHANGESOLDIERNUM && Battle.BATTLE.CurrentTurnAlly == Battle.BATTLE.MyAlly)
				{
					Battle.BATTLE.ShowColosseumSummonEffect = false;
					if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_EMERGENCY_CALL_DLG))
					{
						NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.BATTLE_EMERGENCY_CALL_DLG);
					}
				}
				else
				{
					Battle.BATTLE.ShowColosseumSummonEffect = true;
					Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_EMERGENCY_CALL_DLG);
					if (form != null)
					{
						form.Close();
					}
				}
			}
			else if (Battle.BATTLE.Observer)
			{
				ColosseumObserverSummonDlg colosseumObserverSummonDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUM_OBSERVER_SUMMON_DLG) as ColosseumObserverSummonDlg;
				if (colosseumObserverSummonDlg != null)
				{
					colosseumObserverSummonDlg.SetDialgPos((short)packet.i8Ally);
				}
			}
		}

		public static void GS_COLOSSEUM_BATCH_SOLDIERLIST_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_COLOSSEUM_BATCH_SOLDIERLIST_ACK packet = kDeserializePacket.GetPacket<GS_COLOSSEUM_BATCH_SOLDIERLIST_ACK>();
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.ClearColosseumEnableBatchSoldierKind();
			for (int i = 0; i < (int)packet.i16SolCount; i++)
			{
				GS_COLOSSEUM_BATCH_SOLDIERLIST_KIND packet2 = kDeserializePacket.GetPacket<GS_COLOSSEUM_BATCH_SOLDIERLIST_KIND>();
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(packet2.i32SolKind);
				if (charKindInfo != null)
				{
					NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.AddColosseumEnableBatchsoldierKind(packet2.i32SolKind);
				}
			}
			ColosseumDlg colosseumDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUMMAIN_DLG) as ColosseumDlg;
			if (colosseumDlg != null)
			{
				colosseumDlg.bReceiveBatchList = true;
				colosseumDlg.Show();
			}
		}

		public static void GS_TOURNAMENT_START_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_TOURNAMENT_START_ACK packet = kDeserializePacket.GetPacket<GS_TOURNAMENT_START_ACK>();
			TournamentMasterDlg tournamentMasterDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TOURNAMENT_MASTER_DLG) as TournamentMasterDlg;
			if (tournamentMasterDlg != null)
			{
				tournamentMasterDlg.SetStart(packet.bStart);
			}
		}

		public static void GS_TOURNAMENT_MATCHLIST_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_TOURNAMENT_MATCHLIST_ACK packet = kDeserializePacket.GetPacket<GS_TOURNAMENT_MATCHLIST_ACK>();
			TournamentMasterDlg tournamentMasterDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TOURNAMENT_MASTER_DLG) as TournamentMasterDlg;
			if (tournamentMasterDlg != null)
			{
				tournamentMasterDlg.SetStart(packet.bStart);
				for (int i = 0; i < packet.i32Count; i++)
				{
					GS_TOURNAMENT_MATCHLIST_INFO packet2 = kDeserializePacket.GetPacket<GS_TOURNAMENT_MATCHLIST_INFO>();
					tournamentMasterDlg.SetMatchList(packet2);
				}
				tournamentMasterDlg.SetShowList();
			}
		}

		public static void GS_TOURNAMENT_PLAYER_STATE_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_TOURNAMENT_PLAYER_STATE_NFY packet = kDeserializePacket.GetPacket<GS_TOURNAMENT_PLAYER_STATE_NFY>();
			TournamentMasterDlg tournamentMasterDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TOURNAMENT_MASTER_DLG) as TournamentMasterDlg;
			if (tournamentMasterDlg != null)
			{
				tournamentMasterDlg.ChangePlayerState(packet);
			}
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo.Tournament && (packet.nPlayerState == 3 || packet.nPlayerState == 2))
			{
				ColosseumNoticeDlg colosseumNoticeDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUMNOTICE_DLG) as ColosseumNoticeDlg;
				if (colosseumNoticeDlg != null)
				{
					colosseumNoticeDlg.SetReady(packet);
				}
			}
			if (packet.nPlayerState == 10)
			{
				ColosseumNoticeDlg colosseumNoticeDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUMNOTICE_DLG) as ColosseumNoticeDlg;
				if (colosseumNoticeDlg2 != null)
				{
					colosseumNoticeDlg2.Close();
				}
				TournamentLobbyDlg tournamentLobbyDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TOURNAMENT_LOBBY_DLG) as TournamentLobbyDlg;
				if (tournamentLobbyDlg != null)
				{
					tournamentLobbyDlg.Close();
				}
			}
		}

		public static void GS_TOURNAMENT_JOIN_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_TOURNAMENT_JOIN_ACK packet = kDeserializePacket.GetPacket<GS_TOURNAMENT_JOIN_ACK>();
			if (packet.nResult != 0)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("733"));
			}
		}

		public static void GS_TOURNAMENT_LOBBY_INFO_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_TOURNAMENT_LOBBY_INFO_NFY packet = kDeserializePacket.GetPacket<GS_TOURNAMENT_LOBBY_INFO_NFY>();
			for (int i = 0; i < packet.nSoldierCount; i++)
			{
				GS_COLOSSEUM_BATCH_SOLDIERLIST_KIND packet2 = kDeserializePacket.GetPacket<GS_COLOSSEUM_BATCH_SOLDIERLIST_KIND>();
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(packet2.i32SolKind);
				if (charKindInfo != null)
				{
					NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.AddColosseumEnableBatchsoldierKind(packet2.i32SolKind);
				}
			}
			if (!(NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TOURNAMENT_LOBBY_DLG) is TournamentLobbyDlg))
			{
				TournamentLobbyDlg tournamentLobbyDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.TOURNAMENT_LOBBY_DLG) as TournamentLobbyDlg;
				tournamentLobbyDlg.LobbyIndex = packet.nLobbyIndex;
				tournamentLobbyDlg.SetUserName(TKString.NEWString(packet.szPlayer1), TKString.NEWString(packet.szPlayer2));
				tournamentLobbyDlg.SetCharUnqiue(packet.nPlayerUnique1, packet.nPlayerUnique2);
				tournamentLobbyDlg.SetSolList();
			}
		}

		public static void GS_TOURNAMENT_LOBBY_STEP_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_TOURNAMENT_LOBBY_STEP_NFY packet = kDeserializePacket.GetPacket<GS_TOURNAMENT_LOBBY_STEP_NFY>();
			TournamentLobbyDlg tournamentLobbyDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TOURNAMENT_LOBBY_DLG) as TournamentLobbyDlg;
			if (tournamentLobbyDlg != null)
			{
				tournamentLobbyDlg.SetStep((eTMLOBBY_STEP)packet.eLobbyStep, (short)packet.nActiveAlly);
				tournamentLobbyDlg.m_bUpdate = true;
			}
		}

		public static void GS_TOURNAMENT_LOBBY_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_TOURNAMENT_LOBBY_SET_ACK packet = kDeserializePacket.GetPacket<GS_TOURNAMENT_LOBBY_SET_ACK>();
			TournamentLobbyDlg tournamentLobbyDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TOURNAMENT_LOBBY_DLG) as TournamentLobbyDlg;
			if (tournamentLobbyDlg != null)
			{
				tournamentLobbyDlg.SetData(packet);
			}
		}

		public static void GS_DECLAREWAR_LIST_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_DECLAREWAR_LIST_ACK packet = kDeserializePacket.GetPacket<GS_DECLAREWAR_LIST_ACK>();
			NewGuildMainDlg newGuildMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_MAIN_DLG) as NewGuildMainDlg;
			if (newGuildMainDlg == null)
			{
				return;
			}
			newGuildMainDlg.ClearDeclareWarList();
			string targetName = TKString.NEWString(packet.strTargetGuildName);
			for (int i = 0; i < (int)packet.i16Count; i++)
			{
				GUILD_NAME packet2 = kDeserializePacket.GetPacket<GUILD_NAME>();
				newGuildMainDlg.SetDeclareWarInfo(TKString.NEWString(packet2.strGuildName), targetName);
			}
			newGuildMainDlg.SetDeclareWarInfoEnd();
		}

		public static string GetStrDayToWeek(int day)
		{
			string result = string.Empty;
			switch (day)
			{
			case 0:
				result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2243");
				break;
			case 1:
				result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2244");
				break;
			case 2:
				result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2245");
				break;
			case 3:
				result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2246");
				break;
			case 4:
				result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2247");
				break;
			case 5:
				result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2248");
				break;
			case 6:
				result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2249");
				break;
			}
			return result;
		}

		public static void GS_DECLAREWAR_SET_TARGET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_DECLAREWAR_SET_TARGET_ACK packet = kDeserializePacket.GetPacket<GS_DECLAREWAR_SET_TARGET_ACK>();
			string text = TKString.NEWString(packet.strEnemyGuildName);
			if (packet.i32Result == 0)
			{
				string empty = string.Empty;
				if (packet.i64GuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("782"),
						"targetname",
						text
					});
				}
				else
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("821"),
						"targetname",
						text
					});
				}
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				NewGuildMainDlg newGuildMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWGUILD_MAIN_DLG) as NewGuildMainDlg;
				if (newGuildMainDlg != null)
				{
					newGuildMainDlg.SetIsDeclareWarTarget(true);
					newGuildMainDlg.SetButton();
					newGuildMainDlg.Send_GS_DECLAREWAR_LIST_REQ();
				}
			}
			else if (packet.i32Result == 9500)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("791"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			else if (packet.i32Result == 9550)
			{
				string empty2 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("823"),
					"targetname",
					text
				});
				Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			else if (packet.i32Result == 9551)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("791"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			else if (packet.i32Result == 9560)
			{
				if (MINE_CONSTANT_Manager.GetInstance() == null)
				{
					string message = string.Format("Warning Result {0}", packet.i32Result);
					Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
					return;
				}
				string strDayToWeek = NrReceiveGame.GetStrDayToWeek((int)packet.bDaclareDay);
				string strDayToWeek2 = NrReceiveGame.GetStrDayToWeek((int)packet.bDeclareWarStartDay);
				string empty3 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("794"),
					"day1",
					strDayToWeek,
					"hour1",
					string.Format("{0:D2}", packet.bDaclareHour),
					"min1",
					string.Format("{0:D2}", packet.bDaclareMin),
					"day2",
					strDayToWeek2,
					"hour2",
					string.Format("{0:D2}", packet.bDeclareWarStartHour),
					"min2",
					string.Format("{0:D2}", packet.bDeclareWarStartMin)
				});
				Main_UI_SystemMessage.ADDMessage(empty3, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(string.Format("result {0}", packet.i32Result), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
			}
		}

		public static void GS_DECLAREWAR_CANCEL_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_DECLAREWAR_CANCEL_ACK packet = kDeserializePacket.GetPacket<GS_DECLAREWAR_CANCEL_ACK>();
			NewGuildMainDlg newGuildMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWGUILD_MAIN_DLG) as NewGuildMainDlg;
			if (packet.i32Result == 0)
			{
				if (newGuildMainDlg != null)
				{
					newGuildMainDlg.SetIsDeclareWarTarget(false);
					newGuildMainDlg.SetButton();
					newGuildMainDlg.Send_GS_DECLAREWAR_LIST_REQ();
				}
				if (packet.i64GuildID == NrTSingleton<NewGuildManager>.Instance.GetGuildID())
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("820"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
				}
				else
				{
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("822"),
						"targetname",
						TKString.NEWString(packet.strEnemyGuildName)
					});
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
				}
			}
			else if (packet.i32Result == 9500)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("791"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			else if (packet.i32Result == 9560)
			{
				if (MINE_CONSTANT_Manager.GetInstance() == null)
				{
					string message = string.Format("Warning Result {0}", packet.i32Result);
					Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
					return;
				}
				string strDayToWeek = NrReceiveGame.GetStrDayToWeek((int)packet.bDaclareDay);
				string strDayToWeek2 = NrReceiveGame.GetStrDayToWeek((int)packet.bDeclareWarStartDay);
				string empty2 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("794"),
					"day1",
					strDayToWeek,
					"hour1",
					string.Format("{0:D2}", packet.bDaclareHour),
					"min1",
					string.Format("{0:D2}", packet.bDaclareMin),
					"day2",
					strDayToWeek2,
					"hour2",
					string.Format("{0:D2}", packet.bDeclareWarStartHour),
					"min2",
					string.Format("{0:D2}", packet.bDeclareWarStartMin)
				});
				Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
			}
			else if (packet.i32Result == 9552)
			{
				if (newGuildMainDlg != null)
				{
					newGuildMainDlg.SetIsDeclareWarTarget(false);
					newGuildMainDlg.SetButton();
				}
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("810"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
			}
			else
			{
				string message2 = string.Format("result {0}", packet.i32Result);
				Main_UI_SystemMessage.ADDMessage(message2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
			}
		}

		public static void GS_EXPEDITION_START_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_EXPEDITION_START_ACK packet = kDeserializePacket.GetPacket<GS_EXPEDITION_START_ACK>();
			if (packet.i32Result == 0)
			{
				NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
				if (kMyCharInfo == null)
				{
					return;
				}
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (charPersonInfo == null)
				{
					return;
				}
				NkMilitaryList militaryList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetMilitaryList();
				if (militaryList == null)
				{
					return;
				}
				kMyCharInfo.SetCharDetail(10, packet.i64TotalCount_ExpeditionDayjoin);
				SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
				NkExpeditionMilitaryInfo validExpeditionMilitaryInfo = militaryList.GetValidExpeditionMilitaryInfo(packet.ui8ExpeditionMilitaryUniq);
				if (validExpeditionMilitaryInfo != null)
				{
					for (int i = 0; i < 15; i++)
					{
						if (packet.i64SolID[i] > 0L)
						{
							NkSoldierInfo soldierUpdate = charPersonInfo.ChangeSolPosType(packet.i64SolID[i], 6, packet.i8SolPosIndex[i], packet.ui8ExpeditionMilitaryUniq, packet.i16BattlePos[i]);
							if (solMilitaryGroupDlg != null)
							{
								solMilitaryGroupDlg.SetSoldierUpdate(soldierUpdate);
							}
						}
					}
					validExpeditionMilitaryInfo.SetSolCount();
				}
				if (solMilitaryGroupDlg != null)
				{
					solMilitaryGroupDlg.CheckSkillUpSolNum();
				}
				string message = string.Empty;
				message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("328");
				Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
		}

		public static void GS_EXPEDITION_SERACH_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_EXPEDITION_SERACH_ACK packet = kDeserializePacket.GetPacket<GS_EXPEDITION_SERACH_ACK>();
			if (packet.i32Result == 0)
			{
				if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
				{
					return;
				}
				if (NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1) == null)
				{
					return;
				}
				ExpeditionSearchDetallInfoDlg expeditionSearchDetallInfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EXPEDITION_SEARCHDETAILINFO_DLG) as ExpeditionSearchDetallInfoDlg;
				if (expeditionSearchDetallInfoDlg == null)
				{
					expeditionSearchDetallInfoDlg = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EXPEDITION_SEARCHDETAILINFO_DLG) as ExpeditionSearchDetallInfoDlg);
				}
				if (expeditionSearchDetallInfoDlg != null)
				{
					expeditionSearchDetallInfoDlg.SetExpeditionSearchData(packet.ui8ExpeditionGrade, packet.i16ExpeditionCreateDataID, packet.i32MineNum, packet.i16MonLevel, packet.i32MonPlunderItemNum);
					expeditionSearchDetallInfoDlg.SetExpeditionInfo(null, eExpeditionSearchDetailInfo_Mode.eEXPEDITION_DETAILDLG_SEARCH);
				}
			}
			else if (packet.i32Result == 17)
			{
				string message = string.Empty;
				message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89");
				Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
		}

		public static void GS_EXPEDITION_RERRY_NOTIFY_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_EXPEDITION_RERRY_NOTIFY_NFY packet = kDeserializePacket.GetPacket<GS_EXPEDITION_RERRY_NOTIFY_NFY>();
			NrTSingleton<GameGuideManager>.Instance.CheckGameGuide(GameGuideType.EXPEDITION_ITEMGET);
			GameGuideExpeditionNotify gameGuideExpeditionNotify = NrTSingleton<GameGuideManager>.Instance.GetGuide(GameGuideType.EXPEDITION_ITEMGET) as GameGuideExpeditionNotify;
			if (gameGuideExpeditionNotify != null)
			{
				gameGuideExpeditionNotify.SetInfo(packet.ui8ExpeditionMilitaryUniq, packet.i16ExpeditionCreateDataID, packet.i32ItemNum);
			}
		}

		public static void GS_EXPEDITION_OCCUPY_ITEM_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_EXPEDITION_OCCUPY_ITEM_GET_ACK packet = kDeserializePacket.GetPacket<GS_EXPEDITION_OCCUPY_ITEM_GET_ACK>();
			if (packet.i32Result == 0)
			{
			}
		}

		public static void GS_EXPEDITION_CURRENTSTATUS_INFO_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_EXPEDITION_CURRENTSTATUS_INFO_GET_ACK packet = kDeserializePacket.GetPacket<GS_EXPEDITION_CURRENTSTATUS_INFO_GET_ACK>();
			if (packet.i32InfoCount <= 0)
			{
				ExpeditionSearchDlg expeditionSearchDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EXPEDITION_SEARCH_DLG) as ExpeditionSearchDlg;
				if (expeditionSearchDlg != null)
				{
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("753");
					Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				else
				{
					expeditionSearchDlg = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EXPEDITION_SEARCH_DLG) as ExpeditionSearchDlg);
					if (expeditionSearchDlg != null)
					{
						expeditionSearchDlg.Show();
					}
				}
				ExpeditionCurrentStatusInfoDlg expeditionCurrentStatusInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EXPEDITION_CURRENTSTATUSINFO_DLG) as ExpeditionCurrentStatusInfoDlg;
				if (expeditionCurrentStatusInfoDlg != null)
				{
					NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.EXPEDITION_CURRENTSTATUSINFO_DLG);
				}
			}
			else
			{
				ExpeditionCurrentStatusInfoDlg expeditionCurrentStatusInfoDlg2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EXPEDITION_CURRENTSTATUSINFO_DLG) as ExpeditionCurrentStatusInfoDlg;
				EXPEDITION_CURRENT_STATE_INFO[] array = new EXPEDITION_CURRENT_STATE_INFO[packet.i32InfoCount];
				for (int i = 0; i < packet.i32InfoCount; i++)
				{
					array[i] = kDeserializePacket.GetPacket<EXPEDITION_CURRENT_STATE_INFO>();
				}
				if (expeditionCurrentStatusInfoDlg2 != null)
				{
					expeditionCurrentStatusInfoDlg2.AddInfo(array);
				}
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.EXPEDITION_SEARCH_DLG);
			}
		}

		public static void GS_EXPEDITION_CHAR_MILITARY_CHECK_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_EXPEDITION_CHAR_MILITARY_CHECK_NFY packet = kDeserializePacket.GetPacket<GS_EXPEDITION_CHAR_MILITARY_CHECK_NFY>();
			if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
			{
				return;
			}
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo == null)
			{
				return;
			}
			NkMilitaryList militaryList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetMilitaryList();
			if (militaryList == null)
			{
				return;
			}
			SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
			NkExpeditionMilitaryInfo validExpeditionMilitaryInfo = militaryList.GetValidExpeditionMilitaryInfo(packet.byExpeditionMilitaryUniq);
			if (validExpeditionMilitaryInfo != null)
			{
				for (int i = 0; i < 15; i++)
				{
					if (packet.i64SolID[i] > 0L)
					{
						NkSoldierInfo soldierUpdate = charPersonInfo.ChangeSolPosType(packet.i64SolID[i], 0, 0, 0, 0);
						if (solMilitaryGroupDlg != null)
						{
							solMilitaryGroupDlg.SetSoldierUpdate(soldierUpdate);
						}
					}
				}
				validExpeditionMilitaryInfo.SetSolCount();
				validExpeditionMilitaryInfo.Init();
			}
			if (solMilitaryGroupDlg != null)
			{
				solMilitaryGroupDlg.CheckSkillUpSolNum();
			}
		}

		public static void GS_EXPEDITION_MILITARY_BACKMOVE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_EXPEDITION_MILITARY_BACKMOVE_ACK packet = kDeserializePacket.GetPacket<GS_EXPEDITION_MILITARY_BACKMOVE_ACK>();
			if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
			{
				return;
			}
			if (packet.i32Result == 0)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("329");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				ExpeditionCurrentStatusInfoDlg expeditionCurrentStatusInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EXPEDITION_CURRENTSTATUSINFO_DLG) as ExpeditionCurrentStatusInfoDlg;
				if (expeditionCurrentStatusInfoDlg != null)
				{
					expeditionCurrentStatusInfoDlg.RefreshList();
				}
			}
		}

		public static void GS_EXPEDITION_DETAILINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_EXPEDITION_DETAILINFO_ACK packet = kDeserializePacket.GetPacket<GS_EXPEDITION_DETAILINFO_ACK>();
			if (packet.i32Result == 0)
			{
				if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetMilitaryList() == null)
				{
					return;
				}
				ExpeditionSearchDetallInfoDlg expeditionSearchDetallInfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EXPEDITION_SEARCHDETAILINFO_DLG) as ExpeditionSearchDetallInfoDlg;
				if (expeditionSearchDetallInfoDlg != null)
				{
					expeditionSearchDetallInfoDlg.SetExpeditionInfo(packet, eExpeditionSearchDetailInfo_Mode.eEXPEDITION_DETAILDLG_ATTACK);
				}
			}
			else
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("758");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}

		public static void GS_FRIEND_LIST_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_FRIEND_LIST_NFY packet = kDeserializePacket.GetPacket<GS_FRIEND_LIST_NFY>();
			for (int i = 0; i < (int)packet.byFriendInfoCount; i++)
			{
				USER_FRIEND_INFO packet2 = kDeserializePacket.GetPacket<USER_FRIEND_INFO>();
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.AddFriend(packet2);
			}
			Tapjoy.SetUserFriendCount((int)packet.byFriendInfoCount);
		}

		public static void GS_FRIEND_APPLY_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_FRIEND_APPLY_ACK packet = kDeserializePacket.GetPacket<GS_FRIEND_APPLY_ACK>();
			if (packet.Result != 0)
			{
				if (packet.Result == 1)
				{
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("21");
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						textFromNotify,
						"Charname",
						TKString.NEWString(packet.Name)
					});
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
				else if (packet.Result == 401)
				{
					string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("42");
					string empty2 = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
					{
						textFromNotify2,
						"Charname",
						TKString.NEWString(packet.Name)
					});
					Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				else if (packet.Result == 402)
				{
					string textFromNotify3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("22");
					string empty3 = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
					{
						textFromNotify3,
						"Charname",
						TKString.NEWString(packet.Name)
					});
					Main_UI_SystemMessage.ADDMessage(empty3, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
				else if (packet.Result == 403)
				{
					string textFromNotify4 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("311");
					Main_UI_SystemMessage.ADDMessage(textFromNotify4, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
				else if (packet.Result == 404)
				{
					string textFromNotify5 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("433");
					Main_UI_SystemMessage.ADDMessage(textFromNotify5, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
				else if (packet.Result == 405)
				{
					string textFromNotify6 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("693");
					Main_UI_SystemMessage.ADDMessage(textFromNotify6, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
				else if (packet.Result == 9300)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("744"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
			}
		}

		public static void GS_ADD_FRIEND_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ADD_FRIEND_ACK packet = kDeserializePacket.GetPacket<GS_ADD_FRIEND_ACK>();
			if (packet.Result == 0)
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.AddFriend(packet.pFriend);
				if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.COMMUNITY_DLG))
				{
					CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
					communityUI_DLG.UpdatePage();
				}
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("24");
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromNotify,
					"Charname",
					TKString.NEWString(packet.pFriend.szName)
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				if (PlayerPrefs.GetInt("Community DLG Effect") == 0 && Scene.CurScene == Scene.Type.WORLD)
				{
					CommunityUI_DLG communityUI_DLG2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
					if (communityUI_DLG2 != null)
					{
						communityUI_DLG2.RequestCommunityData(eCOMMUNITYDLG_SHOWTYPE.eSHOWTYPE_HELPSOLSET);
					}
				}
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "COMMUNITY", "ACCEPT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			}
			else
			{
				if (packet.Result != 1)
				{
					if (packet.Result == 404)
					{
						string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("433");
						Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					}
					else if (packet.Result == 405)
					{
						string textFromNotify3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("693");
						Main_UI_SystemMessage.ADDMessage(textFromNotify3, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					}
				}
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "COMMUNITY", "CANCEL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			}
			Tapjoy.SetUserFriendCount(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriendCount());
		}

		public static void GS_DEL_FRIEND_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_DEL_FRIEND_ACK packet = kDeserializePacket.GetPacket<GS_DEL_FRIEND_ACK>();
			if (packet.Result == 0)
			{
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (charPersonInfo == null)
				{
					return;
				}
				USER_FRIEND_INFO friend = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriend(packet.i64FriendPersonID);
				string text = string.Empty;
				if (friend != null)
				{
					text = TKString.NEWString(friend.szName);
				}
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.DelFriend(packet.i64FriendPersonID);
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet.i64HelpSolID);
				if (soldierInfoFromSolID != null)
				{
					soldierInfoFromSolID.SetFriendPersonID(0L);
				}
				if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.COMMUNITY_DLG))
				{
					CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
					communityUI_DLG.UpdatePage();
				}
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("26");
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromNotify,
					"Charname",
					text
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.DelFriend(friend.nPersonID);
			}
			else if (packet.Result == 1)
			{
				USER_FRIEND_INFO friend2 = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriend(packet.i64FriendPersonID);
				string text2 = string.Empty;
				if (friend2 != null)
				{
					text2 = TKString.NEWString(friend2.szName);
				}
				string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("25");
				string empty2 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					textFromNotify2,
					"Charname",
					text2
				});
				Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			else if (packet.Result == 5000)
			{
				string textFromNotify3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("563");
				Main_UI_SystemMessage.ADDMessage(textFromNotify3, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			Tapjoy.SetUserFriendCount(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriendCount());
		}

		public static void GS_FRIEND_LOGIN_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_FRIEND_LOGIN_NFY packet = kDeserializePacket.GetPacket<GS_FRIEND_LOGIN_NFY>();
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.UpdateFriend(packet.FriendInfo);
			if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.COMMUNITY_DLG))
			{
				CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
				communityUI_DLG.UpdateFriend(packet.FriendInfo.nPersonID, true);
			}
		}

		public static void GS_FRIEND_LOGOUT_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_FRIEND_LOGOUT_NFY packet = kDeserializePacket.GetPacket<GS_FRIEND_LOGOUT_NFY>();
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.UpdateFriend(packet.FriendInfo);
			if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.COMMUNITY_DLG))
			{
				CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
				communityUI_DLG.UpdateFriend(packet.FriendInfo.nPersonID, true);
			}
		}

		public static void GS_FRIEND_UPDATE_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_FRIEND_UPDATE_NFY packet = kDeserializePacket.GetPacket<GS_FRIEND_UPDATE_NFY>();
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.UpdateFriend(packet.FriendInfo);
			if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.COMMUNITY_DLG))
			{
				CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
				communityUI_DLG.UpdateFriend(packet.FriendInfo.nPersonID, true);
			}
		}

		public static void GS_OTHER_FRIEND_LIST_PAGE_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_OTHER_FRIEND_LIST_PAGE_NFY packet = kDeserializePacket.GetPacket<GS_OTHER_FRIEND_LIST_PAGE_NFY>();
			DLG_OtherCharDetailInfo dLG_OtherCharDetailInfo = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_OTHER_CHAR_DETAIL) as DLG_OtherCharDetailInfo;
			for (int i = 0; i < (int)packet.ui8FriendCount; i++)
			{
				USER_FRIEND_INFO packet2 = kDeserializePacket.GetPacket<USER_FRIEND_INFO>();
				if (dLG_OtherCharDetailInfo != null && packet2 != null)
				{
					dLG_OtherCharDetailInfo.UpdateFriendList(packet2);
				}
			}
			if (dLG_OtherCharDetailInfo != null)
			{
				dLG_OtherCharDetailInfo.UpdateList((int)packet.ui8FriendMaxCount);
			}
		}

		public static void GS_GUIDE_INVITE_FRIEND_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_GUIDE_INVITE_FRIEND_NFY packet = kDeserializePacket.GetPacket<GS_GUIDE_INVITE_FRIEND_NFY>();
			if (packet.nResult == 0)
			{
				INVITE_PERSONINFO iNVITE_PERSONINFO = new INVITE_PERSONINFO();
				iNVITE_PERSONINFO.eInvte_type = (eFRIEND_INVITETYPE)packet.ui8Invite_type;
				iNVITE_PERSONINFO.Invite_PersonID = packet.nPersonID;
				iNVITE_PERSONINFO.Invite_PersonLevel = packet.nLevel;
				iNVITE_PERSONINFO.Invite_PersonFaceCharKind = packet.nFaceCharKind;
				iNVITE_PERSONINFO.Invite_UserName = TKString.NEWString(packet.InviteName);
				iNVITE_PERSONINFO.Invite_User_InfoMsg = TKString.NEWString(packet.InviteIntroMsg);
				INIVITEPERSON_FRIENDINFO item = new INIVITEPERSON_FRIENDINFO();
				for (int i = 0; i < packet.nInvitePerson_FriendCount; i++)
				{
					item = kDeserializePacket.GetPacket<INIVITEPERSON_FRIENDINFO>();
					iNVITE_PERSONINFO.list_InvitePerson_FriendList.Add(item);
				}
				GameGuideAddFriend gameGuideAddFriend = NrTSingleton<GameGuideManager>.Instance.GetGuide(GameGuideType.FRIEND_RECOMMEND1) as GameGuideAddFriend;
				if (gameGuideAddFriend == null)
				{
					gameGuideAddFriend = (NrTSingleton<GameGuideManager>.Instance.GetGuide(GameGuideType.FRIEND_RECOMMEND2) as GameGuideAddFriend);
				}
				if (gameGuideAddFriend == null)
				{
					gameGuideAddFriend = (NrTSingleton<GameGuideManager>.Instance.GetGuide(GameGuideType.FRIEND_RECOMMEND3) as GameGuideAddFriend);
				}
				if (gameGuideAddFriend != null)
				{
					gameGuideAddFriend.SetInvite(iNVITE_PERSONINFO);
				}
				if (packet.ui8ReqType == 1)
				{
					NrTSingleton<GameGuideManager>.Instance.CheckGameGuide(GameGuideType.FRIEND_RECOMMEND1);
					NrTSingleton<GameGuideManager>.Instance.CheckGameGuide(GameGuideType.FRIEND_RECOMMEND2);
					NrTSingleton<GameGuideManager>.Instance.CheckGameGuide(GameGuideType.FRIEND_RECOMMEND3);
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("159");
					Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
			}
			else if (packet.nResult == 5001 && packet.ui8ReqType == 1)
			{
				string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("162");
				Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}

		public static void GS_FOLLOWCHAR_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_FOLLOWCHAR_ACK packet = kDeserializePacket.GetPacket<GS_FOLLOWCHAR_ACK>();
			if (packet.i16Result > 0)
			{
				NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
				if (nrCharUser != null && nrCharUser.m_kCharMove != null)
				{
					nrCharUser.m_kCharMove.AutoMoveTo((int)packet.i16DunID, (short)packet.pos.x, (short)packet.pos.z, true);
					NrTSingleton<NkQuestManager>.Instance.SetAutoPathInfo(null);
					nrCharUser.SetFollowCharPersonID(packet.i64CID, TKString.NEWString(packet.Name));
					NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_FOLLOWCHAR);
					NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_FOLLOWCHAR);
				}
			}
			else
			{
				if (packet.i16Result == -1)
				{
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("332");
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						textFromNotify,
						"User Name",
						TKString.NEWString(packet.Name)
					});
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				else if (packet.i16Result == -2)
				{
					string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1543");
					string empty2 = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
					{
						textFromInterface,
						"username",
						TKString.NEWString(packet.Name)
					});
					Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				NrCharUser nrCharUser2 = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
				if (nrCharUser2 != null)
				{
					nrCharUser2.SetFollowCharPersonID(0L, string.Empty);
				}
			}
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_bRequestFollowCharPos = false;
		}

		public static void GS_FRIEND_HELPSOLINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_FRIEND_HELPSOLINFO_ACK packet = kDeserializePacket.GetPacket<GS_FRIEND_HELPSOLINFO_ACK>();
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			for (int i = 0; i < (int)packet.ui8HelpSol_ExpCount; i++)
			{
				HELPSOL_EXPINFO packet2 = kDeserializePacket.GetPacket<HELPSOL_EXPINFO>();
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet2.i64SolID);
				if (soldierInfoFromSolID != null)
				{
					soldierInfoFromSolID.AddHelpExp = packet2.i64Exp;
					if (packet2.i32UseCount > 0)
					{
						soldierInfoFromSolID.HelpSolUse = true;
					}
				}
			}
			BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
			if (bookmarkDlg != null)
			{
				bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.COMMUNITY);
			}
			CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
			if (communityUI_DLG != null)
			{
				communityUI_DLG.Show(packet.type);
			}
		}

		public static void GS_FRIEND_HELPSOL_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_FRIEND_HELPSOL_SET_ACK packet = kDeserializePacket.GetPacket<GS_FRIEND_HELPSOL_SET_ACK>();
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (packet.i32Result == 0)
			{
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet.i64SolID);
				if (soldierInfoFromSolID != null)
				{
					soldierInfoFromSolID.SetFriendPersonID(packet.i64FriendPersonID);
					if (packet.byHelpSolUse > 0)
					{
						soldierInfoFromSolID.HelpSolUse = true;
					}
					USER_FRIEND_INFO friend = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().m_kFriendInfo.GetFriend(packet.i64FriendPersonID);
					if (friend != null)
					{
						string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("41");
						string empty = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							textFromNotify,
							"username",
							TKString.NEWString(friend.szName),
							"solname",
							soldierInfoFromSolID.GetName()
						});
						Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
					}
				}
				CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
				if (communityUI_DLG != null)
				{
					communityUI_DLG.UpdateFriend(packet.i64FriendPersonID, true);
				}
				SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
				if (solMilitaryGroupDlg != null)
				{
					solMilitaryGroupDlg.RefreshSolList();
				}
			}
			else
			{
				string text = string.Empty;
				int i32Result = packet.i32Result;
				if (i32Result == 400)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("623");
				}
				if (text != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
		}

		public static void GS_FRIEND_HELPSOL_UNSET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_FRIEND_HELPSOL_UNSET_ACK packet = kDeserializePacket.GetPacket<GS_FRIEND_HELPSOL_UNSET_ACK>();
			if (packet.i32Result == 0)
			{
				string text = string.Empty;
				string text2 = string.Empty;
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet.i64SolID);
				NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
				if (kMyCharInfo == null)
				{
					return;
				}
				if (soldierInfoFromSolID != null)
				{
					USER_FRIEND_INFO friend = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().m_kFriendInfo.GetFriend(packet.i64FriendPersonID);
					if (friend != null)
					{
						string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("43");
						string empty = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							textFromNotify,
							"username",
							TKString.NEWString(friend.szName)
						});
						Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
					}
					kMyCharInfo.SetCharDetail(9, packet.i64CharDetailCount_HelpSolGiveItem);
					long charDetail = kMyCharInfo.GetCharDetail(9);
					int limitFriendCount = BASE_FRIENDCOUNTLIMIT_DATA.GetLimitFriendCount((short)kMyCharInfo.GetLevel());
					if (charDetail >= (long)limitFriendCount)
					{
						text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("502");
						Main_UI_SystemMessage.ADDMessage(text2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					}
					else if (soldierInfoFromSolID.AddHelpExp > 0L)
					{
						if (soldierInfoFromSolID.GetSolMaxLevel() <= soldierInfoFromSolID.GetLevel())
						{
							soldierInfoFromSolID.AddHelpExp = 0L;
						}
						byte byRewardType = packet.byRewardType;
						if (byRewardType == 0)
						{
							text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("612");
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
							{
								text,
								"solname",
								soldierInfoFromSolID.GetName(),
								soldierInfoFromSolID.GetName(),
								"exp",
								soldierInfoFromSolID.AddHelpExp.ToString()
							});
						}
						else if (byRewardType == 2 || byRewardType == 3)
						{
							int i32ItemUnique = packet.i32ItemUnique;
							string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(i32ItemUnique);
							text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("737");
							if (packet.i32VipItemUnique == 0 && packet.i32VipItemNum > 0)
							{
								NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
								{
									text,
									"itemname",
									itemNameByItemUnique,
									"itemnum",
									(packet.i32ItemNum - packet.i32VipItemNum).ToString(),
									"solname",
									soldierInfoFromSolID.GetName(),
									soldierInfoFromSolID.GetName(),
									"exp",
									soldierInfoFromSolID.AddHelpExp.ToString()
								});
							}
							else
							{
								NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
								{
									text,
									"itemname",
									itemNameByItemUnique,
									"itemnum",
									packet.i32ItemNum.ToString(),
									"solname",
									soldierInfoFromSolID.GetName(),
									soldierInfoFromSolID.GetName(),
									"exp",
									soldierInfoFromSolID.AddHelpExp.ToString()
								});
							}
						}
						else if (byRewardType == 1)
						{
							text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("613");
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
							{
								text,
								"gold",
								packet.i64GiveMoney,
								"solname",
								soldierInfoFromSolID.GetName(),
								soldierInfoFromSolID.GetName(),
								"exp",
								soldierInfoFromSolID.AddHelpExp.ToString()
							});
							NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64TotalMoney;
						}
						if (text2 != string.Empty)
						{
							Main_UI_SystemMessage.ADDMessage(text2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
						}
						if (!NrTSingleton<ContentsLimitManager>.Instance.IsVipExp() && packet.i32VipItemNum > 0)
						{
							int i32VipItemUnique = packet.i32VipItemUnique;
							int i32VipItemNum = packet.i32VipItemNum;
							string text3 = string.Empty;
							if (packet.i32VipItemUnique > 0)
							{
								text3 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(i32VipItemUnique);
							}
							else
							{
								if (packet.i32ItemUnique <= 0)
								{
									return;
								}
								text3 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.i32ItemUnique);
							}
							text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("779");
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
							{
								text,
								"itemname",
								text3,
								"itemnum",
								i32VipItemNum.ToString()
							});
							Main_UI_SystemMessage.ADDMessage(text2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
						}
					}
					soldierInfoFromSolID.AddHelpExp = 0L;
					soldierInfoFromSolID.HelpSolUse = false;
					soldierInfoFromSolID.SetFriendPersonID(0L);
				}
				CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
				if (communityUI_DLG != null)
				{
					communityUI_DLG.UpdateFriend(packet.i64FriendPersonID, true);
				}
				BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
				if (bookmarkDlg != null)
				{
					bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.COMMUNITY);
				}
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLCOMPOSE_MAIN_DLG))
				{
					if (SolComposeMainDlg.Instance.ContainSubSoldier(packet.i64SolID))
					{
						SolComposeMainDlg.Instance.MakeSubSolList();
					}
					if (SolComposeMainDlg.Instance.ContainExtractSoldier(packet.i64SolID))
					{
						SolComposeMainDlg.Instance.RefreshSelectExtract();
					}
				}
				SolDetail_Info_Dlg solDetail_Info_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLDETAIL_DLG) as SolDetail_Info_Dlg;
				if (solDetail_Info_Dlg != null)
				{
					solDetail_Info_Dlg.GetSelectToolBarRefresh(packet.i64SolID);
				}
				Myth_Legend_Info_DLG myth_Legend_Info_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTH_LEGEND_INFO_DLG) as Myth_Legend_Info_DLG;
				if (myth_Legend_Info_DLG != null)
				{
					NkSoldierInfo soldierInfoFromSolID2 = charPersonInfo.GetSoldierInfoFromSolID(packet.i64SolID);
					if (soldierInfoFromSolID2 == null)
					{
						return;
					}
					myth_Legend_Info_DLG.InitSetCharKind(soldierInfoFromSolID2.GetCharKind());
				}
			}
			else if (packet.i32Result == 31)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46"));
			}
			else if (packet.i32Result == 76)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("773"));
			}
		}

		public static void GS_FRIEND_HELPSOL_EXP_GIVE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_FRIEND_HELPSOL_EXP_GIVE_ACK packet = kDeserializePacket.GetPacket<GS_FRIEND_HELPSOL_EXP_GIVE_ACK>();
			if (packet.i32Result == 0)
			{
				string text = string.Empty;
				string empty = string.Empty;
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet.i64SolID);
				NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
				if (kMyCharInfo == null)
				{
					return;
				}
				if (soldierInfoFromSolID != null)
				{
					if (soldierInfoFromSolID.GetSolMaxLevel() <= soldierInfoFromSolID.GetLevel())
					{
						soldierInfoFromSolID.AddHelpExp = 0L;
					}
					byte byRewardType = packet.byRewardType;
					if (byRewardType == 0)
					{
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("612");
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							text,
							"solname",
							soldierInfoFromSolID.GetName(),
							soldierInfoFromSolID.GetName(),
							"exp",
							soldierInfoFromSolID.AddHelpExp.ToString()
						});
					}
					else if (byRewardType == 2 || byRewardType == 3)
					{
						int i32ItemUnique = packet.i32ItemUnique;
						int i32ItemNum = packet.i32ItemNum;
						string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(i32ItemUnique);
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("737");
						if (packet.i32VipItemUnique == 0 && packet.i32VipItemNum > 0)
						{
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
							{
								text,
								"itemname",
								itemNameByItemUnique,
								"itemnum",
								(i32ItemNum - packet.i32VipItemNum).ToString(),
								"solname",
								soldierInfoFromSolID.GetName(),
								soldierInfoFromSolID.GetName(),
								"exp",
								soldierInfoFromSolID.AddHelpExp.ToString()
							});
						}
						else
						{
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
							{
								text,
								"itemname",
								itemNameByItemUnique,
								"itemnum",
								i32ItemNum.ToString(),
								"solname",
								soldierInfoFromSolID.GetName(),
								soldierInfoFromSolID.GetName(),
								"exp",
								soldierInfoFromSolID.AddHelpExp.ToString()
							});
						}
					}
					else if (byRewardType == 1 && packet.i64GiveMoney > 0L)
					{
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("613");
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							text,
							"gold",
							packet.i64GiveMoney,
							"solname",
							soldierInfoFromSolID.GetName(),
							soldierInfoFromSolID.GetName(),
							"exp",
							soldierInfoFromSolID.AddHelpExp.ToString()
						});
						NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64TotalMoney;
					}
					if (empty != string.Empty)
					{
						Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
					}
					soldierInfoFromSolID.AddHelpExp = 0L;
					if (!NrTSingleton<ContentsLimitManager>.Instance.IsVipExp() && packet.i32VipItemNum > 0)
					{
						int i32VipItemUnique = packet.i32VipItemUnique;
						int i32VipItemNum = packet.i32VipItemNum;
						string text2 = string.Empty;
						if (packet.i32VipItemUnique > 0)
						{
							text2 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(i32VipItemUnique);
						}
						else
						{
							if (packet.i32ItemUnique <= 0)
							{
								return;
							}
							text2 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.i32ItemUnique);
						}
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("779");
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							text,
							"itemname",
							text2,
							"itemnum",
							i32VipItemNum.ToString()
						});
						Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
					}
				}
				kMyCharInfo.SetCharDetail(9, packet.i64CharDetailCount_HelpSolGiveItem);
				CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
				if (communityUI_DLG != null)
				{
					communityUI_DLG.UpdateFriend(soldierInfoFromSolID.GetFriendPersonID(), true);
				}
				BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
				if (bookmarkDlg != null)
				{
					bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.COMMUNITY);
				}
				SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
				if (solMilitaryGroupDlg != null)
				{
					solMilitaryGroupDlg.RefreshSolList();
				}
				NrTSingleton<FiveRocksEventManager>.Instance.HeartsInflow(eHEARTS_INFLOW.FRIEND_HELP_REWARD, 1L);
			}
			else if (packet.i32Result == 31)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46"));
			}
			else if (packet.i32Result == 76)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("773"));
			}
		}

		public static void GS_FRIEND_HELPSOL_ALL_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_FRIEND_HELPSOL_ALL_ACK packet = kDeserializePacket.GetPacket<GS_FRIEND_HELPSOL_ALL_ACK>();
			if (packet.i32Result == 0)
			{
				if (packet.i32ItemNum == 0 && packet.i32VipItemNum == 0 && packet.i64GiveMoney == 0L)
				{
					return;
				}
				string text = string.Empty;
				string empty = string.Empty;
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
				if (kMyCharInfo == null)
				{
					return;
				}
				CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
				for (int i = 0; i < 30; i++)
				{
					if (packet.i64SolID[i] > 0L)
					{
						NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet.i64SolID[i]);
						if (soldierInfoFromSolID != null)
						{
							TsLog.LogWarning(" {0} : Solid = {1}", new object[]
							{
								i,
								packet.i64SolID[i]
							});
							soldierInfoFromSolID.AddHelpExp = 0L;
							kMyCharInfo.SetCharDetail(9, packet.i64CharDetailCount_HelpSolGiveItem);
							NrTSingleton<FiveRocksEventManager>.Instance.HeartsInflow(eHEARTS_INFLOW.FRIEND_HELP_REWARD, 1L);
						}
					}
				}
				if (communityUI_DLG != null)
				{
					communityUI_DLG.Close();
				}
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("869");
				string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.i32ItemUnique);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					text,
					"itemname",
					itemNameByItemUnique,
					"itemnum",
					packet.i32ItemNum.ToString(),
					"gold",
					packet.i64GiveMoney
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
				TsLog.LogWarning(" item {0} : num {1} : Gold {2}", new object[]
				{
					packet.i32ItemUnique,
					packet.i32ItemNum,
					packet.i64TotalMoney
				});
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64TotalMoney;
				if (!NrTSingleton<ContentsLimitManager>.Instance.IsVipExp() && packet.i32VipItemNum > 0)
				{
					int i32VipItemUnique = packet.i32VipItemUnique;
					int i32VipItemNum = packet.i32VipItemNum;
					string text2 = string.Empty;
					if (packet.i32VipItemUnique > 0)
					{
						text2 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(i32VipItemUnique);
					}
					else
					{
						if (packet.i32ItemUnique <= 0)
						{
							return;
						}
						text2 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.i32ItemUnique);
					}
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("779");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						text,
						"itemname",
						text2,
						"itemnum",
						i32VipItemNum.ToString()
					});
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
					TsLog.LogWarning(" Vip item {0} : num {1} ", new object[]
					{
						packet.i32VipItemUnique,
						i32VipItemNum
					});
				}
				BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
				if (bookmarkDlg != null)
				{
					bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.COMMUNITY);
				}
				SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
				if (solMilitaryGroupDlg != null)
				{
					solMilitaryGroupDlg.RefreshSolList();
				}
			}
			else if (packet.i32Result != -10)
			{
				if (packet.i32Result != -20)
				{
					if (packet.i32Result != -30)
					{
						if (packet.i32Result != -40)
						{
							if (packet.i32Result != -50)
							{
								if (packet.i32Result != -60)
								{
									if (packet.i32Result == 31)
									{
										Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46"));
									}
									else if (packet.i32Result == 76)
									{
										Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("773"));
									}
								}
							}
						}
					}
				}
			}
		}

		public static void GS_FRIEND_HELPSOLINFO_UPDATE_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_FRIEND_HELPSOLINFO_UPDATE_NFY packet = kDeserializePacket.GetPacket<GS_FRIEND_HELPSOLINFO_UPDATE_NFY>();
			USER_FRIEND_INFO friend = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriend(packet.i64FriendPersonID);
			if (friend != null)
			{
				friend.FriendHelpSolInfo = packet.FriendHelpSolInfo;
			}
			CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
			if (communityUI_DLG != null)
			{
				communityUI_DLG.UpdateFriend(packet.i64FriendPersonID, true);
			}
		}

		public static void GS_FRIEND_HELPSOL_ADDEXP_UPDATE_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_FRIEND_HELPSOL_ADDEXP_UPDATE_NFY packet = kDeserializePacket.GetPacket<GS_FRIEND_HELPSOL_ADDEXP_UPDATE_NFY>();
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet.i64SolID);
			if (soldierInfoFromSolID != null)
			{
				soldierInfoFromSolID.AddHelpExp = packet.i64AddExp;
				if (packet.byHelpSolUse > 0)
				{
					soldierInfoFromSolID.HelpSolUse = true;
				}
			}
			CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
			if (communityUI_DLG != null)
			{
				communityUI_DLG.UpdateFriend(packet.i64FriendPersonID, true);
			}
			BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
			if (bookmarkDlg != null)
			{
				bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.COMMUNITY);
			}
		}

		public static void GS_FACEBOOK_FRIENDINFO_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_FACEBOOK_FRIENDINFO_GET_ACK packet = kDeserializePacket.GetPacket<GS_FACEBOOK_FRIENDINFO_GET_ACK>();
			FacebookFriendInviteDlg facebookFriendInviteDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.FACEBOOK_FRIEND_INVITE) as FacebookFriendInviteDlg;
			for (int i = 0; i < packet.InfoCount; i++)
			{
				FACEBOOK_FRIEND_GAMEINFO packet2 = kDeserializePacket.GetPacket<FACEBOOK_FRIEND_GAMEINFO>();
				if (facebookFriendInviteDlg != null)
				{
					facebookFriendInviteDlg.SetUserData(packet2);
				}
				else
				{
					FacebookFriendInviteDlg.FacebookFriendUpdate(packet2);
				}
			}
			if (facebookFriendInviteDlg != null)
			{
				facebookFriendInviteDlg.UpdateList();
			}
		}

		public static void GS_FRIEND_HELPSOL_USE_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_FRIEND_HELPSOL_USE_NFY packet = kDeserializePacket.GetPacket<GS_FRIEND_HELPSOL_USE_NFY>();
			USER_FRIEND_INFO friend = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriend(packet.i64FriendPersonID);
			if (friend != null)
			{
				friend.ui8HelpUse = packet.ui8HelpSolUse;
			}
			CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
			if (communityUI_DLG != null)
			{
				communityUI_DLG.UpdateFriend(packet.i64FriendPersonID, true);
			}
		}

		public static void GS_FRIEND_DETAILINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_FRIEND_DETAILINFO_ACK packet = kDeserializePacket.GetPacket<GS_FRIEND_DETAILINFO_ACK>();
			DLG_OtherCharDetailInfo dLG_OtherCharDetailInfo = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_OTHER_CHAR_DETAIL) as DLG_OtherCharDetailInfo;
			if (dLG_OtherCharDetailInfo != null)
			{
				dLG_OtherCharDetailInfo.SetFriendDetailInfo(packet, kDeserializePacket);
			}
		}

		public static void GS_INVITE_REWARD_ACK(NkDeserializePacket kDeserializePacket)
		{
			if (kDeserializePacket.GetPacket<GS_INVITE_REWARD_ACK>().Result == 0)
			{
				long personID = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID;
				PlayerPrefs.SetInt(string.Format("{0}{1}", NrPrefsKey.KAKAO_REWARD_SET, personID), 1);
			}
		}

		public static void GS_SOLDIER_CHANGE_POSTYPE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLDIER_CHANGE_POSTYPE_ACK packet = kDeserializePacket.GetPacket<GS_SOLDIER_CHANGE_POSTYPE_ACK>();
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo == null)
			{
				return;
			}
			charPersonInfo.ChangeSolPosType(packet.SolID, packet.SolPosType, packet.SolPosIndex, packet.MilitaryUnique, packet.BattlePos);
			SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
			if (solMilitaryGroupDlg != null)
			{
				solMilitaryGroupDlg.RefreshBattleSolList();
				solMilitaryGroupDlg.MakeSolListAndSort();
				solMilitaryGroupDlg.CheckSkillUpSolNum();
			}
			SolMilitarySelectDlg solMilitarySelectDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYSELECT_DLG) as SolMilitarySelectDlg;
			if (solMilitarySelectDlg != null)
			{
				solMilitarySelectDlg.Refresh();
			}
			BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
			if (bookmarkDlg != null)
			{
				bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.HERO);
			}
			HeroCollect_DLG heroCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.HEROCOLLECT_DLG) as HeroCollect_DLG;
			if (heroCollect_DLG != null)
			{
				heroCollect_DLG.Update_Notice();
			}
		}

		public static void GS_SET_SOLDIER_MILITARY_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SET_SOLDIER_MILITARY_ACK packet = kDeserializePacket.GetPacket<GS_SET_SOLDIER_MILITARY_ACK>();
			if (packet.nResult == 0)
			{
				NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				NkMilitaryList militaryList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetMilitaryList();
				if (charPersonInfo == null || kMyCharInfo == null || militaryList == null)
				{
					return;
				}
				SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
				NkMineMilitaryInfo validMineMilitaryInfo = militaryList.GetValidMineMilitaryInfo(packet.nMilitaryUnique);
				if (validMineMilitaryInfo != null)
				{
					validMineMilitaryInfo.SetMilitaryLegionID(packet.nLegionID);
					validMineMilitaryInfo.SetMilitaryLegionActionID(packet.nLegionActionID);
					validMineMilitaryInfo.SetMilitaryStatus(packet.nState);
					for (int i = 0; i < 5; i++)
					{
						if (packet.MilitaryInfo[i].SolID > 0L)
						{
							NkSoldierInfo soldierUpdate = charPersonInfo.ChangeSolPosType(packet.MilitaryInfo[i].SolID, packet.MilitaryInfo[i].SolPosType, packet.MilitaryInfo[i].SolPosIndex, packet.MilitaryInfo[i].MilitaryUnique, packet.MilitaryInfo[i].BattlePos);
							if (solMilitaryGroupDlg != null)
							{
								solMilitaryGroupDlg.SetSoldierUpdate(soldierUpdate);
							}
						}
					}
					validMineMilitaryInfo.SetSolCount();
					if (packet.nState == 7)
					{
						validMineMilitaryInfo.Init();
					}
					else
					{
						kMyCharInfo.SetCharDetail(8, packet.i64TotalCount_MineDayJoin);
						StageWorld.MINEMSG_TYPE = eMINE_MESSAGE.eMINE_MESSAGE_GO_MILITARY_SUCCESS;
					}
				}
				if (solMilitaryGroupDlg != null)
				{
					solMilitaryGroupDlg.CheckSkillUpSolNum();
				}
				MineGuildCurrentStatusInfoDlg mineGuildCurrentStatusInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MINE_GUILD_CURRENTSTATUSINFO_DLG) as MineGuildCurrentStatusInfoDlg;
				if (mineGuildCurrentStatusInfoDlg != null)
				{
					mineGuildCurrentStatusInfoDlg.RefreshList();
				}
				DateTime dueDate = PublicMethod.GetDueDate(PublicMethod.GetCurTime());
				if (packet.nLeaderMilitary > 0 && packet.i32GuildPushCount < 10 && 8 <= dueDate.Hour && 21 >= dueDate.Hour && (NrTSingleton<NewGuildManager>.Instance.IsMaster(kMyCharInfo.m_PersonID) || NrTSingleton<NewGuildManager>.Instance.IsSubMaster(kMyCharInfo.m_PersonID) || NrTSingleton<NewGuildManager>.Instance.IsOfficer(kMyCharInfo.m_PersonID)))
				{
					NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.MINE_GUILDPUSH_CONFIRM_DLG);
				}
			}
			else if (packet.nResult == 9101)
			{
				StageWorld.MINEMSG_TYPE = eMINE_MESSAGE.eMINE_MESSAGE_GO_MILITARY_FAIL01;
			}
			else if (packet.nResult == 9100)
			{
				StageWorld.MINEMSG_TYPE = eMINE_MESSAGE.eMINE_MESSAGE_GO_MILITARY_FAIL03;
			}
			else if (packet.nResult == 9102)
			{
				StageWorld.MINEMSG_TYPE = eMINE_MESSAGE.eMINE_MESSAGE_GO_MILITARY_FAIL02;
			}
			else if (packet.nResult == 9107)
			{
				StageWorld.MINEMSG_TYPE = eMINE_MESSAGE.eMINE_MESSAGE_GO_MILITARY_FAIL04;
			}
			else if (packet.nResult == 9560)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("730"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			else
			{
				StageWorld.MINEMSG_TYPE = eMINE_MESSAGE.eMINE_MESSAGE_GO_MILITARY_FAIL02;
			}
		}

		public static void OnPushGuildMember(object EventObject)
		{
			PostDlg postDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.POST_DLG) as PostDlg;
			if (postDlg != null)
			{
				postDlg.SetSendState(PostDlg.eSEND_STATE.eSEND_STATE_NEWGUILD);
			}
		}

		public static void GS_MILITARY_REMOVE_ACK(NkDeserializePacket kDeserializePacket)
		{
		}

		public static void GS_SOLDIER_CHANGE_BATTLEPOS_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLDIER_CHANGE_BATTLEPOS_ACK packet = kDeserializePacket.GetPacket<GS_SOLDIER_CHANGE_BATTLEPOS_ACK>();
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo == null)
			{
				return;
			}
			for (int i = 0; i < 6; i++)
			{
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet.SolID[i]);
				if (soldierInfoFromSolID != null)
				{
					soldierInfoFromSolID.SetBattlePos((short)packet.BattlePos[i]);
				}
			}
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("410"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
		}

		public static void GS_SOLDIER_ACTIVE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLDIER_ACTIVE_ACK packet = kDeserializePacket.GetPacket<GS_SOLDIER_ACTIVE_ACK>();
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo == null)
			{
				return;
			}
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(packet.Index);
			NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
			if (soldierInfo != null && soldierInfo.GetSolID() > 0L && soldierInfo.GetSolID() == packet.ReadySolID)
			{
				soldierInfo.SetSolPosType(0);
				soldierInfo.SetSolPosIndex(0);
				readySolList.AddSolInfo(soldierInfo);
				soldierInfo.Init();
			}
			NkSoldierInfo solInfo = readySolList.GetSolInfo(packet.ActiveSolID);
			if (solInfo != null)
			{
				solInfo.SetSolPosType(1);
				solInfo.SetSolPosIndex((byte)packet.Index);
				soldierInfo.Set(solInfo);
				soldierInfo.SetBattlePos((short)packet.BattlePos);
			}
			readySolList.DelSol(packet.ActiveSolID);
			SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
			if (solMilitaryGroupDlg != null)
			{
				solMilitaryGroupDlg.RefreshSolList();
			}
		}

		public static void GS_SOLDIER_RECRUIT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLDIER_RECRUIT_ACK packet = kDeserializePacket.GetPacket<GS_SOLDIER_RECRUIT_ACK>();
			SolRecruitDlg solRecruitDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLRECRUIT_DLG) as SolRecruitDlg;
			if (solRecruitDlg != null)
			{
				solRecruitDlg.SetRecruitButtonEnable(true);
			}
			if (packet.i8bPosted == 1)
			{
				NkUserInventory.GetInstance().SetInfo(packet.kItem, -1);
				NoticeIconDlg.SetIcon(ICON_TYPE.POST, true);
				if (solRecruitDlg != null)
				{
					solRecruitDlg.SetTicketList();
				}
				if (packet.SolCount == 0)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("685"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
			if (packet.Result != 0)
			{
				NrTSingleton<NkClientLogic>.Instance.SetCanOpenTicket(true);
				return;
			}
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo == null)
			{
				return;
			}
			SOLDIER_INFO sOLDIER_INFO = new SOLDIER_INFO();
			SOLDIER_INFO[] array = new SOLDIER_INFO[packet.SolCount];
			for (int i = 0; i < packet.SolCount; i++)
			{
				SOLDIER_TOTAL_INFO packet2 = kDeserializePacket.GetPacket<SOLDIER_TOTAL_INFO>();
				if (packet2.SOLINFO.SolPosType == 1)
				{
					NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo((int)packet2.SOLINFO.SolPosIndex);
					if (soldierInfo != null)
					{
						if (!soldierInfo.IsValid())
						{
							soldierInfo.Set(packet2.SOLINFO);
							soldierInfo.SetBattleSkillInfo(packet2.BATTLESKILLINFO);
						}
						soldierInfo.SetReceivedEquipItem(true);
						soldierInfo.UpdateSoldierStatInfo();
					}
				}
				else
				{
					NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
					readySolList.AddSolInfo(packet2.SOLINFO, packet2.BATTLESKILLINFO, true);
				}
				sOLDIER_INFO.Set(ref packet2.SOLINFO);
				if (array[i] == null)
				{
					array[i] = new SOLDIER_INFO();
				}
				array[i].Set(ref packet2.SOLINFO);
				NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
				kMyCharInfo.SetCharSolGuide(packet2.SOLINFO.CharKind);
			}
			for (int j = 0; j < packet.SolSubDataCount; j++)
			{
				SOLDIER_SUBDATA packet3 = kDeserializePacket.GetPacket<SOLDIER_SUBDATA>();
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet3.nSolID);
				if (soldierInfoFromSolID != null)
				{
					soldierInfoFromSolID.SetSolSubData(packet3.nSubDataType, packet3.nSubDataValue);
				}
			}
			NrReceiveGame.SolRecruitAfter(sOLDIER_INFO, array, packet.SolCount, packet.RecruitType, packet.kItem, 1 == packet.i8bPosted, null);
			COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
			if (instance == null)
			{
				return;
			}
			int recruitType = packet.RecruitType;
			if (recruitType != 0)
			{
				if (recruitType == 1)
				{
					NrTSingleton<FiveRocksEventManager>.Instance.HeartsConsume(eHEARTS_CONSUME.BUY_HERO, (long)instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CASHCOUNT_FORTEN));
				}
			}
			else
			{
				NrTSingleton<FiveRocksEventManager>.Instance.HeartsConsume(eHEARTS_CONSUME.BUY_HERO, (long)instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CASHCOUNT_FORONE));
			}
			if (NrTSingleton<ItemMallItemManager>.Instance.IsItemVoucherType((eVOUCHER_TYPE)packet.ui8VoucherType))
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.SetVoucherRefreshTime((eVOUCHER_TYPE)packet.ui8VoucherType, packet.i64ItemMallID, packet.i64RefreshTime);
			}
		}

		public static void GS_SOLDIER_EQUIPITEM_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLDIER_EQUIPITEM_NFY packet = kDeserializePacket.GetPacket<GS_SOLDIER_EQUIPITEM_NFY>();
			if (packet.Result == 1)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < 20; i++)
			{
				num += (int)packet.nItemCount[i];
			}
			ITEM[] array = new ITEM[num];
			for (int j = 0; j < num; j++)
			{
				array[j] = kDeserializePacket.GetPacket<ITEM>();
			}
			int num2 = 0;
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser != null)
			{
				for (int k = 0; k < 20; k++)
				{
					if (packet.SolID[k] == 0L)
					{
						num2 += (int)packet.nItemCount[k];
					}
					else
					{
						bool flag = false;
						NkSoldierInfo soldierInfoFromSolID = nrCharUser.GetPersonInfo().GetSoldierInfoFromSolID(packet.SolID[k]);
						if (soldierInfoFromSolID == null)
						{
							num2 += (int)packet.nItemCount[k];
						}
						else
						{
							for (int l = 0; l < (int)packet.nItemCount[k]; l++)
							{
								int num3 = l + num2;
								if (num3 < 0 || num3 >= num)
								{
									break;
								}
								if (soldierInfoFromSolID.SetItem(array[num3]))
								{
									flag = true;
								}
							}
							num2 += (int)packet.nItemCount[k];
							soldierInfoFromSolID.SetReceivedEquipItem(true);
							soldierInfoFromSolID.UpdateSoldierStatInfo();
							if (flag && nrCharUser.GetPersonInfo().GetLeaderSoldierInfo().GetSolID() == packet.SolID[k])
							{
								nrCharUser.ChangeEquipItem();
							}
							if (flag)
							{
								SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
								if (solMilitaryGroupDlg != null)
								{
									solMilitaryGroupDlg.RefreshEquipItem(soldierInfoFromSolID);
								}
							}
						}
					}
				}
			}
		}

		public static void GS_SOLDIER_EQUIPITEM_DURA_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLDIER_EQUIPITEM_DURA_NFY packet = kDeserializePacket.GetPacket<GS_SOLDIER_EQUIPITEM_DURA_NFY>();
			int cnt = packet.Cnt;
			NkItemDuration[] array = new NkItemDuration[cnt];
			for (int i = 0; i < cnt; i++)
			{
				array[i] = kDeserializePacket.GetPacket<NkItemDuration>();
			}
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			for (int j = 0; j < cnt; j++)
			{
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(array[j].m_nSolID);
				if (soldierInfoFromSolID != null)
				{
					for (int k = 0; k < 6; k++)
					{
						if (soldierInfoFromSolID.GetEquipItemInfo().m_kItem[k].GetItemID() > 0L)
						{
							if (soldierInfoFromSolID.GetEquipItemInfo().m_kItem[k].GetItemID() == array[j].m_nItemID)
							{
								soldierInfoFromSolID.GetEquipItemInfo().m_kItem[k].SetDurability(array[j].m_nItemDuration);
							}
						}
					}
				}
			}
		}

		public static void GS_ITEM_EQUIP_DURA_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEM_EQUIP_DURA_NFY packet = kDeserializePacket.GetPacket<GS_ITEM_EQUIP_DURA_NFY>();
			ITEM item = NkUserInventory.GetInstance().GetItem(packet.i8PosType, packet.i16PosItem);
			if (item != null)
			{
				item.m_nDurability = packet.i32Durability;
			}
			NkUserInventory.GetInstance().Inventory_Refresh(packet.i8PosType, packet.i16PosItem);
		}

		public static void GS_CLOSE_SESSION_ACK(NkDeserializePacket kDeserializePacket)
		{
			kDeserializePacket.GetPacket<CLOSE_SESSION_ACK>();
			BaseNet_Game.GetInstance().Quit();
		}

		public static void GS_NOTICE_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_NOTICE_NFY packet = kDeserializePacket.GetPacket<GS_NOTICE_NFY>();
			string text = TKString.NEWString(packet.szTextColor);
			string text2 = TKString.NEWString(packet.Msg);
			string text3 = TKString.NEWString(packet.szForWardText);
			if (text.Length == 0)
			{
				if (text3.Length == 0)
				{
					NrTSingleton<ChatManager>.Instance.PushSystemMsg(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1841"), text2, "303");
				}
				else
				{
					string name = string.Format("[{0}]", text3);
					NrTSingleton<ChatManager>.Instance.PushSystemMsg(name, text2, "303");
				}
				if (text2.Contains("{@"))
				{
					text2 = text2.Replace("{@U", string.Empty);
					text2 = text2.Replace("}", string.Empty);
				}
				if (Battle.BATTLE != null && Battle.BATTLE.ColosseumObserver)
				{
					return;
				}
				Main_UI_SystemMessage.ADDMessage(text2, SYSTEM_MESSAGE_TYPE.ADMIN_SYSTEM_MESSAGE);
			}
			else
			{
				if (text3.Length == 0)
				{
					NrTSingleton<ChatManager>.Instance.PushSystemMsg(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1841"), text2, text);
				}
				else
				{
					string name2 = string.Format("[{0}]", text3);
					NrTSingleton<ChatManager>.Instance.PushSystemMsg(name2, text2, text);
				}
				if (text2.Contains("{@"))
				{
					text2 = text2.Replace("{@U", string.Empty);
					text2 = text2.Replace("}", string.Empty);
				}
				if (Battle.BATTLE != null && Battle.BATTLE.ColosseumObserver)
				{
					return;
				}
				Main_UI_SystemMessage.ADDMessage(text2, SYSTEM_MESSAGE_TYPE.ADMIN_SYSTEM_MESSAGE, text);
			}
		}

		public static void GS_NOTICE_ICON_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_NOTICE_ICON_NFY packet = kDeserializePacket.GetPacket<GS_NOTICE_ICON_NFY>();
			NoticeIconDlg.SetIcon((ICON_TYPE)packet.Type, true);
			if (packet.Type == 4)
			{
				MineGuildCurrentStatusInfoDlg mineGuildCurrentStatusInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MINE_GUILD_CURRENTSTATUSINFO_DLG) as MineGuildCurrentStatusInfoDlg;
				if (mineGuildCurrentStatusInfoDlg != null)
				{
					mineGuildCurrentStatusInfoDlg.RefreshList();
				}
			}
		}

		public static void GS_ENHANCEITEM_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ENHANCEITEM_ACK packet = kDeserializePacket.GetPacket<GS_ENHANCEITEM_ACK>();
			bool flag = true;
			ReduceMainDlg reduceMainDlg;
			if (packet.Result == 0)
			{
				flag = false;
				switch (packet.UpgradeType)
				{
				case 0:
				{
					ReforgeResultDlg reforgeResultDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.REFORGERESULT_DLG) as ReforgeResultDlg;
					reforgeResultDlg.SetData(packet);
					break;
				}
				case 1:
				{
					NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.LeftMoney;
					ItemSkill_Dlg itemSkill_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMSKILL_DLG) as ItemSkill_Dlg;
					if (itemSkill_Dlg != null)
					{
						itemSkill_Dlg.RefrshData();
					}
					ItemSkillResult_Dlg itemSkillResult_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMSKILL_RESULT_DLG) as ItemSkillResult_Dlg;
					if (itemSkillResult_Dlg != null)
					{
						itemSkillResult_Dlg.SetData(packet);
					}
					break;
				}
				case 2:
				{
					ReduceResultDlg reduceResultDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.REDUCERESULT_DLG) as ReduceResultDlg;
					if (reduceResultDlg != null)
					{
						reduceResultDlg.SetResult(packet);
					}
					reduceMainDlg = (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REDUCEMAIN_DLG) as ReduceMainDlg);
					if (reduceMainDlg != null)
					{
						reduceMainDlg.RefreshSelectItemInfo(packet);
					}
					break;
				}
				}
				NrTSingleton<GameGuideManager>.Instance.RemoveEquipGuide();
			}
			if (flag)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("9"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ENHANCE", "FAILURE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				ReforgeSelectDlg reforgeSelectDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REFORGESELECT_DLG) as ReforgeSelectDlg;
				if (reforgeSelectDlg != null)
				{
					reforgeSelectDlg.closeButton.Visible = true;
				}
			}
			reduceMainDlg = (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REDUCEMAIN_DLG) as ReduceMainDlg);
			if (reduceMainDlg != null)
			{
				reduceMainDlg.SetEnable(true);
			}
		}

		public static void GS_ITEMSKILL_REINFORCE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEMSKILL_REINFORCE_ACK packet = kDeserializePacket.GetPacket<GS_ITEMSKILL_REINFORCE_ACK>();
			if (packet.RessultType == -100)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("9"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			else
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.LeftMoney;
				if (packet.RepairType == 0)
				{
					ItemSkill_Dlg itemSkill_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMSKILL_DLG) as ItemSkill_Dlg;
					if (itemSkill_Dlg != null)
					{
						itemSkill_Dlg.RefrshData();
					}
					ItemSkillResult_Dlg itemSkillResult_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMSKILL_RESULT_DLG) as ItemSkillResult_Dlg;
					if (itemSkillResult_Dlg != null)
					{
						itemSkillResult_Dlg.SetItemSkillReinforceData(packet);
					}
				}
				else
				{
					ReduceResultDlg reduceResultDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.REDUCERESULT_DLG) as ReduceResultDlg;
					if (reduceResultDlg != null)
					{
						reduceResultDlg.SetItemrepairResult(packet);
					}
					ReduceMainDlg reduceMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REDUCEMAIN_DLG) as ReduceMainDlg;
					if (reduceMainDlg != null)
					{
						reduceMainDlg.InitRepairData();
					}
				}
			}
		}

		public static void GS_REPAIRITEM_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_REPAIRITEM_ACK packet = kDeserializePacket.GetPacket<GS_REPAIRITEM_ACK>();
			if (packet.Result == 0)
			{
			}
		}

		public static void GS_ITEMEVOLUTION_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEMEVOLUTION_ACK packet = kDeserializePacket.GetPacket<GS_ITEMEVOLUTION_ACK>();
			ItemEvolution_Dlg itemEvolution_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMEVOLUTION_DLG) as ItemEvolution_Dlg;
			if (itemEvolution_Dlg != null)
			{
				itemEvolution_Dlg.RefreshData();
			}
			if (packet.Result == -100)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("9"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			else
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.LeftMoney;
				CompleteBox_DLG completeBox_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COMPLETEBOX_DLG) as CompleteBox_DLG;
				if (completeBox_DLG != null)
				{
					completeBox_DLG.SetEvolutionResultData(packet);
				}
			}
		}

		public static void GS_SELLITEM_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SELLITEM_ACK packet = kDeserializePacket.GetPacket<GS_SELLITEM_ACK>();
			if (packet.Result == 0)
			{
				string empty = string.Empty;
				string text = string.Empty;
				string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.m_SellItemUnique);
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("354");
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "DISASSEMBLE", "SUCCESS", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				long num = packet.m_lMoney - NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money;
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.m_lMoney;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					text,
					"targetname",
					itemNameByItemUnique,
					"count",
					ANNUALIZED.Convert(num)
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
			}
		}

		public static void GS_DISASSEMBLEITEM_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_DISASSEMBLEITEM_ACK packet = kDeserializePacket.GetPacket<GS_DISASSEMBLEITEM_ACK>();
			if (packet.Result == 0)
			{
				ReduceResultDlg reduceResultDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.REDUCERESULT_DLG) as ReduceResultDlg;
				if (reduceResultDlg != null)
				{
					reduceResultDlg.SetResult(packet);
				}
			}
			else if (packet.Result == 31)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46"));
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("487"));
			}
		}

		public static void GS_ITEM_COMPOSE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEM_COMPOSE_ACK packet = kDeserializePacket.GetPacket<GS_ITEM_COMPOSE_ACK>();
			ItemComposeDlg itemComposeDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMCOMPOSE_DLG) as ItemComposeDlg;
			if (itemComposeDlg != null)
			{
				if (packet.m_nResult == 0)
				{
					int nowSelectItemCount = itemComposeDlg.m_NowSelectItemCount;
					itemComposeDlg.ActionCompose(packet.m_CreateItemUnique, nowSelectItemCount);
				}
				itemComposeDlg.InitData();
			}
		}

		public static void GS_CURRENT_MONEY_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_CURRENT_MONEY_NFY packet = kDeserializePacket.GetPacket<GS_CURRENT_MONEY_NFY>();
			if (packet.i32Result == 0)
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64CurrnetMoney;
				if (packet.i8CreateType == 14)
				{
					NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().SetEquipSellMoney(0L, false);
				}
				if (packet.i8CreateType == 21)
				{
					TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MARKET", "BUY", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				}
			}
		}

		public static void GS_CURRENT_CASHINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
		}

		public static void GS_OTHERCHAR_INFO_PERMIT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_OTHERCHAR_INFO_PERMIT_ACK packet = kDeserializePacket.GetPacket<GS_OTHERCHAR_INFO_PERMIT_ACK>();
			if (packet.Result != 0)
			{
				Debug.LogError("NrReceiveGame_Game.cs--GS_OTHERCHAR_INFO_PERMIT_ACK Fail" + packet.Result);
				return;
			}
			DLG_OtherCharDetailInfo dLG_OtherCharDetailInfo = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_OTHER_CHAR_DETAIL) as DLG_OtherCharDetailInfo;
			if (dLG_OtherCharDetailInfo != null)
			{
				long i64TargetPersonID = packet.i64TargetPersonID;
				string szCharName = TKString.NEWString(packet.Name);
				string szIntroMsg = TKString.NEWString(packet.szIntroMsg);
				dLG_OtherCharDetailInfo.SetPersonID(i64TargetPersonID, szCharName, szIntroMsg);
			}
		}

		public static void GS_LOAD_OTHERCHAR_SOLDIER_BASEINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_LOAD_OTHERCHAR_SOLDIER_BASEINFO_ACK packet = kDeserializePacket.GetPacket<GS_LOAD_OTHERCHAR_SOLDIER_BASEINFO_ACK>();
			if (packet.Result != 0)
			{
				return;
			}
			NrSoldierList nrSoldierList = new NrSoldierList();
			int i16SoldierCnt = (int)packet.i16SoldierCnt;
			for (int i = 0; i < i16SoldierCnt; i++)
			{
				SOLDIER_INFO packet2 = kDeserializePacket.GetPacket<SOLDIER_INFO>();
				nrSoldierList.SetSoldierInfo(i, packet2);
			}
			for (int j = 0; j < (int)packet.NumSolSubData; j++)
			{
				SOLDIER_SUBDATA packet3 = kDeserializePacket.GetPacket<SOLDIER_SUBDATA>();
				nrSoldierList.SetSolSubData(packet3);
			}
			DLG_OtherCharDetailInfo dLG_OtherCharDetailInfo = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_OTHER_CHAR_DETAIL) as DLG_OtherCharDetailInfo;
			if (dLG_OtherCharDetailInfo != null)
			{
				dLG_OtherCharDetailInfo.SetSoldierList(nrSoldierList);
			}
		}

		public static void GS_LOAD_OTHERCHAR_SOLDIER_EQUIPINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_LOAD_OTHERCHAR_SOLDIER_EQUIPINFO_ACK packet = kDeserializePacket.GetPacket<GS_LOAD_OTHERCHAR_SOLDIER_EQUIPINFO_ACK>();
			if (packet.i32Result != 0)
			{
				return;
			}
			int i16Cnt = (int)packet.i16Cnt;
			long i64SolID = packet.i64SolID;
			NrEquipItemInfo nrEquipItemInfo = new NrEquipItemInfo();
			for (int i = 0; i < i16Cnt; i++)
			{
				ITEM packet2 = kDeserializePacket.GetPacket<ITEM>();
				nrEquipItemInfo.SetEquipItem(packet2);
			}
			DLG_OtherCharDetailInfo dLG_OtherCharDetailInfo = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_OTHER_CHAR_DETAIL) as DLG_OtherCharDetailInfo;
			if (dLG_OtherCharDetailInfo != null)
			{
				nrEquipItemInfo.SetReceiveData(true);
				dLG_OtherCharDetailInfo.SetSoldierEquipItem(i64SolID, nrEquipItemInfo);
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_OTHER_CHAR_EQUIPMENT);
			}
		}

		public static void GS_LOAD_MONSTER_SOLDIER_BASEINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_LOAD_MONSTER_SOLDIER_BASEINFO_ACK packet = kDeserializePacket.GetPacket<GS_LOAD_MONSTER_SOLDIER_BASEINFO_ACK>();
			if (packet.Result != 0)
			{
				return;
			}
		}

		public static void GS_ACCOUNT_WORLD_MOVE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ACCOUNT_WORLD_MOVE_ACK packet = kDeserializePacket.GetPacket<GS_ACCOUNT_WORLD_MOVE_ACK>();
			if (packet.Result != 0)
			{
				string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94");
				string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("31");
				string message = string.Empty;
				message = textFromMessageBox2 + "  [ERRORCODE] = " + packet.Result.ToString();
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				msgBoxUI.SetMsg(null, null, textFromMessageBox, message, eMsgType.MB_OK, 2);
				return;
			}
			NrTSingleton<CMovingServer>.Instance.SetMovingWorldInfo(packet.m_nUID, packet.i64MovingWorld_KEY, TKString.NEWString(packet.szMoveFrontServerIP), packet.i16Port, packet.m_nCHMoveTargetPersonID, packet.m_nCHMoveType, packet.i8AgitMove);
			MsgHandler.Handle("Rcv_ServerMoveResetStage", new object[0]);
			NrTSingleton<CMovingServer>.Instance.ReqMovingCharInit = true;
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo != null)
			{
				myCharInfo.SetAutoBattle(E_BF_AUTO_TYPE.MANUAL);
			}
		}

		public static void GS_EQUIPSELLMONEY_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_EQUIPSELLMONEY_ACK packet = kDeserializePacket.GetPacket<GS_EQUIPSELLMONEY_ACK>();
			NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().SetEquipSellMoney(packet.m_nMoney, false);
		}

		public static void GS_ACCOUNT_CHANNEL_MOVE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ACCOUNT_CHANNEL_MOVE_ACK packet = kDeserializePacket.GetPacket<GS_ACCOUNT_CHANNEL_MOVE_ACK>();
			Debug.Log("GS_ACCOUNT_CHANNEL_MOVE_ACK [result:" + packet.Result.ToString() + "]");
			if (packet.Result != 0)
			{
				return;
			}
			NrTSingleton<CMovingServer>.Instance.SetMovingChannelInfo(NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_UID, NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID);
			MsgHandler.Handle("Rcv_ServerMoveResetStage", new object[0]);
			NrTSingleton<CMovingServer>.Instance.ReqMovingCharInit = true;
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo != null)
			{
				myCharInfo.SetAutoBattle(E_BF_AUTO_TYPE.MANUAL);
			}
			Debug.Log(string.Concat(new string[]
			{
				"GS_ACCOUNT_CHANNEL_MOVE_ACK [",
				NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_UID.ToString(),
				" / ",
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID.ToString(),
				"]"
			}));
		}

		public static void GS_PING_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_PING_ACK packet = kDeserializePacket.GetPacket<GS_PING_ACK>();
			float num = Time.realtimeSinceStartup - packet.fPingSendTime;
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.DLG_FPS))
			{
				DLG_FPS dLG_FPS = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_FPS) as DLG_FPS;
				if (dLG_FPS != null)
				{
					dLG_FPS.m_fFpsSec = num;
					dLG_FPS.m_dwGSTick = packet.m_GSTick;
					dLG_FPS.m_dwWSTick = packet.m_WSTick;
				}
			}
			if (num > 1f || packet.m_GSTick > 100 || packet.m_WSTick > 500)
			{
				string strLog = string.Format("PING : {0}, GS : {1}, WS : {2}", num * 1000f, packet.m_GSTick, packet.m_WSTick);
				TsPlatform.FileLog(strLog);
			}
			if (num > 3f)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("169"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}

		public static void GS_SET_USERPLAYSTATE_CHANGE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SET_USERPLAYSTATE_CHANGE_ACK packet = kDeserializePacket.GetPacket<GS_SET_USERPLAYSTATE_CHANGE_ACK>();
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_byUserPlayState = packet.i8UserPlayState;
		}

		public static void GS_DIFFICULTY_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_DIFFICULTY_SET_ACK packet = kDeserializePacket.GetPacket<GS_DIFFICULTY_SET_ACK>();
			if (packet.i32Result == 0)
			{
				NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
				if (@char != null)
				{
					NrPersonInfoUser nrPersonInfoUser = @char.GetPersonInfo() as NrPersonInfoUser;
					nrPersonInfoUser.Difficulty = packet.i8Difficulty;
					NrTSingleton<NkCharManager>.Instance.RefreshCharName();
				}
			}
		}

		public static void GS_CLEAR_ALL_SUBCHAR_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_CLEAR_ALL_SUBCHAR_NFY packet = kDeserializePacket.GetPacket<GS_CLEAR_ALL_SUBCHAR_NFY>();
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(packet.i16CharUnique) as NrCharUser;
			if (nrCharUser != null)
			{
				nrCharUser.DeleteSubCharAll();
			}
		}

		public static void GS_QUEST_GET_CHAR_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_QUEST_GET_CHAR_ACK packet = kDeserializePacket.GetPacket<GS_QUEST_GET_CHAR_ACK>();
			if (packet.Result == 0)
			{
				NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(packet.i16CharUnique) as NrCharUser;
				NrTSingleton<NkQuestManager>.Instance.SubCharProcess(packet.i16CharUnique, packet.i32CharKind, packet.ui8rCharKindSlot, 3);
				if (nrCharUser != null && nrCharUser.GetID() == 1)
				{
					NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64rCharMoney_Cur;
				}
			}
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TAKETALK_DLG);
			NrTSingleton<NkQuestManager>.Instance.UpdateClientNpc(0);
		}

		public static void GS_SET_SOLDIER_INITIATIVE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SET_SOLDIER_INITIATIVE_ACK packet = kDeserializePacket.GetPacket<GS_SET_SOLDIER_INITIATIVE_ACK>();
			if (packet.nResult == 0)
			{
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (charPersonInfo != null)
				{
					NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet.nSolID);
					if (soldierInfoFromSolID != null)
					{
						soldierInfoFromSolID.SetInitiativeValue(packet.nInitiativeValue);
					}
					SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
					if (solMilitaryGroupDlg != null)
					{
						solMilitaryGroupDlg.ChangeShowSoldier(soldierInfoFromSolID);
					}
				}
			}
		}

		public static void GS_SET_SOLDIER_BATTLESKILL_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SET_SOLDIER_BATTLESKILL_ACK packet = kDeserializePacket.GetPacket<GS_SET_SOLDIER_BATTLESKILL_ACK>();
			BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(packet.nBattleSkillUnique);
			if (battleSkillBase == null)
			{
				return;
			}
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey);
			if (packet.nResult == 0)
			{
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (charPersonInfo != null)
				{
					string empty = string.Empty;
					if (textFromInterface != null && packet.bBattleSkillMessageShow)
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("340"),
							"skillname",
							textFromInterface,
							"skilllevel",
							packet.nBattleSkillLevel
						});
						Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
						NrSound.ImmedatePlay("UI_SFX", "PRODUCTION", "GET_SKILL");
					}
					NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet.nSolID);
					if (soldierInfoFromSolID != null)
					{
						soldierInfoFromSolID.SetBattleSkillData(packet.nBattleSkillIndex, packet.nBattleSkillUnique, packet.nBattleSkillLevel);
						SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
						if (solMilitaryGroupDlg != null)
						{
							solMilitaryGroupDlg.RefreshSkillInfo(soldierInfoFromSolID);
						}
						SolSkillUpdateDlg solSkillUpdateDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLSKILLUPDATE_DLG) as SolSkillUpdateDlg;
						if (solSkillUpdateDlg != null)
						{
							if (battleSkillBase.m_nMythSkillType == 1)
							{
								solSkillUpdateDlg.SetData(ref soldierInfoFromSolID, packet.nBattleSkillUnique, true);
							}
							else
							{
								solSkillUpdateDlg.SetData(ref soldierInfoFromSolID, packet.nBattleSkillUnique, false);
							}
						}
					}
					NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.lLeftMoney;
					if (battleSkillBase.m_nSkillType == 2)
					{
						NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
						if (nrCharUser == null)
						{
							return;
						}
						NkSoldierInfo soldierInfoFromSolID2 = nrCharUser.GetPersonInfo().GetSoldierInfoFromSolID(packet.nSolID);
						if (soldierInfoFromSolID2 == null)
						{
							return;
						}
						soldierInfoFromSolID2.UpdateSoldierStatInfo();
					}
					BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
					if (bookmarkDlg != null)
					{
						bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.HERO);
					}
					HeroCollect_DLG heroCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.HEROCOLLECT_DLG) as HeroCollect_DLG;
					if (heroCollect_DLG != null)
					{
						heroCollect_DLG.Update_Notice();
					}
					SolMilitaryGroupDlg solMilitaryGroupDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
					if (solMilitaryGroupDlg2 != null)
					{
						solMilitaryGroupDlg2.CheckSkillUpSolNum();
					}
					if (soldierInfoFromSolID != null)
					{
						soldierInfoFromSolID.UpdateSoldierStatInfo();
					}
				}
			}
			else
			{
				string text = string.Empty;
				string empty2 = string.Empty;
				switch (packet.nResult)
				{
				case 4004:
					break;
				case 4005:
					if (textFromInterface != null)
					{
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("344");
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
						{
							text,
							"skillname",
							textFromInterface,
							"skilllevel",
							packet.nBattleSkillLevel
						});
					}
					break;
				case 4006:
					if (textFromInterface != null)
					{
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("341");
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
						{
							text,
							"skillname",
							textFromInterface,
							"skilllevel",
							packet.nBattleSkillLevel
						});
					}
					break;
				case 4007:
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("343");
					int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_MYTH_SKILL_ITEMUNIQUE);
					string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(value);
					if (itemNameByItemUnique != null)
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
						{
							text,
							"itemname",
							itemNameByItemUnique,
							"skillname",
							textFromInterface,
							"skilllevel",
							packet.nBattleSkillLevel
						});
					}
					else
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
						{
							text,
							"itemname",
							"None-Itemname",
							"skillname",
							textFromInterface,
							"skilllevel",
							packet.nBattleSkillLevel
						});
					}
					break;
				}
				case 4008:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("345");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
					{
						text
					});
					Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					break;
				default:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("345");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
					{
						text
					});
					Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					break;
				}
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PRODUCTION", "LEVELUP-CANCLE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}

		public static void GS_SET_SOLDIER_MYTH_BATTLESKILL_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SET_SOLDIER_MYTH_BATTLESKILL_ACK packet = kDeserializePacket.GetPacket<GS_SET_SOLDIER_MYTH_BATTLESKILL_ACK>();
			BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(packet.nBattleSkillUnique);
			if (battleSkillBase == null)
			{
				return;
			}
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey);
			if (packet.nResult == 0)
			{
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (charPersonInfo != null)
				{
					string empty = string.Empty;
					if (textFromInterface != null && packet.bBattleSkillMessageShow)
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("340"),
							"skillname",
							textFromInterface,
							"skilllevel",
							packet.nBattleSkillLevel
						});
						Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
						NrSound.ImmedatePlay("UI_SFX", "PRODUCTION", "GET_SKILL");
					}
					NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet.nSolID);
					if (soldierInfoFromSolID != null)
					{
						soldierInfoFromSolID.SetBattleSkillData(packet.nBattleSkillIndex, packet.nBattleSkillUnique, packet.nBattleSkillLevel);
						if (packet.nBattleSkillUnique_Second > 0)
						{
							soldierInfoFromSolID.SetBattleSkillData(packet.nBattleSkillIndex_Second, packet.nBattleSkillUnique_Second, packet.nBattleSkillLevel_Second);
						}
						soldierInfoFromSolID.UpdateSoldierStatInfo();
						SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
						if (solMilitaryGroupDlg != null)
						{
							solMilitaryGroupDlg.RefreshSkillInfo(soldierInfoFromSolID);
						}
						SolSkillUpdateDlg solSkillUpdateDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLSKILLUPDATE_DLG) as SolSkillUpdateDlg;
						if (solSkillUpdateDlg != null)
						{
							if (battleSkillBase.m_nMythSkillType == 1)
							{
								solSkillUpdateDlg.SetData(ref soldierInfoFromSolID, packet.nBattleSkillUnique, true);
							}
							else
							{
								solSkillUpdateDlg.SetData(ref soldierInfoFromSolID, packet.nBattleSkillUnique, false);
							}
						}
					}
					BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
					if (bookmarkDlg != null)
					{
						bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.HERO);
					}
					HeroCollect_DLG heroCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.HEROCOLLECT_DLG) as HeroCollect_DLG;
					if (heroCollect_DLG != null)
					{
						heroCollect_DLG.Update_Notice();
					}
					SolMilitaryGroupDlg solMilitaryGroupDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
					if (solMilitaryGroupDlg2 != null)
					{
						solMilitaryGroupDlg2.CheckSkillUpSolNum();
					}
					if (soldierInfoFromSolID != null)
					{
						soldierInfoFromSolID.UpdateSoldierStatInfo();
					}
				}
			}
			else
			{
				string text = string.Empty;
				string empty2 = string.Empty;
				switch (packet.nResult)
				{
				case 4004:
					break;
				case 4005:
					if (textFromInterface != null)
					{
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("344");
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
						{
							text,
							"skillname",
							textFromInterface,
							"skilllevel",
							packet.nBattleSkillLevel
						});
					}
					break;
				case 4006:
					if (textFromInterface != null)
					{
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("341");
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
						{
							text,
							"skillname",
							textFromInterface,
							"skilllevel",
							packet.nBattleSkillLevel
						});
					}
					break;
				case 4007:
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("343");
					int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_MYTH_SKILL_ITEMUNIQUE);
					string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(value);
					if (itemNameByItemUnique != null)
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
						{
							text,
							"itemname",
							itemNameByItemUnique,
							"skillname",
							textFromInterface,
							"skilllevel",
							packet.nBattleSkillLevel
						});
					}
					else
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
						{
							text,
							"itemname",
							"None-Itemname",
							"skillname",
							textFromInterface,
							"skilllevel",
							packet.nBattleSkillLevel
						});
					}
					break;
				}
				case 4008:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("345");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
					{
						text
					});
					Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					break;
				default:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("345");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
					{
						text
					});
					Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					break;
				}
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PRODUCTION", "LEVELUP-CANCLE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}

		public static void GS_CHATCHANNEL_CHANGE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_CHATCHANNEL_CHANGE_ACK packet = kDeserializePacket.GetPacket<GS_CHATCHANNEL_CHANGE_ACK>();
			if (packet.i32Result == 0)
			{
				NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
				int chatChannel = kMyCharInfo.ChatChannel;
				kMyCharInfo.ChatChannel = packet.i32Change_ChatChannel;
				if (chatChannel == 301)
				{
					MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
					if (msgBoxUI != null)
					{
						string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2577");
						string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("326");
						msgBoxUI.SetMsg(null, null, textFromInterface, textFromMessageBox, eMsgType.MB_OK, 2);
					}
				}
				string text = string.Empty;
				string empty = string.Empty;
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("821");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					text,
					"count",
					kMyCharInfo.ChatChannel
				});
				NrTSingleton<ChatManager>.Instance.PushMsg(CHAT_TYPE.SYSTEM, "[" + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("556") + "]", empty);
				ChatMobile_Sub_Dlg chatMobile_Sub_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MOBILE_SUB_DLG) as ChatMobile_Sub_Dlg;
				if (chatMobile_Sub_Dlg != null)
				{
					chatMobile_Sub_Dlg.SetChannelText();
				}
			}
		}

		public static void GS_CONTENTSLIMIT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_CONTENTSLIMIT_ACK packet = kDeserializePacket.GetPacket<GS_CONTENTSLIMIT_ACK>();
			if (0 < packet.i16Count)
			{
				NrTSingleton<ContentsLimitManager>.Instance.Clear();
			}
			for (int i = 0; i < (int)packet.i16Count; i++)
			{
				CONTENTSLIMIT_DATA packet2 = kDeserializePacket.GetPacket<CONTENTSLIMIT_DATA>();
				NrTSingleton<ContentsLimitManager>.Instance.AddLimitData(packet2);
			}
			if (packet.i8Reload == 1)
			{
				NrTSingleton<ContentsLimitManager>.Instance.SetReload();
			}
			List<KeyValuePair<long, SolCombinationInfo_Data>> combinationInfoSortedSorList = NrTSingleton<NrSolCombinationSkillInfoManager>.Instance.GetCombinationInfoSortedSorList();
			foreach (KeyValuePair<long, SolCombinationInfo_Data> current in combinationInfoSortedSorList)
			{
				SolCombinationInfo_Data value = current.Value;
				if (value != null)
				{
					List<string> solCombinationCharCodeList = value.GetSolCombinationCharCodeList();
					foreach (string current2 in solCombinationCharCodeList)
					{
						int charKindByCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(current2);
						if (NrTSingleton<ContentsLimitManager>.Instance.IsSoldierRecruit(charKindByCode))
						{
							current.Value.m_nCombinationIsShow = 0;
						}
					}
				}
			}
		}

		public static void GS_COMMONCONSTANT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_COMMONCONSTANT_ACK packet = kDeserializePacket.GetPacket<GS_COMMONCONSTANT_ACK>();
			for (int i = 1; i < 164; i++)
			{
				COMMON_CONSTANT_Manager.GetInstance().SetData((eCOMMON_CONSTANT)i, packet.i32Constant[i]);
			}
		}

		public static void GS_COUPON_USE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_COUPON_USE_ACK packet = kDeserializePacket.GetPacket<GS_COUPON_USE_ACK>();
			if (packet.i32Result == 0)
			{
				NrTSingleton<FormsManager>.Instance.ShowMessageBox(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("917"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("84"), eMsgType.MB_OK, null, null);
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.ShowMessageBox(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("923"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("85"), eMsgType.MB_OK, null, null);
			}
		}

		public static void GS_CHANGE_CLASS_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_CHANGE_CLASS_ACK packet = kDeserializePacket.GetPacket<GS_CHANGE_CLASS_ACK>();
			if (packet.i32Result == 0)
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64Money;
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (charPersonInfo != null)
				{
					NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet.i64SolID);
					if (soldierInfoFromSolID != null)
					{
						soldierInfoFromSolID.SetCharKind(packet.i32CharKind);
						for (int i = 0; i < 6; i++)
						{
							soldierInfoFromSolID.SetBattleSkillData(i, packet.i32SkillUnique[i], (int)packet.i16SkillLevel[i]);
						}
						SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
						if (solMilitaryGroupDlg != null)
						{
							solMilitaryGroupDlg.RefreshSolList();
						}
						if (soldierInfoFromSolID.IsLeader())
						{
							NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
							if (nrCharUser != null)
							{
								if (0 >= nrCharUser.GetFaceCharKind())
								{
									nrCharUser.SetClassChage(packet.i32CharKind);
								}
								else
								{
									nrCharUser.SetCharKind(packet.i32CharKind, false);
								}
							}
						}
					}
				}
				CharChangeMainDlg charChangeMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHARCHANGEMAIN_DLG) as CharChangeMainDlg;
				if (charChangeMainDlg != null)
				{
					charChangeMainDlg.SetEnableControl(false);
					charChangeMainDlg.ShowResultEffect(packet.i32CharKind);
				}
			}
		}

		public static void SolRecruitAfter(SOLDIER_INFO solinfo, SOLDIER_INFO[] solArray, int iSolCount, int RecruitType, ITEM _item, bool bRcvRemainSolPost = false, NkSoldierInfo paramSolinfo = null)
		{
			SolRecruitSuccessDlg solRecruitSuccessDlg = (SolRecruitSuccessDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLRECRUITSUCCESS_DLG);
			SolRecruitSuccessDlg.eBundleIndexType eBundleIndexType = solRecruitSuccessDlg.CheckBundleIndex(RecruitType);
			if (solRecruitSuccessDlg != null)
			{
				if (iSolCount == 1)
				{
					if (RecruitType == 0 || RecruitType == 1 || RecruitType == 22)
					{
						SolRecruitSuccess_RenewalDlg solRecruitSuccess_RenewalDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLRECRUITSUCCESS_RENEWAL_DLG) as SolRecruitSuccess_RenewalDlg;
						if (solRecruitSuccess_RenewalDlg != null)
						{
							solRecruitSuccess_RenewalDlg.SetList(solinfo, RecruitType);
						}
					}
					else
					{
						solRecruitSuccessDlg.StartSoldierGetBundle(RecruitType, iSolCount);
						solRecruitSuccessDlg.SetImage(solinfo, paramSolinfo);
					}
				}
				else if (1 < iSolCount)
				{
					if (eBundleIndexType == SolRecruitSuccessDlg.eBundleIndexType.BI_NOMAL && RecruitType != 1)
					{
						SolRecruitSuccessGroupDlg solRecruitSuccessGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLRECRUITSUCCESS_GROUP_DLG) as SolRecruitSuccessGroupDlg;
						if (solRecruitSuccessGroupDlg != null)
						{
							solRecruitSuccessGroupDlg.Close();
						}
						solRecruitSuccessGroupDlg = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLRECRUITSUCCESS_GROUP_DLG) as SolRecruitSuccessGroupDlg);
						if (solRecruitSuccessGroupDlg != null)
						{
							solRecruitSuccessGroupDlg.SetList(solArray, NrTSingleton<ItemManager>.Instance.GetName(_item));
						}
					}
					else if (RecruitType == 1)
					{
						SolRecruitSuccess_RenewalDlg solRecruitSuccess_RenewalDlg2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLRECRUITSUCCESS_RENEWAL_DLG) as SolRecruitSuccess_RenewalDlg;
						if (solRecruitSuccess_RenewalDlg2 != null)
						{
							solRecruitSuccess_RenewalDlg2.SetList(solArray);
						}
					}
					else
					{
						solRecruitSuccessDlg.StartSoldierGetBundle(RecruitType, iSolCount);
						solRecruitSuccessDlg.SetImage(solArray);
					}
				}
				ItemBoxContinue_Dlg itemBoxContinue_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEM_BOX_CONTINUE_DLG) as ItemBoxContinue_Dlg;
				if (itemBoxContinue_Dlg != null)
				{
					itemBoxContinue_Dlg.SetItemData(_item, ItemBoxContinue_Dlg.SHOW_TYPE.ITEM_TICKET);
				}
				solRecruitSuccessDlg.SetNotifyByRemainSolPost(bRcvRemainSolPost);
			}
			SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
			if (solMilitaryGroupDlg != null)
			{
				solMilitaryGroupDlg.RefreshSolList();
			}
			NrTSingleton<FiveRocksEventManager>.Instance.SolRecruit(iSolCount);
		}

		public static void GS_ELEMENT_SOL_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ELEMENT_SOL_GET_ACK packet = kDeserializePacket.GetPacket<GS_ELEMENT_SOL_GET_ACK>();
			if (packet.i32Result != 0)
			{
				TsLog.LogWarning(" GS_ELEMENT_SOL_GET_ACK Error ===> {0}", new object[]
				{
					packet.i32Result
				});
				return;
			}
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo == null)
			{
				return;
			}
			for (int i = 0; i < 5; i++)
			{
				NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
				readySolList.DelSol(packet.i64SolID[i]);
			}
			SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
			if (solMilitaryGroupDlg != null)
			{
				solMilitaryGroupDlg.RefreshSolList();
			}
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64CurrentMoney;
			SOLDIER_INFO sOLDIER_INFO = new SOLDIER_INFO();
			SOLDIER_INFO[] array = new SOLDIER_INFO[packet.SolCount];
			for (int i = 0; i < packet.SolCount; i++)
			{
				SOLDIER_TOTAL_INFO packet2 = kDeserializePacket.GetPacket<SOLDIER_TOTAL_INFO>();
				if (packet2.SOLINFO.SolPosType == 1)
				{
					NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo((int)packet2.SOLINFO.SolPosIndex);
					if (soldierInfo != null)
					{
						if (!soldierInfo.IsValid())
						{
							soldierInfo.Set(packet2.SOLINFO);
							soldierInfo.SetBattleSkillInfo(packet2.BATTLESKILLINFO);
						}
						soldierInfo.SetReceivedEquipItem(true);
						soldierInfo.UpdateSoldierStatInfo();
					}
				}
				else
				{
					NkReadySolList readySolList2 = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
					readySolList2.AddSolInfo(packet2.SOLINFO, packet2.BATTLESKILLINFO, true);
				}
				sOLDIER_INFO.Set(ref packet2.SOLINFO);
				if (array[i] == null)
				{
					array[i] = new SOLDIER_INFO();
				}
				array[i].Set(ref packet2.SOLINFO);
				NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
				kMyCharInfo.SetCharSolGuide(packet2.SOLINFO.CharKind);
			}
			for (int i = 0; i < packet.SolSubDataCount; i++)
			{
				SOLDIER_SUBDATA packet3 = kDeserializePacket.GetPacket<SOLDIER_SUBDATA>();
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet3.nSolID);
				if (soldierInfoFromSolID != null)
				{
					soldierInfoFromSolID.SetSolSubData(packet3.nSubDataType, packet3.nSubDataValue);
				}
			}
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLDETAIL_DLG))
			{
				SolDetail_Info_Dlg solDetail_Info_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLDETAIL_DLG) as SolDetail_Info_Dlg;
				solDetail_Info_Dlg.ElementSolSuccess(sOLDIER_INFO, false);
			}
		}

		public static void GS_ELEMENT_LEGENDSOL_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ELEMENT_LEGENDSOL_GET_ACK packet = kDeserializePacket.GetPacket<GS_ELEMENT_LEGENDSOL_GET_ACK>();
			if (packet.i32Result != 0)
			{
				TsLog.LogWarning(" GS_ELEMENT_LEGENDSOL_GET_ACK Error ===> {0}", new object[]
				{
					packet.i32Result
				});
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				return;
			}
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo == null)
			{
				return;
			}
			for (int i = 0; i < 5; i++)
			{
				NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
				readySolList.DelSol(packet.i64SolID[i]);
			}
			SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
			if (solMilitaryGroupDlg != null)
			{
				solMilitaryGroupDlg.RefreshSolList();
			}
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64CurrentMoney;
			SOLDIER_INFO sOLDIER_INFO = new SOLDIER_INFO();
			SOLDIER_INFO[] array = new SOLDIER_INFO[packet.SolCount];
			for (int i = 0; i < packet.SolCount; i++)
			{
				SOLDIER_TOTAL_INFO packet2 = kDeserializePacket.GetPacket<SOLDIER_TOTAL_INFO>();
				if (packet2.SOLINFO.SolPosType == 1)
				{
					NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo((int)packet2.SOLINFO.SolPosIndex);
					if (soldierInfo != null)
					{
						if (!soldierInfo.IsValid())
						{
							soldierInfo.Set(packet2.SOLINFO);
							soldierInfo.SetBattleSkillInfo(packet2.BATTLESKILLINFO);
						}
						soldierInfo.SetReceivedEquipItem(true);
						soldierInfo.UpdateSoldierStatInfo();
					}
				}
				else
				{
					NkReadySolList readySolList2 = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
					readySolList2.AddSolInfo(packet2.SOLINFO, packet2.BATTLESKILLINFO, true);
				}
				sOLDIER_INFO.Set(ref packet2.SOLINFO);
				if (array[i] == null)
				{
					array[i] = new SOLDIER_INFO();
				}
				array[i].Set(ref packet2.SOLINFO);
				NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
				kMyCharInfo.SetCharSolGuide(packet2.SOLINFO.CharKind);
			}
			for (int i = 0; i < packet.SolSubDataCount; i++)
			{
				SOLDIER_SUBDATA packet3 = kDeserializePacket.GetPacket<SOLDIER_SUBDATA>();
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet3.nSolID);
				if (soldierInfoFromSolID != null)
				{
					soldierInfoFromSolID.SetSolSubData(packet3.nSubDataType, packet3.nSubDataValue);
				}
			}
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MYTH_LEGEND_INFO_DLG))
			{
				Myth_Legend_Info_DLG myth_Legend_Info_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTH_LEGEND_INFO_DLG) as Myth_Legend_Info_DLG;
				myth_Legend_Info_DLG.ElementSolSuccess(sOLDIER_INFO);
			}
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MYTH_EVOLUTION_MAIN_DLG))
			{
				Myth_Evolution_Main_DLG myth_Evolution_Main_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTH_EVOLUTION_MAIN_DLG) as Myth_Evolution_Main_DLG;
				myth_Evolution_Main_DLG.SetLegend();
			}
		}

		public static void GS_ENHANCEITEM_EXTRA_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ENHANCEITEM_EXTRA_ACK packet = kDeserializePacket.GetPacket<GS_ENHANCEITEM_EXTRA_ACK>();
			bool flag = true;
			if (packet.Result == 0)
			{
				flag = false;
				ItemSkill_Dlg itemSkill_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMSKILL_DLG) as ItemSkill_Dlg;
				if (itemSkill_Dlg != null)
				{
					itemSkill_Dlg.RefrshData();
					if (packet.SrcPosType != 10)
					{
						itemSkill_Dlg.UpdateData(packet.SrcItemPos, packet.SrcPosType, 0L);
					}
				}
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("736"));
			}
			if (flag)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("9"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ENHANCE", "FAILURE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			}
		}

		public static void GS_TIMESHOP_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_TIMESHOP_ACK packet = kDeserializePacket.GetPacket<GS_TIMESHOP_ACK>();
			for (int i = 0; i < (int)packet.i16Count; i++)
			{
				TIMESHOP_SERVERDATA packet2 = kDeserializePacket.GetPacket<TIMESHOP_SERVERDATA>();
				NrTSingleton<NrTableTimeShopManager>.Instance.Load_ServerValue(packet2);
			}
			if (packet.i8Reload == 0)
			{
				if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
				{
					return;
				}
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.Set_UserTimeShopInfo(packet.i16RefreshCount, packet.i64RefreshEndTime);
			}
		}

		public static void GS_PUSH_BLOCK_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_PUSH_BLOCK_GET_ACK packet = kDeserializePacket.GetPacket<GS_PUSH_BLOCK_GET_ACK>();
			if (packet.i32Result == 0)
			{
				System_Option_Dlg system_Option_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SYSTEM_OPTION) as System_Option_Dlg;
				if (system_Option_Dlg != null)
				{
					system_Option_Dlg.SetPushSetting(packet.byNotice, packet.byFriend, packet.byGuild, false);
				}
			}
		}

		public static void GS_PUSH_BLOCK_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_PUSH_BLOCK_SET_ACK packet = kDeserializePacket.GetPacket<GS_PUSH_BLOCK_SET_ACK>();
			if (packet.i32Result == 0)
			{
				System_Option_Dlg system_Option_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SYSTEM_OPTION) as System_Option_Dlg;
				if (system_Option_Dlg != null)
				{
					system_Option_Dlg.SetPushSetting(packet.byNotice, packet.byFriend, packet.byGuild, true);
				}
			}
		}

		public static void GS_GUILDWAR_MATCH_LIST_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_GUILDWAR_MATCH_LIST_ACK packet = kDeserializePacket.GetPacket<GS_GUILDWAR_MATCH_LIST_ACK>();
			GuildWarListDlg guildWarListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GUILDWAR_LIST_DLG) as GuildWarListDlg;
			if (guildWarListDlg == null)
			{
				return;
			}
			guildWarListDlg.ClaerList();
			guildWarListDlg.SetPageText((int)packet.i16CurPage, (int)packet.i16MaxPage);
			guildWarListDlg.SetTimeSate(packet.i8TimeState);
			for (int i = 0; i < (int)packet.ui8Count; i++)
			{
				GUILDWAR_MATCH_INFO packet2 = kDeserializePacket.GetPacket<GUILDWAR_MATCH_INFO>();
				guildWarListDlg.AddInfo(packet2);
			}
			guildWarListDlg.SetList();
		}

		public static void GS_GUILDWAR_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_GUILDWAR_INFO_ACK packet = kDeserializePacket.GetPacket<GS_GUILDWAR_INFO_ACK>();
			NrTSingleton<MineManager>.Instance.ClearDlg();
			NrTSingleton<GuildWarManager>.Instance.ClearDlg();
			GuildWarMainDlg guildWarMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GUILDWAR_MAIN_DLG) as GuildWarMainDlg;
			if (guildWarMainDlg == null)
			{
				return;
			}
			guildWarMainDlg.SetInfo(packet);
		}

		public static void GS_GUILDWAR_APPLY_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_GUILDWAR_APPLY_ACK packet = kDeserializePacket.GetPacket<GS_GUILDWAR_APPLY_ACK>();
			if (packet.i32Result == 0)
			{
				if (packet.bIsApply && !packet.bIsCancelReservation)
				{
					if (!NrTSingleton<GuildWarManager>.Instance.bIsGuildWar)
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("828"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					}
					else
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("832"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					}
				}
				else if (packet.bIsApply && packet.bIsCancelReservation)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("829"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
				else if (!packet.bIsApply)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("835"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
				NrTSingleton<GuildWarManager>.Instance.bIsGuildWar = packet.bIsApply;
				NrTSingleton<GuildWarManager>.Instance.bIsGuildWarCancelReservation = packet.bIsCancelReservation;
				GuildWarMainDlg guildWarMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GUILDWAR_MAIN_DLG) as GuildWarMainDlg;
				if (guildWarMainDlg != null)
				{
					guildWarMainDlg.SetApplyButton();
				}
				NrCharUser nrCharUser = (NrCharUser)NrTSingleton<NkCharManager>.Instance.GetChar(1);
				if (nrCharUser != null)
				{
					if (0 < NrTSingleton<NewGuildManager>.Instance.GetSetImage())
					{
						nrCharUser.SetUserGuildName(NrTSingleton<NewGuildManager>.Instance.GetGuildName(), NrTSingleton<NewGuildManager>.Instance.GetGuildID(), NrTSingleton<NewGuildManager>.Instance.IsGuildWar());
					}
					else
					{
						nrCharUser.SetUserGuildName(NrTSingleton<NewGuildManager>.Instance.GetGuildName(), 0L, NrTSingleton<NewGuildManager>.Instance.IsGuildWar());
					}
				}
			}
			else
			{
				int i32Result = packet.i32Result;
				if (i32Result != 9561)
				{
					if (i32Result != 9562)
					{
						if (i32Result != 9500)
						{
							Main_UI_SystemMessage.ADDMessage(string.Format("GuildWar Apply Error: {0}", packet.i32Result), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
						}
						else
						{
							Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("831"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
						}
					}
					else
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("833"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					}
				}
				else
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("833"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
		}

		public static void GS_GUILDWAR_IS_WAR_TIME_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_GUILDWAR_IS_WAR_TIME_ACK packet = kDeserializePacket.GetPacket<GS_GUILDWAR_IS_WAR_TIME_ACK>();
			MineSearchDlg mineSearchDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MINE_SEARCH_DLG) as MineSearchDlg;
			if (mineSearchDlg != null)
			{
				mineSearchDlg.SetGrade(5, packet.bIsWarTime);
			}
		}

		public static void GS_GUILDWAR_RANKINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_GUILDWAR_RANKINFO_ACK packet = kDeserializePacket.GetPacket<GS_GUILDWAR_RANKINFO_ACK>();
			NewGuildWarRewardInfoDlg newGuildWarRewardInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GUILDWAR_REWARDINFO_DLG) as NewGuildWarRewardInfoDlg;
			if (newGuildWarRewardInfoDlg != null)
			{
				newGuildWarRewardInfoDlg.ClearGuildData();
			}
			newGuildWarRewardInfoDlg.SetPage(packet.i16CurPage, packet.i16MaxPage);
			newGuildWarRewardInfoDlg.SetRewardButtonEnable(packet.bCanGetReward);
			for (int i = 0; i < (int)packet.i16GuildDataCount; i++)
			{
				GUILDWAR_RANK_DATA packet2 = kDeserializePacket.GetPacket<GUILDWAR_RANK_DATA>();
				if (newGuildWarRewardInfoDlg != null)
				{
					newGuildWarRewardInfoDlg.AddGuildData(packet2);
				}
			}
			newGuildWarRewardInfoDlg.RefreshRankInfo();
		}

		public static void GS_GUILDWAR_REWARD_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_GUILDWAR_REWARD_ACK packet = kDeserializePacket.GetPacket<GS_GUILDWAR_REWARD_ACK>();
			if (packet.i32Result == 0)
			{
				NrTSingleton<GuildWarManager>.Instance.SetCanGetGuildWarReward(false);
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64CurMoney;
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("804"),
					"count",
					ANNUALIZED.Convert(packet.i32RewardGold)
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("798"),
					"itemname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.kRewarditem.m_nItemUnique),
					"count",
					packet.kRewarditem.m_nItemNum
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				NewGuildWarRewardInfoDlg newGuildWarRewardInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GUILDWAR_REWARDINFO_DLG) as NewGuildWarRewardInfoDlg;
				if (newGuildWarRewardInfoDlg != null)
				{
					newGuildWarRewardInfoDlg.SetRewardButtonEnable(false);
				}
			}
			else
			{
				int i32Result = packet.i32Result;
				switch (i32Result)
				{
				case 9561:
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("805"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					break;
				case 9562:
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("214"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					break;
				case 9563:
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("848"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					if (packet.CanGetRewardTime > 0L)
					{
						string empty2 = string.Empty;
						DateTime dueDate = PublicMethod.GetDueDate(packet.CanGetRewardTime);
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("847"),
							"month",
							dueDate.Month,
							"day",
							dueDate.Day,
							"hour",
							dueDate.Hour,
							"min",
							dueDate.Minute
						});
						Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					}
					break;
				default:
					if (i32Result != -30)
					{
						if (i32Result == -2 || i32Result == 21)
						{
							Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("199"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
							break;
						}
						if (i32Result != 31)
						{
							if (i32Result != 9500)
							{
								Main_UI_SystemMessage.ADDMessage(string.Format("GUILDWAR_REWARD Fail: {0}", packet.i32Result), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
								break;
							}
							Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("846"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
							break;
						}
					}
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("633"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					break;
				}
			}
		}

		public static void GS_GUILDWAR_GUILDINFO_INIT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_GUILDWAR_GUILDINFO_INIT_ACK packet = kDeserializePacket.GetPacket<GS_GUILDWAR_GUILDINFO_INIT_ACK>();
			TsLog.LogOnlyEditor("GS_GUILDWAR_GUILDINFO_INIT_ACK " + packet.i64GuildID);
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo == null)
			{
				return;
			}
			SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
			NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
			if (readySolList == null)
			{
				return;
			}
			foreach (NkSoldierInfo current in readySolList.GetList().Values)
			{
				charPersonInfo.ChangeSolPosType(current.GetSolID(), 0, 0, 0, 0);
				if (solMilitaryGroupDlg != null)
				{
					solMilitaryGroupDlg.SetSoldierUpdate(current);
				}
			}
			if (solMilitaryGroupDlg != null)
			{
				solMilitaryGroupDlg.CheckSkillUpSolNum();
			}
		}

		public static void GS_INDUN_OPEN_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_INDUN_OPEN_ACK packet = kDeserializePacket.GetPacket<GS_INDUN_OPEN_ACK>();
			NrTSingleton<NkIndunManager>.Instance.SetIndunInfo(packet.nIndunIDX, packet.nIndunUnique);
		}

		public static void GS_INDUN_TIME_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_INDUN_TIME_NFY packet = kDeserializePacket.GetPacket<GS_INDUN_TIME_NFY>();
			NrTSingleton<NkIndunManager>.Instance.SetIndunTime(packet.m_fTime);
			Debug.Log("Remain Indun time : " + packet.m_fTime);
		}

		public static void GS_INDUN_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_INDUN_INFO_ACK packet = kDeserializePacket.GetPacket<GS_INDUN_INFO_ACK>();
			if (packet.m_nRoomState <= 4)
			{
				NrTSingleton<NkIndunManager>.Instance.SetIndunInfo(packet.m_nIndunIDX, packet.m_nIndunUnique);
				NrTSingleton<NkIndunManager>.Instance.SetIndunInfoDlg(packet.m_fTime, packet.m_nUserNum);
			}
			else if (packet.m_nRoomState == 5)
			{
				NrTSingleton<NkIndunManager>.Instance.Clear();
			}
		}

		public static void GS_INDUN_RESULT_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_INDUN_RESULT_NFY packet = kDeserializePacket.GetPacket<GS_INDUN_RESULT_NFY>();
			NrTSingleton<NkIndunManager>.Instance.SetResult(packet.nIndunUnique, packet.bWin, (eINDUN_CLOSE_REASON)packet.nReason, packet.RewardGold);
		}

		public static void GS_INDUN_EXCEPT_AREA_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_INDUN_EXCEPT_AREA_NFY packet = kDeserializePacket.GetPacket<GS_INDUN_EXCEPT_AREA_NFY>();
			if (packet.m_nCount <= 0)
			{
				return;
			}
			GS_INDUN_EXCEPT_AREA_INFO[] array = new GS_INDUN_EXCEPT_AREA_INFO[packet.m_nCount];
			sbyte b = 0;
			while ((int)b < packet.m_nCount)
			{
				array[(int)b] = kDeserializePacket.GetPacket<GS_INDUN_EXCEPT_AREA_INFO>();
				if (array[(int)b].m_bMode)
				{
					NrTSingleton<NrNpcPosManager>.Instance.AddIndunExceptArea(array[(int)b]);
				}
				else
				{
					NrTSingleton<NrNpcPosManager>.Instance.DelIndunExceptArea(array[(int)b]);
				}
				b += 1;
			}
		}

		public static void GS_INDUN_CHAR_ATB_TOTAL(NkDeserializePacket kDeserializePacket)
		{
			GS_INDUN_CHAR_ATB_TOTAL packet = kDeserializePacket.GetPacket<GS_INDUN_CHAR_ATB_TOTAL>();
			if (packet.m_nCount <= 0)
			{
				return;
			}
			GS_INDUN_CHAR_ATB_INFO_NFY[] array = new GS_INDUN_CHAR_ATB_INFO_NFY[packet.m_nCount];
			for (int i = 0; i < packet.m_nCount; i++)
			{
				array[i] = kDeserializePacket.GetPacket<GS_INDUN_CHAR_ATB_INFO_NFY>();
				NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(array[i].m_nCharUnique);
				if (charByCharUnique != null)
				{
					charByCharUnique.SetIndunCharATB(array[i].m_nATB);
				}
				NrTSingleton<NkCharManager>.Instance.SetIndunATBReservation(array[i].m_nCharUnique, array[i].m_nATB);
			}
		}

		public static void GS_INDUN_CHAR_ATB_INFO_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_INDUN_CHAR_ATB_INFO_NFY packet = kDeserializePacket.GetPacket<GS_INDUN_CHAR_ATB_INFO_NFY>();
			NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(packet.m_nCharUnique);
			if (charByCharUnique != null)
			{
				charByCharUnique.SetIndunCharATB(packet.m_nATB);
			}
			NrTSingleton<NkCharManager>.Instance.SetIndunATBReservation(packet.m_nCharUnique, packet.m_nATB);
		}

		public static void GS_INDUN_EVENT_TRIGGER_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_INDUN_EVENT_TRIGGER_NFY packet = kDeserializePacket.GetPacket<GS_INDUN_EVENT_TRIGGER_NFY>();
			INDUNTRIGGERTYPE i32Type = (INDUNTRIGGERTYPE)packet.i32Type;
			if (i32Type != INDUNTRIGGERTYPE.INDUNTRIGGERTYPE_TRIGGER)
			{
				if (i32Type == INDUNTRIGGERTYPE.INDUNTRIGGERTYPE_ACTION)
				{
					NrReceiveGame.ProcessAction((INDUN_ACTIONKIND)packet.i32Code, packet.i32Params);
				}
			}
			else
			{
				NrReceiveGame.ProcessTrigger((INDUN_TRIGGERKIND)packet.i32Code, packet.i32Params);
			}
		}

		public static void GS_INDUN_EVENT_ACTION_EFFECT_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_INDUN_EVENT_ACTION_EFFECT_NFY packet = kDeserializePacket.GetPacket<GS_INDUN_EVENT_ACTION_EFFECT_NFY>();
			if (packet.nType == 0)
			{
				NrTSingleton<NkEffectManager>.Instance.DeleteEffectFromName(TKString.NEWString(packet.szEffectName));
				return;
			}
			if (packet.nCharKind != 0)
			{
				ArrayList arrayList = new ArrayList();
				arrayList.Clear();
				NrCharBase[] @char = NrTSingleton<NkCharManager>.Instance.Get_Char();
				if (@char == null)
				{
					return;
				}
				NrCharBase[] array = @char;
				for (int i = 0; i < array.Length; i++)
				{
					NrCharBase nrCharBase = array[i];
					if (nrCharBase != null)
					{
						if (packet.nCharKind == 9999)
						{
							if (!NrTSingleton<NrCharKindInfoManager>.Instance.IsUserCharKind(nrCharBase.GetPersonInfo().GetKind(0)))
							{
								goto IL_EC;
							}
						}
						else if (nrCharBase.GetPersonInfo().GetKind(0) != packet.nCharKind)
						{
							goto IL_EC;
						}
						if (!arrayList.Contains(nrCharBase.GetID()))
						{
							arrayList.Add(nrCharBase.GetCharUnique());
						}
					}
					IL_EC:;
				}
				if (arrayList.Count > 0)
				{
					for (int j = 0; j < arrayList.Count; j++)
					{
						NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique((short)arrayList[j]);
						if (charByCharUnique != null)
						{
							if (charByCharUnique.IsReadyCharAction())
							{
								Vector3 centerPosition = charByCharUnique.GetCenterPosition();
								uint num = NrTSingleton<NkEffectManager>.Instance.AddEffect(TKString.NEWString(packet.szEffectCode), centerPosition);
								if (num != 0u)
								{
									NkEffectUnit effectUnit = NrTSingleton<NkEffectManager>.Instance.GetEffectUnit(num);
									float num2 = 0f;
									if (packet.nType == 1)
									{
										num2 = float.PositiveInfinity;
									}
									if (effectUnit == null)
									{
										NrTSingleton<NkEffectManager>.Instance.SetReservationEffectData(num, TKString.NEWString(packet.szEffectName), num2, (float)packet.nHorizon);
									}
									else
									{
										effectUnit.EffectName = TKString.NEWString(packet.szEffectName);
										if (packet.nType == 1)
										{
											effectUnit.LifeTime = num2;
										}
										effectUnit.SetRotate((float)packet.nHorizon);
									}
								}
							}
						}
					}
				}
			}
			else
			{
				Vector3 worldHeight = new Vector3(packet.fPosX, 0f, packet.fPosZ);
				worldHeight = NrTSingleton<NrTerrain>.Instance.GetWorldHeight(new Vector3(worldHeight.x, 0f, worldHeight.z));
				uint num3 = NrTSingleton<NkEffectManager>.Instance.AddEffect(TKString.NEWString(packet.szEffectCode), worldHeight);
				if (num3 != 0u)
				{
					NkEffectUnit effectUnit2 = NrTSingleton<NkEffectManager>.Instance.GetEffectUnit(num3);
					float num4 = 0f;
					if (packet.nType == 1)
					{
						num4 = float.PositiveInfinity;
					}
					if (effectUnit2 == null)
					{
						NrTSingleton<NkEffectManager>.Instance.SetReservationEffectData(num3, TKString.NEWString(packet.szEffectName), num4, (float)packet.nHorizon);
						return;
					}
					effectUnit2.EffectName = TKString.NEWString(packet.szEffectName);
					if (packet.nType == 1)
					{
						effectUnit2.LifeTime = num4;
					}
					effectUnit2.SetRotate((float)packet.nHorizon);
				}
			}
		}

		public static void GS_INDUN_EVENT_DRAMA_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_INDUN_EVENT_DRAMA_NFY packet = kDeserializePacket.GetPacket<GS_INDUN_EVENT_DRAMA_NFY>();
			string str = TKString.NEWString(packet.szDramaCode);
			NrTSingleton<NkCharManager>.Instance.InitChar3DModelAll();
			if (NrTSingleton<NrGlobalReference>.Instance.localWWW)
			{
				if (NrTSingleton<NrGlobalReference>.Instance.useCache)
				{
					string str2 = string.Format("{0}GameDrama/", Option.GetProtocolRootPath(Protocol.HTTP));
					NmMainFrameWork.PlayMovieURL(str2 + str + ".mp4", true, false, true);
				}
				else
				{
					NmMainFrameWork.PlayMovieURL("http://klohw.ndoors.com/at2mobile_android/GameDrama/" + str + ".mp4", true, false, true);
				}
			}
			else
			{
				string str3 = string.Format("{0}GameDrama/", NrTSingleton<NrGlobalReference>.Instance.basePath);
				NmMainFrameWork.PlayMovieURL(str3 + str + ".mp4", true, false, true);
			}
		}

		private static void ProcessTrigger(INDUN_TRIGGERKIND eKind, int[] i32Params)
		{
		}

		private static void ProcessAction(INDUN_ACTIONKIND eKind, int[] i32Params)
		{
			switch (eKind)
			{
			case INDUN_ACTIONKIND.INDUN_ACTIONKIND_SHOWTEXT:
			{
				int nTextIndex = i32Params[0];
				float fTime = (float)i32Params[1] / 100f;
				int num = i32Params[2];
				Indun_HeadUpTalk indun_HeadUpTalk = NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.INDUN_HEADUP_TALK) as Indun_HeadUpTalk;
				string strName = string.Empty;
				NrCharBase nrCharBase;
				if (num == 9999)
				{
					nrCharBase = NrTSingleton<NkCharManager>.Instance.GetChar(1);
					if (nrCharBase != null)
					{
						NrPersonInfoUser nrPersonInfoUser = nrCharBase.GetPersonInfo() as NrPersonInfoUser;
						if (nrPersonInfoUser != null)
						{
							strName = nrPersonInfoUser.GetCharName();
						}
					}
				}
				else
				{
					nrCharBase = NrTSingleton<NkCharManager>.Instance.GetCharByCharKind(num);
					if (nrCharBase != null)
					{
						strName = nrCharBase.GetCharName();
					}
				}
				if (indun_HeadUpTalk != null && nrCharBase != null)
				{
					indun_HeadUpTalk.Set(nrCharBase, strName, fTime, nTextIndex);
				}
				return;
			}
			case INDUN_ACTIONKIND.INDUN_ACTIONKIND_VICTORYWAR:
			case INDUN_ACTIONKIND.INDUN_ACTIONKIND_FAILWAR:
			{
				IL_21:
				if (eKind != INDUN_ACTIONKIND.INDUN_ACTIONKIND_CAMERA)
				{
					return;
				}
				int num2 = i32Params[0];
				int num3 = i32Params[1];
				float x = (float)i32Params[2] / 100f;
				float y = (float)i32Params[3] / 100f;
				float z = (float)i32Params[4] / 100f;
				float fAngle = (float)i32Params[5];
				Vector3 cameraPosition = new Vector3(x, y, z);
				if (num2 == 1)
				{
					maxCamera component = Camera.main.GetComponent<maxCamera>();
					if (component == null)
					{
						return;
					}
					component.SetCameraMode(1, cameraPosition, fAngle);
				}
				else if (num2 == 2)
				{
					maxCamera component2 = Camera.main.GetComponent<maxCamera>();
					if (component2 == null)
					{
						return;
					}
					component2.SetCameraMode(2, cameraPosition, fAngle);
				}
				else if (num2 == 3)
				{
					NrCharBase nrCharBase2;
					if (num3 == 9999)
					{
						nrCharBase2 = NrTSingleton<NkCharManager>.Instance.GetChar(1);
						if (nrCharBase2 != null)
						{
							cameraPosition = nrCharBase2.GetCameraPosition();
						}
					}
					else
					{
						nrCharBase2 = NrTSingleton<NkCharManager>.Instance.GetCharByCharKind(num3);
						if (nrCharBase2 != null)
						{
							cameraPosition = nrCharBase2.GetCameraPosition();
						}
					}
					if (nrCharBase2 != null)
					{
						maxCamera component3 = Camera.main.GetComponent<maxCamera>();
						if (component3 == null)
						{
							return;
						}
						component3.SetCameraMode(3, cameraPosition, fAngle);
					}
				}
				else if (num2 == 4)
				{
					maxCamera component4 = Camera.main.GetComponent<maxCamera>();
					if (component4 == null)
					{
						return;
					}
					component4.SetCameraMode(4, cameraPosition, fAngle);
				}
				return;
			}
			case INDUN_ACTIONKIND.INDUN_ACTIONKIND_ANICONTROL:
			{
				int num4 = i32Params[0];
				eCharAnimationType aniType = (eCharAnimationType)i32Params[1];
				int num5 = i32Params[2];
				eCharAnimationType nextAniType = (eCharAnimationType)i32Params[3];
				NrCharBase nrCharBase3;
				if (num4 == 9999)
				{
					nrCharBase3 = NrTSingleton<NkCharManager>.Instance.GetChar(1);
				}
				else
				{
					nrCharBase3 = NrTSingleton<NkCharManager>.Instance.GetCharByCharKind(num4);
				}
				if (nrCharBase3 == null)
				{
					return;
				}
				if (num5 > 0)
				{
					nrCharBase3.SetIndunTriggerAnimation(true, aniType, num5, nextAniType);
				}
				else if (num5 == 0)
				{
					nrCharBase3.SetIndunTriggerAnimation(true, aniType, 2147483647, nextAniType);
				}
				else
				{
					nrCharBase3.SetIndunTriggerAnimation(true, aniType, 1, nextAniType);
				}
				return;
			}
			case INDUN_ACTIONKIND.INDUN_ACTIONKIND_INPUTCONTROL:
				if (i32Params[0] == 0)
				{
					NrTSingleton<NkCharManager>.Instance.InputControl = false;
					NrCharBase[] @char = NrTSingleton<NkCharManager>.Instance.Get_Char();
					for (int i = 0; i < @char.Length; i++)
					{
						NrCharBase nrCharBase4 = @char[i];
						if (nrCharBase4 != null && nrCharBase4.m_e3DCharStep == NrCharBase.e3DCharStep.CHARACTION && nrCharBase4.m_kCharMove.IsMoving())
						{
							nrCharBase4.m_kCharMove.MoveStop(false, false);
						}
					}
				}
				else
				{
					NrTSingleton<NkCharManager>.Instance.InputControl = true;
				}
				return;
			}
			goto IL_21;
		}

		public static void GS_INFIBATTLE_MATCH_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_INFIBATTLE_MATCH_ACK packet = kDeserializePacket.GetPacket<GS_INFIBATTLE_MATCH_ACK>();
			TsLog.LogWarning(string.Concat(new object[]
			{
				"GS_INFIBATTLE_MATCH_ACK : ",
				packet.i32Result,
				" Rank : ",
				packet.i32Rank
			}), new object[0]);
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			SoldierBatch.SOLDIERBATCH.RemoveLoadingEffect = true;
			if (packet.i32Result == 0)
			{
				SoldierBatch.SOLDIERBATCH.m_cTargetInfo.SetTargetCharInfo(TKString.NEWString(packet.strDefenseName), (int)packet.i16CharLevel, 0L);
				PlunderTargetInfoDlg plunderTargetInfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDERTARGETINFO_DLG) as PlunderTargetInfoDlg;
				if (plunderTargetInfoDlg != null)
				{
					plunderTargetInfoDlg.SetTargetInfoInfiBattle(packet.bTargetShow, TKString.NEWString(packet.strDefenseName), packet.i16CharLevel, packet.i32Rank, packet.i32StraightWin);
				}
				if (!packet.bTargetShow)
				{
					for (int i = 0; i < 15; i++)
					{
						TsLog.LogWarning(string.Concat(new object[]
						{
							"!!!!",
							packet.DefenseSolInfo[i].nCharKind,
							" : ",
							packet.DefenseSolInfo[i].nStartPos,
							" : ",
							packet.DefenseSolInfo[i].nBattlePos
						}), new object[0]);
						if (!SoldierBatch.SOLDIERBATCH.m_bMakeEnemyChar)
						{
							SoldierBatch.SOLDIERBATCH.AddEnemyCharInfo(packet.DefenseSolInfo[i]);
						}
						else
						{
							SoldierBatch.SOLDIERBATCH.MakePlunderCharEnemy(packet.DefenseSolInfo[i], i);
						}
					}
				}
				PlunderSolNumDlg plunderSolNumDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLNUM_DLG) as PlunderSolNumDlg;
				if (plunderSolNumDlg != null && (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE))
				{
					plunderSolNumDlg.SetSumFightPowerAlly0(SoldierBatch.SOLDIERBATCH.SumFightPower, true);
				}
				return;
			}
			if (packet.i32Result == -10)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("120"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			else if (packet.i32Result == -20)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("684"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			else if (packet.i32Result == -30)
			{
				long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_INFIBATTLE_COOLTIME);
				long curTime = PublicMethod.GetCurTime();
				if (curTime < charSubData)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("862"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
			else if (packet.i32Result == -40)
			{
				int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_INFIBATTLE_LEVEL);
				if (kMyCharInfo.GetLevel() < value)
				{
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("129"),
						"level",
						value
					});
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("120"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfiBattleCharLevel = 0;
		}

		private static void InFiBattle_Start_Error()
		{
			PlunderSolNumDlg plunderSolNumDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLNUM_DLG) as PlunderSolNumDlg;
			if (plunderSolNumDlg != null)
			{
				plunderSolNumDlg.CloseForm(null);
			}
			InfiCombinationDlg infiCombinationDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.INFICOMBINATION_DLG) as InfiCombinationDlg;
			if (infiCombinationDlg != null)
			{
				infiCombinationDlg.CloseForm(null);
			}
		}

		public static void GS_INFIBATTLE_START_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_INFIBATTLE_START_ACK packet = kDeserializePacket.GetPacket<GS_INFIBATTLE_START_ACK>();
			TsLog.LogWarning("GS_INFIBATTLE_START_ACK Result " + packet.i32Result.ToString(), new object[0]);
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			SoldierBatch.SOLDIERBATCH.RemoveLoadingEffect = true;
			string empty = string.Empty;
			InfiBattleDefine.eINFIBATTLE_STARTRESULT i32Result = (InfiBattleDefine.eINFIBATTLE_STARTRESULT)packet.i32Result;
			switch (i32Result + 5)
			{
			case (InfiBattleDefine.eINFIBATTLE_STARTRESULT)0:
			{
				long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_INFIBATTLE_COOLTIME);
				long curTime = PublicMethod.GetCurTime();
				long num = charSubData - curTime;
				if (num > 0L)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("862"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
				{
					PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
					if (plunderSolListDlg != null)
					{
						int solBatchNum = SoldierBatch.SOLDIERBATCH.GetSolBatchNum();
						plunderSolListDlg.SetSolNum(solBatchNum, false);
						plunderSolListDlg.Show();
					}
				}
				return;
			}
			case InfiBattleDefine.eINFIBATTLE_STARTRESULT.eINFIBATTLE_STARTRESULT_WAITTIME:
			case InfiBattleDefine.eINFIBATTLE_STARTRESULT.eINFIBATTLE_STARTRESULT_NOTPLAYERINFO:
			case InfiBattleDefine.eINFIBATTLE_STARTRESULT.eINFIBATTLE_STARTRESULT_NOTLEADER:
			case InfiBattleDefine.eINFIBATTLE_STARTRESULT.eINFIBATTLE_STARTRESULT_DISCONNECT_CS:
			case InfiBattleDefine.eINFIBATTLE_STARTRESULT.eINFIBATTLE_STARTRESULT_BATTLEMAKE_FAIL:
			case (InfiBattleDefine.eINFIBATTLE_STARTRESULT)9:
				IL_94:
				if (i32Result != (InfiBattleDefine.eINFIBATTLE_STARTRESULT)(-10))
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("120"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					return;
				}
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("853"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
				{
					PlunderSolListDlg plunderSolListDlg2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
					if (plunderSolListDlg2 != null)
					{
						int solBatchNum2 = SoldierBatch.SOLDIERBATCH.GetSolBatchNum();
						plunderSolListDlg2.SetSolNum(solBatchNum2, false);
						plunderSolListDlg2.Show();
					}
				}
				return;
			case InfiBattleDefine.eINFIBATTLE_STARTRESULT.eINFIBATTLE_STARTRESULT_RANKSET:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("120"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			case InfiBattleDefine.eINFIBATTLE_STARTRESULT.eINFIBATTLE_STARTRESULT_BATTLE_START:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("120"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			case (InfiBattleDefine.eINFIBATTLE_STARTRESULT)8:
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("124"),
					"charname",
					@char.GetCharName()
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
				{
					PlunderSolListDlg plunderSolListDlg3 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
					if (plunderSolListDlg3 != null)
					{
						int solBatchNum3 = SoldierBatch.SOLDIERBATCH.GetSolBatchNum();
						plunderSolListDlg3.SetSolNum(solBatchNum3, false);
						plunderSolListDlg3.Show();
					}
				}
				return;
			case (InfiBattleDefine.eINFIBATTLE_STARTRESULT)10:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("120"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			case (InfiBattleDefine.eINFIBATTLE_STARTRESULT)11:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("692"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			case (InfiBattleDefine.eINFIBATTLE_STARTRESULT)12:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("694"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			goto IL_94;
		}

		public static void GS_INFIBATTLE_PRACTICE_START_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_INFIBATTLE_PRACTICE_START_ACK packet = kDeserializePacket.GetPacket<GS_INFIBATTLE_PRACTICE_START_ACK>();
			TsLog.LogWarning("GS_INFIBATTLE_PRACTICE_START_ACK Result " + packet.i32Result.ToString(), new object[0]);
			if (packet.i32Result != 0)
			{
				NrReceiveGame.InFiBattle_Start_Error();
			}
		}

		public static void GS_INFIBATTLE_RANK_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_INFIBATTLE_RANK_ACK packet = kDeserializePacket.GetPacket<GS_INFIBATTLE_RANK_ACK>();
			InfiBattleRankDlg infiBattleRankDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INFIBATTLE_RANK_DLG) as InfiBattleRankDlg;
			infiBattleRankDlg.InitRankInfo();
			for (int i = 0; i < packet.i32SubDataCount; i++)
			{
				INFIBATTLE_RANK_INFO packet2 = kDeserializePacket.GetPacket<INFIBATTLE_RANK_INFO>();
				infiBattleRankDlg.AddRankInfo(packet2);
			}
			infiBattleRankDlg.ShowRank(packet);
		}

		public static void GS_INFIBATTLE_RECORD_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_INFIBATTLE_RECORD_ACK packet = kDeserializePacket.GetPacket<GS_INFIBATTLE_RECORD_ACK>();
			PlunderRecordDlg plunderRecordDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDERRECORD_DLG) as PlunderRecordDlg;
			if (plunderRecordDlg != null)
			{
				for (int i = 0; i < packet.i32RecordCount; i++)
				{
					INFIBATTLE_RECORDINFO packet2 = kDeserializePacket.GetPacket<INFIBATTLE_RECORDINFO>();
					plunderRecordDlg.AddInfiBattleRecordInfo(packet2);
				}
				plunderRecordDlg.SetInfiBattleInfo();
			}
		}

		public static void GS_INFIBATTLE_RESULT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_INFIBATTLE_RESULT_ACK packet = kDeserializePacket.GetPacket<GS_INFIBATTLE_RESULT_ACK>();
			if (Battle.BATTLE == null)
			{
				return;
			}
			TsLog.LogWarning("GS_INFIBATTLE_RESULT_ACK WinAlly : " + packet.i8WinAlly, new object[0]);
			Battle_ResultPlunderDlg battle_ResultPlunderDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_RESULT_PLUNDER_DLG) as Battle_ResultPlunderDlg;
			if (battle_ResultPlunderDlg == null)
			{
				return;
			}
			battle_ResultPlunderDlg.ShowMode();
			battle_ResultPlunderDlg.ClearSolData();
			for (int i = 0; i < (int)packet.i8NumSoldiers; i++)
			{
				GS_BATTLE_RESULT_SOLDIER packet2 = kDeserializePacket.GetPacket<GS_BATTLE_RESULT_SOLDIER>();
				battle_ResultPlunderDlg.AddSolData(packet2);
			}
			battle_ResultPlunderDlg.SetInfiBattleInfo(packet);
			battle_ResultPlunderDlg.LinkData();
		}

		public static void GS_INFIBATTLE_END_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_INFIBATTLE_END_ACK packet = kDeserializePacket.GetPacket<GS_INFIBATTLE_END_ACK>();
			if (packet.i32Rank > 0)
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfinityBattle_Rank = packet.i32Rank;
			}
			if (packet.i32OldRank > 0)
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfinityBattle_OldRank = packet.i32OldRank;
			}
			if (packet.i32StraightWin > 0)
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfiBattleStraightWin = packet.i32StraightWin;
			}
			if (packet.i32BattleWin > 0)
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InifBattle_WinCount = packet.i32BattleWin;
			}
			if (packet.i32BattleTotal > 0)
			{
				if (packet.i32BattleTotal < packet.i32BattleWin)
				{
					NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InifBattle_TotalCount = packet.i32BattleWin;
				}
				else
				{
					NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InifBattle_TotalCount = packet.i32BattleTotal;
				}
			}
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_RESULT_PLUNDER_DLG))
			{
				Battle_ResultPlunderDlg battle_ResultPlunderDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_RESULT_PLUNDER_DLG) as Battle_ResultPlunderDlg;
				if (battle_ResultPlunderDlg != null)
				{
					battle_ResultPlunderDlg.LinkData();
				}
			}
		}

		public static void GS_INFIBATTLE_REWARDINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_INFIBATTLE_REWARDINFO_ACK packet = kDeserializePacket.GetPacket<GS_INFIBATTLE_REWARDINFO_ACK>();
			InfiBattleRankDlg infiBattleRankDlg;
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.INFIBATTLE_RANK_DLG))
			{
				infiBattleRankDlg = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INFIBATTLE_RANK_DLG) as InfiBattleRankDlg);
			}
			else
			{
				infiBattleRankDlg = (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.INFIBATTLE_RANK_DLG) as InfiBattleRankDlg);
			}
			if (infiBattleRankDlg != null)
			{
				infiBattleRankDlg.SetMyOldRankInfo(packet.MyRankData.i32TopRank, TKString.NEWString(packet.MyRankData.szCharName), packet.MyRankData.i32CharLevel, packet.MyRankData.i32BattleCnt, packet.MyRankData.i32WinCnt);
				infiBattleRankDlg.SetTopRankStart();
				for (int i = 0; i < (int)packet.i8SubDataCount; i++)
				{
					INFIBATTLE_TOPRANK packet2 = kDeserializePacket.GetPacket<INFIBATTLE_TOPRANK>();
					infiBattleRankDlg.SetTopRank(i, packet2.i32TopRank, TKString.NEWString(packet2.szCharName), packet2.i32CharLevel, packet2.i32BattleCnt, packet2.i32WinCnt);
				}
				infiBattleRankDlg.SetTopRankEnd();
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.INFIBATTLE_REWARD_DLG);
			}
		}

		public static void GS_INFIBATTLE_GET_REWARDINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_INFIBATTLE_GET_REWARDINFO_ACK packet = kDeserializePacket.GetPacket<GS_INFIBATTLE_GET_REWARDINFO_ACK>();
			InfiBattleReward infiBattleReward;
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.INFIBATTLE_REWARD_DLG))
			{
				infiBattleReward = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INFIBATTLE_REWARD_DLG) as InfiBattleReward);
			}
			else
			{
				infiBattleReward = (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.INFIBATTLE_REWARD_DLG) as InfiBattleReward);
			}
			if (infiBattleReward != null)
			{
				infiBattleReward.SetRewardInfo(packet);
			}
		}

		public static void GS_INFIBATTLE_GETREWARD_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_INFIBATTLE_GETREWARD_ACK packet = kDeserializePacket.GetPacket<GS_INFIBATTLE_GETREWARD_ACK>();
			TsLog.LogWarning("!!!! GS_INFIBATTLE_GETREWARD_ACK {0} : {1} , {2} , {3}", new object[]
			{
				packet.i32Result,
				packet.i32OldRank,
				packet.i32RewardUnique,
				packet.i32RewardNum
			});
			if (packet.i32Result == 0)
			{
				if (packet.i32itempos < 0)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("705"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				else
				{
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("400"),
						"itemname",
						NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.i32RewardUnique),
						"count",
						packet.i32RewardNum
					});
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfiBattleReward = 1;
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERMAIN_DLG))
				{
					PlunderMainDlg plunderMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDERMAIN_DLG) as PlunderMainDlg;
					if (plunderMainDlg != null)
					{
						plunderMainDlg.SetRewardMark();
					}
				}
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.INFIBATTLE_RANK_DLG))
				{
					InfiBattleRankDlg infiBattleRankDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INFIBATTLE_RANK_DLG) as InfiBattleRankDlg;
					if (infiBattleRankDlg != null)
					{
						infiBattleRankDlg.SetRewardTexutre();
					}
				}
				BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
				if (bookmarkDlg != null)
				{
					bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.BATTLE);
				}
				BattleCollect_DLG battleCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLECOLLECT_DLG) as BattleCollect_DLG;
				if (battleCollect_DLG != null)
				{
					battleCollect_DLG.Update_Notice();
				}
			}
			else if (packet.i32Result == -10)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("199"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			else if (packet.i32Result == -40)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("214"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("178"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}

		public static void GS_INFIBATTLE_RANK_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_INFIBATTLE_RANK_GET_ACK packet = kDeserializePacket.GetPacket<GS_INFIBATTLE_RANK_GET_ACK>();
			TsLog.LogWarning("!!!! GS_INFIBATTLE_RANK_GET_ACK Reward {0} :  {1} , {2} , {3}", new object[]
			{
				packet.i32Result,
				packet.i32InfinityBattle_Rank,
				packet.i32InfinityBattle_OldRank,
				packet.i8GetReward
			});
			if (packet.i32Result == 0)
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfinityBattle_Rank = packet.i32InfinityBattle_Rank;
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfinityBattle_OldRank = packet.i32InfinityBattle_OldRank;
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfiBattleStraightWin = packet.i32InfiBattleStraightWin;
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfiBattleReward = packet.i8GetReward;
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InifBattle_TotalCount = packet.i32InfinityBattle_TotalCount;
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InifBattle_WinCount = packet.i32InfinityBattle_WinCount;
				if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERMAIN_DLG))
				{
					PlunderMainDlg plunderMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDERMAIN_DLG) as PlunderMainDlg;
					if (plunderMainDlg != null)
					{
						plunderMainDlg.Show();
					}
				}
			}
			else if (packet.i32Result == -1)
			{
				int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_INFIBATTLE_LEVEL);
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("129");
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromNotify,
					"level",
					value.ToString()
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}

		public static void GS_LOAD_INVEN_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_LOAD_INVEN_NFY packet = kDeserializePacket.GetPacket<GS_LOAD_INVEN_NFY>();
			NkUserInventory.GetInstance().Clear();
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			NkSoldierInfo nkSoldierInfo = null;
			if (nrCharUser != null)
			{
				nkSoldierInfo = nrCharUser.GetPersonInfo().GetLeaderSoldierInfo();
			}
			if (nkSoldierInfo == null)
			{
				return;
			}
			for (int i = 0; i < (int)packet.m_shCount; i++)
			{
				ITEM packet2 = kDeserializePacket.GetPacket<ITEM>();
				if (packet2.m_nPosType != -1)
				{
					if (packet2.m_nItemPos != -1)
					{
						if (packet2.m_nPosType != 10)
						{
							NkUserInventory.GetInstance().SetInfo(packet2, -1);
						}
						else if (nkSoldierInfo != null)
						{
							nkSoldierInfo.SetItem(packet2);
						}
					}
				}
			}
			nkSoldierInfo.SetReceivedEquipItem(true);
			nkSoldierInfo.UpdateSoldierStatInfo();
			nrCharUser.SetReadyPartInfo();
		}

		public static void GS_ITEM_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEM_SET_ACK packet = kDeserializePacket.GetPacket<GS_ITEM_SET_ACK>();
			if (packet.kItem.m_nPosType != 10)
			{
				ITEM iTEM = NkUserInventory.GetInstance().GetItem(packet.kItem.m_nPosType, packet.kItem.m_nItemPos);
				int nItemUnique;
				if (iTEM != null)
				{
					nItemUnique = iTEM.m_nItemUnique;
				}
				else
				{
					nItemUnique = packet.kItem.m_nItemUnique;
					iTEM = packet.kItem;
				}
				if (nItemUnique == 70000)
				{
					int num;
					if (iTEM != null)
					{
						num = iTEM.m_nItemNum - packet.kItem.m_nItemNum;
					}
					else
					{
						num = packet.kItem.m_nItemNum;
					}
					if (num > 0)
					{
						NrTSingleton<FiveRocksEventManager>.Instance.HeartsInflow(eHEARTS_INFLOW.UNKNOWN_INFLOW, (long)num);
					}
					else
					{
						NrTSingleton<FiveRocksEventManager>.Instance.HeartsConsume(eHEARTS_CONSUME.UNKNOWN__USE, (long)num);
					}
				}
				if (packet.kItem.m_nItemUnique > 0)
				{
					NkUserInventory.GetInstance().SetInfo(packet.kItem, packet.m_SetType);
				}
				else
				{
					NkUserInventory.GetInstance().ItemRemove(packet.kItem.m_nPosType, packet.kItem.m_nItemPos);
				}
				bool flag = false;
				ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(nItemUnique);
				if (itemInfo != null && itemInfo.m_nFunctions == 8)
				{
					flag = true;
				}
				if (flag)
				{
					BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
					if (bookmarkDlg != null)
					{
						bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.HERO);
					}
					HeroCollect_DLG heroCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.HEROCOLLECT_DLG) as HeroCollect_DLG;
					if (heroCollect_DLG != null)
					{
						heroCollect_DLG.Update_Notice();
					}
				}
			}
			CostumeRoom_Dlg costumeRoom_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COSTUMEROOM_DLG) as CostumeRoom_Dlg;
			if (costumeRoom_Dlg != null)
			{
				costumeRoom_Dlg.RefreshMoney();
			}
		}

		public static void GS_SET_ITEM_SLOT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SET_ITEM_SLOT_ACK packet = kDeserializePacket.GetPacket<GS_SET_ITEM_SLOT_ACK>();
			Debug.Log("GS_SET_ITEM_SLOT_ACK = nBuildingLevel : " + packet.nBuildingLevel.ToString());
		}

		public static void GS_ITEM_SOL_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEM_SOL_SET_ACK packet = kDeserializePacket.GetPacket<GS_ITEM_SOL_SET_ACK>();
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser == null)
			{
				return;
			}
			NkSoldierInfo soldierInfoFromSolID = nrCharUser.GetPersonInfo().GetSoldierInfoFromSolID(packet.nSolID);
			if (soldierInfoFromSolID == null)
			{
				return;
			}
			if (packet.kItem == null)
			{
				return;
			}
			soldierInfoFromSolID.RemoveItem(packet.kItem.m_nItemPos);
			if (packet.kItem.m_nItemID != 0L)
			{
				soldierInfoFromSolID.SetItem(packet.kItem);
			}
			NrTSingleton<EventConditionHandler>.Instance.ItemEquip.Value.Set((long)packet.kItem.m_nItemUnique);
			NrTSingleton<EventConditionHandler>.Instance.ItemEquip.OnTrigger();
			soldierInfoFromSolID.UpdateSoldierStatInfo();
			if (soldierInfoFromSolID.IsLeader())
			{
				nrCharUser.ChangeEquipItem();
			}
			SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
			if (solMilitaryGroupDlg != null)
			{
				solMilitaryGroupDlg.RefreshEquipItem(soldierInfoFromSolID);
				solMilitaryGroupDlg.SetSoldierUpdate(soldierInfoFromSolID);
			}
			SolEquipItemSelectDlg solEquipItemSelectDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLEQUIPITEMSELECT_DLG) as SolEquipItemSelectDlg;
			if (solEquipItemSelectDlg != null)
			{
				solEquipItemSelectDlg.Refresh();
			}
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLCOMPOSE_MAIN_DLG))
			{
				if (SolComposeMainDlg.Instance.ContainSubSoldier(packet.nSolID) || SolComposeMainDlg.Instance.CheckBaseSoldier(soldierInfoFromSolID))
				{
					SolComposeMainDlg.Instance.MakeSubSolList();
				}
				if (SolComposeMainDlg.Instance.ContainExtractSoldier(packet.nSolID))
				{
					SolComposeMainDlg.Instance.RefreshSelectExtract();
				}
			}
		}

		public static void GS_ITEM_MOVE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEM_MOVE_ACK packet = kDeserializePacket.GetPacket<GS_ITEM_MOVE_ACK>();
			if (packet.m_nResult == 0)
			{
				NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
				if (nrCharUser == null)
				{
					return;
				}
				if (packet.m_byDestPosType == 10)
				{
					SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
					if (solMilitaryGroupDlg != null)
					{
						NkSoldierInfo soldierInfoFromSolID = nrCharUser.GetPersonInfo().GetSoldierInfoFromSolID(packet.m_nDestSolID);
						if (soldierInfoFromSolID == null)
						{
							return;
						}
						solMilitaryGroupDlg.RefreshEquipItem(soldierInfoFromSolID);
					}
				}
				SolEquipItemSelectDlg solEquipItemSelectDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolEquipItemSelectDlg;
				if (solEquipItemSelectDlg != null)
				{
					solEquipItemSelectDlg.Refresh();
				}
				Myth_Legend_Info_DLG myth_Legend_Info_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTH_LEGEND_INFO_DLG) as Myth_Legend_Info_DLG;
				if (myth_Legend_Info_DLG != null)
				{
					NkSoldierInfo soldierInfoFromSolID2 = nrCharUser.GetPersonInfo().GetSoldierInfoFromSolID(packet.m_nSrcSolID);
					if (soldierInfoFromSolID2 == null)
					{
						return;
					}
					myth_Legend_Info_DLG.InitSetCharKind(soldierInfoFromSolID2.GetCharKind());
				}
				if (NrTSingleton<GameGuideManager>.Instance.ExecuteGuide)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("562"), SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
					GameGuideDlg gameGuideDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GAMEGUIDE_DLG) as GameGuideDlg;
					if (gameGuideDlg != null)
					{
						if (NrTSingleton<GameGuideManager>.Instance.ContinueCheckRecommandEquip())
						{
							gameGuideDlg.SetTalkText();
						}
						else
						{
							gameGuideDlg.ClickClose(null);
						}
					}
				}
				else
				{
					NrTSingleton<GameGuideManager>.Instance.RemoveEquipGuide();
				}
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.CHARCHANGEMAIN_DLG))
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("218"), SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
				}
				SolDetail_Info_Dlg solDetail_Info_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLDETAIL_DLG) as SolDetail_Info_Dlg;
				if (solDetail_Info_Dlg != null)
				{
					solDetail_Info_Dlg.GetSelectToolBarRefresh(packet.m_nSrcSolID);
				}
			}
			else
			{
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.INVENTORY_DLG))
				{
					Inventory_Dlg inventory_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.INVENTORY_DLG) as Inventory_Dlg;
					if (inventory_Dlg != null && inventory_Dlg.c_cCopyItem != null)
					{
						UnityEngine.Object.Destroy(inventory_Dlg.c_cCopyItem.gameObject);
						inventory_Dlg.c_cCopyItem = null;
						inventory_Dlg.c_ivImageView.RepositionItems();
					}
				}
				eRESULT nResult = (eRESULT)packet.m_nResult;
				switch (nResult)
				{
				case eRESULT.R_FAIL_GEN_NOT_WAIT:
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("880"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					return;
				case eRESULT.R_FAIL_GEN_WORKING:
					IL_224:
					if (nResult != eRESULT.R_FAIL_ITEM)
					{
						return;
					}
					goto IL_231;
				case eRESULT.R_FAIL_EQUIPCLASS:
					goto IL_231;
				}
				goto IL_224;
				IL_231:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("844"));
			}
		}

		public static void GS_ITEM_INVEN_SORT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEM_INVEN_SORT_ACK packet = kDeserializePacket.GetPacket<GS_ITEM_INVEN_SORT_ACK>();
			ITEM[] array = new ITEM[(int)packet.m_shCount];
			for (int i = 0; i < (int)packet.m_shCount; i++)
			{
				array[i] = kDeserializePacket.GetPacket<ITEM>();
			}
			if (packet.m_shCount > 0)
			{
				NkUserInventory.GetInstance().Get_Tab_List_Clear(array[0].m_nPosType);
				Protocol_Item.Sort_Item_Clear();
				for (int j = 0; j < array.Length; j++)
				{
					NkUserInventory.GetInstance().SetInfo(array[j], -1);
					Protocol_Item.Set_Sort_Item(array[j]);
				}
				Inventory_Dlg inventory_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INVENTORY_DLG) as Inventory_Dlg;
				inventory_Dlg.Item_Draw(array[0].m_nPosType);
			}
		}

		public static void GS_ITEMS_DELETE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEMS_DELETE_ACK packet = kDeserializePacket.GetPacket<GS_ITEMS_DELETE_ACK>();
			if (packet.m_nResult == 0 && !NkUserInventory.GetInstance().ItemRemove((int)((byte)packet.m_byPosType), packet.m_shPosItem))
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					"--------- NrReceiveGame_Item.cs -- GS_ITEMS_DELETE_ACK -- pkACK.m_byPosType -- ",
					packet.m_byPosType,
					" -- ",
					Environment.TickCount
				}));
				Debug.LogWarning(string.Concat(new object[]
				{
					"--------- NrReceiveGame_Item.cs -- GS_ITEMS_DELETE_ACK -- pkACK.m_shPosItem -- ",
					packet.m_shPosItem,
					" -- ",
					Environment.TickCount
				}));
			}
			NrTSingleton<GameGuideManager>.Instance.RemoveEquipGuide();
		}

		public static void GS_ITEM_MULTI_DELETE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEM_MULTI_DELETE_ACK packet = kDeserializePacket.GetPacket<GS_ITEM_MULTI_DELETE_ACK>();
			if (packet.i32Result == 0)
			{
				Debug.LogError("SUCCESS");
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("838"));
			}
			NrTSingleton<GameGuideManager>.Instance.RemoveEquipGuide();
		}

		public static void GS_ITEMS_REMOVE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEMS_REMOVE_ACK packet = kDeserializePacket.GetPacket<GS_ITEMS_REMOVE_ACK>();
			if (packet.Result == 0)
			{
				int itemunique = 0;
				ITEM item = NkUserInventory.GetInstance().GetItem(packet.PosType, packet.ItemPos);
				if (item != null)
				{
					itemunique = item.m_nItemUnique;
				}
				NkUserInventory.GetInstance().ItemRemove((int)((byte)packet.PosType), packet.ItemPos, packet.i16ItemNum);
				bool flag = false;
				ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(itemunique);
				if (itemInfo != null && itemInfo.m_nFunctions == 8)
				{
					flag = true;
				}
				if (flag)
				{
					BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
					if (bookmarkDlg != null)
					{
						bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.HERO);
					}
					HeroCollect_DLG heroCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.HEROCOLLECT_DLG) as HeroCollect_DLG;
					if (heroCollect_DLG != null)
					{
						heroCollect_DLG.Update_Notice();
					}
				}
			}
		}

		public static void GS_ITEM_UPDATE_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEM_UPDATE_NFY packet = kDeserializePacket.GetPacket<GS_ITEM_UPDATE_NFY>();
			sbyte b = 0;
			while ((int)b < (int)packet.m_nCount)
			{
				ITEM packet2 = kDeserializePacket.GetPacket<ITEM>();
				string empty = string.Empty;
				E_ITEM_CREATE_TYPE e_ITEM_CREATE_TYPE = (E_ITEM_CREATE_TYPE)packet.m_nCreateType;
				switch (e_ITEM_CREATE_TYPE)
				{
				case E_ITEM_CREATE_TYPE.E_ITEM_CREATE_TYPE_NORMAL:
				case E_ITEM_CREATE_TYPE.E_ITEM_CREATE_TYPE_TERRITORY:
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("339"),
						"targetname",
						NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet2),
						"count",
						packet.m_nItemCount.ToString()
					});
					goto IL_5E6;
				case E_ITEM_CREATE_TYPE.E_ITEM_CREATE_TYPE_BATTLE_DROP:
					if ((int)packet.m_ParamVar == 1)
					{
						GetItemDlg getItemDlg = (GetItemDlg)NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.GET_ITEM_DLG);
						if (getItemDlg != null)
						{
							getItemDlg.SetItem(packet2.m_nItemUnique, packet.m_nItemCount, packet2.m_nRank);
							getItemDlg.SetIndex(Battle.BATTLE.ListGetItemDlg.Count);
							Battle.BATTLE.ListGetItemDlg.Add(getItemDlg);
						}
					}
					goto IL_5E6;
				case E_ITEM_CREATE_TYPE.E_ITEM_CREATE_TYPE_QUEST_REWARD:
				case (E_ITEM_CREATE_TYPE)3:
				case E_ITEM_CREATE_TYPE.E_ITEM_CREATE_TYPE_QUEST_ACCEPT:
					IL_43:
					switch (e_ITEM_CREATE_TYPE)
					{
					case E_ITEM_CREATE_TYPE.E_ITEM_CREATE_TYPE_BABEL_EVENT_ITEM:
					case E_ITEM_CREATE_TYPE.E_ITEM_CREATE_TYPE_COLOSSEUM_EVENT_ITEM:
					{
						NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
						myCharInfo.SetCharDetail(packet.m_nCharDetailType, packet.m_nDetailValue);
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("339"),
							"targetname",
							NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet2),
							"count",
							packet.m_nItemCount.ToString()
						});
						Event_Dlg event_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EVENT_MAIN) as Event_Dlg;
						if (event_Dlg != null)
						{
							if (event_Dlg.m_eCurrentTapIndex == eEVENT_TYPE.eBUNNING_CURRENT_EVENT)
							{
								event_Dlg.CurrentEventReq();
							}
							else if (event_Dlg.m_eCurrentTapIndex == eEVENT_TYPE.eBUNNING_Schedule_EVENT)
							{
								event_Dlg.ScheduleEventReq();
							}
						}
						goto IL_5E6;
					}
					case E_ITEM_CREATE_TYPE.E_ITEM_CREATE_TYPE_MINE_TUTORIALITEMGET:
					case E_ITEM_CREATE_TYPE.E_ITEM_CREATE_TYPE_USEITEM_ADDMONEY:
						IL_61:
						switch (e_ITEM_CREATE_TYPE)
						{
						case E_ITEM_CREATE_TYPE.E_ITEM_CREATE_TYPE_CHALLENGE_REWARD:
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
							{
								NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("625"),
								"itemname",
								NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet2),
								"count",
								packet.m_nItemCount.ToString()
							});
							goto IL_5E6;
						case E_ITEM_CREATE_TYPE.E_ITEM_CREATE_TYPE_COMPOSE:
						{
							IL_77:
							if (e_ITEM_CREATE_TYPE == E_ITEM_CREATE_TYPE.E_ITEM_CREATE_TYPE_COLOSSEUM_REWARD)
							{
								NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.ColosseumOldRank = 0;
								NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.COLOSSEUMREWARD_DLG);
								BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
								if (bookmarkDlg != null)
								{
									bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.BATTLE);
								}
								BattleCollect_DLG battleCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLECOLLECT_DLG) as BattleCollect_DLG;
								if (battleCollect_DLG != null)
								{
									battleCollect_DLG.Update_Notice();
								}
								if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.COLOSSEUMMAIN_DLG))
								{
									ColosseumDlg colosseumDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUMMAIN_DLG) as ColosseumDlg;
									if (colosseumDlg != null)
									{
										colosseumDlg.SetShowLayer(1, false);
									}
								}
								if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.COLOSSEUMRANKINFO_DLG))
								{
									ColosseumRankInfoDlg colosseumRankInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUMRANKINFO_DLG) as ColosseumRankInfoDlg;
									if (colosseumRankInfoDlg != null)
									{
										colosseumRankInfoDlg.SetShowLayer(3, false);
									}
								}
								NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
								{
									NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("339"),
									"targetname",
									NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet2),
									"count",
									packet.m_nItemCount.ToString()
								});
								goto IL_5E6;
							}
							if (e_ITEM_CREATE_TYPE != E_ITEM_CREATE_TYPE.E_ITEM_CREATE_TYPE_VOUCHER_FREE_GIVE_ITME)
							{
								goto IL_5E6;
							}
							ITEM_VOUCHER_DATA itemVoucherDataFromItemID = NrTSingleton<ItemMallItemManager>.Instance.GetItemVoucherDataFromItemID(packet.i64ItemMallID);
							if (itemVoucherDataFromItemID != null)
							{
								string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("817");
								NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromNotify, new object[]
								{
									textFromNotify,
									"targetname",
									NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(itemVoucherDataFromItemID.i32GiveItemUnique),
									"count",
									itemVoucherDataFromItemID.i32GiveItemNum
								});
								Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
								NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.SetVoucherRefreshTime((eVOUCHER_TYPE)packet.ui8VoucherType, packet.i64ItemMallID, packet.i64VoucherRefreshTime);
								ItemMallDlg itemMallDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMMALL_DLG) as ItemMallDlg;
								if (itemMallDlg != null)
								{
									itemMallDlg.SetShowData();
								}
							}
							goto IL_5E6;
						}
						case E_ITEM_CREATE_TYPE.E_ITEM_CREATE_TYPE_DAILYEVENT:
						{
							NrMyCharInfo myCharInfo2 = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
							if (myCharInfo2 == null)
							{
								return;
							}
							NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
							if (@char != null)
							{
								@char.m_kCharMove.MoveStop(true, false);
							}
							long charSubData = myCharInfo2.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_ATTENDANCE_SEQUENCE);
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
							{
								NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("405"),
								"day",
								charSubData
							});
							myCharInfo2.SetCharDetail(packet.m_nCharDetailType, packet.m_nDetailValue);
							if (packet.m_nCharDetailType == 23)
							{
								MyCharInfoDlg myCharInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
								if (myCharInfoDlg != null)
								{
									myCharInfoDlg.Attend_Notice_Show();
								}
							}
							long charSubData2 = myCharInfo2.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_ATTENDANCE_TYPE);
							if (charSubData2 == 2L)
							{
								New_Attend_Dlg new_Attend_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EVENT_NEW_ATTEND) as New_Attend_Dlg;
								if (new_Attend_Dlg != null)
								{
									new_Attend_Dlg.InitSet();
									new_Attend_Dlg.CheckDailyEventDay((int)charSubData);
									new_Attend_Dlg.SetitemToolTip(packet2.m_nItemUnique, packet.m_nItemCount);
								}
							}
							else
							{
								Normal_Attend_Dlg normal_Attend_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EVENT_NORMAL_ATTEND) as Normal_Attend_Dlg;
								if (normal_Attend_Dlg != null)
								{
									normal_Attend_Dlg.InitSet();
									normal_Attend_Dlg.SetitemToolTip(packet2.m_nItemUnique, packet.m_nItemCount);
								}
							}
							goto IL_5E6;
						}
						}
						goto IL_77;
					case E_ITEM_CREATE_TYPE.E_ITEM_CREATE_TYPE_DAILYDUNGEON_EVENT_ITEM:
					{
						GetItemDlg getItemDlg2 = (GetItemDlg)NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.GET_ITEM_DLG);
						if (getItemDlg2 != null)
						{
							getItemDlg2.SetItem(packet2.m_nItemUnique, packet.m_nItemCount, packet2.m_nRank);
							getItemDlg2.SetIndex(1);
						}
						goto IL_5E6;
					}
					}
					goto IL_61;
				}
				goto IL_43;
				IL_5E6:
				if (empty != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
				}
				b += 1;
			}
			if (NrTSingleton<ItemMallItemManager>.Instance.IsItemVoucherType((eVOUCHER_TYPE)packet.ui8VoucherType))
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.SetVoucherRefreshTime((eVOUCHER_TYPE)packet.ui8VoucherType, packet.i64ItemMallID, packet.i64VoucherRefreshTime);
			}
		}

		public static void GS_ITEM_PROPERTY_UPDATE_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEM_PROPERTY_UPDATE_NFY packet = kDeserializePacket.GetPacket<GS_ITEM_PROPERTY_UPDATE_NFY>();
			ITEM item = NkUserInventory.GetInstance().GetItem(packet.m_nPosType, packet.m_nItemPos);
			if (item != null)
			{
				switch (packet.m_byPROPERTY_IDX)
				{
				case 0:
					item.m_nItemNum = (int)((short)packet.m_lVAR);
					break;
				case 1:
					item.m_nRank = (int)packet.m_lVAR;
					break;
				case 2:
					item.m_nDurability = (int)((byte)packet.m_lVAR);
					break;
				case 3:
					item.m_nLock = (int)((byte)packet.m_lVAR);
					break;
				}
			}
		}

		public static void GS_CONGRATULATORY_MESSAGE_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_CONGRATULATORY_MESSAGE_NFY packet = kDeserializePacket.GetPacket<GS_CONGRATULATORY_MESSAGE_NFY>();
			Congraturation_DLG congraturation_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CONGRATURATIONDLG) as Congraturation_DLG;
			if (congraturation_DLG != null)
			{
				congraturation_DLG.PushMessage(packet);
				congraturation_DLG.Show();
			}
		}

		public static void GS_QUEST_GROUP_REWARD_ACK(NkDeserializePacket kDeserializePacket)
		{
		}

		public static void GS_ITEM_ENABLE_SLOT_NFY(NkDeserializePacket kDeserializePacket)
		{
		}

		public static void GS_ITEM_ENABLE_SLOT_ADD_ACK(NkDeserializePacket kDeserializePacket)
		{
		}

		public static void GS_ITEM_USELEVEL_DEC_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEM_USELEVEL_DEC_ACK packet = kDeserializePacket.GetPacket<GS_ITEM_USELEVEL_DEC_ACK>();
			if (0 >= packet.i32Result)
			{
				ReduceMainDlg reduceMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REDUCEMAIN_DLG) as ReduceMainDlg;
				if (reduceMainDlg != null)
				{
					reduceMainDlg.SetEnable(true);
				}
			}
		}

		public static void GS_ITEM_SELL_ACK(NkDeserializePacket kDeserializePacket)
		{
			NrTSingleton<GameGuideManager>.Instance.RemoveEquipGuide();
			NrTSingleton<GameGuideManager>.Instance.RemoveGuide(GameGuideType.SELL_ITEM);
			GS_ITEM_SELL_ACK packet = kDeserializePacket.GetPacket<GS_ITEM_SELL_ACK>();
			if (packet.i32Result == 0)
			{
				string empty = string.Empty;
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("845");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromNotify,
					"count",
					ANNUALIZED.Convert(packet.i64TotalSellMoney)
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				if (0L < packet.i64AfterMoney)
				{
					NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64AfterMoney;
				}
			}
			else
			{
				int i32Result = packet.i32Result;
				switch (i32Result + 3)
				{
				case 0:
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("726"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
					break;
				case 1:
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("398"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
					NrTSingleton<GameGuideManager>.Instance.InitReserveGuide();
					NrTSingleton<GameGuideManager>.Instance.CheckGameGuide(GameGuideType.EQUIP_SELL);
					break;
				case 2:
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
					break;
				}
			}
		}

		public static void GS_ITEM_SELL_MULTI_ACK(NkDeserializePacket kDeserializePacket)
		{
			NrTSingleton<GameGuideManager>.Instance.RemoveEquipGuide();
			NrTSingleton<GameGuideManager>.Instance.RemoveGuide(GameGuideType.SELL_ITEM);
			GS_ITEM_SELL_MULTI_ACK packet = kDeserializePacket.GetPacket<GS_ITEM_SELL_MULTI_ACK>();
			TsLog.LogWarning(" Current Money = {0} , Add Money {1} , Total Money {2}", new object[]
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money,
				packet.i64CurTotalSellMoney,
				packet.i64AfterMoney
			});
			if (packet.i32Result == 0)
			{
				int num = 0;
				for (int i = 0; i < 30; i++)
				{
					if (0L < packet.i64ItemID[i])
					{
						num++;
					}
				}
				string empty = string.Empty;
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("845");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromNotify,
					"count",
					ANNUALIZED.Convert(packet.i64CurTotalSellMoney)
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				if (0L < packet.i64AfterMoney)
				{
					NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64AfterMoney;
				}
			}
			else
			{
				int i32Result = packet.i32Result;
				if (i32Result != -2)
				{
					if (i32Result == -1)
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
					}
				}
				else
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("398"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
					NrTSingleton<GameGuideManager>.Instance.InitReserveGuide();
					NrTSingleton<GameGuideManager>.Instance.CheckGameGuide(GameGuideType.EQUIP_SELL);
				}
			}
		}

		public static void GS_ITEM_SELL_AUTO_BABEL_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEM_SELL_AUTO_BABEL_ACK packet = kDeserializePacket.GetPacket<GS_ITEM_SELL_AUTO_BABEL_ACK>();
			int i32Result = packet.i32Result;
			switch (i32Result + 3)
			{
			}
		}

		public static void GS_ITEM_LOCK_MULTI_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEM_LOCK_MULTI_ACK packet = kDeserializePacket.GetPacket<GS_ITEM_LOCK_MULTI_ACK>();
			if (packet.i32Result == -101)
			{
				TsLog.LogWarning(string.Format("GS_ITEM_LOCK_MULTI_ACK result: all item fail", packet.i32Result), new object[0]);
			}
			else
			{
				for (int i = 0; i < packet.i64ItemID.Length; i++)
				{
					if (packet.i64ItemID[i] != 0L)
					{
						ITEM itemFromItemID = NkUserInventory.GetInstance().GetItemFromItemID(packet.i64ItemID[i]);
						if (itemFromItemID != null)
						{
							itemFromItemID.m_nLock = ((!packet.bLocked[i]) ? 0 : 1);
						}
					}
				}
				Inventory_Dlg inventory_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.INVENTORY_DLG) as Inventory_Dlg;
				if (inventory_Dlg != null)
				{
					inventory_Dlg.ResetItemLock();
				}
			}
		}

		public static void GS_POINT_BUY_ACK(NkDeserializePacket kDeserializePacket)
		{
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo == null)
			{
				return;
			}
			GS_POINT_BUY_ACK packet = kDeserializePacket.GetPacket<GS_POINT_BUY_ACK>();
			if (packet.nResult == 0)
			{
				long num = 0L;
				if (packet.nType == 0)
				{
					num = myCharInfo.GetHeroPoint();
					myCharInfo.SetHeroPoint(packet.nPoint);
				}
				else if (packet.nType == 1)
				{
					num = myCharInfo.GetEquipPoint();
					myCharInfo.SetEquipPoint(packet.nPoint);
				}
				if (packet.nAddPointType == 0)
				{
					if (packet.nType == 0)
					{
						string empty = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("641"),
							"count",
							packet.nPoint - num
						});
						Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
					}
					else if (packet.nType == 1)
					{
						string empty2 = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("642"),
							"count",
							packet.nPoint - num
						});
						Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
					}
					ExchangePointDlg exchangePointDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EXCHANGE_POINT_DLG) as ExchangePointDlg;
					if (exchangePointDlg != null)
					{
						exchangePointDlg.UpdateUI();
					}
				}
				else
				{
					if (-1 < packet.nIndex)
					{
						myCharInfo.SetCharDetail(packet.nIndex, packet.nValue);
					}
					ExchangeItemDlg exchangeItemDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EXCHANGE_ITEM_DLG) as ExchangeItemDlg;
					if (exchangeItemDlg != null)
					{
						exchangeItemDlg.ResultMessage();
						exchangeItemDlg.UpdateUI();
					}
				}
			}
		}

		public static void GS_EXCHANGE_ITEM_ACK(NkDeserializePacket kDeserializePacket)
		{
			if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo() == null)
			{
				return;
			}
			GS_EXCHANGE_ITEM_ACK packet = kDeserializePacket.GetPacket<GS_EXCHANGE_ITEM_ACK>();
			if (packet.nResult == 0)
			{
				ExchangeItemDlg exchangeItemDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EXCHANGE_ITEM_DLG) as ExchangeItemDlg;
				if (exchangeItemDlg != null)
				{
					exchangeItemDlg.Result();
				}
				else
				{
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("607"),
						"targetname",
						NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.nItemUnique),
						"count",
						packet.nItemNum
					});
					Main_UI_SystemMessage.ADDMessage(empty);
				}
			}
		}

		public static void GS_EXCHANGE_JEWELRY_ACK(NkDeserializePacket kDeserializePacket)
		{
			if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo() == null)
			{
				return;
			}
			GS_EXCHANGE_JEWELRY_ACK packet = kDeserializePacket.GetPacket<GS_EXCHANGE_JEWELRY_ACK>();
			if (packet.nResult == 0)
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("645"),
					"targetname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.nItemUnique),
					"count",
					packet.nItemNum
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				ExchangeJewelryDlg exchangeJewelryDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EXCHANGE_JEWELRY_DLG) as ExchangeJewelryDlg;
				if (exchangeJewelryDlg != null)
				{
					exchangeJewelryDlg.Update_List();
				}
			}
		}

		public static void GS_EXCHANGE_MYTHICSOL_ACK(NkDeserializePacket kDeserializePacket)
		{
			if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo() == null)
			{
				return;
			}
			GS_EXCHANGE_MYTHICSOL_ACK packet = kDeserializePacket.GetPacket<GS_EXCHANGE_MYTHICSOL_ACK>();
			if (packet.nResult == 0)
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("645"),
					"targetname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.nItemUnique),
					"count",
					packet.nItemNum
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				ExchangeMythicSolDlg exchangeMythicSolDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EXCHANGE_MYTHICSOL_DLG) as ExchangeMythicSolDlg;
				if (exchangeMythicSolDlg != null)
				{
					exchangeMythicSolDlg.UpdateUI();
				}
			}
		}

		public static void GS_EXCHANGE_EVOLUTION_ACK(NkDeserializePacket kDeserializePacket)
		{
			if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo() == null)
			{
				return;
			}
			GS_EXCHANGE_EVOLUTION_ACK packet = kDeserializePacket.GetPacket<GS_EXCHANGE_EVOLUTION_ACK>();
			if (packet.nResult == 0)
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("645"),
					"targetname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.nItemUnique),
					"count",
					packet.nItemNum
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				ExchangeEvolutionDlg exchangeEvolutionDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EXCHANGE_EVOLUTION_DLG) as ExchangeEvolutionDlg;
				if (exchangeEvolutionDlg != null)
				{
					exchangeEvolutionDlg.Update_List();
				}
			}
		}

		public static void GS_MYTHICSOLLIMIT_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MYTHICSOLLIMIT_INFO_ACK packet = kDeserializePacket.GetPacket<GS_MYTHICSOLLIMIT_INFO_ACK>();
			NrTSingleton<PointManager>.Instance.ClearMythicSolLimit();
			for (int i = 0; i < 10; i++)
			{
				if (packet.LimitSol[i] != 0)
				{
					NrTSingleton<PointManager>.Instance.AddMythicSolLimit(packet.LimitSol[i]);
				}
			}
			ExchangeMythicSolDlg exchangeMythicSolDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EXCHANGE_MYTHICSOL_DLG) as ExchangeMythicSolDlg;
			if (exchangeMythicSolDlg != null)
			{
				exchangeMythicSolDlg.ShowUI();
			}
		}

		public static void GS_EXCHANGE_GUILDWAR_CHECK_ACK(NkDeserializePacket kDeserializePacket)
		{
			if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo() == null)
			{
				return;
			}
			GS_EXCHANGE_GUILDWAR_CHECK_ACK packet = kDeserializePacket.GetPacket<GS_EXCHANGE_GUILDWAR_CHECK_ACK>();
			if (packet.nResult == 0)
			{
				ExchangeGuildWarDlg exchangeGuildWarDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EXCHANGE_GUILDWAR_DLG) as ExchangeGuildWarDlg;
				if (exchangeGuildWarDlg != null)
				{
					if (packet.nExchangeLimit != -1)
					{
						exchangeGuildWarDlg.AddExchangeLimitUpdate(packet.nItemUnique, packet.nExchangeLimit);
						exchangeGuildWarDlg.ResultMessage();
						exchangeGuildWarDlg.UpdateUI();
					}
					else
					{
						exchangeGuildWarDlg.ResultMessage();
						exchangeGuildWarDlg.UpdateUI();
					}
				}
			}
		}

		public static void GS_EXCHANGE_GUILDWAR_LIMITCOUNT_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo() == null)
			{
				return;
			}
			GS_EXCHANGE_GUILDWAR_LIMITCOUNT_INFO_ACK packet = kDeserializePacket.GetPacket<GS_EXCHANGE_GUILDWAR_LIMITCOUNT_INFO_ACK>();
			ExchangeGuildWarDlg exchangeGuildWarDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EXCHANGE_GUILDWAR_DLG) as ExchangeGuildWarDlg;
			if (exchangeGuildWarDlg != null)
			{
				for (int i = 0; i < 30; i++)
				{
					if (packet.nExchangeLimit[i] != -1)
					{
						exchangeGuildWarDlg.AddExchangeLimitUpdate(packet.nItemUnique[i], packet.nExchangeLimit[i]);
					}
				}
			}
		}

		public static void GS_COSTUME_INFO_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_COSTUME_INFO_NFY packet = kDeserializePacket.GetPacket<GS_COSTUME_INFO_NFY>();
			if (packet == null)
			{
				return;
			}
			if (packet.Result != 0)
			{
				return;
			}
			for (int i = 0; i < (int)packet.i16Count; i++)
			{
				COSTUME_INFO packet2 = kDeserializePacket.GetPacket<COSTUME_INFO>();
				if (packet2 != null)
				{
					if (TsSceneSwitcher.Instance.CurrentSceneType == TsSceneSwitcher.ESceneType.WorldScene)
					{
						COSTUME_INFO costumeInfo = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeInfo(packet2.i32CostumeUnique);
						if (costumeInfo == null || costumeInfo.i32CostumeCount < packet2.i32CostumeCount)
						{
							string text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("861");
							text = text.Replace("@costumename@", NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeName(packet2.i32CostumeUnique));
							Main_UI_SystemMessage.ADDMessage(text);
						}
					}
					NrTSingleton<NrCharCostumeTableManager>.Instance.InsertCostumeData(packet2);
				}
			}
			NrTSingleton<CostumeWearManager>.Instance.Refresh(null, true, true);
		}

		public static void GS_TIMESHOP_ITEMLIST_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_TIMESHOP_ITEMLIST_INFO_ACK packet = kDeserializePacket.GetPacket<GS_TIMESHOP_ITEMLIST_INFO_ACK>();
			if (packet.i32Result == 0)
			{
				if (packet.i32ItemListCount <= 0)
				{
					return;
				}
				if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
				{
					return;
				}
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.Clear_UserTimeShopItemList();
				for (int i = 0; i < packet.i32ItemListCount; i++)
				{
					TIMESHOP_ITEMINFO packet2 = kDeserializePacket.GetPacket<TIMESHOP_ITEMINFO>();
					if (packet2 != null)
					{
						NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.Add_UserTimeShopItemList(packet2);
					}
				}
				short refreshCount = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.RefreshCount;
				if (packet.i16RefreshCount > refreshCount)
				{
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("872")
					});
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.Set_UserTimeShopInfo(packet.i16RefreshCount, packet.i64NextRefreshTime);
				TimeShop_DLG timeShop_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TIMESHOP_DLG) as TimeShop_DLG;
				if (timeShop_DLG != null)
				{
					timeShop_DLG.Set_UserInfo();
				}
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TIMESHOP_DLG);
			}
		}

		public static void GS_TIMESHOP_ITEMLIST_REFRESH_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_TIMESHOP_ITEMLIST_REFRESH_ACK packet = kDeserializePacket.GetPacket<GS_TIMESHOP_ITEMLIST_REFRESH_ACK>();
			if (packet.i32Result != 0)
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("859")
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
		}

		public static void GS_TIMESHOP_ITEM_BUY_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_TIMESHOP_ITEM_BUY_ACK packet = kDeserializePacket.GetPacket<GS_TIMESHOP_ITEM_BUY_ACK>();
			if (packet.i32Result == 0)
			{
				if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
				{
					return;
				}
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.Set_UserTimeShopItemBuy(packet.i64IDX, packet.i8Buy);
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64Money;
				TimeShop_DLG timeShop_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TIMESHOP_DLG) as TimeShop_DLG;
				if (timeShop_DLG != null)
				{
					timeShop_DLG.Set_ItemList();
				}
				string message = string.Empty;
				if (packet.bIsInvenFull)
				{
					message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("874");
					NoticeIconDlg.SetIcon(ICON_TYPE.POST, true);
				}
				else
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref message, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("195")
					});
				}
				Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMMALL_PRODUCTDETAIL_DLG);
			}
			else if (packet.i32Result == 9754)
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("873")
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			else
			{
				string empty2 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("859")
				});
				Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
		}

		public static void GS_ITEMSHOP_ITEMPOPUP_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEMSHOP_ITEMPOPUP_INFO_ACK packet = kDeserializePacket.GetPacket<GS_ITEMSHOP_ITEMPOPUP_INFO_ACK>();
			if (packet.i32Result == 0)
			{
				ItemMallDlg_ChallengeQuest itemMallDlg_ChallengeQuest = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMMALL_CHALLENGEQUEST_DLG) as ItemMallDlg_ChallengeQuest;
				if (itemMallDlg_ChallengeQuest != null)
				{
					return;
				}
				SolRecruitDlg_ChallengeQuest solRecruitDlg_ChallengeQuest = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLRECRUIT_CHALLENGEQUEST_DLG) as SolRecruitDlg_ChallengeQuest;
				if (solRecruitDlg_ChallengeQuest != null)
				{
					return;
				}
				long voucherRemainTimeFromItemID = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetVoucherRemainTimeFromItemID(packet.i64ShopIdx);
				if (voucherRemainTimeFromItemID <= 0L)
				{
					PoPupShopDlg poPupShopDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMMALL_POPUPSHOP_DLG) as PoPupShopDlg;
					if (poPupShopDlg != null)
					{
						poPupShopDlg.SetData((long)packet.i32Idx);
					}
				}
			}
		}

		public static void GS_ITEMSHOP_ITEMPOPUP_DAYCOOLTIME_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEMSHOP_ITEMPOPUP_DAYCOOLTIME_SET_ACK packet = kDeserializePacket.GetPacket<GS_ITEMSHOP_ITEMPOPUP_DAYCOOLTIME_SET_ACK>();
			if (packet.i32Result == 0)
			{
				POPUPSHOP_DATA pOPUPSHOP_DATA = new POPUPSHOP_DATA();
				pOPUPSHOP_DATA.i32Idx = packet.i32Idx;
				pOPUPSHOP_DATA.i32MaxBuyCount = packet.i32MaxBuyCount;
				pOPUPSHOP_DATA.i64CoolTime = packet.i64CoolTime;
				pOPUPSHOP_DATA.i64DayEndTime = packet.i64DayEndTime;
				NrTSingleton<ItemMallPoPupShopManager>.Instance.Set_ServerValue(pOPUPSHOP_DATA);
			}
		}

		public static void GS_EXCHANGE_EVENT_ITEM_ACK(NkDeserializePacket kDeserializePacket)
		{
			if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo() == null)
			{
				return;
			}
			GS_EXCHANGE_EVENT_ITEM_ACK packet = kDeserializePacket.GetPacket<GS_EXCHANGE_EVENT_ITEM_ACK>();
			if (packet.nResult == 0)
			{
				EventItem_ExchangDlg eventItem_ExchangDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EXCHANGE_EVENTITEM_DLG) as EventItem_ExchangDlg;
				if (eventItem_ExchangDlg != null)
				{
					eventItem_ExchangDlg.ReflashData(packet.nItemUnique, packet.nSelectNum);
				}
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("607"),
					"targetname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.nItemUnique),
					"count",
					packet.nSelectNum
				});
				Main_UI_SystemMessage.ADDMessage(empty);
			}
		}

		public static void GS_EXCHANGE_EVENTITEM_LIMITCOUNT_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_EXCHANGE_EVENTITEM_LIMITCOUNT_INFO_ACK packet = kDeserializePacket.GetPacket<GS_EXCHANGE_EVENTITEM_LIMITCOUNT_INFO_ACK>();
			EventItem_ExchangDlg eventItem_ExchangDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EXCHANGE_EVENTITEM_DLG) as EventItem_ExchangDlg;
			if (eventItem_ExchangDlg != null)
			{
				for (int i = 0; i < (int)packet.i16Count; i++)
				{
					ITEM_EXCHANGE_LIMIT packet2 = kDeserializePacket.GetPacket<ITEM_EXCHANGE_LIMIT>();
					eventItem_ExchangDlg.AddLimitData(packet2);
				}
				eventItem_ExchangDlg.ShowUI();
			}
		}

		public static void GS_ITEM_MATERIAL_USE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEM_MATERIAL_USE_ACK packet = kDeserializePacket.GetPacket<GS_ITEM_MATERIAL_USE_ACK>();
			TsLog.LogWarning("=== GS_ITEM_MATERIAL_USE_ACK - {0}", new object[]
			{
				packet.m_i32Result
			});
			if (packet.m_i32Result == 0)
			{
				NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
				if (kMyCharInfo == null)
				{
					return;
				}
				kMyCharInfo.SetCharSubData(packet.m_i32CharSubDataType, packet.m_i32CharSubDataValue);
				if (packet.m_bType == 2)
				{
					Myth_Evolution_Main_DLG myth_Evolution_Main_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTH_EVOLUTION_MAIN_DLG) as Myth_Evolution_Main_DLG;
					if (myth_Evolution_Main_DLG != null)
					{
						myth_Evolution_Main_DLG.SetEvolution();
						myth_Evolution_Main_DLG.SetBaseSol(packet.i64SolID);
					}
				}
				else if (packet.m_bType == 1)
				{
					Myth_Legend_Info_DLG myth_Legend_Info_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTH_LEGEND_INFO_DLG) as Myth_Legend_Info_DLG;
					if (myth_Legend_Info_DLG != null)
					{
						myth_Legend_Info_DLG.InitSetCharKind(packet.i32CharKind);
					}
				}
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("911"), SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
			}
		}

		public static void GS_BOX_USE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BOX_USE_ACK packet = kDeserializePacket.GetPacket<GS_BOX_USE_ACK>();
			if (packet.m_nResult == 0)
			{
				int i = 0;
				while (i < 12)
				{
					if (packet.m_caAddItem[i].m_nItemUnique > 0)
					{
						i++;
					}
					else
					{
						if (i == 0)
						{
							NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEM_BOX_RANDOM_DLG);
							string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("44");
							Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
							return;
						}
						break;
					}
				}
				ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(packet.m_lUnique);
				if (itemInfo != null && itemInfo.IsItemATB(16L))
				{
					if (itemInfo.IsItemATB(16384L))
					{
						Item_Box_RareRandom_Dlg item_Box_RareRandom_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEM_BOX_RARERANDOM_DLG) as Item_Box_RareRandom_Dlg;
						if (item_Box_RareRandom_Dlg != null)
						{
							item_Box_RareRandom_Dlg.Set_Item_Complete(packet.m_caAddItem[0], packet.m_naAddItemNum[0], packet.m_nItemNum);
						}
					}
					else if (packet.m_byAllOpen == 1)
					{
						ItemBoxContinue_Dlg itemBoxContinue_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEM_BOX_CONTINUE_DLG) as ItemBoxContinue_Dlg;
						if (itemBoxContinue_Dlg != null)
						{
							itemBoxContinue_Dlg.RefreshData(packet);
						}
						Item_Box_Random_Result_Dlg item_Box_Random_Result_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEM_BOX_RANDOM_RESULT) as Item_Box_Random_Result_Dlg;
						if (item_Box_Random_Result_Dlg != null)
						{
							item_Box_Random_Result_Dlg.SetData(packet);
						}
					}
				}
			}
			else
			{
				eRESULT nResult = (eRESULT)packet.m_nResult;
				switch (nResult)
				{
				case eRESULT.R_ITEM_NUM:
					NrTSingleton<ItemManager>.Instance.CheckBoxNeedItem(packet.m_lUnique, false, true);
					goto IL_277;
				case eRESULT.R_ITEM_SEAL_INFO:
				{
					IL_164:
					if (nResult == eRESULT.R_FAIL_ITEM)
					{
						string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("2060");
						Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
						goto IL_277;
					}
					if (nResult == eRESULT.R_FAIL_INVENTORY_FULL)
					{
						string textFromNotify3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46");
						Main_UI_SystemMessage.ADDMessage(textFromNotify3, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
						goto IL_277;
					}
					if (nResult == eRESULT.R_FAIL_INVENTORY_FULL_TICKET)
					{
						string textFromNotify4 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("773");
						Main_UI_SystemMessage.ADDMessage(textFromNotify4, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
						goto IL_277;
					}
					if (nResult != eRESULT.R_BOX_NOT_ARRAY_INDEX)
					{
						string message = "Box Item Result = " + packet.m_nResult;
						Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.DEBUG_MESSAGE);
						goto IL_277;
					}
					string message2 = "Box Item Not Array Index";
					Main_UI_SystemMessage.ADDMessage(message2, SYSTEM_MESSAGE_TYPE.DEBUG_MESSAGE);
					goto IL_277;
				}
				case eRESULT.R_ITEM_HIGH_LEVEL:
				{
					string textFromNotify5 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("483");
					Main_UI_SystemMessage.ADDMessage(textFromNotify5, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					goto IL_277;
				}
				case eRESULT.R_ITEM_LOW_LEVEL:
				{
					string textFromNotify6 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("356");
					Main_UI_SystemMessage.ADDMessage(textFromNotify6, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					goto IL_277;
				}
				}
				goto IL_164;
				IL_277:
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEM_BOX_RANDOM_DLG);
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEM_BOX_SELECT_DLG);
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEM_BOX_ALL_DLG);
			}
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEM_BOX_SELECT_DLG);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEM_BOX_ALL_DLG);
		}

		public static void GS_ITEMMALL_BOX_TRADE_ACK(NkDeserializePacket kDeserializePacket)
		{
		}

		public static void GS_ITEM_SUPPLY_USE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEM_SUPPLY_USE_ACK packet = kDeserializePacket.GetPacket<GS_ITEM_SUPPLY_USE_ACK>();
			eITEM_SUPPLY_FUNCTION eITEM_SUPPLY_FUNCTION = Protocol_Item.Get_Item_Supplies_Function_Index(packet.m_nItemUnique);
			if (packet.m_nResult == 0)
			{
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet.m_nDestSolID);
				if (soldierInfoFromSolID == null)
				{
					return;
				}
				NrSoldierList charSoldierList = NrTSingleton<NkCharManager>.Instance.GetCharSoldierList(1);
				eITEM_SUPPLY_FUNCTION eITEM_SUPPLY_FUNCTION2 = eITEM_SUPPLY_FUNCTION;
				switch (eITEM_SUPPLY_FUNCTION2)
				{
				case eITEM_SUPPLY_FUNCTION.SUPPLY_HPHEAL:
					charSoldierList.AddHP(packet.m_shPara2, packet.m_nParaArray1);
					goto IL_2D9;
				case eITEM_SUPPLY_FUNCTION.SUPPLY_MPHEAL:
					goto IL_2D9;
				case eITEM_SUPPLY_FUNCTION.SUPPLY_ALLHEAL:
					charSoldierList.AddHP(packet.m_shPara2, packet.m_nParaArray1);
					goto IL_2D9;
				case eITEM_SUPPLY_FUNCTION.SUPPLY_EXP:
				{
					ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(packet.m_nItemUnique);
					if (itemInfo != null)
					{
						string empty = string.Empty;
						string text = itemInfo.m_nParam[0].ToString();
						string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("359");
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							textFromNotify,
							"targetname1",
							soldierInfoFromSolID.GetName(),
							"targetname2",
							text
						});
						Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					}
					goto IL_2D9;
				}
				case eITEM_SUPPLY_FUNCTION.SUPPLY_ACTIVEUP:
					goto IL_2D9;
				case eITEM_SUPPLY_FUNCTION.SUPPLY_WARP:
				case eITEM_SUPPLY_FUNCTION.SUPPLY_GETSOLDIER:
				case eITEM_SUPPLY_FUNCTION.SUPPLY_INJURYCURE:
					IL_83:
					if (eITEM_SUPPLY_FUNCTION2 != eITEM_SUPPLY_FUNCTION.SUPPLY_QUESTITEM)
					{
						goto IL_2D9;
					}
					goto IL_2D9;
				case eITEM_SUPPLY_FUNCTION.SUPPLY_NEARBYNPC:
				{
					NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique((short)packet.m_shPara1);
					if (charByCharUnique == null)
					{
						return;
					}
					NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
					if (@char == null)
					{
						return;
					}
					@char.MoveTo(charByCharUnique.GetCharObject().transform.position);
					NrTSingleton<NkClientLogic>.Instance.SetPickChar(charByCharUnique);
					goto IL_2D9;
				}
				case eITEM_SUPPLY_FUNCTION.SUPPLY_PROTECTTIME:
				{
					ITEMINFO itemInfo2 = NrTSingleton<ItemManager>.Instance.GetItemInfo(packet.m_nItemUnique);
					if (itemInfo2 != null)
					{
						string empty2 = string.Empty;
						string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("274");
						int num = itemInfo2.m_nParam[0] / 60;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
						{
							textFromNotify2,
							"timestring",
							num.ToString()
						});
						Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					}
					goto IL_2D9;
				}
				case eITEM_SUPPLY_FUNCTION.SUPPLY_ADDMONEY:
				{
					ITEMINFO itemInfo3 = NrTSingleton<ItemManager>.Instance.GetItemInfo(packet.m_nItemUnique);
					if (itemInfo3 != null)
					{
						string empty3 = string.Empty;
						string textFromNotify3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("478");
						long num2 = packet.m_ni64CurrnetMoney - NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money;
						NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.m_ni64CurrnetMoney;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
						{
							textFromNotify3,
							"targetname",
							NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.m_nItemUnique),
							"money",
							ANNUALIZED.Convert(num2)
						});
						Main_UI_SystemMessage.ADDMessage(empty3, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					}
					goto IL_2D9;
				}
				}
				goto IL_83;
				IL_2D9:
				string itemMaterialCode = NrTSingleton<ItemManager>.Instance.GetItemMaterialCode(packet.m_nItemUnique);
				if (!string.IsNullOrEmpty(itemMaterialCode))
				{
					TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_ITEM", itemMaterialCode, "USE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				}
			}
			else if (packet.m_nResult == 811)
			{
				COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
				if (instance == null)
				{
					return;
				}
				int value = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_FASTBATTLE_MAXNUM);
				string empty4 = string.Empty;
				string textFromNotify4 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("801");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty4, new object[]
				{
					textFromNotify4,
					"count",
					value
				});
				Main_UI_SystemMessage.ADDMessage(empty4, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			else if (packet.m_nResult == 817)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("812"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			else if (packet.m_nResult == 814)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			else
			{
				switch (eITEM_SUPPLY_FUNCTION)
				{
				case eITEM_SUPPLY_FUNCTION.SUPPLY_NEARBYNPC:
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("455"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					NrCharBase char2 = NrTSingleton<NkCharManager>.Instance.GetChar(1);
					if (char2 != null)
					{
						char2.MakeChatText(NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo("20223"), true);
					}
					break;
				}
				case eITEM_SUPPLY_FUNCTION.SUPPLY_PROTECTTIME:
				{
					string textFromNotify5 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("314");
					Main_UI_SystemMessage.ADDMessage(textFromNotify5, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					break;
				}
				}
			}
		}

		public static void GS_ITEM_SUPPLY_CANCEL_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_ITEM_SUPPLY_CANCEL_NFY packet = kDeserializePacket.GetPacket<GS_ITEM_SUPPLY_CANCEL_NFY>();
			switch (packet.m_nSupplyFunction)
			{
			}
		}

		public static void GS_MARKET_LIST_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MARKET_LIST_ACK packet = kDeserializePacket.GetPacket<GS_MARKET_LIST_ACK>();
			TsLog.Log(packet.m_nItemNum.ToString(), new object[0]);
		}

		public static void GS_MARKET_PRICE_LIST_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MARKET_PRICE_LIST_ACK packet = kDeserializePacket.GetPacket<GS_MARKET_PRICE_LIST_ACK>();
			TsLog.Log(packet.m_nItemNum.ToString(), new object[0]);
		}

		public static void GS_MARKET_SELL_LIST_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MARKET_SELL_LIST_ACK packet = kDeserializePacket.GetPacket<GS_MARKET_SELL_LIST_ACK>();
			TsLog.Log(packet.m_nItemNum.ToString(), new object[0]);
		}

		public static void GS_MARKET_REGISTRY_SELL_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MARKET_REGISTRY_SELL_ACK packet = kDeserializePacket.GetPacket<GS_MARKET_REGISTRY_SELL_ACK>();
			if (packet.m_nResult == 0)
			{
				ITEM item = NkUserInventory.GetInstance().GetItem(packet.m_byPosType, packet.m_shPosItem);
				if (item != null)
				{
					string empty = string.Empty;
					string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(item);
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("61");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						textFromNotify,
						"Item_Name",
						itemNameByItemUnique
					});
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.m_lMoney;
				SendPacket.GetInstance().SendIDType(304);
			}
			NrTSingleton<GameGuideManager>.Instance.RemoveEquipGuide();
		}

		public static void GS_MARKET_REGISTRY_MULTISELL_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MARKET_REGISTRY_MULTISELL_ACK packet = kDeserializePacket.GetPacket<GS_MARKET_REGISTRY_MULTISELL_ACK>();
			if (packet.m_nResult == 0)
			{
				string empty = string.Empty;
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("845");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromNotify,
					"count",
					packet.m_lSellItemAllMoney
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				SendPacket.GetInstance().SendIDType(304);
			}
			NrTSingleton<GameGuideManager>.Instance.RemoveEquipGuide();
		}

		public static void GS_MARKET_REGISTRY_SELL_CANCEL_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MARKET_REGISTRY_SELL_CANCEL_ACK packet = kDeserializePacket.GetPacket<GS_MARKET_REGISTRY_SELL_CANCEL_ACK>();
			eRESULT nResult = (eRESULT)packet.m_nResult;
			if (nResult == eRESULT.R_OK)
			{
				string empty = string.Empty;
				string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.m_cItem.m_nItemUnique);
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("62");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromNotify,
					"Item_Name",
					itemNameByItemUnique
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			else
			{
				eRESULT eRESULT = nResult;
				if (eRESULT == eRESULT.R_MARKET_SELL_CANCEL)
				{
					string empty2 = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("337"),
						"targetname",
						NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.m_cItem.m_nItemUnique)
					});
					Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					SendPacket.GetInstance().SendIDType(304);
				}
			}
		}

		public static void GS_MARKET_BUY_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MARKET_BUY_ACK packet = kDeserializePacket.GetPacket<GS_MARKET_BUY_ACK>();
			eRESULT nResult = (eRESULT)packet.m_nResult;
			if (nResult == eRESULT.R_OK)
			{
				NkUserInventory.GetInstance().SetInfo(packet.m_cItem, 8);
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.m_lMoney;
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("50");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				GS_MARKET_LIST_REQ gS_MARKET_LIST_REQ = new GS_MARKET_LIST_REQ();
				gS_MARKET_LIST_REQ.m_nItemUnique = packet.m_cItem.m_nItemUnique;
				gS_MARKET_LIST_REQ.m_nMode = packet.m_nMode;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MARKET_LIST_REQ, gS_MARKET_LIST_REQ);
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MARKET", "BUY", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			}
			else
			{
				eRESULT eRESULT = nResult;
				if (eRESULT == eRESULT.R_MARKET_BUY)
				{
					string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("338");
					Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					GS_MARKET_LIST_REQ gS_MARKET_LIST_REQ2 = new GS_MARKET_LIST_REQ();
					gS_MARKET_LIST_REQ2.m_nItemUnique = packet.m_nItemUnique;
					gS_MARKET_LIST_REQ2.m_nMode = packet.m_nMode;
					SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MARKET_LIST_REQ, gS_MARKET_LIST_REQ2);
				}
			}
		}

		public static void GS_MARKET_CURRENT_SELLMONEY_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_MARKET_CURRENT_SELLMONEY_NFY packet = kDeserializePacket.GetPacket<GS_MARKET_CURRENT_SELLMONEY_NFY>();
			NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().SetEquipSellMoney(packet.m_lSellMoney, packet.bAttackPlunder);
		}

		public static void GS_MESSAGE_NFY(NkDeserializePacket kDeserializePacket)
		{
			string name = string.Empty;
			string empty = string.Empty;
			string text = string.Empty;
			kDeserializePacket.GetPacket<GS_MESSAGE_NFY>();
			MESSAGE_HEADER packet = kDeserializePacket.GetPacket<MESSAGE_HEADER>();
			eMESSAGE_ID i16Type = (eMESSAGE_ID)packet.i16Type;
			eMESSAGE_ID eMESSAGE_ID = i16Type;
			switch (eMESSAGE_ID)
			{
			case eMESSAGE_ID.MESSAGE_GUILD_MEMBER_JOINED:
			{
				MESSAGE_GUILD_MEMBER_JOINED packet2 = kDeserializePacket.GetPacket<MESSAGE_GUILD_MEMBER_JOINED>();
				string empty2 = string.Empty;
				string empty3 = string.Empty;
				string empty4 = string.Empty;
				string empty5 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
				{
					empty2,
					"Targetname",
					TKString.NEWString(packet2.szCharName)
				});
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty5, new object[]
				{
					empty4,
					"Targetname",
					TKString.NEWString(packet2.szCharName)
				});
				name = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("17");
				text = empty5;
				break;
			}
			case eMESSAGE_ID.MESSAGE_GUILD_MEMBER_LEFT:
			{
				MESSAGE_GUILD_MEMBER_LEFT packet3 = kDeserializePacket.GetPacket<MESSAGE_GUILD_MEMBER_LEFT>();
				string empty6 = string.Empty;
				string empty7 = string.Empty;
				string empty8 = string.Empty;
				string empty9 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty7, new object[]
				{
					empty6,
					"Targetname",
					TKString.NEWString(packet3.szCharName)
				});
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty9, new object[]
				{
					empty8,
					"Targetname",
					TKString.NEWString(packet3.szCharName)
				});
				name = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("17");
				text = empty9;
				break;
			}
			case eMESSAGE_ID.MESSAGE_GUILD_MEMBER_RANK_CHANGED:
			{
				MESSAGE_GUILD_MEMBER_RANK_CHANGED packet4 = kDeserializePacket.GetPacket<MESSAGE_GUILD_MEMBER_RANK_CHANGED>();
				string empty10 = string.Empty;
				string empty11 = string.Empty;
				string empty12 = string.Empty;
				string empty13 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty11, new object[]
				{
					empty10,
					"Targetname",
					TKString.NEWString(packet4.szCharName),
					"Position",
					packet4.byRank.ToString()
				});
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty13, new object[]
				{
					empty12,
					"Targetname",
					TKString.NEWString(packet4.szCharName),
					"Position",
					packet4.byRank.ToString()
				});
				text = empty13;
				break;
			}
			default:
				if (eMESSAGE_ID != eMESSAGE_ID.MESSAGE_FRIEND_LOGIN)
				{
					if (eMESSAGE_ID != eMESSAGE_ID.MESSAGE_FRIEND_LOGOUT)
					{
						if (eMESSAGE_ID != eMESSAGE_ID.MESSAGE_TEST)
						{
							if (eMESSAGE_ID != eMESSAGE_ID.MESSAGE_MARKET_ITEMSOLD)
							{
								if (eMESSAGE_ID != eMESSAGE_ID.MESSAGE_MAKEITEM_COMPLETED)
								{
									if (eMESSAGE_ID == eMESSAGE_ID.MESSAGE_NEW_MAIL)
									{
										NoticeIconDlg.SetIcon(ICON_TYPE.POST, true);
									}
								}
								else
								{
									MESSAGE_MAKEITEM_COMPLETED packet5 = kDeserializePacket.GetPacket<MESSAGE_MAKEITEM_COMPLETED>();
									string empty14 = string.Empty;
									string empty15 = string.Empty;
									string text2 = string.Empty;
									text2 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet5.nItemUnique);
									NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty15, new object[]
									{
										empty14,
										"Target_Item",
										text2,
										"Count",
										packet5.i32ItemNum.ToString()
									});
									empty = string.Empty;
									text = empty15;
									name = string.Empty;
								}
							}
							else
							{
								MESSAGE_TESTMESSAGE_MARKET_ITEMSOLD packet6 = kDeserializePacket.GetPacket<MESSAGE_TESTMESSAGE_MARKET_ITEMSOLD>();
								text = string.Concat(new object[]
								{
									"[",
									packet6.nItemUnique,
									"]아이템이 [",
									packet6.i32ItemNum,
									"] 개 판매되어 서신으로 ",
									packet6.i64TotalPrice,
									"이 들어왔습니다."
								});
							}
						}
						else
						{
							MESSAGE_TEST packet7 = kDeserializePacket.GetPacket<MESSAGE_TEST>();
							text = TKString.NEWString(packet7.msg);
						}
					}
					else
					{
						MESSAGE_FRIEND_LOGOUT packet8 = kDeserializePacket.GetPacket<MESSAGE_FRIEND_LOGOUT>();
						string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("56");
						name = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("326");
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
						{
							textFromNotify,
							"Charname",
							TKString.NEWString(packet8.szCharName)
						});
					}
				}
				else
				{
					MESSAGE_FRIEND_LOGIN packet9 = kDeserializePacket.GetPacket<MESSAGE_FRIEND_LOGIN>();
					string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("55");
					name = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("326");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						textFromNotify2,
						"Charname",
						TKString.NEWString(packet9.szCharName)
					});
				}
				break;
			}
			empty = string.Empty;
			if (empty != string.Empty)
			{
				MessageDlg messageDlg = (MessageDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MESSAGE_DLG);
				if (messageDlg != null)
				{
					messageDlg.AddMessage(empty, text);
				}
			}
			else if (text != string.Empty)
			{
				NrTSingleton<ChatManager>.Instance.PushSystemMsg(name, text);
			}
		}

		public static void GS_COMMUNITY_MESSAGE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_COMMUNITY_MESSAGE_ACK packet = kDeserializePacket.GetPacket<GS_COMMUNITY_MESSAGE_ACK>();
			TsLog.LogWarning("GS_COMMUNITY_MESSAGE_ACK!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", new object[0]);
			if (packet.byMessageType == 0)
			{
				GS_COMMUNITY_MESSAGE_LevelUP packet2 = kDeserializePacket.GetPacket<GS_COMMUNITY_MESSAGE_LevelUP>();
				TsLog.LogOnlyEditor("GS_COMMUNITY_MESSAGE_ACK Level : " + packet2.ui8Level);
			}
			else if (packet.byMessageType == 8)
			{
				GS_COMMUNITY_MESSAGE_QUESTCOMPLETE packet3 = kDeserializePacket.GetPacket<GS_COMMUNITY_MESSAGE_QUESTCOMPLETE>();
				Dictionary<int, CQuestGroup> hashQuestGroup = NrTSingleton<NkQuestManager>.Instance.GetHashQuestGroup();
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1563");
				string empty = string.Empty;
				foreach (CQuestGroup current in hashQuestGroup.Values)
				{
					if (current.GetGroupUnique() == packet3.i32QuestUniqe)
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							textFromInterface,
							"count1",
							current.GetChapterUnique(),
							"count2",
							current.GetPage()
						});
					}
				}
			}
			else if (packet.byMessageType == 9)
			{
				GS_COMMUNITY_MESSAGE_GENERAL_RECRUIT packet4 = kDeserializePacket.GetPacket<GS_COMMUNITY_MESSAGE_GENERAL_RECRUIT>();
				string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1567");
				string empty2 = string.Empty;
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(packet4.i32CharKind);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					textFromInterface2,
					"targetname",
					charKindInfo.GetName()
				});
				Congraturation_DLG congraturation_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CONGRATURATIONDLG) as Congraturation_DLG;
				if (congraturation_DLG != null)
				{
					congraturation_DLG.PushRECRUITMessage(packet4.i32CharKind, packet.szCharName, packet4.nGrade, packet.byReceibeUerType, packet4.i32ItemUnique);
					congraturation_DLG.Show();
				}
			}
			else if (packet.byMessageType != 2)
			{
				if (packet.byMessageType != 3)
				{
					if (packet.byMessageType == 1)
					{
						GS_COMMUNITY_MESSAGE_BATTLE_RESULT packet5 = kDeserializePacket.GetPacket<GS_COMMUNITY_MESSAGE_BATTLE_RESULT>();
						Congraturation_DLG congraturation_DLG2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CONGRATURATIONDLG) as Congraturation_DLG;
						if (congraturation_DLG2 != null)
						{
							congraturation_DLG2.PushBattleResultMessage(packet.szCharName, packet.i32Param, packet5, packet.byReceibeUerType);
							congraturation_DLG2.Show();
						}
					}
					else if (packet.byMessageType != 4)
					{
						if (packet.byMessageType != 5)
						{
							if (packet.byMessageType == 6 || packet.byMessageType == 18 || packet.byMessageType == 19)
							{
								GS_COMMUNITY_MESSAGE_ITEMGET packet6 = kDeserializePacket.GetPacket<GS_COMMUNITY_MESSAGE_ITEMGET>();
								Congraturation_DLG congraturation_DLG3 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CONGRATURATIONDLG) as Congraturation_DLG;
								if (congraturation_DLG3 != null)
								{
									congraturation_DLG3.PushItemGetMessage(packet6, packet.byReceibeUerType);
									congraturation_DLG3.Show();
								}
							}
							else if (packet.byMessageType != 7)
							{
								if (packet.byMessageType != 10)
								{
									if (packet.byMessageType != 11)
									{
										if (packet.byMessageType != 12)
										{
											if (packet.byMessageType == 13)
											{
												GS_COMMUNITY_MESSAGE_BABELTOWER_START packet7 = kDeserializePacket.GetPacket<GS_COMMUNITY_MESSAGE_BABELTOWER_START>();
												Congraturation_DLG congraturation_DLG4 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CONGRATURATIONDLG) as Congraturation_DLG;
												if (congraturation_DLG4 != null)
												{
													congraturation_DLG4.PushBabelStartMessage(packet7, packet.byReceibeUerType);
													congraturation_DLG4.Show();
												}
											}
											else if (packet.byMessageType == 23)
											{
												GS_COMMUNITY_MESSAGE_BABELTOWER_START packet8 = kDeserializePacket.GetPacket<GS_COMMUNITY_MESSAGE_BABELTOWER_START>();
												Congraturation_DLG congraturation_DLG5 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CONGRATURATIONDLG) as Congraturation_DLG;
												if (congraturation_DLG5 != null)
												{
													congraturation_DLG5.PushMythRaidStartMessage(packet8, packet.byReceibeUerType);
													congraturation_DLG5.Show();
												}
											}
											else if (packet.byMessageType == 14)
											{
												GS_COMMUNITY_MESSAGE_ITEM_SKILL_GET packet9 = kDeserializePacket.GetPacket<GS_COMMUNITY_MESSAGE_ITEM_SKILL_GET>();
												Congraturation_DLG congraturation_DLG6 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CONGRATURATIONDLG) as Congraturation_DLG;
												if (congraturation_DLG6 != null)
												{
													congraturation_DLG6.PushItemSkillGetMessage(packet9, packet.byReceibeUerType);
													congraturation_DLG6.Show();
												}
											}
											else if (packet.byMessageType == 15)
											{
												GS_COMMUNITY_MESSAGE_ELEMENTSOL_GET packet10 = kDeserializePacket.GetPacket<GS_COMMUNITY_MESSAGE_ELEMENTSOL_GET>();
												string textFromInterface3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1567");
												string empty3 = string.Empty;
												NrCharKindInfo charKindInfo2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(packet10.i32CharKind);
												NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
												{
													textFromInterface3,
													"targetname",
													charKindInfo2.GetName()
												});
												Congraturation_DLG congraturation_DLG7 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CONGRATURATIONDLG) as Congraturation_DLG;
												if (congraturation_DLG7 != null)
												{
													congraturation_DLG7.PushElementSolGetMessage(packet10.i32CharKind, packet.szCharName, packet10.nGrade, packet.byReceibeUerType);
													congraturation_DLG7.Show();
												}
											}
											else if (packet.byMessageType == 16)
											{
												GS_COMMUNITY_MESSAGE_GUILD packet11 = kDeserializePacket.GetPacket<GS_COMMUNITY_MESSAGE_GUILD>();
												Congraturation_DLG congraturation_DLG8 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CONGRATURATIONDLG) as Congraturation_DLG;
												if (congraturation_DLG8 != null)
												{
													congraturation_DLG8.PushGuildMessage(packet11, packet.byReceibeUerType);
													congraturation_DLG8.Show();
												}
												if (packet11.i32SubMessageType == 2 || packet11.i32SubMessageType == 1)
												{
													if (SoldierBatch.GUILDBOSS_INFO == null)
													{
														return;
													}
													if ((int)SoldierBatch.GUILDBOSS_INFO.m_i16Floor == packet11.i32Param1)
													{
														SoldierBatch.GUILDBOSS_INFO.m_i64CurPlayer = 0L;
													}
													if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP && NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLNUM_DLG) != null)
													{
														PlunderSolNumDlg plunderSolNumDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLNUM_DLG) as PlunderSolNumDlg;
														plunderSolNumDlg.GuildBossBattleUserName();
													}
												}
											}
											else if (packet.byMessageType == 17)
											{
												GS_COMMUNITY_MESSAGE_GENERAL_RECRUIT packet12 = kDeserializePacket.GetPacket<GS_COMMUNITY_MESSAGE_GENERAL_RECRUIT>();
												string textFromInterface4 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1567");
												string empty4 = string.Empty;
												NrCharKindInfo charKindInfo3 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(packet12.i32CharKind);
												NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty4, new object[]
												{
													textFromInterface4,
													"targetname",
													charKindInfo3.GetName()
												});
												Congraturation_DLG congraturation_DLG9 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CONGRATURATIONDLG) as Congraturation_DLG;
												if (congraturation_DLG9 != null)
												{
													packet.byReceibeUerType = 3;
													congraturation_DLG9.PushRECRUIT_LUCKYMessage(packet12.i32CharKind, packet.szCharName, packet12.nGrade, packet.byReceibeUerType, packet12.i32ItemUnique);
													congraturation_DLG9.Show();
												}
											}
											else if (packet.byMessageType == 20)
											{
												Congraturation_DLG congraturation_DLG10 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CONGRATURATIONDLG) as Congraturation_DLG;
												if (congraturation_DLG10 != null)
												{
													congraturation_DLG10.PushGuildWar(packet.szCharName, packet.i64Param, 2939, 278);
													congraturation_DLG10.Show();
												}
											}
											else if (packet.byMessageType == 21)
											{
												Congraturation_DLG congraturation_DLG11 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CONGRATURATIONDLG) as Congraturation_DLG;
												if (congraturation_DLG11 != null)
												{
													congraturation_DLG11.PushGuildWar(packet.szCharName, packet.i64Param, 2546, 279);
													congraturation_DLG11.Show();
												}
											}
											else if (packet.byMessageType == 22)
											{
												Congraturation_DLG congraturation_DLG12 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CONGRATURATIONDLG) as Congraturation_DLG;
												if (congraturation_DLG12 != null)
												{
													congraturation_DLG12.PushMine(packet.szCharName, packet.i64Param, packet.byReceibeUerType, (short)packet.i32Param, 1307, 2954);
													congraturation_DLG12.Show();
												}
											}
											else if (packet.byMessageType == 24)
											{
												Congraturation_DLG congraturation_DLG13 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CONGRATURATIONDLG) as Congraturation_DLG;
												if (congraturation_DLG13 != null)
												{
													congraturation_DLG13.PushGoldenEgg(packet.szCharName, packet.i32Param, (int)packet.i64Param);
													congraturation_DLG13.Show();
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		public static void GS_MINE_SEARCH_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MINE_SEARCH_ACK packet = kDeserializePacket.GetPacket<GS_MINE_SEARCH_ACK>();
			if (packet.i32Result == 0)
			{
				NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
				kMyCharInfo.m_Money = packet.i64TotalMoney;
				MINE_INFO info = new MINE_INFO();
				for (int i = 0; i < packet.i32MineInfoCount; i++)
				{
					info = kDeserializePacket.GetPacket<MINE_INFO>();
				}
				MINE_MILITARY_USER_SOLINFO[] array = new MINE_MILITARY_USER_SOLINFO[packet.i32SolListCount];
				for (int j = 0; j < packet.i32SolListCount; j++)
				{
					array[j] = kDeserializePacket.GetPacket<MINE_MILITARY_USER_SOLINFO>();
				}
				MineSearchDetailInfoDlg mineSearchDetailInfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MINE_SEARCH_DETAILINFO_DLG) as MineSearchDetailInfoDlg;
				if (mineSearchDetailInfoDlg != null)
				{
					mineSearchDetailInfoDlg.SetMineIninfo(info, packet.i64GuildID, TKString.NEWString(packet.szGuildName), array, (eMineSearchDetailInfo_Mode)packet.m_nMode);
				}
			}
			else
			{
				string text = string.Empty;
				int i32Result = packet.i32Result;
				switch (i32Result)
				{
				case 9000:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("428");
					goto IL_183;
				case 9001:
				case 9002:
					IL_EF:
					if (i32Result == 9560)
					{
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("730");
						goto IL_183;
					}
					if (i32Result == 9561)
					{
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("827");
						goto IL_183;
					}
					if (i32Result != -10)
					{
						goto IL_183;
					}
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89");
					goto IL_183;
				case 9003:
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("428");
					goto IL_183;
				}
				goto IL_EF;
				IL_183:
				if (text != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
		}

		public static void GS_MINE_MILITARY_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MINE_MILITARY_GET_ACK packet = kDeserializePacket.GetPacket<GS_MINE_MILITARY_GET_ACK>();
			MINE_MILITARY_USER_SOLINFO[] array = new MINE_MILITARY_USER_SOLINFO[packet.i32MilitaryCount];
			for (int i = 0; i < packet.i32MilitaryCount; i++)
			{
				array[i] = kDeserializePacket.GetPacket<MINE_MILITARY_USER_SOLINFO>();
			}
			SoldierBatch.MINE_INFO.m_i64MineID = packet.i64MineID;
			SoldierBatch.MINE_INFO.m_nMineGrade = packet.m_nMineGrade;
			SoldierBatch.MINE_INFO.SetMineMilitarySolList(array);
			SoldierBatch.SOLDIER_BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_MINE_MAKEUP;
			FacadeHandler.PushStage(Scene.Type.SOLDIER_BATCH);
		}

		public static void GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_ACK packet = kDeserializePacket.GetPacket<GS_MINE_GUILD_CURRENTSTATUS_INFO_GET_ACK>();
			MINE_GUILD_CURRENTSTATUS_INFO[] array = new MINE_GUILD_CURRENTSTATUS_INFO[packet.i32InfoCount];
			bool flag = true;
			if (packet.i32InfoCount <= 0)
			{
				MineSearchDlg mineSearchDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MINE_SEARCH_DLG) as MineSearchDlg;
				if (mineSearchDlg != null)
				{
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("401");
					Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				else
				{
					mineSearchDlg = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MINE_SEARCH_DLG) as MineSearchDlg);
					if (mineSearchDlg != null && packet.bType != 2)
					{
						mineSearchDlg.Show();
					}
				}
				if (packet.bType == 0)
				{
					flag = false;
				}
			}
			if (flag)
			{
				MineGuildCurrentStatusInfoDlg mineGuildCurrentStatusInfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MINE_GUILD_CURRENTSTATUSINFO_DLG) as MineGuildCurrentStatusInfoDlg;
				mineGuildCurrentStatusInfoDlg.Clear();
				for (int i = 0; i < packet.i32InfoCount; i++)
				{
					array[i] = kDeserializePacket.GetPacket<MINE_GUILD_CURRENTSTATUS_INFO>();
				}
				mineGuildCurrentStatusInfoDlg.SetRemainderCountGiveTime(packet.i64RemainderCounGiveTime);
				mineGuildCurrentStatusInfoDlg.AddInfo(array, packet.bType);
				mineGuildCurrentStatusInfoDlg.Show((packet.i32Page <= 0) ? 1 : packet.i32Page, (packet.i32TotalCount <= 0) ? 1 : packet.i32TotalCount);
				MineSearchDlg mineSearchDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MINE_SEARCH_DLG) as MineSearchDlg;
				if (mineSearchDlg2 != null)
				{
					mineSearchDlg2.Close();
				}
				MineMainSelectDlg mineMainSelectDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MINE_MAINSELECT_DLG) as MineMainSelectDlg;
				if (mineMainSelectDlg != null)
				{
					mineMainSelectDlg.Close();
				}
				MineGuildPushConfirmDlg mineGuildPushConfirmDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MINE_GUILDPUSH_CONFIRM_DLG) as MineGuildPushConfirmDlg;
				if (mineGuildPushConfirmDlg != null)
				{
					mineGuildPushConfirmDlg.Close();
					NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MINE_GUILDPUSH_CONFIRM_DLG);
				}
			}
			else
			{
				MineGuildCurrentStatusInfoDlg mineGuildCurrentStatusInfoDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MINE_GUILD_CURRENTSTATUSINFO_DLG) as MineGuildCurrentStatusInfoDlg;
				if (mineGuildCurrentStatusInfoDlg2 != null)
				{
					mineGuildCurrentStatusInfoDlg2.Clear();
					if (packet.bType == 0)
					{
						NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MINE_GUILD_CURRENTSTATUSINFO_DLG);
					}
				}
			}
		}

		public static void GS_MINE_MILITARY_BACKMOVE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MINE_MILITARY_BACKMOVE_ACK packet = kDeserializePacket.GetPacket<GS_MINE_MILITARY_BACKMOVE_ACK>();
			if (packet.m_nResult == 0)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("329"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			else if (packet.m_nResult == 9104)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("406"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			else if (packet.m_nResult == 9103)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("319"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}

		public static void GS_MINE_RURRYNOTIFY_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_MINE_RURRYNOTIFY_NFY packet = kDeserializePacket.GetPacket<GS_MINE_RURRYNOTIFY_NFY>();
			if (packet.byMode == 0)
			{
				bool flag = NrTSingleton<GameGuideManager>.Instance.CheckGameGuide(GameGuideType.MINE_ITEMGET);
				TsLog.Log("mine notify : state{0}, is check :{1} mine id  : {2} itemunique  : {3} itemnum : {4}", new object[]
				{
					packet.byMode,
					flag,
					packet.i64MineID,
					packet.i32ItemUnique,
					packet.i32ItemNum
				});
				GameGuideMineNotify gameGuideMineNotify = NrTSingleton<GameGuideManager>.Instance.GetGuide(GameGuideType.MINE_ITEMGET) as GameGuideMineNotify;
				if (gameGuideMineNotify != null)
				{
					gameGuideMineNotify.SetInfo(packet.byMode, packet.i64MineID, packet.i32ItemUnique, packet.i32ItemNum, packet.i32BonusItemNum);
				}
			}
			else if (packet.byMode == 2)
			{
				GameGuideMinePlunder gameGuideMinePlunder = NrTSingleton<GameGuideManager>.Instance.GetGuide(GameGuideType.MINE_PLUNDER) as GameGuideMinePlunder;
				if (gameGuideMinePlunder != null)
				{
					gameGuideMinePlunder.SetInfo(packet.i32ItemUnique);
				}
				NrTSingleton<GameGuideManager>.Instance.CheckGameGuide(GameGuideType.MINE_PLUNDER);
				MineGuildCurrentStatusInfoDlg mineGuildCurrentStatusInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MINE_GUILD_CURRENTSTATUSINFO_DLG) as MineGuildCurrentStatusInfoDlg;
				if (mineGuildCurrentStatusInfoDlg != null)
				{
					mineGuildCurrentStatusInfoDlg.RefreshList();
				}
			}
		}

		public static void GS_MINE_OCCUPIE_USER_ITEM_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MINE_OCCUPIE_USER_ITEM_GET_ACK packet = kDeserializePacket.GetPacket<GS_MINE_OCCUPIE_USER_ITEM_GET_ACK>();
			if (packet.i32Result == 0)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("375"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			else if (packet.i32Result == 9002)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("376"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			else if (packet.i32Result == 9001)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("491"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			else if (packet.i32Result == 9003)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("500"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}

		public static void GS_MINE_MILITRAY_CHANGE_POS_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MINE_MILITRAY_CHANGE_POS_ACK packet = kDeserializePacket.GetPacket<GS_MINE_MILITRAY_CHANGE_POS_ACK>();
			if (packet.i32Result == 0)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("112"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			else
			{
				MineSearchDetailInfoDlg mineSearchDetailInfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MINE_SEARCH_DETAILINFO_DLG) as MineSearchDetailInfoDlg;
				if (mineSearchDetailInfoDlg != null)
				{
					mineSearchDetailInfoDlg.RollBackBattlePos();
				}
				if (packet.i32Result == 9105)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("427"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				else if (packet.i32Result == 9106)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("426"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
		}

		public static void GS_MINE_WAIT_MILITARY_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MINE_WAIT_MILITARY_INFO_ACK packet = kDeserializePacket.GetPacket<GS_MINE_WAIT_MILITARY_INFO_ACK>();
			if (packet.i32Result == 0)
			{
				if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MINE_WAITMILTARYINFO_DLG))
				{
					MineWaitMiltaryInfoDlg mineWaitMiltaryInfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MINE_WAITMILTARYINFO_DLG) as MineWaitMiltaryInfoDlg;
					if (mineWaitMiltaryInfoDlg != null)
					{
						string guildname = string.Empty;
						for (int i = 0; i < 10; i++)
						{
							if (packet.clWaitGuildInfo[i].m_nGuildID > 0L)
							{
								guildname = TKString.NEWString(packet.clWaitGuildInfo[i].m_strGuildName);
								mineWaitMiltaryInfoDlg.SetWaitGuildInfo(guildname, packet.clWaitGuildInfo[i].m_nGuildID, packet.clWaitGuildInfo[i].isHiddenInfo);
							}
						}
						mineWaitMiltaryInfoDlg.GuildImageSetting();
					}
				}
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("493"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}

		public static void GS_MINE_TUTORIAL_DIRECTION_ITEM_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MINE_TUTORIAL_DIRECTION_ITEM_GET_ACK packet = kDeserializePacket.GetPacket<GS_MINE_TUTORIAL_DIRECTION_ITEM_GET_ACK>();
			if (packet.i32Result == 0)
			{
				int num = 0;
				MINE_CONSTANT_Manager instance = MINE_CONSTANT_Manager.GetInstance();
				if (instance != null)
				{
					num = instance.GetValue(eMINE_CONSTANT.eMINE_CONSTANT_TUTORIAL_ITEMNUM);
				}
				string text = string.Empty;
				string empty = string.Empty;
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("435");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					text,
					"count",
					num
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				MineTutorialStepDlg mineTutorialStepDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MINE_TUTORIAL_STEP_DLG) as MineTutorialStepDlg;
				if (mineTutorialStepDlg != null)
				{
					mineTutorialStepDlg.Close();
				}
			}
		}

		public static void GS_MINE_BATTLE_NOTIFY_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_MINE_BATTLE_NOTIFY_NFY packet = kDeserializePacket.GetPacket<GS_MINE_BATTLE_NOTIFY_NFY>();
			string empty = string.Empty;
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2129");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromInterface,
				"targetname1",
				TKString.NEWString(packet.szWinGuildName),
				"targetname2",
				TKString.NEWString(packet.szLoseGuildName)
			});
		}

		public static void GS_MINE_JOIN_COUNT_GET_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_MINE_JOIN_COUNT_GET_NFY packet = kDeserializePacket.GetPacket<GS_MINE_JOIN_COUNT_GET_NFY>();
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo == null)
			{
				return;
			}
			myCharInfo.SetCharDetail(8, (long)packet.i16JoinCount);
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("870"),
				"count",
				packet.i16DecreaseJoinCount
			});
			Main_UI_SystemMessage.ADDMessage(empty);
			MineGuildCurrentStatusInfoDlg mineGuildCurrentStatusInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MINE_GUILD_CURRENTSTATUSINFO_DLG) as MineGuildCurrentStatusInfoDlg;
			if (mineGuildCurrentStatusInfoDlg != null)
			{
				mineGuildCurrentStatusInfoDlg.SetRemainderCountGiveTime(packet.i64RemainderCountGiveTime);
				mineGuildCurrentStatusInfoDlg.SetJointCount();
			}
		}

		public static void GS_MINE_BATTLE_RESULT_REPORT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MINE_BATTLE_RESULT_REPORT_ACK packet = kDeserializePacket.GetPacket<GS_MINE_BATTLE_RESULT_REPORT_ACK>();
			if (packet.i32Result == 0)
			{
				Battle_ResultMineDlg battle_ResultMineDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_RESULT_MINE_DLG) as Battle_ResultMineDlg;
				if (battle_ResultMineDlg == null)
				{
					return;
				}
				battle_ResultMineDlg.ClearData();
				battle_ResultMineDlg.AddItemData(packet.i32ItemUnique, packet.i32IteNum, packet.i32BonusIteNum);
				battle_ResultMineDlg.SetMailID(packet.i64MailID);
				for (int i = 0; i < packet.i32MineResultCount; i++)
				{
					GS_BATTLE_RESULT_MINE packet2 = kDeserializePacket.GetPacket<GS_BATTLE_RESULT_MINE>();
					battle_ResultMineDlg.SetBasicData(packet2);
				}
				for (int j = 0; j < packet.i32SolCount; j++)
				{
					GS_BATTLE_RESULT_SOLDIER packet3 = kDeserializePacket.GetPacket<GS_BATTLE_RESULT_SOLDIER>();
					battle_ResultMineDlg.AddSolData(packet3);
				}
				for (int k = 0; k < packet.i32ContRankUserInfoCount; k++)
				{
					MINE_REPORT_CONTRANK_USER_INFO packet4 = kDeserializePacket.GetPacket<MINE_REPORT_CONTRANK_USER_INFO>();
					battle_ResultMineDlg.AddContributionRankInfo(packet4);
				}
				battle_ResultMineDlg.Show();
			}
		}

		public static void GS_MINE_BATTLE_RESULT_REWARD_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MINE_BATTLE_RESULT_REWARD_GET_ACK packet = kDeserializePacket.GetPacket<GS_MINE_BATTLE_RESULT_REWARD_GET_ACK>();
			if (packet.i32Result == -10)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("199"));
				return;
			}
			if (packet.i32Result == 31 || packet.i32Result == -20)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46"));
				return;
			}
			if (packet.i32Result == 76)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("773"));
				return;
			}
			if (packet.i32Result != 0)
			{
				return;
			}
			string empty = string.Empty;
			if (packet.i32ItemUnique > 0 && packet.i32ItemNum > 0)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("607"),
					"targetname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.i32ItemUnique),
					"count",
					packet.i32ItemNum
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			if (packet.i64TotalSolExp > 0L)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("849"),
					"exp",
					ANNUALIZED.Convert(packet.i64TotalSolExp)
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			PostDlg postDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG) as PostDlg;
			if (postDlg != null)
			{
				postDlg.RequestNextRecvList();
			}
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.POST_RECV_DLG);
			MineRecordDlg mineRecordDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MINE_RECORD_DLG) as MineRecordDlg;
			if (mineRecordDlg != null)
			{
				mineRecordDlg.Refresh_If_NonCompleteList();
			}
		}

		public static void GS_MINE_BATTLE_RESULT_REWARD_GET_ALL_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MINE_BATTLE_RESULT_REWARD_GET_ALL_ACK packet = kDeserializePacket.GetPacket<GS_MINE_BATTLE_RESULT_REWARD_GET_ALL_ACK>();
			if (packet.i32Result == -10)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("199"));
				return;
			}
			if (packet.i32Result == 31 || packet.i32Result == -20)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46"));
				return;
			}
			if (packet.i32Result == 76)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("773"));
				return;
			}
			if (packet.i32Result != 0)
			{
				return;
			}
			string empty = string.Empty;
			if (packet.i32ItemUnique > 0 && packet.i32ItemNum > 0)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("607"),
					"targetname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.i32ItemUnique),
					"count",
					packet.i32ItemNum
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			if (packet.i64TotalSolExp > 0L)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("849"),
					"exp",
					ANNUALIZED.Convert(packet.i64TotalSolExp)
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			if (!packet.isEnd)
			{
				return;
			}
			MineRecordDlg mineRecordDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MINE_RECORD_DLG) as MineRecordDlg;
			if (mineRecordDlg != null)
			{
				mineRecordDlg.Refresh_If_NonCompleteList();
			}
		}

		public static void GS_MINE_BATTLE_RESULT_LIST_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MINE_BATTLE_RESULT_LIST_ACK packet = kDeserializePacket.GetPacket<GS_MINE_BATTLE_RESULT_LIST_ACK>();
			MineRecordDlg mineRecordDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MINE_RECORD_DLG) as MineRecordDlg;
			if (mineRecordDlg != null)
			{
				mineRecordDlg.SetList(packet, kDeserializePacket);
			}
		}

		public static void GS_MINE_BATTLE_RESULT_GUILDWAR_LIST_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MINE_BATTLE_RESULT_GUILDWAR_LIST_ACK packet = kDeserializePacket.GetPacket<GS_MINE_BATTLE_RESULT_GUILDWAR_LIST_ACK>();
			MineRecordGuildWarDlg mineRecordGuildWarDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MINE_RECORD_GUILDWAR_DLG) as MineRecordGuildWarDlg;
			if (mineRecordGuildWarDlg != null)
			{
				mineRecordGuildWarDlg.SetList(packet, kDeserializePacket);
			}
		}

		public static void GS_MYTHRAID_GOLOBBY_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MYTHRAID_GOLOBBY_ACK packet = kDeserializePacket.GetPacket<GS_MYTHRAID_GOLOBBY_ACK>();
			if (packet.result == 0)
			{
				NrTSingleton<MythRaidManager>.Instance.SetSeason(packet.i8Season);
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WHISPER_DLG);
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_DLG);
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MYTHRAID_LOBBY_DLG);
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "ATTACK-FORMATION", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				if (!Scene.IsCurScene(Scene.Type.SOLDIER_BATCH))
				{
					SoldierBatch.SOLDIER_BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_MYTHRAID;
					FacadeHandler.PushStage(Scene.Type.SOLDIER_BATCH);
				}
				SoldierBatch.MYTHRAID_INFO.SetMythRaidInfo(packet.nBabelRoomIndex, (eMYTHRAID_DIFFICULTY)packet.enumDifficulty, packet.nLeaderPersonID, packet.nMinLevel, packet.nMaxLevel);
				for (int i = 0; i < 4; i++)
				{
					string charName = TKString.NEWString(packet.stMythRaidPersonInfo[i].szCharName);
					SoldierBatch.MYTHRAID_INFO.stMythRaidPersonInfo[i].SetInfo(packet.stMythRaidPersonInfo[i].nPartyPersonID, charName, packet.stMythRaidPersonInfo[i].nLevel, packet.stMythRaidPersonInfo[i].bReady, packet.stMythRaidPersonInfo[i].nSlotType, packet.stMythRaidPersonInfo[i].nCharKind, (int)packet.stMythRaidPersonInfo[i].i16GuardianAngel);
				}
				SoldierBatch.MYTHRAID_INFO.SetPartyCount();
				if (SoldierBatch.SOLDIERBATCH == null)
				{
					return;
				}
				if (SoldierBatch.MYTHRAID_INFO.GetPartyCount() > 1)
				{
					SoldierBatch.SOLDIERBATCH.SolBatchLock = true;
					NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
					if (charPersonInfo.GetPersonID() == packet.nLeaderPersonID && !SoldierBatch.MYTHRAID_INFO.IsPartyBatch())
					{
						SoldierBatch.SOLDIERBATCH.ResetSolPosition();
						SoldierBatch.SOLDIERBATCH.LoadBatchSolInfo_Party(null);
					}
				}
				NrPersonInfoUser charPersonInfo2 = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (charPersonInfo2.GetPersonID() == packet.nLeaderPersonID)
				{
					Battle.isLeader = true;
				}
				else
				{
					Battle.isLeader = false;
				}
				SoldierBatch.MYTHRAID_INFO.InitReadyState();
				if (SoldierBatch.MYTHRAID_INFO.GetPartyCount() > 1)
				{
					MythRaidLobbyUserListDlg mythRaidLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTHRAID_USERLIST_DLG) as MythRaidLobbyUserListDlg;
					if (mythRaidLobbyUserListDlg != null)
					{
						mythRaidLobbyUserListDlg.RefreshSolInfo();
						mythRaidLobbyUserListDlg.SetWaitingLock(true);
					}
					BabelTower_ChatDlg babelTower_ChatDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABELTOWER_CHAT) as BabelTower_ChatDlg;
					if (babelTower_ChatDlg != null)
					{
						babelTower_ChatDlg.RefreshChatInfo();
					}
					for (int j = 0; j < 4; j++)
					{
						if (packet.stMythRaidPersonInfo[j].nPartyPersonID == packet.nEnterPersonID)
						{
							string text = TKString.NEWString(packet.stMythRaidPersonInfo[j].szCharName);
							string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("187");
							string empty = string.Empty;
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
							{
								textFromNotify,
								"charname",
								text
							});
							Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
							Batch_Chat_DLG batch_Chat_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATCH_CAHT_DLG) as Batch_Chat_DLG;
							if (batch_Chat_DLG != null)
							{
								string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3281");
								string empty2 = string.Empty;
								NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
								{
									textFromInterface,
									"username",
									text
								});
								batch_Chat_DLG.PushMsg(string.Empty, empty2, "2002");
							}
							break;
						}
					}
				}
				StageWorld.BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_MYTHRAID;
				NrTSingleton<MythRaidManager>.Instance.SetRaidType((eMYTHRAID_DIFFICULTY)packet.enumDifficulty);
			}
			else
			{
				string text2 = string.Empty;
				int result = packet.result;
				if (result != 8000)
				{
					if (result != 8001)
					{
						if (result != 1)
						{
							if (result != 8)
							{
								if (result != 701)
								{
									if (result == 9700)
									{
										text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("905");
									}
								}
								else
								{
									text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("203");
								}
							}
							else
							{
								text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("417");
							}
						}
						else
						{
							text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("175");
						}
					}
					else
					{
						text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("160");
					}
				}
				else
				{
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("161");
				}
				if (text2 != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(text2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
		}

		public static void GS_MYTHRAID_TEST_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MYTHRAID_TEST_ACK packet = kDeserializePacket.GetPacket<GS_MYTHRAID_TEST_ACK>();
			Debug.LogError("_ACK : " + packet.Result);
		}

		public static void GS_MYTHRAID_BATTLEPOS_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MYTHRAID_BATTLEPOS_SET_ACK packet = kDeserializePacket.GetPacket<GS_MYTHRAID_BATTLEPOS_SET_ACK>();
			if (SoldierBatch.SOLDIERBATCH == null)
			{
				return;
			}
			SoldierBatch.SOLDIERBATCH.SetMythRaidSoldierBatch(packet);
			MythRaidLobbyUserListDlg mythRaidLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTHRAID_USERLIST_DLG) as MythRaidLobbyUserListDlg;
			if (mythRaidLobbyUserListDlg != null)
			{
				mythRaidLobbyUserListDlg.RefreshSolCount();
			}
		}

		public static void GS_MYTHRAID_LEAVE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MYTHRAID_LEAVE_ACK packet = kDeserializePacket.GetPacket<GS_MYTHRAID_LEAVE_ACK>();
			if (SoldierBatch.SOLDIERBATCH == null)
			{
				return;
			}
			if (!SoldierBatch.MYTHRAID_INFO.IsMythRaidLeader(packet.nLeavePersonID))
			{
				if (packet.mode == 2)
				{
					NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
					if (charPersonInfo.GetPersonID() == packet.nLeavePersonID)
					{
						SoldierBatch.MYTHRAID_INFO.Init();
						NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
						return;
					}
				}
				SoldierBatch.MYTHRAID_INFO.DeletePartyPerson(packet.nLeavePersonID);
				SoldierBatch.MYTHRAID_INFO.SetPartyCount();
				long[] array = new long[12];
				for (int i = 0; i < 12; i++)
				{
					array[i] = 0L;
				}
				GS_MYTHRAID_BATTLEPOS_SET_ACK[] array2 = new GS_MYTHRAID_BATTLEPOS_SET_ACK[(int)packet.nSolCount];
				for (int j = 0; j < (int)packet.nSolCount; j++)
				{
					array2[j] = kDeserializePacket.GetPacket<GS_MYTHRAID_BATTLEPOS_SET_ACK>();
				}
				int num = 0;
				for (int k = 0; k < SoldierBatch.SOLDIERBATCH.GetBabelTowerTotalBatchInfoCount(); k++)
				{
					bool flag = true;
					long babelTowerSolIDFromIndex = SoldierBatch.SOLDIERBATCH.GetBabelTowerSolIDFromIndex(k);
					if (babelTowerSolIDFromIndex > 0L)
					{
						for (int l = 0; l < (int)packet.nSolCount; l++)
						{
							if (babelTowerSolIDFromIndex == array2[l].nSolID)
							{
								flag = false;
							}
						}
						if (flag)
						{
							array[num] = babelTowerSolIDFromIndex;
							num++;
						}
					}
				}
				for (int m = 0; m < 12; m++)
				{
					if (array[m] > 0L)
					{
						SoldierBatch.SOLDIERBATCH.DeleteBabelBatchInfoFromSolID(array[m]);
					}
				}
				SoldierBatch.MYTHRAID_INFO.InitReadyState();
				MythRaidLobbyUserListDlg mythRaidLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTHRAID_USERLIST_DLG) as MythRaidLobbyUserListDlg;
				if (mythRaidLobbyUserListDlg != null)
				{
					mythRaidLobbyUserListDlg.RefreshSolInfo();
					mythRaidLobbyUserListDlg.SetWaitingLock(false);
				}
				BabelTower_ChatDlg babelTower_ChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWER_CHAT) as BabelTower_ChatDlg;
				if (babelTower_ChatDlg != null)
				{
					babelTower_ChatDlg.RefreshChatInfo();
				}
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
				{
					PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
					if (plunderSolListDlg != null)
					{
						plunderSolListDlg.m_bMyBatchMode = true;
						plunderSolListDlg.SetSolNum((int)packet.nSolCount, false);
					}
				}
				Batch_Chat_DLG batch_Chat_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATCH_CAHT_DLG) as Batch_Chat_DLG;
				string text = TKString.NEWString(packet.szCharName);
				if (batch_Chat_DLG != null)
				{
					string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3282");
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						textFromInterface,
						"username",
						text
					});
					batch_Chat_DLG.PushMsg(string.Empty, empty, "1106");
				}
			}
			else
			{
				SoldierBatch.MYTHRAID_INFO.Init();
				NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
			}
		}

		public static void GS_MYTHRAID_CHARINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MYTHRAID_CHARINFO_ACK packet = kDeserializePacket.GetPacket<GS_MYTHRAID_CHARINFO_ACK>();
			NrTSingleton<MythRaidManager>.Instance.SetMyInfo(packet);
		}

		public static void GS_MYTHRAID_BATTLEPOS_REFLASH_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MYTHRAID_BATTLEPOS_REFLASH_ACK packet = kDeserializePacket.GetPacket<GS_MYTHRAID_BATTLEPOS_REFLASH_ACK>();
			if (SoldierBatch.SOLDIERBATCH == null)
			{
				return;
			}
			long[] array = new long[12];
			for (int i = 0; i < 12; i++)
			{
				array[i] = 0L;
			}
			GS_MYTHRAID_BATTLEPOS_SET_ACK[] array2 = new GS_MYTHRAID_BATTLEPOS_SET_ACK[packet.nSolCount];
			for (int j = 0; j < packet.nSolCount; j++)
			{
				array2[j] = kDeserializePacket.GetPacket<GS_MYTHRAID_BATTLEPOS_SET_ACK>();
			}
			int num = 0;
			bool flag = false;
			for (int k = 0; k < SoldierBatch.SOLDIERBATCH.GetBabelTowerTotalBatchInfoCount(); k++)
			{
				bool flag2 = true;
				long babelTowerSolIDFromIndex = SoldierBatch.SOLDIERBATCH.GetBabelTowerSolIDFromIndex(k);
				if (babelTowerSolIDFromIndex > 0L)
				{
					flag = true;
					for (int l = 0; l < packet.nSolCount; l++)
					{
						if (babelTowerSolIDFromIndex == array2[l].nSolID)
						{
							flag2 = false;
						}
					}
					if (flag2)
					{
						array[num] = babelTowerSolIDFromIndex;
						num++;
					}
				}
			}
			for (int m = 0; m < 12; m++)
			{
				if (array[m] > 0L)
				{
					SoldierBatch.SOLDIERBATCH.DeleteBabelBatchInfoFromSolID(array[m]);
				}
			}
			if (!flag)
			{
				for (int n = 0; n < packet.nSolCount; n++)
				{
					SoldierBatch.SOLDIERBATCH.SetMythRaidSoldierBatch(array2[n]);
				}
			}
			if (!SoldierBatch.BABELTOWER_INFO.IsPartyBatch())
			{
				SoldierBatch.SOLDIERBATCH.LoadBatchSolInfo_Party_MythRaid(array2);
			}
			MythRaidLobbyUserListDlg mythRaidLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTHRAID_USERLIST_DLG) as MythRaidLobbyUserListDlg;
			if (mythRaidLobbyUserListDlg != null)
			{
				mythRaidLobbyUserListDlg.RefreshSolCount();
				mythRaidLobbyUserListDlg.SetWaitingLock(false);
			}
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.PLUNDERSOLLIST_DLG))
			{
				PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
				if (plunderSolListDlg != null)
				{
					plunderSolListDlg.m_bMyBatchMode = true;
					NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
					if (charPersonInfo == null)
					{
						return;
					}
					int babelTowerSolCount = SoldierBatch.SOLDIERBATCH.GetBabelTowerSolCount(charPersonInfo.GetPersonID());
					plunderSolListDlg.SetSolNum(babelTowerSolCount, false);
				}
			}
			SoldierBatch.SOLDIERBATCH.SolBatchLock = false;
		}

		public static void GS_MYTHRAID_CHANGE_SLOTTYPE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MYTHRAID_CHANGE_SLOTTYPE_ACK packet = kDeserializePacket.GetPacket<GS_MYTHRAID_CHANGE_SLOTTYPE_ACK>();
			if (SoldierBatch.SOLDIERBATCH == null)
			{
				return;
			}
			if (packet.pos < 0)
			{
				return;
			}
			SoldierBatch.MYTHRAID_INFO.SetSlotType(packet.pos, packet.change_type);
			MythRaidLobbyUserListDlg mythRaidLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTHRAID_USERLIST_DLG) as MythRaidLobbyUserListDlg;
			if (mythRaidLobbyUserListDlg != null)
			{
				mythRaidLobbyUserListDlg.RefreshSolInfo();
				mythRaidLobbyUserListDlg.SetWaitingLock(false);
			}
		}

		public static void GS_MYTHRAID_READY_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MYTHRAID_READY_ACK packet = kDeserializePacket.GetPacket<GS_MYTHRAID_READY_ACK>();
			if (SoldierBatch.SOLDIERBATCH == null)
			{
				return;
			}
			SoldierBatch.MYTHRAID_INFO.SetReadyBattle(packet.nPersonID, packet.bReady);
			MythRaidLobbyUserListDlg mythRaidLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTHRAID_USERLIST_DLG) as MythRaidLobbyUserListDlg;
			if (mythRaidLobbyUserListDlg != null)
			{
				mythRaidLobbyUserListDlg.RefreshSolInfo();
				mythRaidLobbyUserListDlg.SetWaitingLock(false);
			}
		}

		public static void GS_MYTHRAID_RANKINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MYTHRAID_RANKINFO_ACK packet = kDeserializePacket.GetPacket<GS_MYTHRAID_RANKINFO_ACK>();
			MythRaid_Lobby_DLG mythRaid_Lobby_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTHRAID_LOBBY_DLG) as MythRaid_Lobby_DLG;
			if (mythRaid_Lobby_DLG == null)
			{
				return;
			}
			MYTHRAID_RANK_INFO[] array = new MYTHRAID_RANK_INFO[(int)packet.i16DataCount];
			for (int i = 0; i < (int)packet.i16DataCount; i++)
			{
				MYTHRAID_RANK_INFO packet2 = kDeserializePacket.GetPacket<MYTHRAID_RANK_INFO>();
				array[i] = packet2;
			}
			if (packet.ui8Type == 0)
			{
				mythRaid_Lobby_DLG.SetSoloRank(packet.i16PageIndex, array);
			}
			else if (packet.ui8Type == 1)
			{
				mythRaid_Lobby_DLG.SetPartyRank(packet.i16PageIndex, array);
			}
			mythRaid_Lobby_DLG.ShowRank();
		}

		public static void GS_MYTHRAID_GETREWARD_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MYTHRAID_GETREWARD_ACK packet = kDeserializePacket.GetPacket<GS_MYTHRAID_GETREWARD_ACK>();
			for (int i = 0; i < 7; i++)
			{
				if (packet.i32itempos[i] < 0)
				{
					NrTSingleton<MythRaidManager>.Instance.IsRewardMail = true;
				}
			}
			int i32Result = packet.i32Result;
			if (i32Result != 9700)
			{
				if (i32Result != 9701)
				{
					if (i32Result == 0)
					{
						if (packet.i8AskType == 0)
						{
							NrTSingleton<MythRaidManager>.Instance.MyRewardRank = packet.i32RewardInfo;
							NrTSingleton<MythRaidManager>.Instance.CanGetReward = true;
						}
						else if (packet.i8AskType == 1)
						{
							NrTSingleton<MythRaidManager>.Instance.GetReward(packet.i32RewardUnique, packet.i32RewardNum, (eMYTHRAID_DIFFICULTY)packet.i8RaidType);
						}
					}
				}
				else
				{
					NrTSingleton<MythRaidManager>.Instance.MyRewardRank = packet.i32RewardInfo;
				}
			}
			else if (packet.i8AskType == 1)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("905");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}

		public static void GS_MYTHRAID_PARTYSEARCH_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MYTHRAID_PARTYSEARCH_ACK packet = kDeserializePacket.GetPacket<GS_MYTHRAID_PARTYSEARCH_ACK>();
			byte i8MatchType = packet.i8MatchType;
			if (i8MatchType != 0)
			{
				if (i8MatchType == 1)
				{
					NrTSingleton<MythRaidManager>.Instance.IsPartySearch = false;
				}
			}
			else if (packet.i8Result == 0)
			{
				NrTSingleton<MythRaidManager>.Instance.IsPartySearch = true;
			}
			else
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("905");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}

		public static void GS_BATTLE_ANGEL_EFFECT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_BATTLE_ANGEL_EFFECT_ACK packet = kDeserializePacket.GetPacket<GS_BATTLE_ANGEL_EFFECT_ACK>();
			Battle_Control_Dlg battle_Control_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
			if (battle_Control_Dlg != null)
			{
				battle_Control_Dlg.GuardianAngelEffectStart(packet.skillAngelUnique);
				battle_Control_Dlg.Angelskill_Invoke_PersonID = packet.effectStartpersonID;
			}
		}

		public static void GS_MYTHRAID_GUARDIANSELECT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MYTHRAID_GUARDIANSELECT_ACK packet = kDeserializePacket.GetPacket<GS_MYTHRAID_GUARDIANSELECT_ACK>();
			if (packet.i16SelectedGuardianUnique == -2)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("906"));
				return;
			}
			MythRaidLobbyUserListDlg mythRaidLobbyUserListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTHRAID_USERLIST_DLG) as MythRaidLobbyUserListDlg;
			if (mythRaidLobbyUserListDlg != null)
			{
				mythRaidLobbyUserListDlg.SetGuardianInfo(packet.i64PersonID, (int)packet.i16SelectedGuardianUnique);
			}
		}

		public static void GS_MYTHRAID_INVITE_FRIEND_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MYTHRAID_INVITE_FRIEND_ACK packet = kDeserializePacket.GetPacket<GS_MYTHRAID_INVITE_FRIEND_ACK>();
			if (Launcher.Instance.LocalPatchLevel != Launcher.Instance.PatchLevelMax)
			{
				NrReceiveGame.OnMythRaidInviteCancelPatchLevel(packet);
				return;
			}
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (packet.Result == 0)
			{
				if (kMyCharInfo.m_PersonID == packet.ReqPersonID)
				{
					USER_FRIEND_INFO friend = kMyCharInfo.m_kFriendInfo.GetFriend(packet.InvitePersonID);
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("302");
					string empty = string.Empty;
					if (friend != null)
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							textFromNotify,
							"charname",
							TKString.NEWString(friend.szName)
						});
					}
					else
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							textFromNotify,
							"charname",
							TKString.NEWString(packet.InvitePersonName)
						});
					}
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
				else if (kMyCharInfo.m_PersonID == packet.InvitePersonID)
				{
					USER_FRIEND_INFO friend2 = kMyCharInfo.m_kFriendInfo.GetFriend(packet.ReqPersonID);
					string text = string.Empty;
					string empty2 = string.Empty;
					string title = string.Empty;
					title = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("96");
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("97");
					switch (packet.raidType)
					{
					case 0:
						title = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("431");
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("434");
						break;
					case 1:
						title = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("432");
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("435");
						break;
					case 2:
						title = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("433");
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("436");
						break;
					}
					if (friend2 != null)
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
						{
							text,
							"charname",
							TKString.NEWString(friend2.szName)
						});
					}
					else
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
						{
							text,
							"charname",
							TKString.NEWString(packet.ReqPersonName)
						});
					}
					MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
					msgBoxUI.SetMsg(new YesDelegate(NrReceiveGame.OnMythRaidInviteAccept), packet, new NoDelegate(NrReceiveGame.OnMythRaidInviteCancel), packet, title, empty2, eMsgType.MB_OK_CANCEL);
					msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("317"));
					msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("318"));
				}
			}
			else
			{
				string text2 = string.Empty;
				string text3 = string.Empty;
				int result = packet.Result;
				if (result != 1)
				{
					if (result != 29)
					{
						if (result == 501)
						{
							text3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("175");
						}
					}
					else
					{
						text3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("215");
					}
				}
				else
				{
					USER_FRIEND_INFO friend3 = kMyCharInfo.m_kFriendInfo.GetFriend(packet.InvitePersonID);
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("764");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text3, new object[]
					{
						text2,
						"targetname",
						TKString.NEWString(friend3.szName)
					});
				}
				if (text3 != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(text3, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
		}

		public static void OnMythRaidInviteCancelPatchLevel(object EventObject)
		{
			GS_MYTHRAID_INVITE_FRIEND_ACK gS_MYTHRAID_INVITE_FRIEND_ACK = (GS_MYTHRAID_INVITE_FRIEND_ACK)EventObject;
			GS_MYTHRAID_INVITE_FRIEND_AGREE_REQ gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ = new GS_MYTHRAID_INVITE_FRIEND_AGREE_REQ();
			gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ.nInvite = 2;
			gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ.bAccept = false;
			gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ.bMoveWorld = false;
			gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ.WorldID = 0;
			gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ.ChannelID = 0;
			gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ.PersonID = gS_MYTHRAID_INVITE_FRIEND_ACK.ReqPersonID;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MYTHRAID_INVITE_FRIEND_AGREE_REQ, gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ);
		}

		public static void OnMythRaidInviteAccept(object EventObject)
		{
			GS_MYTHRAID_INVITE_FRIEND_ACK gS_MYTHRAID_INVITE_FRIEND_ACK = (GS_MYTHRAID_INVITE_FRIEND_ACK)EventObject;
			if (gS_MYTHRAID_INVITE_FRIEND_ACK == null)
			{
				return;
			}
			if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
			{
				return;
			}
			bool bMoveWorld = false;
			if (Client.m_MyWS != (long)gS_MYTHRAID_INVITE_FRIEND_ACK.nReqPersonWorldID)
			{
				bMoveWorld = true;
			}
			GS_MYTHRAID_INVITE_FRIEND_AGREE_REQ gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ = new GS_MYTHRAID_INVITE_FRIEND_AGREE_REQ();
			gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ.nInvite = 0;
			gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ.bAccept = true;
			gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ.bMoveWorld = bMoveWorld;
			gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ.WorldID = gS_MYTHRAID_INVITE_FRIEND_ACK.nReqPersonWorldID;
			gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ.ChannelID = gS_MYTHRAID_INVITE_FRIEND_ACK.ReqPersonCHID;
			gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ.PersonID = gS_MYTHRAID_INVITE_FRIEND_ACK.ReqPersonID;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MYTHRAID_INVITE_FRIEND_AGREE_REQ, gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ);
		}

		public static void OnMythRaidInviteCancel(object EventObject)
		{
			GS_MYTHRAID_INVITE_FRIEND_ACK gS_MYTHRAID_INVITE_FRIEND_ACK = (GS_MYTHRAID_INVITE_FRIEND_ACK)EventObject;
			GS_MYTHRAID_INVITE_FRIEND_AGREE_REQ gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ = new GS_MYTHRAID_INVITE_FRIEND_AGREE_REQ();
			gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ.nInvite = 1;
			gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ.bAccept = false;
			gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ.bMoveWorld = false;
			gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ.WorldID = 0;
			gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ.ChannelID = 0;
			gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ.PersonID = gS_MYTHRAID_INVITE_FRIEND_ACK.ReqPersonID;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MYTHRAID_INVITE_FRIEND_AGREE_REQ, gS_MYTHRAID_INVITE_FRIEND_AGREE_REQ);
		}

		public static void NewExplorationErrMessage(int i32Result, int ItmeUnique = 0)
		{
			switch (i32Result)
			{
			case 9770:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 9771:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("888"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 9772:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("893"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 9773:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("891"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 9774:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("899"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 9775:
				break;
			default:
				if (i32Result != 6)
				{
					if (i32Result != 31)
					{
						if (i32Result != 40)
						{
							if (i32Result != 76)
							{
								Main_UI_SystemMessage.ADDMessage(string.Format("Fail: {0}", i32Result), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
							}
							else
							{
								Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("773"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
							}
						}
						else
						{
							Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("915"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
						}
					}
					else
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					}
				}
				else
				{
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("894"),
						"targetname",
						NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(ItmeUnique)
					});
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				break;
			}
		}

		public static void GS_NEWEXPLORATION_END_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWEXPLORATION_END_ACK packet = kDeserializePacket.GetPacket<GS_NEWEXPLORATION_END_ACK>();
			if (packet.i32Result != 0)
			{
				NrReceiveGame.NewExplorationErrMessage(packet.i32Result, packet.i32ItemUnique);
				return;
			}
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo != null)
			{
				kMyCharInfo.SetCharSubData(packet.i32SubDataType, packet.i64SubDataValue);
			}
			string empty = string.Empty;
			if (packet.bIsMailBox)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("855"),
					"targetname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.i32ItemUnique),
					"count",
					packet.i32ItemNum
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("607"),
					"targetname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.i32ItemUnique),
					"count",
					packet.i32ItemNum
				});
			}
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			NewExplorationMainDlg newExplorationMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWEXPLORATION_MAIN_DLG) as NewExplorationMainDlg;
			if (newExplorationMainDlg != null)
			{
				newExplorationMainDlg.SetInfo();
			}
			BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
			if (bookmarkDlg != null)
			{
				bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.BATTLE);
			}
			BattleCollect_DLG battleCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLECOLLECT_DLG) as BattleCollect_DLG;
			if (battleCollect_DLG != null)
			{
				battleCollect_DLG.Update_Notice();
			}
		}

		public static void GS_NEWEXPLORATION_STAGE_CHALLENGE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWEXPLORATION_STAGE_CHALLENGE_ACK packet = kDeserializePacket.GetPacket<GS_NEWEXPLORATION_STAGE_CHALLENGE_ACK>();
			if (packet.i32Result != 0)
			{
				NrReceiveGame.NewExplorationErrMessage(packet.i32Result, 0);
				return;
			}
			if (!Scene.IsCurScene(Scene.Type.SOLDIER_BATCH))
			{
				SoldierBatch.SOLDIER_BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_NEWEXPLORATION;
				FacadeHandler.PushStage(Scene.Type.SOLDIER_BATCH);
			}
		}

		public static void GS_NEWEXPLORATION_START_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWEXPLORATION_START_ACK packet = kDeserializePacket.GetPacket<GS_NEWEXPLORATION_START_ACK>();
			if (packet.i32Result != 0)
			{
				NrReceiveGame.NewExplorationErrMessage(packet.i32Result, 0);
				NewExploration_RepeatBgDlg newExploration_RepeatBgDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWEXPLORATION_REPEAT_DLG) as NewExploration_RepeatBgDlg;
				if (newExploration_RepeatBgDlg != null)
				{
					newExploration_RepeatBgDlg.Close();
				}
				return;
			}
		}

		public static void GS_NEWEXPLORATION_TREASURE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWEXPLORATION_TREASURE_ACK packet = kDeserializePacket.GetPacket<GS_NEWEXPLORATION_TREASURE_ACK>();
			if (packet.i32Result != 0)
			{
				NrReceiveGame.NewExplorationErrMessage(packet.i32Result, 0);
				BonusItemInfoDlg bonusItemInfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BONUS_ITEM_INFO_DLG) as BonusItemInfoDlg;
				if (bonusItemInfoDlg != null)
				{
					bonusItemInfoDlg.SetOkButtonEnable(true);
				}
				return;
			}
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo != null)
			{
				kMyCharInfo.SetCharWeekData(packet.i32WeekDataType, packet.i64WeekDataValue);
			}
			NewExplorationMainDlg newExplorationMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWEXPLORATION_MAIN_DLG) as NewExplorationMainDlg;
			if (newExplorationMainDlg != null)
			{
				newExplorationMainDlg.SetFloorList();
			}
			BonusItemInfoDlg bonusItemInfoDlg2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BONUS_ITEM_INFO_DLG) as BonusItemInfoDlg;
			if (bonusItemInfoDlg2 != null)
			{
				bonusItemInfoDlg2.ActiveRewardEffect(new ResultDelegate(NrReceiveGame.BonusItemDelegate), packet);
			}
		}

		public static void BonusItemDelegate(object obj)
		{
			GS_NEWEXPLORATION_TREASURE_ACK gS_NEWEXPLORATION_TREASURE_ACK = obj as GS_NEWEXPLORATION_TREASURE_ACK;
			if (gS_NEWEXPLORATION_TREASURE_ACK == null)
			{
				return;
			}
			string empty = string.Empty;
			for (int i = 0; i < 3; i++)
			{
				if (gS_NEWEXPLORATION_TREASURE_ACK.i32ItemUnique[i] > 0 && gS_NEWEXPLORATION_TREASURE_ACK.i32ItemNum[i] > 0)
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("607"),
						"targetname",
						NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(gS_NEWEXPLORATION_TREASURE_ACK.i32ItemUnique[i]),
						"count",
						gS_NEWEXPLORATION_TREASURE_ACK.i32ItemNum[i]
					});
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
			}
			BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
			if (bookmarkDlg != null)
			{
				bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.BATTLE);
			}
			BattleCollect_DLG battleCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLECOLLECT_DLG) as BattleCollect_DLG;
			if (battleCollect_DLG != null)
			{
				battleCollect_DLG.Update_Notice();
			}
		}

		public static void GS_NEWEXPLORATION_RESET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWEXPLORATION_RESET_ACK packet = kDeserializePacket.GetPacket<GS_NEWEXPLORATION_RESET_ACK>();
			if (packet.i32Result != 0)
			{
				NrReceiveGame.NewExplorationErrMessage(packet.i32Result, 70002);
				return;
			}
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo == null)
			{
				return;
			}
			kMyCharInfo.SetCharDetail(packet.i32DetailType, packet.i64DetailValue);
			sbyte resetCount = NrTSingleton<NewExplorationManager>.Instance.GetResetCount();
			sbyte maxResetCount = NrTSingleton<NewExplorationManager>.Instance.GetMaxResetCount();
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("896"),
				"count",
				(int)maxResetCount - (int)resetCount
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			NewExplorationMainDlg newExplorationMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWEXPLORATION_MAIN_DLG) as NewExplorationMainDlg;
			if (newExplorationMainDlg != null)
			{
				newExplorationMainDlg.SetInfo();
				newExplorationMainDlg.SetFloorList();
			}
		}

		public static void GS_NEWEXPLORATION_RANK_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWEXPLORATION_RANK_GET_ACK packet = kDeserializePacket.GetPacket<GS_NEWEXPLORATION_RANK_GET_ACK>();
			NewExplorationMainDlg newExplorationMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWEXPLORATION_MAIN_DLG) as NewExplorationMainDlg;
			if (newExplorationMainDlg != null)
			{
				newExplorationMainDlg.SetRankInfo(packet.i32Rank, packet.i32TotalUserCount);
			}
		}

		public static void GS_NEWEXPLORATION_LASTWEEK_RANK_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWEXPLORATION_LASTWEEK_RANK_GET_ACK packet = kDeserializePacket.GetPacket<GS_NEWEXPLORATION_LASTWEEK_RANK_GET_ACK>();
		}

		public static void GS_NEWGUILD_CREATE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_CREATE_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_CREATE_ACK>();
			string text = TKString.NEWString(packet.strGuildName);
			int i32Result = packet.i32Result;
			if (i32Result != 0)
			{
				if (i32Result != 1)
				{
					if (i32Result != -50)
					{
						if (i32Result != -30)
						{
							if (i32Result != -20)
							{
								if (i32Result == -5)
								{
									Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("377"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
								}
							}
							else
							{
								Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("91"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
							}
						}
						else
						{
							Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("90"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
						}
					}
					else
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
					}
				}
			}
			else
			{
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "GUILD", "ACCEPT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64AfterCharMoney;
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("518");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromNotify, new object[]
				{
					textFromNotify,
					"targetname",
					text
				});
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				NrCharUser nrCharUser = (NrCharUser)NrTSingleton<NkCharManager>.Instance.GetChar(1);
				nrCharUser.SetUserGuildName(text, 0L, false);
				NrTSingleton<NewGuildManager>.Instance.AddJoin();
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.NEWGUILD_CREATE_DLG);
				NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
				DateTime dateTime = DateTime.Now.ToLocalTime();
				DateTime arg_136_0 = dateTime;
				DateTime dateTime2 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
				int num = (int)(arg_136_0 - dateTime2.ToLocalTime()).TotalSeconds;
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("ts", num.ToString());
				dictionary.Add("step", "create");
				dictionary.Add("level", myCharInfo.GetLevel().ToString());
				dictionary.Add("account_id", myCharInfo.m_SN.ToString());
				GameObject pkGoOminiata = StageSystemCheck.pkGoOminiata;
				if (pkGoOminiata)
				{
					OmniataComponent component = pkGoOminiata.GetComponent<OmniataComponent>();
					if (component)
					{
						component.Track("om_guild", dictionary);
					}
				}
			}
		}

		public static void GS_NEWGUILD_DELETE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_DELETE_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_DELETE_ACK>();
			switch (packet.i32Result)
			{
			case 0:
			{
				NrTSingleton<NewGuildManager>.Instance.Clear();
				NrTSingleton<NewGuildManager>.Instance.ClearDlg();
				NrTSingleton<NewGuildManager>.Instance.Leave();
				NrTSingleton<NewGuildManager>.Instance.ClearAgit();
				NoticeIconDlg.SetIcon(ICON_TYPE.MINE_RECORED, false);
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("101"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				NrCharUser nrCharUser = (NrCharUser)NrTSingleton<NkCharManager>.Instance.GetChar(1);
				nrCharUser.SetUserGuildName(string.Empty, 0L, false);
				break;
			}
			case 1:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("30"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 2:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("701"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			}
		}

		public static void GS_NEWGUILD_UPDATE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_UPDATE_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_UPDATE_ACK>();
			if (packet.i32Result == 0)
			{
				string strGuildMessage = TKString.NEWString(packet.strGuildMessage);
				NrTSingleton<NewGuildManager>.Instance.ChangeGuildMessage(strGuildMessage);
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NEWGUILD_ADMINMENU_DLG))
				{
					NewGuildAdminMenuDlg newGuildAdminMenuDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWGUILD_ADMINMENU_DLG) as NewGuildAdminMenuDlg;
					if (newGuildAdminMenuDlg != null)
					{
						newGuildAdminMenuDlg.RefreshGuildMessage(strGuildMessage);
					}
				}
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("92"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
		}

		public static void GS_NEWGUILD_MEMBER_APPLY_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_MEMBER_APPLY_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_MEMBER_APPLY_ACK>();
			string text = TKString.NEWString(packet.strGuildName);
			int i32Result = packet.i32Result;
			if (i32Result != -10)
			{
				if (i32Result != 0)
				{
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("96");
					Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
				else
				{
					NrTSingleton<NewGuildManager>.Instance.AddApplicantInfo(packet.ApplicantInfo);
					string empty = string.Empty;
					string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("95");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						textFromNotify2,
						"targetname",
						text
					});
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "GUILD", "CANCEL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				}
			}
			else
			{
				string empty2 = string.Empty;
				string textFromNotify3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("97");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					textFromNotify3,
					"Guildname",
					text
				});
				Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
		}

		public static void GS_NEWGUILD_MEMBER_APPLY_CANCEL_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_MEMBER_APPLY_CANCEL_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_MEMBER_APPLY_CANCEL_ACK>();
			TsLog.LogOnlyEditor("GS_NEWGUILD_MEMBER_APPLY_CANCEL_ACK : " + packet.i32Result);
		}

		public static void GS_NEWGUILD_MEMBER_JOIN_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_MEMBER_JOIN_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_MEMBER_JOIN_ACK>();
			if (packet.i32Result == 0)
			{
				NrTSingleton<NewGuildManager>.Instance.RemoveApplicant(packet.i64PersonID_NewMember);
				string text = string.Empty;
				if (packet.bApprove)
				{
					NrTSingleton<NewGuildManager>.Instance.AddMemberInfo(packet.NewGuildMemberInfo);
					if (packet.i64PersonID == NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID)
					{
						TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "GUILD", "ACCEPT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
					}
					if (packet.i64PersonID_NewMember != NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID)
					{
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("521");
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
						{
							text,
							"targetname",
							TKString.NEWString(packet.strName_NewMember)
						});
					}
					else
					{
						text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("520");
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
						{
							text,
							"targetname",
							TKString.NEWString(packet.strGuildName)
						});
						NrTSingleton<NewGuildManager>.Instance.AddJoin();
						NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
						DateTime dateTime = DateTime.Now.ToLocalTime();
						DateTime arg_15C_0 = dateTime;
						DateTime dateTime2 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
						int num = (int)(arg_15C_0 - dateTime2.ToLocalTime()).TotalSeconds;
						Dictionary<string, string> dictionary = new Dictionary<string, string>();
						dictionary.Add("ts", num.ToString());
						dictionary.Add("step", "join");
						dictionary.Add("level", myCharInfo.GetLevel().ToString());
						dictionary.Add("account_id", myCharInfo.m_SN.ToString());
						GameObject pkGoOminiata = StageSystemCheck.pkGoOminiata;
						if (pkGoOminiata)
						{
							OmniataComponent component = pkGoOminiata.GetComponent<OmniataComponent>();
							if (component)
							{
								component.Track("om_guild", dictionary);
							}
						}
					}
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
				else
				{
					if (packet.i64PersonID == NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID)
					{
						TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "GUILD", "CANCEL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
					}
					if (packet.i64PersonID_NewMember == NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID)
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("522"),
							"targetname",
							TKString.NEWString(packet.strGuildName)
						});
						Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					}
				}
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NEWGUILD_MEMBER_DLG))
				{
					NewGuildMemberDlg newGuildMemberDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWGUILD_MEMBER_DLG) as NewGuildMemberDlg;
					if (newGuildMemberDlg != null)
					{
						newGuildMemberDlg.RefreshInfo();
					}
				}
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NEWGUILD_ADMINMENU_DLG))
				{
					NewGuildAdminMenuDlg newGuildAdminMenuDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWGUILD_ADMINMENU_DLG) as NewGuildAdminMenuDlg;
					if (newGuildAdminMenuDlg != null)
					{
						newGuildAdminMenuDlg.RefreshInfo();
					}
				}
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NEWGUILD_MAIN_DLG))
				{
					NewGuildMainDlg newGuildMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWGUILD_MAIN_DLG) as NewGuildMainDlg;
					if (newGuildMainDlg != null)
					{
						newGuildMainDlg.Set_ApplicantNotice();
					}
				}
			}
			else
			{
				if (0L < packet.i64CurGuildID)
				{
					NrTSingleton<NewGuildManager>.Instance.RemoveApplicant(packet.i64PersonID_NewMember);
					if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NEWGUILD_ADMINMENU_DLG))
					{
						NewGuildAdminMenuDlg newGuildAdminMenuDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWGUILD_ADMINMENU_DLG) as NewGuildAdminMenuDlg;
						if (newGuildAdminMenuDlg2 != null)
						{
							newGuildAdminMenuDlg2.RefreshInfo();
						}
					}
				}
				if (packet.i32Result == 1 && NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NEWGUILD_ADMINMENU_DLG))
				{
					NewGuildAdminMenuDlg newGuildAdminMenuDlg3 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWGUILD_ADMINMENU_DLG) as NewGuildAdminMenuDlg;
					if (newGuildAdminMenuDlg3 != null)
					{
						newGuildAdminMenuDlg3.RefreshInfo();
					}
				}
			}
		}

		public static void GS_NEWGUILD_MEMBER_LEAVE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_MEMBER_LEAVE_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_MEMBER_LEAVE_ACK>();
			if (packet.i32Result == 0)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("549");
				if (packet.i64PersonID == NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID)
				{
					NrCharUser nrCharUser = (NrCharUser)NrTSingleton<NkCharManager>.Instance.GetChar(1);
					if (nrCharUser != null)
					{
						string empty = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("549"),
							"targetname",
							NrTSingleton<NewGuildManager>.Instance.GetGuildName()
						});
						Main_UI_SystemMessage.ADDMessage(empty);
						nrCharUser.SetUserGuildName(string.Empty, 0L, false);
					}
					NrTSingleton<NewGuildManager>.Instance.Clear();
					NrTSingleton<NewGuildManager>.Instance.ClearDlg();
					NrTSingleton<NewGuildManager>.Instance.Leave();
					NrTSingleton<NewGuildManager>.Instance.ClearAgit();
					NoticeIconDlg.SetIcon(ICON_TYPE.MINE_RECORED, false);
					NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
					DateTime dateTime = DateTime.Now.ToLocalTime();
					DateTime arg_10C_0 = dateTime;
					DateTime dateTime2 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
					int num = (int)(arg_10C_0 - dateTime2.ToLocalTime()).TotalSeconds;
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("ts", num.ToString());
					dictionary.Add("step", "leave");
					dictionary.Add("level", myCharInfo.GetLevel().ToString());
					dictionary.Add("account_id", myCharInfo.m_SN.ToString());
					GameObject pkGoOminiata = StageSystemCheck.pkGoOminiata;
					if (pkGoOminiata)
					{
						OmniataComponent component = pkGoOminiata.GetComponent<OmniataComponent>();
						if (component)
						{
							component.Track("om_guild", dictionary);
						}
					}
				}
				else
				{
					textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("526");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromNotify, new object[]
					{
						textFromNotify,
						"targetname",
						TKString.NEWString(packet.strCharName)
					});
					Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
			}
			else if (packet.i32Result == -20)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("387"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			else if (packet.i32Result == 1)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("665"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}

		public static void GS_NEWGUILD_MEMBER_DISCHARGE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_MEMBER_DISCHARGE_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_MEMBER_DISCHARGE_ACK>();
			if (packet.i32Result == 0)
			{
				string text = string.Empty;
				if (packet.i64DischargePersonID == NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID)
				{
					NrTSingleton<NewGuildManager>.Instance.Clear();
					NrTSingleton<NewGuildManager>.Instance.ClearDlg();
					NrTSingleton<NewGuildManager>.Instance.Leave();
					NrTSingleton<NewGuildManager>.Instance.ClearAgit();
					NoticeIconDlg.SetIcon(ICON_TYPE.MINE_RECORED, false);
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("378");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						text,
						"targetname",
						TKString.NEWString(packet.strGuildName)
					});
				}
				else
				{
					NrTSingleton<NewGuildManager>.Instance.RemoveGuildMember(packet.i64DischargePersonID);
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("107");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						text,
						"targetname",
						TKString.NEWString(packet.strCharName)
					});
					if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NEWGUILD_MEMBER_DLG))
					{
						NewGuildMemberDlg newGuildMemberDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWGUILD_MEMBER_DLG) as NewGuildMemberDlg;
						if (newGuildMemberDlg != null)
						{
							newGuildMemberDlg.RefreshInfo();
						}
					}
				}
				if (string.Empty != text)
				{
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
			}
			else if (packet.i32Result == -20)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("387"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			else if (packet.i32Result == 1)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("665"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}

		public static void GS_NEWGUILD_MEMBER_CHANGE_RANK_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_MEMBER_CHANGE_RANK_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_MEMBER_CHANGE_RANK_ACK>();
			if (packet.i32Result == 0)
			{
				string text = string.Empty;
				string text2 = string.Empty;
				string text3 = string.Empty;
				string text4 = string.Empty;
				if (0L < packet.i64PersonID_ChangeRank)
				{
					text3 = NrTSingleton<NewGuildManager>.Instance.GetMemberRankText(packet.i64PersonID_ChangeRank);
					text4 = NrTSingleton<NewGuildManager>.Instance.GetRankText((NewGuildDefine.eNEWGUILD_MEMBER_RANK)packet.i8NewRank);
					NrTSingleton<NewGuildManager>.Instance.ChangeMemberRank(packet.i64PersonID_ChangeRank, (NewGuildDefine.eNEWGUILD_MEMBER_RANK)packet.i8NewRank);
					text2 = TKString.NEWString(packet.strCharNameChangeRank);
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("538");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						text,
						"targetname",
						text2,
						"position1",
						text3,
						"position2",
						text4
					});
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
				if (0L < packet.i64PersonID_Swap)
				{
					text3 = NrTSingleton<NewGuildManager>.Instance.GetMemberRankText(packet.i64PersonID_Swap);
					text4 = NrTSingleton<NewGuildManager>.Instance.GetRankText((NewGuildDefine.eNEWGUILD_MEMBER_RANK)packet.ui8MemberRank_Swap);
					NrTSingleton<NewGuildManager>.Instance.ChangeMemberRank(packet.i64PersonID_Swap, (NewGuildDefine.eNEWGUILD_MEMBER_RANK)packet.ui8MemberRank_Swap);
					text2 = TKString.NEWString(packet.strCharNameSwap);
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("538");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						text,
						"targetname",
						text2,
						"position1",
						text3,
						"position2",
						text4
					});
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
			}
		}

		public static void GS_NEWGUILD_LIST_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_LIST_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_LIST_ACK>();
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NEWGUILD_LIST_DLG))
			{
				NewGuildListDlg newGuildListDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWGUILD_LIST_DLG) as NewGuildListDlg;
				if (newGuildListDlg != null)
				{
					newGuildListDlg.SetGuildList(packet, kDeserializePacket);
				}
			}
		}

		public static void GS_NEWGUILD_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_INFO_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_INFO_ACK>();
			NrTSingleton<NewGuildManager>.Instance.Clear();
			NrTSingleton<NewGuildManager>.Instance.SetGuildInfo(packet);
			NrTSingleton<GuildWarManager>.Instance.SetCanGetGuildWarReward(packet.bCanGetGuildWarReward);
			BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
			if (bookmarkDlg != null)
			{
				bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.NEWGUILD);
			}
			GuildCollect_DLG guildCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GUILDCOLLECT_DLG) as GuildCollect_DLG;
			if (guildCollect_DLG != null)
			{
				guildCollect_DLG.Update_Notice();
			}
			for (int i = 0; i < (int)packet.i16NumGuildMembers; i++)
			{
				NEWGUILDMEMBER_INFO packet2 = kDeserializePacket.GetPacket<NEWGUILDMEMBER_INFO>();
				NrTSingleton<NewGuildManager>.Instance.AddMemberInfo(packet2);
			}
			for (int i = 0; i < (int)packet.i16NumGuildApplicants; i++)
			{
				NEWGUILDMEMBER_APPLICANT_INFO packet3 = kDeserializePacket.GetPacket<NEWGUILDMEMBER_APPLICANT_INFO>();
				NrTSingleton<NewGuildManager>.Instance.AddApplicantInfo(packet3);
			}
			if (packet.i16LoadInfoType == 0)
			{
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NEWGUILD_MEMBER_DLG))
				{
					NewGuildMemberDlg newGuildMemberDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWGUILD_MEMBER_DLG) as NewGuildMemberDlg;
					if (newGuildMemberDlg != null)
					{
						newGuildMemberDlg.SetGuildWarEnemyString(TKString.NEWString(packet.strGuildWarEnemyName));
						newGuildMemberDlg.RefreshInfo();
					}
				}
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NEWGUILD_ADMINMENU_DLG))
				{
					NewGuildAdminMenuDlg newGuildAdminMenuDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWGUILD_ADMINMENU_DLG) as NewGuildAdminMenuDlg;
					if (newGuildAdminMenuDlg != null)
					{
						newGuildAdminMenuDlg.RefreshInfo();
					}
				}
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.AGIT_MAIN_DLG))
				{
					Agit_MainDlg agit_MainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AGIT_MAIN_DLG) as Agit_MainDlg;
					if (agit_MainDlg != null)
					{
						agit_MainDlg.RefreshInfo();
					}
				}
				NrCharUser nrCharUser = (NrCharUser)NrTSingleton<NkCharManager>.Instance.GetChar(1);
				if (nrCharUser != null)
				{
					if (0 < NrTSingleton<NewGuildManager>.Instance.GetSetImage())
					{
						nrCharUser.SetUserGuildName(NrTSingleton<NewGuildManager>.Instance.GetGuildName(), NrTSingleton<NewGuildManager>.Instance.GetGuildID(), NrTSingleton<NewGuildManager>.Instance.IsGuildWar());
					}
					else
					{
						nrCharUser.SetUserGuildName(NrTSingleton<NewGuildManager>.Instance.GetGuildName(), 0L, NrTSingleton<NewGuildManager>.Instance.IsGuildWar());
					}
				}
				if (SoldierBatch.SOLDIERBATCH != null && (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MYTHRAID))
				{
					BabelTowerInviteFriendListDlg babelTowerInviteFriendListDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABELTOWER_INVITEFRIENDLIST_DLG) as BabelTowerInviteFriendListDlg;
					if (babelTowerInviteFriendListDlg != null)
					{
						babelTowerInviteFriendListDlg.ShowInivteList();
					}
				}
			}
			else if (0L >= NrTSingleton<NewGuildManager>.Instance.GetGuildID())
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_MAINSELECT_DLG);
			}
			else
			{
				NewGuildMemberDlg newGuildMemberDlg2 = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_MEMBER_DLG) as NewGuildMemberDlg;
				if (newGuildMemberDlg2 != null)
				{
					newGuildMemberDlg2.SetGuildWarEnemyString(TKString.NEWString(packet.strGuildWarEnemyName));
					newGuildMemberDlg2.RefreshInfo();
				}
			}
		}

		public static void GS_NEWGUILD_DETAILINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_DETAILINFO_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_DETAILINFO_ACK>();
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.NEWGUILD_ADMINMENU_DLG);
			NewGuildMainDlg newGuildMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_MAIN_DLG) as NewGuildMainDlg;
			if (newGuildMainDlg != null)
			{
				newGuildMainDlg.SetDetailInfo(packet);
			}
		}

		public static void GS_NEWGUILD_OTHERGUILDINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_OTHERGUILDINFO_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_OTHERGUILDINFO_ACK>();
			TsLog.Log(packet.ToString(), new object[0]);
		}

		public static void GS_NEWGUILD_INVITE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_INVITE_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_INVITE_ACK>();
			string empty = string.Empty;
			string text = TKString.NEWString(packet.strInviteCharName);
			int i32Result = packet.i32Result;
			if (i32Result != 0)
			{
				if (i32Result != 1)
				{
					if (i32Result != 6000)
					{
						if (i32Result == 6001)
						{
							Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("36"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
						}
					}
					else
					{
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("37"),
							"targetname",
							text
						});
						Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					}
				}
				else
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("316"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				}
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("536"),
					"targetname",
					text
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
		}

		public static void GS_NEWGUILD_APPLICANT_COUNT_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_APPLICANT_COUNT_GET_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_APPLICANT_COUNT_GET_ACK>();
			NrTSingleton<NewGuildManager>.Instance.SetReadyApplicantCount(packet.i32ApplicantCount);
			if (NrTSingleton<NewGuildManager>.Instance.IsApplicantMemberNum(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
			{
				BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
				if (bookmarkDlg != null)
				{
					bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.NEWGUILD);
				}
				GuildCollect_DLG guildCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GUILDCOLLECT_DLG) as GuildCollect_DLG;
				if (guildCollect_DLG != null)
				{
					guildCollect_DLG.Update_Notice();
				}
			}
		}

		public static void GS_NEWGUILD_NAME_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_NAME_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_NAME_ACK>();
			if (0L < NrTSingleton<NewGuildManager>.Instance.GetGuildID() && NrTSingleton<NewGuildManager>.Instance.GetGuildID() == packet.i64GuildID)
			{
				if (0 < packet.ui8SetImage)
				{
					NrTSingleton<NewGuildManager>.Instance.SetImage(1);
				}
				else
				{
					NrTSingleton<NewGuildManager>.Instance.SetImage(0);
				}
				NewGuildAdminMenuDlg newGuildAdminMenuDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWGUILD_ADMINMENU_DLG) as NewGuildAdminMenuDlg;
				if (newGuildAdminMenuDlg != null)
				{
					newGuildAdminMenuDlg.SetLoadGuildMark();
				}
			}
			NrCharUser nrCharUser = (NrCharUser)NrTSingleton<NkCharManager>.Instance.GetCharByPersonID(packet.i64PersonID);
			if (nrCharUser != null)
			{
				if (0 < packet.ui8SetImage)
				{
					nrCharUser.SetUserGuildName(TKString.NEWString(packet.strGuildName), packet.i64GuildID, packet.bGuildWar);
				}
				else
				{
					nrCharUser.SetUserGuildName(TKString.NEWString(packet.strGuildName), 0L, packet.bGuildWar);
				}
			}
		}

		public static void GS_NEWGUILD_DEL_NAME_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_DEL_NAME_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_DEL_NAME_ACK>();
			NrCharUser nrCharUser = (NrCharUser)NrTSingleton<NkCharManager>.Instance.GetCharByPersonID(packet.i64PersonID);
			if (nrCharUser != null)
			{
				nrCharUser.SetUserGuildName(string.Empty, 0L, false);
			}
		}

		public static void GS_NEWGUILD_APPLICANT_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_APPLICANT_INFO_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_APPLICANT_INFO_ACK>();
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.NEWGUILD_MAIN_DLG);
			NewGuildApplicationDlg newGuildApplicationDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_APPLICATION_DLG) as NewGuildApplicationDlg;
			if (newGuildApplicationDlg != null)
			{
				string text = TKString.NEWString(packet.strBeforeApplicantGuildName);
				if (text != newGuildApplicationDlg.GetGuildName())
				{
					newGuildApplicationDlg.SetApplicatInfo(text, packet.i64BeforeApplicantGuildID);
				}
				else
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("379"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					newGuildApplicationDlg.CloseNow();
				}
			}
		}

		public static void GS_NEWGUILD_NAME_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_NAME_NFY packet = kDeserializePacket.GetPacket<GS_NEWGUILD_NAME_NFY>();
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo.m_PersonID == packet.PersonID && NrTSingleton<NewGuildManager>.Instance.GetGuildID() != 0L && NrTSingleton<NewGuildManager>.Instance.GetGuildID() == packet.i64GuildID)
			{
				if (packet.bGuildPortrait)
				{
					NrTSingleton<NewGuildManager>.Instance.SetImage(1);
				}
				else
				{
					NrTSingleton<NewGuildManager>.Instance.SetImage(0);
				}
				NewGuildAdminMenuDlg newGuildAdminMenuDlg = (NewGuildAdminMenuDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWGUILD_ADMINMENU_DLG);
				if (newGuildAdminMenuDlg != null)
				{
					newGuildAdminMenuDlg.SetLoadGuildMark();
				}
			}
			NrCharUser nrCharUser = (NrCharUser)NrTSingleton<NkCharManager>.Instance.GetCharByPersonID(packet.PersonID);
			if (nrCharUser != null)
			{
				if (packet.bGuildPortrait)
				{
					nrCharUser.SetUserGuildName(TKString.NEWString(packet.szGuildName), packet.i64GuildID, packet.bGuildWar);
				}
				else
				{
					nrCharUser.SetUserGuildName(TKString.NEWString(packet.szGuildName), 0L, packet.bGuildWar);
				}
			}
		}

		public static void GS_NEWGUILD_MARK_RESET_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_MARK_RESET_NFY packet = kDeserializePacket.GetPacket<GS_NEWGUILD_MARK_RESET_NFY>();
			string guildPortraitURL = NrTSingleton<NkCharManager>.Instance.GetGuildPortraitURL(packet.i64GuildID);
			WebFileCache.RemoveEventItem(guildPortraitURL);
		}

		public static void GS_NEWGUILD_CHANGE_LEVEL_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_CHANGE_LEVEL_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_CHANGE_LEVEL_ACK>();
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("399"),
				"count",
				packet.i16Level
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
		}

		public static void GS_NEWGUILD_BOSS_OPENROOM_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_BOSS_OPENROOM_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_BOSS_OPENROOM_ACK>();
			if (packet.i32Result != 0)
			{
				string text = string.Empty;
				if (packet.i32Result == 1)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("596");
				}
				else if (packet.i32Result == 2)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("554");
				}
				else if (packet.i32Result == 3)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("554");
				}
				else if (packet.i32Result == 11)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("561");
				}
				else if (packet.i32Result == 15)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("599");
				}
				if (text != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
		}

		public static void GS_NEWGUILD_BOSS_ROOMINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_BOSS_ROOMINFO_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_BOSS_ROOMINFO_ACK>();
			if (packet.i32Result == 0)
			{
				BabelGuildBossInfoDlg babelGuildBossInfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABEL_GUILDBOSS_INFO_DLG) as BabelGuildBossInfoDlg;
				if (babelGuildBossInfoDlg != null)
				{
					babelGuildBossInfoDlg.InitInfo();
					babelGuildBossInfoDlg.SetBossInfo(packet.i16Floor, packet.byRoomState, packet.i32BossHP, packet.byReward, packet.i64ClearPersonID);
					for (int i = 0; i < packet.i32Count; i++)
					{
						NEWGUILD_BOSS_PLAYER_INFO packet2 = kDeserializePacket.GetPacket<NEWGUILD_BOSS_PLAYER_INFO>();
						babelGuildBossInfoDlg.AddPlayerInfo(packet2);
					}
					babelGuildBossInfoDlg.Show();
				}
			}
			else
			{
				string text = string.Empty;
				if (packet.i32Result == 4)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("596");
				}
				else if (packet.i32Result == 16)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("609");
				}
				if (text != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
		}

		public static void GS_NEWGUILD_BOSS_ROOMCHECK_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_BOSS_ROOMCHECK_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_BOSS_ROOMCHECK_ACK>();
			if (packet.i32Result == 0)
			{
				SoldierBatch.GUILDBOSS_INFO.m_i16Floor = packet.i16Floor;
				SoldierBatch.GUILDBOSS_INFO.m_i64CurPlayer = packet.i64CurPlayPersonID;
				SoldierBatch.GUILDBOSS_INFO.m_i32BossHP = packet.i32BossHP;
				if (!Scene.IsCurScene(Scene.Type.SOLDIER_BATCH))
				{
					SoldierBatch.SOLDIER_BATCH_MODE = eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP;
					FacadeHandler.PushStage(Scene.Type.SOLDIER_BATCH);
				}
			}
			else
			{
				string text = string.Empty;
				if (packet.i32Result == 1)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("596");
				}
				else if (packet.i32Result == 5)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("558");
				}
				else if (packet.i32Result == 6)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("605");
				}
				else if (packet.i32Result == 9)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("558");
				}
				else if (packet.i32Result == 10)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("592");
				}
				if (text != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				else
				{
					text = string.Format("Reslut {0}", packet.i32Result);
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
		}

		public static void GS_NEWGUILD_BOSS_STARTBATTLE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_BOSS_STARTBATTLE_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_BOSS_STARTBATTLE_ACK>();
			if (packet.i32Result == 0)
			{
				if (packet.i64PersonID != NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID)
				{
					if (SoldierBatch.GUILDBOSS_INFO == null)
					{
						return;
					}
					if (SoldierBatch.GUILDBOSS_INFO.m_i16Floor == packet.i16Floor)
					{
						SoldierBatch.GUILDBOSS_INFO.m_i64CurPlayer = packet.i64PersonID;
						if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_GUILDBOSS_MAKEUP && NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLNUM_DLG) != null)
						{
							PlunderSolNumDlg plunderSolNumDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLNUM_DLG) as PlunderSolNumDlg;
							plunderSolNumDlg.GuildBossBattleUserName();
						}
					}
				}
			}
			else
			{
				string text = string.Empty;
				if (packet.i32Result == 1)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("596");
				}
				else if (packet.i32Result == 10)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("592");
				}
				else if (packet.i32Result == 8)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("559");
				}
				else if (packet.i32Result == 5)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("556");
				}
				else if (packet.i32Result == 9)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("558");
				}
				else if (packet.i32Result == 16)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("609");
				}
				if (text != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				else
				{
					text = string.Format("Reslut {0}", packet.i32Result);
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
		}

		public static void GS_NEWGUILD_BOSS_GETREWARD_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_BOSS_GETREWARD_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_BOSS_GETREWARD_ACK>();
			string text = string.Empty;
			string text2 = string.Empty;
			if (packet.i32Result == 0)
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64AfterMoney;
				BABEL_GUILDBOSS babelGuildBossinfo = NrTSingleton<BabelTowerManager>.Instance.GetBabelGuildBossinfo(packet.i16Floor);
				if (babelGuildBossinfo != null)
				{
					if (0 < packet.i32MailType)
					{
						text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("855");
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
						{
							text2,
							"targetname",
							NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(babelGuildBossinfo.m_nBaseReward_ItemUnique),
							"count",
							babelGuildBossinfo.m_nBaseReward_ItemNum
						});
						Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
					}
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("600");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
					{
						text2,
						"count",
						ANNUALIZED.Convert(packet.i64AddMoney)
					});
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				}
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.AddGuildBossRewardInfo(false);
				BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
				if (bookmarkDlg != null)
				{
					bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.NEWGUILD);
				}
				GuildCollect_DLG guildCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GUILDCOLLECT_DLG) as GuildCollect_DLG;
				if (guildCollect_DLG != null)
				{
					guildCollect_DLG.Update_Notice();
				}
			}
			else
			{
				if (packet.i32Result == 12)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("593");
				}
				else if (packet.i32Result == 13)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("594");
				}
				else if (packet.i32Result == 14)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("595");
				}
				else if (packet.i32Result == 40)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("915");
				}
				else if (packet.i32Result == -10)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("593");
				}
				else
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("593");
				}
				if (text != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
		}

		public static void GS_MAILBOX_SEND_GUILD_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MAILBOX_SEND_GUILD_ACK packet = kDeserializePacket.GetPacket<GS_MAILBOX_SEND_GUILD_ACK>();
			if (packet.i32Result == 0)
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64AfterMoney;
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("510"), SYSTEM_MESSAGE_TYPE.ADMIN_SYSTEM_MESSAGE);
			}
		}

		public static void GS_NEWGUILD_BOSS_MY_ROOMINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_BOSS_MY_ROOMINFO_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_BOSS_MY_ROOMINFO_ACK>();
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (packet.i16Floor <= 0)
			{
				kMyCharInfo.InitGuildBossMyRoomInfo();
			}
			BabelGuildBossDlg babelGuildBossDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABEL_GUILDBOSS_MAIN_DLG) as BabelGuildBossDlg;
			for (short num = 0; num < packet.i16NumMyGuildBossInfo; num += 1)
			{
				NEWGUILD_MY_BOSS_ROOMINFO packet2 = kDeserializePacket.GetPacket<NEWGUILD_MY_BOSS_ROOMINFO>();
				kMyCharInfo.AddGuildBossMyRoomInfo(packet2);
				if (babelGuildBossDlg != null && packet2.i16Floor > 0)
				{
					babelGuildBossDlg.UpdateFloor(packet2.i16Floor);
				}
			}
		}

		public static void GS_NEWGUILD_BOSS_JOININFO_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_BOSS_JOININFO_NFY packet = kDeserializePacket.GetPacket<GS_NEWGUILD_BOSS_JOININFO_NFY>();
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo == null)
			{
				return;
			}
			kMyCharInfo.InitGuildBossRoomStateInfo();
			bool flag;
			if (packet.i32Result == 0)
			{
				flag = true;
				kMyCharInfo.AddGuildBossRewardInfo(flag);
			}
			else
			{
				flag = false;
				kMyCharInfo.AddGuildBossRewardInfo(flag);
			}
			for (short num = 0; num < 16; num += 1)
			{
				if (packet.i16Floor[(int)num] != 0)
				{
					if (packet.i64PersonContribute[(int)num] != -1L)
					{
						kMyCharInfo.AddGuildBossRoomStateInfo(packet.i16Floor[(int)num]);
						flag = true;
					}
				}
			}
			if (flag)
			{
				BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
				if (bookmarkDlg != null)
				{
					bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.NEWGUILD);
				}
				NewGuildMemberDlg newGuildMemberDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWGUILD_MEMBER_DLG) as NewGuildMemberDlg;
				if (newGuildMemberDlg != null)
				{
					newGuildMemberDlg.GuildBossCheck();
				}
				BabelTowerMainDlg babelTowerMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWERMAIN_DLG) as BabelTowerMainDlg;
				if (babelTowerMainDlg != null)
				{
					babelTowerMainDlg.GuildBossCheck();
				}
				BabelGuildBossDlg babelGuildBossDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABEL_GUILDBOSS_MAIN_DLG) as BabelGuildBossDlg;
				if (babelGuildBossDlg != null)
				{
					for (short num2 = 1; num2 <= 16; num2 += 1)
					{
						babelGuildBossDlg.UpdateFloor(num2);
					}
				}
				GuildCollect_DLG guildCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GUILDCOLLECT_DLG) as GuildCollect_DLG;
				if (guildCollect_DLG != null)
				{
					guildCollect_DLG.Update_Notice();
				}
			}
		}

		public static void GS_NEWGUILD_CHANGE_NAME_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_CHANGE_NAME_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_CHANGE_NAME_ACK>();
			int i32Result = packet.i32Result;
			if (i32Result != 0)
			{
				if (i32Result != 1)
				{
					if (i32Result == -20)
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("90"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					}
				}
				else
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("660"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
			else
			{
				NrTSingleton<NewGuildManager>.Instance.ChangeGuildName(TKString.NEWString(packet.strGuildName));
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("661"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}

		public static void GS_NEWGUILD_NOTICE_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_NOTICE_SET_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_NOTICE_SET_ACK>();
			if (packet.i32Result == 0)
			{
				string strGuildNotice = TKString.NEWString(packet.strGuildNotice);
				NrTSingleton<NewGuildManager>.Instance.ChangeGuildNotice(strGuildNotice);
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NEWGUILD_ADMINMENU_DLG))
				{
					NewGuildAdminMenuDlg newGuildAdminMenuDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWGUILD_ADMINMENU_DLG) as NewGuildAdminMenuDlg;
					if (newGuildAdminMenuDlg != null)
					{
						newGuildAdminMenuDlg.RefreshGuildNotice(strGuildNotice);
					}
				}
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("92"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
		}

		public static void GS_NEWGUILD_AGIT_ADD_NPC_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_AGIT_ADD_NPC_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_AGIT_ADD_NPC_ACK>();
			if (packet.i32Result != 0)
			{
				Main_UI_SystemMessage.ADDMessage(string.Format("error result: {0}", packet.i32Result), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				return;
			}
			if (packet.i32Result == 0)
			{
				NrTSingleton<NewGuildManager>.Instance.Set_GS_NEWGUILD_AGIT_ADD_NPC_ACK(packet);
				if (packet.ui8NPCType == 1)
				{
					NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.SetCharSubData(34, 0L);
				}
				Agit_MainDlg agit_MainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AGIT_MAIN_DLG) as Agit_MainDlg;
				if (agit_MainDlg != null)
				{
					agit_MainDlg.RefreshInfo();
				}
				if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kCharMapInfo.MapIndex == 12)
				{
					NrTSingleton<NewGuildManager>.Instance.MakeAgitNPC(packet.ui8NPCType);
				}
			}
			else
			{
				NrTSingleton<NewGuildManager>.Instance.Set_Agit_Resut(packet.i32Result);
			}
		}

		public static void GS_NEWGUILD_AGIT_CREATE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_AGIT_CREATE_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_AGIT_CREATE_ACK>();
			if (packet.i32Result == 0)
			{
				NrTSingleton<NewGuildManager>.Instance.SetFund(packet.i64AfterGuildFund);
				NrTSingleton<NewGuildManager>.Instance.SetExitAgit(true);
				NrTSingleton<NewGuildManager>.Instance.SetAgitLevel(packet.i16AgitLevel);
				NrTSingleton<NewGuildManager>.Instance.SetAgitExp(packet.i64AgitExp);
				NewGuildMemberDlg newGuildMemberDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWGUILD_MEMBER_DLG) as NewGuildMemberDlg;
				if (newGuildMemberDlg != null)
				{
					newGuildMemberDlg.SetLayerState();
					newGuildMemberDlg.CheckAgitEnter();
				}
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("766"),
					"guildname",
					NrTSingleton<NewGuildManager>.Instance.GetGuildName()
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
		}

		public static void GS_NEWGUILD_AGIT_DEL_NPC_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_AGIT_DEL_NPC_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_AGIT_DEL_NPC_ACK>();
			if (packet.i32Result == 0)
			{
				NrTSingleton<NewGuildManager>.Instance.DelAgitNPC(packet.ui8NPCType);
				Agit_MainDlg agit_MainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AGIT_MAIN_DLG) as Agit_MainDlg;
				if (agit_MainDlg != null)
				{
					agit_MainDlg.RefreshInfo();
				}
			}
			else
			{
				NrTSingleton<NewGuildManager>.Instance.Set_Agit_Resut(packet.i32Result);
			}
		}

		public static void GS_NEWGUILD_AGIT_LEVEL_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_AGIT_LEVEL_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_AGIT_LEVEL_ACK>();
			if (packet.i32Result == 0)
			{
				if (NrTSingleton<NewGuildManager>.Instance.GetAgitLevel() != 0 && NrTSingleton<NewGuildManager>.Instance.GetAgitLevel() < packet.i16AfterAgitLevel)
				{
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("767"),
						"level",
						packet.i16AfterAgitLevel
					});
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
				NrTSingleton<NewGuildManager>.Instance.SetFund(packet.i64AfterGuildFund);
				NrTSingleton<NewGuildManager>.Instance.SetAgitLevel(packet.i16AfterAgitLevel);
				NrTSingleton<NewGuildManager>.Instance.SetAgitExp(packet.i64AfterAgitExp);
				Agit_MainDlg agit_MainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AGIT_MAIN_DLG) as Agit_MainDlg;
				if (agit_MainDlg != null)
				{
					agit_MainDlg.RefreshInfo();
				}
			}
			else
			{
				NrTSingleton<NewGuildManager>.Instance.Set_Agit_Resut(packet.i32Result);
			}
		}

		public static void GS_NEWGUILD_DONATION_FUND_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_DONATION_FUND_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_DONATION_FUND_ACK>();
			if (packet.i32Result == 0)
			{
				NrTSingleton<NewGuildManager>.Instance.SetFund(packet.i64AfterGuildFund);
				if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID == packet.i64PersonID)
				{
					NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64AfterCharMoney;
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("768"),
						"count",
						ANNUALIZED.Convert(packet.i64AddGuildFund)
					});
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
				Agit_MainDlg agit_MainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AGIT_MAIN_DLG) as Agit_MainDlg;
				if (agit_MainDlg != null)
				{
					agit_MainDlg.RefreshInfo();
				}
			}
			else
			{
				NrTSingleton<NewGuildManager>.Instance.Set_Agit_Resut(packet.i32Result);
			}
		}

		public static void GS_NEWGUILD_FUND_USE_HISTORY_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_FUND_USE_HISTORY_GET_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_FUND_USE_HISTORY_GET_ACK>();
			Agit_MainDlg agit_MainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AGIT_MAIN_DLG) as Agit_MainDlg;
			if (agit_MainDlg != null)
			{
				agit_MainDlg.ClearFundUseInfo();
			}
			for (int i = 0; i < (int)packet.i16Count; i++)
			{
				NEWGUILD_FUND_USE_INFO packet2 = kDeserializePacket.GetPacket<NEWGUILD_FUND_USE_INFO>();
				if (agit_MainDlg != null)
				{
					agit_MainDlg.AddFundUseInfo(packet2);
				}
			}
			if (agit_MainDlg != null)
			{
				agit_MainDlg.RefreshInfoGold();
			}
		}

		public static void GS_NEWGUILD_AGIT_MERCHANT_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_AGIT_MERCHANT_INFO_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_AGIT_MERCHANT_INFO_ACK>();
			Agit_MerchantDlg agit_MerchantDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AGIT_MERCHANT_DLG) as Agit_MerchantDlg;
			if (agit_MerchantDlg != null)
			{
				agit_MerchantDlg.ClearMerchantItem();
			}
			for (int i = 0; i < (int)packet.i16Count; i++)
			{
				AGIT_MERCHANT_SUB_INFO packet2 = kDeserializePacket.GetPacket<AGIT_MERCHANT_SUB_INFO>();
				if (agit_MerchantDlg != null)
				{
					agit_MerchantDlg.AddMerchantItem(packet2);
				}
			}
			if (agit_MerchantDlg != null)
			{
				agit_MerchantDlg.RefreshInfo();
			}
		}

		public static void GS_NEWGUILD_AGIT_NPC_USE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_AGIT_NPC_USE_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_AGIT_NPC_USE_ACK>();
			if (packet.i32Result == 0)
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.SetCharSubData(packet.i32SubdataType, packet.i64SubdataValue);
				Agit_MerchantDlg agit_MerchantDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AGIT_MERCHANT_DLG) as Agit_MerchantDlg;
				if (agit_MerchantDlg != null)
				{
					agit_MerchantDlg.RefreshInfo();
				}
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("195"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			else if (packet.i32Result == 9500)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("785");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
			}
			else
			{
				NrTSingleton<NewGuildManager>.Instance.Set_GS_NEWGUILD_AGIT_NPC_USE_ACK(packet.i32Result);
			}
		}

		public static void GS_NEWGUILD_AGIT_NPC_SUB_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_AGIT_NPC_SUB_INFO_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_AGIT_NPC_SUB_INFO_ACK>();
			NrTSingleton<NewGuildManager>.Instance.ClearAgit();
			for (int i = 0; i < (int)packet.i16Count; i++)
			{
				AGIT_NPC_SUB_DATA packet2 = kDeserializePacket.GetPacket<AGIT_NPC_SUB_DATA>();
				NrTSingleton<NewGuildManager>.Instance.AddAgitNPCData(packet2);
			}
		}

		public static void GS_NEWGUILD_AGIT_GOLDENEGG_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_AGIT_GOLDENEGG_INFO_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_AGIT_GOLDENEGG_INFO_ACK>();
			List<AGIT_GOLDENEGG_INFO_SUB_DATA> rewardPersonInfoList = NrTSingleton<NewGuildManager>.Instance.GetRewardPersonInfoList();
			if (NrTSingleton<NewGuildManager>.Instance.GetGoldenEggGetCount() != packet.i32GoldenEggGetCount)
			{
				rewardPersonInfoList.Clear();
			}
			NrTSingleton<NewGuildManager>.Instance.SetGoldenEggGetCount(packet.i32GoldenEggGetCount);
			Agit_GoldenEggDlg agit_GoldenEggDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AGIT_GOLDENEGG_DLG) as Agit_GoldenEggDlg;
			if (agit_GoldenEggDlg != null)
			{
				for (int i = 0; i < (int)packet.i16Count; i++)
				{
					AGIT_GOLDENEGG_INFO_SUB_DATA packet2 = kDeserializePacket.GetPacket<AGIT_GOLDENEGG_INFO_SUB_DATA>();
					foreach (AGIT_GOLDENEGG_INFO_SUB_DATA current in rewardPersonInfoList)
					{
						if (current.i64PersonID == packet2.i64PersonID)
						{
						}
					}
					rewardPersonInfoList.Add(packet2);
				}
				agit_GoldenEggDlg.SetInfo(packet.i32GoldenEggPoint, packet.i32GoldenEggGetCount > 0);
			}
		}

		public static void GS_NEWGUILD_AGIT_GOLDENEGG_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_AGIT_GOLDENEGG_GET_ACK packet = kDeserializePacket.GetPacket<GS_NEWGUILD_AGIT_GOLDENEGG_GET_ACK>();
			if (packet.i32Result == 0)
			{
				Agit_GoldenEggDramaDlg agit_GoldenEggDramaDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AGIT_GOLDENEGGDRAMA_DLG) as Agit_GoldenEggDramaDlg;
				if (agit_GoldenEggDramaDlg != null)
				{
					agit_GoldenEggDramaDlg.SetItem(packet.kItem);
					agit_GoldenEggDramaDlg.SetEggType(packet.i8GoldenEggType);
				}
				NrTSingleton<NewGuildManager>.Instance.SetCanGoldenEggReward(false);
			}
			else if (packet.i32Result == 31)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			else if (packet.i32Result == 9500)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("785"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			else if (packet.i32Result == -10)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("199"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
			}
			else if (packet.i32Result == 9400)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("593"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
			}
			else
			{
				string message = string.Format("result {0}", packet.i32Result);
				Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
			}
		}

		public static void GS_NEWGUILD_AGIT_GOLDENEGG_REWARD_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWGUILD_AGIT_GOLDENEGG_REWARD_NFY packet = kDeserializePacket.GetPacket<GS_NEWGUILD_AGIT_GOLDENEGG_REWARD_NFY>();
			NrTSingleton<NewGuildManager>.Instance.SetCanGoldenEggReward(packet.bCanGet);
			if (packet.bCanGet)
			{
				NrTSingleton<NewGuildManager>.Instance.GS_NEWGUILD_AGIT_GOLDENEGG_REWARD_NFY();
			}
		}

		public static void GS_NOTIFY_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_NOTIFY_NFY packet = kDeserializePacket.GetPacket<GS_NOTIFY_NFY>();
			switch (packet.nNotifyCode)
			{
			case 1:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("450"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 2:
			{
				string text = string.Empty;
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("436");
				if (text != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				break;
			}
			case 3:
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("437"),
					"level",
					packet.nPara[0]
				});
				if (empty != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				break;
			}
			case 4:
			{
				float num = (float)packet.nPara[0] / 100f;
				float num2 = (float)packet.nPara[1] / 100f;
				Debug.Log(string.Concat(new object[]
				{
					"Recv Battle Open Fail : CharPos(",
					num,
					", ",
					num2,
					")"
				}));
				break;
			}
			case 5:
			{
				float num3 = (float)packet.nPara[0] / 100f;
				float num4 = (float)packet.nPara[1] / 100f;
				Debug.Log(string.Concat(new object[]
				{
					"Recv Battle Open Fail : TargetPos(",
					num3,
					", ",
					num4,
					")"
				}));
				break;
			}
			case 6:
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify(packet.nPara[0].ToString());
				if (textFromNotify != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
				if (@char != null)
				{
					@char.MakeChatText(NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo(packet.nPara[1].ToString()), true);
				}
				break;
			}
			case 7:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("531"), SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
				break;
			case 8:
			{
				NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
				if (myCharInfo != null)
				{
					myCharInfo.InitCharDetail();
				}
				CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
				if (communityUI_DLG != null)
				{
					communityUI_DLG.RequestCommunityData(communityUI_DLG.CurShowType);
				}
				NewExplorationMainDlg newExplorationMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWEXPLORATION_MAIN_DLG) as NewExplorationMainDlg;
				if (newExplorationMainDlg != null)
				{
					newExplorationMainDlg.SetInfo();
					newExplorationMainDlg.SetFloorList();
				}
				break;
			}
			case 9:
			{
				NrMyCharInfo myCharInfo2 = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
				if (myCharInfo2 != null)
				{
					myCharInfo2.InitCharWeekData();
				}
				NewExplorationMainDlg newExplorationMainDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NEWEXPLORATION_MAIN_DLG) as NewExplorationMainDlg;
				if (newExplorationMainDlg2 != null)
				{
					newExplorationMainDlg2.SetInfo();
					newExplorationMainDlg2.SetFloorList();
				}
				break;
			}
			case 10:
			{
				string message = string.Empty;
				if (packet.nPara[0] == 0)
				{
					Debug.Log("Remove Exit Reservation");
					if (Battle.BATTLE != null)
					{
						Battle.BATTLE.ExitReservation = false;
						message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("480");
					}
				}
				else if (packet.nPara[0] == 1)
				{
					Debug.Log("Set Exit Reservation");
					if (Battle.BATTLE != null)
					{
						Battle.BATTLE.ExitReservation = true;
						message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("479");
					}
				}
				Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			}
			case 11:
			{
				string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("417");
				Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				NrTSingleton<ChatManager>.Instance.PushSystemMsg(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1230"), textFromNotify2);
				break;
			}
			case 13:
			{
				int num5 = packet.nPara[0];
				string text2 = string.Empty;
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (charPersonInfo == null)
				{
					return;
				}
				for (int i = 0; i < 6; i++)
				{
					NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
					if (soldierInfo != null && soldierInfo.IsValid())
					{
						if (soldierInfo.GetBattlePos() == (short)num5)
						{
							text2 = soldierInfo.GetName();
							break;
						}
					}
				}
				string empty2 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("115"),
					"sol",
					text2
				});
				if (empty2 != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				break;
			}
			case 14:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 15:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("238"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 16:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("294"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 17:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1255"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 18:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("271"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 19:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("243"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 20:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("294"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 21:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("297"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 22:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("298"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 23:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("299"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 24:
			{
				AuctionMainDlg auctionMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.AUCTION_MAIN_DLG) as AuctionMainDlg;
				if (auctionMainDlg != null)
				{
					auctionMainDlg.ClickRefresh(null);
				}
				break;
			}
			case 25:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("283"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 26:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("300"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 27:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("447"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 28:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("217"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 29:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 30:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("94"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 31:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("316"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 32:
			{
				string text3 = string.Empty;
				if (0 < packet.nPara[1])
				{
					text3 = string.Format("{0} {1}", packet.nPara[0].ToString(), "Success");
				}
				else
				{
					text3 = string.Format("{0} {1}", packet.nPara[0].ToString(), "Fail");
				}
				Main_UI_SystemMessage.ADDMessage(text3, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				string name = "[GMCOMMAND]";
				NrTSingleton<ChatManager>.Instance.PushMsg(CHAT_TYPE.SYSTEM, name, text3);
				break;
			}
			case 33:
			{
				NewGuildDefine.eNEWGUILD_RESULT eNEWGUILD_RESULT = (NewGuildDefine.eNEWGUILD_RESULT)packet.nPara[0];
				if (eNEWGUILD_RESULT == NewGuildDefine.eNEWGUILD_RESULT.eNEWGUILD_RESULT_DBLOADING)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				break;
			}
			case 34:
				NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nChatBlockDate = (long)packet.nPara[0];
				break;
			case 35:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("395"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 36:
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("434"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				PlunderSolListDlg plunderSolListDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDERSOLLIST_DLG) as PlunderSolListDlg;
				if (plunderSolListDlg != null)
				{
					int solBatchNum = SoldierBatch.SOLDIERBATCH.GetSolBatchNum();
					plunderSolListDlg.SetSolNum(solBatchNum, false);
					plunderSolListDlg.Show();
				}
				break;
			}
			case 37:
			{
				string empty3 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("425"),
					"count",
					NrTSingleton<NewGuildManager>.Instance.GetNewbieLimitTimeHour()
				});
				Main_UI_SystemMessage.ADDMessage(empty3, SYSTEM_MESSAGE_TYPE.ADMIN_SYSTEM_MESSAGE);
				break;
			}
			case 38:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("510"), SYSTEM_MESSAGE_TYPE.ADMIN_SYSTEM_MESSAGE);
				break;
			case 39:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("513"), SYSTEM_MESSAGE_TYPE.ADMIN_SYSTEM_MESSAGE);
				break;
			case 40:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("193"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 41:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("535"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 42:
			{
				int itemunique = packet.nPara[0];
				int num6 = packet.nPara[1];
				string empty4 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty4, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("597"),
					"itemname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(itemunique),
					"count",
					num6.ToString()
				});
				Main_UI_SystemMessage.ADDMessage(empty4, SYSTEM_MESSAGE_TYPE.ADMIN_SYSTEM_MESSAGE);
				break;
			}
			case 43:
				if (NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("616"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					NrTSingleton<NkBabelMacroManager>.Instance.SetStatus(eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_NONE, 0f);
				}
				break;
			case 44:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("643"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 45:
			{
				string empty5 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty5, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("649"),
					"level1",
					packet.nPara[0],
					"level2",
					packet.nPara[1]
				});
				Main_UI_SystemMessage.ADDMessage(empty5, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			}
			case 46:
			{
				string empty6 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty6, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("649"),
					"level1",
					packet.nPara[0],
					"level2",
					packet.nPara[1]
				});
				Main_UI_SystemMessage.ADDMessage(empty6, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			}
			case 47:
			{
				string empty7 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty7, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("656"),
					"count1",
					packet.nPara[0],
					"count2",
					packet.nPara[1]
				});
				Main_UI_SystemMessage.ADDMessage(empty7, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			}
			case 48:
			{
				string empty8 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty8, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("652"),
					"count1",
					packet.nPara[0],
					"count2",
					packet.nPara[1]
				});
				Main_UI_SystemMessage.ADDMessage(empty8, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			}
			case 49:
			{
				string empty9 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty9, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("687"),
					"LEVEL",
					packet.nPara[0]
				});
				Main_UI_SystemMessage.ADDMessage(empty9, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			}
			case 50:
			{
				string empty10 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty10, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("688"),
					"LEVEL",
					packet.nPara[0]
				});
				Main_UI_SystemMessage.ADDMessage(empty10, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			}
			case 51:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("149"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 52:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("147"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 53:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("619"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 54:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("690"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 55:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("695"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 56:
			{
				string empty11 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty11, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("129"),
					"level",
					packet.nPara[0].ToString()
				});
				Main_UI_SystemMessage.ADDMessage(empty11, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			}
			case 57:
			{
				ColosseumDlg colosseumDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUMMAIN_DLG) as ColosseumDlg;
				if (colosseumDlg != null)
				{
					colosseumDlg.bReceiveBatchList = true;
					colosseumDlg.Show();
				}
				TournamentLobbyDlg tournamentLobbyDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TOURNAMENT_LOBBY_DLG) as TournamentLobbyDlg;
				if (tournamentLobbyDlg != null)
				{
					tournamentLobbyDlg.SetSolList();
				}
				break;
			}
			case 58:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("175"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 59:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("738"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 60:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("732"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 61:
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			case 62:
			{
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				if (msgBoxUI == null)
				{
					return;
				}
				msgBoxUI.SetMsg(null, null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("237"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("238"), eMsgType.MB_OK, 2);
				msgBoxUI.Show();
				break;
			}
			case 63:
			{
				int num7 = packet.nPara[0];
				string empty12 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty12, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("776"),
					"count",
					num7.ToString()
				});
				Main_UI_SystemMessage.ADDMessage(empty12, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			}
			case 64:
			{
				NrMyCharInfo myCharInfo3 = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
				if (myCharInfo3 == null)
				{
					return;
				}
				NrCharBase char2 = NrTSingleton<NkCharManager>.Instance.GetChar(1);
				if (char2 != null)
				{
					char2.m_kCharMove.MoveStop(true, false);
				}
				int num8 = packet.nPara[0];
				int a_Type = packet.nPara[1] / 1000;
				int a_i32Group = packet.nPara[1] % 1000;
				string empty13 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty13, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("786")
				});
				Main_UI_SystemMessage.ADDMessage(empty13, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				int num9 = 0;
				short num10 = 0;
				NrTSingleton<NrAttendance_Manager>.Instance.Get_Attend_Item((eATTENDANCE_USERTYPE)a_Type, a_i32Group, (short)num8, myCharInfo3.ConsecutivelyattendanceRewardType, ref num9, ref num10);
				if (num9 <= 0 || num10 <= 0)
				{
					return;
				}
				long charSubData = myCharInfo3.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_ATTENDANCE_TYPE);
				if (charSubData == 2L)
				{
					New_Attend_Dlg new_Attend_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EVENT_NEW_ATTEND) as New_Attend_Dlg;
					if (new_Attend_Dlg != null)
					{
						new_Attend_Dlg.InitSet();
						new_Attend_Dlg.CheckDailyEventDay(num8);
						new_Attend_Dlg.SetitemToolTip(num9, (int)num10);
					}
				}
				else
				{
					Normal_Attend_Dlg normal_Attend_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EVENT_NORMAL_ATTEND) as Normal_Attend_Dlg;
					if (normal_Attend_Dlg != null)
					{
						normal_Attend_Dlg.InitSet();
						normal_Attend_Dlg.SetitemToolTip(num9, (int)num10);
					}
				}
				break;
			}
			case 65:
			{
				int num11 = packet.nPara[0];
				string empty14 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty14, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("811"),
					"hour",
					num11.ToString()
				});
				Main_UI_SystemMessage.ADDMessage(empty14, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				break;
			}
			case 66:
			{
				string textFromNotify3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("816");
				Main_UI_SystemMessage.ADDMessage(textFromNotify3, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				break;
			}
			}
		}

		public static void GS_NOTIFY_TEXT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NOTIFY_TEXT_ACK packet = kDeserializePacket.GetPacket<GS_NOTIFY_TEXT_ACK>();
			string text = TKString.NEWString(packet.strNotify);
			if (string.Empty != text)
			{
				Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			}
		}

		public static void GS_PLUNDER_MATCH_PLAYER_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_PLUNDER_MATCH_PLAYER_ACK packet = kDeserializePacket.GetPacket<GS_PLUNDER_MATCH_PLAYER_ACK>();
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (packet.i32Result == 7000)
			{
				NrTSingleton<NkClientLogic>.Instance.SetClearMiddleStage();
				myCharInfo.PlunderMoney = 0L;
				myCharInfo.PlunderCharName = string.Empty;
				myCharInfo.PlunderCharLevel = 0;
				StageWorld.PLUNDERMSG_TYPE = ePLUNDER_MESSAGE.ePLUNDER_MESSAGE_MATCH_FAIL;
			}
			if (myCharInfo.PlunderCharLevel != 0)
			{
				SoldierBatch.SOLDIERBATCH.RemoveLoadingEffect = true;
			}
		}

		public static void GS_PLUNDER_MATCH_PLAYER_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_PLUNDER_MATCH_PLAYER_INFO_ACK packet = kDeserializePacket.GetPacket<GS_PLUNDER_MATCH_PLAYER_INFO_ACK>();
			long num = packet.m_nMoney + packet.m_nSellMoney;
			SoldierBatch.SOLDIERBATCH.m_cTargetInfo.SetTargetCharInfo(TKString.NEWString(packet.m_szTargetLeaderName), packet.m_nTargetLeaderLevel, num);
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			bool flag = false;
			if (kMyCharInfo.PlunderCharLevel != 0)
			{
				SoldierBatch.SOLDIERBATCH.RemoveLoadingEffect = true;
				flag = true;
			}
			PlunderTargetInfoDlg plunderTargetInfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDERTARGETINFO_DLG) as PlunderTargetInfoDlg;
			if (plunderTargetInfoDlg != null)
			{
				plunderTargetInfoDlg.SetTargetInfo(TKString.NEWString(packet.m_szTargetLeaderName), packet.m_nTargetLeaderLevel, num);
				if (!flag)
				{
					plunderTargetInfoDlg.Hide();
				}
			}
			for (int i = 0; i < 15; i++)
			{
				if (!SoldierBatch.SOLDIERBATCH.m_bMakeEnemyChar)
				{
					SoldierBatch.SOLDIERBATCH.AddEnemyCharInfo(packet.stTagetSolInfo[i]);
				}
				else
				{
					SoldierBatch.SOLDIERBATCH.MakePlunderCharEnemy(packet.stTagetSolInfo[i], i);
				}
			}
		}

		public static void GS_PLUNDER_RECORD_LIST_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_PLUNDER_RECORD_LIST_GET_ACK packet = kDeserializePacket.GetPacket<GS_PLUNDER_RECORD_LIST_GET_ACK>();
			PlunderRecordDlg plunderRecordDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDERRECORD_DLG) as PlunderRecordDlg;
			if (plunderRecordDlg != null)
			{
				for (int i = 0; i < packet.i32RecordCount; i++)
				{
					PLUNDER_RECORDINFO packet2 = kDeserializePacket.GetPacket<PLUNDER_RECORDINFO>();
					plunderRecordDlg.AddPlunderRecordInfo(packet2);
				}
			}
		}

		public static void GS_PLUNDER_PROTECT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_PLUNDER_PROTECT_ACK packet = kDeserializePacket.GetPacket<GS_PLUNDER_PROTECT_ACK>();
			if (packet.nResult == 0)
			{
				int num = 0;
				if (packet.m_nMode == 0)
				{
					num = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_PROTECT_TIME_BUY1) / 60;
				}
				else if (packet.m_nMode == 1)
				{
					num = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_PROTECT_TIME_BUY2) / 60;
				}
				string empty = string.Empty;
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("274");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromNotify,
					"timestring",
					num.ToString()
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
			else
			{
				string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("314");
				Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}

		public static void GS_PLUNDER_AGREE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_PLUNDER_AGREE_ACK packet = kDeserializePacket.GetPacket<GS_PLUNDER_AGREE_ACK>();
			if (packet.nResult == 0 && NrTSingleton<ContentsLimitManager>.Instance.IsHeroBattle())
			{
				PlunderAgreeDlg plunderAgreeDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDER_AGREE_DLG) as PlunderAgreeDlg;
				if (plunderAgreeDlg != null)
				{
					plunderAgreeDlg.SetResultMsgBox();
				}
			}
		}

		public static void GS_PLUNDER_CHARACTER_HISTORY_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_PLUNDER_CHARACTER_HISTORY_NFY packet = kDeserializePacket.GetPacket<GS_PLUNDER_CHARACTER_HISTORY_NFY>();
			GameGuidePlunderInfo gameGuidePlunderInfo = NrTSingleton<GameGuideManager>.Instance.GetGuide(GameGuideType.PLUNDER_INFO) as GameGuidePlunderInfo;
			if (gameGuidePlunderInfo != null)
			{
				gameGuidePlunderInfo.SetPlunderInfo(packet.i32PlunderCount, packet.i64PlunderCurMoney, packet.i64PlunderSellMoney, packet.i64PlunderSupportMoney);
				NrTSingleton<GameGuideManager>.Instance.Update(GameGuideType.PLUNDER_INFO);
			}
		}

		public static void GS_PLUNDER_RANKINFO_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_PLUNDER_RANKINFO_GET_ACK packet = kDeserializePacket.GetPacket<GS_PLUNDER_RANKINFO_GET_ACK>();
			if (packet.i32Result == 0)
			{
				PlunderRankInfoDlg plunderRankInfoDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.PLUNDER_RANKINFO_DLG) as PlunderRankInfoDlg;
				bool flag = false;
				if (packet.byRank_GetType == 0)
				{
					flag = true;
				}
				for (int i = 0; i < packet.count; i++)
				{
					PLUNDER_RANKINFO packet2 = kDeserializePacket.GetPacket<PLUNDER_RANKINFO>();
					if (plunderRankInfoDlg != null)
					{
						if (packet2.byRankAckType == 0)
						{
							plunderRankInfoDlg.AddRankInfo(packet2);
						}
						else if (packet2.byRankAckType == 1)
						{
							plunderRankInfoDlg.SetTargetInfo(packet2);
						}
					}
				}
				if (plunderRankInfoDlg != null)
				{
					if (flag)
					{
						plunderRankInfoDlg.ShowInfo(eRANK_SHOWTYPE.eRANK_SHOWTYPE_TOTALRANK);
					}
					else
					{
						plunderRankInfoDlg.ShowTargetInfo();
					}
				}
			}
			else if (packet.i32Result == -1)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("144"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
		}

		public static void GS_PLUNDER_FRIENDRANKINFO_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_PLUNDER_FRIENDRANKINFO_GET_ACK packet = kDeserializePacket.GetPacket<GS_PLUNDER_FRIENDRANKINFO_GET_ACK>();
			PlunderRankInfoDlg plunderRankInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDER_RANKINFO_DLG) as PlunderRankInfoDlg;
			if (plunderRankInfoDlg != null)
			{
				plunderRankInfoDlg.InitData();
			}
			for (int i = 0; i < packet.count; i++)
			{
				PLUNDER_FRIEND_RANKINFO packet2 = kDeserializePacket.GetPacket<PLUNDER_FRIEND_RANKINFO>();
				if (plunderRankInfoDlg != null)
				{
					plunderRankInfoDlg.AddFriendRankInfo(packet2);
				}
			}
			if (plunderRankInfoDlg != null)
			{
				plunderRankInfoDlg.ShowInfo(eRANK_SHOWTYPE.eRANK_SHOWTYPE_FRIENDRANK);
			}
		}

		public static void GS_PLUNDER_OBJECT_BATCH_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_PLUNDER_OBJECT_BATCH_ACK packet = kDeserializePacket.GetPacket<GS_PLUNDER_OBJECT_BATCH_ACK>();
			if (SoldierBatch.SOLDIERBATCH == null)
			{
				return;
			}
			SoldierBatch.SOLDIERBATCH.GS_PLUNDER_OBJECT_BATCH_ACK(packet);
		}

		public static void GS_MAILBOX_VERIFY_CHARNAME_ACK(NkDeserializePacket kDeserializePacket)
		{
			string title = string.Empty;
			string message = string.Empty;
			GS_MAILBOX_VERIFY_CHARNAME_ACK packet = kDeserializePacket.GetPacket<GS_MAILBOX_VERIFY_CHARNAME_ACK>();
			title = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1111");
			if (packet.Result != 0)
			{
				message = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("103");
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				msgBoxUI.SetMsg(null, null, title, message, eMsgType.MB_OK, 2);
			}
			else
			{
				PostDlg postDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG) as PostDlg;
				if (postDlg != null)
				{
					postDlg.OnCharNameVerified();
				}
			}
		}

		public static void GS_MAILBOX_MINE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MAILBOX_MINE_ACK packet = kDeserializePacket.GetPacket<GS_MAILBOX_MINE_ACK>();
			int num = packet.i32MailBoxNum;
			GS_MAILBOX_INFO[] array = new GS_MAILBOX_INFO[num];
			if (5 < num)
			{
				num = 5;
			}
			for (int i = 0; i < num; i++)
			{
				array[i] = kDeserializePacket.GetPacket<GS_MAILBOX_INFO>();
				if (Application.isEditor)
				{
					TsLog.LogWarning(" {0} : {1} , {2} , {3} , {4} , {5} , {6} , {7} , {8} , {9} , {10} , {11} ", new object[]
					{
						i,
						array[i].i64MailID,
						array[i].i32MailType,
						array[i].i64DateVary_Send,
						array[i].i64CharMoney,
						array[i].i64ItemID,
						array[i].i64DateVary_End,
						array[i].i32ItemUnique,
						array[i].i32ItemNum,
						array[i].i32CharKind,
						array[i].i64SolID,
						array[i].i8Grade
					});
				}
			}
			PostDlg postDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG) as PostDlg;
			if (postDlg != null)
			{
				postDlg.SetRecvList(packet.i32TotalMailNum, num, ref array);
			}
			if (num <= 0)
			{
				NoticeIconDlg.SetIcon(ICON_TYPE.POST, false);
			}
			else if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.POST_DLG))
			{
				NoticeIconDlg.SetIcon(ICON_TYPE.POST, true);
			}
		}

		public static void GS_MAILBOX_MAILINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MAILBOX_MAILINFO_ACK packet = kDeserializePacket.GetPacket<GS_MAILBOX_MAILINFO_ACK>();
			PostDlg postDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG) as PostDlg;
			if (postDlg != null)
			{
				PostRecvDlg postRecvDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.POST_RECV_DLG) as PostRecvDlg;
				if (postRecvDlg != null)
				{
					postRecvDlg.ShowMailInfo(packet);
				}
			}
		}

		public static void GS_MAILBOX_MAILINFO_REPORT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MAILBOX_MAILINFO_REPORT_ACK packet = kDeserializePacket.GetPacket<GS_MAILBOX_MAILINFO_REPORT_ACK>();
			Debug.Log("mailtype : " + packet.i32MailType.ToString());
			eMAIL_TYPE i32MailType = (eMAIL_TYPE)packet.i32MailType;
			Debug.Log("mailtype : NONE");
		}

		public static void GS_MAILBOX_SEND_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MAILBOX_SEND_ACK packet = kDeserializePacket.GetPacket<GS_MAILBOX_SEND_ACK>();
			if (packet.nResult != 0)
			{
				if (packet.nResult == 9300)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("744"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
				}
				return;
			}
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo != null)
			{
				kMyCharInfo.m_Money = packet.nMoney;
			}
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("99");
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				textFromNotify,
				"targetname",
				TKString.NEWString(packet.szRecvName)
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
			PostDlg postDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG) as PostDlg;
			if (postDlg != null)
			{
				postDlg.ItemSlotClear();
			}
		}

		public static void GS_MAILBOX_TAKE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MAILBOX_TAKE_ACK packet = kDeserializePacket.GetPacket<GS_MAILBOX_TAKE_ACK>();
			if (packet.result != 0)
			{
				return;
			}
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.money;
			if (packet.Addmoney > 0L)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("804");
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromNotify,
					"count",
					packet.Addmoney
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
			}
			PostDlg postDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG) as PostDlg;
			if (postDlg != null)
			{
				postDlg.RequestNextRecvList();
			}
			if (packet.nItem.IsValid())
			{
				string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.nItem.m_nItemUnique);
				string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("803");
				string empty2 = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					textFromNotify2,
					"targetname",
					itemNameByItemUnique,
					"count",
					packet.AddItemNum
				});
				Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
			}
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MAIL", "MAILGET", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			if (0L < packet.SoldierInfo.SolID)
			{
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (charPersonInfo != null)
				{
					SOLDIER_BATTLESKILL_INFO sOLDIER_BATTLESKILL_INFO = new SOLDIER_BATTLESKILL_INFO();
					sOLDIER_BATTLESKILL_INFO.SolID = packet.SoldierInfo.SolID;
					for (int i = 0; i < 6; i++)
					{
						sOLDIER_BATTLESKILL_INFO.BattleSkillData[i].BattleSkillUnique = packet.BattleSkillData[i].BattleSkillUnique;
						sOLDIER_BATTLESKILL_INFO.BattleSkillData[i].BattleSkillLevel = packet.BattleSkillData[i].BattleSkillLevel;
					}
					NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
					readySolList.AddSolInfo(packet.SoldierInfo, sOLDIER_BATTLESKILL_INFO, true);
					for (int i = 0; i < 16; i++)
					{
						NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet.SoldierInfo.SolID);
						if (soldierInfoFromSolID != null)
						{
							soldierInfoFromSolID.SetSolSubData(i, packet.SolSubData[i]);
						}
					}
					NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
					kMyCharInfo.SetCharSolGuide(packet.SoldierInfo.CharKind);
					NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(packet.SoldierInfo.CharKind);
					if (charKindInfo == null)
					{
						return;
					}
					string textFromNotify3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("802");
					string empty3 = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
					{
						textFromNotify3,
						"targetname",
						charKindInfo.GetName()
					});
					Main_UI_SystemMessage.ADDMessage(empty3, SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
				}
			}
		}

		public static void GS_MAILBOX_TAKE_REPORT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MAILBOX_TAKE_REPORT_ACK packet = kDeserializePacket.GetPacket<GS_MAILBOX_TAKE_REPORT_ACK>();
			if (packet.Result != 0)
			{
				return;
			}
			PostDlg postDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG) as PostDlg;
			if (postDlg != null)
			{
				postDlg.RequestNextRecvList();
			}
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.POST_RECV_DLG);
			MineRecordDlg mineRecordDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MINE_RECORD_DLG) as MineRecordDlg;
			if (mineRecordDlg != null)
			{
				mineRecordDlg.Refresh_If_NonCompleteList();
			}
		}

		public static void GS_MAILBOX_HISTORY_LIST_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MAILBOX_HISTORY_LIST_ACK packet = kDeserializePacket.GetPacket<GS_MAILBOX_HISTORY_LIST_ACK>();
			int i32MailHistoryNum = packet.i32MailHistoryNum;
			MAILBOXHISTORY_INFO[] array = new MAILBOXHISTORY_INFO[i32MailHistoryNum];
			for (int i = 0; i < i32MailHistoryNum; i++)
			{
				array[i] = kDeserializePacket.GetPacket<MAILBOXHISTORY_INFO>();
				if (Application.isEditor)
				{
					TsLog.LogWarning(" {0} : {1} , {2} , {3} , {4} , {5} , {6} ", new object[]
					{
						i,
						array[i].i64MailID,
						array[i].i32MailType,
						array[i].i64DateVary_Send,
						array[i].nMoney,
						array[i].nItemUnique,
						array[i].nSolKind
					});
				}
			}
			PostDlg postDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG) as PostDlg;
			if (postDlg != null)
			{
				postDlg.ShowHistory(packet, packet.i32TotalMailHistoryNum, i32MailHistoryNum, ref array);
			}
		}

		public static void GS_MAILBOX_HISTORY_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MAILBOX_HISTORY_ACK packet = kDeserializePacket.GetPacket<GS_MAILBOX_HISTORY_ACK>();
			PostDlg postDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG) as PostDlg;
			if (postDlg != null)
			{
				PostRecvDlg postRecvDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.POST_RECV_DLG) as PostRecvDlg;
				if (postRecvDlg != null)
				{
					postRecvDlg.ShowHistoryInfo(packet, kDeserializePacket);
				}
			}
		}

		public static void GS_MAILBOX_TAKE_GETMAILALL_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MAILBOX_TAKE_GETMAILALL_ACK packet = kDeserializePacket.GetPacket<GS_MAILBOX_TAKE_GETMAILALL_ACK>();
			TsLog.LogWarning(" GS_MAILBOX_TAKE_GETMAILALL_ACK : Result : {0} ", new object[]
			{
				packet.Result
			});
			string text = string.Empty;
			int result = packet.Result;
			if (result != -50)
			{
				if (result == -40)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("507");
					goto IL_A3;
				}
				if (result == -30)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("447");
					goto IL_A3;
				}
				if (result != -20 && result != -10)
				{
					goto IL_A3;
				}
			}
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270");
			IL_A3:
			if (packet.Result != 0)
			{
				if (text != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
				}
				else
				{
					Debug.LogError("======= GS_MAILBOX_TAKE_GETMAILALL_ACK : " + packet.Result);
				}
			}
		}

		public static void GS_MAILBOX_TAKE_GETMAIL_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MAILBOX_TAKE_GETMAIL_ACK packet = kDeserializePacket.GetPacket<GS_MAILBOX_TAKE_GETMAIL_ACK>();
			TsLog.LogWarning(" GS_MAILBOX_TAKE_GETMAIL_ACK : Result : {0} ", new object[]
			{
				packet.Result
			});
			string text = string.Empty;
			int result = packet.Result;
			if (result != -50)
			{
				if (result == -40)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("507");
					goto IL_A3;
				}
				if (result == -30)
				{
					text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("447");
					goto IL_A3;
				}
				if (result != -20 && result != -10)
				{
					goto IL_A3;
				}
			}
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270");
			IL_A3:
			if (packet.Result != 0)
			{
				if (text != string.Empty)
				{
					Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
				}
				else
				{
					Debug.LogError("======= GS_MAILBOX_TAKE_GETMAIL_ACK : " + packet.Result);
				}
			}
		}

		public static void GS_MAILBOX_ALLSENDITEM_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_MAILBOX_ALLSENDITEM_NFY packet = kDeserializePacket.GetPacket<GS_MAILBOX_ALLSENDITEM_NFY>();
			if (packet == null)
			{
				return;
			}
			TsLog.LogWarning(" GS_MAILBOX_ALLSENDITEM_NFY : Result : {0} , {1} ", new object[]
			{
				packet.i32result,
				packet.i8MailReawrdType
			});
			if (packet.i32result == 0)
			{
				if (packet.i8MailReawrdType == 1)
				{
					NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64Maxmoney;
					TsLog.LogWarning(" ADD Money : {0} , Money {1} ", new object[]
					{
						packet.i64AddMoney,
						packet.i64Maxmoney
					});
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("804");
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						textFromNotify,
						"count",
						packet.i64AddMoney
					});
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
				}
				if (packet.i8MailReawrdType == 2)
				{
					TsLog.LogWarning(" ADD Item {0}", new object[]
					{
						packet.item.m_nItemUnique
					});
					string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.item.m_nItemUnique);
					string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("803");
					string empty2 = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
					{
						textFromNotify2,
						"targetname",
						itemNameByItemUnique,
						"count",
						packet.i32AddItemNum
					});
					Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
				}
				if (0L < packet.SoldierInfo.SolID)
				{
					TsLog.LogWarning(" ADD SOl : SolID {0} , SOlKind {1} ", new object[]
					{
						packet.SoldierInfo.CharKind,
						packet.SoldierInfo.SolID
					});
					NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
					if (charPersonInfo != null)
					{
						SOLDIER_BATTLESKILL_INFO sOLDIER_BATTLESKILL_INFO = new SOLDIER_BATTLESKILL_INFO();
						sOLDIER_BATTLESKILL_INFO.SolID = packet.SoldierInfo.SolID;
						for (int i = 0; i < 6; i++)
						{
							sOLDIER_BATTLESKILL_INFO.BattleSkillData[i].BattleSkillUnique = packet.BattleSkillData[i].BattleSkillUnique;
							sOLDIER_BATTLESKILL_INFO.BattleSkillData[i].BattleSkillLevel = packet.BattleSkillData[i].BattleSkillLevel;
						}
						NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
						readySolList.AddSolInfo(packet.SoldierInfo, sOLDIER_BATTLESKILL_INFO, true);
						for (int i = 0; i < 16; i++)
						{
							NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet.SoldierInfo.SolID);
							if (soldierInfoFromSolID != null)
							{
								soldierInfoFromSolID.SetSolSubData(i, packet.SolSubData[i]);
							}
						}
						NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
						kMyCharInfo.SetCharSolGuide(packet.SoldierInfo.CharKind);
						NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(packet.SoldierInfo.CharKind);
						if (charKindInfo == null)
						{
							return;
						}
						string textFromNotify3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("802");
						string empty3 = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
						{
							textFromNotify3,
							"targetname",
							charKindInfo.GetName()
						});
						Main_UI_SystemMessage.ADDMessage(empty3, SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
						SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
						if (solMilitaryGroupDlg != null)
						{
							solMilitaryGroupDlg.RefreshSolList();
						}
					}
				}
				else
				{
					TsLog.LogWarning(" ADD Sol Error SOlID", new object[0]);
				}
				PostDlg postDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG) as PostDlg;
				if (postDlg != null)
				{
					postDlg.RequestNextRecvList();
				}
			}
		}

		public static void GS_FRIEND_PUSH_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_FRIEND_PUSH_ACK packet = kDeserializePacket.GetPacket<GS_FRIEND_PUSH_ACK>();
			if (packet == null)
			{
				return;
			}
			if (packet.i32Result == -2)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("284"));
			}
			else if (packet.i32Result == -1)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("287"));
			}
		}

		public static void GS_MAILBOX_REPORT_EXPEDITION_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MAILBOX_REPORT_EXPEDITION_ACK packet = kDeserializePacket.GetPacket<GS_MAILBOX_REPORT_EXPEDITION_ACK>();
			if (packet.i32Result == 0)
			{
				Battle_ResultExpeditionDlg battle_ResultExpeditionDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EXPEDITION_BATTLE_RESULT_DLG) as Battle_ResultExpeditionDlg;
				if (battle_ResultExpeditionDlg == null)
				{
					return;
				}
				battle_ResultExpeditionDlg.SetMailID(packet.i64MailID);
				for (int i = 0; i < packet.i32ExpeditionResultCount; i++)
				{
					GS_BATTLE_RESULT_EXPEDITION packet2 = kDeserializePacket.GetPacket<GS_BATTLE_RESULT_EXPEDITION>();
					battle_ResultExpeditionDlg.SetBasicData(packet2);
				}
				for (int j = 0; j < packet.i32SolCount; j++)
				{
					GS_BATTLE_RESULT_SOLDIER packet3 = kDeserializePacket.GetPacket<GS_BATTLE_RESULT_SOLDIER>();
					battle_ResultExpeditionDlg.AddSolData(packet3);
				}
				for (int k = 0; k < packet.i32ItemCount; k++)
				{
					ITEM packet4 = kDeserializePacket.GetPacket<ITEM>();
					battle_ResultExpeditionDlg.AddItemData(packet4);
				}
				battle_ResultExpeditionDlg.Show();
			}
		}

		public static void GS_QUEST_ACCEPT_ACK(NkDeserializePacket kDeserializePacket)
		{
			NrTSingleton<NkQuestManager>.Instance.Request = false;
			GS_QUEST_ACCEPT_ACK packet = kDeserializePacket.GetPacket<GS_QUEST_ACCEPT_ACK>();
			string text = TKString.NEWString(packet.strQuestUnique);
			CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(text);
			if (questByQuestUnique == null)
			{
				return;
			}
			if (packet.Result == 0)
			{
				NrTSingleton<NkQuestManager>.Instance.AcceptQuest(packet.i64QuestID, packet.i32QuestGroupUnique, text, packet.i32Grade, packet.i64Time, packet.i64LastTime);
				NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
				DateTime dateTime = DateTime.Now.ToLocalTime();
				DateTime arg_9C_0 = dateTime;
				DateTime dateTime2 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
				int num = (int)(arg_9C_0 - dateTime2.ToLocalTime()).TotalSeconds;
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("ts", num.ToString());
				dictionary.Add("quest", text.ToString());
				dictionary.Add("step", "start");
				dictionary.Add("level", myCharInfo.GetLevel().ToString());
				dictionary.Add("account_id", myCharInfo.m_SN.ToString());
				GameObject pkGoOminiata = StageSystemCheck.pkGoOminiata;
				if (pkGoOminiata)
				{
					OmniataComponent component = pkGoOminiata.GetComponent<OmniataComponent>();
					if (component)
					{
						component.Track("om_quest", dictionary);
					}
				}
				QuestList_DLG questList_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.QUESTLIST_DLG) as QuestList_DLG;
				if (questList_DLG != null)
				{
					questList_DLG.SetOnGoing();
					questList_DLG.ShowOnGoing();
				}
				RewardInfo kInfo = new RewardInfo();
				bool flag = false;
				for (byte b = 0; b < 3; b += 1)
				{
					if (packet.kGood[(int)b].i32Type == 1)
					{
						NrTSingleton<NkQuestManager>.Instance.SetItem((int)packet.kGood[(int)b].i64Param, (int)packet.kGood[(int)b].i64ParamVal, ref kInfo, b);
						flag = true;
					}
				}
				if (flag)
				{
					NrTSingleton<NkQuestManager>.Instance.PushRewardInfo(kInfo);
					NrTSingleton<NkQuestManager>.Instance.ShowReward();
				}
				RightMenuQuestUI rightMenuQuestUI = (RightMenuQuestUI)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAIN_QUEST);
				if (rightMenuQuestUI != null)
				{
					rightMenuQuestUI.QuestUpdate();
					if (NrTSingleton<NkClientLogic>.Instance.IsNPCTalkState())
					{
						rightMenuQuestUI.Hide();
					}
				}
				if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NPCTALK_DLG))
				{
					if (questByQuestUnique.GetQuestCommon().szTellDoingKey != string.Empty)
					{
						CQuestNpcTell cQuestNpcTell = new CQuestNpcTell();
						cQuestNpcTell.nCharKind = questByQuestUnique.GetQuestCommon().i32QuestCharKind;
						cQuestNpcTell.szTextKey = questByQuestUnique.GetQuestCommon().szTellDoingKey;
						cQuestNpcTell.bTellAll = questByQuestUnique.GetQuestCommon().bTellDoingType;
						NrTSingleton<NkQuestManager>.Instance.EnqueueNpcTell(cQuestNpcTell);
					}
					NpcTalkUI_DLG npcTalkUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NPCTALK_DLG) as NpcTalkUI_DLG;
					if (npcTalkUI_DLG != null)
					{
						npcTalkUI_DLG.AcceptExit();
					}
				}
				else
				{
					CQuestGroup questGroupByGroupUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByGroupUnique(questByQuestUnique.GetQuestGroupUnique());
					if (questGroupByGroupUnique != null && questGroupByGroupUnique.IsFristQuest(text) && questGroupByGroupUnique.GetQuestType() == 1)
					{
						ChapterStart_DLG chapterStart_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.QUEST_CHAPTERSTART) as ChapterStart_DLG;
						if (chapterStart_DLG != null)
						{
							NrTSingleton<FormsManager>.Instance.HideExcept(G_ID.QUEST_CHAPTERSTART);
							chapterStart_DLG.SetInfo(text);
							chapterStart_DLG.Show();
						}
					}
				}
				for (int i = 0; i < 3; i++)
				{
					if (questByQuestUnique.GetQuestCommon().cQuestCondition[i].i32QuestCode == 155)
					{
						MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
						if (msgBoxUI != null)
						{
							string empty = string.Empty;
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
							{
								NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1533"),
								"targetname",
								NrTSingleton<NrCharKindInfoManager>.Instance.GetName((int)questByQuestUnique.GetQuestCommon().cQuestCondition[1].i64Param)
							});
							msgBoxUI.SetMsg(new YesDelegate(NrTSingleton<NkQuestManager>.Instance.OpenQuestBattle), questByQuestUnique, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1532"), empty, eMsgType.MB_OK_CANCEL, 2);
							msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("320"));
							msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("321"));
						}
						break;
					}
				}
				if (!NrTSingleton<NkAdventureManager>.Instance.IsAcceptQuest())
				{
					BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
					if (bookmarkDlg != null)
					{
						bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.ADVENTURE);
					}
					AdventureCollect_DLG adventureCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ADVENTURECOLLECT_DLG) as AdventureCollect_DLG;
					if (adventureCollect_DLG != null)
					{
						adventureCollect_DLG.Update_Notice();
					}
				}
				NrTSingleton<FiveRocksEventManager>.Instance.QuestAccept(text);
				NrTSingleton<NkQuestManager>.Instance.UpdateClientNpc(0);
			}
			else
			{
				if (packet.Result == 2007)
				{
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify(packet.Result.ToString());
					CQuest subCharQuest = NrTSingleton<NkQuestManager>.Instance.GetSubCharQuest();
					if (subCharQuest != null)
					{
						string empty2 = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
						{
							textFromNotify,
							"targetname",
							subCharQuest.GetQuestTitle()
						});
						Main_UI_SystemMessage.ADDMessage(empty2);
					}
				}
				else if (packet.Result != 2080)
				{
					if (packet.Result == 31)
					{
						string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("471");
						Main_UI_SystemMessage.ADDMessage(textFromNotify2);
					}
					else if (packet.Result == 2010)
					{
					}
				}
				NpcTalkUI_DLG npcTalkUI_DLG2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NPCTALK_DLG) as NpcTalkUI_DLG;
				if (npcTalkUI_DLG2 != null)
				{
					npcTalkUI_DLG2.Clear();
					npcTalkUI_DLG2.Close();
				}
			}
		}

		public static void GS_QUEST_COMPLETE_ACK(NkDeserializePacket kDeserializePacket)
		{
			NrTSingleton<NkQuestManager>.Instance.Request = false;
			GS_QUEST_COMPLETE_ACK packet = kDeserializePacket.GetPacket<GS_QUEST_COMPLETE_ACK>();
			NkQuestManager instance = NrTSingleton<NkQuestManager>.Instance;
			if (packet.Result != 0)
			{
				if (packet.Result == 31)
				{
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("472");
					Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				else if (packet.Result == 76)
				{
					string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("818");
					Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
				NpcTalkUI_DLG npcTalkUI_DLG = (NpcTalkUI_DLG)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NPCTALK_DLG);
				if (npcTalkUI_DLG != null)
				{
					npcTalkUI_DLG.Clear();
					npcTalkUI_DLG.Close();
				}
				return;
			}
			if (instance != null)
			{
				string text = TKString.NEWString(packet.strQuestUnique);
				instance.CompleteQuest(text, packet.i32GroupUnique, packet.byCompleteQuest, packet.bCleared, packet.i32Grade, packet.i32CurGrade);
				NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
				DateTime dateTime = DateTime.Now.ToLocalTime();
				DateTime arg_109_0 = dateTime;
				DateTime dateTime2 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
				int num = (int)(arg_109_0 - dateTime2.ToLocalTime()).TotalSeconds;
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("ts", num.ToString());
				dictionary.Add("quest", text.ToString());
				dictionary.Add("step", "finish");
				dictionary.Add("level", myCharInfo.GetLevel().ToString());
				dictionary.Add("account_id", myCharInfo.m_SN.ToString());
				GameObject pkGoOminiata = StageSystemCheck.pkGoOminiata;
				if (pkGoOminiata)
				{
					OmniataComponent component = pkGoOminiata.GetComponent<OmniataComponent>();
					if (component)
					{
						component.Track("om_quest", dictionary);
					}
				}
				CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(text);
				if (questByQuestUnique != null)
				{
					RightMenuQuestUI rightMenuQuestUI = (RightMenuQuestUI)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAIN_QUEST);
					if (rightMenuQuestUI != null)
					{
						rightMenuQuestUI.QuestUpdate();
						rightMenuQuestUI.InitClickTouch();
					}
					QuestList_DLG questList_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.QUESTLIST_DLG) as QuestList_DLG;
					if (questList_DLG != null)
					{
						questList_DLG.SetOnGoing();
						questList_DLG.ShowOnGoing();
					}
					if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.NPCTALK_DLG))
					{
						NrTSingleton<NkQuestManager>.Instance.UpdateClientNpc(0);
					}
					if (packet.i64CurrentMoney > 0L)
					{
						NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64CurrentMoney;
					}
					NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(packet.i16CharUnique) as NrCharUser;
					if (nrCharUser != null && packet.m_ui8IsRemoveSubCharacter == 1)
					{
						nrCharUser.DeleteSubCharAll();
					}
					NrTSingleton<NkQuestManager>.Instance.SetQuestReward(questByQuestUnique, packet.i32CurGrade, packet.i32RewardType);
					if (questByQuestUnique.GetQuestCommon().szTellCompleteKey != string.Empty)
					{
						CQuestNpcTell cQuestNpcTell = new CQuestNpcTell();
						cQuestNpcTell.nCharKind = (int)questByQuestUnique.GetQuestCommon().i64EndTypeVal;
						cQuestNpcTell.szTextKey = questByQuestUnique.GetQuestCommon().szTellCompleteKey;
						cQuestNpcTell.bTellAll = questByQuestUnique.GetQuestCommon().bTellCompleteType;
						NrTSingleton<NkQuestManager>.Instance.EnqueueNpcTell(cQuestNpcTell);
					}
					if (NrTSingleton<NkAdventureManager>.Instance.IsAcceptQuest())
					{
						BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
						if (bookmarkDlg != null)
						{
							bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.ADVENTURE);
						}
						AdventureCollect_DLG adventureCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ADVENTURECOLLECT_DLG) as AdventureCollect_DLG;
						if (adventureCollect_DLG != null)
						{
							adventureCollect_DLG.Update_Notice();
						}
					}
					NrTSingleton<FiveRocksEventManager>.Instance.QuestComplete(text);
					if (NrTSingleton<NkQuestManager>.Instance.IsCompletedQuest("10108_040"))
					{
						NrTSingleton<GameGuideManager>.Instance.CheckGameGuide(GameGuideType.REVIEW);
					}
					MenuIconDlg menuIconDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MENUICON_DLG) as MenuIconDlg;
					if (menuIconDlg != null)
					{
						menuIconDlg.ShowDownButton(true);
					}
				}
				if (packet.bGm == 0 && packet.i32Flag == 0)
				{
					NpcTalkUI_DLG npcTalkUI_DLG2 = (NpcTalkUI_DLG)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NPCTALK_DLG);
					if (npcTalkUI_DLG2 != null)
					{
						npcTalkUI_DLG2.SetStep(E_NPC_TALK_STEP.E_NPC_TALK_STEP_COMPETE);
						NrTSingleton<NkQuestManager>.Instance.ShowReward();
						CQuest nextQuest = NrTSingleton<NkQuestManager>.Instance.GetNextQuest(text);
						if (((nextQuest != null && questByQuestUnique.GetQuestCommon().i32EndType == 0) || (nextQuest != null && questByQuestUnique.GetQuestCommon().i32EndType == 2)) && NrTSingleton<NkQuestManager>.Instance.GetQuestState(nextQuest.GetQuestUnique()) == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE)
						{
							if (!NrTSingleton<NkQuestManager>.Instance.RewardShow)
							{
								npcTalkUI_DLG2.BtnClickNext(null);
							}
						}
					}
					TsAudioManager.Container.RequestAudioClip("UI_SFX", "QUEST", "COMPLETE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				}
			}
		}

		public static void GS_CURRENT_QUEST_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_CURRENT_QUEST_NFY packet = kDeserializePacket.GetPacket<GS_CURRENT_QUEST_NFY>();
			NkQuestManager instance = NrTSingleton<NkQuestManager>.Instance;
			if (instance != null)
			{
				instance.ClearCurrentQuest();
				for (int i = 0; i < packet.i8QuestCount; i++)
				{
					USER_CURRENT_QUEST_INFO_PACKET packet2 = kDeserializePacket.GetPacket<USER_CURRENT_QUEST_INFO_PACKET>();
					instance.AddCurrentQuest(packet2);
				}
				NrTSingleton<NkQuestManager>.Instance.SortCurrentQuest();
			}
		}

		public static void GS_COMPLETED_QUEST_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_COMPLETED_QUEST_NFY packet = kDeserializePacket.GetPacket<GS_COMPLETED_QUEST_NFY>();
			NkQuestManager instance = NrTSingleton<NkQuestManager>.Instance;
			if (instance != null)
			{
				instance.ClearCompleteQuest();
				for (int i = 0; i < packet.i8QuestCount; i++)
				{
					USER_QUEST_COMPLETE_INFO packet2 = kDeserializePacket.GetPacket<USER_QUEST_COMPLETE_INFO>();
					instance.AddCompleteQuest(packet2);
				}
				instance.SetLoadCompletedQuest(true);
			}
			if (0 < packet.nRefresh)
			{
				RightMenuQuestUI rightMenuQuestUI = (RightMenuQuestUI)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAIN_QUEST);
				if (rightMenuQuestUI != null)
				{
					rightMenuQuestUI.QuestUpdate();
				}
				NrTSingleton<NkQuestManager>.Instance.UpdateClientNpc(0);
			}
		}

		public static void GS_UPDATE_COMPLETED_QUEST_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_UPDATE_COMPLETED_QUEST_NFY packet = kDeserializePacket.GetPacket<GS_UPDATE_COMPLETED_QUEST_NFY>();
			NkQuestManager instance = NrTSingleton<NkQuestManager>.Instance;
			if (instance != null)
			{
				instance.SetQuestCompleteInfo(packet.i32QuesGroupUnique, packet.byCompleteQuest);
			}
		}

		public static void GS_QUEST_PARAM_UPDATE_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_QUEST_PARAM_UPDATE_NFY packet = kDeserializePacket.GetPacket<GS_QUEST_PARAM_UPDATE_NFY>();
			QUEST_CONDITION[] array = new QUEST_CONDITION[3];
			for (int i = 0; i < 3; i++)
			{
				array[i] = new QUEST_CONDITION();
			}
			array[0].i64ParamVal = packet.i64ParamVal1;
			array[1].i64ParamVal = packet.i64ParamVal2;
			array[2].i64ParamVal = packet.i64ParamVal3;
			string text = TKString.NEWString(packet.strQuestUnique);
			if (NrTSingleton<NkQuestManager>.Instance != null)
			{
				NrTSingleton<NkQuestManager>.Instance.UpdateQuestParamVal(text, array);
				NrTSingleton<NkQuestManager>.Instance.Update(text, array, packet.i64Unique);
			}
			if (NrTSingleton<NkQuestManager>.Instance.p_deQuestUpdate != null)
			{
				NrTSingleton<NkQuestManager>.Instance.p_deQuestUpdate(text);
			}
			if (NrTSingleton<NkQuestManager>.Instance.GetQuestState(text) == QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE)
			{
				NrTSingleton<NkQuestManager>.Instance.AutoQuestExcute();
			}
		}

		public static void GS_QUEST_CANCLE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_QUEST_CANCLE_ACK packet = kDeserializePacket.GetPacket<GS_QUEST_CANCLE_ACK>();
			NkQuestManager instance = NrTSingleton<NkQuestManager>.Instance;
			if (instance != null)
			{
				string strQuestUnique = TKString.NEWString(packet.strQuestUnique);
				instance.CancleQuest(strQuestUnique);
				NrTSingleton<NkQuestManager>.Instance.UpdateClientNpc(0);
				CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(strQuestUnique);
				if (questByQuestUnique != null)
				{
					string textFromQuest_Title = NrTSingleton<NrTextMgr>.Instance.GetTextFromQuest_Title(questByQuestUnique.GetQuestCommon().strTextKey);
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("4");
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						textFromNotify,
						"targetname",
						textFromQuest_Title
					});
				}
				RightMenuQuestUI rightMenuQuestUI = (RightMenuQuestUI)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAIN_QUEST);
				if (rightMenuQuestUI != null)
				{
					rightMenuQuestUI.QuestUpdate();
					rightMenuQuestUI.RePosition();
				}
				QuestList_DLG questList_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.QUESTLIST_DLG) as QuestList_DLG;
				if (questList_DLG != null)
				{
					questList_DLG.SetOnGoing();
					questList_DLG.ShowOnGoing();
					questList_DLG.ExpandTree();
				}
				if (NrTSingleton<NkAdventureManager>.Instance.IsAcceptQuest())
				{
					BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
					if (bookmarkDlg != null)
					{
						bookmarkDlg.UpdateBookmarkInfo(BookmarkDlg.TYPE.ADVENTURE);
					}
					AdventureCollect_DLG adventureCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ADVENTURECOLLECT_DLG) as AdventureCollect_DLG;
					if (adventureCollect_DLG != null)
					{
						adventureCollect_DLG.Update_Notice();
					}
				}
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "QUEST", "CANCEL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				NrTSingleton<NkCharManager>.Instance.DeleteQuestMonsterEffect();
			}
		}

		public static void GS_QUEST_GROUP_RESET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_QUEST_GROUP_RESET_ACK packet = kDeserializePacket.GetPacket<GS_QUEST_GROUP_RESET_ACK>();
			CQuestGroup questGroupByGroupUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByGroupUnique(packet.i32GroupUnique);
			if (questGroupByGroupUnique == null)
			{
				return;
			}
			NrTSingleton<NkQuestManager>.Instance.SetResetQuestGroup(questGroupByGroupUnique, packet.i32GroupUnique, packet.byReset, packet.i32CurGrade, packet.bCleard);
			NrTSingleton<NkQuestManager>.Instance.UpdateClientNpc(0);
			RightMenuQuestUI rightMenuQuestUI = (RightMenuQuestUI)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAIN_QUEST);
			if (rightMenuQuestUI != null)
			{
				rightMenuQuestUI.QuestUpdate();
			}
			NrTSingleton<NkCharManager>.Instance.DeleteQuestMonsterEffect();
		}

		public static void GS_CHAR_REPUTE_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
		}

		public static void GS_CHAR_REPUTE_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
		}

		public static void GS_CHAR_CHALLENGE_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_CHAR_CHALLENGE_GET_ACK packet = kDeserializePacket.GetPacket<GS_CHAR_CHALLENGE_GET_ACK>();
			int nTotalNum = packet.m_nTotalNum;
			if (0 < nTotalNum)
			{
				NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
				if (kMyCharInfo == null)
				{
					return;
				}
				UserChallengeInfo userChallengeInfo = kMyCharInfo.GetUserChallengeInfo();
				if (userChallengeInfo == null)
				{
					return;
				}
				Challenge_Info[] array = new Challenge_Info[nTotalNum];
				for (int i = 0; i < nTotalNum; i++)
				{
					array[i] = kDeserializePacket.GetPacket<Challenge_Info>();
					userChallengeInfo.SetUserChallengeInfo(array[i]);
				}
				NrTSingleton<ChallengeManager>.Instance.CalcContinueRewardNoticeCount();
				NrTSingleton<ChallengeManager>.Instance.CalcDayRewardNoticeCount();
			}
		}

		public static void GS_CHAR_CHALLENGE_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_CHAR_CHALLENGE_SET_ACK packet = kDeserializePacket.GetPacket<GS_CHAR_CHALLENGE_SET_ACK>();
			if (packet.m_nResult == 0)
			{
				if (0 < packet.m_kChallengeInfo.m_nUnique)
				{
					NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
					if (kMyCharInfo == null)
					{
						return;
					}
					UserChallengeInfo userChallengeInfo = kMyCharInfo.GetUserChallengeInfo();
					if (userChallengeInfo == null)
					{
						return;
					}
					userChallengeInfo.SetUserChallengeInfo(packet.m_kChallengeInfo);
					NrTSingleton<ChallengeManager>.Instance.CalcContinueRewardNoticeCount();
					NrTSingleton<ChallengeManager>.Instance.CalcDayRewardNoticeCount();
					RightMenuQuestUI rightMenuQuestUI = (RightMenuQuestUI)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAIN_QUEST);
					if (rightMenuQuestUI != null)
					{
						rightMenuQuestUI.QuestUpdate();
					}
					if (!string.IsNullOrEmpty(Social.localUser.id))
					{
						short nUnique = packet.m_kChallengeInfo.m_nUnique;
						switch (nUnique)
						{
						case 3170:
							Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(14), 100.0, delegate(bool success)
							{
							});
							break;
						case 3171:
							Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(15), 100.0, delegate(bool success)
							{
							});
							break;
						case 3172:
							Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(16), 100.0, delegate(bool success)
							{
							});
							break;
						default:
							if (nUnique != 3030)
							{
								if (nUnique != 3050)
								{
									if (nUnique != 3060)
									{
										if (nUnique != 3080)
										{
											if (nUnique != 3090)
											{
												if (nUnique != 3120)
												{
													if (nUnique != 3130)
													{
														if (nUnique != 3150)
														{
															if (nUnique != 10010)
															{
																if (nUnique != 10020)
																{
																	if (nUnique != 10040)
																	{
																		if (nUnique != 10050)
																		{
																			if (nUnique == 10060)
																			{
																				Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(21), 100.0, delegate(bool success)
																				{
																				});
																			}
																		}
																		else
																		{
																			Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(20), 100.0, delegate(bool success)
																			{
																			});
																		}
																	}
																	else
																	{
																		Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(19), 100.0, delegate(bool success)
																		{
																		});
																	}
																}
																else
																{
																	Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(18), 100.0, delegate(bool success)
																	{
																	});
																}
															}
															else
															{
																Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(17), 100.0, delegate(bool success)
																{
																});
															}
														}
														else
														{
															int level = kMyCharInfo.GetLevel();
															if (level >= 10)
															{
																Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(1), 100.0, delegate(bool success)
																{
																});
															}
															if (level >= 30)
															{
																Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(2), 100.0, delegate(bool success)
																{
																});
															}
															if (level >= 50)
															{
																Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(3), 100.0, delegate(bool success)
																{
																});
															}
														}
													}
													else if (packet.m_kChallengeInfo.m_nValue == 1L)
													{
														Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(12), 100.0, delegate(bool success)
														{
														});
													}
													else if (packet.m_kChallengeInfo.m_nValue == 10L)
													{
														Social.ReportProgress("CgkIzeOZyPQCEAIQDA", 100.0, delegate(bool success)
														{
														});
													}
													else if (packet.m_kChallengeInfo.m_nValue == 30L)
													{
														Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(13), 100.0, delegate(bool success)
														{
														});
													}
												}
												else if (packet.m_kChallengeInfo.m_nValue == 10L)
												{
													Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(11), 100.0, delegate(bool success)
													{
													});
												}
											}
											else if (packet.m_kChallengeInfo.m_nValue == 5L)
											{
												Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(24), 100.0, delegate(bool success)
												{
												});
											}
										}
										else if (packet.m_kChallengeInfo.m_nValue == 10L)
										{
											Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(22), 100.0, delegate(bool success)
											{
											});
										}
										else if (packet.m_kChallengeInfo.m_nValue == 30L)
										{
											Social.ReportProgress("CgkIzeOZyPQCEAIQDQ", 100.0, delegate(bool success)
											{
											});
										}
										else if (packet.m_kChallengeInfo.m_nValue == 50L)
										{
											Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(23), 100.0, delegate(bool success)
											{
											});
										}
									}
									else if (packet.m_kChallengeInfo.m_nValue == 1L)
									{
										Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(7), 100.0, delegate(bool success)
										{
										});
									}
									else if (packet.m_kChallengeInfo.m_nValue == 10L)
									{
										Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(8), 100.0, delegate(bool success)
										{
										});
									}
									else if (packet.m_kChallengeInfo.m_nValue == 30L)
									{
										Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(9), 100.0, delegate(bool success)
										{
										});
									}
								}
								else
								{
									if (packet.m_kChallengeInfo.m_nValue >= 1L)
									{
										Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(4), 100.0, delegate(bool success)
										{
										});
									}
									if (packet.m_kChallengeInfo.m_nValue >= 20L)
									{
										Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(5), 100.0, delegate(bool success)
										{
										});
									}
									if (packet.m_kChallengeInfo.m_nValue >= 30L)
									{
										Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(6), 100.0, delegate(bool success)
										{
										});
									}
								}
							}
							else if (packet.m_kChallengeInfo.m_nValue == 20L)
							{
								Social.ReportProgress(NrTSingleton<NrAchivementGoogleInfoMAnager>.Instance.GetCode(10), 100.0, delegate(bool success)
								{
								});
							}
							break;
						}
					}
					if (packet.m_nReward == 1)
					{
						ChallengeTable challengeTable = NrTSingleton<ChallengeManager>.Instance.GetChallengeTable(packet.m_kChallengeInfo.m_nUnique);
						if (challengeTable == null)
						{
							return;
						}
						if (packet.m_nIndex >= challengeTable.m_kRewardInfo.Count)
						{
							return;
						}
						ChallengeTable.RewardInfo rewardInfo = challengeTable.m_kRewardInfo[packet.m_nIndex];
						if (rewardInfo == null)
						{
							return;
						}
						if (0L < rewardInfo.m_nMoney)
						{
							string empty = string.Empty;
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
							{
								NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("158"),
								"count",
								rewardInfo.m_nMoney
							});
							Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
						}
						else if (0 < rewardInfo.m_nItemUnique)
						{
							string empty2 = string.Empty;
							if (challengeTable.m_nUnique != 20000)
							{
								NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
								{
									NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("157"),
									"itemname",
									NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(rewardInfo.m_nItemUnique),
									"count",
									rewardInfo.m_nItemNum
								});
								Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
							}
							if (rewardInfo.m_nItemUnique == 70000)
							{
								NrTSingleton<FiveRocksEventManager>.Instance.HeartsInflow(eHEARTS_INFLOW.CHALLENGE_REWARD, (long)rewardInfo.m_nItemNum);
							}
						}
						NrTSingleton<ChallengeManager>.Instance.DeleteNotice(packet.m_kChallengeInfo.m_nUnique);
						if (challengeTable.m_nType == 1)
						{
							kMyCharInfo.SetCharDetail(12, packet.m_nDetailValue);
						}
						if (challengeTable.m_nUnique == 20000)
						{
							string empty3 = string.Empty;
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty3, new object[]
							{
								NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("622"),
								"itemname",
								NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(rewardInfo.m_nItemUnique),
								"count",
								rewardInfo.m_nItemNum
							});
							Main_UI_SystemMessage.ADDMessage(empty3);
						}
					}
					else
					{
						ChallengeTable challengeTable2 = NrTSingleton<ChallengeManager>.Instance.GetChallengeTable(packet.m_kChallengeInfo.m_nUnique);
						if (challengeTable2 == null)
						{
							return;
						}
						Challenge_Info userChallengeInfo2 = userChallengeInfo.GetUserChallengeInfo(packet.m_kChallengeInfo.m_nUnique);
						if (userChallengeInfo2 == null)
						{
							return;
						}
						short num = 64;
						short num2 = 0;
						while ((int)num2 < challengeTable2.m_kRewardInfo.Count)
						{
							if ((int)challengeTable2.m_nLevel <= kMyCharInfo.GetLevel())
							{
								if (challengeTable2.m_nType != 1 && challengeTable2.m_nType != 2)
								{
									bool flag = false;
									if (num2 < num)
									{
										long num3 = 1L << (int)(num2 & 31);
										if ((userChallengeInfo2.m_bGetReward1 & num3) == 0L)
										{
											flag = true;
										}
									}
									else
									{
										long num4 = 1L << (int)(num2 - num & 31);
										if ((userChallengeInfo2.m_bGetReward1 & num4) == 0L)
										{
											flag = true;
										}
									}
									if (userChallengeInfo2.m_nValue >= (long)challengeTable2.m_kRewardInfo[(int)num2].m_nConditionCount && flag && challengeTable2.m_nUnique != 20000)
									{
										NrTSingleton<ChallengeManager>.Instance.InsertNotice(packet.m_kChallengeInfo.m_nUnique, num2);
										break;
									}
								}
							}
							num2 += 1;
						}
						if (packet.m_kChallengeInfo.m_nUnique == 4010)
						{
							Application.OpenURL("https://docs.google.com/forms/d/1iJyYQGdyW1BCmvcyE6130-9vmCyCpZ9cueQorYL2bfk/viewform");
						}
						else if (packet.m_kChallengeInfo.m_nUnique == 3140)
						{
							Application.OpenURL(TsPlatform.GetAppURL());
						}
						else if (NrTSingleton<ChallengeManager>.Instance.isEquipChallenge(packet.m_kChallengeInfo.m_nUnique))
						{
							ChallengeBundleDlg challengeBundleDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CHALLENGE_BUNDLE_DLG) as ChallengeBundleDlg;
							if (challengeBundleDlg != null)
							{
								challengeBundleDlg.SetData((ChallengeManager.eCHALLENGECODE)packet.m_kChallengeInfo.m_nUnique);
							}
						}
						else if (packet.m_kChallengeInfo.m_nUnique == 20000)
						{
						}
					}
					ChallengeDlg challengeDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHALLENGE_DLG) as ChallengeDlg;
					if (challengeDlg != null)
					{
						challengeDlg.SetChallengeInfo();
					}
					TimeShopInfo_DLG timeShopInfo_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TIMESHOP_INFO_DLG) as TimeShopInfo_DLG;
					if (timeShopInfo_DLG != null)
					{
						timeShopInfo_DLG.Set_ChallengeInfo();
					}
					TimeShop_DLG timeShop_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.TIMESHOP_DLG) as TimeShop_DLG;
					if (timeShop_DLG != null)
					{
						timeShop_DLG.Set_RewardButton();
					}
				}
			}
			else if (packet.m_nResult == -1)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}

		public static void GS_SOLCOMBINATION_SYNC_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLCOMBINATION_SYNC_ACK packet = kDeserializePacket.GetPacket<GS_SOLCOMBINATION_SYNC_ACK>();
			PlunderSolNumDlg plunderSolNumDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLNUM_DLG) as PlunderSolNumDlg;
			if (plunderSolNumDlg == null)
			{
				PlunderSolNumDlg._syncSolCombinationUniqueKey = packet.i32SolCombinationUnique;
				Debug.Log("NORMAL, NrReceiveGame_SolCombination.cs, GS_SOLCOMBINATION_SYNC_ACK(), PlunderSolNumDlg is Null");
				return;
			}
			plunderSolNumDlg.RenewCompleteCombinationLabel(packet.i32SolCombinationUnique, 0);
		}

		public static void GS_SOLDIER_LOAD_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLDIER_LOAD_GET_ACK packet = kDeserializePacket.GetPacket<GS_SOLDIER_LOAD_GET_ACK>();
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.AddSolWarehouseInfo(packet);
		}

		public static void GS_SOLDIER_WAREHOUSE_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
		}

		public static void GS_SOLDIER_WAREHOUSE_MOVE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLDIER_WAREHOUSE_MOVE_ACK packet = kDeserializePacket.GetPacket<GS_SOLDIER_WAREHOUSE_MOVE_ACK>();
			if (packet.i32Result == 0)
			{
				if (packet.ui8SolPosType == 0)
				{
					NkSoldierInfo nkSoldierInfo = null;
					NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
					if (charPersonInfo != null)
					{
						NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.RemoveSolWarehouse(packet.SoldierInfo.SolID);
						SOLDIER_BATTLESKILL_INFO sOLDIER_BATTLESKILL_INFO = new SOLDIER_BATTLESKILL_INFO();
						sOLDIER_BATTLESKILL_INFO.SolID = packet.SoldierInfo.SolID;
						for (int i = 0; i < 6; i++)
						{
							sOLDIER_BATTLESKILL_INFO.BattleSkillData[i].BattleSkillUnique = packet.BattleSkillData[i].BattleSkillUnique;
							sOLDIER_BATTLESKILL_INFO.BattleSkillData[i].BattleSkillLevel = packet.BattleSkillData[i].BattleSkillLevel;
						}
						NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
						readySolList.AddSolInfo(packet.SoldierInfo, sOLDIER_BATTLESKILL_INFO, true);
						nkSoldierInfo = charPersonInfo.GetSoldierInfoFromSolID(packet.SoldierInfo.SolID);
						for (int i = 0; i < 16; i++)
						{
							if (nkSoldierInfo != null)
							{
								nkSoldierInfo.SetSolSubData(i, packet.SolSubData[i]);
							}
						}
						for (int i = 0; i < 6; i++)
						{
							if (0L < packet.EquipItem[i].m_nItemID)
							{
								nkSoldierInfo.SetItem(packet.EquipItem[i]);
							}
						}
						nkSoldierInfo.SetReceivedEquipItem(true);
						nkSoldierInfo.UpdateSoldierStatInfo();
					}
					if (nkSoldierInfo != null)
					{
						string empty = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("589"),
							"targetname",
							nkSoldierInfo.GetName()
						});
						Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
					}
				}
				else if (packet.ui8SolPosType == 5)
				{
					NkReadySolList readySolList2 = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
					if (readySolList2 != null)
					{
						NkSoldierInfo solInfo = readySolList2.GetSolInfo(packet.SoldierInfo.SolID);
						if (solInfo != null)
						{
							NrTSingleton<ExplorationManager>.Instance.RemoveSolInfo(packet.SoldierInfo.SolID);
							readySolList2.DelSol(packet.SoldierInfo.SolID);
							NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.AddSolWarehouseInfo(solInfo);
							string empty2 = string.Empty;
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
							{
								NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("588"),
								"targetname",
								solInfo.GetName()
							});
							Main_UI_SystemMessage.ADDMessage(empty2, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
						}
					}
				}
				SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
				if (solMilitaryGroupDlg != null)
				{
					solMilitaryGroupDlg.RefreshSolWarehouse();
				}
			}
		}

		public static void GS_SOLDIER_WAREHOUSE_ADD_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLDIER_WAREHOUSE_ADD_ACK packet = kDeserializePacket.GetPacket<GS_SOLDIER_WAREHOUSE_ADD_ACK>();
			if (packet.i32Result == 0)
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.SetCharSubData(packet.i32SubDataType, packet.i64SubDataValue);
				SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
				if (solMilitaryGroupDlg != null)
				{
					solMilitaryGroupDlg.RefreshSolWarehouseAdd();
				}
			}
		}

		public static void GS_SOLDIER_REINCARNATION_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLDIER_REINCARNATION_SET_ACK packet = kDeserializePacket.GetPacket<GS_SOLDIER_REINCARNATION_SET_ACK>();
			if (packet.i32Result == 0)
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64CurMoney;
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.SetCharSubData(packet.i32CharSubDataType, packet.i64CharSubDataValue);
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (charPersonInfo == null)
				{
					return;
				}
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet.i64SolID);
				if (soldierInfoFromSolID == null)
				{
					return;
				}
				for (int i = 0; i < 6; i++)
				{
					soldierInfoFromSolID.SetBattleSkillData(i, packet.i32SkillUnique[i], (int)packet.i16SkillLevel[i]);
				}
				soldierInfoFromSolID.SetExp(packet.i64Exp);
				soldierInfoFromSolID.SetGrade(packet.i8Grade);
				soldierInfoFromSolID.SetLevel(packet.i16Level);
				soldierInfoFromSolID.SetSolSubData(packet.i32SolSubDataType1, packet.i64SolSubDataValue1);
				soldierInfoFromSolID.SetCharKind(packet.i32CharKind);
				int iBaseCharKind = 0;
				if (soldierInfoFromSolID.IsLeader())
				{
					NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
					if (nrCharUser != null)
					{
						iBaseCharKind = nrCharUser.GetCharKind();
						int i32CharKind = packet.i32CharKind;
						if (0 >= nrCharUser.GetFaceCharKind() && 0 < i32CharKind && i32CharKind != nrCharUser.GetCharKind())
						{
							nrCharUser.SetClassChage(i32CharKind);
						}
						else
						{
							nrCharUser.SetCharKind(i32CharKind, false);
						}
					}
				}
				SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
				if (solMilitaryGroupDlg != null)
				{
					solMilitaryGroupDlg.RefereshSelectSolInfo(soldierInfoFromSolID);
					solMilitaryGroupDlg.SetShowReincarnation(false);
				}
				if (0 < packet.i8ResultEffect)
				{
					ReincarnationSuccessDlg reincarnationSuccessDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.REINCARNATION_SUCCESS_DLG) as ReincarnationSuccessDlg;
					if (reincarnationSuccessDlg != null)
					{
						reincarnationSuccessDlg.SetResult(iBaseCharKind, packet);
					}
				}
			}
		}

		public static void GS_TICKET_SELL_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_TICKET_SELL_INFO_ACK packet = kDeserializePacket.GetPacket<GS_TICKET_SELL_INFO_ACK>();
			SolRecruitDlg solRecruitDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLRECRUIT_DLG) as SolRecruitDlg;
			if (solRecruitDlg != null)
			{
				solRecruitDlg.SetTicketSellInfo(packet, kDeserializePacket);
			}
		}

		public static void GS_SOLAWAKENING_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLAWAKENING_INFO_ACK packet = kDeserializePacket.GetPacket<GS_SOLAWAKENING_INFO_ACK>();
			SolAwakeningDlg solAwakeningDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLAWAKENING_DLG) as SolAwakeningDlg;
			if (solAwakeningDlg != null)
			{
				solAwakeningDlg.SetAwakeningInfo(packet, kDeserializePacket);
			}
		}

		public static void GS_SOLAWAKENING_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLAWAKENING_ACK packet = kDeserializePacket.GetPacket<GS_SOLAWAKENING_ACK>();
			TsLog.LogOnlyEditor("GS_SOLAWAKENING_ACK : " + packet.i32Result.ToString());
		}

		public static void GS_SOLAWAKENING_STAT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLAWAKENING_STAT_ACK packet = kDeserializePacket.GetPacket<GS_SOLAWAKENING_STAT_ACK>();
			if (packet.i32Result == 0)
			{
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				NrSoldierList soldierList = charPersonInfo.GetSoldierList();
				NkSoldierInfo nkSoldierInfo = soldierList.GetSoldierInfoBySolID(packet.i64SolID);
				if (nkSoldierInfo == null)
				{
					nkSoldierInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySoldierInfoBySolID(packet.i64SolID);
				}
				if (nkSoldierInfo != null)
				{
					for (int i = 0; i < 3; i++)
					{
						if (packet.i32SolSubDataType[i] > 0)
						{
							nkSoldierInfo.SetSolSubData(packet.i32SolSubDataType[i], packet.i64SolSubDataValue[i]);
						}
					}
				}
				SolAwakeningDlg solAwakeningDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLAWAKENING_DLG) as SolAwakeningDlg;
				if (solAwakeningDlg != null)
				{
					solAwakeningDlg.SetAwakeningStat(packet);
				}
			}
		}

		public static void GS_SOLAWAKENING_RESET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLAWAKENING_RESET_ACK packet = kDeserializePacket.GetPacket<GS_SOLAWAKENING_RESET_ACK>();
			if (packet.i32Result == 0)
			{
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				NrSoldierList soldierList = charPersonInfo.GetSoldierList();
				NkSoldierInfo nkSoldierInfo = soldierList.GetSoldierInfoBySolID(packet.i64SolID);
				if (nkSoldierInfo == null)
				{
					nkSoldierInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySoldierInfoBySolID(packet.i64SolID);
				}
				if (nkSoldierInfo != null)
				{
					nkSoldierInfo.SetSolSubData(packet.i32SolSubDataType, packet.i64SolSubDataValue);
				}
				SolAwakeningDlg solAwakeningDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLAWAKENING_DLG) as SolAwakeningDlg;
				if (solAwakeningDlg != null)
				{
					solAwakeningDlg.SetAwakeningReset(packet);
				}
			}
		}

		public static void GS_SOLGUIDE_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLGUIDE_INFO_ACK packet = kDeserializePacket.GetPacket<GS_SOLGUIDE_INFO_ACK>();
			if (packet.bElementMark)
			{
				Myth_Evolution_Main_DLG myth_Evolution_Main_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTH_EVOLUTION_MAIN_DLG) as Myth_Evolution_Main_DLG;
				if (myth_Evolution_Main_DLG != null)
				{
					myth_Evolution_Main_DLG.ClearMythSolInfo();
					for (int i = 0; i < (int)packet.i16Count; i++)
					{
						SOLGUIDE_DATA packet2 = kDeserializePacket.GetPacket<SOLGUIDE_DATA>();
						myth_Evolution_Main_DLG.AddMythSolInfo(packet2);
					}
					myth_Evolution_Main_DLG.SetLegend();
				}
			}
			else
			{
				SolGuide_Dlg solGuide_Dlg = (SolGuide_Dlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLGUIDE_DLG);
				if (solGuide_Dlg != null)
				{
					solGuide_Dlg.ClearSolGuideInfo();
					for (int j = 0; j < (int)packet.i16Count; j++)
					{
						SOLGUIDE_DATA packet3 = kDeserializePacket.GetPacket<SOLGUIDE_DATA>();
						solGuide_Dlg.AddSolGuideInfo(packet3);
					}
					solGuide_Dlg.SetGuideGuiSet(packet.bElementMark);
					solGuide_Dlg.Show();
					if (packet.i32CharKind > 0 && !NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLDETAIL_DLG))
					{
						SolDetail_Info_Dlg solDetail_Info_Dlg = (SolDetail_Info_Dlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLDETAIL_DLG);
						if (!solDetail_Info_Dlg.Visible)
						{
							solDetail_Info_Dlg.Show();
						}
						SolSlotData solGuideData = solGuide_Dlg.GetSolGuideData(packet.i32CharKind);
						if (solGuideData != null && solGuideData.i32KindInfo != 0)
						{
							solDetail_Info_Dlg.SetSolKind(solGuideData);
						}
					}
				}
			}
		}

		public static void GS_SOLSUBDATA_COMMONFLAG_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLSUBDATA_COMMONFLAG_ACK packet = kDeserializePacket.GetPacket<GS_SOLSUBDATA_COMMONFLAG_ACK>();
			if (packet.i32Result == 0)
			{
				NkSoldierInfo soldierInfoBySolID = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetSoldierInfoBySolID(packet.i64SolID);
				if (soldierInfoBySolID != null)
				{
					soldierInfoBySolID.SetSolSubData(13, packet.i64SubDataValue);
					if (soldierInfoBySolID.IsAtbCommonFlag(1L))
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("722"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					}
					else
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("723"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					}
					SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLDETAILINFO_DLG) as SolMilitaryGroupDlg;
					if (solMilitaryGroupDlg != null)
					{
						solMilitaryGroupDlg.RefreshSolInfo(soldierInfoBySolID);
					}
				}
				SolDetailinfoDlg solDetailinfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLDETAILINFO_DLG) as SolDetailinfoDlg;
				if (solDetailinfoDlg != null)
				{
					solDetailinfoDlg.SetSolder();
				}
			}
		}

		public static void GS_PREVIEW_HERO_START_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_PREVIEW_HERO_START_ACK packet = kDeserializePacket.GetPacket<GS_PREVIEW_HERO_START_ACK>();
			if (packet.i32Result == 0)
			{
				if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.SOLGUIDE_DLG))
				{
					SolGuide_Dlg solGuide_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLGUIDE_DLG) as SolGuide_Dlg;
					if (solGuide_Dlg != null)
					{
						solGuide_Dlg.ChangeSceneDestory = false;
						solGuide_Dlg.Hide();
					}
				}
				else if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.SOLMILITARYGROUP_DLG))
				{
					SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
					if (solMilitaryGroupDlg != null)
					{
						solMilitaryGroupDlg.ChangeSceneDestory = false;
						solMilitaryGroupDlg.Hide();
					}
				}
				else if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.MYTH_EVOLUTION_MAIN_DLG))
				{
					Myth_Evolution_Main_DLG myth_Evolution_Main_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYTH_EVOLUTION_MAIN_DLG) as Myth_Evolution_Main_DLG;
					if (myth_Evolution_Main_DLG != null)
					{
						myth_Evolution_Main_DLG.ChangeSceneDestory = false;
						myth_Evolution_Main_DLG.Hide();
					}
				}
			}
		}

		public static void GS_COSTUME_BUY_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_COSTUME_BUY_ACK packet = kDeserializePacket.GetPacket<GS_COSTUME_BUY_ACK>();
			if (packet.Result == -1000)
			{
				string empty = string.Empty;
				int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_COSTUME_PRICE_ITEM_UNIQUE_1);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("198"),
					"targetname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(value)
				});
				Main_UI_SystemMessage.ADDMessage(empty);
			}
			else if (packet.Result == -1100)
			{
				string empty2 = string.Empty;
				int value2 = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_COSTUME_PRICE_ITEM_UNIQUE_2);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("198"),
					"targetname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(value2)
				});
				Main_UI_SystemMessage.ADDMessage(empty2);
			}
			if (packet.Result != 0)
			{
				return;
			}
			NrTSingleton<CostumeBuyManager>.Instance.BuyCostumeEnd(packet);
		}

		public static void GS_MYTH_EVOLUTION_SOL_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_MYTH_EVOLUTION_SOL_ACK packet = kDeserializePacket.GetPacket<GS_MYTH_EVOLUTION_SOL_ACK>();
			TsLog.LogWarning("!!!!!!!!!!!!!!!!! GS_MYTH_EVOLUTION_SOL_ACK Result : {0}", new object[]
			{
				packet.i32Result
			});
			if (packet.i32Result == 0)
			{
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				NrSoldierList soldierList = charPersonInfo.GetSoldierList();
				NkSoldierInfo nkSoldierInfo = soldierList.GetSoldierInfoBySolID(packet.i64SolID);
				if (nkSoldierInfo == null)
				{
					nkSoldierInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySoldierInfoBySolID(packet.i64SolID);
				}
				if (nkSoldierInfo != null)
				{
					byte grade = nkSoldierInfo.GetGrade();
					nkSoldierInfo.SetGrade(packet.i8Grade);
					for (int i = 0; i < 2; i++)
					{
						nkSoldierInfo.SetBattleSkillData(packet.i32BattleSkillIndex[i], packet.i32BattleSkillUnique[i], packet.i32BattleSkillLevel[i]);
					}
					nkSoldierInfo.UpdateSoldierStatInfo();
					Myth_Evolution_Success_DLG myth_Evolution_Success_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTH_EVOLUTION_SUCCESS_DLG) as Myth_Evolution_Success_DLG;
					if (myth_Evolution_Success_DLG != null)
					{
						myth_Evolution_Success_DLG.LoadMyth_Evolurion(nkSoldierInfo, grade + 1, packet.i8Grade + 1);
					}
				}
			}
			else
			{
				string text = string.Empty;
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270") + " ( " + packet.i32Result.ToString() + ")";
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("270"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			}
		}

		public static void GS_GMCOMMAND_MYTHSKILL_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_GMCOMMAND_MYTHSKILL_NFY packet = kDeserializePacket.GetPacket<GS_GMCOMMAND_MYTHSKILL_NFY>();
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			NrSoldierList soldierList = charPersonInfo.GetSoldierList();
			NkSoldierInfo nkSoldierInfo = soldierList.GetSoldierInfoBySolID(packet.i64BaseSOLID);
			if (nkSoldierInfo == null)
			{
				nkSoldierInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySoldierInfoBySolID(packet.i64BaseSOLID);
			}
			if (nkSoldierInfo != null)
			{
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(nkSoldierInfo.GetCharKind());
				if (charKindInfo == null)
				{
					return;
				}
				if (packet.i32BattleSkillUnique == 0)
				{
					for (int i = 1; i < 6; i++)
					{
						int battleSkillUnique = charKindInfo.GetBattleSkillUnique(i);
						int battleSkillLevelByIndex = charKindInfo.GetBattleSkillLevelByIndex(i);
						if (packet.i32BattleSkillUnique > 0)
						{
							nkSoldierInfo.SetBattleSkillData(i, battleSkillUnique, battleSkillLevelByIndex);
						}
						else
						{
							nkSoldierInfo.SetBattleSkillData(i, 0, 0);
						}
					}
				}
				else
				{
					for (int j = 1; j < 6; j++)
					{
						int battleSkillUnique = charKindInfo.GetBattleSkillUnique(j);
						if (packet.i32BattleSkillUnique == battleSkillUnique)
						{
							nkSoldierInfo.SetBattleSkillData(j, packet.i32BattleSkillUnique, packet.i32BattleSkillLevel);
						}
						else
						{
							nkSoldierInfo.SetBattleSkillData(j, 0, 0);
						}
					}
				}
			}
		}

		public static void GS_STORYCHAT_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_STORYCHAT_GET_ACK packet = kDeserializePacket.GetPacket<GS_STORYCHAT_GET_ACK>();
			int nTotalNum = packet.nTotalNum;
			if (0 < nTotalNum)
			{
				StoryChat_Info[] array = new StoryChat_Info[nTotalNum];
				for (int i = 0; i < nTotalNum; i++)
				{
					array[i] = kDeserializePacket.GetPacket<StoryChat_Info>();
				}
				StoryChatDlg storyChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHAT_DLG) as StoryChatDlg;
				if (storyChatDlg != null)
				{
					if (packet.nType == 3)
					{
						storyChatDlg.SetBattleStoryChatList(array);
						string key = "ReplayStoryChatID";
						if (!PlayerPrefs.HasKey(key))
						{
							PlayerPrefs.SetString(key, array[0].nStoryChatID.ToString());
						}
						else
						{
							string @string = PlayerPrefs.GetString(key);
							long num = long.Parse(@string);
							if (array[0].nStoryChatID > num)
							{
								PlayerPrefs.SetString(key, array[0].nStoryChatID.ToString());
							}
						}
					}
					else
					{
						storyChatDlg.SetStoryChatList((int)packet.nType, array);
						if (packet.nType == 0)
						{
							NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
							if (charPersonInfo == null)
							{
								return;
							}
							string charName = charPersonInfo.GetCharName();
							string key2 = charName + "NewStoryChatID";
							if (!PlayerPrefs.HasKey(key2))
							{
								PlayerPrefs.SetString(key2, array[0].nStoryChatID.ToString());
							}
							else
							{
								string string2 = PlayerPrefs.GetString(key2);
								long num2 = long.Parse(string2);
								if (array[0].nStoryChatID > num2)
								{
									PlayerPrefs.SetString(key2, array[0].nStoryChatID.ToString());
								}
							}
						}
						else if (packet.nType == 1)
						{
							NrPersonInfoUser charPersonInfo2 = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
							if (charPersonInfo2 == null)
							{
								return;
							}
							string key3 = charPersonInfo2.GetCharName() + NrTSingleton<NewGuildManager>.Instance.GetGuildName();
							if (!PlayerPrefs.HasKey(key3))
							{
								PlayerPrefs.SetString(key3, array[0].nStoryChatID.ToString());
							}
							else
							{
								string string3 = PlayerPrefs.GetString(key3);
								long num3 = long.Parse(string3);
								if (array[0].nStoryChatID > num3)
								{
									PlayerPrefs.SetString(key3, array[0].nStoryChatID.ToString());
								}
							}
						}
					}
				}
			}
			else
			{
				StoryChatDlg storyChatDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHAT_DLG) as StoryChatDlg;
				if (storyChatDlg2 != null)
				{
					storyChatDlg2.InitStoryChatList((int)packet.nType);
				}
			}
		}

		public static void GS_STORYCHAT_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_STORYCHAT_SET_ACK packet = kDeserializePacket.GetPacket<GS_STORYCHAT_SET_ACK>();
			if (-1L < packet.nNewLastCommentID)
			{
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (charPersonInfo == null)
				{
					return;
				}
				string charName = charPersonInfo.GetCharName();
				PlayerPrefs.SetString(charName + "NewCommentID", packet.nNewLastCommentID.ToString());
				StoryChatDlg storyChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHAT_DLG) as StoryChatDlg;
				if (storyChatDlg != null)
				{
					storyChatDlg.m_nCurrentPage = 1;
				}
				GS_STORYCHAT_GET_REQ gS_STORYCHAT_GET_REQ = new GS_STORYCHAT_GET_REQ();
				gS_STORYCHAT_GET_REQ.nPersonID = NrTSingleton<NkCharManager>.Instance.GetChar(1).GetPersonID();
				gS_STORYCHAT_GET_REQ.nType = (byte)packet.m_nType;
				gS_STORYCHAT_GET_REQ.nPage = 1;
				gS_STORYCHAT_GET_REQ.nPageSize = 4;
				gS_STORYCHAT_GET_REQ.nFirstStoryChatID = 0L;
				gS_STORYCHAT_GET_REQ.nLastStoryChatID = 0L;
				gS_STORYCHAT_GET_REQ.bNextRequest = 0;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_STORYCHAT_GET_REQ, gS_STORYCHAT_GET_REQ);
			}
			else if (0L < packet.m_nStoryChatID)
			{
				byte nType = 0;
				StoryChatDlg storyChatDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHAT_DLG) as StoryChatDlg;
				if (storyChatDlg2 != null)
				{
					nType = (byte)storyChatDlg2.m_nCurrentTabInex;
				}
				GS_STORYCHAT_GET_REQ gS_STORYCHAT_GET_REQ2 = new GS_STORYCHAT_GET_REQ();
				gS_STORYCHAT_GET_REQ2.nPersonID = NrTSingleton<NkCharManager>.Instance.GetChar(1).GetPersonID();
				gS_STORYCHAT_GET_REQ2.nType = nType;
				gS_STORYCHAT_GET_REQ2.nPage = 1;
				gS_STORYCHAT_GET_REQ2.nPageSize = 4;
				gS_STORYCHAT_GET_REQ2.nFirstStoryChatID = 0L;
				gS_STORYCHAT_GET_REQ2.nLastStoryChatID = 0L;
				gS_STORYCHAT_GET_REQ2.bNextRequest = 0;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_STORYCHAT_GET_REQ, gS_STORYCHAT_GET_REQ2);
			}
		}

		public static void GS_STORYCOMMENT_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_STORYCOMMENT_GET_ACK packet = kDeserializePacket.GetPacket<GS_STORYCOMMENT_GET_ACK>();
			int nTotalNum = packet.nTotalNum;
			if (0 < nTotalNum)
			{
				StoryComment_Info[] array = new StoryComment_Info[nTotalNum];
				for (int i = 0; i < nTotalNum; i++)
				{
					array[i] = kDeserializePacket.GetPacket<StoryComment_Info>();
				}
				StoryChatDetailDlg storyChatDetailDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHATDETAIL_DLG) as StoryChatDetailDlg;
				if (storyChatDetailDlg != null)
				{
					storyChatDetailDlg.SetStoryCommentList(packet.bCanLike, array);
				}
			}
			else
			{
				StoryChatDetailDlg storyChatDetailDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHATDETAIL_DLG) as StoryChatDetailDlg;
				if (storyChatDetailDlg2 != null)
				{
					storyChatDetailDlg2.SetStoryCommentList(packet.bCanLike, null);
				}
			}
		}

		public static void GS_STORYCOMMENT_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_STORYCOMMENT_SET_ACK packet = kDeserializePacket.GetPacket<GS_STORYCOMMENT_SET_ACK>();
			if (-1L < packet.nNewLastCommentID)
			{
				StoryChatDlg storyChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHAT_DLG) as StoryChatDlg;
				if (storyChatDlg != null)
				{
					storyChatDlg.UpdateCommentNumText(true);
				}
				StoryChatDetailDlg storyChatDetailDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHATDETAIL_DLG) as StoryChatDetailDlg;
				if (storyChatDetailDlg != null)
				{
					storyChatDetailDlg.UpdateCommentNumText(true);
					storyChatDetailDlg.SetCommentList(packet.nNewLastCommentID);
				}
			}
			else if (0L < packet.m_nStoryCommentID)
			{
				StoryChatDlg storyChatDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHAT_DLG) as StoryChatDlg;
				if (storyChatDlg2 != null)
				{
					storyChatDlg2.UpdateCommentNumText(false);
				}
				StoryChatDetailDlg storyChatDetailDlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHATDETAIL_DLG) as StoryChatDetailDlg;
				if (storyChatDetailDlg2 != null)
				{
					storyChatDetailDlg2.UpdateCommentNumText(false);
				}
			}
		}

		public static void GS_STORYCHATLIKE_GET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_STORYCHATLIKE_GET_ACK packet = kDeserializePacket.GetPacket<GS_STORYCHATLIKE_GET_ACK>();
			int nTotalNum = packet.nTotalNum;
			StoryChatLike_Info[] array = new StoryChatLike_Info[nTotalNum];
			for (int i = 0; i < nTotalNum; i++)
			{
				array[i] = kDeserializePacket.GetPacket<StoryChatLike_Info>();
			}
			StoryChatLikeListDlg storyChatLikeListDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.STORYCHAT_LIKELIST_DLG) as StoryChatLikeListDlg;
			if (storyChatLikeListDlg != null)
			{
				storyChatLikeListDlg.SetStoryChatLikeList(array);
			}
		}

		public static void GS_STORYCHATLIKE_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_STORYCHATLIKE_SET_ACK packet = kDeserializePacket.GetPacket<GS_STORYCHATLIKE_SET_ACK>();
			if (packet.nResult == 0)
			{
				StoryChatDlg storyChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHAT_DLG) as StoryChatDlg;
				if (storyChatDlg != null)
				{
					storyChatDlg.UpdateLikeNumText();
				}
				StoryChatDetailDlg storyChatDetailDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHATDETAIL_DLG) as StoryChatDetailDlg;
				if (storyChatDetailDlg != null)
				{
					storyChatDetailDlg.UpdateLikeNumText();
				}
			}
		}

		public static void GS_STORYCOMMENT_NEWCOUNT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_STORYCOMMENT_NEWCOUNT_ACK packet = kDeserializePacket.GetPacket<GS_STORYCOMMENT_NEWCOUNT_ACK>();
			if (NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1) == null)
			{
				return;
			}
			StoryChatDlg storyChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHAT_DLG) as StoryChatDlg;
			if (storyChatDlg != null)
			{
				storyChatDlg.SetTabNoticeCount(packet.nFriendChatCount, packet.nFriendChatID, packet.nGuildChatCount, packet.nGuildChatID, packet.nReplayChatCount, packet.nReplayChatID);
			}
		}

		public static void GS_NEWSTORYCOMMENT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_NEWSTORYCOMMENT_ACK packet = kDeserializePacket.GetPacket<GS_NEWSTORYCOMMENT_ACK>();
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo == null)
			{
				return;
			}
			string charName = charPersonInfo.GetCharName();
			if (-1L < packet.nNewLastStoryCommentID)
			{
				if (!PlayerPrefs.HasKey(charName + "NewCommentID"))
				{
					PlayerPrefs.SetString(charName + "NewCommentID", packet.nNewLastStoryCommentID.ToString());
				}
				else
				{
					long num = long.Parse(PlayerPrefs.GetString(charName + "NewCommentID"));
					if (packet.nNewLastStoryCommentID > num)
					{
						NrTSingleton<UIDataManager>.Instance.NoticeStoryChat = true;
						MenuIconDlg menuIconDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MENUICON_DLG) as MenuIconDlg;
						if (menuIconDlg != null)
						{
							menuIconDlg.ShowNotice();
						}
						PlayerPrefs.SetString(charName + "NewCommentID", packet.nNewLastStoryCommentID.ToString());
					}
				}
			}
		}

		public static void GS_LOAD_CHAR_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_LOAD_CHAR_NFY packet = kDeserializePacket.GetPacket<GS_LOAD_CHAR_NFY>();
			NEW_MAKECHAR_INFO packet2 = kDeserializePacket.GetPacket<NEW_MAKECHAR_INFO>();
			PublicMethod.SERVERTIME = packet.nServerTime;
			PublicMethod.CLIENTTIME = Time.realtimeSinceStartup;
			NrTSingleton<NkClientLogic>.Instance.SetGameWorld(true);
			NrTSingleton<NkCharManager>.Instance.SetMyChar(packet);
			NrTSingleton<NkCharManager>.Instance.SetChar(packet2, false, false);
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser != null)
			{
				NkCharIDInfo iDInfo = nrCharUser.GetIDInfo();
				if (iDInfo != null)
				{
					iDInfo.m_nWorldID = (int)packet.WorldID;
				}
				NrPersonInfoUser nrPersonInfoUser = nrCharUser.GetPersonInfo() as NrPersonInfoUser;
				nrPersonInfoUser.SetBasePart(packet.kPartInfo.m_kBasePart);
			}
			Debug.LogWarning("PersonID :" + packet.PersonID);
			SUBCHAR_INFO packet3 = kDeserializePacket.GetPacket<SUBCHAR_INFO>();
			if (nrCharUser != null)
			{
				nrCharUser.SetSubCharKindFromList(packet3.i32SubCharKind);
			}
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_bGameConnected = true;
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.SetActivityMax();
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.PlunderRank = packet.m_nPlunderRank;
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.ColosseumGrade = packet.m_nColosseumGrade;
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.ColosseumGradePoint = packet.m_nColosseumGradePoint;
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.ColosseumOldGrade = packet.m_nColosseumOldGrade;
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.ColosseumOldRank = packet.m_nColosseumOldRank;
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.ColosseumWinCount = packet.m_nColosseumWinCount;
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfiBattleReward = packet.m_i8RankingReward;
			NrTSingleton<NkClientLogic>.Instance.SetReadyResponse(true);
			NrTSingleton<NkClientLogic>.Instance.SetLoginGameServer(false);
			Debug.LogWarning("========== GS_LOAD_CHAR_NFY : SetLoginGameServer false ----- ");
			NrTSingleton<GameGuideManager>.Instance.Update(GameGuideCheck.LOGIN);
			Tapjoy.SetUserID(nrCharUser.GetCharName());
			Tapjoy.SetUserLevel(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel());
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo != null)
			{
				PlayerPrefs.SetInt(NrPrefsKey.PLAYER_LEVEL, charPersonInfo.GetLevel(0L));
			}
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.HP_Auth = packet.i32HP_Auth;
			System_Option_Dlg system_Option_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SYSTEM_OPTION) as System_Option_Dlg;
			if (system_Option_Dlg != null)
			{
				system_Option_Dlg.CheckColosseumInvite();
			}
			int mapIndexFromUnique = NrTSingleton<MapManager>.Instance.GetMapIndexFromUnique(packet.MapUnique);
			if (mapIndexFromUnique != 2 && mapIndexFromUnique != 60 && mapIndexFromUnique != 4 && mapIndexFromUnique != 61)
			{
				if (Launcher.Instance.LocalPatchLevel != Launcher.Instance.PatchLevelMax)
				{
					Launcher.Instance.SavePatchLevel(Launcher.Instance.PatchLevelMax);
					NrTSingleton<NrMainSystem>.Instance.ReLogin(false);
				}
			}
			GS_INQUIRE_ANSWER_COUNT_REQ obj = new GS_INQUIRE_ANSWER_COUNT_REQ();
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_INQUIRE_ANSWER_COUNT_REQ, obj);
		}

		public static void GS_CHAR_LOGINDATAINFO_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_CHAR_LOGINDATAINFO_NFY packet = kDeserializePacket.GetPacket<GS_CHAR_LOGINDATAINFO_NFY>();
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
			for (int i = 0; i < (int)packet.nSubDataCount; i++)
			{
				CHARACTER_SUBDATA packet2 = kDeserializePacket.GetPacket<CHARACTER_SUBDATA>();
				myCharInfo.SetCharSubData(packet2.nSubDataType, packet2.nSubDataValue);
				myCharInfo.ResultCharSubData(packet2.nSubDataType, packet2.nSubDataValue, 0L);
			}
			for (int j = 0; j < (int)packet.nMonthDataCount; j++)
			{
				CHAR_MONTHDATA_INFO packet3 = kDeserializePacket.GetPacket<CHAR_MONTHDATA_INFO>();
				myCharInfo.SetCharMonthData(packet3.nType, packet3.nValue);
				int nType = packet3.nType;
				if (nType != 0)
				{
				}
			}
			for (int k = 0; k < (int)packet.nWeekDataCount; k++)
			{
				CHAR_WEEKDATA_INFO packet4 = kDeserializePacket.GetPacket<CHAR_WEEKDATA_INFO>();
				myCharInfo.SetCharWeekData(packet4.nType, packet4.nValue);
				int nType = packet4.nType;
				if (nType != 0)
				{
				}
			}
			for (int l = 0; l < (int)packet.nDetailinfoCount; l++)
			{
				CHAR_DETAIL_INFO packet5 = kDeserializePacket.GetPacket<CHAR_DETAIL_INFO>();
				myCharInfo.SetCharDetail(packet5.nType, packet5.nValue);
				myCharInfo.ResultCharDetail(packet5.nType, packet5.nValue);
			}
			myCharInfo.ClearTreasureMapData();
			for (int m = 0; m < (int)packet.nTreasureCount; m++)
			{
				TREASURE_MAPDATA packet6 = kDeserializePacket.GetPacket<TREASURE_MAPDATA>();
				myCharInfo.AddTreasureMapData(packet6.i32MapIndex);
			}
			if (packet.nTreasureCount == 0)
			{
				GameGuideTreasureAlarm gameGuideTreasureAlarm = NrTSingleton<GameGuideManager>.Instance.GetGuide(GameGuideType.TREASURE_ALARM) as GameGuideTreasureAlarm;
				if (gameGuideTreasureAlarm != null)
				{
					gameGuideTreasureAlarm.SetInfo(string.Empty, 0, 0);
				}
			}
			NrTSingleton<NrTableEvnetHeroManager>.Instance.EventHeroDataClear();
			for (int n = 0; n < (int)packet.nEventHeroCount; n++)
			{
				EVENT_HEROINFO packet7 = kDeserializePacket.GetPacket<EVENT_HEROINFO>();
				NrTSingleton<NrTableEvnetHeroManager>.Instance.SetServerEventHero(packet7);
			}
			if (solMilitaryGroupDlg != null)
			{
				solMilitaryGroupDlg.RefreshSolList();
			}
			NrTSingleton<NrTableVipManager>.Instance.VipDataClear();
			for (int num = 0; num < (int)packet.nVipDataCount; num++)
			{
				VIP_INFODATA packet8 = kDeserializePacket.GetPacket<VIP_INFODATA>();
				NrTSingleton<NrTableVipManager>.Instance.AddVipInfo(packet8);
			}
			if (packet.nVipDataCount > 0 && myCharInfo != null && NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
			{
				myCharInfo.SetActivityTime(PublicMethod.GetCurTime());
				myCharInfo.SetActivityMax();
				myCharInfo.RefreshVipActivityAddTime();
			}
			NrTSingleton<NewGuildManager>.Instance.ClearAgit();
			for (int num2 = 0; num2 < (int)packet.nAgitNpcCount; num2++)
			{
				AGIT_NPC_SUB_DATA packet9 = kDeserializePacket.GetPacket<AGIT_NPC_SUB_DATA>();
				NrTSingleton<NewGuildManager>.Instance.AddAgitNPCData(packet9);
			}
		}

		public static void GS_CHAR_ETCDATAINFO_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_CHAR_ETCDATAINFO_NFY packet = kDeserializePacket.GetPacket<GS_CHAR_ETCDATAINFO_NFY>();
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			kMyCharInfo.IntroMsg = TKString.NEWString(packet.szIntroMsg);
			string str = string.Empty;
			if (charPersonInfo != null)
			{
				str = charPersonInfo.GetCharName();
			}
			if (packet.nNewLastStoryCommentID >= 0L)
			{
				if (!PlayerPrefs.HasKey(str + "NewCommentID"))
				{
					PlayerPrefs.SetString(str + "NewCommentID", packet.nNewLastStoryCommentID.ToString());
				}
				else
				{
					long num = long.Parse(PlayerPrefs.GetString(str + "NewCommentID"));
					if (packet.nNewLastStoryCommentID > num)
					{
						NrTSingleton<UIDataManager>.Instance.NoticeStoryChat = true;
						PlayerPrefs.SetString(str + "NewCommentID", packet.nNewLastStoryCommentID.ToString());
					}
				}
			}
			NrTSingleton<BountyHuntManager>.Instance.CheckBountyHuntInfoNPCCharKind();
			kMyCharInfo.ClearBountyHuntClearInfo();
			kMyCharInfo.BountyHuntUnique = packet.nBountyHuntUnique;
			for (int i = 0; i < (int)packet.nBountyHuntCount; i++)
			{
				BOUNTYHUNT_CLEARINFO packet2 = kDeserializePacket.GetPacket<BOUNTYHUNT_CLEARINFO>();
				kMyCharInfo.AddBountyHuntClearInfo(packet2);
			}
			NrTSingleton<BountyHuntManager>.Instance.UpdateClientNpc(0);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BOUNTYCHECK_DLG);
			BountyHuntingDlg bountyHuntingDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOUNTYHUNTING_DLG) as BountyHuntingDlg;
			if (bountyHuntingDlg != null)
			{
				bountyHuntingDlg.SetData();
			}
			UserChallengeInfo userChallengeInfo = kMyCharInfo.GetUserChallengeInfo();
			for (int j = 0; j < (int)packet.nChallengeNum; j++)
			{
				Challenge_Info packet3 = kDeserializePacket.GetPacket<Challenge_Info>();
				if (userChallengeInfo != null)
				{
					userChallengeInfo.SetUserChallengeInfo(packet3);
				}
			}
			userChallengeInfo.SetLoadData(true);
			NrTSingleton<ChallengeManager>.Instance.CalcContinueRewardNoticeCount();
			NrTSingleton<ChallengeManager>.Instance.CalcDayRewardNoticeCount();
			kMyCharInfo.ClearVoucherData();
			for (int k = 0; k < (int)packet.nVoucherCount; k++)
			{
				VOUCHER_DATA packet4 = kDeserializePacket.GetPacket<VOUCHER_DATA>();
				kMyCharInfo.AddVoucherData(packet4);
			}
			NrTSingleton<ChallengeManager>.Instance.ChallengeEventRewardInfoInit();
			for (int l = 0; l < (int)packet.nChallengeEvent_RewardCount; l++)
			{
				CHALLENGEEVENT_REWARDINFO packet5 = kDeserializePacket.GetPacket<CHALLENGEEVENT_REWARDINFO>();
				NrTSingleton<ChallengeManager>.Instance.SetChallengeEventRewardInfo(packet5.i16ChallengeEvent_Unique);
			}
			for (int m = 0; m < (int)packet.nItemShop_PopUpShopCount; m++)
			{
				POPUPSHOP_DATA packet6 = kDeserializePacket.GetPacket<POPUPSHOP_DATA>();
				NrTSingleton<ItemMallPoPupShopManager>.Instance.Set_ServerValue(packet6);
			}
		}

		public static void GS_CHARACTER_INTRO_MSG_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_CHARACTER_INTRO_MSG_NFY packet = kDeserializePacket.GetPacket<GS_CHARACTER_INTRO_MSG_NFY>();
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			kMyCharInfo.IntroMsg = TKString.NEWString(packet.szIntromsg);
		}

		public static void GS_CHARACTER_INTRO_MSG_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_CHARACTER_INTRO_MSG_SET_ACK packet = kDeserializePacket.GetPacket<GS_CHARACTER_INTRO_MSG_SET_ACK>();
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			kMyCharInfo.IntroMsg = TKString.NEWString(packet.szIntromsg);
			MainMenuDlg mainMenuDlg = (MainMenuDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MAINMENU_DLG);
			if (mainMenuDlg != null)
			{
				mainMenuDlg.SetItroMsg(kMyCharInfo.IntroMsg);
			}
		}

		public static void GS_CLIENT_RELOGIN_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_CLIENT_RELOGIN_ACK packet = kDeserializePacket.GetPacket<GS_CLIENT_RELOGIN_ACK>();
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser != null)
			{
				NkCharIDInfo iDInfo = nrCharUser.GetIDInfo();
				if (iDInfo != null)
				{
					iDInfo.m_nWorldID = (int)packet.WorldID;
				}
				NrPersonInfoUser nrPersonInfoUser = nrCharUser.GetPersonInfo() as NrPersonInfoUser;
				nrPersonInfoUser.SetPersonID(packet.PersonID);
				nrPersonInfoUser.SetBasePart(packet.kPartInfo.m_kBasePart);
				nrCharUser.SetCharState(packet.CharState);
			}
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo != null)
			{
				myCharInfo.m_PersonID = packet.PersonID;
				myCharInfo.m_Money = packet.Money;
				myCharInfo.m_kCharMapInfo.MapUnique = packet.MapUnique;
			}
			if (packet.BFID == -1 && Scene.IsCurScene(Scene.Type.BATTLE))
			{
				if (NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
				{
					NrTSingleton<NkBabelMacroManager>.Instance.SetStatus(eBABEL_MACRO_STATUS.eBABEL_MACRO_STATUS_NONE, 0f);
					UIDataManager.MuteSound(false);
					NrTSingleton<NkBabelMacroManager>.Instance.SetStop(false);
				}
				if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_RESULT_DLG) && !NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_RESULT_PLUNDER_DLG))
				{
					GS_BATTLE_SREWARD_REQ gS_BATTLE_SREWARD_REQ = new GS_BATTLE_SREWARD_REQ();
					gS_BATTLE_SREWARD_REQ.m_nRewardUnique = -1;
					SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BATTLE_SREWARD_REQ, gS_BATTLE_SREWARD_REQ);
					Battle.BATTLE.Observer = true;
					MsgHandler.Handle("Rcv_BATTLE_RESULT", new object[0]);
				}
			}
			if (Scene.IsCurScene(Scene.Type.WORLD))
			{
			}
		}

		public static void GS_SOLDIERS_NFY(NkDeserializePacket kDeserializePacket)
		{
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser == null)
			{
				return;
			}
			NrPersonInfoBase personInfo = nrCharUser.GetPersonInfo();
			if (personInfo == null)
			{
				return;
			}
			GS_SOLDIERS_NFY packet = kDeserializePacket.GetPacket<GS_SOLDIERS_NFY>();
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			kMyCharInfo.DepolyCombatPower = 0L;
			NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
			NrSolWarehouse warehouseSolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetWarehouseSolList();
			NkMilitaryList militaryList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetMilitaryList();
			List<long> list = new List<long>();
			for (int i = 0; i < (int)packet.SolCount; i++)
			{
				SOLDIER_INFO packet2 = kDeserializePacket.GetPacket<SOLDIER_INFO>();
				list.Add(packet2.SolID);
				if (packet2.SolPosType == 1)
				{
					personInfo.SetSoldierInfo((int)packet2.SolPosIndex, packet2);
				}
				else
				{
					if (packet2.SolPosType == 5)
					{
						warehouseSolList.AddSolInfo(packet2, null, false);
					}
					else
					{
						readySolList.AddSolInfo(packet2, null, false);
					}
					if (packet2.SolPosType == 2)
					{
						NkSoldierInfo solInfo = readySolList.GetSolInfo(packet2.SolID);
						NkMineMilitaryInfo mineMilitaryInfo = militaryList.GetMineMilitaryInfo(packet2.MilitaryUnique);
						if (mineMilitaryInfo != null)
						{
							militaryList.AddMilitarySoldier(mineMilitaryInfo, ref solInfo);
						}
					}
					else if (packet2.SolPosType == 6)
					{
						NkSoldierInfo solInfo2 = readySolList.GetSolInfo(packet2.SolID);
						NkExpeditionMilitaryInfo expeditionMilitaryInfo = militaryList.GetExpeditionMilitaryInfo(packet2.MilitaryUnique);
						if (expeditionMilitaryInfo != null)
						{
							militaryList.AddExpeditionMilitarySoldier(expeditionMilitaryInfo, ref solInfo2);
						}
					}
				}
			}
			for (int j = 0; j < (int)packet.BattleSkillCount; j++)
			{
				SOLDIER_SIMPLE_BATTLESKILL_INFO packet3 = kDeserializePacket.GetPacket<SOLDIER_SIMPLE_BATTLESKILL_INFO>();
				NkSoldierInfo soldierInfoFromSolID = personInfo.GetSoldierInfoFromSolID(packet3.SolID);
				if (soldierInfoFromSolID != null)
				{
					soldierInfoFromSolID.SetBattleSkillData((int)packet3.SkillIndex, ref packet3.BattleSkillData);
				}
			}
			for (int k = 0; k < (int)packet.SolSubDataCount; k++)
			{
				SOLDIER_SUBDATA packet4 = kDeserializePacket.GetPacket<SOLDIER_SUBDATA>();
				NkSoldierInfo soldierInfoFromSolID2 = personInfo.GetSoldierInfoFromSolID(packet4.nSolID);
				if (soldierInfoFromSolID2 != null)
				{
					soldierInfoFromSolID2.SetSolSubData(packet4.nSubDataType, packet4.nSubDataValue);
				}
			}
			int faceCharKind = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetFaceCharKind();
			byte faceSolGrade = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetFaceSolGrade();
			long faceSolID = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetFaceSolID();
			int faceCostumeUnique = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetFaceCostumeUnique();
			nrCharUser.ChangeCharModel(faceCharKind, faceSolGrade, faceSolID, faceCostumeUnique);
			SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
			if (solMilitaryGroupDlg != null)
			{
				for (int l = 0; l < list.Count; l++)
				{
					NkSoldierInfo soldierInfoFromSolID3 = personInfo.GetSoldierInfoFromSolID(list[l]);
					if (soldierInfoFromSolID3 != null)
					{
						solMilitaryGroupDlg.RefereshSelectSolInfo(soldierInfoFromSolID3);
					}
				}
			}
		}

		public static void GS_USERCHAR_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_USERCHAR_INFO_ACK packet = kDeserializePacket.GetPacket<GS_USERCHAR_INFO_ACK>();
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(packet.CharUnique) as NrCharUser;
			if (nrCharUser != null)
			{
				NkCharIDInfo iDInfo = nrCharUser.GetIDInfo();
				if (iDInfo != null)
				{
					iDInfo.m_nWorldID = packet.WorldID;
				}
				nrCharUser.ChangeCharModel(packet.kShapeInfo.nFaceCharKind, packet.kShapeInfo.nFaceGrade, packet.kShapeInfo.nFaceCharSolID, packet.kShapeInfo.nFaceCostumeUnique);
				nrCharUser.ChangeCharPartInfo(packet.kShapeInfo.kPartInfo, true, true);
				SUBCHAR_INFO packet2 = kDeserializePacket.GetPacket<SUBCHAR_INFO>();
				nrCharUser.SetSubCharKindFromList(packet2.i32SubCharKind);
				nrCharUser.SetColosseumGrade(packet.iColosseumGrade);
				string text = TKString.NEWString(packet.szCharGuildName);
				if (text != string.Empty)
				{
					if (packet.bCharGuildPortrait)
					{
						nrCharUser.SetUserGuildName(text, packet.GuildID, packet.bGuildWar);
					}
					else
					{
						nrCharUser.SetUserGuildName(text, 0L, packet.bGuildWar);
					}
				}
			}
		}

		public static void GS_CHARPART_INFO_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_CHARPART_INFO_NFY packet = kDeserializePacket.GetPacket<GS_CHARPART_INFO_NFY>();
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(packet.CharUnique) as NrCharUser;
			if (nrCharUser != null)
			{
				if (nrCharUser.GetPersonInfo().GetPersonID() == NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID)
				{
					return;
				}
				if (0 < packet.i32CharKind)
				{
					nrCharUser.SetClassChage(packet.i32CharKind);
				}
				else
				{
					nrCharUser.ChangeCharModel(packet.kShapeInfo.nFaceCharKind, packet.kShapeInfo.nFaceGrade, packet.kShapeInfo.nFaceCharSolID, packet.kShapeInfo.nFaceCostumeUnique);
					nrCharUser.ChangeCharPartInfo(packet.kShapeInfo.kPartInfo, true, true);
				}
			}
			else
			{
				NrTSingleton<NkCharManager>.Instance.SetReservedCharPartInfo(packet.CharUnique, packet.kShapeInfo.nFaceCharKind, packet.kShapeInfo.nFaceGrade, packet.kShapeInfo.kPartInfo);
			}
		}

		public static void GS_NPC_POSLIST_ACK(NkDeserializePacket kDeserializePacket)
		{
		}

		public static void GS_GMCOMMAND_CMONEY_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_GMCOMMAND_CMONEY_NFY packet = kDeserializePacket.GetPacket<GS_GMCOMMAND_CMONEY_NFY>();
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = packet.i64NewMoney;
		}

		public static void GS_GMCOMMAND_CITEM_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_GMCOMMAND_CITEM_NFY packet = kDeserializePacket.GetPacket<GS_GMCOMMAND_CITEM_NFY>();
			NkUserInventory.GetInstance().SetInfo(packet.item, -1);
			RightMenuQuestUI rightMenuQuestUI = (RightMenuQuestUI)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MAIN_QUEST);
			if (rightMenuQuestUI != null)
			{
				rightMenuQuestUI.QuestUpdate();
			}
		}

		public static void GS_GMCOMMAND_PWTIME_NFY(NkDeserializePacket kDeserializePacket)
		{
		}

		public static void GS_GMCOMMAND_SCENE_ACK(NkDeserializePacket kDeserializePacket)
		{
			NrTSingleton<NkCharManager>.Instance.InitChar3DModelAll();
		}

		public static void GS_SERVER_CHARINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SERVER_CHARINFO_ACK packet = kDeserializePacket.GetPacket<GS_SERVER_CHARINFO_ACK>();
			if (!packet.bTimeOnly)
			{
				DLG_FPS dLG_FPS = (DLG_FPS)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_FPS);
				dLG_FPS.SetUserInfo((int)packet.i16CurUserCount, (int)packet.i16MaxUserCount, packet.i64TotalTime, packet.i64RealTime);
				dLG_FPS.SetMapCharInfo((int)packet.i16MonsterCount, (int)packet.i16NPCCount, (int)packet.i16UserCount);
			}
			else
			{
				MainMenuDlg mainMenuDlg = (MainMenuDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MAINMENU_DLG);
				if (mainMenuDlg != null)
				{
					mainMenuDlg.SetTimeData(packet.i64TotalTime, packet.i64RealTime);
				}
			}
		}

		public static void GS_AUTH_SESSION_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_AUTH_SESSION_ACK packet = kDeserializePacket.GetPacket<GS_AUTH_SESSION_ACK>();
			if (packet.Result == 0)
			{
				NrTSingleton<NkCharManager>.Instance.Init(false);
				MsgHandler.Handle("Rcv_GoToPrepareGameStage", new object[0]);
			}
		}

		public static void GS_MAKE_CHAR_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_MAKE_CHAR_NFY packet = kDeserializePacket.GetPacket<GS_MAKE_CHAR_NFY>();
			NrTSingleton<NkCharManager>.Instance.SetChar(packet.MakeCharInfo, true, false);
		}

		public static void GS_MAKEMANY_CHAR_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_MAKEMANY_CHAR_NFY packet = kDeserializePacket.GetPacket<GS_MAKEMANY_CHAR_NFY>();
			int shNumberChar = (int)packet.m_shNumberChar;
			for (int i = 0; i < shNumberChar; i++)
			{
				NEW_MAKECHAR_INFO packet2 = kDeserializePacket.GetPacket<NEW_MAKECHAR_INFO>();
				NrTSingleton<NkCharManager>.Instance.SetChar(packet2, true, false);
			}
		}

		public static void GS_DELETE_CHAR_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_DELETE_CHAR_NFY packet = kDeserializePacket.GetPacket<GS_DELETE_CHAR_NFY>();
			NrTSingleton<NkCharManager>.Instance.DelReservedCharMakeInfo(packet.m_shCharUnique);
			NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(packet.m_shCharUnique);
			if (charByCharUnique != null && charByCharUnique.Get3DCharStep() != NrCharBase.e3DCharStep.DIED)
			{
				NrTSingleton<NkCharManager>.Instance.DeleteChar(charByCharUnique.GetID());
			}
		}

		public static void GS_DELETEMANY_CHAR_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_DELETEMANY_CHAR_NFY packet = kDeserializePacket.GetPacket<GS_DELETEMANY_CHAR_NFY>();
			int shNumberChar = (int)packet.m_shNumberChar;
			for (int i = 0; i < shNumberChar; i++)
			{
				DEL_CHAR_INFO packet2 = kDeserializePacket.GetPacket<DEL_CHAR_INFO>();
				NrTSingleton<NkCharManager>.Instance.DelReservedCharMakeInfo(packet2.CharUnique);
				NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(packet2.CharUnique);
				if (charByCharUnique != null)
				{
					NrTSingleton<NkCharManager>.Instance.DeleteChar(charByCharUnique.GetID());
				}
			}
		}

		public static void GS_SOLDIER_UPDATE_INFO_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLDIER_UPDATE_INFO_NFY packet = kDeserializePacket.GetPacket<GS_SOLDIER_UPDATE_INFO_NFY>();
			List<NkSoldierInfo> list = new List<NkSoldierInfo>();
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			for (int i = 0; i < packet.i32UpdateCount; i++)
			{
				SOLDIER_UPDATE_INFO packet2 = kDeserializePacket.GetPacket<SOLDIER_UPDATE_INFO>();
				NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(packet.i16CharUnique) as NrCharUser;
				if (nrCharUser != null)
				{
					NkSoldierInfo soldierInfoFromSolID = nrCharUser.GetPersonInfo().GetSoldierInfoFromSolID(packet2.nSolID);
					if (soldierInfoFromSolID != null)
					{
						if (NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(soldierInfoFromSolID.GetCharKind()) != null)
						{
							bool flag = true;
							for (int j = 0; j < list.Count; j++)
							{
								if (list[j].GetSolID() == soldierInfoFromSolID.GetSolID())
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								list.Add(soldierInfoFromSolID);
							}
							E_SOLDIER_UPDATE_INFO_TYPE e_SOLDIER_UPDATE_INFO_TYPE = (E_SOLDIER_UPDATE_INFO_TYPE)packet2.nUpdateType;
							if (e_SOLDIER_UPDATE_INFO_TYPE != E_SOLDIER_UPDATE_INFO_TYPE.E_SOLDIER_UPDATE_INFO_TYPE_LEVEL)
							{
								if (e_SOLDIER_UPDATE_INFO_TYPE == E_SOLDIER_UPDATE_INFO_TYPE.E_SOLDIER_UPDATE_INFO_TYPE_EXP)
								{
									soldierInfoFromSolID.SetExp(packet2.nUpdateData);
								}
							}
							else
							{
								bool flag2 = false;
								if (soldierInfoFromSolID.GetLevel() != (short)packet2.nUpdateData)
								{
									flag2 = true;
								}
								if (soldierInfoFromSolID.IsLeader())
								{
									short level = soldierInfoFromSolID.GetLevel();
									soldierInfoFromSolID.SetLevel((short)packet2.nUpdateData);
									if (level < 7 && (short)packet2.nUpdateData >= 7)
									{
										MenuIconDlg menuIconDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MENUICON_DLG) as MenuIconDlg;
										if (menuIconDlg != null)
										{
											menuIconDlg.ShowDownButton(true);
										}
									}
								}
								else
								{
									soldierInfoFromSolID.SetLevel((short)packet2.nUpdateData);
								}
								soldierInfoFromSolID.SetHP(soldierInfoFromSolID.GetMaxHP(), 0);
								NrCharUser nrCharUser2 = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
								if (nrCharUser2 != null)
								{
									if (nrCharUser2.GetID() == nrCharUser.GetID())
									{
										if (flag2)
										{
											AlarmManager.GetInstance().AddSolAlarm(packet2.nSolID);
										}
										if (soldierInfoFromSolID.IsLeader())
										{
											if (NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
											{
												kMyCharInfo.SetActivityMax();
												if (kMyCharInfo.m_nActivityPoint < kMyCharInfo.m_nMaxActivityPoint)
												{
													kMyCharInfo.SetActivityPoint(kMyCharInfo.m_nMaxActivityPoint);
												}
											}
											NrTSingleton<GameGuideManager>.Instance.Update(GameGuideCheck.LEVELUP);
											BookmarkDlg bookmarkDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BOOKMARK_DLG) as BookmarkDlg;
											if (bookmarkDlg != null)
											{
												bookmarkDlg.SetBookmarkInfo();
											}
										}
									}
									else if (NrTSingleton<NkClientLogic>.Instance.IsEffectEnable())
									{
										NrTSingleton<NkEffectManager>.Instance.AddEffect("LEVELUP", nrCharUser);
									}
								}
							}
						}
					}
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				list[j].UpdateSoldierInfo();
			}
		}

		public static void GS_SOLDIER_STATUS_UPDATE_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLDIER_STATUS_UPDATE_NFY packet = kDeserializePacket.GetPacket<GS_SOLDIER_STATUS_UPDATE_NFY>();
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo != null)
			{
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet.SolID);
				if (soldierInfoFromSolID == null)
				{
					return;
				}
				soldierInfoFromSolID.SetSolStatus(packet.SolStatus);
				soldierInfoFromSolID.SetBattlePos(packet.BattlePos);
			}
		}

		public static void GS_SOLDIER_GROUPSTATUS_UPDATE_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_SOLDIER_GROUPSTATUS_UPDATE_NFY packet = kDeserializePacket.GetPacket<GS_SOLDIER_GROUPSTATUS_UPDATE_NFY>();
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (charPersonInfo != null)
			{
				for (int i = 0; i < 6; i++)
				{
					if (packet.SolID[i] > 0L)
					{
						NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(packet.SolID[i]);
						if (soldierInfoFromSolID != null)
						{
							soldierInfoFromSolID.SetSolStatus(packet.SolStatus[i]);
							soldierInfoFromSolID.SetBattlePos(packet.BattlePos[i]);
						}
					}
				}
			}
		}

		public static void GS_ECO_OBJECT_MAKECHAR_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ECO_OBJECT_MAKECHAR_ACK packet = kDeserializePacket.GetPacket<GS_ECO_OBJECT_MAKECHAR_ACK>();
			NrCharObject nrCharObject = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(packet.i16ObjCharUnique) as NrCharObject;
			if (nrCharObject != null)
			{
				nrCharObject.MakeMonsterAnimation();
			}
		}

		public static void GS_ECO_CHAR_SIT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_ECO_CHAR_SIT_ACK packet = kDeserializePacket.GetPacket<GS_ECO_CHAR_SIT_ACK>();
			NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(packet.i16CharUnique);
			if (charByCharUnique != null)
			{
				if (charByCharUnique.m_kCharMove != null && charByCharUnique.IsReady3DModel())
				{
					Vector2 a = new Vector2(charByCharUnique.m_k3DChar.GetRootGameObject().transform.position.x, charByCharUnique.m_k3DChar.GetRootGameObject().transform.position.z);
					Vector2 b = new Vector2(packet.PosSit.x, packet.PosSit.z);
					if (Vector2.Distance(a, b) > 0.5f)
					{
						Vector3 targetPos = charByCharUnique.m_kCharMove.GetTargetPos();
						if (targetPos != Vector3.zero)
						{
							charByCharUnique.SitDown(false, packet.PosLookAt);
						}
						else
						{
							charByCharUnique.MoveTo(packet.PosSit);
							charByCharUnique.SitDown(false, packet.PosLookAt);
						}
					}
					else
					{
						charByCharUnique.SitDown(true, packet.PosLookAt);
					}
				}
				else
				{
					charByCharUnique.GetPersonInfo().SetCharPos(packet.PosSit.x, packet.PosSit.y, packet.PosSit.z);
					Vector3 vector = new Vector3(packet.PosLookAt.x - packet.PosSit.x, 0f, packet.PosLookAt.z - packet.PosSit.z);
					charByCharUnique.GetPersonInfo().SetDirection(vector.x, vector.y, vector.z);
					charByCharUnique.ClearReservedMoveTarget();
				}
			}
		}

		public static void GS_SET_CHAR_STATUS_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SET_CHAR_STATUS_ACK packet = kDeserializePacket.GetPacket<GS_SET_CHAR_STATUS_ACK>();
			NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(packet.CharUnique);
			if (charByCharUnique != null)
			{
				charByCharUnique.SetCharState_ALL(packet.Status);
				if (charByCharUnique.IsReadyCharAction())
				{
					charByCharUnique.SetAnimationFromState();
				}
			}
			else if ((packet.Status & 32L) != 0L)
			{
				NrTSingleton<NkCharManager>.Instance.DelReservedCharMakeInfo(packet.CharUnique);
			}
		}

		public static void GS_SET_CHAR_GROUP_STATUS_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_SET_CHAR_GROUP_STATUS_ACK packet = kDeserializePacket.GetPacket<GS_SET_CHAR_GROUP_STATUS_ACK>();
			for (int i = 0; i < packet.StatusCount; i++)
			{
				CHAR_STATUS_INFO packet2 = kDeserializePacket.GetPacket<CHAR_STATUS_INFO>();
				NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(packet2.CharUnique);
				if (charByCharUnique != null)
				{
					charByCharUnique.SetCharState_ALL(packet2.Status);
					if (charByCharUnique.IsReadyCharAction())
					{
						charByCharUnique.SetAnimationFromState();
					}
				}
				else if ((packet2.Status & 32L) != 0L)
				{
					NrTSingleton<NkCharManager>.Instance.DelReservedCharMakeInfo(packet2.CharUnique);
				}
			}
		}

		public static void GS_UPDATE_SOLDIER_ENHANCE(NkDeserializePacket kDeserializePacket)
		{
		}

		public static void GS_EXPLORATION_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_EXPLORATION_ACK packet = kDeserializePacket.GetPacket<GS_EXPLORATION_ACK>();
			if (packet.m_nResult == 0)
			{
				NrTSingleton<ExplorationManager>.Instance.AddReward(packet.m_nAddMoney, packet.m_kRewardItem);
				ExplorationPlayDlg explorationPlayDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EXPLORATION_PLAY_DLG) as ExplorationPlayDlg;
				if (explorationPlayDlg != null)
				{
					explorationPlayDlg.PlayEnd(packet.m_nTableIndex, packet.m_nIndex, packet.m_nAddMoney, packet.m_kRewardItem);
				}
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.EXPLORATION_PLAY_DLG);
				eRESULT nResult = (eRESULT)packet.m_nResult;
				if (nResult != eRESULT.R_FAIL_INVENTORY_FULL)
				{
					if (nResult == eRESULT.R_FAIL_INVENTORY_FULL_TICKET)
					{
						string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("773");
						Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
					}
				}
				else
				{
					string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("46");
					Main_UI_SystemMessage.ADDMessage(textFromNotify2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				}
			}
		}

		public static void GS_RECOMMEND_ADD_ACK(NkDeserializePacket kDeserializePacket)
		{
		}

		public static void GS_RELOAD_NDT_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_RELOAD_NDT_ACK packet = kDeserializePacket.GetPacket<GS_RELOAD_NDT_ACK>();
			string text = string.Empty;
			if (packet.i32Result == 0)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("291"),
					"ndtname",
					TKString.NEWString(packet.strNDT)
				});
				Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			}
			else
			{
				text = string.Format("Reload Fail {0}", TKString.NEWString(packet.strNDT));
				Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
			string name = "[RELOAD]";
			NrTSingleton<ChatManager>.Instance.PushMsg(CHAT_TYPE.SYSTEM, name, text);
		}

		public static void GS_EVENTHERO_ALLDATA_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_EVENTHERO_ALLDATA_NFY packet = kDeserializePacket.GetPacket<GS_EVENTHERO_ALLDATA_NFY>();
			NrTSingleton<NrTableEvnetHeroManager>.Instance.EventHeroDataClear();
			for (int i = 0; i < packet.i32EventHeroCount; i++)
			{
				EVENT_HEROINFO packet2 = kDeserializePacket.GetPacket<EVENT_HEROINFO>();
				NrTSingleton<NrTableEvnetHeroManager>.Instance.SetServerEventHero(packet2);
			}
			SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
			if (solMilitaryGroupDlg != null)
			{
				solMilitaryGroupDlg.RefreshSolList();
			}
		}

		public static void GS_EVENTHERO_ADDDATA_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_EVENTHERO_ADDDATA_NFY packet = kDeserializePacket.GetPacket<GS_EVENTHERO_ADDDATA_NFY>();
			TsLog.LogWarning("!!! Event GS_EVENTHERO_ADDDATA_NFY == " + packet.i32EventHeroCount + string.Empty, new object[0]);
			for (int i = 0; i < packet.i32EventHeroCount; i++)
			{
				EVENT_HEROINFO packet2 = kDeserializePacket.GetPacket<EVENT_HEROINFO>();
				NrTSingleton<NrTableEvnetHeroManager>.Instance.AddServerEventHero(packet2);
			}
			if (packet.i32EventHeroCount > 0)
			{
				SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
				if (solMilitaryGroupDlg != null)
				{
					solMilitaryGroupDlg.RefreshSolList();
				}
			}
		}

		public static void GS_EVENTHERO_DELDATA_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_EVENTHERO_DELDATA_NFY packet = kDeserializePacket.GetPacket<GS_EVENTHERO_DELDATA_NFY>();
			TsLog.LogWarning("!!! Event GS_EVENTHERO_DELDATA_NFY == " + packet.i32EventHeroCount + string.Empty, new object[0]);
			for (int i = 0; i < packet.i32EventHeroCount; i++)
			{
				EVENT_HEROINFO packet2 = kDeserializePacket.GetPacket<EVENT_HEROINFO>();
				NrTSingleton<NrTableEvnetHeroManager>.Instance.DelServerEventHero(packet2);
			}
			if (packet.i32EventHeroCount > 0)
			{
				SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
				if (solMilitaryGroupDlg != null)
				{
					solMilitaryGroupDlg.RefreshSolList();
				}
			}
		}

		public static void GS_ADD_EVENT_TYPE_COUNT_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_ADD_EVENT_TYPE_COUNT_NFY packet = kDeserializePacket.GetPacket<GS_ADD_EVENT_TYPE_COUNT_NFY>();
			TsLog.LogWarning("!!! Event GS_ADD_EVENT_TYPE_COUNT_NFY == " + packet.i32EventCount + string.Empty, new object[0]);
			for (int i = 0; i < packet.i32EventCount; i++)
			{
				GS_ADD_EVENT_TYPE packet2 = kDeserializePacket.GetPacket<GS_ADD_EVENT_TYPE>();
				NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.AddEvent(packet2.m_nEventInfoWeek, packet2.m_nEventType, packet2.m_nStartTime, packet2.m_nEndDurationTime, packet2.m_nMaxLimitCount, packet2.m_nLeftEventTime, packet2.m_nDay, packet2.m_nWeek, packet2.m_nEventTitleText, packet2.m_nEventExplainText);
				if (packet2.m_nPushNotice == 0)
				{
					if (packet2.m_nEventType == 15 || packet2.m_nEventType == 16 || packet2.m_nEventType == 17)
					{
						NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
						if (myCharInfo == null)
						{
							return;
						}
						bool flag = false;
						int num = (int)myCharInfo.GetCharDetail(5);
						if (0 < num && NrTSingleton<NkQuestManager>.Instance.IsCompletedQuestGroup(num))
						{
							flag = true;
						}
						if (!flag)
						{
							AlarmManager.GetInstance().AddEventType(packet2.m_nEventType, packet2.m_nEventTitleText, packet2.m_nEventExplainText);
						}
					}
					else
					{
						AlarmManager.GetInstance().AddEventType(packet2.m_nEventType, packet2.m_nEventTitleText, packet2.m_nEventExplainText);
					}
				}
				if (packet2.m_nEventType == 14 && NrTSingleton<FormsManager>.Instance.IsShow(G_ID.DAILYDUNGEON_MAIN))
				{
					DailyDungeon_Main_Dlg dailyDungeon_Main_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DAILYDUNGEON_MAIN) as DailyDungeon_Main_Dlg;
					if (dailyDungeon_Main_Dlg != null)
					{
						sbyte nDayOfWeek = (sbyte)NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetEventWeek();
						dailyDungeon_Main_Dlg.SetBG();
						dailyDungeon_Main_Dlg.SetBasicData(nDayOfWeek, false);
					}
				}
			}
			MyCharInfoDlg myCharInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
			if (myCharInfoDlg != null)
			{
				myCharInfoDlg.UpdateNoticeInfo();
			}
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.EVENT_MAIN))
			{
				Event_Dlg event_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EVENT_MAIN) as Event_Dlg;
				if (event_Dlg != null)
				{
					if (event_Dlg.m_eCurrentTapIndex == eEVENT_TYPE.eBUNNING_CURRENT_EVENT)
					{
						event_Dlg.CurrentEventReq();
					}
					else if (event_Dlg.m_eCurrentTapIndex == eEVENT_TYPE.eBUNNING_Schedule_EVENT)
					{
						event_Dlg.ScheduleEventReq();
					}
				}
			}
		}

		public static void GS_ADD_EVENT_TYPE_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_ADD_EVENT_TYPE_NFY packet = kDeserializePacket.GetPacket<GS_ADD_EVENT_TYPE_NFY>();
			if (packet.m_nMode == 0)
			{
				NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.AddEvent(packet.m_nEventInfoWeek, packet.m_nEventType, packet.m_nStartTime, packet.m_nEndDurationTime, packet.m_nMaxLimitCount, packet.m_nLeftEventTime, packet.m_nDay, packet.m_nWeek, packet.m_nEventTitleText, packet.m_nEventExplainText);
				if (packet.m_nPushNotice == 0)
				{
					if (packet.m_nEventType == 15 || packet.m_nEventType == 16 || packet.m_nEventType == 17)
					{
						NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
						if (myCharInfo == null)
						{
							return;
						}
						bool flag = false;
						int num = (int)myCharInfo.GetCharDetail(5);
						if (0 < num && NrTSingleton<NkQuestManager>.Instance.IsCompletedQuestGroup(num))
						{
							flag = true;
						}
						if (!flag)
						{
							AlarmManager.GetInstance().AddEventType(packet.m_nEventType, packet.m_nEventTitleText, packet.m_nEventExplainText);
						}
					}
					else
					{
						AlarmManager.GetInstance().AddEventType(packet.m_nEventType, packet.m_nEventTitleText, packet.m_nEventExplainText);
					}
				}
				if (packet.m_nEventType == 14 && NrTSingleton<FormsManager>.Instance.IsShow(G_ID.DAILYDUNGEON_MAIN))
				{
					DailyDungeon_Main_Dlg dailyDungeon_Main_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DAILYDUNGEON_MAIN) as DailyDungeon_Main_Dlg;
					if (dailyDungeon_Main_Dlg != null)
					{
						sbyte nDayOfWeek = (sbyte)NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetEventWeek();
						dailyDungeon_Main_Dlg.SetBG();
						dailyDungeon_Main_Dlg.SetBasicData(nDayOfWeek, false);
					}
				}
			}
			else
			{
				NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.DeleteEvent(packet.m_nEventInfoWeek, packet.m_nEventType);
			}
			MyCharInfoDlg myCharInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
			if (myCharInfoDlg != null)
			{
				myCharInfoDlg.UpdateNoticeInfo();
			}
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.EVENT_MAIN))
			{
				Event_Dlg event_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EVENT_MAIN) as Event_Dlg;
				if (event_Dlg != null)
				{
					if (event_Dlg.m_eCurrentTapIndex == eEVENT_TYPE.eBUNNING_CURRENT_EVENT)
					{
						event_Dlg.CurrentEventReq();
					}
					else if (event_Dlg.m_eCurrentTapIndex == eEVENT_TYPE.eBUNNING_Schedule_EVENT)
					{
						event_Dlg.ScheduleEventReq();
					}
				}
			}
		}

		public static void GS_RELOAD_EVENT_TYPE_NFY(NkDeserializePacket kDeserializePacket)
		{
		}

		public static void GS_GET_EVENT_REWARD_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_GET_EVENT_REWARD_ACK packet = kDeserializePacket.GetPacket<GS_GET_EVENT_REWARD_ACK>();
			if (packet.m_nResult == 0)
			{
				eBUNNING_EVENT nEventType = (eBUNNING_EVENT)packet.m_nEventType;
				if (nEventType != eBUNNING_EVENT.eBUNNING_EVENT_BABELPARTY)
				{
					if (nEventType != eBUNNING_EVENT.eBUNNING_EVENT_COLOSSEUM)
					{
					}
				}
			}
		}

		public static void GS_GET_EVENT_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_GET_EVENT_INFO_ACK packet = kDeserializePacket.GetPacket<GS_GET_EVENT_INFO_ACK>();
			if (packet.m_nResult == 0)
			{
				int nEventMode = packet.m_nEventMode;
				if (nEventMode != 0)
				{
					if (nEventMode == 1)
					{
						NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.InitEventReflash();
						GS_GET_EVENT_LIST_ACK[] array = new GS_GET_EVENT_LIST_ACK[packet.m_nCount];
						for (int i = 0; i < packet.m_nCount; i++)
						{
							array[i] = kDeserializePacket.GetPacket<GS_GET_EVENT_LIST_ACK>();
							BUNNING_EVENT_REFLASH_INFO bUNNING_EVENT_REFLASH_INFO = new BUNNING_EVENT_REFLASH_INFO();
							bUNNING_EVENT_REFLASH_INFO.m_eEventType = (eBUNNING_EVENT)array[i].m_eEventType;
							bUNNING_EVENT_REFLASH_INFO.m_nStartTime = array[i].m_nStartTime;
							bUNNING_EVENT_REFLASH_INFO.m_nEndTime = array[i].m_nEndTime;
							bUNNING_EVENT_REFLASH_INFO.m_nLimitCount = array[i].m_nLimitCount;
							bUNNING_EVENT_REFLASH_INFO.m_nTitleText = array[i].m_nTitleText;
							bUNNING_EVENT_REFLASH_INFO.m_nExplain = array[i].m_nExplain;
							NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.SetEventReflash(bUNNING_EVENT_REFLASH_INFO);
						}
						if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.EVENT_MAIN))
						{
							Event_Dlg event_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EVENT_MAIN) as Event_Dlg;
							if (event_Dlg != null)
							{
								event_Dlg.ScheduleEventSetData();
							}
						}
					}
				}
				else
				{
					GS_GET_CURRENT_EVENT_LIST_ACK[] array2 = new GS_GET_CURRENT_EVENT_LIST_ACK[packet.m_nCount];
					NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.InitCurrentEventInfo();
					for (int j = 0; j < packet.m_nCount; j++)
					{
						array2[j] = kDeserializePacket.GetPacket<GS_GET_CURRENT_EVENT_LIST_ACK>();
						NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.AddEvent(array2[j].m_nEventInfoWeek, array2[j].m_nEventType, array2[j].m_nStartTime, array2[j].m_nEndEventTime, array2[j].m_nMaxLimitCount, array2[j].m_nLeftEventTime, packet.m_nDay, packet.m_nWeek, array2[j].m_nTitleText, array2[j].m_nExplain);
					}
					if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.EVENT_MAIN))
					{
						Event_Dlg event_Dlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EVENT_MAIN) as Event_Dlg;
						if (event_Dlg2 != null)
						{
							event_Dlg2.CurrentEventSetData();
						}
					}
				}
			}
		}

		public static void ReqWebUserImageCallback(Texture2D txtr, object _param)
		{
		}

		public static void GS_USER_PORTRAIT_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_USER_PORTRAIT_NFY packet = kDeserializePacket.GetPacket<GS_USER_PORTRAIT_NFY>();
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo != null)
			{
				if (packet.bLogInTime && myCharInfo.m_PersonID == packet.i64PersonID)
				{
					if (packet.i32Result == 0)
					{
						myCharInfo.UserPortrait = true;
						myCharInfo.GetUserPortrait(false);
					}
				}
				else if (myCharInfo.m_PersonID == packet.i64PersonID)
				{
					if (packet.i32Result == 0)
					{
						myCharInfo.UserPortrait = true;
						myCharInfo.GetUserPortrait(true);
					}
				}
				else
				{
					string userPortraitURL = NrTSingleton<NkCharManager>.Instance.GetUserPortraitURL(packet.i64PersonID);
					WebFileCache.RemoveEventItem(userPortraitURL);
					WebFileCache.RequestImageWebFile(userPortraitURL, new WebFileCache.ReqTextureCallback(NrReceiveGame.ReqWebUserImageCallback), null);
					USER_FRIEND_INFO friend = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriend(packet.i64PersonID);
					if (friend != null)
					{
						friend.ui8UserPortrait = 1;
					}
					if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.COMMUNITY_DLG))
					{
						CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
						communityUI_DLG.UpdateFriend(packet.i64PersonID, true);
					}
					if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.DLG_OTHER_CHAR_DETAIL))
					{
						DLG_OtherCharDetailInfo dLG_OtherCharDetailInfo = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_OTHER_CHAR_DETAIL) as DLG_OtherCharDetailInfo;
						dLG_OtherCharDetailInfo.UpsateSoldierList();
					}
				}
			}
		}

		public static void GS_VIP_ALLDATA_NFY(NkDeserializePacket kDeserializePacket)
		{
			GS_VIP_ALLDATA_NFY packet = kDeserializePacket.GetPacket<GS_VIP_ALLDATA_NFY>();
			NrTSingleton<NrTableVipManager>.Instance.VipDataClear();
			for (int i = 0; i < (int)packet.i8VipInfoCount; i++)
			{
				VIP_INFODATA packet2 = kDeserializePacket.GetPacket<VIP_INFODATA>();
				NrTSingleton<NrTableVipManager>.Instance.AddVipInfo(packet2);
			}
			if (packet.i8VipInfoCount > 0 && NrTSingleton<ContentsLimitManager>.Instance.IsWillSpend())
			{
				NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
				if (myCharInfo != null)
				{
					myCharInfo.SetActivityTime(PublicMethod.GetCurTime());
					myCharInfo.SetActivityMax();
				}
			}
		}

		public static void GS_CONSECUTIVELY_ATTENDACNE_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_CONSECUTIVELY_ATTENDACNE_ACK packet = kDeserializePacket.GetPacket<GS_CONSECUTIVELY_ATTENDACNE_ACK>();
			NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
			if (myCharInfo != null)
			{
				myCharInfo.ConsecutivelyattendanceTotalNum = packet.i8TotalNum;
				myCharInfo.ConsecutivelyattendanceCurrentNum = packet.i8CurrentNum;
				myCharInfo.ConsecutivelyattendanceReward = packet.bGetReward;
				myCharInfo.ConsecutivelyattendanceRewardType = packet.i8RewardType;
			}
			TsLog.LogWarning("GS_CONSECUTIVELY_ATTENDACNE_NFY == {0} , {1} ,{2}, {3}", new object[]
			{
				packet.i8TotalNum,
				packet.i8CurrentNum,
				packet.bGetReward,
				packet.i8RewardType
			});
			Normal_Attend_Dlg normal_Attend_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EVENT_NORMAL_ATTEND) as Normal_Attend_Dlg;
			if (normal_Attend_Dlg != null)
			{
				normal_Attend_Dlg.Init_Consecutively_Attend();
			}
			if (packet.bGetReward)
			{
				MyCharInfoDlg myCharInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
				if (myCharInfoDlg != null)
				{
					myCharInfoDlg.Attend_Notice_Show();
				}
			}
		}

		public static void GS_CONSECUTIVELY_ATTENDACNE_REWARD_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_CONSECUTIVELY_ATTENDACNE_REWARD_ACK packet = kDeserializePacket.GetPacket<GS_CONSECUTIVELY_ATTENDACNE_REWARD_ACK>();
			if (packet.i32Result == 0)
			{
				NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
				if (myCharInfo != null)
				{
					myCharInfo.ConsecutivelyattendanceReward = packet.bReward;
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("400"),
						"itemname",
						NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(packet.i32RewardUnique),
						"count",
						packet.i32RewardNum
					});
					Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
					MyCharInfoDlg myCharInfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG) as MyCharInfoDlg;
					if (myCharInfoDlg != null)
					{
						myCharInfoDlg.Attend_Notice_Show();
					}
					Normal_Attend_Dlg normal_Attend_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.EVENT_NORMAL_ATTEND) as Normal_Attend_Dlg;
					if (normal_Attend_Dlg != null)
					{
						normal_Attend_Dlg.Init_Consecutively_Attend();
					}
				}
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("214"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			}
		}

		public static void GS_WARP_ACK(NkDeserializePacket kDeserializePacket)
		{
			GS_WARP_ACK packet = kDeserializePacket.GetPacket<GS_WARP_ACK>();
			NrTSingleton<NkClientLogic>.Instance.SetWarpGateIndex(0);
			JoyStickDlg joyStickDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.JOYSTICK_DLG) as JoyStickDlg;
			if (joyStickDlg != null)
			{
				joyStickDlg.SetJoystickEnable(false);
			}
			if (packet.m_byResult != 0)
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_bNoMove = false;
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_nIndunUnique = -1;
				NrTSingleton<NkIndunManager>.Instance.Clear();
				if (packet.m_byResult == 700)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("202"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				}
				return;
			}
			NEW_MAKECHAR_INFO packet2 = kDeserializePacket.GetPacket<NEW_MAKECHAR_INFO>();
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"name =",
					TKString.NEWString(packet2.CharName),
					" CharKind kind = ",
					packet2.CharKind.ToString(),
					"  PacketInfo = ",
					kDeserializePacket.ToString()
				}));
			}
			NrTSingleton<NkClientLogic>.Instance.SetGameWorld(true);
			NrTSingleton<NkCharManager>.Instance.SetMyChar(packet);
			NrTSingleton<NkCharManager>.Instance.SetChar(packet2, false, false);
			if (packet.Mode == 1)
			{
				Scene.Type curScene = Scene.CurScene;
				if (curScene == Scene.Type.BATTLE)
				{
					StageSystem.ReservePopStage();
				}
			}
			else
			{
				if (NrTSingleton<NkQuestManager>.Instance.m_showMsgDlg)
				{
					NrTSingleton<NkClientLogic>.Instance.showDown = false;
					NrTSingleton<NkClientLogic>.Instance.SetWarp(true);
					StageSystem.ReloadStage();
				}
				StageSystem.ReloadStage();
			}
			if (Scene.IsCurScene(Scene.Type.WORLD))
			{
				bool flag = true;
				if (NrTSingleton<ContentsLimitManager>.Instance.IsTutorialBattleStart() && NrTSingleton<NkQuestManager>.Instance.IsWorldFirst())
				{
					flag = false;
				}
				if (flag)
				{
					NrTSingleton<NkQuestManager>.Instance.AutoQuestExcute();
				}
			}
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_nIndunUnique = packet.nIndunUnique;
		}

		public static void WS_ENCRYPTKEY_NFY(NkDeserializePacket kDeserializePacket)
		{
			WS_ENCRYPTKEY_NFY packet = kDeserializePacket.GetPacket<WS_ENCRYPTKEY_NFY>();
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				TsLog.LogWarning("RECEIVED WS_ENCRYPTKEY_NFY", new object[0]);
			}
			SendPacket.GetInstance().SetEncryptKey(packet.ui8Key1_send, packet.ui8Key2_send, false);
			if (NrTSingleton<CMovingServer>.Instance.IsMovingWorld())
			{
				NrTSingleton<CMovingServer>.Instance.OnReceivedEncryptKey();
				if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
				{
					TsLog.LogWarning("Reason : Moving World", new object[0]);
				}
			}
			else if (NrTSingleton<NkAutoRelogin>.Instance.IsWaitingEncryptKey())
			{
				NrTSingleton<NkAutoRelogin>.Instance.OnReceivedEncryptKey();
				if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
				{
					TsLog.LogWarning("Reason : Relogin", new object[0]);
				}
			}
			else
			{
				MsgHandler.Handle("Req_CLIENT_VERIFY_REQ", new object[0]);
			}
		}

		public static void WS_USER_HEARTBEAT_ACK(NkDeserializePacket kDeserializePacket)
		{
			WS_USER_HEARTBEAT_ACK packet = kDeserializePacket.GetPacket<WS_USER_HEARTBEAT_ACK>();
			NrTSingleton<NkClientLogic>.Instance.SetServerTickCount((ulong)packet.TickCount);
		}

		public static void WS_USER_PINGCOUNT_ACK(NkDeserializePacket kDeserializePacket)
		{
			NrTSingleton<NrMainSystem>.Instance.m_nWorldServerPingCount = 0;
		}

		public static void WS_CLIENT_VERIFY_ACK(NkDeserializePacket kDeserializePacket)
		{
			Debug.LogWarning("WS_CLIENT_VERIFY_ACK received.");
			WS_CLIENT_VERIFY_ACK packet = kDeserializePacket.GetPacket<WS_CLIENT_VERIFY_ACK>();
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					"WS_CLIENT_VERIFY_ACK. [RESULT: ",
					packet.Result,
					"] [Version: ",
					packet.nPatchVersion,
					"]"
				}));
			}
			if (packet.Result != 0 && !TsPlatform.IsEditor)
			{
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				string textFromPreloadText = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("2");
				string textFromPreloadText2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("1");
				msgBoxUI.SetMsg(new YesDelegate(NrReceiveGame.On_OK_URL), null, textFromPreloadText, textFromPreloadText2, eMsgType.MB_OK, 2);
				int num = (int)(GUICamera.width / 2f - msgBoxUI.GetSize().x / 2f);
				int num2 = (int)(GUICamera.height / 2f - msgBoxUI.GetSize().y / 2f);
				msgBoxUI.SetLocation((float)num, (float)num2, 50f);
				BaseNet_Game.GetInstance().Quit();
			}
			else if (!MsgHandler.Handle("RequestWorldLogin", new object[]
			{
				NrTSingleton<NrMainSystem>.Instance.m_strWorldServerIP,
				NrTSingleton<NrMainSystem>.Instance.m_nWorldServerPort
			}))
			{
			}
		}

		public static void WS_USER_LOGIN_ACK(NkDeserializePacket kDeserializePacket)
		{
			Debug.LogWarning("Login for world server.");
			WS_USER_LOGIN_ACK packet = kDeserializePacket.GetPacket<WS_USER_LOGIN_ACK>();
			Debug.Log("RECEIVE WS_USER_LOGIN_ACK [RESULT: " + packet.Result + "]");
			if (packet.Result != 0)
			{
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("27");
				string message = string.Empty;
				if (packet.Result == 41)
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref message, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("28"),
						"world",
						packet.i16WorldID_LoggedIn,
						"channel",
						packet.i16ChannelID_LoggedIn
					});
				}
				else if (packet.Result == 600)
				{
					textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("25");
					message = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("26");
				}
				else
				{
					message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("83");
				}
				msgBoxUI.SetMsg(null, null, textFromMessageBox, message, eMsgType.MB_OK, 2);
				int num = (int)(GUICamera.width / 2f - msgBoxUI.GetSize().x / 2f);
				int num2 = (int)(GUICamera.height / 2f - msgBoxUI.GetSize().y / 2f);
				msgBoxUI.SetLocation((float)num, (float)num2, 50f);
				BaseNet_Game.GetInstance().Quit();
				return;
			}
			NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_UID = packet.UID;
			NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_siAuthSessionKey = packet.SessionKey;
			NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_i64AccountWorldInfoKey = packet.i64AccountWorldInfoKey;
			NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nMasterLevel = packet.nMasterLevel;
			NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_szAuthKey = TKString.NEWString(packet.szNewAuthKey);
			NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nConfirmCheck = packet.nConfirmCheck;
			NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nGetConfirmItem = packet.nGetConfirmItem;
			NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_nChatBlockDate = packet.tUnblockDate;
			if (!TsPlatform.IsWeb)
			{
				NrTSingleton<NkAutoRelogin>.Instance.SetActivity(true);
			}
			NrTSingleton<NrMainSystem>.Instance.m_ReLogin = false;
			if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID == 0L)
			{
				WS_CHARLIST_REQ obj = new WS_CHARLIST_REQ();
				SendPacket.GetInstance().SendObject(16777256, obj);
				Debug.LogWarning("WS_CHARLIST_REQ SEND");
			}
			else
			{
				GS_AUTH_SESSION_REQ gS_AUTH_SESSION_REQ = new GS_AUTH_SESSION_REQ();
				gS_AUTH_SESSION_REQ.UID = NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_UID;
				gS_AUTH_SESSION_REQ.SessionKey = NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_siAuthSessionKey;
				gS_AUTH_SESSION_REQ.PersonID = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID;
				gS_AUTH_SESSION_REQ.nMode = 200;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_AUTH_SESSION_REQ, gS_AUTH_SESSION_REQ);
			}
			PlayerPrefs.SetString(NrPrefsKey.PLAYER_PREFS_MOBILEAUTHKEY, NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_szAuthKey);
			TsLog.LogWarning("PlayerPrefs.SetString AuthKey = {0}  GetString = {1}", new object[]
			{
				NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_szAuthKey,
				PlayerPrefs.GetString(NrPrefsKey.PLAYER_PREFS_MOBILEAUTHKEY)
			});
			PlayerPrefs.SetInt(NrPrefsKey.LAST_AUTH_PLATFORM, NrTSingleton<NkClientLogic>.Instance.GetAuthPlatformType());
			if (TsPlatform.IsAndroid && !TsPlatform.IsEditor)
			{
				eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
				if (!currentServiceArea.ToString().Contains("LOCAL"))
				{
					TsPlatform.Operator.RegisterPushToken();
				}
			}
			GameObject pkGoOminiata = StageSystemCheck.pkGoOminiata;
			if (pkGoOminiata != null)
			{
				OmniataComponent component = pkGoOminiata.GetComponent<OmniataComponent>();
				if (component)
				{
					DateTime dateTime = DateTime.Now.ToLocalTime();
					DateTime arg_3DC_0 = dateTime;
					DateTime dateTime2 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
					int num3 = (int)(arg_3DC_0 - dateTime2.ToLocalTime()).TotalSeconds;
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("ts", num3.ToString());
					dictionary.Add("step", "login");
					dictionary.Add("device", SystemInfo.deviceUniqueIdentifier);
					if (TsPlatform.IsAndroid)
					{
						dictionary.Add("version", TsPlatform.APP_VERSION_AND);
					}
					else if (TsPlatform.IsIPhone)
					{
						dictionary.Add("version", TsPlatform.APP_VERSION_IOS);
					}
					component.TrackLoad(dictionary);
				}
			}
			NrTSingleton<MATEventManager>.Instance.Set_UserID(NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_UID.ToString());
			NrTSingleton<MATEventManager>.Instance.MeasureEvent("Login");
		}

		public static void WS_USER_LOGIN_MOVING_WORLD_ACK(NkDeserializePacket kDeserializePacket)
		{
			WS_USER_LOGIN_MOVING_WORLD_ACK packet = kDeserializePacket.GetPacket<WS_USER_LOGIN_MOVING_WORLD_ACK>();
			Debug.Log("RECEIVE WS_USER_LOGIN_MOVING_WORLD_ACK [RESULT: " + packet.Result + "]");
			if (packet.Result != 0)
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.RECONNECT_DLG);
				return;
			}
			WS_DEVICEID_SET_REQ wS_DEVICEID_SET_REQ = new WS_DEVICEID_SET_REQ();
			if (TsPlatform.IsMobile && !TsPlatform.IsEditor)
			{
				TKString.StringChar(TsPlatform.Operator.GetMobileDeviceId(), ref wS_DEVICEID_SET_REQ.DeviceID);
			}
			SendPacket.GetInstance().SendObject(16777299, wS_DEVICEID_SET_REQ);
			NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_UID = packet.m_nUID;
			NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_siAuthSessionKey = packet.m_nSessionKey;
			GS_AUTH_SESSION_REQ gS_AUTH_SESSION_REQ = new GS_AUTH_SESSION_REQ();
			gS_AUTH_SESSION_REQ.UID = NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_UID;
			gS_AUTH_SESSION_REQ.SessionKey = NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.m_siAuthSessionKey;
			gS_AUTH_SESSION_REQ.PersonID = packet.PersonID;
			gS_AUTH_SESSION_REQ.nMode = 300;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_AUTH_SESSION_REQ, gS_AUTH_SESSION_REQ);
			if (!TsPlatform.IsWeb)
			{
				NrTSingleton<NkAutoRelogin>.Instance.SetActivity(true);
			}
		}

		public static void WS_NAME_DUPLICATION_CHECK_ACK(NkDeserializePacket kDeserializePacket)
		{
			WS_NAME_DUPLICATION_CHECK_ACK packet = kDeserializePacket.GetPacket<WS_NAME_DUPLICATION_CHECK_ACK>();
			if (packet.Result == 0)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("1");
				Main_UI_SystemMessage.ADDMessage(textFromNotify);
				StageSystem.CurrentStageHandleMessage("ConfirmCharName", new object[]
				{
					true
				});
			}
			else if (packet.Result == 2)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("90"));
			}
			else
			{
				string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("91");
				Main_UI_SystemMessage.ADDMessage(textFromNotify2);
			}
		}

		public static void WS_CREATE_CHAR_ACK(NkDeserializePacket kDeserializePacket)
		{
			WS_CREATE_CHAR_ACK packet = kDeserializePacket.GetPacket<WS_CREATE_CHAR_ACK>();
			if (packet.Result == 0)
			{
				WS_CHARLIST_ACK.NEW_CHARLIST_INFO nEW_CHARLIST_INFO = new WS_CHARLIST_ACK.NEW_CHARLIST_INFO();
				nEW_CHARLIST_INFO.PersonID = packet.PersonID;
				nEW_CHARLIST_INFO.szCharName = packet.szCharName;
				nEW_CHARLIST_INFO.Level = 1;
				nEW_CHARLIST_INFO.SolID = packet.SolID;
				nEW_CHARLIST_INFO.CharKind = packet.CharKind;
				nEW_CHARLIST_INFO.kBasePart.SetData(packet.kBasePart);
				nEW_CHARLIST_INFO.LastLoginTime = 0L;
				int id = NrTSingleton<NkCharManager>.Instance.SetChar(nEW_CHARLIST_INFO, true);
				NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(id) as NrCharUser;
				if (nrCharUser == null)
				{
					return;
				}
				NkSoldierInfo leaderSoldierInfo = nrCharUser.GetPersonInfo().GetLeaderSoldierInfo();
				if (leaderSoldierInfo == null)
				{
					return;
				}
				leaderSoldierInfo.UpdateSoldierStatInfo();
				string empty = string.Empty;
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("75");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromNotify,
					"Char_Name",
					TKString.NEWString(packet.szCharName)
				});
				Main_UI_SystemMessage.ADDMessage(empty);
				StageSystem.CurrentStageHandleMessage("CreateComplete", new object[]
				{
					true
				});
				if (packet.PersonID != 0L)
				{
					NrTSingleton<NrMainSystem>.Instance.SetLatestPersonID(packet.PersonID.ToString());
					MsgHandler.Handle("Req_CONNECT_GAMESERVER_REQ", new object[]
					{
						packet.PersonID
					});
				}
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(((eRESULT)packet.Result).ToString());
				NrTSingleton<NrLogSystem>.Instance.AddString("can not create your avatar.", true);
			}
		}

		public static void WS_DELETE_CHAR_ACK(NkDeserializePacket kDeserializePacket)
		{
			WS_DELETE_CHAR_ACK packet = kDeserializePacket.GetPacket<WS_DELETE_CHAR_ACK>();
			if ((int)packet.Result == 0)
			{
				NrCharBase charByPersonID = NrTSingleton<NkCharManager>.Instance.GetCharByPersonID(packet.nPersonID);
				if (charByPersonID != null)
				{
					NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.DelSlotChar(charByPersonID.GetID());
					NrTSingleton<NkCharManager>.Instance.ReleaseChar(charByPersonID);
					string empty = string.Empty;
					string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("76");
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						textFromNotify,
						"Char_Name",
						charByPersonID.GetPersonInfo().GetCharName()
					});
					Main_UI_SystemMessage.ADDMessage(empty);
					StageSystem.CurrentStageHandleMessage("DisplayAvatar", new object[]
					{
						true
					});
				}
			}
			else
			{
				sbyte result = packet.Result;
				if (result != -30)
				{
					if (result != -20)
					{
						if (result != -10)
						{
							if ((int)packet.Result == 36)
							{
								string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("265");
								Main_UI_SystemMessage.ADDMessage(textFromNotify2);
							}
							else
							{
								Main_UI_SystemMessage.ADDMessage("DELETE FAIL");
							}
						}
						else
						{
							Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("265"));
						}
					}
					else
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("374"));
					}
				}
				else
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("373"));
				}
			}
		}

		public static void WS_CHANNEL_LIST_ACK(NkDeserializePacket kDeserializePacket)
		{
			WS_CHANNEL_LIST_ACK packet = kDeserializePacket.GetPacket<WS_CHANNEL_LIST_ACK>();
			int numChannels = (int)packet.NumChannels;
			CHANNEL_STATE_INFO[] array = new CHANNEL_STATE_INFO[numChannels];
			for (int i = 0; i < numChannels; i++)
			{
				array[i] = kDeserializePacket.GetPacket<CHANNEL_STATE_INFO>();
			}
			ChannelMove_DLG channelMove_DLG = (ChannelMove_DLG)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CHANNEL_MOVE_DLG);
			if (channelMove_DLG != null)
			{
				channelMove_DLG.SetChannelInfo(array, numChannels);
				channelMove_DLG.Show();
			}
		}

		public static void WS_ALL_WORLDSERVER_INFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			WS_ALL_WORLDSERVER_INFO_ACK packet = kDeserializePacket.GetPacket<WS_ALL_WORLDSERVER_INFO_ACK>();
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			kMyCharInfo.m_szChannel = packet.byCHID.ToString();
			Client.m_MyWS = (long)packet.i16WSID;
			Client.m_MyCH = packet.byCHID;
			Client.m_WorldServer_Count = (int)packet.i16WS_INFO_Count;
			Client.m_WorldServer_Infos = new WorldServer_INFO[Client.m_WorldServer_Count];
			Client.m_MyOriginWS = packet.i32OrginWorldID;
			for (int i = 0; i < Client.m_WorldServer_Count; i++)
			{
				Client.m_WorldServer_Infos[i] = kDeserializePacket.GetPacket<WorldServer_INFO>();
				if (Client.m_MyWS == (long)Client.m_WorldServer_Infos[i].i32SVID)
				{
					kMyCharInfo.m_szServerName = TKString.NEWString(Client.m_WorldServer_Infos[i].szName);
					kMyCharInfo.m_szWorldType = TKString.NEWString(Client.m_WorldServer_Infos[i].szWorldType);
				}
			}
			char[] buffer = new char[16];
			int num = 0;
			bool flag = Client.GetInstance().Get_UnionWorldServer_Info(ref buffer, ref num);
			string text = TKString.NEWString(buffer);
			Debug.Log(string.Concat(new string[]
			{
				"Get_UnionWorldServer_Info szIP : ",
				text,
				" Port : ",
				num.ToString(),
				" BOOL : ",
				flag.ToString()
			}));
			flag = Client.GetInstance().Get_WorldServer_InfoFromID(1, ref buffer, ref num);
			string text2 = TKString.NEWString(buffer);
			Debug.Log(string.Concat(new string[]
			{
				"Get_WorldServer_InfoFromID szIP : ",
				text2,
				" Port : ",
				num.ToString(),
				" BOOL : ",
				flag.ToString()
			}));
		}

		public static void WS_CHARLIST_ACK(NkDeserializePacket kDeserializePacket)
		{
			WS_CHARLIST_ACK packet = kDeserializePacket.GetPacket<WS_CHARLIST_ACK>();
			if (packet.Result != 0)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("83");
				Main_UI_SystemMessage.ADDMessage(textFromNotify);
				return;
			}
			NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.InitSlotChar();
			long num = 0L;
			long num2 = 0L;
			WS_CHARLIST_ACK.NEW_CHARLIST_INFO nEW_CHARLIST_INFO = null;
			for (int i = 0; i < (int)packet.NumChars; i++)
			{
				WS_CHARLIST_ACK.NEW_CHARLIST_INFO packet2 = kDeserializePacket.GetPacket<WS_CHARLIST_ACK.NEW_CHARLIST_INFO>();
				bool bSelectChar = false;
				if (num < packet2.LastLoginTime)
				{
					num = packet2.LastLoginTime;
					if (packet2.LastLoginTime != 0L)
					{
						bSelectChar = true;
					}
				}
				NrTSingleton<NkCharManager>.Instance.SetChar(packet2, bSelectChar);
				if (num2 == 0L)
				{
					num2 = packet2.PersonID;
				}
				if (nEW_CHARLIST_INFO == null || (nEW_CHARLIST_INFO != null && nEW_CHARLIST_INFO.Level < packet2.Level))
				{
					nEW_CHARLIST_INFO = packet2;
				}
			}
			for (int j = 0; j < 3; j++)
			{
				int charID = NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.GetCharID(j + 1);
				NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(charID) as NrCharUser;
				if (nrCharUser != null)
				{
					NkSoldierInfo leaderSoldierInfo = nrCharUser.GetPersonInfo().GetLeaderSoldierInfo();
					if (leaderSoldierInfo != null)
					{
						leaderSoldierInfo.UpdateSoldierStatInfo();
					}
				}
			}
			NrTSingleton<NkCharManager>.Instance.CharacterListSetComplete = true;
			if (packet.NumChars > 0 && num2 != 0L)
			{
				NrCharUser nrCharUser2 = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
				if (nrCharUser2 != null && nrCharUser2.GetCharName().Contains("*"))
				{
					NrTSingleton<NkCharManager>.Instance.CharacterChangeName = true;
				}
				else
				{
					NrTSingleton<NrMainSystem>.Instance.SetLatestPersonID(nEW_CHARLIST_INFO.PersonID.ToString());
					MsgHandler.Handle("Req_CONNECT_GAMESERVER_REQ", new object[]
					{
						num2
					});
					FacadeHandler.MoveStage(Scene.Type.PREPAREGAME);
					NrTSingleton<NkQuestManager>.Instance.SortingQuestInGroup();
				}
			}
		}

		public static void WS_CHANGE_CHAR_NAME_ACK(NkDeserializePacket kDeserializePacket)
		{
			WS_CHANGE_CHAR_NAME_ACK packet = kDeserializePacket.GetPacket<WS_CHANGE_CHAR_NAME_ACK>();
			ChangeNameDlg changeNameDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHANGENAME_DLG) as ChangeNameDlg;
			if (changeNameDlg != null)
			{
				changeNameDlg.ChangeNameAck((long)packet.nResult);
			}
		}

		public static void WS_CONNECT_GAMESERVER_ACK(NkDeserializePacket kDeserializePacket)
		{
			WS_CONNECT_GAMESERVER_ACK packet = kDeserializePacket.GetPacket<WS_CONNECT_GAMESERVER_ACK>();
			Debug.LogWarning("WS_CONNECT_GAMESERVER_ACK. [RESULT:" + packet.Result + "]");
			if (packet.Result != 0)
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.RECONNECT_DLG);
			}
			else
			{
				Debug.LogWarning("###GAMESERVER CONNECT###");
				MsgHandler.Handle("Rcv_WS_CONNECT_GAMESERVER_ACK", new object[0]);
			}
		}

		public static void WS_USER_RELOGIN_ACK(NkDeserializePacket kDeserializePacket)
		{
			WS_USER_RELOGIN_ACK packet = kDeserializePacket.GetPacket<WS_USER_RELOGIN_ACK>();
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.WAIT_DLG);
			Debug.LogWarning("WS_USER_RELOGIN_ACK RESULT:" + packet.Result);
			if (packet.Result == 42)
			{
				if (TsPlatform.IsMobile)
				{
					NrTSingleton<NkAutoRelogin>.Instance.OnReloginFailed();
				}
				else
				{
					NrTSingleton<NrMainSystem>.Instance.ReLogin(true);
				}
			}
			else if (packet.Result != 0)
			{
				if (packet.Result == 600)
				{
					string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("25");
					string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("26");
					MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
					msgBoxUI.SetMsg(null, null, textFromMessageBox, textFromMessageBox2, eMsgType.MB_OK, 2);
					NrTSingleton<NrMainSystem>.Instance.ReLogin(true);
				}
				else if (TsPlatform.IsMobile)
				{
					NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.RECONNECT_DLG);
				}
				else
				{
					NrTSingleton<NrMainSystem>.Instance.ReLogin(true);
				}
			}
			else
			{
				NrTSingleton<NkAutoRelogin>.Instance.OnReloginSuccessed();
			}
		}

		public static void WS_USER_LOGIN_OUT_NFY(NkDeserializePacket kDeserializePacket)
		{
			WS_USER_LOGIN_OUT_NFY packet = kDeserializePacket.GetPacket<WS_USER_LOGIN_OUT_NFY>();
			if (packet.Result == 0)
			{
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				if (msgBoxUI != null)
				{
					msgBoxUI.SetMsg(new YesDelegate(NrReceiveGame.On_LoginOUT_OK), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("116"), eMsgType.MB_OK, 2);
				}
				else
				{
					BaseNet_Game.GetInstance().Quit();
				}
			}
		}

		public static void WS_PLATFORMID_SET_ACK(NkDeserializePacket kDeserializePacket)
		{
			WS_PLATFORMID_SET_ACK packet = kDeserializePacket.GetPacket<WS_PLATFORMID_SET_ACK>();
			Debug.LogWarning("WS_PLATFORMID_SET_ACK RESULT:" + packet.Result);
			if (packet.Result != 0)
			{
				string message = string.Empty;
				if (packet.Result == 2)
				{
					message = NrTSingleton<NrTextMgr>.Instance.GetTextFromPreloadText("Message5");
				}
				else if (packet.Result == -2)
				{
					message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("719");
					NmFacebookManager.instance.initFBUserData();
				}
				else
				{
					message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("145");
					NmFacebookManager.instance.initFBUserData();
				}
				NmFacebookManager.instance.FacebookActive = false;
				Main_UI_SystemMessage.ADDMessage(message);
			}
			else
			{
				NmFacebookManager.instance.FacebookActive = true;
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("716"));
			}
		}

		public static void WS_OTPAUTH_ACK(NkDeserializePacket kDeserializePacket)
		{
			WS_OTPAUTH_ACK packet = kDeserializePacket.GetPacket<WS_OTPAUTH_ACK>();
			Debug.LogWarning("WS_OTPAUTH_ACK RESULT:" + packet.Result);
			if (packet.Result != 0)
			{
				NrTSingleton<NkClientLogic>.Instance.SetOTPAuthKey(packet.nOTPRequestType, string.Empty);
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("508"));
			}
			else
			{
				NrTSingleton<NkClientLogic>.Instance.SetOTPAuthKey(packet.nOTPRequestType, TKString.NEWString(packet.szOTPAuthKey));
			}
		}

		public static void WS_PLAYLOCK_REWAED_ACK(NkDeserializePacket kDeserializePacket)
		{
			WS_PLAYLOCK_REWAED_ACK packet = kDeserializePacket.GetPacket<WS_PLAYLOCK_REWAED_ACK>();
			Debug.LogWarning("WS_PLAYLOCK_REWAED_ACK RESULT:" + packet.Result);
			if (packet.Result != 9999)
			{
				PlayerPrefs.SetInt(NrPrefsKey.PLAYER_PLAYLOCKDEVICEID_SEND, 1);
			}
		}

		public static void WS_COUPON_USE_ACK(NkDeserializePacket kDeserializePacket)
		{
			WS_COUPON_USE_ACK packet = kDeserializePacket.GetPacket<WS_COUPON_USE_ACK>();
			if (packet.i32Result != 0)
			{
				string empty = string.Empty;
				int i32Result = packet.i32Result;
				if (i32Result != -6)
				{
					if (i32Result != -5)
					{
						NrTSingleton<FormsManager>.Instance.ShowMessageBox(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("923"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("85"), eMsgType.MB_OK, null, null);
					}
					else
					{
						DateTime dueDate = PublicMethod.GetDueDate(packet.i64CreateDate_Limit);
						DateTime dueDate2 = PublicMethod.GetDueDate(packet.i64EndTime);
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("193"),
							"createyear",
							dueDate.Year,
							"createmonth",
							dueDate.Month,
							"createday",
							dueDate.Day,
							"expireyear",
							dueDate2.Year,
							"expiremonth",
							dueDate2.Month,
							"expireday",
							dueDate2.Day
						});
						NrTSingleton<FormsManager>.Instance.ShowMessageBox(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("556"), empty, eMsgType.MB_OK, null, null);
					}
				}
				else
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("192"),
						"minlevel",
						packet.i16StartLevel,
						"maxlevel",
						packet.i16EndLevel
					});
					NrTSingleton<FormsManager>.Instance.ShowMessageBox(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("556"), empty, eMsgType.MB_OK, null, null);
				}
			}
		}

		public static void On_OK(object a_oObject)
		{
			NrTSingleton<NrMainSystem>.Instance.QuitGame(false);
		}

		public static void On_OK_URL(object a_oObject)
		{
			string arg = "android";
			if (TsPlatform.IsIPhone)
			{
				arg = "ios";
			}
			string url = string.Format("http://{0}/mobile/updateurl.aspx?code={1}&platform={2}", NrGlobalReference.strWebPageDomain, NrGlobalReference.MOBILEID, arg);
			Application.OpenURL(url);
		}

		public static void On_LoginOUT_OK(object a_oObject)
		{
			NrMobileAuthSystem.Instance.Auth.DeleteAuthInfo();
			NrTSingleton<NrMainSystem>.Instance.QuitGame(false);
		}

		public static void WS_SUPPORTER_ADD_ACK(NkDeserializePacket kDeserializePacket)
		{
			WS_SUPPORTER_ADD_ACK packet = kDeserializePacket.GetPacket<WS_SUPPORTER_ADD_ACK>();
			SupporterSubDlg supporterSubDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SUPPORTERSUB_DLG) as SupporterSubDlg;
			if (supporterSubDlg != null)
			{
				supporterSubDlg.SetSUPPORTER_ADD_ACK(packet);
			}
		}

		public static void WS_CONTENTSLIMIT_ACK(NkDeserializePacket kDeserializePacket)
		{
			WS_CONTENTSLIMIT_ACK packet = kDeserializePacket.GetPacket<WS_CONTENTSLIMIT_ACK>();
			if (0 < packet.i16Count)
			{
				NrTSingleton<ContentsLimitManager>.Instance.Clear();
			}
			for (int i = 0; i < (int)packet.i16Count; i++)
			{
				CONTENTSLIMIT_DATA packet2 = kDeserializePacket.GetPacket<CONTENTSLIMIT_DATA>();
				NrTSingleton<ContentsLimitManager>.Instance.AddLimitData(packet2);
			}
			if (packet.i8Reload == 1)
			{
				NrTSingleton<ContentsLimitManager>.Instance.SetReload();
			}
		}

		public static void WS_PLATFORM_INVITE_FRIENDS_ACK(NkDeserializePacket kDeserializePacket)
		{
			WS_PLATFORM_INVITE_FRIENDS_ACK packet = kDeserializePacket.GetPacket<WS_PLATFORM_INVITE_FRIENDS_ACK>();
			if (packet.i32Result == -10)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("624"));
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("629"));
			}
		}

		public static void WS_AUCTION_AUTHINFO_ACK(NkDeserializePacket kDeserializePacket)
		{
			WS_AUCTION_AUTHINFO_ACK packet = kDeserializePacket.GetPacket<WS_AUCTION_AUTHINFO_ACK>();
			if (packet.i32Result == 0)
			{
				NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.AUCTION_MAIN_DLG);
			}
			else
			{
				MainMenuDlg mainMenuDlg = (MainMenuDlg)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MAINMENU_DLG);
				if (mainMenuDlg != null)
				{
					mainMenuDlg.SetHP_AuthMessageBox();
				}
			}
		}

		public static void WS_INQUIRE_ANSWER_COUNT_ACK(NkDeserializePacket kDeserializePacket)
		{
			WS_INQUIRE_ANSWER_COUNT_ACK packet = kDeserializePacket.GetPacket<WS_INQUIRE_ANSWER_COUNT_ACK>();
			if (packet == null)
			{
				return;
			}
			Debug.Log("Customer Answer Count : " + packet.iAnswerCount);
			if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo() != null)
			{
				NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().CustomerAnswerCount = packet.iAnswerCount;
			}
		}
	}
}
