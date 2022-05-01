# 🏆 原神成就识别

原神成就识别，主要用于快速查找未完成的隐藏成就，所以只支持“天地万象”的成就导出。

* 支持任意分辨率下的窗口化原神。
* 仅支持中文识别，准确率约 80%，分辨率越高识别越准确。

![](https://raw.githubusercontent.com/babalae/genshin-achievement-toy/master/Docs/demo.gif)
                                              
## 下载地址

[📥Github下载](https://github.com/babalae/genshin-achievement-toy/releases/download/v1.2/GenshinAchievement_v1.2.zip)

## 使用方法

你的系统需要满足以下条件：
  * **Windows 10 或更高版本**
  * [.NET Framework 4.5.2](https://www.microsoft.com/en-us/download/details.aspx?id=42642) 或更高版本（正常情况下 Win10 自带此框架）。


1. 首先打开原神，并设置成窗口化，打开“天地万象”成就页面。

2. 点击“一键识别成就”，在弹出框中确认识别选区没有问题后，然后点击“确定”，程序会自动滚动成就页面进行截图识别。**此时不要移动鼠标，等成就识别完成即可，如果出现异常情况可以按<kbd>F11</kbd>停止识别**

3. 识别完成后，可以选择以下网站进行数据导入并查看，具体导入方式可以看注释。
    * [cocogoat.work](https://cocogoat.work/)
    * [seelie.me](https://seelie.me/)
    * [paimon.moe](https://paimon.moe/)

## FAQ
* 为什么需要管理员权限？
  * 因为游戏以管理员权限启动，软件不以管理员权限启动的话没法模拟鼠标点击与滚动。
* 如何修改停止快捷键（F11）以及其他程序参数？
  * 软件同级目录下的 `config.json` 可以修改相关参数。
* 识别率一直很低怎么办
  * 一键识别成就后，可以把软件所在目录的 UserData/*_img_section/ 文件夹中的所有图片上传至 [cocogoat.work](https://cocogoat.work/achievement) 进行识别。

## 更多
本软件大部分功能已经作为插件 [SG.Plugin.Achievement.Exporter](https://github.com/emako/SG.Plugin.Achievement.Exporter) 移植到 [Snap Genshin](https://github.com/DGP-Studio/Snap.Genshin)。

感谢 [@emako](https://github.com/emako) 的移植和贡献。

感谢 [椰羊 cocogoat](https://github.com/YuehaiTeam/cocogoat) 提供的成就图片三方导入识别入口。
