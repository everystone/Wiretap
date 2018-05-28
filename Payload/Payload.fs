namespace Payload
open Native
open EasyHook
open System
open System.Diagnostics
open System.Threading

type public Payload(context: RemoteHooking.IContext, channelName: string, processId: int, applicationName: string) = 
  let handler: HookCallbackHandler = RemoteHooking.IpcConnectClient<HookCallbackHandler>(channelName)
  let processName: string = Process.GetProcessById(processId).ProcessName
  let applicationName = applicationName
  let mutable testHook: LocalHook = null
  let createFileHook (fileName: string, desiredAddress: uint32, shareMode: uint32, securityAttr: IntPtr, creationDisp: uint32, flags: uint32, template: IntPtr) =
    ()
  // https://stackoverflow.com/questions/5682927/how-can-i-pass-an-f-delegate-to-a-p-invoke-method-expecting-a-function-pointer
  let readFileHook file lpBuffer nNumberOfBytesToRead lpNumberOfBytesRead overlapped =
    (handler :> IHookCallbackHandler).OnHookInvoked(applicationName, "readFileHook", processName);
    true


  do
    (handler :> IHookCallbackHandler).OnHookInvoked(applicationName, "init..", processName)

  interface IEntryPoint
  member public x.Run (context: RemoteHooking.IContext, channelName: string, processId: int, applicationName: string) =
    (handler :> IHookCallbackHandler).OnHookInvoked(applicationName, "Run.", processName)
    try
      let d: ReadFileDelegate = new ReadFileDelegate(readFileHook)
      testHook <- LocalHook.Create(LocalHook.GetProcAddress("kernel32.dll", "ReadFile"), d, x)
      testHook.ThreadACL.SetExclusiveACL([|0|]);
      // RemoteHooking.WakeUpProcess();
      (handler :> IHookCallbackHandler).OnHookInstalled(applicationName, processName)
      while true do
        Thread.Sleep 50
        // (handler :> IHookCallbackHandler).OnHookInvoked(applicationName, "readFileHook", processName);

    with
      | :? System.ArgumentException as e -> (handler :> IHookCallbackHandler).OnError(applicationName, processName, e)
      | :? System.OutOfMemoryException as e -> (handler :> IHookCallbackHandler).OnError(applicationName, processName, e)
      | :? Exception as e -> (handler :> IHookCallbackHandler).OnError(applicationName, processName, e)
