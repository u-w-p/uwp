
## Remove invisible barrier(s) halting player (especially forest entry)
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
	if node.name == "invis_walls":
		node.queue_free()
