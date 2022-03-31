using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDFtoBase64
{
    class Base64Encoder
    {
        //Encode pdf
        public static void Encode(string inFileName, string outFileName)
        {
            System.Security.Cryptography.ICryptoTransform transform = new System.Security.Cryptography.ToBase64Transform();
            using(System.IO.FileStream inFile = System.IO.File.OpenRead(inFileName),
                                      outFile = System.IO.File.Create(outFileName))
            using (System.Security.Cryptography.CryptoStream cryptStream = new System.Security.Cryptography.CryptoStream(outFile, transform, System.Security.Cryptography.CryptoStreamMode.Write))
            {
                // used a 4k buffer, change if needed
                byte[] buffer = new byte[4096];
                int bytesRead;

                while ((bytesRead = inFile.Read(buffer, 0, buffer.Length)) > 0)
                    cryptStream.Write(buffer, 0, bytesRead);

                cryptStream.FlushFinalBlock();
            }
        }

        //Decode from base 64 to pdf
        public static void Decode(string inFileName, string outFileName)
        {
            System.Security.Cryptography.ICryptoTransform transform = new System.Security.Cryptography.FromBase64Transform();
            using (System.IO.FileStream inFile = System.IO.File.OpenRead(inFileName),
                                      outFile = System.IO.File.Create(outFileName))
            using (System.Security.Cryptography.CryptoStream cryptStream = new System.Security.Cryptography.CryptoStream(inFile, transform, System.Security.Cryptography.CryptoStreamMode.Read))
            {
                byte[] buffer = new byte[4096];
                int bytesRead;

                while ((bytesRead = cryptStream.Read(buffer, 0, buffer.Length)) > 0)
                    outFile.Write(buffer, 0, bytesRead);

                outFile.Flush();
            }
        }


        // Another option to Encode
        // The crytostream gives identical output using less memory in a bigger file
        /* public static void MemoryEncode(string inFileName, string outFileName)
           {      
               byte[] bytes = System.IO.File.ReadAllBytes(inFileName);
               System.IO.File.WriteAllText(outFileName, System.Convert.ToBase64String(bytes));
           }
         */

        static void Main(string[] args)
        {
            string inFile = @"C:\TEMP\Base64\ORIGINAL.pdf";
            string exitPdf = @"C:\TEMP\Base64\transform.pdf";

            string entry = inFile;                                            // path in  
            string outFile = inFile.Replace(".pdf", ".txt");                  // path out            
           
            Encode(entry, outFile);
            Decode(outFile, exitPdf);

            //MemoryEncode(entry, outFile);
        }
    }
}
