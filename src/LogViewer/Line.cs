using System;

namespace LogViewer
{
    public class Line
    {
        public string DateTime { get; private set; }
        public string Source { get; private set; }
        public string LogLevel { get; private set; }
        public string ConfigKey { get; private set; }
        public string InstanceId { get; private set; }
        public string ThreadId { get; private set; }
        public string Message { get; private set; }

        public Line(string line)
        {
            if(line[0] != '[')
            {
                DateTime = "Err";
                return;
            }

            DateTime = line.Substring(1, 13);
            line = line.Substring(16);
            var nextCloseBracket = line.IndexOf(']');
            LogLevel = line.Substring(0, nextCloseBracket);
            line = line.Substring(nextCloseBracket + 2);
            nextCloseBracket = line.IndexOf(']');
            Source = line.Substring(0, nextCloseBracket);
            line = line.Substring(nextCloseBracket + 2);
            nextCloseBracket = line.IndexOf(']');
            ConfigKey = line.Substring(0, nextCloseBracket);
            line = line.Substring(nextCloseBracket + 2);
            nextCloseBracket = line.IndexOf(']');
            InstanceId = line.Substring(0, nextCloseBracket);
            line = line.Substring(nextCloseBracket + 2);
            nextCloseBracket = line.IndexOf(']');
            ThreadId = line.Substring(0, nextCloseBracket);
            line = line.Substring(nextCloseBracket + 2);
            Message = line;
        }
    }
}