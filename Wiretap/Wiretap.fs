namespace Wiretap
open Native
open EasyHook
open System
open System.Diagnostics
open System.Threading
open System.Runtime.InteropServices
open ByteTools

type public Wiretap(context: RemoteHooking.IContext, channelName: string, processId: int, applicationName: string) = 
  //let handler: IHookCallbackHandler = RemoteHooking.IpcConnectClient<HookCallbackHandler>(channelName) :> IHookCallbackHandler
  let handler: IHookCallbackHandler = RemoteHooking.IpcConnectClient<HookCallbackHandler>(channelName) :> IHookCallbackHandler
  let processName: string = Process.GetProcessById(processId).ProcessName
  let applicationName = applicationName
  let mutable sendHook: LocalHook = null
  let mutable connectHook: LocalHook = null

  let reportError ex =
    try
      handler.OnHookError(applicationName, processName, ex)
    with Exception as e -> ()

  let sendHookFun socket buff len flags =
    //let message = nativeIntToHexString buff len
    //handler.OnHookInvoked(applicationName, processName, "send", message);
    let bytes: byte[] = nativeintToBytes buff len
    handler.OnHookData("send", bytes, len)
    send(socket, buff, len, flags)

  let connectHookFun sock addr size =
    //let address: string = Marshal.nativeIntToHexStringringAuto(sock)
    let address: string = nativeIntToHexString addr size
    handler.OnHookInvoked(applicationName, processName, "connect", address);
    let ret: int = connect(sock, addr, size)
    if ret < 0 then
      let error: int = Marshal.GetLastWin32Error();
      handler.OnHookErrorCaptured(applicationName, processName, "connect", error);
      error
    else 
      ret


  do
    // subscribe to events from handler
    

    handler.OnHookInvoked(applicationName, processName, "ctor", "init")

  interface IEntryPoint
  member public x.Run (context: RemoteHooking.IContext, channelName: string, processId: int, applicationName: string) =
    handler.OnHookInvoked(applicationName, processName, "Run", "")
    try
      sendHook <- LocalHook.Create(LocalHook.GetProcAddress("ws2_32.dll", "send"),  new SendDelegate(sendHookFun), x)
      sendHook.ThreadACL.SetExclusiveACL([|0|]);

      connectHook <- LocalHook.Create(LocalHook.GetProcAddress("ws2_32.dll", "connect"),  new ConnectDelegate(connectHookFun), x)
      connectHook.ThreadACL.SetExclusiveACL([|0|])

      // RemoteHooking.WakeUpProcess();
      handler.OnHookInstalled(applicationName, processName)
      MessageBox(IntPtr.Zero, "Hooked!", "wiretap", 0) |> ignore
      while true do
        handler.Ping() // will throw exception if host process is down, and release hooks.
        Thread.Sleep(150)

    with
      | :? Exception as e ->
        MessageBox(IntPtr.Zero, "Releasing Hooks", "Yo", 0) |> ignore
        sendHook.Dispose()
        connectHook.Dispose()
        reportError e
        EasyHook.LocalHook.Release()
