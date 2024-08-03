using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UndertaleModLib;
using UndertaleModLib.Models;

namespace DataWinLoad.Utils {
    internal class Types {

        public struct Texture {
            public string textureID { get; set; }
            public string path { get; set; }
        }
        public struct PageItem {
            public string textureID { get; set; }

            public ushort[] sourcePosition { get; set; } // X/Y
            public ushort[] sourceSize { get; set; } // Width/Height

            public ushort[] targetPosition { get; set; } // X/Y
            public ushort[] targetSize { get; set; } // Width/Height

            public ushort[] boundingSize { get; set; } // Width/Height
        }

        public struct Sprite {
            public string spriteName { get; set; }
            public uint[] size { get; set; } // Width/Height
            public int[] margin { get; set; } // Left/Right/Bottom/Top
            public List<PageItem> pageItems { get; set; }
        }

        public struct Script {
            public string name { get; set; }
            public string path { get; set; }
            public bool? append { get; set; }
            public bool? global { get; set; }
        }

        public static Tuple<EventType, uint> MkTupl(EventType evType, uint subtype) {
            return new Tuple<EventType, uint>(evType, subtype);
        }

        public static Tuple<EventType, uint> GetEventType(UndertaleData data, string eventName, string? subtypeName) {
            switch (eventName) {
                case "other":
                    return MkTupl(EventTypes[eventName], (uint)OtherSubtypes[subtypeName]);
                case "step":
                    return MkTupl(EventTypes[eventName], (uint)StepSubtypes[subtypeName]);
                case "collision":
                    UndertaleGameObject obj = data.GameObjects.ByName(subtypeName);
                    uint objId = (uint) data.GameObjects.IndexOf(obj);
                    return MkTupl(EventType.Collision, objId);
                default:
                    uint subid = 0;
                    if (subtypeName != null)
                        _ = uint.TryParse(subtypeName, out subid);
                    
                    return MkTupl(EventTypes[eventName], subid);
            }
        }
        public static Dictionary<string, EventType> EventTypes = new() {
            ["create"] = EventType.Create,
            ["collision"] = EventType.Collision,
            ["alarm"] = EventType.Alarm,
            ["draw"] = EventType.Draw,
            ["other"] = EventType.Other,
            ["step"] = EventType.Step,
        };

        public static Dictionary<string, EventSubtypeOther> OtherSubtypes = new() {
            ["roomStart"] = EventSubtypeOther.RoomStart,
            ["roomEnd"] = EventSubtypeOther.RoomEnd
        };

        public static Dictionary<string, EventSubtypeStep> StepSubtypes = new() {
            ["beginStep"] = EventSubtypeStep.BeginStep,
            ["step"] = EventSubtypeStep.Step,
            ["endStep"] = EventSubtypeStep.EndStep
        };

        public struct Event {
            public string eventType { get; set; }
            public string? subtypeID { get; set; }
            public string scriptName { get; set; }
        }

        public struct Object {
            public string name { get; set; }
            public string? parent { get; set; }
            public string? spriteName { get; set; }
            public bool? visible { get; set; }
            public bool? persistent { get; set; }
            public bool? solid { get; set; }
            public List<Event>? events { get; set; }
        }

        public struct BuildJSON {
            public string dataPath { get; set; }
            public string outputDataPath { get; set; }
            public string workingDir { get; set; }
            public List<Object> objects { get; set; }
            public List<Script> scripts { get; set; }
            public List<Sprite> sprites { get; set; }
            public List<Texture> textures { get; set; }

        }
    }
}
