## New main menu scenes for the world view
extends Node

const DEBUG := false


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
	cam.fov = 70
	cam.near = 1.39
	cam.far = 8192
	cam.rotation_degrees.x = 9
	cam.rotation_degrees.y = 221.5

	# Spawn bridge
	cam.translation.x = 23.8
	cam.translation.y = 5.5
	cam.translation.z = -82.6

	# TODO: Add other camera scenes
