module Loader

open EasyHook
open System.IO
open System.Reflection
open System.Diagnostics
open Payload
open System.Threading
open System
// https://easyhook.github.io/tutorials/remotefilemonitor.html

let load (processName: string, dll: string) =
  let path: string = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), dll);
  let mutable channel: string = null;
  let p: Process[] = Process.GetProcessesByName(processName)
  if p.Length > 0 then
    try
      let id: int = p.[0].Id;
      RemoteHooking.IpcCreateServer<HookCallbackHandler>(&channel, System.Runtime.Remoting.WellKnownObjectMode.Singleton) |> ignore
      let objects : obj[] = [|channel; id; processName|]
      RemoteHooking.Inject(id, InjectionOptions.DoNotRequireStrongName, path, path, objects)
    with
    | :? Exception as e -> printfn "Exception: %s" e.Message
    Console.ReadKey() |> ignore
