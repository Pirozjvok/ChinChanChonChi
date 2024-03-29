namespace Peak.Can.Basic
{
	public enum TPCANParameter : byte
	{
		PCAN_DEVICE_NUMBER = 1,
		PCAN_5VOLTS_POWER,
		PCAN_RECEIVE_EVENT,
		PCAN_MESSAGE_FILTER,
		PCAN_API_VERSION,
		PCAN_CHANNEL_VERSION,
		PCAN_BUSOFF_AUTORESET,
		PCAN_LISTEN_ONLY,
		PCAN_LOG_LOCATION,
		PCAN_LOG_STATUS,
		PCAN_LOG_CONFIGURE,
		PCAN_LOG_TEXT,
		PCAN_CHANNEL_CONDITION,
		PCAN_HARDWARE_NAME,
		PCAN_RECEIVE_STATUS,
		PCAN_CONTROLLER_NUMBER,
		PCAN_TRACE_LOCATION,
		PCAN_TRACE_STATUS,
		PCAN_TRACE_SIZE,
		PCAN_TRACE_CONFIGURE,
		PCAN_CHANNEL_IDENTIFYING,
		PCAN_CHANNEL_FEATURES,
		PCAN_BITRATE_ADAPTING,
		PCAN_BITRATE_INFO,
		PCAN_BITRATE_INFO_FD,
		PCAN_BUSSPEED_NOMINAL,
		PCAN_BUSSPEED_DATA,
		PCAN_IP_ADDRESS,
		PCAN_LAN_SERVICE_STATUS,
		PCAN_ALLOW_STATUS_FRAMES,
		PCAN_ALLOW_RTR_FRAMES,
		PCAN_ALLOW_ERROR_FRAMES,
		PCAN_INTERFRAME_DELAY,
		PCAN_ACCEPTANCE_FILTER_11BIT,
		PCAN_ACCEPTANCE_FILTER_29BIT,
		PCAN_IO_DIGITAL_CONFIGURATION,
		PCAN_IO_DIGITAL_VALUE,
		PCAN_IO_DIGITAL_SET,
		PCAN_IO_DIGITAL_CLEAR,
		PCAN_IO_ANALOG_VALUE
	}
}
