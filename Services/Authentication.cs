using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using PFE_reclamation.DAL;
using PFE_reclamation.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;

namespace PFE_reclamation.Services
    {
    public class Authentication
        {

        private DatabContext db = new DatabContext();





        public User AuthUser(string mail, string password)
            {
            User u = db.Users.Where(x => x.mail == mail).FirstOrDefault();
           
            if (u!=null && VerifyHashedPassword(u.password, password))
                {
                     return u;
                }
            else
                {
                return null;
                }




           

            }

        public  string HashPassword(string password)
            {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
                {
                throw new ArgumentNullException("password");
                }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
                {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
                }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
            }

        public  bool VerifyHashedPassword(string hashedPassword, string password)
            {
            byte[] buffer4;
            if (hashedPassword == null)
                {
                return false;
                }
            if (password == null)
                {
                throw new ArgumentNullException("password");
                }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
                {
                return false;
                }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
                {
                buffer4 = bytes.GetBytes(0x20);
                }
            return AreHashesEqual(buffer3, buffer4);

            }

        private static bool AreHashesEqual(byte[] firstHash, byte[] secondHash)
            {
            int _minHashLength = firstHash.Length <= secondHash.Length ? firstHash.Length : secondHash.Length;
            var xor = firstHash.Length ^ secondHash.Length;
            for (int i = 0; i < _minHashLength; i++)
                xor |= firstHash[i] ^ secondHash[i];
            return 0 == xor;
            }






        public  string Decrypt(string cipherText) {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_Pwd, _Salt);
            byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return System.Text.Encoding.Unicode.GetString(decryptedData);
            }

        private static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV) {
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = null;
            try {
                Rijndael alg = Rijndael.Create();
                alg.Key = Key;
                alg.IV = IV;
                cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(cipherData, 0, cipherData.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
                } catch {
                return null;
                } finally {
                cs.Close();
                }
            }

       
        
        
        public  string Encrypt(string clearText) {
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_Pwd, _Salt);
            byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return Convert.ToBase64String(encryptedData);
            }


        private static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV) {
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = null;
            try {
                Rijndael alg = Rijndael.Create();
                alg.Key = Key;
                alg.IV = IV;
                cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(clearData, 0, clearData.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
                } catch {
                return null;
                } finally {
                cs.Close();
                }
            }
     
        
        static string _Pwd = "CRMReclam"; //Be careful storing this in code unless it’s secured and not distributed
        static byte[] _Salt = new byte[] { 0x45, 0xF1, 0x61, 0x6e, 0x20, 0x00, 0x65, 0x64, 0x76, 0x65, 0x64, 0x03, 0x76 };


        }
    }