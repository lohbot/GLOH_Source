using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class IUIIME
{
	private const int GW_HWNDFIRST = 0;

	private const int GW_HWNDLAST = 1;

	private const int GW_HWNDNEXT = 2;

	private const int GW_HWNDPREV = 3;

	private const int GW_OWNER = 4;

	private const int GW_CHILD = 5;

	private const int WM_IME_COMPOSITION = 271;

	public const int GCS_RESULTSTR = 2048;

	public const int GCS_COMPSTR = 8;

	private const int IMM_ERROR_NODATA = -1;

	private const int IMM_ERROR_GENERAL = -2;

	private const int MAX_COMPSTRING_LENGTH = 32;

	private IntPtr m_hWnd = IntPtr.Zero;

	private IntPtr m_hChildWnd = IntPtr.Zero;

	private bool m_bReayComposition;

	private int m_siCompStatus;

	private string m_szCompString;

	[DllImport("user32")]
	private static extern IntPtr GetForegroundWindow();

	[DllImport("user32")]
	private static extern IntPtr GetActiveWindow();

	[DllImport("user32")]
	private static extern IntPtr GetWindow(IntPtr hWnd, int nCmd);

	[DllImport("user32")]
	private static extern IntPtr FindWindow([MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpClassName, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpWindowName);

	[DllImport("user32")]
	private static extern IntPtr FindWindowEx(IntPtr hWnd, IntPtr hChildWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpClassName, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpWindowName);

	[DllImport("user32")]
	private static extern int GetWindowText(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpWindowName, int nLength);

	[DllImport("UnityImeDLL")]
	private static extern int imeStartWndProcByHwnd(IntPtr hWnd);

	[DllImport("UnityImeDLL")]
	private static extern IntPtr imeGetHWND();

	[DllImport("UnityImeDLL")]
	private static extern IntPtr imeGetHIMC();

	[DllImport("UnityImeDLL")]
	private static extern int imeGetCompStatus();

	[DllImport("UnityImeDLL")]
	private static extern int imeGetCompLength();

	[DllImport("UnityImeDLL", CharSet = CharSet.Auto)]
	private static extern int imeGetCompositionString(byte[] lpStr);

	[DllImport("UnityImeDLL")]
	private static extern void Release();

	~IUIIME()
	{
		IUIIME.Release();
	}

	public void Start()
	{
		this.m_hWnd = IUIIME.GetForegroundWindow();
		if (this.m_hWnd == IntPtr.Zero)
		{
			this.m_bReayComposition = false;
			TsLog.Log("FindWindow failed!", new object[0]);
			return;
		}
		switch (Application.platform)
		{
		case RuntimePlatform.WindowsWebPlayer:
		{
			TsLog.Log("FindWindow started!", new object[0]);
			StringBuilder stringBuilder = new StringBuilder(32);
			stringBuilder.Append("UnityWindow");
			TsLog.Log("FindWindow = " + this.m_hWnd.ToString(), new object[0]);
			this.m_hWnd = this.FindChildWindowEX(this.m_hWnd, stringBuilder);
			break;
		}
		case RuntimePlatform.WindowsEditor:
			this.m_hChildWnd = IUIIME.GetWindow(this.m_hWnd, 5);
			this.m_hChildWnd = IUIIME.GetWindow(this.m_hChildWnd, 1);
			this.m_hChildWnd = IUIIME.GetWindow(this.m_hChildWnd, 3);
			if (this.m_hChildWnd != IntPtr.Zero)
			{
				this.m_hWnd = this.m_hChildWnd;
			}
			break;
		}
		if (this.m_hWnd == IntPtr.Zero)
		{
			this.m_bReayComposition = false;
			TsLog.Log("FindWindow failed!", new object[0]);
			return;
		}
		this.m_siCompStatus = 0;
		this.m_szCompString = string.Empty;
		int num = IUIIME.imeStartWndProcByHwnd(this.m_hWnd);
		if (num == 1)
		{
			this.m_bReayComposition = true;
			TsLog.Log("FindWindow OK!", new object[0]);
		}
		else
		{
			this.m_bReayComposition = false;
		}
	}

	public IntPtr FindChildWindowEX(IntPtr hParentWnd, StringBuilder winname)
	{
		StringBuilder stringBuilder = new StringBuilder(128);
		IntPtr intPtr = IUIIME.FindWindowEx(hParentWnd, IntPtr.Zero, null, winname);
		if (intPtr == IntPtr.Zero)
		{
			IntPtr window = IUIIME.GetWindow(hParentWnd, 5);
			IUIIME.GetWindowText(window, stringBuilder, 128);
			TsLog.Log("FindWindow = " + stringBuilder.ToString(), new object[0]);
			if (window == IntPtr.Zero)
			{
				TsLog.Log("FindWindow = " + this.m_hWnd.ToString(), new object[0]);
				return intPtr;
			}
			IntPtr window2 = IUIIME.GetWindow(window, 1);
			for (int i = 0; i < 10; i++)
			{
				intPtr = this.FindChildWindowEX(window, winname);
				if (intPtr == IntPtr.Zero)
				{
					if (window == window2)
					{
						break;
					}
					window = IUIIME.GetWindow(window, 2);
					IUIIME.GetWindowText(window, stringBuilder, 128);
					TsLog.Log("FindWindow = " + stringBuilder.ToString(), new object[0]);
				}
			}
		}
		return intPtr;
	}

	public bool IsReadyComposition()
	{
		return this.m_bReayComposition;
	}

	public int GetCompStatus()
	{
		return this.m_siCompStatus;
	}

	public string GetCompositionString()
	{
		return this.m_szCompString;
	}

	public void CheckImeStatus()
	{
		this.m_siCompStatus = 0;
		this.m_szCompString = string.Empty;
		this.Update();
	}

	private void Update()
	{
		if (!this.m_bReayComposition)
		{
			return;
		}
		this.m_siCompStatus = IUIIME.imeGetCompStatus();
		if (this.m_siCompStatus > 0)
		{
			int num = IUIIME.imeGetCompLength();
			if (num > 0)
			{
				byte[] array = new byte[num + 1];
				int num2 = IUIIME.imeGetCompositionString(array);
				if (num2 == 1)
				{
					this.m_szCompString = Encoding.Unicode.GetString(array);
				}
			}
		}
	}
}
