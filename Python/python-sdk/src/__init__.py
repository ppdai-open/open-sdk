#coding=utf-8
from openapi_client import openapi_client as client
from core.rsa_client import rsa_client as rsa
import pickle
import json
import time
import datetime
import os
import base64

appid="XXXXXXXXXXXXXXXXXXXXXXX"

code = "xx"

#step 1 授权
#authorizeStr = client.authorize(appid=appid,code=code) #获得授权
#authorizeObj = json.loads(authorizeStr) # 将返回的authorize对象反序列化成对象，成功得到 OpenID、AccessToken、RefreshToken、ExpiresIn
#{"OpenID":"xx","AccessToken":"xxx","RefreshToken":"xxx","ExpiresIn":604800}

access_token = "XXXXXXXXXXXXXXXXXXXXXXX"

#step 1 刷新令牌
#openid="xx"
#refreshtoken= "xx-5d27-4dea-ac03-xx"
#new_token_info = client.refresh_token(appid, openid, refreshtoken)

#step 2 发送数据（可投标列表接口）
# access_url = "http://gw.open.ppdai.com/auth/LoginService/AutoLogin"
# access_url = "http://gw.open.ppdai.com/auth/registerservice/accountexist"
# access_url = "http://gw.open.ppdai.com/invest/LLoanInfoService/BatchListingInfos"
access_url = "http://gw.open.ppdai.com/balance/balanceService/QueryBalance"
# utctime = datetime.datetime.utcnow()
# data = {"Timestamp":utctime.strftime('%Y-%m-%d %H:%M:%S')}#time.strftime('%Y-%m-%d %H:%M:%S',)
# data = { "AccountName": "15200000001"}
# data = {"ListingIds": [100001,123456]}
data = {}
sort_data = rsa.sort(data)
sign = rsa.sign(sort_data)
list_result = client.send(access_url,json.dumps(data) , appid, sign, access_token)

#加密/解密
# encrypt_data = "ta5346sw34rfe"
# encrypted = rsa.encrypt(encrypt_data)
# decrypt = rsa.decrypt(encrypted)
# print decrypt
