using ComputerCheck;
using ComputerCheck.My;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

[DesignerGenerated]
public class GetLog : Form
{
	private class AsmComparer : IComparer<Assembly>
	{
		[DebuggerNonUserCode]
		public AsmComparer()
		{
		}

		public int Compare(Assembly x, Assembly y)
		{
			return string.Compare(x.ToString(), y.ToString());
		}

		int IComparer<Assembly>.Compare(Assembly x, Assembly y)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Compare
			return this.Compare(x, y);
		}
	}

	private IContainer components;

	[AccessedThroughProperty("rtfError")]
	private TextBox _rtfError;

	[AccessedThroughProperty("Button1")]
	private Button _Button1;

	[AccessedThroughProperty("Button2")]
	private Button _Button2;

	[AccessedThroughProperty("Button3")]
	private Button _Button3;

	[AccessedThroughProperty("PrintDialog1")]
	private PrintDialog _PrintDialog1;

	internal virtual TextBox rtfError
	{
		[DebuggerNonUserCode]
		get
		{
			return _rtfError;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			_rtfError = value;
		}
	}

	internal virtual Button Button1
	{
		[DebuggerNonUserCode]
		get
		{
			return _Button1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			EventHandler value2 = new EventHandler(Button1_Click);
			if (_Button1 != null)
			{
				_Button1.Click -= value2;
			}
			_Button1 = value;
			if (_Button1 != null)
			{
				_Button1.Click += value2;
			}
		}
	}

	internal virtual Button Button2
	{
		[DebuggerNonUserCode]
		get
		{
			return _Button2;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			EventHandler value2 = new EventHandler(Button2_Click);
			if (_Button2 != null)
			{
				_Button2.Click -= value2;
			}
			_Button2 = value;
			if (_Button2 != null)
			{
				_Button2.Click += value2;
			}
		}
	}

	internal virtual Button Button3
	{
		[DebuggerNonUserCode]
		get
		{
			return _Button3;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			EventHandler value2 = new EventHandler(Button3_Click);
			if (_Button3 != null)
			{
				_Button3.Click -= value2;
			}
			_Button3 = value;
			if (_Button3 != null)
			{
				_Button3.Click += value2;
			}
		}
	}

	internal virtual PrintDialog PrintDialog1
	{
		[DebuggerNonUserCode]
		get
		{
			return _PrintDialog1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			_PrintDialog1 = value;
		}
	}

	[DebuggerNonUserCode]
	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	[System.Diagnostics.DebuggerStepThrough]
	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComputerCheck.GetLog));
		rtfError = new System.Windows.Forms.TextBox();
		Button1 = new System.Windows.Forms.Button();
		Button2 = new System.Windows.Forms.Button();
		Button3 = new System.Windows.Forms.Button();
		PrintDialog1 = new System.Windows.Forms.PrintDialog();
		SuspendLayout();
		System.Drawing.Point point2 = rtfError.Location = new System.Drawing.Point(74, 21);
		rtfError.Multiline = true;
		rtfError.Name = "rtfError";
		rtfError.ScrollBars = System.Windows.Forms.ScrollBars.Both;
		System.Drawing.Size size2 = rtfError.Size = new System.Drawing.Size(485, 292);
		rtfError.TabIndex = 0;
		point2 = (Button1.Location = new System.Drawing.Point(148, 329));
		Button1.Name = "Button1";
		size2 = (Button1.Size = new System.Drawing.Size(149, 39));
		Button1.TabIndex = 1;
		Button1.Text = "Select all And copy";
		Button1.UseVisualStyleBackColor = true;
		point2 = (Button2.Location = new System.Drawing.Point(316, 330));
		Button2.Name = "Button2";
		size2 = (Button2.Size = new System.Drawing.Size(99, 38));
		Button2.TabIndex = 2;
		Button2.Text = "Close";
		Button2.UseVisualStyleBackColor = true;
		point2 = (Button3.Location = new System.Drawing.Point(446, 330));
		Button3.Name = "Button3";
		size2 = (Button3.Size = new System.Drawing.Size(112, 37));
		Button3.TabIndex = 3;
		Button3.Text = "Button3";
		Button3.UseVisualStyleBackColor = true;
		Button3.Visible = false;
		PrintDialog1.UseEXDialog = true;
		System.Drawing.SizeF sizeF2 = AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		size2 = (ClientSize = new System.Drawing.Size(599, 387));
		Controls.Add(Button3);
		Controls.Add(Button2);
		Controls.Add(Button1);
		Controls.Add(rtfError);
		Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		Name = "GetLog";
		Text = "ComputerCheck";
		ResumeLayout(false);
		PerformLayout();
	}

	private void Form1_Load(object sender, EventArgs e)
	{
		//Discarded unreachable code: IL_000a, IL_0020, IL_0022, IL_0029, IL_002c, IL_002d, IL_003a, IL_005c
		int num = default(int);
		int num2 = default(int);
		try
		{
			ProjectData.ClearProjectError();
			num = -2;
		}
		catch (object obj) when (obj is Exception && num != 0 && num2 == 0)
		{
			ProjectData.SetProjectError((Exception)obj);
			/*Error near IL_005a: Could not find block for branch target IL_0022*/;
		}
		if (num2 != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private string GetHardDriveCode(byte DrvIdx)
	{
		try
		{
			object objectValue = RuntimeHelpers.GetObjectValue(Interaction.GetObject("winmgmts:"));
			string text = "Win32_PhysicalMedia";
			string text2 = text + ".Tag=\"\\\\\\\\.\\\\PHYSICALDRIVE" + Conversions.ToString(DrvIdx) + "\"";
			object[] array = new object[1]
			{
				text
			};
			bool[] array2 = new bool[1]
			{
				true
			};
			object instance = NewLateBinding.LateGet(objectValue, null, "InstancesOf", array, null, null, array2);
			if (array2[0])
			{
				text = (string)Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array[0]), typeof(string));
			}
			string result = NewLateBinding.LateGet(NewLateBinding.LateIndexGet(instance, new object[1]
			{
				text2
			}, null), null, "SerialNumber", new object[0], null, null, null).ToString().Trim();
			Marshal.ReleaseComObject(RuntimeHelpers.GetObjectValue(objectValue));
			return result;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			string result = "";
			ProjectData.ClearProjectError();
			return result;
		}
	}

	public GetLog()
	{
		//Discarded unreachable code: IL_0fcb, IL_1257, IL_1259, IL_1263, IL_1266, IL_1268, IL_1275, IL_1299
		int num = default(int);
		int num6 = default(int);
		try
		{
			base.Shown += new EventHandler(Form1_Shown);
			base.Load += new EventHandler(Form1_Load);
			ProjectData.ClearProjectError();
			num = -2;
			int num2 = 2;
			InitializeComponent();
			num2 = 3;
			List<Assembly> list = new List<Assembly>();
			num2 = 4;
			IEnumerator<Assembly> enumerator = MyProject.Application.Info.LoadedAssemblies.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Assembly current = enumerator.Current;
				num2 = 5;
				list.Add(current);
				num2 = 6;
			}
			if (enumerator != null)
			{
				enumerator.Dispose();
			}
			num2 = 7;
			rtfError.AppendText("//===========================OS Info============================//\r\n");
			num2 = 8;
			rtfError.AppendText("OS Name:           " + MyProject.Computer.Info.OSFullName + "\r\n");
			num2 = 9;
			rtfError.AppendText("OS Version:        " + MyProject.Computer.Info.OSVersion + "\r\n");
			num2 = 10;
			rtfError.AppendText("OS Platform:        " + MyProject.Computer.Info.OSPlatform + "\r\n");
			num2 = 11;
			ManagementClass managementClass = new ManagementClass("Win32_OperatingSystem");
			num2 = 12;
			ManagementObjectCollection.ManagementObjectEnumerator enumerator2 = managementClass.GetInstances().GetEnumerator();
			while (enumerator2.MoveNext())
			{
				ManagementObject managementObject = (ManagementObject)enumerator2.Current;
				num2 = 13;
				rtfError.AppendText("OS Platform:       " + managementObject.GetPropertyValue("OSArchitecture").ToString() + "\r\n");
				num2 = 14;
			}
			if (enumerator2 != null)
			{
				((IDisposable)enumerator2).Dispose();
			}
			num2 = 15;
			rtfError.AppendText("//===========================System Info============================//\r\n");
			num2 = 16;
			ManagementObjectCollection managementObjectCollection;
			ManagementObjectCollection.ManagementObjectEnumerator enumerator3;
			int num4;
			checked
			{
				rtfError.AppendText("Physical Memory:   " + FormatBytes((long)MyProject.Computer.Info.AvailablePhysicalMemory) + " / " + FormatBytes((long)MyProject.Computer.Info.TotalPhysicalMemory) + " (Free / Total)\r\n");
				num2 = 17;
				rtfError.AppendText("Virtual Memory:    " + FormatBytes((long)MyProject.Computer.Info.AvailableVirtualMemory) + " / " + FormatBytes((long)MyProject.Computer.Info.TotalVirtualMemory) + " (Free / Total)\r\n");
				num2 = 18;
				rtfError.AppendText("Virtual Memory:    " + FormatBytes((long)MyProject.Computer.Info.AvailableVirtualMemory) + " / " + FormatBytes((long)MyProject.Computer.Info.TotalVirtualMemory) + " (Free / Total)\r\n");
				num2 = 19;
				NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
				num2 = 20;
				NetworkInterface[] array = allNetworkInterfaces;
				int num3 = 0;
				while (num3 < array.Length)
				{
					NetworkInterface networkInterface = array[num3];
					num2 = 21;
					if ((Operators.CompareString(networkInterface.GetPhysicalAddress().ToString(), "", false) != 0) & (networkInterface.GetPhysicalAddress().ToString().Length == 12))
					{
						num2 = 22;
						rtfError.AppendText("InterfaceType:    " + networkInterface.NetworkInterfaceType.ToString() + "\r\n");
						num2 = 23;
						rtfError.AppendText("Description:    " + networkInterface.Description + "\r\n");
						num2 = 24;
						rtfError.AppendText("MACaddress:    " + networkInterface.GetPhysicalAddress().ToString() + "\r\n");
						num2 = 25;
						rtfError.AppendText("IP:    " + networkInterface.GetIPProperties().UnicastAddresses[0].Address.ToString() + "\r\n");
					}
					num3++;
					num2 = 27;
				}
				num2 = 28;
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM  Win32_PhysicalMedia");
				num2 = 29;
				managementObjectCollection = managementObjectSearcher.Get();
				num2 = 30;
				string text = "";
				num2 = 31;
				num4 = 0;
				num2 = 32;
				enumerator3 = managementObjectCollection.GetEnumerator();
			}
			while (enumerator3.MoveNext())
			{
				ManagementObject managementObject2 = (ManagementObject)enumerator3.Current;
				num2 = 33;
				PropertyDataCollection.PropertyDataEnumerator enumerator4 = managementObject2.Properties.GetEnumerator();
				while (enumerator4.MoveNext())
				{
					PropertyData current2 = enumerator4.Current;
					num2 = 34;
					if (!current2.IsArray)
					{
						num2 = 36;
						num2 = 37;
						if (Operators.CompareString(current2.Name, "SerialNumber", false) == 0 && num4 == 0)
						{
							num2 = 38;
							rtfError.AppendText("Hard Disk Number:" + string.Format("{0} = {1}", current2.Name, RuntimeHelpers.GetObjectValue(managementObject2["SerialNumber"])) + "\r\n");
							num2 = 39;
							num4 = 1;
						}
					}
					num2 = 42;
				}
				num2 = 43;
			}
			if (enumerator3 != null)
			{
				((IDisposable)enumerator3).Dispose();
			}
			num2 = 44;
			num4 = 0;
			num2 = 45;
			ManagementObjectCollection.ManagementObjectEnumerator enumerator5 = managementObjectCollection.GetEnumerator();
			while (enumerator5.MoveNext())
			{
				ManagementObject managementObject3 = (ManagementObject)enumerator5.Current;
				num2 = 46;
				PropertyDataCollection.PropertyDataEnumerator enumerator6 = managementObject3.Properties.GetEnumerator();
				while (enumerator6.MoveNext())
				{
					PropertyData current3 = enumerator6.Current;
					num2 = 47;
					if (!current3.IsArray)
					{
						num2 = 49;
						num2 = 50;
						if (Operators.CompareString(current3.Name, "Product", false) == 0 && num4 == 0)
						{
							num2 = 51;
							rtfError.AppendText("Hard Disk Product:" + string.Format("{0} = {1}", current3.Name, RuntimeHelpers.GetObjectValue(managementObject3["Product"])) + "\r\n");
							num2 = 52;
							num4 = 1;
						}
					}
					num2 = 55;
				}
				num2 = 56;
			}
			if (enumerator5 != null)
			{
				((IDisposable)enumerator5).Dispose();
			}
			num2 = 57;
			string text2 = ".";
			num2 = 58;
			ManagementObjectSearcher managementObjectSearcher2 = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
			num2 = 59;
			ManagementObjectCollection managementObjectCollection2 = managementObjectSearcher2.Get();
			num2 = 60;
			ManagementObjectCollection.ManagementObjectEnumerator enumerator7 = managementObjectCollection2.GetEnumerator();
			while (enumerator7.MoveNext())
			{
				ManagementObject managementObject4 = (ManagementObject)enumerator7.Current;
				num2 = 61;
				PropertyDataCollection.PropertyDataEnumerator enumerator8 = managementObject4.Properties.GetEnumerator();
				while (enumerator8.MoveNext())
				{
					PropertyData current4 = enumerator8.Current;
					num2 = 62;
					if (!current4.IsArray)
					{
						num2 = 64;
						num2 = 65;
						if ((Operators.CompareString(current4.Name, "Product", false) == 0) | (Operators.CompareString(current4.Name, "SerialNumber", false) == 0))
						{
							num2 = 66;
							rtfError.AppendText(string.Format("{0} = {1}", current4.Name, RuntimeHelpers.GetObjectValue(current4.Value)) + "\r\n");
						}
					}
					num2 = 69;
				}
				num2 = 70;
			}
			if (enumerator7 != null)
			{
				((IDisposable)enumerator7).Dispose();
			}
			num2 = 71;
			managementObjectSearcher2 = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
			num2 = 72;
			managementObjectCollection2 = managementObjectSearcher2.Get();
			num2 = 73;
			ManagementObjectCollection.ManagementObjectEnumerator enumerator9 = managementObjectCollection2.GetEnumerator();
			while (enumerator9.MoveNext())
			{
				ManagementObject managementObject5 = (ManagementObject)enumerator9.Current;
				num2 = 74;
				PropertyDataCollection.PropertyDataEnumerator enumerator10 = managementObject5.Properties.GetEnumerator();
				while (enumerator10.MoveNext())
				{
					PropertyData current5 = enumerator10.Current;
					num2 = 75;
					if (!current5.IsArray)
					{
						num2 = 77;
						num2 = 78;
						if ((Operators.CompareString(current5.Name, "ProcessorId", false) == 0) | (Operators.CompareString(current5.Name, "Name", false) == 0) | (Operators.CompareString(current5.Name, "NumberOfCores", false) == 0) | (Operators.CompareString(current5.Name, "NumberOfLogicalProcessors", false) == 0))
						{
							num2 = 79;
							rtfError.AppendText(string.Format("{0} = {1}", current5.Name, RuntimeHelpers.GetObjectValue(current5.Value)) + "\r\n");
						}
					}
					num2 = 82;
				}
				num2 = 83;
			}
			if (enumerator9 != null)
			{
				((IDisposable)enumerator9).Dispose();
			}
			num2 = 84;
			Type typeFromHandle = typeof(PrinterSettings);
			num2 = 85;
			FieldInfo field = typeFromHandle.GetField("outputPort", BindingFlags.Instance | BindingFlags.NonPublic);
			num2 = 86;
			string text3 = Conversions.ToString(field.GetValue(PrintDialog1.PrinterSettings));
			num2 = 87;
			rtfError.AppendText("Default Printer Name:    " + PrintDialog1.PrinterSettings.PrinterName + "\r\n");
			num2 = 88;
			ManagementObjectSearcher managementObjectSearcher3 = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
			num2 = 89;
			ManagementObjectCollection managementObjectCollection3 = managementObjectSearcher3.Get();
			num2 = 90;
			ManagementObjectCollection.ManagementObjectEnumerator enumerator11 = managementObjectCollection3.GetEnumerator();
			while (enumerator11.MoveNext())
			{
				ManagementObject managementObject6 = (ManagementObject)enumerator11.Current;
				num2 = 91;
				string str = string.Format("{0}", RuntimeHelpers.GetObjectValue(managementObject6["Name"]));
				num2 = 92;
				string str2 = string.Format("{0}", RuntimeHelpers.GetObjectValue(managementObject6["PortName"]));
				num2 = 93;
				rtfError.AppendText("Printer Name:    " + str + "\r\n");
				num2 = 94;
				rtfError.AppendText("Printer p=Port:    " + str2 + "\r\n");
				num2 = 95;
			}
			if (enumerator11 != null)
			{
				((IDisposable)enumerator11).Dispose();
			}
			num2 = 96;
			managementObjectSearcher3 = new ManagementObjectSearcher("SELECT * FROM Win32_PrinterDriver");
			num2 = 97;
			managementObjectCollection3 = managementObjectSearcher3.Get();
			num2 = 98;
			ManagementObjectCollection.ManagementObjectEnumerator enumerator12 = managementObjectCollection3.GetEnumerator();
			while (enumerator12.MoveNext())
			{
				ManagementObject managementObject7 = (ManagementObject)enumerator12.Current;
				num2 = 99;
				string str3 = string.Format("{0}", RuntimeHelpers.GetObjectValue(managementObject7["Name"]));
				num2 = 100;
				string str4 = string.Format("{0}", RuntimeHelpers.GetObjectValue(managementObject7["DataFile"]));
				num2 = 101;
				rtfError.AppendText("Printer Name:    " + str3 + "\r\n");
				num2 = 102;
				rtfError.AppendText("Printer Path:    " + str4 + "\r\n");
				num2 = 103;
			}
			if (enumerator12 != null)
			{
				((IDisposable)enumerator12).Dispose();
			}
			num2 = 104;
			TimeZone currentTimeZone = TimeZone.CurrentTimeZone;
			num2 = 105;
			rtfError.AppendText("TimeZone Name:" + currentTimeZone.StandardName + "\r\n");
			num2 = 106;
			rtfError.AppendText("//===========================Hyview Info=============================//\r\n");
			num2 = 107;
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{5c54e79c-bef2-4cf6-9e98-5bdc325cb058}", false);
			num2 = 108;
			if (registryKey == null)
			{
				num2 = 109;
				registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{5c54e79c-bef2-4cf6-9e98-5bdc325cb058}", false);
				num2 = 110;
				if (registryKey != null)
				{
					num2 = 111;
					rtfError.AppendText(Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("Hyview Reader Name:    ", registryKey.GetValue("DisplayName")), "\r\n")));
					num2 = 112;
					rtfError.AppendText(Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("Hyview Reader Version:    ", registryKey.GetValue("DisplayVersion")), "\r\n")));
				}
			}
			else
			{
				num2 = 115;
				num2 = 116;
				rtfError.AppendText(Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("Hyview Reader Name:    ", registryKey.GetValue("DisplayName")), "\r\n")));
				num2 = 117;
				rtfError.AppendText(Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("Hyview Reader Version:    ", registryKey.GetValue("DisplayVersion")), "\r\n")));
			}
			num2 = 119;
			registryKey.Close();
			num2 = 120;
			registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP", false);
			num2 = 121;
			string[] subKeyNames = registryKey.GetSubKeyNames();
			num2 = 122;
			checked
			{
				int num5 = subKeyNames.Length - 1;
				for (int i = 0; i <= num5; i++)
				{
					num2 = 123;
					rtfError.AppendText(".net Framework:    " + subKeyNames[i] + "\r\n");
					num2 = 124;
				}
				num2 = 125;
				registryKey.Close();
				num2 = 126;
				registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\DataAccess", false);
				num2 = 127;
				rtfError.AppendText(Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("MDAC version:    ", registryKey.GetValue("Version", 0.0)), "\r\n")));
				num2 = 128;
				registryKey.Close();
				num2 = 129;
				rtfError.AppendText("Desktop path:    " + Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\r\n");
				num2 = 130;
				rtfError.AppendText("Desktop path:    " + MyProject.Computer.FileSystem.SpecialDirectories.Desktop + "\r\n");
				num2 = 131;
				rtfError.AppendText("MyDocuments path:    " + Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\r\n");
				num2 = 132;
				rtfError.AppendText("MyDocuments path:    " + MyProject.Computer.FileSystem.SpecialDirectories.MyDocuments + "\r\n");
				num2 = 133;
				string setting = Interaction.GetSetting("HyviewReader", "Settings", "ProxyAddr");
				num2 = 134;
				string setting2 = Interaction.GetSetting("HyviewReader", "Settings", "ProxyPort");
				num2 = 135;
				string setting3 = Interaction.GetSetting("HyviewReader", "Settings", "ProxyAcc");
				num2 = 136;
				string setting4 = Interaction.GetSetting("HyviewReader", "Settings", "ProxyChk", "0");
				num2 = 137;
				rtfError.AppendText("Proxy:    " + setting + "\r\n");
				num2 = 138;
				rtfError.AppendText("Port:    " + setting2 + "\r\n");
				num2 = 139;
				rtfError.AppendText("Acc:    " + setting3 + "\r\n");
				num2 = 140;
				if (Operators.CompareString(setting4, "0", false) == 0)
				{
					num2 = 141;
					rtfError.AppendText("Proxy Select:    不使用Proxy\r\n");
				}
				else
				{
					num2 = 143;
					if (Operators.CompareString(setting4, "1", false) == 0)
					{
						num2 = 144;
						rtfError.AppendText("Proxy Select:    使用系統設定\r\n");
					}
					else
					{
						num2 = 146;
						if (Operators.CompareString(setting4, "2", false) == 0)
						{
							num2 = 147;
							rtfError.AppendText("Proxy Select:    手動設定\r\n");
						}
					}
				}
				num2 = 149;
				rtfError.AppendText("//===========================Other Info=============================//\r\n");
				num2 = 150;
				AsmComparer comparer = new AsmComparer();
				num2 = 151;
				list.Sort(comparer);
				num2 = 152;
				List<Assembly>.Enumerator enumerator13 = list.GetEnumerator();
				while (enumerator13.MoveNext())
				{
					Assembly current6 = enumerator13.Current;
					num2 = 153;
					if (Operators.CompareString(current6.Location.ToUpper(), Application.ExecutablePath.ToUpper(), false) != 0)
					{
						num2 = 156;
						rtfError.AppendText("Loaded Assembly:   " + current6.ToString() + "\r\n");
					}
					num2 = 157;
				}
				((IDisposable)enumerator13).Dispose();
				num2 = 158;
				rtfError.AppendText("\r\n");
			}
		}
		catch (object obj) when (obj is Exception && num != 0 && num6 == 0)
		{
			ProjectData.SetProjectError((Exception)obj);
			/*Error near IL_1297: Could not find block for branch target IL_1259*/;
		}
		if (num6 != 0)
		{
			ProjectData.ClearProjectError();
		}
	}

	private string FormatBytes(long bytes)
	{
		if (bytes < 1000)
		{
			return Conversions.ToString(bytes) + "B";
		}
		if (bytes < 1000000)
		{
			return Strings.FormatNumber((double)bytes / 1024.0, 2) + "KB";
		}
		if (bytes < 1000000000)
		{
			return Strings.FormatNumber((double)bytes / 1048576.0, 2) + "MB";
		}
		return Strings.FormatNumber((double)bytes / 1073741824.0, 2) + "GB";
	}

	private void Form1_Shown(object sender, EventArgs e)
	{
		string sourceFileName = Environment.GetEnvironmentVariable("WINDIR") + "\\Microsoft.NET\\Framework\\v2.0.50727\\config\\machine.config";
		string destFileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\HyviewReader\\log\\machine.config";
		File.Copy(sourceFileName, destFileName, true);
		sourceFileName = Environment.GetEnvironmentVariable("WINDIR") + "\\Microsoft.NET\\Framework\\v2.0.50727\\config\\machine.config.default";
		destFileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\HyviewReader\\log\\machine.config.default";
		File.Copy(sourceFileName, destFileName, true);
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\HyviewReader";
		string text2 = text + "\\log";
		string text3 = text + "\\hyviewer.dat";
		string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\HyviewReader\\log\\hyviwer_env.txt";
		if (Directory.Exists(text))
		{
			if (Directory.Exists(text2))
			{
				StreamWriter streamWriter = new StreamWriter(path, false, Encoding.Default);
				streamWriter.Write(rtfError.Text);
				streamWriter.Close();
				string text4 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\hyview_env.zip";
				string fileName = Application.StartupPath + "\\7za.exe";
				Process.Start(fileName, "a -tzip \"" + text4 + "\" \"" + text2 + "\" \"" + text3 + "\"");
				object objectValue = RuntimeHelpers.GetObjectValue(Interaction.CreateObject("Scripting.FileSystemObject"));
				Interaction.MsgBox("己完成，請將桌面上的hyview_env.zip\r\n以附檔寄到客服信箱，謝謝。");
				MemoryStream memoryStream = new MemoryStream();
				string url = "http://61.63.46.175/hyviewservice/cipher/drm/getEncryptLSOFile";
				string data = "<?xml version=\"1.0\" encoding=\"utf-8\"?><body><id>cp001</id><timestamp>2013-03-06T02:26Z</timestamp><cipherText>rbXGUx7fvH+Pl0MnUpr3L4KPBsKYCzp2pHcuZqTv4K9dPxL0PNEskY9hOFKoKOPrhp3tlHuVmmGI9y65jNHqGQzs2abhtkOfLH6UhgREg5bhUS0/IT4UWYB5WfCGC7cMZT/kSc7tTe1Lba9oZXZ67SIImMWpLzuuTTMJD5a2KFZl2snRvX9IgLJnMmpUQjuU+wfnCrVNYaSSdZQ7ixbl6h7a7jsTgiusFZL6/sgmWnI+pMrBdF2bu9OBcpZ0piLys3ooS0aLiSz/wayDT5q8RXrw4upM3e8T1wm1UhK9HJeTNOR0ipDAO0KvQJZZEgSykfRzX6131P37xIDn7NeNmg==</cipherText></body>";
				string serialno = "YDkBcmjpOVIpGV13SsWKtKbMlKa+WtyW";
				string kvh = "5ee0236ae8bcabecf8388757b02e729e";
				memoryStream = getServiceStream(url, data, serialno, kvh);
			}
			else
			{
				Interaction.MsgBox("log路徑不存在");
			}
		}
		else
		{
			Interaction.MsgBox("log路徑不存在");
		}
	}

	private void Button1_Click(object sender, EventArgs e)
	{
		rtfError.SelectionStart = 0;
		rtfError.SelectionLength = Strings.Len(rtfError.Text);
		rtfError.Focus();
		Clipboard.Clear();
		Clipboard.SetDataObject(rtfError.SelectedText);
	}

	private void Button2_Click(object sender, EventArgs e)
	{
		Close();
	}

	public MemoryStream getServiceStream(string url, string data, string serialno, string kvh)
	{
		try
		{
			ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
			int num = 0;
			int num2 = 1;
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.KeepAlive = true;
				httpWebRequest.Proxy = null;
				if (Operators.CompareString(serialno, "", false) != 0)
				{
					httpWebRequest.Headers.Add("request-serial-no", serialno);
				}
				if (Operators.CompareString(kvh, "", false) != 0)
				{
					httpWebRequest.Headers.Add("kvh", kvh);
				}
				int num3 = 2;
				httpWebRequest.Method = "POST";
				httpWebRequest.ServicePoint.Expect100Continue = false;
				httpWebRequest.Timeout = 10000;
				byte[] bytes = aSCIIEncoding.GetBytes(data);
				httpWebRequest.ContentType = "text/xml";
				httpWebRequest.ContentLength = bytes.Length;
				Stream requestStream = httpWebRequest.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
				Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
				byte[] buffer = new byte[10241];
				int num4 = 0;
				int num5 = responseStream.Read(buffer, 0, 10240);
				MemoryStream memoryStream = new MemoryStream();
				while (num5 > 0)
				{
					num4 = checked(num4 + num5);
					memoryStream.Write(buffer, 0, num5);
					num5 = responseStream.Read(buffer, 0, 10240);
				}
				return memoryStream;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				MemoryStream result = null;
				ProjectData.ClearProjectError();
				return result;
			}
		}
		catch (Exception ex3)
		{
			ProjectData.SetProjectError(ex3);
			Exception ex4 = ex3;
			MemoryStream result = null;
			ProjectData.ClearProjectError();
			return result;
		}
	}

	private void Button3_Click(object sender, EventArgs e)
	{
	}
}
