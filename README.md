# BiliBiliPlayerInfo
该项目采集了BiliBili的视频播放页面的一些信息以便于调用
## 采集信息及函数
### GetInfo类
* 构造函数参数为视频的av号
* cid: getCid()
* aid: getAid()
* 播放页名: getTitle()
* 关键字: getKeywords()
* 视频描述: getDescription()
* 作者: getAuther()
* 作者ID: getAutherID()
* 封面: getCover()
* H5播放器地址: getH5Player()

### Page类
* 获取网页HTML: GetHtml(string url)

## 类库及实例
使用Winform写了一个简单的输入av号返回信息的exe（原来是想写一个Bilibili桌面客户端，准备用Chromeium和H5播放器，结果dll不支持FFMPEG，除非重编译，我太懒了，太监了）

[下载实例](https://github.com/WWILLV/BiliBiliPlayerInfo/blob/master/example/BilibiliPlayerInfo_Winform.zip)

![截图](https://github.com/WWILLV/BiliBiliPlayerInfo/blob/master/example/pic.png)

[Release](https://github.com/WWILLV/BiliBiliPlayerInfo/releases)

## License
[MIT](https://github.com/WWILLV/BiliBiliPlayerInfo/blob/master/LICENSE)