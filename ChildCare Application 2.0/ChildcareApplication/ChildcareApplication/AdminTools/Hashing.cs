using System.Security.Cryptography;
using System.Text;

namespace ChildcareApplication.AdminTools {
    class Hashing {

        public static string HashPass(string pass) {

            string sSourceData;
            byte[] tmpSource;
            byte[] tmpHash;

            string hashed;
            //Create a byte array from source data
            sSourceData = pass;
            tmpSource = ASCIIEncoding.ASCII.GetBytes(sSourceData);

            //Compute hash based on source data
            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            hashed = ByteArrayToString(tmpHash);

            return hashed;
        }

        static string ByteArrayToString(byte[] arrInput) {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length - 1; i++) {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}
