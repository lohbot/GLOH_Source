using Ndoors.Framework.Stage;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class NrAutoPath : NrTSingleton<NrAutoPath>
{
	public const short STARTRPIDX = 10000;

	private NrMapLinker m_cMapLinker;

	private MapLinkAStar m_cMapAstar;

	private NrGateManager m_cGateMgr;

	private GxRoadPointManager m_cGxRoadPointMgr;

	private GxRoadPointAstar m_cGxRoadPointAstar;

	private LinkedList<GxRpAsNode> m_RPPath = new LinkedList<GxRpAsNode>();

	private LinkedList<GxRpAsNode> m_PathForward = new LinkedList<GxRpAsNode>();

	private LinkedList<GxRpAsNode> m_PathBackward = new LinkedList<GxRpAsNode>();

	private LinkedList<Vector3> m_MovePath = new LinkedList<Vector3>();

	private bool m_bAutoMove;

	private LinkedList<int> m_MapPath = new LinkedList<int>();

	public bool bGoNPC;

	private Vector3 m_v3Dest = Vector3.zero;

	private int m_iDestMapIdx;

	private int m_iCurMapIdx;

	public int m_iCurNextIdx;

	private GameObject m_pkUserObj;

	private NrCharBase m_MainUser;

	private NrCharMapInfo m_pkCharMapInfo;

	private List<GameObject> m_RPPoint = new List<GameObject>();

	private List<GameObject> m_LinkLine = new List<GameObject>();

	private GameObject m_ParentRP;

	private List<string> m_Debugstring = new List<string>();

	public int DestMapIdx
	{
		get
		{
			return this.m_iDestMapIdx;
		}
	}

	public Vector3 DestPoint
	{
		get
		{
			return this.m_v3Dest;
		}
	}

	public int CurMapIdx
	{
		get
		{
			return this.m_iCurMapIdx;
		}
	}

	private NrAutoPath()
	{
	}

	public void ClearUserData()
	{
		this.m_MainUser = null;
		this.m_pkUserObj = null;
		this.m_pkCharMapInfo = null;
	}

	public void initData()
	{
		this.m_cGateMgr = new NrGateManager();
		this.m_cMapLinker = new NrMapLinker();
		this.m_cMapLinker.SetGateManager(ref this.m_cGateMgr);
		this.m_cMapAstar = new MapLinkAStar();
		this.m_cMapAstar.SetMapData(ref this.m_cMapLinker);
		this.m_cGxRoadPointMgr = NrTSingleton<GxRoadPointManager>.Instance;
		this.m_cGxRoadPointAstar = new GxRoadPointAstar();
		this.m_cGxRoadPointAstar.m_pkRPSys = this.m_cGxRoadPointMgr;
		this.m_bAutoMove = false;
		this.m_pkUserObj = null;
		this.m_pkCharMapInfo = null;
	}

	private bool ReadyCharInfo()
	{
		if (this.m_pkUserObj == null)
		{
			GameObject myCharObject = NrTSingleton<NkCharManager>.Instance.GetMyCharObject();
			if (myCharObject == null)
			{
				this.m_Debugstring.Add("ReadyCharInfo: NOT FOUND USER ");
				return false;
			}
			this.m_MainUser = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			this.m_pkUserObj = myCharObject;
			this.m_pkCharMapInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kCharMapInfo;
		}
		return true;
	}

	public int Generate(int DestMapIndex, short DestX, short DestY)
	{
		return this.Generate(DestMapIndex, DestX, DestY, false);
	}

	public int Generate(int DestMapIndex, short DestX, short DestY, bool bFollowChar)
	{
		if (!this.ReadyCharInfo())
		{
			return 0;
		}
		if (!bFollowChar && this.m_MainUser != null && this.m_MainUser.IsCharKindATB(1L))
		{
			NrCharUser nrCharUser = (NrCharUser)this.m_MainUser;
			if (nrCharUser.GetFollowCharPersonID() > 0L)
			{
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.DLG_STOPAUTOMOVE);
				return 0;
			}
		}
		if (this.m_pkUserObj == null)
		{
			this.m_Debugstring.Add("ERROR USER NOT FOUND");
		}
		if (this.m_cGxRoadPointMgr == null)
		{
			this.m_Debugstring.Add("ERROR M_CGXROADPOINTMGR NOT FOUND");
		}
		int nMapIndex = this.m_pkCharMapInfo.m_nMapIndex;
		this.m_Debugstring.Add(string.Concat(new object[]
		{
			"AUTOPATH \n STARTMAP: ",
			nMapIndex,
			"\tDESTMAP:",
			DestMapIndex,
			" \nDEST POSX:",
			DestX,
			" POSY:",
			DestY
		}));
		NrTSingleton<NkQuestManager>.Instance.IncreaseQuestParamVal(2, 0L, 1L);
		Vector3 v3Dest = new Vector3((float)DestX, 0f, (float)DestY);
		this.m_v3Dest = v3Dest;
		this.m_iDestMapIdx = DestMapIndex;
		if (nMapIndex == DestMapIndex)
		{
			this.CurrentMapMove(DestX, DestY);
		}
		else
		{
			this.NextMapMove(nMapIndex, this.DestMapIdx);
		}
		this.ClearUserData();
		if (this.MovePathCount() > 0)
		{
			return 1;
		}
		return 0;
	}

	public LinkedList<GxRpAsNode> FindRPPath(NrCharBase kChar, short SrcX, short SrcY, short DestX, short DestY)
	{
		LinkedList<GxRpAsNode> result = new LinkedList<GxRpAsNode>();
		if (!this.ReadyCharInfo())
		{
			return result;
		}
		short nerestRP = this.m_cGxRoadPointMgr.GetNerestRP(0, SrcX, SrcY, false, false);
		short nerestRP2 = this.m_cGxRoadPointMgr.GetNerestRP(0, DestX, DestY, false, false);
		this.m_cGxRoadPointAstar.GeneratePath(0, nerestRP, nerestRP2, ref result);
		Vector3 vDest = new Vector3((float)DestX, 0f, (float)DestY);
		NrCharBase mainUser = this.m_MainUser;
		GameObject pkUserObj = this.m_pkUserObj;
		this.m_pkUserObj = kChar.GetCharObject();
		this.m_MainUser = kChar;
		this.DirectionCheck(vDest, ref result);
		this.m_MainUser = mainUser;
		this.m_pkUserObj = pkUserObj;
		return result;
	}

	private void CurrentMapMove(short DestX, short DestY)
	{
		this.m_MapPath.Clear();
		this.m_RPPath.Clear();
		if (DestX == 0 && DestY == 0)
		{
			return;
		}
		short nerestRP = this.m_cGxRoadPointMgr.GetNerestRP(0, (short)this.m_pkUserObj.transform.position.x, (short)this.m_pkUserObj.transform.position.z, false, false);
		short nerestRP2 = this.m_cGxRoadPointMgr.GetNerestRP(0, DestX, DestY, false, false);
		Vector2 a = new Vector2(this.m_pkUserObj.transform.position.x, this.m_pkUserObj.transform.position.z);
		Vector2 b = new Vector2(this.m_cGxRoadPointMgr.GetRP(0, (int)nerestRP).GetPos().x, this.m_cGxRoadPointMgr.GetRP(0, (int)nerestRP).GetPos().z);
		if (Vector2.Distance(a, b) < Vector2.Distance(a, new Vector2(this.m_v3Dest.x, this.m_v3Dest.z)))
		{
			this.GenerateMinLenPath(nerestRP, nerestRP2, ref this.m_RPPath);
			this.SetMovePath(this.m_v3Dest);
		}
		else
		{
			this.SetMovePath(this.m_v3Dest);
		}
	}

	private void NextMapMove(int StartMapIdx, int DestMapIndex)
	{
		this.m_MapPath.Clear();
		this.m_Debugstring.Add("MAP LINK ASTAR");
		this.m_cMapAstar.Generate(StartMapIdx, DestMapIndex, ref this.m_MapPath);
		if (this.m_MapPath.Count == 0)
		{
			return;
		}
		int value = this.m_MapPath.First.Next.Value;
		this.m_iCurNextIdx = value;
		this.m_RPPath.Clear();
		Vector2 pos = this.m_cGateMgr.GetPos(StartMapIdx, value);
		this.m_Debugstring.Add("GATE POS:" + pos);
		short nerestRP = this.m_cGxRoadPointMgr.GetNerestRP(0, (short)this.m_pkUserObj.transform.position.x, (short)this.m_pkUserObj.transform.position.z, false, false);
		short nerestRP2 = this.m_cGxRoadPointMgr.GetNerestRP(0, (short)pos.x, (short)pos.y, false, false);
		this.GenerateMinLenPath(nerestRP, nerestRP2, ref this.m_RPPath);
		Vector3 vector = new Vector3(pos.x, 0f, pos.y);
		vector.y = NrCharMove.CalcHeight(vector);
		this.SetMovePath(vector);
	}

	public bool GenerateMinLenPath(short l_StartIdx, short l_Destindex, ref LinkedList<GxRpAsNode> RPPath)
	{
		if (l_StartIdx == l_Destindex)
		{
			return true;
		}
		this.m_PathForward.Clear();
		this.m_cGxRoadPointAstar.GeneratePath(0, l_StartIdx, l_Destindex, ref this.m_PathForward);
		this.m_PathBackward.Clear();
		this.m_cGxRoadPointAstar.GeneratePath(0, l_Destindex, l_StartIdx, ref this.m_PathBackward);
		float num = 0f;
		float num2 = 0f;
		GxRP gxRP = null;
		foreach (GxRpAsNode current in this.m_PathForward)
		{
			GxRP rP = this.m_cGxRoadPointMgr.GetRP(0, (int)current.RPIdx);
			Vector2 pos = rP.GetPos2();
			if (gxRP != null)
			{
				Vector2 pos2 = gxRP.GetPos2();
				num += Vector2.Distance(pos, pos2);
			}
			gxRP = rP;
		}
		gxRP = null;
		foreach (GxRpAsNode current2 in this.m_PathBackward)
		{
			GxRP rP2 = this.m_cGxRoadPointMgr.GetRP(0, (int)current2.RPIdx);
			Vector2 pos3 = rP2.GetPos2();
			if (gxRP != null)
			{
				Vector2 pos4 = gxRP.GetPos2();
				num2 += Vector2.Distance(pos3, pos4);
			}
			gxRP = rP2;
		}
		if (num != 0f && num2 != 0f)
		{
			if (num <= num2)
			{
				foreach (GxRpAsNode current3 in this.m_PathForward)
				{
					RPPath.AddLast(current3);
				}
			}
			else
			{
				foreach (GxRpAsNode current4 in this.m_PathBackward)
				{
					RPPath.AddFirst(current4);
				}
			}
		}
		else if (num == 0f && num2 != 0f)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"NotFind Rp FOWARD Path : start ",
				l_StartIdx,
				" end",
				l_Destindex
			}));
			foreach (GxRpAsNode current5 in this.m_PathBackward)
			{
				RPPath.AddFirst(current5);
			}
		}
		else if (num != 0f && num2 == 0f)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"NotFind Rp BACK Path : start ",
				l_StartIdx,
				" end",
				l_Destindex
			}));
			foreach (GxRpAsNode current6 in this.m_PathForward)
			{
				RPPath.AddLast(current6);
			}
		}
		else
		{
			Debug.LogError(string.Concat(new object[]
			{
				"NotFind Rp Path : start ",
				l_StartIdx,
				" end",
				l_Destindex
			}));
		}
		return true;
	}

	public bool SceneChange()
	{
		try
		{
			if (!this.ReadyCharInfo())
			{
				this.m_Debugstring.Add("AUTO MOVE MAP PATH CNT: ReadyCharInfo() == false)");
				bool result = false;
				return result;
			}
			if (this.m_MainUser.IsCharKindATB(1L) && Scene.PreScene != Scene.Type.BATTLE)
			{
				NrCharUser nrCharUser = (NrCharUser)this.m_MainUser;
				if (nrCharUser.GetFollowCharPersonID() > 0L)
				{
					nrCharUser.RefreshFollowCharPos();
					bool result = true;
					return result;
				}
			}
			if (this.m_MapPath == null)
			{
				this.m_Debugstring.Add("m_MapPath is null");
				bool result = true;
				return result;
			}
			if (this.m_bAutoMove && this.m_MapPath.Count > 0 && !this.m_MainUser.m_kCharMove.AutoMoveTo(this.m_iDestMapIdx, (short)this.m_v3Dest.x, (short)this.m_v3Dest.z))
			{
				this.ResetData();
			}
		}
		catch (Exception ex)
		{
			this.m_Debugstring.Add("AutoPath " + ex.Message);
		}
		this.ClearUserData();
		return true;
	}

	public LinkedList<Vector3> GetMovePathList()
	{
		return this.m_MovePath;
	}

	public Vector3 PopMovePath()
	{
		if (this.m_MovePath.Count > 0)
		{
			Vector3 value = this.m_MovePath.First.Value;
			this.m_MovePath.RemoveFirst();
			return value;
		}
		return Vector3.zero;
	}

	public int MovePathCount()
	{
		return this.m_MovePath.Count;
	}

	private void SetMovePath(Vector3 vDestPos)
	{
		this.DirectionCheck(vDestPos, ref this.m_RPPath);
		this.m_MovePath.Clear();
		foreach (GxRpAsNode current in this.m_RPPath)
		{
			GxRP rP = this.m_cGxRoadPointMgr.GetRP(current.sMapIdx, (int)current.RPIdx);
			Vector3 pos = rP.GetPos();
			this.m_MovePath.AddLast(new Vector3(pos.x, pos.y, pos.z));
		}
		this.m_Debugstring.Add("DEST POS : " + vDestPos);
		if (vDestPos != Vector3.zero)
		{
			if (vDestPos.y == 0f)
			{
				vDestPos.y = NrCharMove.CalcHeight(vDestPos);
			}
			this.m_MovePath.AddLast(new Vector3(vDestPos.x, vDestPos.y, vDestPos.z));
		}
		if (this.m_MovePath.Count > 0)
		{
			this.m_bAutoMove = true;
			this.SetFollowHeroCamera(true);
		}
	}

	public int MapPathCount()
	{
		return this.m_MapPath.Count;
	}

	public void ResetData()
	{
		this.ClearUserData();
		this.m_iDestMapIdx = -1;
		this.m_v3Dest = Vector3.zero;
		this.m_MapPath.Clear();
		this.m_MovePath.Clear();
		this.m_bAutoMove = false;
		this.SetFollowHeroCamera(false);
	}

	public bool IsAutoMoving()
	{
		return this.m_bAutoMove;
	}

	public void DirectionCheck(Vector3 vDest, ref LinkedList<GxRpAsNode> RPPath)
	{
		if (RPPath.Count > 1)
		{
			GxRpAsNode value = RPPath.First.Value;
			GxRP rP = this.m_cGxRoadPointMgr.GetRP(value.sMapIdx, (int)value.RPIdx);
			value = RPPath.First.Next.Value;
			GxRP rP2 = this.m_cGxRoadPointMgr.GetRP(value.sMapIdx, (int)value.RPIdx);
			Vector3 position = this.m_pkUserObj.transform.position;
			Vector3 from = rP2.GetPos() - rP.GetPos();
			from.y = 0f;
			Vector3 to = position - rP.GetPos();
			to.y = 0f;
			float value2 = Vector3.Angle(from, to);
			if (Math.Abs(value2) < 90f)
			{
				RPPath.RemoveFirst();
			}
		}
		if (RPPath.Count > 1)
		{
			GxRpAsNode value3 = RPPath.Last.Value;
			GxRP rP3 = this.m_cGxRoadPointMgr.GetRP(value3.sMapIdx, (int)value3.RPIdx);
			value3 = RPPath.Last.Previous.Value;
			GxRP rP4 = this.m_cGxRoadPointMgr.GetRP(value3.sMapIdx, (int)value3.RPIdx);
			Vector3 from2 = rP4.GetPos() - rP3.GetPos();
			from2.y = 0f;
			Vector3 to2 = vDest - rP3.GetPos();
			to2.y = 0f;
			float value4 = Vector3.Angle(from2, to2);
			if (Math.Abs(value4) < 90f)
			{
				RPPath.RemoveLast();
			}
		}
		if (RPPath.Count == 1)
		{
			GxRpAsNode value5 = RPPath.First.Value;
			GxRP rP5 = this.m_cGxRoadPointMgr.GetRP(value5.sMapIdx, (int)value5.RPIdx);
			Vector3 position2 = this.m_pkUserObj.transform.position;
			Vector3 from3 = position2 - rP5.GetPos();
			from3.y = 0f;
			Vector3 to3 = vDest - rP5.GetPos();
			to3.y = 0f;
			float value6 = Vector3.Angle(from3, to3);
			if (Math.Abs(value6) < 90f)
			{
				RPPath.RemoveFirst();
			}
		}
	}

	public void ShowRPPoint()
	{
		List<GxRP> list = this.m_cGxRoadPointMgr.GetList(0);
		if (list == null)
		{
			return;
		}
		if (this.m_RPPoint.Count != 0)
		{
			return;
		}
		this.m_ParentRP = new GameObject();
		this.m_ParentRP.name = "AutoPath";
		for (int i = 1; i < list.Count; i++)
		{
			try
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(NrTSingleton<UIDataManager>.Instance.FilePath + "Prefabs/AutoPathPoint"), list[i].GetPos(), Quaternion.identity);
				gameObject.name = i.ToString();
				gameObject.transform.parent = this.m_ParentRP.transform;
				this.m_RPPoint.Add(gameObject);
				Material material = new Material(Shader.Find("Particles/Additive"));
				for (int j = 0; j < list[i].GetLinkedCount(); j++)
				{
					short linkedRP = list[i].GetLinkedRP(j);
					if (linkedRP != 0)
					{
						GameObject gameObject2 = new GameObject();
						LineRenderer lineRenderer = new LineRenderer();
						lineRenderer = gameObject2.AddComponent<LineRenderer>();
						lineRenderer.material = material;
						lineRenderer.SetColors(Color.red, Color.red);
						Vector3 pos = list[i].GetPos();
						Vector3 pos2 = list[(int)linkedRP].GetPos();
						pos.y += 1f;
						pos2.y += 1f;
						lineRenderer.SetPosition(0, pos);
						lineRenderer.SetPosition(1, pos2);
						gameObject2.transform.parent = gameObject.transform;
						Vector3 position = gameObject2.transform.position;
						position.y += 1f;
						gameObject2.transform.localPosition = position;
						this.m_LinkLine.Add(gameObject2);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"COUNT: ",
					i,
					"  \n",
					ex
				}));
			}
		}
	}

	public void ClearRPPoint()
	{
		foreach (GameObject current in this.m_LinkLine)
		{
			UnityEngine.Object.Destroy(current);
		}
		foreach (GameObject current2 in this.m_RPPoint)
		{
			UnityEngine.Object.Destroy(current2);
		}
		this.m_RPPoint.Clear();
		this.m_LinkLine.Clear();
		UnityEngine.Object.DestroyObject(this.m_ParentRP);
	}

	public string GetDebug()
	{
		string text = string.Empty;
		foreach (string current in this.m_Debugstring)
		{
			text += current;
			text += "\n";
		}
		return text;
	}

	public void AddDebug(string str)
	{
		this.m_Debugstring.Add(str);
	}

	public void SetFollowHeroCamera(bool bSet)
	{
		if (Camera.main != null)
		{
			maxCamera component = Camera.main.GetComponent<maxCamera>();
			if (component != null)
			{
				component.SetFollowHero(bSet);
			}
		}
	}
}
