// 
// Copyright (c) 2004,2005 Jaroslaw Kowalski <jkowalski@users.sourceforge.net>
// 
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
// 
// * Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer. 
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution. 
// 
// * Neither the name of the Jaroslaw Kowalski nor the names of its 
//   contributors may be used to endorse or promote products derived from this
//   software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
// THE POSSIBILITY OF SUCH DAMAGE.
// 

using System;
using System.Collections.Specialized;

using NLog.Viewer.Configuration;

namespace NLog.Viewer.Receivers
{
	public class LogReceiverFactory
	{
        private static StringToLogEventReceiverInfoMap _name2receiver = new StringToLogEventReceiverInfoMap();
        private static LogEventReceiverInfoCollection _receivers = new LogEventReceiverInfoCollection();

        public static LogEventReceiverInfoCollection Receivers
        {
            get { return _receivers; }
        }

        static LogReceiverFactory()
        {
            LogEventReceiverInfo ri;
            
            // UDP receiver
            ri = new LogEventReceiverInfo();
            ri.Name = "UDP";
            ri.Description = "Receives events from the network using UDP protocol";
            ri.Type = typeof(UDPEventReceiver);
            AddReceiverInfo(ri);

            // TCP receiver
            ri = new LogEventReceiverInfo();
            ri.Name = "TCP";
            ri.Description = "Receives events from the network using TCP protocol";
            ri.Type = typeof(TCPEventReceiver);

            AddReceiverInfo(ri);
        }

        public static void AddReceiverInfo(LogEventReceiverInfo ri)
        {
            _name2receiver[ri.Name] = ri;
            _receivers.Add(ri);
        }

        public static LogEventReceiver CreateLogReceiver(string type, ReceiverParameterCollection parameters)
        {
            LogEventReceiverInfo ri = _name2receiver[type];
            if (ri == null)
                throw new ArgumentException("Unknown receiver type: " + type);

            object o = Activator.CreateInstance(ri.Type, new object[] { parameters });
            return (LogEventReceiver)o;
        }
	}
}