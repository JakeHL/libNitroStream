using System;

namespace libNitroStream
{
	public class ClientManager
	{

        public enum ConnectionIntents
        {
            RemotePlay,
            MemoryPatch
        }

		public NtrClient Client;

        private ConnectionIntents _NextConnectionIntent;
        private ViewSettings _ViewSettings;

		public ClientManager(ViewSettings vs)
		{
			Client = new NtrClient ();
            _ViewSettings = vs;
            Client.Connected += onClientConnected;
		}

        public void onClientConnected (bool Connected)
        {
            if (_NextConnectionIntent == ConnectionIntents.RemotePlay)
            {
                uint priority = (uint)(_ViewSettings.PriorityMode ? 1 : 0);
                remoteplay(priority, _ViewSettings.PriorityFactor, _ViewSettings.PictureQuality, _ViewSettings.QosValue);
                ViewerManager.Launch(_ViewSettings);
            }
            else
            {
                byte[] bytes = { 0x70, 0x47 };
                WriteToDeviceMemory(0x0105AE4, bytes, 0x1a);
            }

            Logger.Log("Client will disconnect in 10 seconds.");
            System.Timers.Timer disconnectTimeout = new System.Timers.Timer(1000);
            disconnectTimeout.Elapsed += delegate
            {
                    Disconnect();
                    disconnectTimeout.Stop();
            };
            disconnectTimeout.Start();
        }

        public void Initiate(ConnectionIntents intent)
        {
            _NextConnectionIntent = intent;
            Connect(_ViewSettings.IPAddress, 8000);
        }

		public void Connect(string host, int port)
		{
			Client.setServer(host, port);
			Client.connectToServer();
		}

		public void Disconnect()
		{
			Client.disconnect();
		}

		private void WriteToDeviceMemory(uint addr, byte[] buf, int pid = -1)
		{
			Client.sendWriteMemPacket(addr, (uint)pid, buf);
		}

		public void remoteplay(uint priorityMode = 0, uint priorityFactor = 5, uint quality = 90, double qosValue = 15)
		{
			uint num = 1;
			if (priorityMode == 1)
			{
				num = 0;
			}
			uint qosval = (uint)(qosValue * 1024 * 1024 / 8);
			Client.sendEmptyPacket(901, num << 8 | priorityFactor, quality, qosval);
            Logger.Log("Remoteplay Started.");
		}

	}
}

