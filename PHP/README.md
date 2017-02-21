## PHP-SDK使用介绍

### 调用方法(参考根目录中index.php的demo)
```
/*step 1 通过code获取授权信息*/
$authorizeResult = authorize("dbff240axxxx4a0e9501e0954a7cda4d");
echo $authorizeResult;

/*保存用户授权信息后可获取做权限内的接口调用*/
$url = "http://gw.open.ppdai.com/invest/BidproductlistService/LoanDebtList";
$accessToken="ed62c66f-xxxx-47fe-9f75-0de9a6fbfd8f";
$request = '{
          "PageIndex": 1,
          "PageSize": 1500
        }';
$result = send($url, $request,$accessToken);
```