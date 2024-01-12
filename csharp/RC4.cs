using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicantTest
{
    /// <summary>
    /// Please modify this class as much as you want.
    /// Your goal is to be faster while keeping a clean and understandable code.
    /// </summary>
    public class RC4
    {
        public static string Decrypt(string cipherText, string key)
        {
            if (key[0] == '0' && key[1] == 'x')
            {
                string tmpK = "";
                for (int i = 2; i < key.Length; i += 2)
                {
                    tmpK += char.ConvertFromUtf32(Convert.ToInt32(key[i].ToString() + key[i + 1].ToString(), 16));
                }
                key = tmpK;
            }
            int[] S = new int[256];
            int[] T = new int[256];
            for (int i = 0; i < 256; i++)
            {
                S[i] = i;
                T[i] = key[i % key.Length];
            }

            int j = 0;
            for (int i = 0; i < 256; i++)
            {
                j = (j + S[i] + T[i]) % 256;
                int tmp = S[i];
                S[i] = S[j];
                S[j] = tmp;
            }

            int a = 0, l = 0, k = 0;
            int plLength = cipherText.Length;
            int t;

            string C = "";
            for (int i = 0; i < cipherText.Length; i++)
            {
                a = (a + 1) % 256;
                l = (l + S[a]) % 256;
                int tmp;
                tmp = S[a];
                S[a] = S[l];
                S[l] = tmp;
                t = (S[a] + S[l]) % 256;
                k = S[t];
                //Console.WriteLine(plainText[i].ToString() + k.ToString());
                C += char.ConvertFromUtf32((cipherText[i] ^ k));
            }

            return C;
        }

        public static string Encrypt(string plainText, string key)
        {
            if (key[0] == '0' && key[1] == 'x')
            {
                string tmpK = "";
                for (int i = 2; i < key.Length; i += 2)
                {
                    tmpK += char.ConvertFromUtf32(Convert.ToInt32(key[i].ToString() + key[i + 1].ToString(), 16));
                }
                key = tmpK;
            }
            int[] S = new int[256];
            int[] T = new int[256];
            for (int i = 0; i < 256; i++)
            {
                S[i] = i;
                T[i] = key[i % key.Length];
            }

            int j = 0;
            for (int i = 0; i < 256; i++)
            {
                j = (j + S[i] + T[i]) % 256;
                int tmp = S[i];
                S[i] = S[j];
                S[j] = tmp;
            }

            int a = 0, l = 0, k = 0;
            int plLength = plainText.Length;
            int t;

            string C = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                a = (a + 1) % 256;
                l = (l + S[a]) % 256;
                int tmp;
                tmp = S[a];
                S[a] = S[l];
                S[l] = tmp;
                t = (S[a] + S[l]) % 256;
                k = S[t];
                //Console.WriteLine(plainText[i].ToString() + k.ToString());
                C += char.ConvertFromUtf32((plainText[i] ^ k));
            }

            return C;
        }
    }
}
