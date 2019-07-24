using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace TealiumCSharp

{
    /// <summary>
    /// Tealium.
    /// </summary>
    public class Tealium
    {
        private ModulesManager modulesManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TealiumCSharp.Tealium"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        public Tealium(Config config)
        {
            modulesManager = new ModulesManager();
            modulesManager.StartModules(config);
        }

        /// <summary>
        /// Track with a specified title.
        /// </summary>
        /// <returns>The track.</returns>
        /// <param name="title">Required title. Does not need to be unique.</param>
        public void Track(string title)
        {
            Track(title, null, null);
        }

        /// <summary>
        /// Track with a specified title and customData.
        /// </summary>
        /// <returns>The track.</returns>
        /// <param name="title">Required title. Does not need to be unique.</param>
        /// <param name="customData">Custom data containing string or string[] values.</param>
        public void Track(string title,
                          Dictionary<string, object> customData)
        {
            Track(title, customData, null);
        }


        /// <summary>
        /// Track with a title, customData and completion.
        /// </summary>
        /// <returns></returns>
        /// <param name="title">Title.</param>
        /// <param name="customData">Custom data.</param>
        /// <param name="completion">Completion.</param>
        public void Track(string title,
                          Dictionary<string, object> customData,
                          TrackCompletion completion)
        {
            Dictionary<string, object> newData = new Dictionary<string, object>();
            newData = Utils.MergeDictionary(newData, customData);
            newData.Add(Constants.EVENT, title);

            if (TraceId != null)
            {
                newData.Add(Constants.TRACE_ID, TraceId);
            }
            modulesManager.Track(newData,
                                completion);

        }

        /// <summary>
        /// Gets the trace identifier if one has been set.
        /// </summary>
        /// <value>The trace identifier. Nullable</value>
        public string TraceId
        {
            get;
            private set;
        }

        /// <summary>
        /// <para>
        /// Joins the trace identified by the given traceId. The traceId will be
        /// sent on all subsequent events as the key <see cref="Constants.TRACE_ID"/>
        /// until the <see cref="LeaveTrace"/> method is called.
        /// </para>
        /// <para>
        /// This should be generated in advance within the Tealium UDH.
        /// </para>
        /// </summary>
        /// <param name="traceId">Trace identifier.</param>
        public void JoinTrace(string traceId)
        {
            if (traceId != null)
            {
                TraceId = traceId;
            }
        }

        /// <summary>
        /// Leaves the Trace, if one has been joined. <see cref="TraceId"/> will 
        /// be set to <see langword="null"/>.
        /// </summary>
        public void LeaveTrace()
        {
            TraceId = null;
        }

        /// <summary>
        /// <para>
        /// Issues a specific event that kills the trace visitor session at the
        /// server. This is useful for testing end-of-session behaviour in the UDH.
        /// </para>
        /// <para>
        /// This will also execute the <see cref="LeaveTrace"/> method, and therefore
        /// wont sent the <see cref="TraceId"/> in any subsequent events.
        /// </para>
        /// </summary>
        public void KillTraceSession()
        {
            modulesManager.Track(new Dictionary<string, object>()
            {
                {"event", "kill_visitor_session"},
                {Constants.TRACE_ID, TraceId},
            }, null);
            LeaveTrace();
        }

        /// <summary>
        /// Enable this instance after a Disable() call.
        /// </summary>
        public void Enable()
        {
            modulesManager.ResumeModules();
        }

        /// <summary>
        /// Disable this instance.
        /// </summary>
        public void Disable()
        {
            modulesManager.PauseModules();
        }

    }
}
