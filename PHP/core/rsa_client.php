<?php
include 'core/global.php';

/**
 * 排序Request至待签名字符串
 *
 * @param $request: json格式Request
 */
function sortToSign($request){
    $obj = json_decode($request);
    $arr = array();
    foreach ($obj as $key=>$value){
        if(is_array($value)){
            continue;
        }else{
            $arr[$key] = $value;
        }
    }
    ksort($arr);
    $str = "";
    foreach ($arr as $key => $value){
        $str = $str.$key.$value;
    }
    $str = strtolower($str);
    return $str;
}


/**
 * RSA私钥签名
 * 
 * @param $signdata: 待签名字符串      	     	
 */
function sign($signdata){
    global $appPrivateKey;
    if(openssl_sign($signdata,$sign,$appPrivateKey))
        $sign = base64_encode($sign);
    return $sign;
}


/**
 * RSA公钥验签
 *
 * @param $signdata: 待签名字符串
 * @param $signeddata: 已签名字符串
 */
function verify($signdata,$signeddata){
    global $appPublicKey;
    $signeddata = base64_decode($signeddata);
    if (openssl_verify($signdata, $signeddata, $appPublicKey))
        return true;
    else 
        return false;
}


/**
 * RSA公钥加密
 *
 * @param $encryptdata: 待加密字符串
 */
function encrypt($encryptdata){
    global $appPublicKey;
    openssl_public_encrypt($encryptdata,$encrypted,$appPublicKey);
    return base64_encode($encrypted);
}


/**
 * RSA私钥解密
 *
 * @param $decryptdata: 待解密字符串
 */
function decrypt($encrypteddata){
    global $appPrivateKey;
    openssl_private_decrypt(base64_decode($encrypteddata),$decrypted,$appPrivateKey);
    return $decrypted;
}

