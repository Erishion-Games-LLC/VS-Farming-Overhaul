using Vintagestory.API.Common;

namespace FarmingOverhaulTests.tests.UtilitiesForTesting
{
    public class TestLogger : ILogger
    {
        public List<string> Errors { get; } = [];

        public void Error(string message)
        {
            Errors.Add(message);
        }

        public void Error(string format, params object[] args)
        {
            Errors.Add(string.Format(format, args));
        }

        public void Error(Exception e)
        {
            Errors.Add(e.Message);
        }


        public bool TraceLog { get; set; }
        public event LogEntryDelegate EntryAdded;

        public void Audit(string format, params object[] args) { }
        public void Audit(string message) { }
        public void Build(string format, params object[] args) { }
        public void Build(string message) { }
        public void Chat(string format, params object[] args) { }
        public void Chat(string message) { }
        public void ClearWatchers() { }
        public void Debug(string format, params object[] args) { }
        public void Debug(string message) { }
        public void Event(string format, params object[] args) { }
        public void Event(string message) { }
        public void Fatal(string format, params object[] args) { }
        public void Fatal(string message) { }
        public void Fatal(Exception e) { }
        public void Log(EnumLogType logType, string format, params object[] args) { }
        public void Log(EnumLogType logType, string message) { }
        public void LogException(EnumLogType logType, Exception e) { }
        public void Notification(string format, params object[] args) { }
        public void Notification(string message) { }
        public void StoryEvent(string format, params object[] args) { }
        public void StoryEvent(string message) { }
        public void VerboseDebug(string format, params object[] args) { }
        public void VerboseDebug(string message) { }
        public void Warning(string format, params object[] args) { }
        public void Warning(string message) { }
        public void Warning(Exception e) { }
    }
}