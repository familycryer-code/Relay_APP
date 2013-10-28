/*****************************************************************************/
/* Triangle MicroWorks, Inc.                         Copyright (c) 2008-2011 */
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

/* file: LinioDefs.h
 * description: Definition of target types and macros for Linux
 */

#ifndef LinioDefs_DEFINED
#define LinioDefs_DEFINED

#include <termios.h>

#include "linuxio.h"
#include "liniocnfg.h"

#include "tmwscl/utils/tmwtypes.h"
#include "tmwscl/utils/tmwdefs.h"
#include "tmwscl/utils/tmwtarg.h"

#if LINIOTARG_SUPPORT_232

typedef enum Lin232ParityEnum {
  LIN232_PARITY_NONE,
  LIN232_PARITY_EVEN,
  LIN232_PARITY_ODD
} LIN232_PARITY;

typedef enum Lin232StopBitsEnum {
  LIN232_STOP_BITS_1,
  LIN232_STOP_BITS_2
} LIN232_STOP_BITS;

typedef enum Lin232DataBitsEnum {
  LIN232_DATA_BITS_7,
  LIN232_DATA_BITS_8
} LIN232_DATA_BITS;

typedef enum Lin232PortMode
{
  LIN232_MODE_NONE,
  LIN232_MODE_HARDWARE
} LIN232_PORT_MODE;

typedef enum Lin232BaudRate
{
  LIN232_BAUD_110     = B110,
  LIN232_BAUD_300     = B300,
  LIN232_BAUD_600     = B600,
  LIN232_BAUD_1200    = B1200,
  LIN232_BAUD_2400    = B2400,
  LIN232_BAUD_4800    = B4800,
  LIN232_BAUD_9600    = B9600,
  LIN232_BAUD_19200   = B19200,
  LIN232_BAUD_38400   = B38400,
  LIN232_BAUD_57600   = B57600,
  LIN232_BAUD_115200  = B115200,
  LIN232_BAUD_230400  = B230400,
  LIN232_BAUD_576000  = B576000,
  LIN232_BAUD_921600  = B921600,
  LIN232_BAUD_1152000 = B1152000
} LIN232_BAUD_RATE;


typedef struct Lin232ConfigStruct {
  /* User specified channel name */
  TMWTYPES_CHAR    chnlName[LINIOCNFG_MAX_NAME_LEN]; 

  /* "/dev/ttyS0", "/dev/ttyS1", etc. */
  TMWTYPES_CHAR    portName[LINIOCNFG_MAX_NAME_LEN]; 

  /* flow control: none, hardware */
  LIN232_PORT_MODE portMode;

  /* baud rate */
  LIN232_BAUD_RATE baudRate;

  /* parity */
  LIN232_PARITY    parity;
 
  /* 7 or 8 */
  LIN232_DATA_BITS numDataBits;

  /* 1 or 2 */
  LIN232_STOP_BITS numStopBits;

  /* MODBUS RTU or not */
  TMWTYPES_BOOL    bModbusRTU;
} LIN232_CONFIG;

/* Define serial port channel */
typedef struct SerialIOChannel {
  
  /* Callback function for this channel */
  TMWTARG_CHANNEL_CALLBACK_FUNC     pChannelCallback;      /* From TMWTARG_CONFIG  */
  void                             *pChannelCallbackParam; /* From TMWTARG_CONFIG  */
  TMWTARG_CHANNEL_READY_CBK_FUNC    pChannelReadyCallback; /* From TMWTARG_CONFIG  */
  void                             *pChannelReadyCbkParam; /* From TMWTARG_CONFIG  */
  TMWTARG_CHANNEL_RECEIVE_CBK_FUNC  pReceiveCallbackFunc;  /* From openChannel    */
  TMWTARG_CHECK_ADDRESS_FUNC        pCheckAddrCallbackFunc;/* From openChannel    */
  void                             *pCallbackParam;        /* From openChannel, used by both above */

  /* parity */
  LIN232_PARITY      parity;

  /* flow control: none, hardware */
  LIN232_PORT_MODE   portMode;

  /* 7 or 8 */
  LIN232_DATA_BITS   numDataBits;

  /* 1 or 2 */
  LIN232_STOP_BITS   numStopBits;

  /* MODBUS RTU or not */
  TMWTYPES_BOOL      bModbusRTU; 
   
  /* 
   * Specifies the amount of time (in character times) to use to 
   * determine that a frame has been completed.  For modbus RTU this 
   * value is 3.5 (i.e. 4 will be used)
   */
  TMWTYPES_USHORT numCharTimesBetweenFrames;

  /*
   * Specifies the amount of time to use to 
   * determine that an inter character timeout has occured.  
   * For modbus RTU this value is 1.5 character times (i.e. 2 would be used)
   */
  TMWTYPES_USHORT interCharTimeout;
  
  /* Linux tty fd must be carried around */
  TMWTYPES_INT       ttyfd;
 
  LIN232_BAUD_RATE   baudRate;

  TMWTYPES_CHAR      chnlName[LINIOCNFG_MAX_NAME_LEN]; 
  TMWTYPES_CHAR      portName[LINIOCNFG_MAX_NAME_LEN];

  TMWTYPES_CHAR      chanInfoBuf[128];

  struct SerialIOChannel   *pNext;
} SERIAL_IO_CHANNEL;


#endif /* #if LINIOTARG_SUPPORT_232 */

#if LINIOTARG_SUPPORT_TCP
 
#define LINTCP_BACKLOG          5
#define LINTCP_RECV_FLAGS       0


/* Define data types used to interface to the TCP target library. */
typedef enum LinTCPModeEnum {
  /* listen for connection */
  LINTCP_MODE_SERVER,

  /* attempt to make a connection */
  LINTCP_MODE_CLIENT,

  /* both client and server functionality */
  LINTCP_MODE_DUAL_ENDPOINT,

  /* UDP only, no TCP connection */
  LINTCP_MODE_UDP
} LINTCP_MODE;

typedef enum LinTCPRoleEnum {
  /* Master, this only matters for DNP Dual End Point */
  LINTCP_ROLE_MASTER,

  /* Outstation, this only matters for DNP Dual End Point */
  LINTCP_ROLE_OUTSTATION
} LINTCP_ROLE;


/* NOTE: These match the WinIoTarg config structure exactly, which is convenient for test applications */
typedef struct LinTCPConfigStruct {
  TMWTYPES_CHAR   chnlName[LINIOCNFG_MAX_NAME_LEN];     /* User specified channel name */
  
  /* On client - 
   *      this is the IP address to set up TCP connection to
   * On server and Dual End Point Device - 
   *      this is the IP address to accept TCP connection from
   *      May be *.*.*.* indicating accept connection from any client
   */
  TMWTYPES_CHAR   ipAddress[20];  
  
  /* On client - 
   *      this is the port to connect to
   * On server and Dual End Point Device - 
   *      this is the port to listen on
   */
  TMWTYPES_USHORT ipPort;

  /* Number of milliseconds to wait for TCP connect to succeed or fail */
  TMWTYPES_ULONG  ipConnectTimeout;

  /* Indicate CLIENT, SERVER, DUAL END POINT, or UDP only
   *  (DUAL END POINT provides both CLIENT and SERVER functionality but 
   *  with only one connection at a time)
   */
  LINTCP_MODE     mode;
  
  /* If TRUE, when a new connect indication comes in and this channel is 
   * already connected, it will be marked for disconnect. This will allow a
   * new connection to come in next time. This handles not receiving notification
   * of disconnect from the remote end, but remote end trying to reconnect.
   * If you want to allow multiple simultaneous connections to multiple channels
   * from any IP address to a particular port number, this parameter should be
   * set to FALSE.
   * 
   * For DNP this should be set to TMWDEFS_FALSE according to DNP3 Specification
   *  IP Networking. Keep alive will detect that original connection has
   * failed, which would then allow a new connection to be rcvd.
   */
  TMWTYPES_BOOL   disconnectOnNewSyn;
    
  /* NOTE: The following configuration parameters are required to support 
   *   DNP3 Specification IP Networking. These are not required for
   *   the IEC or Modbus protocols.
  
   *  Indicate master or outstation (slave) role in dnp networking
   *  as specified by DNP3 Specification IP Networking
   */
  LINTCP_ROLE     role;

  /* If Dual End Point is supported a listen will be done on the above ipPort 
   *  and a connection request will be sent to this port number when needed.
   *  This should match ipPort on remote device.
   *  Normal state is listen, connection will be made when there is data to send.
   */
  TMWTYPES_USHORT dualEndPointIpPort;
  
  /* Destination IP address for UDP broadcast requests.
   * This is only used by a DNP Master when TCP and UDP are supported.
   * If UDP ONLY is configured, ipAddress will be used as destination for all requests.
   */
  TMWTYPES_CHAR     udpBroadcastAddress[20];
  
  /* Local port for sending and receiving UDP datagrams on.
   * If this is set to TMWTARG_UDP_PORT_NONE, UDP will not be enabled. 
   * For DNP networking UDP should be supported. 
   * It is not needed for any of the current IEC or modbus protocols.
   * On Master - If this is set to TMWTARG_UDP_PORT_ANY, an unspecified available 
   *             port will be used. 
   * On Slave  - This should be chosen to match the UDP port that the master uses
   *             to send Datagram messages to. 
   *             This must not be TMWTARGP_UDP_PORT_ANY or TMWTARG_UDP_PORT_SRC.
   */
  TMWTYPES_USHORT localUDPPort;

  /* On Master - if TCP and UDP is configured this specifies the destination UDP/IP 
   *              port to send broadcast requests in UDP datagrams to.
   *             if UDP ONLY is configured this specifies the destination UDP/IP 
   *              port to send all requests in UDP datagrams to.
   *             This must match the "localUDPPort" on the slave.
   * On Slave  - if TCP and UDP this is not used.
   *             if UDP ONLY is configured this specifies the destination UDP/IP
   *              port to send responses to. 
   *              Can be TMWTARG_UDP_PORT_SRC indicating use the src port from a 
   *              UDP request received from master.
   */
  TMWTYPES_USHORT destUDPPort;
  
  /* On master - Not used.
   * On Slave  - if TCP and UDP not used.
   *             if UDP ONLY is configured this specifies the destination UDP/IP 
   *              port to send the initial Unsolicited Null response to.
   *              After receiving a UDP request from master, destUDPPort (which)
   *              may indicate use src port) will be used for all responses.
   *              This must not be TMWTARG_UDP_PORT_NONE, TMWTARG_UDP_PORT_ANY, or
   *              TMWTARG_UDP_PORT_SRC for a slave that supports UDP.
   */
  TMWTYPES_USHORT initUnsolUDPPort;

   /* Whether or not to validate source address of received UDP datagram. */
  TMWTYPES_BOOL   validateUDPAddress;

} LINTCP_CONFIG;
 
typedef struct TcpIOChannel {
  TMWTYPES_CHAR chnlName[LINIOCNFG_MAX_NAME_LEN];     /* User specified channel name */

  LINTCP_MODE           mode;
  LINTCP_ROLE           role;

  /* Callback function for this channel */
  TMWTARG_CHANNEL_CALLBACK_FUNC     pChannelCallback;      /* From TMWTARG_CONFIG */
  void                             *pChannelCallbackParam; /* From TMWTARG_CONFIG */
  TMWTARG_CHANNEL_READY_CBK_FUNC    pChannelReadyCallback; /* From TMWTARG_CONFIG (not used)         */
  void                             *pChannelReadyCbkParam; /* From TMWTARG_CONFIG (not used)         */
  TMWTARG_CHANNEL_RECEIVE_CBK_FUNC  pReceiveCallbackFunc;  /* From openChannel    (for event driven) */
  TMWTARG_CHECK_ADDRESS_FUNC        pCheckAddrCallbackFunc;/* From openChannel    (not used)         */
  void                             *pCallbackParam;        /* From openChannel, used by both above   */

  TMWTYPES_CHAR        ipAddress[20]; 
  TMWTYPES_CHAR        ipAddress_listen[20]; /* Allows listening for a specific client */
  TMWTYPES_CHAR        nicName[20];    /* Allows binding to a specific interface */ 
  TMWTYPES_CHAR        udpBroadcastAddress[20];
  TMWTYPES_USHORT      ipPort;
  TMWTYPES_USHORT      dualEndPointIpPort;
  TMWTYPES_ULONG       ipConnectTimeout;
  TMWTYPES_BOOL        disconnectOnNewSyn;

  TMWTYPES_CHAR        chanInfoBuf[64];
  SOCKET               dataSocket;

  TMWTYPES_USHORT      destUDPPort;
  TMWTYPES_USHORT      initUnsolUDPPort;
  TMWTYPES_USHORT      localUDPPort;
  TMWTYPES_USHORT      sourceUDPPort;
  TMWTYPES_BOOL        validateUDPAddress;
  TMWTYPES_ULONG       validUDPAddress;
  SOCKET               udpSocket;
  SOCKADDR_IN          destUDPaddr;
  int                  destUDPaddrLen;

  TMWTYPES_UCHAR       udpBuffer[LINIOCNFG_UDP_BUFFER_SIZE];
  int                  udpReadIndex;
  int                  udpWriteIndex;

  TMWTYPES_BOOL        bChannelFree;

  struct TcpIOChannel *pNext;
} TCP_IO_CHANNEL;

 
typedef struct TcpListener {
  SOCKET              listenSocket;
  TMWTYPES_USHORT     listenPort;
  TMWTYPES_BOOL       bChannelFree;

  struct TcpListener *pNext;
} TCP_LISTENER;

#endif /* #if LINIOTARG_SUPPORT_TCP */

typedef enum LinIOTypeEnum {
  LINIO_TYPE_232=1,
  LINIO_TYPE_TCP,
  LINIO_TYPE_NONE
} LINIO_TYPE_ENUM;

typedef struct LinIOChannel {
  LINIO_TYPE_ENUM      type;
  void                 *pChannelInfo;
} LINIO_CHANNEL;

/* Top level control structure */
typedef struct LinIOContext {
  TMWTYPES_BOOL         bStarted;

#if LINIOTARG_SUPPORT_232
  TMW_TP_Semaphore      serialSemaphore;
  SERIAL_IO_CHANNEL    *pSerialChannels;
#endif

#if LINIOTARG_SUPPORT_TCP
  TMW_TP_Semaphore      tcpSemaphore;
  TCP_IO_CHANNEL       *pTcpChannels;
  TCP_LISTENER         *pTcpListeners; 
  TMWTYPES_BOOL         bSocketThreadRunning;
  TMWTYPES_BOOL         bConnectThreadRunning;
  TMW_ThreadId          socketThreadHandle;
  TMW_ThreadId          connectThreadHandle;
#endif

} LINIO_CONTEXT;

/* Configuration structure */
typedef struct LinIOConfig {
  LINIO_TYPE_ENUM type;

#if LINIOTARG_SUPPORT_232
  LIN232_CONFIG lin232;
#endif

#if LINIOTARG_SUPPORT_TCP
  LINTCP_CONFIG linTCP;
#endif
} LINIO_CONFIG;

#endif /*  LinioDefs_DEFINED */
