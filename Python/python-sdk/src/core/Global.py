#coding=utf-8
__author__ = "yangl"
'''''
全局变量
'''
'''''
publickey为服务端的公钥
privatekey为自己客户端的私钥
PS：python的密钥都是PKCS1的
站点上的客户端私钥需要剔除
-----BEGIN RSA PRIVATE KEY-----
AND
-----END RSA PRIVATE KEY-----
并且将回车符剔除

备注：Python只支持pkcs1密钥，不支持pkcs8
'''

privatekey='''
-----BEGIN RSA PRIVATE KEY-----
-----END RSA PRIVATE KEY-----
'''

publickey='''
-----BEGIN PUBLIC KEY-----
-----END PUBLIC KEY-----
'''
