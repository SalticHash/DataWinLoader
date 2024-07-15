# DataWinLoader
A project for loading a mod IN development to a data.win file (Game Maker, Game Data)
This is meant to make development of mods for Game Maker Games easier!
Normally you have to make your changes in the data.win and loose it all due to a corruption error.
Now you can have your files separated and just load them in with a JSON file.

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
