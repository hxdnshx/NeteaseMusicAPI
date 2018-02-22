using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;

namespace NeteaseMusicAPI
{
    public class RSAHelper
    {
        /*
         * From:http://blog.csdn.net/jiangxinyu/article/details/4712137
         * 此处只有一个转载来源，最初来源无法考证.
         * 由于最终输出的是string，与网易api不符等原因进行了一定修改
         */



        /// <summary>
        /// 通过公钥加密
        /// </summary>
        /// <param name="dataStr">待加密字符串</param>
        /// <param name="n">大整数n</param>
        /// <param name="e">公钥</param>
        /// <returns>加密结果</returns>
        public static string EncryptByPublicKey(string dataStr, string n, string e)
        {
            //大整数N
            BigInteger biN = BigInteger.Parse(n, NumberStyles.HexNumber);
            //公钥大素数
            BigInteger biE = BigInteger.Parse(e, NumberStyles.HexNumber);
            //加密
            var result = EncryptString(dataStr, biE, biN);
            for (;;)
            {
                if (result.First() == '0' && result.Length % 2 != 0)
                    result = result.Substring(1);
                else
                {
                    break;
                }
            }
            return result;
        }

        private static string EncryptString(string dataStr, BigInteger keyNum, BigInteger nNum)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(dataStr);
            int len = bytes.Length;
            int len1 = 0;
            if ((len % 120) == 0)
                len1 = len / 120;
            else
                len1 = len / 120 + 1;
            List<byte> tempbytes = new List<byte>();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < len1; i++)
            {
                var blockLen = 0;
                if (len >= 120)
                {
                    blockLen = 120;
                }
                else
                {
                    blockLen = len;
                }
                byte[] oText = new byte[blockLen];
                Array.Copy(bytes, i * 120, oText, 0, blockLen);
                string res = Encoding.UTF8.GetString(oText);
                BigInteger biText = new BigInteger(oText);
                BigInteger biEnText = BigInteger.ModPow(biText,keyNum,nNum);//biText.ModPow(keyNum, nNum);
                //补位
                string resultStr = biEnText.ToString("x");//biEnText.ToHexString();
                if (resultStr.Length < 256)
                {
                    for (int j = resultStr.Length; j < 256; j++)
                    {
                        builder.Append('0');
                    }
                }
                builder.Append(resultStr);
            }

            return builder.ToString();
        }
    }
}
