# coding=utf-8
from __builtin__ import str

__author__ = "yangl"


import base64 
import rsa
import Global
 
# rsa操作类
class rsa_client:
    
    '''''
    RSA签名
    '''
    @staticmethod
    def sign(signdata):
        '''''
        @param signdata: 需要签名的字符串
        '''
        signdata = signdata.lower()
        PrivateKey = rsa.PrivateKey.load_pkcs1(Global.privatekey)
        signature = base64.b64encode(rsa.sign(signdata, PrivateKey, 'SHA-1'))
        return signature
#       

    @staticmethod
    def sort(dicts):
        '''''
        作用类似与java的treemap,
        取出key值,按照字母排序后将keyvalue拼接起来
        返回字符串
        '''
        dics = sorted(dicts.items(), key=lambda k : k[0])
        params = ""
        for dic in dics:
            if type(dic[1]) is str:
                params += dic[0] + dic[1]
        return params
