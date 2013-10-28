/*
	Linux header file for Linux support
*/

#ifndef LINUXIO_H
#define LINUXIO_H

#include <stdio.h>
#include <stdarg.h>
#include <unistd.h>
#include <sys/timeb.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netdb.h>
#include <time.h>
#include <errno.h>
#include <string.h> /* V7.997 - avoid compiler warnings */

/* Semaphore handling */
#include <pthread.h>


#define TMW_TP_Semaphore        pthread_mutex_t *
#define TMW_TP_TakeSem( a )     pthread_mutex_lock( a )
#define TMW_TP_ReleaseSem( a )  pthread_mutex_unlock( a )

/* Linux socket defintions */
#include <sys/time.h>
#include <sys/socket.h>
#include <string.h>
#include <sys/types.h>
#include <unistd.h>
#include <netinet/in.h>
#include <netinet/tcp.h>
#include <asm/errno.h>
#include <asm/ioctls.h>
#include <errno.h>
#include <fcntl.h>
#include <signal.h>
#define SOCKET              int
#define SOCKADDR_IN         struct sockaddr_in
#define SOCKADDR_LEN        sizeof(struct sockaddr_in)
#define LPSOCKADDR          struct sockaddr *
#define SOCKET_ERROR        -1
#define INVALID_SOCKET      -1
#define CLOSESOCKET( s )    close(s)
#define IOCTL_SOCKET(fd, func, arg) ioctl(fd, func, arg)
#define GET_LAST_ERROR( )   errno
#define LINTCP_EWOULDBLOCK     EWOULDBLOCK
#define LINTCP_EINPROGRESS     EINPROGRESS
#define LINTCP_ENOBUFS         ENOBUFS

#define MAKE_SOCKET_NON_BLOCKING( s, retval ) \
    { \
struct sigaction sa; \
memset( &sa, 0, sizeof(sa )); \
sa.sa_handler = SIG_IGN; \
sigaction( SIGPIPE, &sa, (struct sigaction *) NULL ); \
	retval = fcntl( s, F_SETFL, O_NONBLOCK ); \
    }

#define MAKE_SOCKET_REUSEADDR( s, ret ) \
    { \
        int cmd_arg = 1; \
        ret = setsockopt( s, SOL_SOCKET, SO_REUSEADDR,  \
                (char *) &cmd_arg, sizeof(cmd_arg) ); \
    }

#define MAKE_SOCKET_KEEPALIVE( s, ret ) \
    { \
        int cmd_arg = 1; \
        ret = setsockopt( s, SOL_SOCKET, SO_KEEPALIVE,  \
                (char *) &cmd_arg, sizeof(cmd_arg) ); \
    }

#define MAKE_SOCKET_NO_DELAY( s, ret ) \
    { \
        int cmd_arg = 1; \
        ret = setsockopt( s, SOL_TCP, TCP_NODELAY,  \
                (char *) &cmd_arg, sizeof(cmd_arg) ); \
    }

#define WRITE_SELECTED( s, result ) \
    {   \
        struct timeval timeout;  \
        fd_set write_fds;  \
        timeout.tv_sec = 0;  \
        timeout.tv_usec = 0;  \
        FD_ZERO( &write_fds );  \
        FD_SET( s, &write_fds );  \
        result = select( s + 1, NULL, &write_fds, NULL, &timeout );  \
    }
#define READ_SELECTED( s, result ) \
    {  \
        struct timeval timeout; \
        fd_set read_fds; \
        timeout.tv_sec = 0;  \
        timeout.tv_usec = 0;  \
        FD_ZERO( &read_fds ); \
        FD_SET( s, &read_fds ); \
        result = select( s + 1, &read_fds, NULL, NULL, &timeout ); \
    }
#define CLOSE_SELECTED( s, result ) \
    {  \
        struct timeval timeout; \
        fd_set except_fds[FD_SETSIZE]; \
        timeout.tv_sec = 0;  \
        timeout.tv_usec = 0;  \
        FD_ZERO( &except_fds ); \
        FD_SET( s, &except_fds ); \
        result = select( s, NULL, NULL, except_fds, &timeout ); \
    }

/* Thread creation macros */
#define TMW_ThreadId            pthread_t
#define TMW_ThreadDecl          void *
#define TMW_ThreadReturn        void *
#define TMW_ThreadArg           void *
#define TMW_DEFAULT_STACK_SIZE  512 /* Not used */
#define TMW_CreateThread( pId, pFunc, pFuncParams, stackSize ) \
    (pthread_create( pId, NULL, pFunc, (TMW_ThreadArg) pFuncParams ))

typedef void (* TMW_ThreadFunction)( void *threadParams );
typedef void *TMW_ThreadHandle;

#define Sleep( x ) usleep( x * 10 )
   

//  Delay for a specified number of milliseconds
void        LinDelay( unsigned ms );

// Modify for Linux bit rates
typedef unsigned long serialRate;

#define BR_115200   115200UL
#define BR_57600     57600UL
#define BR_38400     38400UL
#define BR_19200     19200UL
#define BR_9600       9600UL
#define BR_4800       4800UL
#define BR_2400       2400UL
#define BR_1200       1200UL
#define BR_600         600UL
#define BR_300         300UL

//  Mode for data bits
#define SER_7BITS   0x04            // 7 data bits
#define SER_8BITS   0x00            // 8 data bits

/* Prototypes */

//  Initialize serial port
int     LinSerialInit(  char    *device, 
						serialRate rate, 
						int     dataBits, 
						int     parity, 
						char    *recvBuffer, 
						int     recvSize, 
						char    *tranBuffer, 
						int     tranSize );

//  Return the number of characters in the port's input queue.
unsigned  LinSerialRecvCount( int ttyfd );

//  Get error state of serial channel.  Cleared by reading.
unsigned char   LinSerialError( int ttyfd );

//  Character serial getc and putc functions.
int     LinSerialGetc( int ttyfd );

int     LinSerialPutc( int ttyfd, int ch );

#endif  // LINUXIO_H
