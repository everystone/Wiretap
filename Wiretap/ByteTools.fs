module Wiretap.ByteTools

open System.Runtime.InteropServices

let byteToHex bytes = 
    bytes 
    |> Array.map (fun (x : byte) -> System.String.Format("{0:X2}", x))
    |> String.concat System.String.Empty

let nativeintToBytes n len =
  let managedArray: byte[] = Array.zeroCreate len
  Marshal.Copy(n, managedArray, 0, len);
  managedArray
  

let nativeIntToString n len =
  nativeintToBytes n len |> byteToHex