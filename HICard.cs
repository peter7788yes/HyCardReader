using CardInterface;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class HICard : ICard
{
	public ResultObj Execute(Dictionary<string, string> args)
	{
		ResultObj resobj = new ResultObj();
		resobj.IsSuccess = false;
		try
		{
			string a = args["Method"];
			if (!(a == "Moica"))
			{
				if (a == "Moeaca")
				{
					resobj.IsSuccess = true;
					getMoeacaICCert(ref resobj);
					getMoeacaICCert(ref resobj);
				}
				else
				{
					resobj.ErrorMsg = "hello world";
					resobj.CertStatus = CertStatus.未驗證;
					resobj.ErrorType = ErrorType.非預期錯誤;
					resobj.IsSuccess = false;
				}
			}
			else if (CheckPin(args["PIN"].ToString(), ref resobj))
			{
				resobj.IsSuccess = true;
				getMoicaICCert(ref resobj);
				getMoicaICCert(ref resobj);
			}
			else
			{
				resobj.IsSuccess = false;
				resobj.ErrorType = ErrorType.PIN碼錯誤;
			}
		}
		catch (Exception ex)
		{
			resobj.ErrorMsg = "讀取卡片發生錯誤 (" + ex.Message + ")";
		}
		return resobj;
	}

	public static bool CheckPin(string pincode, ref ResultObj resobj)
	{
		IntPtr ptr = verificationPin("data", pincode);
		int num = 0;
		string[] array = new string[256];
		do
		{
			array[num] = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(ptr, num));
			string text = "";
			switch (num)
			{
			case 0:
				text = "IsSuccess";
				if (array[num].ToLower() == "true")
				{
					resobj.IsSuccess = true;
				}
				else
				{
					resobj.IsSuccess = false;
				}
				resobj.CardInfo[text] = array[num].Replace("\n", "");
				break;
			case 1:
				text = "ErrorMsg";
				resobj.ErrorMsg = array[num].Replace("\n", "");
				resobj.CardInfo[text] = array[num].Replace("\n", "");
				break;
			case 2:
				text = "CertStatus";
				switch (array[num])
				{
				case "0":
					resobj.CertStatus = CertStatus.未驗證;
					break;
				case "1":
					resobj.CertStatus = CertStatus.驗證通過;
					break;
				case "2":
					resobj.CertStatus = CertStatus.驗證不通過;
					break;
				}
				break;
			default:
				text = num.ToString();
				break;
			}
			num++;
		}
		while (array[num - 1] != null);
		return resobj.IsSuccess;
	}

	public static void getMoicaICCert(ref ResultObj resobj)
	{
		IntPtr moicaCard = getMoicaCard();
		int num = 0;
		string[] array = new string[256];
		do
		{
			array[num] = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(moicaCard, 4 * num));
			string text = "";
			switch (num)
			{
			case 0:
				text = "Verify";
				if (array[num].ToLower() != "verify ok")
				{
					resobj.ErrorType = ErrorType.非指定的卡片類型;
					resobj.IsSuccess = false;
				}
				break;
			case 1:
				text = "subject";
				resobj.CardInfo[text] = array[num].Replace("\n", "");
				break;
			case 2:
				text = "issuer";
				break;
			case 3:
				text = "notBeforeDate";
				break;
			case 4:
				text = "notAfterDate";
				break;
			case 5:
				text = "subjectEmail";
				break;
			case 6:
				text = "certPolicy";
				break;
			case 7:
				text = "crl";
				break;
			case 8:
				text = "citizenID";
				resobj.CardInfo[text] = array[num].Replace("\n", "");
				break;
			case 9:
				text = "OCSP";
				break;
			case 10:
				text = "IsSuccess";
				if (array[num].ToLower() == "true")
				{
					resobj.IsSuccess = true;
				}
				else
				{
					resobj.IsSuccess = false;
				}
				break;
			case 11:
				text = "ErrorMsg";
				resobj.ErrorMsg = array[num].Replace("\n", "");
				break;
			case 12:
				text = "CertStatus";
				switch (array[num])
				{
				case "0":
					resobj.CertStatus = CertStatus.未驗證;
					break;
				case "1":
					resobj.CertStatus = CertStatus.驗證通過;
					break;
				case "2":
					resobj.CertStatus = CertStatus.驗證不通過;
					break;
				}
				break;
			default:
				text = num.ToString();
				break;
			}
			num++;
		}
		while (array[num - 1] != null);
	}

	public static void getMoeacaICCert(ref ResultObj resobj)
	{
		IntPtr moeacaCard = getMoeacaCard();
		int num = 0;
		string[] array = new string[256];
		do
		{
			array[num] = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(moeacaCard, 4 * num));
			string text = "";
			switch (num)
			{
			case 0:
				text = "Verify";
				if (array[num].ToLower() != "verify ok")
				{
					resobj.ErrorType = ErrorType.非指定的卡片類型;
					resobj.IsSuccess = false;
				}
				break;
			case 1:
				text = "subject";
				resobj.CardInfo[text] = array[num].Replace("\n", "");
				break;
			case 2:
				text = "issuer";
				break;
			case 3:
				text = "notBeforeDate";
				break;
			case 4:
				text = "notAfterDate";
				break;
			case 5:
				text = "subjectEmail";
				break;
			case 6:
				text = "certPolicy";
				break;
			case 7:
				text = "crl";
				break;
			case 8:
				text = "uid";
				resobj.CardInfo[text] = array[num].Replace("\n", "");
				break;
			case 9:
				text = "IsSuccess";
				if (array[num].ToLower() == "true")
				{
					resobj.IsSuccess = true;
				}
				else
				{
					resobj.IsSuccess = false;
				}
				break;
			case 10:
				text = "ErrorMsg";
				resobj.ErrorMsg = array[num].Replace("\n", "");
				break;
			case 11:
				text = "CertStatus";
				switch (array[num])
				{
				case "0":
					resobj.CertStatus = CertStatus.未驗證;
					break;
				case "1":
					resobj.CertStatus = CertStatus.驗證通過;
					break;
				case "2":
					resobj.CertStatus = CertStatus.驗證不通過;
					break;
				}
				break;
			default:
				text = num.ToString();
				break;
			}
			num++;
		}
		while (array[num - 1] != null);
	}

	[DllImport("HICardCert.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
	public static extern IntPtr getMoicaCard();

	[DllImport("HICardCert.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
	public static extern IntPtr getMoeacaCard();

	[DllImport("HICardCert.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
	public static extern IntPtr verificationPin(string data, string pin);

	[DllImport("HICardCert.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
	public static extern IntPtr listReadersAndCards();
}
