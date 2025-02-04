# Sample Project
Within this directory is a sample project built using the framework. It contains Composers, Processes and Modules and demostrates their usage.

The Processes present are split into two sections:
- Simple - Processes that can be ran by themselves using the [run](../Documentation/CLIFunctions.md#run) command, these examples demonstrate simpler operations with little dependency injection or other more advanced funtionality.
- Advanced - Processes that must be invoked by a composer using the [compose](../Documentation/CLIFunctions.md#compose) command, the Advanced Processes require previous Processes to be ran and various flavours of dependency injection in order to run.

The below table details the project's Composers/Processes which are intended as entry points to be ran directly.
| Type | Name | Example Command | Code Link |
|------|------|------|------|
| Composer | AdvancedComposer | doing compose -name AdvancedComposer -showReports -Person1Age 477 | [Code](./SampleProject/Composers/AdvancedComposer.ps1) |
| Composer | ConfigurationComposer | doing compose -name ConfigurationComposer -"MySecondConfig.MyDouble" 8.2221 | [Code](./SampleProject/Composers/ConfigurationComposer.ps1) |
| Process | DoublesANumber | doing run -name DoublesANumber -MySwitch | [Code](./SampleProject/Processes/Simple/DoublesANumber.ps1) |
| Process | FailingProcess | doing run -name FailingProcess -doOutput | [Code](./SampleProject/Processes/Simple/FailingProcess.ps1) |
| Process | SimpleProcess | doing run -name SimpleProcess | [Code](./SampleProject/Processes/Simple/SimpleProcess.ps1) |

More info regarding the sample:
- Example modules are provided to demonstrate their capabilities, take note of the using statements at the top of the Process/Test files.
- Three .env files are included to show their usage.
- Try running the tests using the [test](../Documentation/CLIFunctions.md#test) command!
