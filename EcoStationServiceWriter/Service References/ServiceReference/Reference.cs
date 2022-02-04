﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EcoStationServiceWriter.ServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://ecostationservice", ConfigurationName="ServiceReference.EcoStationService")]
    public interface EcoStationService {
        
        // CODEGEN: Generating message contract since element name message from namespace http://ecostationservice is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        EcoStationServiceWriter.ServiceReference.addEventResponse addEvent(EcoStationServiceWriter.ServiceReference.addEventRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<EcoStationServiceWriter.ServiceReference.addEventResponse> addEventAsync(EcoStationServiceWriter.ServiceReference.addEventRequest request);
        
        // CODEGEN: Generating message contract since element name status from namespace http://ecostationservice is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        EcoStationServiceWriter.ServiceReference.addResultResponse addResult(EcoStationServiceWriter.ServiceReference.addResultRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<EcoStationServiceWriter.ServiceReference.addResultResponse> addResultAsync(EcoStationServiceWriter.ServiceReference.addResultRequest request);
        
        // CODEGEN: Generating message contract since element name status from namespace http://ecostationservice is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        EcoStationServiceWriter.ServiceReference.addEmptyResultResponse addEmptyResult(EcoStationServiceWriter.ServiceReference.addEmptyResultRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<EcoStationServiceWriter.ServiceReference.addEmptyResultResponse> addEmptyResultAsync(EcoStationServiceWriter.ServiceReference.addEmptyResultRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="addProbeReturn")]
        int addProbe(System.DateTime probeDate, int pointCode, int period, bool emergency);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="addProbeReturn")]
        System.Threading.Tasks.Task<int> addProbeAsync(System.DateTime probeDate, int pointCode, int period, bool emergency);
        
        // CODEGEN: Generating message contract since element name pointName from namespace http://ecostationservice is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        EcoStationServiceWriter.ServiceReference.addControlPointResponse addControlPoint(EcoStationServiceWriter.ServiceReference.addControlPointRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<EcoStationServiceWriter.ServiceReference.addControlPointResponse> addControlPointAsync(EcoStationServiceWriter.ServiceReference.addControlPointRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="isPointExistsReturn")]
        bool isPointExists(int pointCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="isPointExistsReturn")]
        System.Threading.Tasks.Task<bool> isPointExistsAsync(int pointCode);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class addEventRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="addEvent", Namespace="http://ecostationservice", Order=0)]
        public EcoStationServiceWriter.ServiceReference.addEventRequestBody Body;
        
        public addEventRequest() {
        }
        
        public addEventRequest(EcoStationServiceWriter.ServiceReference.addEventRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://ecostationservice")]
    public partial class addEventRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int pointCode;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public System.DateTime eventDate;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public int paramCode;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=3)]
        public int eventType;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string message;
        
        public addEventRequestBody() {
        }
        
        public addEventRequestBody(int pointCode, System.DateTime eventDate, int paramCode, int eventType, string message) {
            this.pointCode = pointCode;
            this.eventDate = eventDate;
            this.paramCode = paramCode;
            this.eventType = eventType;
            this.message = message;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class addEventResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="addEventResponse", Namespace="http://ecostationservice", Order=0)]
        public EcoStationServiceWriter.ServiceReference.addEventResponseBody Body;
        
        public addEventResponse() {
        }
        
        public addEventResponse(EcoStationServiceWriter.ServiceReference.addEventResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class addEventResponseBody {
        
        public addEventResponseBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class addResultRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="addResult", Namespace="http://ecostationservice", Order=0)]
        public EcoStationServiceWriter.ServiceReference.addResultRequestBody Body;
        
        public addResultRequest() {
        }
        
        public addResultRequest(EcoStationServiceWriter.ServiceReference.addResultRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://ecostationservice")]
    public partial class addResultRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int probeID;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public int paramCode;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public double value;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=3)]
        public bool alarm;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string status;
        
        public addResultRequestBody() {
        }
        
        public addResultRequestBody(int probeID, int paramCode, double value, bool alarm, string status) {
            this.probeID = probeID;
            this.paramCode = paramCode;
            this.value = value;
            this.alarm = alarm;
            this.status = status;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class addResultResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="addResultResponse", Namespace="http://ecostationservice", Order=0)]
        public EcoStationServiceWriter.ServiceReference.addResultResponseBody Body;
        
        public addResultResponse() {
        }
        
        public addResultResponse(EcoStationServiceWriter.ServiceReference.addResultResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://ecostationservice")]
    public partial class addResultResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public bool addResultReturn;
        
        public addResultResponseBody() {
        }
        
        public addResultResponseBody(bool addResultReturn) {
            this.addResultReturn = addResultReturn;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class addEmptyResultRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="addEmptyResult", Namespace="http://ecostationservice", Order=0)]
        public EcoStationServiceWriter.ServiceReference.addEmptyResultRequestBody Body;
        
        public addEmptyResultRequest() {
        }
        
        public addEmptyResultRequest(EcoStationServiceWriter.ServiceReference.addEmptyResultRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://ecostationservice")]
    public partial class addEmptyResultRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int probeID;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public int paramCode;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public bool alarm;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string status;
        
        public addEmptyResultRequestBody() {
        }
        
        public addEmptyResultRequestBody(int probeID, int paramCode, bool alarm, string status) {
            this.probeID = probeID;
            this.paramCode = paramCode;
            this.alarm = alarm;
            this.status = status;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class addEmptyResultResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="addEmptyResultResponse", Namespace="http://ecostationservice", Order=0)]
        public EcoStationServiceWriter.ServiceReference.addEmptyResultResponseBody Body;
        
        public addEmptyResultResponse() {
        }
        
        public addEmptyResultResponse(EcoStationServiceWriter.ServiceReference.addEmptyResultResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://ecostationservice")]
    public partial class addEmptyResultResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public bool addEmptyResultReturn;
        
        public addEmptyResultResponseBody() {
        }
        
        public addEmptyResultResponseBody(bool addEmptyResultReturn) {
            this.addEmptyResultReturn = addEmptyResultReturn;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class addControlPointRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="addControlPoint", Namespace="http://ecostationservice", Order=0)]
        public EcoStationServiceWriter.ServiceReference.addControlPointRequestBody Body;
        
        public addControlPointRequest() {
        }
        
        public addControlPointRequest(EcoStationServiceWriter.ServiceReference.addControlPointRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://ecostationservice")]
    public partial class addControlPointRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int pointCode;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string pointName;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string address;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=3)]
        public double lon;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=4)]
        public double lat;
        
        public addControlPointRequestBody() {
        }
        
        public addControlPointRequestBody(int pointCode, string pointName, string address, double lon, double lat) {
            this.pointCode = pointCode;
            this.pointName = pointName;
            this.address = address;
            this.lon = lon;
            this.lat = lat;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class addControlPointResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="addControlPointResponse", Namespace="http://ecostationservice", Order=0)]
        public EcoStationServiceWriter.ServiceReference.addControlPointResponseBody Body;
        
        public addControlPointResponse() {
        }
        
        public addControlPointResponse(EcoStationServiceWriter.ServiceReference.addControlPointResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class addControlPointResponseBody {
        
        public addControlPointResponseBody() {
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface EcoStationServiceChannel : EcoStationServiceWriter.ServiceReference.EcoStationService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class EcoStationServiceClient : System.ServiceModel.ClientBase<EcoStationServiceWriter.ServiceReference.EcoStationService>, EcoStationServiceWriter.ServiceReference.EcoStationService {
        
        public EcoStationServiceClient() {
        }
        
        public EcoStationServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public EcoStationServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EcoStationServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EcoStationServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        EcoStationServiceWriter.ServiceReference.addEventResponse EcoStationServiceWriter.ServiceReference.EcoStationService.addEvent(EcoStationServiceWriter.ServiceReference.addEventRequest request) {
            return base.Channel.addEvent(request);
        }
        
        public void addEvent(int pointCode, System.DateTime eventDate, int paramCode, int eventType, string message) {
            EcoStationServiceWriter.ServiceReference.addEventRequest inValue = new EcoStationServiceWriter.ServiceReference.addEventRequest();
            inValue.Body = new EcoStationServiceWriter.ServiceReference.addEventRequestBody();
            inValue.Body.pointCode = pointCode;
            inValue.Body.eventDate = eventDate;
            inValue.Body.paramCode = paramCode;
            inValue.Body.eventType = eventType;
            inValue.Body.message = message;
            EcoStationServiceWriter.ServiceReference.addEventResponse retVal = ((EcoStationServiceWriter.ServiceReference.EcoStationService)(this)).addEvent(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<EcoStationServiceWriter.ServiceReference.addEventResponse> EcoStationServiceWriter.ServiceReference.EcoStationService.addEventAsync(EcoStationServiceWriter.ServiceReference.addEventRequest request) {
            return base.Channel.addEventAsync(request);
        }
        
        public System.Threading.Tasks.Task<EcoStationServiceWriter.ServiceReference.addEventResponse> addEventAsync(int pointCode, System.DateTime eventDate, int paramCode, int eventType, string message) {
            EcoStationServiceWriter.ServiceReference.addEventRequest inValue = new EcoStationServiceWriter.ServiceReference.addEventRequest();
            inValue.Body = new EcoStationServiceWriter.ServiceReference.addEventRequestBody();
            inValue.Body.pointCode = pointCode;
            inValue.Body.eventDate = eventDate;
            inValue.Body.paramCode = paramCode;
            inValue.Body.eventType = eventType;
            inValue.Body.message = message;
            return ((EcoStationServiceWriter.ServiceReference.EcoStationService)(this)).addEventAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        EcoStationServiceWriter.ServiceReference.addResultResponse EcoStationServiceWriter.ServiceReference.EcoStationService.addResult(EcoStationServiceWriter.ServiceReference.addResultRequest request) {
            return base.Channel.addResult(request);
        }
        
        public bool addResult(int probeID, int paramCode, double value, bool alarm, string status) {
            EcoStationServiceWriter.ServiceReference.addResultRequest inValue = new EcoStationServiceWriter.ServiceReference.addResultRequest();
            inValue.Body = new EcoStationServiceWriter.ServiceReference.addResultRequestBody();
            inValue.Body.probeID = probeID;
            inValue.Body.paramCode = paramCode;
            inValue.Body.value = value;
            inValue.Body.alarm = alarm;
            inValue.Body.status = status;
            EcoStationServiceWriter.ServiceReference.addResultResponse retVal = ((EcoStationServiceWriter.ServiceReference.EcoStationService)(this)).addResult(inValue);
            return retVal.Body.addResultReturn;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<EcoStationServiceWriter.ServiceReference.addResultResponse> EcoStationServiceWriter.ServiceReference.EcoStationService.addResultAsync(EcoStationServiceWriter.ServiceReference.addResultRequest request) {
            return base.Channel.addResultAsync(request);
        }
        
        public System.Threading.Tasks.Task<EcoStationServiceWriter.ServiceReference.addResultResponse> addResultAsync(int probeID, int paramCode, double value, bool alarm, string status) {
            EcoStationServiceWriter.ServiceReference.addResultRequest inValue = new EcoStationServiceWriter.ServiceReference.addResultRequest();
            inValue.Body = new EcoStationServiceWriter.ServiceReference.addResultRequestBody();
            inValue.Body.probeID = probeID;
            inValue.Body.paramCode = paramCode;
            inValue.Body.value = value;
            inValue.Body.alarm = alarm;
            inValue.Body.status = status;
            return ((EcoStationServiceWriter.ServiceReference.EcoStationService)(this)).addResultAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        EcoStationServiceWriter.ServiceReference.addEmptyResultResponse EcoStationServiceWriter.ServiceReference.EcoStationService.addEmptyResult(EcoStationServiceWriter.ServiceReference.addEmptyResultRequest request) {
            return base.Channel.addEmptyResult(request);
        }
        
        public bool addEmptyResult(int probeID, int paramCode, bool alarm, string status) {
            EcoStationServiceWriter.ServiceReference.addEmptyResultRequest inValue = new EcoStationServiceWriter.ServiceReference.addEmptyResultRequest();
            inValue.Body = new EcoStationServiceWriter.ServiceReference.addEmptyResultRequestBody();
            inValue.Body.probeID = probeID;
            inValue.Body.paramCode = paramCode;
            inValue.Body.alarm = alarm;
            inValue.Body.status = status;
            EcoStationServiceWriter.ServiceReference.addEmptyResultResponse retVal = ((EcoStationServiceWriter.ServiceReference.EcoStationService)(this)).addEmptyResult(inValue);
            return retVal.Body.addEmptyResultReturn;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<EcoStationServiceWriter.ServiceReference.addEmptyResultResponse> EcoStationServiceWriter.ServiceReference.EcoStationService.addEmptyResultAsync(EcoStationServiceWriter.ServiceReference.addEmptyResultRequest request) {
            return base.Channel.addEmptyResultAsync(request);
        }
        
        public System.Threading.Tasks.Task<EcoStationServiceWriter.ServiceReference.addEmptyResultResponse> addEmptyResultAsync(int probeID, int paramCode, bool alarm, string status) {
            EcoStationServiceWriter.ServiceReference.addEmptyResultRequest inValue = new EcoStationServiceWriter.ServiceReference.addEmptyResultRequest();
            inValue.Body = new EcoStationServiceWriter.ServiceReference.addEmptyResultRequestBody();
            inValue.Body.probeID = probeID;
            inValue.Body.paramCode = paramCode;
            inValue.Body.alarm = alarm;
            inValue.Body.status = status;
            return ((EcoStationServiceWriter.ServiceReference.EcoStationService)(this)).addEmptyResultAsync(inValue);
        }
        
        public int addProbe(System.DateTime probeDate, int pointCode, int period, bool emergency) {
            return base.Channel.addProbe(probeDate, pointCode, period, emergency);
        }
        
        public System.Threading.Tasks.Task<int> addProbeAsync(System.DateTime probeDate, int pointCode, int period, bool emergency) {
            return base.Channel.addProbeAsync(probeDate, pointCode, period, emergency);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        EcoStationServiceWriter.ServiceReference.addControlPointResponse EcoStationServiceWriter.ServiceReference.EcoStationService.addControlPoint(EcoStationServiceWriter.ServiceReference.addControlPointRequest request) {
            return base.Channel.addControlPoint(request);
        }
        
        public void addControlPoint(int pointCode, string pointName, string address, double lon, double lat) {
            EcoStationServiceWriter.ServiceReference.addControlPointRequest inValue = new EcoStationServiceWriter.ServiceReference.addControlPointRequest();
            inValue.Body = new EcoStationServiceWriter.ServiceReference.addControlPointRequestBody();
            inValue.Body.pointCode = pointCode;
            inValue.Body.pointName = pointName;
            inValue.Body.address = address;
            inValue.Body.lon = lon;
            inValue.Body.lat = lat;
            EcoStationServiceWriter.ServiceReference.addControlPointResponse retVal = ((EcoStationServiceWriter.ServiceReference.EcoStationService)(this)).addControlPoint(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<EcoStationServiceWriter.ServiceReference.addControlPointResponse> EcoStationServiceWriter.ServiceReference.EcoStationService.addControlPointAsync(EcoStationServiceWriter.ServiceReference.addControlPointRequest request) {
            return base.Channel.addControlPointAsync(request);
        }
        
        public System.Threading.Tasks.Task<EcoStationServiceWriter.ServiceReference.addControlPointResponse> addControlPointAsync(int pointCode, string pointName, string address, double lon, double lat) {
            EcoStationServiceWriter.ServiceReference.addControlPointRequest inValue = new EcoStationServiceWriter.ServiceReference.addControlPointRequest();
            inValue.Body = new EcoStationServiceWriter.ServiceReference.addControlPointRequestBody();
            inValue.Body.pointCode = pointCode;
            inValue.Body.pointName = pointName;
            inValue.Body.address = address;
            inValue.Body.lon = lon;
            inValue.Body.lat = lat;
            return ((EcoStationServiceWriter.ServiceReference.EcoStationService)(this)).addControlPointAsync(inValue);
        }
        
        public bool isPointExists(int pointCode) {
            return base.Channel.isPointExists(pointCode);
        }
        
        public System.Threading.Tasks.Task<bool> isPointExistsAsync(int pointCode) {
            return base.Channel.isPointExistsAsync(pointCode);
        }
    }
}