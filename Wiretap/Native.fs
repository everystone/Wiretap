module Wiretap.Native

open System.Runtime.InteropServices
open System
open Payload

[<DllImport("kernel32.dll", SetLastError = true)>]
extern bool ReadProcessMemory(IntPtr hProcess, uint32 lpBaseAddress, byte[] buffer, int size, IntPtr& lpNumberOfBytesRead)
[<DllImport("kernel32.dll", SetLastError = true)>]
extern bool WriteProcessMemory(IntPtr hProcess, uint32 lpBaseAddress, byte[] buffer, int size, IntPtr& lpNumberOfBytesWritten)
[<DllImport("kernel32.dll", SetLastError = true)>]
extern IntPtr OpenProcess(uint32 dwDesiredAccess, bool bInheritHandle, int dwProcessId)

[<Flags>]
type SendDataFlags =
    | None  = 0
    | DontRoute = 1
    | OOB   = 2

type SockAddr = struct
  val Family: int16
  val Port: uint16
  val IPAddress: System.Net.IPAddress
end



//[<UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)>]
//delegate bool ReadFile_Delegate(
//    IntPtr hFile,
//    IntPtr lpBuffer,
//    uint nNumberOfBytesToRead,
//    out uint lpNumberOfBytesRead,
//    IntPtr lpOverlapped);


[<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)>]
type ReadFileDelegate = delegate of nativeint * nativeint * uint32 * uint32 * nativeint -> [<MarshalAs(UnmanagedType.Bool)>] bool


[<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)>]
type SendToDelegate =
  delegate of nativeint * nativeint * int * SendDataFlags * SockAddr * int -> int

[<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)>]
type ConnectDelegate =
  delegate of nativeint * nativeint * int -> int

[<UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)>]
type SendDelegate =
  delegate of nativeint * nativeint * int * int -> int


[<DllImport("Ws2_32.dll", CharSet = CharSet.Ansi)>]
extern uint32 inet_addr(string cp);

[<DllImport("Ws2_32.dll")>]
extern uint16 htons(uint16 hostshort);

[<DllImport("ws2_32.dll", CharSet = CharSet.Unicode, SetLastError = true)>]
extern IntPtr socket(int16 af, int16 socket_type, int protocol);

[<DllImport("Ws2_32.dll")>]
extern int send(IntPtr s, IntPtr buf, int len, int flags);

[<DllImport("Ws2_32.dll")>]
extern int recv(IntPtr s, IntPtr buf, int len, int flags);

[<DllImport("ws2_32.dll", CharSet = CharSet.Unicode, SetLastError = true)>]
extern int closesocket(IntPtr s);


[<DllImport("ws2_32.dll", CharSet = CharSet.Auto, SetLastError = true)>]
extern int sendto(IntPtr Socket, IntPtr buff, int len, SendDataFlags flags, SockAddr To, int tomlen)

[<DllImport("ws2_32.dll", CharSet = CharSet.Auto, SetLastError = true)>]
extern void WSASetLastError(int set)

[<DllImport("WS2_32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)>]
extern int connect(nativeint sock, nativeint addr, int addrsize);

[<DllImport("user32.dll", CharSet=CharSet.Auto)>]
extern int MessageBox(nativeint hWnd, String text, String caption, int options);