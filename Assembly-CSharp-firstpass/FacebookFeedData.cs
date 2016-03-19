using System;
using TsLibs;

public class FacebookFeedData : NrTableData
{
	public string Feed_Code = string.Empty;

	public string Code_Name = string.Empty;

	public string Game_ImageKey = string.Empty;

	public string Web_ImgageURL = string.Empty;

	public string Title_Text_Key = string.Empty;

	public string Msg_Text_Key = string.Empty;

	public string User_Message = string.Empty;

	public FacebookFeedData()
	{
		this.Init();
	}

	public void Init()
	{
		this.Feed_Code = string.Empty;
		this.Code_Name = string.Empty;
		this.Game_ImageKey = string.Empty;
		this.Web_ImgageURL = string.Empty;
		this.Title_Text_Key = string.Empty;
		this.Msg_Text_Key = string.Empty;
		this.User_Message = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		row.GetColumn(num++, out this.Feed_Code);
		row.GetColumn(num++, out this.Code_Name);
		row.GetColumn(num++, out this.Game_ImageKey);
		row.GetColumn(num++, out this.Web_ImgageURL);
		row.GetColumn(num++, out this.Title_Text_Key);
		row.GetColumn(num++, out this.Msg_Text_Key);
	}

	public void Copy(FacebookFeedData data)
	{
		this.Feed_Code = data.Feed_Code;
		this.Code_Name = data.Code_Name;
		this.Game_ImageKey = data.Game_ImageKey;
		this.Web_ImgageURL = data.Web_ImgageURL;
		this.Title_Text_Key = data.Title_Text_Key;
		this.Msg_Text_Key = data.Msg_Text_Key;
		this.User_Message = data.User_Message;
	}
}
