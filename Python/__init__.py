#coding=utf-8
from openapi_client import openapi_client as client
from core.rsa_client import rsa_client as rsa
import pickle
import json
import time
import datetime
import os

appid="a769b53eb26849eba5d5e81ccb381a32"

code = "5ae2ee0d135b47ac806fb822fe5477bd"

#step 1 授权
#authorizeStr = client.authorize(appid=appid,code=code) #获得授权
#authorizeObj = pickle.loads(authorizeStr) # 将返回的authorize对象反序列化成对象，成功得到 OpenID、AccessToken、RefreshToken、ExpiresIn
#{"OpenID":"xx","AccessToken":"xxx","RefreshToken":"xxx","ExpiresIn":604800}
#{"OpenID":"cecef9dee01d430484342f5d45cf4b1b","AccessToken":"bdfbfc75-ca6e-4ccd-8304-b37e79ff795f","RefreshToken":"aaeca9c2-5d27-4dea-ac03-1a1fa84b136f","ExpiresIn":604800}

#step 1 刷新令牌
#openid="cecef9dee01d430484342f5d45cf4b1b"
#refreshtoken= "aaeca9c2-5d27-4dea-ac03-1a1fa84b136f"
#new_token_info = client.refresh_token(appid, openid, refreshtoken)

#step 2 发送数据（可投标列表接口）
access_url = "http://gw.open.ppdai.com/invest/LLoanInfoService/BatchListingInfos"
#"http://gw.open.ppdai.com/auth/registerservice/accountexist" 
#"http://gw.open.ppdai.com/invest/LLoanInfoService/BatchListingInfos"
#"http://gw.open.ppdai.com/invest/BidproductlistService/LoanList"
access_token = "bdfbfc75-ca6e-4ccd-8304-b37e79ff795f"
#access_token=""
utctime = datetime.datetime.utcnow()
#data = {"timestamp":utctime.strftime('%Y-%m-%d %H:%M:%S')}#time.strftime('%Y-%m-%d %H:%M:%S',)
data = {"Listingids": [27450479,27450480]}
#data = { "AccountName": "15200000001"}
sort_data = rsa.sort(data)
sign = rsa.sign(sort_data)
#data = {"listingids": [27450479,27450480]}
list_result = client.send(access_url,json.dumps(data) , appid, sign, access_token)
