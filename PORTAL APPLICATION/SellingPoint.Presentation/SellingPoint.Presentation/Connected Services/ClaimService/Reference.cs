﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SellingPoint.Presentation.ClaimService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="OCLAIMCOUNT-NUMBER-OUT", Namespace="http://xmlns.oracle.com/orawsv/WEBSERVICEUSER/GETCLAIMCOUNT")]
    [System.SerializableAttribute()]
    public partial class OCLAIMCOUNTNUMBEROUT : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://xmlns.oracle.com/orawsv/WEBSERVICEUSER/GETCLAIMCOUNT", ConfigurationName="ClaimService.GETCLAIMCOUNTPortType")]
    public interface GETCLAIMCOUNTPortType {
        
        // CODEGEN: Generating message contract since the wrapper name (GETCLAIMCOUNTInput) of message GETCLAIMCOUNTRequest does not match the default value (GETCLAIMCOUNT)
        [System.ServiceModel.OperationContractAttribute(Action="GETCLAIMCOUNT", ReplyAction="*")]
        SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTResponse GETCLAIMCOUNT(SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="GETCLAIMCOUNT", ReplyAction="*")]
        System.Threading.Tasks.Task<SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTResponse> GETCLAIMCOUNTAsync(SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GETCLAIMCOUNTInput", WrapperNamespace="http://xmlns.oracle.com/orawsv/WEBSERVICEUSER/GETCLAIMCOUNT", IsWrapped=true)]
    public partial class GETCLAIMCOUNTRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="OCLAIMCOUNT-NUMBER-OUT", Namespace="http://xmlns.oracle.com/orawsv/WEBSERVICEUSER/GETCLAIMCOUNT", Order=0)]
        public SellingPoint.Presentation.ClaimService.OCLAIMCOUNTNUMBEROUT OCLAIMCOUNTNUMBEROUT;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PPOLICYNO-VARCHAR2-IN", Namespace="http://xmlns.oracle.com/orawsv/WEBSERVICEUSER/GETCLAIMCOUNT", Order=1)]
        public string PPOLICYNOVARCHAR2IN;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PRENEWALCOUNT-NUMBER-IN", Namespace="http://xmlns.oracle.com/orawsv/WEBSERVICEUSER/GETCLAIMCOUNT", Order=2)]
        public double PRENEWALCOUNTNUMBERIN;
        
        public GETCLAIMCOUNTRequest() {
        }
        
        public GETCLAIMCOUNTRequest(SellingPoint.Presentation.ClaimService.OCLAIMCOUNTNUMBEROUT OCLAIMCOUNTNUMBEROUT, string PPOLICYNOVARCHAR2IN, double PRENEWALCOUNTNUMBERIN) {
            this.OCLAIMCOUNTNUMBEROUT = OCLAIMCOUNTNUMBEROUT;
            this.PPOLICYNOVARCHAR2IN = PPOLICYNOVARCHAR2IN;
            this.PRENEWALCOUNTNUMBERIN = PRENEWALCOUNTNUMBERIN;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GETCLAIMCOUNTOutput", WrapperNamespace="http://xmlns.oracle.com/orawsv/WEBSERVICEUSER/GETCLAIMCOUNT", IsWrapped=true)]
    public partial class GETCLAIMCOUNTResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://xmlns.oracle.com/orawsv/WEBSERVICEUSER/GETCLAIMCOUNT", Order=0)]
        public double OCLAIMCOUNT;
        
        public GETCLAIMCOUNTResponse() {
        }
        
        public GETCLAIMCOUNTResponse(double OCLAIMCOUNT) {
            this.OCLAIMCOUNT = OCLAIMCOUNT;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface GETCLAIMCOUNTPortTypeChannel : SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTPortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GETCLAIMCOUNTPortTypeClient : System.ServiceModel.ClientBase<SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTPortType>, SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTPortType {
        
        public GETCLAIMCOUNTPortTypeClient() {
        }
        
        public GETCLAIMCOUNTPortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public GETCLAIMCOUNTPortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GETCLAIMCOUNTPortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GETCLAIMCOUNTPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTResponse SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTPortType.GETCLAIMCOUNT(SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTRequest request) {
            return base.Channel.GETCLAIMCOUNT(request);
        }
        
        public double GETCLAIMCOUNT(SellingPoint.Presentation.ClaimService.OCLAIMCOUNTNUMBEROUT OCLAIMCOUNTNUMBEROUT, string PPOLICYNOVARCHAR2IN, double PRENEWALCOUNTNUMBERIN) {
            SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTRequest inValue = new SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTRequest();
            inValue.OCLAIMCOUNTNUMBEROUT = OCLAIMCOUNTNUMBEROUT;
            inValue.PPOLICYNOVARCHAR2IN = PPOLICYNOVARCHAR2IN;
            inValue.PRENEWALCOUNTNUMBERIN = PRENEWALCOUNTNUMBERIN;
            SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTResponse retVal = ((SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTPortType)(this)).GETCLAIMCOUNT(inValue);
            return retVal.OCLAIMCOUNT;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTResponse> SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTPortType.GETCLAIMCOUNTAsync(SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTRequest request) {
            return base.Channel.GETCLAIMCOUNTAsync(request);
        }
        
        public System.Threading.Tasks.Task<SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTResponse> GETCLAIMCOUNTAsync(SellingPoint.Presentation.ClaimService.OCLAIMCOUNTNUMBEROUT OCLAIMCOUNTNUMBEROUT, string PPOLICYNOVARCHAR2IN, double PRENEWALCOUNTNUMBERIN) {
            SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTRequest inValue = new SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTRequest();
            inValue.OCLAIMCOUNTNUMBEROUT = OCLAIMCOUNTNUMBEROUT;
            inValue.PPOLICYNOVARCHAR2IN = PPOLICYNOVARCHAR2IN;
            inValue.PRENEWALCOUNTNUMBERIN = PRENEWALCOUNTNUMBERIN;
            return ((SellingPoint.Presentation.ClaimService.GETCLAIMCOUNTPortType)(this)).GETCLAIMCOUNTAsync(inValue);
        }
    }
}