open Wiretap.Loader
open Wiretap
open System

// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

[<EntryPoint>]
let main argv = 
    // printfn "%A" argv
    let callbacks: HookCallbacks = HookCallbacks()
    load("WoW", "Wiretap.dll", callbacks) //  :> IHookCallbackHandler

    let dance: byte[] = [| 0x31uy; 0x69uy; 0x60uy; 0xC0uy; 0xE3uy; 0x88uy; 0x22uy; 0x00uy; 0x00uy; 0x00uy; 0xFFuy; 0xFFuy; 0xFFuy; 0xFFuy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; |]
    while true do
      Console.ReadKey() |> ignore
      //callbacks.TriggerCommand(dance)
      callbacks.Send dance
    0