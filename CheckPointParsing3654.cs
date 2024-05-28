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



    }
}
