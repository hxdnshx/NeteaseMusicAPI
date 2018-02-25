using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using NeteaseMusicAPI.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NeteaseMusicAPI
{
    public class NeteaseMusicAPI
    {
        private const string AesKey1 = "0CoJUm6Qyw8W8jud";
        private const string PubKey = "010001";
        private const string Modulus =
            "00e0b509f6259df8642dbc35662901477df22677ec152b5ff68ace615bb7b725152b3ab17a876aea8a5aa76d2e417629ec4ee341f56135fccf695280104e0312ecbda92557c93870114af6c9d05c4f7f0c3685b7a46bee255932575cce10b424d813cfe4875d3e82047b97ddef52741d546b8e289dc6935b3ece0462db0a22b8e7";

        private const string RandomSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private HttpClient _client;

        private string EncrpytAes(string str, string key)
        {
            Aes encrypt = new AesCryptoServiceProvider();
            encrypt.Mode = CipherMode.CBC;
            encrypt.Padding = PaddingMode.PKCS7;
            encrypt.IV = Encoding.UTF8.GetBytes("0102030405060708");
            encrypt.Key = Encoding.UTF8.GetBytes(key);

            var encryptor = encrypt.CreateEncryptor();
            var data = Encoding.UTF8.GetBytes(str);
            var result = encryptor.TransformFinalBlock(data, 0, data.Length);
            return Convert.ToBase64String(result);
        }

        private string RandomKey()
        {
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();
            for (int i = 0; i < 16; i++)
            {
                sb.Append(RandomSet[rand.Next(0, RandomSet.Length)]);
            }

            return sb.ToString();
        }

        protected string HttpPost(string uri, FormUrlEncodedContent content)
        {
            var result = _client.PostAsync(uri, content).Result;
            if (result.Content.Headers.ContentEncoding.Contains("gzip"))
            {
                var rawResult = result.Content.ReadAsByteArrayAsync().Result;
                var finResult = GZipHelper.Decompress_GZip(rawResult);
                return Encoding.UTF8.GetString(finResult);
            }

            return result.Content.ReadAsStringAsync().Result;
        }

        protected string HttpGet(string uri)
        {
            var result = _client.GetAsync(uri).Result;
            if (result.Content.Headers.ContentEncoding.Contains("gzip"))
            {
                var rawResult = result.Content.ReadAsByteArrayAsync().Result;
                var finResult = GZipHelper.Decompress_GZip(rawResult);
                return Encoding.UTF8.GetString(finResult);
            }

            return result.Content.ReadAsStringAsync().Result;
        }

        public string SendData(string uri, string content,string key = null)
        {
            var encrypted = EncrpytAes(content, AesKey1);
            key = key??RandomKey();
            encrypted = EncrpytAes(encrypted, key);
            var encryptedKey = RSAHelper.EncryptByPublicKey(key, Modulus, PubKey);
            var form = new FormUrlEncodedContent(new Dictionary<string, string>
            {

                {"params", encrypted},
                {"encSecKey", encryptedKey},
            });
            return HttpPost(uri, form);
        }

        public NeteaseMusicAPI()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 11_0 like Mac OS X) AppleWebKit/604.1.38 (KHTML, like Gecko) Version/11.0 Mobile/15A356 Safari/604.1");
            //client.DefaultRequestHeaders.Add("Content-Type", "application /x-www-form-urlencoded");
            _client.DefaultRequestHeaders.Add("Referer", "http://music.163.com");
            _client.DefaultRequestHeaders.Add("Origin", "http://music.163.com");
            _client.DefaultRequestHeaders.Add("Host", "music.163.com");
            _client.DefaultRequestHeaders.Add("Accept", "*/*");
            _client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            _client.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.8");
        }


        /// <summary>
        /// {
        /// "id": playlist_id,
        /// "offset": 0,
        /// "total": True,
        /// "limit": 1000,
        /// "n": 1000,
        /// "csrf_token": csrf
        /// }
        /// </summary>
        /// <param name="playlistid"></param>
        /// <returns></returns>
        public JObject GetPlaylistDetail(int playlistid, int offset = 0,bool isTotal = true, int limit = 1000, int n = 1000)
        {
            JObject obj = new JObject
            {
                {"id", playlistid},
                {"offset", offset},
                {"total", isTotal},
                {"limit", limit.ToString()},
                {"n", n},
                {"csrf_token", ""}
            };
            var result = SendData("http://music.163.com/weapi/v3/playlist/detail?csrf_token=", obj.ToString());
            return JObject.Parse(result);
        }

        /// <summary>
        /// {"uid":"1","wordwrap":"7","offset":"0","total":"true","limit":"36","csrf_token":""}
        /// </summary>
        /// <returns></returns>
        public JObject GetUserPlaylists(int uid, int wordwarp = 7, int offset = 0, bool total = true, int limit = 36)
        {
            JObject obj = new JObject
            {
                {"uid", uid},
                {"wordwarp", wordwarp},
                {"offset", offset},
                {"total", total},
                {"limit", limit},
                {"csrf_token", ""}
            };
            var result = SendData("http://music.163.com/weapi/user/playlist", obj.ToString());
            return JObject.Parse(result);
        }

        //{"userId":"1","total":"true","limit":"20","time":"-1","getcounts":"true","csrf_token":""}
        public JObject GetUserEvents(int uid, bool total = true, int limit = 20, int time = -1, bool getcounts = true)
        {
            JObject obj = new JObject
            {
                {"userId", uid},
                {"total", total},
                {"limit", limit},
                {"time", time},
                {"getcounts", getcounts},
                {"csrf_token", ""}
            };
            var result = SendData($"http://music.163.com/weapi/event/get/{uid}", obj.ToString());
            return JObject.Parse(result);
        }

        /// <summary>
        /// {"uid":"1","offset":"0","total":"true","limit":"20","csrf_token":""}
        /// </summary>
        /// <returns></returns>
        public JObject GetUserFollows(int uid, int offset = 0, bool total = true, int limit = 20)
        {
            JObject obj = new JObject
            {
                {"uid", uid},
                {"offset", offset},
                {"total", total},
                {"limit", limit},
                {"csrf_token", ""}
            };
            var result = SendData($"http://music.163.com/weapi/user/getfollows/{uid}", obj.ToString());
            return JObject.Parse(result);
        }

        /// <summary>
        /// {"userId":"1","offset":"0","total":"true","limit":"20","csrf_token":""}
        /// </summary>
        /// <returns></returns>
        public JObject GetUserFollowers(int uid, int offset = 0, bool total = true, int limit = 20)
        {
            JObject obj = new JObject
            {
                {"userId", uid},
                {"offset", offset},
                {"total", total},
                {"limit", limit},
                {"csrf_token", ""}
            };
            var result = SendData("http://music.163.com/weapi/user/getfolloweds", obj.ToString());
            return JObject.Parse(result);
        }

        /// <summary>
        /// {"uid":"1","type":"-1","limit":"1000","offset":"0","total":"true","csrf_token":""}
        /// Type:1 for weekly record, -1 for all record
        /// </summary>
        /// <returns></returns>
        public JObject GetUserPlayRecords(int uid, int type = -1, int offset = 0, bool total = true, int limit = 1000)
        {
            JObject obj = new JObject
            {
                {"uid", uid},
                {"type", type},
                {"limit", limit},
                {"offset", offset},
                {"total", total},
                {"csrf_token", ""}
            };
            var result = SendData($"http://music.163.com/weapi/v1/play/record", obj.ToString());
            return JObject.Parse(result);
        }

        private static readonly Regex jsonfinder = new Regex("window\\.REDUX_STATE = ([^\n]+});");

        public JObject GetUserBriefInfo(int uid)
        {
            var result = HttpGet($"http://music.163.com/user/home?id={uid}");
            var match = jsonfinder.Match(result);
            if (match.Success == false)
                return null;
            var jsontxt = match.Groups[1].Value;
            var json = JObject.Parse(jsontxt);
            return json["User"]["info"]["profile"] as JObject;
        }

        public JObject GetLyric(int id, int lv = -1, int tv = -1)
        {
            JObject obj = new JObject
            {
                {"id", id},
                {"lv", lv},
                {"tv", tv},
                {"csrf_token", ""}
            };
            var result = SendData($"http://music.163.com/weapi/song/lyric", obj.ToString());
            return JObject.Parse(result);
        }

        public JObject GetComments(string id, int offset = 0, bool total = true, int limit = 1000)
        {
            JObject obj = new JObject
            {
                {"rid", id},
                {"offset", offset},
                {"total", total},
                {"limit", limit},
                {"csrf_token", ""}
            };
            var result = SendData($"http://music.163.com/weapi/v1/resource/comments/{id}", obj.ToString());
            return JObject.Parse(result);
        }

        public JObject GetSongComments(int id, int offset = 0, bool total = true, int limit = 1000)
        {
            return GetComments($"R_SO_4_{id}", offset, total, limit);
        }

        public JObject GetAlbumComments(int id, int offset = 0, bool total = true, int limit = 1000)
        {
            return GetComments($"R_AL_3_{id}", offset, total, limit);
        }

        public JObject GetPlaylistComments(int id, int offset = 0, bool total = true, int limit = 1000)
        {
            return GetComments($"R_AL_0_{id}", offset, total, limit);
        }
    }

}
