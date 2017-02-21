<?php
include 'core/global.php';
include 'core/http.php';
/**
 * 获取授权 AppId 应用ID code 授权码
 * 
 * @param unknown $appid        	
 * @param unknown $code        	
 */
function authorize($code) {
	global $appid;
	$request = '{"AppID":"' . $appid . '","code":"' . $code . '"}';
	echo $request;
	$url = "https://ac.ppdai.com/oauth2/authorize";
	return SendAuthRequest ( $url, $request );
}

/**
 * 刷新AccessToken
 * AppId 应用ID
 * OpenId 用户唯一标识
 * RefreshToken 刷新令牌Token
 * 
 * @param unknown $openid        	
 * @param unknown $refreshtoken        	
 */
function refresh_token($openid, $refreshtoken) {
	global $appid;
	$request = '{"AppID":"' . $appid . '","OpenID":"' . $openId . '","RefreshToken":"' . $refreshToken . '"}';
	$url = "https://ac.ppdai.com/oauth2/refreshtoken";
	return SendAuthRequest ( $url, $request );
}

/**
 * 向拍拍贷网关发送请求
 * Url 请求地址
 * Data 请求报文
 * AppId 应用编号
 * Sign 签名信息
 * AccessToken 访问令牌
 * 
 * @param unknown $url        	
 * @param unknown $data        	
 * @param string $accesstoken        	
 */
function send($url, $data, $accesstoken = '') {
	global $appid;
	global $appPrivateKey;
	return SendRequest ( $url, $data, $appid, $appPrivateKey, $accesstoken );
}