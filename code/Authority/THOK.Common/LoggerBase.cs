using System;

using log4net;

namespace THOK.Common
{
	/// <summary>
	/// This is the base class for loggin purposes. 
	/// Any class that require to log errors/debug/info, it must be derived from this.
	/// </summary>
	public abstract class LoggerBase
	{
		#region Member Variables

		/// <summary>
		/// Member variable to hold the <see cref="ILog"/> instance.
		/// </summary>
		private readonly log4net.ILog logger = null;

		#endregion

		#region Properties

		/// <summary>
		/// Abstract property which must be overridden by the derived classes.
		/// The logger prefix is used to create the logger instance.
		/// </summary>
		protected abstract System.Type LogPrefix
		{
			get;
		}

		#endregion

		#region Constructors

        private static bool isConfigured = false;
		/// <summary>
		/// Constructor of the class.
		/// </summary>
		public LoggerBase()
		{
			// initiate logging class           
            if (!isConfigured)
            {
                log4net.Config.DOMConfigurator.Configure();
                isConfigured = true;
            }
			logger = log4net.LogManager.GetLogger(this.LogPrefix);
		}

		#endregion

		#region Methods

		#region Protected Methods

		/// <summary>
		/// Information level messages are logged to the logger.
		/// </summary>
		/// <param name="message">String that needs to be logged.</param>
		protected void LogInfo(string message)
		{
			if (this.logger.IsInfoEnabled)
			{
				this.logger.Info(message);
			}
		}

		/// <summary>
		/// Information level messages are logged to the logger.
		/// </summary>
		/// <param name="message">String that needs to be logged.</param>
		/// <param name="e">The exception that needs to be logged.</param>
		protected void LogInfo(string message, Exception e)
		{
			if (this.logger.IsInfoEnabled)
			{
				this.logger.Info(message, e);
			}
		}

		/// <summary>
		/// Warning level messages are logged to the logger.
		/// </summary>
		/// <param name="message">String that needs to be logged.</param>
		protected void LogWarn(string message)
		{
			if (this.logger.IsWarnEnabled)
			{
				this.logger.Warn(message);
			}
		}

		/// <summary>
		/// Warning level messages are logged to the logger.
		/// </summary>
		/// <param name="message">String that needs to be logged.</param>
		/// <param name="e">The exception that needs to be logged.</param>
		protected void LogWarn(string message, Exception e)
		{
			if (this.logger.IsWarnEnabled)
			{
				this.logger.Warn(message, e);
			}
		}

		/// <summary>
		/// Error level messages are logged to the logger.
		/// </summary>
		/// <param name="message">String that needs to be logged.</param>
		protected void LogError(string message)
		{
			if (this.logger.IsErrorEnabled)
			{
				this.logger.Error(message);
			}
		}

		/// <summary>
		/// Error level messages are logged to the logger.
		/// </summary>
		/// <param name="message">String that needs to be logged.</param>
		/// <param name="e">The exception that needs to be logged.</param>
		protected void LogError(string message, Exception e)
		{
			if (this.logger.IsErrorEnabled)
			{
				this.logger.Error(message, e);
			}
		}

		/// <summary>
		/// Debug level messages are logged to the logger.
		/// </summary>
		/// <param name="message">String that needs to be logged</param>
		protected void LogDebug(string message)
		{
			if (this.logger.IsDebugEnabled)
			{
				this.logger.Debug(message);
			}
		}

		/// <summary>
		/// Debug level messages are logged to the logger.
		/// </summary>
		/// <param name="message">String that needs to be logged</param>
		/// <param name="e">The exception that needs to be logged</param>
		protected void LogDebug(string message, Exception e)
		{
			if (this.logger.IsDebugEnabled)
			{
				this.logger.Debug(message, e);
			}
		}

		/// <summary>
		/// Fatal level messages are logged to the logger.
		/// </summary>
		/// <param name="message">String that needs to be logged</param>
		protected void LogFatal(string message)
		{
			if (this.logger.IsFatalEnabled)
			{
				this.logger.Fatal(message);
			}
		}

		/// <summary>
		/// Fatal level messages are logged to the logger.
		/// </summary>
		/// <param name="message">String that needs to be logged</param>
		/// <param name="e">The exception that needs to be logged</param>
		protected void LogFatal(string message, Exception e)
		{
			if (this.logger.IsFatalEnabled)
			{
				this.logger.Fatal(message, e);
			}
		}

		#endregion

		#endregion
	}
}
