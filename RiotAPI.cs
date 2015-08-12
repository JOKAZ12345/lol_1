using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.InteropServices.ComTypes;

namespace Riot_API_1
{
    class RiotApi
    {
        private string API_KEY = "";

        // URL ZONES
        const string EUW = "https://euw.api.pvp.net/api/lol/euw/v2.2";
        // TODO: ADD MORE

        // WHAT WE NEED ?
        const string match = "/match";
        // TODO: ADD MORE

        // timeline by default its true
        private bool timeLine = true;


        // FUNCTIONS
        public string GetDataFromUrl(string url)
        {
            using (var wc = new WebClient())
            {
                return wc.DownloadString(url); // TODO: Handle error responses (beh 429)
            }
        }

        public string MatchStringBuilder(string zone, string matchId, bool time)
        {
            return zone + match + "/" + matchId + "?includeTimeline=" + time.ToString() + "&api_key=" + API_KEY;
        }
    }
}
