namespace DynamoXmlConverter.API.Extensions
{
    public static class StringExtensions
    {
        public static string XmlToPrettyJson(this string xml)
        {
            var doc = System.Xml.Linq.XDocument.Parse(xml);
            return Newtonsoft.Json.JsonConvert.SerializeXNode(doc, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
		/// Writes to a file in a thread-safe manner using a Mutex lock.
		/// Overwrites files by default.
		/// </summary>
		/// <remarks>Inspired by https://stackoverflow.com/a/229567 .</remarks>
		/// <param name="input">Input string to write to the file.</param>
		/// <param name="filePath">Path of file to write to.</param>
		/// <param name="overwrite">Whether to overwrite pre-existing files.</param>
		public static void SafelyWriteToFile(this string input, string filePath, bool overwrite = true)
        {

            // Unique id for global mutex - Global prefix means it is global to the machine
            // We use filePath to ensure the mutex is only held for the particular file
            string mutexId = string.Format("Global\\{{{0}}}", Path.GetFileNameWithoutExtension(filePath));

            // We create/query the Mutex
            using (var mutex = new Mutex(false, mutexId))
            {

                var hasHandle = false;

                try
                {

                    // We wait for lock to release
                    hasHandle = mutex.WaitOne(Timeout.Infinite, false);

                    // Write to file
                    if (overwrite)
                        System.IO.File.WriteAllText(filePath, input);
                    else
                        System.IO.File.AppendAllText(filePath, input);

                }
                finally
                {

                    // If we have the Mutex, we release it
                    if (hasHandle)
                        mutex.ReleaseMutex();
                }
            }
        }
    }
}