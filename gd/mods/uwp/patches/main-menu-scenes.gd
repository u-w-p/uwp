## New main menu scenes for the world view
extends Node

const DEBUG := false

const scenes := [
	## Bridge
	{
		"fov": 70,
		"near": 1.39,
		"far": 8192,
		"translation": {
			"x": 23.8,
			"y": 5.5,
			"z": -82.6
		},
		"rotation_degrees": {
			"x": 9.0,
			"y": 221.5
		}
	},

	## Aquarium
	{
			"fov": 106,
			"near": 1.39,
			"far": 8192,
			"translation": {
				"x": -132.9,
				"y": 5.848,
				"z": -379.368
			},
			"rotation_degrees": {
				"x": 2.84,
				"y": -1.76
			}
		},
]


func _debug(msg, data = null) -> void:
	if not DEBUG:
		return
	print("[UWP]: %s" % msg)
	if data != null:
		print(JSON.print(data, "\t"))


func _ready():
	get_tree().connect("node_added", self, "on_node_add")


func on_node_add(node):
	if node.name != "main_menu":
		return
	var world: ViewportContainer = node.get_node("world")
	if not world.backdrop:
		return

	var cam = get_node("/root/main_menu/world/Viewport/main/track_camera/Camera")

	## TODO: Allow selection or randomization
	var scene_settings: Dictionary = scenes[0]
	for prop in scene_settings.keys():
		var val = scene_settings[prop]
		if typeof(val) in [TYPE_INT, TYPE_REAL]:
			cam[prop] = scene_settings[prop]
		elif typeof(val) == TYPE_DICTIONARY:
			for key in val.keys():
				cam[prop][key] = val[key]
		else:
			# Invalid!!!
			print_stack()
			breakpoint
