namespace CDO.Core.Models.Enums {
    public class UM {
        private UM(string value) {
            Value = value;
        }

        public string Value { get; private set; }

        public static UM Service {
            get { return new UM("Service"); }
        }

        public static UM Each {
            get { return new UM("Each"); }
        }

        public override string ToString() {
            return Value;
        }

        public static UM[] AllItems() {
            return [
                UM.Service,
                UM.Each
            ];
        }
    }
}
