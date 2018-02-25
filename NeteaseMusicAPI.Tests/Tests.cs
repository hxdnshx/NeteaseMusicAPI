using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeteaseMusicAPI.Tests
{
    [TestClass]
    public class Tests
    {
        private const string AesKey1 = "0CoJUm6Qyw8W8jud";
        private const string PubKey = "010001";
        private const string Modulus =
            "00e0b509f6259df8642dbc35662901477df22677ec152b5ff68ace615bb7b725152b3ab17a876aea8a5aa76d2e417629ec4ee341f56135fccf695280104e0312ecbda92557c93870114af6c9d05c4f7f0c3685b7a46bee255932575cce10b424d813cfe4875d3e82047b97ddef52741d546b8e289dc6935b3ece0462db0a22b8e7";

        private const int TestNeteaseID = 375540586;//随便选的一个测试id


        [TestMethod]
        public void RSATest()
        {
            Dictionary<string,string> testValues = new Dictionary<string, string>
            {
                {"tENV2V1qazRhfL2V", "43f3587ea7dd5c19848c6aa704d9b81f01312b978d1501e5d6a5b59fa575fc95af7a96705f987e690d6e43e0ca2a657e31993bacb093ed30295fc04be6c6bff26b37e8d0aa46c47cd0cf91c0b826a4c3da5d4a761366658bd7f94a1f9ee070d6e536c68b019d796ad6876ce96bbaf3657034f02a1e54129bfeadf8de12642aa3"},
                {"Odcsdlcx4tK2kbkZ", "debc4f2b175f7cf0340edad68315a2b15ce6eca4642a40c6e4d9b66d025676bb78aa66278cafca138a1ae56f6f63d75933d9fb1340baff41d87287f40f66f5da3161e9d0aa062cc6a5d5c4ab97a86c854ef2ca63204a9dea6d01baf6d987fe675873b9ebda727869fe85a25fd06b273c7f87dbf134165a181b2480e2ab244741"},
                {"Bwkj1LLNk5GE83KW", "196e553c2d6752e51729311131ec82ceea94a272742b0721f02c1b0ef0b63acbabb33df8cad72024da387ccf92c065faa5fa54d91f2e06e71c8948b40f000443e3f4bb9e7733448a3d5040f65b5ed8a790a8e8dda8ecccde39a744eac8cb22c1d56df98481dd675ddff345118a7a16568a840b09b0aeb57f18d6ac85daab7993"},
                {"yLWqByPTdohGYGkV", "c3a7d879cc01e60dbf645800dd79152fa32e960d51a13be6a381191c4ff3e0ceaf481ae60cd92c7b5ff1750c88a8e113953716ff49322897c100f9662efee3c5e3de3566948c6caee3b0a8ddacc6ec68d406d3d3824853467e5a06953f9b418007350a270583580999d6b3ae6643fad1f4eeeb62fade19db6aa3fe8faa17a874"},
                {"CmFXKvmhoELzu892", "c53c7fba6d297a63fdd5f6fc7f1236afa09d54903717a88c3c6276f7897d5d5a26643b1b90621fbf2f423ac9d3809972e40f67c27c58b160cb936648af671ec1f3ffe1b7f15932fece365ee084a47c5aedbc0abfc8ceb4c2190d4669573b7c57447e415cc49ff732a750c313596f4df54e02afeb3c9d2d5f3862fdd484883c58"},
                {"q5BhbiN9PdHmwlRM", "9ac030f35abfb70a5524d9e5cae0ef3cf04145e83a9cf54c35a1f38b2cab2f3f562af385974adbf7d8f819abc86d5f7753958861c959cfae58e29ce5230c48a1e3da6db3500d1e0c567326c00f02d27c77b5698f0542d8efeda69a4fc2e6bcca830171519fc8acd395d4b19b49dcd990ce21297b9ca3ad92d64fa86e87a3e05e"},
                {"DXviAvflnoGfMhFr", "9b3a15615f9b1ec40469c3aea145535ad1abacc0c5918dae1dead0ef6e5de7a93530071442f44c7584a1f5418f645ed8bf444dbd93cc813cec6122e5cc5b6d863c8a67503762ffd9979e86e4783d26363072536173239264c7abfe4daf75e4832ab9d28e52b5e6fb2306bd7760a64de06be72613bf58fdd481b53f777380c8c6"},
                {"kfc2Ho5DOggKlsB3", "0a7135f60d4154dda3b35d89d9f960d55c3afec54c36440014c286c4827619ef681e8a372f8167a65d0f9e3acde975b1f8cdf260668e7d8920b96f0845c1b9698952281c69c3e5cc7e02bea3c3fd22ddcd142535c6889fefe66b87121f24ec8a3bc532bf864460c426dfee695987a28a252708bc89c2aefb46a0ccd30bf31a3e"},
                {"i86jIMm9dhy5r6HD", "5287992f61e6510d4b5cca02710f33910d2daed44abd554c4414893474234d0da950ac37aaac8b5bf7aa0d0777903fa8ff12dbe9840b781100ef4f81fc751770119ffa48a5588f7d0532f0a291cae57a60ca7cf3de7eb3402ce258b74d8a6ab2ed7a1078b4735c75dcc43ce9d3fe54f4579a58d87dec1ab43c00852417d37854"},
                {"zZPG6TWokLiWDzHb", "2ad824139161f33dd35a424f133988ba6f05b74d2ff8cc2eb654ea8fe20aa408046bb13fb672174ab264520fc2582480f163ed83fba143b972fef0d3d98159c72fcede785de0c8d6885647a638e15513293afdd025a44989c98cfa5b4fa4041da9298c1fe5d35ed1b0471d857111cd8beb341ca187d13bf21c105023fc8ce0d9"},
                {"MIuZI74e7YJUgy3r", "6437513d03b418ceadcaf6f25f87d88ab5e228b45536bf46b98f2355376bed1bc792c3b2e36496035da1fb7a8ffc44cd87038a97e546445c1e6ce76b9e616f354301c958d7cb4ed65c34cea6f03f97c06d2ef57b65ba7fd3afee3316b5b41e3560f8a93cfb9539ccecd536137226eb025eb8c528e684133eed2c1787e5dd26bf"},
                {"RgEfsGFMKgnWqvu8", "d64c7fd7c7a9aff8a8047512e8a3ce32ef6cf58954d8f1634ced6e8ce66efcbcf88cec146e3a084219946b2a0b30c9cb4efaac7eb0da0f8f2d0a37e54e35d9266d94cfd2f7c19828e5b29722e7d2b3993f70862ac427c5659a1673e99888f1a83febb5f17bb08eb3451febfe1bdd07a5fc714cb29bcff9685752f0eed8274a31"},
                {"135y3FoyCnlRGzTl", "9eabd078ac706b3a754e46433e25d49dad8f4c0013a3af8bb3615aebfe13b2d9f129d5c0c5a69f1f15082fa78eb98bb0249c05dc90d78155b764459b818ef11838477be308e858ba7463a6733f75d6e7ce452c1d94094771a6362e22530482850fcdd4dd2eb4f77666469c52fd563708a7f0c43b38c2baa00be2bc453d80c591"},
                {"dZrL9r0W5Yj9yB7l", "417320a03fba100122f0440d42fe829b506ebec363ca0e5ce651ed63d9f444451bc0e0a89cb395b624e17509b623b614244942a295e5e17af384c44e5a35c57b19bdb0391a743c38eb25b9fc630a5fc5fd61bfd4aad3292a9024e6209da1570e352288d49c83f5ee214d0d2406d301563eda574f8ef6cd67860fd882f6936aae"},
                {"iNXTrX3qLAjCNhal", "2255a4a142c6cd8c0fc9a309ebf76060b34396a22d6083771e5fe5409eba421bf3f1eec013c7ac7c3d1134cb8aee4b1fa94b1335f9b80e835e0bc70ddd6961c403f2cc7c2b50d0f7f55f43573963542ee9ff6560744fe19cd03fb6064906d8f5d5adda62a4e6019918919314230e3fccbd7f22b752df64cec3873ba104fbc90c"},
                {"9Y0XCBNo1qeIZ8Pl", "450b1d2f750a60c0ebb6e1d3b2a821dcb1b8b6e01b7a85a4adf9282faa410d4b2c716039ea9ea0322bb46450dab0c73724ca4d452ce34f5b5be6d5bf16c521557c37a1368efc97bd7fc37f15d482c251f095c5d154f5602449ad1e3ed3bff6fd42d52e9b9a847fbfb2f1ff148464b4ec67b8adc5638fb058a7c720a909193f70"},
            };
            //src:
            //result:
            foreach (var testValue in testValues)
            {
                string result = RSAHelper.EncryptByPublicKey(testValue.Key, Modulus, PubKey);
                Assert.AreEqual(testValue.Value, result);
            }
        }

        /*
         * "{"logs":"[{\"action\":\"bannerimpress\",\"json\":{\"type\":\"1_歌曲\",\"url\":\"/song?id=536570080\",\"id\":\"536570080\",\"position\":3}}]","csrf_token":""}"
         */
        //"daGV1Oz01eIDx8iS"
        //"AtAnN95rq7Xc02cX8h6NhRmf6BeR7rV1bJao+PrmqWEwyONtlK2F0REclPc7wru6UnavXhtuNJSyXIVY7kG0pWW2y0fDN+h0XZgUUp6ebMgMG/IZ2p4i2JhhPXT9hV0rq02/PeXFvLG9FfvdsAqbRc9zvtqjPsNm0YeMH/W7e9lMqQ6GDuJo6felBAQskqLJMj1qmgdrA/wYeoGHvFFgMIO1xwPHisvIRPyzEqZtF5Pq/lzbxIMayLmh7Nz5Ksed+iezGN6vSMYVXrtgT1IVaDX+l7NcJdw1MZWtZTVlWyw="
        //"8a1c6ae53616bb3d6b011d3b03f92c9cc7342777c3c1f5f57b85d95166b29c1c43f8b8c5bbc3d84bd9d9a74d93328f7e69c2aa43a90a2736c0cb0edb167094d8f71214026c3e2ba4da3962c97f7ae46841ec7f26a9870d7f838dcba10a7aace8c6edbf26118dd18c2174de49f4f7232dc389e0d52c60c86c27b65cb86521b9ab"
        [TestMethod]
        public void HttpPostTest()
        {
            NeteaseMusicAPI api = new NeteaseMusicAPI();
            var mi = typeof(NeteaseMusicAPI).GetMethod("HttpPost", BindingFlags.NonPublic | BindingFlags.Instance);
            var CallFunc =
                (Func<string, FormUrlEncodedContent, string>) Delegate.CreateDelegate(
                    typeof(Func<string, FormUrlEncodedContent, string>), api, mi);

            //Text Response Request
            {
                var form = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {
                        "params",
                        "T9tlab0mBb/kSLfGU72O8fS0DismJUSO0rGS7t0I4foDPGWTLkAFy61fvw7ddoZIXvcsAypCrfvVvsZtQqi32UiQtIWVTre7mgJnTzPXwo2NW8ctoQIvH2gZmVYBeGiqWsfh/+9ct4ZzDds56q3ETMPWPbjL40UF0o29e0tChzy+72VAbzbxrFfY9F0OfrMBYx5y+JhigZEbmQv9BPDTq9DD+CmU+Oaek2EAz6QfvA52Gw0WCr/v45x7jro8f+UgocazZeMF0dbWia175e4qHD4ALt8smEpR0S3Ipbk5zA+MBe9EHijVHzq7RV5+ib3siJoq49804fAeqlR8zm225nqJ6X/HyTsUHNzh+Pudsh0="
                    },
                    {
                        "encSecKey",
                        "6437513d03b418ceadcaf6f25f87d88ab5e228b45536bf46b98f2355376bed1bc792c3b2e36496035da1fb7a8ffc44cd87038a97e546445c1e6ce76b9e616f354301c958d7cb4ed65c34cea6f03f97c06d2ef57b65ba7fd3afee3316b5b41e3560f8a93cfb9539ccecd536137226eb025eb8c528e684133eed2c1787e5dd26bf"
                    }
                });
                var result = CallFunc("http://music.163.com/weapi/feedback/weblog", form);
                Assert.AreEqual("{\"code\":200}", result);
            }
            /*
            //GZip Response Request
            {
                var form = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {
                        "params",
                        "sjOijmHnXA/HOpUj9YcfcfD50GETwiGotS6v+I6+js1tT+9Y1/700Xw/MliMEg2P"
                    },
                    {
                        "encSecKey",
                        "c3aca22b9f1121390d34ea415914c59c1fa7a523a94a4452a77d6e3d320626e451afe402b2fa28fc3232d8fe0920251763d03ddd2b63ef4d569eed2460a93d987519db02ade38effe49c5fde9d7db186c28056ee9d305db73a5856d6eadc99e86b46df164ae14461b5e40e7c34c675de1071b34d0bf86606bf3e3ac4dafcba63"
                    }
                });
                var result = CallFunc("http://music.163.com/weapi/discovery/recommend/resource", form);
                Assert.AreEqual("{\"msg\":null,\"code\":301}", result);
            }
            */
        }

        //"{"rid":"A_PL_0_740840138","offset":"0","total":"true","limit":"20","csrf_token":""}"
        //"viK2GeGEcwnIPOzB"
        //sec:"573588d2e3f2dec75ed56e05e461ef6a8de7275fb796d0fff2d02314a590255a8fbebf017a1f39410b1564888036c6bee6ab9bbc3b8f1b75b00f335e5a44e566f5a65f6b7924995bbd8f6bb19927e68740acf4eb96437dbee6b29a5d27b50e9e3f3e2326d4fb42f41596ffae69e92ef7146d81868b9dd2624680824f8be1f3c7"
        //txt:"2CGVS8VUL00UuoTmqAe3YAHUrLpEAD5hGxLqzvAS3urKrNt+M1SFyORsGZr8F67Hu90Lc/gkLi0nDtytM2wZPLf1TYbkESxkWBctJm2xbSr+RhaGEHDi61IXzYQxSw7s2Gh7rrr82bG/7ihcLvOGkl26PLamiEYm5Lb8nJpSlOLAAn01ZfD1Zc+MVeJ5jltm"
        //url:"http://music.163.com/weapi/v1/resource/comments/A_PL_0_740840138"
        [TestMethod]
        public void SendRequestTest()
        {
            NeteaseMusicAPI api = new NeteaseMusicAPI();
            var result =    
                api.SendData("http://music.163.com/weapi/v1/resource/comments/A_PL_0_740840138",
                    "{\"rid\":\"A_PL_0_740840138\",\"offset\":\"0\",\"total\":\"true\",\"limit\":\"20\",\"csrf_token\":\"\"}", "viK2GeGEcwnIPOzB");
            Assert.IsTrue(result?.IndexOf("isMusician")!=-1);
            result =
                api.SendData("http://music.163.com/weapi/v1/resource/comments/A_PL_0_740840138",
                    "{\"rid\":\"A_PL_0_740840138\",\"offset\":\"0\",\"total\":\"true\",\"limit\":\"20\",\"csrf_token\":\"\"}");
            Assert.IsTrue(result?.IndexOf("isMusician") != -1);
        }

        [TestMethod]
        public void GetPlaylistTest()
        {
            NeteaseMusicAPI api = new NeteaseMusicAPI();
            var result = api.GetPlaylistDetail(TestNeteaseID);
        }

        [TestMethod]
        public void GetUserInfoTest()
        {
            NeteaseMusicAPI api = new NeteaseMusicAPI();
            var result = api.GetUserPlaylists(TestNeteaseID);
            result = api.GetUserEvents(TestNeteaseID);
            result = api.GetUserFollowers(TestNeteaseID);
            result = api.GetUserFollows(TestNeteaseID);
            result = api.GetUserPlayRecords(TestNeteaseID);
        }
    }
}
