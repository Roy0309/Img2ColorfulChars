# Img2ColorfulChars
Load an image and print colorful chars in C# console.

## Supported formats
.jpg; .jpeg; .png; .bmp; .ico; .tiff; **.gif**

## Usage
1. Get `Img2ColorfulChars.exe` in the following ways.
  - Build this project in Visual Studio.
  - Find it in [_Output](_Output).
  - Find it in [Releases](https://github.com/Roy0309/Img2ColorfulChars/releases).
    
2. Run `Img2ColorfulChars.exe` in the following ways.
  - Run command `Img2ColorfulChars.exe ImagePath Scale`. Enjoy!
  ``` cmd
  cd _Output
  Img2ColorfulChars.exe np.png 4
  ```
  > Notice: Scale should be positive integer, and larger scale gets smaller image.
    
  - Double click `Img2ColorfulChars.exe`. Select an image and set scale (suggested scale as default). Enjoy!

## Output

### Static image
Original image:  
<img width="300" src="Images/original.png"/>  

Output:  
<img width="300" src="Images/output.png"/>  

### GIF
Original image:  
<img width="300" src="Images/original.gif"/>  

Output:  
<img width="300" src="Images/output.gif"/>  


## 思路总结/zh_CN
- [来自多彩世界的控制台——C#控制台输出彩色字符 - 简书](https://www.jianshu.com/p/8a083421c11d)  
  适用于1.1版。