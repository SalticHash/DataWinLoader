# DataWinLoader
A project for loading a mod IN development to a data.win file (Game Maker, Game Data)
This is meant to make development of mods for Game Maker Games easier!
Normally you have to make your changes in the data.win and loose it all due to a corruption error.
Now you can have your files separated and just load them in with a JSON file.

With DataWinLoader you can just add the scripts, objects and sprites you need for the mod in a json, in it you configure the path of the original `data.win` that you are modding, the path of the output resulting `data.win` and the working directory (The folder where your project resides)

## JSON Example
```json
{
  "dataPath": "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Pizza Tower\\backup.win",
  "outputDataPath": "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Pizza Tower\\data.win",
  "workingDir": "C:\\Users\\Admin\\Documents\\GitHub\\bunch-of-complex-stuff",
  "objects": [
    {
      "name": "obj_delay",
      "parent": "obj_trigger",
      "spriteName": "delay_trigger",
      "visible": false,
      "persistent": false,
      "solid": false,
      "events": [
        {
          "eventType": "create",
          "scriptName": "gml_Object_obj_delay_Create_0"
        },
        {
          "eventType": "collision",
          "subtypeID": "obj_player",
          "scriptName": "gml_Object_obj_delay_Collision_obj_player"
        },
        {
          "eventType": "other",
          "subtypeID": "roomStart",
          "scriptName": "gml_Object_obj_delay_Other_4"
        }
      ]
    }
  ],
  "scripts": [
    {
      "name": "gml_GlobalScript_bocs_functions",
      "path": ".\\bocs_functions.gml",
      "global": true
    },
    {
      "name": "gml_Object_obj_delay_Create_0",
      "path": ".\\obj_trigger\\obj_delay\\create.gml"
    },
    {
      "name": "gml_Object_obj_delay_Collision_obj_player",
      "path": ".\\obj_trigger\\self\\obj_player_collision.gml"
    },
    {
      "name": "gml_Object_obj_delay_Other_4",
      "path": ".\\obj_trigger\\obj_delay\\room_start.gml"
    }
  ],
  "sprites": [
    {
      "spriteName": "delay_trigger",
      "size": [ 32, 32 ],
      "margin": [ 0, 32, 32, 0 ],
      "pageItems": [
        {
          "textureID": "0",
          "sourcePosition": [ 64, 0 ],
          "sourceSize": [ 32, 32 ],

          "targetPosition": [ 0, 0 ],
          "targetSize": [ 32, 32 ],
          "boundingSize": [ 32, 32 ]
        }
      ]
    }
  ],
  "textures": [
    {
      "textureID": "0",
      "path": ".\\assets\\bocs_textures.png"
    }
  ]
}
```

## Building
1. Build UndertaleModLib and get the .nupkg file
1. Open in Visual Studio
1. Add it to the dependencies
1. Build
