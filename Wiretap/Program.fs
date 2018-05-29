open Loader

// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

[<EntryPoint>]
let main argv = 
    // printfn "%A" argv
    load("WoW", "Payload.dll")
    0