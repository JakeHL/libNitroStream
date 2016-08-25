using System;
using System.Text;

namespace libNitroStream
{
	public class ViewerManager
	{
		public static void Launch(ViewSettings vs)
		{
			if (System.IO.File.Exists(vs.ViewerPath))
			{
                Logger.Log("Starting NTRViewer.");
				StringBuilder args = new StringBuilder();

				args.Append("-l ");
				args.Append(((vs.ViewMode == Orientations.Vertical) ? "1" : "0") + " ");
				args.Append("-t " + vs.TopScale.ToString() + " ");
				args.Append("-b " + vs.BottomScale.ToString());

				System.Diagnostics.ProcessStartInfo p = new System.Diagnostics.ProcessStartInfo(vs.ViewerPath);
				p.Verb = "runas";
				p.Arguments = args.ToString().Replace(',','.');
				System.Diagnostics.Process.Start(p);
			}
			else
                Logger.Log("NTRViewer not found. Config -> Define NTR Viewer path");
		}
	}
}