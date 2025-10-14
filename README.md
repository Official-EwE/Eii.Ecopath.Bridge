The Bridge is a plug-in that converts the invocation of specific plug-in points to callback functions. These can greatly help when wrapping the EwE engine in a console application, for instance.

For example:

    internal class cEcosimModifier : cRuntimeModifier
    {
        public cEcosimModifier(cCore core, cEwEConfiguration config, cEcosimRunInstructions runmodel) : base(core, "ecosim", config, runmodel)
        {
            // Create a plug-in bridge to be able to intervene into the
            // running Ecosim model during time stepping
            IPlugin? pi = GetPlugin(typeof(cEcosimCallbackPluginPoint));
            if (pi != null)
            {
                // Pipe all ecosim plug-in points to a local callback function
                cEcosimCallbackPluginPoint ppt = (cEcosimCallbackPluginPoint)pi;
                ppt.BridgeCallback = BridgeCallback;
            }
        }

        public override void ConfigureAutosave()
        {
            // ToDo
        }

        public override bool Run()
        {
            this.RunSuccess = true;
            // Go for it
            this.RunSuccess &= this.Core.RunEcosim();
            // Done
            return RunSuccess;
        }

        // Plug-in callback for making specific modifications.
        protected void BridgeCallback (cEcosimCallbackPluginPoint.EventType e, int iTime)
        {
            if (e== cEcosimCallbackPluginPoint.EventType.BeginTimeStep)
            {
                cEcosimDatastructures ds = this.Core.EcosimDataStructures;

                // Print out time tracking
                if ((iTime - 1) % ds.NumStepsPerYear == 0)
                {
                    Console.WriteLine("{0}",
                        (int) this.Core.EcosimFirstYear() + ((iTime - 1) / ds.NumStepsPerYear));
                }
                this.RunSuccess &= this.Apply(iTime);
            }
        }
    }
}
