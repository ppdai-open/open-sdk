#coding=utf-8
'''
Created on 2016年7月5日

@author: yanglei
'''
from core.http import http_client
from core.rsa_client import rsa_client as rsa
import datetime
import gzip, StringIO
import json

'''
OpenAapi提交请求客户端
'''
class openapi_client:
    '''
    oauth2授权地址
    '''
    AUTHORIZE_URL = "https://ac.ppdai.com/oauth2/authorize"
    
    '''
       刷新Token地址
    '''
    REFRESHTOKEN_URL = "https://ac.ppdai.com/oauth2/refreshtoken"


    def __init__(self, params):
        '''
        Constructor
        '''
    
    '''
        获取授权
    AppId 应用ID
    code 授权码
    '''
    @staticmethod
    def authorize(appid,code):
        data = "{\"AppID\":\"%s\",\"Code\":\"%s\"}" % (appid,code)
        data = data.encode("utf-8")
        result = http_client.http_post(openapi_client.AUTHORIZE_URL,data)
        #result = gzip.GzipFile(fileobj=StringIO.StringIO(result),mode="r")
        #result = result.read().decode("gbk").encode("utf-8")
        print("authorize_data:%s" % (result))
        return result
        
    '''
        刷新AccessToken
    AppId 应用ID
    OpenId 用户唯一标识
    RefreshToken 刷新令牌Token
    '''
    @staticmethod
    def refresh_token(appid,openid,refreshtoken):
        data = "{\"AppID\":\"%s\",\"OpenId\":\"%s\",\"RefreshToken\":\"%s\"}" % (appid,openid,refreshtoken)
        result = http_client.http_post(openapi_client.REFRESHTOKEN_URL,data)
        print("refresh_token_data:%s" % (result))
        return result
    
    '''
        向拍拍贷网关发送请求
    Url 请求地址
    Data 请求报文
    AppId 应用编号
    Sign 签名信息
    AccessToken 访问令牌
    '''
    @staticmethod
    def send(url,data,appid,sign,accesstoken=''):
        utctime = datetime.datetime.utcnow()
        timestamp = utctime.strftime('%Y-%m-%d %H:%M:%S')
        headers = {"X-PPD-APPID":appid,
                   "X-PPD-SIGN":sign,
                   "X-PPD-TIMESTAMP":timestamp,
                   "X-PPD-TIMESTAMP-SIGN":rsa.sign("%s%s" % (appid,timestamp)),
                   "Accept":"application/json;charset=UTF-8"}
        if accesstoken.strip():
            headers["X-PPD-ACCESSTOKEN"] = accesstoken
        result = http_client.http_post(url,data,headers=headers)
        # json.loads(result)
        print("send_data:\n%s" % (result))
        return result
        
        
        