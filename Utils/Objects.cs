
using DataWinLoad.Utils;
using UndertaleModLib.Models;
using UndertaleModLib;

namespace DataWinLoad.Utils {
    internal class Objects {
        public static UndertaleGameObject? AddObject(UndertaleData data, Types.Object objectRef) {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Adding Object \"{objectRef.name}\"");

            var name = data.Strings.MakeString(objectRef.name);

            UndertaleGameObject? parentID = null;
            if (objectRef.parent != null)
                parentID = data.GameObjects.ByName(objectRef.parent);
            

            UndertaleSprite? sprite = null;
            if (objectRef.spriteName != null)
                sprite = data.Sprites.ByName(objectRef.spriteName);

            
            UndertaleGameObject ObjectInst = data.GameObjects.ByName(objectRef.name);
            bool exists = ObjectInst != null;
            ObjectInst ??= new();

            ObjectInst.Name = name;
            if (parentID != null)
                ObjectInst.ParentId = parentID;
            if (sprite != null)
                ObjectInst.Sprite = sprite;
            if (objectRef.visible != null)
                ObjectInst.Visible = (bool) objectRef.visible;
            if (objectRef.persistent != null)
                ObjectInst.Persistent = (bool) objectRef.persistent;
            if (objectRef.solid != null)
                ObjectInst.Solid = (bool) objectRef.solid;


            if (!exists)
                data.GameObjects.Add(ObjectInst);

            return ObjectInst;
        }

        public static UndertaleGameObject? AddObjectEvents(UndertaleData data, Types.Object objectRef) {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Adding Object Events of \"{objectRef.name}\"");


            UndertaleGameObject? ObjectInst = data.GameObjects.ByName(objectRef.name);

            if (ObjectInst == null) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Missing Object");
                return null;
            }


            AddEvents(data, ObjectInst, objectRef);


            return ObjectInst;
        }

        public static void AddEvents(UndertaleData data, UndertaleGameObject objectInst, Types.Object objectRef) {
            if (objectRef.events == null) {
                return;
            }
            foreach (var ev in objectRef.events) {
                var tp = Types.GetEventType(data, ev.eventType, ev.subtypeID);
                EventType evType = tp.Item1;
                uint evSubtypeID = tp.Item2;
                AddAction(objectInst, evType, evSubtypeID, ev.scriptName, data);
            }
        }

        public static void AddAction(UndertaleGameObject objectInst, EventType eventType, uint eventSubtype, string scriptName, UndertaleData data) {

            // Find or create the event
            var eventList = objectInst.Events[(int)eventType];
            UndertaleGameObject.Event? targetEvent = null;

            foreach (var ev in eventList) {
                if (ev.EventSubtype == eventSubtype)
                    targetEvent = ev;
            }

            if (targetEvent == null) {
                targetEvent = new UndertaleGameObject.Event {
                    EventSubtype = eventSubtype
                };

                eventList.Add(targetEvent);
            }

            // Create the event action
            UndertaleCode codeObject = data.Code.ByName(scriptName);
            UndertaleString nameStr = data.Strings.MakeString(scriptName);
            var eventAction = new UndertaleGameObject.EventAction {
                ActionName = nameStr,
                CodeId = codeObject
            };

            // Add the action to the event (and remove old one)
            try {
                targetEvent.Actions.RemoveAt((int)eventSubtype);
            } catch {}

            targetEvent.Actions.Add(eventAction);
        }
    }
}
