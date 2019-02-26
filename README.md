# ImageUploader

简洁易用的图床工具, 目前支持: 阿里云OSS, 七牛云-对象存储. 其它供应商补全中...

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
</ImageUploaderConfig>
```

`Name`属性为每一项配置的名称, 以便识别. 其它属性与供应商有关:

### 阿里云OSS - AliyunStorageProvider

依赖包: `Aliyun.OSS.SDK`

在阿里云Oss创建的Bucket至少应有公共读权限, 否则生成的外链无效 

生成的外链为简单拼接`https://<Bucket>.<EndPoint>/<Key>`, 上传时可以编辑`Key`, 用`/`符号来划分目录.

```xml
  <!-- 可以简写为Oss, 注意大小写 -->
  <AliyunStorageProvider
    Name="Aliyun Oss"
    EndPoint="YourEndPoint"
    AccessKeyId="YourAccessKeyId"
    AccessKeySecret="YourAccessKeySecret"
    BucketName="YourBucketName"/>
```

### 七牛云-对象存储 - QiniuStorageProvider

依赖包: `Qiniu`, `Newtonsoft.Json`

> 这一个Json库占了整个工具的一半大小(660KB), 如果你不用七牛云, 可以删掉 Qiniu.dll 和 Newtonsoft.Json.dll 

```xml
  <!-- 可以简写为Qiniu, 注意大小写 -->
  <QiniuStorageProvider
    Name="Qiniu Storage"
    Domain="YourDomain"
    AccessKey="YourAccessKey"
    SecretKey="YourSecretKey"
    Bucket="YourBucket" />
```

注意, Domain为外链域名, 不带http头的那种. 生成的外链也是简单拼接`http://<Domain>/<Key>`; Bucket至少应有公共读权限. 