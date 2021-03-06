﻿// The MIT License (MIT)
// 
// Copyright (c) 2015 Allan Lindqvist
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;

namespace Cowboy.Sockets.Experimental
{
    public class RioTcpListener : RioConnectionOrientedSocketPool
    {
        internal IntPtr _listenerSocket;
        internal IntPtr _listenIocp;
        public Action<RioConnectionOrientedSocket> OnAccepted;

        public unsafe RioTcpListener(RioFixedBufferPool sendPool, RioFixedBufferPool revicePool, uint socketCount, uint maxOutstandingReceive = 1024, uint maxOutstandingSend = 1024)
            : base(sendPool, revicePool, socketCount, maxOutstandingReceive, maxOutstandingSend, (maxOutstandingReceive + maxOutstandingSend) * socketCount)
        {
            if ((_listenerSocket = WinSock.WSASocket(ADDRESS_FAMILIES.AF_INET, SOCKET_TYPE.SOCK_STREAM, PROTOCOL.IPPROTO_TCP, IntPtr.Zero, 0, SOCKET_FLAGS.REGISTERED_IO | SOCKET_FLAGS.WSA_FLAG_OVERLAPPED)) == IntPtr.Zero)
                WinSock.ThrowLastWSAError();

            int True = -1;
            UInt32 dwBytes = 0;

            WinSock.setsockopt(_listenerSocket, WinSock.IPPROTO_TCP, WinSock.TCP_NODELAY, (char*)&True, 4);
            WinSock.WSAIoctlGeneral(_listenerSocket, WinSock.SIO_LOOPBACK_FAST_PATH, &True, 4, null, 0, out dwBytes, IntPtr.Zero, IntPtr.Zero);

            if ((_listenIocp = Kernel32.CreateIoCompletionPort(_listenerSocket, _listenIocp, 0, 1)) == IntPtr.Zero)
                Kernel32.ThrowLastError();

            Thread ListenIocpThread = new Thread(ListenIocpComplete);
            ListenIocpThread.IsBackground = true;
            ListenIocpThread.Start();
        }

        unsafe void BeginAccept(RioConnectionOrientedSocket acceptSocket)
        {
            int recived = 0;
            acceptSocket.ResetOverlapped();
            if (!RioStatic.AcceptEx(_listenerSocket, acceptSocket.Socket, acceptSocket._adressBuffer, 0, sizeof(sockaddr_in) + 16, sizeof(sockaddr_in) + 16, out recived, acceptSocket._overlapped))
            {
                if (WinSock.WSAGetLastError() != 997) // error_io_pending
                    WinSock.ThrowLastWSAError();
            }
            else
                OnAccepted(acceptSocket);
        }

        public void Listen(IPEndPoint localEP, int backlog)
        {
            in_addr inAddress = new in_addr();
            inAddress.s_b1 = localEP.Address.GetAddressBytes()[0];
            inAddress.s_b2 = localEP.Address.GetAddressBytes()[1];
            inAddress.s_b3 = localEP.Address.GetAddressBytes()[2];
            inAddress.s_b4 = localEP.Address.GetAddressBytes()[3];

            sockaddr_in sa = new sockaddr_in();
            sa.sin_family = ADDRESS_FAMILIES.AF_INET;
            sa.sin_port = WinSock.htons((ushort)localEP.Port);
            //Imports.ThrowLastWSAError();
            sa.sin_addr = inAddress;

            unsafe
            {
                if (WinSock.bind(_listenerSocket, ref sa, sizeof(sockaddr_in)) == WinSock.SOCKET_ERROR)
                    WinSock.ThrowLastWSAError();
            }

            if (WinSock.listen(_listenerSocket, backlog) == WinSock.SOCKET_ERROR)
                WinSock.ThrowLastWSAError();

            foreach (var s in allSockets)
                BeginAccept(s);
        }

        unsafe void ListenIocpComplete(object o)
        {
            IntPtr lpNumberOfBytes;
            IntPtr lpCompletionKey;
            RioNativeOverlapped* lpOverlapped = stackalloc RioNativeOverlapped[1];
            int lpcbTransfer;
            int lpdwFlags;

            while (true)
            {
                if (Kernel32.GetQueuedCompletionStatusRio(_listenIocp, out lpNumberOfBytes, out lpCompletionKey, out lpOverlapped, -1))
                {
                    if (WinSock.WSAGetOverlappedResult(_listenerSocket, lpOverlapped, out lpcbTransfer, false, out lpdwFlags))
                    {
                        var res = allSockets[lpOverlapped->SocketIndex];
                        activeSockets.TryAdd(res.GetHashCode(), res);
                        OnAccepted(res);
                    }
                    else {
                        //recycle socket
                    }
                }
                else {
                    var error = Marshal.GetLastWin32Error();

                    if (error != 0 && error != 64) //connection no longer available
                        throw new Win32Exception(error);

                }
            }
        }

        protected override unsafe void SocketIocpComplete(object o)
        {
            IntPtr lpNumberOfBytes;
            IntPtr lpCompletionKey;
            RioNativeOverlapped* lpOverlapped = stackalloc RioNativeOverlapped[1];

            while (true)
            {
                if (Kernel32.GetQueuedCompletionStatusRio(socketIocp, out lpNumberOfBytes, out lpCompletionKey, out lpOverlapped, -1))
                    BeginAccept(allSockets[lpOverlapped->SocketIndex]);
                else
                    Kernel32.ThrowLastError();
            }
        }

        public override void Dispose()
        {
            Kernel32.CloseHandle(_listenIocp);
            WinSock.closesocket(_listenerSocket);
            base.Dispose();
        }
    }
}
