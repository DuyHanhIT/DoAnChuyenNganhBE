using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebApiShoesStoreDACN.Models
{
    public class UserMasterRepository : IDisposable
    {
        DBShoesStore db = new DBShoesStore();
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        public account ValidateUser(string username, string password)
        {
            var f_password = password;//GetMD5(password);
            return db.accounts.FirstOrDefault(x => x.username.Equals(username) && x.password.Equals(f_password));

        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}