/*****************************************************************************/
/* Triangle MicroWorks, Inc.                         Copyright (c) 1997-2011 */
/*****************************************************************************/
/*                                                                           */
/* This file is the property of:                                             */
/*                                                                           */
/*                       Triangle MicroWorks, Inc.                           */
/*                      Raleigh, North Carolina USA                          */
/*                       www.TriangleMicroWorks.com                          */
/*                          (919) 870-6615                                   */
/*                                                                           */
/* This Source Code and the associated Documentation contain proprietary     */
/* information of Triangle MicroWorks, Inc. and may not be copied or         */
/* distributed in any form without the written permission of Triangle        */
/* MicroWorks, Inc.  Copies of the source code may be made only for backup   */
/* purposes.                                                                 */
/*                                                                           */
/* Your License agreement may limit the installation of this source code to  */
/* specific products.  Before installing this source code on a new           */
/* application, check your license agreement to ensure it allows use on the  */
/* product in question.  Contact Triangle MicroWorks for information about   */
/* extending the number of products that may use this source code library or */
/* obtaining the newest revision.                                            */
/*                                                                           */
/*****************************************************************************/

/* file: WinTCPChannel.cpp
 * description: Implementation of Windows TCP I/O Target interface. This
 *  implementation uses two classes to provice a standard TCP/IP client
 *  server interface. The first class implements a single 'channel' where
 *  a channel consists of a specific IP address:port to IP address:port
 *  connection. An instance of this class is created for each channel
 *  in the source code library.
 *
 */
#include "stdafx.h"
#pragma warning(disable: 4100)

#include <vector>

#ifdef _DEBUG
#define _CRTDBG_MAP_ALLOC
#include <stdlib.h>
#include <crtdbg.h>
#endif

#pragma comment(lib, "Ws2_32.lib")

#include "WinIoTarg/include/WinIoTarg.h"
#include "WinIoTarg/include/WinIoTargDefs.h"
#include "WinIoTarg/WinIoInterface.h"
#include "WinIoTarg/WinTCPChannel.h" 

std::vector<WinTCPListener *> *WinTCPChannel::m_pWinTCPListenerList = WINIOTARG_NULL;
AutoCriticalSection WinTCPChannel::m_listenerListCrit;

#if TMW_SUPPORT_SSL 
#   pragma comment(lib,"libeay32.lib")
#   pragma comment(lib,"ssleay32.lib")


BIO *globalSslBioErr = TMWDEFS_NULL; 
 
static int password_cb(char *buf,int num, int rwflag, void *userdata)
{
  char *pPassword = (char *)userdata;
  if(num<(int)strlen(pPassword)+1)
    return(0);

  strcpy(buf,pPassword);
  return(strlen(pPassword));
}
 
void WinTCPChannel::load_dh_params(SSL_CTX *ctx,char *file) 
{
  DH *ret=0;
  BIO *bio;

  if ((bio=BIO_new_file(file,"r")) == NULL) 
    LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP SSL, Couldn't open DH file");
  else
    LogMessage(TMWDIAG_ID_TARGET, "TCP SSL, Opened DH file");

  ret = PEM_read_bio_DHparams(bio,NULL,NULL,NULL);

  BIO_free(bio);

  if(SSL_CTX_set_tmp_dh(ctx,ret)<0)
    LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP, SSL Couldn't set DH parameters");
  else
    LogMessage(TMWDIAG_ID_TARGET, "TCP SSL, Set DH parameters");
}
 
SSL_CTX *WinTCPChannel::initialize_ctx()
{ 
  if(!globalSslBioErr)
  {
    /* Global system initialization*/
    SSL_library_init();
    SSL_load_error_strings();
    
    /* An error write context */
    globalSslBioErr = BIO_new_fp(stderr, BIO_NOCLOSE);
    
    LogMessage(TMWDIAG_ID_TARGET, "TCP SSL, Initialized library");
  }

  /* If this has not yet been created */
  if(m_sslCtx == TMWDEFS_NULL)
  {
    /* Create our context*/ 
    m_sslCtx = SSL_CTX_new(SSLv23_method());
    if(m_sslCtx == TMWDEFS_NULL)
    {
      LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP SSL, Couldn't create a new context");
      return TMWDEFS_NULL;
    }
    else
    {
      LogMessage(TMWDIAG_ID_TARGET, "TCP SSL, Created context");
    }
  }

  return m_sslCtx;
}
      
void WinTCPChannel::destroy_ctx()
{
  if(m_sslCtx != TMWDEFS_NULL)
    SSL_CTX_free(m_sslCtx);

  m_sslCtx = TMWDEFS_NULL;
}

SSL_CTX *WinTCPChannel::tmw_initSsl()
{ 
  if(m_sslCtx == NULL)
  {
    HINSTANCE hinst = LoadLibrary("ssleay32.dll");
    if (hinst == NULL) 
    {
      WinIoTarg_formatErrorMsg(m_errorBuffer, WSAGetLastError());   
      LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP SSL, load ssleay32.dll failed\n%19s%s", " ",m_errorBuffer);
      return TMWDEFS_NULL;
    }
    hinst = LoadLibrary("libeay32.dll");
    if (hinst == NULL) 
    {
      WinIoTarg_formatErrorMsg(m_errorBuffer, WSAGetLastError()); 
      LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP SSL, load libeay32.dll failed\n%19s%s", " ",m_errorBuffer);
      return TMWDEFS_NULL;
    } 

    /* Build our SSL context*/ 
    m_sslCtx = initialize_ctx();
    if(m_sslCtx == TMWDEFS_NULL)
    {
      LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP SSL, initialize context failed");
      return TMWDEFS_NULL;
    }
    else
      LogMessage(TMWDIAG_ID_TARGET, "TCP SSL, Initialized Context");

    load_dh_params(m_sslCtx, DHFILE);
  }
  
  if(m_sslCtx != NULL)
  {
    /* Load our keys and certificates*/
    if(!m_sslCredFileOpen)
    {
      if(!(SSL_CTX_use_certificate_chain_file(m_sslCtx, m_config.sslCredFile)))
      { 
        LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP SSL, Can't read certificate file %s", m_config.sslCredFile);
      }
      else
      {
        m_sslCredFileOpen = TMWDEFS_TRUE;
        LogMessage(TMWDIAG_ID_TARGET, "TCP SSL, Read certificate file %s", m_config.sslCredFile);
      }
  
      //spm how safe is this to point into the config data?
      SSL_CTX_set_default_passwd_cb_userdata(m_sslCtx, m_config.sslPassword);
      SSL_CTX_set_default_passwd_cb(m_sslCtx, password_cb);
   
      if(!(SSL_CTX_use_PrivateKey_file(m_sslCtx, m_config.sslCredFile, SSL_FILETYPE_PEM)))
      { 
        LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP SSL, Can't read key file %s", m_config.sslCredFile);
      }
      else
      {
        LogMessage(TMWDIAG_ID_TARGET, "TCP SSL, Read key file %s", m_config.sslCredFile);
      }
    }

    /* Load the CAs we trust  */ 
    if(!m_sslCaListFileOpen)
    {
      if(!(SSL_CTX_load_verify_locations(m_sslCtx, m_config.sslCaListFile, 0)))
      { 
        LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP SSL, Can't read CA list %s", m_config.sslCaListFile);
      }
      else
      {
        m_sslCaListFileOpen = TMWDEFS_TRUE; 
        LogMessage(TMWDIAG_ID_TARGET, "TCP SSL, Read CA list %s", m_config.sslCaListFile);
      }
    }
  }
  return m_sslCtx;
}
 
bool WinTCPChannel::check_cert(SSL *ssl)
{ 
	X509 *peer;
  TMWTYPES_LONG retValue;
	char buf[512];
  
  retValue = SSL_get_verify_result(ssl);
  
	peer = SSL_get_peer_certificate(ssl);
	if (peer != NULL)
	{

	  LogMessage(TMWDIAG_ID_TARGET, "Certificate"); 
	  X509_NAME_oneline(X509_get_subject_name(peer), buf, sizeof buf);
	  LogMessage(TMWDIAG_ID_TARGET,"subject=%s", buf);
	  X509_NAME_oneline(X509_get_issuer_name(peer), buf, sizeof buf);
	  LogMessage(TMWDIAG_ID_TARGET, "issuer=%s", buf);
	  X509_free(peer);
	}

  if(retValue != X509_V_OK)
  { 
    LogMessage(TMWDIAG_ID_TARGET| TMWDIAG_ID_ERROR, "TCP SSL, Certificate doesn't verify, verify result %d\n %s", retValue, X509_verify_cert_error_string(retValue)); 

    if(m_config.sslVerifyCallback != TMWDEFS_NULL)
    {
      if(m_config.sslVerifyCallback(m_config.sslVerifyCallbackParam, ssl, retValue))
        return true;
    }
    return false;
  }
  return true;
}

int tmw_sslRecv(SSL *ssl, SOCKET commSocket, 
                TMWTYPES_UCHAR *pBuff, int maxNumChars)
{ 
  int r = SSL_read(ssl, pBuff, maxNumChars);  
  return r;
}

int WinTCPChannel::tmw_sslSend(SSL *ssl, SOCKET commSocket, 
                TMWTYPES_UCHAR *pBuff, int numCharsToSend)
{
  /* Try to write */
  int r=SSL_write(ssl, pBuff, numCharsToSend);
  int error = SSL_get_error(ssl,r);
  switch(error){
    /* We wrote something*/
    case SSL_ERROR_NONE:
      numCharsToSend-=r;
      pBuff+=r;
      break;
        
      /* We would have blocked */
    case SSL_ERROR_WANT_WRITE:
        LogMessage(TMWDIAG_ID_TARGET, "TCP, SSL want write");
      break;

      /* We get a WANT_READ if we're
         trying to rehandshake and we block on
         write during the current connection.
         
         We need to wait on the socket to be readable
         but reinitiate our write when it is */
    case SSL_ERROR_WANT_READ: 
        LogMessage(TMWDIAG_ID_TARGET, "TCP, SSL want read");
      break;
        
        /* Some other error */
    default:	      
        LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP, SSL write problem %d", error);
      break;
  }
  return r;
}
#endif


WinTCPIpAddressList::WinTCPIpAddressList()
{
  m_ipAddressList = new std::vector<char *>;
}

// Delete the list of addresses
WinTCPIpAddressList::~WinTCPIpAddressList()
{   
  IPAddressList::iterator Iter;
  while(m_ipAddressList->size() > 0)
  {
    Iter = m_ipAddressList->begin();
    char *ptr = *Iter;
    m_ipAddressList->erase(Iter);
    delete [] ptr;
  }

  delete m_ipAddressList;
}

static bool isValidIp4 (char *str) 
{ 
  int segs = 0;   /* Segment count. */ 
  int chcnt = 0;  /* Character count within segment. */ 
  int accum = 0;  /* Accumulator for segment. */ 

  /* Catch NULL pointer. */ 
  if (str == NULL) 
    return false; 

  /* Process every character in string. */ 
  while (*str != '\0') { 
    /* Segment changeover. */ 

    if (*str == '.') { 
      /* Must have some digits in segment. */ 
      if (chcnt == 0) 
        return false; 

      /* Limit number of segments. */ 
      if (++segs == 4) 
        return false; 

      /* Reset segment values and restart loop. */ 
      chcnt = accum = 0; 
      str++; 
      continue; 
    } 


    /* Check numeric. */ 
    if ((*str < '0') || (*str > '9')) 
      return false; 

    /* Accumulate and check segment. */ 
    if ((accum = accum * 10 + *str - '0') > 255) 
      return false; 

    /* Advance other segment specific stuff and continue loop. */ 
    chcnt++; 
    str++; 
  } 

  /* Check enough segments and enough characters in last segment. */ 
  if (segs != 3) 
    return false; 

  if (chcnt == 0) 
    return false; 

  /* Address okay. */ 
  return true; 
} 


void _convertNameToIpAddress(char *pName)
{
  // If this is not a valid IPV4 addr then try to look it up with DNS
  if(isValidIp4(pName) == false)
  {
    HOSTENT *h = gethostbyname(pName);
    if(h != NULL)
    {   
      struct in_addr *ip_addr = (struct in_addr *)h->h_addr_list[0];
      char *pConnectIPAddress = inet_ntoa(*ip_addr);
      strcpy(pName, pConnectIPAddress);
    }
    else
    { 
      WinIoInterface::ProtoAnaLog(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP: gethostbyname failed for host named %s",pName);
    }
  }
}

#define MAX_HOST_LEN 128
// Add this address to the list of allowed addresses.
bool WinTCPIpAddressList::Add(const char *pAddresses)
{
  int index; 
  char  *entryPtr =  new char[MAX_HOST_LEN];
  m_ipAddressList->push_back(entryPtr);
   
  index = 0;
  while( *pAddresses != 0x0 )
  {
    if(*pAddresses != ';' && *pAddresses != ',')
    {
      if(*pAddresses != ' ')
        entryPtr[index++] = *pAddresses;
    }
    else
    {
      // Null terminate completed address or name
      entryPtr[index++] = 0;

      entryPtr =  new char[MAX_HOST_LEN];
      m_ipAddressList->push_back(entryPtr);
      index = 0;
    }

    pAddresses++;

    if(index == (MAX_HOST_LEN-1))
      // Too long
      break;
  }

  entryPtr[index++] = 0;

  return true;
}

// Is this address in the list of allowed addresses? (*.*.*.* allows all addresses)
bool WinTCPIpAddressList::IsAddrConfigured(const char *pAddress)
{  
  //IPAddressList *v1 = m_ipAddressList;
  IPAddressList::iterator Iter;
  for ( Iter = m_ipAddressList->begin( ) ; Iter != m_ipAddressList->end( ) ; Iter++ )
  {
    char *ptr = (char *)*Iter;
    
    if(strcmp(ptr, "*.*.*.*") != 0) 
      _convertNameToIpAddress(ptr);

    if(strcmp(ptr, pAddress) == 0)
      return true;
  }
  return false;
}


char *WinTCPIpAddressList::GetFirstAddress()
{ 
  if(m_ipAddressList->size() > 0)
  {
    IPAddressList::iterator Iter; 
    Iter = m_ipAddressList->begin();
    char *ptr = (char *)*Iter;
    return (ptr);
  }
  else 
    return TMWDEFS_NULL;
}

// Constructor
WinTCPChannel::WinTCPChannel(const WINTCP_CONFIG &config, TMWTARG_CONFIG *pTmwTargConfig) : 
  m_commSocket(INVALID_SOCKET), m_clientSocket(INVALID_SOCKET), m_serverSocket(INVALID_SOCKET)
{
  m_bNewConnection = false;
  m_bForceReset = false;

  // Copy configuration data structure
  m_config = config;
  strncpy(m_config.ipAddress, config.ipAddress, WINIOTARG_STR_LEN);
  strncpy(m_config.chnlName, config.chnlName, WINIOTARG_STR_LEN);

  ipAddressList.Add(config.ipAddress);

  // Set channel name
  m_name = (m_config.chnlName) ? m_config.chnlName : m_config.ipAddress;

  // UDP related configuration 
  m_UDPthreadIsRunning    = false;
  m_UDPSocket             = INVALID_SOCKET;
  m_udpReaderThreadHandle = INVALID_HANDLE_VALUE;
  m_srcUDPPort            = WINTCP_UDP_PORT_NONE;
  m_validUDPAddress       = 0;
  m_bufferReadIndex       = 0;
  m_bufferWriteIndex      = 0;

#if TMW_SUPPORT_SSL
  m_sslCtx                = TMWDEFS_NULL;
  m_ssl                   = TMWDEFS_NULL;
  m_sslbio                = TMWDEFS_NULL;
  m_moreSSLData           = 0;
  m_sslCredFileOpen       = TMWDEFS_FALSE;
  m_sslCaListFileOpen     = TMWDEFS_FALSE;     
#endif

#ifdef TMW_USE_IO_COMPLETION_PORTS
  memset(&m_overlap,0,sizeof(WSAOVERLAPPED));
#endif
}

// Destructor
WinTCPChannel::~WinTCPChannel(void) 
{
  // Close channel if not already closed
  if (isChannelOpen())
    close();
  if(m_udpReaderThreadHandle == INVALID_HANDLE_VALUE)
    CloseHandle(m_udpReaderThreadHandle);

#if TMW_SUPPORT_SSL
  destroy_ctx();
#endif
}

// Is this address in the list of allowed addresses?
bool WinTCPChannel::IsAddrConfigured(const char *pConnectAddress)
{
  return (ipAddressList.IsAddrConfigured(pConnectAddress));
}

/**********************************************************************************\
	Function :			WinTCPChannel::setSocket
	Description : set the class members socket	
	Return :			void	-	
	Parameters :
			TMWTYPES_ULONG commSocket	-	
	Note : [none]
\**********************************************************************************/
void WinTCPChannel::setSocket(TMWTYPES_ULONG acceptSocket, bool isClient)
{
  if(isClient)
    m_clientSocket = acceptSocket;
  else
    m_serverSocket = acceptSocket;

  m_commSocket = acceptSocket;

  if(m_pChannelCallback != WINIOTARG_NULL)
    m_pChannelCallback(m_pCallbackParam, true, TMWDEFS_TARG_OC_SUCCESS);
}


// Return this channel's name
const char *WinTCPChannel::getName(void)
{
  return m_name;
}

// Return info about this channel
const char *WinTCPChannel::getInfo(void)
{ 
  _stprintf(m_info, "Port: %s:%d", m_config.ipAddress, m_config.ipPort);
  return m_info;
}

// Get current channel status
const char *WinTCPChannel::getStatus(void)
{
  if(isChannelOpen() == false)
  {
    _stprintf(m_status, "Closed");
  }
  else
  {
    _stprintf(m_status, "Open");
  }

  return(m_status);
}

/**********************************************************************************\
	Function :			WinTCPChannel::connect
	Description : Attempt to connect to a remote server
	Return :			bool	-	
	Parameters :
			void	-	
	Note : [none]
\**********************************************************************************/
bool WinTCPChannel::connect(void)
{
  SOCKET tempCommSocket;

  LogMessage(TMWDIAG_ID_TARGET, "TCP Opening connection");

  // if already connected, just return true;
  if(m_commSocket != INVALID_SOCKET)
  {
    LogMessage(TMWDIAG_ID_TARGET, "TCP Connection is in progress"); 
    return(true);
  }

  tempCommSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_IP);
  if(tempCommSocket == INVALID_SOCKET)
  {   
    WinIoTarg_formatErrorMsg(m_errorBuffer, WSAGetLastError());  
    LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP connect socket failed\n%19s%s", " ",m_errorBuffer);
    return(false);
  }

  /* Disable the Nagle algorithm to make sure packets are sent in a timely fashion */
  BOOL flag = TRUE;
  if(setsockopt(tempCommSocket, IPPROTO_TCP, TCP_NODELAY, (char *)&flag, sizeof(BOOL)) != 0)
  { 
    WinIoTarg_formatErrorMsg(m_errorBuffer, WSAGetLastError());   
    LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP Connect setsockopt failed\n%19s%s", " ",m_errorBuffer);
    return(false);
  }
 
  SOCKADDR_IN sockAddr;
  memset(&sockAddr, 0, sizeof(sockAddr));

  char *ipAddress = ipAddressList.GetFirstAddress();
  _convertNameToIpAddress(ipAddress);
  TMWTYPES_ULONG ulongAddress = inet_addr(ipAddress);
  if(ulongAddress == INADDR_NONE)
  {
    LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP Connect inet_addr failed");
    closesocket(tempCommSocket);
    return(false);
  }

  sockAddr.sin_family = AF_INET;
  if(isClient())
  {
    sockAddr.sin_port = htons(m_config.ipPort);
  }

  // For DNP Networking support dual end point.
  // ipPort would be used for listening on, not connecting to.
  else if(isDualEndPoint())
  {
    sockAddr.sin_port = htons(m_config.dualEndPointIpPort);
  }

  sockAddr.sin_addr.s_addr = ulongAddress;

  TMWTYPES_ULONG blockmode = 1;  // enable non-blocking
  if(ioctlsocket(tempCommSocket, FIONBIO, &blockmode) == SOCKET_ERROR)
  {
    WinIoTarg_formatErrorMsg(m_errorBuffer, WSAGetLastError());    
    LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP Connect NONBLOCKING ioctl failed\n%19s%s", " ",m_errorBuffer);
    closesocket(tempCommSocket);
    return(false);
  }

  ///* perform ARP here, if 10+ connections are being made to IP addresses that cannot 
  // * be resolved, sometimes connections can't be made to the good IP addresses. Either SYN requests
  // * are not sent, or SYN ACK is not seen by select(). Could be some resource problem in the TCP stack.
  // * There was also a security update to windows to limit the number of "open connections that are not 
  // * yet established" Calling SendARP and sleeping when arp fails instead of calling connect() seems 
  // * to solve the problem.
  // */
  //if(strcmp(m_config.ipAddress, "127.0.0.1"))
  //{
  //  ULONG   pulMac[2];
  //  ULONG   ulLen;
  //  IPAddr  ipAddr; 
  //  ipAddr = inet_addr (m_config.ipAddress);
  //  memset (pulMac, 0xff, sizeof (pulMac));
  //  ulLen = 6;
  //  if(SendARP (ipAddr, 0, pulMac, &ulLen) !=0)
  //  {
  //    LogMessage(TMWDIAG_ID_TARGET, "SendArp failed ");
  //    closesocket(tempCommSocket);
  //    Sleep(m_config.ipConnectTimeout);
  //    return(false);
  //  }
  //}
#if 0
  /* perform PING here, if 10+ connections are being made to IP addresses that cannot 
   * be resolved, sometimes connections can't be made to the good IP addresses. Either SYN requests
   * are not sent, or SYN ACK is not seen by select(). Could be some resource problem in the TCP stack.
   * There was also a security update to windows to limit the number of "open connections that are not 
   * yet established" Calling Ping instead of calling connect() seems to solve the problem.
   */
  if(strcmp(m_config.ipAddress, "127.0.0.1"))
  {
    WinIoPing pinger;
    
    if(pinger.Ping(2,m_config.ipConnectTimeout,m_config.ipAddress) != true)
    {
      LogMessage(TMWDIAG_ID_TARGET, "Ping failed ");
      closesocket(tempCommSocket);
      return(false);
    }
  }
#endif
    
  /* This code will force the client to use a particular IP Address and/or port for
     the outgoing connection (1700 commented out in this example) */
  SOCKADDR_IN sockAddr1;
  memset(&sockAddr1,0,sizeof(sockAddr));
  sockAddr1.sin_family = AF_INET;
  sockAddr1.sin_port = htons(0); /*htons(1700);*/
  sockAddr1.sin_addr.s_addr = inet_addr(m_config.localIpAddress);

  if(bind(tempCommSocket, (struct sockaddr *) &sockAddr1, sizeof(sockAddr1)) != 0)
  { 
    WinIoTarg_formatErrorMsg(m_errorBuffer, WSAGetLastError());     
    LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP Bind failed on local IP Address\n%19s%s", " ",m_errorBuffer);
    /* Don't closesocket and return, just let it use whatever address it can */
  } 

  /* This function should not block since ioctlsocket() has been called */
  int result = ::connect(tempCommSocket, (struct sockaddr *)&sockAddr, sizeof(sockAddr));

  /* A non blocking connect should always return SOCKET_ERROR */
  if(result != SOCKET_ERROR)
  { 
    WinIoTarg_formatErrorMsg(m_errorBuffer, WSAGetLastError());     
    LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP Connect failed\n%19s%s", " ",m_errorBuffer);
    closesocket(tempCommSocket);
    return(false);
  }

  /* Error code should be would block */
  DWORD last_error = WSAGetLastError();
  if(last_error != WSAEWOULDBLOCK)
  {
    WinIoTarg_formatErrorMsg(m_errorBuffer, last_error);      
    LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR,  "TCP Connect failed\n%19s%s", " ",m_errorBuffer);
    closesocket(tempCommSocket);
    return(false);
  }

  /* Use select to wait for the connection to be accepted */
  fd_set writefds;
  FD_ZERO(&writefds);
#pragma warning(disable:4127)
  FD_SET(tempCommSocket, &writefds);
#pragma warning(default:4127)
  struct timeval timeout = {0, m_config.ipConnectTimeout * 1000};
  result = select(0, WINIOTARG_NULL, &writefds, WINIOTARG_NULL, &timeout);

  /* If error we did not connect */
  if(result == SOCKET_ERROR)
  { 
    WinIoTarg_formatErrorMsg(m_errorBuffer, WSAGetLastError());      
    LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP Connect select failed\n%19s%s", " ",m_errorBuffer);
    closesocket(tempCommSocket);
    return(false);
  }

  /* If no error, see if activity on the correct file descriptor */
  if(!FD_ISSET(tempCommSocket, &writefds))
  {
    LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP Connect failed, no activity on socket");
    closesocket(tempCommSocket);
    return(false);
  }

  // For DNP Networking, save the address 
  if(m_config.validateUDPAddress)
  {
    m_validUDPAddress = ulongAddress;
  }

#if TMW_SUPPORT_SSL
  if(m_config.useSSL)
  {
    if(m_sslCtx == TMWDEFS_NULL)
    { 
      LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP SSL, connect error, ssl context == NULL");
      closesocket(tempCommSocket);
      return(false);
    }

    /* Connect the SSL socket */
    m_ssl = SSL_new(m_sslCtx);
    m_sslbio = BIO_new_socket(tempCommSocket, BIO_NOCLOSE);
    SSL_set_bio(m_ssl, m_sslbio, m_sslbio);
    
    int retCode;
    int r = 0;
    int loopCount = 0;
    while(r <=0)
    {
      r = SSL_connect(m_ssl);
      if(r<=0)
      {
        retCode = SSL_get_error(m_ssl, r);
        if(retCode != 2)
        {
          LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP SSL, connect error, %d, %d", r, retCode);
          SSL_shutdown(m_ssl);
          SSL_free(m_ssl); 
          m_ssl = TMWDEFS_NULL;
          closesocket(tempCommSocket);
          return(false);
        }
        Sleep(100);
        if(loopCount++ > 40)
        {
          LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP SSL, connect error, handshake failure");
          closesocket(tempCommSocket);
          return(false);
        }
      }
    } 
    bool retVal = check_cert(m_ssl);
    if(!retVal)
    { 
      LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP SSL, connect error, certification failure");
      closesocket(tempCommSocket);
      return(false);
    }
  }
#endif

  // Tell the channel we have a new socket (connection)
  setSocket(tempCommSocket, true);

  /* Return success */
  LogMessage(TMWDIAG_ID_TARGET, "TCP Connect success");
  return(true);
}

/**********************************************************************************\
	Function :			WinTCPChannel::listen
	Description : start waiting for incoming connection
	Return :			bool	-	
	Parameters :
			void	-	
	Note : [none]
\**********************************************************************************/
bool WinTCPChannel::listen(void)
{
  CriticalSectionLock lock(getListenerListLock());
  bool status = false;
  // create the listen list if it does not exist
  if (m_pWinTCPListenerList == WINIOTARG_NULL)
  {
    m_pWinTCPListenerList = new std::vector<WinTCPListener *>;
  }

  LogMessage(TMWDIAG_ID_TARGET, "TCP listen for a connection");

  /* search listening threads for a thread that is listening 
   * for a connection on this same port  
   */
  WinTCPListener *pWinTCPListener = WINIOTARG_NULL;
  bool foundListener = false;
  for(unsigned int i = 0; i < getWinTCPListenerList()->size(); i++)
  {
    pWinTCPListener = getWinTCPListenerList()->at(i);
    if(pWinTCPListener->isSamePort(m_config.ipPort))
    {
      LogMessage(TMWDIAG_ID_TARGET, "TCP listen, found an existing Listener to use");
      foundListener = true;
      break;
    }
  }

  if(!foundListener)
  {
    LogMessage(TMWDIAG_ID_TARGET,  "TCP listen, no existing Listener found, creating one");
    pWinTCPListener = new WinTCPListener(m_config.ipPort);
  }

  if(pWinTCPListener != WINIOTARG_NULL)
  {
    LogMessage(TMWDIAG_ID_TARGET, "TCP Listen, add this channel to the listener");
    status = pWinTCPListener->addChannelToListener(this);
  }

  if(status)
    LogMessage(TMWDIAG_ID_TARGET, "TCP Listen, successfully listening");
  else
    LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR,  "TCP Listen, failed");

  return(status);
}

/**********************************************************************************\
	Function :			WinTCPChannel::setChannelCallback
	Description : [none]	
	Return :			void	-	
	Parameters :
			  TMWTARG_CHANNEL_CALLBACK_FUNC pChannelCallback	-	
			  void *pCallbackParam	-	
	Note : [none]
\**********************************************************************************/
void WinTCPChannel::setChannelCallback(
  TMWTARG_CHANNEL_CALLBACK_FUNC pChannelCallback, 
  void *pCallbackParam)
{
  m_pChannelCallback = pChannelCallback;
  m_pCallbackParam = pCallbackParam;
}

/**********************************************************************************\
	Function :			WinTCPChannel::open
	Description : [none]	
	Return :			bool	-	
	Parameters :
			void	-	
	Note : [none]
\**********************************************************************************/
bool WinTCPChannel::open(void)
{
  bool status = false;

  LogMessage(TMWDIAG_ID_TARGET, "TCP open");

#if TMW_SUPPORT_SSL
  if(m_config.useSSL)
  {
    m_sslCtx = tmw_initSsl();
    if(m_sslCtx == TMWDEFS_NULL)
      return false;
  }
#endif

  // Special logic to handle Dual End Point and UDP Only for DNP is required.
 
  // If configured to use SSL, don't allow the use of UDP 
  if(!m_config.useSSL)
  {
    // For DNP normally, both master and outstation should always support UDP
    // But for DNP and especially the other protocols check config to see if 
    // user wants it.
    if(m_config.localUDPPort != WINTCP_UDP_PORT_NONE)
    {
      // If UDP has not been opened yet
      if(m_UDPSocket == INVALID_SOCKET)
      {
        // If configured for UDP only, don't use broadcast address,
        // Return success or failure
        if(m_config.mode == WINTCP_MODE_UDP)
        {
          char *ipAddress = ipAddressList.GetFirstAddress();
          _convertNameToIpAddress(ipAddress); 
          status = udpEndPoint(ipAddress);
          if(m_config.validateUDPAddress)
          {
            m_validUDPAddress = inet_addr(ipAddress);
          }
          return(status);
        }
        else
        {
          // This is tolerant of failure, if TCP AND UDP. may want to catch this
          // The problem is both master and slave use 20000 by default so it often fails
          // when testing in loopback.
          status = udpEndPoint(m_config.udpBroadcastAddress);
        }
      }
    }
  }

  if(isServer())
  {
    // Setup listener if not already listening
    status = listen();

    // If listener is running see if socket is valid
    if(status)
    {
      if(m_serverSocket == INVALID_SOCKET)
      {
        status = false;
      }
    }
  }
  else if(isClient())
  {
    status = connect();
  }
  else if(isDualEndPoint())
  {
    WinIoConnector *pIoConnector = getIoConnector();
    if(pIoConnector->isRunning())
      status = connect();
    else
    {
      // Setup listener if not already listening
      // Tell library it is connected,
      // so that SCL will attempt to send data when needed
      // which will cause a TCP connect request to be attempted.
      status = listen();
    }
  }

  return(status);
}

/**********************************************************************************\
	Function :			WinTCPChannel::close
	Description : [none]	
	Return :			void	-	
	Parameters :
			void	-	
	Note : [none]
\**********************************************************************************/
void WinTCPChannel::close(void) 
{
  {
    CriticalSectionLock lock(getListenerListLock());
    /* If we're a server, quit listening for connections */
    if(isServer() ||isDualEndPoint())
    {
      LogMessage(TMWDIAG_ID_TARGET, "TCP close");
      if (m_pWinTCPListenerList)
      {
        for(unsigned int i = 0; i < getWinTCPListenerList()->size(); i++)
        {
          WinTCPListener *pWinTCPListener = getWinTCPListenerList()->at(i);
          if(pWinTCPListener->isSamePort(m_config.ipPort))
          {
            pWinTCPListener->removeChannelFromListener(this);
              
            // If this is the listener thread running, don't try to terminate it 
            DWORD threadId = GetCurrentThreadId();
            if(threadId != pWinTCPListener->getThreadId())
            {
              // call method to see if channel list is empty.
              if(pWinTCPListener->getChannelListSize() == 0)
              {
                // if so wait for thread to end
                DWORD status = WaitForSingleObject(pWinTCPListener->m_listenThreadHandle, 5000);
                if (status == WAIT_TIMEOUT)
                {     
                  WinIoTarg_endThread(pWinTCPListener->m_listenThreadHandle);
                  pWinTCPListener->forceClose();

                  LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP close, listen thread terminated and forced close");
                }
                // destructor removes listener from listener list.
                delete pWinTCPListener;
              }
            }
            break;
          }
        }
      }
    }

    if (m_pWinTCPListenerList)
    {
      if (getWinTCPListenerList()->size() == 0)
      {
        LogMessage(TMWDIAG_ID_TARGET, "TCP close, no more channels listening for this port, delete Listener");
        delete m_pWinTCPListenerList;
        m_pWinTCPListenerList = WINIOTARG_NULL;
      }
    }
  }

  StopUDPReaderThread();
 
  
#if TMW_SUPPORT_SSL
  if(m_ssl != TMWDEFS_NULL)
  { 
    int r=SSL_shutdown(m_ssl);
    if(!r){
      /* If we called SSL_shutdown() first then
         we always get return value of '0'. In
         this case, try again, but first send a
         TCP FIN to trigger the other side's
         close_notify*/
      if(m_clientSocket != INVALID_SOCKET)
        shutdown(m_clientSocket, 1);
      if(m_serverSocket != INVALID_SOCKET)
        shutdown(m_serverSocket, 1);

      r=SSL_shutdown(m_ssl);
    } 

    SSL_free(m_ssl); 
    m_ssl = TMWDEFS_NULL;
  }
#endif

  if(m_UDPSocket != INVALID_SOCKET)
  {
    LogMessage(TMWDIAG_ID_TARGET, "TCP close UDP socket");
    closesocket(m_UDPSocket); 
    m_UDPSocket = INVALID_SOCKET;
  }

  if(m_clientSocket != INVALID_SOCKET)
  {
    LogMessage(TMWDIAG_ID_TARGET, "TCP close Client socket");
    closesocket(m_clientSocket);
    m_clientSocket = INVALID_SOCKET;
  }

  if(m_serverSocket != INVALID_SOCKET)
  {
    LogMessage(TMWDIAG_ID_TARGET, "TCP close Server socket");
    closesocket(m_serverSocket); 
    m_serverSocket = INVALID_SOCKET;
  }
  m_commSocket = INVALID_SOCKET;

  m_validUDPAddress = 0;
}

/**********************************************************************************\
	Function :			WinTCPChannel::lowReceive
	Description : Low level receive method	
	Return :			TMWTYPES_USHORT	-	
	Parameters :
			  TMWTYPES_UCHAR *pBuff	-	
			  TMWTYPES_ULONG maxNumChars	-	
			  bool peekOnly	-	
	Note : [none]
\**********************************************************************************/
TMWTYPES_USHORT WinTCPChannel::lowReceive(
  TMWTYPES_UCHAR *pBuff, 
  TMWTYPES_ULONG maxNumChars, 
  bool peekOnly) 
{
  TMWTYPES_USHORT numReceived = 0;

  fd_set readfds;

  if(m_commSocket == INVALID_SOCKET)
  {
    return(0);
  }

  if(m_bForceReset == true)
  {
    m_bForceReset = false;
    if(m_pChannelCallback != WINIOTARG_NULL)
      m_pChannelCallback(m_pCallbackParam, false, TMWDEFS_TARG_OC_RESET);
  } 

  FD_ZERO(&readfds);
#pragma warning(disable:4127)
  FD_SET(m_commSocket,&readfds);
#pragma warning(default:4127)
  struct timeval timeout = {0,0};
 
#if TMW_SUPPORT_SSL
  if(m_config.useSSL && m_moreSSLData)
  {
    numReceived = (TMWTYPES_USHORT)tmw_sslRecv(m_ssl, m_commSocket, pBuff, maxNumChars); 
    m_moreSSLData = SSL_pending(m_ssl); 
    return numReceived;
  } 
#endif

  // Call select with timeout of 0 to see if data is available
  if(select(0, &readfds, WINIOTARG_NULL,WINIOTARG_NULL, &timeout) == SOCKET_ERROR)
  {
    if(m_pChannelCallback != WINIOTARG_NULL)
      m_pChannelCallback(m_pCallbackParam, false,TMWDEFS_TARG_OC_FAILURE); 
    else
      close();
  }
  else if(FD_ISSET(m_commSocket, &readfds))
  {
    int returnValue;
#ifdef TMW_USE_IO_COMPLETION_PORTS
      DWORD dwFlags = peekOnly ? MSG_PEEK:0;
      int retVal;
      WSABUF wsaBuf;
      wsaBuf.len = maxNumChars;
      wsaBuf.buf = (char*)pBuff;
      DWORD nRecvdBytes;
      retVal = WSARecv(m_commSocket, &wsaBuf, 1, &nRecvdBytes, &dwFlags ,&m_overlap, NULL); 
      if (retVal != SOCKET_ERROR)
      {
        returnValue = nRecvdBytes;
      }
      else
      {
        returnValue = retVal;
      }
#else
#if TMW_SUPPORT_SSL
    if(!m_config.useSSL)
      returnValue = recv(m_commSocket, (char*)pBuff, maxNumChars, peekOnly ? MSG_PEEK:0); 
    else
    {
      returnValue = tmw_sslRecv(m_ssl, m_commSocket, pBuff, maxNumChars);       
      m_moreSSLData = SSL_pending(m_ssl);   
    }
#else
    returnValue = recv(m_commSocket, (char*)pBuff, maxNumChars, peekOnly ? MSG_PEEK:0); 
#endif
#endif

    if(returnValue == SOCKET_ERROR)
    {
      if(m_pChannelCallback != WINIOTARG_NULL)
        m_pChannelCallback(m_pCallbackParam, false, TMWDEFS_TARG_OC_FAILURE);

      else
        close();
      /* DWORD last_error = WSAGetLastError(); */
    } 
    else if(returnValue == 0) 
    {
      if(m_pChannelCallback != WINIOTARG_NULL)
        m_pChannelCallback(m_pCallbackParam, false, TMWDEFS_TARG_OC_FAILURE);
      else
        close();
    }
    else
    {
      numReceived = (TMWTYPES_USHORT)returnValue;
    }
  }

  return(numReceived);
}

/**********************************************************************************\
	Function :			WinTCPChannel::receive
	Description : Receive bytes from channel, called from SCL
	Return :			TMWTYPES_USHORT	-	
	Parameters :
			  TMWTYPES_UCHAR  *pBuff	-	
			  TMWTYPES_ULONG  maxNumChars	-	
	Note : [none]
\**********************************************************************************/
TMWTYPES_USHORT WinTCPChannel::receive(
  TMWTYPES_UCHAR  *pBuff, 
  TMWTYPES_ULONG  maxNumChars)
{
  TMWTYPES_USHORT numReceived;
 
  // first see if there are any bytes received on the UDP socket 
  // Currently only for DNP Networking.
  if(m_config.localUDPPort != WINTCP_UDP_PORT_NONE)
  {
    numReceived = UDPReceive(pBuff, maxNumChars);
    if(numReceived > 0)
    {
      return(numReceived);
    }
  }

  // Now check TCP socket 
  if (m_bNewConnection == true)
  {
    m_bNewConnection = false;
    if(m_pChannelCallback != WINIOTARG_NULL)
      m_pChannelCallback(m_pCallbackParam, false, TMWDEFS_TARG_OC_NEW_CONNECTION);
    else
      close();
    numReceived = 0;
  }
  else
  {
    numReceived = lowReceive(pBuff, maxNumChars, false);
  }
  return(numReceived);
}

/**********************************************************************************\
	Function :			WinTCPChannel::isTransmitReady
	Description : Are we ready to transmit?
	Return :			 
	Parameters :
			 
	Note : [none]
\**********************************************************************************/
TMWTYPES_MILLISECONDS WinTCPChannel::isTransmitReady(void)
{
  // We are ready as long as the remote end has not closed
  // so peek to see if socket is still open

  // There is a problem calling lowReceive. The select may say
  // there was something to read, but the recv would return 0
  // This would cause the callback function to be called, which
  // would recurse through here until the stack overflowed.
  // unsigned char buffer;
  // lowReceive(&buffer, sizeof(buffer), true);

  // If connection is not open and this is a dual end point system
  // attempt to make the TCP connection. 
  // In any case if connection is not open return some delay.
  // If connected return no delay.
  if(!isChannelOpen())
  {
    if(isDualEndPoint())
    {
      WinIoConnector *pIoConnector = getIoConnector();
      // If connector thread is not running, start the connector thread
      if (pIoConnector->isRunning() == false)
      {
        pIoConnector->StartConnectorThread(this,0);
      }
      return(3000);
    }
    return(500);
  }
  return(0);
}

// Transmit bytes to this TCP connection 
bool WinTCPChannel::transmit(
  const TMWTYPES_UCHAR *pBufferToSend, 
  TMWTYPES_USHORT numCharsToSend)
{
  bool success = false;

  // synchronize with connection thread so it doesn't
  // connect or disconnect while we try to receive data
  if(isChannelOpen())
  {
    LogMessage(TMWDIAG_ID_TARGET, "TCP transmit %d bytes", numCharsToSend);

#if TMW_SUPPORT_SSL
    int returnValue;
    if(!m_config.useSSL)
       returnValue = send(m_commSocket, (char*)pBufferToSend, numCharsToSend, 0);
    else 
    {
      returnValue = tmw_sslSend(m_ssl, m_commSocket, (TMWTYPES_UCHAR *)pBufferToSend, numCharsToSend);
      if(returnValue != numCharsToSend)
      {
        LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "TCP transmit ssl, not all bytes were transmitted %d %d", numCharsToSend, returnValue);
      }
    }
#else
    int returnValue = send(m_commSocket, (char*)pBufferToSend, numCharsToSend, 0);
#endif
   
    if(returnValue == SOCKET_ERROR)
    {
      if(m_pChannelCallback != WINIOTARG_NULL)
        m_pChannelCallback(m_pCallbackParam, false,TMWDEFS_TARG_OC_FAILURE); 
      else
        close();
    } 
    else
    {
      success = true;
    }
  }

  return(success);
}

/* function: Create */
WinTCPChannel *WinTCPChannel::Create(
  const void *pConfig, 
  TMWTARG_CONFIG *pTmwTargConfig)
{
  WINTCP_CONFIG *pTCPConfig = (WINTCP_CONFIG *)pConfig;
  WinTCPChannel *pChannel = new WinTCPChannel(*pTCPConfig, pTmwTargConfig);
  pChannel->setChannelCallback(pTmwTargConfig->pChannelCallback, pTmwTargConfig->pCallbackParam);
  pChannel->m_pChannel = pTmwTargConfig->pChannel;
  return pChannel;
}

/* function: wintcp_deleteChannel */
void WinTCPChannel::deleteWinIoChannel()
{
  closeWinIoChannel();
}

bool WinTCPChannel::modifyWinIoChannel(const void *pUserConfig)
{
  WINIO_CONFIG *tcpUserConfig = (WINIO_CONFIG *)pUserConfig;
  if (tcpUserConfig->winTCP.chnlName != NULL)
  {
    strncpy(m_config.chnlName, tcpUserConfig->winTCP.chnlName, WINIOTARG_STR_LEN);
  }
  return false;
}

/* function: wintcp_openChannel */
bool WinTCPChannel::openWinIoChannel(
   TMWTARG_CHANNEL_RECEIVE_CBK_FUNC pCallbackFunc, 
   TMWTARG_CHECK_ADDRESS_FUNC pCheckAddrCallbackFunc, 
   void *pCallbackParam, WINIO_OPEN_MODE_ENUM openMode)
{
  m_pRecvDataFunc = pCallbackFunc;
  m_pCheckAddrFunc = pCheckAddrCallbackFunc;
  m_pChanContextCBData = pCallbackParam;
  return(open());
}

/* function: wintcp_closeChannel */
void WinTCPChannel::closeWinIoChannel()
{
  close();
}

/* function: wintcp_closeChannel */
bool WinTCPChannel::resetWinIoChannel()
{
  m_bForceReset = true;
  return true;
}

/* function: wintcp_getChannelName */
const char *WinTCPChannel::getChannelName()
{
  return getName();
}

/* function: wintcp_getChannelInfo */
const char *WinTCPChannel::getChannelInfo()
{
  return getInfo();
}

// Returns true if this channel is currently open
// isChannelOpen will not call lowreceive because isTransmitReady calls it.
// lowReceive should not be called by the listenThread or it may
// call back to main thread.
bool WinTCPChannel::isChannelOpen(void)
{  
  if(m_config.mode == WINTCP_MODE_UDP)
    return(m_UDPSocket != INVALID_SOCKET); 

  return(m_commSocket != INVALID_SOCKET); 
}

/* function: wintcp_getChannelStatus */
const char *WinTCPChannel::getChannelStatus()
{
  return getStatus();
}

/* function: wintcp_getTransmitReady */
TMWTYPES_MILLISECONDS WinTCPChannel::getTransmitReady()
{
  return isTransmitReady();
}

/* function: wintcp_receive */
TMWTYPES_USHORT WinTCPChannel::receiveOnChannel(
  TMWTYPES_UCHAR *pBuff, 
  TMWTYPES_USHORT maxBytes, 
  TMWTYPES_MILLISECONDS maxTimeout,
  bool  *timeoutOccured)
{
  *timeoutOccured = false;
  return receive(pBuff, maxBytes);
}

/* function: wintcp_transmit */
bool WinTCPChannel::transmitOnChannel(
  TMWTYPES_UCHAR *buf, 
  TMWTYPES_USHORT numBytes)
{
  return transmit(buf, numBytes);
}


/* The rest of this file was added to support DNP3 Spec IP Networking */

/**********************************************************************************\
	Function :			WinTCPChannel::udpEndPoint
	Description : Attempt to open a socket to be used for UDP send/receive 
        To support DNP3 Spec IP Networking
	Return :			bool	-	
	Parameters :
			void	-	
	Note : [none]
\**********************************************************************************/
bool WinTCPChannel::udpEndPoint(char *pIpAddressString)
{
  SOCKET tempCommSocket;
  SOCKADDR_IN my_addr;

  LogMessage(TMWDIAG_ID_TARGET, "UDP: Opening UDP End Point");

  // See if this UDP end point is already open 
  if(m_UDPSocket != INVALID_SOCKET)
  {
    LogMessage(TMWDIAG_ID_TARGET, "UDP: End Point already open");
    return(true);
  }
  memset(&m_destSockAddr, 0, sizeof(m_destSockAddr));

  TMWTYPES_ULONG ulongAddress;

  if(strchr(pIpAddressString, '.') == 0)
  {
    HOSTENT *h = gethostbyname(pIpAddressString);

    if(h == NULL)
    {
      LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "UDP: failed to resolve host name: %s", pIpAddressString);
      return(false);
    }
    
    struct in_addr *ip_addr = (struct in_addr *)h->h_addr_list[0];

    /* Eg. 211.40.35.76 split up like this. */
    int a = ip_addr->S_un.S_un_b.s_b1;  /* 211 */
    int b = ip_addr->S_un.S_un_b.s_b2;  /* 40 */
    int c = ip_addr->S_un.S_un_b.s_b3;  /* 35 */
    int d = ip_addr->S_un.S_un_b.s_b4;  /* 76 */

    sprintf(pIpAddressString, "%d.%d.%d.%d", a, b, c, d);
    LogMessage(TMWDIAG_ID_TARGET , "UDP: Resolved host name to IP Address: %s", pIpAddressString);
  }

  ulongAddress = inet_addr(pIpAddressString);

  if(ulongAddress == INADDR_NONE)
  {
    /* We don't have to specify the ip address of the remote system
     * for UDP and TCP 
     */
    ulongAddress = INADDR_ANY;
  }

  /* Let UDPAddress for validation get set when connection is made */
  if(m_config.validateUDPAddress)
    m_validUDPAddress = INADDR_NONE; 

  m_destSockAddr.sin_family = AF_INET;
  m_destSockAddr.sin_port = htons(m_config.destUDPPort);
  m_destSockAddr.sin_addr.s_addr = ulongAddress;


  /* Now set up UDP socket to send and receive on. */
  tempCommSocket = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
  if(tempCommSocket < 0)
  {    
    LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "UDP failed to open socket");
    return(false);
  }

  // success,  bind to configured local UDP port.
  my_addr.sin_family = AF_INET; 
  my_addr.sin_addr.s_addr = inet_addr(m_config.localIpAddress);
  if(m_config.localUDPPort == WINTCP_UDP_PORT_ANY)
  {
    //zero lets bind choose an available port number
    my_addr.sin_port = 0;
  }
  else
  {
    my_addr.sin_port = htons(m_config.localUDPPort);
  }

  // bind to assign a port to the socket 
  if (bind(tempCommSocket, (struct sockaddr *)&my_addr, sizeof(my_addr)) < 0) 
  {
    char m_errorBuffer[128];
    WinIoTarg_formatErrorMsg(m_errorBuffer, WSAGetLastError()); 
    WinIoInterface::ProtoAnaLog(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "UDP failed to bind to port %d, %s", htons(my_addr.sin_port), m_errorBuffer);
    closesocket(tempCommSocket);
    return(false);
  }
   
  /*This is not required for enabling broadcast
   * BOOL flag = TRUE;
   * if(setsockopt(tempCommSocket, SOL_SOCKET, SO_BROADCAST, (char *)&flag, sizeof(BOOL)) != 0)
   * { 
   *  WinIoTarg_formatErrorMsg(m_errorBuffer, WSAGetLastError());   
   *  LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "UDP setsockopt failed\n%19s%s", " ",m_errorBuffer);
   *  return(false);
   * }
   */

  // Store new socket to be used for UDP
  m_UDPSocket = tempCommSocket;

  if(!StartUDPReaderThread())
  {
    closesocket(m_UDPSocket);
    m_UDPSocket = INVALID_SOCKET;
    LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "UDP failed to start UDP reader thread");
    return(false);
  }
 
  // Return success 
  LogMessage(TMWDIAG_ID_TARGET, "UDP Open End Point returned success");
  return(true);
}

/**********************************************************************************\
	Function :			WinTCPChannel::inputData
	Description : Put data from datagram into circular buffer, for reading by SCL.
        To support DNP3 Spec IP Networking 
	Return :			void	-	
	Parameters :
	Note : [none]
\**********************************************************************************/
void WinTCPChannel::inputData(TMWTYPES_UCHAR  *buf, TMWTYPES_USHORT length)
{
  int roomLeft = WINTCP_BUFFER_SIZE - m_bufferWriteIndex;

  /* Is there enough room at end of buffer?  */
  if(roomLeft >= length)
  {
    memcpy(&m_buffer[m_bufferWriteIndex], buf, length);
    m_bufferWriteIndex += length;
  }
  else
  {
    memcpy(&m_buffer[m_bufferWriteIndex], buf, roomLeft);
    length = (TMWTYPES_USHORT)(length - roomLeft);
    memcpy(&m_buffer[0], &buf[roomLeft], length);
    m_bufferWriteIndex = length;
  }
}

/**********************************************************************************\
	Function :			WinTCPChannel::StopUDPReaderThread
	Description : Stop UDP Reader thread
        To support DNP3 Spec IP Networking
	Return :			void	-	
	Parameters :
	Note : [none]
\**********************************************************************************/
void WinTCPChannel::StopUDPReaderThread()
{  
  
  // Stop reader thread 
  if (m_UDPthreadIsRunning == true)
  { 
    m_bUdpReaderThreadRun = false;

    if (WaitForSingleObject(m_udpReaderThreadHandle,5000) == WAIT_TIMEOUT)
    {
      m_UDPthreadIsRunning = false;
      WinIoTarg_endThread(m_udpReaderThreadHandle);
    }
  }
}
/**********************************************************************************\
	Function :			WinTCPChannel::StartUDPReaderThread
	Description : Start UDP Reader thread
        To support DNP3 Spec IP Networking
	Return :			bool	-	
	Parameters :
			void	-	
	Note : [none]
\**********************************************************************************/
bool WinTCPChannel::StartUDPReaderThread(void)
{

  // Start thread reading on UDP port
  if (m_UDPthreadIsRunning == false)
  {
    m_bUdpReaderThreadRun = true;

    m_udpReaderThreadHandle = (HANDLE)WinIoTarg_startThread(UDPReaderThread,this,&m_threadID,THREAD_PRIORITY_ABOVE_NORMAL);
    if(m_udpReaderThreadHandle == INVALID_HANDLE_VALUE)
      return false;
  }

  return true;
}

/**********************************************************************************\
	Function :			WinTCPChannel::UDPReaderThread
	Description :  UDP reader thread
        To support DNP3 Spec IP Networking
	Return :			unsigned int __stdcall	-	
	Parameters :
			void *pParam	-	
	Note : [none]
\**********************************************************************************/
unsigned int __stdcall WinTCPChannel::UDPReaderThread(void *pParam)
{
  TMWTYPES_UCHAR rcvBuf[WINIOTARG_MAX_UDP_RCVLEN];
  WinTCPChannel *pTCPChannel = (WinTCPChannel *)pParam;

  pTCPChannel->m_UDPthreadIsRunning = true;
  
	while(pTCPChannel->m_bUdpReaderThreadRun)
  {
    fd_set readfds;
    SOCKET socket = pTCPChannel->m_UDPSocket;

    if(socket == INVALID_SOCKET)
    {
      if (pTCPChannel->m_udpReaderThreadHandle != INVALID_HANDLE_VALUE)
      {
        CloseHandle(pTCPChannel->m_udpReaderThreadHandle);
        pTCPChannel->m_udpReaderThreadHandle = INVALID_HANDLE_VALUE;
      }
      pTCPChannel->m_UDPthreadIsRunning = false;
      return(1);
    }

    FD_ZERO(&readfds);
#pragma warning(disable:4127)
    FD_SET(socket,&readfds);
#pragma warning(default:4127)
    struct timeval timeout = {1,0};

    // Call select with timeout of 0 to see if data is available
    if(select(0, &readfds, WINIOTARG_NULL,WINIOTARG_NULL, &timeout) == SOCKET_ERROR)
    {
      /* Should we exit here? */
    }
    else if(FD_ISSET(socket, &readfds))
    {
      int returnValue;
      int  fromLen;  
      struct sockaddr from;
      struct sockaddr_in *pFrom;

      fromLen = sizeof(from);
      returnValue = recvfrom(socket, (char*)rcvBuf, WINIOTARG_MAX_UDP_RCVLEN, 0,
       &from, &fromLen);

      if(returnValue < 0) 
      {
        int temp = WSAGetLastError();
        if(temp == WSAEMSGSIZE)  
        {
          WinIoInterface::ProtoAnaLog(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "UDP Error returned from recvFrom, UDP datagram received was larger than %d bytes", WINIOTARG_MAX_UDP_RCVLEN);
          returnValue = WINIOTARG_MAX_UDP_RCVLEN;
        }
        else
        {
          char m_errorBuffer[128];
          WinIoTarg_formatErrorMsg(m_errorBuffer, temp); 
          WinIoInterface::ProtoAnaLog(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "UDP Error returned from recvFrom, %s", m_errorBuffer);
        }
      }

      if(returnValue>0)
      {
        pFrom = (struct sockaddr_in *)&from;

        // If outstation TCP and UDP, verify the src address with TCP connection end point.
        // If outstation UDP only, verify the src address of master
        // If master UDP only, verify src address of slave.
        // These are done by setting m_validUDPAddress to appropriate address
        //   if validation is enabled.
        if((pTCPChannel->m_validUDPAddress == 0)
          ||(pFrom->sin_addr.s_addr == pTCPChannel->m_validUDPAddress))
        {
          pTCPChannel->m_srcUDPPort = pFrom->sin_port;
       
          // put the bytes received into a circular buffer for reading by SCL
          pTCPChannel->inputData(rcvBuf, (TMWTYPES_USHORT)returnValue);
        }
        else
        {
          WinIoInterface::ProtoAnaLog(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "UDP data discarded from %s", inet_ntoa(pFrom->sin_addr)); 
        }
      }
    }
	}

  if (pTCPChannel->m_udpReaderThreadHandle != INVALID_HANDLE_VALUE)
  {
    CloseHandle(pTCPChannel->m_udpReaderThreadHandle);
    pTCPChannel->m_udpReaderThreadHandle = INVALID_HANDLE_VALUE;
  }
  pTCPChannel->m_UDPthreadIsRunning = false;
  return 1;
}

/**********************************************************************************\
	Function :			WinTCPChannel::setUDPDestPort
	Description :  Set UDP destination port based on parameter
        and whether or not we have an source port from a previous request
        To support DNP3 Spec IP Networking
	Return :			unsigned int __stdcall	-	
	Parameters :
    TMWTYPES_UCHAR UDPPort -
      TMWTARG_UDP_SEND       - Send to the remote port to be used for 
                               requests or responses
      TMWTARG_UDP_SEND_UNSOL - Send to the remote port to be used for   
                               unsolicited responses. Once outstation has
                               received a request from master this would be
                               same port as all responses.
	Note : [none]
\**********************************************************************************/
void WinTCPChannel::setUDPDestPort(
  TMWTYPES_UCHAR UDPPort)
{
  // If we don't have a source port yet, use the configured port 
  if(m_srcUDPPort == WINTCP_UDP_PORT_NONE)
  {
    if(UDPPort == TMWTARG_UDP_SEND)
    {
      m_destSockAddr.sin_port = htons(m_config.destUDPPort); 
    }
    else /* must be TMWTARG_UDP_SEND_UNSOL */
    {
      m_destSockAddr.sin_port = htons(m_config.initUnsolUDPPort);
    }
  }
  else /* we have a source port from a previous message */
  {
    // If configuration allows it, use src port from previous request
    // This would only be allowed on an outstation.
    if(m_config.destUDPPort == WINTCP_UDP_PORT_SRC)
    {
      m_destSockAddr.sin_port = m_srcUDPPort;
    }
    else
    {
      m_destSockAddr.sin_port = htons(m_config.destUDPPort);
    }
  }
}

/**********************************************************************************\
	Function :			WinTCPChannel::transmitUDP
	Description : [none]	
	Return :			TMWTYPES_BOOL	-	
	Parameters :
			TMWTYPES_UCHAR UDPPort	-	 A define that indicates the remote UDP port to
       transmit to. 
        TMWTARG_UDP_SEND       - Send to the remote port to be used for 
                                 requests or responses
        TMWTARG_UDP_SEND_UNSOL - Send to the remote port to be used for   
                                 unsolicited responses
			TMWTYPES_UCHAR *pBuff	-	
			TMWTYPES_USHORT numBytes	-	
	Note : [none]
\**********************************************************************************/
bool WinTCPChannel::transmitUDP(
  TMWTYPES_UCHAR UDPPort,
  TMWTYPES_UCHAR *buf, 
  TMWTYPES_USHORT numBytes)
{
  setUDPDestPort(UDPPort);

  LogMessage(TMWDIAG_ID_TARGET, "UDP transmit %d bytes", numBytes);
  if(SOCKET_ERROR == sendto(m_UDPSocket, (char*)buf, numBytes, 0,
    (const sockaddr *)&m_destSockAddr, sizeof(m_destSockAddr)))
  {
    WinIoTarg_formatErrorMsg(m_errorBuffer, WSAGetLastError());  
    LogMessage(TMWDIAG_ID_TARGET | TMWDIAG_ID_ERROR, "UDP sendto failed\n%19s%s", " ",m_errorBuffer);
    return(false);
  }
  
  return(true);
}

/**********************************************************************************\
	Function :			WinTCPChannel::UDPReceive
	Description : [none]	
	Return :			TMWTYPES_BOOL	-	
	Parameters :
		
	Note : [none]
\**********************************************************************************/
TMWTYPES_USHORT WinTCPChannel::UDPReceive(
  TMWTYPES_UCHAR  *pBuff, 
  TMWTYPES_ULONG  maxBytes)
{
  TMWTYPES_ULONG bytesRead = 0;
  TMWTYPES_UCHAR *readPtr = WINIOTARG_NULL;

  // See if there are any bytes in the circular buffer
  if(m_bufferReadIndex == m_bufferWriteIndex)
    return(0);

  readPtr = &m_buffer[m_bufferReadIndex];

  if(m_bufferReadIndex < m_bufferWriteIndex)
  {
    bytesRead = m_bufferWriteIndex - m_bufferReadIndex;
    if(bytesRead > maxBytes)
    {
      bytesRead = maxBytes;
    }
    m_bufferReadIndex += bytesRead;
  }
  else
  {
    bytesRead = WINTCP_BUFFER_SIZE - m_bufferReadIndex;
    if(bytesRead > maxBytes)
    {
      bytesRead = maxBytes;
      m_bufferReadIndex += bytesRead;
    }
    else
    {
      m_bufferReadIndex = 0;
    }
  }

  memcpy(pBuff, readPtr, bytesRead);

  return((TMWTYPES_USHORT)bytesRead);
}

void WinTCPChannel::LogMessage(TMWDIAG_ID sourceID, const char *format, ...)
{
  if (WinIoTargProtoAnaLogFun != WINIOTARG_NULL)
  {
    CriticalSectionLock lock(getLogLock());
    char _chanID[2048];
    _stprintf(_chanID,_T("%s - %s:%d - "),this->m_config.chnlName,this->m_config.ipAddress,this->m_config.ipPort);
    
    va_list va;
    va_start(va, format);
    WinIoTargProtoAnaLogFun(m_pChannel, sourceID, _chanID, format, va);
    va_end(va);
  }
}
