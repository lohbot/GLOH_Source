using System;
using System.Collections;
using UnityEngine;

[AddComponentMenu("TsScripts/Auto Placement")]
public class TsAutoPlacement : MonoBehaviour
{
	public GameObject m_SimpleSource;

	public GameObject[] m_Sources;

	public bool m_Active;

	public bool m_Animation = true;

	public Vector2 m_Gap = new Vector2(0f, 0f);

	public Vector2 m_RepeatCount = new Vector2(10f, 10f);

	public bool m_RandomPlacement;

	private ArrayList m_Testers;

	private bool m_OldActive;

	private bool m_OldAnimation;

	private void Start()
	{
		this._Init();
		this._Generate();
	}

	private void _Init()
	{
		this.m_OldActive = this.m_Active;
		this.m_OldAnimation = this.m_Animation;
		this.m_Testers = new ArrayList();
	}

	private void _Generate()
	{
		if (this.m_SimpleSource == null && this.m_Sources.Length == 0)
		{
			return;
		}
		Vector2 vector;
		vector.x = 0f;
		vector.y = 0f;
		if (this.m_RepeatCount.x > 1f)
		{
			vector.x = this.m_RepeatCount.x * this.m_Gap.x * -0.5f;
		}
		if (this.m_RepeatCount.y > 1f)
		{
			vector.y = this.m_RepeatCount.y * this.m_Gap.y * -0.5f;
		}
		int num = (!this.m_RandomPlacement) ? 0 : this.m_Sources.Length;
		bool flag = this.m_Sources.Length > 0;
		int num2 = this.m_Sources.Length - 1;
		int num3 = 0;
		this.m_Testers.Clear();
		for (int i = 0; i < (int)this.m_RepeatCount.y; i++)
		{
			for (int j = 0; j < (int)this.m_RepeatCount.x; j++)
			{
				Vector3 position = base.transform.position;
				position.x += this.m_Gap.x * (float)j + vector.x;
				position.z += this.m_Gap.y * (float)i + vector.y;
				GameObject gameObject;
				if (num > 0)
				{
					int num4 = UnityEngine.Random.Range(0, num);
					gameObject = this.m_Sources[num4];
				}
				else if (flag)
				{
					gameObject = this.m_Sources[num3];
					if (++num3 > num2)
					{
						num3 = 0;
					}
				}
				else
				{
					gameObject = this.m_SimpleSource;
				}
				if (gameObject != null)
				{
					GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject, position, gameObject.transform.rotation);
					gameObject2.transform.parent = base.transform;
					gameObject2.SetActive(this.m_Active);
					this.m_Testers.Add(gameObject2);
				}
			}
		}
	}

	private void Update()
	{
		bool flag = false;
		if (this.m_Active != this.m_OldActive)
		{
			foreach (GameObject gameObject in this.m_Testers)
			{
				gameObject.SetActive(this.m_Active);
			}
			this.m_OldActive = this.m_Active;
			flag = true;
		}
		if (flag || (this.m_Active && this.m_Animation != this.m_OldAnimation))
		{
			foreach (GameObject gameObject2 in this.m_Testers)
			{
				Animation[] componentsInChildren = gameObject2.GetComponentsInChildren<Animation>();
				Animation[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					Animation animation = array[i];
					animation.enabled = this.m_Animation;
				}
			}
			this.m_OldAnimation = this.m_Animation;
		}
	}
}
