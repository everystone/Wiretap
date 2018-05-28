namespace Payload

open System

type HookCallbackHandler () =
  inherit MarshalByRefObject()
  interface IHookCallbackHandler with
    member this.OnError(appName: string, processName: string, e: System.Exception): unit = 
      printfn "Error: %s" e.Message
      raise e
    member this.OnErrorCaptured(appName: string, methodName: string, processName: string, errorCode: int): unit = 
      printfn "Error: %s %s %s %i" appName processName methodName errorCode
    member this.OnHookInstalled(appName: string, processName: string): unit = 
      printfn "Hook installed: %s %s" appName processName
    member this.OnHookInvoked(appName: string, methodName: string, processName: string): unit = 
      printfn "Hook invoked: %s" methodName
    member this.OnHookUnInstalled(appName: string, processName: string): unit = 
      printfn "Hook uninstalled"