# ImageUploader

简洁易用的图床工具, 目前支持: 阿里云OSS, 七牛云-对象存储, 腾讯云COS. 其它供应商补全中...

你可以拖拽或从剪贴板粘贴图像, 也可以双击从目录中选择图片.

上传成功后会生成外链及其Markdown格式.

> 程序同样支持非图像文件上传.

![Demo.gif](https://zae-public.oss-cn-beijing.aliyuncs.com/share/ImageUploader/Demo.gif)

## 使用

[下载程序](https://github.com/Zaeworks/ImageUploader/releases)

解压后运行`ImageUploader.exe`. 如果之前没有配置, 会在目录下生成一个`config.xml`文件, 编辑并配置.

## 配置

配置文件大概是这个样子:

```xml
<?xml version="1.0" encoding="utf-8"?>
<ImageUploaderConfig>
  <Oss Name="Oss1" EndPoint="..." AccessKeyId="..." AccessKeySecret="..." BucketName="..." />
  <Qiniu Name="Qiniu1" Domain="..." AccessKey="..." SecretKey="..." Bucket="..." />
  <Cos Name="QCloud" Domain="..." SecretId="..." SecretKey="..." Bucket="..."/>
</ImageUploaderConfig>
```

属性与供应商有关, 但大体上都有以下通用属性:

||||
---|---|---|
Name|用于标识配置
SecretId|密钥ID
SecretKey|密钥Key
Bucket|存储桶
Domain|域名|不包含http/https头的域名
EndPoint|节点|相当于部分域名, 生成外链时会和Bucket拼接成Domain

- 这些属性会有别名, 例如OSS的`SecretId/SecretKey`实际为`AccessKeyId/AccessKeySecret`, 程序都能够识别; 另外供应商也有别名, 例如`AliyunStorageProvider`可以简写为`Oss`或`Aliyun`.


### 阿里云OSS - AliyunStorageProvider

依赖包: `Aliyun.OSS.SDK`

在阿里云Oss创建的Bucket至少应有公共读权限, 否则生成的外链无效 

生成的外链为简单拼接`https://<Bucket>.<EndPoint>/<Key>`, 上传时可以编辑`Key`, 用`/`符号来划分目录.

```xml
  <AliyunStorageProvider Name="" EndPoint="" AccessKeyId="" AccessKeySecret="" BucketName=""/>
  <!-- 简化后, 注意大小写 -->
  <Oss Name="" EndPoint="" SecretId="" SecretKey="" Bucket=""/>
```

### 七牛云-对象存储 - QiniuStorageProvider

依赖包: `Qiniu`, `Newtonsoft.Json`

> 这一个Json库占了整个工具的一半大小(660KB), 如果你不用七牛云, 可以删掉 Qiniu.dll 和 Newtonsoft.Json.dll 

```xml
  <QiniuStorageProvider Name="" Domain="" AccessKey="" SecretKey="" Bucket=""/>
  <!-- 简化后, 注意大小写 -->
  <Qiniu Name="" Domain="" SecretId="" SecretKey="" Bucket=""/>
```

注意, Domain为外链域名, 不带http头的那种. 生成的外链也是简单拼接`http://<Domain>/<Key>`; Bucket至少应有公共读权限. 

### 腾讯云COS - QCloudStorageProvider

依赖包: `Tencent.QCloud.Cos.Sdk`, `Newtonsoft.Json`

> 这一个Json库占了整个工具的一半大小(660KB), 如果你不用腾讯云, 可以删掉 Tencent.QCloud.Cos.Sdk.dll 和 Newtonsoft.Json.dll 

```xml
  <QCloudStorageProvider Name="" Domain="" SecretId="" SecretKey="" Bucket=""/>
  <!-- 简化后, 注意大小写 -->
  <Cos Name="" Domain="" SecretId="" SecretKey="" Bucket=""/>
  <Cos Name="" Domain="" SecretId="" SecretKey="" Bucket="" AppId="" Region=""/>
```

腾讯云实际上还需要提供 `AppId` 和 `Region` 属性:
  - 腾讯云的 `Bucket` 格式如 `xxx-1234567890`, 可以识别出 `AppId="1234567890"`, 如果你的Bucket不是这样的话就需要手动指定 `AppId`
  - 使用腾讯云提供的默认域名, 如 `xxx-1234567890.cos.ap-beijing.myqcloud.com`, 可以识别出 `Region="ap-beijing"`
  - 如果用了自定义域名则需要手动指定 `Region`

生成的外链为简单拼接`https://<Domain>/<Key>`; Bucket至少应有公共读权限. 