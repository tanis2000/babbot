/*
    This file is part of BabBot.

    BabBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    BabBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BabBot.  If not, see <http://www.gnu.org/licenses/>.
  
    Copyright 2009 BabBot Team
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BabBot.Common
{
    /// <summary>
    /// Encryption\Decryption utilities
    /// </summary>
    public static class Security
    {
        // key is static ... that's not for PayPal password encryption anyway :)
        private static string key = "babbot_4ever";

        private static System.Security.Cryptography.TripleDESCryptoServiceProvider des =
                new System.Security.Cryptography.TripleDESCryptoServiceProvider();

        static Security()
        {
            des.Key = CalcMD5(key);
            des.Mode = System.Security.Cryptography.CipherMode.ECB;
        }

        /// <summary>
        /// Calculate MD5 Hash given a string
        /// </summary>
        private static byte[] CalcMD5(string s)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x =
                new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(s);
            return x.ComputeHash(data);
        }

        /// <summary>
        /// Encrypt given string
        /// </summary>
        /// <param name="s">Given String</param>
        public static string Encrypt(string s)
        {
            if (s.Equals("")) return s;

            byte[] buff = ASCIIEncoding.ASCII.GetBytes(s);
            return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length));
        }

        /// <summary>
        /// Decrypt given string
        /// </summary>
        /// <param name="x">Given String</param>
        public static string Decrypt(string x)
        {
            if (x.Equals("")) return x;

            byte[] buff = Convert.FromBase64String(x);
            return ASCIIEncoding.ASCII.GetString(
                des.CreateDecryptor().TransformFinalBlock(buff, 0, buff.Length));
        }
    }
}
