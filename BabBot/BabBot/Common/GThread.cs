/*
    This file is part of BabBot.

    BabBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    BabBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BabBot.  If not, see <http://www.gnu.org/licenses/>.
  
    Copyright 2009 BabBot Team
*/
// TODO - Need english translation of comments :)
using System;
using System.Threading;

namespace BabBot.Common
{
    public class GThread
    {
        #region Delegates

        public delegate void DlgCommon();

        public delegate void DlgThreadBeforeStart();

        public delegate void DlgThreadBeforeStop();

        public delegate void DlgThreadException(Exception e, ThreadPhase phase);

        public delegate void DlgThreadFinalize();

        public delegate void DlgThreadInitialize();

        public delegate void DlgThreadRun();

        #endregion

        #region ThreadPhase enum

        public enum ThreadPhase
        {
            Initialize,
            Run,
            Finalize,
            Unknow
        } ;

        #endregion

        private readonly EventWaitHandle m_evStart; // Handle per la sincronizzazione allo start del thread  
        private readonly EventWaitHandle m_evStop; // Handle per la sincronizzazione allo stop del thread  
        private bool m_forceStop; // Se a true, interrompe un eventuale stato di sleep,wait,join del thread  
        private string m_name; // Nome associato al thread          
        protected bool m_running; // A true quando il thread è in fase di esecuzione  
        private Thread m_thread; // Oggetto Thread del .NET Framework  

        public GThread()
        {
            m_running = false;
            m_evStart = new EventWaitHandle(false, EventResetMode.AutoReset);
            m_evStop = new EventWaitHandle(false, EventResetMode.AutoReset);
        }

        public bool Running
        {
            get { return m_running; }
        }

        public bool ForceStop
        {
            get { return m_forceStop; }
            set { m_forceStop = value; }
        }

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

// Delegato    

        public event DlgThreadRun OnRun; // Evento che viene generato ciclicamente quando il thread è attivo  

        public event DlgThreadBeforeStart OnBeforeStart;
        // Evento che viene generato prima della partenza del thread - in capo al chiamante  

        public event DlgThreadInitialize OnInitialize;
        // Evento che viene generato subito dopo la partenza del thread - in capo al thread  

        public event DlgThreadBeforeStop OnBeforeStop;
        // Evento che viene generato prima di fermare il thread - in capo al chiamante  

        public event DlgThreadFinalize OnFinalize;
        // Evento che viene generato appena prima l'arresto del thread - in capo al thread  

        public event DlgThreadException OnException;
        // Evento che viene generato se avviene un'eccezione durante la vita del thread  

        // Reinitialize internal parameters 
        public event DlgCommon OnInit;

        public void Start()
        {
            // Creo il thread solo se già non esiste  
            if (m_thread != null)
            {
                return;
            }

            // First load configuratin parameters
            if (OnInit != null)
            {
                Output.Instance.Debug("char", "Initialize bot parameters ...", this);
                OnInit();
            }

            // Genere l'evento OnBeforeStart prima di far partire il thread  
            if (OnBeforeStart != null)
            {
                OnBeforeStart();
            }

            // Creo fisicamente il Thread a livello di sistema operativo  
            m_thread = new Thread(Run) {Name = m_name};

            // Faccio partire il thread  
            m_running = true;
            m_thread.Start();

            // Metto in attesa il thread chiamante dell'effettivo start del thread dell'oggetto corrente  
            m_evStart.WaitOne();
        }

        public void Stop()
        {
            // Eseguo lo stop solo se il thread esiste ed è vivo  
            if ((m_thread == null) || (!m_thread.IsAlive) || (!m_running))
            {
                Output.Instance.Log("char", "Bot not running.");
                return;
            }

            // Genero l'evento OnBeforerSop prima di arrestare il thread  
            if (OnBeforeStop != null)
            {
                OnBeforeStop();
            }

            // Faccio in modo che il thread termini l'esecuzione del suo metodo  
            m_running = false;

            // Se lo stato del thread è Wait, Sleep o Join lo interrompo  
            // se la configurazione di questo oggetto lo permette  
            try
            {
                if ((m_forceStop) && (m_thread.ThreadState == ThreadState.WaitSleepJoin))
                {
                    Output.Instance.Debug("char", "Forcing bot stop");
                    m_thread.Interrupt();
                }
            }
            catch (ThreadInterruptedException)
            {
            }

            // Metto in attesa il thread chiamante dell'effettivo stop del thread dell'oggetto corrente  
            // NOTE: tanis - I'm not that sure that blocking this thread waiting for the main one to finish is a good idea. Most of the time it just hangs everything.
            Output.Instance.Debug("char", "Waiting bot termination ...");
            //m_evStop.WaitOne();
            Output.Instance.Debug("char", "Bot terminated");

            m_thread = null;
        }

        public void Run()
        {
            try
            {
                // Chiamo l'inizializzazione del thread  
                try
                {
                    if (OnInitialize != null)
                    {
                        OnInitialize();
                    }
                }
                catch (Exception e)
                {
                    // Notifico eventuali eccezioni avvenute durante l'inizialize del thread  
                    if (OnException != null)
                    {
                        OnException(e, ThreadPhase.Initialize);
                    }
                }

                // Setto l'handle che indica che il thread è partito  
                m_evStart.Set();

                // Finchè il thread è attivo viene eseguito il seguente codice che genera l'evento OnRun  
                while (m_running)
                {
                    try
                    {
                        if (OnRun != null)
                        {
                            OnRun();
                        }
                    }
                    catch (ThreadInterruptedException)
                    {
                    }
                    catch (Exception e)
                    {
                        // Notifico eventuali eccezioni avvenute durante la vita del thread  
                        if (OnException != null)
                        {
                            OnException(e, ThreadPhase.Run);
                        }
                    }
                }

                // Chiamo il finalize del thread  
                try
                {
                    if (OnFinalize != null)
                    {
                        OnFinalize();
                    }
                }
                catch (Exception e)
                {
                    // Notifico eventuali eccezioni avvenute durante la finalize del thread  
                    if (OnException != null)
                    {
                        OnException(e, ThreadPhase.Finalize);
                    }
                }

                // Setto l'handle che indica che il thread si è fermato  
                m_evStop.Set();
            }
            catch (Exception e)
            {
                // Notifico eventuali eccezioni non caturate precedentemente  
                if (OnException != null)
                {
                    OnException(e, ThreadPhase.Unknow);
                }
            }
        }
    }
}