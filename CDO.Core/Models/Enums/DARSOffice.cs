namespace CDO.Core.Models.Enums;

public class DARSOffice {
    private DARSOffice(string address) {
        Address = address;
    }

    public string Address { get; private set; }

    public static DARSOffice NEFO {
        get { return new DARSOffice("11711 N I-35 #170 San Antonio, TX 78233"); }
    }

    public static DARSOffice NFO {
        get { return new DARSOffice("9725 Datapoint Dr #200 San Antonio, TX 78229"); }
    }

    public static DARSOffice WFO {
        get { return new DARSOffice("8518 Culebra Road Ste. 105 San Antonio, TX 78251"); }
    }

    public static DARSOffice SFO {
        get { return new DARSOffice("6723 S Flores St #100, San Antonio, TX 78221"); }
    }

    public static DARSOffice EHFO {
        get { return new DARSOffice("4535 E Houston St San Antonio, TX 78220"); }
    }

    public static DARSOffice NBFO {
        get { return new DARSOffice("183 S IH 35, New Braunfels TX 78130"); }
    }

    public static DARSOffice SEFO {
        get { return new DARSOffice("1411 E Court St Seguin, TX 78155"); }
    }

    public override string ToString() {
        return Address;
    }

    public static DARSOffice[] AllItems() {
        return [
            DARSOffice.NEFO,
            DARSOffice.NFO,
            DARSOffice.WFO,
            DARSOffice.SFO,
            DARSOffice.EHFO,
            DARSOffice.NBFO,
            DARSOffice.SEFO
            ];
    }
}
