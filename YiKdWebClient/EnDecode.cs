using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace YiKdWebClient
{   
    #pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public static class EnDecode
    {
        // Token: 0x060000B2 RID: 178 RVA: 0x00003500 File Offset: 0x00001700
        // Disable the warning.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Encode(object data)
        {
            string s = "KingdeeK";
            string s2 = "KingdeeK";
            string result;

            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(s);
                byte[] bytes2 = Encoding.ASCII.GetBytes(s2);
                byte[]? inArray = null;
                int length = 0;

#pragma warning disable SYSLIB0021
                using (DESCryptoServiceProvider descryptoServiceProvider = new DESCryptoServiceProvider())
                {
                    int keySize = descryptoServiceProvider.KeySize;
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, descryptoServiceProvider.CreateEncryptor(bytes, bytes2), CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                            {
                                streamWriter.Write(data);
                                streamWriter.Flush();
                                cryptoStream.FlushFinalBlock();
                                streamWriter.Flush();
                                inArray = memoryStream.GetBuffer();
                                length = (int)memoryStream.Length;
                            }
                        }
                    }
                }
                result = Convert.ToBase64String(inArray, 0, length);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string EncodeNew1(object data)
        {
            // 确保传入数据为字符串类型，可按需扩展支持更多类型
            //if (!(data is string))
            //{
            //    throw new ArgumentException("data 必须为字符串类型", nameof(data));
            //}

            string s = "KingdeeK";
            string s2 = "KingdeeK";
            string result;

            try
            {
                byte[] key = Encoding.ASCII.GetBytes(s);
                byte[] iv = Encoding.ASCII.GetBytes(s2);
                byte[]? inArray = null;
                int length = 0;

                using (DES des = DES.Create())
                {
                    des.Key = key;
                    des.IV = iv;

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            byte[] dataBytes = Encoding.UTF8.GetBytes((string)data);
                            cryptoStream.Write(dataBytes, 0, dataBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            inArray = memoryStream.ToArray();
                            length = inArray.Length;
                        }
                    }
                }
                result = Convert.ToBase64String(inArray, 0, length);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <param name="encoding"></param>
        /// <param name="isHex"></param>
        /// <returns></returns>
        // Token: 0x060000B3 RID: 179 RVA: 0x0000362C File Offset: 0x0000182C
        public static string HmacSHA256(string message, string secret, Encoding encoding, bool isHex = false)
        {
            secret = (secret ?? "");
            byte[] bytes = encoding.GetBytes(secret);
            byte[] bytes2 = encoding.GetBytes(message);
            string result;
            using (HMACSHA256 hmacsha = new HMACSHA256(bytes))
            {
                byte[] array = hmacsha.ComputeHash(bytes2);
                if (isHex)
                {
                    string s = EnDecode.ByteToHexStr(array).ToLower();
                    result = Convert.ToBase64String(encoding.GetBytes(s));
                }
                else
                {
                    result = Convert.ToBase64String(array);
                }
            }
            return result;
        }

        // Token: 0x060000B4 RID: 180 RVA: 0x000036A8 File Offset: 0x000018A8
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToHexStr(byte[] bytes)
        {
            string text = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    text += bytes[i].ToString("X2");
                }
            }
            return text;
        }

        // Token: 0x060000B5 RID: 181 RVA: 0x000036E5 File Offset: 0x000018E5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        internal static string EncryptAppSecret(string appSecret)
        {
            if (Regex.IsMatch(appSecret, "^([0-9a-zA-Z]{32})$"))
            {
                return Convert.ToBase64String(EnDecode.XOREncode(Convert.FromBase64String(appSecret)));
            }
            return EnDecode.ROT13Encode(appSecret);
        }

        // Token: 0x060000B6 RID: 182 RVA: 0x0000370B File Offset: 0x0000190B
        internal static string DecryptAppSecret(string appSecret)
        {
            if (appSecret.Length == 32)
            {
                return Convert.ToBase64String(EnDecode.XOREncode(Convert.FromBase64String(appSecret)));
            }
            return EnDecode.ROT13Encode(appSecret);
        }

        // Token: 0x060000B7 RID: 183 RVA: 0x00003730 File Offset: 0x00001930
        private static byte[] XOREncode(byte[] input)
        {
            string s = "0054f397c6234378b09ca7d3e5debce7";
            byte[] array = new byte[input.Length];
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            for (int i = 0; i < input.Length; i++)
            {
                array[i] = BitConverter.GetBytes((int)(input[i] ^ bytes[i]))[0];
            }
            return array;
        }

        // Token: 0x060000B8 RID: 184 RVA: 0x00003778 File Offset: 0x00001978
        private static string ROT13Encode(string InputText)
        {
            string text = "";
            for (int i = 0; i < InputText.Length; i++)
            {
                int num = (int)Convert.ToChar(InputText.Substring(i, 1));
                if (num >= 97 && num <= 109)
                {
                    num += 13;
                }
                else if (num >= 110 && num <= 122)
                {
                    num -= 13;
                }
                else if (num >= 65 && num <= 77)
                {
                    num += 13;
                }
                else if (num >= 78 && num <= 90)
                {
                    num -= 13;
                }
                text += ((char)num).ToString();
            }
            return text;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>

        // Token: 0x060000B9 RID: 185 RVA: 0x00003800 File Offset: 0x00001A00
        public static string UrlEncodeWithUpperCode(string str, Encoding encoding)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char value in str)
            {


                if (System.Web.HttpUtility.UrlEncode(value.ToString()).Length > 1)

                {
                    stringBuilder.Append(System.Web.HttpUtility.UrlEncode(value.ToString(), encoding).ToUpper());




                }
                else
                {
                    stringBuilder.Append(value);
                }
            }
            return stringBuilder.ToString();
        }
    }
}
