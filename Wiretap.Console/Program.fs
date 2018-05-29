open Wiretap.Loader
open Wiretap

// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

[<EntryPoint>]
let main argv = 
    // printfn "%A" argv
    let callbacks: IHookCallbackHandler = HookCallbacks() :> IHookCallbackHandler
    load("WoW", "Wiretap.dll", callbacks)
    0