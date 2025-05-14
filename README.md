# ExternalCall

This is a test project demonstrating how to call functions from an external native DLL (`TestLib.dll`) in a C# application.

## Project Structure

- `ExternalCall/`  
  Main C# project that loads and calls functions from the native DLL.
- `TestLib/`  
  C++ project that builds the native DLL (`TestLib.dll`).

## How to Build

1. **Build the Native DLL**
   - Open `TestLib/TestLib.vcxproj` in Visual Studio.
   - Build the project in `x64/Debug` mode.
   - The output `TestLib.dll` will be in `x64/Debug/`.

2. **Build the C# Project**
   - Open `ExternalCall/ExternalCall.csproj` in Visual Studio.
   - Build the project (ensure the platform is set to x64 to match the DLL).
   - The output will be in `ExternalCall/bin/Debug/net8.0/`.

3. **Run the Application**
   - Ensure `TestLib.dll` is present in the same directory as `ExternalCall.exe` (`ExternalCall/bin/Debug/net8.0/`).
   - Run `ExternalCall.exe`.

## Methods Used in Program.cs

- **ExtLibFunctions.TestLogCallback**: Calls the external DLL function with a callback to log messages at a specified trace level. The callback receives the trace level and message from the native code.

- **ExtLibFunctions.TestSale**: Creates a `SaleRequest` struct and calls the external DLL function to process a sale, returning a `SaleResponse` struct with the sale ID and amount.

- **ExtLibFunctions.ModifyInputAnsi**: Calls the external DLL function to modify an ANSI string and returns the modified result as a string.

- **ExtLibFunctions.ModifyInputUni**: Calls the external DLL function to modify a Unicode string and returns the modified result as a string.

## Notes

- The C# project uses P/Invoke to call functions from the native DLL.
- Make sure the build configurations (x64/Debug) match between the C++ and C# projects.
- You may need to rebuild the native DLL if you change its exported functions.

## License

This project is for testing and demonstration purposes.
