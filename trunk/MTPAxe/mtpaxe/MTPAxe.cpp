// MTPAxe.cpp : Defines the entry point for the console application.
//Important: need to increase the stack size in the linker system options (8mb is enough)

#include "stdafx.h"
#include "MTPAxe.h"

#define MTPAXE_ver "MTPAxe by Dr. Zoidberg v0.2.5\n"

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
IWMDMDevice3 *pCurrDev; //the currently selected device

//this array is filled by devicesEnumerateStorage
//it contains all the storage items for the device 
//devicesEnumerateStorage was called with
int numStorageItems=0;
struct arrStorageItem{
	IWMDMStorage3 *pStorage3;	//the storage item
	IWMDMStorage3 *pStorage3Parent;
	int level;					//the level in the directory tree (level 0 is the root)
	int type;					//the type of the item e.g. file or folder (since a file and a folder can have the same name at thesame directory level)
	unsigned long long size;	//the size of the file in bytes
};
arrStorageItem arrStorageItems[MTPAXE_MAXNUMBEROFSTORAGEITEMS];



int _tmain(int argc, _TCHAR* argv[])
{
	CoInitialize(NULL);

	setlocale(LC_ALL,"German");

	//general purpose temp buffer
	char buffer[MTPAXE_MAXFILENAMESIZE];

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
						setCurrentDevice("WALKMAN");
						deviceEnumerateStorage();
						
						//char s[100];
						storageGetSizeInfo();
						
						//sprintf(s,"<1,295176,Storage Media>MUSIC");
						//sprintf(s,"<1,17039624,Storage Media>PICTURES");
						//storageCreateFromFile("c:\\01 - Cosmic Disturbance.m4a",s,0);
						//storageCreateFromFile("c:\\testart.jpg",s,0);

						break;}
				case -3:
					playlistEnumerateContents("playlist_name2");
					break;
				case -100:{
					//this is for troubleshooting purposes
					gl_output_to_file=true;	//enable output to file
					MTPAxe_version();
					wmdmAuthenticate();
					printf("Available devices:\n");
					enumerateDevices();
					char name[30];
					printf("enter the device to enumerate (case sensitive): ");
					scanf("%\n",name);
					scanf("%[^\n]",name);
					if (setCurrentDevice(name)==-1)
					{
						gl_output_to_file=false;
						printf("Device '%s' not found. enter 0 to quit",name);
						break;
					}
					printf("enumerating storage...");
					deviceEnumerateStorage();
					gl_output_to_file=false;  //disable output to flie
					printf("\nenter 0 to exit:");
					break;
						 }
				case -101:{
					//for trouble shooting purposes, deltes a playlist if nothing else can delete it
					wmdmAuthenticate();
					printf("Available devices:\n");
					enumerateDevices();
					char name[30];
					printf("enter the device to enumerate (case sensitive): ");
					scanf("%s",name);
					setCurrentDevice(name);
					printf("enumerating storage...");
					deviceEnumerateStorage();
					printf("\nEnter name of playlist to delete: ");
					scanf("%s",name);
					deviceDeletePlaylist(name);
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
					scanf("%\n",buffer);	//stupid scanf... this weird thing is so the program will read 
					scanf("%[^\n]",buffer); //an entire line (incl. whitespace) but wait until a newline is encountered, instead of reading the stream immediately
					setCurrentDevice(buffer);
					break;
				case MTPAXE_M_DEVICE_GETMANUFACTURER:
					getDeviceManufacturer();
					break;
				case MTPAXE_M_DEVICE_GETTYPE:
					getDeviceType();
					break;
				case MTPAXE_M_DEVICE_ENUMERATESTORAGE:
					deviceEnumerateStorage();
					break;
				case MTPAXE_M_DEVICE_GETICON:
					scanf("%\n",buffer);	//stupid scanf... this weird thing is so the program will read 
					scanf("%[^\n]",buffer); //an entire line (incl. whitespace) but wait until a newline is encountered, instead of reading the stream immediately
					deviceGetIcon(buffer);
					break;
				case MTPAXE_M_DEVICE_GETSUPPORTEDFORMATS:
					deviceGetSupportedFormats();
					break;
				case MTPAXE_M_DEVICE_CREATEPLAYLIST:
					char items[MTPAXE_DEVICEENUMSTORAGE_MAXOUTPUTSTRINGSIZE];
					scanf("%\n",buffer);	//stupid scanf... this weird thing is so the program will read 
					scanf("%[^\n]",buffer); //an entire line (incl. whitespace) but wait until a newline is encountered, instead of reading the stream immediately
					scanf("%\n",items);
					scanf("%[^\n]",items);
					deviceCreatePlaylist(buffer,items);
					break;
				case MTPAXE_M_DEVICE_DELETEPLAYLIST:
					scanf("%\n",buffer);	//stupid scanf... this weird thing is so the program will read 
					scanf("%[^\n]",buffer); //an entire line (incl. whitespace) but wait until a newline is encountered, instead of reading the stream imme
					deviceDeletePlaylist(buffer);
					break;
				case MTPAXE_M_PLAYLIST_ENUMERATECONTENTS:
					scanf("%\n",buffer);	
					scanf("%[^\n]",buffer);
					playlistEnumerateContents(buffer);
					break;
				case MTPAXE_M_STORAGE_GETSIZEINFO:
					storageGetSizeInfo();
					break;
				case MTPAXE_M_STORAGE_CREATEFROMFILE:{
					char item[MTPAXE_MAXFILENAMESIZE];
					char type[2];
					scanf("%\n",item);
					scanf("%[^\n]",item);
					scanf("%\n",buffer);	
					scanf("%[^\n]",buffer);
					scanf("%\n",type);	
					scanf("%[^\n]",type);
					storageCreateFromFile(item,buffer,atoi(type));
					break;}

				default: {MTPAxe_version();break;}

			}
		}else{MTPAxe_version();}

	}while(!msg==MTPAXE_M_QUIT);

	if(!m_pIdvMgr==NULL) m_pIdvMgr->Release();
	if (f!=NULL) fclose(f);

	CoUninitialize();
}

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

void MTPAxe_version(void)
{	//prints out the version of the program
	returnMsg(MTPAXE_ver);
}
int setCurrentDevice(char *deviceName)
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
void getDeviceManufacturer(void)
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
void enumerateDevices(void)
{/*returns a ':' separated string containing the names
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
			sprintf(devices,"%s:",ch);
		}else{
			sprintf(ch2,"%s:",ch);
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





void getDeviceType(void)
{/*gets the device attributes for the currently active device
   returns a ';' separated list of the supported attributes, -1 on error
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
		if (tempDW & WMDM_DEVICE_TYPE_PLAYBACK)					strcat(attr,"WMDM_DEVICE_TYPE_PLAYBACK;");
		if (tempDW & WMDM_DEVICE_TYPE_RECORD)					strcat(attr,"WMDM_DEVICE_TYPE_RECORD;");
		if (tempDW & WMDM_DEVICE_TYPE_DECODE)					strcat(attr,"WMDM_DEVICE_TYPE_DECODE;");
		if (tempDW & WMDM_DEVICE_TYPE_ENCODE)					strcat(attr,"WMDM_DEVICE_TYPE_ENCODE;");
		if (tempDW & WMDM_DEVICE_TYPE_STORAGE)					strcat(attr,"WMDM_DEVICE_TYPE_STORAGE;");
		if (tempDW & WMDM_DEVICE_TYPE_VIRTUAL)					strcat(attr,"WMDM_DEVICE_TYPE_VIRTUAL;");
		if (tempDW & WMDM_DEVICE_TYPE_SDMI)						strcat(attr,"WMDM_DEVICE_TYPE_SDMI;");
		if (tempDW & WMDM_DEVICE_TYPE_NONSDMI)					strcat(attr,"WMDM_DEVICE_TYPE_NONSDMI;");
		if (tempDW & WMDM_DEVICE_TYPE_NONREENTRANT)				strcat(attr,"WMDM_DEVICE_TYPE_NONREENTRANT;");
		if (tempDW & WMDM_DEVICE_TYPE_FILELISTRESYNC)			strcat(attr,"WMDM_DEVICE_TYPE_FILELISTRESYNC;");
		if (tempDW & WMDM_DEVICE_TYPE_VIEW_PREF_METADATAVIEW)	strcat(attr,"WMDM_DEVICE_TYPE_VIEW_PREF_METADATAVIEW;");
		strcat(attr,"\n");
		returnMsg(attr);
	}else{
		returnMsg("-1\n","getDeviceType: could not get device attributes\n");
	}
	
	delete attr;
}

int deviceEnumerateStorage(void)
{	/*gets the full tree of all storage objects in the current device, -1 on error
	when called internally, it returns -1 on error, 0 for operation completed
	
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
			if(arrStorageItems[i].pStorage3!=NULL)
				arrStorageItems[i].pStorage3->Release();
	
		numStorageItems=-1;
		deviceEnumerateStorage_helper(pIEnumStorage,NULL,buffer,0);

		pIEnumStorage->Release();
	}

	//if there were no storages dound
	if (strcmp(buffer,"-1")==0)
	{
		returnMsg("-1\n","deviceEnumerateStorage: no storage found\n");
		return -1;
	}else{
		//remove the trailiing ':'
		int len=0;
		len=strlen(buffer);		//get the number of bytes in the string
		buffer[len-1]=0;		//change the terminator to the previous byte
		strcat(buffer,"\n");
		returnMsg(buffer);
	}

	return 0;
}
void deviceEnumerateStorage_helper(IWMDMEnumStorage *pIEnumStorage,IWMDMStorage3 *pParent, char *buffer,int currLevel)
{	/*loop though the storage enumerator.  if a given storage returned by the enumerator
	is a file, append it to the buffer, else make a recursive call.
	*/

	pIEnumStorage->Reset();

	IWMDMStorage3 *pStorage3=NULL;					//the storage 
	unsigned long ulNumFetched;						//used for the storage enumerator
	WCHAR storName[MTPAXE_MAXFILENAMESIZE];			//stroage name
	WCHAR storParentName[MTPAXE_MAXFILENAMESIZE];	//stroage name of the parent
	char buf[MTPAXE_MAXFILENAMESIZE*2];				//buffers for widechar conversion
	char buff[MTPAXE_MAXFILENAMESIZE*2];
	char buf2[(MTPAXE_MAXFILENAMESIZE*2)*2+20];
	size_t retr;
	DWORD tempDW;						//for the getAtturbutes call
	_WAVEFORMATEX format;				//.
	DWORD sizeLO;						//for getting the size of teh storage
	DWORD sizeHI;						//.
	unsigned long long  size;			//.
	HRESULT hr2;						//hresult for retrieving storage info.
	HRESULT hr3;						//mustn't use the global var 'hr' here b/c this function is recursive


	
	//loop through each storage item until there is an error or there are no more items
	do
	{
		hr2 = pIEnumStorage->Next(1, (IWMDMStorage**)&pStorage3, &ulNumFetched);
		if (SUCCEEDED(hr2) && hr2!=S_FALSE)
		{					
			ZeroMemory(storName,sizeof(storName));
			ZeroMemory(storParentName,sizeof(storParentName));

			hr3=pStorage3->GetName(storName,MTPAXE_MAXFILENAMESIZE);
			if FAILED(hr3){return;}
			if(pParent!=NULL)
			{
				hr3=pParent->GetName(storParentName,MTPAXE_MAXFILENAMESIZE);
				if FAILED(hr3){return;}
			}
			hr3=pStorage3->GetAttributes(&tempDW,&format);
			if FAILED(hr3){return;}
			//note:could also get the metadata however
			//the sony only supports the WMDM/FileName metadata which
			//is the same as getting the storage name as above

			//get the storage size
			sizeLO=0;
			sizeHI=0;
			size=0;
			hr3=pStorage3->GetSize(&sizeLO,&sizeHI);
			if FAILED(hr3){/*dont return if failed, size will simply be 0*/}
			size=sizeHI;
			size=(size<<32)+sizeLO;

			wcstombs_s(&retr, buf, MTPAXE_MAXFILENAMESIZE*2, storName,_TRUNCATE);
			wcstombs_s(&retr, buff, MTPAXE_MAXFILENAMESIZE*2, storParentName,_TRUNCATE);

			//add the storage to the array for later use
			numStorageItems++;
			arrStorageItem item;
			item.pStorage3=pStorage3;
			item.pStorage3Parent=pParent;
			item.level=currLevel;
			item.type=tempDW;
			item.size=size;
			arrStorageItems[numStorageItems]=item;

			sprintf(buf2,"<%d/%d/%s/%llu>%s:",currLevel,tempDW,buff,size,buf);
			strcat(buffer,buf2);

			//see if the current storage is a folder, if it is,
			//make a recursive call
			if (tempDW & WMDM_FILE_ATTR_FOLDER)
			{
				//get the storage enumerator for the folder
				IWMDMEnumStorage *pEnumFolderStorage;
				hr3=pStorage3->EnumStorage(&pEnumFolderStorage);
				if SUCCEEDED(hr3)
				{
					deviceEnumerateStorage_helper(pEnumFolderStorage,pStorage3,buffer,currLevel+1);
					pEnumFolderStorage->Release();
				}
			}
		}

	}while (SUCCEEDED(hr2) && hr2 != S_FALSE);

}
void deviceGetIcon(char *iconSavePath )
{	//gets the icon for the device and saves it to the specified path


	if(m_pIdvMgr==NULL){returnMsg("-1\n","getDeviceIcon: DeviceManager not initialized\n");return;}
	if(pCurrDev==NULL){returnMsg("-1\n","getDeviceIcon: no active device is set\n");return;}
	
	IWMDMStorage3 *pStor=NULL;		//pointer to the icon file (if found)
	IWMDMStorageControl3 *pStorCtrl;//pointer to the storagecontrol for the icon file

	//storage for the current device must have been previously enumerated for this to work.
	//don't enumerate here since it will slow down the app with unecessary enumerations
	pStor=findStorageFromPath(1,WMDM_FILE_ATTR_FILE,"DevIcon.fil");
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
	pStor=storItem.pStorage3;
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

void deviceDeletePlaylist(char *playlistName)
{	//deltes the currently selected playlist on the device. 
	//return 0 on sucesss, -1 on error

	if(m_pIdvMgr==NULL){returnMsg("-1\n","deviceDeletePlaylist: DeviceManager not initialized\n");return;}
	if(pCurrDev==NULL){returnMsg("-1\n","deviceDeletePlaylist: no active device is set\n");return;}


	//find the desired playlist
	IWMDMStorage3 *thePlaylist=NULL;
	thePlaylist=findStorageFromPath(2,WMDM_FILE_ATTR_FILE,playlistName);
	if(thePlaylist==NULL){returnMsg("-1\n","deviceDeletePlaylist: coudn't find the playlist\n");return;}

	IWMDMStorage3 *pStor=NULL;
	hr=thePlaylist->QueryInterface(IID_IWMDMStorage3,(void**)&pStor);
	if(FAILED(hr)){returnMsg("-1\n","deviceDeletePlaylist: coudn't get storage interface to playlist\n");return;}
	
	//now have a storage interface the Playlist to delete

	IWMDMStorageControl3 *pStorCtrl;
	hr = pStor->QueryInterface(IID_IWMDMStorageControl3,(void**)&pStorCtrl);
	if(FAILED(hr)){returnMsg("-1\n","deviceDeletePlaylist: could not get storage control interface of the playlist\n");return;}

	//now have a storagecontrol3 interface for the playlist to delete

	hr=pStorCtrl->Delete(WMDM_MODE_BLOCK,NULL);
	if(FAILED(hr)){returnMsg("-1\n","deviceDeletePlaylist could not get delete playlist\n");return;}

	pStorCtrl->Release();
	pStor->Release();

	returnMsg("0\n");

}
void deviceCreatePlaylist(char *playlistName,char *items)
{	//creates a playlist on the device. items is a : separated list in the same format as output
	//by deviceEnumerateStorage
	//return 0 on sucesss, -1 on error

	if(m_pIdvMgr==NULL){returnMsg("-1\n","deviceCreatePlaylist: DeviceManager not initialized\n");return;}
	if(pCurrDev==NULL){returnMsg("-1\n","deviceCreatePlaylist: no active device is set\n");return;}

	//find the PLAYLISTS folder. if it's not there, create it
	IWMDMStorage3 *getPlaylistStor=NULL;
	getPlaylistStor=findStorageFromPath(1,WMDM_FILE_ATTR_FOLDER,"Playlists");
	if(getPlaylistStor==NULL)
	{
		//insert the playlists folder
		//find the root strage (it will always be the first item in the storage array)
		IWMDMStorage3 *rootStor=NULL;
		rootStor=arrStorageItems[0].pStorage3;

		//get the storage control
		IWMDMStorageControl *pRootCtrl;
		hr=rootStor->QueryInterface(IID_IWMDMStorageControl3,(void**)&pRootCtrl);
		if(FAILED(hr)){returnMsg("-1\n","deviceDeletePlaylist: coudn't create Playlists folder: error getting root storage control\n");return;}

		hr=pRootCtrl->Insert(WMDM_MODE_BLOCK|WMDM_STORAGECONTROL_INSERTINTO|WMDM_CONTENT_FOLDER|WMDM_FILE_CREATE_OVERWRITE,
							 L"Playlists",
							 NULL,
							 NULL,
							 (IWMDMStorage**)&getPlaylistStor);
		if(FAILED(hr)){returnMsg("-1\n","deviceDeletePlaylist: coudn't create Playlists folder\n");return;}

		pRootCtrl->Release();
	}
	IWMDMStorage3 *pStor=NULL; 
	hr=getPlaylistStor->QueryInterface(IID_IWMDMStorage3,(void**)&pStor);
	if(FAILED(hr)){returnMsg("-1\n","deviceDeletePlaylist: coudn't get storage interface to playlist folder\n");return;}

	//now have a storage interface the Playlists folder

	IWMDMMetaData *pMetaData;
	hr=pStor->CreateEmptyMetadataObject(&pMetaData);
	if(FAILED(hr)){returnMsg("-1\n","deviceCreatePlaylist: could not create empty metadata object in the storage\n");return;}

	DWORD dw=WMDM_FORMATCODE_ABSTRACTAUDIOVIDEOPLAYLIST;
	hr=pMetaData->AddItem(WMDM_TYPE_DWORD,g_wszWMDMFormatCode,(BYTE *)&dw, sizeof(dw));
	if(FAILED(hr)){returnMsg("-1\n","deviceCreatePlaylist: could not add playlist to metadata object\n");return;}

	IWMDMStorageControl3 *pStorCtrl;
	hr = pStor->QueryInterface(IID_IWMDMStorageControl3,(void**)&pStorCtrl);
	if(FAILED(hr)){returnMsg("-1\n","deviceCreatePlaylist: could not get storage control interface\n");return;}

	//now have a storagecontrol3 interface

	//add the playlist to the playlists folder
	IWMDMStorage *pStorPlaylist;
	//convert the list name to LPWSTR
	_towchar listNameW(playlistName);
	hr = pStorCtrl->Insert3(WMDM_MODE_BLOCK | WMDM_CONTENT_FILE,
                            0,
                            NULL,
                            listNameW,
                            NULL,
                            NULL,
                            pMetaData,
                            NULL,
                            &pStorPlaylist);
	
	if(FAILED(hr)){returnMsg("-1\n","deviceCreatePlaylist: could not insert playlist\n");return;}

	//insertion was successful. add it now to the storage items array
	IWMDMStorage3 *pStorPlaylist3;
	hr=pStorPlaylist->QueryInterface(IID_IWMDMStorage3,(void**)&pStorPlaylist3);
	if(FAILED(hr)){returnMsg("-1\n","deviceCreatePlaylist: error inserting into storageitem array\n");return;}
	numStorageItems++;
	arrStorageItem plItem;
	plItem.level=2;
	plItem.pStorage3=pStorPlaylist3;
	plItem.pStorage3Parent=pStor;
	plItem.type=WMDM_FILE_ATTR_FILE;
	arrStorageItems[numStorageItems]=plItem;
	
	//get a storage4 interface to set the references
	IWMDMStorage4 *pStorPlaylist_setreferences;
	hr = pStorPlaylist->QueryInterface(IID_IWMDMStorage4, (void**)&pStorPlaylist_setreferences);
	if(FAILED(hr)){returnMsg("-1\n","deviceCreatePlaylist: could not get Storage4 interface from playlist storage\n");return;}

	//now have a Storage4 interface so we can set the references

	//get the storage items to set the references to
	IWMDMStorage *plItems[MTPAXE_MAXNUMBEROFSTORAGEITEMS];
	DWORD numItems=0; //setting this to 0 clears all references
	deviceCreatePlaylist_helper(items,&numItems,plItems);

	//set the references
	hr=pStorPlaylist_setreferences->SetReferences(numItems,plItems);
	pStorPlaylist_setreferences->Release();
	if(FAILED(hr)){returnMsg("-1\n","deviceCreatePlaylist: could not set playlist references\n");return;}

	pMetaData->Release();
	pStor->Release();
	//pStorPlaylist->Release(); //don't release this, it causes problems for some reason when using findStorageFrompath
	pStorPlaylist_setreferences->Release();

	returnMsg("0\n");

}
void deviceCreatePlaylist_helper(char *items,unsigned long *pFoundItemsCount,IWMDMStorage **arrStorRet)
{	//parses the items list and returns an array of pointers to
	//the storage items referenced in the list. foundItemsCount is the number of items in this array
	//if a referenced storage item is not found, it will not be included in the returned array

	*pFoundItemsCount=0;

	char *item;
	char *attribs;
	int level;
	char *clevel;
	unsigned long type;  //DWORD
	char *ctype;
	char parent[MTPAXE_MAXFILENAMESIZE];
	char *name;
	
	arrStorageItem arrItem;
	IWMDMStorage3 *pStor=NULL;						//the storage item in arritem
	IWMDMStorage3 *pParentStor=NULL;				//the storageParent item in arritem
	WCHAR storName[MTPAXE_MAXFILENAMESIZE];			//name of the storage item in arrItem
	WCHAR storParentName[MTPAXE_MAXFILENAMESIZE];	//name of the parent storage item in arrItem
	char buf[MTPAXE_MAXFILENAMESIZE*2];				//buffer for widechar to multibyte char conversion
	size_t retr;
	bool found;										//flag to see if we found a matching storage. break out of the loop if ture

	CStrTok tokenizer[3]; //the tokenizer
	//get the first item
	item=tokenizer[0].GetFirst(items,":");
	while(item)
	{
		found=false;

		//now have an item
	
		//get the item attributes string
		item=tokenizer[1].GetFirst(item,"<");	//remove the leading <
		attribs=tokenizer[1].GetFirst(item,">");
	
		//get each attribute from the attributes string
		clevel=tokenizer[2].GetFirst(attribs,"/");
		level=atoi(clevel);
		ctype=tokenizer[2].GetNext("/");
		type=strtoul(ctype,NULL,10);
		sprintf(parent,"%s",tokenizer[2].GetNext("/"));

		//get the item name
		name=tokenizer[1].GetNext(">");

		//now that the item string is parsed, look for the item
		//in the arrStorageItems array. (only look for file items
		//since it doesn't make sense to add folder items to the playlist)
		if((type & WMDM_FILE_ATTR_FILE)==WMDM_FILE_ATTR_FILE)
			
			//don't use findStorageFromPath since we need to make sure that
			//the item we found has the same parent as the item we want to add
			//(there could be a file with the same name but in a different directory)
			for(int i=0;i<=numStorageItems;i++)
			{	

				//break out of the loop if we found a match
				if(!found)
				{
					arrItem=arrStorageItems[i];

					pStor=arrItem.pStorage3 ;
					hr=pStor->GetName(storName,256);
					if SUCCEEDED(hr)
					{
						pStor=arrItem.pStorage3Parent;
						if (pStor!=NULL)
							hr=pStor->GetName(storParentName,256);
						else
							swprintf(storParentName,L":");
						if SUCCEEDED(hr)
						{
							wcstombs_s(&retr, buf, MTPAXE_MAXFILENAMESIZE*2, storName,_TRUNCATE);

							if(strcmp(buf,name)==0)
								if(arrItem.level==level)
									if(arrItem.type==type)
									{	
										wcstombs_s(&retr, buf, MTPAXE_MAXFILENAMESIZE*2, storParentName,_TRUNCATE);
										if(strcmp(buf,parent)==0)
										{
											arrStorRet[*pFoundItemsCount]=arrItem.pStorage3;
											(*pFoundItemsCount)++;
											found=true;
										}
									}
						}//if succeeded
					}//if suceeded
				}else{break;}//if not found

			}//end for

		//move to the next item
		item=tokenizer[0].GetNext(":");
	}
}

void playlistEnumerateContents(char *playlistName)
{	//enumerates the contents of a playlist.  returns a : separated list (in the same format as
	//deviceEnumerateStorage) of the storage items contained in the playlist. -1 on error

	char buffer[MTPAXE_DEVICEENUMSTORAGE_MAXOUTPUTSTRINGSIZE];				//the return string
	sprintf(buffer,"-1");

	if(m_pIdvMgr==NULL){returnMsg("-1\n","playlistEnumerateContents: DeviceManager not initialized\n");return;}
	if(pCurrDev==NULL){returnMsg("-1\n","playlistEnumerateContentst: no active device is set\n");return;}

	//find the desired playlist
	IWMDMStorage3 *thePlaylist=NULL;
	thePlaylist=findStorageFromPath(2,WMDM_FILE_ATTR_FILE,playlistName);
	if(thePlaylist==NULL){returnMsg("-1\n","playlistEnumerateContents: coudn't find the playlist\n");return;}

	IWMDMStorage4 *pStor=NULL;
	hr=thePlaylist->QueryInterface(IID_IWMDMStorage4,(void**)&pStor);
	if(FAILED(hr)){returnMsg("-1\n","playlistEnumerateContents: coudn't get storage interface to playlist\n");return;}
	
	//now have a storage4 interface the Playlist to get the references of

	IWMDMStorage **pReferencesArray;
	DWORD numRefs;

	hr=pStor->GetReferences(&numRefs,&pReferencesArray);
	if(FAILED(hr)){returnMsg("-1\n","playlistEnumerateContents: coudn't get storage interface to playlist\n");return;}
	pStor->Release();

	//initialize buffer to remove to -1
	sprintf(buffer,"");

	WCHAR storName[MTPAXE_MAXFILENAMESIZE];				//stroage name
	WCHAR storParentName[MTPAXE_MAXFILENAMESIZE];
	char buf[MTPAXE_MAXFILENAMESIZE*2];					//for wide char to multi byte conversion
	char buff[MTPAXE_MAXFILENAMESIZE*2];				//for wide char to multi byte conversion
	char buf2[(MTPAXE_MAXFILENAMESIZE*2)*2+20];
	size_t retr;
	DWORD tempDW;										//for the getAtturbutes call
	_WAVEFORMATEX format;								//.
	IWMDMStorage *pParent=NULL;							//the parent object of pStor
	DWORD sizeLO;										//for getting the size of teh storage
	DWORD sizeHI;										//.
	unsigned long long size;							//.
	int level;

	for(unsigned int i=0;i<numRefs;i++)
	{
		ZeroMemory(storName,sizeof(storName));
		ZeroMemory(storParentName,sizeof(storParentName));

		hr=pReferencesArray[i]->QueryInterface(IID_IWMDMStorage4,(void**)&pStor);
		if(FAILED(hr)){returnMsg("-1\n","playlistEnumerateContents: coudn't get storage4 interface to playlist item\n");return;}
		pReferencesArray[i]->Release();

		hr=pStor->GetName(storName,MTPAXE_MAXFILENAMESIZE);
		if(FAILED(hr)){returnMsg("-1\n","playlistEnumerateContents: coudn't get playlist item name\n");return;}
		hr=pStor->GetParent(&pParent);
		if(FAILED(hr)){returnMsg("-1\n","playlistEnumerateContents: coudn't get playlist item parent\n");return;}
		hr=pParent->GetName(storParentName,MTPAXE_MAXFILENAMESIZE);

		hr=pStor->GetAttributes(&tempDW,&format);
		if(FAILED(hr)){returnMsg("-1\n","playlistEnumerateContents: coudn't get playlist item type\n");return;}

		//get the storage size
		sizeLO=0;
		sizeHI=0;
		size=0;
		hr=pStor->GetSize(&sizeLO,&sizeHI);
		if FAILED(hr){/*dont return if failed, size will simply be 0*/}
		size=sizeHI;
		size=(size<<32)+sizeLO;

		//find the level of the referenced playlist item 
		level=1;//start at level=1 b/c we already got the parent of the file in question
		while(SUCCEEDED(hr))
		{
			pStor->Release();
			pStor=NULL;

			hr=pParent->QueryInterface(IID_IWMDMStorage4,(void**)&pStor);
			if(FAILED(hr)){returnMsg("-1\n","playlistEnumerateContents: coudn't get storage4 interface to playlist item parent\n");return;}
			pParent->Release();
			pParent=NULL;

			//pStor now points to the parent of the item that was stored in pStor

			hr=pStor->GetParent(&pParent);

			//pParent is now the grandparent of the original playlist item

			if(SUCCEEDED(hr) && hr!=S_FALSE)
				//if we're here, the parent did indeed have a parent
				level++;
			else
				//the parent of pStor did not have a parent, it's top level
				hr=-1;
		}
		

		wcstombs_s(&retr, buf, MTPAXE_MAXFILENAMESIZE*2, storName,_TRUNCATE);
		wcstombs_s(&retr, buff, MTPAXE_MAXFILENAMESIZE*2, storParentName,_TRUNCATE);

		sprintf(buf2,"<%d/%d/%s/%llu>%s:",level,tempDW,buff,size,buf);
		strcat(buffer,buf2);
	}
	
	if(numRefs>0)
	{
		//remove the trailiing ':'
		int len=0;
		len=strlen(buffer);		//get the number of bytes in the string
		buffer[len-1]=0;		//change the terminator to the previous byte
		strcat(buffer,"\n");
		returnMsg(buffer);
	}else{
		returnMsg("-1\n","playlistEnumerateContents: playlist is empty\n");
	}

	CoTaskMemFree(pReferencesArray);
}
void storageCreateFromFile(char *itemPath,char *destStorage, int type)
{	//copies a file to the specified destination storage.
	//item path is the path to the file to copy. dest storge is a astring specifying
	//the storage in the same format as returned by enumerateStorage
	//type is 0=file, 1=folder. when type=1, itemPath specifes the name of the folder
	//returns 0 on success, -1 on error

	if(m_pIdvMgr==NULL){returnMsg("-1\n","storageCreateFromFile: DeviceManager not initialized\n");return;}
	if(pCurrDev==NULL){returnMsg("-1\n","storageCreateFromFile: no active device is set\n");return;}

	IWMDMStorage3 *pDestStor=NULL;
	int level=-1;
	pDestStor=storageCreateFromFile_helper(destStorage,&level);
	if(pDestStor==NULL){returnMsg("-1\n","storageCreateFromFile: destination storage not found or it is not a folder\n");return;}

	//now have a storage item of the destination

	IWMDMStorageControl3 *pDestStorCtrl;
	hr = pDestStor->QueryInterface(IID_IWMDMStorageControl3, (void**)&pDestStorCtrl);
	if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: could not get StorageControl3 interface from destination storage\n");return;}

	//now have a storagecontrol3 interface

	//convert the path to LPWSTR
	_towchar itemPathW(itemPath);

	IWMDMStorage *insertedStorage=NULL;
	if (type==0)
	{
		hr=pDestStorCtrl->Insert(WMDM_MODE_BLOCK | WMDM_STORAGECONTROL_INSERTINTO | WMDM_CONTENT_FILE,
							     itemPathW,NULL,NULL,&insertedStorage);
	}else
	{
		hr=pDestStorCtrl->Insert(WMDM_MODE_BLOCK | WMDM_STORAGECONTROL_INSERTINTO | WMDM_CONTENT_FOLDER,
							 itemPathW,NULL,NULL,&insertedStorage);
	}

	
	if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: insert failed\n");return;}

	//insert was successful. now add the new storage item to the array

	IWMDMStorage3 *insertedStorage3=NULL;
	hr=insertedStorage->QueryInterface(IID_IWMDMStorage3,(void**)&insertedStorage3);
	if(FAILED(hr)){returnMsg("-1\n","storageCreateFromFile: couldn't get storage3 interface of inserted item\n");return;}

	//get the storage size
	DWORD sizeLO=0;
	DWORD sizeHI=0;
	unsigned long long size=0;
	hr=insertedStorage->GetSize(&sizeLO,&sizeHI);
	if FAILED(hr){/*dont return if failed, size will simply be 0*/}
	size=sizeHI;
	size=(size<<32)+sizeLO;

	numStorageItems++;
	arrStorageItem plItem;
	plItem.level=level+1;
	plItem.pStorage3=insertedStorage3;
	plItem.pStorage3Parent=pDestStor;
	plItem.size=size;
	if(type==0){plItem.type=WMDM_FILE_ATTR_FILE;}else{plItem.type=WMDM_FILE_ATTR_FOLDER;}
	arrStorageItems[numStorageItems]=plItem;


	returnMsg("0\n");
}
IWMDMStorage3 * storageCreateFromFile_helper(char *destStorage,int *theLevel)
{	//searches for destStorage in the arrStorItems array. if it's not found, NULL is returned
	//the level of the destination is returned in varible level.  this is needed
	//for inserting into the arrStorItems array later.

	//first thing to do is parse the destStorage string
	char *item;
	char *attribs;
	int level;
	char *clevel;
	unsigned long type;  //DWORD
	char *ctype;
	char parent[MTPAXE_MAXFILENAMESIZE];
	char *name;

	CStrTok tokenizer[3]; //the tokenizer
	item=tokenizer[0].GetFirst(destStorage,"<");//remove the leading <

	//get the item attributes string
	attribs=tokenizer[1].GetFirst(item,">");

	//get each attribute from the attributes string
	clevel=tokenizer[2].GetFirst(attribs,"/");
	level=atoi(clevel);
	ctype=tokenizer[2].GetNext("/");
	type=strtoul(ctype,NULL,10);
	sprintf(parent,"%s",tokenizer[2].GetNext("/"));

	//get the item name
	name=tokenizer[1].GetNext(">");

	//string is now parsed

	//check if it's a folder
	if (!(type & WMDM_FILE_ATTR_FOLDER)){return NULL;}

	*theLevel=level;

	//search for this storage
	return findStorageFromPath(level,type,name);
}
/*TODO*/ 
void deviceGetSupportedFormats(void)
{	//gets the supported file/audio/video/playlist formats of the device
	//returns -1 on error, else a string containing the number which has
	//the formats (in a bitmask)

	if(m_pIdvMgr==NULL){returnMsg("-1\n","deviceGetSupportedFormats: DeviceManager not initialized\n");return;}
	if(pCurrDev==NULL){returnMsg("-1\n","deviceGetSupportedFormats: no active device is set\n");return;}

	////_WAVEFORMATEX *pAudioFormat;
	////unsigned int audioFormatCount;
	////_VIDEOINFOHEADER *pVideoFormat;
	////unsigned int videoFormatCount;
	////WMFILECAPABILITIES *pFileFormat;
	////unsigned int fileFormatCount;
	////LPWSTR *pMIMEFormats;
	////unsigned int mimeFormatCount;

	////hr=pCurrDev->GetFormatSupport2(WMDM_GET_FORMAT_SUPPORT_AUDIO |
	////							   WMDM_GET_FORMAT_SUPPORT_VIDEO |
	////							   WMDM_GET_FORMAT_SUPPORT_FILE,
	////							   &pAudioFormat,&audioFormatCount,
	////							   &pVideoFormat,&videoFormatCount,
	////							   &pFileFormat,&fileFormatCount);
	////hr=pCurrDev->GetFormatSupport(&pAudioFormat,&audioFormatCount,
	////							  &pMIMEFormats,&mimeFormatCount);
	//if FAILED(hr){returnMsg("-1\n","deviceGetSupportedFormats: could not get supported formats\n");return;}

	////loop through audio formats
	//for (int i=0;i<audioFormatCount;i++)
	//{	
	//	//PrintWaveFormatGuid(
	//	//pAudioFormat[0].wFormatTag 
	//}
	//char buffer[2510];
	//sprintf(buffer,"%d",hr);

	//strcat(buffer,"\n");
	//returnMsg(buffer);
}
void deviceGetSupportedFormats_helper(WMDM_FORMATCODE *fc,char *buffer)
{	//finds the string associated with formatCode and appends it to buffer
	if (*fc==WMDM_FORMATCODE_MP3) strcat(buffer,"WMDM_FORMATCODE_ALLIMAGES");
//WMDM_FORMATCODE_UNDEFINED  
//WMDM_FORMATCODE_ASSOCIATION 
//WMDM_FORMATCODE_SCRIPT  
//WMDM_FORMATCODE_EXECUTABLE 
//WMDM_FORMATCODE_TEXT  
//WMDM_FORMATCODE_HTML  
//WMDM_FORMATCODE_DPOF  
//WMDM_FORMATCODE_AIFF 
//WMDM_FORMATCODE_WAVE  
//WMDM_FORMATCODE_MP3  
//WMDM_FORMATCODE_AVI 
//WMDM_FORMATCODE_MPEG 
//WMDM_FORMATCODE_ASF 
//WMDM_FORMATCODE_RESERVED_FIRST  
//WMDM_FORMATCODE_RESERVED_LAST 
//WMDM_FORMATCODE_IMAGE_UNDEFINED  
//WMDM_FORMATCODE_IMAGE_EXIF  
//WMDM_FORMATCODE_IMAGE_TIFFEP  
//WMDM_FORMATCODE_IMAGE_FLASHPIX  
//WMDM_FORMATCODE_IMAGE_BMP  
//WMDM_FORMATCODE_IMAGE_CIFF 
//WMDM_FORMATCODE_IMAGE_GIF  
//WMDM_FORMATCODE_IMAGE_JFIF  
//WMDM_FORMATCODE_IMAGE_PCD  
//WMDM_FORMATCODE_IMAGE_PICT  
//WMDM_FORMATCODE_IMAGE_PNG  
//WMDM_FORMATCODE_IMAGE_TIFF  
//WMDM_FORMATCODE_IMAGE_TIFFIT  
//WMDM_FORMATCODE_IMAGE_JP2 
//WMDM_FORMATCODE_IMAGE_JPX 
//WMDM_FORMATCODE_IMAGE_RESERVED_FIRST 
//WMDM_FORMATCODE_IMAGE_RESERVED_LAST  
//WMDM_FORMATCODE_UNDEFINEDFIRMWARE  
//WMDM_FORMATCODE_WINDOWSIMAGEFORMAT 
//WMDM_FORMATCODE_UNDEFINEDAUDIO 
//WMDM_FORMATCODE_WMA  
//WMDM_FORMATCODE_OGG 
//WMDM_FORMATCODE_AAC 
//WMDM_FORMATCODE_AUDIBLE 
//WMDM_FORMATCODE_FLAC 
//WMDM_FORMATCODE_UNDEFINEDVIDEO  
//WMDM_FORMATCODE_WMV 
//WMDM_FORMATCODE_MP4 
//WMDM_FORMATCODE_MP2
//WMDM_FORMATCODE_UNDEFINEDCOLLECTION 
//WMDM_FORMATCODE_ABSTRACTMULTIMEDIAALBUM 
//WMDM_FORMATCODE_ABSTRACTIMAGEALBUM  
//WMDM_FORMATCODE_ABSTRACTAUDIOALBUM 
//WMDM_FORMATCODE_ABSTRACTVIDEOALBUM  
//WMDM_FORMATCODE_ABSTRACTAUDIOVIDEOPLAYLIST 
//WMDM_FORMATCODE_ABSTRACTCONTACTGROUP
//WMDM_FORMATCODE_ABSTRACTMESSAGEFOLDER  
//WMDM_FORMATCODE_ABSTRACTCHAPTEREDPRODUCTION 
//WMDM_FORMATCODE_WPLPLAYLIST  
//WMDM_FORMATCODE_M3UPLAYLIST  
//WMDM_FORMATCODE_MPLPLAYLIST  
//WMDM_FORMATCODE_ASXPLAYLIST  
//WMDM_FORMATCODE_PLSPLAYLIST  
//WMDM_FORMATCODE_UNDEFINEDDOCUMENT 
//WMDM_FORMATCODE_ABSTRACTDOCUMENT 
//WMDM_FORMATCODE_XMLDOCUMENT 
//WMDM_FORMATCODE_MICROSOFTWORDDOCUMENT
//WMDM_FORMATCODE_MHTCOMPILEDHTMLDOCUMENT 
//WMDM_FORMATCODE_MICROSOFTEXCELSPREADSHEET 
//WMDM_FORMATCODE_MICROSOFTPOWERPOINTDOCUMENT 
//WMDM_FORMATCODE_UNDEFINEDMESSAGE 
//WMDM_FORMATCODE_ABSTRACTMESSAGE  
//WMDM_FORMATCODE_UNDEFINEDCONTACT 
//WMDM_FORMATCODE_ABSTRACTCONTACT 
//WMDM_FORMATCODE_VCARD2  
//WMDM_FORMATCODE_VCARD3  
//WMDM_FORMATCODE_UNDEFINEDCALENDARITEM 
//WMDM_FORMATCODE_ABSTRACTCALENDARITEM 
//WMDM_FORMATCODE_VCALENDAR1
//WMDM_FORMATCODE_VCALENDAR2
//WMDM_FORMATCODE_UNDEFINEDWINDOWSEXECUTABLE 
//WMDM_FORMATCODE_MEDIA_CAST 
//WMDM_FORMATCODE_SECTION 
}
IWMDMDevice3 * findDevice(char* deviceName)
{	//returns a device object based on the provided device name. returns NULL if not found
	//the value is returned through the device pointer passed in

	int i;
	WCHAR cName[MTPAXE_MAXFILENAMESIZE];				//temp arrays for wide char conversions
	char ch[MTPAXE_MAXFILENAMESIZE*2];					//.	
	size_t ret;
	char name[MTPAXE_MAXFILENAMESIZE*2];					//array for string comparison
	IWMDMDevice3 *pDev=NULL;		//pointer to the device (if found)
	for(i=0;i<=numDevices;i++)
	{
		//get the name
		arrDevices[i]->GetName(cName,MTPAXE_MAXFILENAMESIZE);
		//convert to multibyte
		wcstombs_s(&ret, ch, MTPAXE_MAXFILENAMESIZE*2, cName,_TRUNCATE);
		sprintf(name,"%s",ch);
		if (strcmp(deviceName,name)==0)
		{
			pDev=arrDevices[i];
		}
	}

	return pDev;
}
IWMDMStorage3 * findStorageFromPath(int path,int type, char *name)
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
			IWMDMStorage3 *pStorage3=NULL;
			WCHAR storName[MTPAXE_MAXFILENAMESIZE];
			char buf[MTPAXE_MAXFILENAMESIZE*2];
			size_t retr;

			pStorage3=sItem.pStorage3;
			hr=pStorage3->GetName(storName,MTPAXE_MAXFILENAMESIZE);
			if SUCCEEDED(hr)//don't exit the function here on error since we want to keep looking, there could be another matching item later on
			{
				wcstombs_s(&retr, buf,MTPAXE_MAXFILENAMESIZE*2, storName,_TRUNCATE);

				if(strcmp(buf,name)==0)
				{	//we found the item
					return pStorage3;
				}
			}
		}
	}

	return NULL;
}

