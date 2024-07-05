using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace Parsing
{
    public static class ParsingPoint
    {
        public static bool Point25(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                // Menggunakan HashSet untuk menyimpan event labels
                HashSet<string> eventLabels = new HashSet<string>();

                foreach (var subsystem in jsonArray)
                {
                    // Memeriksa apakah subsystem memiliki events
                    if (subsystem["events"] != null)
                    {
                        foreach (var eventItem in subsystem["events"])
                        {
                            var eventName = eventItem["event_name"]?.ToString();
                            var stateModelName = eventItem["state_model_name"]?.ToString();
                            var eventData = eventItem["event_data"] as JObject;
                            var eventLabel = eventItem["event_label"]?.ToString();

                            // Memeriksa apakah komponen-komponen event ada dan tidak kosong
                            if (string.IsNullOrWhiteSpace(eventName))
                            {
                                msgBox.AppendText("Syntax error: Event name is empty. \r\n");
                            }

                            if (string.IsNullOrWhiteSpace(stateModelName))
                            {
                                msgBox.AppendText("Syntax error: State model name is empty. \r\n");
                            }

                            if (eventData == null || !eventData.HasValues)
                            {
                                msgBox.AppendText("Syntax error: Event data is empty. \r\n");
                            }

                            if (string.IsNullOrWhiteSpace(eventLabel))
                            {
                                msgBox.AppendText("Syntax error: Event label is empty. \r\n");
                            }

                            // Memeriksa duplikasi event label
                            if (eventLabels.Contains(eventLabel))
                            {
                                msgBox.AppendText($"Syntax error: Duplicate event label {eventLabel}. \r\n");
                            }
                            else
                            {
                                eventLabels.Add(eventLabel);
                            }
                        }
                    }
                }
                msgBox.AppendText($"Success 25: All events have unique labels.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error: " + ex.Message + "\r\n");
                return false;
            }
        }


        public static bool Point27(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();

            try
            {
                HashSet<string> strings = new HashSet<string>();
                Func<JToken, bool> processItem = null;
                processItem = (item) =>
                {
                    var itemType = item["type"]?.ToString();
                    if (itemType == "class")
                    {
                        var className = item["class_name"]?.ToString();
                        if (className == "mahasiswa" || className == "dosen")
                        {
                            var stateArray = item["states"] as JArray;
                            if (stateArray != null)
                            {
                                foreach (var state in stateArray)
                                {
                                    var stateName = state["state_model_name"]?.ToString();
                                    var stateEvent = state["state_event"]?.ToString();
                                    if (stateEvent != null)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        msgBox.AppendText($"Syntax error 27: event for {stateName} state is not implemented. \r\n");
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                msgBox.AppendText($"Syntax error 27: states label for class {className} is not implemented. \r\n");
                                return false;
                            }
                        }
                    }
                    return true;
                };
                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        if (!processItem(item))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 27: " + ex.Message + ".\r\n");
                return false;
            }
        }

        public static bool Point28(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                Func<JToken, bool> processItem = null;
                processItem = (item) =>
                {
                    var itemType = item["type"]?.ToString();
                    if (itemType == "class")
                    {
                        var className = item["class_name"]?.ToString();
                        var classId = item["class_id"]?.ToString();
                        var classKL = item["KL"]?.ToString();
                        if (className != null && classId != null && classKL != null)
                        {
                            return true;
                        }
                    }
                    return true;
                };
                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        if (!processItem(item))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 28: " + ex.Message + ".\r\n");
                return false;
            }
        }


        public static bool Point29(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    Dictionary<string, Dictionary<string, HashSet<string>>> stateEventsData = new Dictionary<string, Dictionary<string, HashSet<string>>>();

                    foreach (var item in subsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();

                        if (itemType == "class" || itemType == "association_class")
                        {
                            foreach (var state in item["states"])
                            {
                                var stateName = state["state_model_name"]?.ToString();

                                if (!stateEventsData.ContainsKey(stateName))
                                {
                                    stateEventsData[stateName] = new Dictionary<string, HashSet<string>>();
                                }

                                foreach (var eventName in state["state_event"])
                                {
                                    var eventString = eventName?.ToString();
                                    var eventData = string.Join(",", state["state_event"]); // Simulasikan data event

                                    if (!stateEventsData[stateName].ContainsKey(eventString))
                                    {
                                        stateEventsData[stateName][eventString] = new HashSet<string>();
                                    }

                                    if (stateEventsData[stateName][eventString].Count == 0)
                                    {
                                        stateEventsData[stateName][eventString].Add(eventData);
                                    }
                                    else
                                    {
                                        if (!stateEventsData[stateName][eventString].Contains(eventData))
                                        {
                                            msgBox.AppendText($"Syntax error: Event data inconsistency in state {stateName} for event {eventString}. \r\n");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                msgBox.AppendText($"Success 29: There are no inconsistencies in event data within each state.\r\n");

                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Error: " + ex.Message + "\r\n");
                return false;
            }
        }



        public static bool Point30(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();

                        // Check for TIM1 and TIM2 within association_class or class types that involve timers
                        if (itemType == "class" || itemType == "association_class")
                        {
                            if (item["attributes"] != null)
                            {
                                foreach (var attribute in item["attributes"])
                                {
                                    if (attribute["attribute_name"]?.ToString() == "timer_id")
                                    {
                                        var timerId = attribute["attribute_name"]?.ToString();
                                        if (string.IsNullOrWhiteSpace(timerId))
                                        {
                                            msgBox.AppendText("Syntax error: timer_id is empty. \r\n");
                                        }
                                    }

                                    if (attribute["attribute_name"]?.ToString() == "ELx")
                                    {
                                        var elx = attribute["attribute_name"]?.ToString();
                                        if (string.IsNullOrWhiteSpace(elx))
                                        {
                                            msgBox.AppendText("Syntax error: ELx is empty. \r\n");
                                        }
                                    }

                                    if (attribute["attribute_name"]?.ToString() == "instance_id")
                                    {
                                        var instanceId = attribute["attribute_name"]?.ToString();
                                        if (string.IsNullOrWhiteSpace(instanceId))
                                        {
                                            msgBox.AppendText("Syntax error: instance_id is empty. \r\n");
                                        }
                                    }

                                    if (attribute["attribute_name"]?.ToString() == "time_interval")
                                    {
                                        var timeInterval = attribute["attribute_name"]?.ToString();
                                        if (string.IsNullOrWhiteSpace(timeInterval))
                                        {
                                            msgBox.AppendText("Syntax error: time_interval is empty. \r\n");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                msgBox.AppendText("Success 30: Every state in classes has been included in the timer setting.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Error validating timers: " + ex.Message + "\r\n");
                return false;
            }
        }




        public static bool Point34(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                Func<JToken, bool> processItem = null;
                processItem = (item) =>
                {
                    var itemType = item["type"]?.ToString();
                    if (itemType == "class")
                    {
                        var className = item["class_name"]?.ToString();
                        var states = item["states"] as JArray;
                        if (states != null)
                        {
                            foreach (var sub in states)
                            {
                                var stateName = sub["state_model_name"]?.ToString();
                                var stateEvent = sub["state_event"]?.ToString();
                                if (stateEvent != null)
                                {
                                    return true;
                                }
                                else
                                {
                                    msgBox.AppendText($"Syntax error 34: there is not action for {className} class itself.\r\n");
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            msgBox.AppendText($"Syntax error 34: states for {className} class is null.\r\n");
                            return false;
                        }
                    }
                    return true;
                };
                foreach (var subsystem in jsonArray)
                {
                    foreach (var item in subsystem["model"])
                    {
                        if (item.Contains("states"))
                        {
                            if (!processItem(item))
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText($"Syntax error 34: " + ex.Message + ".\r\n");
                return false;
            }
        }

        public static bool Point35(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                // Loop through subsystems and model elements
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var element in model)
                        {
                            var elementType = element["type"]?.ToString();

                            // Check for classes and their attributes
                            if (elementType == "class" && element["attributes"] is JArray attributes)
                            {
                                var className = element["class_name"]?.ToString();
                                var classInstances = new Dictionary<string, Dictionary<string, string>>();

                                // Collect initial data for each instance of the class
                                if (element["instances"] is JArray instances)
                                {
                                    foreach (var instance in instances)
                                    {
                                        var instanceId = instance["id"]?.ToString();
                                        if (instanceId != null)
                                        {
                                            foreach (var attribute in attributes)
                                            {
                                                var attributeName = attribute["attribute_name"]?.ToString();
                                                if (attributeName != null)
                                                {
                                                    var attributeValue = instance[attributeName]?.ToString();
                                                    if (attributeValue != null)
                                                    {
                                                        if (!classInstances.ContainsKey(instanceId))
                                                        {
                                                            classInstances[instanceId] = new Dictionary<string, string>();
                                                        }
                                                        classInstances[instanceId][attributeName] = attributeValue;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                // Check actions that modify attributes
                                if (element["actions"] is JArray actions)
                                {
                                    foreach (var action in actions)
                                    {
                                        var actionType = action["type"]?.ToString();
                                        if (actionType == "modify" && action["target"]?.ToString() == "descriptive_attribute")
                                        {
                                            var targetAttribute = action["attribute_name"]?.ToString();
                                            var newValue = action["new_value"]?.ToString();
                                            var instanceId = action["instance_id"]?.ToString();

                                            if (targetAttribute != null && instanceId != null && newValue != null)
                                            {
                                                if (classInstances.ContainsKey(instanceId))
                                                {
                                                    classInstances[instanceId][targetAttribute] = newValue;

                                                    // Check for consistency (example: no empty values)
                                                    if (classInstances[instanceId] != null)
                                                    {
                                                        foreach (var attributeValue in classInstances[instanceId].Values)
                                                        {
                                                            if (string.IsNullOrEmpty(attributeValue))
                                                            {
                                                                msgBox.AppendText($"Syntax error 35: Inconsistent data found for instance {instanceId} of class {className} after modifying attribute {targetAttribute}.\r\n");
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
                    }
                }

                msgBox.AppendText("Success 35: All data modifications ensure self-consistency for instances.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 35: " + ex.Message + "\r\n");
                return false;
            }
        }

    }
}