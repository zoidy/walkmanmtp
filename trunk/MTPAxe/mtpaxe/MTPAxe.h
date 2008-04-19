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

int setCurrentDevice(char*);
void getDeviceManagerRevision(void);
void getDeviceCount(void);
void getDeviceManufacturer(void);
void getDeviceType(void);
void enumerateDevices(void);
void wmdmAuthenticate(void);
int deviceEnumerateStorage(void);
void deviceGetIcon(char*);
void deviceGetSupportedFormats(void);
void deviceCreatePlaylist(char *,char *);
void deviceDeletePlaylist(char*);
void storageGetSizeInfo(void);
void storageCreateFromFile(char*,char*,int);
void playlistEnumerateContents(char*);


//these functions are internal only
IWMDMStorage3 * findStorageFromPath(int,int, char *);
IWMDMDevice3 * findDevice(char*);
void deviceCreatePlaylist_helper(char*,unsigned long*,IWMDMStorage**);
void deviceEnumerateStorage_helper(IWMDMEnumStorage*,IWMDMStorage3*,char*,int);
void deviceGetSupportedFormats_helper(WMDM_FORMATCODE *,char *);
IWMDMStorage3 * storageCreateFromFile_helper(char *,int*);
