using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading;

namespace ClientWorker.Helper
{
    //referência: https://docs.microsoft.com/pt-pt/archive/blogs/kebab/executing-powershell-scripts-from-c
    public class PowerShellManager
    {
        
        public static void PowerShellExecuter(string command)
        {
            
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {

                // this script has a sleep in it to simulate a long running script
                PowerShellInstance.AddScript(command);

                // begin invoke execution on the pipeline
                IAsyncResult result = PowerShellInstance.BeginInvoke();

                // do something else until execution has completed.
                // this could be sleep/wait, or perhaps some other work
                while (result.IsCompleted == false)
                {

                    Thread.Sleep(500);
                    // might want to place a timeout here...
                }
            }
        }
    }
}
