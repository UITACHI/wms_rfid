using System;
using System.Configuration;
using System.Web;

namespace THOK.Common
{
	/// <summary>
	/// This Class is used to read configuration settings from the app.config file.
	/// </summary>
	public sealed class ConfigSettings
	{
        static LogManager logger = new LogManager();
		#region Constants

		/// <summary>
		/// Database connection string.
		/// </summary>
		public const string DB_CONNECTION_STRING = "DBConnectionString";

		/// <summary>
		/// Key uses to fetch the Database Provider. "SQL, Oracle, etc..."
		/// </summary>
		public const string DB_PROVIDER = "DBProvider";

		#endregion

		#region Constructors

		public ConfigSettings()
		{
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Reads the given configuration value from the app.config. 
		/// If the key does not exists then returns the default value.
		/// </summary>
		/// <param name="key">Configuration key to be read.</param>
		/// <param name="defaultValue">Default value of the key.</param>
		/// <returns></returns>
		public static string ReadConfigValue(string key, string defaultValue)
		{
			string configValue;
			
			try
			{
                configValue =  ConfigurationManager.AppSettings[key];
                if (configValue == null)
                    configValue = defaultValue;
			}
			catch// (Exception ex)
			{
				configValue = defaultValue;
			}

			return configValue;
		}

        /// <summary>
        /// Read the query string values of from the URL
        /// </summary>
        /// <param name="key">KEY to be searched for</param>
        /// <param name="val">value to be copied to </param>
        /// <returns>True return if the operation is successful, else a 'false' returns</returns>
        public static bool GetQueryStringValue(string key, out short val)
        {
            try
            {
                val = 0;
                string strVal = HttpContext.Current.Request.QueryString.Get(key);
                if (!string.IsNullOrEmpty(strVal))
                {
                    if (short.TryParse(strVal, out val)) return true;
                }
            }
            catch (System.Exception e)
            {
                logger.LogError(logger.GetType(), e);
                val = 0;
                return false;
            }
            return false;
        }

		#endregion
	}
}
