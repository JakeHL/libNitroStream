using System;

namespace libNitroStream
{
	public enum Orientations
	{		
		Vertical = 0,
		Horizontal = 1
	}

	public class ViewSettings
	{
		string _IPAddress;
		public string IPAddress
		{
			get { return _IPAddress; }
			set
			{
				_IPAddress = value;
			}
		}

		bool _ShowLog;
		public bool ShowLog
		{
			get { return _ShowLog; }
			set
			{
				_ShowLog = value;
			}
		}

		double _TopScale;
		public double TopScale
		{
			get { return _TopScale; }
			set
			{
				_TopScale = value;
			}
		}

		double _BottomScale;
		public double BottomScale
		{
			get { return _BottomScale; }
			set
			{
				_BottomScale = value;
			}
		}

		Orientations _ViewMode;
		public Orientations ViewMode
		{
			get { return _ViewMode; }
			set
			{
				_ViewMode = value;
			}
		}

		uint _PictureQuality;
		public uint PictureQuality
		{
			get { return _PictureQuality; }
			set
			{
				_PictureQuality = value;
			}
		}

		uint _PriorityFactor;
		public uint PriorityFactor
		{
			get { return _PriorityFactor; }
			set
			{
				_PriorityFactor = value;
			}
		}

		double _QosValue;
		public double QosValue
		{
			get { return _QosValue; }
			set
			{
				_QosValue = value;
			}
		}

		bool _PriorityMode;
		public bool PriorityMode
		{
			get { return _PriorityMode; }
			set
			{
				_PriorityMode = value;
			}
		}

		string _ViewerPath;
		public string ViewerPath
		{
			get { return _ViewerPath; }
			set
			{
				_ViewerPath = value;
			}
		}

		public ViewSettings()
		{
			_IPAddress = "AAA.BBB.YYY.ZZZ";
			_TopScale = 1;
			_BottomScale = 1;
			_ViewMode = 0;
			_PriorityMode = false;
			_PriorityFactor = 5;
			_PictureQuality = 90;
			_QosValue = 15;
			_ViewerPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NTRViewer.exe");
			_ShowLog = true;
		}

		public void Save(string path)
		{
			if (System.IO.File.Exists(path))
			{
				System.IO.File.Delete(path);
			}

			System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(this.GetType());

			using (System.IO.StreamWriter s = new System.IO.StreamWriter(path))
			{
				xs.Serialize(s, this);
			}
		}

		public static ViewSettings Load(string path)
		{
			ViewSettings vs = new ViewSettings();
			if (System.IO.File.Exists(path))
			{
				System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(vs.GetType());

				using (System.IO.StreamReader s = new System.IO.StreamReader(path))
				{

					vs = (ViewSettings)xs.Deserialize(s);
					return vs;
				}
			}
			else
			{
				return null;
			}
		}
	}
}

