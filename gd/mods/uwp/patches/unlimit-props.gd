## Replaces built-in prop limits with reasonable amount
extends Node

var DEBUG := OS.has_feature("editor") and true


func _debug(msg, data = null):
	if not DEBUG:
		return
	print("[UWP]: %s" % msg)
	if data != null:
		print(JSON.print(data, "\t"))


func _on_child_entered(node):
	if node.name == "world":
		_update_prop_limits(node)


func _update_prop_limits(world):
	for key in world.ACTOR_BANK.keys():
		var entry = world.ACTOR_BANK[key]
		var admin_only = entry[1]
		var limit = entry[2]

		print(key, str(admin_only), str(limit))

		if admin_only == false and key != "_" and limit < 10:
			entry[2] = 12
		# Birdpocalypse
		# ambient_bird
	_debug("ACTOR_BANK Prop limits updated!")


func _ready():
	get_node("/root").connect("child_entered_tree", self, "_on_child_entered")
