using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Peak.Can.Basic
{
	public static class PCANBasic
	{
		public const ushort PCAN_NONEBUS = 0;

		public const ushort PCAN_ISABUS1 = 33;

		public const ushort PCAN_ISABUS2 = 34;

		public const ushort PCAN_ISABUS3 = 35;

		public const ushort PCAN_ISABUS4 = 36;

		public const ushort PCAN_ISABUS5 = 37;

		public const ushort PCAN_ISABUS6 = 38;

		public const ushort PCAN_ISABUS7 = 39;

		public const ushort PCAN_ISABUS8 = 40;

		public const ushort PCAN_DNGBUS1 = 49;

		public const ushort PCAN_PCIBUS1 = 65;

		public const ushort PCAN_PCIBUS2 = 66;

		public const ushort PCAN_PCIBUS3 = 67;

		public const ushort PCAN_PCIBUS4 = 68;

		public const ushort PCAN_PCIBUS5 = 69;

		public const ushort PCAN_PCIBUS6 = 70;

		public const ushort PCAN_PCIBUS7 = 71;

		public const ushort PCAN_PCIBUS8 = 72;

		public const ushort PCAN_PCIBUS9 = 1033;

		public const ushort PCAN_PCIBUS10 = 1034;

		public const ushort PCAN_PCIBUS11 = 1035;

		public const ushort PCAN_PCIBUS12 = 1036;

		public const ushort PCAN_PCIBUS13 = 1037;

		public const ushort PCAN_PCIBUS14 = 1038;

		public const ushort PCAN_PCIBUS15 = 1039;

		public const ushort PCAN_PCIBUS16 = 1040;

		public const ushort PCAN_USBBUS1 = 81;

		public const ushort PCAN_USBBUS2 = 82;

		public const ushort PCAN_USBBUS3 = 83;

		public const ushort PCAN_USBBUS4 = 84;

		public const ushort PCAN_USBBUS5 = 85;

		public const ushort PCAN_USBBUS6 = 86;

		public const ushort PCAN_USBBUS7 = 87;

		public const ushort PCAN_USBBUS8 = 88;

		public const ushort PCAN_USBBUS9 = 1289;

		public const ushort PCAN_USBBUS10 = 1290;

		public const ushort PCAN_USBBUS11 = 1291;

		public const ushort PCAN_USBBUS12 = 1292;

		public const ushort PCAN_USBBUS13 = 1293;

		public const ushort PCAN_USBBUS14 = 1294;

		public const ushort PCAN_USBBUS15 = 1295;

		public const ushort PCAN_USBBUS16 = 1296;

		public const ushort PCAN_PCCBUS1 = 97;

		public const ushort PCAN_PCCBUS2 = 98;

		public const ushort PCAN_LANBUS1 = 2049;

		public const ushort PCAN_LANBUS2 = 2050;

		public const ushort PCAN_LANBUS3 = 2051;

		public const ushort PCAN_LANBUS4 = 2052;

		public const ushort PCAN_LANBUS5 = 2053;

		public const ushort PCAN_LANBUS6 = 2054;

		public const ushort PCAN_LANBUS7 = 2055;

		public const ushort PCAN_LANBUS8 = 2056;

		public const ushort PCAN_LANBUS9 = 2057;

		public const ushort PCAN_LANBUS10 = 2058;

		public const ushort PCAN_LANBUS11 = 2059;

		public const ushort PCAN_LANBUS12 = 2060;

		public const ushort PCAN_LANBUS13 = 2061;

		public const ushort PCAN_LANBUS14 = 2062;

		public const ushort PCAN_LANBUS15 = 2063;

		public const ushort PCAN_LANBUS16 = 2064;

		public const string PCAN_BR_CLOCK = "f_clock";

		public const string PCAN_BR_CLOCK_MHZ = "f_clock_mhz";

		public const string PCAN_BR_NOM_BRP = "nom_brp";

		public const string PCAN_BR_NOM_TSEG1 = "nom_tseg1";

		public const string PCAN_BR_NOM_TSEG2 = "nom_tseg2";

		public const string PCAN_BR_NOM_SJW = "nom_sjw";

		public const string PCAN_BR_NOM_SAMPLE = "nom_sam";

		public const string PCAN_BR_DATA_BRP = "data_brp";

		public const string PCAN_BR_DATA_TSEG1 = "data_tseg1";

		public const string PCAN_BR_DATA_TSEG2 = "data_tseg2";

		public const string PCAN_BR_DATA_SJW = "data_sjw";

		public const string PCAN_BR_DATA_SAMPLE = "data_ssp_offset";

		public const int PCAN_PARAMETER_OFF = 0;

		public const int PCAN_PARAMETER_ON = 1;

		public const int PCAN_FILTER_CLOSE = 0;

		public const int PCAN_FILTER_OPEN = 1;

		public const int PCAN_FILTER_CUSTOM = 2;

		public const int PCAN_CHANNEL_UNAVAILABLE = 0;

		public const int PCAN_CHANNEL_AVAILABLE = 1;

		public const int PCAN_CHANNEL_OCCUPIED = 2;

		public const int PCAN_CHANNEL_PCANVIEW = 3;

		public const int LOG_FUNCTION_DEFAULT = 0;

		public const int LOG_FUNCTION_ENTRY = 1;

		public const int LOG_FUNCTION_PARAMETERS = 2;

		public const int LOG_FUNCTION_LEAVE = 4;

		public const int LOG_FUNCTION_WRITE = 8;

		public const int LOG_FUNCTION_READ = 16;

		public const int LOG_FUNCTION_ALL = 65535;

		public const int TRACE_FILE_SINGLE = 0;

		public const int TRACE_FILE_SEGMENTED = 1;

		public const int TRACE_FILE_DATE = 2;

		public const int TRACE_FILE_TIME = 4;

		public const int TRACE_FILE_OVERWRITE = 128;

		public const int FEATURE_FD_CAPABLE = 1;

		public const int FEATURE_DELAY_CAPABLE = 2;

		public const int FEATURE_IO_CAPABLE = 4;

		public const int SERVICE_STATUS_STOPPED = 1;

		public const int SERVICE_STATUS_RUNNING = 4;

		[DllImport(".\\dll\\PCANBasic.dll", EntryPoint = "CAN_Initialize")]
		public static extern TPCANStatus Initialize([MarshalAs(UnmanagedType.U2)] ushort Channel, [MarshalAs(UnmanagedType.U2)] TPCANBaudrate Btr0Btr1, [MarshalAs(UnmanagedType.U1)] TPCANType HwType, uint IOPort, ushort Interrupt);

		public static TPCANStatus Initialize(ushort Channel, TPCANBaudrate Btr0Btr1)
		{
			return Initialize(Channel, Btr0Btr1, (TPCANType)0, 0u, 0);
		}

		[DllImport(".\\dll\\PCANBasic.dll", EntryPoint = "CAN_InitializeFD")]
		public static extern TPCANStatus InitializeFD([MarshalAs(UnmanagedType.U2)] ushort Channel, string BitrateFD);

		[DllImport(".\\dll\\PCANBasic.dll", EntryPoint = "CAN_Uninitialize")]
		public static extern TPCANStatus Uninitialize([MarshalAs(UnmanagedType.U2)] ushort Channel);

		[DllImport(".\\dll\\PCANBasic.dll", EntryPoint = "CAN_Reset")]
		public static extern TPCANStatus Reset([MarshalAs(UnmanagedType.U2)] ushort Channel);

		[DllImport(".\\dll\\PCANBasic.dll", EntryPoint = "CAN_GetStatus")]
		public static extern TPCANStatus GetStatus([MarshalAs(UnmanagedType.U2)] ushort Channel);

		[DllImport(".\\dll\\PCANBasic.dll", EntryPoint = "CAN_Read")]
		public static extern TPCANStatus Read([MarshalAs(UnmanagedType.U2)] ushort Channel, out TPCANMsg MessageBuffer, out TPCANTimestamp TimestampBuffer);

		[DllImport(".\\dll\\PCANBasic.dll", EntryPoint = "CAN_Read")]
		private static extern TPCANStatus Read([MarshalAs(UnmanagedType.U2)] ushort Channel, out TPCANMsg MessageBuffer, IntPtr bufferPointer);

		public static TPCANStatus Read(ushort Channel, out TPCANMsg MessageBuffer)
		{
			return Read(Channel, out MessageBuffer, IntPtr.Zero);
		}

		[DllImport(".\\dll\\PCANBasic.dll", EntryPoint = "CAN_ReadFD")]
		public static extern TPCANStatus ReadFD([MarshalAs(UnmanagedType.U2)] ushort Channel, out TPCANMsgFD MessageBuffer, out ulong TimestampBuffer);

		[DllImport(".\\dll\\PCANBasic.dll", EntryPoint = "CAN_ReadFD")]
		private static extern TPCANStatus ReadFD([MarshalAs(UnmanagedType.U2)] ushort Channel, out TPCANMsgFD MessageBuffer, IntPtr TimestampBuffer);

		public static TPCANStatus ReadFD(ushort Channel, out TPCANMsgFD MessageBuffer)
		{
			return ReadFD(Channel, out MessageBuffer, IntPtr.Zero);
		}

		[DllImport(".\\dll\\PCANBasic.dll", EntryPoint = "CAN_Write")]
		public static extern TPCANStatus Write([MarshalAs(UnmanagedType.U2)] ushort Channel, ref TPCANMsg MessageBuffer);

		[DllImport(".\\dll\\PCANBasic.dll", EntryPoint = "CAN_WriteFD")]
		public static extern TPCANStatus WriteFD([MarshalAs(UnmanagedType.U2)] ushort Channel, ref TPCANMsgFD MessageBuffer);

		[DllImport(".\\dll\\PCANBasic.dll", EntryPoint = "CAN_FilterMessages")]
		public static extern TPCANStatus FilterMessages([MarshalAs(UnmanagedType.U2)] ushort Channel, uint FromID, uint ToID, [MarshalAs(UnmanagedType.U1)] TPCANMode Mode);

		[DllImport(".\\dll\\PCANBasic.dll", EntryPoint = "CAN_GetValue")]
		public static extern TPCANStatus GetValue([MarshalAs(UnmanagedType.U2)] ushort Channel, [MarshalAs(UnmanagedType.U1)] TPCANParameter Parameter, StringBuilder StringBuffer, uint BufferLength);

		[DllImport(".\\dll\\PCANBasic.dll", EntryPoint = "CAN_GetValue")]
		public static extern TPCANStatus GetValue([MarshalAs(UnmanagedType.U2)] ushort Channel, [MarshalAs(UnmanagedType.U1)] TPCANParameter Parameter, out uint NumericBuffer, uint BufferLength);

		[DllImport(".\\dll\\PCANBasic.dll", EntryPoint = "CAN_GetValue")]
		public static extern TPCANStatus GetValue([MarshalAs(UnmanagedType.U2)] ushort Channel, [MarshalAs(UnmanagedType.U1)] TPCANParameter Parameter, out ulong NumericBuffer, uint BufferLength);

		[DllImport(".\\dll\\PCANBasic.dll", EntryPoint = "CAN_SetValue")]
		public static extern TPCANStatus SetValue([MarshalAs(UnmanagedType.U2)] ushort Channel, [MarshalAs(UnmanagedType.U1)] TPCANParameter Parameter, ref uint NumericBuffer, uint BufferLength);

		[DllImport(".\\dll\\PCANBasic.dll", EntryPoint = "CAN_SetValue")]
		public static extern TPCANStatus SetValue([MarshalAs(UnmanagedType.U2)] ushort Channel, [MarshalAs(UnmanagedType.U1)] TPCANParameter Parameter, ref ulong NumericBuffer, uint BufferLength);

		[DllImport(".\\dll\\PCANBasic.dll", EntryPoint = "CAN_SetValue")]
		public static extern TPCANStatus SetValue([MarshalAs(UnmanagedType.U2)] ushort Channel, [MarshalAs(UnmanagedType.U1)] TPCANParameter Parameter, [MarshalAs(UnmanagedType.LPStr)] string StringBuffer, uint BufferLength);

		[DllImport(".\\dll\\PCANBasic.dll", EntryPoint = "CAN_GetErrorText")]
		public static extern TPCANStatus GetErrorText([MarshalAs(UnmanagedType.U4)] TPCANStatus Error, ushort Language, StringBuilder StringBuffer);
	}
}
