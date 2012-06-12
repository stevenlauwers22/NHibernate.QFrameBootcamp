using System;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace NHibernateCourse.Demo7.Tests
{
    public class LogSpyForNHibernateSql : LogSpy
    {
        public LogSpyForNHibernateSql() : base("NHibernate.SQL")
        {
        }
    }

    public class LogSpy : IDisposable
    {
        private readonly MemoryAppender _appender;
        private readonly Logger _logger;
        private readonly Level _prevLogLevel;

        static LogSpy()
        {
            XmlConfigurator.Configure();
        }

        public LogSpy(ILoggerWrapper log, Level level)
        {
            _logger = log.Logger as Logger;
            if (_logger == null)
            {
                throw new Exception("Unable to get the logger");
            }

            // Change the log level to DEBUG and temporarily save the previous log level
            _prevLogLevel = _logger.Level;
            _logger.Level = level;

            // Add a new MemoryAppender to the logger.
            _appender = new MemoryAppender();
            _logger.AddAppender(_appender);
        }

        public LogSpy(ILoggerWrapper log, bool disable)
            : this(log, disable ? Level.Off : Level.Debug)
        {
        }

        public LogSpy(ILoggerWrapper log) 
            : this(log, false)
        {
        }

        public LogSpy(Type loggerType) 
            : this(LogManager.GetLogger(loggerType), false)
        {

        }

        public LogSpy(Type loggerType, bool disable) 
            : this(LogManager.GetLogger(loggerType), disable)
        {
        }

        public LogSpy(string loggerName) 
            : this(LogManager.GetLogger(loggerName), false)
        {
        }

        public LogSpy(string loggerName, bool disable) 
            : this(LogManager.GetLogger(loggerName), disable)
        {
        }

        public MemoryAppender Appender
        {
            get { return _appender; }
        }

        public virtual string GetWholeLog()
        {
            var wholeMessage = new StringBuilder();
            foreach (var loggingEvent in Appender.GetEvents())
            {
                wholeMessage
                    .Append(loggingEvent.LoggerName)
                    .Append(" ")
                    .Append(loggingEvent.RenderedMessage)
                    .AppendLine();
            }

            return wholeMessage.ToString();
        }

        public virtual void Dispose()
        {
            // Restore the previous log level of the SQL logger and remove the MemoryAppender
            _logger.Level = _prevLogLevel;
            _logger.RemoveAppender(_appender);
        }
    }
}