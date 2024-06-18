using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parsing
{
    public static class CheckPointParsing2233
    {
        public static bool Point22(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                foreach (var subsystem in jsonArray)
                {
                    HashSet<string> classNames = new HashSet<string>();
                    HashSet<string> classIds = new HashSet<string>();

                    foreach (var item in subsystem["model"])
                    {
                        var itemType = item["type"]?.ToString();

                        if ((itemType == "class" || itemType == "association_class") && item["class_name"] != null)
                        {
                            var className = item["class_name"]?.ToString();
                            var classId = item["class_id"]?.ToString();

                            if (string.IsNullOrWhiteSpace(className) || string.IsNullOrWhiteSpace(classId))
                            {
                                msgBox.AppendText("Syntax error 22: Class name or class_id is empty in the subsystem. \r\n");
                            }

                            if (classNames.Contains(className))
                            {
                                msgBox.AppendText($"Syntax error 22: Duplicate class name {className} within this subsystem. \r\n");
                            }

                            if (classIds.Contains(classId))
                            {
                                msgBox.AppendText($"Syntax error 22: Duplicate class_id {classId} within this subsystem. \r\n");
                            }

                            classNames.Add(className);
                            classIds.Add(classId);

                            // Check state events
                            if (item["states"] != null)
                            {
                                foreach (var state in item["states"])
                                {
                                    var stateEvents = state["state_event"]?.ToArray();
                                    if (stateEvents == null || stateEvents.Length == 0)
                                    {
                                        msgBox.AppendText($"Syntax error 22: State {state["state_name"]} does not generate any events.\r\n");
                                    }
                                }
                            }
                        }

                        if (itemType == "association" && item["model"] is JObject associationModel)
                        {
                            var associationItemType = associationModel["type"]?.ToString();

                            if (associationItemType == "association_class" && associationModel["class_name"] != null)
                            {
                                var associationClassName = associationModel["class_name"]?.ToString();
                                var associationClassId = associationModel["class_id"]?.ToString();

                                if (string.IsNullOrWhiteSpace(associationClassName) || string.IsNullOrWhiteSpace(associationClassId))
                                {
                                    msgBox.AppendText("Syntax error 22: Class name or class_id is empty in the subsystem. \r\n");
                                }

                                if (classNames.Contains(associationClassName))
                                {
                                    msgBox.AppendText($"Syntax error 22: Duplicate class name {associationClassName} within this subsystem. \r\n");
                                }

                                if (classIds.Contains(associationClassId))
                                {
                                    msgBox.AppendText($"Syntax error 22: Duplicate class_id {associationClassId} within this subsystem. \r\n");
                                }

                                classNames.Add(associationClassName);
                                classIds.Add(associationClassId);

                                // Check state events for association_class
                                if (associationModel["states"] != null)
                                {
                                    foreach (var state in associationModel["states"])
                                    {
                                        var stateEvents = state["state_event"]?.ToArray();
                                        if (stateEvents == null || stateEvents.Length == 0)
                                        {
                                            msgBox.AppendText($"Syntax error 22: State {state["state_name"]} does not generate any events.\r\n");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                msgBox.AppendText($"Success 22: All classes and association classes have unique names and IDs.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 22: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point31(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                HashSet<string> entitiesWithEvents = new HashSet<string>();

                // Loop through subsystems and model elements
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var element in model)
                        {
                            var elementType = element["type"]?.ToString();

                            // Check state models
                            if (elementType == "class" && element["states"] is JArray states)
                            {
                                var className = element["class_name"]?.ToString();
                                if (className != "TIMER" && states.Any(state => (state["state_event"] as JArray)?.Count > 0))
                                {
                                    entitiesWithEvents.Add(className);
                                }
                            }

                            // Check external entities
                            if (elementType == "external_entity" && element["events"] is JArray events)
                            {
                                var entityName = element["entity_name"]?.ToString();
                                if (entityName != "TIMER" && events.Count > 0)
                                {
                                    entitiesWithEvents.Add(entityName);
                                }
                            }
                        }
                    }
                }

                // Check if entities with events are present in Object Communication Model
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["OCM"] is JArray ocm)
                    {
                        foreach (var element in ocm)
                        {
                            var elementName = element["name"]?.ToString();
                            if (entitiesWithEvents.Contains(elementName))
                            {
                                entitiesWithEvents.Remove(elementName);
                            }
                        }
                    }
                }

                if (entitiesWithEvents.Count > 0)
                {
                    msgBox.AppendText("Syntax error 31: The following entities with events are missing from the Object Communication Model: " + string.Join(", ", entitiesWithEvents) + "\r\n");
                    return false;
                }

                msgBox.AppendText("Success 31: All entities with events are correctly listed in the Object Communication Model.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 31: " + ex.Message + "\r\n");
                return false;
            }
        }


        public static bool Point32(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                HashSet<string> eventsInvolved = new HashSet<string>();

                // Loop through subsystems and model elements
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var element in model)
                        {
                            var elementType = element["type"]?.ToString();

                            // Check external entities for events
                            if (elementType == "external_entity" && element["events"] is JArray events)
                            {
                                foreach (var evt in events)
                                {
                                    var eventName = evt["event_name"]?.ToString();
                                    eventsInvolved.Add(eventName);
                                }
                            }

                            // Check state models for events
                            if (elementType == "class" && element["states"] is JArray states)
                            {
                                foreach (var state in states)
                                {
                                    var stateEvents = state["state_event"] as JArray;
                                    if (stateEvents != null)
                                    {
                                        foreach (var eventName in stateEvents)
                                        {
                                            eventsInvolved.Add(eventName.ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Check if events involved are present in Object Communication Model (OCM)
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["OCM"] is JArray ocm)
                    {
                        foreach (var element in ocm)
                        {
                            var elementEvents = element["events"] as JArray;
                            if (elementEvents != null)
                            {
                                foreach (var evt in elementEvents)
                                {
                                    var eventName = evt.ToString();
                                    if (eventsInvolved.Contains(eventName))
                                    {
                                        eventsInvolved.Remove(eventName);
                                    }
                                }
                            }
                        }
                    }
                }

                if (eventsInvolved.Count > 0)
                {
                    msgBox.AppendText("Syntax error 32: The following events are missing from the Object Communication Model: " + string.Join(", ", eventsInvolved) + "\r\n");
                    return false;
                }

                msgBox.AppendText("Success 32: All involved events are correctly listed in the Object Communication Model.\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 32: " + ex.Message + "\r\n");
                return false;
            }
        }

        public static bool Point33(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                bool timerFound = false;
                HashSet<string> timerEvents = new HashSet<string>();

                // Identify TIMER state model and related events
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["model"] is JArray model)
                    {
                        foreach (var element in model)
                        {
                            var elementType = element["type"]?.ToString();

                            // Check for TIMER state model
                            if (elementType == "class" && element["class_name"]?.ToString() == "TIMER")
                            {
                                timerFound = true;

                                // Check for events associated with TIMER
                                var states = element["states"] as JArray;
                                if (states != null)
                                {
                                    foreach (var state in states)
                                    {
                                        var stateEvents = state["state_event"] as JArray;
                                        if (stateEvents != null)
                                        {
                                            foreach (var evt in stateEvents)
                                            {
                                                timerEvents.Add(evt.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Check if TIMER and its events are present in Object Communication Model (OCM)
                foreach (var subsystem in jsonArray)
                {
                    if (subsystem["OCM"] is JArray ocm)
                    {
                        foreach (var element in ocm)
                        {
                            var elementName = element["name"]?.ToString();
                            if (elementName == "TIMER" || timerEvents.Contains(elementName))
                            {
                                msgBox.AppendText("Syntax error 33: The TIMER state model or its events are shown on the Object Communication Model (OCM).\r\n");
                                return false;
                            }
                        }
                    }
                }

                if (!timerFound)
                {
                    msgBox.AppendText("Syntax error 33: The TIMER state model is not found in the model.\r\n");
                    return false;
                }

                msgBox.AppendText("Success 33: The TIMER state model and its events are not shown on the Object Communication Model (OCM).\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error 33: " + ex.Message + "\r\n");
                return false;
            }
        }

    }
}
