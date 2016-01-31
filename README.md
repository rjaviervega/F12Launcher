F12Launcher 
============

F12Launcher is a Windows XP/Vista/7 application created to switch and position windows automatically.

I started this project as a hacking hobbie over a weekend to get around some of the anoying nature of
working with many windows and a big monitor.There are a few features that I would like to have to deal 
with multiple open windows/programs on my desktop.

After looking around the web I haven't found an application that will do what I wanted so I decided to 
start this one. So far this application is in very early stage, just as a proof of concept so hopefully 
someone on the web can find it interesting and contribute to it.

The purpose of this application is the following:

1. Allow to instantly switch between programs and position them automatically.
2. Split a set of 2 programs vertically and position then right next to each other.
3. Automatically launch the applications if they are not running.
4. Switch Sets of applications with one button (F12)
5. Include in the future more features to manage multiple windows and position them automatically.


Spec Details
-------------

1. This app was built using MS Visual Studio 2010 C#.
2. This application uses WindowScrape DLL  (http://www.programmersheaven.com/download/56171/download.aspx)
to determine information of programs running, windows sizes, programs names, etc.
3. This programs makes a few calls to USER32.DLL library to get details of programs running.
4. This program uses keyboard hooks to capture globaly Key events and handle them.
 

Installation
------------

To install F12Launcher download the files "Setup.exe" and "F12LauncherSetup.msi" from the folder 
"F12LauncherSetup/Release" in the project directory.

Then execute the file just downloaded "Setup.exe".

This program requires the .NET framework 4.0 installed on Windows which can be downloaded from:
http://www.microsoft.com/download/en/details.aspx?id=17851


Video Demo
----------

This video gives a quick presentation of what F12Launcher does.

[![Video1](/rjaviervega/F12Launcher/blob/master/Screenshots/video-1.jpg?raw=true)](http://youtu.be/mjYua_xFNig)

Technical Video
---------------

This video explains a bit the technical details of F12Launcher, the challenges, the features that can
be added and how this concept can be used to provide a nice user experiences for any OS.

[![Video2](/rjaviervega/F12Launcher/blob/master/Screenshots/video-2.jpg?raw=true)](http://youtu.be/7DxUZO814xI)



Screenshots
-----------

![https://raw.github.com/rjaviervega/F12Launcher/master/Screenshots/screenshot1.jpg](https://raw.github.com/rjaviervega/F12Launcher/master/Screenshots/screenshot1.jpg "Main Window")

![https://raw.github.com/rjaviervega/F12Launcher/master/Screenshots/screenshot2.jpg](https://raw.github.com/rjaviervega/F12Launcher/master/Screenshots/screenshot2.jpg "Add Instances")

License
--------

Copyright (C) 2012 R. Javier Vega http://www.rjaviervega.com

This program is created under the GPL-3.0 license.

Copyright (C) 2007 Free Software Foundation, Inc. <http://fsf.org/>

Everyone is permitted to copy and distribute verbatim copies of this license document, but changing it is not allowed.

Preamble
--------

The GNU General Public License is a free, copyleft license for software and other kinds of works.

The licenses for most software and other practical works are designed to take away your freedom to share and change the works. By contrast, the GNU General Public License is intended to guarantee your freedom to share and change all versions of a program--to make sure it remains free software for all its users. We, the Free Software Foundation, use the GNU General Public License for most of our software; it applies also to any other work released this way by its authors. You can apply it to your programs, too.

http://www.opensource.org/licenses/GPL-3.0
