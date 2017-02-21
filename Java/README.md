## PHP-JAVA使用介绍

### 调用方法(参考test文件夹下BizTest.java的Demo)
```
	/**
	* step 1 跳转到AC的oauth2.0联合登录
	* https://ac.ppdai.com/oauth2/login?AppID=8cf65377538741c2ba8xxxxxxxa22299&ReturnUrl=http://mysite.com/auth/gettoken
	*
	*/

	/**
	* step 2 登录成功后 oauth2.0 跳转到http://mysite.com/auth/gettoken?code=XXXXXXXXXXXXXXXXXXXXXXXXXXX
	* 添加WebApi接口gettoken
	*/

	/**
     * 根据授权码获取授权信息
     * @param code
     * @throws IOException
     */
     void gettoken(String code) throws Exception {
         OpenApiClient.Init(appid, RsaCryptoHelper.PKCSType.PKCS8,pubKey,privKey);
        authInfo =  OpenApiClient.authorize(code);
    }

	/**
     * 刷新令牌
     * @throws IOException
     */
     void refreshToken() throws Exception {
         OpenApiClient.Init(appid, RsaCryptoHelper.PKCSType.PKCS8,pubKey,privKey);
         authInfo = OpenApiClient.refreshToken(authInfo.getOpenID(),authInfo.getRefreshToken());
    }

	//=====================================================================
	// 上面的步骤为获取用户授权信息，下面的步骤是根据用户授权信息来调用API接口
	//=====================================================================

	String token = gettoken("code");
 	System.out.println("测试 AutoLogin.QueryUserInfo");
    result = OpenApiClient.send(gwurl + "/auth/LoginService/QueryUserInfo", token);
```