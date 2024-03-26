using System;
using System.Collections.Generic;
using System.Text;

namespace ContisGroup.MessageServer
{
    [Serializable()]
	public class TCPDelegates
	{
		public delegate void SrvClientEvent(object sender, TCPEventArgs e);
	}
}
