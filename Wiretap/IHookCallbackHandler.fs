namespace Wiretap

open System

type IHookCallbackHandler =
  abstract member OnHookError: appName: string * processName: string * e: Exception -> unit
  abstract member OnHookErrorCaptured: appName: string * processName: string * methodName: string * errorCode: int -> unit
  abstract member OnHookInvoked: appName: string * processName: string * methodName: string * data: string -> unit
  abstract member OnHookInstalled: appName: string * processName: string -> unit
  abstract member OnHookUnInstalled: appName: string * processName: string -> unit
  abstract member Ping: unit -> unit

  abstract member OnHookData: hookName: string * data: byte[] * len: int -> unit