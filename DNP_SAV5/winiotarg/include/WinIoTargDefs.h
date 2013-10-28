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

/** \file WinIoTargDefs.h
 * Provides the definition of various macros and types for the WinIoTarg DLL. 
 */

#ifndef WinIoTargDefs_DEFINED
#define WinIoTargDefs_DEFINED

#include <tchar.h>
#include "tmwscl/utils/tmwdefs.h"
#include "tmwscl/utils/tmwtypes.h"
#include "tmwscl/utils/tmwdiag.h"
#include "WinIoTargEnums.h"
 

// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the WINIOTARG_EXPORTS
// symbol defined on the command line. this symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// WINIOTARG_API functions as being imported from a DLL, wheras this DLL sees symbols
// defined with this macro as being exported.
#ifdef WINIOTARG_EXPORTS
#define WINIOTARG_API __declspec(dllexport)
#else
#define WINIOTARG_API __declspec(dllimport)
#endif

#define WINIOTARG_NULL (0)

#define WINIOTARG_STR_LEN 256

#define WINIOTARG_MAX_UDP_RCVLEN 2500

/**
A template for a Protocol Analyzer logging function
The function is expected to be implemented outside of the DLL.
It will be called when the DLL calls proto ana log
*/
typedef void (*WinIoTargProtoAnaLogFunType)(void *pChannel, TMWDIAG_ID sourceId, const char *chanID, const char *format, va_list ap);

/**
  generic WinIoTarg callback
 */
typedef TMWTYPES_BOOL   (*WINIO_CALLBACK)(
  void *pContext, 
  void *pCallbackParam,
  TMWTYPES_USHORT numBytes,
  TMWTYPES_USHORT maxBytes,
  TMWTYPES_USHORT *pReturnedNumBytes,
  TMWTYPES_UCHAR *pBuf);

/**
  status WinIoTarg callback
 */
typedef void (*WINIO_STATUS_CALLBACK)(
  void *pCallbackParam,
  WINIO_STATUS eStatus,
  TMWTYPES_ULONG iStatusCode);
 
/**
  SSL Secure Socket Layer Verify callback
 */
typedef TMWTYPES_BOOL   (*WINIO_SSLVERIFYCALLBACK)( 
  void *pCallbackParam, 
  void *ssl,
  int   error); 

/**
  Data type used to configure the the RS232 interface. 
 */
typedef struct Win232ConfigStruct {
  char   chnlName[WINIOTARG_STR_LEN];                      /*!<  User specified channel name */
  char   portName[WINIOTARG_STR_LEN];                      /*!<  "COM1", "COM2", etc. */
  WIN232_PORT_MODE  portMode;            /*!<  hardware, software, windows */
  char   baudRate[WINIOTARG_STR_LEN];                      /*!<  in string form; example: "9600" */
  WIN232_PARITY parity;                  /*!<  parity */
  WIN232_DATA_BITS numDataBits;          /*!<  7 or 8 */
  WIN232_STOP_BITS numStopBits;          /*!<  1 or 2 */
  TMWTYPES_BOOL   bModbusRTU;
  //TMWTYPES_BOOL   bSyncMode;
  WIN232_DTR_MODE dtrMode;
  WIN232_RTS_MODE rtsMode;
  TMWTYPES_BOOL   disabled;
} WIN232_CONFIG;

/**
  Data type used to configure a Modem Pool channel. 
 */
typedef struct WinModemPoolChannelConfigStruct {
  char   chnlName[WINIOTARG_STR_LEN];                      /*!<  User specified channel name */
  char   poolName[WINIOTARG_STR_LEN];                      /*!<  the pool to use */
  char   phoneNumber[WINIOTARG_STR_LEN];                   /*!<  number to dial out */
  TMWTYPES_ULONG answerTime;             /*!<  time to wait after dial for answer */
  TMWTYPES_ULONG redialLimit;            /*!<  number of times to attempt to dial */
  TMWTYPES_ULONG idleTime;               /*!<  time to wait after idle to hang up modem */
  TMWTYPES_BOOL   bModbusRTU;
  TMWTYPES_BOOL   bDialOut;
} WINMODEM_POOL_CHANNEL_CONFIG;

/**
  Data type used to configure a Modem. 
 */
typedef struct WinModemConfigStruct {
  char   chnlName[WINIOTARG_STR_LEN];                      /*!<  User specified channel name */
  char   poolName[WINIOTARG_STR_LEN];                      /*!<  The pool this modem is a member of */

  /* com port stuff */
  char   *portName;                      /*!<  "COM1", "COM2", etc. */
  WIN232_PORT_MODE  portMode;            /*!<  hardware, software, windows */
  char   *baudRate;                      /*!<  in string form; example: "9600" */
  WIN232_PARITY parity;                  /*!<  parity */
  WIN232_DATA_BITS numDataBits;          /*!<  7 or 8 */
  WIN232_STOP_BITS numStopBits;          /*!<  1 or 2 */
  /* modem stuff */
  char   *initString;
  char   *hangupString;
  WINMODEM_DIALING_MODE dialingMode;
  TMWTYPES_USHORT readCommandTimeout;
  TMWTYPES_USHORT writeCommandTimeout;
  WINMODEM_RESP_CHAR respTerminatorChar;
  TMWTYPES_BOOL   bNoDialOut;
  TMWTYPES_BOOL   bEnable;
  void *pWinModemPool;
  WINIO_STATUS_CALLBACK statusCB;
  void *statusCBdata;
} WINMODEM_CONFIG;

/**
  Data type used to configure a Modem Pool. 
 */
typedef struct WinModemPoolConfigStruct {
  char   poolName[WINIOTARG_STR_LEN];                /*!<  User specified pool name */
} WINMODEM_POOL_CONFIG;


// The following defines may be used when configuring UDP ports. 

// Don't open a socket for UDP
#define WINTCP_UDP_PORT_NONE TMWTARG_UDP_PORT_NONE

// Let the UDP/IP stack determine what port number to use (master)
#define WINTCP_UDP_PORT_ANY  TMWTARG_UDP_PORT_ANY

// When sending responses use the source port number from the request (slave)
#define WINTCP_UDP_PORT_SRC  TMWTARG_UDP_PORT_SRC

/**
  Data Type used to configure the TCP/IP interface.
 */
typedef struct WinTCPConfigStruct {
  char chnlName[WINIOTARG_STR_LEN];                /*!<  User specified channel name */

  // On client - 
  //      this is the host name or IP address to set up TCP connection to
  // On server - 
  //      this is the host name or IP address to accept TCP connection from
  //      May be *.*.*.* indicating accept connection from any client
  //      May also be a list of ';' or ',' separated host names or ip addresses
  //        to allow connections from.
  // On Dual End Point Device - 
  //      this is the host names or IP address to accept TCP connection from or to connect to. 
  //      May also be a list of ';' or ',' separated host names or ip addresses
  //        to allow connections from, will only try to connect to the first on the list.
  char ipAddress[WINIOTARG_STR_LEN];

  // On Client -
  //      Address to bind socket to. This allows you to specify which IP address 
  //      to send as source address in TCP messages if there are multiple IP Addresses, 
  //      for example when there are multiple Network Interface Cards (NICs). 
  //      if "0.0.0.0" is used the TCP stack will choose which IP Address to use.
  //      If an address that is not present is specified, the bind will fail and 
  //      the TCP stack will choose which address.
  //      This address is also used for DNP Master when sending UDP datagrams.
  //      Binding this address does not guarantee sending on a particular NIC. 
  //      This is determined by the IP Routing Table depending on the destination 
  //      IP Address. You can display this table by entering "route print" in 
  //      a command window. It is possible to add manual routes to cause a particular 
  //      NIC to be used. "route add destIPAddress gateway". 
  //      Enter "route ?" for more details.
  // On Server - 
  //      not currently used for listeners. 
  //      (Note: this address IS used for DNP Outstation if configured for UDP ONLY)
  char localIpAddress[WINIOTARG_STR_LEN];

  // On client - 
  //      this is the port to connect to
  // On server and Dual End Point Device - 
  //      this is the port to listen on
  TMWTYPES_USHORT ipPort;

  // Number of milliseconds to wait for TCP connect to succeed or fail
  TMWTYPES_ULONG ipConnectTimeout;

  // Indicate CLIENT, SERVER, DUAL END POINT, or UDP only
  // (DUAL END POINT provides both CLIENT and SERVER functionality but 
  //  with only one connection at a time)
  WINTCP_MODE mode;

  // If TRUE, when a new connect indication comes in and this channel is 
  // already connected, it will be marked for disconnect. This will allow a
  // new connection to come in next time. This handles not receiving notification
  // of disconnect from the remote end, but remote end trying to reconnect.
  // If you want to allow multiple simultaneous connections to multiple channels
  // from any IP address to a particular port number, this parameter should be
  // set to FALSE.
  // 
  // For DNP this should be set to TMWDEFS_FALSE according to DNP3 Specification
  //  IP Networking. Keep alive will detect that original connection has
  //  failed, which would then allow a new connection to be rcvd.
  TMWTYPES_BOOL disconnectOnNewSyn;

  // NOTE: The following configuration parameters are required to support 
  // DNP3 Specification IP Networking. These are not required for
  // the IEC or Modbus protocols.
  
  // Indicate master or outstation (slave) role in dnp networking
  // as specified by DNP3 Specification IP Networking
  // Master has priority on new connections
  WINTCP_ROLE role;

  // If Dual End Point is supported a listen will be done on the above ipPort 
  // and a connection request will be sent to this port number when needed.
  // This should match ipPort on remote device.
  // Normal state is listen, connection will be made when there is data to send.
  TMWTYPES_USHORT dualEndPointIpPort;

  // Destination IP address for UDP broadcast requests.
  // This is only used by a DNP Master when TCP and UDP are supported.
  // If UDP ONLY is configured, ipAddress will be used as destination for all requests.
  char udpBroadcastAddress[WINIOTARG_STR_LEN];

  // Local port for sending and receiving UDP datagrams on.
  // If this is set to WINTCP_UDP_PORT_NONE, UDP will not be enabled. 
  // For DNP networking UDP should be supported. 
  // It is not needed for any of the current IEC or modbus protocols.
  // On Master - If this is set to WINTCP_UDP_PORT_ANY, an unspecified available 
  //             port will be used. 
  // On Slave  - This should be chosen to match the UDP port that the master uses
  //             to send Datagram messages to. 
  //             This must not be WINTCP_UDP_PORT_ANY or WINTCP_UDP_PORT_SRC.
  TMWTYPES_USHORT localUDPPort;

  // On Master - if TCP and UDP is configured this specifies the destination UDP/IP 
  //              port to send broadcast requests in UDP datagrams to.
  //             if UDP ONLY is configured this specifies the destination UDP/IP 
  //              port to send all requests in UDP datagrams to.
  //             This must match the "localUDPPort" on the slave.
  // On Slave  - if TCP and UDP this is not used.
  //             if UDP ONLY is configured this specifies the destination UDP/IP
  //              port to send responses to. 
  //             Can be WINTCP_UDP_PORT_SRC indicating use the src port from a 
  //              UDP request received from master.
  TMWTYPES_USHORT destUDPPort;
  
  // On master - Not used.
  // On Slave  - if TCP and UDP not used.
  //             if UDP ONLY is configured this specifies the destination UDP/IP 
  //              port to send the initial Unsolicited Null response to.
  //              After receiving a UDP request from master, destUDPPort (which)
  //              may indicate use src port) will be used for all responses.
  //              This must not be WINTCP_UDP_PORT_NONE, WINTCP_UDP_PORT_ANY, or
  //              WINTCP_UDP_PORT_SRC for a slave that supports UDP.
  TMWTYPES_USHORT initUnsolUDPPort;

  // Whether or not to validate source address of received UDP datagram.
  TMWTYPES_BOOL   validateUDPAddress;

  /* Use SSL/TLS for this channel */
  TMWTYPES_BOOL   useSSL; 
  TMWTYPES_BOOL   SSLVerifyClient;
  
  /* Name of credentials file */
  char sslCredFile[WINIOTARG_STR_LEN];

  /* password to decrypt key from credentials file */
  char sslPassword[WINIOTARG_STR_LEN];

  /* Credential Authority List file */
  char sslCaListFile[WINIOTARG_STR_LEN];
     
  void *sslVerifyCallbackParam;
  WINIO_SSLVERIFYCALLBACK sslVerifyCallback;

} WINTCP_CONFIG;

/**
 * Data type used to configure the modbus plus interface.\n
 *\n
 * Modbus Plus Route configuration\n
 *\n
 * MASTER MODBUS PLUS (i.e. as a client)\n
 * routePath specifies how to get to the slave.\n
 *   NOTE: \n
 *   1. For example if a slave on the local MB+ network is node 63 and is listening on path 1 the routePath is 63.1\n
 *   \n
 * cardNum is the card on which to send the message
 * slavePath does not apply for master
 * \n
 * SLAVE MODBUS PLUS (i.e. as a server)
 * routePath does not apply
 * cardNum is the card to listen for messages on
 * slavePath is the path on the card to listen on\n
 *    NOTE:\n
 *      1. The node id (i.e. 63 in the above example) is set in the MBX driver.\n
 *      2. Diffrent values for slavePath allow reciept of messages by multiple applications on the computer.\n
 * 
 */
typedef struct WinMBPConfigStruct {
  char chnlName[WINIOTARG_STR_LEN];            /*!<  User specified channel name                  */
  char routePath[WINIOTARG_STR_LEN];           /*!<  MBP Routing Path of the form xx.xx.xx.xx.xx  */
  TMWTYPES_USHORT cardNum;       /*!<  MBP Host Adapter number, 0-65535             */
  TMWTYPES_UCHAR  slavePath;     /*!<  MBP Slave Path for Server channels, 1-8      */
  TMWTYPES_ULONG  recvTimeout;   /*!<  Channel Receive Timeout, not currently used  */
  WINMBP_MODE     mode;          /*!<  Clent or Server Mode, see WinMBPModeEnum     */
  TMWTYPES_BOOL   leaveMasterPortOpen;
} WINMBP_CONFIG;

/**
  Data Type used to configure the Monitor Interface.
 */
typedef struct  {
  TMWTYPES_BOOL    monitorMode;
  char             chnlName[WINIOTARG_STR_LEN];       /*!<  User specified channel name */
  char             pFilter[WINIOTARG_STR_LEN];                  
  TMWTYPES_BOOL    commandInput;  
  TMWTYPES_CHAR    interfaceNum;                      /*!< Left for backward compatibility */
  TMWTYPES_CHAR    interfaceDescr[WINIOTARG_STR_LEN]; /*!< Which TCP interface to monitor on */
  char             ipAddress[WINIOTARG_STR_LEN];
  TMWTYPES_USHORT  ipPort;
} WINMON_CONFIG;

/**
  The context returned from WinIoTarg_Create()
 */
typedef struct WinIOContext
{
  void *pWinIoInterface;
  WINIO_TYPE_ENUM type;
  TMWTYPES_BOOL   allowConnectorThread;
  TMWTYPES_ULONG  connectDelay;

  void *pTxCallbackParam;
  WINIO_CALLBACK pTxCallback;

  void *pRxCallbackParam;
  WINIO_CALLBACK pRxCallback;

} WINIO_CONTEXT;

/**
  A Data Type that includes all other configuration data types
 */
typedef struct WinIOConfig {
  WINIO_TYPE_ENUM type;
  WINIO_TIME_MODE timeMode;
  TMWTYPES_ULONG  connectDelay;
  TMWTYPES_BOOL forceDisconnected;
  WIN232_CONFIG win232;
  WINMODEM_POOL_CHANNEL_CONFIG winModemPoolChannel;
  WINMODEM_POOL_CONFIG winModemPool;
  WINMODEM_CONFIG winModem;
  WINTCP_CONFIG winTCP;
  WINMBP_CONFIG winMBP;
  WINMON_CONFIG winMON;
} WINIO_CONFIG;

/**
  prototype for a thread function
 */
typedef unsigned( __stdcall  *WINIO_THREAD_FUN )( void * );

#endif // WinIoTargDefs_DEFINED