<?php
include 'core/global.php';
include 'core/http.php';

/**
 * 获取授权
 * 
 * @param $appid: 应用ID      	
 * @param $code        	
 */
function authorize($code) {
	global $appid;
	$request = '{"AppID": "'.$appid.'","Code": "'.$code.'"}';
	$url = "https://ac.ppdai.com/oauth2/authorize";
	return SendAuthRequest ( $url, $request );
}

/**
 * 刷新AccessToken
 * 
 * @param $openid: 用户唯一标识
 * @param $openid: 应用ID        	
 * @param $refreshtoken: 刷新令牌Token       	
 */
function refresh_token($openid, $refreshtoken) {
	global $appid;
	$request = '{"AppID":"' . $appid . '","OpenID":"' . $openid. '","RefreshToken":"' . $refreshtoken. '"}';
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
function send($url, $request, $accesstoken = '') {
	global $appid;
	global $appPrivateKey;
	return SendRequest ( $url, $request, $appid, $accesstoken );
}





