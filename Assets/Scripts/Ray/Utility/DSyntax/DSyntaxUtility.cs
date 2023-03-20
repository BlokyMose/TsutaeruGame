using Encore.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DialogueSyntax.DSyntaxData;

namespace DialogueSyntax
{
    public static class DSyntaxUtility
    {
        #region [Writer/Reader]

        #region [Classes]

        public class Command
        {
            public string name;
            public List<string> parameters;
            public string rawText;

            public Command(string name, List<string> parameters, string rawText)
            {
                this.name = name;
                this.parameters = parameters;
                this.rawText = rawText;
            }

            /// <summary>
            /// Returns a parameter using its parameterName; if failed, get it by index; if failed again, returns empty string
            /// </summary>
            /// <param name="parameterName">The name of the parameter which is located behind the separator token</param>
            /// <param name="index">Parameter's default location (index) inside the parameters list</param>
            public string GetParameter(DSyntaxSettings settings, string parameterName, int index, string defaultValue = "")
            {
                return GetParameter(settings.TOKEN_PARAMETER_NAME, parameters, parameterName, index, defaultValue);
            }

            /// <summary>
            /// Returns a parameter using its index; if failed, returns empty string
            /// </summary>
            /// <param name="index">Parameter's location (index) inside the parameters list</param>
            public string GetParameter(DSyntaxSettings settings, int index, string defaultValue = "")
            {
                return GetParameter(settings.TOKEN_PARAMETER_NAME, parameters, "", index, defaultValue);
            }

            public static string GetParameter(string parameterNameToken, List<string> parameters, string parameterName, int index, string defaultValue = "")
            {
                // Find parameter with parameterName
                if (!string.IsNullOrEmpty(parameterName))
                    foreach (var parameter in parameters)
                    {
                        var nameAndValue = parameter.SplitHalf(parameterNameToken);
                        var _parameterName = nameAndValue.Item1.Trim();
                        var _parameterValue = nameAndValue.Item2.Trim();
                        if (_parameterName.Equals(parameterName, System.StringComparison.CurrentCultureIgnoreCase))
                            return _parameterValue;
                    }

                // Find parameter with index
                var parameterByIndex = parameters.GetAt(index, defaultValue).Trim();

                // This parameter has a name which differs with the targeted parameterName
                if (!string.IsNullOrEmpty(parameterByIndex) && !string.IsNullOrEmpty(parameterByIndex.SplitHalf(parameterNameToken).Item1))
                    return defaultValue;

                // This parameter has no name, thus it's safe to assume this is the desired parameter
                else
                    return parameterByIndex;
            }
        }

        /// <summary>
        /// A class to group parameters based on unwrapped parameter
        /// </summary>
        public class ListCommand : Command
        {
            public List<List<string>> childParameters;

            public ListCommand(Command command, List<List<string>> childParameters) : base(command.name, command.parameters, command.rawText)
            {
                this.childParameters = childParameters;
            }

            /// <summary>Try to get a parameter of a childParameter if exists, else return defaultValue</summary>
            public string GetChildParameter(int indexList, int index, string defaultValue = "")
            {
                if (childParameters.Count > indexList)
                    return childParameters[indexList].GetAt(index, defaultValue);
                else
                    return defaultValue;
            }

            /// <summary>Try to get a parameter of a childParameter if exists, else return defaultValue</summary>
            public string GetChildParameter(DSyntaxSettings settings, int indexList, string parameterName, int index, string defaultValue = "")
            {
                if (childParameters.Count > indexList)
                {
                    var result = GetParameter(settings.TOKEN_PARAMETER_NAME, childParameters[indexList], parameterName, index, defaultValue);
                    return result;
                }
                else
                    return defaultValue;
            }


        }

        public class Parameter
        {
            public string name;
            public string value;

            public Parameter(string name, string value)
            {
                this.name = name;
                this.value = value;
            }
        }

        #endregion

        #region [Methods: Write]

        /// <summary>Wrap command name using command opening and closing token, and wrap parameter text using parameter opening and closing token</summary>
        public static string WriteCommand(DSyntaxSettings settings, string commandName, string parameter, bool wrapParameter = false)
        {
            if (wrapParameter)
                return settings.TOKEN_COMMAND_OPENING + commandName + settings.TOKEN_COMMAND_CLOSING + " " + WriteParameter(settings, parameter) + "\n";
            else
                return settings.TOKEN_COMMAND_OPENING + commandName + settings.TOKEN_COMMAND_CLOSING + " " + parameter + "\n";
        }

        /// <summary>Wrap command name using command opening and closing token, and wrap parameter text using parameter opening and closing token</summary>
        public static string WriteCommand(DSyntaxSettings settings, string commandName, Parameter parameter, bool wrapParameter = false)
        {
            if (wrapParameter)
                return settings.TOKEN_COMMAND_OPENING + commandName + settings.TOKEN_COMMAND_CLOSING + " " + WriteParameter(settings, parameter) + "\n";
            else
                return settings.TOKEN_COMMAND_OPENING + commandName + settings.TOKEN_COMMAND_CLOSING + " " + parameter.value + "\n";
        }

        /// <summary>Wrap command name using command opening and closing token, and wrap parameter text using parameter opening and closing token</summary>
        public static string WriteCommand(DSyntaxSettings settings, string commandName, List<string> parameters, bool wrapFirstParameter = false)
        {
            string result = settings.TOKEN_COMMAND_OPENING + commandName + settings.TOKEN_COMMAND_CLOSING + " ";
            if (!wrapFirstParameter)
            {
                result += parameters[0] + "\n";
                parameters.RemoveAt(0);
            }

            foreach (var parameter in parameters) result += WriteParameter(settings, parameter) + "\n";

            return result;
        }

        /// <summary>Wrap command name using command opening and closing token, and wrap parameter text using parameter opening and closing token</summary>
        public static string WriteCommand(DSyntaxSettings settings, string commandName, List<Parameter> parameters)
        {
            string result = settings.TOKEN_COMMAND_OPENING + commandName + settings.TOKEN_COMMAND_CLOSING + " ";

            foreach (var parameter in parameters) result += WriteParameter(settings, parameter) + "\n";

            return result;
        }

        /// <summary>Wrap command name using command opening and closing token, and wrap parameter text using parameter opening and closing token</summary>
        /// <param name="wrapParentFirstParameter">First parent's parameter can have no wrapping tokens if there are more than one parent's parameter</param>
        public static string WriteListCommand(DSyntaxSettings settings, string commandName, List<string> parentParameters, List<List<string>> childParameters, bool wrapParentFirstParameter = false)
        {
            string result = settings.TOKEN_COMMAND_OPENING + commandName + settings.TOKEN_COMMAND_CLOSING + " ";

            if (parentParameters != null)
            {
                if (parentParameters.Count > 1 && !wrapParentFirstParameter)
                {
                    result += parentParameters[0];
                    parentParameters.RemoveAt(0);
                }

                foreach (var parameter in parentParameters) result += WriteParameter(settings, parameter);
            }

            result += "\n";

            foreach (var parametersList in childParameters)
            {
                result += parametersList[0];
                parametersList.RemoveAt(0);
                foreach (var parameter in parametersList) result += WriteParameter(settings, parameter);
                result += "\n";
            }

            return result;
        }

        /// <summary>Wrap command name using command opening and closing token, and wrap parameter text using parameter opening and closing token</summary>
        /// <param name="wrapParentFirstParameter">First parent's parameter can have no wrapping tokens if there are more than one parent's parameter</param>
        public static string WriteListCommand(DSyntaxSettings settings, string commandName, List<Parameter> parentParameters, List<List<Parameter>> childParameters)
        {
            string result = settings.TOKEN_COMMAND_OPENING + commandName + settings.TOKEN_COMMAND_CLOSING + " ";

            if (parentParameters != null)
            {
                foreach (var parameter in parentParameters) result += WriteParameter(settings, parameter);
            }

            result += "\n";

            foreach (var parametersList in childParameters)
            {
                result += parametersList[0].value;
                parametersList.RemoveAt(0);
                foreach (var parameter in parametersList) result += WriteParameter(settings, parameter);
                result += "\n";
            }

            return result;
        }

        /// <summary>Wrap parameter text using Parameter opening and closing token</summary>
        public static string WriteParameter(DSyntaxSettings settings, string parameter)
        {
            return settings.TOKEN_PARAMETER_OPENING + parameter + settings.TOKEN_PARAMETER_CLOSING;
        }        
        
        /// <summary>Wrap parameter's name and value using Parameter opening and closing token</summary>
        public static string WriteParameter(DSyntaxSettings settings, Parameter parameter)
        {
            if (!string.IsNullOrEmpty(parameter.name))
                return settings.TOKEN_PARAMETER_OPENING + parameter.name + settings.TOKEN_PARAMETER_NAME + " " + parameter.value + settings.TOKEN_PARAMETER_CLOSING;
            else
                return WriteParameter(settings, parameter.value);
        }

        /// <summary>Wrap each parameter texts using Parameter opening and closing token, but doesn't wrap empty parameters</summary>
        public static string WriteParameters(DSyntaxSettings settings, List<string> parameters)
        {
            string result = "";

            for (int i = parameters.Count - 1; i >= 0; i--)
                if (string.IsNullOrEmpty(parameters[i]))
                    result += WriteParameter(settings, parameters[i]);

            return result;
        }

        /// <summary>Wrap each parameter name and value using Parameter opening and closing token, but doesn't wrap empty parameters</summary>
        public static string WriteParameters(DSyntaxSettings settings, List<Parameter> parameters)
        {
            string result = "";

            for (int i = parameters.Count - 1; i >= 0; i--)
                if (parameters[i]!=null)
                    result += WriteParameter(settings, parameters[i]);

            return result;
        }

        #endregion

        #region [Methods: Read]

        /// <summary>Extract a list of parameters by using parameter tokens</summary>
        public static List<string> ReadParameters(DSyntaxSettings settings, string text)
        {
            return StringUtility.ExtractAll(text, settings.TOKEN_PARAMETER_OPENING, settings.TOKEN_PARAMETER_CLOSING, suppressWarning: true);
        }

        /// <summary>Extract parameter by using parameter tokens</summary>
        public static string ReadParameter(DSyntaxSettings settings, string text)
        {
            return StringUtility.Extract(text, settings.TOKEN_PARAMETER_OPENING, settings.TOKEN_PARAMETER_CLOSING, suppressWarning: true);
        }

        /// <summary>Extract command's name and its parameter from a dialogue syntax text</summary>
        public static Command ReadCommand(DSyntaxSettings settings, string text)
        {
            var _text = text;

            // Extract command name
            var commandName = _text.Extract(settings.TOKEN_COMMAND_OPENING, settings.TOKEN_COMMAND_CLOSING);
            _text = _text.ReplaceFirst(commandName, "");

            // Extract parameters that are wrapped by tokens
            var parameters = _text.ExtractAll(settings.TOKEN_PARAMETER_OPENING, settings.TOKEN_PARAMETER_CLOSING, suppressWarning: true);
            _text = _text.ReplaceFirst(parameters, "");

            // Extract one parameter that is not wrapped by tokens
            _text = RemoveWrapperTokens(settings, _text);
            if (!string.IsNullOrEmpty(_text))
                parameters.Insert(0, _text);

            return new Command(commandName, parameters, text);
        }

        /// <summary>Extract multiple command's name and its parameter into a list from a dialogue syntax text</summary>
        public static List<Command> ReadCommands(DSyntaxSettings settings, string text)
        {
            List<Command> result = new List<Command>();
            var commands = StringUtility.SplitAfterToken(text, settings.TOKEN_COMMAND_OPENING);
            foreach (var command in commands) result.Add(ReadCommand(settings, command));

            return result;
        }

        /// <summary> Extract multiple lists of commands by separating each group by a command name </summary>
        /// <param name="separatorCommandName">Command name which starts a group</param>
        /// <param name="removeSeparatorCommand">Whether to remove the command which separates groups</param>
        public static List<List<Command>> ReadCommandsByGroups(DSyntaxSettings settings, string text, string separatorCommandName)
        {
            List<List<Command>> result = new List<List<Command>>();

            var groups = StringUtility.SplitAfterToken(text, settings.TOKEN_COMMAND_OPENING + separatorCommandName + settings.TOKEN_COMMAND_CLOSING);
            foreach (var group in groups)
            {
                var commands = ReadCommands(settings, group);
                result.Add(commands);
            }

            return result;
        }

        #endregion

        public static ListCommand ConvertToListCommand (this Command command, DSyntaxSettings settings, bool parentHasParameters)
        {
            ListCommand listCommand = new ListCommand(command, childParameters: new List<List<string>>());
            listCommand.parameters = new List<string>();

            // Use temporal variable to store text
            var text = command.rawText.Trim();

            // Extract parent's parameter
            if (parentHasParameters)
            {
                // Extract parent's unwrapped parameters
                var parentUnwrappedParameter = text.Extract(settings.TOKEN_COMMAND_CLOSING, settings.TOKEN_PARAMETER_OPENING);
                if (!string.IsNullOrEmpty(parentUnwrappedParameter))
                {
                    listCommand.parameters.Add(parentUnwrappedParameter);
                    text = text.Remove(0, text.IndexOf(settings.TOKEN_PARAMETER_OPENING));
                }

                // Extract parent's wrapped parameters
                while (text.Length > 0)
                {
                    var param = text.Extract(settings.TOKEN_PARAMETER_OPENING, settings.TOKEN_PARAMETER_CLOSING);
                    listCommand.parameters.Add(param);
                    text = text.Remove(0, text.IndexOf(settings.TOKEN_PARAMETER_CLOSING));

                    if (!string.IsNullOrEmpty(text.Extract(settings.TOKEN_PARAMETER_CLOSING, settings.TOKEN_PARAMETER_OPENING)))
                        break;

                    text = text.Remove(0, text.IndexOf(settings.TOKEN_PARAMETER_CLOSING) + settings.TOKEN_PARAMETER_CLOSING.Length);
                }
            }
            else
            {
                text = text.RemoveByTokens(settings.TOKEN_COMMAND_OPENING, settings.TOKEN_COMMAND_CLOSING).Trim();
                text = text.Insert(0, settings.TOKEN_PARAMETER_CLOSING); // Add extra token at start, so it can be extracted by child
            }

            // Extract child's parameters
            while (text.Length > 0) 
            {
                // Prevent adding parameters if there are no more wrapping tokens
                if (text.IndexOf(settings.TOKEN_PARAMETER_OPENING) == -1 || text.IndexOf(settings.TOKEN_PARAMETER_CLOSING) == -1)
                    break;

                // Extract child's main parameters, and create new list
                var mainParameter = text.Extract(settings.TOKEN_PARAMETER_CLOSING, settings.TOKEN_PARAMETER_OPENING, suppressWarning: true);
                if (!string.IsNullOrEmpty(mainParameter))
                    listCommand.childParameters.Add(new List<string>() { mainParameter });
                text = text.Remove(0, text.IndexOf(settings.TOKEN_PARAMETER_OPENING));

                // Extract child's wrapped parameters
                var param = text.Extract(settings.TOKEN_PARAMETER_OPENING, settings.TOKEN_PARAMETER_CLOSING);
                listCommand.childParameters[listCommand.childParameters.Count-1].Add(param);
                text = text.Remove(0, text.IndexOf(settings.TOKEN_PARAMETER_CLOSING));
            }

            return listCommand;
        }

        public static string RemoveWrapperTokens(DSyntaxSettings settings, string text)
        {
            var result = text;
            result = result.Replace(settings.TOKEN_COMMAND_OPENING, "");
            result = result.Replace(settings.TOKEN_COMMAND_CLOSING, "");
            result = result.Replace(settings.TOKEN_PARAMETER_OPENING, "");
            result = result.Replace(settings.TOKEN_PARAMETER_CLOSING, "");
            result = result.Trim();
            return result;
        }

        #endregion

        #region [Sample Generators]

        public static string GenerateDialogueSimple(DSyntaxSettings settings, List<string> actorNames = null)
        {
            if (actorNames == null) actorNames = new List<string>() { "Zach", "Gabriel" };
            string GetRandomName() { return actorNames[UnityEngine.Random.Range(0, actorNames.Count)]; }

            var result = WriteCommand(settings, settings.COMMAND_BRANCH, settings.START, true);
            result += WriteCommand(settings, GetRandomName(), "Hello, World!");

            return result;
        }

        public static string GenerateDialogueMultiBranches(DSyntaxSettings settings, List<string> actorNames = null)
        {
            if (actorNames == null) actorNames = new List<string>() { "Zach", "Gabriel" };
            string GetRandomName() { return actorNames[UnityEngine.Random.Range(0, actorNames.Count)]; }

            var result = WriteCommand(settings, settings.COMMAND_BRANCH, settings.START, true);
            foreach (var actorName in actorNames) result += WriteCommand(settings, actorName, "Hello, World!");

            result += WriteCommand(settings, settings.COMMAND_BRANCH, "branch_one", true);
            foreach (var actorName in actorNames) result += WriteCommand(settings, actorName, "We're in branch_one");

            return result;
        }

        public static string GenerateDialogueChoices(DSyntaxSettings settings, List<string> actorNames = null)
        {
            if (actorNames == null) actorNames = new List<string>() { "Zach", "Gabriel" };
            string GetRandomName() { return actorNames[UnityEngine.Random.Range(0, actorNames.Count)]; }

            var result = WriteCommand(settings, settings.COMMAND_BRANCH, settings.START, true);
            result += WriteCommand(settings, GetRandomName(), "Let's choose!");

            result += WriteListCommand(settings, settings.COMMAND_CHOICES, 
                new List<string>() { "Title here" }, new List<List<string>>() 
                {
                    new List<string>(){ "Go to branch_one ", "branch_one" },
                    new List<string>(){ "Go to branch_two ", "branch_two" }
                },
                wrapParentFirstParameter: true);
  

            result += "\n";

            result += WriteCommand(settings, settings.COMMAND_BRANCH, "branch_one", true);
            result += WriteCommand(settings, GetRandomName(), "We are in branch_one");

            result += "\n";

            result += WriteCommand(settings, settings.COMMAND_BRANCH, "branch_two", true);
            result += WriteCommand(settings, GetRandomName(), "We are in branch_two");

            return result;
        }


        public static string GenerateComplex(DSyntaxSettings settings, List<string> actorNames = null)
        {
            if (actorNames == null) actorNames = new List<string>() { "Zach", "Gabriel" };
            string GetRandomName() { return actorNames[UnityEngine.Random.Range(0,actorNames.Count)]; }

            var result = WriteCommand(settings, settings.COMMAND_BRANCH, settings.START, true);
            result += WriteCommand(settings, GetRandomName(), "Let's choose!");

            result += WriteListCommand(settings, settings.COMMAND_CHOICES, 
                new List<string>() { "Title here" }, new List<List<string>>()
                {
                    new List<string>(){ "Go to branch_one ", "branch_one" },
                    new List<string>(){ "Go to branch_two ", "branch_two" }
                },
                wrapParentFirstParameter: true);


            result += "\n";

            result += WriteCommand(settings, settings.COMMAND_BRANCH, "branch_one", true);
            result += WriteCommand(settings, settings.COMMAND_ONCE, "branch_two");
            result += WriteCommand(settings, GetRandomName(), "We are in branch_one, but only for one time");
            result += WriteCommand(settings, settings.COMMAND_SET, "int_liar += 1");
            result += WriteCommand(settings, settings.COMMAND_GOTO, settings.START);

            result += "\n";

            result += WriteCommand(settings, settings.COMMAND_BRANCH, "branch_two", true);
            result += WriteCommand(settings, GetRandomName(), "We are in branch_two");
            result += WriteListCommand(settings, settings.COMMAND_URGENT, 
                new List<string> () { "Title here", "2" }, new List<List<string>>() 
                {
                    new List<string>(){"Let's go to branch_one", "branch_one", "speed:3", "color:red"},
                    new List<string>(){"Let's visit branch_three", "branch_three", "int_liar < 2", "speed:1" },
                    new List<string>(){"DONE", "branch_four" }
                });

            result += "\n";

            result += WriteCommand(settings, settings.COMMAND_BRANCH, "branch_three", true);
            result += WriteCommand(settings, GetRandomName(), "We are in branch_three, let go to branch_two");
            result += WriteCommand(settings, settings.COMMAND_GOTO, "branch_two");

            result += "\n";

            result += WriteCommand(settings, settings.COMMAND_BRANCH, "branch_four", true);
            result += WriteCommand(settings, GetRandomName(), "Finish!");

            return result;
        }

        #endregion
    }
}

