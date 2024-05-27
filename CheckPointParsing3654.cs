﻿using Newtonsoft.Json.Linq;
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

                return true;
            }
            catch (Exception ex)
            {
                msgBox.AppendText("Syntax error: " + ex.Message + "\r\n");
                return false;
            }
        }
    }
            
            return true;
        }

        public static bool Point45(Parsing form1, JArray jsonArray)
        {
            TextBox msgBox = form1.GetMessageBox();
            try
            {
                // Dictionary to track data stores and their occurrences
                Dictionary<string, List<string>> dataStoreOccurrences = new Dictionary<string, List<string>>();

                // Traverse through each item in the JSON array
                foreach (var item in jsonArray)
                {
                    if (item["type"]?.ToString() == "subsystem" && item["model"] is JArray model)
                    {
                        foreach (var element in model)
                        {
                            // Handle different types of model elements
                            string className = element["class_name"]?.ToString();
                            string classId = element["class_id"]?.ToString();

                            if (!string.IsNullOrEmpty(className) && !string.IsNullOrEmpty(classId))
                            {
                                // Record the occurrence of the data store
                                if (!dataStoreOccurrences.ContainsKey(classId))
                                {
                                    dataStoreOccurrences[classId] = new List<string>();
                                }
                                dataStoreOccurrences[classId].Add(item["sub_id"]?.ToString());
                            }
                        }
                    }
                }

                // Check for data stores appearing in multiple places
                foreach (var entry in dataStoreOccurrences)
                {
                    if (entry.Value.Distinct().Count() < 2)
                    {
                        msgBox.AppendText($"Syntax error 45: The data store with class_id '{entry.Key}' does not appear in multiple places.\r\n");
                        return false;
                    }
                }

                msgBox.AppendText("Success 45: All data stores appear in multiple places in the process model(s).\r\n");
                return true;
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


    }
}
