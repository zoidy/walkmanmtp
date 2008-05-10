WalkmanMTP
==========

Allows for the creation of playlists and album art on the S610/A810 series
without using Windows Media Player.

Requirements
============
-Sony Walkman S610/A810 series (may work on other MTP based players as well)
-.Net framework 2.0 or above
-MTPAxe (included in package)


Usage
======
1. Plug the walkman in and make sure it is detected by windows
2. run WalkmanMTP
3. Program will then read filesystem
4. Use program
5. Close program
6. Unplug walkman

Note: It's not a good idea to add/delete stuff with explorer/WMP while the program is running.  If you
do, make sure to refresh the program before continuing.

Making playlists:
-in the playlists tab, drag files from the player to a new or existing playlist.
-files will be played in the order they appear on the playlist.  
-Drag and drop items to rearrange them
-click the Sync button to upload the playlists to the player.

Making albums:
-not yet supported

File management:
-Go to the file management tab and select the folder you wish to copy files to.
-then drag and drop files and folders to the right pane.  they will be uploaded
 immediately.  Note that it is possible to upload any kind of file, as long as
 there is enough space on the device.
-To delete a file or folder, select it in the right hand pane. Note deleting a folder
 will delete everything inside it too.


Troubleshooting
================
If the program crashes, there will be a log saved to %temp%\WMTP.log
This log file gets overwritten each time the program is run so make sure to
save it first before restarting the program in the event of a crash.

If the program doesn't start, try running MTPAxe on it's own. If you get an error
saying the program is not installed properly, you probably need to install the Visual C++
Runtime 2008.

If the program runs but crashes, run MTPAxe on its own. In the console window type -100
and hit enter.  Then follow the prompts.  If this command completes, it will save a file
MTPAxe_dump.txt and MTPAXE_dump_items.txt in the same directory as MTPAxe.  Please send me this file so I can fix the bug.

If the program crashes for some reason while making playlists, you may get some empty playlists that can't
be deleted. To delete these playlists, run MTPAxe and type -101 and follow the prompts.  The ID that
it asks for can be obtained from the MTpAxe_dump_items.txt file.  Make sure to use the ID (the string with
the { } including the brackets) of the item itself and not the parent folder.  If you use the ID of the parent
folder by mistake, it will delete the whole folder with no warnings.

Running this program from a network share may cause unpredictable results. Copy it to the local
drive instead

To Do
=====
-add the ability to add albums and album art
-better international filename support
