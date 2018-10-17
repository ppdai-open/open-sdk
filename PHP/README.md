## PHP-SDK使用介绍

### 调用方法(参考根目录中index.php的demo)
```
/*step 1 通过code获取授权信息*/
$authorizeResult = authorize("dbff240axxxx4a0e9501e0954a7cda4d");
echo $authorizeResult;

/*保存用户授权信息后可获取做权限内的接口调用*/
$url = "https://openapi.ppdai.com/invest/BidproductlistService/LoanDebtList";
$accessToken="ed62c66f-xxxx-47fe-9f75-0de9a6fbfd8f";
$request = '{
          "PageIndex": 1,
          "PageSize": 1500
        }';
$result = send($url, $request,$accessToken);
```
如果使用过程中遇到调用https有问题可以参考这篇[文章](http://unitstep.net/blog/2009/05/05/using-curl-in-php-to-access-https-ssltls-protected-sites/)
