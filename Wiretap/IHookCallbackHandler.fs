namespace Wiretap

open System

type IHookCallbackHandler  =
  abstract member OnError: appName: string * processName: string * e: Exception -> unit
  abstract member OnErrorCaptured: appName: string * processName: string * methodName: string * errorCode: int -> unit
  abstract member OnHookInvoked: appName: string * processName: string * methodName: string * data: string -> unit
  abstract member OnHookInstalled: appName: string * processName: string -> unit
  abstract member OnHookUnInstalled: appName: string * processName: string -> unit
  abstract member Ping: unit -> unit