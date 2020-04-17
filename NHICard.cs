using CardInterface;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

public class NHICard : ICard
{
	private struct SCARD_IO_REQUEST
	{
		public int dwProtocol;

		public int cbPciLength;
	}

	public ResultObj Execute(Dictionary<string, string> args)
	{
		ResultObj resobj = new ResultObj();
		resobj.IsSuccess = false;
		resobj.CertStatus = CertStatus.未驗證;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		int phContext = 0;
		int phCard = 0;
		int ActiveProtocol = 0;
		string empty = string.Empty;
		byte[] array = new byte[21]
		{
			0,
			164,
			4,
			0,
			16,
			209,
			88,
			0,
			0,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			17,
			0
		};
		byte[] array2 = new byte[7]
		{
			0,
			202,
			17,
			0,
			2,
			0,
			0
		};
		byte[] array3 = new byte[2];
		int pcbRecvLength = 2;
		byte[] array4 = new byte[59];
		int pcbRecvLength2 = 59;
		uint num = 0u;
		num = SCardEstablishContext(0u, 0, 0, ref phContext);
		if (num != 0)
		{
			resobj.ErrorMsg = ErrCode.errMsg(num);
		}
		else
		{
			int num2 = 0;
			byte[] array5 = new byte[num2];
			List<string> list = new List<string>();
			int pcchReaders = 0;
			num = SCardListReaders(phContext, null, null, ref pcchReaders);
			if (num == 0)
			{
				byte[] array6 = new byte[pcchReaders];
				num = SCardListReaders(phContext, null, array6, ref pcchReaders);
				if (num == 0)
				{
					ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
					string text = aSCIIEncoding.GetString(array6);
					int num3 = 0;
					char c = '\0';
					int num4 = pcchReaders;
					while (text[0] != c)
					{
						num3 = text.IndexOf(c);
						string text2 = text.Substring(0, num3);
						list.Add(text2);
						num4 -= text2.Length + 1;
						text = text.Substring(num3 + 1, num4);
					}
				}
			}
			if (num != 0)
			{
				SetErrorMsg(ref resobj, num);
			}
			else if (list.Count == 0)
			{
				SetErrorMsg(ref resobj, 2148532270u);
			}
			else
			{
				uint num5 = 0u;
				foreach (string item in list)
				{
					num = SCardConnect(phContext, item, 1u, 2u, ref phCard, ref ActiveProtocol);
					if (num != 0)
					{
						num5 = num;
					}
					else
					{
						SCARD_IO_REQUEST pioSendPci = default(SCARD_IO_REQUEST);
						SCARD_IO_REQUEST pioRecvPci = default(SCARD_IO_REQUEST);
						pioSendPci.dwProtocol = (pioRecvPci.dwProtocol = ActiveProtocol);
						pioSendPci.cbPciLength = (pioRecvPci.cbPciLength = 8);
						num = SCardTransmit(phCard, ref pioSendPci, array, array.Length, ref pioRecvPci, ref array3[0], ref pcbRecvLength);
						if (num == 0)
						{
							num = SCardTransmit(phCard, ref pioSendPci, array2, array2.Length, ref pioRecvPci, ref array4[0], ref pcbRecvLength2);
							if (num != 0)
							{
								SetErrorMsg(ref resobj, num);
							}
							else
							{
								try
								{
									string text3 = Encoding.Default.GetString(array4, 0, 12).Replace("\0", "");
									double result = 0.0;
									double.TryParse(text3, out result);
									if (text3.Length != 12 || result == 0.0)
									{
										throw new Exception();
									}
									dictionary.Add("健保卡ID", text3);
									dictionary.Add("姓名", Encoding.Default.GetString(array4, 12, 20).Replace("\0", ""));
									dictionary.Add("身分證字號", Encoding.Default.GetString(array4, 32, 10));
									dictionary.Add("生日", Encoding.Default.GetString(array4, 43, 2) + "/" + Encoding.Default.GetString(array4, 45, 2) + "/" + Encoding.Default.GetString(array4, 47, 2));
									dictionary.Add("姓別", Encoding.Default.GetString(array4, 49, 1));
									dictionary.Add("發卡日期", Encoding.Default.GetString(array4, 51, 2) + "/" + Encoding.Default.GetString(array4, 53, 2) + "/" + Encoding.Default.GetString(array4, 55, 2));
									resobj.IsSuccess = true;
									resobj.CardInfo = dictionary;
								}
								catch
								{
									resobj.IsSuccess = false;
									SetErrorMsg(ref resobj, 2148532326u);
								}
							}
							SCardDisconnect(phCard, 0);
							break;
						}
						SetErrorMsg(ref resobj, num);
					}
				}
				if (!resobj.IsSuccess && num5 != 0)
				{
					SetErrorMsg(ref resobj, num5);
				}
			}
			SCardReleaseContext(phContext);
		}
		if (!resobj.IsSuccess && resobj.ErrorMsg.Equals(""))
		{
			resobj.ErrorType = ErrorType.非預期錯誤;
			resobj.ErrorMsg = "非預期錯誤，無法讀取健保卡";
		}
		return resobj;
	}

	private void SetErrorMsg(ref ResultObj resobj, uint tmpCode)
	{
		switch (tmpCode)
		{
		case 0u:
			resobj.ErrorType = ErrorType.正常;
			break;
		case 2148532266u:
		case 2148532331u:
			resobj.ErrorType = ErrorType.PIN碼錯誤;
			break;
		case 2148532270u:
			resobj.ErrorType = ErrorType.未接上讀卡機;
			break;
		case 2148532236u:
		case 2148532329u:
			resobj.ErrorType = ErrorType.未插入卡片;
			break;
		case 2148532326u:
			resobj.ErrorType = ErrorType.非指定的卡片類型;
			break;
		case 2148532268u:
		case 2148532269u:
			resobj.ErrorType = ErrorType.憑證錯誤;
			break;
		default:
			resobj.ErrorType = ErrorType.非預期錯誤;
			resobj.ErrorMsg = ErrCode.errMsg(tmpCode);
			break;
		}
	}

	[DllImport("WinScard.dll")]
	private static extern uint SCardEstablishContext(uint dwScope, int nNotUsed1, int nNotUsed2, ref int phContext);

	[DllImport("WinScard.dll")]
	private static extern uint SCardReleaseContext(int phContext);

	[DllImport("WinScard.dll")]
	private static extern uint SCardConnect(int hContext, string cReaderName, uint dwShareMode, uint dwPrefProtocol, ref int phCard, ref int ActiveProtocol);

	[DllImport("WinScard.dll")]
	private static extern uint SCardDisconnect(int hCard, int Disposition);

	[DllImport("WinScard.dll")]
	private static extern uint SCardListReaders(int hContext, byte[] mszGroups, byte[] mszReaders, ref int pcchReaders);

	[DllImport("WinScard.dll")]
	private static extern uint SCardTransmit(int hCard, ref SCARD_IO_REQUEST pioSendPci, byte[] pbSendBuffer, int cbSendLength, ref SCARD_IO_REQUEST pioRecvPci, ref byte pbRecvBuffer, ref int pcbRecvLength);
}
