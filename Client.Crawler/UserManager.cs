using Crawler.Configuration;
using Crawler.Dtos;
using Crawler.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Crawler
{

    public class UserManager
    {
        public string AccessToken { get; set; }

        public string Username { get; set; }

        public bool Login(string username, string password)
        {
            var grant_type = "password";
            var urlEncodedPost = string.Format(
                "username={0}&password={1}&grant_type={2}",
                username,
                password,
                grant_type);
            var json = new Requester().Post(CrawlerConfig.TrackerLoginUrl, urlEncodedPost);
            
            if (json == null)
            {
                return false;
            }

            var js = new JavaScriptSerializer();
            var userData = js.Deserialize<UserDataDto>(json);
            if (userData.access_token != null)
            {
                this.AccessToken = userData.access_token;
                this.Username = username;
                return true;
            }

            return false;
        }
    }
}
