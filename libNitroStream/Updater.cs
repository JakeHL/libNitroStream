using System.Text;
using System;

namespace libNitroStream
{
    public class Updater
    {
        
		public delegate void UpdateFoundEventHandler(object source, EventArgs e);
		public event UpdateFoundEventHandler UpdateFound;

        public bool UpdateAvailable { get; private set; }

        private bool _statsAvailable;
        private string _Stats;
        public string Stats { get {return _statsAvailable ? _Stats : "No stats, please run GetStats() first"; } }

        private string _LatestUrl;
        private Octokit.GitHubClient _Client;
        private string _User, _Repo;
        private string _LocalVersion;

        public Updater(string header, string user, string repo, string localver)
        {
			_Client = new Octokit.GitHubClient(new Octokit.ProductHeaderValue(header));
			_User = user;
			_Repo = repo;
			_LocalVersion = localver;
        }

        public async void CheckUpdate()
        {
            try
            {
                var Latest = await _Client.Repository.Release.GetLatest(_User, _Repo);  

                _LatestUrl = Latest.HtmlUrl;
                string ServerVer, LocalVer;
                ServerVer = Latest.TagName.Replace(".", "").Replace("v", "0");
                LocalVer = _LocalVersion.Replace(".","");
                int sver, lver;
                if (int.TryParse(ServerVer, out sver) && int.TryParse(LocalVer, out lver))
                {
                    if (sver > lver)
                        onUpdateFound();
                }
            }
            catch
            {
                // TODO Throw an exception here once they get written to the log
            }
        }

		public virtual void onUpdateFound()
		{
			if (UpdateFound != null)
				UpdateFound(this, EventArgs.Empty);
		}

        private async void GetStats()
        {
            var releases = await _Client.Repository.Release.GetAll("JakeHL", "NitroStream");
            int total = 0;
            StringBuilder sb = new StringBuilder();
            foreach (var r in releases)
            {
                int itotal = 0;
                StringBuilder ib = new StringBuilder();
                ib.Append(string.Format("Version {0} {1} \n", r.TagName, r.Name));
                foreach (var a in r.Assets)
                {
                    total += a.DownloadCount;
                    itotal += a.DownloadCount;
                    ib.Append(string.Format("{0} : {1} \n", a.Name, a.DownloadCount));
                }
                ib.Append(string.Format("Version total: {0} \n \n", itotal));
                sb.Append(ib.ToString());
            }
            sb.Append(string.Format("Running total: {0}", total));
            _statsAvailable = true;
            _Stats = sb.ToString();
        }

        public void GetUpdate()
        {
            System.Diagnostics.Process.Start(_LatestUrl);
        }

    }
}
