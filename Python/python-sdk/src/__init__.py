#coding=utf-8
from openapi_client import openapi_client as client
from core.rsa_client import rsa_client as rsa
import pickle
import json
import time
import datetime
import os

appid="xx"

code = "xx"

#step 1 授权
#authorizeStr = client.authorize(appid=appid,code=code) #获得授权
#authorizeObj = pickle.loads(authorizeStr) # 将返回的authorize对象反序列化成对象，成功得到 OpenID、AccessToken、RefreshToken、ExpiresIn
#{"OpenID":"xx","AccessToken":"xxx","RefreshToken":"xxx","ExpiresIn":604800}

#step 1 刷新令牌
#openid="xx"
#refreshtoken= "xx-5d27-4dea-ac03-xx"
#new_token_info = client.refresh_token(appid, openid, refreshtoken)

#step 2 发送数据（可投标列表接口）
access_url = "http://gw.open.ppdai.com/invest/LLoanInfoService/BatchListingInfos"
#"http://gw.open.ppdai.com/auth/registerservice/accountexist" 
#"http://gw.open.ppdai.com/invest/LLoanInfoService/BatchListingInfos"
#"http://gw.open.ppdai.com/invest/BidproductlistService/LoanList"
access_token = "xx-ca6e-4ccd-8304-xx"
#access_token=""
utctime = datetime.datetime.utcnow()
#data = {"timestamp":utctime.strftime('%Y-%m-%d %H:%M:%S')}#time.strftime('%Y-%m-%d %H:%M:%S',)
data = {"Listingids": [27450479,27450480]}
#data = { "AccountName": "15200000001"}
sort_data = rsa.sort(data)
sign = rsa.sign(sort_data)
#data = {"listingids": [27450479,27450480]}
list_result = client.send(access_url,json.dumps(data) , appid, sign, access_token)
