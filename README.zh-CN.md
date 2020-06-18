# Img2ColorfulChars
将图片或视频转换为彩色字符图后在 C# 控制台中输出。

[English ReadMe](README.md)

## 一 / 原理
1. 彩色输出：在 Windows 10 的周年更新后，控制台支持传入 VT100 及类似的控制字符序列，以能控制鼠标移动、颜色字体及其他操作。（参见 [Console Virtual Terminal Sequences - Windows Console | Microsoft Docs](
https://docs.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences)）
2. 视频处理：利用 FFmpeg 将视频转换为图片序列，按顺序输出图片即可。

## 二 / 要求
- 系统要求：Windows 10
- 格式要求：（其他格式可能也可，但未经测试，可自行修改代码后测试）  
图片： .jpg, .jpeg, .png, .bmp, .ico, .tiff, **.gif**  
视频： .3gp, .asf, .avi, .flv, .m4v, .mov, .mp4, .mpg, .mpeg, .mkv, .rm, .rmvb, .wmv

## 三 / 用法
1. 通过以下途径获取 `Img2ColorfulChars.exe` 。
  - 下载本项目，使用 Visual Studio 编译生成。
  - 或在本项目 [_Output](_Output) 中下载。
  - 或在 [Releases](https://github.com/Roy0309/Img2ColorfulChars/releases) 中下载。
    
2. 运行 `Img2ColorfulChars.exe` 。
  - 双击运行 `Img2ColorfulChars.exe` 。
  - 在第一个窗口中选择要处理的图片或视频。
  - 在第二个窗口中选择缩小比例。（给出的是适应控制台窗口的最佳比例，可以另行修改，比例越大，图像越小。）
  - 观看处理后的图像或视频！

## 四 / 输出

### 1. 静态图片
<img width="250" src="Images/original.png"/><img width="250" src="Images/output.png"/>

### 2. 动态图片
<img width="250" src="Images/original.gif"/><img width="250" src="Images/output.gif"/>

### 3. 视频
如遇到帧率过低，请适当增加缩小比例。  
<img width="250" src="Images/originalvideo.gif"/><img width="250" src="Images/outputvideo.gif"/>

## 五 / 思路总结
- [来自多彩世界的控制台——C#控制台输出彩色字符 - 简书](https://www.jianshu.com/p/8a083421c11d)  
  适用于1.1版。