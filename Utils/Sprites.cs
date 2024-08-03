using UndertaleModLib;
using UndertaleModLib.Models;
using DataWinLoad;
using static DataWinLoad.Utils.Types;


namespace DataWinLoad.Utils {
    internal class Sprites {
        public static UndertaleSprite AddSprite(UndertaleData data, Types.Sprite sprite) {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Adding Sprite {sprite.spriteName}");

            UndertaleSprite spriteObject = data.Sprites.ByName(sprite.spriteName);
            bool exists = spriteObject != null;
            spriteObject ??= new();

            spriteObject.Name = data.Strings.MakeString(sprite.spriteName);

            spriteObject.Width = sprite.size[0];
            spriteObject.Height = sprite.size[1];

            spriteObject.MarginLeft = sprite.margin[0];
            spriteObject.MarginRight = sprite.margin[1];
            spriteObject.MarginBottom = sprite.margin[2];
            spriteObject.MarginTop = sprite.margin[3];


            int i = 0;
            spriteObject.Textures.Clear();
            foreach (var pageItemRep in sprite.pageItems) {
                var pageItem = AddPageItem(data, pageItemRep, $"{sprite.spriteName}_pgItem_{i}");
                if (pageItem == null) {
                    continue;
                }
                var textureEntry = new UndertaleSprite.TextureEntry() { Texture = pageItem };
                spriteObject.Textures.Add(textureEntry);
                i++;
            }

            if (!exists)
                data.Sprites.Add(spriteObject);

            return spriteObject;
        }

        public static UndertaleEmbeddedTexture? AddTexture(UndertaleData data, Types.Texture texture) {
            var name = Path.GetFileName(texture.path);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Adding Texture with sprite {name}");

            UndertaleEmbeddedTexture textureObject;
            DataWinLoad.textureIDs.TryGetValue(texture.textureID, out textureObject);
            bool exists = textureObject != null;
            textureObject ??= new();

            string? workingDir = DataWinLoad.workingDir;

            if (DataWinLoad.workingDir == null) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Working directory not set.");
                return null;
            };

            var path = Path.GetFullPath(texture.path, DataWinLoad.workingDir);
            if (!File.Exists(path)) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"File doesnt exist: {path}");
                return null;
            }
            textureObject.TextureData.TextureBlob = File.ReadAllBytes(path);


            if (!exists) {
                DataWinLoad.textureIDs.Add(texture.textureID, textureObject);
                data.EmbeddedTextures.Add(textureObject);
            }

            return textureObject;
        }

        public static UndertaleTexturePageItem? AddPageItem(UndertaleData data, Types.PageItem pageItem, string id) {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Adding PageItem that uses texture with id \"{pageItem.textureID}\"");

            UndertaleEmbeddedTexture? texture;
            DataWinLoad.textureIDs.TryGetValue(pageItem.textureID, out texture);

            if (texture == null) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed, texture not found.");
                return null;
            }

            UndertaleTexturePageItem pageItemObject = new();
            DataWinLoad.pageItemIDs.TryGetValue(id, out pageItemObject);
            bool exists = pageItemObject != null;
            pageItemObject ??= new();

            pageItemObject.SourceX = pageItem.sourcePosition[0];
            pageItemObject.SourceY = pageItem.sourcePosition[1];
            pageItemObject.SourceWidth = pageItem.sourceSize[0];
            pageItemObject.SourceHeight = pageItem.sourceSize[1];

            pageItemObject.TargetX = pageItem.targetPosition[0];
            pageItemObject.TargetY = pageItem.targetPosition[1];
            pageItemObject.TargetWidth = pageItem.targetSize[0];
            pageItemObject.TargetHeight = pageItem.targetSize[1];

            pageItemObject.BoundingWidth = pageItem.boundingSize[0];
            pageItemObject.BoundingHeight = pageItem.boundingSize[1];

            pageItemObject.TexturePage = DataWinLoad.textureIDs[pageItem.textureID];



            if (!exists) {
                DataWinLoad.pageItemIDs.Add(id, pageItemObject);
                data.TexturePageItems.Add(pageItemObject);
            }


            return pageItemObject;
        }
    }
}
