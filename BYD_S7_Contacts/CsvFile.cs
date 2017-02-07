using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BYD_S7_Contacts
{
    /// <summary>
    /// https://www.codeproject.com/articles/415732/reading-and-writing-csv-files-in-csharp
    /// </summary>
    public class CsvFile
    {
        public static List<List<string>> Reader(string filename, Encoding encoding)
        {
            List<string> row;
            var rows = new List<List<string>>();

            var sr = new StreamReader(filename, encoding);

            while (!sr.EndOfStream)
            {
                var lineText = sr.ReadLine();
                row = ReadRow(lineText);
                if (row != null)
                {
                    rows.Add(row);
                }
            }
            sr.Close();
            sr.Dispose();

            return rows;
        }


        /// <summary>
        /// Writes a single row to a CSV file.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="encoding"></param>
        /// <param name="rows">The rows to be written</param>
        public static void Writer(string filename, Encoding encoding, List<List<string>> rows)
        {
            var sw = new StreamWriter(filename, false, encoding);

            foreach (var row in rows)
            {
                StringBuilder builder = new StringBuilder();
                bool firstColumn = true;
                foreach (string value in row)
                {
                    // Add separator if this isn't the first value
                    if (!firstColumn)
                        builder.Append(',');
                    // Implement special handling for values that contain comma or quote
                    // Enclose in quotes and double up any double quotes
                    if (value.IndexOfAny(new[] { '"', ',' }) != -1)
                        builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                    else
                        builder.Append(value);
                    firstColumn = false;
                }
                sw.WriteLine(builder.ToString());
            }
            sw.Close();
            sw.Dispose();
        }

        /// <summary>
        /// Reads a row of data from a CSV file
        /// </summary>
        /// <returns></returns>
        private static List<string> ReadRow(string lineText)
        {
            if (string.IsNullOrEmpty(lineText))
            {
                return null;
            }

            int pos = 0;
            int rows = 0;

            List<string> row = new List<string>();
            string value;
            while (pos < lineText.Length)
            {
                // Special handling for quoted field
                if (lineText[pos] == '"')
                {
                    // Skip initial quote
                    pos++;

                    // Parse quoted value
                    int start = pos;
                    while (pos < lineText.Length)
                    {
                        // Test for quote character
                        if (lineText[pos] == '"')
                        {
                            // Found one
                            pos++;

                            // If two quotes together, keep one
                            // Otherwise, indicates end of value
                            if (pos >= lineText.Length || lineText[pos] != '"')
                            {
                                pos--;
                                break;
                            }
                        }
                        pos++;
                    }
                    value = lineText.Substring(start, pos - start);
                    value = value.Replace("\"\"", "\"");
                }
                else
                {
                    // Parse unquoted value
                    int start = pos;
                    while (pos < lineText.Length && lineText[pos] != ',')
                    {
                        pos++;
                    }
                    value = lineText.Substring(start, pos - start);
                }

                // Add field to list
                if (rows < row.Count)
                {
                    row[rows] = value;
                }
                else
                {
                    row.Add(value);
                }
                rows++;

                // Eat up to and including next comma
                while (pos < lineText.Length && lineText[pos] != ',')
                {
                    pos++;
                }
                if (pos < lineText.Length)
                {
                    pos++;
                }
            }

            // Delete any unused items
            while (row.Count > rows)
            {
                row.RemoveAt(rows);
            }

            if (row.Count == 0)
            {
                return null;
            }

            return row;
        }
    }
}