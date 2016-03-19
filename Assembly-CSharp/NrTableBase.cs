using System;
using System.IO;
using TsBundle;
using TsLibs;
using UnityEngine;
using UnityForms;

public abstract class NrTableBase : TsDataReader.IBindingRow, TsDataReader.IBinding
{
	public bool m_bFinishProcess;

	public string m_strFilePath = string.Empty;

	protected bool m_bUseNDT;

	public WWWItem m_wItem;

	private bool m_bForceNDT;

	public bool ForceUseNDT
	{
		get
		{
			return false;
		}
	}

	public NrTableBase(string strFileName)
	{
		this._Construct(strFileName);
	}

	public NrTableBase(string strFileName, bool bUseNDT)
	{
		if (!NrTSingleton<NrGlobalReference>.Instance.useCache)
		{
			this.m_bUseNDT = true;
		}
		else
		{
			this.m_bUseNDT = false;
		}
		this._Construct(strFileName);
	}

	private void _Construct(string strFileName)
	{
		string arg = string.Empty;
		string arg2 = string.Empty;
		if (Path.HasExtension(strFileName))
		{
			strFileName = Path.GetFileNameWithoutExtension(strFileName);
		}
		if (!NrTSingleton<NrGlobalReference>.Instance.useCache)
		{
			arg = CDefinePath.NDTPath();
			arg2 = ".ndt";
		}
		else
		{
			arg = CDefinePath.XMLBundlePath();
			arg2 = ".assetbundle";
			if (strFileName.Contains("/"))
			{
				int num = strFileName.IndexOf("/");
				if (num < strFileName.Length)
				{
					strFileName = strFileName.Substring(num + 1);
				}
			}
		}
		if (NrTSingleton<NrGlobalReference>.Instance.useCache && TsPlatform.IsMobile)
		{
			strFileName += "_mobile";
		}
		this.m_strFilePath = string.Format("{0}{1}{2}", arg, strFileName, arg2);
		this.m_wItem = this.CreateRequest();
	}

	private WWWItem CreateRequest()
	{
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(this.m_strFilePath, Option.defaultStackName);
		wWWItem.SetCallback(new PostProcPerItem(this._OnCompleteDownload), null);
		if (!NrTSingleton<NrGlobalReference>.Instance.useCache)
		{
			wWWItem.SetItemType(ItemType.USER_BYTESA);
		}
		else
		{
			wWWItem.SetItemType(ItemType.USER_ASSETB);
		}
		if (this.m_bForceNDT)
		{
			wWWItem.SetItemType(ItemType.USER_BYTESA);
		}
		return wWWItem;
	}

	public bool RequestDownload()
	{
		if (this.m_wItem != null)
		{
			TsImmortal.bundleService.RequestDownloadCoroutine(this.m_wItem, DownGroup.RUNTIME, true);
			return true;
		}
		return false;
	}

	public bool IsFinishProcess()
	{
		return this.m_bFinishProcess;
	}

	public virtual void Finish()
	{
	}

	public void SetFinishProcess()
	{
		this.m_bFinishProcess = true;
		this.Finish();
	}

	public bool ReadFrom(TsDataReader.Row tsRow)
	{
		this.ParseRowData(tsRow);
		return true;
	}

	public bool ReadFrom(TsDataReader dr)
	{
		return this.ParseDataFromNDT(dr);
	}

	private void _OnCompleteDownload(IDownloadedItem wItem, object obj)
	{
		try
		{
			if (!wItem.canAccessString)
			{
				if (wItem.canAccessBytes)
				{
					using (TsDataReader tsDataReader = new TsDataReader())
					{
						float realtimeSinceStartup = Time.realtimeSinceStartup;
						bool flag;
						if (tsDataReader.LoadFrom(wItem.safeBytes))
						{
							tsDataReader.BeginSection("[Table]");
							flag = this.ParseDataFromNDT(tsDataReader);
						}
						else
						{
							flag = false;
						}
						if (!flag)
						{
						}
						float num = Time.realtimeSinceStartup - realtimeSinceStartup;
						UIDataManager.fTotalTime += num;
					}
				}
				else if (wItem.mainAsset != null)
				{
					GameObject gameObject = wItem.mainAsset as GameObject;
					TsGameDataAdapter component = gameObject.GetComponent<TsGameDataAdapter>();
					TsGameData gameData = component.GameData;
					this.ParseDataFromBundle(gameData);
					wItem.unloadImmediate = true;
				}
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		finally
		{
			this.SetFinishProcess();
		}
	}

	public virtual bool ParseDataFromNDT(TsDataReader dr)
	{
		return false;
	}

	public void ParseDataFromBundle(TsGameData tsData)
	{
		try
		{
			switch (tsData.Format)
			{
			case TsGameData.FORMAT.TABLE:
				using (TsDataReader tsDataReader = new TsDataReader())
				{
					tsDataReader.LoadFrom(tsData.serializeGameDatas[0], "[Table]", this);
				}
				break;
			case TsGameData.FORMAT.NDT:
				using (TsDataReader tsDataReader2 = new TsDataReader())
				{
					if (tsDataReader2.LoadFrom(tsData.serializeGameDatas[0]))
					{
						tsDataReader2.BeginSection("[Table]");
						this.ParseDataFromNDT(tsDataReader2);
					}
				}
				break;
			}
		}
		catch (Exception ex)
		{
			TsLog.LogError(string.Concat(new object[]
			{
				"NrTableBase::__ParseDataFromTsGameData_TsDataReaderRow :",
				this.m_strFilePath,
				" - Exception:",
				ex
			}), new object[0]);
		}
	}

	public WWWItem GetWWWItem()
	{
		return this.m_wItem;
	}

	public bool ParseDataFromNDT_ForHelper<T>(TsDataReader dr) where T : NrTableData, new()
	{
		foreach (TsDataReader.Row data in dr)
		{
			T t = Activator.CreateInstance<T>();
			t.SetData(data);
			NrTSingleton<NrBaseTableManager>.Instance.SetData(t);
		}
		return true;
	}

	public virtual void ParseRowData(TsDataReader.Row row)
	{
		TsLog.LogWarning("Must ParseRowData", new object[0]);
	}

	public virtual bool LoadFromStream(Stream stream)
	{
		bool flag = false;
		using (TsDataReader tsDataReader = new TsDataReader())
		{
			flag = tsDataReader.LoadFrom(stream);
			tsDataReader.BeginSection("[Table]");
			flag = this.ParseDataFromNDT(tsDataReader);
		}
		if (!flag)
		{
			TsLog.LogError("##### Failed! LoadFromTableContext! - " + base.GetType().ToString(), new object[0]);
		}
		return flag;
	}

	public virtual bool LoadFromTableContext(string strContext)
	{
		bool flag = false;
		using (TsDataReader tsDataReader = new TsDataReader())
		{
			flag = tsDataReader.LoadFrom(strContext, "[Table]", this);
		}
		if (!flag)
		{
			TsLog.LogError("##### Failed! LoadFromTableContext! - " + base.GetType().ToString(), new object[0]);
		}
		return true;
	}
}
