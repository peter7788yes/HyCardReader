sing CardInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

public class HCA : ICard
{
	public struct CrlRequestStruct
	{
		public unsafe fixed byte ucRequestCertSerialnumber[128];

		public short iSerialNumberLength;

		public unsafe fixed byte cCRLthisUpdate[20];

		public tm CRLthisUpdate;

		public unsafe fixed byte cCRLnextUpdate[20];

		public tm CRLnextUpdate;

		public int CertStatus;

		public int RevokedReason;

		public unsafe fixed byte cRevokedDate[15];

		public tm RevokedDate;
	}

	public struct tm
	{
		public int tm_sec;

		public int tm_min;

		public int tm_hour;

		public int tm_mday;

		public int tm_mon;

		public int tm_year;

		public int tm_wday;

		public int tm_yday;

		public int tm_isdst;
	}

	public struct CRLBasicStruct
	{
		public int iCRLType;

		public unsafe char* pTbsCRL;

		public int iTbsCRLLength;

		public unsafe char* pSignatureAlgorithm;

		public int iSignatureAlgorithmLength;

		public unsafe char* pSignature;

		public int iSignatureLength;
	}

	public struct CertBasicStruct
	{
		public unsafe byte* pTbsCertificate;

		public short iTbsCertificateLength;

		public unsafe byte* pSignatureAlgorithm;

		public short iSignatureAlgorithmLength;

		public unsafe byte* pSignature;

		public short iSignatureLength;
	}

	public struct OCSstruct
	{
		public unsafe fixed byte SerialNumber[128];

		public int iSerialNumberLength;

		public int ResponseStatus;

		public int CertStatus;

		public int RevokeReason;

		public unsafe fixed byte RequestTime[20];
	}

	private const string HCA_OCSP_URL = "http://hcaocsp.nat.gov.tw/cgi-bin/OCSP/ocsp_server.exe";

	private const string verifyUrl = "https://www.hyacrd.com.tw/Verify.aspx?crl=HCA&serial=";

	private const uint FSP11_RTN_BUFFER_TOO_SMALL = 9075u;

	private const short CKF_SERIAL_SESSION = 4;

	private uint hICModule = 0u;

	private uint hICSession = 0u;

	public ResultObj Execute(Dictionary<string, string> args)
	{
		ResultObj resobj = new ResultObj();
		resobj.IsSuccess = false;
		resobj.CertStatus = CertStatus.未驗證;
		uint num = 0u;
		num = InitModule("HCAPKCS11", IntPtr.Zero, ref hICModule);
		if (num != 0)
		{
			resobj.ErrorType = ErrorType.非預期錯誤;
			resobj.ErrorMsg = "初始化密碼模組失敗 (" + ErrCode.errMsg(num) + ")";
			if (hICModule != 0)
			{
				CloseModule(hICModule);
			}
			return resobj;
		}
		string a = args["Method"];
		if (!(a == "GetBasicData"))
		{
			if (a == "ValidCard")
			{
				if (args["PIN"] == null || args["PIN"].ToString() == "")
				{
					resobj.ErrorType = ErrorType.PIN碼錯誤;
					resobj.ErrorMsg = "未輸入pin碼";
					if (hICModule != 0)
					{
						CloseModule(hICModule);
					}
					return resobj;
				}
				num = InitSession(hICModule, 4, args["PIN"].ToString(), (short)args["PIN"].ToString().Length, ref hICSession);
				if (num != 0)
				{
					resobj.ErrorType = ErrorType.非預期錯誤;
					resobj.ErrorMsg = "初始化Session失敗 (" + ErrCode.errMsg(num) + ")";
					if (hICModule != 0)
					{
						CloseModule(hICModule);
					}
					return resobj;
				}
				GetBasicData(hICModule, ref resobj);
				if (hICSession != 0)
				{
					CloseSession(hICModule, hICSession);
				}
			}
			else
			{
				resobj.ErrorType = ErrorType.非預期錯誤;
				resobj.IsSuccess = false;
				resobj.ErrorMsg = "hello world";
			}
		}
		else
		{
			GetBasicData(hICModule, ref resobj);
		}
		if (hICModule != 0)
		{
			CloseModule(hICModule);
		}
		return resobj;
	}

	private void GetBasicData(uint hICModule, ref ResultObj resobj)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		byte[] array = new byte[1024];
		byte[] array2 = new byte[128];
		uint lenBasicData = 1024u;
		short cardType = 0;
		short cardVer = 0;
		uint hCABasicData = GetHCABasicData(hICModule, ref array[0], ref lenBasicData);
		if (hCABasicData != 0)
		{
			resobj.ErrorType = ErrorType.非指定的卡片類型;
			resobj.IsSuccess = false;
			resobj.ErrorMsg = "讀取卡片基本資料失敗 (" + ErrCode.errMsg(hCABasicData) + ")";
			return;
		}
		hCABasicData = GetHCACardType(hICModule, ref cardType);
		if (hCABasicData != 0)
		{
			resobj.ErrorType = ErrorType.非指定的卡片類型;
			resobj.IsSuccess = false;
			resobj.ErrorMsg = "取卡別失敗 (" + ErrCode.errMsg(hCABasicData) + ")";
			return;
		}
		hCABasicData = GetHCACardVersion(hICModule, ref cardVer);
		if (hCABasicData != 0)
		{
			resobj.ErrorType = ErrorType.非指定的卡片類型;
			resobj.IsSuccess = false;
			resobj.ErrorMsg = "取得卡片版本失敗 (" + ErrCode.errMsg(hCABasicData) + ")";
			return;
		}
		switch (cardType)
		{
		case 0:
			resobj.ErrorType = ErrorType.非指定的卡片類型;
			resobj.IsSuccess = false;
			resobj.ErrorMsg = "不支援醫事機構卡";
			break;
		case 1:
			try
			{
				if (cardVer == 2)
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
					dictionary.Add("身分證號", Encoding.ASCII.GetString(array2, 0, 10).Replace("\0", ""));
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
					array2[0] = array[158];
					array2[1] = 0;
					dictionary.Add("類別", Encoding.ASCII.GetString(array2, 0, 1).Replace("\0", ""));
				}
				resobj.IsSuccess = true;
				resobj.CardInfo = dictionary;
			}
			catch
			{
				resobj.ErrorType = ErrorType.非預期錯誤;
				resobj.IsSuccess = false;
				resobj.ErrorMsg = "讀取卡片資料失敗";
			}
			break;
		default:
			resobj.ErrorType = ErrorType.非指定的卡片類型;
			resobj.IsSuccess = false;
			resobj.ErrorMsg = "未支援之卡別資料";
			break;
		}
	}

	private bool ValidCard(ref ResultObj resobj)
	{
		bool flag = false;
		string text = "";
		byte[] array = new byte[50];
		int lenCardInfo = 50;
		uint hCACardInfo = GetHCACardInfo(hICModule, ref array[0], ref lenCardInfo);
		if (hCACardInfo != 0)
		{
			resobj.CertStatus = CertStatus.驗證不通過;
			resobj.ErrorType = ErrorType.憑證錯誤;
			resobj.ErrorMsg = "取卡片序號失敗 (" + ErrCode.errMsg(hCACardInfo) + ")";
			return false;
		}
		int num = Convert.ToInt32(Encoding.ASCII.GetString(array, 8, 7));
		int num2 = Convert.ToInt32(Encoding.ASCII.GetString(array, 15, 7));
		int num3 = (DateTime.Now.Year - 1911) * 10000 + DateTime.Now.Month * 100 + DateTime.Now.Day;
		if (num > num3 || num3 > num2)
		{
			resobj.CertStatus = CertStatus.驗證不通過;
			resobj.ErrorType = ErrorType.憑證錯誤;
			resobj.ErrorMsg = "卡片不在有效日期區間";
			return false;
		}
		byte[] array2 = new byte[0];
		short piCertificateLength = 0;
		hCACardInfo = GetCertificateFromGPKICard(hICModule, hICSession, 0, null, ref piCertificateLength, "");
		if (hCACardInfo == 9075)
		{
			array2 = new byte[piCertificateLength];
			hCACardInfo = GetCertificateFromGPKICard(hICModule, hICSession, 0, array2, ref piCertificateLength, "");
		}
		if (hCACardInfo != 0)
		{
			resobj.CertStatus = CertStatus.驗證不通過;
			resobj.ErrorMsg = "取CA憑證失敗 (" + ErrCode.errMsg(hCACardInfo) + ")";
			resobj.ErrorType = ErrorType.憑證錯誤;
			return false;
		}
		byte[] array3 = new byte[0];
		short piCertificateLength2 = 0;
		hCACardInfo = GetCertificateFromGPKICard(hICModule, hICSession, 1, null, ref piCertificateLength2, "");
		if (hCACardInfo == 9075)
		{
			array3 = new byte[piCertificateLength2];
			hCACardInfo = GetCertificateFromGPKICard(hICModule, hICSession, 1, array3, ref piCertificateLength2, "");
		}
		if (hCACardInfo != 0)
		{
			resobj.CertStatus = CertStatus.驗證不通過;
			resobj.ErrorMsg = "取簽章憑證失敗 (" + ErrCode.errMsg(hCACardInfo) + ")";
			resobj.ErrorType = ErrorType.憑證錯誤;
			return false;
		}
		CertBasicStruct SCertificate = default(CertBasicStruct);
		hCACardInfo = DecodeCertificate(array3, piCertificateLength2, ref SCertificate);
		if (hCACardInfo != 0)
		{
			resobj.CertStatus = CertStatus.驗證不通過;
			resobj.ErrorMsg = "載入簽章憑證失敗 (" + ErrCode.errMsg(hCACardInfo) + ")";
			resobj.ErrorType = ErrorType.憑證錯誤;
			return false;
		}
		CertBasicStruct SCertificate2 = default(CertBasicStruct);
		hCACardInfo = DecodeCertificate(array2, piCertificateLength, ref SCertificate2);
		if (hCACardInfo != 0)
		{
			resobj.CertStatus = CertStatus.驗證不通過;
			resobj.ErrorMsg = "載入CA憑證失敗 (" + ErrCode.errMsg(hCACardInfo) + ")";
			resobj.ErrorType = ErrorType.憑證錯誤;
			return false;
		}
		short num4 = VerifyCertSignature(ref SCertificate, ref SCertificate2);
		if (hCACardInfo != 0)
		{
			resobj.CertStatus = CertStatus.驗證不通過;
			resobj.ErrorMsg = "驗章失敗 (" + ErrCode.errMsg(hCACardInfo) + ")";
			resobj.ErrorType = ErrorType.憑證錯誤;
			return false;
		}
		byte[] array4 = new byte[128];
		short piSerialNumberLength = 128;
		hCACardInfo = GetSerialNumber(ref SCertificate, array4, ref piSerialNumberLength);
		if (hCACardInfo != 0)
		{
			resobj.CertStatus = CertStatus.驗證不通過;
			resobj.ErrorMsg = "取憑證序號失敗 (" + ErrCode.errMsg(hCACardInfo) + ")";
			resobj.ErrorType = ErrorType.憑證錯誤;
			return false;
		}
		text = Bytes2Hex(array4, (uint)piSerialNumberLength);
		string requestUriString = "https://www.hyacrd.com.tw/Verify.aspx?crl=HCA&serial=" + text;
		string text2 = "";
		try
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			Stream responseStream = httpWebResponse.GetResponseStream();
			StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
			text2 = streamReader.ReadToEnd();
			streamReader.Close();
			responseStream.Close();
			if (text2 == "OK")
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

	private byte[] ReadBinaryFile(string path, ref int len)
	{
		FileStream fileStream = File.OpenRead(path);
		byte[] array = new byte[fileStream.Length];
		len = fileStream.Read(array, 0, array.Length);
		fileStream.Close();
		return array;
	}

	private unsafe int Hex2Bytes(string Hex, byte* toBytes)
	{
		int[] array = new int[23]
		{
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			10,
			11,
			12,
			13,
			14,
			15
		};
		int num = 0;
		int num2 = 0;
		while (num2 < Hex.Length)
		{
			*toBytes = (byte)((array[char.ToUpper(Hex[num2]) - 48] << 4) | array[char.ToUpper(Hex[num2 + 1]) - 48]);
			num2 += 2;
			num++;
			toBytes++;
		}
		return Hex.Length / 2;
	}

	private string Bytes2Hex(byte[] Bytes, uint count)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string text = "0123456789ABCDEF";
		for (uint num = 0u; num < count; num++)
		{
			stringBuilder.Append(text[Bytes[num] >> 4]);
			stringBuilder.Append(text[Bytes[num] & 0xF]);
		}
		return stringBuilder.ToString();
	}

	[DllImport("HCAAPI.dll")]
	private static extern short VerifyCRLSignature(ref CRLBasicStruct SCRList, ref CertBasicStruct SCACertificate);

	[DllImport("HCAAPI.dll")]
	private static extern short SearchCRL(ref CrlRequestStruct SCRS, ref CRLBasicStruct SCRList, short iCrlNumber);

	[DllImport("HCAAPI.dll")]
	private static extern short DecodeCRL(byte[] pCRL, int iCRLLength, ref CRLBasicStruct SCRL);

	[DllImport("HCAAPI.dll")]
	private unsafe static extern short BuildTobesignedOCSPRequest(ref OCSstruct OCSp, short num, byte[] pNounce, short iNounceLength, byte[] RequestIssueCertificate, short iRequestIssueCertificateLength, byte** ucppTobesignedOCSPReq, ref short iTobesignedOCSPReqLength);

	[DllImport("HCAAPI.dll")]
	private static extern short QueryOCSfromSignedOCSPRequest([In] [Out] OCSstruct[] OCSp, short num, byte[] ucpTobesignedOCSPReq, short iTobesignedOCSPReqLength, byte[] TbsRequestSignature, short iTbsRequestSignatureLength, byte[] RequestIssueCertificate, short iRequestIssueCertificateLength, byte[] SenderCertificate, short iSenderCertificateLength, byte[] OCSPServerCertificate, short iOCSPServerCertificateLength, char[] ServerURL, char[] ProxyName, short ProxyPort);

	[DllImport("HCAAPI.dll")]
	private static extern ushort DecodeCertificate(byte[] pCertificate, short iCertificateLength, ref CertBasicStruct SCertificate);

	[DllImport("HCAAPI.dll")]
	private static extern ushort GetSerialNumber(ref CertBasicStruct SCertificate, [Out] byte[] ppSerialNumber, ref short piSerialNumberLength);

	[DllImport("HCAAPI.dll")]
	private static extern uint InitModule(string pszModuleName, IntPtr pInitArgs, ref uint pulModuleHandle);

	[DllImport("HCAAPI.dll")]
	private static extern uint CloseModule(uint pulModuleHandle);

	[DllImport("HCAAPI.dll")]
	private static extern uint InitSession(uint ulModuleHandle, short iFlags, string pszUserPin, short iUserPinLength, ref uint pulSessionHandle);

	[DllImport("HCAAPI.dll")]
	private static extern uint CloseSession(uint ulModuleHandle, uint ulSessionHandle);

	[DllImport("HCAAPI.dll")]
	private static extern uint GetHCACardVersion(uint ulModuleHandle, ref short cardVer);

	[DllImport("HCAAPI.dll")]
	private static extern uint GetHCACardType(uint ulModuleHandle, ref short cardType);

	[DllImport("HCAAPI.dll")]
	private static extern uint GetHCABasicData(uint ulModuleHandle, ref byte basicData, ref uint lenBasicData);

	[DllImport("HCAAPI.dll")]
	private static extern uint GetCertificateFromGPKICard(uint ulModuleHandle, uint ulSessionHandle, short iCertID, [In] [Out] byte[] ppucCertificate, ref short piCertificateLength, string sReaderName);

	[DllImport("HCAAPI.dll")]
	private static extern uint GetHCACardInfo(uint ulModuleHandle, ref byte cardInfo, ref int lenCardInfo);

	[DllImport("HCAAPI.dll")]
	private static extern short VerifyCertSignature(ref CertBasicStruct SCertificate, ref CertBasicStruct SCACertificate);
}
