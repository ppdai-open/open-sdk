<?php
function SendAuthRequest($url, $request) {
	$curl = curl_init ( $url );
	$header = array ();
	$header [] = 'Content-Type:application/json;charset=UTF-8';
	// echo "<br> Request: $request <br>";
	
	curl_setopt ( $curl, CURLOPT_HTTPHEADER, $header );
	curl_setopt ( $curl, CURLOPT_POST, 1 );
	curl_setopt ( $curl, CURLOPT_POSTFIELDS, $request );
	curl_setopt ( $curl, CURLOPT_RETURNTRANSFER, 1 );
	$result = curl_exec ( $curl );
	var_dump ( $result );
	curl_close ( $curl );
	
	$auth = json_decode ( $result, true );
	var_dump ( $auth );
	if ($auth == NULL || $auth == false) {
		return $result;
	}
	return $auth;
}

// 包装好的发送请求函数
function SendRequest($url, $request, $appID, $appPrivateKey, $accessToken) {
	$curl = curl_init ( $url );
	
	$timestamp = gmdate ( "Y-m-d h:m:s", time () ); // UTC format
	openssl_sign ( $appID . $timestamp, $Sign_request, $appPrivateKey );
	$Sign_request = base64_encode ( $Sign_request );
	
	openssl_sign ( $request, $Sign, $appPrivateKey );
	$Sign = base64_encode ( $Sign );
	$header = array ();
	$header [] = 'Content-Type:application/json;charset=UTF-8';
	
	$header [] = 'X-PPD-TIMESTAMP:' . $timestamp;
	$header [] = 'X-PPD-TIMESTAMP-SIGN:' . $Sign_request;
	$header [] = 'X-PPD-APPID:' . $appID;
	$header [] = 'X-PPD-SIGN:' . $Sign;
	if ($accessToken != null)
		$header [] = 'X-PPD-ACCESSTOKEN:' . $accessToken;
	curl_setopt ( $curl, CURLOPT_HTTPHEADER, $header );
	curl_setopt ( $curl, CURLOPT_POST, 1 );
	curl_setopt ( $curl, CURLOPT_POSTFIELDS, $request );
	curl_setopt ( $curl, CURLOPT_RETURNTRANSFER, 1 );
	$result = curl_exec ( $curl );
	curl_close ( $curl );
	$j = json_decode ( $result, true );
	if ($j ["Message"] == '令牌校验失败：‘用户无效或令牌已过有效期！’') {
		var_dump ( $j );
	}
	return $j;
}
