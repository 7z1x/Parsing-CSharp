using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Parsing;

namespace Parsing
{
    public partial class Parsing : Form
    {
        private string[] fileNames;

        public Parsing()
        {
            InitializeComponent();
        }

        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            if (!string.IsNullOrEmpty(Properties.Settings.Default.LastFolderPath))
            {
                folderDialog.SelectedPath = Properties.Settings.Default.LastFolderPath;
            }

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                string folderPath = folderDialog.SelectedPath;
                txtPath.Text = folderPath;
                Properties.Settings.Default.LastFolderPath = folderPath;
                Properties.Settings.Default.Save();
                ProcessFilesInFolder(folderPath);
            }
        }

        private void ProcessFilesInFolder(string folderPath)
        {
            try
            {
                this.fileNames = Directory.GetFiles(folderPath, "*.json");

                if (this.fileNames.Length == 0)
                {
                    MessageBox.Show("No JSON files found in this folder.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                this.ProcessJson(this.fileNames);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private JArray ProcessJson(string[] fileNames)
        {
            List<string> jsonArrayList = new List<string>();

            foreach (var fileName in fileNames)
            {
                try
                {
                    string jsonContent = File.ReadAllText(fileName);
                    jsonArrayList.Add(jsonContent);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error reading the file {Path.GetFileName(fileName)}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }

            JArray jsonArray = new JArray(jsonArrayList.Select(JToken.Parse));

            textSourceCode.Text = jsonArray.ToString();
            return jsonArray;
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (fileNames == null || fileNames.Length == 0)
            {
                MessageBox.Show("Please select a folder containing JSON files first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            JArray jsonArray = this.ProcessJson(fileNames);

            msgBox.Clear();

            CheckParsing15.Point1(this, jsonArray);
            CheckParsing15.Point2(this, jsonArray);
            CheckParsing15.Point3(this, jsonArray);
            CheckParsing15.Point4(this, jsonArray);
            CheckParsing15.Point5(this, jsonArray);
            CheckParsing610.Point6(this, jsonArray);
            CheckParsing610.Point7(this, jsonArray);
            CheckParsing610.Point8(this, jsonArray);
            CheckParsing610.Point9(this, jsonArray);
            CheckParsing610.Point10(this, jsonArray);
            CheckParsing1115.Point11(this, jsonArray);
            CheckParsing1115.Point13(this, jsonArray);
            CheckParsing1115.Point14(this, jsonArray);
            CheckParsing1115.Point15(this, jsonArray);

            btnCheck_Click1(sender, e);

            ParsingPoint.Point25(this, jsonArray);
            ParsingPoint.Point27(this, jsonArray);
            ParsingPoint.Point28(this, jsonArray);
            ParsingPoint.Point29(this, jsonArray);
            ParsingPoint.Point30(this, jsonArray);
            ParsingPoint.Point34(this, jsonArray);
            ParsingPoint.Point35(this, jsonArray);

            CheckParsing1115.Point99(this, jsonArray);

            CheckPointParsing3654.Point36(this, jsonArray);
            CheckPointParsing3654.Point37(this, jsonArray);
            CheckPointParsing3654.Point38(this, jsonArray);
            CheckPointParsing3654.Point39(this, jsonArray);
            CheckPointParsing3654.Point40(this, jsonArray);
            CheckPointParsing3654.Point41(this, jsonArray);
            CheckPointParsing3654.Point42(this, jsonArray);
            CheckPointParsing3654.Point43(this, jsonArray);
            CheckPointParsing3654.Point44(this, jsonArray);

            CheckPointParsing3654.Point45(this, jsonArray);
            CheckPointParsing3654.Point46(this, jsonArray);
            CheckPointParsing3654.Point47(this, jsonArray);
            CheckPointParsing3654.Point48(this, jsonArray);
            CheckPointParsing3654.Point49(this, jsonArray);
            CheckPointParsing3654.Point50(this, jsonArray);
            CheckPointParsing3654.Point51(this, jsonArray);
            CheckPointParsing3654.Point52(this, jsonArray);
            CheckPointParsing3654.Point53(this, jsonArray);
            CheckPointParsing3654.Point54(this, jsonArray);

            CheckParsing5572.Point55(this, jsonArray);
            CheckParsing5572.Point56(this, jsonArray);
            CheckParsing5572.Point57(this, jsonArray);
            CheckParsing5572.Point58(this, jsonArray);
            CheckParsing5572.Point59(this, jsonArray);
            CheckParsing5572.Point60(this, jsonArray);
            CheckParsing5572.Point61(this, jsonArray);
            CheckParsing5572.Point62(this, jsonArray);
            CheckParsing5572.Point63(this, jsonArray);
            CheckParsing5572.Point64(this, jsonArray);
            CheckParsing5572.Point65(this, jsonArray);
            CheckParsing5572.Point66(this, jsonArray);
            CheckParsing5572.Point67(this, jsonArray);
            CheckParsing5572.Point68(this, jsonArray);
            CheckParsing5572.Point69(this, jsonArray);
            CheckParsing5572.Point70(this, jsonArray);
            CheckParsing5572.Point71(this, jsonArray);
            CheckParsing5572.Point72(this, jsonArray);


            if (string.IsNullOrWhiteSpace(msgBox.Text))
            {
                msgBox.Text = "Model has successfully passed parsing";
            }
        }
        public TextBox GetMessageBox()
        {
            return msgBox;
        }

        private void btnCheck_Click1(object sender, EventArgs e)
        {  try
            {
                if (fileNames == null || fileNames.Length == 0)
                {
                    MessageBox.Show("Please select a folder containing JSON files first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return;
                }

                foreach (var fileName in this.fileNames)
                {
                    string jsonContent = File.ReadAllText(fileName);
                    CheckJsonCompliance(jsonContent);
                }


            }
            catch (Exception ex)
            {
                HandleError($"Error: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            ShowHelpMessageBox();
        }

        private void ShowHelpMessageBox()
        {
            string helpMessage = GenerateHelpMessage();
            MessageBox.Show(helpMessage, "User Guide", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string GenerateHelpMessage()
        {
            StringBuilder helpMessage = new StringBuilder();

            helpMessage.AppendLine("User Guide for Parser:");
            helpMessage.AppendLine();
            helpMessage.AppendLine("1. Find Folder Path:");
            helpMessage.AppendLine("   - Click the 'Browse' button to select the folder with JSON file inside you want to check.");
            helpMessage.AppendLine("   - After selecting the folder, the folder path will be displayed in the path box.");
            helpMessage.AppendLine("2. Initiate Checking:");
            helpMessage.AppendLine("   - After choosing the folder, click the 'Check' button to start the checking process.");
            helpMessage.AppendLine("   - The JSON source code and the checking result will be displayed in the output text box.");
            helpMessage.AppendLine("3. Interpret Checking Results:");
            helpMessage.AppendLine("   - Review the checking results in the output text box.");
            helpMessage.AppendLine("   - If there are errors, error messages will be provided to guide the corrections.");

            return helpMessage.ToString();
        }

        private void HandleError(string errorMessage)
        {
            msgBox.Text += $"{errorMessage}{Environment.NewLine}";
            Console.WriteLine(errorMessage);
        }

        private void CheckJsonCompliance(string jsonContent)
        {
            try
            {
                JObject jsonObj = JObject.Parse(jsonContent);

                // Dictionary to store state model information
                Dictionary<string, string> stateModels = new Dictionary<string, string>();
                HashSet<string> usedKeyLetters = new HashSet<string>();
                HashSet<int> stateNumbers = new HashSet<int>();

                JToken subsystemsToken = jsonObj["subsystems"];
                if (subsystemsToken != null && subsystemsToken.Type == JTokenType.Array)
                {
                    // Iterasi untuk setiap subsystem dalam subsystemsToken
                    foreach (var subsystem in subsystemsToken)
                    {
                        JToken modelToken = subsystem["model"];
                        if (modelToken != null && modelToken.Type == JTokenType.Array)
                        {
                            foreach (var model in modelToken)
                            {
                                ValidateClassModel(model, stateModels, usedKeyLetters, stateNumbers);
                            }
                        }
                    }

                    // Setelah memvalidasi semua model, panggil ValidateEventDirectedToStateModelHelper untuk setiap subsystem
                    foreach (var subsystem in subsystemsToken)
                    {
                        ValidateEventDirectedToStateModelHelper(subsystem["model"], stateModels, null);
                    }
                }

                ValidateTimerModel(jsonObj, usedKeyLetters);
            }
            catch (Exception ex)
            {
                HandleError($"Error: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ValidateClassModel(JToken model, Dictionary<string, string> stateModels, HashSet<string> usedKeyLetters, HashSet<int> stateNumbers)
        {
            string objectType = model["type"]?.ToString();
            string objectName = model["class_name"]?.ToString();
            Console.WriteLine($"Running CheckKeyLetterUniqueness for {objectName}");

            if (objectType == "class")
            {
                Console.WriteLine($"Checking class: {objectName}");

                string assignerStateModelName = $"{objectName}_ASSIGNER";
                JToken assignerStateModelToken = model[assignerStateModelName];

                if (assignerStateModelToken == null || assignerStateModelToken.Type != JTokenType.Object)
                {
                    HandleError($"Syntax error 16: Assigner state model not found for {objectName}.");
                    return;
                }

                string keyLetter = model["KL"]?.ToString();

                // Pemanggilan CheckKeyLetterUniqueness
                CheckKeyLetterUniqueness(usedKeyLetters, keyLetter, objectName);

                // Check if KeyLetter is correct
                JToken keyLetterToken = assignerStateModelToken?["KeyLetter"];
                if (keyLetterToken != null && keyLetterToken.ToString() != keyLetter)
                {
                    HandleError($"Syntax error 17: KeyLetter for {objectName} does not match the rules.");
                }

                // Check uniqueness of states
                CheckStateUniqueness(stateModels, assignerStateModelToken?["states"], objectName, assignerStateModelName);

                // Check uniqueness of state numbers
                CheckStateNumberUniqueness(stateNumbers, assignerStateModelToken?["states"], objectName);

                // Store state model information
                string stateModelKey = $"{objectName}.{assignerStateModelName}";
                stateModels[stateModelKey] = objectName;
            }
        }

        private void CheckStateUniqueness(Dictionary<string, string> stateModels, JToken statesToken, string objectName, string assignerStateModelName)
        {
            if (statesToken is JArray states)
            {
                HashSet<string> uniqueStates = new HashSet<string>();

                foreach (var state in states)
                {
                    string stateName = state["state_name"]?.ToString();
                    string stateModelName = $"{objectName}.{stateName}";

                    // Check uniqueness of state model
                    if (!uniqueStates.Add(stateModelName))
                    {
                        HandleError($"Syntax error 18: State {stateModelName} is not unique in {assignerStateModelName}.");
                    }
                }
            }
        }

        private void CheckStateNumberUniqueness(HashSet<int> stateNumbers, JToken statesToken, string objectName)
        {
            if (statesToken is JArray stateArray)
            {
                foreach (var state in stateArray)
                {
                    int stateNumber = state["state_number"]?.ToObject<int>() ?? 0;

                    if (!stateNumbers.Add(stateNumber))
                    {
                        HandleError($"Syntax error 19: State number {stateNumber} is not unique.");
                    }
                }
            }
        }

        private void CheckKeyLetterUniqueness(HashSet<string> usedKeyLetters, string keyLetter, string objectName)
        {
            string expectedKeyLetter = $"{keyLetter}_A";
            Console.WriteLine("Running ValidateClassModel");
            Console.WriteLine($"Checking KeyLetter uniqueness: {expectedKeyLetter} for {objectName}");

            if (!usedKeyLetters.Add(expectedKeyLetter))
            {
                HandleError($"Syntax error 20: KeyLetter for {objectName} is not unique.");
            }
        }

        private void ValidateTimerModel(JObject jsonObj, HashSet<string> usedKeyLetters)
        {
            string timerKeyLetter = jsonObj["subsystems"]?[0]?["model"]?[0]?["KL"]?.ToString();
            string timerStateModelName = $"{timerKeyLetter}_ASSIGNER";

            JToken timerModelToken = jsonObj["subsystems"]?[0]?["model"]?[0];
            JToken timerStateModelToken = jsonObj["subsystems"]?[0]?["model"]?[0]?[timerStateModelName];

            // Check if Timer state model exists
            if (timerStateModelToken == null || timerStateModelToken.Type != JTokenType.Object)
            {
                HandleError($"Syntax error 21: Timer state model not found for TIMER.");
                return;
            }

            // Check KeyLetter of Timer state model
            JToken keyLetterToken = timerStateModelToken?["KeyLetter"];
            if (keyLetterToken == null || keyLetterToken.ToString() != timerKeyLetter)
            {
                HandleError($"Syntax error 21: KeyLetter for TIMER does not match the rules.");
            }
        }

        private void ValidateEventDirectedToStateModelHelper(JToken modelsToken, Dictionary<string, string> stateModels, string modelName)
        {
            if (modelsToken != null && modelsToken.Type == JTokenType.Array)
            {
                foreach (var model in modelsToken)
                {
                    string modelType = model["type"]?.ToString();
                    string className = model["class_name"]?.ToString();

                    if (modelType == "class")
                    {
                        JToken assignerToken = model[$"{className}_ASSIGNER"];

                        if (assignerToken != null)
                        {
                            Console.WriteLine($"assignerToken.Type: {assignerToken.Type}");

                            if (assignerToken.Type == JTokenType.Object)
                            {
                                JToken statesToken = assignerToken["states"];

                                if (statesToken != null && statesToken.Type == JTokenType.Array)
                                {
                                    JArray statesArray = (JArray)statesToken;

                                    foreach (var stateItem in statesArray)
                                    {
                                        string stateName = stateItem["state_name"]?.ToString();
                                        string stateModelName = $"{modelName}.{stateName}";

                                        JToken eventsToken = stateItem["events"];
                                        if (eventsToken is JArray events)
                                        {
                                            foreach (var evt in events)
                                            {
                                                string eventName = evt["event_name"]?.ToString();
                                                JToken targetsToken = evt["targets"];

                                                if (targetsToken is JArray targets)
                                                {
                                                    foreach (var target in targets)
                                                    {
                                                        string targetStateModel = target?.ToString();

                                                        // Check if target state model is in the state models dictionary
                                                        if (!stateModels.ContainsKey(targetStateModel))
                                                        {
                                                            HandleError($"Syntax error 24: Event '{eventName}' in state '{stateModelName}' targets non-existent state model '{targetStateModel}'.");
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
        }

    }
}
