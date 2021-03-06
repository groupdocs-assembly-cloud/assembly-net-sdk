﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="GroupDocs" file="AssemblyApiTests.cs">
//   Copyright (c) 2018 GroupDocs.Assembly for Cloud
// </copyright>
// <summary>
//   Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GroupDocs.Assembly.Cloud.Sdk.Test.Api
{
    using System.IO;

    using GroupDocs.Assembly.Cloud.Sdk.Model;
    using GroupDocs.Assembly.Cloud.Sdk.Model.Requests;
    using GroupDocs.Assembly.Cloud.Sdk.Test.Base;

    using NUnit.Framework;

    /// <summary>
    /// Class for testing Assembly for Cloud
    /// </summary>
    [TestFixture]
    public class AssemblyApiTests : BaseTestContext
    {
        /// <summary>
        /// Assemble document test
        /// </summary>
        [Test]
        public void TestPostAssembleDocument()
        {
            var fileName = "TableFeatures.odt";
            var dataName = "TableData.json";
            var data = File.ReadAllText(Path.Combine(BaseTestContext.LocalTestDataFolder, dataName));
            var saveOptions = new AssembleOptions() { SaveFormat = "pdf", ReportData = data, TemplateFileInfo = new TemplateFileInfo { FilePath = Path.Combine(BaseTestContext.RemoteBaseTestDataFolder, "GroupDocs.Assembly", fileName) } };
            this.UploadFileToStorage(Path.Combine(BaseTestContext.RemoteBaseTestDataFolder, "GroupDocs.Assembly", fileName), null, null, File.ReadAllBytes(Path.Combine(BaseTestContext.LocalTestDataFolder, fileName)));

            var request = new AssembleDocumentRequest(saveOptions);
            var result = this.AssemblyApi.AssembleDocument(request);

            Assert.IsTrue(result.Length > 0, "Error occurred while assemble document");
        }

        /// <summary>
        /// Assemble document test
        /// </summary>
        [Test]
        public void TestPostAssembleDocumentThrows()
        {
            var fileName = "TableFeatures.odt";
            var saveOptions = new AssembleOptions() { SaveFormat = "pdf", ReportData = string.Empty, TemplateFileInfo = new TemplateFileInfo { FilePath = Path.Combine(BaseTestContext.RemoteBaseTestDataFolder, "GroupDocs.Assembly", fileName) } };
            this.UploadFileToStorage(Path.Combine(BaseTestContext.RemoteBaseTestDataFolder, "GroupDocs.Assembly", fileName), null, null, File.ReadAllBytes(Path.Combine(BaseTestContext.LocalTestDataFolder, fileName)));

            var request = new AssembleDocumentRequest(saveOptions);
            try
            {
                this.AssemblyApi.AssembleDocument(request);
            }
            catch (ApiException e)
            {
                Assert.AreEqual(400, e.ErrorCode);
                Assert.AreEqual("assembleOptions.ReportData is required in the request body.", e.Message);
            }
        }
    }
}