using System;
using System.IO;
using System.Text;
using System.Web;

namespace SansPapier.Variation.Portail.Utilitaires
{
    namespace MultipartFormData
    {
        /// <summary>
        /// Helper methods for <see cref="System.Web.HttpPostedFile"/>.
        /// </summary>
        class HttpPostedFileHelper
        {
            /// <summary>
            /// Template for a file item in multipart/form-data format.
            /// </summary>
            public const string HeaderTemplate = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n";

            /// <summary>
            /// Writes a file to a stream in multipart/form-data format.
            /// </summary>
            /// <param name="file">The file that should be written.</param>
            /// <param name="stream">The stream to which the file should be written.</param>
            /// <param name="mimeBoundary">The MIME multipart form boundary string.</param>
            /// <param name="mimeType">The MIME type of the file.</param>
            /// <param name="formKey">The name of the form parameter corresponding to the file upload.</param>
            /// <exception cref="System.ArgumentNullException">
            /// Thrown if any parameter is <see langword="null" />.
            /// </exception>
            /// <exception cref="System.ArgumentException">
            /// Thrown if <paramref name="mimeBoundary" />, <paramref name="mimeType" />,
            /// or <paramref name="formKey" /> is empty.
            /// </exception>
            /// <exception cref="System.IO.FileNotFoundException">
            /// Thrown if <paramref name="file" /> does not exist.
            /// </exception>
            public static void WriteMultipartFormData(HttpPostedFile file, Stream stream, string mimeBoundary, string mimeType, string formKey)
            {
                if (file == null)
                {
                    throw new ArgumentNullException("file");
                }
                if (file.InputStream == null)
                {
                    throw new FileNotFoundException("Unable to find file to write to stream.", file.FileName);
                }
                if (stream == null)
                {
                    throw new ArgumentNullException("stream");
                }
                if (mimeBoundary == null)
                {
                    throw new ArgumentNullException("mimeBoundary");
                }
                if (mimeBoundary.Length == 0)
                {
                    throw new ArgumentException("MIME boundary may not be empty.", "mimeBoundary");
                }
                if (mimeType == null)
                {
                    throw new ArgumentNullException("mimeType");
                }
                if (mimeType.Length == 0)
                {
                    throw new ArgumentException("MIME type may not be empty.", "mimeType");
                }
                if (formKey == null)
                {
                    throw new ArgumentNullException("formKey");
                }
                if (formKey.Length == 0)
                {
                    throw new ArgumentException("Form key may not be empty.", "formKey");
                }
                string header = String.Format(HeaderTemplate, mimeBoundary, formKey, file.FileName, mimeType);
                byte[] headerbytes = Encoding.UTF8.GetBytes(header);
                stream.Write(headerbytes, 0, headerbytes.Length);
                
                using (Stream fileStream = file.InputStream)
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        stream.Write(buffer, 0, bytesRead);
                    }
                    fileStream.Close();
                }
                byte[] newlineBytes = Encoding.UTF8.GetBytes("\r\n");
                stream.Write(newlineBytes, 0, newlineBytes.Length);
            }
        }
    }
}
