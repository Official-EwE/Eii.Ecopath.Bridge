using EwEPlugin;

namespace EwEBridge.Ecospace
{
    /// <summary>
    /// Simple plug-in hook to allow the Run Console to intercept specific plug-in calls
    /// </summary>
    public class cEcospaceBridgePlugin :
        IEcospaceInitRunStartedPlugin,
        IEcospaceRunCompletedPlugin,
        IEcospaceBeginTimestepPlugin, 
        IEcospaceBeginTimestepPostPlugin, 
        IEcospaceEndTimestepPlugin, 
        IEcospaceEndTimestepPostPlugin, 
        IEcospacePostFishingEffortModTimestepPlugin
    {
        public const string NAME = "xxxEwESpaceBridge";

        public enum EventType : int
        {
            None = 0,
            BeginRun,
            EndRun,
            BeginTimeStep,
            BeginTimeStepPost,
            EndTimeStep,
            EndTimeStepPost,
            EffortDistrPost
        }

        public cEcospaceBridgePlugin()
        {
            this.Name = NAME;
        }

        public string? Name { get; }

        public string? DisplayName { get; }

        public string? Description { get; }

        public string? Author { get; }

        public string? Contact { get; }

        void IPlugin.Initialize(object core)
        {
            // NOP
        }

        #region Ecospace integration

        public void EcospaceInitRunStarted(object EcospaceDatastructures)
        {
            this.SendEvent(EventType.BeginRun, -1);
        }

        public void EcospaceRunCompleted(object EcoSpaceDatastructures)
        {
            this.SendEvent(EventType.EndRun, -1);
        }

        public void EcospaceBeginTimeStep(object EcospaceDatastructures, int iTime)
        {
            this.SendEvent(EventType.BeginTimeStep, iTime);
        }

        public void EcospaceBeginTimeStepPost(object EcospaceDatastructures, int iTime)
        {
            this.SendEvent(EventType.BeginTimeStepPost, iTime);
        }

        public void EcospaceEndTimeStep(object EcospaceDatastructures, int iTime)
        {
            this.SendEvent(EventType.EndTimeStep, iTime);
        }

        public void EcospaceEndTimeStepPost(object EcospaceDatastructures, int iTime)
        {
            this.SendEvent(EventType.EndTimeStepPost, iTime);
        }
        public void EcospacePostFishingEffortModTimestep(object EcospaceDatastructures, int iTime)
        {
            this.SendEvent(EventType.EffortDistrPost, iTime);
        }

        #endregion // Ecospace integration

        #region Callback

        private void SendEvent(EventType e, int iTime)
        {
            try
            {
                if (this.BridgeCallback != null)
                   this.BridgeCallback(e, iTime);
            }
            catch
            {
                // NOP
            }
        }

        public delegate void BridgeEcosimBeginTimestepPost(EventType e, int iTime);

        public BridgeEcosimBeginTimestepPost? BridgeCallback { get; set; }

        #endregion // Callback

    }
}
