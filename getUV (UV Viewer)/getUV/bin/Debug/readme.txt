+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+
+                     UV Viewer for Re-Volt (2)                               +
+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+

   / - Intro:
       *****
 -------------------------------------------------------------
|                                                             |
| Original Program: Texture Mesh Viewer for Re-Volt (TMVfR)   |
|иииииииииииииииии                                            |
| Original Creator: ?                                         |
| иииииииииииииииии                                           |
 иииииииииииииииииииииииииииииииииииииииииииииииииииииииииииии

New program: UV Viewer for Re-Volt (2) (uv viewer)
ииииииииииии
Creator    : theKDL (Kallel A.Y)
иииииии

Date:      : 12:05 24/12/2011
иииии

  // - Getting started:
      *****************

The program has 2 different 'modes', the simple and the advanced



  //-i : simple mode
  [Start]
- Just "pick a prm file" and everything will be done automatically

  [Background]
- You can "pick a background", but to be seen "use bitmap file" must be checked
- UV viewer for Re-Volt (2) has 'prm (tool)' engine so, there is also "autopick"

  [Change Color]
- Click on 'change' (LINES or BACKground) to change the colors
- the final colors will be previewed in small box
! - You must now restart the render, click on "Reload" OR minimize then maximize the window
>> The colors will be saved to the next session!

  [Export]
- You can export .bmp, .jpg or .gif file:
select the resolution:
256: 256x256 [Native: Rv1.0,Rv1.1, WolfR4, Rv1.2*, nVolt*, FreeVolts*]
512: 512x512 [Double: WolfR4, Rv1.2, nVolt]
1024: 1024x1024 [Quad: Rv1.2,nVolt]
2048: 2048x2048 [Octave: Rv1.2,nVolt]
4096: 4096x4096 [Circa Dx9: Rv1.2, nVolt]
8192: 8192x8192 [Circa Dx10: Rv1.2,nVolt]

* Rv1.2		: alpha (2011)
* nVolt		: Re-Volt engine for .NET, including Car::Load, Car manager::reload, prm viewer
* FreeVolts	: freeVolts (Re-Volt engine for Irrlicht)


  //-ii: Advanced mode:
[All simple mode are included except: reload]
- reload is replaced with "load"

[using %rvdir%]
- You should first install Car::Load, the directory you picked there is called %rvdir%
- In execution, %rvdir% will be replaced with Car::Load's Re-Volt directory
- "Load" ensures the loading of the text inside the textbox



  /// - Difference between TMVfR and UV viewer
       ***************************************
+ TMVfR is far faster (it uses GDI) ; UV Viewer uses GDI+ (not GPU accelerated)
+ TMVfR is easier than UV Viewer; UV Viewer has 2 modes: simple and advanced
+ TMVfR is set to be compatible with triangular polies only (unlike UV viewer supporting Quads as well)
+ TMVfR is set to load 256x256 background images BMP only; 
    UV viewer can load any image type .jpg, .png and .bmp as well
+ TMVfR can only save to 256x256 BMP ; UV viewer can save to 256->8192 (png, bmp or jpg)
+ TMVfR can change line color to black or white ; Uv Viewer can change line color to any color
+ UV viewer can change background color as well


  |\/ - Copyrights
        **********
You're free to do whatever you want with this program AS LONG AS IT DOESN'T CONFLICT WITH THOSE:
- YOU CANNOT reverse engineering (hack, resource edit) the program and remove 'KDL' from the program
- YOU CAN get the source code from me and do whatever you want with the program [PM at RV House, RVL, ORP]

This program is licensed under EULA (for now...) 

   \/ - Contact and support
        *******************
Any problem, suggestion, bug fix or any kind of support to this program can be done in one of those places:
1. my blog ( http://thekdl.wordpress.com/ ) 
2. RVL     ( http://z3.invisionfree.com/Revolt_Live )
3. ORP     ( http://z3.invisionfree.com/Our_Revolt_Pub )
4. Rv House( download and install from rvhouse.zackattackgames.com )

Thanks for trying this program and have a good day

OH AND/  VI - BONUS:
Double click on the form :3
