#coding=utf-8
import urllib2

__author__ = "yangl"

from urllib2 import Request  # Python 2

#网络请求操作类
class http_client:

    '''
        用于请求的Headers
    '''
    REQUEST_HEADER = {'Connection': 'keep-alive',
                  'Cache-Control': 'max-age=0',
                  #'Accept-Encoding': 'gzip, deflate, sdch',
                  'Accept-Language': 'en-US,en;q=0.8,zh-CN;q=0.6,zh;q=0.4',
                  'Content-Type':'application/json;charset=utf-8'
                  }

    #发送post请求
    @staticmethod
    def http_post(url,data,headers={}):
        try:
            req = Request(url)
            for header in http_client.REQUEST_HEADER:
                req.add_header(header, http_client.REQUEST_HEADER[header])

            for head in headers:
                req.add_header(head, headers[head])
            opener = urllib2.build_opener(urllib2.HTTPCookieProcessor())
            response = opener.open(req,data = data,timeout=5)
#             data = StringIO.StringIO(response.read())
#             gzipper = gzip.GzipFile(fileobj=data)
#             return gzipper.read()
            return response.read()
        except Exception as ex:
            print (ex)
        return ""
