module Payload.Native

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

[<DllImport("ws2_32.dll", CharSet = CharSet.Auto, SetLastError = true)>]
extern int sendto(IntPtr Socket, IntPtr buff, int len, SendDataFlags flags, SockAddr To, int tomlen)

[<DllImport("ws2_32.dll", CharSet = CharSet.Auto, SetLastError = true)>]
extern void WSASetLastError(int set)

 [<DllImport("WS2_32.dll", SetLastError = true)>]
 extern int connect(IntPtr s, IntPtr addr, int addrsize);


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
