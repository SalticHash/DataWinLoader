
using DataWinLoad.Utils;
using UndertaleModLib.Models;
using UndertaleModLib;

namespace DataWinLoad.Utils {
    internal class Objects {
        public static UndertaleGameObject? AddObject(UndertaleData data, Types.Object objectRef) {
            Console.WriteLine($"Adding Object that with name / id {objectRef.name}");

            var name = data.Strings.MakeString(objectRef.name);

            UndertaleGameObject parentID;
            parentID = data.GameObjects.ByName(objectRef.parent);

            UndertaleSprite sprite;
            sprite = data.Sprites.ByName(objectRef.spriteName);

            
            UndertaleGameObject ObjectInst = data.GameObjects.ByName(objectRef.name);
            bool exists = ObjectInst != null;
            ObjectInst ??= new();

            ObjectInst.Name = name;
            ObjectInst.ParentId = parentID;
            ObjectInst.Sprite = sprite;
            ObjectInst.Visible = objectRef.visible;

            ObjectInst.Persistent = objectRef.persistent;
            ObjectInst.Solid = objectRef.solid;

            AddEvents(data, ObjectInst, objectRef);

            if (!exists)
                data.GameObjects.Add(ObjectInst);

            return ObjectInst;
        }

        public static void AddEvents(UndertaleData data, UndertaleGameObject objectInst, Types.Object objectRef) {
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
