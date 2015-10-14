namespace FSharpWpfMvvmTemplate.ViewModel

open System.Security.Cryptography
open Elliptic

type EccViewModel() =
    inherit ViewModelBase()

    let mutable aliceRandomBytes = [||]
    let mutable aliceRandomText = ""

    let mutable alicePrivateBytes = [||]
    let mutable alicePrivateKeyText = ""

    let mutable alicePublicBytes = [||]
    let mutable alicePublicKeyText = ""

    let mutable bobRandomBytes = [||]
    let mutable bobRandomText = ""

    let mutable bobPrivateBytes = [||]
    let mutable bobPrivateKeyText = ""

    let mutable bobPublicBytes = [||]
    let mutable bobPublicKeyText = ""

    let mutable aliceBobSharedBytes = [||]
    let mutable aliceBobSharedKeyText = ""

    let mutable bobAliceSharedBytes = [||]
    let mutable bobAliceSharedKeyText = ""

    let ByteToHex bytes =
      bytes
      |> Array.map (fun (x : byte) -> System.String.Format("{0:X2}", x))
      |> String.concat System.String.Empty

    member x.GenAliceRandomCommand =
        new RelayCommand ((fun canExecute -> true),
            (fun _ ->
                alicePrivateBytes <- [||]
                x.AlicePrivateKeyText <- ""

                alicePublicBytes <- [||]
                x.AlicePublicKeyText <- ""

                aliceBobSharedBytes <- [||]
                bobAliceSharedBytes <- [||]

                x.AliceBobSharedKeyText <- ""
                x.BobAliceSharedKeyText <- ""

                aliceRandomBytes <- Array.zeroCreate 32
                RNGCryptoServiceProvider.Create().GetBytes(aliceRandomBytes)
                x.AliceRandomText <- ByteToHex aliceRandomBytes))

    member x.AliceRandomText
        with get() = aliceRandomText
        and set value =
            aliceRandomText <- value
            x.OnPropertyChanged "AliceRandomText"

    member x.ComputeAlicePrivateFromRandomCommand =
        new RelayCommand ((fun canExecute -> true),
            (fun _ ->
                 if (aliceRandomBytes <> [||]) then
                     alicePrivateBytes <- Curve25519.ClampPrivateKey(aliceRandomBytes)
                     x.AlicePrivateKeyText <- ByteToHex alicePrivateBytes))

    member x.AlicePrivateKeyText
        with get() = alicePrivateKeyText
        and set value =
            alicePrivateKeyText <- value
            x.OnPropertyChanged "AlicePrivateKeyText"

    member x.ComputeAlicePublicFromPrivateCommand =
        new RelayCommand ((fun canExecute -> true),
            (fun _ ->
                 if (alicePrivateBytes <> [||]) then
                     alicePublicBytes <- Curve25519.GetPublicKey(alicePrivateBytes)
                     x.AlicePublicKeyText <- ByteToHex alicePublicBytes))

    member x.AlicePublicKeyText
        with get() = alicePublicKeyText
        and set value =
            alicePublicKeyText <- value
            x.OnPropertyChanged "AlicePublicKeyText"

    member x.GenBobRandomCommand =
        new RelayCommand ((fun canExecute -> true),
            (fun _ ->
                bobPrivateBytes <- [||]
                x.BobPrivateKeyText <- ""

                bobPublicBytes <- [||]
                x.BobPublicKeyText <- ""

                aliceBobSharedBytes <- [||]
                bobAliceSharedBytes <- [||]

                x.AliceBobSharedKeyText <- ""
                x.BobAliceSharedKeyText <- ""

                bobRandomBytes <- Array.zeroCreate 32
                RNGCryptoServiceProvider.Create().GetBytes(bobRandomBytes)
                x.BobRandomText <- ByteToHex bobRandomBytes))

    member x.BobRandomText
        with get() = bobRandomText
        and set value =
            bobRandomText <- value
            x.OnPropertyChanged "BobRandomText"

    member x.ComputeBobPrivateFromRandomCommand =
        new RelayCommand ((fun canExecute -> true),
            (fun _ ->
                 if (bobRandomBytes <> [||]) then
                     bobPrivateBytes <- Curve25519.ClampPrivateKey(bobRandomBytes)
                     x.BobPrivateKeyText <- ByteToHex bobPrivateBytes))

    member x.BobPrivateKeyText
        with get() = bobPrivateKeyText
        and set value =
            bobPrivateKeyText <- value
            x.OnPropertyChanged "BobPrivateKeyText"

    member x.ComputeBobPublicFromPrivateCommand =
        new RelayCommand ((fun canExecute -> true),
            (fun _ ->
                 if (bobPrivateBytes <> [||]) then
                     bobPublicBytes <- Curve25519.GetPublicKey(bobPrivateBytes)
                     x.BobPublicKeyText <- ByteToHex bobPublicBytes))

    member x.BobPublicKeyText
        with get() = bobPublicKeyText
        and set value =
            bobPublicKeyText <- value
            x.OnPropertyChanged "BobPublicKeyText"

    member x.ComputeAliceBobSharedCommand =
        new RelayCommand ((fun canExecute -> true),
            (fun _ ->
            if (alicePrivateBytes <> [||] && bobPublicBytes <> [||]) then
              aliceBobSharedBytes <- Curve25519.GetSharedSecret(alicePrivateBytes, bobPublicBytes)
              x.AliceBobSharedKeyText <- ByteToHex aliceBobSharedBytes))

    member x.AliceBobSharedKeyText
        with get() = aliceBobSharedKeyText
        and set value =
            aliceBobSharedKeyText <- value
            x.OnPropertyChanged "AliceBobSharedKeyText"

    member x.ComputeBobAliceSharedCommand =
        new RelayCommand ((fun canExecute -> true),
            (fun _ ->
            if (bobPrivateBytes <> [||] && alicePublicBytes <> [||]) then
              bobAliceSharedBytes <- Curve25519.GetSharedSecret(bobPrivateBytes, alicePublicBytes)
              x.BobAliceSharedKeyText <- ByteToHex bobAliceSharedBytes))

    member x.BobAliceSharedKeyText
        with get() = bobAliceSharedKeyText
        and set value =
            bobAliceSharedKeyText <- value
            x.OnPropertyChanged "BobAliceSharedKeyText"
