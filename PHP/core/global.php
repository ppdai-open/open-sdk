<?php
//请替换自己的appid和appPrivateKey, 申请地址：https://open.ppdai.com/
$appid="XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

/**
 * php在生成密钥时，需要选择PEM PKCS#1格式的密钥，否则会报错
 * @var string $appPrivateKey
 * @var string $appPublicKey
 */
$appPrivateKey ="
-----BEGIN RSA PRIVATE KEY-----
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
-----END RSA PRIVATE KEY-----
";
$appPublicKey ="
-----BEGIN PUBLIC KEY-----
XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
-----END PUBLIC KEY-----
";
