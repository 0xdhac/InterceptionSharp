using System;
using System.Runtime.InteropServices;

public class Interception
{
	public const short INTERCEPTION_MAX_KEYBOARD = 10;
	public const short INTERCEPTION_MAX_MOUSE = 10;
	public const short INTERCEPTION_MAX_DEVICE = (INTERCEPTION_MAX_KEYBOARD + INTERCEPTION_MAX_MOUSE);

	public enum InterceptionKeyState
	{
		INTERCEPTION_KEY_DOWN = 0x00,
		INTERCEPTION_KEY_UP = 0x01,
		INTERCEPTION_KEY_E0 = 0x02,
		INTERCEPTION_KEY_E1 = 0x04,
		INTERCEPTION_KEY_TERMSRV_SET_LED = 0x08,
		INTERCEPTION_KEY_TERMSRV_SHADOW = 0x10,
		INTERCEPTION_KEY_TERMSRV_VKPACKET = 0x20
	};

	public enum InterceptionFilterKeyState
	{
		INTERCEPTION_FILTER_KEY_NONE = 0x0000,
		INTERCEPTION_FILTER_KEY_ALL = 0xFFFF,
		INTERCEPTION_FILTER_KEY_DOWN = InterceptionKeyState.INTERCEPTION_KEY_UP,
		INTERCEPTION_FILTER_KEY_UP = InterceptionKeyState.INTERCEPTION_KEY_UP << 1,
		INTERCEPTION_FILTER_KEY_E0 = InterceptionKeyState.INTERCEPTION_KEY_E0 << 1,
		INTERCEPTION_FILTER_KEY_E1 = InterceptionKeyState.INTERCEPTION_KEY_E1 << 1,
		INTERCEPTION_FILTER_KEY_TERMSRV_SET_LED = InterceptionKeyState.INTERCEPTION_KEY_TERMSRV_SET_LED << 1,
		INTERCEPTION_FILTER_KEY_TERMSRV_SHADOW = InterceptionKeyState.INTERCEPTION_KEY_TERMSRV_SHADOW << 1,
		INTERCEPTION_FILTER_KEY_TERMSRV_VKPACKET = InterceptionKeyState.INTERCEPTION_KEY_TERMSRV_VKPACKET << 1
	};

	public enum InterceptionMouseState
	{
		INTERCEPTION_MOUSE_LEFT_BUTTON_DOWN = 0x001,
		INTERCEPTION_MOUSE_LEFT_BUTTON_UP = 0x002,
		INTERCEPTION_MOUSE_RIGHT_BUTTON_DOWN = 0x004,
		INTERCEPTION_MOUSE_RIGHT_BUTTON_UP = 0x008,
		INTERCEPTION_MOUSE_MIDDLE_BUTTON_DOWN = 0x010,
		INTERCEPTION_MOUSE_MIDDLE_BUTTON_UP = 0x020,

		INTERCEPTION_MOUSE_BUTTON_1_DOWN = INTERCEPTION_MOUSE_LEFT_BUTTON_DOWN,
		INTERCEPTION_MOUSE_BUTTON_1_UP = INTERCEPTION_MOUSE_LEFT_BUTTON_UP,
		INTERCEPTION_MOUSE_BUTTON_2_DOWN = INTERCEPTION_MOUSE_RIGHT_BUTTON_DOWN,
		INTERCEPTION_MOUSE_BUTTON_2_UP = INTERCEPTION_MOUSE_RIGHT_BUTTON_UP,
		INTERCEPTION_MOUSE_BUTTON_3_DOWN = INTERCEPTION_MOUSE_MIDDLE_BUTTON_DOWN,
		INTERCEPTION_MOUSE_BUTTON_3_UP = INTERCEPTION_MOUSE_MIDDLE_BUTTON_UP,

		INTERCEPTION_MOUSE_BUTTON_4_DOWN = 0x040,
		INTERCEPTION_MOUSE_BUTTON_4_UP = 0x080,
		INTERCEPTION_MOUSE_BUTTON_5_DOWN = 0x100,
		INTERCEPTION_MOUSE_BUTTON_5_UP = 0x200,

		INTERCEPTION_MOUSE_WHEEL = 0x400,
		INTERCEPTION_MOUSE_HWHEEL = 0x800
	};

	public enum InterceptionFilterMouseState
	{
		INTERCEPTION_FILTER_MOUSE_NONE = 0x0000,
		INTERCEPTION_FILTER_MOUSE_ALL = 0xFFFF,

		INTERCEPTION_FILTER_MOUSE_LEFT_BUTTON_DOWN = InterceptionMouseState.INTERCEPTION_MOUSE_LEFT_BUTTON_DOWN,
		INTERCEPTION_FILTER_MOUSE_LEFT_BUTTON_UP = InterceptionMouseState.INTERCEPTION_MOUSE_LEFT_BUTTON_UP,
		INTERCEPTION_FILTER_MOUSE_RIGHT_BUTTON_DOWN = InterceptionMouseState.INTERCEPTION_MOUSE_RIGHT_BUTTON_DOWN,
		INTERCEPTION_FILTER_MOUSE_RIGHT_BUTTON_UP = InterceptionMouseState.INTERCEPTION_MOUSE_RIGHT_BUTTON_UP,
		INTERCEPTION_FILTER_MOUSE_MIDDLE_BUTTON_DOWN = InterceptionMouseState.INTERCEPTION_MOUSE_MIDDLE_BUTTON_DOWN,
		INTERCEPTION_FILTER_MOUSE_MIDDLE_BUTTON_UP = InterceptionMouseState.INTERCEPTION_MOUSE_MIDDLE_BUTTON_UP,

		INTERCEPTION_FILTER_MOUSE_BUTTON_1_DOWN = InterceptionMouseState.INTERCEPTION_MOUSE_BUTTON_1_DOWN,
		INTERCEPTION_FILTER_MOUSE_BUTTON_1_UP = InterceptionMouseState.INTERCEPTION_MOUSE_BUTTON_1_UP,
		INTERCEPTION_FILTER_MOUSE_BUTTON_2_DOWN = InterceptionMouseState.INTERCEPTION_MOUSE_BUTTON_2_DOWN,
		INTERCEPTION_FILTER_MOUSE_BUTTON_2_UP = InterceptionMouseState.INTERCEPTION_MOUSE_BUTTON_2_UP,
		INTERCEPTION_FILTER_MOUSE_BUTTON_3_DOWN = InterceptionMouseState.INTERCEPTION_MOUSE_BUTTON_3_DOWN,
		INTERCEPTION_FILTER_MOUSE_BUTTON_3_UP = InterceptionMouseState.INTERCEPTION_MOUSE_BUTTON_3_UP,

		INTERCEPTION_FILTER_MOUSE_BUTTON_4_DOWN = InterceptionMouseState.INTERCEPTION_MOUSE_BUTTON_4_DOWN,
		INTERCEPTION_FILTER_MOUSE_BUTTON_4_UP = InterceptionMouseState.INTERCEPTION_MOUSE_BUTTON_4_UP,
		INTERCEPTION_FILTER_MOUSE_BUTTON_5_DOWN = InterceptionMouseState.INTERCEPTION_MOUSE_BUTTON_5_DOWN,
		INTERCEPTION_FILTER_MOUSE_BUTTON_5_UP = InterceptionMouseState.INTERCEPTION_MOUSE_BUTTON_5_UP,

		INTERCEPTION_FILTER_MOUSE_WHEEL = InterceptionMouseState.INTERCEPTION_MOUSE_WHEEL,
		INTERCEPTION_FILTER_MOUSE_HWHEEL = InterceptionMouseState.INTERCEPTION_MOUSE_HWHEEL,

		INTERCEPTION_FILTER_MOUSE_MOVE = 0x1000
	};

	public enum InterceptionMouseFlag
	{
		INTERCEPTION_MOUSE_MOVE_RELATIVE = 0x000,
		INTERCEPTION_MOUSE_MOVE_ABSOLUTE = 0x001,
		INTERCEPTION_MOUSE_VIRTUAL_DESKTOP = 0x002,
		INTERCEPTION_MOUSE_ATTRIBUTES_CHANGED = 0x004,
		INTERCEPTION_MOUSE_MOVE_NOCOALESCE = 0x008,
		INTERCEPTION_MOUSE_TERMSRV_SRC_SHADOW = 0x100
	};

	public struct InterceptionMouseStroke
	{
		public ushort state;
		public ushort flags;
		public short rolling;
		public int x;
		public int y;
		public uint information;
	};

	public struct InterceptionKeyStroke
	{
		public ushort code;
		public ushort state;
		public uint information;
	};

	public unsafe struct InterceptionStroke
	{
		public fixed byte data[18];
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
	public static extern int interception_receive(IntPtr context, int device, ref InterceptionStroke stroke, uint nstroke);

	[DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern uint interception_get_hardware_id(IntPtr context, int device, IntPtr hardware_id_buffer, uint buffer_size);

	[DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int interception_is_invalid(int device);

	[DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int interception_is_keyboard(int device);

	[DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int interception_is_mouse(int device);
}
