using System;
using UnityEngine;

public sealed class TsForceIncludeShaders : MonoBehaviour
{
	[SerializeField]
	private Shader m_shader1;

	[SerializeField]
	private Shader m_shader2;

	[SerializeField]
	private Shader m_shader3;

	[SerializeField]
	private Shader m_shader4;

	[SerializeField]
	private Shader m_shader5;

	[SerializeField]
	private Shader m_shader6;

	[SerializeField]
	private Shader m_shader7;

	[SerializeField]
	private Shader m_shader8;

	[SerializeField]
	private Shader m_shader9;

	[SerializeField]
	private Shader m_shader10;

	private void Awake()
	{
		if (this.m_shader1 == null)
		{
			TsLog.LogError("Inspector 창에서 TsForceIncludeShaders 컴포넌트를 Reset 시켜주세요.", new object[0]);
			this._AssignShders();
		}
	}

	public void Reset()
	{
		this._AssignShders();
	}

	private void _AssignShders()
	{
		this.m_shader1 = Shader.Find("Nature/Tree Soft Occlusion Leaves");
		this.m_shader2 = Shader.Find("Nature/Tree Soft Occlusion Bark");
		this.m_shader3 = Shader.Find("Nature/Tree Creator Bark");
		this.m_shader4 = Shader.Find("Nature/Tree Creator Leaves");
		this.m_shader5 = Shader.Find("Hidden/Nature/Tree Soft Occlusion Leaves Rendertex");
		this.m_shader6 = Shader.Find("Hidden/Nature/Tree Soft Occlusion Bark Rendertex");
		this.m_shader7 = Shader.Find("Hidden/Nature/Tree Creator Bark Rendertex");
		this.m_shader8 = Shader.Find("Hidden/Nature/Tree Creator Leaves Rendertex");
		this.m_shader9 = Shader.Find("Hidden/Nature/Tree Creator Bark Optimized");
		this.m_shader10 = Shader.Find("Hidden/Nature/Tree Creator Leaves Optimized");
	}
}
