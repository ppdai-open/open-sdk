using System;
using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace OpenApi.Utility
{
    /// <summary>
    /// RSA加解密、签名及验签
    /// 同JAVA互通，以JAVA生成的密钥为基础
    /// </summary>
    public class RsaCryptoHelper
    {
        private byte[] publicKey;
        private byte[] privateKey;
        private PKCSType pkcsType;

        public RsaCryptoHelper(PKCSType pkcsType,string publicKey, string privateKey) {
            this.pkcsType = pkcsType;
            publicKey = publicKey.Trim('\n');
            privateKey = privateKey.Trim('\n');

            if (publicKey.StartsWith("----")) {
                int fidx = publicKey.IndexOf("\n");
                int lidx = publicKey.LastIndexOf("\n");
                publicKey = publicKey.Substring(fidx + 1, lidx - 1 - fidx).Replace("\n", "");
            }
            this.publicKey = Convert.FromBase64String(publicKey);

            if (privateKey.StartsWith("----")) {
                int fidx = privateKey.IndexOf("\n");
                int lidx = privateKey.LastIndexOf("\n");
                privateKey = privateKey.Substring(fidx + 1, lidx - 1 - fidx).Replace("\n", "");
            }
            this.privateKey = Convert.FromBase64String(privateKey);
        }

        #region 加密解密
        public byte[] EncryptByPublicKey(byte[] data)
        {
            using (RSACryptoServiceProvider rsa = BuildRsaServiceProviderFromPublicKey())
            {
                byte[] result = rsa.Encrypt(data, false);
                return result;
            }
        }

        public string EncryptByPublicKey(string data)
        {
            return Convert.ToBase64String(EncryptByPublicKey(System.Text.Encoding.UTF8.GetBytes(data)));
        }

        public byte[] DecryptByPrivateKey(byte[] data)
        {
            using (RSACryptoServiceProvider rsa = BuildRsaServiceProviderFromPrivateKey())
            {
                byte[] result = rsa.Decrypt(data, false);
                return result;
            }
        }

        public string DecryptByPrivateKey(string data)
        {
            return System.Text.Encoding.UTF8.GetString(
                DecryptByPrivateKey(Convert.FromBase64String(data)));
        }

        #endregion

        #region 签名&验签

        /// <summary>
        /// RSA魔码
        /// </summary>
        private static byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };

        /// <summary>
        /// 比较两数组是否相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool CompareByteArrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 获取整数长度
        /// </summary>
        /// <param name="binr"></param>
        /// <returns></returns>
        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();	// data size in next byte
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }

            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }

        /// <summary>
        /// 由公钥创建Rsa的服务提供者
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <returns>Rsa服务提供者</returns>
        private RSACryptoServiceProvider BuildRsaServiceProviderFromPublicKey()
        {
            byte[] seq = new byte[15];
            int length = publicKey.Length;

            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            using (MemoryStream mem = new MemoryStream(publicKey))
            {
                using (BinaryReader binr = new BinaryReader(mem))  //wrap Memory Stream with BinaryReader for easy reading
                {
                    byte bt = 0;
                    ushort twobytes = 0;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    seq = binr.ReadBytes(15);       //read the Sequence OID
                    if (!CompareByteArrays(seq, SeqOID))    //make sure Sequence for OID is correct
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8203)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    bt = binr.ReadByte();
                    if (bt != 0x00)     //expect null byte next
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    twobytes = binr.ReadUInt16();
                    byte lowbyte = 0x00;
                    byte highbyte = 0x00;

                    if (twobytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)
                        lowbyte = binr.ReadByte();  // read next bytes which is bytes in modulus
                    else if (twobytes == 0x8202)
                    {
                        highbyte = binr.ReadByte(); //advance 2 bytes
                        lowbyte = binr.ReadByte();
                    }
                    else
                        return null;
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
                    int modsize = BitConverter.ToInt32(modint, 0);

                    int firstbyte = binr.PeekChar();
                    if (firstbyte == 0x00)
                    {   //if first byte (highest order) of modulus is zero, don't include it
                        binr.ReadByte();    //skip this null byte
                        modsize -= 1;   //reduce modulus buffer size by 1
                    }

                    byte[] modulus = binr.ReadBytes(modsize);   //read the modulus bytes

                    if (binr.ReadByte() != 0x02)            //expect an Integer for the exponent data
                        return null;
                    int expbytes = (int)binr.ReadByte();        // should only need one byte for actual exponent data (for all useful values)
                    byte[] exponent = binr.ReadBytes(expbytes);

                    // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                    RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                    RSAParameters RSAKeyInfo = new RSAParameters();
                    RSAKeyInfo.Modulus = modulus;
                    RSAKeyInfo.Exponent = exponent;
                    RSA.ImportParameters(RSAKeyInfo);

                    return RSA;
                }
            }
        }

        /// <summary>
        /// 由私钥创建Rsa的服务提供者
        /// </summary>
        /// <param name="privateKey">私钥</param>
        /// <returns>Rsa服务提供者</returns>
        private RSACryptoServiceProvider BuildRsaServiceProviderFromPrivateKey()
        {
            return pkcsType == PKCSType.PKCS8 ? BuildRsaServiceProviderFromPKCS8() : BuildRsaServiceProviderFromPKCS1();
        }

        /// <summary>
        /// 由私钥创建Rsa的服务提供者--pkcs8
        /// </summary>
        /// <param name="privkey"></param>
        /// <returns></returns>
        private RSACryptoServiceProvider BuildRsaServiceProviderFromPKCS8()
        {
            using (MemoryStream memoryStream = new MemoryStream(privateKey))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                {
                    byte bt = 0;
                    ushort twobytes = 0;

                    twobytes = binaryReader.ReadUInt16();
                    if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
                        binaryReader.ReadByte();	//advance 1 byte
                    else if (twobytes == 0x8230)
                        binaryReader.ReadInt16();	//advance 2 bytes
                    else
                        return null;

                    bt = binaryReader.ReadByte();
                    if (bt != 0x02)
                        return null;

                    twobytes = binaryReader.ReadUInt16();

                    if (twobytes != 0x0001)
                        return null;

                    byte[] seq = binaryReader.ReadBytes(15);		//read the Sequence OID
                    if (!CompareByteArrays(seq, SeqOID))	//make sure Sequence for OID is correct
                        return null;

                    bt = binaryReader.ReadByte();
                    if (bt != 0x04)	//expect an Octet string 
                        return null;

                    bt = binaryReader.ReadByte();		//read next byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the byte count
                    if (bt == 0x81)
                        binaryReader.ReadByte();
                    else
                        if (bt == 0x82)
                        binaryReader.ReadUInt16();

                    byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

                    twobytes = binaryReader.ReadUInt16();
                    if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
                        binaryReader.ReadByte();	//advance 1 byte
                    else if (twobytes == 0x8230)
                        binaryReader.ReadInt16();	//advance 2 bytes
                    else
                        return null;

                    twobytes = binaryReader.ReadUInt16();
                    if (twobytes != 0x0102)	//version number
                        return null;
                    bt = binaryReader.ReadByte();
                    if (bt != 0x00)
                        return null;

                    //------  all private key components are Integer sequences ----
                    int elems = GetIntegerSize(binaryReader);
                    MODULUS = binaryReader.ReadBytes(elems);

                    elems = GetIntegerSize(binaryReader);
                    E = binaryReader.ReadBytes(elems);

                    elems = GetIntegerSize(binaryReader);
                    D = binaryReader.ReadBytes(elems);

                    elems = GetIntegerSize(binaryReader);
                    P = binaryReader.ReadBytes(elems);

                    elems = GetIntegerSize(binaryReader);
                    Q = binaryReader.ReadBytes(elems);

                    elems = GetIntegerSize(binaryReader);
                    DP = binaryReader.ReadBytes(elems);

                    elems = GetIntegerSize(binaryReader);
                    DQ = binaryReader.ReadBytes(elems);

                    elems = GetIntegerSize(binaryReader);
                    IQ = binaryReader.ReadBytes(elems);

                    RSAParameters para = new RSAParameters();
                    para.Modulus = MODULUS;
                    para.Exponent = E;
                    para.D = D;
                    para.P = P;
                    para.Q = Q;
                    para.DP = DP;
                    para.DQ = DQ;
                    para.InverseQ = IQ;

                    RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
                    provider.ImportParameters(para);
                    return provider;
                }
            }
        }

        /// <summary>
        /// 由私钥创建Rsa的服务提供者--pkcs1
        /// </summary>
        /// <param name="privkey"></param>
        /// <returns></returns>
        private RSACryptoServiceProvider BuildRsaServiceProviderFromPKCS1()
        {
            // --------- Set up stream to decode the asn.1 encoded RSA private key ------
            using (MemoryStream memoryStream = new MemoryStream(privateKey))
            {
                //wrap Memory Stream with BinaryReader for easy reading
                using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                {
                    byte bt = 0;
                    ushort twobytes = 0;
                    int elems = 0;
                    try
                    {
                        twobytes = binaryReader.ReadUInt16();
                        if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                            binaryReader.ReadByte();    //advance 1 byte
                        else if (twobytes == 0x8230)
                            binaryReader.ReadInt16();    //advance 2 bytes
                        else
                            return null;

                        twobytes = binaryReader.ReadUInt16();
                        if (twobytes != 0x0102) //version number
                            return null;
                        bt = binaryReader.ReadByte();
                        if (bt != 0x00)
                            return null;


                        //------ all private key components are Integer sequences ----

                        byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;
                        elems = GetIntegerSize(binaryReader);
                        MODULUS = binaryReader.ReadBytes(elems);

                        elems = GetIntegerSize(binaryReader);
                        E = binaryReader.ReadBytes(elems);

                        elems = GetIntegerSize(binaryReader);
                        D = binaryReader.ReadBytes(elems);

                        elems = GetIntegerSize(binaryReader);
                        P = binaryReader.ReadBytes(elems);

                        elems = GetIntegerSize(binaryReader);
                        Q = binaryReader.ReadBytes(elems);

                        elems = GetIntegerSize(binaryReader);
                        DP = binaryReader.ReadBytes(elems);

                        elems = GetIntegerSize(binaryReader);
                        DQ = binaryReader.ReadBytes(elems);

                        elems = GetIntegerSize(binaryReader);
                        IQ = binaryReader.ReadBytes(elems);

                        // ------- create RSACryptoServiceProvider instance and initialize with public key -----

                        RSAParameters para = new RSAParameters();
                        para.Modulus = MODULUS;
                        para.Exponent = E;
                        para.D = D;
                        para.P = P;
                        para.Q = Q;
                        para.DP = DP;
                        para.DQ = DQ;
                        para.InverseQ = IQ;

                        CspParameters CspParameters = new CspParameters();
                        CspParameters.Flags = CspProviderFlags.UseMachineKeyStore;
                        RSACryptoServiceProvider provider = new RSACryptoServiceProvider(1024, CspParameters);
                        provider.ImportParameters(para);
                        return provider;
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }
        }

        public byte[] SignByPrivateKey(byte[] data)
        {
            using (RSACryptoServiceProvider rsa = BuildRsaServiceProviderFromPrivateKey())
            {
                using (SHA1 sh = new SHA1CryptoServiceProvider())
                {
                    byte[] signData = rsa.SignData(data, sh);
                    return signData;
                }
            }
        }

        public string SignByPrivateKey(string data)
        {
            return Convert.ToBase64String(SignByPrivateKey(System.Text.Encoding.UTF8.GetBytes(data)));
        }

        /// <summary>
        /// 利用公钥验签
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="stringData">数据字符串</param>
        /// <param name="signString">签名字符串</param>
        /// <returns>是否验签成功</returns>
        public bool VerifyByPublicKey(string stringData, string signString)
        {
            return VerifyByPublicKey(System.Text.Encoding.UTF8.GetBytes(stringData), Convert.FromBase64String(signString));
        }

        /// <summary>
        /// 利用公钥验签
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="data">数据</param>
        /// <param name="sign">签名</param>
        /// <returns>是否验签成功</returns>
        public bool VerifyByPublicKey(byte[] data, byte[] sign)
        {
            try
            {
                using (RSACryptoServiceProvider rsaPub = BuildRsaServiceProviderFromPublicKey())
                {
                    using (SHA1 sh = new SHA1CryptoServiceProvider())
                    {
                        bool result = rsaPub.VerifyData(data, sh, sign);
                        return result;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }

    /// <summary>
    /// 私钥填充类型
    /// </summary>
    public enum PKCSType {
        PKCS1,
        PKCS8
    }
}
