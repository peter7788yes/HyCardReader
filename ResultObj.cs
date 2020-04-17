using CardInterface;
using System.Collections.Generic;

public class ResultObj
{
	private bool m_isSuccess;

	private ErrorType m_ErrorType;

	private CertStatus m_CertStatus;

	private string m_ErrorMsg = "";

	private Dictionary<string, string> m_CardInfo = new Dictionary<string, string>();

	public bool IsSuccess
	{
		get
		{
			return m_isSuccess;
		}
		set
		{
			m_isSuccess = value;
		}
	}

	public CertStatus CertStatus
	{
		get
		{
			return m_CertStatus;
		}
		set
		{
			m_CertStatus = value;
		}
	}

	public Dictionary<string, string> CardInfo
	{
		get
		{
			return m_CardInfo;
		}
		set
		{
			m_CardInfo = value;
		}
	}

	public string ErrorMsg
	{
		get
		{
			if (ErrorType == ErrorType.非預期錯誤)
			{
				return m_ErrorMsg;
			}
			if (ErrorType == ErrorType.正常)
			{
				return "";
			}
			return m_ErrorType.ToString();
		}
		set
		{
			m_ErrorMsg = value;
		}
	}

	public ErrorType ErrorType
	{
		get
		{
			return m_ErrorType;
		}
		set
		{
			m_ErrorType = value;
		}
	}
}
