//------------------------------------------------------------------------------
// <auto-generated>
//     Ezt a kódot eszköz generálta.
//     Futásidejű verzió:4.0.30319.42000
//
//     Ennek a fájlnak a módosítása helytelen viselkedést eredményezhet, és elvész, ha
//     a kódot újragenerálják.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.6.1055.0.
// 

    ///Alkalmazás alapbeállításait tartalmazza

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute("TDConfiguration", Namespace ="", IsNullable=false)]
public partial class TDConfiguration {
    
    private string endpointAddressField; //Szerver URL
    
    private int timerIntervalField; //Időzítés
    
    private string userIDField; //Felhasználó neve
    
    /// <remarks/>
    public string EndpointAddress {
        get {
            return this.endpointAddressField;
        }
        set {
            this.endpointAddressField = value;
        }
    }
    
    /// <remarks/>
    public int TimerInterval {
        get {
            return this.timerIntervalField;
        }
        set {
            this.timerIntervalField = value;
        }
    }
    
    /// <remarks/>
    public string UserID {
        get {
            return this.userIDField;
        }
        set {
            this.userIDField = value;
        }
    }
}