using System;

public interface IKeyFocusable
{
	string Content
	{
		get;
	}

	string OriginalContent
	{
		get;
		set;
	}

	bool EnterMode
	{
		get;
		set;
	}

	bool NumberMode
	{
		get;
		set;
	}

	long MaxValue
	{
		get;
		set;
	}

	long MinValue
	{
		get;
		set;
	}

	int MaxLineCount
	{
		get;
		set;
	}

	void LostFocus();

	string SetInputText(string inputText, ref int insertPt);

	string SetInputTextNoComposition(string inputText, ref int insertPt);

	string GetInputText(ref KEYBOARD_INFO info);

	void Commit();

	void SetCommitDelegate(EZKeyboardCommitDelegate del);

	void ToggleCaretShow();

	void GoUp();

	void GoDown();
}
