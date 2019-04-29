# InterceptionSharp
C# Library for the Interception driver by [oblitum](https://github.com/oblitum)

## Using
1. Install the Interception driver here https://github.com/oblitum/Interception
2. Make sure interception.dll is in the same folder as your executable or in your Windows directory
3. Enable the 'unsafe' option in your C# project

## Example
```csharp
using (Process p = Process.GetCurrentProcess())
  p.PriorityClass = ProcessPriorityClass.High;

IntPtr context;
int device;

Interception.InterceptionStroke stroke = new Interception.InterceptionStroke();

context = Interception.interception_create_context();

Interception.InterceptionPredicate del = Interception.interception_is_keyboard;
Interception.interception_set_filter(
  context,
  del,
  ((ushort)Interception.InterceptionFilterKeyState.INTERCEPTION_FILTER_KEY_DOWN | (ushort)Interception.InterceptionFilterKeyState.INTERCEPTION_FILTER_KEY_UP));

while (Interception.interception_receive(context, device = Interception.interception_wait(context), ref stroke, 1) > 0)
{
  byte[] strokeBytes = Interception.getBytes(stroke);
  Interception.InterceptionKeyStroke kstroke = Interception.ByteArrayToStructure<Interception.InterceptionKeyStroke>(strokeBytes);
  if (kstroke.code == (ushort)ScanCode.SCANCODE_X)
  {
    kstroke.code = (ushort)ScanCode.SCANCODE_Y;
  }
  strokeBytes = Interception.getBytes(kstroke);
  Interception.interception_send(context, device, strokeBytes, 1);
}

Interception.interception_destroy_context(context);
```
