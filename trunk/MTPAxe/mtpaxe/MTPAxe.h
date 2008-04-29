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
#define MTPAXE_M_DEVICE_GETICON 25
#define MTPAXE_M_DEVICE_CREATEPLAYLIST 26
#define MTPAXE_M_DEVICE_DELETEPLAYLIST 27
#define MTPAXE_M_PLAYLIST_ENUMERATECONTENTS 30
#define MTPAXE_M_STORAGE_GETSIZEINFO 40
#define MTPAXE_M_STORAGE_CREATEFROMFILE 41


#define MTPAXE_DEVICEENUMSTORAGE_MAXOUTPUTSTRINGSIZE 4194304
#define MTPAXE_MAXNUMBEROFSTORAGEITEMS 40000
#define MTPAXE_MAXFILENAMESIZE 256	//wide char needs twice this amount

void returnMsg(char*,char* =NULL);
void MTPAxe_version(void);

int setCurrentDevice(wchar_t*);
void getDeviceManagerRevision(void);
void getDeviceCount(void);
void getDeviceManufacturer(void);
void getDeviceType(void);
void enumerateDevices(void);
void wmdmAuthenticate(void);
int deviceEnumerateStorage(void);
void deviceGetIcon(wchar_t*);
void deviceGetSupportedFormats(void);
void deviceCreatePlaylist(wchar_t *,wchar_t *);
void deviceDeletePlaylist(wchar_t*);
void storageGetSizeInfo(void);
void storageCreateFromFile(wchar_t*,wchar_t*,int,wchar_t*,wchar_t*,wchar_t*,wchar_t*,wchar_t*,wchar_t*);
void playlistEnumerateContents(wchar_t*);


//these functions are internal only
IWMDMStorage4 * findStorageFromPath(int,int, wchar_t*);
IWMDMStorage4 * findStorageFromID(wchar_t*);
IWMDMDevice3 * findDevice(wchar_t*);
void deviceCreatePlaylist_helper(wchar_t*,unsigned long*,IWMDMStorage**);
void deviceEnumerateStorage_helper(IWMDMEnumStorage*,IWMDMStorage4*,int);
void dumpStorageItemsArray(void);
void deviceGetSupportedFormats_helper(WMDM_FORMATCODE *,wchar_t *);
