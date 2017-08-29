using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SansPapier.Variation.Portail.Utilitaires
{
    namespace MultipartFormData
    {
        class MultiPartFormDataBoundary
        {
            /// <summary>
            /// Creates a multipart/form-data boundary.
            /// </summary>
            /// <returns>
            /// A dynamically generated form boundary for use in posting multipart/form-data requests.
            /// </returns>
            public static string CreateFormDataBoundary()
            {
                return "---------------------------" + DateTime.Now.Ticks.ToString("x");
            }
        }
    }
}
