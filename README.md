# ImageUploader

简洁易用的图床工具, 目前支持: 阿里云OSS. 其它供应商补全中...

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
  <Oss Name="Oss2" EndPoint="..." AccessKeyId="..." AccessKeySecret="..." BucketName="..." />
</ImageUploaderConfig>
```

`Name`属性为每一项配置的名称, 以便识别. 其它属性与供应商有关:

### 阿里云OSS - AliyunStorageProvider

在阿里云Oss创建的Bucket应设为公共读权限, 否则生成的外链无效

生成外链为简单拼接`https://<Bucket>.<EndPoint>/<Key>`, 上传时可以编辑`Key`, 用`/`符号来划分目录.

```xml
  <!-- 可以简写为Oss, 注意大小写 -->
  <AliyunStorageProvider
    Name="Aliyun Oss"
    EndPoint="YourEndPoint"
    AccessKeyId="YourAccessKeyId"
    AccessKeySecret="YourAccessKeySecret"
    BucketName="YourBucketName"/>
```

