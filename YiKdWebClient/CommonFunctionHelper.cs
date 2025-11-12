
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace YiKdWebClient
{
    /// <summary>
    /// 通用功能函数
    /// </summary>
    public static class CommonFunctionHelper
    {
        /// <summary>
        /// Unix时间戳,定义为从格林威治时间1970年01月01日00时00分00秒起
        /// </summary>
        public static DateTimeOffset UnixStart = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
        /// <summary>
        ///  Unix 时间戳总毫秒数
        /// </summary>
        /// <returns></returns>
        public static long CurrentTimeMillis()
        {
            //  return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
            // 创建一个 DateTimeOffset 对象，表示 Unix 时间戳的起始时间（1970年1月1日）
            //// DateTimeOffset unixStart = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

            // 计算当前时间与起始时间之间的时间间隔
            TimeSpan unixTimeSpan = DateTimeOffset.UtcNow - UnixStart;

            // 通过 TotalSeconds 属性将时间间隔转换为秒数，并将其转换为长整型
            long unixTimestamp = (long)unixTimeSpan.TotalMilliseconds;

            return unixTimestamp;
        }
        /// <summary>
        ///  Unix 时间戳总秒数
        /// </summary>
        /// <returns></returns>
        public static long GetTimestamp()
        {
            // 计算当前时间与起始时间之间的时间间隔
            TimeSpan unixTimeSpan = DateTimeOffset.UtcNow - UnixStart;

            // 通过 TotalSeconds 属性将时间间隔转换为秒数，并将其转换为长整型
            long unixTimestamp = (long)unixTimeSpan.TotalSeconds;

            return unixTimestamp;

        }
        /// <summary>
        /// 判断地址最后是否为/，并补充
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetServerUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            try
            {
                if (!url.EndsWith("/"))
                {
                    return url + "/";
                }
            }
            catch (Exception)
            {

                //throw;
            }

            return url;
        }
        /// <summary>
        /// 接受一个字符串作为输入，并使用默认的 UTF-8 编码将其转换为字节数组。然后，它调用 Sha256Hex(byte[] bytes) 函数计算字节数组的 SHA-256 哈希值，并返回十六进制字符串表示。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Sha256Hex(string data)
        {
            return Sha256Hex(data, Encoding.UTF8);
        }
        /// <summary>
        /// 接受一个字符串作为输入，并使用默认的 UTF-8 编码将其转换为字节数组;允许你指定自定义的编码来处理输入字符串。你可以传递一个不同于 UTF-8 的编码对象
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>

        public static string Sha256Hex(string data, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(data);
            return Sha256Hex(bytes);
        }
        /// <summary>
        /// 接受一个字节数组作为输入，直接计算字节数组的 SHA-256 哈希值，并返回十六进制字符串表示。
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string Sha256Hex(byte[] bytes)
        {
            using SHA256 sHA = SHA256.Create();
            byte[] data = sHA.ComputeHash(bytes);
            return ToHexString(data);
        }
        /// <summary>
        /// 将字节数组转换为十六进制字符串表示。将字节数组转换为字符串，然后处理字符串，删除可能存在的连字符 -，并根据 toLowerCase 参数决定返回字符串的大小写
        /// </summary>
        /// <param name="data"></param>
        /// <param name="toLowerCase"></param>
        /// <returns></returns>
        public static string ToHexString(byte[] data, bool toLowerCase = true)
        {
            string text = BitConverter.ToString(data).Replace("-", string.Empty);
            if (!toLowerCase)
            {
                return text.ToUpperInvariant();
            }

            return text.ToLowerInvariant();
        }
        /// <summary>
        /// 定义了一个静态方法 ToBase64，用于将字节数组 binBuffer 转换为 Base64 编码的字符串。
        /// </summary>
        /// <param name="binBuffer"></param>
        /// <returns></returns>
        public static string ToBase64(this byte[] binBuffer)
        {
            int num = (int)Math.Ceiling((double)binBuffer.Length / 3.0) * 4;
            char[] array = new char[num];
            Convert.ToBase64CharArray(binBuffer, 0, binBuffer.Length, array, 0);
            return new string(array);
        }
        /// <summary>
        /// 对传入的字符串数组进行排序和拼接，然后计算拼接后的字符串的 SHA-1 哈希值，并将其以十六进制字符串的形式返回。
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string GetSignatureSHA1Util(string[] arr)
        {
            Array.Sort(arr, StringComparer.Ordinal);
            string s = string.Join("", arr);
            SHA1 sHA = SHA1.Create();
            byte[] array = sHA.ComputeHash(Encoding.UTF8.GetBytes(s));
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array2 = array;
            foreach (byte b in array2)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 数组SHA1
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string GetSHA1(string[] arr)

        {

            Array.Sort(arr, StringComparer.Ordinal);

            string s = string.Join("", arr);

            SHA1 sHA = SHA1.Create();

            byte[] array = sHA.ComputeHash(Encoding.UTF8.GetBytes(s));

            StringBuilder stringBuilder = new StringBuilder();

            byte[] array2 = array;

            foreach (byte b in array2)

            {

                stringBuilder.AppendFormat("{0:x2}", b);

            }



            return stringBuilder.ToString();

        }

        /// <summary>
        /// 数组SHA256加密
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string GetSHA256(string[] arr)

        {
           
           
            if (arr != null|| arr!.Length>0)
            {

                Array.Sort(arr, StringComparer.Ordinal);

                byte[] bytes = Encoding.UTF8.GetBytes(string.Join(string.Empty, arr));

                using (SHA256 sHA = SHA256.Create())

                {

                    byte[] data = sHA.ComputeHash(bytes);

                    return ToHexString(data);
                   

                }

            }

            return string.Empty;

        }

     


    }
}
