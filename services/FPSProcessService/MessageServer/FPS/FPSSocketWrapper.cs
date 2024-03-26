using System;
using System.Configuration;
using System.Net.Sockets;
using System.Timers;

namespace ContisGroup.MessageServer
{
    public class FPSSocketWrapper : SocketWrapper
    {
        //int headerLength = 0;
        //byte[] headerBuffer;
        public ProcessorWrapper.DProcessHeader ProcessHeader = null;
        ReceiveState _headerState = null;
        // Event for notifying caller of client connection

        public delegate void ProcessSendMessageTimeoutEventHandler(object sender, TCPEventArgs args);

        public event ProcessSendMessageTimeoutEventHandler SendMessageTimeout;
        public bool SignOnCompleted { get; set; } // TODOSP : Mark this true when Signon completed

        public Timer ProcessNextTimer = null;

        public void ResetSendMessageTimeout()
        {
            double timerInterval = 0;
            double.TryParse(ConfigurationManager.AppSettings["TimerInterval"], out timerInterval);
            if (timerInterval <= 0) timerInterval = 60000;

            ProcessNextTimer = new Timer();
            ProcessNextTimer.Interval = timerInterval; 
            ProcessNextTimer.AutoReset = false;
            ProcessNextTimer.Elapsed += ProcessNextTimer_Elapsed;
            ProcessNextTimer.AutoReset = false;
            ProcessNextTimer.Start();

        }

        public bool IsSendMessageTimeoutBind { get; set; }


        private void ProcessNextTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TCPEventArgs tcpE = new TCPEventArgs(string.Empty, _settings, this, DateTime.Now);
            SendMessageTimeout?.Invoke(sender, tcpE);
        }

        public void SetDataMessageLength(int dataMessageLength)
        {
            _headerState.MessageLength = dataMessageLength;
        }
        /// <summary>
        /// Initialises the socket wrapper object and binds the delegates
        /// </summary>
        /// <param name="baseSocket">A CONNECTED socket returned from the AcceptSocket or EndAcceptSocket functions</param>
        /// <param name="eofFn">Function to check for EOF</param>
        /// <param name="proFn">Function to process a complete message</param>
        public FPSSocketWrapper(
            Socket baseSocket, 
            ProcessorWrapper.DEOFLengthCheck eofFn, 
            ProcessorWrapper.DProcessHeader headerFn, 
            ProcessorWrapper.DProcessMsg proFn, 
            TCPSettings settings)
        {

            // Update the last action
            Actioned();

            // First load in the passed settings
            _settings = settings;

            // We need a socket passed through from the listener
            _baseSocket = baseSocket;
            DEofLength = eofFn;
            ProcessFn = proFn;
            ProcessHeader = headerFn;

            // Set the client IP into local var
            this.GetClientIP();

            // We need a state control
            _state = new ReceiveState(BUFFER_SIZE);
            _headerState = new ReceiveState();
        }

        protected override void SubCleanUp()
        {
            _headerState.Close();
            _headerState = new ReceiveState(BUFFER_SIZE);
        }
    }
}
