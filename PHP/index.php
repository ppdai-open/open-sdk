<?php
include 'openapi_client.php';

/*step 1 通过code获取授权信息*/
// $code = "52c974893a46478bbf8a0d993a169c8c";
// $authorizeResult = authorize($code);
// var_dump($authorizeResult) ;
$accessToken = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

/*保存用户授权信息后可获取做权限内的接口调用*/
// $url = "http://gw.open.ppdai.com/open/openApiPublicQueryService/QueryUserNameByOpenID";
// $url = "http://gw.open.ppdai.com/auth/authservice/sendsmsauthcode";
// $url = "http://gw.open.ppdai.com/auth/LoginService/AutoLogin";
// $url = "http://gw.open.ppdai.com/invest/LLoanInfoService/LoanList";
// $url = "http://gw.open.ppdai.com/invest/LLoanInfoService/BatchListingInfos";
$url = "http://gw.open.ppdai.com/balance/balanceService/QueryBalance";

// $request = '{"OpenID": "be47e5e4b9444047b0f8fe9311a8ea29"}';
// $request = '{"Mobile": "15026671512","DeviceFP": "123456"}';
$request = '{"Timestamp": "2017-03-14 19:15:22"}';
// $request = '{"PageIndex": 1,"StartDateTime": "2015-11-11 12:00:00.000"}';
// $request = '{"ListingIds": [100001,123456]}';
$request = '{}';

$result = send($url, $request,$accessToken);

/*加解密*/
// $data = "test";
// $encrypted = encrypt($data);
// $decrypted = decrypt($encrypted);
// echo "Encrypted: ".$encrypted."<br>";
// echo "Decrypted: ".$decrypted;
