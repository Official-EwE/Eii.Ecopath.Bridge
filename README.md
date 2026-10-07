# Ecopath bridge plug-in #
The EwE API provides a powerful mechanism for orchestrating EwE execution flows, but it operates outside the internal execution of the model. EwE plug-in points, in contrast, allow custom code to intervene at specific stages during model execution. They can be used, for example, to override standard EwE calculations, modify intermediate results, or perform custom calculations within an Ecospace time step. Such interventions cannot be achieved through the regular EwE API alone.

The EwE Bridge Plug-in was developed to make these internal plug-in points accessible to scripts that use the EwE API. At each supported plug-in point, the Bridge raises an event that can be handled by an external script. This allows scripted workflows to respond to EwE plug-in points during model execution and therefore combine the orchestration capabilities of the EwE API with the deeper integration offered by EwE's native plug-in architecture.

The Bridge itself contains no modelling logic. Its sole purpose is to expose selected EwE plug-in points as script-accessible events, providing a lightweight connection between the EwE execution engine and externally scripted workflows.
