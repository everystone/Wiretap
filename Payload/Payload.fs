namespace Payload
open Native
open EasyHook
open System
open System.Diagnostics

type Payload(context: RemoteHooking.IContext, channelName: string, processId: int, applicationName: string) = 
  let handler: HookCallbackHandler = RemoteHooking.IpcConnectClient<HookCallbackHandler>(channelName)
  let processName: string = Process.GetProcessById(processId).ProcessName
  let applicationName = applicationName
  let mutable testHook: LocalHook = null
  let createFileHook (fileName: string, desiredAddress: uint32, shareMode: uint32, securityAttr: IntPtr, creationDisp: uint32, flags: uint32, template: IntPtr) =
    ()
  let readFileHook (file: IntPtr, lpBuffer: IntPtr, nNumberOfBytesToRead: uint32, lpNumberOfBytesRead: uint32, overlapped: IntPtr) =
    (handler :> IHookCallbackHandler).OnHookInvocted(applicationName, "readFileHook", processName);
    true
  do
    (handler :> IHookCallbackHandler).OnHookInstalled(applicationName, processName);

  interface IEntryPoint
  member x.Run (context: RemoteHooking.IContext, channelName: string, processId: int, applicationName: string) =
    try
      testHook <- LocalHook.Create(LocalHook.GetProcAddress("kernel32.dll", "ReadFile"), new ReadFileDelegate(readFileHook), x)
      testHook.ThreadACL.SetExclusiveACL([|0|]);
      (handler :> IHookCallbackHandler).OnHookInstalled(applicationName, processName);
    with
      | :? System.ArgumentException as e -> Console.WriteLine(e.Message);
      | :? System.OutOfMemoryException as e -> Console.WriteLine(e.Message);

