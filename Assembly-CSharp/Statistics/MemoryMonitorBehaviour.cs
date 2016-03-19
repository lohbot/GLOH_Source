using ConsoleCommand.Statistics;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityForms;

namespace Statistics
{
	public class MemoryMonitorBehaviour : MonoBehaviour
	{
		private Vector2 m_ScrollPos;

		private List<int> m_growUpHistory;

		private int m_preTotalSize;

		private int m_curTotalSize;

		private StringBuilder m_sb;

		private static float GROWUP_RATE = 0.3f;

		private GUIStyle m_style;

		private string m_DisplayText;

		public GUITexture RedRing;

		private float m_TextureSizeRate;

		private int m_PreScreenHeight;

		private bool m_VisibleStatistics;

		private int m_EverageGrowUpBytes;

		private void Awake()
		{
			this.m_growUpHistory = new List<int>(10);
			this.m_sb = new StringBuilder(1024);
			this.m_curTotalSize = 0;
			this.m_preTotalSize = 0;
			this.RedRing = null;
			this.m_style = new GUIStyle();
			this.m_style.fontStyle = FontStyle.Bold;
			this.m_style.normal.textColor = new Color(0f, 0.635f, 0.909f);
			this.m_PreScreenHeight = -1;
			this.m_VisibleStatistics = true;
			this.m_EverageGrowUpBytes = 0;
		}

		private void Start()
		{
			if (this.RedRing == null)
			{
				return;
			}
			Texture texture = this.RedRing.texture;
			this.m_TextureSizeRate = (float)texture.width / (float)texture.height;
			this.RedRing.color = Color.red;
			this.RedRing.enabled = false;
			this.m_curTotalSize = MemoryCollection.Monitoring();
			float num = (float)MemoryMonitor.cycleTime * 60f;
			base.InvokeRepeating("Monitoring", num, num);
		}

		private void _ResizeTexture()
		{
			if (this.RedRing == null)
			{
				return;
			}
			int height = Screen.height;
			if (this.m_PreScreenHeight == height)
			{
				return;
			}
			int num = (int)((float)height * this.m_TextureSizeRate);
			this.RedRing.pixelInset = new Rect((float)(-(float)num / 2), (float)(-(float)height / 2), (float)num, (float)height);
		}

		private void Monitoring()
		{
			try
			{
				this.m_preTotalSize = this.m_curTotalSize;
				this.m_curTotalSize = MemoryCollection.Monitoring();
				if (this.m_growUpHistory.Count == 10)
				{
					this.m_growUpHistory.RemoveAt(0);
				}
				this.m_growUpHistory.Add(this.m_curTotalSize - this.m_preTotalSize);
				int num = 0;
				foreach (int current in this.m_growUpHistory)
				{
					num += current;
				}
				this.m_EverageGrowUpBytes = num / this.m_growUpHistory.Count;
				float num2 = (float)(this.m_curTotalSize - this.m_preTotalSize) / (float)this.m_preTotalSize;
				if (num2 > MemoryMonitorBehaviour.GROWUP_RATE || this.m_EverageGrowUpBytes > MemoryMonitor.growUpAllowedBytes)
				{
					this.OutputAlertText();
					this.ShowAlert(30f);
				}
				else
				{
					TsLog.Log("[MemoryMonitor] (^ㅁ^)/ \"좋아요. 메모리 증가가 크지 않습니다.\" (증가율={0}%)", new object[]
					{
						(int)(num2 * 100f)
					});
				}
			}
			catch (DivideByZeroException ex)
			{
				TsLog.LogWarning("[MemoryMonitor] 이전에 수집된 로딩된 오브젝트들의 메모리 크기가 0입니다. ({0})", new object[]
				{
					ex
				});
			}
			catch (Exception ex2)
			{
				TsLog.LogWarning("[MemoryMonitor] {0}", new object[]
				{
					ex2
				});
			}
		}

		private void HideAlert()
		{
			if (this.RedRing != null)
			{
				this.RedRing.enabled = false;
			}
			if (this.m_VisibleStatistics)
			{
				NrTSingleton<FormsManager>.Instance.ShowAll();
			}
		}

		public void ShowAlert(float displayTime = 30f)
		{
			if (this.RedRing == null)
			{
				return;
			}
			this.RedRing.enabled = true;
			this._ResizeTexture();
			this._ShowForms();
			base.Invoke("HideAlert", displayTime);
		}

		private void OutputAlertText()
		{
			this.m_sb.Length = 0;
			this.m_sb.AppendFormat("{0}분 전의 메모리 사용량 보다 {1}% 증가되었습니다. (현재 메모리={2} bytes, 이전 메모리={3} bytes)\n", new object[]
			{
				MemoryMonitor.cycleTime,
				(int)((float)(this.m_curTotalSize - this.m_preTotalSize) / (float)this.m_preTotalSize * 100f),
				this.m_curTotalSize.ToString("###,###,###"),
				this.m_preTotalSize.ToString("###,###,###")
			});
			this.m_sb.AppendFormat("최근 {0}회 동안 평균 메모리 증가량 = {1} bytes{2}\n", this.m_growUpHistory.Count, this.m_EverageGrowUpBytes.ToString("###,###,###"), (this.m_EverageGrowUpBytes <= MemoryMonitor.growUpAllowedBytes) ? string.Empty : " => 평균 메모리 증가량이 너무 높습니다. (주의)");
			this.m_sb.AppendLine();
			foreach (CollectionSummary current in MemoryCollection.CollectionSummaries())
			{
				this.m_sb.AppendLine("=========================================================================");
				this.m_sb.AppendFormat("[{0}] 오브젝트 총 개수={1}, 오브젝트 총 크기={2} bytes\n", current.collectionType, current.cllectedCount, (current.collectedSize != 0) ? current.collectedSize.ToString("###,###,###") : "<unknown>");
				int num = 0;
				this.m_sb.AppendLine("=========================================================================");
				this.m_sb.AppendLine("<<새로운 오브젝트 중 Top 10>>");
				foreach (LoadedUnityItem current2 in current.TopRanker(10, true))
				{
					this.m_sb.AppendFormat("   # {0}. \"{1}\" ( {2} bytes )\n", ++num, current2.objectName, (current2.objectBytes != 0) ? current2.objectBytes.ToString("###,###,###") : "0");
				}
				this.m_sb.AppendLine();
				num = 0;
				this.m_sb.AppendLine("<<모든 오브젝트 중 Top 10>>");
				foreach (LoadedUnityItem current3 in current.TopRanker(10, false))
				{
					this.m_sb.AppendFormat("   [{4}] \"{1}\" ( Hits = {3}, Size = {2} bytes )\n", new object[]
					{
						++num,
						current3.objectName,
						(current3.objectBytes != 0) ? current3.objectBytes.ToString("###,###,###") : "0",
						current3.hitCount,
						current3.objectType
					});
				}
				this.m_sb.AppendLine();
				int newlyObjectTotalSize = current.GetNewlyObjectTotalSize();
				int leakObjectTotalSize = current.GetLeakObjectTotalSize();
				this.m_sb.AppendFormat("새로운 오브젝트 총 용량 = {0} bytes\n", (newlyObjectTotalSize != 0) ? newlyObjectTotalSize.ToString("###,###,###") : "?");
				this.m_sb.AppendFormat("릭 오브젝트 총 용량 = {0} bytes\n", (leakObjectTotalSize != 0) ? leakObjectTotalSize.ToString("###,###,###") : "?");
				this.m_sb.AppendFormat("모든 오브젝트 총 용량 = {0} bytes\n", (current.collectedSize != 0) ? current.collectedSize.ToString("###,###,###") : "?");
				this.m_sb.AppendLine();
			}
			this.m_sb.AppendLine("=========================================================================");
			this.m_sb.AppendFormat("현재 사용중인 메모리 크기 (heap size) = {0} bytes", Profiler.usedHeapSize.ToString("#,###,###,###"));
			this.m_sb.AppendLine();
			this.m_DisplayText = this.m_sb.ToString();
			string text = DateTime.Now.ToString();
			TsLog.Log("[MemoryMonitor] {0}\n{1}", new object[]
			{
				text,
				this.m_DisplayText
			});
		}

		private void _ShowForms()
		{
			if (NrTSingleton<FormsManager>.Instance.isShowAllForm == !this.m_VisibleStatistics)
			{
				return;
			}
			if (this.m_VisibleStatistics)
			{
				NrTSingleton<FormsManager>.Instance.HideAll();
			}
			else
			{
				NrTSingleton<FormsManager>.Instance.ShowAll();
			}
		}
	}
}
