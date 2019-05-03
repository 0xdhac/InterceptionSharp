using System;
using System.Runtime.InteropServices;

public class Interception
{
	public const short INTERCEPTION_MAX_KEYBOARD = 10;
	public const short INTERCEPTION_MAX_MOUSE = 10;
	public const short INTERCEPTION_MAX_DEVICE = (INTERCEPTION_MAX_KEYBOARD + INTERCEPTION_MAX_MOUSE);

	public enum KeyState : ushort
	{
		KeyDown = 0x00,
		KeyUp = 0x01,
		E0 = 0x02,
		E1 = 0x04,
		SetLed = 0x08,
		TermSrv_Shadow = 0x10,
		TermSrv_VkPacket = 0x20
	};

	public enum FilterKeyState : ushort
	{
		None = 0x0000,
		All = 0xFFFF,
		KeyDown = KeyState.KeyUp,
		KeyUp = KeyState.KeyUp << 1,
		E0 = KeyState.E0 << 1,
		E1 = KeyState.E1 << 1,
		SetLed = KeyState.SetLed << 1,
		TermSrv_Shadow = KeyState.TermSrv_Shadow << 1,
		TermSrv_VkPacket = KeyState.TermSrv_VkPacket << 1
	};

	public enum MouseState : ushort
	{
		LeftDown = 0x001,
		LeftUp = 0x002,
		RightDown = 0x004,
		RightUp = 0x008,
		MiddleDown = 0x010,
		MiddleUp = 0x020,

		Button1Down = LeftDown,
		Button1Up = LeftUp,
		Button2Down = RightDown,
		Button2Up = RightUp,
		Button3Down = MiddleDown,
		Button3Up = MiddleUp,

		Button4Down = 0x040,
		Button4Up = 0x080,
		Button5Down = 0x100,
		Button5Up = 0x200,

		MouseWheel = 0x400,
		MouseHWheel = 0x800
	};

	public enum FilterMouseState : ushort
	{
		None = 0x0000,
		All = 0xFFFF,

		LeftDown = MouseState.LeftDown,
		LeftUp = MouseState.LeftUp,
		RightDown = MouseState.RightDown,
		RightUp = MouseState.RightUp,
		MiddleDown = MouseState.MiddleDown,
		MiddleUp = MouseState.MiddleUp,

		Button1Down = MouseState.Button1Down,
		Button1Up = MouseState.Button1Up,
		Button2Down = MouseState.Button2Down,
		Button2Up = MouseState.Button2Up,
		Button3Down = MouseState.Button3Down,
		Button3Up = MouseState.Button3Up,

		Button4Down = MouseState.Button4Down,
		Button4Up = MouseState.Button4Up,
		Button5Down = MouseState.Button5Down,
		Button5Up = MouseState.Button5Up,

		MouseWheel = MouseState.MouseWheel,
		MouseHWheel = MouseState.MouseHWheel,

		MouseMove = 0x1000
	};

	public enum MouseFlag
	{
		Relative = 0x000,
		Absolute = 0x001,
		VirtualDesktop = 0x002,
		AttributesChanged = 0x004,
		Move_NoCoalesce = 0x008,
		TermSrv_Src_Shadow = 0x100
	};

	public struct MouseStroke
	{
		public ushort state;
		public ushort flags;
		public short rolling;
		public int x;
		public int y;
		public uint information;

		public static implicit operator Stroke(MouseStroke d)
		{
			byte[] strokeBytes = getBytes(d);
			return ByteArrayToStructure<Stroke>(strokeBytes);
		}
	};

	public struct KeyStroke
	{
		public ushort code;
		public ushort state;
		public uint information;

		public static implicit operator Stroke(KeyStroke d)
		{
			byte[] strokeBytes = getBytes(d);
			return ByteArrayToStructure<Stroke>(strokeBytes);
		}
	};

	public unsafe struct Stroke
	{
		public fixed byte data[18];

		public static implicit operator MouseStroke(Stroke d)
		{
			byte[] strokeBytes = getBytes(d);
			return ByteArrayToStructure<MouseStroke>(strokeBytes);
		}

		public static implicit operator KeyStroke(Stroke d)
		{
			byte[] strokeBytes = getBytes(d);
			return ByteArrayToStructure<KeyStroke>(strokeBytes);
		}
	};

	[DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr interception_create_context();

	[DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern void interception_destroy_context(IntPtr context);

	[DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int interception_get_precedence(IntPtr context, int device);

	[DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern ushort interception_get_filter(IntPtr context, int device);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate int InterceptionPredicate(int x);

	[DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int interception_set_filter(IntPtr context, InterceptionPredicate predicate, ushort filter);

	[DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int interception_wait_with_timeout(IntPtr context, ulong milliseconds);

	[DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int interception_wait(IntPtr context);

	[DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int interception_send(IntPtr context, int device, byte[] stroke, uint nstroke);

	[DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int interception_receive(IntPtr context, int device, ref Stroke stroke, uint nstroke);

	[DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern uint interception_get_hardware_id(IntPtr context, int device, IntPtr hardware_id_buffer, uint buffer_size);

	[DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int interception_is_invalid(int device);

	[DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int interception_is_keyboard(int device);

	[DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int interception_is_mouse(int device);

	private static unsafe T ByteArrayToStructure<T>(byte[] bytes) where T : struct
	{
		fixed (byte* ptr = &bytes[0])
		{
			return (T)Marshal.PtrToStructure((IntPtr)ptr, typeof(T));
		}
	}

	public static byte[] getBytes(object str)
	{
		int size = Marshal.SizeOf(str);
		byte[] arr = new byte[size];

		IntPtr ptr = Marshal.AllocHGlobal(size);
		Marshal.StructureToPtr(str, ptr, true);
		Marshal.Copy(ptr, arr, 0, size);
		Marshal.FreeHGlobal(ptr);
		return arr;
	}
}
