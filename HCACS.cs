using CardInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

public class HCACS : ICard
{
	private const string verifyUrl = "https://www.hyacrd.com.tw/Verify.aspx?crl=HCA&serial=";

	private const int HCA_F_GetBasicData = 10101;

	private const int HCA_F_GetCardInfo = 10102;

	private const int HCA_F_GetCardType = 10103;

	private const int HCA_F_GetCardSN = 10104;

	private const int HCA_F_VerifyPIN = 10105;

	private const int HCA_F_GetCert = 10201;

	private const int HCA_F_CertValidate = 10214;

	private const int HCA_F_Finalize = 10216;

	private const int HCA_F_GetCardVersion = 10217;

	private const int HCA_F_GetCertAttr = 10212;

	private int portnum = 0;

	public ResultObj Execute(Dictionary<string, string> args)
	{
		ResultObj resobj = new ResultObj();
		resobj.IsSuccess = false;
		if (!OpenPort())
		{
			resobj.ErrorType = ErrorType.非預期錯誤;
			resobj.ErrorMsg = "開啟ComPort " + (portnum + 1) + "失敗";
			return resobj;
		}
		string a = args["Method"];
		if (!(a == "GetBasicData"))
		{
			if (a == "ValidCard")
			{
				byte OutBuf = 0;
				if (args["PIN"] == null || args["PIN"].ToString() == "")
				{
					resobj.ErrorType = ErrorType.PIN碼錯誤;
					resobj.ErrorMsg = "未輸入pin碼";
					return resobj;
				}
				if (HCA_GNFuncCall(10105, Encoding.ASCII.GetBytes(args["PIN"].ToString().ToCharArray()), ref OutBuf, 6, 0, 0, 0, 0) != 0)
				{
					resobj.ErrorType = ErrorType.PIN碼錯誤;
					resobj.ErrorMsg = "pin碼錯誤";
					return resobj;
				}
				GetBasicData(ref resobj);
				ValidCard(ref resobj);
			}
		}
		else
		{
			GetBasicData(ref resobj);
			resobj.CertStatus = CertStatus.未驗證;
		}
		if (!ClosePort())
		{
			resobj.IsSuccess = false;
			resobj.ErrorType = ErrorType.非預期錯誤;
			resobj.ErrorMsg = "關閉ComPort " + (portnum + 1) + "失敗";
			return resobj;
		}
		return resobj;
	}

	private void GetBasicData(ref ResultObj resobj)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		byte OutBuf = 0;
		int num = 0;
		byte[] array = new byte[1024];
		byte[] array2 = new byte[256];
		num = HCA_GNFuncCall(10101, null, ref array[0], 0, 1024, 0, 0, 0);
		if (num == 0)
		{
			num = HCA_GNFuncCall(10103, null, ref OutBuf, 0, 0, 0, 0, 0);
			long num2 = 1L;
			num2 = HCA_GNFuncCall(10217, null, ref OutBuf, 0, 0, 0, 0, 0);
			switch (num)
			{
			case 0:
				resobj.IsSuccess = false;
				resobj.ErrorType = ErrorType.非指定的卡片類型;
				resobj.ErrorMsg = "不支援醫事機構卡";
				break;
			case 1:
				try
				{
					if (num2 == 2)
					{
						Array.Copy(array, array2, 40);
						dictionary.Add("中文姓名", Encoding.GetEncoding("BIG5").GetString(array2, 0, 40).Replace("\0", ""));
						Array.Copy(array, 40, array2, 0, 100);
						array2[100] = 0;
						dictionary.Add("英文姓名", Encoding.ASCII.GetString(array2, 0, 100).Replace("\0", ""));
						array2[0] = array[140];
						array2[1] = 0;
						dictionary.Add("性別", Encoding.ASCII.GetString(array2, 0, 1).Replace("\0", ""));
						Array.Copy(array, 141, array2, 0, 7);
						array2[7] = 0;
						dictionary.Add("出生日期", Encoding.ASCII.GetString(array2, 0, 7).Replace("\0", ""));
						Array.Copy(array, 148, array2, 0, 10);
						array2[10] = 0;
						dictionary.Add("身份證號", Encoding.ASCII.GetString(array2, 0, 10).Replace("\0", ""));
						array2[0] = array[158];
						array2[1] = 0;
						dictionary.Add("類別", Encoding.ASCII.GetString(array2, 0, 1).Replace("\0", ""));
					}
					else
					{
						Array.Copy(array, array2, 12);
						dictionary.Add("中文姓名", Encoding.GetEncoding("BIG5").GetString(array2, 0, 12).Replace("\0", ""));
						Array.Copy(array, 12, array2, 0, 30);
						array2[30] = 0;
						dictionary.Add("英文姓名", Encoding.ASCII.GetString(array2, 0, 30).Replace("\0", ""));
						array2[0] = array[42];
						array2[1] = 0;
						dictionary.Add("性別", Encoding.ASCII.GetString(array2, 0, 1).Replace("\0", ""));
						Array.Copy(array, 43, array2, 0, 7);
						array2[7] = 0;
						dictionary.Add("出生日期", Encoding.ASCII.GetString(array2, 0, 7).Replace("\0", ""));
						Array.Copy(array, 50, array2, 0, 11);
						array2[11] = 0;
						dictionary.Add("身份證號", Encoding.ASCII.GetString(array2, 0, 11).Replace("\0", ""));
						array2[0] = array[61];
						array2[1] = 0;
						dictionary.Add("類別", Encoding.ASCII.GetString(array2, 0, 1).Replace("\0", ""));
					}
					resobj.IsSuccess = true;
					resobj.CardInfo = dictionary;
				}
				catch
				{
					resobj.IsSuccess = false;
					resobj.ErrorType = ErrorType.非指定的卡片類型;
					resobj.ErrorMsg = "讀取卡片資料失敗";
				}
				break;
			default:
				resobj.IsSuccess = false;
				resobj.ErrorType = ErrorType.非指定的卡片類型;
				resobj.ErrorMsg = "未支援之卡別資料";
				break;
			}
		}
		else
		{
			resobj.IsSuccess = false;
			if (num == 30037)
			{
				resobj.ErrorType = ErrorType.未插入卡片;
				return;
			}
			resobj.ErrorType = ErrorType.非預期錯誤;
			resobj.ErrorMsg = "error code:" + num;
		}
	}

	private bool ValidCard(ref ResultObj resobj)
	{
		int num = 0;
		byte[] array = new byte[21];
		if (HCA_GNFuncCall(10102, null, ref array[0], 0, 21, 0, 0, 0) != 0)
		{
			resobj.CertStatus = CertStatus.驗證不通過;
			resobj.ErrorType = ErrorType.憑證錯誤;
			resobj.ErrorMsg = "取卡片序號失敗";
			return false;
		}
		int num2 = Convert.ToInt32(Encoding.ASCII.GetString(array, 7, 7));
		int num3 = Convert.ToInt32(Encoding.ASCII.GetString(array, 14, 7));
		int num4 = (DateTime.Now.Year - 1911) * 10000 + DateTime.Now.Month * 100 + DateTime.Now.Day;
		if (num2 > num4 || num4 > num3)
		{
			resobj.CertStatus = CertStatus.驗證不通過;
			resobj.ErrorType = ErrorType.憑證錯誤;
			resobj.ErrorMsg = "卡片不在有效日期區間";
			return false;
		}
		byte[] array2 = new byte[4096];
		int num5 = 4096;
		num = HCA_GNFuncCall(10201, null, ref array2[0], 0, 4096, 0, 0, 0);
		if (num != 0)
		{
			resobj.CertStatus = CertStatus.驗證不通過;
			resobj.ErrorType = ErrorType.憑證錯誤;
			resobj.ErrorMsg = "取CA憑證失敗 (" + ErrCode.errMsg(num) + ")";
			return false;
		}
		int num6 = array2[1] & 0xF;
		num5 = 0;
		for (int i = 0; i < num6; i++)
		{
			num5 = ((num5 << 8) | (array2[2 + i] & 0xFF));
		}
		num5 += 2 + num6;
		byte[] array3 = new byte[2048];
		int num7 = 2048;
		num = HCA_GNFuncCall(10201, null, ref array3[0], 1, 2048, 0, 0, 0);
		if (num != 0)
		{
			resobj.CertStatus = CertStatus.驗證不通過;
			resobj.ErrorType = ErrorType.憑證錯誤;
			resobj.ErrorMsg = "取簽章憑證失敗 (" + ErrCode.errMsg(num) + ")";
			return false;
		}
		num6 = (array3[1] & 0xF);
		num7 = 0;
		for (int i = 0; i < num6; i++)
		{
			num7 = ((num7 << 8) | (array3[2 + i] & 0xFF));
		}
		num7 += 2 + num6;
		num = HCA_GNFuncCall(10214, array3, ref array2[0], num7, num5, 0, 0, 0);
		if (num != 0)
		{
			resobj.CertStatus = CertStatus.驗證不通過;
			resobj.ErrorType = ErrorType.憑證錯誤;
			resobj.ErrorMsg = "驗章失敗 (" + ErrCode.errMsg(num) + ")";
			return false;
		}
		byte[] array4 = new byte[1024];
		num = HCA_GNFuncCall(10212, array3, ref array4[0], num7, 1024, 1, 1, 0);
		if (num != 0)
		{
			resobj.CertStatus = CertStatus.驗證不通過;
			resobj.ErrorType = ErrorType.憑證錯誤;
			resobj.ErrorMsg = "取不到憑證序號 (" + ErrCode.errMsg(num) + ")";
			return false;
		}
		string @string = Encoding.GetEncoding("BIG5").GetString(array4);
		string requestUriString = "https://www.hyacrd.com.tw/Verify.aspx?crl=HCA&serial=" + @string;
		string text = "";
		try
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			Stream responseStream = httpWebResponse.GetResponseStream();
			StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
			text = streamReader.ReadToEnd();
			streamReader.Close();
			responseStream.Close();
			if (text == "OK")
			{
				resobj.CertStatus = CertStatus.驗證通過;
				return true;
			}
			resobj.CertStatus = CertStatus.驗證不通過;
			resobj.ErrorMsg = "憑證狀態失敗";
			resobj.ErrorType = ErrorType.憑證錯誤;
			return false;
		}
		catch
		{
			resobj.CertStatus = CertStatus.未驗證;
			resobj.ErrorType = ErrorType.憑證錯誤;
			return false;
		}
	}

	private bool OpenPort()
	{
		if (csOpenCom(portnum) == 0)
		{
			return true;
		}
		return false;
	}

	private bool ClosePort()
	{
		byte[] array = new byte[10];
		HCA_GNFuncCall(10216, null, ref array[0], 0, 0, 0, 0, 0);
		if (csCloseCom() == 0)
		{
			return true;
		}
		return false;
	}

	[DllImport("CsHis.dll")]
	private static extern int csOpenCom(int pComNum);

	[DllImport("CsHis.dll")]
	private static extern int csCloseCom();

	[DllImport("CsHis.dll")]
	private static extern int HCA_GNFuncCall(int FuncID, byte[] InBuf, ref byte OutBuf, int P1, int P2, int P3, int P4, int P5);
}
