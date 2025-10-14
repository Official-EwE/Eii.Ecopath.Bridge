Its plug-in structure has proven an invaluable asset for extending and customizing the functionality of the Ecopath with Ecosim food web approach. At the time of writing (2025), the plug-in structure offers a much more versatile method to interact with - and to intervene in - the EwE execution than via the EwE API. The downside is that plug-ins execute within the EwE flow, and their capabilities cannot be harnassed when using the EwE API in a scripted environment.

The Bridge plug-in was developed to expose the capabilities offered by plug-ins to scripts that use the EwE API. IT's quite simple actually: the bridge is a plug-in that invokes a user-designated callback function, as follows:

    internal class cEcosimModifier : cRuntimeModifier
    {
        public cEcosimModifier(cCore core, cEwEConfiguration config, cEcosimRunInstructions runmodel) : base(core, "ecosim", config, runmodel)
        {
            // Find the bridge plug-in
            IPlugin? pi = GetPlugin(typeof(cEcosimCallbackPluginPoint));
            // Got it?
            if (pi != null)
            {
                // #es: pipe all ecosim plug-in points to a local callback function. Yay.
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
