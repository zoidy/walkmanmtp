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
-Drag and drop files from explorer into the Albums List (left hand panel on the albums tab)
 Files will automatically be sorted into their respective albums and album art will
 be added automatically
-it is possible to add/replace the album art by dragging an image to the album art box
-For drag and drop behaviour see the section
 "Album Creation Behaviour" below
-supported album art file types:BMP, GIF, EXIG, JPG, PNG and TIFF

FOR THIS FEATURE TO WORK TO ITS FULL CAPACITY IT IS CRITICAL TO HAVE PROPERLY TAGGED FILES!!
Notes: some embedded album art gives trouble. i.e. the extracted file is not a valid jpg
       Using Mp3Tag along with id3v2.3 works well to avoid the problem
       Any album images will be resized to optimize for storage space on the player

-Limitations: Can't edit album metadata/song order/add or delete songs - the reason for this
	      limitation is that the player makes its own internal list of songs and albums
	      ignoring any user specified metadata and order.  Adding or deleting songs is not implemented
	      for because some quirks of the player which make it too easy cause the album art
	      not to be displayed

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
-better international filename support



===========================================================================================
=Album Creation Behaviour
==========================================================================================
This section describes how albums will be created on the device when
any combination of valid songs, album art in any folder structure is dragged into the
"Albums" list (the left hand pane in the Albums tab)

Summary:
Files can be in any directory or subdirectory. The program will read the tags
and place it into the appropriate album. This means that songs of the same album
don't have to be in the same directory (although this is the preferred way of doing things).
Embedded album art will be used when available, else, the first valid image file will be used
Any album art added will be resized to compromise between quality and size

Important note: If there are two separate folders, each with songs having the same album name, 
all of the songs will be placed under a single album, and not two separate albums. This is by design.
If this occurs under separate drag and drop operations, you will be asked to overwrite the album
instead.



Detailed cases:

CASE1
=======
Dragged item is a file(s)



**Example1 (single file)**
song1byArtist1onAlbumTitle1Genre1

After dragging in the song, the album list will show
title          artist     genre      
------------------------------------------------
AlbumTitle1    Artist1    Genre1    

No album art will be added to the album unless an image is found in the same directory as the song.
If the song contains embedded album art, then the embedded art will override any image in the directory.
EMBEDDED ALBUM ART OVERRIDES ANY OTHER IMAGES PRESENT




**Example2 (multiple files on the same album)**
song1byArtist1onAlbumTitle1Genre1
song2byArtist1onAlbumTitle1Genre1
image.jpg

After dragging in the songs, the album list will show
title           artist       genre      
------------------------------------------------
AlbumTitle1    Artist1       Genre1    <image.jpg>

Note that it doesn't matter if image.jpg is dragged in or not. It will be added as album art
since it resides in the same directory as the files dragged in.



**Example3 (multiple dragged files with different album titles and different artists/cover art on the same album)**
song1byArtist1onAlbumTitle1Genre1
song2byArtist1onAlbumTitle1Genre1
song1byArtist2onAlbumTitle1Genre2<WithEmbeddedCover1>
song1byArtist2onAlbumTitle2Genre2<WithEmbeddedCover2>
song2byArtist2onAlbumTitle2Genre2<WithEmbeddedCover3>
song1byArtist2onAlbumTitle3Genre2
song1byArtist3onAlbumTitle4Genre4
cover1.jpg
cover2.jpg

After dragging in the files, the album list will show
title           artist       genre      
------------------------------------------------
AlbumTitle1    Various      Various   <embedded cover1>
AlbumTitle2    Artist2      Genre2    <embedded cover2>
AlbumTitle3    Artist2      Genre2    <cover1.jpg>
AlbumTitle4    Artist3      Genre4    <cover1.jpg>

If multiple files are dragged in, an album will be created for each distinct album title that is 
found. since jpg images are present, the first image found is used for the cover art
of all files with no embedded art. If files in the album have different cover art, the first 
valid image file will be used for the album art (remembering that embedded album art takes
precedence over jpg files)




**Example4 (songs with missing tags)**
if the AlbumTitle tag is missing (but it's otherwise a valid song), the song will be placed 
in album Unknown



**Example5 (no valid files)**
cover1.jpg
textfile.txt

Any invalid files are ignored. Since there are no songs, cover1.jpg is ignored.  Music
files with incorrect extension (e.g. AAC file renamed .mp3) are considered invalid





CASE 2
========
Dragged item is a folders(s). 
Process each folder recursively. For each item inside of the opened folder, if the item is
a folder go to CASE2. If the item is a file go to CASE1



**Example 1 (folder contains only subfolders)**
Note: here the items that were dragged from explorer were FOLDER1 and FOLDER2
FOLDER1
   ALBUMFOLDER1
      song1byArtist1onAlbumTitle1Genre1
      song2byArtist1onAlbumTitle1Genre1
      cover1.jpg
   ALBUMFOLDER2
      song1byArtist2onAlbumTitle2Genre2<WithEmbeddedCover>
      song2byArtist2onAlbumTitle2Genre2<WithEmbeddedCover>
   ALBUMFOLDER3
      song3byArtist2onAlbumTitle1Genre1
      song1byArtist3onAlbumTitle3Genre1
      cover2.jpg
FOLDER2
   ALBUMFOLDER4
      image.jpg


After dragging in FOLDER1 from explorer,  The albums will appear as (see previous rules)
title           artist     genre      
------------------------------------------------
AlbumTitle1    Various     Genre1    <cover1.jpg>
AlbumTitle2    Artist2     Genre2    <embedded cover>
AlbumTitle3    Artist3     Genre1    <cover2.jpg>




CASE3
=======
mixed files and folders. this can be decomposed to CASE1 and CASE2

**Example1 (folder with files and subfolders)**
FOLDER1
    ALBUMFOLDER1
      ALBUMFOLDER2
         song1byArtist1onAlbumTitle1Genre1
         song1byArtist1onAlbumTitle2Genre1
      song1byArtist1onAlbumTitle3Genre1
      song2byArtist2onAlbumTitle3Genre1
      cover.jpg
    song1byArtist2onAlbumTitle4Genre2
    song2byArtist3onAlbumTitle4Genre2
    song3byArtist3onAlbumTitle3Genre1
    image.jpg

After dragging in FOLDER1,  The albums will appear as (see previous rules)
title           artist     genre      
----------------------------------------------
AlbumTitle1    Artist1    Genre1    <no album art>
AlbumTitle2    Artist1    Genre1    <no album art>
AlbumTitle3    Various    Genre1    <cover.jpg>
AlbumTitle4    Various    Genre2    <image.jpg>

