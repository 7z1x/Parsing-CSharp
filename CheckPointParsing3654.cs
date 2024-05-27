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
