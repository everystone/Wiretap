namespace Wiretap

open System

[<AbstractClass>]
type HookCallbackHandler () =
  inherit MarshalByRefObject()

  // events
  //let command = new Event<_>()
  //[<CLIEvent>]
  //member this.CommandEvent = command.Publish
  //member this.TriggerCommand(data: byte[]) =
  //  command.Trigger(this, data)

  //let mutable messageQueue: byte[] = Array.zeroCreate 5
  let mutable messageQueue: byte[] = [| |]
  member this.Send buffer =
    messageQueue <- buffer

  // Internal Hook callbacks.
  interface IHookCallbackHandler with
    member this.OnHookData(hookName: string, data: byte [], len: int): unit = 
      this.onData (hookName, data, len)
    member this.Ping(): byte[] = 
      let buffer: byte[] = messageQueue
      messageQueue <- [||]
      buffer
    member this.OnHookError(appName: string, processName: string, e: System.Exception): unit = 
      printfn "Error: %s" e.Message
      raise e
    member this.OnHookErrorCaptured(appName: string, methodName: string, processName: string, errorCode: int): unit = 
      printfn "Error: %s %s %s %i" appName processName methodName errorCode
    member this.OnHookInstalled(appName: string, processName: string): unit = 
      printfn "Hook installed: %s %s" appName processName
    member this.OnHookInvoked(appName: string, processName: string, methodName: string, data: string): unit = 
      printfn "Hook %s: %s" methodName data
    member this.OnHookUnInstalled(appName: string, processName: string): unit = 
      printfn "Hook uninstalled"

  // These are implemented by user of library.
  abstract member onData: string * byte[] * int -> unit