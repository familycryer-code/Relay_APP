/*****************************************************************************/
/* Triangle MicroWorks, Inc.                         Copyright(c) 1997-2011  */
/*****************************************************************************/
/*                                                                           */
/* This file is the property of:                                             */
/*                                                                           */
/*                       Triangle MicroWorks, Inc.                           */
/*                      Raleigh, North Carolina USA                          */
/*                       www.TriangleMicroWorks.com                          */
/* (919) 870 - 6615                                                          */
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
/* file: WinThreading.h
 * description:  Critical section code
 */
#pragma once

class Lockable
{
protected:
	Lockable(const Lockable&) {}
	Lockable& operator=(const Lockable&) {return *this;}

public:
	Lockable() {}
	virtual ~Lockable() {}

	virtual void lock() = 0;
	virtual void unlock() = 0;
};

class AutoCriticalSection : public Lockable
{
protected:
	CRITICAL_SECTION	m_sec;

	AutoCriticalSection(const AutoCriticalSection&);
	AutoCriticalSection& operator=(const AutoCriticalSection&);

public:
	AutoCriticalSection()
  {
    InitializeCriticalSection(&m_sec);
  }
	virtual ~AutoCriticalSection()
  {
    DeleteCriticalSection(&m_sec);
  }

  virtual void lock()
  {
    EnterCriticalSection(&m_sec);
  }

  virtual void unlock()
  {
    LeaveCriticalSection(&m_sec);
  }
};

class Guard
{
private:
	Lockable	&m_lock;

	Guard();
	Guard(const Guard&);
	Guard& operator=(const Guard&);

  friend class UnGuard;
public:
	Guard(Lockable &lock)
		: m_lock(lock)
	{
		m_lock.lock();
	}
	~Guard()
	{
		m_lock.unlock();
	}
};

class UnGuard
{
private:
	Lockable	&m_lock;

	UnGuard();
	UnGuard(const UnGuard&);
	UnGuard& operator=(const UnGuard&);

public:
	UnGuard(Guard &guard)
		: m_lock(guard.m_lock)
	{
		m_lock.unlock();
	}
	~UnGuard()
	{
		m_lock.lock();
	}
};

typedef Guard CriticalSectionLock;

