using System;
using Microsoft.Extensions.Logging;

namespace GPWebpayNet.Example
{
    public class Logger: ILogger
    {
        public void Error(string message)
        {
            Console.WriteLine(message);
        }

        public void Information(string message)
        {
            Console.WriteLine(message);
        }

        public void Warning(string message)
        {
            Console.WriteLine(message);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            switch (logLevel)
            {
                case LogLevel.Information:
                    Console.WriteLine(state.ToString());
                    break;
                case LogLevel.Warning:
                    Console.WriteLine(state.ToString());
                    break;
                case LogLevel.Error:
                    Console.WriteLine(state.ToString());
                    break;
                case LogLevel.Debug:
                    Console.WriteLine(state.ToString());
                    break;
                case LogLevel.Critical:
                    Console.WriteLine(state.ToString());
                    break;
                case LogLevel.Trace:
                    Console.WriteLine(state.ToString());
                    break;
                default:
                    Console.WriteLine(state.ToString());
                    break;
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
    }
    
    public class Logger<T>: ILogger<T>
    {
        public void Error(string message)
        {
            Console.WriteLine(message);
        }

        public void Information(string message)
        {
            Console.WriteLine(message);
        }

        public void Warning(string message)
        {
            Console.WriteLine(message);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            switch (logLevel)
            {
                case LogLevel.Information:
                    Console.WriteLine(state.ToString());
                    break;
                case LogLevel.Warning:
                    Console.WriteLine(state.ToString());
                    break;
                case LogLevel.Error:
                    Console.WriteLine(state.ToString());
                    break;
                case LogLevel.Debug:
                    Console.WriteLine(state.ToString());
                    break;
                case LogLevel.Critical:
                    Console.WriteLine(state.ToString());
                    break;
                case LogLevel.Trace:
                    Console.WriteLine(state.ToString());
                    break;
                default:
                    Console.WriteLine(state.ToString());
                    break;
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
    }
}