using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace Parsing
{
    public static class CheckPointParsing3654
    {
        public static bool Point36(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();

            try
            {
                foreach (var subsistem in jsonArray)
                {
                    if (subsistem?["type"]?.ToString() == "subsystem")
                    {
                        var model = subsistem["model"];

                        foreach (var item in model)
                        {
                            if (item?["type"]?.ToString() == "action")
                            {
                                var actionType = item["action_type"]?.ToString();
                                if (actionType == "create" || actionType == "delete")
                                {
                                    var affectedClass = item["affected_class"]?.ToString();

                                    // Check relationships consistency
                                    bool isConsistent = true;
                                    foreach (var sub in jsonArray)
                                    {
                                        var subModel = sub["model"];
                                        foreach (var subItem in subModel)
                                        {
                                            if (subItem?["type"]?.ToString() == "association")
                                            {
                                                var classes = subItem["class"];
                                                if (classes != null && classes.Any(cls => cls["class_name"]?.ToString() == affectedClass))
                                                {
                                                    // Add detailed consistency checks here if needed
                                                    if (string.IsNullOrEmpty(subItem["name"]?.ToString()) || classes.Count() < 2)
                                                    {
                                                        isConsistent = false;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (!isConsistent)
                                    {
                                        msgBox.AppendText($"Syntax error: Action {actionType} on class {affectedClass} has inconsistent relationships. \r\n");
                                    }
                                }
                            }
                        }
                    }
                }
                msgBox.AppendText("Sukses 36 \n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point37(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();

            try
            {
                foreach (var subsistem in jsonArray)
                {
                    if (subsistem?["type"]?.ToString() == "subsystem")
                    {
                        var model = subsistem["model"];

                        foreach (var item in model)
                        {
                            if (item?["type"]?.ToString() == "action")
                            {
                                var subtypes = item["subtypes"];
                                var supertypes = item["supertypes"];

                                // Check if subtypes and supertypes are consistently populated
                                if (subtypes == null || supertypes == null || !subtypes.Any() || !supertypes.Any())
                                {
                                    msgBox.AppendText($"Syntax error 37: Action {item["name"]} does not leave subtypes and supertypes consistently populated. \r\n");
                                    continue;
                                }

                                // Check for consistent population
                                foreach (var subtype in subtypes)
                                {
                                    var subtypeId = subtype["id"]?.ToString();
                                    bool foundCorrespondingSupertype = false;

                                    foreach (var supertype in supertypes)
                                    {
                                        var supertypeId = supertype["id"]?.ToString();
                                        if (subtypeId == supertypeId)
                                        {
                                            foundCorrespondingSupertype = true;
                                            break;
                                        }
                                    }

                                    if (!foundCorrespondingSupertype)
                                    {
                                        msgBox.AppendText($"Syntax error 37: Action {item["name"]} has subtype {subtypeId} without a corresponding supertype. \r\n");
                                    }
                                }

                                foreach (var supertype in supertypes)
                                {
                                    var supertypeId = supertype["id"]?.ToString();
                                    bool foundCorrespondingSubtype = false;

                                    foreach (var subtype in subtypes)
                                    {
                                        var subtypeId = subtype["id"]?.ToString();
                                        if (supertypeId == subtypeId)
                                        {
                                            foundCorrespondingSubtype = true;
                                            break;
                                        }
                                    }

                                    if (!foundCorrespondingSubtype)
                                    {
                                        msgBox.AppendText($"Syntax error 37: Action {item["name"]} has supertype {supertypeId} without a corresponding subtype. \r\n");
                                    }
                                }
                            }
                        }
                    }
                }
                msgBox.AppendText("Sukses 37 \n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 37: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point38(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();

            try
            {
                foreach (var subsistem in jsonArray)
                {
                    if (subsistem?["type"]?.ToString() == "subsystem")
                    {
                        var model = subsistem["model"];

                        foreach (var item in model)
                        {
                            if (item?["type"]?.ToString() == "action")
                            {
                                var stateTransitions = item["stateTransitions"];
                                var actionName = item["name"]?.ToString();

                                // Check if action is a deletion state
                                bool isDeletionState = item["deletionState"]?.ToObject<bool>() ?? false;

                                if (!isDeletionState)
                                {
                                    bool updatesCurrentState = false;

                                    if (stateTransitions != null)
                                    {
                                        foreach (var transition in stateTransitions)
                                        {
                                            var transitionAttributes = transition["attributes"];
                                            if (transitionAttributes != null)
                                            {
                                                foreach (var attribute in transitionAttributes)
                                                {
                                                    if (attribute["name"]?.ToString() == "currentState")
                                                    {
                                                        updatesCurrentState = true;
                                                        break;
                                                    }
                                                }
                                            }
                                            if (updatesCurrentState) break;
                                        }
                                    }

                                    if (!updatesCurrentState)
                                    {
                                        msgBox.AppendText($"Syntax error 38: Action {actionName} does not update the current state attribute.\r\n");
                                    }
                                }
                            }
                        }
                    }
                }

                msgBox.AppendText("Sukses 38 \n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 38: " + ex.Message + "\r\n");
                return false;
            }
        }
        public static bool Point39(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();

            try
            {
                foreach (var subsistem in jsonArray)
                {
                    if (subsistem?["type"]?.ToString() == "subsystem")
                    {
                        var model = subsistem["model"];

                        foreach (var item in model)
                        {
                            if (item?["type"]?.ToString() == "action")
                            {
                                var stateName = item["state"]?["name"]?.ToString();
                                var processModelName = item["processModel"]?["name"]?.ToString();
                                var actionName = item["name"]?.ToString();

                                // Check if stateName and processModelName are not null and are equal
                                if (stateName == null || processModelName == null || stateName != processModelName)
                                {
                                    msgBox.AppendText($"Syntax error 39: Action {actionName} has state {stateName} but the process model is {processModelName}.\r\n");
                                }
                            }
                        }
                    }
                }
                msgBox.AppendText("Sukses 39 \r\n");

                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 39: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point40(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();

            try
            {
                foreach (var subsistem in jsonArray)
                {
                    if (subsistem?["type"]?.ToString() == "subsystem")
                    {
                        var model = subsistem["model"];

                        foreach (var item in model)
                        {
                            if (item?["type"]?.ToString() == "processModel")
                            {
                                var processes = item["processes"];
                                var dataFlows = item["dataFlows"];
                                var controlFlows = item["controlFlows"];
                                var dataStores = item["dataStores"];
                                var processModelName = item["name"]?.ToString();

                                // Check if processes and data flows are not null and have at least one element
                                bool hasProcesses = processes != null && processes.Any();
                                bool hasDataFlows = dataFlows != null && dataFlows.Any();

                                if (!hasProcesses)
                                {
                                    msgBox.AppendText($"Syntax error 40: Process model {processModelName} does not have any processes.\r\n");
                                }

                                if (!hasDataFlows)
                                {
                                    msgBox.AppendText($"Syntax error 40: Process model {processModelName} does not have any data flows.\r\n");
                                }

                                // Check optional control flows and data stores (not required)
                                bool hasControlFlows = controlFlows != null && controlFlows.Any();
                                bool hasDataStores = dataStores != null && dataStores.Any();

                                // Optionally report the presence of control flows and data stores
                                if (!hasControlFlows)
                                {
                                    msgBox.AppendText($"Note: Process model {processModelName} does not have any control flows.\r\n");
                                }

                                if (!hasDataStores)
                                {
                                    msgBox.AppendText($"Note: Process model {processModelName} does not have any data stores.\r\n");
                                }
                            }
                        }
                    }
                }
                msgBox.AppendText("Sukses 40  \r\n");

                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 40: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point41(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();

            try
            {
                // Gather all object names from the IM (Information Model)
                HashSet<string> objectNames = new HashSet<string>();
                foreach (var subsistem in jsonArray)
                {
                    if (subsistem?["type"]?.ToString() == "subsystem")
                    {
                        var model = subsistem["model"];
                        foreach (var item in model)
                        {
                            if (item?["type"]?.ToString() == "object")
                            {
                                var objectName = item["name"]?.ToString();
                                if (!string.IsNullOrEmpty(objectName))
                                {
                                    objectNames.Add(objectName);
                                    msgBox.AppendText(objectName + "\r\n");
                                }
                            }
                        }
                    }
                }

                // Check data store labels
                foreach (var subsistem in jsonArray)
                {
                    if (subsistem?["type"]?.ToString() == "subsystem")
                    {
                        var model = subsistem["model"];
                        foreach (var item in model)
                        {
                            if (item?["type"]?.ToString() == "processModel")
                            {
                                var dataStores = item["dataStores"];
                                var processModelName = item["name"]?.ToString();

                                if (dataStores != null)
                                {
                                    foreach (var dataStore in dataStores)
                                    {
                                        var dataStoreName = dataStore["name"]?.ToString();
                                        if (dataStoreName != "Timer" && dataStoreName != "Current Time" && !objectNames.Contains(dataStoreName))
                                        {
                                            msgBox.AppendText($"Syntax error 41: Data store {dataStoreName} in process model {processModelName} is not labeled 'Timer', 'Current Time', or with a valid object name from the IM.\r\n");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                msgBox.AppendText("Sukses 41 \r\n");

                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 41: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point42(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();

            try
            {
                // Check each subsystem
                foreach (var subsistem in jsonArray)
                {
                    if (subsistem?["type"]?.ToString() == "subsystem")
                    {                      
                        var model = subsistem["model"];

                        // Check each process model in the subsystem
                        foreach (var item in model)
                        {
                            if (item?["type"]?.ToString() == "processModel")
                            {
                                var dataStores = item["dataStores"];
                                var processModelName = item["name"]?.ToString();

                                if (dataStores != null)
                                {
                                    foreach (var dataStore in dataStores)
                                    {
                                        var dataStoreName = dataStore["name"]?.ToString();
                                        var dataStoreDescription = dataStore["description"]?.ToString();

                                        // Check if data store is labeled "Timer"
                                        if (dataStoreName == "Timer")
                                        {
                                            // Ensure it represents the time left on each timer in the system
                                            if (dataStoreDescription == null || !dataStoreDescription.Contains("time left on each timer"))
                                            {
                                                msgBox.AppendText($"Syntax error 42: Data store 'Timer' in process model {processModelName} does not represent the time left on each timer in the system.\r\n");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                msgBox.AppendText("Sukses 42 \r\n");

                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 42: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point43(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();

            try
            {
                // Check each subsystem
                foreach (var subsistem in jsonArray)
                {
                    if (subsistem?["type"]?.ToString() == "subsystem")
                    {
                        var model = subsistem["model"];

                        // Check each process model in the subsystem
                        foreach (var item in model)
                        {
                            if (item?["type"]?.ToString() == "processModel")
                            {
                                var dataStores = item["dataStores"];
                                var processModelName = item["name"]?.ToString();

                                if (dataStores != null)
                                {
                                    foreach (var dataStore in dataStores)
                                    {
                                        var dataStoreName = dataStore["name"]?.ToString();
                                        var dataStoreDescription = dataStore["description"]?.ToString();

                                        // Check if data store is labeled "Current Time"
                                        if (dataStoreName == "Current Time")
                                        {
                                            // Ensure it represents data describing current time
                                            if (dataStoreDescription == null || !dataStoreDescription.Contains("current time"))
                                            {
                                                msgBox.AppendText($"Syntax error 43: Data store 'Current Time' in process model {processModelName} does not represent data describing current time.\r\n");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                msgBox.AppendText("Sukses 43 \r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 43: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point44(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();

            try
            {
                foreach (var subsistem in jsonArray)
                {
                    if (subsistem?["type"]?.ToString() == "subsystem")
                    {
                        var model = subsistem["model"];

                        foreach (var item in model)
                        {
                            if (item?["type"]?.ToString() == "data_store")
                            {
                                var dataStoreName = item["name"]?.ToString();

                                // Check if data store name corresponds to any object in IM
                                bool isValidDataStore = false;

                                foreach (var subItem in model)
                                {
                                    if (subItem?["type"]?.ToString() == "class" && subItem["class_name"]?.ToString() == dataStoreName)
                                    {
                                        var attributes = subItem["attributes"];
                                        if (attributes != null && attributes.Any())
                                        {
                                            isValidDataStore = true;
                                            break;
                                        }
                                    }
                                }

                                if (!isValidDataStore)
                                {
                                    msgBox.AppendText($"Syntax error: Data store {dataStoreName} does not correspond to any object or lacks attributes in the IM. \r\n");
                                }
                            }
                        }
                    }
                }
                msgBox.AppendText("Sukses 44 \r\n");

                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point45(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                HashSet<string> dataStoreIds = new HashSet<string>();

                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var element in model)
                        {
                            if (element["type"]?.ToString() == "dataStore")
                            {
                                var dataStoreId = element["dataStoreId"]?.ToString();

                                if (dataStoreIds.Contains(dataStoreId))
                                {
                                    // If the data store is already encountered, it's appearing multiple times
                                    msgBox.AppendText($"Success 45: Data store '{dataStoreId}' appears in multiple places within the process model(s).\r\n");
                                    return true;
                                }
                                else
                                {
                                    // Add the data store ID to the HashSet
                                    dataStoreIds.Add(dataStoreId);
                                }
                            }
                        }
                    }
                }

                // If the loop completes without finding any duplicate data store, return false
                msgBox.AppendText($"Syntax error 45: No data store appears in multiple places within the process model(s).\r\n");
                return false;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 45: " + ex.Message + "\r\n");
                return false;
            }
        }


        public static bool Point46(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var element in model)
                        {
                            if (element["type"]?.ToString() == "process" && element["controlFlows"] is JArray controlFlows)
                            {
                                foreach (var controlFlow in controlFlows)
                                {
                                    var condition = controlFlow["condition"]?.ToString();
                                    var destination = controlFlow["destination"]?.ToString();

                                    // Ensure all conditional control flows have a condition label
                                    if (condition == null)
                                    {
                                        msgBox.AppendText($"Syntax error 46: The control flow to '{destination}' is missing a condition label.\r\n");
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }

                // If all checks pass, append success message
                msgBox.AppendText("Success 46: All conditional control flows are correctly labeled with the circumstances under which they are generated.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 46: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point47(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var element in model)
                        {
                            if (element["type"]?.ToString() == "process" && element["controlFlows"] is JArray controlFlows)
                            {
                                foreach (var controlFlow in controlFlows)
                                {
                                    var condition = controlFlow["condition"]?.ToString();
                                    var destination = controlFlow["destination"]?.ToString();

                                    // Ensure unconditional control flows are unlabelled
                                    if (condition == null && controlFlow["label"] != null)
                                    {
                                        msgBox.AppendText($"Syntax error 47: The unconditional control flow to '{destination}' should not have a label.\r\n");
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }

                // If all checks pass, append success message
                msgBox.AppendText("Success 47: All unconditional control flows are correctly unlabelled.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 47: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point48(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var element in model)
                        {
                            // Process the conditional and unconditional data flows
                            if (element["type"]?.ToString() == "process")
                            {
                                if (element["dataFlows"] is JArray dataFlows)
                                {
                                    foreach (var dataFlow in dataFlows)
                                    {
                                        var dataFlowType = dataFlow["type"]?.ToString();
                                        var dataFlowLabel = dataFlow["label"]?.ToString();

                                        // Ensure the data flow is labeled with data elements it carries
                                        if (string.IsNullOrEmpty(dataFlowLabel))
                                        {
                                            msgBox.AppendText($"Syntax error 48: A {dataFlowType} data flow in process '{element["name"]}' is not labeled with the names of the data elements it carries.\r\n");
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // If all checks pass, append success message
                msgBox.AppendText("Success 48: All data flows are correctly labeled with the names of the data elements they carry.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 48: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point49(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var element in model)
                        {
                            if (element["type"]?.ToString() == "process")
                            {
                                var processName = element["name"]?.ToString();
                                var dataFlows = element["dataFlows"] as JArray;

                                if (dataFlows != null)
                                {
                                    foreach (var dataFlow in dataFlows)
                                    {
                                        var target = dataFlow["target"]?.ToString();
                                        var label = dataFlow["label"]?.ToString();

                                        if (string.IsNullOrEmpty(target) || string.IsNullOrEmpty(label))
                                        {
                                            msgBox.AppendText($"Syntax error 49: Data flow in process '{processName}' must have a target and a label.\r\n");
                                            return false;
                                        }

                                        // Assuming the label should contain attributes read or written by the process
                                        bool validLabel = false;
                                        foreach (var elementModel in model)
                                        {
                                            if (elementModel["type"]?.ToString() == "class" &&
                                                elementModel["class_name"]?.ToString() == target)
                                            {
                                                var attributes = elementModel["attributes"] as JArray;
                                                if (attributes != null)
                                                {
                                                    foreach (var attribute in attributes)
                                                    {
                                                        var attributeName = attribute["attribute_name"]?.ToString();
                                                        if (label.Contains(attributeName))
                                                        {
                                                            validLabel = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                                break;
                                            }
                                        }

                                        if (!validLabel)
                                        {
                                            msgBox.AppendText($"Syntax error 49: Data flow from process '{processName}' to '{target}' has an invalid label '{label}'.\r\n");
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // If all checks pass, append success message
                msgBox.AppendText("Success 49: All data flows between processes and object data stores are correctly labeled.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 49: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point50(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var element in model)
                        {
                            if (element["type"]?.ToString() == "process")
                            {
                                var processName = element["name"]?.ToString();
                                var dataFlows = element["dataFlows"] as JArray;

                                if (dataFlows != null)
                                {
                                    foreach (var dataFlow in dataFlows)
                                    {
                                        var dataFlowName = dataFlow["name"]?.ToString();
                                        var attributes = dataFlow["attributes"] as JArray;

                                        // Validate each data flow
                                        bool isValidDataFlow = false;

                                        if (attributes != null)
                                        {
                                            foreach (var attribute in attributes)
                                            {
                                                var attributeName = attribute["name"]?.ToString();
                                                var attributeType = attribute["type"]?.ToString();

                                                if (attributeName != null && attributeType != null)
                                                {
                                                    // Check if the attribute represents object attributes or transient data
                                                    if (attributeType == "object_attribute" || attributeType == "transient_data")
                                                    {
                                                        isValidDataFlow = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }

                                        if (!isValidDataFlow)
                                        {
                                            msgBox.AppendText($"Syntax error 50: The data flow '{dataFlowName}' in process '{processName}' does not represent valid object attributes or transient data.\r\n");
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // If all checks pass, append success message
                msgBox.AppendText("Success 50: All data flows between processes represent valid object attributes or transient data.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 50: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point51(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var element in model)
                        {
                            if (element["type"]?.ToString() == "process")
                            {
                                var processName = element["name"]?.ToString();
                                var dataFlows = element["dataFlows"] as JArray;

                                if (dataFlows != null)
                                {
                                    foreach (var dataFlow in dataFlows)
                                    {
                                        var dataFlowName = dataFlow["name"]?.ToString();
                                        var attributes = dataFlow["attributes"] as JArray;

                                        if (attributes != null)
                                        {
                                            foreach (var attribute in attributes)
                                            {
                                                var attributeName = attribute["name"]?.ToString();
                                                var attributeType = attribute["type"]?.ToString();

                                                // Check if the attribute represents transient data
                                                if (attributeType == "transient_data")
                                                {
                                                    // Check if the data flow is labelled correctly
                                                    if (dataFlowName == null || !dataFlowName.Contains("(transient)"))
                                                    {
                                                        msgBox.AppendText($"Syntax error 51: The transient data flow '{dataFlowName}' in process '{processName}' is not correctly labelled with (transient).\r\n");
                                                        return false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // If all checks pass, append success message
                msgBox.AppendText("Success 51: All transient data flows between processes are correctly labelled with an appropriate variable name and (transient).\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 51: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point52(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var element in model)
                        {
                            if (element["type"]?.ToString() == "process")
                            {
                                var processName = element["name"]?.ToString();
                                var dataFlows = element["dataFlows"] as JArray;

                                if (dataFlows != null)
                                {
                                    foreach (var dataFlow in dataFlows)
                                    {
                                        var dataFlowName = dataFlow["name"]?.ToString();
                                        var attributes = dataFlow["attributes"] as JArray;

                                        if (attributes != null)
                                        {
                                            foreach (var attribute in attributes)
                                            {
                                                var attributeName = attribute["name"]?.ToString();
                                                var attributeType = attribute["type"]?.ToString();

                                                // Check if the attribute represents persistent data
                                                if (attributeType == "persistent_data")
                                                {
                                                    // Ensure the data flow is labelled correctly
                                                    if (dataFlowName == null)
                                                    {
                                                        msgBox.AppendText($"Syntax error 52: The persistent data flow in process '{processName}' is not labelled.\r\n");
                                                        return false;
                                                    }

                                                    var expectedLabel = $"{attribute["objectP"]}.{attribute["attribute1"]}={attribute["objectC"]}.{attribute["attribute2"]}";

                                                    if (dataFlowName != expectedLabel && dataFlowName != attributeName)
                                                    {
                                                        msgBox.AppendText($"Syntax error 52: The persistent data flow '{dataFlowName}' in process '{processName}' is not labelled correctly. Expected label: '{expectedLabel}' or '{attributeName}'.\r\n");
                                                        return false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // If all checks pass, append success message
                msgBox.AppendText("Success 52: All persistent data flows between processes are correctly labelled with the appropriate attribute names.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 52: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point53(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var element in model)
                        {
                            if (element["type"]?.ToString() == "process")
                            {
                                var processName = element["name"]?.ToString();
                                var transitions = element["transitions"] as JArray;

                                if (transitions != null)
                                {
                                    foreach (var transition in transitions)
                                    {
                                        var eventData = transition["eventData"] as JArray;

                                        if (eventData != null)
                                        {
                                            foreach (var data in eventData)
                                            {
                                                var dataFlowName = data["name"]?.ToString();
                                                var requiredAttributes = data["requiredAttributes"] as JArray;

                                                if (requiredAttributes != null)
                                                {
                                                    var requiredAttributesList = requiredAttributes.Select(attr => attr.ToString()).ToList();

                                                    // Ensure the data flow is labelled correctly
                                                    if (dataFlowName == null || !requiredAttributesList.All(attr => dataFlowName.Contains(attr)))
                                                    {
                                                        msgBox.AppendText($"Syntax error 53: The event data flow into process '{processName}' is not labelled correctly with the required attributes.\r\n");
                                                        return false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // If all checks pass, append success message
                msgBox.AppendText("Success 53: All event data flows into processes are correctly labelled with the required attributes.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 53: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point54(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var element in model)
                        {
                            if (element["type"]?.ToString() == "process")
                            {
                                var processName = element["name"]?.ToString();
                                var eventsGenerated = element["eventsGenerated"] as JArray;

                                if (eventsGenerated != null)
                                {
                                    foreach (var eventGenerated in eventsGenerated)
                                    {
                                        var eventName = eventGenerated["name"]?.ToString();
                                        var eventMeaning = eventGenerated["meaning"]?.ToString();
                                        var eventData = eventGenerated["eventData"]?.ToString();

                                        if (string.IsNullOrEmpty(eventName) || string.IsNullOrEmpty(eventMeaning) || string.IsNullOrEmpty(eventData))
                                        {
                                            msgBox.AppendText($"Syntax error 54: The event generated by process '{processName}' is not correctly labelled with the event's label, meaning, and event data.\r\n");
                                            return false;
                                        }
                                        else
                                        {
                                            // This represents the data flow directed away from the process
                                            msgBox.AppendText($"Event '{eventName}' generated by process '{processName}' with meaning '{eventMeaning}' and event data '{eventData}' is correctly labelled and directed away from the process.\r\n");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // If all checks pass, append success message
                msgBox.AppendText("Success 54: All events generated by processes are correctly labelled and represented as data flows directed away from the process.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 54: " + ex.Message + "\r\n");
                return false;
            }
        }

    }
}
