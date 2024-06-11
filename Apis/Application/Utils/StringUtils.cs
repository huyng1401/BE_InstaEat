using System.Security.Cryptography;
using System.Text;

namespace Application.Utils
{
    public static class StringUtils
    {

        //public static void WriteStringToFile(string content)
        //{
        //    // Write the string content to the file
        //    File.WriteAllText("C:\\Users\\nhat\\Desktop\\TESTAPI.txt", content);
        //}

        public static String HmacSHA256(string inputData, string key)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                string hex = BitConverter.ToString(hashmessage);
                hex = hex.Replace("-", "").ToLower();
                return hex;
            }
        }
    }
}
