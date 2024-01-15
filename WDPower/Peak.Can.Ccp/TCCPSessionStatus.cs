namespace Peak.Can.Ccp
{
	public enum TCCPSessionStatus : byte
	{
		CCP_STS_CALIBRATING = 1,
		CCP_STS_ACQUIRING = 2,
		CCP_STS_RESUME_REQUEST = 4,
		CCP_STS_STORE_REQUEST = 0x40,
		CCP_STS_RUNNING = 0x80
	}
}
