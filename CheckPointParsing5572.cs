﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
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
                msgBox.AppendText("Syntax error 55: " + ex.Message + "\r\n");
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
                                        msgBox.AppendText($"Syntax error 56: Data input {inputName} for process {processName} is not available. \r\n");
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
                                        msgBox.AppendText($"Syntax error 56: Control input {inputName} for process {processName} is not available. \r\n");
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
                msgBox.AppendText("Syntax error 56: " + ex.Message + "\r\n");
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
                                        msgBox.AppendText($"Syntax error 57: Data output {outputName} from process {processName} is not available in data stores. \r\n");
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
                                        msgBox.AppendText($"Syntax error 57: Control output {outputName} from process {processName} is not available in control stores. \r\n");
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
                msgBox.AppendText("Success 57: All processes checked successfully and all outputs are available.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 57: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point58(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            int num = 1;

            try
            {
                var dataStores = new HashSet<string>();
                var controlStores = new HashSet<string>();

                // Mengumpulkan semua data store dan control store yang tersedia
                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        var itemType = item["typedata"]?.ToString();

                        if (itemType == "dataStore")
                        {
                            var storeName = item["store_name"]?.ToString();
                            if (!string.IsNullOrWhiteSpace(storeName))
                            {
                                dataStores.Add(storeName);
                                Debug.WriteLine($"DataStore : {storeName}");
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
                                    var stateName = state["state_model_name"]?.ToString();
                                    var stateId = state["state_id"]?.ToString();
                                    var stateEvents = state["state_event"] as JArray;
                                    var eventData = state["event_data"] as JArray;
                                    
                                    if (stateEvents != null)
                                    {

                                        if (eventData == null)
                                        {
                                            msgBox.AppendText($"Syntax error 58: State {stateName} with id {stateId} of class {className} has events but no event data defined. \r\n");
                                            return false;
                                        }

                                        foreach (var stateEvent in stateEvents)
                                        {
                                            var eventName = stateEvent?.ToString();


                                            foreach (var data in eventData)
                                            {
                                                var dataName = data.ToString();
                                                Debug.WriteLine($"dn : {dataName}");
                                                controlStores.ToList<String>().ForEach(x => Debug.WriteLine($"dns : {x}"));

                                                if (!dataStores.First().Contains(dataName) && !controlStores.Contains(dataName))
                                                {
                                                    msgBox.AppendText($"Syntax error 58: Event data {dataName} for event {eventName} in state {stateName} of class {className} is not available on {String.Join(",", dataStores )} or on .{String.Join(",", controlStores)} \r\n");
                                                    return false;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        msgBox.AppendText($"Syntax error 58: State {stateName} of class {className} has no events defined. 2 \r\n");
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }

                msgBox.AppendText("Success 58: All event data are available.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 58: " + ex.Message + "\r\n");
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

                msgBox.AppendText("Success 59: All data stores are available.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 59: " + ex.Message + "\r\n");
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
                        msgBox.AppendText($"Syntax error 59: Data store {dataStoreName} in {listName} is not available. \r\n");
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
                            msgBox.AppendText($"Syntax error 60: Process ID \"{processID}\" is not unique or empty. \r\n");
                            return false;
                        }
                        else
                        {
                            processIDs.Add(processID);
                        }

                        // Check for meaningful process names
                        if (string.IsNullOrWhiteSpace(processName))
                        {
                            msgBox.AppendText($"Syntax error 60: Process \"{processID}\" has an empty name. \r\n");
                            return false;
                        }
                        else
                        {
                            processNames.Add(processName);
                        }
                    }
                }

                msgBox.AppendText("Success 60: All process IDs and names are unique and meaningful.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 60: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point61(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        if (item["processes"] is JArray processes)
                        {
                            foreach (var process in processes)
                            {
                                var processType = process["process_type"]?.ToString();
                                if (string.IsNullOrWhiteSpace(processType))
                                {
                                    msgBox.AppendText("Syntax error 61: Process type is missing.\r\n");
                                    continue;
                                }

                                switch (processType)
                                {
                                    case "accessor":
                                        if (process["data_store"] == null)
                                        {
                                            msgBox.AppendText($"Syntax error 61: Accessor process {process["process_name"]} must specify a data store.\r\n");
                                        }
                                        break;
                                    case "transformation":
                                        if (process["input_data"] == null || process["output_data"] == null)
                                        {
                                            msgBox.AppendText($"Syntax error 61: Transformation process {process["process_name"]} must specify input and output data.\r\n");
                                        }
                                        break;
                                    case "event_generator":
                                        if (process["event"] == null)
                                        {
                                            msgBox.AppendText($"Syntax error 61: Event generator process {process["process_name"]} must specify an event.\r\n");
                                        }
                                        break;
                                    case "test":
                                        if (process["condition"] == null || process["true_output"] == null || process["false_output"] == null)
                                        {
                                            msgBox.AppendText($"Syntax error 61: Test process {process["process_name"]} must specify a condition and true/false outputs.\r\n");
                                        }
                                        break;
                                    default:
                                        msgBox.AppendText($"Syntax error 61: Unknown process type {processType} in process {process["process_name"]}.\r\n");
                                        break;
                                }
                            }
                        }
                    }
                }
                msgBox.AppendText("Success 61: All processes checked successfully. No syntax errors found.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 61: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point62(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        var processDict = new Dictionary<string, JObject>();

                        foreach (var item in model)
                        {
                            if (item["processes"] is JArray processes)
                            {
                                foreach (var process in processes)
                                {
                                    var processType = process["process_type"]?.ToString();
                                    var dataStore = process["data_store"]?.ToString();
                                    var inputData = process["input_data"]?.ToString();
                                    var outputData = process["output_data"]?.ToString();
                                    var eventGenerated = process["event"]?.ToString();
                                    var condition = process["condition"]?.ToString();
                                    var trueOutput = process["true_output"]?.ToString();
                                    var falseOutput = process["false_output"]?.ToString();

                                    var processKey = $"{processType}-{dataStore}-{inputData}-{outputData}-{eventGenerated}-{condition}-{trueOutput}-{falseOutput}";

                                    if (processDict.ContainsKey(processKey))
                                    {
                                        var existingProcess = processDict[processKey];
                                        var existingProcessId = existingProcess["process_id"]?.ToString();
                                        var existingProcessName = existingProcess["process_name"]?.ToString();
                                        var currentProcessId = process["process_id"]?.ToString();
                                        var currentProcessName = process["process_name"]?.ToString();

                                        if (existingProcessId != currentProcessId || existingProcessName != currentProcessName)
                                        {
                                            msgBox.AppendText($"Syntax error 62: Duplicate process with different identifiers or names. Existing: {existingProcessName} ({existingProcessId}), Current: {currentProcessName} ({currentProcessId})\r\n");
                                        }
                                    }
                                    else
                                    {
                                        processDict[processKey] = (JObject)process;
                                    }
                                }
                            }
                        }
                    }
                }

                // Menambahkan pesan "Success" jika tidak ada kesalahan
                msgBox.AppendText("Success 62: No duplicate processes found.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 62: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point63(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                var keyLetterDict = new Dictionary<string, HashSet<int>>();

                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var item in model)
                        {
                            if (item["processes"] is JArray processes)
                            {
                                foreach (var process in processes)
                                {
                                    var processId = process["process_id"]?.ToString();
                                    if (string.IsNullOrWhiteSpace(processId))
                                    {
                                        msgBox.AppendText("Syntax error 63: Process identifier is missing.\r\n");
                                        continue;
                                    }

                                    // Regex to match process identifier format KL.i
                                    var match = Regex.Match(processId, @"^(?<KL>[A-Za-z]+)\.(?<i>\d+)$");
                                    if (!match.Success)
                                    {
                                        msgBox.AppendText($"Syntax error 63: Process identifier {processId} does not match the format KL.i.\r\n");
                                        continue;
                                    }

                                    var keyLetter = match.Groups["KL"].Value;
                                    var index = int.Parse(match.Groups["i"].Value);

                                    if (!keyLetterDict.ContainsKey(keyLetter))
                                    {
                                        keyLetterDict[keyLetter] = new HashSet<int>();
                                    }

                                    if (keyLetterDict[keyLetter].Contains(index))
                                    {
                                        msgBox.AppendText($"Syntax error 63: Process identifier {processId} has a duplicate integer for KeyLetter {keyLetter}.\r\n");
                                    }
                                    else
                                    {
                                        keyLetterDict[keyLetter].Add(index);
                                    }
                                }
                            }
                        }
                    }
                }

                // Menambahkan pesan "Success" jika tidak ada kesalahan
                msgBox.AppendText("Success 63: All process identifiers are valid.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 63: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point64(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                var dataStoreToKeyLetter = new Dictionary<string, string>();

                // Map data stores to KeyLetters
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var item in model)
                        {
                            var dataStore = item["data_store"]?.ToString();
                            var keyLetter = item["KL"]?.ToString();

                            if (!string.IsNullOrWhiteSpace(dataStore) && !string.IsNullOrWhiteSpace(keyLetter))
                            {
                                dataStoreToKeyLetter[dataStore] = keyLetter;
                            }
                        }
                    }
                }

                // Check each accessor process
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var item in model)
                        {
                            if (item["processes"] is JArray processes)
                            {
                                foreach (var process in processes)
                                {
                                    var processType = process["process_type"]?.ToString();
                                    if (processType == "accessor")
                                    {
                                        var processId = process["process_id"]?.ToString();
                                        var dataStore = process["data_store"]?.ToString();

                                        if (string.IsNullOrWhiteSpace(processId) || string.IsNullOrWhiteSpace(dataStore))
                                        {
                                            msgBox.AppendText("Syntax error 64: Accessor process identifier or data store is missing.\r\n");
                                            continue;
                                        }

                                        var match = Regex.Match(processId, @"^(?<KL>[A-Za-z]+)\.\d+$");
                                        if (!match.Success)
                                        {
                                            msgBox.AppendText($"Syntax error 64: Process identifier {processId} does not match the format KL.i.\r\n");
                                            continue;
                                        }

                                        var processKeyLetter = match.Groups["KL"].Value;

                                        if (dataStoreToKeyLetter.TryGetValue(dataStore, out var expectedKeyLetter))
                                        {
                                            if (processKeyLetter != expectedKeyLetter)
                                            {
                                                msgBox.AppendText($"Syntax error 64: Accessor process {processId} has KeyLetter {processKeyLetter} which does not match the expected KeyLetter {expectedKeyLetter} for data store {dataStore}.\r\n");
                                            }
                                        }
                                        else
                                        {
                                            msgBox.AppendText($"Syntax error 64: Data store {dataStore} is not mapped to any KeyLetter.\r\n");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Menambahkan pesan "Success" jika tidak ada kesalahan
                msgBox.AppendText("Success 64: All accessor processes have valid KeyLetters.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 64: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point65(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var item in model)
                        {
                            if (item["processes"] is JArray processes)
                            {
                                foreach (var process in processes)
                                {
                                    var processType = process["process_type"]?.ToString();
                                    var dataStore = process["data_store"]?.ToString();
                                    var processId = process["process_id"]?.ToString();
                                    var processName = process["process_name"]?.ToString();

                                    // Check if it's an accessor for timer data store
                                    if (processType == "accessor" && dataStore == "timer")
                                    {
                                        if (processId == "Tim.3" && processName != "createTimer")
                                        {
                                            msgBox.AppendText("Syntax error 65: Accessor Tim.3 should be named createTimer.\r\n");
                                        }
                                        else if (processId == "Tim.4" && processName != "deleteTimer")
                                        {
                                            msgBox.AppendText("Syntax error 65: Accessor Tim.4 should be named deleteTimer.\r\n");
                                        }
                                        else if (processId == "Tim.5" && processName != "getRemainingTime")
                                        {
                                            msgBox.AppendText("Syntax error 65: Accessor Tim.5 should be named getRemainingTime.\r\n");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Menambahkan pesan "Success" jika tidak ada kesalahan
                msgBox.AppendText("Success 65: All timer accessor names are correct.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 65: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point66(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                var stateModelToKeyLetter = new Dictionary<string, string>();
                var dataStoreToKeyLetter = new Dictionary<string, string>();

                // Map state models to KeyLetters
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var item in model)
                        {
                            var itemType = item["type"]?.ToString();
                            var classId = item["class_id"]?.ToString();
                            var keyLetter = item["KL"]?.ToString();

                            if (itemType == "class" && !string.IsNullOrWhiteSpace(classId) && !string.IsNullOrWhiteSpace(keyLetter))
                            {
                                stateModelToKeyLetter[classId] = keyLetter;

                                // If the class has a data store, map it to KeyLetter as well
                                var dataStore = item["data_store"]?.ToString();
                                if (!string.IsNullOrWhiteSpace(dataStore))
                                {
                                    dataStoreToKeyLetter[dataStore] = keyLetter;
                                }
                            }
                        }
                    }
                }

                // Check each transformation process
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var item in model)
                        {
                            if (item["processes"] is JArray processes)
                            {
                                foreach (var process in processes)
                                {
                                    var processType = process["process_type"]?.ToString();
                                    var processId = process["process_id"]?.ToString();
                                    var processName = process["process_name"]?.ToString();

                                    // Check if it's a transformation process
                                    if (processType == "transformation")
                                    {
                                        var stateModelId = process["state_model_id"]?.ToString();
                                        if (string.IsNullOrWhiteSpace(stateModelId))
                                        {
                                            msgBox.AppendText($"Syntax error 66: Transformation process {processId} is missing state_model_id.\r\n");
                                            continue;
                                        }

                                        if (!stateModelToKeyLetter.TryGetValue(stateModelId, out var expectedKeyLetter))
                                        {
                                            msgBox.AppendText($"Syntax error 66: State model with ID {stateModelId} is not mapped to any KeyLetter.\r\n");
                                            continue;
                                        }

                                        var processKeyLetter = processId?.Split('.')[0];
                                        if (processKeyLetter != expectedKeyLetter)
                                        {
                                            msgBox.AppendText($"Syntax error 66: Transformation process {processId} has KeyLetter {processKeyLetter} which does not match the expected KeyLetter {expectedKeyLetter} for state model {stateModelId}.\r\n");
                                        }

                                        // Check if it accesses data store
                                        var dataStore = process["data_store"]?.ToString();
                                        if (!string.IsNullOrWhiteSpace(dataStore))
                                        {
                                            if (dataStoreToKeyLetter.TryGetValue(dataStore, out var dataStoreKeyLetter) && dataStoreKeyLetter != expectedKeyLetter)
                                            {
                                                msgBox.AppendText($"Syntax error 66: Transformation process {processId} accesses data store {dataStore} with KeyLetter {dataStoreKeyLetter} which does not match the expected KeyLetter {expectedKeyLetter} for state model {stateModelId}.\r\n");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                msgBox.AppendText("Success 66: All transformation processes have correct KeyLetters.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 66: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point67(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                var objectToKeyLetter = new Dictionary<string, string>();

                // Map objects and external entities to KeyLetters
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var item in model)
                        {
                            var itemType = item["type"]?.ToString();
                            var objectId = item["class_id"]?.ToString();
                            var keyLetter = item["KL"]?.ToString();

                            if (itemType == "class" && !string.IsNullOrWhiteSpace(objectId) && !string.IsNullOrWhiteSpace(keyLetter))
                            {
                                objectToKeyLetter[objectId] = keyLetter;
                            }
                        }
                    }
                }

                // Check each event generator process
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var item in model)
                        {
                            if (item["processes"] is JArray processes)
                            {
                                foreach (var process in processes)
                                {
                                    var processType = process["process_type"]?.ToString();
                                    var processId = process["process_id"]?.ToString();
                                    var processName = process["process_name"]?.ToString();

                                    // Check if it's an event generator
                                    if (processType == "event_generator")
                                    {
                                        var targetObjectId = process["class_id"]?.ToString();
                                        if (string.IsNullOrWhiteSpace(targetObjectId))
                                        {
                                            msgBox.AppendText($"Syntax error 67: Event generator process {processId} is missing class_id.\r\n");
                                            continue;
                                        }

                                        if (!objectToKeyLetter.TryGetValue(targetObjectId, out var expectedKeyLetter))
                                        {
                                            msgBox.AppendText($"Syntax error 67: Object or external entity with ID {targetObjectId} is not mapped to any KeyLetter.\r\n");
                                            continue;
                                        }

                                        var processKeyLetter = processId?.Split('.')[0];
                                        if (processKeyLetter != expectedKeyLetter)
                                        {
                                            msgBox.AppendText($"Syntax error 67: Event generator process {processId} has KeyLetter {processKeyLetter} which does not match the expected KeyLetter {expectedKeyLetter} for object or external entity {targetObjectId}.\r\n");
                                        }

                                        // Check if it accesses data store
                                        var dataStore = process["data_store"]?.ToString();
                                        if (!string.IsNullOrWhiteSpace(dataStore))
                                        {
                                            msgBox.AppendText($"Syntax error 67: Event generator process {processId} should not access any data stores, but it accesses data store {dataStore}.\r\n");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Menambahkan pesan "Success" jika tidak ada kesalahan
                msgBox.AppendText("Success 67: All event generator processes have correct KeyLetters.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 67: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point68(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                bool hasError = false;

                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var item in model)
                        {
                            if (item["processes"] is JArray processes)
                            {
                                foreach (var process in processes)
                                {
                                    var processType = process["process_type"]?.ToString();
                                    var processId = process["process_id"]?.ToString();
                                    var dataStore = process["data_store"]?.ToString();

                                    // Check if it's an event generator involving TIMER data store
                                    if (processType == "event_generator" && dataStore == "TIMER")
                                    {
                                        if (processId == "TIM.1")
                                        {
                                            if (process["process_name"]?.ToString() != "setTimer")
                                            {
                                                msgBox.AppendText($"Syntax error 68: Event generator process {processId} should be named 'setTimer'.\r\n");
                                                hasError = true;
                                            }
                                        }
                                        else if (processId == "TIM.2")
                                        {
                                            if (process["process_name"]?.ToString() != "resetTimer")
                                            {
                                                msgBox.AppendText($"Syntax error 68: Event generator process {processId} should be named 'resetTimer'.\r\n");
                                                hasError = true;
                                            }
                                        }
                                        else
                                        {
                                            msgBox.AppendText($"Syntax error 68: Invalid event generator process ID {processId} for TIMER data store. Expected 'TIM.1' or 'TIM.2'.\r\n");
                                            hasError = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (!hasError)
                {
                    msgBox.AppendText("Success 68: All timer event generator processes have correct names.\r\n");
                }

                return !hasError;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 68: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point69(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                var stateModelToKeyLetter = new Dictionary<string, string>();
                var dataStoreToKeyLetter = new Dictionary<string, string>();
                bool hasError = false;

                // Map state models to KeyLetters
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var item in model)
                        {
                            var itemType = item["type"]?.ToString();
                            var classId = item["class_id"]?.ToString();
                            var keyLetter = item["KL"]?.ToString();

                            if (itemType == "class" && !string.IsNullOrWhiteSpace(classId) && !string.IsNullOrWhiteSpace(keyLetter))
                            {
                                stateModelToKeyLetter[classId] = keyLetter;

                                // If the class has a data store, map it to KeyLetter as well
                                var dataStore = item["data_store"]?.ToString();
                                if (!string.IsNullOrWhiteSpace(dataStore))
                                {
                                    dataStoreToKeyLetter[dataStore] = keyLetter;
                                }
                            }
                        }
                    }
                }

                // Check each test process
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var item in model)
                        {
                            if (item["processes"] is JArray processes)
                            {
                                foreach (var process in processes)
                                {
                                    var processType = process["process_type"]?.ToString();
                                    var processId = process["process_id"]?.ToString();

                                    // Check if it's a test process
                                    if (processType == "test")
                                    {
                                        var stateModelId = process["state_model_id"]?.ToString();
                                        if (string.IsNullOrWhiteSpace(stateModelId))
                                        {
                                            msgBox.AppendText($"Syntax error 69: Test process {processId} is missing state_model_id.\r\n");
                                            hasError = true;
                                            continue;
                                        }

                                        if (!stateModelToKeyLetter.TryGetValue(stateModelId, out var expectedKeyLetter))
                                        {
                                            msgBox.AppendText($"Syntax error 69: State model with ID {stateModelId} is not mapped to any KeyLetter.\r\n");
                                            hasError = true;
                                            continue;
                                        }

                                        var processKeyLetter = processId?.Split('.')[0];
                                        if (processKeyLetter != expectedKeyLetter)
                                        {
                                            msgBox.AppendText($"Syntax error 69: Test process {processId} has KeyLetter {processKeyLetter} which does not match the expected KeyLetter {expectedKeyLetter} for state model {stateModelId}.\r\n");
                                            hasError = true;
                                        }

                                        // Check if it accesses data store
                                        var dataStore = process["data_store"]?.ToString();
                                        if (!string.IsNullOrWhiteSpace(dataStore) && dataStoreToKeyLetter.TryGetValue(dataStore, out var dataStoreKeyLetter))
                                        {
                                            if (dataStoreKeyLetter != expectedKeyLetter)
                                            {
                                                msgBox.AppendText($"Syntax error 69: Test process {processId} accesses data store {dataStore} with KeyLetter {dataStoreKeyLetter} which does not match the expected KeyLetter {expectedKeyLetter} for state model {stateModelId}.\r\n");
                                                hasError = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (!hasError)
                {
                    msgBox.AppendText("Success 69: All test processes have correct KeyLetters.\r\n");
                }

                return !hasError;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 69: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point70(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                var objectToKeyLetter = new Dictionary<string, string>();
                var dataStoreToObject = new Dictionary<string, string>();
                var objectAccessModel = new List<Tuple<string, string, string>>(); // Tuple<SourceObject, TargetObject, ProcessId>

                // Map objects to KeyLetters and data stores to objects
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var item in model)
                        {
                            var itemType = item["type"]?.ToString();
                            var objectId = item["class_id"]?.ToString();
                            var keyLetter = item["KL"]?.ToString();
                            var dataStore = item["data_store"]?.ToString();

                            if (itemType == "class" && !string.IsNullOrWhiteSpace(objectId) && !string.IsNullOrWhiteSpace(keyLetter))
                            {
                                objectToKeyLetter[objectId] = keyLetter;

                                if (!string.IsNullOrWhiteSpace(dataStore))
                                {
                                    dataStoreToObject[dataStore] = objectId;
                                }
                            }
                        }
                    }
                }

                // Check each process in state models
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var item in model)
                        {
                            if (item["state_diagram"] is JArray stateDiagram)
                            {
                                foreach (var state in stateDiagram)
                                {
                                    if (state["actions"] is JArray actions)
                                    {
                                        foreach (var action in actions)
                                        {
                                            var actionType = action["action_type"]?.ToString();
                                            var actionId = action["action_id"]?.ToString();
                                            var processId = action["process_id"]?.ToString();
                                            var dataStore = action["data_store"]?.ToString();

                                            if (actionType == "access" && !string.IsNullOrWhiteSpace(dataStore))
                                            {
                                                if (dataStoreToObject.TryGetValue(dataStore, out var targetObjectId))
                                                {
                                                    var sourceObjectId = action["class_id"]?.ToString();

                                                    if (!string.IsNullOrWhiteSpace(sourceObjectId) && !string.IsNullOrWhiteSpace(targetObjectId))
                                                    {
                                                        objectAccessModel.Add(new Tuple<string, string, string>(sourceObjectId, targetObjectId, processId));
                                                    }
                                                    else
                                                    {
                                                        msgBox.AppendText($"Syntax error 70: Missing source or target object ID for action {actionId} accessing data store {dataStore}.\r\n");
                                                    }
                                                }
                                                else
                                                {
                                                    msgBox.AppendText($"Syntax error 70: Data store {dataStore} is not mapped to any object.\r\n");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Validate object access model
                foreach (var access in objectAccessModel)
                {
                    var sourceObject = access.Item1;
                    var targetObject = access.Item2;
                    var processId = access.Item3;

                    if (objectToKeyLetter.TryGetValue(sourceObject, out var sourceKeyLetter) &&
                        objectToKeyLetter.TryGetValue(targetObject, out var targetKeyLetter))
                    {
                        var expectedProcessId = $"{sourceKeyLetter}.{processId.Split('.')[1]}";

                        if (processId != expectedProcessId)
                        {
                            msgBox.AppendText($"Syntax error 70: Process ID {processId} for action accessing from {sourceKeyLetter} to {targetKeyLetter} should be {expectedProcessId}.\r\n");
                            return false;

                        }
                    }
                    else
                    {
                        msgBox.AppendText($"Syntax error 70: Source or target object not found in key letter mapping.\r\n");
                        return false;

                    }
                }
                msgBox.AppendText("Success 70: All access actions have correct process IDs.\r\n");
                return true;

            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 70: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point71(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                HashSet<string> domainSubsystems = new HashSet<string>();
                HashSet<string> communicationModelSubsystems = new HashSet<string>();

                // Identify all subsystems in the domain
                foreach (var subsystem in jsonArray)
                {
                    var subName = subsystem["sub_name"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(subName))
                    {
                        domainSubsystems.Add(subName);
                    }
                }

                // Find communication model and check subsystems
                var communicationModel = jsonArray[0]["model"].FirstOrDefault(x => x["type"]?.ToString() == "subsystem_communication_model");
                Debug.WriteLine(communicationModel.ToString());
                if (communicationModel != null)
                {
                    var subsystemsArray = communicationModel["subsystems"] as JArray;
                    if (subsystemsArray != null)
                    {
                        foreach (var subsystem in subsystemsArray)
                        {
                            var communicationModelSubName = subsystem["sub_name"]?.ToString();
                            if (!string.IsNullOrWhiteSpace(communicationModelSubName))
                            {
                                communicationModelSubsystems.Add(communicationModelSubName);
                            }
                        }
                    }
                }

                // Check if all domain subsystems appear in the Subsystem Communication Model
                foreach (var subName in domainSubsystems)
                {
                    if (!communicationModelSubsystems.Contains(subName))
                    {
                        msgBox.AppendText($"Syntax error 71: Subsystem '{subName}' does not appear in the Subsystem Communication Model.\r\n");
                        return false;
                    }
                }

                msgBox.AppendText("Success 71: All domain subsystems appear in the Subsystem Communication Model.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 71: " + ex.Message + "\r\n");
                return false;
            }
        }



        public static bool Point72(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                Dictionary<string, HashSet<string>> subsystemGeneratedEvents = new Dictionary<string, HashSet<string>>();
                Dictionary<string, HashSet<string>> subsystemReceivedEvents = new Dictionary<string, HashSet<string>>();

                // Identify events generated and received by state models in each subsystem
                foreach (var subsystem in jsonArray)
                {
                    var subName = subsystem["sub_name"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(subName))
                    {
                        var generatedEvents = new HashSet<string>();
                        var receivedEvents = new HashSet<string>();

                        if (subsystem["model"] is JArray model)
                        {
                            foreach (var item in model)
                            {
                                if (item["state_diagram"] is JArray stateDiagram)
                                {
                                    foreach (var state in stateDiagram)
                                    {
                                        if (state["actions"] is JArray actions)
                                        {
                                            foreach (var action in actions)
                                            {
                                                var actionType = action["action_type"]?.ToString();
                                                var eventName = action["event_name"]?.ToString();

                                                if (actionType == "generate" && !string.IsNullOrWhiteSpace(eventName))
                                                {
                                                    generatedEvents.Add(eventName);
                                                }
                                                else if (actionType == "receive" && !string.IsNullOrWhiteSpace(eventName))
                                                {
                                                    receivedEvents.Add(eventName);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        subsystemGeneratedEvents[subName] = generatedEvents;
                        subsystemReceivedEvents[subName] = receivedEvents;
                    }
                }

                // Check if events generated and received between subsystems appear in the Subsystem Communication Model
                foreach (var senderEntry in subsystemGeneratedEvents)
                {
                    var senderSub = senderEntry.Key;
                    var senderEvents = senderEntry.Value;

                    foreach (var receiverEntry in subsystemReceivedEvents)
                    {
                        var receiverSub = receiverEntry.Key;
                        var receiverEvents = receiverEntry.Value;

                        if (senderSub != receiverSub)
                        {
                            foreach (var evt in senderEvents)
                            {
                                if (receiverEvents.Contains(evt))
                                {
                                    msgBox.AppendText($"Syntax error 72: Event '{evt}' generated by state model in '{senderSub}' and received by state model in '{receiverSub}' must appear in the Subsystem Communication Model.\r\n");
                                    return false;
                                }
                            }
                        }
                    }
                }
                msgBox.AppendText("Success 72: All events are correctly placed in the Subsystem Communication Model.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 72: " + ex.Message + "\r\n");
                return false;
            }

        }
        public static bool Point21(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                HashSet<string> reservedNames = new HashSet<string> { "TIMER" };
                HashSet<string> reservedKeyLetters = new HashSet<string> { "TIM" };
                bool timerObjectFound = false;

                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();
                        var className = item["class_name"]?.ToString();
                        var keyLetter = item["KL"]?.ToString();

                        if (itemType == "class" && className == "TIMER" && keyLetter == "TIM")
                        {
                            timerObjectFound = true;
                            var attributes = item["attributes"] as JArray;
                            var requiredAttributes = new HashSet<string> { "timer_id", "instance_id", "event_label", "time_remaining", "timer_status" };

                            foreach (var attribute in attributes)
                            {
                                var attributeName = attribute["attribute_name"]?.ToString();
                                requiredAttributes.Remove(attributeName);
                            }

                            if (requiredAttributes.Count > 0)
                            {
                                msgBox.AppendText("Syntax error 21: TIMER object is missing required attributes: " + string.Join(", ", requiredAttributes) + ".\r\n");
                            }
                        }
                        else
                        {
                            if (reservedNames.Contains(className))
                            {
                                msgBox.AppendText($"Syntax error 21: Reserved name {className} used for another object.\r\n");
                            }

                            if (reservedKeyLetters.Contains(keyLetter))
                            {
                                msgBox.AppendText($"Syntax error 21: Reserved KeyLetter {keyLetter} used for another object.\r\n");
                            }
                        }
                    }
                }

                if (!timerObjectFound)
                {
                    msgBox.AppendText("Syntax error 21: TIMER object with KeyLetter TIM not found in the subsystem.\r\n");
                }

                msgBox.AppendText("Success 21: TIMER object with KeyLetter TIM found in the subsystem.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 21: " + ex.Message + "\r\n");
                return false;
            }
        }
    }
}