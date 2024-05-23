using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parsing
{
    public static class CheckParsing5572
    {
        public static bool Point55(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["processes"] is JArray processes)
                    {
                        foreach (var process in processes)
                        {
                            var processType = process["type"]?.ToString();
                            var processName = process["name"]?.ToString();

                            if (processType == "delete" && process["dataFlows"] is JArray dataFlows)
                            {
                                foreach (var dataFlow in dataFlows)
                                {
                                    var dataFlowLabel = dataFlow["label"]?.ToString();

                                    if (dataFlowLabel != "(delete)")
                                    {
                                        msgBox.AppendText($"Syntax error: The data flow from the delete process '{processName}' is not labeled with (delete).\r\n");
                                        return false;
                                    }

                                    // Ensure no attribute names are shown on the data flow
                                    if (dataFlow["attributes"] != null)
                                    {
                                        msgBox.AppendText($"Syntax error: The data flow from the delete process '{processName}' should not show any attribute names.\r\n");
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }

                // If all checks pass, append success message
                msgBox.AppendText("success : All delete processes are correctly labeled with (delete) and have no attribute names.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error: " + ex.Message + "\r\n");
                return false;
            }
        }

    }
}