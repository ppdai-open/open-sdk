## 拍拍贷开放平台官方SDK

本文为拍拍贷开放平台JAVA版SDK的新手使用教程，只涉及教授SDK的使用方法，默认读者已经熟悉IDE的基本使用方法（本文以eclipse为例），以及具有一定的编程知识基础等。

## 官方SDK接入指南（Java为例）

### 前期准备
* 首先需要有一个PPDAI账号（注册地址： [https://ac.ppdai.com/User/Register](https://ac.ppdai.com/User/Register)）
并已成为开发者审核通过，应用申请审核通过。

### 下载拍拍贷开放平台SDK
* JAR包主要包含3部分内容： （其中，只有ppdai-openapi-sdk-1.0.jar是必须的）
ppdai-openapi-sdk-1.0.jar（封装了如何调用OpenApi的通用方法）
请前往“[工具下载页](http://open.ppdai.com/doc/download)”下载最新SDK包

### 搭建开发环境
* 在Eclipse中建立你的工程。
* 在工程中新建一个libs目录，将开发工具包中libs目录下的ppdai-openapi-sdk-1.0.jar复制到该目录中（如下图所示，建立了一个名为testDemo右键单击工程”Properties→Java Build Path→Libraries→Add External JARs）
![dev](http://open.ppdai.com/resources/images/doc/sdkjava_01.png)

* 自己搭建Maven私服形式，复制ppdai-openapi-sdk-1.0.jar到自己搭建的Maven私服,Maven的Pom.xml 增加如下配置
![dev1](http://open.ppdai.com/resources/images/doc/sdkjava_02.png)

* 这样你就可以开始使用sdk提供的方法了~

### 在代码中使用工具包
* 跳转拍拍贷登录界面
现在，你的程序要跳转到拍拍贷登录页面，可由如下下列代码实现
跳转登录页面地址为[https://ac.ppdai.com/oath2/login?AppID={0}&ReturnUrl={1}](https://ac.ppdai.com/oath2/login?AppID={0}&ReturnUrl={1}) ，例如：
![](http://open.ppdai.com/resources/images/doc/sdkdotnet_04.png)
用户登录成功后，拍拍贷会把code返回给传入的ReturnUrl地址。
* 根据用户Code进行用户授权方法：OpenApiClient. Authorize()
根据[1]返回的code,进行身份校验，可由下列SDK实现
用户登录成功后，拍拍贷会根据returnURL返回,例：[http://mysite.com/auth/gettoken?code=02af593bf9fd49f1af6b8694595cb3f5&state=](http://mysite.com/auth/gettoken?code=02af593bf9fd49f1af6b8694595cb3f5&state=)
	
```
public AuthInfo Authorize (String appid,String code){
	AuthInfo authInfo = OpenApiClient.Authorize(appid,code);
}
```
* 返回结果：
验证通过可获得实体类AuthInfo（ErrMsg[处理结果]，OPENID,ACCESSTOKEN[登录令牌],REFRESHTOKEN[用于刷新TOKEN]，EXPIRESIN[有效期]）。
[3] ACCESSTOKEN有效期过期，根据REFRESHTOKEN刷新Token ：OpenApiClient. RefreshToken()

### 结束
* 至此，你已经能使用拍拍贷开放平台的SDK了。如果想更详细了解每个API函数的用法，请查阅 API接口参考手册， 或自行并可下载[JAVA版SDK](https://gitlab.com/ppdai-open/open-sdk/tree/master/Java)/ [.NET版SDK](https://gitlab.com/ppdai-open/open-sdk/tree/master/C%23) 自行调试。