using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenApi.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.Utility.Tests
{
    [TestClass()]
    public class RsaCryptoHelperTests
    {
        [TestMethod()]
        public void RsaCryptoHelperTest()
        {

        }

        [TestMethod]
        public void PKCS1Test() {
            string pubKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC8iMpEG3mnFlMfufO95DfAfor80RL3I/IzF828aoDDw/Xy86jPiihJyGyG2ZmbqsAw+8nj8eGc+U9LmKASgQhS9e0R/MmYDa9R/O2f4tQZUQr3nE3uUTES0tqCLoE3TVSd59lnVExeDL5IW+F/Yc9mz1v+xSDFcSKyfHEo0FDnnwIDAQAB";
            string privKey = "MIICWwIBAAKBgQC8iMpEG3mnFlMfufO95DfAfor80RL3I/IzF828aoDDw/Xy86jPiihJyGyG2ZmbqsAw+8nj8eGc+U9LmKASgQhS9e0R/MmYDa9R/O2f4tQZUQr3nE3uUTES0tqCLoE3TVSd59lnVExeDL5IW+F/Yc9mz1v+xSDFcSKyfHEo0FDnnwIDAQABAoGAJ5wxqrd/CpzFIBBIZmfxUq8DcnRWoLfbpeJlZiWWIgskvEN2/wuOxVmne3lyLWNld6Ue2JY0CW/TuhU55ElZvv91NiTreBqr5WfZ8EYI+/lwEUKC4GzogVwrmpL1PpSaNJymvTujiShmP/+hia2mav9fhMOYm8MaMRwPELwASiECQQD0nW8xWF9IRT90v89y+P/htW+g3E4HZVAYPXyhfAnFJsGC06XAXwO0hDS8Sao7Nktj2sNSacNFjZvndGrQPOePAkEAxU8o7+QHqm/HYsO0XN49xn6zWQRvAOonhl5/+NKm7NfGEVTGwhP5KbNsJPv3TTtCPrS2V6MlIScg1yLXkFF28QJAGoEYdDNMF6uRJZhG5QE/0Hf1QWu9dKWwmP/IikLDWD5Lx14hXoetAhk1EZW1wTav0oD4muxkwRuH4ftGO4vt1wJAKkjdsBOBZRBRfaQNWj2ypYBvtSsTEvIbiFtmN5AFgAp6AyrU8bDQHBS8n2x0QlPpzYBy93MaOPGmwxRPeDlNMQJAKubPrAE9Qe++95xvvfpZgj6wOZoKGa4Yj3dd1PYcO2fU9eVSW1W6IrvJc36NIGz4Egyw2EiqFBBIJL92ZhjQ2Q==";
            string txt = "abc";

            RsaCryptoHelper rsaCryptoHelper = new RsaCryptoHelper(PKCSType.PKCS1, pubKey, privKey);

            string txt2 = rsaCryptoHelper.EncryptByPublicKey(txt);
            string txt3 = rsaCryptoHelper.DecryptByPrivateKey(txt2);

            string sign = rsaCryptoHelper.SignByPrivateKey(txt);
            bool isSign = rsaCryptoHelper.VerifyByPublicKey(txt, sign);
        }

        [TestMethod]
        public void PEMTest() {
            string pubKey = @"-----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDk/B01fncBoYj83jQnR3kAAftP
7U7hVX62K+pKZY8sx6nGyskjoxJXT/kFaCmB/gQKi0n9/F5NFNbuY73gMGTlOU4B
jAuhT0n/WiiSi8UA1FqQ5b6Sz7Mr2qhlg83qTyedUzyk+y+SyiRuewWTQ2FHw9v9
A/XF+mstJbLgqRL+ywIDAQAB
-----END PUBLIC KEY-----";
            string privKey = @"-----BEGIN PRIVATE KEY-----
MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBAOT8HTV+dwGhiPze
NCdHeQAB+0/tTuFVfrYr6kpljyzHqcbKySOjEldP+QVoKYH+BAqLSf38Xk0U1u5j
veAwZOU5TgGMC6FPSf9aKJKLxQDUWpDlvpLPsyvaqGWDzepPJ51TPKT7L5LKJG57
BZNDYUfD2/0D9cX6ay0lsuCpEv7LAgMBAAECgYA9AiLyHrisWZJ69OTmVjeZ1e1U
VUC/7pxtAvRQUBC+eI/2ZA8FDKyVULxjQWZVuQzwlj3nirbBSL0fFLoBIkOvAcfi
nd7DcdO51lKC6k4V64nl9YvYXFgEEOtUfrnq9VzehJ6QQTvWQRfURFuXppx5+x9X
c+oFCil3Mr+PjTHGiQJBAP/no5063IqFKlgK1c14pDB4vtRZeqN1x0zY9PpMtTP3
fif66sCyPgWansHIvyMXE8UsAIqZllL9M/PlHnrLzecCQQDlEemONP0AITetgEHi
gvUBXWhW1Vtgj25c/0mvGtFkFhIKPQmmwvNBvn+n4Xlp/XUcgPHMA2X4pT20Eotw
dEN9AkBo35tT0k2TjyNdVYNtY2WWX8WE7O6vkpMM0VUERu9zzpeq9s/CDMoSLd2l
+Qkr7kcx5OiL5ImQlSf3agxlsqQ9AkEAy2d2bnIa3gyg9g1Xc505lXat+b0GoN17
8FQ3x6cWm7sFVdYRReUCQDS6Agay2yzW2vKcwr2ZxIpmGgoFi1uRuQJBAMWgRVMa
aaE2JBriRkxkOUiEYieE7U3bDNZP4BpFmUD6iqkQUeNl9YvIUxE/2tKsMMsR4crI
G/7xG0sUQFNKD9o=
-----END PRIVATE KEY-----";

            string txt = "abc";

            RsaCryptoHelper rsaCryptoHelper = new RsaCryptoHelper(PKCSType.PKCS8, pubKey, privKey);

            string txt2 = rsaCryptoHelper.EncryptByPublicKey(txt);
            string txt3 = rsaCryptoHelper.DecryptByPrivateKey(txt2);

            string sign = rsaCryptoHelper.SignByPrivateKey(txt);
            bool isSign = rsaCryptoHelper.VerifyByPublicKey(txt, sign);
        }
    }
}