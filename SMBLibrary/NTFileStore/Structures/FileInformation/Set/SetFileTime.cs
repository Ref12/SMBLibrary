/* Copyright (C) 2017 Tal Aloni <tal.aloni.il@gmail.com>. All rights reserved.
 * 
 * You can redistribute this program and/or modify it under the terms of
 * the GNU Lesser Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 */
using System;

namespace SMBLibrary
{
    /// <summary>
    /// [MS-FSCC] When setting file attributes, a value of -1 indicates to the server that it MUST NOT change this attribute for all subsequent operations on the same file handle.
    /// </summary>
    public struct SetFileTime
    {
        private long m_value;
        private DateTime? m_time;

        public SetFileTime(long value)
        {
            m_value = value;
            m_time = null;
        }

        public SetFileTime(DateTime? time)
        {
            m_value = 0;
            m_time = time;
        }

        public long ToFileTimeUtc()
        {
            return m_time?.ToFileTimeUtc() ?? m_value;
        }

        public DateTime? Time
        {
            get
            {
                return m_time;
            }
            set
            {
                m_value = 0;
                m_time = value;
            }
        }

        public static SetFileTime FromFileTimeUtc(long span)
        {
            if (span > 0)
            {
                DateTime value = DateTime.FromFileTimeUtc(span);
                return new SetFileTime(value);
            }
            else
            {
                return new SetFileTime(span);
            }
        }

        public static implicit operator DateTime?(SetFileTime setTime)
        {
            return setTime.Time;
        }

        public static implicit operator SetFileTime(DateTime? time)
        {
            return new SetFileTime(time);
        }
    }
}
