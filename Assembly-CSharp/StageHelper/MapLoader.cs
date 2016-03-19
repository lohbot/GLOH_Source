using Ndoors.Framework.CoTask;
using Ndoors.Framework.Stage;
using System;
using System.Collections;
using System.Diagnostics;
using TsBundle;

namespace StageHelper
{
	public struct MapLoader
	{
		private AStage _stgYldr;

		private MAP_INFO _mapinfo;

		public MapLoader(AStage stg, MAP_INFO mapinfo)
		{
			this._stgYldr = stg;
			this._mapinfo = mapinfo;
		}

		public void Reset()
		{
			this._mapinfo = null;
		}

		[DebuggerHidden]
		public IEnumerator StartLoadMap()
		{
			MapLoader.<StartLoadMap>c__Iterator21 <StartLoadMap>c__Iterator = new MapLoader.<StartLoadMap>c__Iterator21();
			<StartLoadMap>c__Iterator.<>f__this = this;
			return <StartLoadMap>c__Iterator;
		}

		private BaseCoTask _GetTable()
		{
			string strFileName = string.Format("{0}/TI_{1}", CDefinePath.TILEINFO_BASE_URL, this._mapinfo.TILE_INFO);
			TableInspector tableInspector = new TableInspector(this._stgYldr, true);
			tableInspector.Regist(new NkTableMapTileInfo(strFileName, true));
			return tableInspector;
		}

		private BaseCoTask _GetEventTrigger(int mapidx)
		{
			EventTriggerMapManager.Instance.LoadMapTrigger(mapidx);
			if (EventTriggerStageLoader.HasLoadItem)
			{
				TsLog.Log("OOOOOO --- MAPLOAD START EventTrigger!", new object[0]);
				return new WaitTask(EventTriggerStageLoader.LoadEventTrigger());
			}
			TsLog.Log("XXXXXX --- MAPLOAD SKIP! Event Trigger", new object[0]);
			return null;
		}

		private BaseCoTask _GetMapLoader()
		{
			if (!string.IsNullOrEmpty(this._mapinfo.BUNDLE_PATH))
			{
				TsLog.Log("--- MAPLOAD START Scene LoadLevelAdditive! = " + this._mapinfo.BUNDLE_PATH, new object[0]);
				return new WaitTask(CommonTasks.LoadLevelSubScene(this._mapinfo.GetBundlePath(), Option.IndependentFromStageStackName));
			}
			TsLog.Log("--- MAPLOAD SKIP Map Scene!", new object[0]);
			return null;
		}

		private BaseCoTask _GetPortalLoader()
		{
			if (Scene.CurScene != Scene.Type.WORLD)
			{
				TsLog.Log("--- MAPLOAD SKIP Portal!", new object[0]);
				return null;
			}
			TsLog.Log("--- MAPLOAD START Portal!", new object[0]);
			return new WaitTask(this._DownloadPortal());
		}

		[DebuggerHidden]
		private IEnumerator _DownloadPortal()
		{
			MapLoader.<_DownloadPortal>c__Iterator22 <_DownloadPortal>c__Iterator = new MapLoader.<_DownloadPortal>c__Iterator22();
			<_DownloadPortal>c__Iterator.<>f__this = this;
			return <_DownloadPortal>c__Iterator;
		}

		private BaseCoTask _GetRoadPoint()
		{
			string strURL = string.Format("{0}/RP_{1}", CDefinePath.ROADPOINT_BASE_URL, this._mapinfo.TILE_INFO);
			TableInspector tableInspector = new TableInspector(this._stgYldr, true);
			tableInspector.Regist(new NkTableRoadPoint(strURL));
			return tableInspector;
		}
	}
}
