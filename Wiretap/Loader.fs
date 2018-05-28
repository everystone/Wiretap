module Loader

open EasyHook
open System.IO
open System.Reflection
open System.Diagnostics
open Payload
open System.Threading
open System
// https://easyhook.github.io/tutorials/remotefilemonitor.html

let load (app: string, dll: string) =
  let path: string = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), dll);
  let mutable channel: string = null;
  let p: Process[] = Process.GetProcessesByName(app)
  if p.Length > 0 then
    let id: int = p.[0].Id;
    RemoteHooking.IpcCreateServer<HookCallbackHandler>(&channel, System.Runtime.Remoting.WellKnownObjectMode.Singleton) |> ignore
    let objects : obj[] = [|channel; id; app|]
    RemoteHooking.Inject(id, InjectionOptions.DoNotRequireStrongName, path, path, objects)
    Console.WriteLine("Injected");
    Thread.Sleep(30000);
  else 
    ()
