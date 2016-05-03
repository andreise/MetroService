﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace MetroModel.GraphServiceReference {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="GraphServiceSoap", Namespace="http://andreysergeev.org/graphservice")]
    public partial class GraphService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetVertexDeletingSequenceForConnectedGraphOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public GraphService() {
            this.Url = global::MetroModel.Properties.Settings.Default.MetroModel_Implementation_GraphServiceReference_GraphService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GetVertexDeletingSequenceForConnectedGraphCompletedEventHandler GetVertexDeletingSequenceForConnectedGraphCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://andreysergeev.org/graphservice/GetVertexDeletingSequenceForConnectedGraph", RequestNamespace="http://andreysergeev.org/graphservice", ResponseNamespace="http://andreysergeev.org/graphservice", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool GetVertexDeletingSequenceForConnectedGraph(string inputXml, out int[] result, out string errorMessage) {
            object[] results = this.Invoke("GetVertexDeletingSequenceForConnectedGraph", new object[] {
                        inputXml});
            result = ((int[])(results[1]));
            errorMessage = ((string)(results[2]));
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void GetVertexDeletingSequenceForConnectedGraphAsync(string inputXml) {
            this.GetVertexDeletingSequenceForConnectedGraphAsync(inputXml, null);
        }
        
        /// <remarks/>
        public void GetVertexDeletingSequenceForConnectedGraphAsync(string inputXml, object userState) {
            if ((this.GetVertexDeletingSequenceForConnectedGraphOperationCompleted == null)) {
                this.GetVertexDeletingSequenceForConnectedGraphOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetVertexDeletingSequenceForConnectedGraphOperationCompleted);
            }
            this.InvokeAsync("GetVertexDeletingSequenceForConnectedGraph", new object[] {
                        inputXml}, this.GetVertexDeletingSequenceForConnectedGraphOperationCompleted, userState);
        }
        
        private void OnGetVertexDeletingSequenceForConnectedGraphOperationCompleted(object arg) {
            if ((this.GetVertexDeletingSequenceForConnectedGraphCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetVertexDeletingSequenceForConnectedGraphCompleted(this, new GetVertexDeletingSequenceForConnectedGraphCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    public delegate void GetVertexDeletingSequenceForConnectedGraphCompletedEventHandler(object sender, GetVertexDeletingSequenceForConnectedGraphCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetVertexDeletingSequenceForConnectedGraphCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetVertexDeletingSequenceForConnectedGraphCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public int[] result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int[])(this.results[1]));
            }
        }
        
        /// <remarks/>
        public string errorMessage {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[2]));
            }
        }
    }
}

#pragma warning restore 1591