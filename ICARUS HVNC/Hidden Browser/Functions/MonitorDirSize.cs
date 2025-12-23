using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DLL.Functions
{
	public class MonitorDirSize
	{
		private Thread newthread;

		public void StartMonitoring(string directory)
		{
			newthread = new Thread(delegate (object a0)
			{
				WorkerThread(Conversions.ToString(a0));
			});
			newthread.IsBackground = true;
			newthread.SetApartmentState(ApartmentState.STA);
			newthread.Start(directory);
		}

		private void WorkerThread(string directory)
		{
			while (true)
			{
				try
				{
					if (Directory.Exists(directory))
					{
						Outils.SendInformation(Outils.nstream, "23|" + Conversions.ToString(Math.Round((double)new GetDirSize().GetDirSizez(directory) / 1024.0 / 1024.0)));
					}
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ProjectData.ClearProjectError();
				}
				Thread.Sleep(100);
			}
		}

		public void StopMonitoring()
		{
			newthread.Abort();
		}
	}
}
