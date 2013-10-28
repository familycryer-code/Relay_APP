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

/* file: tmwcrypto.h
 * description: This file defines the interface between the Triangle 
 *  MicroWorks, Inc. DNP master source code library and the cryptography library
 *  to perform encryption/decrypton signing/verification etc.
 */
#ifndef TMWCRYPTO_DEFINED
#define TMWCRYPTO_DEFINED

#include "tmwscl/utils/tmwdefs.h" 
#include "tmwscl/utils/tmwtypes.h" 
#if DNPCNFG_SUPPORT_AUTHENTICATION
#include <cyassl/ctaocrypt/dsa.h>
#include <cyassl/ctaocrypt/rsa.h>
#endif
 
/* The following defines are the enumerated values 
 * for interfacing to tmwcrypto functions. The source code
 * library will use these to specify which alogrithms to use.
 * These are NOT values transmitted over the DNP or other protocols.
 */
typedef enum algorithmEnum
{
  TMWCRYPTO_ALG_UNDEFINED,

  /* Key wrap algorithms  */
  TMWCRYPTO_ALG_KEYWRAP_AES128,
  TMWCRYPTO_ALG_KEYWRAP_AES256, 

  /* symmetric encryption algorithms */
  TMWCRYPTO_ALG_SYM_AES128,
  TMWCRYPTO_ALG_SYM_AES256,

  /* Asymmetric encryption algorithms */
  TMWCRYPTO_ALG_ASYM_RSAESOAEP1024,
  TMWCRYPTO_ALG_ASYM_RSAESOAEP2048, 
  TMWCRYPTO_ALG_ASYM_RSAESOAEP3072, 

  /* Signing algorithms */
  TMWCRYPTO_ALG_SIGN_DSA_SHA1, 
  TMWCRYPTO_ALG_SIGN_DSA_SHA256,

  /* MAC algorithms */
  TMWCRYPTO_ALG_MAC_SHA1,
  TMWCRYPTO_ALG_MAC_SHA256,
  TMWCRYPTO_ALG_MAC_AESGMAC

} TMWCRYPTO_ALGORITHM;


/* Enumerations specifying the type of key being requested by the library 
 */
typedef enum keyTypeEnum
{
  /* User Update Key, used to encrypt/decrypt Session Keys when sending 
   * from master to Outstation 
   */
  TMWCRYPTO_USER_UPDATE_KEY,  

  /* User Control Direction Session Key, used for performing hash function 
   * on critical messages 
   */
  TMWCRYPTO_USER_CONTROL_SESSION_KEY,  

  /* User Monitor Direction Session Key, used for performing hash function 
   * on critical messages 
   */
  TMWCRYPTO_USER_MONITOR_SESSION_KEY,  

  /* User Public Key when Asymmetric Key Update is supported */
  TMWCRYPTO_USER_ASYM_PUB_KEY,  

  /* User Private Key when Asymmetric Key Update is supported */
  TMWCRYPTO_USER_ASYM_PRV_KEY, 
  
  /* Outstation Public Key when Asymmetric Key Update is supported */
  TMWCRYPTO_OS_ASYM_PUB_KEY,

  /* Outstation Private Key when Asymmetric Key Update is supported */
  TMWCRYPTO_OS_ASYM_PRV_KEY,   

  /* Authority Certification Key when Symmetric Key Update is supported */
  TMWCRYPTO_AUTH_CERT_SYM_KEY, 
  
  /* Authority Public Key when Asymmetric Key Update is supported */
  TMWCRYPTO_AUTH_ASYM_PUB_KEY,

  /* Authority Private Key is only known on the Authority         */
  TMWCRYPTO_AUTH_ASYM_PRV_KEY


} TMWCRYPTO_KEYTYPE;


/* Key Structure passed across cryptography interface 
 * The contents of this structure are not used by the Source Code Library.
 * The SCL will retrieve a key (or a handle to a key) in this structure from the 
 * key management functions and then pass this to the cryptography functions.
 * These key management and cryptography functions are defined in this file.
 * This structure can be modified if you are not using the OpenSSL sample code
 * provided as an example.
 */
#define TMWCRYPTO_MAX_KEY_LENGTH     128
#define TMWCRYPTO_MAX_IVECTOR_LENGTH 128
typedef struct tmwcrypto_key {

  /* Type of key this structure contains */
  TMWCRYPTO_KEYTYPE keyType;

  /* The default OpenSSL interface will use the following convention.
   *  If symmetric, this will contain the key itself, 
   *  If asymmetric, this will contain the name of a file that contains the key.  
   */
  TMWTYPES_UCHAR  value[TMWCRYPTO_MAX_KEY_LENGTH];
  TMWTYPES_USHORT length;

  /* If an initialization vector is required, set it here 
   * This will be set by the TMW SCL using tmwcrypto_setIVector
   * for algorithms such as AES-GMAC that require different init vectors.
   */
  TMWTYPES_UCHAR  iv[TMWCRYPTO_MAX_IVECTOR_LENGTH];
  /* This is should be set to zero if no initialization vector */
  TMWTYPES_USHORT ivLength;

} TMWCRYPTO_KEY;

 
#ifdef __cplusplus
extern "C" {
#endif

  /* function: tmwcrypto_init 
   * purpose: initialize the cryptography interface as necessary.
   *  This will be called by each session or sector when opened. This could 
   *  be used to provide separate cryptography contexts if desired.
   *  pUserHandle - handle indicating what session or sector called this.
   * returns:
   *  handle to be used in other tmwcrypto functions
   *  TMWDEFS_NULL if failure
   */
  void *TMWDEFS_GLOBAL tmwcrypto_init(void *pUserHandle);
  
  /* function: tmwcrypto_close
   * purpose: close the cryptography interface as necessary.
   *  This will be called by each session or sector when closed. 
   *  pCryptoHandle - handle that was returned by tmwcrypto_init.
   * returns:
   *  void
   */
  void TMWDEFS_GLOBAL tmwcrypto_close(void *pCryptoHandle);

  /* function: tmwcrypto_algorithmSupport 
   * purpose: Is this algorithm supported
   * arguments:
   *  pCryptoHandle - handle returned by tmwcrypto_init
   *  algorithm - algorithm to check to see if it supported
   *   TMWCRYPTO_ALG_KEYWRAP_AES128 
   *   TMWCRYPTO_ALG_KEYWRAP_AES256 
   *   TMWCRYPTO_ALG_SYM_AES128
   *   TMWCRYPTO_ALG_SYM_AES256
   *   TMWCRYPTO_ALG_ASYM_RSAESOAEP1024
   *   TMWCRYPTO_ALG_ASYM_RSAESOAEP2048 
   *   TMWCRYPTO_ALG_SIGN_DSA_SHA1, 
   *   TMWCRYPTO_ALG_SIGN_DSA_SHA256,
   *   TMWCRYPTO_ALG_MAC_SHA1,
   *   TMWCRYPTO_ALG_MAC_SHA256,
   *   TMWCRYPTO_ALG_MAC_AESGMAC
   * returns:
   *  TMWDEFS_TRUE if supported
   *  TMWDEFS_FALSE otherwise
   */
  TMWTYPES_BOOL TMWDEFS_GLOBAL tmwcrypto_algorithmSupport(
    void *pCryptoHandle,
    TMWTYPES_ULONG algorithm);
  
  /* function: tmwcrypto_setIVector 
   * purpose: Set the initialization vector to be used with this key
   *  This is currently only required for AES-GMAC support.
   * arguments:
   *  pCryptoHandle - handle returned by tmwcrypto_init 
   *  pKey - key to set initialization vector on.  
   *  pIVector - pointer to initialization vector
   *  IVectorLength - length of initialization vector
   * returns:
   *  TMWDEFS_TRUE if successful
   *  TMWDEFS_FALSE otherwise
   */
  TMWTYPES_BOOL TMWDEFS_GLOBAL tmwcrypto_setIVector(
    void             *pCryptoHandle,  
    TMWCRYPTO_KEY    *pKey,
    TMWTYPES_UCHAR   *pIVector,
    TMWTYPES_USHORT   IVectorLength);
     
  /* function: tmwcrypto_MACValue 
   * purpose: Using the specified algorithm and key calculate the MAC value.
   * Copy requestedLength bytes into *pMACValue and
   * set *pMACValueLength to the number of bytes copied.
   * arguments:
   *  pCryptoHandle - handle returned by tmwcrypto_init
   *  algorithm - algorithm to use for creating hash value
   *    TMWCRYPTO_ALG_MAC_SHA1
   *    TMWCRYPTO_ALG_MAC_SHA256
   *    TMWCRYPTO_ALG_MAC_AESGMAC
   *  pKey - key to use 
   *  requestedLength - length of hash data requested.
   *  pData - pointer to data to hash
   *  dataLength - length of data to hash
   *  pMACValue - pointer where hashed data should be copied
   *  pMACValueLength - on return this should be set to the length of the hashed data.
   *   NOTE: While this IS the maximum size of bytes that can be copied into *pMACValue, 
   *         for this function requestedLength bytes which WILL fit should be copied.
   * returns:
   *  TMWDEFS_TRUE if successful
   *  TMWDEFS_FALSE otherwise
   */
  TMWTYPES_BOOL TMWDEFS_GLOBAL tmwcrypto_MACValue(
    void             *pCryptoHandle, 
    TMWTYPES_ULONG    algorithm,
    TMWCRYPTO_KEY    *pKey,
    TMWTYPES_USHORT   requestedLength,
    TMWTYPES_UCHAR   *pData,
    TMWTYPES_USHORT   dataLength,
    TMWTYPES_UCHAR   *pMACValue,
    TMWTYPES_USHORT  *pMACValueLength);
 
  /* function: tmwcrypto_getRandomData
   * purpose:  generate random data using the algorithm 
   *   specified in FIPS 186-2 Digital Signal Standard 
   * arguments:  
   *  pCryptoHandle - handle returned by tmwcrypto_init
   *  minLength - minimum length of random data required
   *  pBuf - pointer to where random data should be copied 
   *  pLength -  when called this is the maximum length allowed for the random data,
   *   on return this should be set to the length of the random data.
   * returns:
   *  TMWDEFS_TRUE of successful
   *  TMWDEFS_FALSE otherwise 
   */ 
  TMWTYPES_BOOL TMWDEFS_GLOBAL tmwcrypto_getRandomData(
    void            *pCryptoHandle, 
    TMWTYPES_USHORT  minLength,
    TMWTYPES_UCHAR  *pBuf,
    TMWTYPES_USHORT *pLength);
 
  /* function: tmwcrypto_genDigitalSignature (Master only)
   * purpose:  Generate a digital signature for this data  
   * arguments: 
   *  pCryptoHandle - handle returned by tmwcrypto_init
   *  algorithm - signing algorithm to use.
   *    TMWCRYPTO_ALG_SIGN_DSA_SHA1
   *    TMWCRYPTO_ALG_SIGN_DSA_SHA256
   *  pKey - key to use for signing data
   *  pData - data to generate signature for.
   *  dataLength - length of data to generate signature for
   *  pSignatureData - pointer to where signature should be copied
   *  pSignatureLength - when called this is the maximum length allowed for the 
   *   signature data, on return this should be set to the length of the signature.
   */
  TMWTYPES_BOOL TMWDEFS_GLOBAL tmwcrypto_genDigitalSignature(
    void            *pCryptoHandle, 
    TMWTYPES_ULONG   algorithm,
    TMWCRYPTO_KEY   *pKey,
    TMWTYPES_UCHAR  *pData, 
    TMWTYPES_USHORT  dataLength, 
    TMWTYPES_UCHAR  *pSignatureData,
    TMWTYPES_USHORT *pSignatureLength);

  /* function: tmwcrypto_verifySignature (Outstation only)
   * purpose: Verify the signature for the data
   * arguments:  
   *  algorithm
   *    TMWCRYPTO_ALG_SIGN_DSA_SHA1
   *    TMWCRYPTO_ALG_SIGN_DSA_SHA256
   *  pKey - key to use for verifying signature
   *  pData - data to use for verifying signature.
   *  dataLength - length of data to use for verifying signature. 
   *  pSignatureData - pointer to signature data (certification) to be verified
   *  signatureDataLength - length of the signature (certification) data.
   * returns: 
   *  TMWDEFS_TRUE if signature has been verified
   *  TMWDEFS_FALSE otherwise
   */ 
  TMWTYPES_BOOL TMWDEFS_GLOBAL tmwcrypto_verifySignature(
    void            *pCryptoHandle, 
    TMWTYPES_ULONG   algorithm,
    TMWCRYPTO_KEY   *pKey,
    TMWTYPES_UCHAR  *pData,
    TMWTYPES_USHORT  dataLength,
    TMWTYPES_UCHAR  *pSignatureData,
    TMWTYPES_USHORT  signatureLength);

  /* function: tmwcrypto_encryptData 
   * purpose:  Encrypt the data using the algorithm and key specified  
   *  NOTE the special case described in tmwcrypto_getKey() where 
   *    pKey->keyType==TMWCRYPTO_AUTH_CERT_SYM_KEY
   *    On Master this key would only be known if acting as the Authority.  
   *    If Central Authority is going to encrypt the data ask the Authority 
   *    to encrypt the data using the proper key preshared with the OS.
   * arguments: 
   *  pCryptoHandle - handle returned by tmwcrypto_init
   *  algorithm - Encryption algorithm to use 
   *    TMWCRYPTO_ALG_KEYWRAP_AES128 
   *    TMWCRYPTO_ALG_KEYWRAP_AES256
   *     NOTE: The AES Key Wrap Algorithm specified in RFC3394 is not the same 
   *     thing as AES encryption. The key wrap algorithm actually will call 
   *     the AES encryption function multiple times depending on the length
   *     the key data to be encrypted.
   *    TMWCRYPTO_ALG_SYM_AES128
   *    TMWCRYPTO_ALG_SYM_AES256
   *    TMWCRYPTO_ALG_ASYM_RSAESOAEP1024 (can encrypt up to (1024/8) – 41 = 128 – 41 = 87 bytes)
   *    TMWCRYPTO_ALG_ASYM_RSAESOAEP2048 (can encrypt up to (2048/8) – 41 = 256 – 41 = 215 bytes)
   *
   * How Much Data Can You Encrypt with RSA Keys?
   * The first parameter to the RSA_public_encrypt function is flen. This is an integer that indicates the number of bytes to encrypt. 
   * Its maximum value depends on the padding mode. For OAEP padding, recommended for all new applications, 
   * it must be less than the size of the key modulus – 41 (all in bytes).
   * To get the size of the modulus of an RSA key call the function RSA_size.
   * The modulus size is the key size in bits / 8. 
   * Thus a 1024-bit RSA key using OAEP padding can encrypt up to (1024/8) – 41 = 128 – 41 = 87 bytes.
   * A 2048-bit key can encrypt up to (2048/8) – 41 = 256 – 41 = 215 bytes
   * A 512-bit key can encrypt up to  (512/8)  - 41 =  64 - 41 = 23 bytes
   *  pKey - key to use for encrypting data
   *  pPlainData - pointer to data to be encrypted
   *  plainDataLength - length of data to be encrypted
   *  pEncryptedData - where to copy the encrypted data
   *  pEncryptedLength - when called this is the maximum length allowed for 
   *    ecnrypted data, on return this should be set to the length of the 
   *    encrypted data.
   * returns:
   *  TMWDEFS_TRUE if successful
   *  TMWDEFS_FALSE otherwise
   */ 
  TMWTYPES_BOOL TMWDEFS_GLOBAL tmwcrypto_encryptData( 
    void                *pCryptoHandle,
    TMWTYPES_ULONG       algorithm,
    TMWCRYPTO_KEY       *pKey,
    TMWTYPES_UCHAR      *pPlainData, 
    TMWTYPES_USHORT      plainDataLength, 
    TMWTYPES_UCHAR      *pEncryptedData,
    TMWTYPES_USHORT     *pEncryptedLength);

  /* function: tmwcrypto_decryptData  (Outstation only)
   * purpose: Decrypt the data using the algorithm and key specified
   * arguments:   
   *  pCryptoHandle - handle to database returned from tmwcrypto_init 
   *  algorithm - Encryption algorithm to use 
   *   TMWCRYPTO_ALG_KEYWRAP_AES128  is the only key wrap algorithm currently
   *   specified. Other values reserved for future use or vendor specific choices 
   *   NOTE: The AES Key Wrap Algorithm specified in RFC3394 is not the same 
   *    thing as AES encryption. The key wrap algorithm actually will call 
   *    the AES encryption function multiple times depending on the length
   *    the key data to be encrypted.
   *   TMWCRYPTO_ALG_SYM_AES128
   *   TMWCRYPTO_ALG_SYM_AES256
   *   TMWCRYPTO_ALG_ASYM_RSAESOAEP1024
   *   TMWCRYPTO_ALG_ASYM_RSAESOAEP2048
   *  pKey - key to use for decrypting data
   *  pEncryptedData - pointer to data to be decrypted
   *  encryptedDataLength - length of data to be decrypted
   *  pPlainValue - where to copy the decrypted (plain) data
   *  pPlainLength - when called this is the maximum length allowed for 
   *   the decrypted (plain) data, on return this should be set to the 
   *  length of the plain data.
   * returns:
   *  TMWDEFS_TRUE if successful
   *  TMWDEFS_FALSE otherwise
   */ 
  TMWTYPES_BOOL TMWDEFS_GLOBAL tmwcrypto_decryptData(
    void                *pCryptoHandle,
    TMWTYPES_ULONG       algorithm,
    TMWCRYPTO_KEY       *pKey,
    TMWTYPES_UCHAR      *pEncryptedData, 
    TMWTYPES_USHORT      encryptedDataLength, 
    TMWTYPES_UCHAR      *pPlainValue,
    TMWTYPES_USHORT     *pPlainLength);

  /* function: tmwcrypto_generateNewKey (Master only)
   * purpose: generate a new key of the specified type.
   * arguments:
   *  pCryptoHandle - handle returned by tmwcrypto_init
   *  keyType - type of key to generate 
   *      TMWCRYPTO_USER_UPDATE_KEY
   *      TMWCRYPTO_USER_CONTROL_SESSION_KEY
   *      TMWCRYPTO_USER_MONITOR_SESSION_KEY 
   *  length - length of key requested
   *  pKey - pointer to key structure to be filled in.
   * returns:
   *  TMWDEFS_TRUE if generated
   *  TMWDEFS_FALSE otherwise
   */
  TMWTYPES_BOOL TMWDEFS_GLOBAL tmwcrypto_generateNewKey(
    void              *pCryptoHandle,
    TMWCRYPTO_KEYTYPE  keyType,
    TMWTYPES_USHORT    length,
    TMWCRYPTO_KEY     *pKey);

  /* function: tmwcrypto_getKey   
   * purpose:  Get the specified key 
   * arguments:    
   *  pCryptoHandle - handle to database returned from tmwcrypto_init 
   *  keyType - enumeration indicating what key is to be retrieved
   *    TMWCRYPTO_USER_UPDATE_KEY   (master and outstation) 
   *      User Update Key for periodically sending Session Keys from
   *      master to outstation, if Secure Authentication is supported.
   *    TMWCRYPTO_USER_ASYM_PRV_KEY (master) 
   *      User Private Key - The master shall use the user’s Private Key to 
   *      digitally sign a new Update Key, if asymmetric key update is supported.
   *    TMWCRYPTO_USER_ASYM_PUB_KEY (master and outstation)
   *      User Public Key - The outstation shall use this to verify data if
   *      asymmetric key update is supported.
   *      This would be configured on or provided to master. The outstation would
   *      receive this public key from the master over the protocol 
   *    TMWCRYPTO_OS_ASYM_PUB_KEY   (master)
   *      Outstation Public Key - The master shall encrypt a new Update Key 
   *      for a user if asymmetric key update is supported.
   *    TMWCRYPTO_OS_ASYM_PRV_KEY   (outstation)
   *      Outstation Private Key - The outstation shall decrypt a new Update Key,
   *      if asymmetric key update is supported.
   *    TMWCRYPTO_AUTH_CERT_SYM_KEY (authority(master) and outstation)
   *      Authority Certification Key that was PRESHARED with Outstation to decrypt a new 
   *      Update Key if (symmetric) key update is supported.
   *      NOTE: On Master this key would only be known if acting as the Authority.  
   *      If Central Authority is going to encrypt data return TMWDEFS_TRUE with 
   *      pKey->keyType==TMWCRYPTO_AUTH_CERT_SYM_KEY
   *      When tmwcrypto_encryptData() is called ask the Authority to encrypt the
   *      data using the proper key.
   *    TMWCRYPTO_AUTH_ASYM_PUB_KEY (outstation)
   *      Authority Public Key - Outstation shall use this to validate the
   *      Public Key of a user, if asymmetric key update is supported.
   *    TMWCRYPTO_AUTH_ASYM_PRV_KEY (authority(master))
   *      Authority Private Key - Authority shall use this to sign the
   *      Public Key of a user, if asymmetric key update is supported.
   *  keyHandle - handle further specifying which key is requested
   *      If keyType specifies one of the "USER" key types this would be the user number
   *      Other keyTypes currently do not use this parameter.
   *  pKey - pointer to key structure to be filled in
   * returns: 
   *  TMWDEFS_TRUE if successful
   *  TMWDEFS_FALSE otherwise
   */ 
  TMWTYPES_BOOL TMWDEFS_GLOBAL tmwcrypto_getKey(
    void                *pCryptoHandle, 
    TMWCRYPTO_KEYTYPE    keyType,
    void *               keyHandle,
    TMWCRYPTO_KEY       *pKey);

  /* function: tmwcrypto_getAlgorithm
   * purpose: Return the algorithm for specified key.
   * arguments:
   *  pCryptoHandle - handle returned by tmwcrypto_init
   *  keyType - type of key to get algorithm for 
   *      TMWCRYPTO_USER_UPDATE_KEY
   *    NOTE: currently this is only called for UPDATE KEY.
   *  keyHandle - handle for key to get algorithm for
   * returns:
   *  TMWDEFS_TRUE if generated
   *  TMWDEFS_FALSE otherwise
   */
  TMWTYPES_UCHAR TMWDEFS_GLOBAL tmwcrypto_getAlgorithm(
    void                *pCryptoHandle,
    TMWCRYPTO_KEYTYPE    keyType,
    void *               keyHandle);

  /* function: tmwcrypto_getKeyData (Master only)
   * purpose:  Get specified key in a form that can be sent to
   *  the outstation.
   * arguments:   
   *  pCryptoHandle - handle to database returned from tmwcrypto_init 
   *  pKey - key that was retrieved by tmwcrypto_getKey 
   *    Currently the only keys that need to be retrieved are 
   *    TMWCRYPTO_USER_ASYM_PUB_KEY (master)
   *      From Jan 6 2011 DNP3 Tech SA Teleconference TC11-01-06-SA Minutes.
   *      The public key should be an octet by octet copy of the
   *      SubjectPublicKeyInfo field from the X509 certificate (RFC 5280).
   *    TMWCRYPTO_USER_UPDATE_KEY   (master)
   *    TMWCRYPTO_USER_CONTROL_SESSION_KEY (master)  
   *    TMWCRYPTO_USER_MONITOR_SESSION_KEY (master)
   *  pKeyData - pointer to buffer to be filled in with key data
   *  pKeyLength - when called this is the maximum length allowed for 
   *   key data, on return this should be set to the length of the key data.
   */
  TMWTYPES_BOOL TMWDEFS_GLOBAL tmwcrypto_getKeyData(
    void                *pCryptoHandle, 
    TMWCRYPTO_KEY       *pKey,
    TMWTYPES_UCHAR      *pKeyData,
    TMWTYPES_USHORT     *pKeyLength);



 
  /* function: tmwcrypto_setKeyData  (Outstation only)
   * purpose:  Store the specified key.
   * arguments:  
   *  pCryptoHandle - handle to database returned from tmwcrypto_init  
   *  keyType - enumeration indicating what key is to be stored
   *    Currently the only keys that will be stored by the Source Code Library are
   *    TMWCRYPTO_USER_ASYM_PUB_KEY (outstation)
   *      user public key if asymmetric key change method is supported.
   *      Will be retrieved by tmwcrypto_getKey when needed to verify signature.
   *      This key would be configured on master. This would be received by outstation
   *      over the protocol (ie DNP).
   *    TMWCRYPTO_USER_UPDATE_KEY   (outstation)
   *      Tentatively store the new update key on the outstation.
   *      Until tmwcrypto_commitUpdateKey with commit==TMWDEFS_TRUE is called
   *      the old update key must be retained in case this key change fails.
   *  keyHandle - handle further specifying which key is to be stored
   *      If keyType specifies one of the "USER" key types this would be 
   *      the user number.
   *      Other keyTypes currently do not use this parameter.
   *  pKeyData - pointer to the key to be stored
   *  keyLength - length of key to be stored
   * returns:
   *  TMWDEFS_TRUE if successful
   *  TMWDEFS_FALSE otherwise
   */
  TMWTYPES_BOOL TMWDEFS_GLOBAL tmwcrypto_setKeyData(
    void                *pCryptoHandle, 
    TMWCRYPTO_KEYTYPE    keyType,
    void *               keyHandle,
    TMWTYPES_UCHAR      *pKeyData,
    TMWTYPES_USHORT      keyLength);
       
  /* function: tmwcrypto_commitKey (Outstation only)
   * purpose: Indicate if Key stored by tmwcrypto_setKeyData
   *  should be put into use, or discarded.
   * arguments:  
   *  pCryptoHandle - handle to database returned from tmwcrypto_init  
   *  keyType - enumeration indicating what key is to be committed
   *    Currently the only key that this applies to is
   *    TMWCRYPTO_USER_UPDATE_KEY   (outstation)
   *  keyHandle - handle further specifying which key is to be stored
   *    If keyType specifies User Update Key this is the user number.
   *  commit - if TMWDEFS_TRUE the new key should be used 
   *           if TMWDEFS_FALSE the new key should be discarded
   * returns:
   *  void
   */
  void TMWDEFS_GLOBAL tmwcrypto_commitKey(
    void                *pCryptoHandle,
    TMWCRYPTO_KEYTYPE    keyType,
    void *               keyHandle,
    TMWTYPES_BOOL        commit); 

  /* function: tmwcrypto_getCertificate
   * purpose:  Get the EC 62351-8 Certificate for the user specified by this userHandle 
   * arguments:  
   *  pCryptoHandle - handle to database returned from tmwcrypto_init
   *  userNameDbHandle - handle for looking up the data for a particular user name.
   *    This handle is meaningful to the database and is intended to be more 
   *    convenient than the full user name. This handle was determined when the 
   *    user name and other data was added to the database. The actual User 
   *    Number will not be known on the master when remote Update Key 
   *    distribution is used until the outstation assigns one and informs 
   *    the master.
   * pCertData - pointer to buffer to be filled in with certification data.
   *   NOTE: This certification data must be a EC 62351-8 Certificate
   * pCertDataLength - pointer to the maximum certification length allowed and should be
   *  filled in on return with the actual length of the certification data. 
   * pCertType - pointer to certificate type to be filled in. 
   *  1 if ID certificate containing User's Public Key
   *  2 if Attribute Certificate does not contain Public Key and is used to change 
   *    role or other characteristics without providing Public Key again
   */
  TMWTYPES_BOOL TMWDEFS_GLOBAL tmwcrypto_getCertificate(
    void            *pCryptoHandle, 
    void            *userNameDbHandle,
    TMWTYPES_UCHAR  *pCertData,
    TMWTYPES_USHORT *pCertDataLength,
    TMWTYPES_UCHAR  *pCertType);

#ifdef __cplusplus
}
#endif
#endif /* TMWCRYPTO_DEFINED */