namespace Wiretap

open System
open ByteTools

type HookCallbacks () =
  inherit HookCallbackHandler()
  override x.onData (name: string, buffer: byte[], len: int) =
    if len > 10 then
      let hexString: string = byteToHex buffer
      printfn "%s: %i %s" name len hexString