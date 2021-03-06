﻿using System;
using System.Net;

namespace com.negociosit.XMPP.XMPPphoton
{
    /// <summary>
    /// Handles different types of xml fragments on our streams, such as negotiation, IQ's, etc
    /// </summary>
    public class Logic
    {
        public Logic(XMPPClient client)
        {
            XMPPClient = client;
        }

        protected XMPPClient m_objXMPPClient = null;
        public XMPPClient XMPPClient
        {
            get { return m_objXMPPClient; }
            set { m_objXMPPClient = value; }
        }

        public virtual void Start()
        {
        }


        /// <summary>
        /// A new XML fragment has been received
        /// </summary>
        /// <param name="node"></param>
        /// <returns>returns true if we handled this fragment, false if other wise</returns>
        public virtual bool NewXMLFragment(XMPPStanza stanza)
        {
            return false;
        }

        public virtual bool NewIQ(IQ iq)
        {
            return false;
        }

        public virtual bool NewMessage(Message iq)
        {
            return false;
        }

        public virtual bool NewPresence(PresenceMessage iq)
        {
            return false;
        }

        protected bool m_bCompleted = false;

        /// <summary>
        /// Set to true if we have completed our logic and should be removed from the logic list
        /// </summary>
        public bool IsCompleted
        {
            get { return m_bCompleted; }
            set { m_bCompleted = value; }
        }

        private bool m_bSuccess = false;

        public bool Success
        {
            get { return m_bSuccess; }
            set { m_bSuccess = value; }
        }

    }

    public class WaitableLogic : Logic, IDisposable
    {
        public WaitableLogic(XMPPClient client) : base(client)
        {
        }

     
        protected System.Threading.ManualResetEvent GotEvent = new System.Threading.ManualResetEvent(false);

        public virtual bool Wait(int nTimeoutMs)
        {
            Success = GotEvent.WaitOne(nTimeoutMs);
            return Success;
        }

        private SerializationMethod m_eSerializationMethod = SerializationMethod.MessageXMLProperty;

        public SerializationMethod SerializationMethod
        {
            get { return m_eSerializationMethod; }
            set { m_eSerializationMethod = value; }
        }

        #region IDisposable Members

        bool m_bIsDisposed = false;
        public void Dispose()
        {
            if (m_bIsDisposed == false)
            {
                GotEvent.Close();
                GotEvent.Dispose();
                m_bIsDisposed = true;
            }
        }

        #endregion
    }

}
