
using EwEPlugin;

namespace EwEBridge.Ecosim
{
    /// <summary>
    /// Simple plug-in hook to allow the Run Console to intercept specific plug-in calls
    /// </summary>
    public class cEcosimBridgePlugin :
        IEcosimBeginTimestepPlugin, IEcosimBeginTimestepPostPlugin, IEcosimEndTimestepPlugin, IEcosimEndTimestepPostPlugin
    {
        public const string NAME = "xxxEwESimBridge";
        public enum EventType : int
        {
            None = 0,
            BeginTimeStep = 1,
            BeginTimeStepPost = 2,
            EndTimeStep = 3,
            EndTimeStepPost = 4
        }

        public cEcosimBridgePlugin()
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

        #region Ecosim integration
    
        void IEcosimBeginTimestepPlugin.EcosimBeginTimeStep(ref float[] BiomassAtTimestep, object EcosimDatastructures, int iTime)
        {
            SendEvent(EventType.BeginTimeStep, iTime);
        }

        void IEcosimBeginTimestepPostPlugin.EcosimBeginTimeStepPost(ref float[] BiomassAtTimestep, object EcosimDatastructures, int iTime)
        {
            SendEvent(EventType.BeginTimeStepPost, iTime);
        }

        public void EcosimEndTimeStep(ref float[] BiomassAtTimestep, object EcosimDatastructures, int iTime, object Ecosimresults)
        {
            SendEvent(EventType.EndTimeStep, iTime);
        }

        public void EcosimEndTimeStepPost(ref float[] BiomassAtTimestep, object EcosimDatastructures, int iTime, object Ecosimresults)
        {
            SendEvent(EventType.EndTimeStepPost, iTime);
        }

        #endregion // Ecosim integration

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
