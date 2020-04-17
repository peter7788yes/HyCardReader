using CardInterface;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

public class HISCard : ICard
{
	private Encoding BIG5 = Encoding.GetEncoding("big5");

	public ResultObj Execute(Dictionary<string, string> args)
	{
		ResultObj resobj = new ResultObj();
		resobj.IsSuccess = false;
		resobj.CertStatus = CertStatus.未驗證;
		switch (args["Method"])
		{
		case "GetBasicData":
			getBasicData(ref resobj);
			break;
		case "GetInoculateData":
			getInoculateData(ref resobj);
			break;
		case "WriteInoculateData":
			writeInoculateData(ref resobj, args["VaccineType"], args["BatchNo"]);
			break;
		default:
		{
			resobj.IsSuccess = true;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("default", "hello world");
			resobj.CardInfo = dictionary;
			break;
		}
		}
		return resobj;
	}

	private void getBasicData(ref ResultObj resobj)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (!openCom())
		{
			resobj.IsSuccess = false;
			resobj.ErrorType = ErrorType.非預期錯誤;
			resobj.ErrorMsg = "開啟讀卡機通訊埠，失敗！請檢查讀卡機連線。";
			return;
		}
		int iBufferLen = 72;
		byte[] array = new byte[iBufferLen];
		int num = hisGetBasicData(array, ref iBufferLen);
		closeCom();
		if (num != 0)
		{
			resobj.IsSuccess = false;
			resobj.ErrorMsg = ErrCode.errMsg(num);
			return;
		}
		dictionary.Add("卡片號碼", BIG5.GetString(array, 0, 12).Trim());
		dictionary.Add("姓名", BIG5.GetString(array, 12, 20).Trim());
		dictionary.Add("身分證號", BIG5.GetString(array, 32, 10).Trim());
		dictionary.Add("出生日期", BIG5.GetString(array, 42, 7).Trim());
		dictionary.Add("性別", BIG5.GetString(array, 49, 1).Trim());
		dictionary.Add("發卡日期", BIG5.GetString(array, 50, 7).Trim());
		dictionary.Add("卡片註銷註記", BIG5.GetString(array, 57, 1).Trim());
		dictionary.Add("緊急聯絡電話", BIG5.GetString(array, 58, 14).Trim());
		resobj.IsSuccess = true;
		resobj.CardInfo = dictionary;
	}

	private void getInoculateData(ref ResultObj resobj)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		StringBuilder stringBuilder = new StringBuilder();
		if (!openCom())
		{
			resobj.IsSuccess = false;
			resobj.ErrorMsg = "開啟讀卡機通訊埠，失敗！請檢查讀卡機連線。";
			return;
		}
		int iBufferLen = 1400;
		byte[] array = new byte[iBufferLen];
		int num = hisGetInoculateData(array, ref iBufferLen);
		closeCom();
		if (num == 0)
		{
			int num2 = 0;
			for (int i = 0; i < 40; i++)
			{
				if (BIG5.GetString(array, i * 35, 6).Trim() == "")
				{
					dictionary.Add("資料筆數", num2.ToString());
					break;
				}
				num2++;
				dictionary.Add(num2 + ".疫苗種類", BIG5.GetString(array, i * 35, 6).Trim());
				dictionary.Add(num2 + ".接種日期", BIG5.GetString(array, i * 35 + 6, 7).Trim());
				dictionary.Add(num2 + ".醫療院所代碼", BIG5.GetString(array, i * 35 + 13, 10).Trim());
				dictionary.Add(num2 + ".疫苗批號", BIG5.GetString(array, i * 35 + 23, 12).Trim());
			}
			resobj.IsSuccess = true;
		}
		else
		{
			resobj.IsSuccess = false;
			resobj.ErrorMsg = ErrCode.errMsg(num);
		}
	}

	private void writeInoculateData(ref ResultObj resobj, string VaccineType, string BatchNo)
	{
		byte[] array = new byte[11];
		byte[] array2 = new byte[8];
		byte[] bytes = BIG5.GetBytes(VaccineType + "\0");
		byte[] bytes2 = BIG5.GetBytes(BatchNo + "\0");
		if (!openCom())
		{
			resobj.IsSuccess = false;
			resobj.ErrorMsg = "開啟讀卡機通訊埠，失敗！請檢查讀卡機連線。";
			return;
		}
		int iBufferLen = 72;
		byte[] array3 = new byte[iBufferLen];
		int num = hisGetBasicData(array3, ref iBufferLen);
		Array.Copy(array3, 32, array, 0, 10);
		array[10] = 0;
		Array.Copy(array2, 42, array, 0, 7);
		array2[7] = 0;
		num = csVerifySAMDC();
		if (num != 0)
		{
			resobj.IsSuccess = false;
			resobj.ErrorMsg = "讀卡機認證失敗，錯誤訊息：" + ErrCode.errMsg(num);
			return;
		}
		int num2 = hisWriteInoculateData(array, array2, bytes, bytes2);
		closeCom();
		if (num2 == 0)
		{
			resobj.IsSuccess = true;
			return;
		}
		resobj.IsSuccess = false;
		resobj.ErrorMsg = ErrCode.errMsg(num2);
	}

	[DllImport("CsHis.dll")]
	private static extern int csOpenCom(int pComNum);

	[DllImport("CsHis.dll")]
	private static extern int csCloseCom();

	[DllImport("CsHis.dll")]
	private static extern int hisGetCardStatus(int CardType);

	[DllImport("CsHis.dll")]
	private static extern int hisGetBasicData(byte[] pBuffer, ref int iBufferLen);

	[DllImport("CsHis.dll")]
	private static extern int hisGetInoculateData(byte[] pBuffer, ref int iBufferLen);

	[DllImport("CsHis.dll")]
	private static extern int hisWriteInoculateData(byte[] pPatientID, byte[] pPatientBirthDate, byte[] pItem, byte[] pPackageNumber);

	[DllImport("CsHis.dll")]
	private static extern int csVerifySAMDC();

	[DllImport("CsHis.dll")]
	private static extern int hpcVerifyHPCPIN();

	private bool checkSAMStatus()
	{
		openCom();
		int num = hisGetCardStatus(1);
		closeCom();
		if (num == 2)
		{
			MessageBox.Show("讀卡機認證狀態：已認證");
			return true;
		}
		MessageBox.Show("讀卡機認證狀態：未認證");
		return false;
	}

	private bool checkCardStatus()
	{
		openCom();
		int num = hisGetCardStatus(2);
		closeCom();
		switch (num)
		{
		case 0:
			MessageBox.Show("卡片未置入");
			return false;
		case 1:
			MessageBox.Show("健保IC卡尚未與安全模組認證");
			return false;
		case 9:
			MessageBox.Show("所置入非健保IC卡");
			return false;
		default:
			return true;
		}
	}

	private bool openCom()
	{
		if (csOpenCom(0) != 0)
		{
			return false;
		}
		return true;
	}

	private void closeCom()
	{
		csCloseCom();
	}
}
