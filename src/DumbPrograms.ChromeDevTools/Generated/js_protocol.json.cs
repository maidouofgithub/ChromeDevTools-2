using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DumbPrograms.ChromeDevTools.Protocol
{

    /// <summary>
    /// This domain is deprecated - use Runtime or Log instead.
    /// </summary>
    namespace Console
    {

        #region Types

        /// <summary>
        /// Console message.
        /// </summary>
        public class ConsoleMessage
        {

            /// <summary>
            /// Message source.
            /// </summary>
            [JsonProperty("source")]
            public string Source { get; set; }

            /// <summary>
            /// Message severity.
            /// </summary>
            [JsonProperty("level")]
            public string Level { get; set; }

            /// <summary>
            /// Message text.
            /// </summary>
            [JsonProperty("text")]
            public string Text { get; set; }

            /// <summary>
            /// URL of the message origin.
            /// </summary>
            [JsonProperty("url")]
            public string Url { get; set; }

            /// <summary>
            /// Line number in the resource that generated this message (1-based).
            /// </summary>
            [JsonProperty("line")]
            public int Line { get; set; }

            /// <summary>
            /// Column number in the resource that generated this message (1-based).
            /// </summary>
            [JsonProperty("column")]
            public int Column { get; set; }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Does nothing.
        /// </summary>
        public class ClearMessagesCommand : ICommand
        {
            string ICommand.Name { get; } = "Console.clearMessages";
        }

        /// <summary>
        /// Disables console domain, prevents further console messages from being reported to the client.
        /// </summary>
        public class DisableCommand : ICommand
        {
            string ICommand.Name { get; } = "Console.disable";
        }

        /// <summary>
        /// Enables console domain, sends the messages collected so far to the client by means of the
        /// `messageAdded` notification.
        /// </summary>
        public class EnableCommand : ICommand
        {
            string ICommand.Name { get; } = "Console.enable";
        }

        #endregion

        #region Events

        /// <summary>
        /// Issued when new console message is added.
        /// </summary>
        public class MessageAddedEvent : ICommand
        {
            string ICommand.Name { get; } = "Console.messageAdded";

            /// <summary>
            /// Console message that has been added.
            /// </summary>
            [JsonProperty("message")]
            public ConsoleMessage Message { get; set; }
        }

        #endregion
    }

    /// <summary>
    /// Debugger domain exposes JavaScript debugging capabilities. It allows setting and removing
    /// breakpoints, stepping through execution, exploring stack traces, etc.
    /// </summary>
    namespace Debugger
    {

        #region Types

        /// <summary>
        /// Breakpoint identifier.
        /// </summary>
        public struct BreakpointId : IAlias<string>
        {
            public string Value { get; private set; }

            public BreakpointId(string value)
            {
                Value = value;
            }

        }

        /// <summary>
        /// Call frame identifier.
        /// </summary>
        public struct CallFrameId : IAlias<string>
        {
            public string Value { get; private set; }

            public CallFrameId(string value)
            {
                Value = value;
            }

        }

        /// <summary>
        /// Location in the source code.
        /// </summary>
        public class Location
        {

            /// <summary>
            /// Script identifier as reported in the `Debugger.scriptParsed`.
            /// </summary>
            [JsonProperty("scriptId")]
            public Runtime.ScriptId ScriptId { get; set; }

            /// <summary>
            /// Line number in the script (0-based).
            /// </summary>
            [JsonProperty("lineNumber")]
            public int LineNumber { get; set; }

            /// <summary>
            /// Column number in the script (0-based).
            /// </summary>
            [JsonProperty("columnNumber")]
            public int ColumnNumber { get; set; }
        }

        /// <summary>
        /// Location in the source code.
        /// </summary>
        public class ScriptPosition
        {

            [JsonProperty("lineNumber")]
            public int LineNumber { get; set; }

            [JsonProperty("columnNumber")]
            public int ColumnNumber { get; set; }
        }

        /// <summary>
        /// JavaScript call frame. Array of call frames form the call stack.
        /// </summary>
        public class CallFrame
        {

            /// <summary>
            /// Call frame identifier. This identifier is only valid while the virtual machine is paused.
            /// </summary>
            [JsonProperty("callFrameId")]
            public CallFrameId CallFrameId { get; set; }

            /// <summary>
            /// Name of the JavaScript function called on this call frame.
            /// </summary>
            [JsonProperty("functionName")]
            public string FunctionName { get; set; }

            /// <summary>
            /// Location in the source code.
            /// </summary>
            [JsonProperty("functionLocation")]
            public Location FunctionLocation { get; set; }

            /// <summary>
            /// Location in the source code.
            /// </summary>
            [JsonProperty("location")]
            public Location Location { get; set; }

            /// <summary>
            /// JavaScript script name or url.
            /// </summary>
            [JsonProperty("url")]
            public string Url { get; set; }

            /// <summary>
            /// Scope chain for this call frame.
            /// </summary>
            [JsonProperty("scopeChain")]
            public Scope[] ScopeChain { get; set; }

            /// <summary>
            /// `this` object for this call frame.
            /// </summary>
            [JsonProperty("this")]
            public Runtime.RemoteObject This { get; set; }

            /// <summary>
            /// The value being returned, if the function is at return point.
            /// </summary>
            [JsonProperty("returnValue")]
            public Runtime.RemoteObject ReturnValue { get; set; }
        }

        /// <summary>
        /// Scope description.
        /// </summary>
        public class Scope
        {

            /// <summary>
            /// Scope type.
            /// </summary>
            [JsonProperty("type")]
            public string Type { get; set; }

            /// <summary>
            /// Object representing the scope. For `global` and `with` scopes it represents the actual
            /// object; for the rest of the scopes, it is artificial transient object enumerating scope
            /// variables as its properties.
            /// </summary>
            [JsonProperty("object")]
            public Runtime.RemoteObject Object { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// Location in the source code where scope starts
            /// </summary>
            [JsonProperty("startLocation")]
            public Location StartLocation { get; set; }

            /// <summary>
            /// Location in the source code where scope ends
            /// </summary>
            [JsonProperty("endLocation")]
            public Location EndLocation { get; set; }
        }

        /// <summary>
        /// Search match for resource.
        /// </summary>
        public class SearchMatch
        {

            /// <summary>
            /// Line number in resource content.
            /// </summary>
            [JsonProperty("lineNumber")]
            public double LineNumber { get; set; }

            /// <summary>
            /// Line with match content.
            /// </summary>
            [JsonProperty("lineContent")]
            public string LineContent { get; set; }
        }

        public class BreakLocation
        {

            /// <summary>
            /// Script identifier as reported in the `Debugger.scriptParsed`.
            /// </summary>
            [JsonProperty("scriptId")]
            public Runtime.ScriptId ScriptId { get; set; }

            /// <summary>
            /// Line number in the script (0-based).
            /// </summary>
            [JsonProperty("lineNumber")]
            public int LineNumber { get; set; }

            /// <summary>
            /// Column number in the script (0-based).
            /// </summary>
            [JsonProperty("columnNumber")]
            public int ColumnNumber { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Continues execution until specific location is reached.
        /// </summary>
        public class ContinueToLocationCommand : ICommand
        {
            string ICommand.Name { get; } = "Debugger.continueToLocation";

            /// <summary>
            /// Location to continue to.
            /// </summary>
            [JsonProperty("location")]
            public Location Location { get; set; }

            [JsonProperty("targetCallFrames")]
            public string TargetCallFrames { get; set; }
        }

        /// <summary>
        /// Disables debugger for given page.
        /// </summary>
        public class DisableCommand : ICommand
        {
            string ICommand.Name { get; } = "Debugger.disable";
        }

        /// <summary>
        /// Enables debugger for the given page. Clients should not assume that the debugging has been
        /// enabled until the result for this command is received.
        /// </summary>
        public class EnableCommand : ICommand<EnableResponse>
        {
            string ICommand.Name { get; } = "Debugger.enable";
        }

        public class EnableResponse
        {

            /// <summary>
            /// Unique identifier of the debugger.
            /// </summary>
            [JsonProperty("debuggerId")]
            public Runtime.UniqueDebuggerId DebuggerId { get; set; }
        }

        /// <summary>
        /// Evaluates expression on a given call frame.
        /// </summary>
        public class EvaluateOnCallFrameCommand : ICommand<EvaluateOnCallFrameResponse>
        {
            string ICommand.Name { get; } = "Debugger.evaluateOnCallFrame";

            /// <summary>
            /// Call frame identifier to evaluate on.
            /// </summary>
            [JsonProperty("callFrameId")]
            public CallFrameId CallFrameId { get; set; }

            /// <summary>
            /// Expression to evaluate.
            /// </summary>
            [JsonProperty("expression")]
            public string Expression { get; set; }

            /// <summary>
            /// String object group name to put result into (allows rapid releasing resulting object handles
            /// using `releaseObjectGroup`).
            /// </summary>
            [JsonProperty("objectGroup")]
            public string ObjectGroup { get; set; }

            /// <summary>
            /// Specifies whether command line API should be available to the evaluated expression, defaults
            /// to false.
            /// </summary>
            [JsonProperty("includeCommandLineAPI")]
            public bool IncludeCommandLineAPI { get; set; }

            /// <summary>
            /// In silent mode exceptions thrown during evaluation are not reported and do not pause
            /// execution. Overrides `setPauseOnException` state.
            /// </summary>
            [JsonProperty("silent")]
            public bool Silent { get; set; }

            /// <summary>
            /// Whether the result is expected to be a JSON object that should be sent by value.
            /// </summary>
            [JsonProperty("returnByValue")]
            public bool ReturnByValue { get; set; }

            /// <summary>
            /// Whether preview should be generated for the result.
            /// </summary>
            [JsonProperty("generatePreview")]
            public bool GeneratePreview { get; set; }

            /// <summary>
            /// Whether to throw an exception if side effect cannot be ruled out during evaluation.
            /// </summary>
            [JsonProperty("throwOnSideEffect")]
            public bool ThrowOnSideEffect { get; set; }

            /// <summary>
            /// Terminate execution after timing out (number of milliseconds).
            /// </summary>
            [JsonProperty("timeout")]
            public Runtime.TimeDelta Timeout { get; set; }
        }

        public class EvaluateOnCallFrameResponse
        {

            /// <summary>
            /// Object wrapper for the evaluation result.
            /// </summary>
            [JsonProperty("result")]
            public Runtime.RemoteObject Result { get; set; }

            /// <summary>
            /// Exception details.
            /// </summary>
            [JsonProperty("exceptionDetails")]
            public Runtime.ExceptionDetails ExceptionDetails { get; set; }
        }

        /// <summary>
        /// Returns possible locations for breakpoint. scriptId in start and end range locations should be
        /// the same.
        /// </summary>
        public class GetPossibleBreakpointsCommand : ICommand<GetPossibleBreakpointsResponse>
        {
            string ICommand.Name { get; } = "Debugger.getPossibleBreakpoints";

            /// <summary>
            /// Start of range to search possible breakpoint locations in.
            /// </summary>
            [JsonProperty("start")]
            public Location Start { get; set; }

            /// <summary>
            /// End of range to search possible breakpoint locations in (excluding). When not specified, end
            /// of scripts is used as end of range.
            /// </summary>
            [JsonProperty("end")]
            public Location End { get; set; }

            /// <summary>
            /// Only consider locations which are in the same (non-nested) function as start.
            /// </summary>
            [JsonProperty("restrictToFunction")]
            public bool RestrictToFunction { get; set; }
        }

        public class GetPossibleBreakpointsResponse
        {

            /// <summary>
            /// List of the possible breakpoint locations.
            /// </summary>
            [JsonProperty("locations")]
            public BreakLocation[] Locations { get; set; }
        }

        /// <summary>
        /// Returns source for the script with given id.
        /// </summary>
        public class GetScriptSourceCommand : ICommand<GetScriptSourceResponse>
        {
            string ICommand.Name { get; } = "Debugger.getScriptSource";

            /// <summary>
            /// Id of the script to get source for.
            /// </summary>
            [JsonProperty("scriptId")]
            public Runtime.ScriptId ScriptId { get; set; }
        }

        public class GetScriptSourceResponse
        {

            /// <summary>
            /// Script source.
            /// </summary>
            [JsonProperty("scriptSource")]
            public string ScriptSource { get; set; }
        }

        /// <summary>
        /// Returns stack trace with given `stackTraceId`.
        /// </summary>
        public class GetStackTraceCommand : ICommand<GetStackTraceResponse>
        {
            string ICommand.Name { get; } = "Debugger.getStackTrace";

            [JsonProperty("stackTraceId")]
            public Runtime.StackTraceId StackTraceId { get; set; }
        }

        public class GetStackTraceResponse
        {

            [JsonProperty("stackTrace")]
            public Runtime.StackTrace StackTrace { get; set; }
        }

        /// <summary>
        /// Stops on the next JavaScript statement.
        /// </summary>
        public class PauseCommand : ICommand
        {
            string ICommand.Name { get; } = "Debugger.pause";
        }

        public class PauseOnAsyncCallCommand : ICommand
        {
            string ICommand.Name { get; } = "Debugger.pauseOnAsyncCall";

            /// <summary>
            /// Debugger will pause when async call with given stack trace is started.
            /// </summary>
            [JsonProperty("parentStackTraceId")]
            public Runtime.StackTraceId ParentStackTraceId { get; set; }
        }

        /// <summary>
        /// Removes JavaScript breakpoint.
        /// </summary>
        public class RemoveBreakpointCommand : ICommand
        {
            string ICommand.Name { get; } = "Debugger.removeBreakpoint";

            [JsonProperty("breakpointId")]
            public BreakpointId BreakpointId { get; set; }
        }

        /// <summary>
        /// Restarts particular call frame from the beginning.
        /// </summary>
        public class RestartFrameCommand : ICommand<RestartFrameResponse>
        {
            string ICommand.Name { get; } = "Debugger.restartFrame";

            /// <summary>
            /// Call frame identifier to evaluate on.
            /// </summary>
            [JsonProperty("callFrameId")]
            public CallFrameId CallFrameId { get; set; }
        }

        public class RestartFrameResponse
        {

            /// <summary>
            /// New stack trace.
            /// </summary>
            [JsonProperty("callFrames")]
            public CallFrame[] CallFrames { get; set; }

            /// <summary>
            /// Async stack trace, if any.
            /// </summary>
            [JsonProperty("asyncStackTrace")]
            public Runtime.StackTrace AsyncStackTrace { get; set; }

            /// <summary>
            /// Async stack trace, if any.
            /// </summary>
            [JsonProperty("asyncStackTraceId")]
            public Runtime.StackTraceId AsyncStackTraceId { get; set; }
        }

        /// <summary>
        /// Resumes JavaScript execution.
        /// </summary>
        public class ResumeCommand : ICommand
        {
            string ICommand.Name { get; } = "Debugger.resume";
        }

        /// <summary>
        /// Searches for given string in script content.
        /// </summary>
        public class SearchInContentCommand : ICommand<SearchInContentResponse>
        {
            string ICommand.Name { get; } = "Debugger.searchInContent";

            /// <summary>
            /// Id of the script to search in.
            /// </summary>
            [JsonProperty("scriptId")]
            public Runtime.ScriptId ScriptId { get; set; }

            /// <summary>
            /// String to search for.
            /// </summary>
            [JsonProperty("query")]
            public string Query { get; set; }

            /// <summary>
            /// If true, search is case sensitive.
            /// </summary>
            [JsonProperty("caseSensitive")]
            public bool CaseSensitive { get; set; }

            /// <summary>
            /// If true, treats string parameter as regex.
            /// </summary>
            [JsonProperty("isRegex")]
            public bool IsRegex { get; set; }
        }

        public class SearchInContentResponse
        {

            /// <summary>
            /// List of search matches.
            /// </summary>
            [JsonProperty("result")]
            public SearchMatch[] Result { get; set; }
        }

        /// <summary>
        /// Enables or disables async call stacks tracking.
        /// </summary>
        public class SetAsyncCallStackDepthCommand : ICommand
        {
            string ICommand.Name { get; } = "Debugger.setAsyncCallStackDepth";

            /// <summary>
            /// Maximum depth of async call stacks. Setting to `0` will effectively disable collecting async
            /// call stacks (default).
            /// </summary>
            [JsonProperty("maxDepth")]
            public int MaxDepth { get; set; }
        }

        /// <summary>
        /// Replace previous blackbox patterns with passed ones. Forces backend to skip stepping/pausing in
        /// scripts with url matching one of the patterns. VM will try to leave blackboxed script by
        /// performing 'step in' several times, finally resorting to 'step out' if unsuccessful.
        /// </summary>
        public class SetBlackboxPatternsCommand : ICommand
        {
            string ICommand.Name { get; } = "Debugger.setBlackboxPatterns";

            /// <summary>
            /// Array of regexps that will be used to check script url for blackbox state.
            /// </summary>
            [JsonProperty("patterns")]
            public string[] Patterns { get; set; }
        }

        /// <summary>
        /// Makes backend skip steps in the script in blackboxed ranges. VM will try leave blacklisted
        /// scripts by performing 'step in' several times, finally resorting to 'step out' if unsuccessful.
        /// Positions array contains positions where blackbox state is changed. First interval isn't
        /// blackboxed. Array should be sorted.
        /// </summary>
        public class SetBlackboxedRangesCommand : ICommand
        {
            string ICommand.Name { get; } = "Debugger.setBlackboxedRanges";

            /// <summary>
            /// Id of the script.
            /// </summary>
            [JsonProperty("scriptId")]
            public Runtime.ScriptId ScriptId { get; set; }

            [JsonProperty("positions")]
            public ScriptPosition[] Positions { get; set; }
        }

        /// <summary>
        /// Sets JavaScript breakpoint at a given location.
        /// </summary>
        public class SetBreakpointCommand : ICommand<SetBreakpointResponse>
        {
            string ICommand.Name { get; } = "Debugger.setBreakpoint";

            /// <summary>
            /// Location to set breakpoint in.
            /// </summary>
            [JsonProperty("location")]
            public Location Location { get; set; }

            /// <summary>
            /// Expression to use as a breakpoint condition. When specified, debugger will only stop on the
            /// breakpoint if this expression evaluates to true.
            /// </summary>
            [JsonProperty("condition")]
            public string Condition { get; set; }
        }

        public class SetBreakpointResponse
        {

            /// <summary>
            /// Id of the created breakpoint for further reference.
            /// </summary>
            [JsonProperty("breakpointId")]
            public BreakpointId BreakpointId { get; set; }

            /// <summary>
            /// Location this breakpoint resolved into.
            /// </summary>
            [JsonProperty("actualLocation")]
            public Location ActualLocation { get; set; }
        }

        /// <summary>
        /// Sets JavaScript breakpoint at given location specified either by URL or URL regex. Once this
        /// command is issued, all existing parsed scripts will have breakpoints resolved and returned in
        /// `locations` property. Further matching script parsing will result in subsequent
        /// `breakpointResolved` events issued. This logical breakpoint will survive page reloads.
        /// </summary>
        public class SetBreakpointByUrlCommand : ICommand<SetBreakpointByUrlResponse>
        {
            string ICommand.Name { get; } = "Debugger.setBreakpointByUrl";

            /// <summary>
            /// Line number to set breakpoint at.
            /// </summary>
            [JsonProperty("lineNumber")]
            public int LineNumber { get; set; }

            /// <summary>
            /// URL of the resources to set breakpoint on.
            /// </summary>
            [JsonProperty("url")]
            public string Url { get; set; }

            /// <summary>
            /// Regex pattern for the URLs of the resources to set breakpoints on. Either `url` or
            /// `urlRegex` must be specified.
            /// </summary>
            [JsonProperty("urlRegex")]
            public string UrlRegex { get; set; }

            /// <summary>
            /// Script hash of the resources to set breakpoint on.
            /// </summary>
            [JsonProperty("scriptHash")]
            public string ScriptHash { get; set; }

            /// <summary>
            /// Offset in the line to set breakpoint at.
            /// </summary>
            [JsonProperty("columnNumber")]
            public int ColumnNumber { get; set; }

            /// <summary>
            /// Expression to use as a breakpoint condition. When specified, debugger will only stop on the
            /// breakpoint if this expression evaluates to true.
            /// </summary>
            [JsonProperty("condition")]
            public string Condition { get; set; }
        }

        public class SetBreakpointByUrlResponse
        {

            /// <summary>
            /// Id of the created breakpoint for further reference.
            /// </summary>
            [JsonProperty("breakpointId")]
            public BreakpointId BreakpointId { get; set; }

            /// <summary>
            /// List of the locations this breakpoint resolved into upon addition.
            /// </summary>
            [JsonProperty("locations")]
            public Location[] Locations { get; set; }
        }

        /// <summary>
        /// Sets JavaScript breakpoint before each call to the given function.
        /// If another function was created from the same source as a given one,
        /// calling it will also trigger the breakpoint.
        /// </summary>
        public class SetBreakpointOnFunctionCallCommand : ICommand<SetBreakpointOnFunctionCallResponse>
        {
            string ICommand.Name { get; } = "Debugger.setBreakpointOnFunctionCall";

            /// <summary>
            /// Function object id.
            /// </summary>
            [JsonProperty("objectId")]
            public Runtime.RemoteObjectId ObjectId { get; set; }

            /// <summary>
            /// Expression to use as a breakpoint condition. When specified, debugger will
            /// stop on the breakpoint if this expression evaluates to true.
            /// </summary>
            [JsonProperty("condition")]
            public string Condition { get; set; }
        }

        public class SetBreakpointOnFunctionCallResponse
        {

            /// <summary>
            /// Id of the created breakpoint for further reference.
            /// </summary>
            [JsonProperty("breakpointId")]
            public BreakpointId BreakpointId { get; set; }
        }

        /// <summary>
        /// Activates / deactivates all breakpoints on the page.
        /// </summary>
        public class SetBreakpointsActiveCommand : ICommand
        {
            string ICommand.Name { get; } = "Debugger.setBreakpointsActive";

            /// <summary>
            /// New value for breakpoints active state.
            /// </summary>
            [JsonProperty("active")]
            public bool Active { get; set; }
        }

        /// <summary>
        /// Defines pause on exceptions state. Can be set to stop on all exceptions, uncaught exceptions or
        /// no exceptions. Initial pause on exceptions state is `none`.
        /// </summary>
        public class SetPauseOnExceptionsCommand : ICommand
        {
            string ICommand.Name { get; } = "Debugger.setPauseOnExceptions";

            /// <summary>
            /// Pause on exceptions mode.
            /// </summary>
            [JsonProperty("state")]
            public string State { get; set; }
        }

        /// <summary>
        /// Changes return value in top frame. Available only at return break position.
        /// </summary>
        public class SetReturnValueCommand : ICommand
        {
            string ICommand.Name { get; } = "Debugger.setReturnValue";

            /// <summary>
            /// New return value.
            /// </summary>
            [JsonProperty("newValue")]
            public Runtime.CallArgument NewValue { get; set; }
        }

        /// <summary>
        /// Edits JavaScript source live.
        /// </summary>
        public class SetScriptSourceCommand : ICommand<SetScriptSourceResponse>
        {
            string ICommand.Name { get; } = "Debugger.setScriptSource";

            /// <summary>
            /// Id of the script to edit.
            /// </summary>
            [JsonProperty("scriptId")]
            public Runtime.ScriptId ScriptId { get; set; }

            /// <summary>
            /// New content of the script.
            /// </summary>
            [JsonProperty("scriptSource")]
            public string ScriptSource { get; set; }

            /// <summary>
            /// If true the change will not actually be applied. Dry run may be used to get result
            /// description without actually modifying the code.
            /// </summary>
            [JsonProperty("dryRun")]
            public bool DryRun { get; set; }
        }

        public class SetScriptSourceResponse
        {

            /// <summary>
            /// New stack trace in case editing has happened while VM was stopped.
            /// </summary>
            [JsonProperty("callFrames")]
            public CallFrame[] CallFrames { get; set; }

            /// <summary>
            /// Whether current call stack  was modified after applying the changes.
            /// </summary>
            [JsonProperty("stackChanged")]
            public bool StackChanged { get; set; }

            /// <summary>
            /// Async stack trace, if any.
            /// </summary>
            [JsonProperty("asyncStackTrace")]
            public Runtime.StackTrace AsyncStackTrace { get; set; }

            /// <summary>
            /// Async stack trace, if any.
            /// </summary>
            [JsonProperty("asyncStackTraceId")]
            public Runtime.StackTraceId AsyncStackTraceId { get; set; }

            /// <summary>
            /// Exception details if any.
            /// </summary>
            [JsonProperty("exceptionDetails")]
            public Runtime.ExceptionDetails ExceptionDetails { get; set; }
        }

        /// <summary>
        /// Makes page not interrupt on any pauses (breakpoint, exception, dom exception etc).
        /// </summary>
        public class SetSkipAllPausesCommand : ICommand
        {
            string ICommand.Name { get; } = "Debugger.setSkipAllPauses";

            /// <summary>
            /// New value for skip pauses state.
            /// </summary>
            [JsonProperty("skip")]
            public bool Skip { get; set; }
        }

        /// <summary>
        /// Changes value of variable in a callframe. Object-based scopes are not supported and must be
        /// mutated manually.
        /// </summary>
        public class SetVariableValueCommand : ICommand
        {
            string ICommand.Name { get; } = "Debugger.setVariableValue";

            /// <summary>
            /// 0-based number of scope as was listed in scope chain. Only 'local', 'closure' and 'catch'
            /// scope types are allowed. Other scopes could be manipulated manually.
            /// </summary>
            [JsonProperty("scopeNumber")]
            public int ScopeNumber { get; set; }

            /// <summary>
            /// Variable name.
            /// </summary>
            [JsonProperty("variableName")]
            public string VariableName { get; set; }

            /// <summary>
            /// New variable value.
            /// </summary>
            [JsonProperty("newValue")]
            public Runtime.CallArgument NewValue { get; set; }

            /// <summary>
            /// Id of callframe that holds variable.
            /// </summary>
            [JsonProperty("callFrameId")]
            public CallFrameId CallFrameId { get; set; }
        }

        /// <summary>
        /// Steps into the function call.
        /// </summary>
        public class StepIntoCommand : ICommand
        {
            string ICommand.Name { get; } = "Debugger.stepInto";

            /// <summary>
            /// Debugger will issue additional Debugger.paused notification if any async task is scheduled
            /// before next pause.
            /// </summary>
            [JsonProperty("breakOnAsyncCall")]
            public bool BreakOnAsyncCall { get; set; }
        }

        /// <summary>
        /// Steps out of the function call.
        /// </summary>
        public class StepOutCommand : ICommand
        {
            string ICommand.Name { get; } = "Debugger.stepOut";
        }

        /// <summary>
        /// Steps over the statement.
        /// </summary>
        public class StepOverCommand : ICommand
        {
            string ICommand.Name { get; } = "Debugger.stepOver";
        }

        #endregion

        #region Events

        /// <summary>
        /// Fired when breakpoint is resolved to an actual script and location.
        /// </summary>
        public class BreakpointResolvedEvent : ICommand
        {
            string ICommand.Name { get; } = "Debugger.breakpointResolved";

            /// <summary>
            /// Breakpoint unique identifier.
            /// </summary>
            [JsonProperty("breakpointId")]
            public BreakpointId BreakpointId { get; set; }

            /// <summary>
            /// Actual breakpoint location.
            /// </summary>
            [JsonProperty("location")]
            public Location Location { get; set; }
        }

        /// <summary>
        /// Fired when the virtual machine stopped on breakpoint or exception or any other stop criteria.
        /// </summary>
        public class PausedEvent : ICommand
        {
            string ICommand.Name { get; } = "Debugger.paused";

            /// <summary>
            /// Call stack the virtual machine stopped on.
            /// </summary>
            [JsonProperty("callFrames")]
            public CallFrame[] CallFrames { get; set; }

            /// <summary>
            /// Pause reason.
            /// </summary>
            [JsonProperty("reason")]
            public string Reason { get; set; }

            /// <summary>
            /// Object containing break-specific auxiliary properties.
            /// </summary>
            [JsonProperty("data")]
            public object Data { get; set; }

            /// <summary>
            /// Hit breakpoints IDs
            /// </summary>
            [JsonProperty("hitBreakpoints")]
            public string[] HitBreakpoints { get; set; }

            /// <summary>
            /// Async stack trace, if any.
            /// </summary>
            [JsonProperty("asyncStackTrace")]
            public Runtime.StackTrace AsyncStackTrace { get; set; }

            /// <summary>
            /// Async stack trace, if any.
            /// </summary>
            [JsonProperty("asyncStackTraceId")]
            public Runtime.StackTraceId AsyncStackTraceId { get; set; }

            /// <summary>
            /// Just scheduled async call will have this stack trace as parent stack during async execution.
            /// This field is available only after `Debugger.stepInto` call with `breakOnAsynCall` flag.
            /// </summary>
            [JsonProperty("asyncCallStackTraceId")]
            public Runtime.StackTraceId AsyncCallStackTraceId { get; set; }
        }

        /// <summary>
        /// Fired when the virtual machine resumed execution.
        /// </summary>
        public class ResumedEvent : ICommand
        {
            string ICommand.Name { get; } = "Debugger.resumed";
        }

        /// <summary>
        /// Fired when virtual machine fails to parse the script.
        /// </summary>
        public class ScriptFailedToParseEvent : ICommand
        {
            string ICommand.Name { get; } = "Debugger.scriptFailedToParse";

            /// <summary>
            /// Identifier of the script parsed.
            /// </summary>
            [JsonProperty("scriptId")]
            public Runtime.ScriptId ScriptId { get; set; }

            /// <summary>
            /// URL or name of the script parsed (if any).
            /// </summary>
            [JsonProperty("url")]
            public string Url { get; set; }

            /// <summary>
            /// Line offset of the script within the resource with given URL (for script tags).
            /// </summary>
            [JsonProperty("startLine")]
            public int StartLine { get; set; }

            /// <summary>
            /// Column offset of the script within the resource with given URL.
            /// </summary>
            [JsonProperty("startColumn")]
            public int StartColumn { get; set; }

            /// <summary>
            /// Last line of the script.
            /// </summary>
            [JsonProperty("endLine")]
            public int EndLine { get; set; }

            /// <summary>
            /// Length of the last line of the script.
            /// </summary>
            [JsonProperty("endColumn")]
            public int EndColumn { get; set; }

            /// <summary>
            /// Specifies script creation context.
            /// </summary>
            [JsonProperty("executionContextId")]
            public Runtime.ExecutionContextId ExecutionContextId { get; set; }

            /// <summary>
            /// Content hash of the script.
            /// </summary>
            [JsonProperty("hash")]
            public string Hash { get; set; }

            /// <summary>
            /// Embedder-specific auxiliary data.
            /// </summary>
            [JsonProperty("executionContextAuxData")]
            public object ExecutionContextAuxData { get; set; }

            /// <summary>
            /// URL of source map associated with script (if any).
            /// </summary>
            [JsonProperty("sourceMapURL")]
            public string SourceMapURL { get; set; }

            /// <summary>
            /// True, if this script has sourceURL.
            /// </summary>
            [JsonProperty("hasSourceURL")]
            public bool HasSourceURL { get; set; }

            /// <summary>
            /// True, if this script is ES6 module.
            /// </summary>
            [JsonProperty("isModule")]
            public bool IsModule { get; set; }

            /// <summary>
            /// This script length.
            /// </summary>
            [JsonProperty("length")]
            public int Length { get; set; }

            /// <summary>
            /// JavaScript top stack frame of where the script parsed event was triggered if available.
            /// </summary>
            [JsonProperty("stackTrace")]
            public Runtime.StackTrace StackTrace { get; set; }
        }

        /// <summary>
        /// Fired when virtual machine parses script. This event is also fired for all known and uncollected
        /// scripts upon enabling debugger.
        /// </summary>
        public class ScriptParsedEvent : ICommand
        {
            string ICommand.Name { get; } = "Debugger.scriptParsed";

            /// <summary>
            /// Identifier of the script parsed.
            /// </summary>
            [JsonProperty("scriptId")]
            public Runtime.ScriptId ScriptId { get; set; }

            /// <summary>
            /// URL or name of the script parsed (if any).
            /// </summary>
            [JsonProperty("url")]
            public string Url { get; set; }

            /// <summary>
            /// Line offset of the script within the resource with given URL (for script tags).
            /// </summary>
            [JsonProperty("startLine")]
            public int StartLine { get; set; }

            /// <summary>
            /// Column offset of the script within the resource with given URL.
            /// </summary>
            [JsonProperty("startColumn")]
            public int StartColumn { get; set; }

            /// <summary>
            /// Last line of the script.
            /// </summary>
            [JsonProperty("endLine")]
            public int EndLine { get; set; }

            /// <summary>
            /// Length of the last line of the script.
            /// </summary>
            [JsonProperty("endColumn")]
            public int EndColumn { get; set; }

            /// <summary>
            /// Specifies script creation context.
            /// </summary>
            [JsonProperty("executionContextId")]
            public Runtime.ExecutionContextId ExecutionContextId { get; set; }

            /// <summary>
            /// Content hash of the script.
            /// </summary>
            [JsonProperty("hash")]
            public string Hash { get; set; }

            /// <summary>
            /// Embedder-specific auxiliary data.
            /// </summary>
            [JsonProperty("executionContextAuxData")]
            public object ExecutionContextAuxData { get; set; }

            /// <summary>
            /// True, if this script is generated as a result of the live edit operation.
            /// </summary>
            [JsonProperty("isLiveEdit")]
            public bool IsLiveEdit { get; set; }

            /// <summary>
            /// URL of source map associated with script (if any).
            /// </summary>
            [JsonProperty("sourceMapURL")]
            public string SourceMapURL { get; set; }

            /// <summary>
            /// True, if this script has sourceURL.
            /// </summary>
            [JsonProperty("hasSourceURL")]
            public bool HasSourceURL { get; set; }

            /// <summary>
            /// True, if this script is ES6 module.
            /// </summary>
            [JsonProperty("isModule")]
            public bool IsModule { get; set; }

            /// <summary>
            /// This script length.
            /// </summary>
            [JsonProperty("length")]
            public int Length { get; set; }

            /// <summary>
            /// JavaScript top stack frame of where the script parsed event was triggered if available.
            /// </summary>
            [JsonProperty("stackTrace")]
            public Runtime.StackTrace StackTrace { get; set; }
        }

        #endregion
    }

    namespace HeapProfiler
    {

        #region Types

        /// <summary>
        /// Heap snapshot object id.
        /// </summary>
        public struct HeapSnapshotObjectId : IAlias<string>
        {
            public string Value { get; private set; }

            public HeapSnapshotObjectId(string value)
            {
                Value = value;
            }

        }

        /// <summary>
        /// Sampling Heap Profile node. Holds callsite information, allocation statistics and child nodes.
        /// </summary>
        public class SamplingHeapProfileNode
        {

            /// <summary>
            /// Function location.
            /// </summary>
            [JsonProperty("callFrame")]
            public Runtime.CallFrame CallFrame { get; set; }

            /// <summary>
            /// Allocations size in bytes for the node excluding children.
            /// </summary>
            [JsonProperty("selfSize")]
            public double SelfSize { get; set; }

            /// <summary>
            /// Node id. Ids are unique across all profiles collected between startSampling and stopSampling.
            /// </summary>
            [JsonProperty("id")]
            public int Id { get; set; }

            /// <summary>
            /// Child nodes.
            /// </summary>
            [JsonProperty("children")]
            public SamplingHeapProfileNode[] Children { get; set; }
        }

        /// <summary>
        /// A single sample from a sampling profile.
        /// </summary>
        public class SamplingHeapProfileSample
        {

            /// <summary>
            /// Allocation size in bytes attributed to the sample.
            /// </summary>
            [JsonProperty("size")]
            public double Size { get; set; }

            /// <summary>
            /// Id of the corresponding profile tree node.
            /// </summary>
            [JsonProperty("nodeId")]
            public int NodeId { get; set; }

            /// <summary>
            /// Time-ordered sample ordinal number. It is unique across all profiles retrieved
            /// between startSampling and stopSampling.
            /// </summary>
            [JsonProperty("ordinal")]
            public double Ordinal { get; set; }
        }

        /// <summary>
        /// Sampling profile.
        /// </summary>
        public class SamplingHeapProfile
        {

            [JsonProperty("head")]
            public SamplingHeapProfileNode Head { get; set; }

            [JsonProperty("samples")]
            public SamplingHeapProfileSample[] Samples { get; set; }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Enables console to refer to the node with given id via $x (see Command Line API for more details
        /// $x functions).
        /// </summary>
        public class AddInspectedHeapObjectCommand : ICommand
        {
            string ICommand.Name { get; } = "HeapProfiler.addInspectedHeapObject";

            /// <summary>
            /// Heap snapshot object id to be accessible by means of $x command line API.
            /// </summary>
            [JsonProperty("heapObjectId")]
            public HeapSnapshotObjectId HeapObjectId { get; set; }
        }

        public class CollectGarbageCommand : ICommand
        {
            string ICommand.Name { get; } = "HeapProfiler.collectGarbage";
        }

        public class DisableCommand : ICommand
        {
            string ICommand.Name { get; } = "HeapProfiler.disable";
        }

        public class EnableCommand : ICommand
        {
            string ICommand.Name { get; } = "HeapProfiler.enable";
        }

        public class GetHeapObjectIdCommand : ICommand<GetHeapObjectIdResponse>
        {
            string ICommand.Name { get; } = "HeapProfiler.getHeapObjectId";

            /// <summary>
            /// Identifier of the object to get heap object id for.
            /// </summary>
            [JsonProperty("objectId")]
            public Runtime.RemoteObjectId ObjectId { get; set; }
        }

        public class GetHeapObjectIdResponse
        {

            /// <summary>
            /// Id of the heap snapshot object corresponding to the passed remote object id.
            /// </summary>
            [JsonProperty("heapSnapshotObjectId")]
            public HeapSnapshotObjectId HeapSnapshotObjectId { get; set; }
        }

        public class GetObjectByHeapObjectIdCommand : ICommand<GetObjectByHeapObjectIdResponse>
        {
            string ICommand.Name { get; } = "HeapProfiler.getObjectByHeapObjectId";

            [JsonProperty("objectId")]
            public HeapSnapshotObjectId ObjectId { get; set; }

            /// <summary>
            /// Symbolic group name that can be used to release multiple objects.
            /// </summary>
            [JsonProperty("objectGroup")]
            public string ObjectGroup { get; set; }
        }

        public class GetObjectByHeapObjectIdResponse
        {

            /// <summary>
            /// Evaluation result.
            /// </summary>
            [JsonProperty("result")]
            public Runtime.RemoteObject Result { get; set; }
        }

        public class GetSamplingProfileCommand : ICommand<GetSamplingProfileResponse>
        {
            string ICommand.Name { get; } = "HeapProfiler.getSamplingProfile";
        }

        public class GetSamplingProfileResponse
        {

            /// <summary>
            /// Return the sampling profile being collected.
            /// </summary>
            [JsonProperty("profile")]
            public SamplingHeapProfile Profile { get; set; }
        }

        public class StartSamplingCommand : ICommand
        {
            string ICommand.Name { get; } = "HeapProfiler.startSampling";

            /// <summary>
            /// Average sample interval in bytes. Poisson distribution is used for the intervals. The
            /// default value is 32768 bytes.
            /// </summary>
            [JsonProperty("samplingInterval")]
            public double SamplingInterval { get; set; }
        }

        public class StartTrackingHeapObjectsCommand : ICommand
        {
            string ICommand.Name { get; } = "HeapProfiler.startTrackingHeapObjects";

            [JsonProperty("trackAllocations")]
            public bool TrackAllocations { get; set; }
        }

        public class StopSamplingCommand : ICommand<StopSamplingResponse>
        {
            string ICommand.Name { get; } = "HeapProfiler.stopSampling";
        }

        public class StopSamplingResponse
        {

            /// <summary>
            /// Recorded sampling heap profile.
            /// </summary>
            [JsonProperty("profile")]
            public SamplingHeapProfile Profile { get; set; }
        }

        public class StopTrackingHeapObjectsCommand : ICommand
        {
            string ICommand.Name { get; } = "HeapProfiler.stopTrackingHeapObjects";

            /// <summary>
            /// If true 'reportHeapSnapshotProgress' events will be generated while snapshot is being taken
            /// when the tracking is stopped.
            /// </summary>
            [JsonProperty("reportProgress")]
            public bool ReportProgress { get; set; }
        }

        public class TakeHeapSnapshotCommand : ICommand
        {
            string ICommand.Name { get; } = "HeapProfiler.takeHeapSnapshot";

            /// <summary>
            /// If true 'reportHeapSnapshotProgress' events will be generated while snapshot is being taken.
            /// </summary>
            [JsonProperty("reportProgress")]
            public bool ReportProgress { get; set; }
        }

        #endregion

        #region Events

        public class AddHeapSnapshotChunkEvent : ICommand
        {
            string ICommand.Name { get; } = "HeapProfiler.addHeapSnapshotChunk";

            [JsonProperty("chunk")]
            public string Chunk { get; set; }
        }

        /// <summary>
        /// If heap objects tracking has been started then backend may send update for one or more fragments
        /// </summary>
        public class HeapStatsUpdateEvent : ICommand
        {
            string ICommand.Name { get; } = "HeapProfiler.heapStatsUpdate";

            /// <summary>
            /// An array of triplets. Each triplet describes a fragment. The first integer is the fragment
            /// index, the second integer is a total count of objects for the fragment, the third integer is
            /// a total size of the objects for the fragment.
            /// </summary>
            [JsonProperty("statsUpdate")]
            public int[] StatsUpdate { get; set; }
        }

        /// <summary>
        /// If heap objects tracking has been started then backend regularly sends a current value for last
        /// seen object id and corresponding timestamp. If the were changes in the heap since last event
        /// then one or more heapStatsUpdate events will be sent before a new lastSeenObjectId event.
        /// </summary>
        public class LastSeenObjectIdEvent : ICommand
        {
            string ICommand.Name { get; } = "HeapProfiler.lastSeenObjectId";

            [JsonProperty("lastSeenObjectId")]
            public int LastSeenObjectId { get; set; }

            [JsonProperty("timestamp")]
            public double Timestamp { get; set; }
        }

        public class ReportHeapSnapshotProgressEvent : ICommand
        {
            string ICommand.Name { get; } = "HeapProfiler.reportHeapSnapshotProgress";

            [JsonProperty("done")]
            public int Done { get; set; }

            [JsonProperty("total")]
            public int Total { get; set; }

            [JsonProperty("finished")]
            public bool Finished { get; set; }
        }

        public class ResetProfilesEvent : ICommand
        {
            string ICommand.Name { get; } = "HeapProfiler.resetProfiles";
        }

        #endregion
    }

    namespace Profiler
    {

        #region Types

        /// <summary>
        /// Profile node. Holds callsite information, execution statistics and child nodes.
        /// </summary>
        public class ProfileNode
        {

            /// <summary>
            /// Unique id of the node.
            /// </summary>
            [JsonProperty("id")]
            public int Id { get; set; }

            /// <summary>
            /// Function location.
            /// </summary>
            [JsonProperty("callFrame")]
            public Runtime.CallFrame CallFrame { get; set; }

            /// <summary>
            /// Number of samples where this node was on top of the call stack.
            /// </summary>
            [JsonProperty("hitCount")]
            public int HitCount { get; set; }

            /// <summary>
            /// Child node ids.
            /// </summary>
            [JsonProperty("children")]
            public int[] Children { get; set; }

            /// <summary>
            /// The reason of being not optimized. The function may be deoptimized or marked as don't
            /// optimize.
            /// </summary>
            [JsonProperty("deoptReason")]
            public string DeoptReason { get; set; }

            /// <summary>
            /// An array of source position ticks.
            /// </summary>
            [JsonProperty("positionTicks")]
            public PositionTickInfo[] PositionTicks { get; set; }
        }

        /// <summary>
        /// Profile.
        /// </summary>
        public class Profile
        {

            /// <summary>
            /// The list of profile nodes. First item is the root node.
            /// </summary>
            [JsonProperty("nodes")]
            public ProfileNode[] Nodes { get; set; }

            /// <summary>
            /// Profiling start timestamp in microseconds.
            /// </summary>
            [JsonProperty("startTime")]
            public double StartTime { get; set; }

            /// <summary>
            /// Profiling end timestamp in microseconds.
            /// </summary>
            [JsonProperty("endTime")]
            public double EndTime { get; set; }

            /// <summary>
            /// Ids of samples top nodes.
            /// </summary>
            [JsonProperty("samples")]
            public int[] Samples { get; set; }

            /// <summary>
            /// Time intervals between adjacent samples in microseconds. The first delta is relative to the
            /// profile startTime.
            /// </summary>
            [JsonProperty("timeDeltas")]
            public int[] TimeDeltas { get; set; }
        }

        /// <summary>
        /// Specifies a number of samples attributed to a certain source position.
        /// </summary>
        public class PositionTickInfo
        {

            /// <summary>
            /// Source line number (1-based).
            /// </summary>
            [JsonProperty("line")]
            public int Line { get; set; }

            /// <summary>
            /// Number of samples attributed to the source line.
            /// </summary>
            [JsonProperty("ticks")]
            public int Ticks { get; set; }
        }

        /// <summary>
        /// Coverage data for a source range.
        /// </summary>
        public class CoverageRange
        {

            /// <summary>
            /// JavaScript script source offset for the range start.
            /// </summary>
            [JsonProperty("startOffset")]
            public int StartOffset { get; set; }

            /// <summary>
            /// JavaScript script source offset for the range end.
            /// </summary>
            [JsonProperty("endOffset")]
            public int EndOffset { get; set; }

            /// <summary>
            /// Collected execution count of the source range.
            /// </summary>
            [JsonProperty("count")]
            public int Count { get; set; }
        }

        /// <summary>
        /// Coverage data for a JavaScript function.
        /// </summary>
        public class FunctionCoverage
        {

            /// <summary>
            /// JavaScript function name.
            /// </summary>
            [JsonProperty("functionName")]
            public string FunctionName { get; set; }

            /// <summary>
            /// Source ranges inside the function with coverage data.
            /// </summary>
            [JsonProperty("ranges")]
            public CoverageRange[] Ranges { get; set; }

            /// <summary>
            /// Whether coverage data for this function has block granularity.
            /// </summary>
            [JsonProperty("isBlockCoverage")]
            public bool IsBlockCoverage { get; set; }
        }

        /// <summary>
        /// Coverage data for a JavaScript script.
        /// </summary>
        public class ScriptCoverage
        {

            /// <summary>
            /// JavaScript script id.
            /// </summary>
            [JsonProperty("scriptId")]
            public Runtime.ScriptId ScriptId { get; set; }

            /// <summary>
            /// JavaScript script name or url.
            /// </summary>
            [JsonProperty("url")]
            public string Url { get; set; }

            /// <summary>
            /// Functions contained in the script that has coverage data.
            /// </summary>
            [JsonProperty("functions")]
            public FunctionCoverage[] Functions { get; set; }
        }

        /// <summary>
        /// Describes a type collected during runtime.
        /// </summary>
        public class TypeObject
        {

            /// <summary>
            /// Name of a type collected with type profiling.
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }
        }

        /// <summary>
        /// Source offset and types for a parameter or return value.
        /// </summary>
        public class TypeProfileEntry
        {

            /// <summary>
            /// Source offset of the parameter or end of function for return values.
            /// </summary>
            [JsonProperty("offset")]
            public int Offset { get; set; }

            /// <summary>
            /// The types for this parameter or return value.
            /// </summary>
            [JsonProperty("types")]
            public TypeObject[] Types { get; set; }
        }

        /// <summary>
        /// Type profile data collected during runtime for a JavaScript script.
        /// </summary>
        public class ScriptTypeProfile
        {

            /// <summary>
            /// JavaScript script id.
            /// </summary>
            [JsonProperty("scriptId")]
            public Runtime.ScriptId ScriptId { get; set; }

            /// <summary>
            /// JavaScript script name or url.
            /// </summary>
            [JsonProperty("url")]
            public string Url { get; set; }

            /// <summary>
            /// Type profile entries for parameters and return values of the functions in the script.
            /// </summary>
            [JsonProperty("entries")]
            public TypeProfileEntry[] Entries { get; set; }
        }

        #endregion

        #region Commands

        public class DisableCommand : ICommand
        {
            string ICommand.Name { get; } = "Profiler.disable";
        }

        public class EnableCommand : ICommand
        {
            string ICommand.Name { get; } = "Profiler.enable";
        }

        /// <summary>
        /// Collect coverage data for the current isolate. The coverage data may be incomplete due to
        /// garbage collection.
        /// </summary>
        public class GetBestEffortCoverageCommand : ICommand<GetBestEffortCoverageResponse>
        {
            string ICommand.Name { get; } = "Profiler.getBestEffortCoverage";
        }

        public class GetBestEffortCoverageResponse
        {

            /// <summary>
            /// Coverage data for the current isolate.
            /// </summary>
            [JsonProperty("result")]
            public ScriptCoverage[] Result { get; set; }
        }

        /// <summary>
        /// Changes CPU profiler sampling interval. Must be called before CPU profiles recording started.
        /// </summary>
        public class SetSamplingIntervalCommand : ICommand
        {
            string ICommand.Name { get; } = "Profiler.setSamplingInterval";

            /// <summary>
            /// New sampling interval in microseconds.
            /// </summary>
            [JsonProperty("interval")]
            public int Interval { get; set; }
        }

        public class StartCommand : ICommand
        {
            string ICommand.Name { get; } = "Profiler.start";
        }

        /// <summary>
        /// Enable precise code coverage. Coverage data for JavaScript executed before enabling precise code
        /// coverage may be incomplete. Enabling prevents running optimized code and resets execution
        /// counters.
        /// </summary>
        public class StartPreciseCoverageCommand : ICommand
        {
            string ICommand.Name { get; } = "Profiler.startPreciseCoverage";

            /// <summary>
            /// Collect accurate call counts beyond simple 'covered' or 'not covered'.
            /// </summary>
            [JsonProperty("callCount")]
            public bool CallCount { get; set; }

            /// <summary>
            /// Collect block-based coverage.
            /// </summary>
            [JsonProperty("detailed")]
            public bool Detailed { get; set; }
        }

        /// <summary>
        /// Enable type profile.
        /// </summary>
        public class StartTypeProfileCommand : ICommand
        {
            string ICommand.Name { get; } = "Profiler.startTypeProfile";
        }

        public class StopCommand : ICommand<StopResponse>
        {
            string ICommand.Name { get; } = "Profiler.stop";
        }

        public class StopResponse
        {

            /// <summary>
            /// Recorded profile.
            /// </summary>
            [JsonProperty("profile")]
            public Profile Profile { get; set; }
        }

        /// <summary>
        /// Disable precise code coverage. Disabling releases unnecessary execution count records and allows
        /// executing optimized code.
        /// </summary>
        public class StopPreciseCoverageCommand : ICommand
        {
            string ICommand.Name { get; } = "Profiler.stopPreciseCoverage";
        }

        /// <summary>
        /// Disable type profile. Disabling releases type profile data collected so far.
        /// </summary>
        public class StopTypeProfileCommand : ICommand
        {
            string ICommand.Name { get; } = "Profiler.stopTypeProfile";
        }

        /// <summary>
        /// Collect coverage data for the current isolate, and resets execution counters. Precise code
        /// coverage needs to have started.
        /// </summary>
        public class TakePreciseCoverageCommand : ICommand<TakePreciseCoverageResponse>
        {
            string ICommand.Name { get; } = "Profiler.takePreciseCoverage";
        }

        public class TakePreciseCoverageResponse
        {

            /// <summary>
            /// Coverage data for the current isolate.
            /// </summary>
            [JsonProperty("result")]
            public ScriptCoverage[] Result { get; set; }
        }

        /// <summary>
        /// Collect type profile.
        /// </summary>
        public class TakeTypeProfileCommand : ICommand<TakeTypeProfileResponse>
        {
            string ICommand.Name { get; } = "Profiler.takeTypeProfile";
        }

        public class TakeTypeProfileResponse
        {

            /// <summary>
            /// Type profile for all scripts since startTypeProfile() was turned on.
            /// </summary>
            [JsonProperty("result")]
            public ScriptTypeProfile[] Result { get; set; }
        }

        #endregion

        #region Events

        public class ConsoleProfileFinishedEvent : ICommand
        {
            string ICommand.Name { get; } = "Profiler.consoleProfileFinished";

            [JsonProperty("id")]
            public string Id { get; set; }

            /// <summary>
            /// Location of console.profileEnd().
            /// </summary>
            [JsonProperty("location")]
            public Debugger.Location Location { get; set; }

            [JsonProperty("profile")]
            public Profile Profile { get; set; }

            /// <summary>
            /// Profile title passed as an argument to console.profile().
            /// </summary>
            [JsonProperty("title")]
            public string Title { get; set; }
        }

        /// <summary>
        /// Sent when new profile recording is started using console.profile() call.
        /// </summary>
        public class ConsoleProfileStartedEvent : ICommand
        {
            string ICommand.Name { get; } = "Profiler.consoleProfileStarted";

            [JsonProperty("id")]
            public string Id { get; set; }

            /// <summary>
            /// Location of console.profile().
            /// </summary>
            [JsonProperty("location")]
            public Debugger.Location Location { get; set; }

            /// <summary>
            /// Profile title passed as an argument to console.profile().
            /// </summary>
            [JsonProperty("title")]
            public string Title { get; set; }
        }

        #endregion
    }

    /// <summary>
    /// Runtime domain exposes JavaScript runtime by means of remote evaluation and mirror objects.
    /// Evaluation results are returned as mirror object that expose object type, string representation
    /// and unique identifier that can be used for further object reference. Original objects are
    /// maintained in memory unless they are either explicitly released or are released along with the
    /// other objects in their object group.
    /// </summary>
    namespace Runtime
    {

        #region Types

        /// <summary>
        /// Unique script identifier.
        /// </summary>
        public struct ScriptId : IAlias<string>
        {
            public string Value { get; private set; }

            public ScriptId(string value)
            {
                Value = value;
            }

        }

        /// <summary>
        /// Unique object identifier.
        /// </summary>
        public struct RemoteObjectId : IAlias<string>
        {
            public string Value { get; private set; }

            public RemoteObjectId(string value)
            {
                Value = value;
            }

        }

        /// <summary>
        /// Primitive value which cannot be JSON-stringified. Includes values `-0`, `NaN`, `Infinity`,
        /// `-Infinity`, and bigint literals.
        /// </summary>
        public struct UnserializableValue : IAlias<string>
        {
            public string Value { get; private set; }

            public UnserializableValue(string value)
            {
                Value = value;
            }

        }

        /// <summary>
        /// Mirror object referencing original JavaScript object.
        /// </summary>
        public class RemoteObject
        {

            /// <summary>
            /// Object type.
            /// </summary>
            [JsonProperty("type")]
            public string Type { get; set; }

            /// <summary>
            /// Object subtype hint. Specified for `object` type values only.
            /// </summary>
            [JsonProperty("subtype")]
            public string Subtype { get; set; }

            /// <summary>
            /// Object class (constructor) name. Specified for `object` type values only.
            /// </summary>
            [JsonProperty("className")]
            public string ClassName { get; set; }

            /// <summary>
            /// Remote object value in case of primitive values or JSON values (if it was requested).
            /// </summary>
            [JsonProperty("value")]
            public object Value { get; set; }

            /// <summary>
            /// Primitive value which can not be JSON-stringified does not have `value`, but gets this
            /// property.
            /// </summary>
            [JsonProperty("unserializableValue")]
            public UnserializableValue UnserializableValue { get; set; }

            /// <summary>
            /// String representation of the object.
            /// </summary>
            [JsonProperty("description")]
            public string Description { get; set; }

            /// <summary>
            /// Unique object identifier (for non-primitive values).
            /// </summary>
            [JsonProperty("objectId")]
            public RemoteObjectId ObjectId { get; set; }

            /// <summary>
            /// Preview containing abbreviated property values. Specified for `object` type values only.
            /// </summary>
            [JsonProperty("preview")]
            public ObjectPreview Preview { get; set; }

            [JsonProperty("customPreview")]
            public CustomPreview CustomPreview { get; set; }
        }

        public class CustomPreview
        {

            /// <summary>
            /// The JSON-stringified result of formatter.header(object, config) call.
            /// It contains json ML array that represents RemoteObject.
            /// </summary>
            [JsonProperty("header")]
            public string Header { get; set; }

            /// <summary>
            /// If formatter returns true as a result of formatter.hasBody call then bodyGetterId will
            /// contain RemoteObjectId for the function that returns result of formatter.body(object, config) call.
            /// The result value is json ML array.
            /// </summary>
            [JsonProperty("bodyGetterId")]
            public RemoteObjectId BodyGetterId { get; set; }
        }

        /// <summary>
        /// Object containing abbreviated remote object value.
        /// </summary>
        public class ObjectPreview
        {

            /// <summary>
            /// Object type.
            /// </summary>
            [JsonProperty("type")]
            public string Type { get; set; }

            /// <summary>
            /// Object subtype hint. Specified for `object` type values only.
            /// </summary>
            [JsonProperty("subtype")]
            public string Subtype { get; set; }

            /// <summary>
            /// String representation of the object.
            /// </summary>
            [JsonProperty("description")]
            public string Description { get; set; }

            /// <summary>
            /// True iff some of the properties or entries of the original object did not fit.
            /// </summary>
            [JsonProperty("overflow")]
            public bool Overflow { get; set; }

            /// <summary>
            /// List of the properties.
            /// </summary>
            [JsonProperty("properties")]
            public PropertyPreview[] Properties { get; set; }

            /// <summary>
            /// List of the entries. Specified for `map` and `set` subtype values only.
            /// </summary>
            [JsonProperty("entries")]
            public EntryPreview[] Entries { get; set; }
        }

        public class PropertyPreview
        {

            /// <summary>
            /// Property name.
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// Object type. Accessor means that the property itself is an accessor property.
            /// </summary>
            [JsonProperty("type")]
            public string Type { get; set; }

            /// <summary>
            /// User-friendly property value string.
            /// </summary>
            [JsonProperty("value")]
            public string Value { get; set; }

            /// <summary>
            /// Nested value preview.
            /// </summary>
            [JsonProperty("valuePreview")]
            public ObjectPreview ValuePreview { get; set; }

            /// <summary>
            /// Object subtype hint. Specified for `object` type values only.
            /// </summary>
            [JsonProperty("subtype")]
            public string Subtype { get; set; }
        }

        public class EntryPreview
        {

            /// <summary>
            /// Preview of the key. Specified for map-like collection entries.
            /// </summary>
            [JsonProperty("key")]
            public ObjectPreview Key { get; set; }

            /// <summary>
            /// Preview of the value.
            /// </summary>
            [JsonProperty("value")]
            public ObjectPreview Value { get; set; }
        }

        /// <summary>
        /// Object property descriptor.
        /// </summary>
        public class PropertyDescriptor
        {

            /// <summary>
            /// Property name or symbol description.
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// The value associated with the property.
            /// </summary>
            [JsonProperty("value")]
            public RemoteObject Value { get; set; }

            /// <summary>
            /// True if the value associated with the property may be changed (data descriptors only).
            /// </summary>
            [JsonProperty("writable")]
            public bool Writable { get; set; }

            /// <summary>
            /// A function which serves as a getter for the property, or `undefined` if there is no getter
            /// (accessor descriptors only).
            /// </summary>
            [JsonProperty("get")]
            public RemoteObject Get { get; set; }

            /// <summary>
            /// A function which serves as a setter for the property, or `undefined` if there is no setter
            /// (accessor descriptors only).
            /// </summary>
            [JsonProperty("set")]
            public RemoteObject Set { get; set; }

            /// <summary>
            /// True if the type of this property descriptor may be changed and if the property may be
            /// deleted from the corresponding object.
            /// </summary>
            [JsonProperty("configurable")]
            public bool Configurable { get; set; }

            /// <summary>
            /// True if this property shows up during enumeration of the properties on the corresponding
            /// object.
            /// </summary>
            [JsonProperty("enumerable")]
            public bool Enumerable { get; set; }

            /// <summary>
            /// True if the result was thrown during the evaluation.
            /// </summary>
            [JsonProperty("wasThrown")]
            public bool WasThrown { get; set; }

            /// <summary>
            /// True if the property is owned for the object.
            /// </summary>
            [JsonProperty("isOwn")]
            public bool IsOwn { get; set; }

            /// <summary>
            /// Property symbol object, if the property is of the `symbol` type.
            /// </summary>
            [JsonProperty("symbol")]
            public RemoteObject Symbol { get; set; }
        }

        /// <summary>
        /// Object internal property descriptor. This property isn't normally visible in JavaScript code.
        /// </summary>
        public class InternalPropertyDescriptor
        {

            /// <summary>
            /// Conventional property name.
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// The value associated with the property.
            /// </summary>
            [JsonProperty("value")]
            public RemoteObject Value { get; set; }
        }

        /// <summary>
        /// Represents function call argument. Either remote object id `objectId`, primitive `value`,
        /// unserializable primitive value or neither of (for undefined) them should be specified.
        /// </summary>
        public class CallArgument
        {

            /// <summary>
            /// Primitive value or serializable javascript object.
            /// </summary>
            [JsonProperty("value")]
            public object Value { get; set; }

            /// <summary>
            /// Primitive value which can not be JSON-stringified.
            /// </summary>
            [JsonProperty("unserializableValue")]
            public UnserializableValue UnserializableValue { get; set; }

            /// <summary>
            /// Remote object handle.
            /// </summary>
            [JsonProperty("objectId")]
            public RemoteObjectId ObjectId { get; set; }
        }

        /// <summary>
        /// Id of an execution context.
        /// </summary>
        public struct ExecutionContextId : IAlias<int>
        {
            public int Value { get; private set; }

            public ExecutionContextId(int value)
            {
                Value = value;
            }

        }

        /// <summary>
        /// Description of an isolated world.
        /// </summary>
        public class ExecutionContextDescription
        {

            /// <summary>
            /// Unique id of the execution context. It can be used to specify in which execution context
            /// script evaluation should be performed.
            /// </summary>
            [JsonProperty("id")]
            public ExecutionContextId Id { get; set; }

            /// <summary>
            /// Execution context origin.
            /// </summary>
            [JsonProperty("origin")]
            public string Origin { get; set; }

            /// <summary>
            /// Human readable name describing given context.
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// Embedder-specific auxiliary data.
            /// </summary>
            [JsonProperty("auxData")]
            public object AuxData { get; set; }
        }

        /// <summary>
        /// Detailed information about exception (or error) that was thrown during script compilation or
        /// execution.
        /// </summary>
        public class ExceptionDetails
        {

            /// <summary>
            /// Exception id.
            /// </summary>
            [JsonProperty("exceptionId")]
            public int ExceptionId { get; set; }

            /// <summary>
            /// Exception text, which should be used together with exception object when available.
            /// </summary>
            [JsonProperty("text")]
            public string Text { get; set; }

            /// <summary>
            /// Line number of the exception location (0-based).
            /// </summary>
            [JsonProperty("lineNumber")]
            public int LineNumber { get; set; }

            /// <summary>
            /// Column number of the exception location (0-based).
            /// </summary>
            [JsonProperty("columnNumber")]
            public int ColumnNumber { get; set; }

            /// <summary>
            /// Script ID of the exception location.
            /// </summary>
            [JsonProperty("scriptId")]
            public ScriptId ScriptId { get; set; }

            /// <summary>
            /// URL of the exception location, to be used when the script was not reported.
            /// </summary>
            [JsonProperty("url")]
            public string Url { get; set; }

            /// <summary>
            /// JavaScript stack trace if available.
            /// </summary>
            [JsonProperty("stackTrace")]
            public StackTrace StackTrace { get; set; }

            /// <summary>
            /// Exception object if available.
            /// </summary>
            [JsonProperty("exception")]
            public RemoteObject Exception { get; set; }

            /// <summary>
            /// Identifier of the context where exception happened.
            /// </summary>
            [JsonProperty("executionContextId")]
            public ExecutionContextId ExecutionContextId { get; set; }
        }

        /// <summary>
        /// Number of milliseconds since epoch.
        /// </summary>
        public struct Timestamp : IAlias<double>
        {
            public double Value { get; private set; }

            public Timestamp(double value)
            {
                Value = value;
            }

        }

        /// <summary>
        /// Number of milliseconds.
        /// </summary>
        public struct TimeDelta : IAlias<double>
        {
            public double Value { get; private set; }

            public TimeDelta(double value)
            {
                Value = value;
            }

        }

        /// <summary>
        /// Stack entry for runtime errors and assertions.
        /// </summary>
        public class CallFrame
        {

            /// <summary>
            /// JavaScript function name.
            /// </summary>
            [JsonProperty("functionName")]
            public string FunctionName { get; set; }

            /// <summary>
            /// JavaScript script id.
            /// </summary>
            [JsonProperty("scriptId")]
            public ScriptId ScriptId { get; set; }

            /// <summary>
            /// JavaScript script name or url.
            /// </summary>
            [JsonProperty("url")]
            public string Url { get; set; }

            /// <summary>
            /// JavaScript script line number (0-based).
            /// </summary>
            [JsonProperty("lineNumber")]
            public int LineNumber { get; set; }

            /// <summary>
            /// JavaScript script column number (0-based).
            /// </summary>
            [JsonProperty("columnNumber")]
            public int ColumnNumber { get; set; }
        }

        /// <summary>
        /// Call frames for assertions or error messages.
        /// </summary>
        public class StackTrace
        {

            /// <summary>
            /// String label of this stack trace. For async traces this may be a name of the function that
            /// initiated the async call.
            /// </summary>
            [JsonProperty("description")]
            public string Description { get; set; }

            /// <summary>
            /// JavaScript function name.
            /// </summary>
            [JsonProperty("callFrames")]
            public CallFrame[] CallFrames { get; set; }

            /// <summary>
            /// Asynchronous JavaScript stack trace that preceded this stack, if available.
            /// </summary>
            [JsonProperty("parent")]
            public StackTrace Parent { get; set; }

            /// <summary>
            /// Asynchronous JavaScript stack trace that preceded this stack, if available.
            /// </summary>
            [JsonProperty("parentId")]
            public StackTraceId ParentId { get; set; }
        }

        /// <summary>
        /// Unique identifier of current debugger.
        /// </summary>
        public struct UniqueDebuggerId : IAlias<string>
        {
            public string Value { get; private set; }

            public UniqueDebuggerId(string value)
            {
                Value = value;
            }

        }

        /// <summary>
        /// If `debuggerId` is set stack trace comes from another debugger and can be resolved there. This
        /// allows to track cross-debugger calls. See `Runtime.StackTrace` and `Debugger.paused` for usages.
        /// </summary>
        public class StackTraceId
        {

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("debuggerId")]
            public UniqueDebuggerId DebuggerId { get; set; }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Add handler to promise with given promise object id.
        /// </summary>
        public class AwaitPromiseCommand : ICommand<AwaitPromiseResponse>
        {
            string ICommand.Name { get; } = "Runtime.awaitPromise";

            /// <summary>
            /// Identifier of the promise.
            /// </summary>
            [JsonProperty("promiseObjectId")]
            public RemoteObjectId PromiseObjectId { get; set; }

            /// <summary>
            /// Whether the result is expected to be a JSON object that should be sent by value.
            /// </summary>
            [JsonProperty("returnByValue")]
            public bool ReturnByValue { get; set; }

            /// <summary>
            /// Whether preview should be generated for the result.
            /// </summary>
            [JsonProperty("generatePreview")]
            public bool GeneratePreview { get; set; }
        }

        public class AwaitPromiseResponse
        {

            /// <summary>
            /// Promise result. Will contain rejected value if promise was rejected.
            /// </summary>
            [JsonProperty("result")]
            public RemoteObject Result { get; set; }

            /// <summary>
            /// Exception details if stack strace is available.
            /// </summary>
            [JsonProperty("exceptionDetails")]
            public ExceptionDetails ExceptionDetails { get; set; }
        }

        /// <summary>
        /// Calls function with given declaration on the given object. Object group of the result is
        /// inherited from the target object.
        /// </summary>
        public class CallFunctionOnCommand : ICommand<CallFunctionOnResponse>
        {
            string ICommand.Name { get; } = "Runtime.callFunctionOn";

            /// <summary>
            /// Declaration of the function to call.
            /// </summary>
            [JsonProperty("functionDeclaration")]
            public string FunctionDeclaration { get; set; }

            /// <summary>
            /// Identifier of the object to call function on. Either objectId or executionContextId should
            /// be specified.
            /// </summary>
            [JsonProperty("objectId")]
            public RemoteObjectId ObjectId { get; set; }

            /// <summary>
            /// Call arguments. All call arguments must belong to the same JavaScript world as the target
            /// object.
            /// </summary>
            [JsonProperty("arguments")]
            public CallArgument[] Arguments { get; set; }

            /// <summary>
            /// In silent mode exceptions thrown during evaluation are not reported and do not pause
            /// execution. Overrides `setPauseOnException` state.
            /// </summary>
            [JsonProperty("silent")]
            public bool Silent { get; set; }

            /// <summary>
            /// Whether the result is expected to be a JSON object which should be sent by value.
            /// </summary>
            [JsonProperty("returnByValue")]
            public bool ReturnByValue { get; set; }

            /// <summary>
            /// Whether preview should be generated for the result.
            /// </summary>
            [JsonProperty("generatePreview")]
            public bool GeneratePreview { get; set; }

            /// <summary>
            /// Whether execution should be treated as initiated by user in the UI.
            /// </summary>
            [JsonProperty("userGesture")]
            public bool UserGesture { get; set; }

            /// <summary>
            /// Whether execution should `await` for resulting value and return once awaited promise is
            /// resolved.
            /// </summary>
            [JsonProperty("awaitPromise")]
            public bool AwaitPromise { get; set; }

            /// <summary>
            /// Specifies execution context which global object will be used to call function on. Either
            /// executionContextId or objectId should be specified.
            /// </summary>
            [JsonProperty("executionContextId")]
            public ExecutionContextId ExecutionContextId { get; set; }

            /// <summary>
            /// Symbolic group name that can be used to release multiple objects. If objectGroup is not
            /// specified and objectId is, objectGroup will be inherited from object.
            /// </summary>
            [JsonProperty("objectGroup")]
            public string ObjectGroup { get; set; }
        }

        public class CallFunctionOnResponse
        {

            /// <summary>
            /// Call result.
            /// </summary>
            [JsonProperty("result")]
            public RemoteObject Result { get; set; }

            /// <summary>
            /// Exception details.
            /// </summary>
            [JsonProperty("exceptionDetails")]
            public ExceptionDetails ExceptionDetails { get; set; }
        }

        /// <summary>
        /// Compiles expression.
        /// </summary>
        public class CompileScriptCommand : ICommand<CompileScriptResponse>
        {
            string ICommand.Name { get; } = "Runtime.compileScript";

            /// <summary>
            /// Expression to compile.
            /// </summary>
            [JsonProperty("expression")]
            public string Expression { get; set; }

            /// <summary>
            /// Source url to be set for the script.
            /// </summary>
            [JsonProperty("sourceURL")]
            public string SourceURL { get; set; }

            /// <summary>
            /// Specifies whether the compiled script should be persisted.
            /// </summary>
            [JsonProperty("persistScript")]
            public bool PersistScript { get; set; }

            /// <summary>
            /// Specifies in which execution context to perform script run. If the parameter is omitted the
            /// evaluation will be performed in the context of the inspected page.
            /// </summary>
            [JsonProperty("executionContextId")]
            public ExecutionContextId ExecutionContextId { get; set; }
        }

        public class CompileScriptResponse
        {

            /// <summary>
            /// Id of the script.
            /// </summary>
            [JsonProperty("scriptId")]
            public ScriptId ScriptId { get; set; }

            /// <summary>
            /// Exception details.
            /// </summary>
            [JsonProperty("exceptionDetails")]
            public ExceptionDetails ExceptionDetails { get; set; }
        }

        /// <summary>
        /// Disables reporting of execution contexts creation.
        /// </summary>
        public class DisableCommand : ICommand
        {
            string ICommand.Name { get; } = "Runtime.disable";
        }

        /// <summary>
        /// Discards collected exceptions and console API calls.
        /// </summary>
        public class DiscardConsoleEntriesCommand : ICommand
        {
            string ICommand.Name { get; } = "Runtime.discardConsoleEntries";
        }

        /// <summary>
        /// Enables reporting of execution contexts creation by means of `executionContextCreated` event.
        /// When the reporting gets enabled the event will be sent immediately for each existing execution
        /// context.
        /// </summary>
        public class EnableCommand : ICommand
        {
            string ICommand.Name { get; } = "Runtime.enable";
        }

        /// <summary>
        /// Evaluates expression on global object.
        /// </summary>
        public class EvaluateCommand : ICommand<EvaluateResponse>
        {
            string ICommand.Name { get; } = "Runtime.evaluate";

            /// <summary>
            /// Expression to evaluate.
            /// </summary>
            [JsonProperty("expression")]
            public string Expression { get; set; }

            /// <summary>
            /// Symbolic group name that can be used to release multiple objects.
            /// </summary>
            [JsonProperty("objectGroup")]
            public string ObjectGroup { get; set; }

            /// <summary>
            /// Determines whether Command Line API should be available during the evaluation.
            /// </summary>
            [JsonProperty("includeCommandLineAPI")]
            public bool IncludeCommandLineAPI { get; set; }

            /// <summary>
            /// In silent mode exceptions thrown during evaluation are not reported and do not pause
            /// execution. Overrides `setPauseOnException` state.
            /// </summary>
            [JsonProperty("silent")]
            public bool Silent { get; set; }

            /// <summary>
            /// Specifies in which execution context to perform evaluation. If the parameter is omitted the
            /// evaluation will be performed in the context of the inspected page.
            /// </summary>
            [JsonProperty("contextId")]
            public ExecutionContextId ContextId { get; set; }

            /// <summary>
            /// Whether the result is expected to be a JSON object that should be sent by value.
            /// </summary>
            [JsonProperty("returnByValue")]
            public bool ReturnByValue { get; set; }

            /// <summary>
            /// Whether preview should be generated for the result.
            /// </summary>
            [JsonProperty("generatePreview")]
            public bool GeneratePreview { get; set; }

            /// <summary>
            /// Whether execution should be treated as initiated by user in the UI.
            /// </summary>
            [JsonProperty("userGesture")]
            public bool UserGesture { get; set; }

            /// <summary>
            /// Whether execution should `await` for resulting value and return once awaited promise is
            /// resolved.
            /// </summary>
            [JsonProperty("awaitPromise")]
            public bool AwaitPromise { get; set; }

            /// <summary>
            /// Whether to throw an exception if side effect cannot be ruled out during evaluation.
            /// </summary>
            [JsonProperty("throwOnSideEffect")]
            public bool ThrowOnSideEffect { get; set; }

            /// <summary>
            /// Terminate execution after timing out (number of milliseconds).
            /// </summary>
            [JsonProperty("timeout")]
            public TimeDelta Timeout { get; set; }
        }

        public class EvaluateResponse
        {

            /// <summary>
            /// Evaluation result.
            /// </summary>
            [JsonProperty("result")]
            public RemoteObject Result { get; set; }

            /// <summary>
            /// Exception details.
            /// </summary>
            [JsonProperty("exceptionDetails")]
            public ExceptionDetails ExceptionDetails { get; set; }
        }

        /// <summary>
        /// Returns the isolate id.
        /// </summary>
        public class GetIsolateIdCommand : ICommand<GetIsolateIdResponse>
        {
            string ICommand.Name { get; } = "Runtime.getIsolateId";
        }

        public class GetIsolateIdResponse
        {

            /// <summary>
            /// The isolate id.
            /// </summary>
            [JsonProperty("id")]
            public string Id { get; set; }
        }

        /// <summary>
        /// Returns the JavaScript heap usage.
        /// It is the total usage of the corresponding isolate not scoped to a particular Runtime.
        /// </summary>
        public class GetHeapUsageCommand : ICommand<GetHeapUsageResponse>
        {
            string ICommand.Name { get; } = "Runtime.getHeapUsage";
        }

        public class GetHeapUsageResponse
        {

            /// <summary>
            /// Used heap size in bytes.
            /// </summary>
            [JsonProperty("usedSize")]
            public double UsedSize { get; set; }

            /// <summary>
            /// Allocated heap size in bytes.
            /// </summary>
            [JsonProperty("totalSize")]
            public double TotalSize { get; set; }
        }

        /// <summary>
        /// Returns properties of a given object. Object group of the result is inherited from the target
        /// object.
        /// </summary>
        public class GetPropertiesCommand : ICommand<GetPropertiesResponse>
        {
            string ICommand.Name { get; } = "Runtime.getProperties";

            /// <summary>
            /// Identifier of the object to return properties for.
            /// </summary>
            [JsonProperty("objectId")]
            public RemoteObjectId ObjectId { get; set; }

            /// <summary>
            /// If true, returns properties belonging only to the element itself, not to its prototype
            /// chain.
            /// </summary>
            [JsonProperty("ownProperties")]
            public bool OwnProperties { get; set; }

            /// <summary>
            /// If true, returns accessor properties (with getter/setter) only; internal properties are not
            /// returned either.
            /// </summary>
            [JsonProperty("accessorPropertiesOnly")]
            public bool AccessorPropertiesOnly { get; set; }

            /// <summary>
            /// Whether preview should be generated for the results.
            /// </summary>
            [JsonProperty("generatePreview")]
            public bool GeneratePreview { get; set; }
        }

        public class GetPropertiesResponse
        {

            /// <summary>
            /// Object properties.
            /// </summary>
            [JsonProperty("result")]
            public PropertyDescriptor[] Result { get; set; }

            /// <summary>
            /// Internal object properties (only of the element itself).
            /// </summary>
            [JsonProperty("internalProperties")]
            public InternalPropertyDescriptor[] InternalProperties { get; set; }

            /// <summary>
            /// Exception details.
            /// </summary>
            [JsonProperty("exceptionDetails")]
            public ExceptionDetails ExceptionDetails { get; set; }
        }

        /// <summary>
        /// Returns all let, const and class variables from global scope.
        /// </summary>
        public class GlobalLexicalScopeNamesCommand : ICommand<GlobalLexicalScopeNamesResponse>
        {
            string ICommand.Name { get; } = "Runtime.globalLexicalScopeNames";

            /// <summary>
            /// Specifies in which execution context to lookup global scope variables.
            /// </summary>
            [JsonProperty("executionContextId")]
            public ExecutionContextId ExecutionContextId { get; set; }
        }

        public class GlobalLexicalScopeNamesResponse
        {

            [JsonProperty("names")]
            public string[] Names { get; set; }
        }

        public class QueryObjectsCommand : ICommand<QueryObjectsResponse>
        {
            string ICommand.Name { get; } = "Runtime.queryObjects";

            /// <summary>
            /// Identifier of the prototype to return objects for.
            /// </summary>
            [JsonProperty("prototypeObjectId")]
            public RemoteObjectId PrototypeObjectId { get; set; }

            /// <summary>
            /// Symbolic group name that can be used to release the results.
            /// </summary>
            [JsonProperty("objectGroup")]
            public string ObjectGroup { get; set; }
        }

        public class QueryObjectsResponse
        {

            /// <summary>
            /// Array with objects.
            /// </summary>
            [JsonProperty("objects")]
            public RemoteObject Objects { get; set; }
        }

        /// <summary>
        /// Releases remote object with given id.
        /// </summary>
        public class ReleaseObjectCommand : ICommand
        {
            string ICommand.Name { get; } = "Runtime.releaseObject";

            /// <summary>
            /// Identifier of the object to release.
            /// </summary>
            [JsonProperty("objectId")]
            public RemoteObjectId ObjectId { get; set; }
        }

        /// <summary>
        /// Releases all remote objects that belong to a given group.
        /// </summary>
        public class ReleaseObjectGroupCommand : ICommand
        {
            string ICommand.Name { get; } = "Runtime.releaseObjectGroup";

            /// <summary>
            /// Symbolic object group name.
            /// </summary>
            [JsonProperty("objectGroup")]
            public string ObjectGroup { get; set; }
        }

        /// <summary>
        /// Tells inspected instance to run if it was waiting for debugger to attach.
        /// </summary>
        public class RunIfWaitingForDebuggerCommand : ICommand
        {
            string ICommand.Name { get; } = "Runtime.runIfWaitingForDebugger";
        }

        /// <summary>
        /// Runs script with given id in a given context.
        /// </summary>
        public class RunScriptCommand : ICommand<RunScriptResponse>
        {
            string ICommand.Name { get; } = "Runtime.runScript";

            /// <summary>
            /// Id of the script to run.
            /// </summary>
            [JsonProperty("scriptId")]
            public ScriptId ScriptId { get; set; }

            /// <summary>
            /// Specifies in which execution context to perform script run. If the parameter is omitted the
            /// evaluation will be performed in the context of the inspected page.
            /// </summary>
            [JsonProperty("executionContextId")]
            public ExecutionContextId ExecutionContextId { get; set; }

            /// <summary>
            /// Symbolic group name that can be used to release multiple objects.
            /// </summary>
            [JsonProperty("objectGroup")]
            public string ObjectGroup { get; set; }

            /// <summary>
            /// In silent mode exceptions thrown during evaluation are not reported and do not pause
            /// execution. Overrides `setPauseOnException` state.
            /// </summary>
            [JsonProperty("silent")]
            public bool Silent { get; set; }

            /// <summary>
            /// Determines whether Command Line API should be available during the evaluation.
            /// </summary>
            [JsonProperty("includeCommandLineAPI")]
            public bool IncludeCommandLineAPI { get; set; }

            /// <summary>
            /// Whether the result is expected to be a JSON object which should be sent by value.
            /// </summary>
            [JsonProperty("returnByValue")]
            public bool ReturnByValue { get; set; }

            /// <summary>
            /// Whether preview should be generated for the result.
            /// </summary>
            [JsonProperty("generatePreview")]
            public bool GeneratePreview { get; set; }

            /// <summary>
            /// Whether execution should `await` for resulting value and return once awaited promise is
            /// resolved.
            /// </summary>
            [JsonProperty("awaitPromise")]
            public bool AwaitPromise { get; set; }
        }

        public class RunScriptResponse
        {

            /// <summary>
            /// Run result.
            /// </summary>
            [JsonProperty("result")]
            public RemoteObject Result { get; set; }

            /// <summary>
            /// Exception details.
            /// </summary>
            [JsonProperty("exceptionDetails")]
            public ExceptionDetails ExceptionDetails { get; set; }
        }

        /// <summary>
        /// Enables or disables async call stacks tracking.
        /// </summary>
        public class SetAsyncCallStackDepthCommand : ICommand
        {
            string ICommand.Name { get; } = "Runtime.setAsyncCallStackDepth";

            /// <summary>
            /// Maximum depth of async call stacks. Setting to `0` will effectively disable collecting async
            /// call stacks (default).
            /// </summary>
            [JsonProperty("maxDepth")]
            public int MaxDepth { get; set; }
        }

        public class SetCustomObjectFormatterEnabledCommand : ICommand
        {
            string ICommand.Name { get; } = "Runtime.setCustomObjectFormatterEnabled";

            [JsonProperty("enabled")]
            public bool Enabled { get; set; }
        }

        public class SetMaxCallStackSizeToCaptureCommand : ICommand
        {
            string ICommand.Name { get; } = "Runtime.setMaxCallStackSizeToCapture";

            [JsonProperty("size")]
            public int Size { get; set; }
        }

        /// <summary>
        /// Terminate current or next JavaScript execution.
        /// Will cancel the termination when the outer-most script execution ends.
        /// </summary>
        public class TerminateExecutionCommand : ICommand
        {
            string ICommand.Name { get; } = "Runtime.terminateExecution";
        }

        /// <summary>
        /// If executionContextId is empty, adds binding with the given name on the
        /// global objects of all inspected contexts, including those created later,
        /// bindings survive reloads.
        /// If executionContextId is specified, adds binding only on global object of
        /// given execution context.
        /// Binding function takes exactly one argument, this argument should be string,
        /// in case of any other input, function throws an exception.
        /// Each binding function call produces Runtime.bindingCalled notification.
        /// </summary>
        public class AddBindingCommand : ICommand
        {
            string ICommand.Name { get; } = "Runtime.addBinding";

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("executionContextId")]
            public ExecutionContextId ExecutionContextId { get; set; }
        }

        /// <summary>
        /// This method does not remove binding function from global object but
        /// unsubscribes current runtime agent from Runtime.bindingCalled notifications.
        /// </summary>
        public class RemoveBindingCommand : ICommand
        {
            string ICommand.Name { get; } = "Runtime.removeBinding";

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Notification is issued every time when binding is called.
        /// </summary>
        public class BindingCalledEvent : ICommand
        {
            string ICommand.Name { get; } = "Runtime.bindingCalled";

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("payload")]
            public string Payload { get; set; }

            /// <summary>
            /// Identifier of the context where the call was made.
            /// </summary>
            [JsonProperty("executionContextId")]
            public ExecutionContextId ExecutionContextId { get; set; }
        }

        /// <summary>
        /// Issued when console API was called.
        /// </summary>
        public class ConsoleAPICalledEvent : ICommand
        {
            string ICommand.Name { get; } = "Runtime.consoleAPICalled";

            /// <summary>
            /// Type of the call.
            /// </summary>
            [JsonProperty("type")]
            public string Type { get; set; }

            /// <summary>
            /// Call arguments.
            /// </summary>
            [JsonProperty("args")]
            public RemoteObject[] Args { get; set; }

            /// <summary>
            /// Identifier of the context where the call was made.
            /// </summary>
            [JsonProperty("executionContextId")]
            public ExecutionContextId ExecutionContextId { get; set; }

            /// <summary>
            /// Call timestamp.
            /// </summary>
            [JsonProperty("timestamp")]
            public Timestamp Timestamp { get; set; }

            /// <summary>
            /// Stack trace captured when the call was made.
            /// </summary>
            [JsonProperty("stackTrace")]
            public StackTrace StackTrace { get; set; }

            /// <summary>
            /// Console context descriptor for calls on non-default console context (not console.*):
            /// 'anonymous#unique-logger-id' for call on unnamed context, 'name#unique-logger-id' for call
            /// on named context.
            /// </summary>
            [JsonProperty("context")]
            public string Context { get; set; }
        }

        /// <summary>
        /// Issued when unhandled exception was revoked.
        /// </summary>
        public class ExceptionRevokedEvent : ICommand
        {
            string ICommand.Name { get; } = "Runtime.exceptionRevoked";

            /// <summary>
            /// Reason describing why exception was revoked.
            /// </summary>
            [JsonProperty("reason")]
            public string Reason { get; set; }

            /// <summary>
            /// The id of revoked exception, as reported in `exceptionThrown`.
            /// </summary>
            [JsonProperty("exceptionId")]
            public int ExceptionId { get; set; }
        }

        /// <summary>
        /// Issued when exception was thrown and unhandled.
        /// </summary>
        public class ExceptionThrownEvent : ICommand
        {
            string ICommand.Name { get; } = "Runtime.exceptionThrown";

            /// <summary>
            /// Timestamp of the exception.
            /// </summary>
            [JsonProperty("timestamp")]
            public Timestamp Timestamp { get; set; }

            [JsonProperty("exceptionDetails")]
            public ExceptionDetails ExceptionDetails { get; set; }
        }

        /// <summary>
        /// Issued when new execution context is created.
        /// </summary>
        public class ExecutionContextCreatedEvent : ICommand
        {
            string ICommand.Name { get; } = "Runtime.executionContextCreated";

            /// <summary>
            /// A newly created execution context.
            /// </summary>
            [JsonProperty("context")]
            public ExecutionContextDescription Context { get; set; }
        }

        /// <summary>
        /// Issued when execution context is destroyed.
        /// </summary>
        public class ExecutionContextDestroyedEvent : ICommand
        {
            string ICommand.Name { get; } = "Runtime.executionContextDestroyed";

            /// <summary>
            /// Id of the destroyed context
            /// </summary>
            [JsonProperty("executionContextId")]
            public ExecutionContextId ExecutionContextId { get; set; }
        }

        /// <summary>
        /// Issued when all executionContexts were cleared in browser
        /// </summary>
        public class ExecutionContextsClearedEvent : ICommand
        {
            string ICommand.Name { get; } = "Runtime.executionContextsCleared";
        }

        /// <summary>
        /// Issued when object should be inspected (for example, as a result of inspect() command line API
        /// call).
        /// </summary>
        public class InspectRequestedEvent : ICommand
        {
            string ICommand.Name { get; } = "Runtime.inspectRequested";

            [JsonProperty("object")]
            public RemoteObject Object { get; set; }

            [JsonProperty("hints")]
            public object Hints { get; set; }
        }

        #endregion
    }

    /// <summary>
    /// This domain is deprecated.
    /// </summary>
    namespace Schema
    {

        #region Types

        /// <summary>
        /// Description of the protocol domain.
        /// </summary>
        public class Domain
        {

            /// <summary>
            /// Domain name.
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// Domain version.
            /// </summary>
            [JsonProperty("version")]
            public string Version { get; set; }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Returns supported domains.
        /// </summary>
        public class GetDomainsCommand : ICommand<GetDomainsResponse>
        {
            string ICommand.Name { get; } = "Schema.getDomains";
        }

        public class GetDomainsResponse
        {

            /// <summary>
            /// List of supported domains.
            /// </summary>
            [JsonProperty("domains")]
            public Domain[] Domains { get; set; }
        }

        #endregion
    }
}