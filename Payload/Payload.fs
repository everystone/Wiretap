namespace Payload
open Native
open EasyHook
open System
open System.Diagnostics
open System.Threading
open System.Runtime.InteropServices

type public Payload(context: RemoteHooking.IContext, channelName: string, processId: int, applicationName: string) = 
  let handler: HookCallbackHandler = RemoteHooking.IpcConnectClient<HookCallbackHandler>(channelName)
  let processName: string = Process.GetProcessById(processId).ProcessName
  let applicationName = applicationName
  let mutable testHook: LocalHook = null
  //let createFileHook (fileName: string, desiredAddress: uint32, shareMode: uint32, securityAttr: IntPtr, creationDisp: uint32, flags: uint32, template: IntPtr) =
  //  ()
  // https://stackoverflow.com/questions/5682927/how-can-i-pass-an-f-delegate-to-a-p-invoke-method-expecting-a-function-pointer
  let readFileHook file lpBuffer (nNumberOfBytesToRead: uint32) (lpNumberOfBytesRead: uint32) overlapped =
    try
      let size: int = Convert.ToInt32(nNumberOfBytesToRead)
      //let data: string = System.Text.Encoding.ASCII.GetString(lpBuffer, 0, 10)
      //let data: string = sprintf "size: %A" size
      //let message: string = sprintf "read %A bytes: %A" size data
      let message: string = Marshal.PtrToStringAuto(lpBuffer)
      (handler :> IHookCallbackHandler).OnHookInvoked(applicationName, processName, "readFileHook", message);
      true
    with
    | :? Exception as e ->
      (handler :> IHookCallbackHandler).OnHookInvoked(applicationName, processName, "readFileHook", e.Message)
      false

  let sendToHook socket buff len flags too tomlen =
    let message: string = Marshal.PtrToStringAuto(buff)
    (handler :> IHookCallbackHandler).OnHookInvoked(applicationName, processName, "sendTo", message);
    0

  let connectHook sock addr size =
    //let message: string = Marshal.PtrToStringAuto(buff)
    (handler :> IHookCallbackHandler).OnHookInvoked(applicationName, processName, "connect", "connect");
    0

  do
    (handler :> IHookCallbackHandler).OnHookInvoked(applicationName, processName, "ctor", "init")

  interface IEntryPoint
  member public x.Run (context: RemoteHooking.IContext, channelName: string, processId: int, applicationName: string) =
    (handler :> IHookCallbackHandler).OnHookInvoked(applicationName, processName, "Run", "")
    try
      testHook <- LocalHook.Create(LocalHook.GetProcAddress("ws2_32.dll", "sendto"),  new SendToDelegate(sendToHook), x)
      testHook.ThreadACL.SetExclusiveACL([|0|]);

      let connect: LocalHook = LocalHook.Create(LocalHook.GetProcAddress("ws2_32.dll", "connect"),  new ConnectDelegate(connectHook), x)
      connect.ThreadACL.SetExclusiveACL([|0|])

      // RemoteHooking.WakeUpProcess();
      (handler :> IHookCallbackHandler).OnHookInstalled(applicationName, processName)
      while true do
        Thread.Sleep 50
        // (handler :> IHookCallbackHandler).OnHookInvoked(applicationName, "readFileHook", processName);

    with
      | :? System.ArgumentException as e -> (handler :> IHookCallbackHandler).OnError(applicationName, processName, e)
      | :? System.OutOfMemoryException as e -> (handler :> IHookCallbackHandler).OnError(applicationName, processName, e)
      | :? Exception as e -> (handler :> IHookCallbackHandler).OnError(applicationName, processName, e)
