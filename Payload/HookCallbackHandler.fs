namespace Payload

open System

type HookCallbackHandler () =
  inherit MarshalByRefObject()
  interface IHookCallbackHandler with
    member this.OnError(appName: string, processName: string, e: System.Exception): unit = 
      e.Message |> Console.WriteLine
    member this.OnErrorCaptured(appName: string, methodName: string, processName: string, errorCode: int): unit = 
      methodName |> Console.WriteLine
    member this.OnHookInstalled(appName: string, processName: string): unit = 
      String.Format("Hook installed: {0} ({1})", appName, processName) |> Console.WriteLine
    member this.OnHookInvocted(appName: string, methodName: string, processName: string): unit = 
      methodName |> Console.WriteLine
    member this.OnHookUnInstalled(appName: string, processName: string): unit = 
      raise (System.NotImplementedException())