This file is the project description from Google Code

#WalkmanMTP

Open source utility that allows for the creation/modification of playlists, the addition of album art to files without embedding cover images into each file (thus saving space and eliminating redundancy). It also allows for the uploading of video/image files. All of this is accomplished without having to install Windows Media Player 11 (WMP10 is the minimum requrement).

This is meant to be a simple way to make playlists out of files already on the player, and add files to the player without having to install WMP11 and mess around with creating a library. It will never be a replacement for Windows Media Player, or any other kind of media manager or ID3 tag manager/editor.

Dr. Zoidberg test118 @ Hotmail Dot Com

#Requirements

- Windows XP (Vista may cause problems. See below)
- Windows Media Player 10
- .Net Framework 2.0 or above
- MTPAxe (included in the package)
- ffmpeg.exe for video thumbnails(not included. download from Gianluigi Tiesi http://oss.netfarm.it/mplayer-win32.php)
- Possible requirement for MTPAxe: Visual C++ Runtime 9.0
- Tested on NWZ-S615F, should work on NWZ-S616F, NWZ-S618F, NWZ-A815, NWZ-A816, NWZ-A818. Possibly works on other MTP based players

*Windows Vista note:* Sometimes function calls will fail for no reason, other times garbage data is returned (even when using windows explorer drag and drop) . As a result, windows vista is a hit and miss. If there are problems, close the program, then unplug and reconnect the device.

#Compiled Using

- Visual Basic 2008 for WalkmanMTP front end
- Visual C++ 2008, Windows Media Device Manager 10 SDK for MTPAxe

*MTPAxe* is a custom communications interface written in C++ to communicate with MTP devices. If you want to get in the code, http://connect.creativelabs.com/opensource/Wiki/MTP%20Programming.aspx is a good MTP programming resource that helped me a lot.
