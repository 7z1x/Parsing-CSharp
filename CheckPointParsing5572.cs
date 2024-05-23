using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
                                        msgBox.AppendText($"Syntax error 55: The data flow from the delete process '{processName}' is not labeled with (delete).\r\n");
                                        return false;
                                    }

                                    // Ensure no attribute names are shown on the data flow
                                    if (dataFlow["attributes"] != null)
                                    {
                                        msgBox.AppendText($"Syntax error 55: The data flow from the delete process '{processName}' should not show any attribute names.\r\n");
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }

                // If all checks pass, append success message
                msgBox.AppendText("Success 55 : All delete processes are correctly labeled with (delete) and have no attribute names.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point56(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                var dataStores = new HashSet<string>();
                var controlStores = new HashSet<string>();

                // Collect all available data and control stores
                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();

                        if (itemType == "data_store")
                        {
                            var storeName = item["store_name"]?.ToString();
                            if (!string.IsNullOrWhiteSpace(storeName))
                            {
                                dataStores.Add(storeName);
                            }
                        }
                        else if (itemType == "control_store")
                        {
                            var controlName = item["control_name"]?.ToString();
                            if (!string.IsNullOrWhiteSpace(controlName))
                            {
                                controlStores.Add(controlName);
                            }
                        }
                    }
                }

                // Check each process for required inputs
                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();

                        if (itemType == "process")
                        {
                            var processName = item["process_name"]?.ToString();
                            var dataInputs = item["data_inputs"] as JArray;
                            var controlInputs = item["control_inputs"] as JArray;
                            bool allInputsAvailable = true;

                            if (dataInputs != null)
                            {
                                foreach (var input in dataInputs)
                                {
                                    var inputName = input.ToString();
                                    if (!dataStores.Contains(inputName))
                                    {
                                        msgBox.AppendText($"Syntax error: Data input {inputName} for process {processName} is not available. \r\n");
                                        allInputsAvailable = false;
                                    }
                                }
                            }

                            if (controlInputs != null)
                            {
                                foreach (var input in controlInputs)
                                {
                                    var inputName = input.ToString();
                                    if (!controlStores.Contains(inputName))
                                    {
                                        msgBox.AppendText($"Syntax error: Control input {inputName} for process {processName} is not available. \r\n");
                                        allInputsAvailable = false;
                                    }
                                }
                            }

                            if (!allInputsAvailable)
                            {
                                return false;
                            }
                        }
                    }
                }
                msgBox.AppendText("Success 56: All processes have their required data and control inputs available.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point57(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                var dataStores = new HashSet<string>();
                var controlStores = new HashSet<string>();

                // Collect all available data and control stores
                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();

                        if (itemType == "data_store")
                        {
                            var storeName = item["store_name"]?.ToString();
                            if (!string.IsNullOrWhiteSpace(storeName))
                            {
                                dataStores.Add(storeName);
                            }
                        }
                        else if (itemType == "control_store")
                        {
                            var controlName = item["control_name"]?.ToString();
                            if (!string.IsNullOrWhiteSpace(controlName))
                            {
                                controlStores.Add(controlName);
                            }
                        }
                    }
                }

                // Check each process for required outputs
                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();

                        if (itemType == "process")
                        {
                            var processName = item["process_name"]?.ToString();
                            var dataOutputs = item["data_outputs"] as JArray;
                            var controlOutputs = item["control_outputs"] as JArray;
                            bool allOutputsAvailable = true;

                            if (dataOutputs != null)
                            {
                                foreach (var output in dataOutputs)
                                {
                                    var outputName = output.ToString();
                                    if (!dataStores.Contains(outputName))
                                    {
                                        msgBox.AppendText($"Syntax error: Data output {outputName} from process {processName} is not available in data stores. \r\n");
                                        allOutputsAvailable = false;
                                    }
                                }
                            }

                            if (controlOutputs != null)
                            {
                                foreach (var output in controlOutputs)
                                {
                                    var outputName = output.ToString();
                                    if (!controlStores.Contains(outputName))
                                    {
                                        msgBox.AppendText($"Syntax error: Control output {outputName} from process {processName} is not available in control stores. \r\n");
                                        allOutputsAvailable = false;
                                    }
                                }
                            }

                            if (!allOutputsAvailable)
                            {
                                return false;
                            }
                        }
                    }
                }
                msgBox.AppendText("All processes checked successfully and all outputs are available.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point58(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                var dataStores = new HashSet<string>();
                var controlStores = new HashSet<string>();

                // Mengumpulkan semua data store dan control store yang tersedia
                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();

                        if (itemType == "data_store")
                        {
                            var storeName = item["store_name"]?.ToString();
                            if (!string.IsNullOrWhiteSpace(storeName))
                            {
                                dataStores.Add(storeName);
                            }
                        }
                        else if (itemType == "control_store")
                        {
                            var controlName = item["control_name"]?.ToString();
                            if (!string.IsNullOrWhiteSpace(controlName))
                            {
                                controlStores.Add(controlName);
                            }
                        }
                    }
                }

                // Memeriksa setiap state untuk event data yang diperlukan
                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();

                        if (itemType == "class")
                        {
                            var className = item["class_name"]?.ToString();
                            var states = item["states"] as JArray;

                            if (states != null)
                            {
                                foreach (var state in states)
                                {
                                    var stateName = state["state_name"]?.ToString();
                                    var stateEvents = state["state_event"] as JArray;
                                    var eventData = state["event_data"] as JArray;

                                    if (stateEvents != null)
                                    {
                                        if (eventData == null)
                                        {
                                            msgBox.AppendText($"Syntax error: State {stateName} of class {className} has events but no event data defined. \r\n");
                                            return false;
                                        }

                                        foreach (var stateEvent in stateEvents)
                                        {
                                            var eventName = stateEvent?.ToString();

                                            foreach (var data in eventData)
                                            {
                                                var dataName = data.ToString();
                                                if (!dataStores.Contains(dataName) && !controlStores.Contains(dataName))
                                                {
                                                    msgBox.AppendText($"Syntax error: Event data {dataName} for event {eventName} in state {stateName} of class {className} is not available. \r\n");
                                                    return false;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        msgBox.AppendText($"Syntax error: State {stateName} of class {className} has no events defined. \r\n");
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }

                msgBox.AppendText("All event data are available.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point59(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                var availableDataStores = new HashSet<string>();

                // Mengumpulkan semua data store yang tersedia
                foreach (var item in jsonArray)
                {
                    var itemType = item["type"]?.ToString();

                    if (itemType == "data_store")
                    {
                        var storeName = item["store_name"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(storeName))
                        {
                            availableDataStores.Add(storeName);
                        }
                    }
                }

                // Memeriksa setiap proses, state, dan event untuk data store yang diperlukan
                foreach (var item in jsonArray)
                {
                    var itemType = item["type"]?.ToString();

                    if (itemType == "process")
                    {
                        var dataInputs = item["data_inputs"] as JArray;
                        var dataOutputs = item["data_outputs"] as JArray;

                        CheckDataStoreList(dataInputs, "data_inputs", availableDataStores, msgBox);
                        CheckDataStoreList(dataOutputs, "data_outputs", availableDataStores, msgBox);
                    }
                    else if (itemType == "class")
                    {
                        var states = item["states"] as JArray;
                        if (states != null)
                        {
                            foreach (var state in states)
                            {
                                var stateEvents = state["state_event"] as JArray;
                                if (stateEvents != null)
                                {
                                    foreach (var stateEvent in stateEvents)
                                    {
                                        var eventData = stateEvent["event_data"] as JArray;
                                        CheckDataStoreList(eventData, "event_data", availableDataStores, msgBox);
                                    }
                                }
                            }
                        }
                    }
                }

                msgBox.AppendText("All data stores are available.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error: " + ex.Message + "\r\n");
                return false;
            }
        }

        private static void CheckDataStoreList(JArray dataStores, string listName, HashSet<string> availableDataStores, TextBox msgBox)
        {
            if (dataStores != null)
            {
                foreach (var dataStore in dataStores)
                {
                    var dataStoreName = dataStore.ToString();
                    if (!availableDataStores.Contains(dataStoreName))
                    {
                        msgBox.AppendText($"Syntax error: Data store {dataStoreName} in {listName} is not available. \r\n");
                    }
                }
            }
        }

        public static bool Point60(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                HashSet<string> processIDs = new HashSet<string>();
                HashSet<string> processNames = new HashSet<string>();

                foreach (var item in jsonArray)
                {
                    var itemType = item["type"]?.ToString();

                    if (itemType == "process")
                    {
                        var processID = item["process_id"]?.ToString();
                        var processName = item["process_name"]?.ToString();

                        // Check for unique process IDs
                        if (string.IsNullOrWhiteSpace(processID) || processIDs.Contains(processID))
                        {
                            msgBox.AppendText($"Syntax error: Process ID \"{processID}\" is not unique or empty. \r\n");
                            return false;
                        }
                        else
                        {
                            processIDs.Add(processID);
                        }

                        // Check for meaningful process names
                        if (string.IsNullOrWhiteSpace(processName))
                        {
                            msgBox.AppendText($"Syntax error: Process \"{processID}\" has an empty name. \r\n");
                            return false;
                        }
                        else
                        {
                            processNames.Add(processName);
                        }
                    }
                }

                msgBox.AppendText("All process IDs and names are unique and meaningful.\r\n");
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