/*
Copyright 2008 Dr. Zoidberg
 
Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0 

Unless required by applicable law or agreed to in writing, software 
distributed under the License is distributed on an "AS IS" 
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions 
and limitations under the License. 
*/

#pragma warning(disable:4996 4156 4154)
#include <windows.h>

// WMDM includes
#include "mswmdm_i.c"
#include "mswmdm.h"
#include "sac.h"
#include "scclient.h"

#include "TConvert.h"
#include "StrTok.h"

#define MTPAXE_M_QUIT 0
#define MTPAXE_M_WMDM_AUTHENTICATE 1
#define MTPAXE_M_DEVICEMANAGER_GETREVISION 10
#define MTPAXE_M_DEVICEMANAGER_GETDEVICECOUNT 11
#define MTPAXE_M_DEVICEMANAGER_ENUMERATEDEVICES 12
#define MTPAXE_M_DEVICEMANAGER_SETCURRENTDEVICE 13
#define MTPAXE_M_DEVICE_GETMANUFACTURER 20
#define MTPAXE_M_DEVICE_GETTYPE 21
#define MTPAXE_M_DEVICE_ENUMERATESTORAGE 22
#define MTPAXE_M_DEVICE_GETSUPPORTEDFORMATS 23
#define MTPAXE_M_DEVICE_GETADDITIONALINFO 24
#define MTPAXE_M_DEVICE_GETICON 25
#define MTPAXE_M_DEVICE_CREATEPLAYLIST 26
#define MTPAXE_M_DEVICE_CREATEALBUM 27
#define MTPAXE_M_PLAYLIST_ENUMERATECONTENTS 30
#define MTPAXE_M_STORAGE_GETSIZEINFO 40
#define MTPAXE_M_STORAGE_CREATEFROMFILE 41
#define MTPAXE_M_STORAGE_DELETE 42
#define MTPAXE_M_STORAGE_GETALBUMARTIMAGE 43


#define MTPAXE_DEVICEENUMSTORAGE_MAXOUTPUTSTRINGSIZE 4194304
#define MTPAXE_MAXNUMBEROFSTORAGEITEMS 40000
#define MTPAXE_MAXFILENAMESIZE 256	//wide char needs twice this amount

void returnMsg(char*,char* =NULL);
void MTPAxe_version(void);

int setCurrentDevice(wchar_t*);
void getDeviceManagerRevision(void);
void getDeviceCount(void);
void deviceGetManufacturer(void);
void deviceGetAdditionalInfo(void);
void deviceGetType(void);
void enumerateDevices(void);
void wmdmAuthenticate(void);
int deviceEnumerateStorage(void);
void deviceGetIcon(wchar_t*);
void deviceCreatePlaylist(wchar_t *,wchar_t *);
void deviceCreateAlbum(wchar_t*,wchar_t*,wchar_t*,wchar_t*,wchar_t*,wchar_t*);
void storageDeleteStorage(wchar_t*);
void storageGetSizeInfo(void);
void storageGetAlbumArtImage(wchar_t*);
void storageCreateFromFile(wchar_t*,wchar_t*,int,wchar_t*,wchar_t*,wchar_t*,wchar_t*,wchar_t*,wchar_t*);
void playlistEnumerateContents(wchar_t*);
void deviceGetFormatsSupport(void);


//these functions are internal only
IWMDMStorage4 * findStorageFromPath(int,int, wchar_t*);
IWMDMStorage4 * findStorageFromID(wchar_t*);
IWMDMDevice3 * findDevice(wchar_t*);
void createStorageReferencesContainer(unsigned long,wchar_t*,wchar_t*,wchar_t* =NULL,wchar_t* =NULL,wchar_t* =NULL,wchar_t* =NULL);
void itemsListToStorageArray(wchar_t*,unsigned long*,IWMDMStorage**);
void deviceEnumerateStorage_helper(IWMDMEnumStorage*,IWMDMStorage4*,int);
void dumpStorageItemsArray(void);
void freePointerFromString(wchar_t *);
wchar_t * readWideCharFromPointer(wchar_t *);
