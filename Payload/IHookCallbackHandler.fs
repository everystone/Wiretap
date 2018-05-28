namespace Payload

open System

type IHookCallbackHandler  =
  abstract member OnError: appName: string * processName: string * e: Exception -> unit
  abstract member OnErrorCaptured: appName: string * methodName: string * processName: string * errorCode: int -> unit
  abstract member OnHookInstalled: appName: string * processName: string -> unit
  abstract member OnHookUnInstalled: appName: string * processName: string -> unit
  abstract member OnHookInvoked: appName: string * methodName: string * processName: string -> unit
  