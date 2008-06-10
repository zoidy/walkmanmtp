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

// MTPAxe.cpp : Defines the entry point for the console application.
//Important: need to increase the stack size in the linker system options (15mb is enough)

#include "stdafx.h"
#include "MTPAxe.h"

#define MTPAXE_ver "MTPAxe by Dr. Zoidberg v0.4.1.1\n"

//file for writing returnMsg output to file
FILE *f=NULL;

//general purpose HRESULT
HRESULT hr;

//used for controlling whether to output to console or no
//useful when calling routines internally
bool gl_output_enabled=true;
bool gl_output_to_file=false;

//the device manager. all interaction after authentication
//is done with the device manager
IWMDeviceManager3* m_pIdvMgr = NULL;

//this array is filled by enumerateDevices
//in order for the external program, to reference a device,
//the device must exists in this array
int numDevices;
IWMDMDevice3 *arrDevices[10];
IWMDMDevice3 *pCurrDev=NULL; //the currently selected device

//this array is filled by devicesEnumerateStorage
//it contains all the storage items for the device 
//devicesEnumerateStorage was called with
int numStorageItems=0;
struct arrStorageItem{
	IWMDMStorage4 *pStorage;	//the storage item
	IWMDMStorage4 *pStorageParent;
	int level;					//the level in the directory tree (level 0 is the root)
	int type;					//the type of the item e.g. file or folder (since a file and a folder can have the same name at thesame directory level)
	unsigned long long size;	//the size of the file in bytes
	wchar_t *parentFileName;
	wchar_t *fileName;
	wchar_t *title;
	wchar_t *albumArtist;
	wchar_t *albumTitle;
	wchar_t *genre;
	wchar_t *year;
	unsigned long trackNum;
	wchar_t *persistentUniqueID;
	wchar_t *parentUniqueID;
};
arrStorageItem arrStorageItems[MTPAXE_MAXNUMBEROFSTORAGEITEMS];

int _tmain(int argc, _TCHAR* argv[])
{
	CoInitialize(NULL);

	//general purpose temp buffer
	wchar_t buffer[MAX_PATH];
	char type[2];
	wchar_t title[400];
	wchar_t artist[100];
	wchar_t album[300];
	wchar_t genre[100];
	wchar_t year[20];
	wchar_t trackNum[4];
	wchar_t items[MTPAXE_DEVICEENUMSTORAGE_MAXOUTPUTSTRINGSIZE];

	//start the message loop
	int msg=99;	
	do
	{
		if(scanf("%d",&msg)>0)
		{
			switch(msg)
			{
				case -1:{
						//this is for debugging. normally, these would be called by the external program
						wmdmAuthenticate();
						enumerateDevices();
						setCurrentDevice(L"WALKMAN");
						deviceEnumerateStorage();
							
						//deviceGetType();
						//deviceGetFormatsSupport();
						//deviceGetAdditionalInfo();
						
						//deviceEnumerateStorage();
						//storageCreateFromFile(L"C:\\wmtp.mp3",L"{00000004-0000-0000-0000-000000000000}",0,L"aa bb",L"bsd s",L"1\u010Ec fsd",L"d1d sd",L"1888",L"2");

						//swprintf(buffer,MTPAXE_MAXFILENAMESIZE,L"%s",L"{00000081-0000-0000-0000-000000000000}");
						//deviceCreatePlaylist(L"test",buffer);
						//deviceEnumerateStorage();
						//playlistEnumerateContents(L"{00000027-0000-0000-B1A9-2548581E0C00}");

						//swprintf(buffer,MTPAXE_MAXFILENAMESIZE,L"%s",L"{00000062-0000-0000-0000-000000000000}:{00000063-0000-0000-0000-000000000000}:{00000064-0000-0000-0000-000000000000}:{00000065-0000-0000-0000-000000000000}");
						//swprintf(buffer,MTPAXE_MAXFILENAMESIZE,L"%s",L"{00000062-0000-0000-0000-000000000000}:{00000063-0000-0000-0000-000000000000}:{00000065-0000-0000-0000-000000000000}:{00000064-0000-0000-0000-000000000000}");
						swprintf(buffer,MTPAXE_MAXFILENAMESIZE,L"%s",L"{000000B6-0000-0000-9FF0-4848BE330E00}");
						deviceCreateAlbum(L"Test1",buffer,L"artist",L"year",L"genre",L"c:\\testart.jpg");
						//deviceCreatePlaylist(L"test",buffer);

						//swprintf(buffer,MTPAXE_MAXFILENAMESIZE,L"%s",L"{00000026-0000-0000-4E94-30484DB50500}");
						//storageGetAlbumArtImage(buffer);
						break;}
				case -3:
					playlistEnumerateContents(L"playlist_name2");
					break;
				case -100:{
					//this is for troubleshooting purposes
					gl_output_to_file=true;	//enable output to file
					MTPAxe_version();
					wmdmAuthenticate();
					printf("Dump directory tree - Available devices:\n");
					enumerateDevices();
					wchar_t name[30];
					printf("enter the device to enumerate (case sensitive): ");
					wscanf(L"%\n",name);
					wscanf(L"%[^\n]",name);
					if (setCurrentDevice(name)==-1)
					{
						gl_output_to_file=false;
						printf("Device '%s' not found. enter 0 to quit",name);
						break;
					}
					printf("enumerating storage...");
					deviceEnumerateStorage();
					dumpStorageItemsArray();
					gl_output_to_file=false;  //disable output to flie
					printf("\nenter 0 to exit:");
					break;
						 }
				case -101:{
					//for trouble shooting purposes, deltes a storage item if nothing else can delete it
					wmdmAuthenticate();
					printf("Delete storage item - Available devices:\n");
					enumerateDevices();
					wchar_t name[50];
					printf("enter the device to enumerate (case sensitive): ");
					wscanf(L"%\n",name);
					wscanf(L"%[^\n]",name);
					setCurrentDevice(name);
					printf("enumerating storage...");
					deviceEnumerateStorage();
					printf("\nEnter ID of playlist to delete: ");
					wscanf(L"%\n",name);
					wscanf(L"%[^\n]",name);
					storageDeleteStorage(name);
					printf("enter 0 to exit:");
					break;
						  }
				case MTPAXE_M_DEVICEMANAGER_GETREVISION:
					getDeviceManagerRevision();
					break;
				case MTPAXE_M_WMDM_AUTHENTICATE:
					wmdmAuthenticate();
					break;
				case MTPAXE_M_DEVICEMANAGER_GETDEVICECOUNT:
					getDeviceCount();
					break;
				case MTPAXE_M_DEVICEMANAGER_ENUMERATEDEVICES:
					enumerateDevices();
					break;
				case MTPAXE_M_DEVICEMANAGER_SETCURRENTDEVICE:
					//the external program will need to send the
					//parameter via stdin										
					wscanf(L"%\n",buffer);	//stupid scanf... this weird thing is so the program will read 
					wscanf(L"%[^\n]",buffer); //an entire line (incl. whitespace) but wait until a newline is encountered, instead of reading the stream immediately
					setCurrentDevice(buffer);
					break;
				case MTPAXE_M_DEVICE_GETMANUFACTURER:
					deviceGetManufacturer();
					break;
				case MTPAXE_M_DEVICE_GETTYPE:
					deviceGetType();
					break;
				case MTPAXE_M_DEVICE_GETADDITIONALINFO:
					deviceGetAdditionalInfo();
					break;
				case MTPAXE_M_DEVICE_ENUMERATESTORAGE:
					deviceEnumerateStorage();
					break;
				case MTPAXE_M_DEVICE_GETICON:
					wscanf(L"%\n",buffer);	//stupid scanf... this weird thing is so the program will read 
					wscanf(L"%[^\n]",buffer); //an entire line (incl. whitespace) but wait until a newline is encountered, instead of reading the stream immediately
					deviceGetIcon(buffer);
					break;
				case MTPAXE_M_DEVICE_GETSUPPORTEDFORMATS:
					deviceGetFormatsSupport();
					break;
				case MTPAXE_M_DEVICE_CREATEPLAYLIST:
					wscanf(L"%\n",buffer);	//stupid scanf... this weird thing is so the program will read 
					wscanf(L"%[^\n]",buffer); //an entire line (incl. whitespace) but wait until a newline is encountered, instead of reading the stream immediately
					wscanf(L"%\n",items);
					wscanf(L"%[^\n]",items);
					deviceCreatePlaylist(buffer,items);
					break;
				case MTPAXE_M_DEVICE_CREATEALBUM:
					wscanf(L"%\n",buffer);	
					wscanf(L"%[^\n]",buffer);
					wscanf(L"%\n",items);
					wscanf(L"%[^\n]",items);
					wscanf(L"%\n",artist);
					wscanf(L"%[^\n]",artist);
					wscanf(L"%\n",genre);
					wscanf(L"%[^\n]",genre);
					wscanf(L"%\n",year);
					wscanf(L"%[^\n]",year);
					wscanf(L"%\n",title);//this is the album art path
					wscanf(L"%[^\n]",title);
					if(wcscmp(artist,L"`")==0) swprintf(artist,2,L"");
					if(wcscmp(genre,L"`")==0) swprintf(genre,2,L"");
					if(wcscmp(year,L"`")==0) swprintf(year,2,L"");
					if(wcscmp(year,L"`")==0) swprintf(title,2,L"");
					deviceCreateAlbum(buffer,items,artist,genre,year,title);
					break;
				case MTPAXE_M_STORAGE_GETALBUMARTIMAGE:
					wscanf(L"%\n",buffer);	
					wscanf(L"%[^\n]",buffer);
					storageGetAlbumArtImage(buffer);
					break;
				case MTPAXE_M_STORAGE_DELETE:
					wscanf(L"%\n",buffer);	//stupid scanf... this weird thing is so the program will read 
					wscanf(L"%[^\n]",buffer); //an entire line (incl. whitespace) but wait until a newline is encountered, instead of reading the stream imme
					storageDeleteStorage(buffer);
					break;
				case MTPAXE_M_PLAYLIST_ENUMERATECONTENTS:
					wscanf(L"%\n",buffer);	
					wscanf(L"%[^\n]",buffer);
					playlistEnumerateContents(buffer);
					break;
				case MTPAXE_M_STORAGE_GETSIZEINFO:
					storageGetSizeInfo();
					break;
				case MTPAXE_M_STORAGE_CREATEFROMFILE:{
					wchar_t item[MAX_PATH];
					wscanf(L"%\n",item);
					wscanf(L"%[^\n]",item);
					wscanf(L"%\n",buffer);	
					wscanf(L"%[^\n]",buffer);
					scanf("%\n",type);	
					scanf("%[^\n]",type);
					wscanf(L"%\n",title);
					wscanf(L"%[^\n]",title);
					wscanf(L"%\n",artist);
					wscanf(L"%[^\n]",artist);
					wscanf(L"%\n",album);
					wscanf(L"%[^\n]",album);
					wscanf(L"%\n",genre);
					wscanf(L"%[^\n]",genre);
					wscanf(L"%\n",year);
					wscanf(L"%[^\n]",year);
					wscanf(L"%\n",trackNum);
					wscanf(L"%[^\n]",trackNum);
					if(wcscmp(title,L"`")==0) swprintf(title,2,L"");
					if(wcscmp(artist,L"`")==0) swprintf(artist,2,L"");
					if(wcscmp(album,L"`")==0) swprintf(album,2,L"");
					if(wcscmp(genre,L"`")==0) swprintf(genre,2,L"");
					if(wcscmp(year,L"`")==0) swprintf(year,2,L"");
					if(wcscmp(trackNum,L"`")==0) swprintf(trackNum,2,L"");
					storageCreateFromFile(item,buffer,atoi(type),title,artist,album,genre,year,trackNum);
					break;}

				default: {MTPAxe_version();break;}

			}
		}else{MTPAxe_version();}

	}while(!msg==MTPAXE_M_QUIT);

	if(!m_pIdvMgr==NULL) m_pIdvMgr->Release();
	if (f!=NULL) fclose(f);

	CoUninitialize();

	return 0;
}

//****************************************************************************
//*                 misc. functions
//****************************************************************************
void MTPAxe_version(void)
{	//prints out the version of the program
	returnMsg(MTPAXE_ver);
}
//*******************************************************************************
//*                          device manager control
//*******************************************************************************
int setCurrentDevice(wchar_t *deviceName)
{	//sets the current device to deviceName. if called internally, this function returns 0 on success
	//and -1 on error. 
	//
	//this function sets the current device pointer to a device in arrDevices that matches devName
	//and resets the numStorageItems counter
	//IMPORTANT: in order to be able to use this device with the functions that operate on the "current"
	//device, it is necessary to call deviceEnumerateStorage. this is so it will fillthe storage array
	//don't do it here since it would slow down the program if we enumerated the storage here and then
	//the calling program called deviceEnumarateStorage for another purpose.

	if(m_pIdvMgr==NULL){returnMsg("-1\n","setCurrentDevice: DeviceManager not initialized\n");return -1;}

	numStorageItems=0;
	pCurrDev=findDevice(deviceName);
	if(pCurrDev==NULL){returnMsg("-1\n","setCurrentDevice: Device not found\n");return -1;}

	returnMsg("0\n");
	return 0;
}

void wmdmAuthenticate(void)
{/*authentication to wmdm is the first step that must be done
   if authentication is successfull 0 is returned. else 1 is returned
  */

	//these are generic keys
	BYTE abPVK[] = {0x00};
	BYTE abCert[] = {0x00};

	IComponentAuthenticate* pICompAuth;
	CSecureChannelClient *m_pSacClient = new CSecureChannelClient;

	// get an authentication interface
	hr = CoCreateInstance(CLSID_MediaDevMgr, NULL, CLSCTX_ALL ,IID_IComponentAuthenticate, (void **)&pICompAuth);
	if FAILED(hr){returnMsg("1\n","Error getting authentication interface\n");return;}

	// create a secure channel client certificate
	hr = m_pSacClient->SetCertificate(SAC_CERT_V1, (BYTE*) abCert, sizeof(abCert), (BYTE*) abPVK, sizeof(abPVK));
	if FAILED(hr){returnMsg("1\n","Error creating a secure channel client certificate\n");return;}
	
	// bind the authentication interface to the secure channel client
	m_pSacClient->SetInterface(pICompAuth);

	// trigger communication
	hr = m_pSacClient->Authenticate(SAC_PROTOCOL_V1);   
	if FAILED(hr){returnMsg("1\n","authentication failed\n");return;}

	// get main interface to media device manager
	hr = pICompAuth->QueryInterface(IID_IWMDeviceManager2, (void**)&m_pIdvMgr);
	if FAILED(hr){returnMsg("1\n","Could not get interfece to device manager\n");return;}

	//at this point, there should be an active interface to the main device manager
	//which we can now use
	returnMsg("0\n");

	pICompAuth->Release();
}

void getDeviceManagerRevision(void)
{/*gets the revision of the active device manager
   returns revision # if everything is ok, -1 otherwise
 */

	if(m_pIdvMgr==NULL){returnMsg("-1\n","getDeviceManagerRevison:DeviceManager not initialized\n");return;}

	DWORD tempDW;
	char *buffer=new char[20];

	hr = m_pIdvMgr->GetRevision(&tempDW);
	if SUCCEEDED(hr) 
	{
		sprintf(buffer,"%x\n",tempDW);
		returnMsg(buffer);
	}else{
		returnMsg("-1\n","could not get manager revision number\n");
	}
	
}
void getDeviceCount(void)
{/*gets the number of devices of the active device manager
   returns # if everything is ok, -1 otherwise
 */

	if(m_pIdvMgr==NULL){returnMsg("-1\n","getDeviceCount: DeviceManager not initialized\n");return;}

	DWORD tempDW;
	char buffer[20];

	hr = m_pIdvMgr->GetDeviceCount(&tempDW);
	if SUCCEEDED(hr) 
	{
		sprintf(buffer,"%d\n",tempDW);
		returnMsg(buffer);
	}else{
		returnMsg("-1\n","could not get device count\n");
	}
	
}
void enumerateDevices(void)
{/*returns a ';' separated string containing the names
   of all connected devices.  returns "-1" if there are no devices
   or an error occured.
  */

	if(m_pIdvMgr==NULL){returnMsg("-1\n","enumerateDevices: DeviceManager not initialized\n");return;}

	char devices[2048];				//the return array
	IWMDMEnumDevice *pIEnumDev;		//the device enumerator
	IWMDMDevice3* pIDevice;			//a device
	WCHAR cName[MTPAXE_MAXFILENAMESIZE];				//temp arrays for wide char conversions
	char ch[MTPAXE_MAXFILENAMESIZE*2];					//.	
	char ch2[MTPAXE_MAXFILENAMESIZE*2];					//.
	size_t ret;						//.
	unsigned long ulNumFetched;		//used by the device enumerator		

	//initialize the return array
	sprintf(devices,"-1");

	// enumerate devices...
	hr = m_pIdvMgr->EnumDevices2(&pIEnumDev);
	if FAILED(hr){returnMsg("-1\n","could not get device enumerator\n");return;}

	//reset the enumerator tto the 0th device. .Next will return the first device
	hr = pIEnumDev->Reset();
	if FAILED(hr){returnMsg("-1\n","error resetting device enumerator\n");return;}
	
	//get the next device
	numDevices=-1;
	hr = pIEnumDev->Next(1, (IWMDMDevice **)&pIDevice, &ulNumFetched);
	while (SUCCEEDED(hr) && (hr != S_FALSE)) 
	{	//if hr is false, that means fewer than the requested # of devices were fetched

		//fill the devices array
		numDevices++;
		arrDevices[numDevices]=pIDevice;
		
		//get the name
		arrDevices[numDevices]->GetName(cName,256);
		//convert to multibyte
		wcstombs_s(&ret, ch, MTPAXE_MAXFILENAMESIZE*2, cName,_TRUNCATE);

		if (strcmp(devices,"-1")==0)
		{	//if it's the first device, overwrite the array to clear the -1
			sprintf(devices,"%s;",ch);
		}else{
			sprintf(ch2,"%s;",ch);
			strcat(devices,ch2);
		}

		hr = pIEnumDev->Next(1, (IWMDMDevice **)&pIDevice, &ulNumFetched);	
	}

	//if there were no devices dound
	if (strcmp(devices,"-1")==0)
	{
		returnMsg("-1\n","enumerateDevices: no devices found\n");
	}else{
		//remove the trailiing ':'
		int len=0;
		len=strlen(devices);		//get the number of bytes in the string
		devices[len-1]=0;			//change the terminator to the previous byte
		strcat(devices,"\n");
		returnMsg(devices);
	}
	
	pIEnumDev->Release();

}
//*******************************************************************************
//*                          device management
//*************************************************************************************
void deviceGetManufacturer(void)
{/*gets the manufacturer of the currently specified device.
   returns -1 on error
 */

	if(m_pIdvMgr==NULL){returnMsg("-1\n","getDeviceManufacturer: DeviceManager not initialized\n");return;}
	if(pCurrDev==NULL){returnMsg("-1\n","getDeviceManufacturer: no active device is set\n");return;}
	
	//we now have a reference to a dvevice
	WCHAR wManName[MTPAXE_MAXFILENAMESIZE];
	char buffer[MTPAXE_MAXFILENAMESIZE*2];
	char buffer2[MTPAXE_MAXFILENAMESIZE*2];
	size_t retr;
	
	hr = pCurrDev->GetManufacturer(wManName,256);
	if SUCCEEDED(hr) 
	{
		wcstombs_s(&retr, buffer, MTPAXE_MAXFILENAMESIZE*2, wManName,_TRUNCATE);
		sprintf(buffer2,"%s\n",buffer);
		returnMsg(buffer2);
	}else{
		returnMsg("-1\n","getDeviceManufacturer:could not get device manufacturer\n");
	}
}

void deviceGetAdditionalInfo(void)
{	//gets more info on the device in a : separated list.  The name of the property is followed by the property value
	//eg. FriendlyName:Walkman
	if(m_pIdvMgr==NULL){returnMsg("-1\n","deviceGetAdditionalInfo: DeviceManager not initialized\n");return;}
	if(pCurrDev==NULL){returnMsg("-1\n","deviceGetAdditionalInfo: no active device is set\n");return;}

	IWMDMDevice3 *pDev3=NULL;

	hr=pCurrDev->QueryInterface(IID_IWMDMDevice3,(void**)&pDev3);
	if(pDev3==NULL){returnMsg("-1\n","deviceGetAdditionalInfo: couldn't get Device3 interface\n");return;}

	PROPVARIANT pvFormatsSupported;
	PropVariantInit(&pvFormatsSupported);

	//initialize return buffer
	wchar_t *buffer=(wchar_t*)CoTaskMemAlloc(700);
	buffer[0]=0;
	buffer[1]=0;

	hr=pDev3->GetProperty(g_wszWMDMDeviceFriendlyName,&pvFormatsSupported);
	if(hr==S_OK)
	{
		wcscat(buffer,L"DeviceFriendlyName:");
		wcscat(buffer,(wchar_t*)pvFormatsSupported.bstrVal);
		wcscat(buffer,L":");
	}
	hr=pDev3->GetProperty(g_wszWMDMDeviceModelName,&pvFormatsSupported);
	if(hr==S_OK){
		wcscat(buffer,L"DeviceModelName:");
		wcscat(buffer,(wchar_t*)pvFormatsSupported.bstrVal);
		wcscat(buffer,L":");
	}
	hr=pDev3->GetProperty(g_wszWMDMDeviceFirmwareVersion,&pvFormatsSupported);
	if(hr==S_OK)
	{
		wcscat(buffer,L"DeviceFirmwareVersion:");
		wcscat(buffer,(wchar_t*)pvFormatsSupported.uintVal );
		wcscat(buffer,L":");
	}
	WMDMID sn;
	unsigned char receivedMAC[WMDM_MAC_LENGTH];
	hr=pDev3->GetSerialNumber(&sn,receivedMAC);
	if(hr==S_OK)
	{
		wcscat(buffer,L"SerialNumber:");

		wchar_t *cSN=(wchar_t*)CoTaskMemAlloc(2*sn.SerialNumberLength+2);
		swprintf(cSN,2*sn.SerialNumberLength+2,L"%s",sn.pID);

		wcscat(buffer,cSN);
		wcscat(buffer,L":");

		CoTaskMemFree(cSN);
	}

	wchar_t *cName=(wchar_t*)CoTaskMemAlloc(200);
	hr=pDev3->GetCanonicalName(cName,100);
	if(hr==S_OK)
	{
		wcscat(buffer,L"CanonicalName:");
		wcscat(buffer,cName);
		wcscat(buffer,L":");
	}

	//remove the trailiing ':'
	buffer[wcslen(buffer)-1]=0;
	wcscat(buffer,L"\n");

	//convert to multibyte
	size_t ret;
	char *retstr=(char*)CoTaskMemAlloc(wcslen(buffer)*2+2);
	wcstombs_s(&ret, retstr, wcslen(buffer)*2+2, buffer,_TRUNCATE);

	returnMsg(retstr);

	CoTaskMemFree(buffer);
	CoTaskMemFree(retstr);
	PropVariantClear(&pvFormatsSupported);
}
void deviceGetType(void)
{/*gets the device attributes for the currently active device
   returns a ':' separated list of the supported attributes, -1 on error
 */


	if(m_pIdvMgr==NULL){returnMsg("-1\n","getDeviceType: DeviceManager not initialized\n");return;}
	if(pCurrDev==NULL){returnMsg("-1\n","getDeviceType: no active device is set\n");return;}
	
	//we now have a reference to a dvevice

	char attr[512];				//the return array
	DWORD tempDW;

	//initialize the return array
	sprintf(attr,"-1");
	
	hr = pCurrDev->GetType(&tempDW);
	if SUCCEEDED(hr) 
	{
		//clear the return array
		sprintf(attr,"");
		if (tempDW & WMDM_DEVICE_TYPE_PLAYBACK)					strcat(attr,"WMDM_DEVICE_TYPE_PLAYBACK:");
		if (tempDW & WMDM_DEVICE_TYPE_RECORD)					strcat(attr,"WMDM_DEVICE_TYPE_RECORD:");
		if (tempDW & WMDM_DEVICE_TYPE_DECODE)					strcat(attr,"WMDM_DEVICE_TYPE_DECODE:");
		if (tempDW & WMDM_DEVICE_TYPE_ENCODE)					strcat(attr,"WMDM_DEVICE_TYPE_ENCODE:");
		if (tempDW & WMDM_DEVICE_TYPE_STORAGE)					strcat(attr,"WMDM_DEVICE_TYPE_STORAGE:");
		if (tempDW & WMDM_DEVICE_TYPE_VIRTUAL)					strcat(attr,"WMDM_DEVICE_TYPE_VIRTUAL:");
		if (tempDW & WMDM_DEVICE_TYPE_SDMI)						strcat(attr,"WMDM_DEVICE_TYPE_SDMI:");
		if (tempDW & WMDM_DEVICE_TYPE_NONSDMI)					strcat(attr,"WMDM_DEVICE_TYPE_NONSDMI:");
		if (tempDW & WMDM_DEVICE_TYPE_NONREENTRANT)				strcat(attr,"WMDM_DEVICE_TYPE_NONREENTRANT:");
		if (tempDW & WMDM_DEVICE_TYPE_FILELISTRESYNC)			strcat(attr,"WMDM_DEVICE_TYPE_FILELISTRESYNC:");
		if (tempDW & WMDM_DEVICE_TYPE_VIEW_PREF_METADATAVIEW)	strcat(attr,"WMDM_DEVICE_TYPE_VIEW_PREF_METADATAVIEW:");
		//remove the traling :
		attr[strlen(attr)-1]=0;
		strcat(attr,"\n");
		returnMsg(attr);
	}else{
		returnMsg("-1\n","getDeviceType: could not get device attributes\n");
	}
	
}

void deviceGetFormatsSupport(void)
{	//returns a : separated list of the formats the device officially supports
	//returns -1 on error ("" is returned for blank list)
	if(m_pIdvMgr==NULL){returnMsg("-1\n","deviceGetFormatsSupport: DeviceManager not initialized\n");return;}
	if(pCurrDev==NULL){returnMsg("-1\n","deviceGetFormatsSupport: no active device is set\n");return;}

	IWMDMDevice3 *pDev3=NULL;

	hr=pCurrDev->QueryInterface(IID_IWMDMDevice3,(void**)&pDev3);
	if(pDev3==NULL){returnMsg("-1\n","deviceGetFormatsSupport: couldn't get Device3 interface\n");return;}

	PROPVARIANT pvFormatsSupported;
	PropVariantInit(&pvFormatsSupported);

	hr=pDev3->GetProperty(g_wszWMDMFormatsSupported,&pvFormatsSupported);
	if(hr!=S_OK)
	{
		returnMsg("-1\n","deviceGetFormatsSupport: coulnd't get property\n");
		PropVariantClear(&pvFormatsSupported);
		return;
	}

	SAFEARRAY *formatList=pvFormatsSupported.parray;
	WMDM_FORMATCODE formatCode=WMDM_FORMATCODE_NOTUSED;
	char *buffer=(char*)CoTaskMemAlloc(3000);
	buffer[0]=0;

	for(long iCap=0;iCap<(long)formatList->rgsabound[0].cElements;iCap++)
	{
		SafeArrayGetElement(formatList,&iCap,&formatCode);
		if(formatCode!=WMDM_FORMATCODE_NOTUSED)
		{
			switch(formatCode)
			{
			case WMDM_FORMATCODE_MP3:{strcat(buffer,"WMDM_FORMATCODE_MP3");break;}
			case 0xb984:{strcat(buffer,"WMDM_FORMATCODE_3GP");break;}
			case WMDM_FORMATCODE_UNDEFINED  :{strcat(buffer,"WMDM_FORMATCODE_UNDEFINED");break;}
		    case WMDM_FORMATCODE_ASSOCIATION:{strcat(buffer,"WMDM_FORMATCODE_ASSOCIATION");break;}
			case WMDM_FORMATCODE_SCRIPT  :{strcat(buffer,"WMDM_FORMATCODE_SCRIPT");break;}
			case WMDM_FORMATCODE_EXECUTABLE:{strcat(buffer,"WMDM_FORMATCODE_EXECUTABLE");break;}
			case WMDM_FORMATCODE_TEXT:{strcat(buffer,"WMDM_FORMATCODE_TEXT");break;}
		    case WMDM_FORMATCODE_HTML :{strcat(buffer,"WMDM_FORMATCODE_HTML");break;}
			case WMDM_FORMATCODE_DPOF:{strcat(buffer,"WMDM_FORMATCODE_DPOF");break;}
			case WMDM_FORMATCODE_AIFF :{strcat(buffer,"WMDM_FORMATCODE_AIFF");break;}
			case WMDM_FORMATCODE_WAVE:{strcat(buffer,"WMDM_FORMATCODE_WAVE");break;}
		    case WMDM_FORMATCODE_AVI :{strcat(buffer,"WMDM_FORMATCODE_AVI");break;}
			case WMDM_FORMATCODE_MPEG:{strcat(buffer,"WMDM_FORMATCODE_MPEG");break;}
			case WMDM_FORMATCODE_ASF:{strcat(buffer,"WMDM_FORMATCODE_ASF");break;}
			case WMDM_FORMATCODE_RESERVED_FIRST:{strcat(buffer,"WMDM_FORMATCODE_RESERVED_FIRST");break;}
		    case WMDM_FORMATCODE_RESERVED_LAST :{strcat(buffer,"WMDM_FORMATCODE_RESERVED_LAST");break;}
			case WMDM_FORMATCODE_IMAGE_UNDEFINED:{strcat(buffer,"WMDM_FORMATCODE_IMAGE_UNDEFINED");break;}
			case WMDM_FORMATCODE_IMAGE_EXIF :{strcat(buffer,"WMDM_FORMATCODE_IMAGE_EXIF");break;}
			case WMDM_FORMATCODE_IMAGE_TIFFEP:{strcat(buffer,"WMDM_FORMATCODE_IMAGE_TIFFEP");break;}
		    case WMDM_FORMATCODE_IMAGE_FLASHPIX:{strcat(buffer,"WMDM_FORMATCODE_IMAGE_FLASHPIX");break;}
			case WMDM_FORMATCODE_IMAGE_BMP:{strcat(buffer,"WMDM_FORMATCODE_IMAGE_BMP");break;}
			case WMDM_FORMATCODE_IMAGE_CIFF:{strcat(buffer,"WMDM_FORMATCODE_IMAGE_CIFF");break;}
			case WMDM_FORMATCODE_IMAGE_GIF:{strcat(buffer,"WMDM_FORMATCODE_IMAGE_GIF");break;}
		    case WMDM_FORMATCODE_IMAGE_JFIF:{strcat(buffer,"WMDM_FORMATCODE_IMAGE_JFIF");break;}
			case WMDM_FORMATCODE_IMAGE_PCD:{strcat(buffer,"WMDM_FORMATCODE_IMAGE_PCD");break;}
			case WMDM_FORMATCODE_IMAGE_PICT:{strcat(buffer,"WMDM_FORMATCODE_IMAGE_PICT");break;}
			case WMDM_FORMATCODE_IMAGE_PNG:{strcat(buffer,"WMDM_FORMATCODE_IMAGE_PNG");break;}
		    case WMDM_FORMATCODE_IMAGE_TIFF:{strcat(buffer,"WMDM_FORMATCODE_IMAGE_TIFF");break;}
			case WMDM_FORMATCODE_IMAGE_TIFFIT:{strcat(buffer,"WMDM_FORMATCODE_IMAGE_TIFFIT");break;}
			case WMDM_FORMATCODE_IMAGE_JP2:{strcat(buffer,"WMDM_FORMATCODE_IMAGE_JP2");break;}
			case WMDM_FORMATCODE_IMAGE_JPX:{strcat(buffer,"WMDM_FORMATCODE_IMAGE_JPX");break;}
		    case WMDM_FORMATCODE_IMAGE_RESERVED_FIRST:{strcat(buffer,"WMDM_FORMATCODE_IMAGE_RESERVED_FIRST");break;}
			case WMDM_FORMATCODE_IMAGE_RESERVED_LAST:{strcat(buffer,"WMDM_FORMATCODE_IMAGE_RESERVED_LAST");break;}
			case WMDM_FORMATCODE_UNDEFINEDFIRMWARE:{strcat(buffer,"WMDM_FORMATCODE_UNDEFINEDFIRMWARE");break;}
			case WMDM_FORMATCODE_UNDEFINEDAUDIO:{strcat(buffer,"WMDM_FORMATCODE_UNDEFINEDAUDIO");break;}
		    case WMDM_FORMATCODE_WMA:{strcat(buffer,"WMDM_FORMATCODE_WMA");break;}
			case 0xb902:{strcat(buffer,"WMDM_FORMATCODE_OGG");break;}
			case 0xb903:{strcat(buffer,"WMDM_FORMATCODE_AAC");break;}
			case 0xb904:{strcat(buffer,"WMDM_FORMATCODE_AUDIBLE");break;}
		    case 0xb906:{strcat(buffer,"WMDM_FORMATCODE_FLAC");break;}
			case WMDM_FORMATCODE_UNDEFINEDVIDEO:{strcat(buffer,"WMDM_FORMATCODE_UNDEFINEDVIDEO");break;}
			case WMDM_FORMATCODE_WMV:{strcat(buffer,"WMDM_FORMATCODE_WMV");break;}
			case 0xb982:{strcat(buffer,"WMDM_FORMATCODE_MP4");break;}
		    case 0xb983:{strcat(buffer,"WMDM_FORMATCODE_MP2");break;}
			case WMDM_FORMATCODE_UNDEFINEDCOLLECTION:{strcat(buffer,"WMDM_FORMATCODE_UNDEFINEDCOLLECTION");break;}
			case WMDM_FORMATCODE_ABSTRACTMULTIMEDIAALBUM:{strcat(buffer,"WMDM_FORMATCODE_ABSTRACTMULTIMEDIAALBUM");break;}
			case WMDM_FORMATCODE_ABSTRACTIMAGEALBUM  :{strcat(buffer,"WMDM_FORMATCODE_ABSTRACTIMAGEALBUM ");break;}
		    case WMDM_FORMATCODE_ABSTRACTAUDIOALBUM:{strcat(buffer,"WMDM_FORMATCODE_ABSTRACTAUDIOALBUM");break;}
			case WMDM_FORMATCODE_ABSTRACTVIDEOALBUM:{strcat(buffer,"WMDM_FORMATCODE_ABSTRACTVIDEOALBUM");break;}
			case WMDM_FORMATCODE_ABSTRACTAUDIOVIDEOPLAYLIST:{strcat(buffer,"WMDM_FORMATCODE_ABSTRACTAUDIOVIDEOPLAYLIST");break;}
			case WMDM_FORMATCODE_ABSTRACTCONTACTGROUP:{strcat(buffer,"WMDM_FORMATCODE_ABSTRACTCONTACTGROUP");break;}
		    case WMDM_FORMATCODE_ABSTRACTMESSAGEFOLDER:{strcat(buffer,"WMDM_FORMATCODE_ABSTRACTMESSAGEFOLDER");break;}
			case WMDM_FORMATCODE_ABSTRACTCHAPTEREDPRODUCTION:{strcat(buffer,"WMDM_FORMATCODE_ABSTRACTCHAPTEREDPRODUCTION");break;}
			case WMDM_FORMATCODE_WPLPLAYLIST :{strcat(buffer,"WMDM_FORMATCODE_WPLPLAYLIST");break;}
			case WMDM_FORMATCODE_M3UPLAYLIST:{strcat(buffer,"WMDM_FORMATCODE_M3UPLAYLIST");break;}
		    case WMDM_FORMATCODE_MPLPLAYLIST:{strcat(buffer,"WMDM_FORMATCODE_MPLPLAYLIST");break;}
			case WMDM_FORMATCODE_ASXPLAYLIST:{strcat(buffer,"WMDM_FORMATCODE_ASXPLAYLIST");break;}
			case WMDM_FORMATCODE_PLSPLAYLIST:{strcat(buffer,"WMDM_FORMATCODE_PLSPLAYLIST");break;}
			case WMDM_FORMATCODE_UNDEFINEDDOCUMENT:{strcat(buffer,"WMDM_FORMATCODE_UNDEFINEDDOCUMENT");break;}
		    case WMDM_FORMATCODE_ABSTRACTDOCUMENT:{strcat(buffer,"WMDM_FORMATCODE_ABSTRACTDOCUMENT");break;}
			case 0xba82:{strcat(buffer,"WMDM_FORMATCODE_XMLDOCUMENT");break;}
			case 0xba83:{strcat(buffer,"WMDM_FORMATCODE_MICROSOFTWORDDOCUMENT");break;}
		    case 0xba84 :{strcat(buffer,"WMDM_FORMATCODE_MHTCOMPILEDHTMLDOCUMENT");break;}
			case 0xba85:{strcat(buffer,"WMDM_FORMATCODE_MICROSOFTEXCELSPREADSHEET");break;}
			case 0xba86:{strcat(buffer,"WMDM_FORMATCODE_MICROSOFTPOWERPOINTDOCUMENT");break;}
			case WMDM_FORMATCODE_UNDEFINEDMESSAGE:{strcat(buffer,"WMDM_FORMATCODE_UNDEFINEDMESSAGE");break;}
		    case WMDM_FORMATCODE_ABSTRACTMESSAGE:{strcat(buffer,"WMDM_FORMATCODE_ABSTRACTMESSAGE");break;}
			case WMDM_FORMATCODE_UNDEFINEDCONTACT:{strcat(buffer,"WMDM_FORMATCODE_UNDEFINEDCONTACT");break;}
			case WMDM_FORMATCODE_ABSTRACTCONTACT:{strcat(buffer,"WMDM_FORMATCODE_ABSTRACTCONTACT");break;}
			case WMDM_FORMATCODE_VCARD2:{strcat(buffer,"WMDM_FORMATCODE_VCARD2");break;}
		    case WMDM_FORMATCODE_VCARD3:{strcat(buffer,"WMDM_FORMATCODE_VCARD3");break;}
			case WMDM_FORMATCODE_UNDEFINEDCALENDARITEM:{strcat(buffer,"WMDM_FORMATCODE_UNDEFINEDCALENDARITEM");break;}
			case WMDM_FORMATCODE_ABSTRACTCALENDARITEM:{strcat(buffer,"WMDM_FORMATCODE_ABSTRACTCALENDARITEM");break;}
			case WMDM_FORMATCODE_VCALENDAR1:{strcat(buffer,"WMDM_FORMATCODE_VCALENDAR1");break;}
		    case 0xbe03 :{strcat(buffer,"WMDM_FORMATCODE_VCALENDAR2");break;}
			case WMDM_FORMATCODE_UNDEFINEDWINDOWSEXECUTABLE:{strcat(buffer,"WMDM_FORMATCODE_UNDEFINEDWINDOWSEXECUTABLE");break;}
			case 0xbe81:{strcat(buffer,"WMDM_FORMATCODE_MEDIA_CAST");break;}
		    case 0xbe82:{strcat(buffer,"WMDM_FORMATCODE_SECTION");break;}
			default:{
							char code[10];
							sprintf(code,"%x",formatCode);
							strcat(buffer,code);
						 }
			}//end switch

			strcat(buffer,":");
		}//end if formatCode!=not used
	}//end for

	//remove the trailiing ':'
	buffer[strlen(buffer)-1]=0;
	strcat(buffer,"\n");

	returnMsg(buffer);
	PropVariantClear(&pvFormatsSupported);
	CoTaskMemFree(buffer);
}
int deviceEnumerateStorage(void)
{	/*gets the full tree of all storage objects in the current device, -1 on error
	when called internally, it returns -1 on error, 0 for operation completed
	
	the adderess of the arrStorageItems array and the number of items are returned

	the format of the returned string is like the following example:

	ROOT
		FOLDER1
			file1
		file2

	would be returned as
	<0/Folder/NULL/0>ROOT:<1/Folder/ROOT/0>FOLDER1:<2/File/FOLDER1/123>file1:<1/File/ROOT/456>file2

	the <#> specifies the level in the heirarchy, the type of node, the parent, the size in bytes.
	Each object is separated by a :
	*/

	if(m_pIdvMgr==NULL){returnMsg("-1\n","deviceEnumerateStorage: DeviceManager not initialized\n");return -1;}
	if(pCurrDev==NULL){returnMsg("-1\n","deviceEnumerateStorage: no active device is set\n");return -1;}
	
	
	//we now have a reference to a dvevice

	char buffer[MTPAXE_DEVICEENUMSTORAGE_MAXOUTPUTSTRINGSIZE];				//the return string
	sprintf(buffer,"-1");

	DWORD tempDW;

	//check to make sure it supports folder heirarchy
	hr = pCurrDev->GetType(&tempDW);
	if SUCCEEDED(hr)
	{
		if (!(tempDW & WMDM_DEVICE_TYPE_STORAGE))
		{
			returnMsg("-1\n","deviceEnumerateStorage: Device is not a storage device\n");
			return-1;
		}

		//get the root storage enumerator
		IWMDMEnumStorage *pIEnumStorage = NULL;
		hr=pCurrDev->EnumStorage(&pIEnumStorage);
		if FAILED(hr){returnMsg("-1\n","could not get storage enumerator\n");return -1;}

		//initialize buffer to remove to -1
		sprintf(buffer,"");
		
		//clear the arrStorageItems array
		for(int i=0;i<numStorageItems;i++)
		{
			if(arrStorageItems[i].pStorage!=NULL)
				arrStorageItems[i].pStorage->Release();
			CoTaskMemFree(arrStorageItems[i].albumArtist);
			CoTaskMemFree(arrStorageItems[i].albumTitle);
			CoTaskMemFree(arrStorageItems[i].parentUniqueID);
			CoTaskMemFree(arrStorageItems[i].fileName);
			CoTaskMemFree(arrStorageItems[i].genre);
			CoTaskMemFree(arrStorageItems[i].persistentUniqueID);
			CoTaskMemFree(arrStorageItems[i].title);
			CoTaskMemFree(arrStorageItems[i].year);
			CoTaskMemFree(arrStorageItems[i].parentFileName);
		}
			
	
		numStorageItems=0;
		deviceEnumerateStorage_helper(pIEnumStorage,NULL,0);

		pIEnumStorage->Release();
	}

	//return the address of the arrStorageItems array, along with the number of items currently in the array
	sprintf(buffer,"%p:%d\n",arrStorageItems,numStorageItems);
	returnMsg(buffer);

	return 0;
}
void deviceEnumerateStorage_helper(IWMDMEnumStorage *pIEnumStorage,IWMDMStorage4 *pParent,int currLevel)
{	/*loop though the storage enumerator.  if a given storage returned by the enumerator
	is a file, append it to the buffer, else make a recursive call.
	*/

	pIEnumStorage->Reset();

	IWMDMStorage4 *pStorage=NULL;					//the storage 
	IWMDMStorage  *tmp=NULL;						//.
	IWMDMMetaData *pMData;							//the metadata associated with the storage
	LPCWSTR MDataAttribs[9];						//the metadata to retreive
	MDataAttribs[0]=g_wszWMDMPersistentUniqueID;	//.
	MDataAttribs[1]=g_wszWMDMTitle;					//.
	MDataAttribs[2]=g_wszWMDMAuthor;				//.
	MDataAttribs[3]=g_wszWMDMAlbumTitle;			//.
	MDataAttribs[4]=g_wszWMDMGenre;					//.
	MDataAttribs[5]=g_wszWMDMYear;					//.
	MDataAttribs[6]=g_wszWMDMTrack;					//.
	MDataAttribs[7]=g_wszWMDMFileSize;				//.
	MDataAttribs[8]=g_wszWMDMFileName;				//.
	WMDM_TAG_DATATYPE type;							//these vars are used for the metadata QueryByName call
	BYTE *value;									//.
	unsigned int len;								//.

	unsigned long ulNumFetched;						//used for the storage enumerator
	wchar_t storName[MTPAXE_MAXFILENAMESIZE];		//stroage name
	wchar_t storParentName[MTPAXE_MAXFILENAMESIZE];	//stroage name of the parent
	wchar_t storParentID[MTPAXE_MAXFILENAMESIZE];	//persistentId of the parent
	DWORD tempDW;									//for the getAtturbutes call
	_WAVEFORMATEX format;							//.
	unsigned long long  size=0;						//.
	HRESULT hr2;									//hresult for retrieving storage info.
	HRESULT hr3;									//mustn't use the global var 'hr' here b/c this function is recursive


	//get some properties of the parent
	if(pParent!=NULL)
	{
		hr3=pParent->GetName(storParentName,MTPAXE_MAXFILENAMESIZE);
		if FAILED(hr3){return;}
		hr3=pParent->GetSpecifiedMetadata(9,MDataAttribs,&pMData);
		if FAILED(hr3){return;}
		hr3=pMData->QueryByName(g_wszWMDMPersistentUniqueID,&type,&value,&len);
		if(hr3==S_OK) 
		{
			swprintf(storParentID,MTPAXE_MAXFILENAMESIZE,L"%s",(wchar_t*)value);
			CoTaskMemFree(value);
		}else
		{
			swprintf(storParentID,1,L"");
		}
		pMData->Release();
	}else
	{	//initialize these properites to "" if failed to get them
		swprintf(storParentName,1,L"");
		swprintf(storParentID,1,L"");
	}
	//loop through each storage item until there is an error or there are no more items
	do
	{
		hr2 = pIEnumStorage->Next(1, (IWMDMStorage**)&tmp, &ulNumFetched);
		if (SUCCEEDED(hr2) && hr2!=S_FALSE)
		{					
			//get a storage4 interface
			hr3=tmp->QueryInterface(IID_IWMDMStorage4,(void**)&pStorage);
			if FAILED(hr3){return;}
			tmp->Release();

			hr3=pStorage->GetName(storName,MTPAXE_MAXFILENAMESIZE);
			if FAILED(hr3){return;}

			//get storage attributes
			hr3=pStorage->GetAttributes(&tempDW,&format);
			if FAILED(hr3){return;}

			arrStorageItem item;							//create a new item to add to the array

			//get the desired metadata
			//first initialize all item fields to default values
			//use cotaskmemalloc instead of malloc so we can use a single
			//call of cotaskmemfree to clear the array of data allocated by WMDM or this program
			value=(BYTE*)CoTaskMemAlloc(2);value[0]=0;value[1]=0;
			item.parentFileName=(wchar_t*)value;
			value=(BYTE*)CoTaskMemAlloc(2);value[0]=0;value[1]=0;
			item.persistentUniqueID =(wchar_t*)value;
			value=(BYTE*)CoTaskMemAlloc(2);value[0]=0;value[1]=0;
			item.fileName=(wchar_t*)value;
			value=(BYTE*)CoTaskMemAlloc(2);value[0]=0;value[1]=0;
			item.title=(wchar_t*)value;
			value=(BYTE*)CoTaskMemAlloc(2);value[0]=0;value[1]=0;
			item.albumArtist=(wchar_t*)value;
			value=(BYTE*)CoTaskMemAlloc(2);value[0]=0;value[1]=0;
			item.albumTitle=(wchar_t*)value;
			value=(BYTE*)CoTaskMemAlloc(2);value[0]=0;value[1]=0;
			item.genre=(wchar_t*)value;
			value=(BYTE*)CoTaskMemAlloc(2);value[0]=0;value[1]=0;
			item.year=(wchar_t*)value;
			value=(BYTE*)CoTaskMemAlloc(2);value[0]=0;value[1]=0;
			item.parentUniqueID=(wchar_t*)value;
			item.size=0;
			item.trackNum=0;
			hr3=pStorage->GetSpecifiedMetadata(9,MDataAttribs,&pMData);
			if SUCCEEDED(hr3)
			{	//only try to get the metatata if the call was successful.
				hr3=pMData->QueryByName(g_wszWMDMTitle,&type,&value,&len);
				if(hr3==S_OK) item.title=(wchar_t*)value;
				hr3=pMData->QueryByName(g_wszWMDMAuthor,&type,&value,&len);
				if(hr3==S_OK) item.albumArtist=(wchar_t*)value;
				hr3=pMData->QueryByName(g_wszWMDMAlbumTitle,&type,&value,&len);
				if(hr3==S_OK) item.albumTitle=(wchar_t*)value;
				hr3=pMData->QueryByName(g_wszWMDMGenre,&type,&value,&len);
				if(hr3==S_OK) item.genre=(wchar_t*)value;
				hr3=pMData->QueryByName(g_wszWMDMYear,&type,&value,&len);
				if(hr3==S_OK) item.year=(wchar_t*)value;
				hr3=pMData->QueryByName(g_wszWMDMTrack,&type,&value,&len);
				if(hr3==S_OK)
				{	
					item.trackNum=item.trackNum<<8;
					item.trackNum=item.trackNum+value[3];
					item.trackNum=item.trackNum<<8;
					item.trackNum=item.trackNum+value[2];
					item.trackNum=item.trackNum<<8;
					item.trackNum=item.trackNum+value[1];
					item.trackNum=item.trackNum<<8;
					item.trackNum=item.trackNum+value[0];
					CoTaskMemFree(value);//can free this value b/c it'sbeen copied to the variable, unlike the strings which are just references
				}
				hr3=pMData->QueryByName(g_wszWMDMFileSize,&type,&value,&len);
				if(hr3==S_OK)
				{	
					item.size=value[7];
					item.size=item.size<<8;
					item.size=item.size+value[6];
					item.size=item.size<<8;
					item.size=item.size+value[5];
					item.size=item.size<<8;					
					item.size=item.size+value[4];
					item.size=item.size<<8;
					item.size=item.size+value[3];
					item.size=item.size<<8;
					item.size=item.size+value[2];
					item.size=item.size<<8;
					item.size=item.size+value[1];
					item.size=item.size<<8;
					item.size=item.size+value[0];
					CoTaskMemFree(value);
				}
				hr3=pMData->QueryByName(g_wszWMDMPersistentUniqueID,&type,&value,&len);
				if(hr3==S_OK) item.persistentUniqueID=(wchar_t*)value;
				//don't get thefile name this way due to compatibility reasons with wmp11.
				//in wmp10, this function returns the proper file name but in wmp11, it returns blank
				//the function GetName, however, returns the same on both, so use that instead (see above)
				//hr3=pMData->QueryByName(g_wszWMDMFileName,&type,&value,&len);
				//if(hr3==S_OK) item.fileName=(wchar_t*)value;

				pMData->Release();
			}
			else{/*dont exit function on metadat fail since we still want to look at the rest of the storage items*/}
			
			//add the storage to the array for later use
			item.pStorage=pStorage;
			item.pStorageParent=pParent;	
			item.level=currLevel;
			item.type=tempDW;
			//make a copy of parent name
			if(wcslen(storName)>0)
			{
				item.fileName=(wchar_t*)CoTaskMemAlloc(wcslen(storName)*2+2);
				swprintf(item.fileName,wcslen(storName)+1,L"%s",storName);
			}
			//make a copy of parent ID if not empty. if it's empty, item.parentID is already "" from before
			if (wcscmp(storParentID,L"")!=0)
			{
				item.parentUniqueID=(wchar_t*)CoTaskMemAlloc(MTPAXE_MAXFILENAMESIZE*2+2);
				swprintf(item.parentUniqueID,wcslen(storParentID)+1,L"%s",storParentID);
			}
			//make a copy of storParentname, if not empty. if it's empty, item.parentname is already "" from before
			if (wcscmp(storParentName,L"")!=0)
			{
				item.parentFileName=(wchar_t*)CoTaskMemAlloc(MTPAXE_MAXFILENAMESIZE*2+2);
				swprintf(item.parentFileName,wcslen(storParentName)+1,L"%s",storParentName);
			}

			arrStorageItems[numStorageItems]=item;
			numStorageItems++;

			/*sprintf(buf2,"<%d/%d/%s/%llu/%S/%S/%S/%S/S>%s:",currLevel,item.type ,buff,item.size,
															item.title,item.albumArtist,item.albumTitle,item.genre,item.year,
															buf);*/
			//sprintf(buf2,"<%d/%d/%s/%llu/%S/%S/%S/%S/%S>%s:",currLevel,item.type ,buff,item.size,
			//												item.title,item.albumTitle,item.albumArtist,item.genre,item.year, 
			//												buf);
			//strcat(buffer,buf2);

			//see if the current storage is a folder, if it is,
			//make a recursive call
			if (tempDW & WMDM_FILE_ATTR_FOLDER)
			{
				//get the storage enumerator for the folder
				IWMDMEnumStorage *pEnumFolderStorage;
				hr3=pStorage->EnumStorage(&pEnumFolderStorage);
				if SUCCEEDED(hr3)
				{
					deviceEnumerateStorage_helper(pEnumFolderStorage,pStorage,currLevel+1);
					pEnumFolderStorage->Release();
				}
			}
		}

	}while (SUCCEEDED(hr2) && hr2 != S_FALSE);

}
void deviceGetIcon(wchar_t *iconSavePath )
{	//gets the icon for the device and saves it to the specified path


	if(m_pIdvMgr==NULL){returnMsg("-1\n","getDeviceIcon: DeviceManager not initialized\n");return;}
	if(pCurrDev==NULL){returnMsg("-1\n","getDeviceIcon: no active device is set\n");return;}
	
	IWMDMStorage3 *pStor=NULL;		//pointer to the icon file (if found)
	IWMDMStorageControl3 *pStorCtrl;//pointer to the storagecontrol for the icon file

	//storage for the current device must have been previously enumerated for this to work.
	//don't enumerate here since it will slow down the app with unecessary enumerations
	pStor=findStorageFromPath(1,WMDM_FILE_ATTR_FILE,L"DevIcon.fil");
	if(pStor==NULL){returnMsg("-1\n","getDeviceIcon: DevIcon.fil not found\n");return;}

	hr = pStor->QueryInterface(IID_IWMDMStorageControl3, (void**)&pStorCtrl);
	if(FAILED(hr)){returnMsg("-1\n","getDeviceIcon: could not get storage control\n");return;}
	
	//we now have a reference to a storage contorl

	//convert the path to wide char LPWSTR
	_towchar iconSavePathW(iconSavePath);

	//read the file to the specified path
	hr=pStorCtrl->Read(WMDM_MODE_BLOCK|WMDM_CONTENT_FILE,iconSavePathW,NULL,NULL);
	if(FAILED(hr)){returnMsg("-1\n","getDeviceIcon: could not save icon\n");return;}

	//the below commented code doesn't seem to work with the s615f
	//ULONG hIcon=0;
	//hr=pDev->GetDeviceIcon (&hIcon);
	//if(FAILED(hr) || hIcon==NULL){
	//	char buff[30];
	//	sprintf(buff,"%x getDeviceIcon: could not get icon\n",hr);
	//	returnMsg("-1\n",buff);
	//	return;
	//}
	//DestroyIcon(hIcon);

	
	returnMsg("0\n");
	
}


void deviceCreatePlaylist(wchar_t *playlistName,wchar_t *items)
{	//don't return anything in this function. status messages will be returned
	//by createStorageReferencesContainer
	createStorageReferencesContainer(WMDM_FORMATCODE_ABSTRACTAUDIOVIDEOPLAYLIST,playlistName,items);
}
void deviceCreateAlbum(wchar_t *albumTitle,wchar_t *items,wchar_t *albumArtist,wchar_t *albumYear,wchar_t *genre,wchar_t *albumArtFile)
{
	//don't return anything in this function. status messages will be returned
	//by createStorageReferencesContainer
	createStorageReferencesContainer(WMDM_FORMATCODE_ABSTRACTAUDIOALBUM,albumTitle,items,albumArtist,albumYear,genre,albumArtFile);
}
void playlistEnumerateContents(wchar_t *playlistID)
{	//enumerates the contents of a playlist.  returns a : separated list of the PersistentUniqueID's
	//of the storage items contained in the playlist. -1 on error

	char buffer[MTPAXE_DEVICEENUMSTORAGE_MAXOUTPUTSTRINGSIZE];	//the return string
	wchar_t tmpBuffer[50];										//buffer for each ID in the loop
	char tmpBuffer2[100];										//buffer for each ID in the loop
	sprintf(buffer,"-1");

	if(m_pIdvMgr==NULL){returnMsg("-1\n","playlistEnumerateContents: DeviceManager not initialized\n");return;}
	if(pCurrDev==NULL){returnMsg("-1\n","playlistEnumerateContentst: no active device is set\n");return;}

	//find the desired playlist
	IWMDMStorage3 *thePlaylist=NULL;
	thePlaylist=findStorageFromID(playlistID);
	if(thePlaylist==NULL){returnMsg("-1\n","playlistEnumerateContents: coudn't find the playlist\n");return;}

	IWMDMStorage4 *pStor=NULL;
	hr=thePlaylist->QueryInterface(IID_IWMDMStorage4,(void**)&pStor);
	if(FAILED(hr)){returnMsg("-1\n","playlistEnumerateContents: coudn't get storage interface to playlist\n");return;}
	
	//now have a storage4 interface the Playlist to get the references of

	IWMDMStorage **pReferencesArray;
	DWORD numRefs;

	hr=pStor->GetReferences(&numRefs,&pReferencesArray);
	if(FAILED(hr)){returnMsg("-1\n","playlistEnumerateContents: coudn't get references\n");return;}
	pStor->Release();

	//initialize buffer to remove to -1
	sprintf(buffer,"");

	IWMDMMetaData *pMData;							//the metadata associated with the storage
	LPCWSTR MDataAttribs[1];						//the metadata to retreive
	MDataAttribs[0]=g_wszWMDMPersistentUniqueID;	//.
	WMDM_TAG_DATATYPE dtype;						//these vars are used for the metadata QueryByName call
	BYTE *value;									//.
	unsigned int len;								//.

	for(unsigned int i=0;i<numRefs;i++)
	{
		hr=pReferencesArray[i]->QueryInterface(IID_IWMDMStorage4,(void**)&pStor);
		if(FAILED(hr)){returnMsg("-1\n","playlistEnumerateContents: coudn't get storage4 interface to playlist item\n");return;}
		pReferencesArray[i]->Release();

		hr=pStor->GetSpecifiedMetadata(1,MDataAttribs,&pMData);
		if(FAILED(hr)){returnMsg("-1\n","playlistEnumerateContents: coudn't get playlist item metadata\n");return;}
		hr=pMData->QueryByName(g_wszWMDMPersistentUniqueID,&dtype,&value,&len);
		if(FAILED(hr)){returnMsg("-1\n","playlistEnumerateContents: coudn't get playlist item ID\n");return;}
		pMData->Release();
		
		swprintf(tmpBuffer,50,(wchar_t*)value);

		//should be safe to convert to char here, since ID shoulnd't have any weird characters
		_tochar cID(tmpBuffer);

		char *t=cID;
		sprintf(tmpBuffer2,"%s:",t);
		strcat(buffer,tmpBuffer2);

		CoTaskMemFree(value);
	}
	
	if(numRefs>0)
	{
		//remove the trailiing ':'
		int len=0;
		len=strlen(buffer); 	//get the number of bytes in the string
		buffer[len-1]=0;		//change the terminator to the previous byte
		strcat(buffer,"\n");	
		returnMsg(buffer);
	}else{
		returnMsg("-1\n","playlistEnumerateContents: playlist is empty\n");
	}

	CoTaskMemFree(pReferencesArray);
}
void storageGetSizeInfo(void)
{	//this function gets the first root storage item
	//of the currently specified device and returns the
	//capacity and free space in the form <capacity>:<free space> in bytes
	//the storage for the current device must have been previously enumerated or else
	//this function won't work.  don't enumerate in this function since we dont want to 
	//slow the app down with extra enumerations
	//returns -1 on error

	if(m_pIdvMgr==NULL){returnMsg("-1\n","storageGetSizeInfo: DeviceManager not initialized\n");return;}
	if(pCurrDev==NULL){returnMsg("-1\n","storageGetSizeInfo: no active device is set\n");return;}
	
	IWMDMStorage3 *pStor=NULL;		//pointer to the root file (if found)

	//get the first storage item (must have been prevously enumerated, see function comments)
	arrStorageItem storItem;
	storItem=arrStorageItems[0];
	pStor=storItem.pStorage;
	if (pStor==NULL){returnMsg("-1\n","storageGetSizeInfo: Error root getting storage\n");return;}
	
	IWMDMStorageGlobals *storGlb;
	hr=pStor->GetStorageGlobals(&storGlb);
	if FAILED(hr){returnMsg("-1\n","storageGetSizeInfo: Error getting storage globals\n");return;}

	DWORD sizeLO;
	DWORD sizeHI;
	DWORD freeLO;
	DWORD freeHI;
	hr=storGlb->GetTotalSize(&sizeLO,&sizeHI);
	if FAILED(hr){returnMsg("-1\n","storageGetSizeInfo: Error getting total size\n");return;}
	hr=storGlb->GetTotalFree(&freeLO,&freeHI);
	if FAILED(hr){returnMsg("-1\n","storageGetSizeInfo: Error getting free size\n");return;}

	storGlb->Release();

	unsigned long long size;
	unsigned long long free;
	size=sizeHI;
	size=(size<<32)+sizeLO;
	free=freeHI;
	free=(free<<32)+freeLO;

	char buffer[100];
	sprintf(buffer,"%llu:%llu\n",size,free);
	returnMsg(buffer);
}

void storageDeleteStorage(wchar_t *storageID)
{	//deltes the specified storage on the device. 
	//return 0 on sucesss, -1 on error

	if(m_pIdvMgr==NULL){returnMsg("-1\n","storageDeleteStorage: DeviceManager not initialized\n");return;}
	if(pCurrDev==NULL){returnMsg("-1\n","storageDeleteStorage: no active device is set\n");return;}


	//find the desired storage
	IWMDMStorage3 *theStorage=NULL;
	theStorage=findStorageFromID(storageID);
	if(theStorage==NULL){returnMsg("-1\n","storageDeleteStorage: coudn't find the storage\n");return;}

	IWMDMStorage3 *pStor=NULL;
	hr=theStorage->QueryInterface(IID_IWMDMStorage3,(void**)&pStor);
	if(FAILED(hr)){returnMsg("-1\n","storageDeleteStorage: coudn't get storage interface to storage\n");return;}
	
	//now have a storage3 interface the Storage to delete

	IWMDMStorageControl3 *pStorCtrl;
	hr = pStor->QueryInterface(IID_IWMDMStorageControl3,(void**)&pStorCtrl);
	if(FAILED(hr)){returnMsg("-1\n","storageDeleteStorage: could not get storage control interface of the Storage to delete\n");return;}

	//now have a storagecontrol3 interface for the storage to delete

	hr=pStorCtrl->Delete(WMDM_MODE_BLOCK|WMDM_MODE_RECURSIVE,NULL );
	if(FAILED(hr)){returnMsg("-1\n","storageDeleteStorage could not get delete storage\n");return;}

	pStorCtrl->Release();
	pStor->Release();

	returnMsg("0\n");

}
void storageCreateFromFile(wchar_t *itemPath,wchar_t *destStorageID, int type,wchar_t *title, wchar_t *albumArtist, wchar_t *albumTitle, wchar_t *genre, wchar_t *year, wchar_t *trackNum)
{	//copies a file to the specified destination storage.
	//item path is the path to the file to copy. deststorgeID is a a string specifying
	//the storage using the PersistentUniqueID string returned by the player when the device contents were enumerated
	//type is 0=file, 1=folder. when type=1, itemPath specifes the name of the folder
	//the metadata must be specified. if it'snot known, an empty string or 'Unknown' should be passed in.
	//this ensures metadata is always written
	//returns the persistentUniqueID of the created item on success, -1 on error

	if(m_pIdvMgr==NULL){returnMsg("-1\n","storageCreateFromFile: DeviceManager not initialized\n");return;}
	if(pCurrDev==NULL){returnMsg("-1\n","storageCreateFromFile: no active device is set\n");return;}
	if(itemPath==NULL){returnMsg("-1\n","storageCreateFromFile: item path is null\n");return;}

	IWMDMStorage4 *pDestStor=NULL;
	int level=-1;
	pDestStor=findStorageFromID(destStorageID);
	if(pDestStor==NULL){returnMsg("-1\n","storageCreateFromFile: destination storage not found\n");return;}
	DWORD tempDW=0;			
	_WAVEFORMATEX format;	
	hr=pDestStor->GetAttributes(&tempDW,&format);
	if((tempDW & WMDM_FILE_ATTR_FOLDER)!=WMDM_FILE_ATTR_FOLDER){returnMsg("-1\n","storageCreateFromFile: destination storage must be a folder\n");return;}

	//now have a storage item of the destination	

	//to find out if we need to set the metadata attributes. 
	//first, we need the extension of the file to set the file type. if it's a supported audio file then we 
	//should add metadata. else don't add any
	bool insertMetaData;
	DWORD formatCode;
	if(type==0)	//don't even bother checking extension if the item is a folder
	{
		wchar_t *ext=wcsrchr(itemPath,L'.');
		if(ext==NULL)
			insertMetaData=false;	//no extension found
		else
		{
			//check for a valid file type
			wcslwr(ext);
			if (wcscmp(ext,L".mp3")==0){insertMetaData=true;formatCode=0x3009;}
			else if (wcscmp(ext,L".wma")==0){insertMetaData=true;formatCode=0xB901;}
			else if ((wcscmp(ext,L".mp4")==0) || wcscmp(ext,L".m4a")==0 ||wcscmp(ext,L".3gp")==0 ){insertMetaData=true;formatCode=0xB903;}//formatCode=0xB982;} use aac code(B903) instead of mp4 code which is for video
			else if (wcscmp(ext,L".wav")==0){insertMetaData=true;formatCode=0x3008;}
			else insertMetaData=false;
		}
	}else
		insertMetaData=false;

	IWMDMStorage *insertedStorage=NULL;
	IWMDMStorageControl3 *pDestStorCtrl;
	unsigned long long size=0;		//the size of the storage. will need this for inserting metadata and for inserting into the arrStorItems array
	if(insertMetaData==false)
	{	
		hr = pDestStor->QueryInterface(IID_IWMDMStorageControl3, (void**)&pDestStorCtrl);
		if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: could not get StorageControl3 interface from destination storage\n");return;}

		//now have a storage control3interface

		if (type==0)
		{
			hr=pDestStorCtrl->Insert(WMDM_MODE_BLOCK | WMDM_STORAGECONTROL_INSERTINTO | WMDM_CONTENT_FILE,
									 itemPath,NULL,NULL,&insertedStorage);
		}else
		{
			hr=pDestStorCtrl->Insert(WMDM_MODE_BLOCK | WMDM_STORAGECONTROL_INSERTINTO | WMDM_CONTENT_FOLDER,
								 itemPath,NULL,NULL,&insertedStorage);
		}
	}else
	{
		//the size of the file.
		FILE *theFile=_wfopen(itemPath,L"r");
		if(theFile==NULL){returnMsg("-1\n","storageCreateFromFile: could not open the file for reading\n");return;}
		fseek(theFile,0,SEEK_END);
		size=_ftelli64(theFile);
		fclose(theFile);

		IWMDMMetaData *pMData=NULL;
		IWMDMStorage3 *pNewStorage=NULL;
		hr=pDestStor->QueryInterface(IID_IWMDMStorage4, (void **)&pNewStorage);
		if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: could not create storage4 interface from destination storage\n");return;}

		hr = pNewStorage->QueryInterface(IID_IWMDMStorageControl3,(void**)&pDestStorCtrl);
		if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: could not get StorageControl3 interface from destination storage\n");return;}

		//now have a storagecontrol3 interface

		hr=pNewStorage->CreateEmptyMetadataObject(&pMData);
		if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: could not create metadata interface from destination storage\n");return;}

		//now have a metadata interface

		hr = pMData->AddItem(WMDM_TYPE_DWORD, g_wszWMDMFormatCode, (BYTE *)&formatCode, sizeof(formatCode)); //the format code is important, without it, player will only recognize mp4 files
		//if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: could not add the format code to MetaData\n");return;}
		hr = pMData->AddItem(WMDM_TYPE_QWORD, g_wszWMDMFileSize, (BYTE *)size, sizeof(size));				 //also important
		//if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: could not add the file size to MetaData\n");return;}
		hr = pMData->AddItem(WMDM_TYPE_STRING, g_wszWMDMTitle, (BYTE *)title, 2*wcslen(title)+2);		
		//if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: could not add the track title to MetaData\n");return;}
		hr = pMData->AddItem(WMDM_TYPE_STRING, g_wszWMDMAuthor, (BYTE *)albumArtist,2*wcslen(albumArtist)+2);		
		//if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: could not add the artist to MetaData\n");return;}
		hr = pMData->AddItem(WMDM_TYPE_STRING, g_wszWMDMAlbumTitle, (BYTE *)albumTitle, 2*wcslen(albumTitle)+2);		
		//if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: could not add the album name to MetaData\n");return;}
		hr = pMData->AddItem(WMDM_TYPE_STRING, g_wszWMDMGenre, (BYTE *)genre, 2*wcslen(genre)+2);		
		//if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: could not add the genre to MetaData\n");return;}
		hr = pMData->AddItem(WMDM_TYPE_STRING, g_wszWMDMYear, (BYTE *)year, 2*wcslen(year)+2);	
		unsigned long track=wcstoul(trackNum,NULL,10);
		if (track!=0) hr = pMData->AddItem(WMDM_TYPE_DWORD, g_wszWMDMTrack, (BYTE *)&track, sizeof(track));		
		//if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: could not add the year to MetaData\n");return;}

		hr = pDestStorCtrl->Insert3(
								 WMDM_MODE_BLOCK | WMDM_CONTENT_FILE, 
								 0, 
								 itemPath, 
								 NULL, 
								 NULL, 
								 NULL,
								 pMData,
								 NULL,
								 &insertedStorage);
		pMData->Release();
	}
	
	if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: insert failed\n");return;}

	//insert was successful. now add the new storage item to the array

	IWMDMStorage4 *insertedStorage4=NULL;
	hr=insertedStorage->QueryInterface(IID_IWMDMStorage4,(void**)&insertedStorage4);
	if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: couldn't get storage4 interface of inserted item\n");return;}

	arrStorageItem plItem;
	IWMDMMetaData *pMData=NULL;						//the metadata associated with the storage
	LPCWSTR MDataAttribs[2];						//the metadata to retreive
	MDataAttribs[0]=g_wszWMDMFileName;				//.
	MDataAttribs[1]=g_wszWMDMPersistentUniqueID;	//.
	WMDM_TAG_DATATYPE dtype;						//these vars are used for the metadata QueryByName call
	BYTE *value;									//.
	unsigned int len;								//.

	hr=insertedStorage4->GetSpecifiedMetadata(2,MDataAttribs,&pMData);
	if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: couldn't get metadata of inserted item\n");return;}

	hr=pMData->QueryByName(g_wszWMDMFileName,&dtype,&value,&len);
	if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: couldn't get filename metadata of inserted item\n");return;}
	plItem.fileName=(wchar_t*)value;

	hr=pMData->QueryByName(g_wszWMDMPersistentUniqueID,&dtype,&value,&len);
	if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: couldn't get ID metadata of inserted item\n");return;}
	plItem.persistentUniqueID=(wchar_t*)value;
	pMData->Release();

	hr=pDestStor->GetSpecifiedMetadata(2,MDataAttribs,&pMData);
	if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: couldn't get metadata of parent of inserted item\n");return;}

	hr=pMData->QueryByName(g_wszWMDMFileName,&dtype,&value,&len);
	if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: couldn't get filename metadata of parent of inserted item\n");return;}
	plItem.parentFileName=(wchar_t*)value;
	
	hr=pMData->QueryByName(g_wszWMDMPersistentUniqueID,&dtype,&value,&len);
	if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: couldn't get ID metadata of parent of inserted item\n");return;}
	plItem.parentUniqueID=(wchar_t*)value;
	pMData->Release();

	plItem.level=level+1;
	plItem.pStorage=insertedStorage4;
	plItem.pStorageParent=pDestStor;
	plItem.size=size;
	plItem.albumArtist=albumArtist;
	plItem.albumTitle=albumTitle;
	plItem.genre=genre;
	plItem.year=year;
	if(type==0){plItem.type=WMDM_FILE_ATTR_FILE;}else{plItem.type=WMDM_FILE_ATTR_FOLDER;}
	arrStorageItems[numStorageItems]=plItem;
	numStorageItems++;

	pDestStorCtrl->Release();

	_tochar cID(plItem.persistentUniqueID);
	char *t=cID;
	char ret[50];
	sprintf(ret,"%s\n",t);
	returnMsg(ret);
}

void storageGetAlbumArtImage(wchar_t *storageID)
{	//gets the album art of any storage (though usually only albums will have it)
	//returns the path where the file was saved, -1 on error

	if(m_pIdvMgr==NULL){returnMsg("-1\n","storageGetAlbumArtImage: DeviceManager not initialized\n");return;}
	if(pCurrDev==NULL){returnMsg("-1\n","storageGetAlbumArtImage: no active device is set\n");return;}

	//first find the storage
	IWMDMStorage4 *pStor=NULL;
	pStor=findStorageFromID(storageID);
	if(pStor==NULL){returnMsg("-1\n","storageGetAlbumArtImage: couldn't find the requested storage\n");return;}
	
	IWMDMMetaData *pMData;							//the metadata associated with the storage
	LPCWSTR MDataAttribs[1];						
	MDataAttribs[0]=g_wszWMDMAlbumCoverData;		

	hr=pStor->GetSpecifiedMetadata(1,MDataAttribs,&pMData);
	if(FAILED(hr)){returnMsg("-1\n","storageGetAlbumArtImage: no album art found\n");return;}

	WMDM_TAG_DATATYPE type;							//these vars are used for the metadata QueryByName call
	BYTE *value;									//.
	unsigned int len;								//.
	hr=pMData->QueryByName(g_wszWMDMAlbumCoverData,&type,&value,&len);
	if(FAILED(hr)){returnMsg("-1\n","storageGetAlbumArtImage:no album art found\n");return;}

	wchar_t *tmpFileNameNoExt=_wtempnam(NULL,L"Axe");
	wchar_t *tmpFileName=(wchar_t*)CoTaskMemAlloc(2*wcslen(tmpFileNameNoExt)+10);
	wsprintf(tmpFileName,L"%s.tmp",tmpFileNameNoExt);
	free(tmpFileNameNoExt);

	FILE *tmpFile=_wfopen(tmpFileName,L"wb");
	if (tmpFile==NULL){returnMsg("-1\n","storageGetAlbumArtImage:album art was found, but coulnd't write it to file\n");return;}
	size_t bytesWritten=fwrite(value,1,len,tmpFile);
	fclose(tmpFile);

	//for some reason, the player only returns the first 64K of an image. therefore if  the full 64k was
	//written, there really was no error. most of the time this should be no problem since the album
	//art images are small anyways
	if (bytesWritten!=len && bytesWritten!=65536)
	{
		char *msg=(char *)CoTaskMemAlloc(200);
		sprintf(msg,"storageGetAlbumArtImage:album art was found, but there was an error writing to the file. Bytes=%u BytesWritten=%u\n",len,bytesWritten);
		returnMsg("-1\n",msg);
		CoTaskMemFree(msg);
		return;
	}
	
	char *ret=(char*)CoTaskMemAlloc(2*wcslen(tmpFileName)+6);
	sprintf(ret,"%S\n",tmpFileName);

	returnMsg(ret);

	CoTaskMemFree(tmpFileName);
	CoTaskMemFree(value);
	pMData->Release();
	CoTaskMemFree(ret);
}
//************************************************************************************************
//*                              Internal functions
//************************************************************************************************
void returnMsg(char *value,char *errorMsg)
{	//prints a return string to stdout and optionally, an error msg. to stderr
	//note:if value represents an error value, an errorMsg description should always be included
	if(gl_output_enabled)
	{
		printf("%s",value);
		_flushall();
		if(!errorMsg==NULL){
			fprintf(stderr,"%s",errorMsg);
			_flushall();
		}
	}

	if(gl_output_to_file)
	{
		if(f==NULL)
			f=fopen("MTPAxe_dump.txt", "w, ccs=UNICODE");
		if(f==NULL)
			printf("couldn't create dump file");
		else{
			_towchar theStringW(value);
			fwprintf(f,L"%s",(LPWSTR)theStringW);
			if(errorMsg!=NULL)
			{
				_towchar theErrorW(errorMsg);
				fwprintf(f,L"error: %s",(LPWSTR)theErrorW);
			}
			_flushall();
		}
	}else//don't close the file until gl_output_to_file is set to false
		if (f!=NULL){fclose(f);f=NULL;}
	
}

IWMDMDevice3 * findDevice(wchar_t* deviceName)
{	//returns a device object based on the provided device name. returns NULL if not found
	//the value is returned through the device pointer passed in

	int i;
	WCHAR cName[MTPAXE_MAXFILENAMESIZE];
	IWMDMDevice3 *pDev=NULL;		//pointer to the device (if found)
	for(i=0;i<=numDevices;i++)
	{
		//get the name
		arrDevices[i]->GetName(cName,MTPAXE_MAXFILENAMESIZE);
		if (wcscmp(deviceName,cName)==0)
		{
			pDev=arrDevices[i];
		}
	}

	return pDev;
}
IWMDMStorage4 * findStorageFromPath(int path,int type, wchar_t *name)
{	//traverse the storage item array (created by deviceEnumerateStorage) for the current
	//device and retruns the first storage with matching level,type and name. returns NULL if no
	//matches are found
	//
	//note: for this function to work properly, the storage for the current device 
	//must have been already enumerated. don't waste extra time here enumerating it again

	int i;
	arrStorageItem sItem;
	for(i=0;i<=numStorageItems;i++)
	{
		sItem=arrStorageItems[i];

		//WMDM_FILE_ATTR_FOLDER is 0x00000008
		//WMDM_FILE_ATTR_FILE   is 0x00000020
		if((sItem.type & type) && (sItem.level==path))
		{
			IWMDMStorage4 *pStorage=NULL;
			WCHAR storName[MTPAXE_MAXFILENAMESIZE];

			pStorage=sItem.pStorage;
			hr=pStorage->GetName(storName,MTPAXE_MAXFILENAMESIZE);
			if SUCCEEDED(hr)//don't exit the function here on error since we want to keep looking, there could be another matching item later on
			{
				if(wcscmp(storName,name)==0)
				{	//we found the item
					return pStorage;
				}
			}
		}
	}

	return NULL;
}


IWMDMStorage4 * findStorageFromID(wchar_t *persistentUniqueID)
{	//searches the arrStorageItems array for the item with the matching ID
	//if it's not found, NULL is returned

	for(int i=0;i<numStorageItems;i++)
	{
		if(wcscmp(arrStorageItems[i].persistentUniqueID,persistentUniqueID)==0)
			return arrStorageItems[i].pStorage;
	}

	return NULL;
}
void createStorageReferencesContainer(unsigned long typeOfContainer,wchar_t *containerName,wchar_t *items,wchar_t *albumArtist,wchar_t *albumYear,wchar_t *genre,wchar_t *albumArtFile)
{	//creates a playlist or an album on the currently selected device.
	//typeOfContainer must be WMDM_FORMATCODE_ABSTRACTAUDIOVIDEOPLAYLIST or WMDM_FORMATCODE_ABSTRACTAUDIOALBUM
	//container name is the name of the playlist or album
	//items is a : separated list of persistentUniqueID's
	//albumAuthor and albumArtFile only apply to albums. they will be ignored if creating a playlist
	//return 0 on sucesss, -1 on error

	if(m_pIdvMgr==NULL){returnMsg("-1\n","createStorageReferencesContainer: DeviceManager not initialized\n");return;}
	if(pCurrDev==NULL){returnMsg("-1\n","createStorageReferencesContainer: no active device is set\n");return;}
	if (typeOfContainer!=WMDM_FORMATCODE_ABSTRACTAUDIOVIDEOPLAYLIST && typeOfContainer!=WMDM_FORMATCODE_ABSTRACTAUDIOALBUM)
	{
		returnMsg("-1\n","createStorageReferencesContainer: invalid container type\n");
		return;
	}

	wchar_t containerFolderName[10];
	if (typeOfContainer==WMDM_FORMATCODE_ABSTRACTAUDIOVIDEOPLAYLIST)
		swprintf(containerFolderName,10,L"Playlists");
	else
		swprintf(containerFolderName,10,L"Albums");

	//find the PLAYLISTS or Albums folder. if it's not there, create it
	IWMDMStorage3 *getPlaylistStor=NULL;
	getPlaylistStor=findStorageFromPath(1,WMDM_FILE_ATTR_FOLDER,containerFolderName);
	if(getPlaylistStor==NULL)
	{
		//insert the playlists or albums folder
		//find the root strage (it will always be the first item in the storage array)
		IWMDMStorage4 *rootStor=NULL;
		rootStor=arrStorageItems[0].pStorage;

		//get the storage control
		IWMDMStorageControl *pRootCtrl;
		hr=rootStor->QueryInterface(IID_IWMDMStorageControl3,(void**)&pRootCtrl);
		if(FAILED(hr)){returnMsg("-1\n","createStorageReferencesContainer: coudn't create Playlists or Albums folder: error getting root storage control\n");return;}

		hr=pRootCtrl->Insert(WMDM_MODE_BLOCK|WMDM_STORAGECONTROL_INSERTINTO|WMDM_CONTENT_FOLDER|WMDM_FILE_CREATE_OVERWRITE,
							 containerFolderName,
							 NULL,
							 NULL,
							 (IWMDMStorage**)&getPlaylistStor);
		if(FAILED(hr)){returnMsg("-1\n","createStorageReferencesContainer: coudn't create Playlists or Albums folder\n");return;}

		//insert was successful. now add the new storage item to the array

		IWMDMStorage4 *insertedStorage4=NULL;
		hr=getPlaylistStor->QueryInterface(IID_IWMDMStorage4,(void**)&insertedStorage4);
		if(FAILED(hr)){returnMsg("-1\n","createStorageReferencesContainer: couldn't get storage4 interface of inserted folder\n");return;}

		arrStorageItem plItem;
		IWMDMMetaData *pMData;							//the metadata associated with the storage
		LPCWSTR MDataAttribs[2];						//the metadata to retreive
		MDataAttribs[0]=g_wszWMDMFileName;				//.
		MDataAttribs[1]=g_wszWMDMPersistentUniqueID;	//.
		WMDM_TAG_DATATYPE dtype;							//these vars are used for the metadata QueryByName call
		BYTE *value;									//.
		unsigned int len;								//.

		hr=insertedStorage4->GetSpecifiedMetadata(2,MDataAttribs,&pMData);
		hr=pMData->QueryByName(g_wszWMDMFileName,&dtype,&value,&len);
		plItem.fileName=(wchar_t*)value;
		hr=pMData->QueryByName(g_wszWMDMPersistentUniqueID,&dtype,&value,&len);
		plItem.persistentUniqueID=(wchar_t*)value;
		pMData->Release();

		hr=rootStor->GetSpecifiedMetadata(2,MDataAttribs,&pMData);
		hr=pMData->QueryByName(g_wszWMDMFileName,&dtype,&value,&len);
		plItem.parentFileName=(wchar_t*)value;
		pMData->Release();

		plItem.level=1;
		plItem.pStorage=insertedStorage4;
		plItem.pStorageParent=rootStor;
		plItem.size=0;
		value=(BYTE*)CoTaskMemAlloc(2);value[0]=0;value[1]=0;
		plItem.albumArtist=(wchar_t*)value;
		value=(BYTE*)CoTaskMemAlloc(2);value[0]=0;value[1]=0;
		plItem.albumTitle=(wchar_t*)value;
		value=(BYTE*)CoTaskMemAlloc(2);value[0]=0;value[1]=0;
		plItem.genre=(wchar_t*)value;
		value=(BYTE*)CoTaskMemAlloc(2);value[0]=0;value[1]=0;
		plItem.year=(wchar_t*)value;
		value=(BYTE*)CoTaskMemAlloc(2);value[0]=0;value[1]=0;
		plItem.title=(wchar_t*)value;
		plItem.type=WMDM_FILE_ATTR_FOLDER;
		arrStorageItems[numStorageItems]=plItem;
		numStorageItems++;

		pRootCtrl->Release();
	}
	IWMDMStorage4 *pStor=NULL; 
	hr=getPlaylistStor->QueryInterface(IID_IWMDMStorage4,(void**)&pStor);
	if(FAILED(hr)){returnMsg("-1\n","createStorageReferencesContainer: coudn't get storage interface to playlist or Albums folder\n");return;}

	//now have a storage interface the Playlists or Albums folder

	IWMDMMetaData *pMetaData;
	hr=pStor->CreateEmptyMetadataObject(&pMetaData);
	if(FAILED(hr)){returnMsg("-1\n","createStorageReferencesContainer: could not create empty metadata object in the storage\n");return;}

	hr=pMetaData->AddItem(WMDM_TYPE_DWORD,g_wszWMDMFormatCode,(BYTE *)&typeOfContainer, sizeof(typeOfContainer));
	if(FAILED(hr)){returnMsg("-1\n","createStorageReferencesContainer: could not add playlist or album to metadata object\n");return;}
	//for albums only
	if(typeOfContainer==WMDM_FORMATCODE_ABSTRACTAUDIOALBUM)
	{
		//store the genre and year in the title tag (the player doesn't seem to write all of the tags)
		wchar_t *genreYear;
		genreYear=(wchar_t*)CoTaskMemAlloc(2*(wcslen(albumYear)+wcslen(genre))+4);
		swprintf(genreYear,L"%s:%s",albumYear,genre);
		
		hr=pMetaData->AddItem(WMDM_TYPE_STRING,g_wszWMDMTitle,(BYTE *)genreYear, 2*wcslen(genreYear)+4);
		CoTaskMemFree(genreYear);
		if(FAILED(hr)){returnMsg("-1\n","createStorageReferencesContainer: could not add genre and year to album metadata object\n");return;}

		//store the artist in the genre tag (it's more likely to have funny characters)
		hr=pMetaData->AddItem(WMDM_TYPE_STRING,g_wszWMDMGenre,(BYTE *)albumArtist, 2*wcslen(albumArtist)+2);
		if(FAILED(hr)){returnMsg("-1\n","createStorageReferencesContainer: could not add artist to album metadata object\n");return;}

		//add the album art if any
		if(wcscmp(albumArtFile,L"")!=0)
		{
			long size=0;
			FILE *theFile=_wfopen(albumArtFile,L"rb");
			if(theFile!=NULL)
			{
				fseek(theFile,0,SEEK_SET);
				fseek(theFile,0,SEEK_END);
				size=ftell(theFile);
				fseek(theFile,0,SEEK_SET);
				char *pCoverData = (char*)CoTaskMemAlloc((size_t)size);
				if(pCoverData)
				{
					fread((void *)pCoverData, 1, (size_t)size, theFile);					

					hr = pMetaData->AddItem(WMDM_TYPE_BINARY, g_wszWMDMAlbumCoverData, (BYTE *)pCoverData, (UINT)size);
					CoTaskMemFree(pCoverData);
					if(FAILED(hr)){returnMsg("-1\n","createStorageReferencesContainer: could not add cover art to album metadata object\n");return;}
					
					//when wmp11 is installed, need to set the image format, or else the image won't be uploaded.
					unsigned long dw=WMDM_FORMATCODE_IMAGE_JFIF;
					hr = pMetaData->AddItem(WMDM_TYPE_DWORD, g_wszWMDMAlbumCoverFormat, (BYTE *)&dw, sizeof(dw));
					if(FAILED(hr)){returnMsg("-1\n","createStorageReferencesContainer: could not add cover art format to album metadata object\n");return;}
				}
				else{returnMsg("-1\n","createStorageReferencesContainer: could not add cover art to album metadata object, not enough memory\n");return;}
				fclose(theFile);
			}
		}
	}
	else
	{
		hr=pMetaData->AddItem(WMDM_TYPE_STRING,g_wszWMDMTitle,(BYTE *)containerName, 2*wcslen(containerName)+2);
		if(FAILED(hr)){returnMsg("-1\n","createStorageReferencesContainer: could not add title to playlist or album metadata object\n");return;}
	}

	IWMDMStorageControl3 *pStorCtrl;
	hr = pStor->QueryInterface(IID_IWMDMStorageControl3,(void**)&pStorCtrl);
	if(FAILED(hr)){returnMsg("-1\n","createStorageReferencesContainer: could not get storage control interface\n");return;}
	pStor->Release();

	//now have a storagecontrol3 interface

	//add the playlist or album to the Playlists or Albums folder (set it to NULL first or
	//insertion may fail for some reason)
	IWMDMStorage *pStorPlaylist=NULL;

	hr = pStorCtrl->Insert3(WMDM_MODE_BLOCK | WMDM_CONTENT_FILE,
                            0,
                            NULL,
                            containerName,
                            NULL,
                            NULL,
                            pMetaData,
                            NULL,
                            &pStorPlaylist);
	if(FAILED(hr)){returnMsg("-1\n","createStorageReferencesContainert: could not insert playlist or album\n");return;}

	//insertion was successful. add it now to the storage items array
	IWMDMStorage4 *pStorPlaylist4;
	hr=pStorPlaylist->QueryInterface(IID_IWMDMStorage4,(void**)&pStorPlaylist4);
	if(FAILED(hr)){returnMsg("-1\n","createStorageReferencesContainer: error getting storage4 interface from inserted playlist or album\n");return;}

	arrStorageItem plItem;
	IWMDMMetaData *pMData;							//the metadata associated with the storage
	LPCWSTR MDataAttribs[2];						//the metadata to retreive
	MDataAttribs[0]=g_wszWMDMFileName;				//.
	MDataAttribs[1]=g_wszWMDMPersistentUniqueID;	//.
	WMDM_TAG_DATATYPE dtype;						//these vars are used for the metadata QueryByName call
	BYTE *value;									//.
	unsigned int len;								//.

	hr=pStorPlaylist4->GetSpecifiedMetadata(2,MDataAttribs,&pMData);
	hr=pMData->QueryByName(g_wszWMDMFileName,&dtype,&value,&len);
	plItem.fileName=(wchar_t*)value;
	hr=pMData->QueryByName(g_wszWMDMPersistentUniqueID,&dtype,&value,&len);
	plItem.persistentUniqueID=(wchar_t*)value;
	pMData->Release();

	plItem.level=2;
	plItem.pStorage=pStorPlaylist4;
	plItem.pStorageParent=pStor;
	plItem.type=WMDM_FILE_ATTR_FILE;
	arrStorageItems[numStorageItems]=plItem;
	numStorageItems++;
	
	//now have a Storage4 interface so we can set the references

	//get the storage items to set the references to
	IWMDMStorage *plItems[MTPAXE_MAXNUMBEROFSTORAGEITEMS];
	DWORD numItems=0; //setting this to 0 clears all references
	itemsListToStorageArray(items,&numItems,plItems);

	//set the references
	hr=pStorPlaylist4->SetReferences(numItems,plItems);
	if(FAILED(hr)){returnMsg("-1\n","createStorageReferencesContainer: could not set playlist or album references\n");return;}

	pMetaData->Release();
	getPlaylistStor->Release();
	pStorPlaylist->Release();
	//pStorPlaylist4->Release(); //don't release this either, since it's now part of the ArrStorageitems array


	returnMsg("0\n");

}
void itemsListToStorageArray(wchar_t *items,unsigned long *pFoundItemsCount,IWMDMStorage **arrStorRet)
{	//parses the items list and returns an array of pointers to
	//the storage items referenced in the list.
	//items is a : separated list of PersistentUniqueID's
	//foundItemsCount is the number of items in the returned array array
	//if a referenced storage item is not found, it will not be included in the returned array

	IWMDMStorage4 *tmpStorItem=NULL;

	wchar_t *itemID=NULL;	//pointer to the next token
	itemID=wcstok(items,L":");
	while(itemID!=NULL)
	{	//should now have a valid item ID.search for it in the arrStorageItems array
		tmpStorItem=findStorageFromID(itemID);

		if(tmpStorItem!=NULL)
		{	//the item was found. add it to the return array
			arrStorRet[*pFoundItemsCount]=tmpStorItem;
			(*pFoundItemsCount)++;
		}
		tmpStorItem=NULL;

		//get the next id
		itemID=wcstok(NULL,L":");
	}
}
void dumpStorageItemsArray(void)
{
	FILE *dumpFile;
	dumpFile=fopen("MTPAxe_dump_items.txt", "w, ccs=UNICODE");
	if(dumpFile==NULL)
	{
		printf("couldn't create dump file");
		return;
	}
	
	fwprintf(dumpFile,L"<level,type,parentName,size,title,artist,album,genre,year,tracknum,ID,parentID>filename\n\n");

	arrStorageItem item;
	for(int i=0;i<numStorageItems;i++)
	{
		item=arrStorageItems[i];
		fwprintf(dumpFile,L"<%d/%d/%s/%llu/%s/%s/%s/%s/%s/%lu/%s/%s>%s\n",item.level ,item.type ,item.parentFileName ,item.size,
															item.title,item.albumArtist,item.albumTitle,item.genre,item.year,
															item.trackNum,item.persistentUniqueID,item.parentUniqueID,item.fileName );
	}

	fclose(dumpFile);
}